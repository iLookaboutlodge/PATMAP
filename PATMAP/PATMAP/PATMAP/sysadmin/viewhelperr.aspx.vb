Public Partial Class viewhelperr
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.helpText)

                grdErrors.PageSize = PATMAP.Global_asax.pageSize
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub grdErrors_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdErrors.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdErrors.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("helpErrors")) Then
                dt = CType(Cache("helpErrors"), DataTable)
                grdErrors.DataSource = dt
                grdErrors.DataBind()
            Else
                performErrorCodeSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            'fill grid with search results
            performErrorCodeSearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub performErrorCodeSearch()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'setup the query to get entity details
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select errorCode, errorName, description from errorcodes where 1 = 1 ", con)

        txtErrName.Text = Trim(txtErrName.Text.Replace("'", "''"))
        txtCode.Text = Trim(txtCode.Text.Replace("'", "''"))

        If Not (txtErrName.Text = "") Then
            da.SelectCommand.CommandText += " and charindex('" & Trim(txtErrName.Text) & "',errorName) > 0 "
        End If
        If Not (txtCode.Text = "") Then
            da.SelectCommand.CommandText += " and charindex('" & Trim(txtCode.Text) & "',errorCode) > 0 "
        End If

        'get the data for the entity grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdErrors.DataSource = dt
        grdErrors.DataBind()

        If IsNothing(Cache("helpErrors")) Then
            Cache.Add("helpErrors", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("helpErrors") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editHelpErrorID", "")
        Response.Redirect("edithelperr.aspx")
    End Sub

    Private Sub grdErrors_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdErrors.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding helpScreenID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim editHelpErrorID As String = grdErrors.DataKeys(index).Values("errorCode")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editErrorCode"
                        Session.Add("editHelpErrorID", editHelpErrorID)
                    Case "deleteErrorCode"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'delete help control
                        query.CommandText = "delete from errorCodes where errorCode = '" & editHelpErrorID & "'" & vbCrLf & _
                                            "delete from errorCodesReset where errorCode = '" & editHelpErrorID & "'"
                        con.Open()

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

                        'update help control search grid
                        performErrorCodeSearch()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editErrorCode" Then
            Response.Redirect("edithelperr.aspx")
        End If

    End Sub

    Private Sub grdErrors_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdErrors.RowDataBound
        Try
            'attaches confirm script to button
            common.confirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "errorName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdErrors_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdErrors.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("helpErrors")) Then
                performErrorCodeSearch()
            End If

            dt = CType(Cache("helpErrors"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdErrors.DataSource = dt
            grdErrors.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class