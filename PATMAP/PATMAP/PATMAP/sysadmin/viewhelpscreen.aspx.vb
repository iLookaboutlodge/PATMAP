Public Partial Class viewhelpscreen
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.helpText)

                grdScreens.PageSize = PATMAP.Global_asax.pageSize

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString

                'get sections for drop down
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as sectionID,'<Select>' as sectionName union select sectionID, sectionName from sections ", con)
                Dim dt As New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlSection.DataSource = dt
                ddlSection.DataValueField = "sectionID"
                ddlSection.DataTextField = "sectionName"
                ddlSection.DataBind()

                loadScreenNameDropDown()
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub loadScreenNameDropDown()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'get screen name for drop down
        Dim da As New SqlClient.SqlDataAdapter
        'check if a section has been selected
        If String.IsNullOrEmpty(ddlSection.SelectedValue) Or ddlSection.SelectedValue = 0 Then
            'no section selected

            '*** Inky's Update (Apr-2010) K-12 and Tax Credit references removed as per new PATMAP retooling
            da.SelectCommand = New SqlClient.SqlCommand("select 0 as screenNameID,'<Select>' as screenName union select screenNameID,screenName from screenNames where screenNameID not in (6, 15, 47, 56, 57, 77, 78, 90)", con) '***Inky (may-2010) removed Satellize screen (id:90)
            'da.SelectCommand = New SqlClient.SqlCommand("select 0 as screenNameID,'<Select>' as screenName union select screenNameID,screenName from screenNames ", con)
            '*** Inky: End

        Else
            'section selected

            '*** Inky's Update (Apr-2010) K-12 and Tax Credit references removed as per 2010 PATMAP retooling.  May-2010: Removed Sattelite reference as well.
            da.SelectCommand = New SqlClient.SqlCommand("select 0 as screenNameID,'<Select>' as screenName union select screenNameID,screenName from screenNames where screenNameID in (select helpScreens.screenNameID from helpScreens inner join screenNames on helpScreens.screenNameID = screenNames.screenNameID  where screenNames.screenNameID not in (6, 15, 47, 56, 57, 77, 78, 90) and sectionID = " & ddlSection.SelectedValue & ")", con)
            'da.SelectCommand = New SqlClient.SqlCommand("select 0 as screenNameID,'<Select>' as screenName union select screenNameID,screenName from screenNames where screenNameID in (select helpScreens.screenNameID from helpScreens inner join screenNames on helpScreens.screenNameID = screenNames.screenNameID  where sectionID = " & ddlSection.SelectedValue & ")", con)
            '*** Inky: End

        End If
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        ddlScreen.DataSource = dt
        ddlScreen.DataValueField = "screenNameID"
        ddlScreen.DataTextField = "screenName"
        ddlScreen.DataBind()
    End Sub

    Protected Sub ddlSection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSection.SelectedIndexChanged
        Try
            'load the screen name drop down based on the section value selected
            loadScreenNameDropDown()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub performHelpScreenSearch()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'setup the query to get entity details
        Dim da As New SqlClient.SqlDataAdapter

        '*** Inky's Update (May-2010): K-12, Tax Credit and Satellite references removed as per new PATMAP retooling
        da.SelectCommand = New SqlClient.SqlCommand("select helpScreens.ScreenNameID, sectionName, screenName, description from helpScreens inner join screenNames on screenNames.screenNameID = helpScreens.screenNameID inner join sections on sections.sectionID = screenNames.sectionID where screenNames.screenNameID not in (6, 15, 47, 56, 57, 77, 78, 90)", con)
        '''''da.SelectCommand = New SqlClient.SqlCommand("select helpScreens.ScreenNameID, sectionName, screenName, description from helpScreens inner join screenNames on screenNames.screenNameID = helpScreens.screenNameID inner join sections on sections.sectionID = screenNames.sectionID where 1 = 1", con)
        '*** Inky: End ***

        If Not (String.IsNullOrEmpty(ddlSection.SelectedValue) Or ddlSection.SelectedValue = 0) Then
            da.SelectCommand.CommandText += " and screenNames.sectionID = " & ddlSection.SelectedValue
        End If
        If Not (String.IsNullOrEmpty(ddlScreen.SelectedValue) Or ddlScreen.SelectedValue = 0) Then
            da.SelectCommand.CommandText += " and screenNames.screenNameID = '" & ddlScreen.SelectedValue & "'"
        End If

        'get the data for the entity grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdScreens.DataSource = dt
        grdScreens.DataBind()

        If IsNothing(Cache("helpScreens")) Then
            Cache.Add("helpScreens", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("helpScreens") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            'fill grid with search results
            performHelpScreenSearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editHelpScreenID", "0")
        Response.Redirect("edithelpscreen.aspx")
    End Sub

    Private Sub grdScreens_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdScreens.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdScreens.PageIndex = e.NewPageIndex

            'fill the help screen grid
            If Not IsNothing(Cache("helpScreens")) Then
                dt = CType(Cache("helpScreens"), DataTable)
                grdScreens.DataSource = dt
                grdScreens.DataBind()
            Else
                performHelpScreenSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdScreens_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdScreens.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding helpScreenID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim editHelpScreenID As String = grdScreens.DataKeys(index).Values("ScreenNameID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editHelpScreen"
                        Session.Add("editHelpScreenID", editHelpScreenID)
                    Case "deleteHelpScreen"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'delete help screen
                        query.CommandText = "delete from helpScreens where screenNameID = " & editHelpScreenID & vbCrLf & _
                                            "delete from helpScreensReset where screenNameID = " & editHelpScreenID
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

                        loadScreenNameDropDown()

                        'update help screen search grid
                        performHelpScreenSearch()

                        Master.errorMsg = ""
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editHelpScreen" Then
            Response.Redirect("edithelpscreen.aspx")
        End If
    End Sub

    Private Sub grdScreens_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdScreens.RowDataBound
        Try
            'attaches confirm script to button
            common.confirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "sectionName") & " - " & DataBinder.Eval(e.Row.DataItem, "screenName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdScreens_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdScreens.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("helpScreens")) Then
                performHelpScreenSearch()
            End If

            dt = CType(Cache("helpScreens"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdScreens.DataSource = dt
            grdScreens.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class