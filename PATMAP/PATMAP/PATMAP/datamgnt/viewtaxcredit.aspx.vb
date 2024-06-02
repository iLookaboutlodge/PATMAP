Public Partial Class viewtaxcredit
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                con.Open()

                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable

                'populate the 'subject yr k12 data set' drop down list                
                da.SelectCommand = New SqlClient.SqlCommand()
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select 0 as taxCreditID, 'Select Data' as dataSetName, 'Select Data' as year union all select taxCreditID, dataSetName, CONVERT(CHAR,year) from taxCreditDescription where statusID=1"

                da.Fill(dt)
                ddlDSN.DataSource = dt
                ddlDSN.DataValueField = "taxCreditID"
                ddlDSN.DataTextField = "dataSetName"
                ddlDSN.DataBind()

                ddlYear.DataSource = common.GetYears()
                ddlYear.DataBind()

                con.Close()

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxCredit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdTaxCredit.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" Then
                'get selected row index and corresponding userID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim taxCreditID As Integer = grdTaxCredit.DataKeys(index).Values("taxCreditID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editTaxCredit"
                        Session.Add("taxCreditID", taxCreditID)
                    Case "deleteTaxCredit"
                        deleteTaxCredit(taxCreditID)

                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        con.Open()

                        Dim da As New SqlClient.SqlDataAdapter
                        Dim dt As New DataTable

                        'populate the 'subject yr k12 data set' drop down list                
                        da.SelectCommand = New SqlClient.SqlCommand()
                        da.SelectCommand.Connection = con
                        da.SelectCommand.CommandText = "select 0 as taxCreditID, 'Select Data' as dataSetName, 'Select Data' as year union all select taxCreditID, dataSetName, CONVERT(CHAR,year) from taxCreditDescription where statusID=1"

                        da.Fill(dt)
                        ddlDSN.DataSource = dt
                        ddlDSN.DataValueField = "taxCreditID"
                        ddlDSN.DataTextField = "dataSetName"
                        ddlDSN.DataBind()

                        con.Close()

                        searchTaxCredit()

                        Master.errorMsg = ""

                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editTaxCredit" Then
            Response.Redirect("edittaxcredit.aspx")
        End If


    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("taxCreditID", 0)
        Response.Redirect("edittaxcredit.aspx")
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            searchTaxCredit()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub searchTaxCredit()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        'populate the 'subject yr k12 data set' drop down list                
        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandText = "select taxCreditID, dataSetName, year from taxCreditDescription where statusID = 1"

        If ddlDSN.SelectedValue <> "0" Then
            da.SelectCommand.CommandText += " and taxCreditID=" & ddlDSN.SelectedValue
        End If

        If ddlYear.SelectedItem.Text <> "<Select>" Then
            da.SelectCommand.CommandText += " and year=" & ddlYear.SelectedItem.Text
        End If

        da.Fill(dt)
        grdTaxCredit.DataSource = dt
        grdTaxCredit.DataBind()

        If IsNothing(Cache("taxCreditDataSets")) Then
            Cache.Add("taxCreditDataSets", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("taxCreditDataSets") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If

        con.Close()
    End Sub

    Private Sub grdTaxCredit_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdTaxCredit.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdTaxCredit.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("taxCreditDataSets")) Then
                dt = CType(Cache("taxCreditDataSets"), DataTable)
                grdTaxCredit.DataSource = dt
                grdTaxCredit.DataBind()
            Else
                searchTaxCredit()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxCredit_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdTaxCredit.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("taxCreditDataSets")) Then
                searchTaxCredit()
            End If

            dt = CType(Cache("taxCreditDataSets"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdTaxCredit.DataSource = dt
            grdTaxCredit.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxCredit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTaxCredit.RowDataBound
        Try
            'attaches confirm script to button
            common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "dataSetName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub deleteTaxCredit(ByVal taxCreditID As Integer)

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()
        Dim query As New SqlClient.SqlCommand
        query.Connection = con

        'check if the dataset is being used in another taxYrModel
        Dim dr As SqlClient.SqlDataReader
        query.CommandText = "select taxYearModelID, taxYearModelName from taxYearModelDescription where taxCreditID = " & taxCreditID & " AND (taxYearStatusID=1 or taxYearStatusID=3)"
        dr = query.ExecuteReader

        If dr.Read() Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP65")
            dr.Close()
            con.Close()
            Exit Sub
        Else
            'delete function
            dr.Close()
            query.CommandText = "update taxCreditDescription set statusID=2 where taxCreditID = " & taxCreditID
            query.ExecuteNonQuery()
        End If

        'cleanup
        dr.Close()
        con.Close()

    End Sub
End Class