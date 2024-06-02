Public Partial Class viewtaxstatus
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Clears out the error message
        Master.errorMsg = ""

        If Not Page.IsPostBack Then
            Try
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                grdTaxStatus.PageSize = PATMAP.Global_asax.pageSize

                'fill the tax status grid
                fillTaxStatusGrid()
            Catch
                'retrieves error message
                Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            End Try
        End If

    End Sub

    Private Sub fillTaxStatusGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'fill the tax status grid
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        da = New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select taxStatusID, taxStatus, description from taxStatus", con)
        dt = New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdTaxStatus.DataSource = dt
        grdTaxStatus.DataBind()

        If IsNothing(Cache("taxStatuses")) Then
            Cache.Add("taxStatuses", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("taxStatuses") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If
    End Sub

    Private Sub grdTaxStatus_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdTaxStatus.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdTaxStatus.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("taxStatuses")) Then
                dt = CType(Cache("taxStatuses"), DataTable)
                grdTaxStatus.DataSource = dt
                grdTaxStatus.DataBind()
            Else
                fillTaxStatusGrid()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxStatus_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdTaxStatus.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding userID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim taxStatusID As Integer = grdTaxStatus.DataKeys(index).Values("taxStatusID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editTaxStatus"
                        Session.Add("editTaxStatusID", taxStatusID)
                    Case "deleteTaxStatus"

                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'delete jurisdiction type from database
                        query.CommandText = "delete from taxStatus where taxStatusID = " & taxStatusID.ToString
                        con.Open()
                        query.ExecuteNonQuery()
                        con.Close()

                        'update jurisdiction type grid
                        fillTaxStatusGrid()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editTaxStatus" Then
            Response.Redirect("edittaxstatus.aspx")
        End If

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editTaxStatusID", 0)
        Response.Redirect("edittaxstatus.aspx")
    End Sub

    Private Sub grdTaxStatus_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTaxStatus.RowDataBound
        Try
            'attaches confirm script to button
            common.confirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "taxStatus"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxStatus_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdTaxStatus.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("taxStatuses")) Then
                fillTaxStatusGrid()
            End If

            dt = CType(Cache("taxStatuses"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdTaxStatus.DataSource = dt
            grdTaxStatus.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class