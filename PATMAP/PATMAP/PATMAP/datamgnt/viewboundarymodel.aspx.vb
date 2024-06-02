Imports System.IO

Partial Public Class viewboundarymodel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                grdBoundaryModel.PageSize = PATMAP.Global_asax.pageSize

                rdlStatus.SelectedIndex = 0

                ddlYear.DataSource = common.GetYears()
                ddlYear.DataBind()
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub performBoundaryModelSearch()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'setup the query to get entity details
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select boundaryModelID, boundaryModelName, [year], status from boundaryModel where 1 = 1 ", con)

        If ddlYear.SelectedValue <> "<Select>" Then
            da.SelectCommand.CommandText += " and year = '" & ddlYear.SelectedValue & "'"
        End If
        If Not String.IsNullOrEmpty(Trim(txtBoundaryModel.Text)) Then
            da.SelectCommand.CommandText += " and boundaryModelName like '%" & Trim(txtBoundaryModel.Text) & "%'"
        End If

        If Not IsNothing(rdlStatus.SelectedItem) Then
            da.SelectCommand.CommandText += " and status = (select StatusID from status where Status = '" & rdlStatus.SelectedValue & "')"
        End If

        'get the data for the entity grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdBoundaryModel.DataSource = dt
        grdBoundaryModel.DataBind()

        If IsNothing(Cache("boundaryModel")) Then
            Cache.Add("boundaryModel", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("boundaryModel") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If
    End Sub

    Private Sub grdBoundaryModel_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdBoundaryModel.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdBoundaryModel.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("boundaryModel")) Then
                dt = CType(Cache("boundaryModel"), DataTable)
                grdBoundaryModel.DataSource = dt
                grdBoundaryModel.DataBind()
            Else
                performBoundaryModelSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdBoundaryModel_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdBoundaryModel.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" Then

                'get selected row index and corresponding boundaryModelID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim boundaryModelID As String = grdBoundaryModel.DataKeys(index).Values("boundaryModelID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editBoundaryModel"
                        Session.Add("boundaryModelID", boundaryModelID)
                    Case "deleteBoundaryModel"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'send boundary model to history
                        query.CommandText = "update boundaryModel set status = 2 where boundaryModelID = " & boundaryModelID
                        con.Open()
                        query.ExecuteNonQuery()
                        con.Close()

                        'update boundary search grid
                        performBoundaryModelSearch()

                        Master.errorMsg = ""
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editBoundaryModel" Then
            Response.Redirect("editboundarymodel.aspx")
        End If

    End Sub

    Private Sub grdBoundaryModel_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdBoundaryModel.RowDataBound
        Try
            'attaches confirm script to button
            common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "boundaryModelName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            'fill grid with search results
            performBoundaryModelSearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdBoundaryModel_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdBoundaryModel.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("boundaryModel")) Then
                performBoundaryModelSearch()
            End If

            dt = CType(Cache("boundaryModel"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdBoundaryModel.DataSource = dt
            grdBoundaryModel.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("boundaryModelID", 0)
        Response.Redirect("editboundarymodel.aspx")
    End Sub
End Class