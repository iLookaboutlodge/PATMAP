Public Partial Class pov
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
                fillPOVGrid()

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

    Private Sub fillPOVGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection

        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim query As String
        Dim userID As Integer = Session("userID")

        query = "select taxClass.*, ROUND(POV.POV, 4) * 100 As BasePOV, isNull(ROUND(livePOV.POV, 4) * 100,0) As SubjectPOV " & vbCrLf & _
                "from" & vbCrLf & _
                "(" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, taxClassID As groupClasss, sort, 0 as subSort from taxClasses where [active] = 1 and parentTaxClassID = 'none' union" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, parentTaxClassID As groupClasss, (select sort from taxClasses class where taxClassID = t.parentTaxClassID) as sort, sort as subSort from taxClasses t where [active] = 1 and parentTaxClassID <> 'none'" & vbCrLf & _
                ") taxClass" & vbCrLf & _
                "inner join POV on POV.taxClassID = taxClass.taxClassID AND POV.povID = (select POVID from taxYearModelDescription where taxYearModelID = (select liveassessmenttaxmodel.baseTaxYearModelID from liveAssessmentTaxModel where userID = " & userID & "))" & vbCrLf & _
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
        common.FormatDecimal(dt.Select(), "BasePOV,SubjectPOV", "2")

        grdPOV.DataSource = dt
        grdPOV.DataBind()

        'stores result in the session
        Session("taxClasses") = dt
    End Sub

    Private Sub grdPOV_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdPOV.RowCommand
        Try
            Dim dv As New DataView
            Dim dt As New DataTable

            If e.CommandName = "expandClass" Then

                'if there's no tax class data set in session,
                'retrieves data from database
                If IsNothing(Session("taxClasses")) Then
                    fillPOVGrid()
                End If

                dt = CType(Session("taxClasses"), DataTable)

                'selects current row in the grid
                grdPOV.SelectedIndex = e.CommandArgument
                'expands selected row to display subclasses
                strFilter = common.ExpandCollapse(dt, strFilter, grdPOV.SelectedDataKey.Value.ToString())
                'stores current filter
                Session("classFilter") = strFilter

                dv = dt.DefaultView
                dv.RowFilter = strFilter

                grdPOV.DataSource = dt
                grdPOV.DataBind()

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    'updateDataSet()
    'Accepts no parameter
    'Updates data set before grid is expanded to show subclasses
    Private Sub updateDataSet()
        Dim counter, i As Integer
        Dim formField As System.Web.UI.WebControls.TextBox
        Dim dt As DataTable
        Dim dr() As DataRow
        Dim POV As Decimal

        dt = CType(Session("taxClasses"), DataTable)

        counter = grdPOV.SelectedIndex

        If (grdPOV.Rows(counter).RowType = DataControlRowType.DataRow) Then

            'gets values entered in the textbox
            formField = CType(grdPOV.Rows(counter).Cells(3).FindControl("txtSubjectPOV"), TextBox)

            If Not IsNothing(formField) Then

                formField.Text = Trim(formField.Text)

                If Not IsNumeric(Replace(formField.Text, "%", "")) Then
                    Master.errorMsg = common.GetErrorMessage("PATMAP72")
                    Exit Sub
                End If

                'If values have the percentage sign,
                'remove it
                If InStr(formField.Text, "%") > 0 Then
                    POV = Replace(formField.Text, "%", "")
                Else
                    POV = formField.Text

                    'If value is less than 1 converts it to percentage value
                    If POV < 1 Then
                        POV *= 100
                    End If
                End If

                If Not common.ValidateRange(POV, 0, 100) Then
                    Master.errorMsg = common.RangeErrorMsg(0, 100)
                    Exit Sub
                End If

                'selects the tax class from the data set
                dr = dt.Select("taxClassID = '" & grdPOV.SelectedDataKey.Value.ToString() & "'")

                'updates tax class's value
                If dr.Length > 0 Then
                    dr(0).Item("SubjectPOV") = POV

                    'selects the sub tax classes from the data set
                    dr = dt.Select("groupClasss = '" & grdPOV.SelectedDataKey.Value.ToString() & "' and taxClassID <> '" & grdPOV.SelectedDataKey.Value.ToString() & "'")

                    For i = 0 To dr.Length - 1
                        dr(i).Item("SubjectPOV") = POV
                    Next

                    'Displays two decimal places for Base and Subject Year POV
                    common.FormatDecimal(dr, "BasePOV,SubjectPOV", "2")
                End If

            End If

        End If

        grdPOV.DataSource = dt
        grdPOV.DataBind()

    End Sub

    Private Sub grdPOV_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPOV.RowDataBound
        Try
            'Displays expand/collapse button for main classes with subclasses
            bckgColor = common.DisplayExpand(e, grdPOV.DataSource, bckgColor, strFilter)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Try
            Dim query As String
            Dim counter As Integer
            Dim POV As Decimal
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

                POV = dr(counter).Item("SubjectPOV") / 100

                'Formats values to be rounded into four decimal places
                POV = Math.Truncate(Math.Round(POV, 4) * 10000) / 10000

                'builds the query that will update database
                query &= "If (select count(*) from livePOV where userID = " & userID & " and  taxClassID = '" & dr(counter).Item("taxClassID") & "') > 0" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	update livePOV set POV = " & POV & " where userID = " & userID & " and  taxClassID = '" & dr(counter).Item("taxClassID") & "'" & vbCrLf & _
                        "End" & vbCrLf & _
                        "Else" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	insert into livePOV (userID, taxClassID, POV) values (" & userID & ",'" & dr(counter).Item("taxClassID") & "'," & POV & ")" & vbCrLf & _
                        "End" & vbCrLf
            Next

            If query <> "" Then
                Dim trans As SqlClient.SqlTransaction
                trans = con.BeginTransaction()
                com.Transaction = trans
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

        'Response.Redirect("edpov.aspx")
        Response.Clear()

        common.gotoNextPage(3, 46, Session("levelID"))
    End Sub

    Public Sub valueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            btnSubmit.Enabled = False
            grdPOV.SelectedIndex = sender.parent.parent.dataitemindex

            updateDataSet()
            btnSubmit.Enabled = True
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
        Try
            Dim counter As Integer
            Dim dt As DataTable

            dt = CType(Session("taxClasses"), DataTable)

            For counter = 0 To dt.Rows.Count - 1
                dt.Rows(counter).Item("SubjectPOV") = 0
            Next

            grdPOV.DataSource = dt
            grdPOV.DataBind()

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

        Dim i As Integer = 0
        Dim sql As String = ""
        For i = 0 To dr.Length - 1 Step 1
            If dr(i).RowState = DataRowState.Modified Then
                sql += "Select + '[' + rtrim(convert(char,GETDATE(),101)) + ']POV changed for " + dr(i).Item(1) + " from ' + rtrim(convert(char,((select POV from livePOV where userID=" & userID.ToString & " AND taxClassID='" & dr(i).Item(0) & "')*100))) + '% to " + dr(i).Item(8).ToString + "%'" + vbCrLf
            End If
        Next

        Dim updateAuditTrailSQL As StringBuilder = Nothing
        If sql <> "" Then
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            Dim com As New SqlClient.SqlCommand
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()
            com.Connection = con

            com.CommandText = sql
            Dim sqlDr As SqlClient.SqlDataReader
            sqlDr = com.ExecuteReader()

            Dim auditTrail As String
            sqlDr.Read()
            auditTrail = sqlDr.GetValue(0).ToString + vbCrLf
            While sqlDr.NextResult() = True
                sqlDr.Read()
                auditTrail += sqlDr.GetValue(0).ToString + vbCrLf
            End While
            sqlDr.Close()

            sql = "select auditTrailText from liveAssessmentTaxModel where userID=" & userID.ToString
            com.CommandText = sql
            sqlDr = com.ExecuteReader
            sqlDr.Read()

            updateAuditTrailSQL = New StringBuilder("update liveAssessmentTaxModel set auditTrailText='" & auditTrail & sqlDr.GetValue(0).ToString.Replace("'", "''") & "' where userID=" & userID.ToString)
            con.Close()

        End If
        Return updateAuditTrailSQL
    End Function
End Class