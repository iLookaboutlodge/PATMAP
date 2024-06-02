Public Partial Class viewhelpfield
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.helpText)

                grdFields.PageSize = PATMAP.Global_asax.pageSize

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

                'get screen name for drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as screenNameID,'<Select>' as screenName", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlScreen.DataSource = dt
                ddlScreen.DataValueField = "screenNameID"
                ddlScreen.DataTextField = "screenName"
                ddlScreen.DataBind()

                'get type for drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlTypeID,'<Select>' as controlType", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlType.DataSource = dt
                ddlType.DataValueField = "controlTypeID"
                ddlType.DataTextField = "controlType"
                ddlType.DataBind()

                'get sections for drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlNameID,'<Select>' as controlName", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlFieldName.DataSource = dt
                ddlFieldName.DataValueField = "controlNameID"
                ddlFieldName.DataTextField = "controlName"
                ddlFieldName.DataBind()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try


    End Sub

    Private Sub grdFields_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdFields.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdFields.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("helpFields")) Then
                dt = CType(Cache("helpFields"), DataTable)
                grdFields.DataSource = dt
                grdFields.DataBind()
            Else
                performHelpControlSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Protected Sub ddlSection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSection.SelectedIndexChanged
        Try
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString

            If ddlSection.SelectedValue <> 0 Then

                'get screen name for drop down
                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable
                da = New SqlClient.SqlDataAdapter

                '***Ink's update (May-2010): removed Tax Credit, K-12 and Satellite references from dropdown list as per PATMAP re-tooling project requirements
                ''''da.SelectCommand = New SqlClient.SqlCommand("select 0 as screenNameID,'<Select>' as screenName union select screennames.screenNameID,screennames.screenName from screennames inner join controls on controls.screenID = screennames.screenNameID inner join helpcontrols on controls.controlID = helpcontrols.controlID where screennames.sectionID = " & ddlSection.SelectedValue & " group by screennames.screenNameID,screennames.screenName order by screenName", con)
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as screenNameID,'<Select>' as screenName union select screennames.screenNameID,screennames.screenName from screennames inner join controls on controls.screenID = screennames.screenNameID inner join helpcontrols on controls.controlID = helpcontrols.controlID where screennames.sectionID = " & ddlSection.SelectedValue & " and screenNames.screenNameID not in (6, 15, 47, 56, 57, 77, 78, 90) group by screennames.screenNameID,screennames.screenName order by screenName", con)
                '***Inky: End ***

                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlScreen.DataSource = dt
                ddlScreen.DataValueField = "screenNameID"
                ddlScreen.DataTextField = "screenName"
                ddlScreen.DataBind()

                ''get type for drop down
                'da = New SqlClient.SqlDataAdapter
                'da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlTypeID,'<Select>' as controlType", con)
                'dt = New DataTable
                'con.Open()
                'da.Fill(dt)
                'con.Close()
                'ddlType.DataSource = dt
                'ddlType.DataValueField = "controlTypeID"
                'ddlType.DataTextField = "controlType"
                'ddlType.DataBind()

                ''get sections for drop down
                'da = New SqlClient.SqlDataAdapter
                'da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlNameID,'<Select>' as controlName", con)
                'dt = New DataTable
                'con.Open()
                'da.Fill(dt)
                'con.Close()
                'ddlFieldName.DataSource = dt
                'ddlFieldName.DataValueField = "controlNameID"
                'ddlFieldName.DataTextField = "controlName"
                'ddlFieldName.DataBind()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub ddlScreen_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlScreen.SelectedIndexChanged
        Try
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString

            If ddlScreen.SelectedValue <> 0 Then

                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable

                'get type for drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlTypeID,'<Select>' as controlType union select controltypes.controlTypeID, controltypes.controlType from helpcontrols inner join controls on controls.controlID = helpcontrols.controlID inner join controltypes on controls.controlTypeID = controltypes.controlTypeID inner join screennames on screennames.screenNameID = controls.screenID where screennames.sectionID = " & ddlSection.SelectedValue & " and controls.screenID = " & ddlScreen.SelectedValue & " group by controltypes.controlTypeID, controltypes.controlType order by controlType", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlType.DataSource = dt
                ddlType.DataValueField = "controlTypeID"
                ddlType.DataTextField = "controlType"
                ddlType.DataBind()

                ''get sections for drop down
                'da = New SqlClient.SqlDataAdapter
                'da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlNameID,'<Select>' as controlName", con)
                'dt = New DataTable
                'con.Open()
                'da.Fill(dt)
                'con.Close()
                'ddlFieldName.DataSource = dt
                'ddlFieldName.DataValueField = "controlNameID"
                'ddlFieldName.DataTextField = "controlName"
                'ddlFieldName.DataBind()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        Try
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString

            If ddlType.SelectedValue <> 0 Then

                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable

                'get sections for drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlNameID,'<Select>' as controlName union select helpcontrols.controlID, controlName from helpcontrols inner join controls on controls.controlID = helpcontrols.controlID inner join screennames on screennames.screenNameID = controls.screenID where screenNames.sectionID = " & ddlSection.SelectedValue & " and controls.screenID = " & ddlScreen.SelectedValue & " and controls.controlTypeID = " & ddlType.SelectedValue & " group by helpcontrols.controlID, controlName order by controlName", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlFieldName.DataSource = dt
                ddlFieldName.DataValueField = "controlNameID"
                ddlFieldName.DataTextField = "controlName"
                ddlFieldName.DataBind()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            'fill grid with search results
            performHelpControlSearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub performHelpControlSearch()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'setup the query to get entity details
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select helpcontrols.controlID, controlName, controlType, description from helpcontrols inner join controls on helpcontrols.controlID = controls.controlID inner join controltypes on controls.controlTypeID = controltypes.controlTypeID inner join screennames on screennames.screenNameID = controls.screenID where 1 = 1 ", con)

        If Not (String.IsNullOrEmpty(ddlSection.SelectedValue) Or ddlSection.SelectedValue = 0) Then
            da.SelectCommand.CommandText += " and screennames.sectionID = " & ddlSection.SelectedValue
        End If
        If Not (String.IsNullOrEmpty(ddlScreen.SelectedValue) Or ddlScreen.SelectedValue = 0) Then
            da.SelectCommand.CommandText += " and controls.screenID = '" & ddlScreen.SelectedValue & "'"
        End If
        If Not (String.IsNullOrEmpty(ddlType.SelectedValue) Or ddlType.SelectedValue = 0) Then
            da.SelectCommand.CommandText += " and controls.controlTypeID = '" & ddlType.SelectedValue & "'"
        End If
        If Not (String.IsNullOrEmpty(ddlFieldName.SelectedValue) Or ddlFieldName.SelectedValue = 0) Then
            da.SelectCommand.CommandText += " and helpcontrols.controlID = '" & ddlFieldName.SelectedValue & "'"
        End If

        'get the data for the entity grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdFields.DataSource = dt
        grdFields.DataBind()

        If IsNothing(Cache("helpFields")) Then
            Cache.Add("helpFields", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("helpFields") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editHelpControlID", "0")
        Response.Redirect("edithelpfield.aspx")
    End Sub

    Private Sub grdFields_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdFields.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding helpScreenID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim editHelpControlID As String = grdFields.DataKeys(index).Values("controlID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editHelpControl"
                        Session.Add("editHelpControlID", editHelpControlID)
                    Case "deleteHelpControl"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'delete help control
                        query.CommandText = "delete from helpcontrols where controlID = " & editHelpControlID & vbCrLf & _
                                            "delete from helpcontrolsReset where controlID = " & editHelpControlID
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

                        'get sections for drop down
                        Dim da As New SqlClient.SqlDataAdapter
                        da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlNameID,'<Select>' as controlName", con)
                        Dim dt As New DataTable
                        con.Open()
                        da.Fill(dt)
                        con.Close()
                        ddlFieldName.DataSource = dt
                        ddlFieldName.DataValueField = "controlNameID"
                        ddlFieldName.DataTextField = "controlName"
                        ddlFieldName.DataBind()

                        con.Close()

                        'update help control search grid
                        performHelpControlSearch()

                        Master.errorMsg = ""
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editHelpControl" Then
            Response.Redirect("edithelpfield.aspx")
        End If


    End Sub

    Private Sub grdFields_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdFields.RowDataBound
        Try
            'attaches confirm script to button
            common.confirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "controlName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdFields_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdFields.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("helpFields")) Then
                performHelpControlSearch()
            End If

            dt = CType(Cache("helpFields"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdFields.DataSource = dt
            grdFields.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class