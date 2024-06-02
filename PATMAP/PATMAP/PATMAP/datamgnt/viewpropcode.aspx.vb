Public Partial Class viewpropcode
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                grdPropCode.PageSize = PATMAP.Global_asax.pageSize

                'fill the present use code grid
                fillPropCodeGrid()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub fillPropCodeGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'fill the present use code grid
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        da = New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select presentuseCodeID, shortDescription from presentUseCodes", con)
        dt = New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdPropCode.DataSource = dt
        grdPropCode.DataBind()

        If IsNothing(Cache("propCode")) Then
            Cache.Add("propCode", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("propCode") = dt
        End If
        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If
    End Sub

    Private Sub grdPropCode_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPropCode.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdPropCode.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("propCode")) Then
                dt = CType(Cache("propCode"), DataTable)
                grdPropCode.DataSource = dt
                grdPropCode.DataBind()
            Else
                fillPropCodeGrid()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdPropCode_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdPropCode.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding userID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim propCodeID As Integer = grdPropCode.DataKeys(index).Values("presentUseCodeID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editPropCode"
                        Session.Add("editPropCode", propCodeID)
                    Case "deletePropCode"

                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'delete jurisdiction type from database
                        query.CommandText = "delete from presentUseCodes where presentUseCodeID = " & propCodeID.ToString
                        con.Open()
                        query.ExecuteNonQuery()
                        con.Close()

                        'update jurisdiction type grid
                        fillPropCodeGrid()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editPropCode" Then
            Response.Redirect("editpropcode.aspx")
        End If

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editPropCode", -1)
        Response.Redirect("editpropcode.aspx")
    End Sub

    Private Sub grdPropCode_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPropCode.RowDataBound
        Try
            'attaches confirm script to button
            common.confirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "shortDescription"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdPropCode_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdPropCode.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("propCode")) Then
                fillPropCodeGrid()
            End If

            dt = CType(Cache("propCode"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdPropCode.DataSource = dt
            grdPropCode.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class