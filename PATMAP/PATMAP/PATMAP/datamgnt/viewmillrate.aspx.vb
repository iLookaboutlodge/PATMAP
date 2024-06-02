Public Partial Class viewmillrate
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)
                BindData_ddl()
                ViewState("sSortBy") = "MunicipalityID"
                ViewState("bCheckDESC") = "false"
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number)
        End Try
    End Sub

    Protected Sub BindData_ddl()
        Dim CommandText As String = " select millRateSurveyID, dataSetName from millRateSurveyDescription where statusID = 1 order by dataSetName " _
        + " select jurisdictionTypeID, jurisdictionType from jurisdictionTypes where jurisdictionTypeID <> 1 order by jurisdictionType"
        Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)
        If (ds.Tables(0).Rows.Count > 0) Then
            ddlDSN.DataSource = ds.Tables(0)
            ddlDSN.DataValueField = "millRateSurveyID"
            ddlDSN.DataTextField = "dataSetName"
            ddlDSN.DataBind()
            ddlDSN.Items.Insert(0, New ListItem("Select Data", "-1"))
            ddlJurisdiction.DataSource = ds.Tables(1)
            ddlJurisdiction.DataValueField = "jurisdictionTypeID"
            ddlJurisdiction.DataTextField = "jurisdictionType"
            ddlJurisdiction.DataBind()
            ddlJurisdiction.Items.Insert(0, New ListItem("Select Data", "-1"))
        Else
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP303")
            btnSearch.Visible = False
            btnAdd.Visible = False
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        DTmillRateSurvey_DataBind()
    End Sub

    Protected Sub DTmillRateSurvey_DataBind()
        Dim CommandText As String = " select M.*, JT.JurisdictionType, D.dataSetName " _
            + " from millRateSurvey as M, millRateSurveyDescription as D, jurisdictionTypes as JT " _
            + " where M.millRateSurveyID = D.millRateSurveyID and JT.jurisdictionTypeID <> 1 " _
            + " and JT.JurisdictionTypeID in (select E.JurisdictionTypeID from entities as E where E.number= M.MunicipalityID)"
        'if (!ddlDSN.SelectedValue.Equals("-1") || !ddlJurisdiction.SelectedValue.Equals("-1"))
        '{
        '    CommandText = CommandText + " where ";
        '}
        If Not ddlDSN.SelectedValue.Equals("-1") Then
            CommandText = (CommandText + (" and M.millRateSurveyID='" _
                        + (ddlDSN.SelectedValue + "'")))
        End If
        'if (!ddlDSN.SelectedValue.Equals("-1") && !ddlJurisdiction.SelectedValue.Equals("-1"))
        '{
        '    CommandText = CommandText + " and ";
        '}
        If Not ddlJurisdiction.SelectedValue.Equals("-1") Then
            CommandText = (CommandText + (" and M.MunicipalityID in (select number from entities where jurisdictionTypeID = '" _
                        + (ddlJurisdiction.SelectedValue + "')")))
        End If
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        txtTotal.Text = dt.Rows.Count.ToString
        ViewState("DTgrdMillRate") = dt
        grdMillRate_DataBind()
    End Sub

    Protected Sub grdMillRate_DataBind()
        Dim sSortBy As String = ViewState("sSortBy").ToString
        Dim bCheckDESC As String = ViewState("bCheckDESC").ToString
        ' set atrributes of DataGrid to remember the sort settings
        grdMillRate.Attributes("SortItem") = sSortBy
        grdMillRate.Attributes("SortDesc") = bCheckDESC.ToString
        ' sort the data view
        Dim dt As DataTable = CType(ViewState("DTgrdMillRate"), DataTable)
        Dim asc As String = ""
        If Boolean.Parse(bCheckDESC) Then
            asc = "DESC"
        Else
            asc = "ASC"
        End If
        dt.DefaultView.Sort = String.Format("{0} {1}", sSortBy, asc)
        'dt.DefaultView.Sort = string.Format("{0} {1}", sSortBy, bool.Parse(bCheckDESC) ? "DESC" : "ASC");
        ' set the data source of DataGrid
        grdMillRate.DataSource = dt
        ' Bind data to DataGrid
        grdMillRate.DataBind()
    End Sub

    Protected Sub grdMillRate_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        grdMillRate.PageIndex = e.NewPageIndex
        grdMillRate_DataBind()
    End Sub

    Protected Sub grdMillRate_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        ' get the new sort item 
        Dim strSortItem As String = e.SortExpression
        ' get the old sort item
        Dim strOldSortItem As String = grdMillRate.Attributes("SortItem")
        ' set the default value to bDescend
        Dim bDescend As Boolean = False
        ' get the old value of bDescend
        Dim bOldDescend As Boolean = (grdMillRate.Attributes("SortDesc") = Boolean.TrueString)
        ' check if the same item be clicked and change the way of sorting
        If (strSortItem = strOldSortItem) Then
            bDescend = Not bOldDescend
        End If
        ' databind sorted data
        ViewState("sSortBy") = strSortItem
        ViewState("bCheckDESC") = bDescend.ToString
        grdMillRate_DataBind()
    End Sub

    Protected Sub grdMillRate_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try
            ' If grid is not being sorted
            If (Not e.CommandName.ToLower.Equals("sort") _
                        AndAlso Not e.CommandName.ToLower.Equals("page")) Then
                ' get selected row index and corresponding userID to that row
                Dim index As Integer = Integer.Parse(e.CommandArgument.ToString)
                Dim editRowID As String = grdMillRate.DataKeys(index)("RowID").ToString
                ' check what type of command has been fired by the grid
                Select Case (e.CommandName)
                    Case "colEdit"
                        Session.Add("millRateSurvey_RowID", editRowID)
                        Response.Redirect("editmillrate.aspx")
                    Case "colDelete"
                        Dim CommandText As String = ("DELETE FROM millRateSurvey WHERE RowID = '" _
                                    + (editRowID + "'"))
                        SqlDbAccess.RunSql(CommandText)
                        DTmillRateSurvey_DataBind()
                End Select
            End If
        Catch

        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Session("millRateSurvey_RowID") = Nothing
        Response.Redirect("editmillrate.aspx")
    End Sub


    Private Sub grdMillRate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMillRate.RowDataBound
        Try
            'attaches confirm script to button
            common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "dataSetName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class