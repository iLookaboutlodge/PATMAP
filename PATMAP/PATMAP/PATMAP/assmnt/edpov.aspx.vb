Public Partial Class edpov
    Inherits System.Web.UI.Page
    Private strFilter As String
    Private bckgColor As System.Drawing.Color

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Sets submenu to be displayed
            subMenu.setStartNode(menu.assmnt)

            Dim levelID As Integer = Session("levelID")

            If levelID = 3 Then
                'Presentation users has to re-save current scenario in 
                'in a different scenario name
                btnSave.ImageUrl = "~/images/btnSaveAs.gif"
            End If

            'Sets default filter
            'Show only main classes
            strFilter = "parentTaxClassID = 'none'"
            Session("classFilter") = strFilter

            'Gets Tax Year Model names used by the scenario 
            common.GetModelNames(txtBaseTaxYrModel.Text, txtSubjectTaxYrModel.Text, Session("userID"), txtScenarioName.Text)

            'Fill grid
            fillEDPOVGrid()

        Else
            If Not IsNothing(Session("classFilter")) Then
                strFilter = Session("classFilter")
            Else
                strFilter = "parentTaxClassID = 'none'"
            End If
        End If
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Try
            Dim query As String
            Dim counter As Integer
            Dim EDPOV As Decimal
            Dim Factor As Boolean
            Dim dr As DataRow()
            Dim dt As DataTable
            Dim userID As Integer = Session("userID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            Dim com As New SqlClient.SqlCommand
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()
            com.Connection = con

            query = ""
            dt = CType(Session("taxClasses"), DataTable)
            dr = dt.Select("", "", DataViewRowState.ModifiedCurrent)

            If dr.Length > 0 Then
                query &= "update liveAssessmentTaxModel set dataStale=1 where userID=" & userID & vbCrLf
            End If

            'iterates through data rows
            For counter = 0 To dr.Length - 1

                EDPOV = dr(counter).Item("EDPOV") / 100
                Factor = dr(counter).Item("EDPOVFactor")

                'builds the query that will update database
                query &= "If (select count(*) from liveEDPOV where userID = " & userID & " and  taxClassID = '" & dr(counter).Item("taxClassID") & "') > 0" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	update liveEDPOV set EDPOV = " & EDPOV & ", EDPOVFactor = '" & Factor & "' where userID = " & userID & " and  taxClassID = '" & dr(counter).Item("taxClassID") & "'" & vbCrLf & _
                        "End" & vbCrLf & _
                        "Else" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	insert into liveEDPOV (userID, taxClassID, EDPOV, EDPOVFactor) values (" & userID & ",'" & dr(counter).Item("taxClassID") & "'," & EDPOV & ",'" & Factor & "')" & vbCrLf & _
                        "End" & vbCrLf
            Next

            getUpdateAuditTrailSQL(userID)

            If query <> "" Then
                Dim trans As SqlClient.SqlTransaction
                trans = con.BeginTransaction()
                com.Transaction = trans

                Dim updateAuditTrailSQL As String = ""
                updateAuditTrailSQL = getUpdateAuditTrailSQL(userID).ToString

                If updateAuditTrailSQL = "" Then
                    com.CommandText = query
                Else
                    com.CommandText = query & getUpdateAuditTrailSQL(userID).ToString
                End If

                com.CommandText = query & getUpdateAuditTrailSQL(userID).ToString
                Try
                    com.ExecuteNonQuery()
                    trans.Commit()
                Catch
                    trans.Rollback()
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
                    con.Close()
                    Exit Sub
                End Try
            End If

            con.Close()

            'Removes any filter or tax class data set in session
            Session.Remove("classFilter")
            Session.Remove("taxClasses")
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        'Response.Redirect("taxcredits.aspx")
        common.gotoNextPage(3, 99, Session("levelID"))
    End Sub

    Public Sub changeName(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim errCode As String

            errCode = common.UpdateScenarioName(Session("userID"), Session("assessmentTaxModelID"), txtScenarioName.Text)

            If errCode <> "" Then
                Master.errorMsg = common.GetErrorMessage(errCode)
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub fillEDPOVGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection

        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim query As String
        Dim userID As Integer = Session("userID")

        query = "select taxClass.*, isNull((EDPOV * 100),100) As EDPOV, isNull(EDPOVFactor,0) as EDPOVFactor, livePOV.POV as BasePOV" & vbCrLf & _
                "from" & vbCrLf & _
                "(" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, taxClassID As groupClasss, sort, 0 as subSort from taxClasses where [active] = 1 and parentTaxClassID = 'none' union" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, parentTaxClassID As groupClasss, (select sort from taxClasses class where taxClassID = t.parentTaxClassID) as sort, sort as subSort from taxClasses t where [active] = 1 and parentTaxClassID <> 'none'" & vbCrLf & _
                ") taxClass" & vbCrLf & _
                "left join liveEDPOV on liveEDPOV.taxClassID = taxClass.taxClassID AND liveEDPOV.userid = " & userID & vbCrLf & _
                "left join livePOV on livePOV.taxClassID = taxClass.taxClassID AND livePOV.userid = " & userID & vbCrLf & _
                "inner join taxClassesPermission on taxClassesPermission.taxClassID = taxClass.taxClassID and taxClassesPermission.levelID = " & Session("levelID") & " and access = 1" & vbCrLf & _
                "order by taxClass.sort, taxClass.subSort"

        'fill in the tax class table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        con.Close()

        'sets grid's filter
        dv = dt.DefaultView
        dv.RowFilter = strFilter

        'Displays two decimal places for Base and Subject Year POV
        'common.FormatDecimal(dt.Select(), "BasePOV,SubjectPOV", "2")

        grdEDPOV.DataSource = dt
        grdEDPOV.DataBind()

        'stores result in the session
        Session("taxClasses") = dt
    End Sub

    Private Sub grdEDPOV_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdEDPOV.RowCommand
        Try
            Dim dv As New DataView
            Dim dt As New DataTable

            If e.CommandName = "expandClass" Then

                'if there's no tax class data set in session,
                'retrieves data from database
                If IsNothing(Session("taxClasses")) Then
                    fillEDPOVGrid()
                End If

                dt = CType(Session("taxClasses"), DataTable)

                'selects current row in the grid
                grdEDPOV.SelectedIndex = e.CommandArgument
                'expands selected row to display subclasses
                strFilter = common.ExpandCollapse(dt, strFilter, grdEDPOV.SelectedDataKey.Value.ToString())
                'stores current filter
                Session("classFilter") = strFilter

                dv = dt.DefaultView
                dv.RowFilter = strFilter

                grdEDPOV.DataSource = dt
                grdEDPOV.DataBind()

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdEDPOV_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdEDPOV.RowDataBound
        Try
            Dim rdlFactorORPOV As RadioButtonList
            Dim txtEdPOVApplied As TextBox
            Dim txtSchoolPOV As TextBox
            Dim basePOV As String

            'if current row is a datarow type
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                basePOV = grdEDPOV.DataKeys(e.Row.RowIndex).Values("BasePOV").ToString()
                txtSchoolPOV = CType(e.Row.Cells.Item(2).FindControl("txtSchoolPOV"), TextBox)
                rdlFactorORPOV = CType(e.Row.Cells.Item(3).FindControl("rdlFactorORPOV"), RadioButtonList)
                txtEdPOVApplied = CType(e.Row.Cells.Item(4).FindControl("txtEdPOVApplied"), TextBox)

                If DataBinder.Eval(e.Row.DataItem, "EDPOVFactor") Then
                    rdlFactorORPOV.SelectedIndex = 1
                    txtEdPOVApplied.Text = txtSchoolPOV.Text
                Else
                    'default - 0 - Factor
                    rdlFactorORPOV.SelectedIndex = 0
                    'txtEdPOVApplied.Text = (basePOV * (txtSchoolPOV.Text.Substring(0, txtSchoolPOV.Text.Length - 1) / 100)) * 100
                    txtEdPOVApplied.Text = CType(basePOV, Double) * CType(txtSchoolPOV.Text.Substring(0, txtSchoolPOV.Text.Length - 1), Double) & "%"
                End If

            End If

            'Displays expand/collapse button for main classes with subclasses
            bckgColor = common.DisplayExpand(e, grdEDPOV.DataSource, bckgColor, strFilter)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Public Sub valueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            btnSubmit.Enabled = False
            grdEDPOV.SelectedIndex = sender.parent.parent.dataitemindex

            updateDataSet()
            btnSubmit.Enabled = True
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    'updateDataSet()
    'Accepts no parameter
    'Updates data set before grid is expanded to 
    'show subclasses
    Private Sub updateDataSet()
        Dim counter, i As Integer
        Dim formField As System.Web.UI.WebControls.TextBox
        Dim radioField As System.Web.UI.WebControls.RadioButtonList
        Dim schoolPOV As String
        Dim factor As Integer
        Dim dt As DataTable
        Dim dr() As DataRow

        dt = CType(Session("taxClasses"), DataTable)

        counter = grdEDPOV.SelectedIndex

        If (grdEDPOV.Rows(counter).RowType = DataControlRowType.DataRow) Then

            'gets values entered in the textbox
            formField = CType(grdEDPOV.Rows(counter).Cells(2).FindControl("txtSchoolPOV"), TextBox)

            If Not IsNothing(formField) Then

                formField.Text = Trim(formField.Text)
                'remove the % from school POV
                schoolPOV = formField.Text.Substring(0, formField.Text.Length - 1)

                'check to see if school POV is a numeric value
                If Not IsNumeric(schoolPOV) Then
                    Master.errorMsg = common.GetErrorMessage("PATMAP72")
                    Exit Sub
                End If

                'check to see school POV is between 0 and 100 (inclusive)
                If Not common.ValidateRange(schoolPOV, 0, 100) Then
                    Master.errorMsg = common.RangeErrorMsg(0)
                    Exit Sub
                End If

                radioField = CType(grdEDPOV.Rows(counter).Cells(3).FindControl("rdlFactorORPOV"), RadioButtonList)
                'if it is a factor (default)
                If radioField.SelectedIndex = 0 Then
                    factor = 0
                Else
                    factor = 1
                End If

                'selects the tax class from the data set
                dr = dt.Select("taxClassID = '" & grdEDPOV.SelectedDataKey.Values("taxClassID").ToString() & "'")

                'updates tax class's value
                If dr.Length > 0 Then
                    dr(0).Item("EDPOV") = schoolPOV
                    dr(0).Item("EDPOVFactor") = factor

                    'selects the sub tax classes from the data set
                    dr = dt.Select("groupClasss = '" & grdEDPOV.SelectedDataKey.Values("taxClassID").ToString() & "' and taxClassID <> '" & grdEDPOV.SelectedDataKey.Values("taxClassID").ToString() & "'")

                    For i = 0 To dr.Length - 1
                        dr(i).Item("EDPOV") = schoolPOV
                        'dr(i).Item("EDPOVFactor") = factor
                    Next

                    'Rounds PMR to 4 decimal places
                    'common.FormatDecimal(dr, "PMR", "4", True)
                End If

            End If

        End If

        grdEDPOV.DataSource = dt
        grdEDPOV.DataBind()

    End Sub

    Private Function getUpdateAuditTrailSQL(ByVal userID As Integer) As StringBuilder

        Dim dr As DataRow()
        Dim dt As DataTable
        dt = CType(Session("taxClasses"), DataTable)
        dr = dt.Select("", "", DataViewRowState.ModifiedCurrent)

        'setup database connection            
        Dim con As New SqlClient.SqlConnection
        Dim com As New SqlClient.SqlCommand
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()
        com.Connection = con
        Dim sqlDr As SqlClient.SqlDataReader

        Dim i As Integer = 0
        Dim auditTrail As String = ""
        For i = 0 To dr.Length - 1 Step 1
            com.CommandText = "select EDPOV, EDPOVFactor from liveEDPOV where userID=" & userID.ToString & " AND taxClassID='" & dr(i).Item(0) & "'"
            sqlDr = com.ExecuteReader()
            sqlDr.Read()
            If sqlDr.GetValue(0) <> (dr(i).Item(7) / 100) Then
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]EDPOV changed for " + dr(i).Item(1) + " from " & (sqlDr.GetValue(0) * 100) & " to " & dr(i).Item(7) & vbCrLf
            End If
            If sqlDr.GetValue(1) <> dr(i).Item(8) Then
                Dim oldAction As String
                Dim newAction As String
                If sqlDr.GetValue(1) = "False" Then
                    oldAction = "Factor"
                Else
                    oldAction = "POV"
                End If
                If dr(i).Item(8) = "False" Then
                    newAction = "Factor"
                Else
                    newAction = "POV"
                End If
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]EDPOV changed for " + dr(i).Item(1) + " from " & oldAction & " to " & newAction & vbCrLf
            End If
            sqlDr.Close()
        Next

        Dim sql As String = ""
        Dim updateAuditTrailSQL As StringBuilder = Nothing
        If auditTrail <> "" Then
            sql = "select auditTrailText from liveAssessmentTaxModel where userID=" & userID.ToString
            com.CommandText = sql
            sqlDr = com.ExecuteReader
            sqlDr.Read()
            updateAuditTrailSQL = New StringBuilder("update liveAssessmentTaxModel set auditTrailText='" & auditTrail & sqlDr.GetValue(0).ToString.Replace("'", "''") & "' where userID=" & userID.ToString)
            sqlDr.Close()
        End If
        con.Close()

        Return updateAuditTrailSQL
    End Function

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
        Try
            Dim counter As Integer
            Dim dt As DataTable

            dt = CType(Session("taxClasses"), DataTable)

            For counter = 0 To dt.Rows.Count - 1
                dt.Rows(counter).Item("EdPOV") = 0 'INKY'S CODE previous code: dt.Rows(counter).Item("SubjectPOV") = 0 --> caused error: "Column 'SubjectPOV' does not belong to table"
            Next

            grdEDPOV.DataSource = dt
            grdEDPOV.DataBind()

            Session("taxClasses") = dt
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            Dim errCode As String

            If Trim(txtScenarioName.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP99")
                Exit Sub
            End If

            errCode = common.UpdateScenarioName(Session("userID"), Session("assessmentTaxModelID"), txtScenarioName.Text)

            If errCode <> "" Then
                Master.errorMsg = common.GetErrorMessage(errCode)
                Exit Sub
            End If

            common.saveLiveModelTables(Session("userID"))

        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("start.aspx")
    End Sub
End Class