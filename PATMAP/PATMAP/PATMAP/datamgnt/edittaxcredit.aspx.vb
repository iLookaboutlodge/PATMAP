Public Partial Class edittaxcredit
    Inherits System.Web.UI.Page

    Private strFilter As String
    Private bckgColor As System.Drawing.Color

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                'fill the all dropdowns
                dllYear.DataSource = common.GetYears()
                dllYear.DataBind()

                If Session("taxCreditID") <> 0 Then
                    'setup database connection
                    Dim con As New SqlClient.SqlConnection
                    con.ConnectionString = PATMAP.Global_asax.connString
                    con.Open()

                    Dim query As New SqlClient.SqlCommand
                    Dim dr As SqlClient.SqlDataReader

                    query.Connection = con
                    query.CommandText = "select dataSetName, year from taxCreditDescription where taxCreditID=" & Session("taxCreditID")
                    dr = query.ExecuteReader
                    dr.Read()

                    If IsDBNull(dr.GetValue(0)) Then
                        txtNewDSN.Text = ""
                    Else
                        txtNewDSN.Text = dr.GetValue(0)
                    End If
                    txtNewDSN.Enabled = False

                    If Not IsDBNull(dr.GetValue(1)) Then
                        dllYear.SelectedValue = dr.GetValue(1)
                    End If
                    dllYear.Enabled = False

                End If

                'Sets default filter
                'Show only main classes
                strFilter = "parentTaxClassID = 'none'"
                Session("classFilter") = strFilter

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
        Dim taxCreditID As Integer = Session("taxCreditID")

        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim query As String = ""

        If taxCreditID <> 0 Then
            query = "select taxClass.*,  ROUND(taxCredit.taxCredit, 4) * 100 As BaseTaxCredit, ROUND(taxCredit.capped, 0) As BaseCapped" & vbCrLf & _
               "from" & vbCrLf & _
               "(" & vbCrLf & _
               "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, taxClassID As groupClasss, sort, 0 as subSort from taxClasses where [active] = 1 and parentTaxClassID = 'none' union" & vbCrLf & _
               "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, parentTaxClassID As groupClasss, (select sort from taxClasses class where taxClassID = t.parentTaxClassID) as sort, sort as subSort from taxClasses t where [active] = 1 and parentTaxClassID <> 'none'" & vbCrLf & _
               ") taxClass" & vbCrLf & _
               "inner join taxCredit on taxCredit.taxClassID = taxClass.taxClassID AND taxCredit.taxCreditID = " & taxCreditID & vbCrLf & _
               "order by taxClass.sort, taxClass.subSort"
        Else
            query = "select taxClass.*,  0 As BaseTaxCredit, 0 As BaseCapped" & vbCrLf & _
               "from" & vbCrLf & _
               "(" & vbCrLf & _
               "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, taxClassID As groupClasss, sort, 0 as subSort from taxClasses where [active] = 1 and parentTaxClassID = 'none' union" & vbCrLf & _
               "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, parentTaxClassID As groupClasss, (select sort from taxClasses class where taxClassID = t.parentTaxClassID) as sort, sort as subSort from taxClasses t where [active] = 1 and parentTaxClassID <> 'none'" & vbCrLf & _
               ") taxClass" & vbCrLf & _
                "order by taxClass.sort, taxClass.subSort"
        End If


        'fill in the tax class table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        con.Close()

        'sets grid's filter
        dv = dt.DefaultView
        dv.RowFilter = strFilter

        'Displays two decimal places for Base Year Tax Credit
        'and no decimal places for Capped values
        common.FormatDecimal(dt.Select(), "BaseTaxCredit,BaseCapped", "2,0")

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

    Private Sub grdTaxCredit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTaxCredit.RowDataBound
        Try
            'Displays expand/collapse button for main classes with subclasses
            bckgColor = common.DisplayExpand(e, grdTaxCredit.DataSource, bckgColor, strFilter)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        'Try
        '    'change page title and breadcrumb to 
        '    'if the mode is either edit or add
        '    If common.ChangeTitle(Session("taxCreditID"), lblTitle) Then
        '        txtNewDSN.Visible = True
        '        ddlDSN.Visible = False
        '    Else
        '        txtNewDSN.Visible = False
        '        ddlDSN.Visible = True
        '    End If
        'Catch
        '    'retrieves error message
        '    Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        'End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            common.UndoChange()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try

            'make sure required fields are filled out
            If String.IsNullOrEmpty(Trim(txtNewDSN.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP135")
                Exit Sub
            End If

            If Not common.ValidateNoSpecialChar(Trim(txtNewDSN.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP84")
                Exit Sub
            End If

            If dllYear.SelectedValue = "<Select>" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP86")
                Exit Sub
            End If

            txtNewDSN.Text = Server.HtmlEncode(Replace(Trim(txtNewDSN.Text), "'", "''"))

            Dim query As String
            Dim counter As Integer
            Dim taxCredit, capped As Decimal
            Dim taxClassID As String
            Dim dr As DataRow()
            Dim dt As DataTable
            Dim userID As Integer = Session("userID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            Dim com As New SqlClient.SqlCommand
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()
            com.Connection = con

            Dim trans As SqlClient.SqlTransaction
            trans = con.BeginTransaction()
            com.Transaction = trans

            query = ""
            dt = CType(Session("taxClasses"), DataTable)
            dr = dt.Select("", "", DataViewRowState.OriginalRows)

            'iterates through data rows
            For counter = 0 To dr.Length - 1

                taxCredit = dr(counter).Item("BaseTaxCredit")
                capped = dr(counter).Item("BaseCapped")
                taxClassID = dr(counter).Item("taxClassID")

                taxCredit /= 100

                'Formats values to be rounded into four decimal places
                taxCredit = Math.Truncate(Math.Round(taxCredit, 4) * 10000) / 10000

                'Formats values to be rounded to nearest zero
                capped = Math.Truncate(Math.Round(capped, 0))

                'builds the query that will update database
                If Session("taxCreditID") <> 0 Then
                    If dr(counter).RowState = DataRowState.Modified Then
                        query &= "update taxCredit set taxCredit=" & taxCredit & ", capped=" & capped & " where taxCreditID=" & Session("taxCreditID") & " and taxClassID='" & taxClassID & "'" & vbCrLf
                    End If
                Else
                    query &= "insert into taxCredit values(@taxCreditID,'" & taxClassID & "'," & taxCredit & "," & capped & ")" & vbCrLf
                End If
            Next

            If Session("taxCreditID") = 0 Then
                com.CommandText = "insert into taxCreditDescription values(" & dllYear.SelectedItem.Text & ",'','" & Trim(txtNewDSN.Text) & "',1)" & vbCrLf
                com.CommandText += "declare @taxCreditID as int" & vbCrLf
                com.CommandText += "select @@IDENTITY" & vbCrLf
                com.CommandText += "select @taxCreditID = @@IDENTITY" & vbCrLf
                com.CommandText += query
            Else
                com.CommandText = "update taxYearModelDescription set dataStale = 1 where taxYearStatusID in (1,3) and taxCreditID = " & Session("taxCreditID")
                com.ExecuteNonQuery()

                com.CommandText = "update assessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where taxCreditID = " & Session("taxCreditID") & ")"
                com.ExecuteNonQuery()
                com.CommandText = "update assessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where taxCreditID = " & Session("taxCreditID") & ")"
                com.ExecuteNonQuery()

                com.CommandText = "update liveAssessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where taxCreditID = " & Session("taxCreditID") & ")"
                com.ExecuteNonQuery()
                com.CommandText = "update liveAssessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where taxCreditID = " & Session("taxCreditID") & ")"
                com.ExecuteNonQuery()

                com.CommandText = query
            End If

            If query <> "" Then
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

            'clean up
            Session.Remove("classFilter")
            Session.Remove("taxClasses")
            Session.Remove("taxCreditID")
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewtaxcredit.aspx")
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
            formField = CType(grdTaxCredit.Rows(counter).Cells(2).FindControl("txtBaseCredit"), TextBox)

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

                formField = CType(grdTaxCredit.Rows(counter).Cells(3).FindControl("txtBaseCapped"), TextBox)

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
                    dr(0).Item("BaseTaxCredit") = taxCredit
                    dr(0).Item("BaseCapped") = capped

                    'selects the sub tax classes from the data set
                    dr = dt.Select("groupClasss = '" & grdTaxCredit.SelectedDataKey.Value.ToString() & "' and taxClassID <> '" & grdTaxCredit.SelectedDataKey.Value.ToString() & "'")

                    For i = 0 To dr.Length - 1
                        dr(i).Item("BaseTaxCredit") = taxCredit
                        dr(i).Item("BaseCapped") = capped
                    Next

                    'Displays two decimal places for Base and Subject Year Tax Credit
                    'and no decimal places for Capped values
                    common.FormatDecimal(dr, "BaseTaxCredit,BaseCapped", "2,0")

                End If

            End If

        End If
        grdTaxCredit.DataSource = dt
        grdTaxCredit.DataBind()
    End Sub

    Public Sub valueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            btnSave.Enabled = False
            grdTaxCredit.SelectedIndex = sender.parent.parent.dataitemindex

            updateDataSet()
            btnSave.Enabled = True
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("classFilter")
        Session.Remove("taxClasses")
        Session.Remove("taxCreditID")

        Response.Redirect("viewtaxcredit.aspx")
    End Sub
End Class