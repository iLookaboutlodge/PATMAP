Public Partial Class viewpov
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                grdPOV.PageSize = PATMAP.Global_asax.pageSize

                ddlYear.DataSource = common.GetYears()
                ddlYear.DataBind()

                BindData_ddl()

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub BindData_ddl()
        Dim CommandText As String = "select 0 as POVID, 'Select Data' as dataSetName union all select POVID, dataSetName from POVDescription where statusID = 1"

        Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)
        If (ds.Tables(0).Rows.Count > 0) Then
            ddlDSN.DataSource = ds.Tables(0)
            ddlDSN.DataValueField = "POVID"
            ddlDSN.DataTextField = "dataSetName"
            ddlDSN.DataBind()
            ddlDSN.Items.Insert(0, New ListItem("Select Data", "-1"))
            'Else
            '    txtTotal.Text = "No data in database"
            '    btnSearch.Visible = False
            '    btnAdd.Visible = False
        End If
    End Sub

    Private Sub grdPOV_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdPOV.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding userID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim POVID As Integer = grdPOV.DataKeys(index).Values("POVID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editPOV"
                        Session.Add("POVID", POVID)
                    Case "deletePOV"
                        deletePOV(POVID)

                        If grdPOV.Rows.Count > 1 Then
                            'update function search grid
                            performPOVSearch()
                        Else
                            BindData_ddl()
                            grdPOV.DataSource = Nothing
                            grdPOV.DataBind()
                            txtTotal.Text = ""
                        End If

                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editPOV" Then
            Response.Redirect("editpov.aspx")
        End If

    End Sub

    Protected Sub deletePOV(ByVal POVID As Integer)

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()
        Dim query As New SqlClient.SqlCommand
        query.Connection = con

        'check if the dataset is being used in another taxYrModel
        Dim dr As SqlClient.SqlDataReader
        query.CommandText = "select taxYearModelID, taxYearModelName from taxYearModelDescription where POVID = " & POVID & " AND (taxYearStatusID=1 or taxYearStatusID=3)"
        dr = query.ExecuteReader

        If dr.Read() Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP65")
            dr.Close()
            con.Close()
            Exit Sub
        Else
            'delete function
            dr.Close()
            query.CommandText = "update POVDescription set statusID=2 where POVID = " & POVID
            query.ExecuteNonQuery()
        End If

        'cleanup
        dr.Close()
        con.Close()

    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("POVID", 0)
        Response.Redirect("editpov.aspx")
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        performPOVSearch()
    End Sub

    Private Sub performPOVSearch()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'setup the query to get entity details
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select POVID, dataSetName, [year], statusID from POVDescription where statusID = 1 ", con)

        If ddlYear.SelectedItem.Text <> "<Select>" Then
            da.SelectCommand.CommandText += " and year = " & ddlYear.SelectedValue
        End If

        If ddlDSN.SelectedItem.Text <> "Select Data" Then
            da.SelectCommand.CommandText += " and dataSetName = '" & ddlDSN.SelectedItem.Text & "'"
        End If

        'get the data for the entity grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdPOV.DataSource = dt
        grdPOV.DataBind()

        If IsNothing(Cache("POV")) Then
            Cache.Add("POV", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("POV") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If

    End Sub

    Private Sub grdPOV_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPOV.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdPOV.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("POV")) Then
                dt = CType(Cache("POV"), DataTable)
                grdPOV.DataSource = dt
                grdPOV.DataBind()
            Else
                performPOVSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdPOV_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdPOV.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("POV")) Then
                performPOVSearch()
            End If

            dt = CType(Cache("POV"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdPOV.DataSource = dt
            grdPOV.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdPOV_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPOV.RowDataBound
        Try
            'attaches confirm script to button
            common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "dataSetName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

   
End Class