Public Partial Class edituser
    Inherits System.Web.UI.Page
    Private blnSave As Boolean


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'Clears out the error message
            Master.errorMsg = ""

            'check if its the first load or a post back
            If Not Page.IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.userGroup)

                'get userID
                Dim editUserID As Integer
                editUserID = Session("editUserID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader

                'fill the provinces drop down
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select provinceID, province from provinces"
                Dim dt As New DataTable
                da.Fill(dt)
                ddlProvince.DataSource = dt
                ddlProvince.DataValueField = "provinceID"
                ddlProvince.DataTextField = "province"
                ddlProvince.DataBind()

                'fill the groups drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select groupID, groupName from groups"
                dt = New DataTable
                da.Fill(dt)
                ddlUserGroup.DataSource = dt
                ddlUserGroup.DataValueField = "groupID"
                ddlUserGroup.DataTextField = "groupName"
                ddlUserGroup.DataBind()

                'fill the scurity question drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select securityQuestionID, securityQuestion from securityQuestions"
                dt = New DataTable
                da.Fill(dt)
                ddlSecurity.DataSource = dt
                ddlSecurity.DataValueField = "securityQuestionID"
                ddlSecurity.DataTextField = "securityQuestion"
                ddlSecurity.DataBind()

                If editUserID <> 0 Then
                    'get users profile details from database
                    con.Open()
                    query.CommandText = "select firstName, lastName, organization, position, email, workPhone, fax, address1, address2, municipality,provinceID, postalCode, interestModel, interestBoundary, loginName, loginPassword, securityQuestionID, securityAnswer, isnull(groupID,''), userStatusID from users where userID = " & editUserID
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill user detail into first section form fields
                    txtFirstName.Text = dr.GetValue(0)
                    txtLastName.Text = dr.GetValue(1)
                    txtOrganization.Text = dr.GetValue(2)
                    txtPosition.Text = dr.GetValue(3)
                    txtEmail.Text = dr.GetValue(4)
                    Dim wp As String = dr.GetValue(5)
                    txtWP1.Text = wp.Substring(0, 3)
                    txtWP2.Text = wp.Substring(3, 3)
                    txtWP3.Text = wp.Substring(6, 4)
                    If wp.Length > 10 Then
                        txtWP4.Text = wp.Substring(10, (wp.Length - 10))
                    End If
                    Dim fax As String = dr.GetValue(6)
                    If fax.Length >= 10 Then
                        txtFax1.Text = fax.Substring(0, 3)
                        txtFax2.Text = fax.Substring(3, 3)
                        txtFax3.Text = fax.Substring(6, 4)
                    End If
                    If Not IsDBNull(dr.GetValue(7)) Then
                        txtAddress1.Text = dr.GetValue(7)
                    End If
                    If Not IsDBNull(dr.GetValue(8)) Then
                        txtAddress2.Text = dr.GetValue(8)
                    End If
                    If Not IsDBNull(dr.GetValue(9)) Then
                        txtMunicipality.Text = dr.GetValue(9)
                    End If
                    If Not IsDBNull(dr.GetValue(10)) Then
                        ddlProvince.SelectedValue = dr.GetValue(10)
                    End If
                    If Not IsDBNull(dr.GetValue(11)) Then
                        txtPostalCode.Text = dr.GetValue(11)
                    End If
                    If Not IsDBNull(dr.GetValue(12)) Then
                        cklRequest.Items(0).Selected = dr.GetValue(12)
                    End If
                    If Not IsDBNull(dr.GetValue(13)) Then
                        cklRequest.Items(1).Selected = dr.GetValue(13)
                    End If

                    'fill user detail into second section form fields
                    txtUsername.Text = dr.GetValue(14)

                    'if the status is pending, then leave the pswd field blank
                    'and do not set the status (active or not)
                    If dr.GetValue(19) = 3 Then
                        txtPassword.Text = ""
                        rblActive.Items(1).Enabled = False
                    Else
                        txtPassword.Attributes.Add("value", "********")
                        If dr.GetValue(19) = 1 Then
                            rblActive.Items(0).Selected = True
                        Else
                            rblActive.Items(1).Selected = True
                        End If
                    End If

                    ddlSecurity.SelectedValue = dr.GetValue(16)
                    txtAnswer.Text = dr.GetValue(17)
                    If dr.GetValue(18) <> 0 Then
                        ddlUserGroup.SelectedValue = dr.GetValue(18)
                    End If
                    'cleanup
                    dr.Close()
                End If

                'fill the levels drop down session variable
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select levelID, levelName from levels where levelID not in (select levelID from userlevels where userID = " & editUserID & ")"
                dt = New DataTable
                da.Fill(dt)
                Session.Add("ddlLevels", dt)

                'fill the user levels list box session variable
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select levels.levelID as levelID, levelName from levels inner join userlevels on levels.levelID = userlevels.levelID where userID = " & editUserID
                dt = New DataTable
                da.Fill(dt)
                Session.Add("lstLevels", dt)

                'fill the user levels list box
                fillUserLevel()

                'fill the section table
                fillSectionGrid()

                'clean up
                con.Close()

                txtWP1.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtWP2.focus(); ")
                txtWP2.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtWP3.focus(); ")
                txtWP3.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 4 ) document.aspnetForm.ctl00$mainContent$txtWP4.focus(); ")

                txtFax1.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtFax2.focus(); ")
                txtFax2.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtFax3.focus(); ")

            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub fillUserLevel()
        'get userID
        Dim editUserID As Integer
        editUserID = Session("editUserID")

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'fill the levels drop down
        ddlUserLevel.DataSource = Session("ddlLevels")
        ddlUserLevel.DataValueField = "levelID"
        ddlUserLevel.DataTextField = "levelName"
        ddlUserLevel.DataBind()

        'fill the user levels list box
        lstUserLevel.DataSource = Session("lstLevels")
        lstUserLevel.DataValueField = "levelID"
        lstUserLevel.DataTextField = "levelName"
        lstUserLevel.DataBind()

    End Sub

    Private Sub fillSectionGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'fill in the section table
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim query As String

        da = New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con

        'creates current user's user level
        Dim userLevel As String = ""
        Dim counter As Integer

        For counter = 0 To lstUserLevel.Items.Count - 1
            userLevel &= "," & lstUserLevel.Items(counter).Value
        Next

        userLevel = Mid(userLevel, 2)

        If userLevel = "" Then
            userLevel = 0
        End If

        query = "select sections.sectionID, sectionName, case when access is null or access = 0 then 'No' else 'Yes' end  as access" & vbCrLf & _
                "from sections" & vbCrLf & _
                "left outer join (" & vbCrLf & _
                "	select distinct(sections.sectionID), 1 as access" & vbCrLf & _
                "	from sections" & vbCrLf & _
                "	left join screenNames on screenNames.sectionID = sections.sectionID" & vbCrLf & _
                "	left join levelsPermission on levelsPermission.screenNameID = screenNames.screenNameID" & vbCrLf & _
                "	where sections.sectionID not in (1,2,7,8) and access = 1 and levelsPermission.levelID in (" & userLevel & ")" & vbCrLf & _
                ") results on results.sectionID = sections.sectionID" & vbCrLf & _
                "where sections.sectionID not in (1,2,7,8)" & vbCrLf & _
                "group by sections.sectionID, sectionName, results.access"

        da.SelectCommand.CommandText = query

        con.Open()
        da.Fill(dt)
        con.Close()

        grdSection.DataSource = dt
        grdSection.DataBind()

        If IsNothing(Cache("sections")) Then
            Cache.Add("sections", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("sections") = dt
        End If

    End Sub

    Protected Sub btnAddLevel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddLevel.Click
        Try
            'make sure a row was selected
            If String.IsNullOrEmpty(ddlUserLevel.SelectedValue) = False Then

                'get index of selected row from user table
                Dim selectedLevelIndex As Integer = ddlUserLevel.SelectedIndex

                'move selected row from level drop down to level list box
                Dim dt_ddlLevels As DataTable = Session("ddlLevels")
                Dim dt_lstLevels As DataTable = Session("lstLevels")
                dt_lstLevels.ImportRow(dt_ddlLevels.Rows(selectedLevelIndex))
                dt_ddlLevels.Rows.RemoveAt(selectedLevelIndex)

                'update session data tables 
                Session("ddlLevels") = dt_ddlLevels
                Session("lstLevels") = dt_lstLevels

                'reload the level list and drop down
                fillUserLevel()

                'fill the section table
                fillSectionGrid()
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnRemoveLevel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnRemoveLevel.Click
        Try
            'make sure a row was selected
            If String.IsNullOrEmpty(lstUserLevel.SelectedValue) = False Then
                'get index of selected row from user table
                Dim selectedLevelIndex As Integer = lstUserLevel.SelectedIndex

                'move selected row from user table to member table
                Dim dt_ddlLevels As DataTable = Session("ddlLevels")
                Dim dt_lstLevels As DataTable = Session("lstLevels")
                dt_ddlLevels.ImportRow(dt_lstLevels.Rows(selectedLevelIndex))
                dt_lstLevels.Rows.RemoveAt(selectedLevelIndex)

                'update session data tables 
                Session("ddlLevels") = dt_ddlLevels
                Session("lstLevels") = dt_lstLevels

                'reload the level list and drop down
                fillUserLevel()

                'fill the section table
                fillSectionGrid()
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("editUserID")
        Session.Remove("ddlLevels")
        Session.Remove("lstLevels")
        Response.Redirect("viewusers.aspx")
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGenerate.Click
        Try
            txtPassword.Attributes.Add("value", common.GeneratePassword)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            'return to home page
            'If blnSave Then
            'make sure required fields are filled out
            If Trim(txtFirstName.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP9")
                Exit Sub
            End If

            If Not common.ValidateNoSpecialChar(Trim(txtFirstName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP78")
                Exit Sub
            End If

            If Trim(txtLastName.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP10")
                Exit Sub
            End If

            If Not common.ValidateNoSpecialChar(Trim(txtLastName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP79")
                Exit Sub
            End If

            If Trim(txtOrganization.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP11")
                Exit Sub
            End If

            If Trim(txtPosition.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP12")
                Exit Sub
            End If

            If Trim(txtEmail.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP13")
                Exit Sub
            End If

            If Not common.ValidateEmail(Trim(txtEmail.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP70")
                Exit Sub
            End If

            If Trim(txtWP1.Text) = "" Or Trim(txtWP2.Text) = "" Or Trim(txtWP3.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP14")
                Exit Sub
            End If

            If Not common.ValidatePhoneFaxNumber(Trim(txtWP1.Text) + Trim(txtWP2.Text) + Trim(txtWP3.Text), Trim(txtWP4.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP75")
                Exit Sub
            End If

            Dim faxNumber As String = Trim(txtFax1.Text) + Trim(txtFax2.Text) + Trim(txtFax3.Text)
            If Not faxNumber = "" And Not common.ValidatePhoneFaxNumber(faxNumber, "") Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP77")
                Exit Sub
            End If

            If Trim(txtUsername.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP68")
                Exit Sub
            End If

            If Not common.ValidateNoSpecialChar(Trim(txtUsername.Text), "~`!@#$%^&*+={}|[]\:"";<>?,./*") Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP91")
                Exit Sub
            End If

            If Trim(txtPassword.Text) = "" Then
                txtPassword.Attributes.Remove("value")
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP16")
                Exit Sub
            End If

            If Trim(txtPassword.Text) <> "********" Then
                If Not common.ValidatePassword(Trim(txtPassword.Text)) Or txtPassword.Text.Length < 8 Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP120")
                    Exit Sub
                End If

                If Not common.ValidateNoSpecialChar(Trim(txtPassword.Text), "~`_-+={}[]:"";'<>?,./") Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP120")
                    Exit Sub
                End If
            End If

            If Trim(txtAnswer.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP15")
                Exit Sub
            End If

            If lstUserLevel.Items.Count = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP17")
                Exit Sub
            End If

            If Not Trim(txtPostalCode.Text) = "" And Not common.ValidatePostalCode(Trim(txtPostalCode.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP71")
                Exit Sub
            End If

            'remove any single quotes from fields
            txtFirstName.Text = Trim(txtFirstName.Text.Replace("'", "''"))
            txtLastName.Text = Trim(txtLastName.Text.Replace("'", "''"))
            txtOrganization.Text = Trim(txtOrganization.Text.Replace("'", "''"))
            txtPosition.Text = Trim(txtPosition.Text.Replace("'", "''"))
            txtEmail.Text = Trim(txtEmail.Text.Replace("'", "''"))
            txtAddress1.Text = Trim(txtAddress1.Text.Replace("'", "''"))
            txtAddress2.Text = Trim(txtAddress2.Text.Replace("'", "''"))
            txtMunicipality.Text = Trim(txtMunicipality.Text.Replace("'", "''"))
            txtPostalCode.Text = Trim(txtPostalCode.Text.Replace("'", "''"))
            txtPassword.Text = Trim(txtPassword.Text.Replace("'", "''"))
            txtAnswer.Text = Trim(txtAnswer.Text.Replace("'", "''"))



            'get userID
            Dim editUserID As Integer
            editUserID = Session("editUserID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()
            Dim dr As SqlClient.SqlDataReader
            Dim query As New SqlClient.SqlCommand
            query.Connection = con

            'convert boolean to integer values for insert into database
            Dim model As Integer
            Dim boundary As Integer
            Dim userStatus As Integer
            If cklRequest.Items(0).Selected = True Then
                model = 1
            Else
                model = 0
            End If
            If cklRequest.Items(1).Selected = True Then
                boundary = 1
            Else
                boundary = 0
            End If
            If rblActive.Items(0).Selected = True Then
                userStatus = 1
            ElseIf rblActive.Items(1).Selected Then
                userStatus = 2
            Else
                userStatus = 3
            End If

            If editUserID = 0 Then

                'check if the username is unique
                query.CommandText = "select userID from users where userStatusID in (1,2,3) and loginName='" & txtUsername.Text & "'"
                dr = query.ExecuteReader
                If dr.Read() Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP67")
                    Exit Sub
                End If
                dr.Close()

                'insert new user into user table
                Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
                Dim enPswd = en.Encrypting(Trim(txtPassword.Text))
                query.CommandText = "insert into users (loginName, loginPassword, firstName, lastname, organization, position, email, workphone, fax, address1, address2,municipality, provinceID, postalCode, interestModel, interestBoundary, securityQuestionID, securityAnswer, groupID, userStatusID) values ('" & txtUsername.Text & "','" & enPswd & "','" & txtFirstName.Text & "','" & txtLastName.Text & "','" & txtOrganization.Text & "','" & txtPosition.Text & "','" & txtEmail.Text & "','" & txtWP1.Text & txtWP2.Text & txtWP3.Text & txtWP4.Text & "','" & txtFax1.Text & txtFax2.Text & txtFax3.Text & "','" & txtAddress1.Text & "','" & txtAddress2.Text & "','" & txtMunicipality.Text & "'," & ddlProvince.SelectedValue & ",'" & txtPostalCode.Text & "'," & model & "," & boundary & "," & ddlSecurity.SelectedValue & ",'" & txtAnswer.Text & "'," & ddlUserGroup.SelectedValue & "," & userStatus & ") SELECT @@IDENTITY AS 'Identity'"
                dr = query.ExecuteReader
                dr.Read()
                editUserID = dr.GetValue(0).ToString
                dr.Close()

                'clear user levels table for selected user
                query.CommandText = "delete from userLevels where userID = " & editUserID
                query.ExecuteNonQuery()

                'update user levels table for selected user
                Dim temp As ListItem
                For Each temp In lstUserLevel.Items
                    query.CommandText = "insert into userLevels select " & editUserID & "," & temp.Value
                    query.ExecuteNonQuery()
                Next
                con.Close()

                'send email to user with user name and password
                Dim Mail As New OpenSmtp.Mail.MailMessage()
                'Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport) 'Inky commented this line out in Apr-2010
                Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPuserName, PATMAP.Global_asax.SMTPpassword) '***Inky's Addition: Apr-2010

                'compose the message
                Dim mailMessage As String
                mailMessage = "Hello,<br><br>Your request for access to the PATMAP system has been approved.<br><br>"
                mailMessage += "Here is your username and password:<br><br>"
                mailMessage += "Username: " & Trim(txtUsername.Text) & "<br>Password: " & Trim(txtPassword.Text) & "<br><br>"
                mailMessage += "Thank You"

                'setting the email properties
                Mail.GetBodyFromFile(Request.MapPath("/includes/Email.html"))
                Mail.AddImage(Request.MapPath("/includes/governmentLogoNew.jpg"), "patmap01")
                Mail.HtmlBody = Mail.Body.Replace("governmentLogoNew.jpg", "cid:patmap01")
                Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)
                Mail.Subject = "RE: Your request for access to the PATMAP system - Approved"
                Mail.To.Add(New OpenSmtp.Mail.EmailAddress(Trim(txtEmail.Text), Trim(txtFirstName.Text) & " " & Trim(txtLastName.Text)))
                Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.ApproverEmailAddress, PATMAP.Global_asax.ApproverEmailName)

                'send email
                SMTP.SendMail(Mail)

            Else
                'check if the username is unique
                query.CommandText = "select userID from users where userStatusID in (1,2,3) and loginName='" & txtUsername.Text & "' and userID <> " & editUserID
                dr = query.ExecuteReader
                If dr.Read() Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP67")
                    Exit Sub
                End If
                dr.Close()

                'get old status
                Dim oldStatus As Integer
                query.CommandText = "select userStatusID from users where userID=" & editUserID
                dr = query.ExecuteReader
                dr.Read()
                oldStatus = CType(dr.GetValue(0).ToString(), Integer)
                dr.Close()

                'update user in user table
                If Trim(txtPassword.Text) = "********" Then
                    query.CommandText = "update users set loginName = '" & txtUsername.Text & "', firstName = '" & txtFirstName.Text & "', lastName = '" & txtLastName.Text & "', organization = '" & txtOrganization.Text & "', position = '" & txtPosition.Text & "', email = '" & txtEmail.Text & "', workphone = '" & txtWP1.Text & txtWP2.Text & txtWP3.Text & txtWP4.Text & "', fax = '" & txtFax1.Text & txtFax2.Text & txtFax3.Text & "', address1 = '" & txtAddress1.Text & "', address2 = '" & txtAddress2.Text & "', municipality = '" & txtMunicipality.Text & "', provinceID = " & ddlProvince.SelectedValue & ", postalCode = '" & txtPostalCode.Text & "', interestModel = " & model & ", interestBoundary = " & boundary & ", SecurityQuestionID = " & ddlSecurity.SelectedValue & ", securityAnswer = '" & txtAnswer.Text & "', groupID = " & ddlUserGroup.SelectedValue & ", userStatusID = " & userStatus & " where userID = " & editUserID
                Else
                    Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
                    Dim enPswd = en.Encrypting(Trim(txtPassword.Text))
                    query.CommandText = "update users set loginName = '" & txtUsername.Text & "', firstName = '" & Trim(txtFirstName.Text) & "', lastName = '" & Trim(txtLastName.Text) & "', organization = '" & Trim(txtOrganization.Text) & "', position = '" & Trim(txtPosition.Text) & "', email = '" & Trim(txtEmail.Text) & "', workphone = '" & Trim(txtWP1.Text) & Trim(txtWP2.Text) & Trim(txtWP3.Text) & Trim(txtWP4.Text) & "', fax = '" & Trim(txtFax1.Text) & Trim(txtFax2.Text) & Trim(txtFax3.Text) & "', address1 = '" & Trim(txtAddress1.Text) & "', address2 = '" & Trim(txtAddress2.Text) & "', municipality = '" & Trim(txtMunicipality.Text) & "', provinceID = " & ddlProvince.SelectedValue & ", postalCode = '" & Trim(txtPostalCode.Text) & "', interestModel = " & model & ", interestBoundary = " & boundary & ", loginPassword = '" & enPswd & "', SecurityQuestionID = " & ddlSecurity.SelectedValue & ", securityAnswer = '" & Trim(txtAnswer.Text) & "', groupID = " & ddlUserGroup.SelectedValue & ", userStatusID = " & userStatus & " where userID = " & editUserID
                End If

                query.ExecuteNonQuery()

                'clear user levels table for selected user
                query.CommandText = "delete from userLevels where userID = " & editUserID
                query.ExecuteNonQuery()

                'update user levels table for selected user
                Dim temp As ListItem
                For Each temp In lstUserLevel.Items
                    query.CommandText = "insert into userLevels select " & editUserID & "," & temp.Value
                    query.ExecuteNonQuery()
                Next
                con.Close()

                'send email to user with user name and password
                Dim Mail As New OpenSmtp.Mail.MailMessage()
                'Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport) 'Inky Commented this line out in Apr-2010
                Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPuserName, PATMAP.Global_asax.SMTPpassword) '***Inky's Addition: Apr-2010

                'compose the message
                Dim mailMessage As String

                'if the user was in a pending status
                If oldStatus = 3 Then

                    If userStatus = 1 Or userStatus = 2 Then
                        mailMessage = "Hello,<br><br>"

                        Select Case userStatus
                            Case 1
                                'if the user is approved
                                mailMessage += "Your request for access to the PATMAP system has been approved.<br><br>"
                                mailMessage += "Here is your username and password:<br><br>"
                                mailMessage += "Username: " & txtUsername.Text & "<br>Password: " & txtPassword.Text
                                Mail.Subject = "RE: Your request for access to the PATMAP system - Approved"

                            Case 2
                                'if the user is rejected
                                mailMessage += "Your request for access to the PATMAP system has been rejected."
                                Mail.Subject = "RE: Your request for access to the PATMAP system - Rejected"
                        End Select

                        mailMessage += "<br><br>Thank You"

                        'setting the email properties
                        Mail.GetBodyFromFile(Request.MapPath("/includes/Email.html"))
                        Mail.AddImage(Request.MapPath("/includes/governmentLogoNew.jpg"), "patmap01")
                        Mail.HtmlBody = Mail.Body.Replace("governmentLogoNew.jpg", "cid:patmap01")
                        Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)
                        Mail.To.Add(New OpenSmtp.Mail.EmailAddress(Trim(txtEmail.Text), Trim(txtFirstName.Text) & " " & Trim(txtLastName.Text)))
                        Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.ApproverEmailAddress, PATMAP.Global_asax.ApproverEmailName)

                        'send email
                        SMTP.SendMail(Mail)
                    End If

                Else
                    If oldStatus <> userStatus Or txtPassword.Text <> "********" Then

                        mailMessage = "Hello,<br><br>"

                        If oldStatus <> userStatus Then
                            Select Case userStatus
                                Case 1
                                    If Trim(txtPassword.Text) <> "********" Then
                                        mailMessage += "Your PATMAP account has been re-activated.<br><br>"
                                        mailMessage += "Here is your username and password:<br><br>"
                                        mailMessage += "Username: " & Trim(txtUsername.Text) & "<br>Password: " & Trim(txtPassword.Text) & "<br><br>"
                                        Mail.Subject = "RE: Your PATMAP account has been re-activated"
                                    Else
                                        mailMessage += "Your PATMAP account has been re-activated."
                                        Mail.Subject = "RE: Your PATMAP account has been re-activated"
                                    End If
                                Case 2
                                    mailMessage += "Your PATMAP account has been disabled."
                                    Mail.Subject = "RE: Your PATMAP account has been disabled"
                            End Select
                        Else
                            If Trim(txtPassword.Text) <> "********" Then
                                mailMessage += "Your PATMAP password has been changed.<br><br>"
                                mailMessage += "Username: " & Trim(txtUsername.Text) & "<br>Password: " & Trim(txtPassword.Text)
                                Mail.Subject = "RE: Your PATMAP account has been updated"
                            End If
                        End If

                        mailMessage += "<br><br>Thank You"

                        'setting the email properties
                        Mail.GetBodyFromFile(Request.MapPath("/includes/Email.html"))
                        Mail.AddImage(Request.MapPath("/includes/governmentLogoNew.jpg"), "patmap01")
                        Mail.HtmlBody = Mail.Body.Replace("governmentLogoNew.jpg", "cid:patmap01")
                        Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)
                        Mail.To.Add(New OpenSmtp.Mail.EmailAddress(Trim(txtEmail.Text), Trim(txtFirstName.Text) & " " & Trim(txtLastName.Text)))
                        Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.ApproverEmailAddress, PATMAP.Global_asax.ApproverEmailName)

                        'send email
                        SMTP.SendMail(Mail)

                    End If
                End If
            End If

            'clean up
            Session.Remove("editUserID")
            Session.Remove("ddlLevels")
            Session.Remove("lstLevels")

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewusers.aspx")

    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("editUserID"), lblTitle)
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

    Public Sub updateUsername(ByVal sender As Object, ByVal e As System.EventArgs)
        'get userID
        Dim editUserID As Integer
        editUserID = Session("editUserID")

        If editUserID = 0 And txtFirstName.Text <> "" And txtLastName.Text <> "" And Trim(txtUsername.Text) = "" Then

            If Not common.ValidateNoSpecialChar(Trim(txtFirstName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP78")
                Exit Sub
            End If

            If Not common.ValidateNoSpecialChar(Trim(txtLastName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP79")
                Exit Sub
            End If

            txtUsername.Text = LCase(txtFirstName.Text & Left(txtLastName.Text, 1))
        End If
    End Sub

    Public Sub passwordChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        txtPassword.Attributes.Add("value", txtPassword.Text)
    End Sub
End Class