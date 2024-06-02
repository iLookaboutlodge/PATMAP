Public Partial Class viewlevels
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.userGroup)

                grdLevels.PageSize = PATMAP.Global_asax.pageSize

                'fill the levels grid
                filllevelsGrid()
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try


    End Sub

    Private Sub filllevelsGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'fill in the pending requests table
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        da = New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select levelID, levelName, description from levels", con)
        dt = New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdLevels.DataSource = dt
        grdLevels.DataBind()

        If IsNothing(Cache("userLevels")) Then
            Cache.Add("userLevels", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("userLevels") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editLevelID", "0")
        Response.Redirect("editlevel.aspx")
    End Sub

    Private Sub grdLevels_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLevels.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdLevels.PageIndex = e.NewPageIndex

            'fill the users level grid
            If Not IsNothing(Cache("userLevels")) Then
                dt = CType(Cache("userLevels"), DataTable)
                grdLevels.DataSource = dt
                grdLevels.DataBind()
            Else
                filllevelsGrid()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdLevels_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdLevels.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding editlevelID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim editlevelID As String = grdLevels.DataKeys(index).Values("levelID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editLevel"
                        Session.Add("editLevelID", editlevelID)
                    Case "deleteLevel"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        Dim dr As SqlClient.SqlDataReader

                        query.CommandText = "select count(*) from userLevels where levelID = " & editlevelID
                        con.Open()
                        dr = query.ExecuteReader()
                        dr.Read()

                        If dr.Item(0) > 0 Then
                            'retrieves error message
                            Master.errorMsg = common.GetErrorMessage("PATMAP121")
                            con.Close()
                            Exit Sub
                        End If

                        dr.Close()

                        'delete user level
                        query.CommandText = "delete from levels where levelID = " & editlevelID & vbCrLf & _
                                            "delete from levelsPermission where levelID = " & editlevelID & vbCrLf & _
                                            "delete from taxClassesPermission where levelID = " & editlevelID & vbCrLf & _
                                            "delete from LTTlevels where levelID = " & editlevelID
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

                        'update search grid
                        filllevelsGrid()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editLevel" Then
            Response.Redirect("editlevel.aspx")
        End If

    End Sub

    Private Sub grdLevels_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLevels.RowDataBound
        Try
            'attaches confirm script to button
            common.confirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "levelName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdLevels_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdLevels.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("userLevels")) Then
                filllevelsGrid()
            End If

            dt = CType(Cache("userLevels"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdLevels.DataSource = dt
            grdLevels.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub grdLevels_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class