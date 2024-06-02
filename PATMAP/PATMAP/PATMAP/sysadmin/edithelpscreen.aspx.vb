Partial Public Class edithelpscreen
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not Page.IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.helpText)

                'get Help screen ID
                Dim editHelpScreenID As Integer
                editHelpScreenID = Session("editHelpScreenID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString

                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader

                'fill the sections drop down
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select sectionID, sectionName from sections"
                Dim dt As New DataTable
                da.Fill(dt)
                ddlSection.DataSource = dt
                ddlSection.DataValueField = "sectionID"
                ddlSection.DataTextField = "sectionName"
                ddlSection.DataBind()

                con.Open()

                If editHelpScreenID <> 0 Then
                    'don't allow user to change the section and screen name dropdown



                    'get help screen details from database
                    query.CommandText = "select sectionID, helpScreens.screenNameID, description, notes, helpText from helpScreens inner join screenNames on helpScreens.screenNameID = screenNames.screenNameID where helpScreens.screenNameID = " & editHelpScreenID
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill user detail into first section form fields
                    ddlSection.SelectedValue = dr.GetValue(0)
                    ddlScreen.SelectedValue = dr.GetValue(1)

                    If Not IsDBNull(dr.GetValue(2)) Then
                        txtDescription.Text = Server.HtmlDecode(dr.GetValue(2))
                    End If

                    If Not IsDBNull(dr.GetValue(3)) Then
                        txtNotes.Text = Server.HtmlDecode(dr.GetValue(3))
                    End If

                    If Not IsDBNull(dr.GetValue(4)) Then
                        ftbHelpText.Text = Server.HtmlDecode(dr.GetValue(4))
                    End If

                    'cleanup
                    dr.Close()

                End If

                'check if should show reset button
                query.CommandText = "select ScreenNameID from helpScreensReset where screenNameID = " & editHelpScreenID
                dr = query.ExecuteReader
                If dr.Read() Then
                    btnReset.Visible = True
                Else
                    btnReset.Visible = False
                End If
                dr.Close()

                loadScreenNameDropDown()

                If editHelpScreenID <> 0 Then
                    'don't allow user to change the section and screen name dropdown
                    ddlSection.Enabled = False
                    ddlScreen.Enabled = False
                Else
                    ddlSection.Enabled = True
                    ddlScreen.Enabled = True
                End If

                'clean up
                con.Close()

            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)

        End Try


    End Sub

    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            'make sure required fields are filled out
            If String.IsNullOrEmpty(ddlSection.SelectedValue) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP29")
                Exit Sub
            End If
            If String.IsNullOrEmpty(ddlScreen.SelectedValue) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP30")
                Exit Sub
            End If
            If ddlSection.SelectedValue = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP29")
                Exit Sub
            End If
            If ddlScreen.SelectedValue = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP30")
                Exit Sub
            End If

            'remove any single quotes from fields
            txtDescription.Text = Server.HtmlEncode(Trim(txtDescription.Text.Replace("'", "''")))
            txtNotes.Text = Server.HtmlEncode(Trim(txtNotes.Text.Replace("'", "''")))
            ftbHelpText.Text = Server.HtmlEncode(Trim(ftbHelpText.Text.Replace("'", "''")))


            'get Help screen ID
            Dim editHelpScreenID As Integer
            editHelpScreenID = Session("editHelpScreenID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            'save the help screen details
            If editHelpScreenID = 0 Then
                'insert help screen 
                query.CommandText = "insert into helpScreens (screenNameID, description, notes, helpText) values (" & ddlScreen.SelectedValue & ",'" & Trim(txtDescription.Text) & "','" & Trim(txtNotes.Text) & "','" & ftbHelpText.Text & "')" & vbCrLf
            Else
                'update help screen
                query.CommandText = "update helpScreens set description = '" & Trim(txtDescription.Text) & "', notes = '" & Trim(txtNotes.Text) & "', helpText = '" & ftbHelpText.Text & "' where ScreenNameID = " & editHelpScreenID & vbCrLf
            End If

            'check if this has been select to be a restore point
            If ckbReset.Checked = True Then
                query.CommandText += "delete from helpScreensReset where ScreenNameID = " & ddlScreen.SelectedValue & vbCrLf
                query.CommandText += "insert into helpScreensReset select * from helpScreens where ScreenNameID = " & ddlScreen.SelectedValue & vbCrLf
            End If

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

            'clean up
            Session.Remove("editHelpScreenID")
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        'return to home page
        Response.Redirect("viewhelpscreen.aspx")

    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("editHelpScreenID"), lblTitle)
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

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("editHelpScreenID")
        Response.Redirect("viewhelpscreen.aspx")
    End Sub

    Private Sub loadScreenNameDropDown()
        'get Help screen ID
        Dim editHelpScreenID As Integer
        editHelpScreenID = Session("editHelpScreenID")

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'get screen name for drop down
        Dim da As New SqlClient.SqlDataAdapter
        'check if a section has been selected
        If String.IsNullOrEmpty(ddlSection.SelectedValue) Or ddlSection.SelectedValue = 0 Then
            'no section selected
            da.SelectCommand = New SqlClient.SqlCommand("select 0 as screenNameID,'<Select>' as screenName", con)
        Else
            'section selected
            If editHelpScreenID = 0 Then
                da.SelectCommand = New SqlClient.SqlCommand("select screenNameID,screenName from screenNames where screenNameID not in (select screenNameID from helpScreens) and sectionID = " & ddlSection.SelectedValue, con)
            Else
                da.SelectCommand = New SqlClient.SqlCommand("select screenNameID,screenName from screenNames where screenNameID in (select screenNameID from helpScreens) and sectionID = " & ddlSection.SelectedValue, con)
            End If
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

    Protected Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReset.Click
        Try
            'get Help screen ID
            Dim editHelpScreenID As Integer
            editHelpScreenID = Session("editHelpScreenID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            Dim dr As SqlClient.SqlDataReader

            'get help screen details from database
            con.Open()
            query.CommandText = "select screenNameID, description, notes, helpText from helpScreensReset where ScreenNameID = " & editHelpScreenID
            dr = query.ExecuteReader
            dr.Read()

            'fill user detail into first section form fields
            'ddlSection.SelectedValue = editHelpScreenID
            'ddlScreen.SelectedValue = dr.GetValue(0)
            txtDescription.Text = Server.HtmlDecode(dr.GetValue(1))
            txtNotes.Text = Server.HtmlDecode(dr.GetValue(2))
            ftbHelpText.Text = Server.HtmlDecode(dr.GetValue(3))

            'cleanup
            dr.Close()
            con.Close()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnLoad_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnLoad.Click
        Try
            'Check if the user selected a file
            If Not fpUploadFile.HasFile Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP37")
                Exit Sub
            End If

            'Get the name and the type of the file
            Dim filePath As String = fpUploadFile.PostedFile.FileName
            Dim fileName As String = System.IO.Path.GetFileName(filePath)
            Dim fileType As String = System.IO.Path.GetExtension(filePath)

            'Save the file
            Dim serverLocation = PATMAP.Global_asax.imageFilePath & fileName

            If Not common.ValidateNoSpecialChar(fileName.Substring(0, (fileName.Length - fileType.Length)), "^!#$%&'*+-./\<>|:,;[]?{}""") Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP119")
                Exit Sub
            End If

            Dim counter As Integer = 0

            'If there's a duplicate filename, change the name
            While System.IO.File.Exists(serverLocation)
                counter += 1
                serverLocation = PATMAP.Global_asax.imageFilePath & fileName.Replace(fileType, "(" & counter & ")" & fileType)
            End While

            Try
                Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, Master.errorMsg)
                fpUploadFile.PostedFile.SaveAs(serverLocation)
                Impersonate.undoImpersonation()
            Catch
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP36")
                Exit Sub
            End Try
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

End Class