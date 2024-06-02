Public Partial Class millrate
    Inherits System.Web.UI.Page
    Private strFilter As String
    Private bckgColor As System.Drawing.Color

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.assmnt)

                Dim levelID As Integer = Session("levelID")

                'Presentation users has to re-save current scenario in 
                'in a different scenario name
                If levelID = 3 Then
                    btnSave.ImageUrl = "~/images/btnSaveAs.gif"
                End If


                'Sets default filter
                'Show only main classes
                strFilter = "parentTaxClassID = 'none'"
                Session("classFilter") = strFilter

                'Gets Tax Year Model names used by the scenario 
                common.GetModelNames(txtBaseTaxYrModel.Text, txtSubjectTaxYrModel.Text, Session("userID"), txtScenarioName.Text)

                'Fill grid
                fillPMRGrid()

                Dim dt As DataTable
                dt = CType(Session("taxClasses"), DataTable)
                dt.AcceptChanges()
                Session("taxClasses") = dt

            Else                
                If Not IsNothing(Session("classFilter")) Then
                    strFilter = Session("classFilter")
                Else
                    strFilter = "parentTaxClassID = 'none'"
                End If
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub fillPMRGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection

        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim query As String
        Dim userID As Integer = Session("userID")

        query = "select taxClass.*, isNull(ROUND(PMR, 8) * 1000,0) As PMR, isNull(PMRReplacement,0) as PMRReplacement, isNull(assessmentInclude,1) as assessmentInclude" & vbCrLf & _
                "from" & vbCrLf & _
                "(" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, taxClassID As groupClasss, sort, 0 as subSort from taxClasses where [active] = 1 and parentTaxClassID = 'none' union" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, parentTaxClassID As groupClasss, (select sort from taxClasses class where taxClassID = t.parentTaxClassID) as sort, sort as subSort from taxClasses t where [active] = 1 and parentTaxClassID <> 'none'" & vbCrLf & _
                ") taxClass" & vbCrLf & _
                "left join livePMR on livePMR.taxClassID = taxClass.taxClassID AND livePMR.userid = " & userID & vbCrLf & _
                "inner join taxClassesPermission on taxClassesPermission.taxClassID = taxClass.taxClassID and taxClassesPermission.levelID = " & Session("levelID") & " and access = 1" & vbCrLf & _
                "order by taxClass.sort, taxClass.subSort"

        'fill in the tax class table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        con.Close()

        'sets grid's filter
        dv = dt.DefaultView
        dv.RowFilter = strFilter

        'Rounds PMR to 4 decimal places
        common.FormatDecimal(dt.Select(), "PMR", "4", True)

        grdMillRate.DataSource = dt
        grdMillRate.DataBind()

        Session("taxClasses") = dt
    End Sub

    Private Sub grdMillRate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdMillRate.RowCommand
        Try
            Dim dv As New DataView
            Dim dt As New DataTable

            If e.CommandName = "expandClass" Then


                'if there's no tax class data set in session,
                'retrieves data from database
                If IsNothing(Session("taxClasses")) Then
                    fillPMRGrid()
                End If

                dt = CType(Session("taxClasses"), DataTable)

                'selects current row in the grid
                grdMillRate.SelectedIndex = e.CommandArgument
                'expands selected row to display subclasses
                strFilter = common.ExpandCollapse(dt, strFilter, grdMillRate.SelectedDataKey.Value.ToString())
                'stores current filter
                Session("classFilter") = strFilter

                dv = dt.DefaultView
                dv.RowFilter = strFilter

                grdMillRate.DataSource = dt
                grdMillRate.DataBind()

            End If
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
        Dim radioFieldExcluded As System.Web.UI.WebControls.RadioButtonList
        Dim dt As DataTable
        Dim dr() As DataRow
        Dim PMR As Decimal
        Dim replaced As Integer
        Dim included As Integer

        dt = CType(Session("taxClasses"), DataTable)

        counter = grdMillRate.SelectedIndex

        If (grdMillRate.Rows(counter).RowType = DataControlRowType.DataRow) Then

            'gets values entered in the textbox
            formField = CType(grdMillRate.Rows(counter).Cells(2).FindControl("txtMillRate"), TextBox)

            If Not IsNothing(formField) Then

                formField.Text = Trim(formField.Text)

                If Not IsNumeric(formField.Text) Then
                    Master.errorMsg = common.GetErrorMessage("PATMAP72")
                    Exit Sub
                End If

                PMR = formField.Text

                'If value is less than 1 converts it to mill value
                If PMR < 1 Then
                    PMR *= 1000
                End If

                If Not common.ValidateRange(PMR, 0) Then
                    Master.errorMsg = common.RangeErrorMsg(0)
                    Exit Sub
                End If

                radioField = CType(grdMillRate.Rows(counter).Cells(3).FindControl("rdlAction"), RadioButtonList)
                If radioField.SelectedIndex = 0 Then
                    replaced = 1
                Else
                    replaced = 0
                End If

                radioFieldExcluded = CType(grdMillRate.Rows(counter).Cells(4).FindControl("rdlLocalLevy"), RadioButtonList)
                If radioFieldExcluded.SelectedIndex = 0 Then
                    included = 1
                Else
                    included = 0
                End If

                'selects the tax class from the data set
                dr = dt.Select("taxClassID = '" & grdMillRate.SelectedDataKey.Value.ToString() & "'")

                'updates tax class's value
                If dr.Length > 0 Then
                    dr(0).Item("PMR") = PMR
                    dr(0).Item("PMRReplacement") = replaced
                    dr(0).Item("assessmentInclude") = included

                    If replaced Then
                        radioFieldExcluded.Items(0).Enabled = True
                        radioFieldExcluded.Items(1).Enabled = True
                    Else
                        radioFieldExcluded.Items(0).Enabled = False
                        radioFieldExcluded.Items(1).Enabled = False
                    End If

                    'selects the sub tax classes from the data set
                    dr = dt.Select("groupClasss = '" & grdMillRate.SelectedDataKey.Value.ToString() & "' and taxClassID <> '" & grdMillRate.SelectedDataKey.Value.ToString() & "'")

                    For i = 0 To dr.Length - 1
                        dr(i).Item("PMR") = PMR
                        dr(i).Item("PMRReplacement") = replaced
                        dr(i).Item("assessmentInclude") = included
                    Next

                    'Rounds PMR to 4 decimal places
                    common.FormatDecimal(dr, "PMR", "4", True)
                End If

            End If

        End If

        grdMillRate.DataSource = dt
        grdMillRate.DataBind()

    End Sub

    Private Sub grdMillRate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMillRate.RowDataBound
        Try
            Dim rdlAction As RadioButtonList
            Dim rdlLocalLevy As RadioButtonList


            'if current row is a datarow type
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                rdlAction = CType(e.Row.Cells.Item(3).FindControl("rdlAction"), RadioButtonList)
                rdlLocalLevy = CType(e.Row.Cells.Item(4).FindControl("rdlLocalLevy"), RadioButtonList)

                If DataBinder.Eval(e.Row.DataItem, "PMRReplacement") Then
                    rdlAction.SelectedIndex = 0
                Else
                    rdlAction.SelectedIndex = 1
                    rdlLocalLevy.Items(0).Enabled = False
                    rdlLocalLevy.Items(1).Enabled = False
                End If

                If DataBinder.Eval(e.Row.DataItem, "assessmentInclude") Then
                    rdlLocalLevy.SelectedIndex = 0
                Else
                    rdlLocalLevy.SelectedIndex = 1
                End If

            End If

            'Displays expand/collapse button for main classes with subclasses
            bckgColor = common.DisplayExpand(e, grdMillRate.DataSource, bckgColor, strFilter)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Try
            Dim query As String
            Dim counter As Integer
            Dim PMR As Decimal
            Dim replaced As Integer
            Dim included As Integer
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

                PMR = dr(counter).Item("PMR")
                replaced = dr(counter).Item("PMRReplacement")
                included = dr(counter).Item("assessmentInclude")

                PMR /= 1000

                'Formats values to be rounded into 6 decimal places
                PMR = Math.Round(PMR, 6)
                PMR = Decimal.Parse(String.Format(CStr(PMR), "{0:#.######}"))

                'builds the query that will update database
                query &= "If (select count(*) from livePMR where userid = " & userID & " and  taxClassID = '" & dr(counter).Item("taxClassID") & "') > 0" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	update livePMR set PMR = " & PMR & ", assessmentInclude = " & included & ", PMRReplacement = " & replaced & " where userid = " & userID & " and  taxClassID = '" & dr(counter).Item("taxClassID") & "'" & vbCrLf & _
                        "End" & vbCrLf & _
                        "Else" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	insert into livePMR (userID, taxClassID, PMR, PMRReplacement, assessmentInclude) values (" & userID & ",'" & dr(counter).Item("taxClassID") & "'," & PMR & "," & replaced & "," & included & ")" & vbCrLf & _
                        "End" & vbCrLf
            Next


            If query <> "" Then
                Dim trans As SqlClient.SqlTransaction
                trans = con.BeginTransaction()
                com.Transaction = trans
                If IsNothing(getUpdateAuditTrailSQL(userID)) Then
                    com.CommandText = query
                Else
                    com.CommandText = query & getUpdateAuditTrailSQL(userID).ToString
                End If

                Try
                    com.ExecuteNonQuery()
                    trans.Commit()
                Catch
                    trans.Rollback()
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
                    con.Close()
                    Exit Sub
                End Try
            Else

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

        'Response.Redirect("audittrail.aspx")
        common.gotoNextPage(3, 48, Session("levelID"))
    End Sub

    Private Sub millrate_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If IsNothing(Session("assessmentTaxModelID")) Then
            Response.Redirect("start.aspx")
        End If
    End Sub

    Public Sub valueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            btnSubmit.Enabled = False
            grdMillRate.SelectedIndex = sender.parent.parent.dataitemindex

            updateDataSet()
            btnSubmit.Enabled = True
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
        Try
            Dim counter As Integer
            Dim dt As DataTable

            dt = CType(Session("taxClasses"), DataTable)

            For counter = 0 To dt.Rows.Count - 1
                dt.Rows(counter).Item("PMR") = 0
                dt.Rows(counter).Item("PMRReplacement") = False
                dt.Rows(counter).Item("assessmentInclude") = False
            Next

            grdMillRate.DataSource = dt
            grdMillRate.DataBind()

            Session("taxClasses") = dt
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
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
            com.CommandText = "select PMR, PMRReplacement, assessmentInclude from livePMR where userID=" & userID.ToString & " AND taxClassID='" & dr(i).Item(0) & "'"
            sqlDr = com.ExecuteReader()
            sqlDr.Read()
            If sqlDr.GetValue(0) <> (dr(i).Item(7) / 1000) Then
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]PMR changed for " + dr(i).Item(1) + " from " & (sqlDr.GetValue(0) * 1000) & " to " & dr(i).Item(7) & vbCrLf
            End If
            If sqlDr.GetValue(1) <> dr(i).Item(8) Then
                Dim oldAction As String
                Dim newAction As String
                If sqlDr.GetValue(1) = "False" Then
                    oldAction = "In Addition"
                Else
                    oldAction = "Replacement"
                End If
                If dr(i).Item(8) = "False" Then
                    newAction = "In Addition"
                Else
                    newAction = "Replacement"
                End If
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]PMR action changed for " + dr(i).Item(1) + " from " & oldAction & " to " & newAction & vbCrLf
            End If
            If sqlDr.GetValue(2) <> dr(i).Item("assessmentInclude") Then
                Dim oldAssessment As String
                Dim newAssessment As String
                If sqlDr.GetValue(2) = "False" Then
                    oldAssessment = "Exclude Assessment"
                Else
                    oldAssessment = "Include Assessment"
                End If
                If dr(i).Item("assessmentInclude") = "False" Then
                    newAssessment = "Exclude Assessment"
                Else
                    newAssessment = "Include Assessment"
                End If
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]PMR Assessment for Subject Year Local Levy changed for " + dr(i).Item(1) + " from " & oldAssessment & " to " & newAssessment & vbCrLf
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
End Class