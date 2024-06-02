
Partial Class viewusers
    Inherits System.Web.UI.Page
    
  
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            'Clears out the error message
            Master.errorMsg = ""

            If Not Page.IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.userGroup)

                grdUsers.PageSize = PATMAP.Global_asax.pageSize
                grdRequests.PageSize = PATMAP.Global_asax.pageSize

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString

                'get user groups for drop down
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as GroupID, '<Please Select>' as groupName union select GroupID, groupName from groups order by groupID", con)
                Dim dt As New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlUserGroup.DataSource = dt
                ddlUserGroup.DataValueField = "GroupID"
                ddlUserGroup.DataTextField = "groupName"
                ddlUserGroup.DataBind()

                'get user levels for drop down
                dt.Clear()
                da.SelectCommand.CommandText = "select 0 as levelID, '<Please Select>' as levelName union select levelID, levelName from levels order by levelID"
                da.Fill(dt)
                ddlUserLevel.DataSource = dt
                ddlUserLevel.DataValueField = "levelID"
                ddlUserLevel.DataTextField = "levelName"
                ddlUserLevel.DataBind()

                'fill in the pending requests table
                fillPendingRequestsGrid()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try


    End Sub

    Private Sub fillPendingRequestsGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'fill in the pending requests table
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        da = New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select userID, firstName, lastName, signupDate from users where userstatusID = 3", con)
        dt = New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdRequests.DataSource = dt
        grdRequests.DataBind()

        If IsNothing(Cache("pendingUsers")) Then
            Cache.Add("pendingUsers", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("pendingUsers") = dt
        End If

        txtPendingRequest.Text = dt.Rows.Count

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editUserID", 0)
        Response.Redirect("edituser.aspx")
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            performUserSearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub performUserSearch()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'form field checks
        txtName.Text = txtName.Text.Replace("'", "''")
        txtUsername.Text = txtUsername.Text.Replace("'", "''")

        'setup the query to get user details
        Dim da As New SqlClient.SqlDataAdapter
        'da.SelectCommand = New SqlClient.SqlCommand("select users.userID, firstName, lastName, loginName, isnull(groupName,'') as groupName, levelName, loginDateTime from users left join groups on users.groupID = groups.groupID left join userLevels on users.userID = userLevels.userID left join levels on userLevels.levelID = levels.levelID left join (select userid,max(loginDateTime) as loginDateTime from userstatistics group by userid) userstatistics on users.userID = userstatistics.userID where userstatusID in (1,2) ", con)
        da.SelectCommand = New SqlClient.SqlCommand("select users.userID, firstName, lastName, loginName, isnull(groupName,'') as groupName, loginDateTime from users left join groups on users.groupID = groups.groupID left join userLevels on users.userID = userLevels.userID left join levels on userLevels.levelID = levels.levelID left join (select userid,max(loginDateTime) as loginDateTime from userstatistics group by userid) userstatistics on users.userID = userstatistics.userID where userstatusID in (1,2) ", con)

        If Trim(txtName.Text) <> "" Then
            da.SelectCommand.CommandText += " and (firstName + ' ' + lastName like '%" & Trim(txtName.Text) & "%') "
        End If
        If Trim(txtUsername.Text) <> "" Then
            da.SelectCommand.CommandText += " and charindex('" & Trim(txtUsername.Text) & "',loginName) > 0 "
        End If
        If ddlUserGroup.SelectedValue <> 0 Then
            da.SelectCommand.CommandText += " and users.groupID = " & ddlUserGroup.SelectedValue.ToString
        End If
        If ddlUserLevel.SelectedValue <> 0 Then
            da.SelectCommand.CommandText += " and levels.levelID = " & ddlUserLevel.SelectedValue.ToString
        End If

        da.SelectCommand.CommandText += " group by users.userID, firstName, lastName, loginName, isnull(groupName,''), loginDateTime"

        'get the data for the user list grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)

        'Dim dr As DataRow
        'Dim prevID As Integer = 0
        'Dim additionalUserLevels = ""
        'Dim rowIndex As Integer = 0
        'Dim firstElemRowIndex As Integer
        'Dim dtModified As New DataTable
        'Dim drModified As DataRow
        'For Each dr In dt.Rows
        '    If dr(0) = prevID Then
        '        additionalUserLevels += dr(5).ToString & vbCrLf                                
        '    Else                
        '        If additionalUserLevels <> "" Then
        '            dr(firstElemRowIndex) = dr(firstElemRowIndex) & additionalUserLevels
        '            additionalUserLevels = ""
        '            drModified = dt.Rows(firstElemRowIndex)
        '            dtModified.Rows.Add(drModified)
        '        End If
        '        firstElemRowIndex = rowIndex
        '        prevID = dr(0)
        '    End If
        '    rowIndex = rowIndex + 1
        'Next

        con.Close()
        grdUsers.DataSource = dt
        grdUsers.DataBind()

        If IsNothing(Cache("users")) Then
            Cache.Add("users", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("users") = dt
        End If

        txtTotalUser.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If

        'update the pending requests grid
        fillPendingRequestsGrid()
    End Sub

    Private Sub grdUsers_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUsers.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdUsers.PageIndex = e.NewPageIndex

            'fill the users grid
            If Not IsNothing(Cache("users")) Then
                dt = CType(Cache("users"), DataTable)
                grdUsers.DataSource = dt
                grdUsers.DataBind()
            Else
                performUserSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdUsers_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdUsers.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding userID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim userID As Integer = grdUsers.DataKeys(index).Values("userID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editUser"
                        Session.Add("editUserID", userID)
                    Case "deleteUser"

                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'delete user
                        query.CommandText = "update users set userStatusID = 5 where userID = " & userID.ToString
                        con.Open()
                        query.ExecuteNonQuery()
                        con.Close()

                        'update user search grid
                        performUserSearch()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editUser" Then
            Response.Redirect("edituser.aspx")
        End If

    End Sub

    Private Sub grdRequests_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdRequests.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdRequests.PageIndex = e.NewPageIndex

            'fill the pending request grid
            If Not IsNothing(Cache("pendingUsers")) Then
                dt = CType(Cache("pendingUsers"), DataTable)
                grdRequests.DataSource = dt
                grdRequests.DataBind()
            Else
                fillPendingRequestsGrid()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdRequests_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdRequests.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding userID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim userID As Integer = grdRequests.DataKeys(index).Values("userID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editUser"
                        Session.Add("editUserID", userID)
                    Case "deleteUser"

                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'delete user
                        query.CommandText = "update users set userStatusID = 4 where userID = " & userID.ToString
                        con.Open()
                        query.ExecuteNonQuery()
                        con.Close()

                        'send email to inform the user that there account request has been denied
                        'Dim Mail As New System.Net.Mail.MailMessage
                        'Dim SMTP As New System.Net.Mail.SmtpClient(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport)
                        Dim Mail As New OpenSmtp.Mail.MailMessage()
                        'Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport) 'Inky commented this line out in Apr-2010
                        Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPuserName, PATMAP.Global_asax.SMTPpassword) '***Inky's Addition: Apr-2010

                        'get the user name and login name for the entered email address
                        query.CommandText = "select firstname + ' ' + lastname, email from users where userID = '" & userID & "'"
                        Dim dr As SqlClient.SqlDataReader
                        con.Open()
                        dr = query.ExecuteReader
                        dr.Read()

                        'compose the message
                        Dim mailMessage As String
                        mailMessage = "Hello,<br><br>Your request for access to the PATMAP system has been denied.<br><br>Thank You"

                        'set email properties
                        Mail.GetBodyFromFile(Request.MapPath("/includes/Email.html"))
                        Mail.AddImage(Request.MapPath("/includes/governmentLogoNew.jpg"), "patmap01")
                        Mail.HtmlBody = Mail.Body.Replace("governmentLogoNew.jpg", "cid:patmap01")
                        Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)
                        Mail.Subject = "RE: Your request for access to the PATMAP system - Denied"
                        Mail.To.Add(New OpenSmtp.Mail.EmailAddress(dr.GetValue(1), dr.GetValue(0)))
                        Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailName)

                        'send the email
                        SMTP.SendMail(Mail)

                        'update user search grid
                        fillPendingRequestsGrid()

                        'cleanup
                        con.Close()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editUser" Then
            Response.Redirect("edituser.aspx", False)
        End If

    End Sub

    Private Sub grdUsers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdUsers.RowDataBound
        Try
            'attaches confirm script to button
            common.confirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "firstName") & " " & DataBinder.Eval(e.Row.DataItem, "lastName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdUsers_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdUsers.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the users grid if there's no data stored in cache
            If IsNothing(Cache("users")) Then
                performUserSearch()
            End If

            dt = CType(Cache("users"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdUsers.DataSource = dt
            grdUsers.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRequests.RowDataBound
        Try
            'attaches confirm script to button
            common.confirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "firstName") & " " & DataBinder.Eval(e.Row.DataItem, "firstName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdRequests_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdRequests.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the users grid if there's no data stored in cache
            If IsNothing(Cache("pendingUsers")) Then
                fillPendingRequestsGrid()
            End If

            dt = CType(Cache("pendingUsers"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdRequests.DataSource = dt
            grdRequests.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
   
End Class
