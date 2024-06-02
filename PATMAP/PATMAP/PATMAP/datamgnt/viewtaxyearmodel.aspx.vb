Imports System.IO

Partial Public Class viewtaxyearmodel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Clears out the error message
        Master.errorMsg = ""

        If Not IsPostBack Then
            Try
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.taxYearModel)

                grdTaxYrModel.PageSize = PATMAP.Global_asax.pageSize

                rdlStatus.SelectedIndex = 0

                'populate the year dropdownlist
                ddlYear.DataSource = common.GetYears()
                ddlYear.DataBind()
            Catch
                'retrieves error message
                Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            End Try
        End If
    End Sub

    Private Sub performTaxYrModelSearch()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'setup the query to get entity details
        Dim da As New SqlClient.SqlDataAdapter

        'create a base query where year or tax year model is not selected
        da.SelectCommand = New SqlClient.SqlCommand("select taxYearModelID, taxYearModelName, [year], taxYearStatusID from taxYearModelDescription where 1 = 1 ", con)

        'if the year is selected
        If ddlYear.SelectedValue <> "<Select>" Then
            da.SelectCommand.CommandText += " and year = '" & ddlYear.SelectedValue & "'"
        End If
        'if the tax year model name is given
        If Not String.IsNullOrEmpty(Trim(txtTaxYrModel.Text)) Then
            da.SelectCommand.CommandText += " and taxYearModelName like '%" & Trim(txtTaxYrModel.Text) & "%'"
        End If
        'active or history
        If Not IsNothing(rdlStatus.SelectedItem) Then
            da.SelectCommand.CommandText += " and taxYearStatusID = (select taxYearStatusID from taxYearStatus where taxYearStatus = '" & rdlStatus.SelectedValue & "')"
        End If

        'get the data for the entity grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdTaxYrModel.DataSource = dt
        grdTaxYrModel.DataBind()

        'cache the data grid if it is not already in the cache
        If IsNothing(Cache("taxYrModel")) Then
            Cache.Add("taxYrModel", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("taxYrModel") = dt
        End If

        'set the number of rows found
        txtTotal.Text = dt.Rows.Count

        'if there is no result, then display the message
        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If
    End Sub

    Private Sub grdTaxYrModel_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdTaxYrModel.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdTaxYrModel.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("taxYrModel")) Then
                dt = CType(Cache("taxYrModel"), DataTable)
                grdTaxYrModel.DataSource = dt
                grdTaxYrModel.DataBind()
            Else
                performTaxYrModelSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxYrModel_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdTaxYrModel.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then

                'get selected row index and corresponding taxYearModelID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim taxYearModelID As String = grdTaxYrModel.DataKeys(index).Values("taxYearModelID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editTaxYearModel"
                        Session.Add("taxYearModelID", taxYearModelID)
                    Case "deleteTaxYearModel"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        con.Open()

                        'check if the dataset is being used in a scenario
                        Dim dr As SqlClient.SqlDataReader
                        query.CommandText = "select assessmentTaxModelID from assessmentTaxModel where assessmentTaxModel.BaseTaxYearModelID=" & taxYearModelID & " or SubjectTaxYearModelID=" & taxYearModelID
                        dr = query.ExecuteReader

                        If dr.Read() Then
                            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP105")
                            dr.Close()
                            con.Close()
                            Exit Sub
                        Else
                            'delete tax year model
                            dr.Close()

                            'set tax year model status to delete
                            query.CommandText = "update taxYearModelDescription set taxYearStatusID = 2 where taxYearModelID = " & taxYearModelID
                            query.ExecuteNonQuery()
                        End If

                        'cleanup
                        con.Close()

                        'update tax year model search grid
                        performTaxYrModelSearch()

                    Case "restoreTaxYearModel"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'restore tax year model from history
                        query.CommandText = "update taxYearModelDescription set taxYearStatusID = 1 where taxYearModelID = " & taxYearModelID
                        con.Open()
                        query.ExecuteNonQuery()
                        con.Close()

                        'update tax year model search grid
                        performTaxYrModelSearch()

                    Case "toHistoryTaxYearModel"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        con.Open()

                        'check if the dataset is being used in a scenario
                        Dim dr As SqlClient.SqlDataReader
                        query.CommandText = "select assessmentTaxModelID from assessmentTaxModel where assessmentTaxModel.BaseTaxYearModelID=" & taxYearModelID & " or assessmentTaxModel.SubjectTaxYearModelID=" & taxYearModelID
                        dr = query.ExecuteReader

                        If dr.Read() Then
                            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP114")
                            Exit Sub
                        Else
                            dr.Close()

                            'send tax year model to history
                            query.CommandText = "update taxYearModelDescription set taxYearStatusID = 3 where taxYearModelID = " & taxYearModelID
                            query.ExecuteNonQuery()
                        End If

                        'cleanup
                        con.Close()

                        'update tax year model search grid
                        performTaxYrModelSearch()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editTaxYearModel" Then
            Response.Redirect("edittaxyearmodel.aspx")
        End If

    End Sub

    Private Sub grdTaxYrModel_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTaxYrModel.RowDataBound
        Try
            'if current row is a datarow type
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim btnHistory As System.Web.UI.WebControls.LinkButton

                btnHistory = CType(e.Row.Cells.Item(2).Controls.Item(0), LinkButton)

                If DataBinder.Eval(e.Row.DataItem, "taxYearStatusID") = 3 Then
                    'If user is searching records in history, change "To History" button
                    'to "Restore"
                    btnHistory.Text = "<img src='../images/btnSmRestore.gif'>"
                    btnHistory.CommandName = "restoreTaxYearModel"
                Else
                    'If user is searching active records, change back to "To History" button
                    btnHistory.Text = "<img src='../images/btnSmHistory.gif'>"
                    btnHistory.CommandName = "toHistoryTaxYearModel"
                End If

                'attaches confirm script to button
                common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "taxYearModelName"))
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            'fill grid with search results
            performTaxYrModelSearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxYrModel_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdTaxYrModel.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("taxYrModel")) Then
                performTaxYrModelSearch()
            End If

            dt = CType(Cache("taxYrModel"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdTaxYrModel.DataSource = dt
            grdTaxYrModel.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
    
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("taxYearModelID", 0)
        Response.Redirect("edittaxyearmodel.aspx")
    End Sub
End Class