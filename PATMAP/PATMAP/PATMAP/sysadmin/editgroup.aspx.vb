Public Partial Class editgroup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            'check if its the first load or a post back
            If Not Page.IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.userGroup)

                'get groupID to edit
                Dim editGroupID As Integer
                editGroupID = Session("editgroupID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader
                con.Open()

                If editGroupID <> 0 Then
                    'get groups details from database
                    query.CommandText = "select groupName, isnull(description,'') as description, isnull(notes,'') as notes from groups where groupID = " & editGroupID
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill group detail into the form fields
                    txtGroupName.Text = dr.GetValue(0)
                    txtDescription.Text = dr.GetValue(1)
                    txtNotes.Text = dr.GetValue(2)

                    'cleanup
                    dr.Close()
                End If

                'fill the users list box
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select userID, firstName + ' ' + lastName as userName from users where userStatusID in (1,2,3) and groupID is null"
                Dim dt As New DataTable
                da.Fill(dt)
                lstUsers.DataSource = dt
                lstUsers.DataValueField = "userID"
                lstUsers.DataTextField = "userName"
                lstUsers.DataBind()
                Session.Add("usersTable", dt)

                'fill the members list box
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select userID, firstName + ' ' + lastName as userName from users where userStatusID in (1,2,3) and groupID = " & editGroupID
                dt = New DataTable
                da.Fill(dt)
                lstMembers.DataSource = dt
                lstMembers.DataValueField = "userID"
                lstMembers.DataTextField = "userName"
                lstMembers.DataBind()
                Session.Add("membersTable", dt)

                'see if changes have been made the members of group an update the list boxes
                updatelistboxes()

                'clean up
                con.Close()

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try


    End Sub

    Public Sub updatelistboxes()
        'see if changes have been made the members of group an update the list boxes
        lstUsers.DataSource = Session("usersTable")
        lstUsers.DataValueField = "userID"
        lstUsers.DataTextField = "userName"
        lstUsers.DataBind()

        lstMembers.DataSource = Session("membersTable")
        lstMembers.DataValueField = "userID"
        lstMembers.DataTextField = "userName"
        lstMembers.DataBind()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("editGroupID")
        Session.Remove("usersTable")
        Session.Remove("membersTable")
        Response.Redirect("viewgroups.aspx")
    End Sub


    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            'make sure required fields are filled out
            If Trim(txtGroupName.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP18")
                Exit Sub
            End If

            If Not common.ValidateNoSpecialChar(Trim(txtGroupName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP88")
                Exit Sub
            End If

            'remove any single quotes from fields
            txtGroupName.Text = Trim(txtGroupName.Text.Replace("'", "''"))
            txtDescription.Text = Trim(txtDescription.Text.Replace("'", "''"))
            txtNotes.Text = Trim(txtNotes.Text.Replace("'", "''"))

            'get groupID to edit
            Dim editGroupID As Integer
            editGroupID = Session("editgroupID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim dr As SqlClient.SqlDataReader
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            query.CommandText = "select groupName from groups where groupName = '" & Trim(txtGroupName.Text) & "' and groupID <> " & editGroupID
            dr = query.ExecuteReader()

            If dr.Read() Then
                dr.Close()
                con.Close()
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP110")
                Exit Sub
            End If

            dr.Close()

            'build add/update string
            If editGroupID = 0 Then
                query.CommandText = "insert into groups (groupName, description, notes) values ('" & Trim(txtGroupName.Text) & "','" & Trim(txtDescription.Text) & "','" & Trim(txtNotes.Text) & "') SELECT @@IDENTITY AS 'Identity'"
                dr = query.ExecuteReader
                dr.Read()
                editGroupID = dr.GetValue(0).ToString
                dr.Close()
            Else
                query.CommandText = "update groups set groupName = '" & Trim(txtGroupName.Text) & "', description = '" & Trim(txtDescription.Text) & "',notes = '" & Trim(txtNotes.Text) & "' where groupID = " & editGroupID
                query.ExecuteNonQuery()
            End If

            'build a string that will allow you to select all the users that should have this group id
            Dim temp As ListItem
            Dim members As String = "("
            For Each temp In lstMembers.Items
                members += temp.Value.ToString + ","
            Next
            members = members.Substring(0, members.Length - 1) + ")"

            'update the groupID values in the user table 
            If members.Length > 2 Then
                'the group has at least one member
                query.CommandText = "update users set groupID = " & editGroupID & " where userID in " & members
                query.ExecuteNonQuery()
                query.CommandText = "update users set groupID = null where groupID = " & editGroupID & " and userID not in " & members
                query.ExecuteNonQuery()
            Else
                'the group has no members
                query.CommandText = "update users set groupID = null where groupID = " & editGroupID
                query.ExecuteNonQuery()
            End If

            'clean up
            con.Close()
            Session.Remove("editGroupID")
            Session.Remove("usersTable")
            Session.Remove("membersTable")

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewgroups.aspx")
    End Sub

    Protected Sub btnAddMember_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddMember.Click
        Try
            'make sure a row was selected
            If String.IsNullOrEmpty(lstUsers.SelectedValue) = False Then
                'get index of selected row from user table
                Dim selectedUserIndex As Integer = lstUsers.SelectedIndex

                'move selected row from user table to member table
                Dim dtUsers As DataTable = Session("usersTable")
                Dim dtMembers As DataTable = Session("membersTable")
                dtMembers.ImportRow(dtUsers.Rows(selectedUserIndex))
                dtUsers.Rows.RemoveAt(selectedUserIndex)

                'update session data tables for the users and memeber tables
                Session("usersTable") = dtUsers
                Session("membersTable") = dtMembers

                'update the list boxes 
                updatelistboxes()
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnRemoveMember_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnRemoveMember.Click
        Try
            'make sure a row was selected
            If String.IsNullOrEmpty(lstMembers.SelectedValue) = False Then
                'get index of selected row from user table
                Dim selectedUserIndex As Integer = lstMembers.SelectedIndex

                'move selected row from user table to member table
                Dim dtUsers As DataTable = Session("usersTable")
                Dim dtMembers As DataTable = Session("membersTable")
                dtUsers.ImportRow(dtMembers.Rows(selectedUserIndex))
                dtMembers.Rows.RemoveAt(selectedUserIndex)

                'update session data tables for the users and memeber tables
                Session("usersTable") = dtUsers
                Session("membersTable") = dtMembers

                'update the list boxes 
                updatelistboxes()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("editgroupID"), lblTitle)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            common.UndoChange()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class