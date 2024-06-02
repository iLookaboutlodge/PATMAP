Public Partial Class viewgroup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not Page.IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.userGroup)

                grdGroups.PageSize = PATMAP.Global_asax.pageSize

                'fill the groups grid
                fillGroupsGrid()

            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub fillGroupsGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'fill in the pending requests table
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        da = New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select groups.groupID, groupName, count(users.userID) as memberCount,description from groups left join users on groups.groupID = users.groupID group by groups.groupID, groupName, description", con)
        dt = New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdGroups.DataSource = dt
        grdGroups.DataBind()

        If IsNothing(Cache("groups")) Then
            Cache.Add("groups", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("groups") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If

    End Sub

    Private Sub grdGroups_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdGroups.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdGroups.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("groups")) Then
                dt = CType(Cache("groups"), DataTable)
                grdGroups.DataSource = dt
                grdGroups.DataBind()
            Else
                fillGroupsGrid()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdGroups_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdGroups.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding userID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim groupID As Integer = grdGroups.DataKeys(index).Values("groupID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editGroup"
                        Session.Add("editGroupID", groupID)
                    Case "deleteGroup"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con
                        con.Open()

                        'set any users in that group to null
                        query.CommandText = "update users set groupID = null where groupID = " & groupID.ToString & vbCrLf
                        query.CommandText += "delete from groups where groupID = " & groupID.ToString & vbCrLf

                        Dim trans As SqlClient.SqlTransaction
                        trans = con.BeginTransaction()
                        query.Transaction = trans
                        Try
                            query.ExecuteNonQuery()
                            trans.Commit()
                        Catch
                            trans.Rollback()
                            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
                            con.Close()
                            Exit Sub
                        End Try

                        con.Close()

                        'update groups grid
                        fillGroupsGrid()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editGroup" Then
            Response.Redirect("editgroup.aspx")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editGroupID", 0)
        Response.Redirect("editgroup.aspx")
    End Sub

    Private Sub grdGroups_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdGroups.RowDataBound
        Try
            'attaches confirm script to button
            common.confirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "groupName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdGroups_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdGroups.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("groups")) Then
                fillGroupsGrid()
            End If

            dt = CType(Cache("groups"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdGroups.DataSource = dt
            grdGroups.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class