Public Partial Class viewjuristype
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        Try

            'Clears out the error message
            Master.errorMsg = ""

            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                grdJurisType.PageSize = PATMAP.Global_asax.pageSize

                'fill the jurisdiction type grid
                fillJurisTypeGrid()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub fillJurisTypeGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'fill the jurisdiction type grid
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        da = New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select jurisdictionTypeID, jurisdictionType, description from jurisdictionTypes", con)
        dt = New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdJurisType.DataSource = dt
        grdJurisType.DataBind()

        If IsNothing(Cache("jurisTypes")) Then
            Cache.Add("jurisTypes", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("jurisTypes") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If
    End Sub

    Private Sub grdJurisType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdJurisType.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdJurisType.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("jurisTypes")) Then
                dt = CType(Cache("jurisTypes"), DataTable)
                grdJurisType.DataSource = dt
                grdJurisType.DataBind()
            Else
                fillJurisTypeGrid()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdJurisType_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdJurisType.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding userID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim jurisTypeID As Integer = grdJurisType.DataKeys(index).Values("jurisdictionTypeID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editJurisType"
                        Session.Add("editJurisTypeID", jurisTypeID)
                    Case "deleteJurisType"

                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'delete jurisdiction type from database
                        query.CommandText = "delete from jurisdictiontypes where jurisdictionTypeID = " & jurisTypeID.ToString
                        con.Open()
                        query.ExecuteNonQuery()
                        con.Close()

                        'update jurisdiction type grid
                        fillJurisTypeGrid()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editJurisType" Then
            Response.Redirect("editjuristype.aspx")
        End If

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editJurisTypeID", 0)
        Response.Redirect("editjuristype.aspx")
    End Sub

    Private Sub grdJurisType_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdJurisType.RowDataBound
        Try
            If DataBinder.Eval(e.Row.DataItem, "jurisdictionTypeID") = 1 Then
                Dim btnDel As System.Web.UI.WebControls.LinkButton

                btnDel = CType(e.Row.Cells.Item(1).Controls.Item(0), LinkButton)
                btnDel.Enabled = False
            Else
                'attaches confirm script to button
                common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "jurisdictionType"))
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdJurisType_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdJurisType.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("jurisTypes")) Then
                fillJurisTypeGrid()
            End If

            dt = CType(Cache("jurisTypes"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdJurisType.DataSource = dt
            grdJurisType.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class