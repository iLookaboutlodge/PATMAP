Public Partial Class viewpotash
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)
                BindData_ddlDSN()
                ViewState("sSortBy") = "MunicipalityID"
                ViewState("bCheckDESC") = "false"
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
    Protected Sub BindData_ddlDSN()
        Dim CommandText As String = " select potashID, dataSetName from potashDescription where statusID = 1 order by dataSetName"
        '+ " select municipalityID from potashRural where potashID <> 1 order by totalPoints";
        Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)
        If (ds.Tables(0).Rows.Count > 0) Then
            ddlDSN.DataSource = ds.Tables(0)
            ddlDSN.DataValueField = "potashID"
            ddlDSN.DataTextField = "dataSetName"
            ddlDSN.DataBind()
            ddlMunicipalityType.Items.Insert(0, "Select Type")
            ddlMunicipalityType.Items.Insert(1, "Rural")
            ddlMunicipalityType.Items.Insert(2, "Urban")
            ddlMunicipalityType.SelectedIndex = 0
            BindData_ddl(Integer.Parse(ddlDSN.SelectedValue.ToString))
        Else
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP303")
            btnSearch.Visible = False
            btnAdd.Visible = False
        End If
    End Sub

    Protected Sub ddlDSN_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        BindData_ddl(Integer.Parse(ddlDSN.SelectedValue.ToString))
    End Sub

    Protected Sub ddlMunicipalityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        BindData_ddl(Integer.Parse(ddlDSN.SelectedValue.ToString))
    End Sub

    Protected Sub BindData_ddl(ByVal potashID As Integer)
        Dim CommandText As String = ""
        Dim MunicipalityType As Integer = Integer.Parse(ddlMunicipalityType.SelectedIndex.ToString)
        If (MunicipalityType = 0) Then
            CommandText = " select distinct Un.municipalityID from (" _
            + " select municipalityID from potashRural where potashID = '" + potashID.ToString() + "'" _
            + " union " _
            + " select municipalityID from potashUrban where potashID = '" + potashID.ToString() + "'" _
            + ") as Un order by municipalityID"
        End If
        If (MunicipalityType = 1) Then
            CommandText = " select municipalityID from potashRural where potashID = '" + potashID.ToString() + "' order by municipalityID"
        End If
        If (MunicipalityType = 2) Then
            CommandText = " select municipalityID from potashUrban where potashID = '" + potashID.ToString() + "' order by municipalityID"
        End If
        Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)
        ddlMunicipalityID.DataSource = ds.Tables(0)
        'ddlMunicipalityID.DataValueField = "municipalityID";
        ddlMunicipalityID.DataTextField = "municipalityID"
        ddlMunicipalityID.DataBind()
        ddlMunicipalityID.Items.Insert(0, New ListItem("Select Municipality ID", "-1"))
        'ddlMunicipalityID.SelectedIndex = 0;
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        DTPotash_DataBind()
    End Sub

    Protected Sub DTPotash_DataBind()
        Dim CommandText As String = ""
        CommandText = " select U.RowID, U.potashAreaID, U.municipalityID, U.totalPoints, U.totalGrant, U.PotashType,U.PotashID from (" _
                + " select RowID, potashAreaID, municipalityID, totalPoints, totalGrant, 'Rural' as PotashType, PotashID from PotashRural where PotashID = '" + ddlDSN.SelectedValue + "'" _
                + " union " _
                + " select RowID, potashAreaID, municipalityID, totalPoints, totalGrant, 'Urban' as PotashType, PotashID from PotashUrban where PotashID = '" + ddlDSN.SelectedValue + "'" _
                + ") as U where U.PotashID = '" + ddlDSN.SelectedValue + "'"
        If (Integer.Parse(ddlMunicipalityType.SelectedIndex.ToString) <> 0) Then
            CommandText = (CommandText + (" and U.PotashType='" _
                        + (ddlMunicipalityType.SelectedValue + "'")))
        End If
        If Not ddlMunicipalityID.SelectedValue.Trim.Equals("-1") Then
            CommandText = (CommandText + (" and U.municipalityID='" _
                        + (ddlMunicipalityID.SelectedValue + "'")))
        End If
        If Not txtpotashAreaID.Text.Trim.Equals(String.Empty) Then
            CommandText = (CommandText + (" and U.potashAreaID='" _
                        + (txtpotashAreaID.Text + "'")))
        End If
        CommandText = (CommandText + " order by U.municipalityID")
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        txtTotal.Text = dt.Rows.Count.ToString
        ViewState("DTgrdPotash") = dt
        grdPotash_DataBind()
    End Sub

    Protected Sub grdPotash_DataBind()
        Dim sSortBy As String = ViewState("sSortBy").ToString
        Dim bCheckDESC As String = ViewState("bCheckDESC").ToString
        ' set atrributes of DataGrid to remember the sort settings
        grdPotash.Attributes("SortItem") = sSortBy
        grdPotash.Attributes("SortDesc") = bCheckDESC.ToString
        ' sort the data view
        Dim dt As DataTable = CType(ViewState("DTgrdPotash"), DataTable)
        Dim asc As String = ""
        If Boolean.Parse(bCheckDESC) Then
            asc = "DESC"
        Else
            asc = "ASC"
        End If
        dt.DefaultView.Sort = String.Format("{0} {1}", sSortBy, asc)
        'dt.DefaultView.Sort = string.Format("{0} {1}", sSortBy, bool.Parse(bCheckDESC) ? "DESC" : "ASC");
        ' set the data source of DataGrid
        grdPotash.DataSource = dt
        ' Bind data to DataGrid
        grdPotash.DataBind()
    End Sub

    Protected Sub grdPotash_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        grdPotash.PageIndex = e.NewPageIndex
        grdPotash_DataBind()
    End Sub

    Protected Sub grdPotash_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        ' get the new sort item 
        Dim strSortItem As String = e.SortExpression
        ' get the old sort item
        Dim strOldSortItem As String = grdPotash.Attributes("SortItem")
        ' set the default value to bDescend
        Dim bDescend As Boolean = False
        ' get the old value of bDescend
        Dim bOldDescend As Boolean = (grdPotash.Attributes("SortDesc") = Boolean.TrueString)
        ' check if the same item be clicked and change the way of sorting
        If (strSortItem = strOldSortItem) Then
            bDescend = Not bOldDescend
        End If
        ' databind sorted data
        ViewState("sSortBy") = strSortItem
        ViewState("bCheckDESC") = bDescend.ToString
        grdPotash_DataBind()
    End Sub

    Protected Sub grdPotash_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try
            ' If grid is not being sorted
            If (Not e.CommandName.ToLower.Trim.Equals("sort") _
                        AndAlso Not e.CommandName.ToLower.Trim.Equals("page")) Then
                ' get selected row index and corresponding userID to that row
                Dim index As Integer = Integer.Parse(e.CommandArgument.ToString)
                Dim editRowID As String = grdPotash.DataKeys(index)("RowID").ToString
                Dim PotashType As String = grdPotash.Rows(index).Cells(7).Text
                ' check what type of command has been fired by the grid
                Select Case (e.CommandName)
                    Case "colEdit"
                        Session.Add("potash_RowID", editRowID)
                        Session.Add("potash_PotashType", PotashType)
                        Response.Redirect("editpotash.aspx")
                    Case "colDelete"
                        'if(PotashType.ToLower().Trim().Equals("rural"))
                        Dim CommandText As String = ("DELETE FROM potash" _
                                    + (PotashType + (" WHERE RowID = '" _
                                    + (editRowID + "'"))))
                        SqlDbAccess.RunSql(CommandText)
                        DTPotash_DataBind()
                End Select
            End If
        Catch

        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Session("potash_RowID") = Nothing
        Session("potash_PotashType") = Nothing
        Response.Redirect("editpotash.aspx")
    End Sub

    Private Sub grdPotash_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPotash.RowDataBound
        Try
            'attaches confirm script to button
            common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "potashAreaID"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class