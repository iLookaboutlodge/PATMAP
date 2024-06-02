Public Partial Class taxcredits
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
                fillTaxCreditGrid()

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

    Private Sub fillTaxCreditGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        Dim userID As Integer = Session("userID")

        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim query As String

        query = "select taxClass.*,  ROUND(taxCredit.taxCredit, 4) * 100 As BaseTaxCredit, ROUND(taxCredit.capped, 0) As BaseCapped,  isNull(ROUND(liveTaxCredit.taxCredit, 4) * 100,0) As SubjectTaxCredit, ROUND(liveTaxCredit.capped, 0) As SubjectCapped" & vbCrLf & _
                "from" & vbCrLf & _
                "(" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, taxClassID As groupClasss, sort, 0 as subSort from taxClasses where [active] = 1 and parentTaxClassID = 'none' union" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, parentTaxClassID As groupClasss, (select sort from taxClasses class where taxClassID = t.parentTaxClassID) as sort, sort as subSort from taxClasses t where [active] = 1 and parentTaxClassID <> 'none'" & vbCrLf & _
                ") taxClass" & vbCrLf & _
                "inner join taxCredit on taxCredit.taxClassID = taxClass.taxClassID AND taxCredit.taxCreditID = (select taxCreditID from taxYearModelDescription where taxYearModelID = (select liveassessmenttaxmodel.baseTaxYearModelID from liveAssessmentTaxModel where userID = " & userID & "))" & vbCrLf & _
                "left join liveTaxCredit on liveTaxCredit.taxClassID = taxClass.taxClassID AND liveTaxCredit.userid =  " & userID & vbCrLf & _
                "inner join taxClassesPermission on taxClassesPermission.taxClassID = taxClass.taxClassID and taxClassesPermission.levelID = " & Session("levelID") & " and access = 1" & vbCrLf & _
                "order by taxClass.sort, taxClass.subSort"

        'fill in the tax class table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        con.Close()

        'sets grid's filter
        dv = dt.DefaultView
        dv.RowFilter = strFilter

        'Displays two decimal places for Base and Subject Year Tax Credit
        'and no decimal places for Capped values
        common.FormatDecimal(dt.Select(), "BaseTaxCredit,SubjectTaxCredit,BaseCapped,SubjectCapped", "2,2,0,0")

        grdTaxCredit.DataSource = dt
        grdTaxCredit.DataBind()

        Session("taxClasses") = dt
    End Sub

    Private Sub grdTaxCredit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdTaxCredit.RowCommand
        Try
            Dim dv As New DataView
            Dim dt As New DataTable

            If e.CommandName = "expandClass" Then

                'if there's no tax class data set in session,
                'retrieves data from database
                If IsNothing(Session("taxClasses")) Then
                    fillTaxCreditGrid()
                End If

                dt = CType(Session("taxClasses"), DataTable)

                'selects current row in the grid
                grdTaxCredit.SelectedIndex = e.CommandArgument
                'expands selected row to display subclasses
                strFilter = common.ExpandCollapse(dt, strFilter, grdTaxCredit.SelectedDataKey.Value.ToString())
                'stores current filter
                Session("classFilter") = strFilter

                dv = dt.DefaultView
                dv.RowFilter = strFilter

                grdTaxCredit.DataSource = dt
                grdTaxCredit.DataBind()

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
        Dim dt As DataTable
        Dim dr() As DataRow
        Dim taxCredit, capped As Decimal

        dt = CType(Session("taxClasses"), DataTable)

        counter = grdTaxCredit.SelectedIndex

        If (grdTaxCredit.Rows(counter).RowType = DataControlRowType.DataRow) Then

            'gets values entered in the textboxes
            formField = CType(grdTaxCredit.Rows(counter).Cells(4).FindControl("txtSubjectCredit"), TextBox)

            If Not IsNothing(formField) Then

                formField.Text = Trim(formField.Text)

                If Not IsNumeric(Replace(formField.Text, "%", "")) Then
                    Master.errorMsg = common.GetErrorMessage("PATMAP72")
                    Exit Sub
                End If

                'If values have the percentage sign,
                'remove it
                If InStr(formField.Text, "%") > 0 Then
                    taxCredit = Replace(formField.Text, "%", "")
                Else
                    taxCredit = formField.Text

                    'If value is less than 1 converts it to percentage value
                    If taxCredit < 1 Then
                        taxCredit *= 100
                    End If

                End If

                If Not common.ValidateRange(taxCredit, 0, 100) Then
                    Master.errorMsg = common.RangeErrorMsg(0, 100)
                    Exit Sub
                End If

                formField = CType(grdTaxCredit.Rows(counter).Cells(5).FindControl("txtSubjectCapped"), TextBox)

                If Not IsNothing(formField) Then

                    formField.Text = Trim(formField.Text)

                    If Not IsNumeric(formField.Text) Then
                        Master.errorMsg = common.GetErrorMessage("PATMAP72")
                        Exit Sub
                    End If

                    capped = formField.Text

                    If Not common.ValidateRange(capped, 0) Then
                        Master.errorMsg = common.RangeErrorMsg(0)
                        Exit Sub
                    End If

                End If


                'selects the tax class from the data set
                dr = dt.Select("taxClassID = '" & grdTaxCredit.SelectedDataKey.Value.ToString() & "'")

                'updates tax class's value
                If dr.Length > 0 Then
                    dr(0).Item("SubjectTaxCredit") = taxCredit
                    dr(0).Item("SubjectCapped") = capped

                    'selects the sub tax classes from the data set
                    dr = dt.Select("groupClasss = '" & grdTaxCredit.SelectedDataKey.Value.ToString() & "' and taxClassID <> '" & grdTaxCredit.SelectedDataKey.Value.ToString() & "'")

                    For i = 0 To dr.Length - 1
                        dr(i).Item("SubjectTaxCredit") = taxCredit
                        dr(i).Item("SubjectCapped") = capped
                    Next

                    'Displays two decimal places for Base and Subject Year Tax Credit
                    'and no decimal places for Capped values
                    common.FormatDecimal(dr, "BaseTaxCredit,SubjectTaxCredit,BaseCapped,SubjectCapped", "2,2,0,0")

                End If

            End If

        End If

        grdTaxCredit.DataSource = dt
        grdTaxCredit.DataBind()


    End Sub

    Private Sub grdTaxCredit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTaxCredit.RowDataBound
        Try
            'Displays expand/collapse button for main classes with subclasses
            bckgColor = common.DisplayExpand(e, grdTaxCredit.DataSource, bckgColor, strFilter)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Try
            Dim query As String
            Dim counter As Integer
            Dim taxCredit, capped As Decimal
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

                taxCredit = dr(counter).Item("SubjectTaxCredit")
                capped = dr(counter).Item("SubjectCapped")

                taxCredit /= 100

                'Formats values to be rounded into four decimal places
                taxCredit = Math.Truncate(Math.Round(taxCredit, 4) * 10000) / 10000

                'Formats values to be rounded to nearest zero
                capped = Math.Truncate(Math.Round(capped, 0))

                'builds the query that will update database
                query &= "If (select count(*) from liveTaxCredit where userid = " & userID & " and  taxClassID = '" & dr(counter).Item("taxClassID") & "') > 0" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	update liveTaxCredit set taxCredit = " & taxCredit & ", capped = " & capped & " where userid = " & userID & " and  taxClassID = '" & dr(counter).Item("taxClassID") & "'" & vbCrLf & _
                        "End" & vbCrLf & _
                        "Else" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	insert into liveTaxCredit (userID, taxClassID, taxCredit, capped) values (" & userID & ",'" & dr(counter).Item("taxClassID") & "'," & taxCredit & "," & capped & ")" & vbCrLf & _
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

        'Response.Redirect("millrate.aspx")
        common.gotoNextPage(3, 47, Session("levelID"))

    End Sub

    Private Sub taxcredits_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If IsNothing(Session("assessmentTaxModelID")) Then
            Response.Redirect("start.aspx")
        End If
    End Sub

    Public Sub valueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            btnSubmit.Enabled = False
            grdTaxCredit.SelectedIndex = sender.parent.parent.dataitemindex

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
                dt.Rows(counter).Item("SubjectTaxCredit") = 0
                dt.Rows(counter).Item("SubjectCapped") = 0
            Next

            grdTaxCredit.DataSource = dt
            grdTaxCredit.DataBind()

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
            com.CommandText = "select taxCredit, capped from liveTaxCredit where userID=" & userID.ToString & " AND taxClassID='" & dr(i).Item(0) & "'"
            sqlDr = com.ExecuteReader()
            sqlDr.Read()
            If sqlDr.GetValue(0) <> (dr(i).Item(9) / 100) Then
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]Tax Credit changed for " + dr(i).Item(1) + " from " & (sqlDr.GetValue(0) * 100) & "% to " & dr(i).Item(9) & "%" & vbCrLf
            End If
            If sqlDr.GetValue(1) <> dr(i).Item(10) Then
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]Tax Capped changed for " + dr(i).Item(1) + " from " & sqlDr.GetValue(1) & " to " & dr(i).Item(10) & vbCrLf
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