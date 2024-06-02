Public Partial Class viewkog
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)
                BindData_ddl()
                ViewState("sSortBy") = "K12ID"
                ViewState("bCheckDESC") = "false"

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number)
        End Try
    End Sub

    Protected Sub BindData_ddl()
        Dim CommandText As String = " select K12ID, dataSetName from K12Description where statusID = 1 select distinct schoolID from K12"
        Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)
        If (ds.Tables(0).Rows.Count > 0) Then
            ddlDSN.DataSource = ds.Tables(0)
            ddlDSN.DataValueField = "K12ID"
            ddlDSN.DataTextField = "dataSetName"
            ddlDSN.DataBind()
            ddlDSN.Items.Insert(0, New ListItem("Select Data", "-1"))
            ddlSchoolDivision.DataSource = ds.Tables(1)
            ddlSchoolDivision.DataTextField = "schoolID"
            ddlSchoolDivision.DataBind()
            ddlSchoolDivision.Items.Insert(0, New ListItem("Select Data", "-1"))
        Else
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP303")
            btnSearch.Visible = False
            btnAdd.Visible = False
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        DTK12_DataBind()
    End Sub

    Protected Sub DTK12_DataBind()
        Dim CommandText As String = " select K.RowID, K.K12ID, K.schoolID, D.dataSetName from K12 AS K, K12Description AS D where K.K12ID = D.K12ID "
        If Not ddlDSN.SelectedValue.Equals("-1") Then
            CommandText = (CommandText + (" and K.K12ID='" _
                        + (ddlDSN.SelectedValue + "'")))
        End If
        If Not ddlSchoolDivision.SelectedValue.Equals("-1") Then
            CommandText = (CommandText + (" and K.schoolID='" _
                        + (ddlSchoolDivision.SelectedValue + "'")))
        End If
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        txtTotal.Text = dt.Rows.Count.ToString
        ViewState("DTgrdKOG") = dt
        grdKOG_DataBind()
    End Sub

    Protected Sub grdKOG_DataBind()
        Dim sSortBy As String = ViewState("sSortBy").ToString
        Dim bCheckDESC As String = ViewState("bCheckDESC").ToString
        ' set atrributes of DataGrid to remember the sort settings
        grdKOG.Attributes("SortItem") = sSortBy
        grdKOG.Attributes("SortDesc") = bCheckDESC.ToString
        ' sort the data view
        Dim dt As DataTable = CType(ViewState("DTgrdKOG"), DataTable)
        Dim asc As String = ""
        If Boolean.Parse(bCheckDESC) Then
            asc = "DESC"
        Else
            asc = "ASC"
        End If
        dt.DefaultView.Sort = String.Format("{0} {1}", sSortBy, asc)
        'dt.DefaultView.Sort = string.Format("{0} {1}", sSortBy, bool.Parse(bCheckDESC) ? "DESC" : "ASC");
        ' set the data source of DataGrid
        grdKOG.DataSource = dt
        ' Bind data to DataGrid
        grdKOG.DataBind()
    End Sub

    Protected Sub grdKOG_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        grdKOG.PageIndex = e.NewPageIndex
        grdKOG_DataBind()
    End Sub

    Protected Sub grdKOG_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        ' get the new sort item 
        Dim strSortItem As String = e.SortExpression
        ' get the old sort item
        Dim strOldSortItem As String = grdKOG.Attributes("SortItem")
        ' set the default value to bDescend
        Dim bDescend As Boolean = False
        ' get the old value of bDescend
        Dim bOldDescend As Boolean = (grdKOG.Attributes("SortDesc") = Boolean.TrueString)
        ' check if the same item be clicked and change the way of sorting
        If (strSortItem = strOldSortItem) Then
            bDescend = Not bOldDescend
        End If
        ' databind sorted data
        ViewState("sSortBy") = strSortItem
        ViewState("bCheckDESC") = bDescend.ToString
        grdKOG_DataBind()
    End Sub

    Protected Sub grdKOG_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try
            ' If grid is not being sorted
            If (Not e.CommandName.ToLower.Equals("sort") _
             AndAlso Not e.CommandName.ToLower.Equals("page")) Then
                ' get selected row index and corresponding userID to that row
                Dim index As Integer = Integer.Parse(e.CommandArgument.ToString)
                Dim editRowID As String = grdKOG.DataKeys(index)("RowID").ToString
                ' check what type of command has been fired by the grid
                Select Case (e.CommandName)
                    Case "colEdit"
                        Session.Add("K12_RowID", editRowID)
                        Response.Redirect("editkog.aspx")
                    Case "colDelete"
                        Dim CommandText As String = ("DELETE FROM K12 WHERE RowID = '" _
                                    + (editRowID + "'"))
                        SqlDbAccess.RunSql(CommandText)
                        DTK12_DataBind()
                End Select
            End If
        Catch
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Session("K12_RowID") = Nothing
        Response.Redirect("editkog.aspx")
    End Sub


    Private Sub grdKOG_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdKOG.RowDataBound
        Try
            'attaches confirm script to button
            common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "schoolID"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class