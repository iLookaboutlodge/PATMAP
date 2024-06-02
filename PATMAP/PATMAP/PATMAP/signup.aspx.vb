
Partial Class SignUp
    Inherits System.Web.UI.Page

    Protected Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
        Try
            txtFirstName.Text = ""
            txtLastName.Text = ""
            txtOrganization.Text = ""
            txtPosition.Text = ""
            txtEmail.Text = ""
            txtWP1.Text = ""
            txtWP2.Text = ""
            txtWP3.Text = ""
            txtWP4.Text = ""
            txtFax1.Text = ""
            txtFax2.Text = ""
            txtFax3.Text = ""
            txtAddress1.Text = ""
            txtAddress2.Text = ""
            txtMunicipality.Text = ""
            ddlProvince.SelectedIndex = 0
            txtPostalCode.Text = ""
            cklRequest.Items(0).Selected = False
            cklRequest.Items(1).Selected = False
            ddlSecurity.SelectedIndex = 0
            txtAnswer.Text = ""
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

	Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
		Try

			'Clears out the error message
			Master.errorMsg = ""

			If Not IsPostBack Then

				Using con As SqlClient.SqlConnection = New SqlClient.SqlConnection(PATMAP.Global_asax.connString)

					Using da As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter
						da.SelectCommand = New SqlClient.SqlCommand
						da.SelectCommand.Connection = con
						da.SelectCommand.CommandText = "select provinceID, province from provinces"
						Using dt As DataTable = New DataTable
							da.Fill(dt)
							ddlProvince.DataSource = dt
							ddlProvince.DataValueField = "provinceID"
							ddlProvince.DataTextField = "province"
							ddlProvince.DataBind()
						End Using
					End Using

					'fill the scurity question drop down
					Using da As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter
						da.SelectCommand = New SqlClient.SqlCommand
						da.SelectCommand.Connection = con
						da.SelectCommand.CommandText = "select securityQuestionID, securityQuestion from securityQuestions"
						Using dt As DataTable = New DataTable
							da.Fill(dt)
							ddlSecurity.DataSource = dt
							ddlSecurity.DataValueField = "securityQuestionID"
							ddlSecurity.DataTextField = "securityQuestion"
							ddlSecurity.DataBind()
						End Using
					End Using

					txtWP1.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtWP2.focus(); ")
					txtWP2.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtWP3.focus(); ")
					txtWP3.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 4 ) document.aspnetForm.ctl00$mainContent$txtWP4.focus(); ")

					txtFax1.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtFax2.focus(); ")
					txtFax2.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtFax3.focus(); ")

				End Using

			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try

	End Sub

	Protected Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
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
		If Trim(txtAnswer.Text) = "" Then
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP15")
			Exit Sub
		End If
		If Not Trim(txtPostalCode.Text) = "" And Not common.ValidatePostalCode(Trim(txtPostalCode.Text)) Then
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP71")
			Exit Sub
		End If
		Dim faxNumber As String = Trim(txtFax1.Text) + Trim(txtFax2.Text) + Trim(txtFax3.Text)
		If Not faxNumber = "" And Not common.ValidatePhoneFaxNumber(faxNumber, "") Then
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP77")
			Exit Sub
		End If

		Try
			Using con As SqlClient.SqlConnection = New SqlClient.SqlConnection(PATMAP.Global_asax.connString)
				If Not con.State = ConnectionState.Open Then
					con.Open()
				End If

				'convert boolean to integer values for insert into database
				Dim model As Integer
				Dim boundary As Integer
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

				'remove any single quotes from fields
				txtFirstName.Text = txtFirstName.Text.Replace("'", "''")
				txtLastName.Text = txtLastName.Text.Replace("'", "''")
				txtOrganization.Text = txtOrganization.Text.Replace("'", "''")
				txtPosition.Text = txtPosition.Text.Replace("'", "''")
				txtAddress1.Text = txtAddress1.Text.Replace("'", "''")
				txtAddress2.Text = txtAddress2.Text.Replace("'", "''")
				txtMunicipality.Text = txtMunicipality.Text.Replace("'", "''")
				txtAnswer.Text = txtAnswer.Text.Replace("'", "''")

				'do any error checking of the form fields

				'check if the email is unique
				Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
					query.Connection = con
					'setup the query to insert the new user            
					Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
					Dim defaultPaswd As String = en.Encrypting(PATMAP.common.GeneratePassword())
					query.CommandText = "insert into users (loginName, loginPassword, firstName, lastname, organization, position, email, workphone, fax, address1, address2,municipality, provinceID, postalCode, interestModel, interestBoundary, securityQuestionID, securityAnswer) values (@loginName, @loginPassword, @firstName, @lastname, @organization, @position, @email, @workphone, @fax, @address1, @address2, @municipality, @provinceID, @postalCode, @interestModel, @interestBoundary, @securityQuestionID, @securityAnswer)"
					query.Parameters.Add(New SqlClient.SqlParameter("@loginName", SqlDbType.VarChar, 50)).Value = Trim(txtFirstName.Text) & Trim(txtLastName.Text.Substring(0, 1))
					query.Parameters.Add(New SqlClient.SqlParameter("@loginPassword", SqlDbType.NVarChar, 50)).Value = defaultPaswd
					query.Parameters.Add(New SqlClient.SqlParameter("@firstName", SqlDbType.VarChar, 50)).Value = Trim(txtFirstName.Text)
					query.Parameters.Add(New SqlClient.SqlParameter("@lastname", SqlDbType.VarChar, 50)).Value = Trim(txtLastName.Text)
					query.Parameters.Add(New SqlClient.SqlParameter("@organization", SqlDbType.VarChar, 50)).Value = txtOrganization.Text
					query.Parameters.Add(New SqlClient.SqlParameter("@position", SqlDbType.VarChar, 50)).Value = Trim(txtPosition.Text)
					query.Parameters.Add(New SqlClient.SqlParameter("@email", SqlDbType.VarChar, 100)).Value = Trim(txtEmail.Text)
					query.Parameters.Add(New SqlClient.SqlParameter("@workphone", SqlDbType.VarChar, 20)).Value = Trim(txtWP1.Text) & Trim(txtWP2.Text) & Trim(txtWP3.Text) & Trim(txtWP4.Text)
					query.Parameters.Add(New SqlClient.SqlParameter("@fax", SqlDbType.VarChar, 20)).Value = Trim(txtFax1.Text) & Trim(txtFax2.Text) & Trim(txtFax3.Text)
					query.Parameters.Add(New SqlClient.SqlParameter("@address1", SqlDbType.VarChar, 50)).Value = Trim(txtAddress1.Text)
					query.Parameters.Add(New SqlClient.SqlParameter("@address2", SqlDbType.VarChar, 50)).Value = Trim(txtAddress2.Text)
					query.Parameters.Add(New SqlClient.SqlParameter("@municipality", SqlDbType.VarChar, 100)).Value = Trim(txtMunicipality.Text)
					query.Parameters.Add(New SqlClient.SqlParameter("@provinceID", SqlDbType.SmallInt)).Value = ddlProvince.SelectedValue
					query.Parameters.Add(New SqlClient.SqlParameter("@postalCode", SqlDbType.VarChar, 7)).Value = Trim(txtPostalCode.Text)
					query.Parameters.Add(New SqlClient.SqlParameter("@interestModel", SqlDbType.Bit)).Value = model
					query.Parameters.Add(New SqlClient.SqlParameter("@interestBoundary", SqlDbType.Bit)).Value = boundary
					query.Parameters.Add(New SqlClient.SqlParameter("@securityQuestionID", SqlDbType.VarChar, 150)).Value = ddlSecurity.SelectedValue
					query.Parameters.Add(New SqlClient.SqlParameter("@securityAnswer", SqlDbType.VarChar, 150)).Value = Trim(txtAnswer.Text)

					query.ExecuteNonQuery()
				End Using

			End Using

			pnlForm.Visible = False

			lblSuccess.Visible = True

			'send email with username and new password
			'Dim Mail As New System.Net.Mail.MailMessage
			'Dim SMTP As New System.Net.Mail.SmtpClient(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport)
			Dim Mail As New OpenSmtp.Mail.MailMessage()
			'Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport) '***Inky commented this line out in Apr-2010
			Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPuserName, PATMAP.Global_asax.SMTPpassword)	'***Inky's Addition: Apr-2010

			'build the email (new user)                
			Mail.GetBodyFromFile(Request.MapPath("/includes/Email.html"))
			Mail.AddImage(Request.MapPath("/includes/governmentLogoNew.jpg"), "patmap01")
			Mail.HtmlBody = Mail.Body.Replace("governmentLogoNew.jpg", "cid:patmap01")

			Dim mailMessage As String
			mailMessage = "Hello,<br><br>"
			mailMessage += "Your information has been successfully submitted.<br><br>"
			mailMessage += "A follow up email will be sent to your email address with your username and password after your request has been approved.<br><br>"
			mailMessage += "Your patience is truly appriciated.<br><br>"
			mailMessage += "Thank You."
			Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)

			Mail.Subject = "Your request has been submitted."
			Mail.To.Add(New OpenSmtp.Mail.EmailAddress(Trim(txtEmail.Text), txtFirstName.Text & " " & txtLastName.Text))
			Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailName)

			SMTP.SendMail(Mail)

			'build the email (approver)
			mailMessage = "Hello,<br><br>"
			mailMessage += "A new user request awaits your approval to access the PATMAP system.<br><br>"
			mailMessage += "First Name:- " & Trim(txtFirstName.Text) & "<br>"
			mailMessage += "Last Name:- " & Trim(txtLastName.Text) & "<br>"
			mailMessage += "Sign-Up Date:- " & DateTime.Now.ToString("M/d/yyyy h:mm:ss tt") & "<br><br>"
			mailMessage += "Thank You."
			Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)

			Mail.Subject = "A new user has been signed up - PATMAP."
			Mail.To.Clear()
			Mail.To.Add(New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.ApproverEmailAddress, PATMAP.Global_asax.ApproverEmailName))
			Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailName)

			SMTP.SendMail(Mail)

			'send to home page
			'Response.Redirect("index.aspx")
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try

	End Sub

	'Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
	'    Try

	'        'Clears out the error message
	'        Master.errorMsg = ""

	'        If Not IsPostBack Then

	'            'setup database connection
	'            Dim con As New SqlClient.SqlConnection
	'            con.ConnectionString = PATMAP.Global_asax.connString

	'            'fill the provinces drop down
	'            Dim da As New SqlClient.SqlDataAdapter
	'            da.SelectCommand = New SqlClient.SqlCommand
	'            da.SelectCommand.Connection = con
	'            da.SelectCommand.CommandText = "select provinceID, province from provinces"
	'            Dim dt As New DataTable
	'            da.Fill(dt)
	'            ddlProvince.DataSource = dt
	'            ddlProvince.DataValueField = "provinceID"
	'            ddlProvince.DataTextField = "province"
	'            ddlProvince.DataBind()

	'            'fill the scurity question drop down
	'            da = New SqlClient.SqlDataAdapter
	'            da.SelectCommand = New SqlClient.SqlCommand
	'            da.SelectCommand.Connection = con
	'            da.SelectCommand.CommandText = "select securityQuestionID, securityQuestion from securityQuestions"
	'            dt = New DataTable
	'            da.Fill(dt)
	'            ddlSecurity.DataSource = dt
	'            ddlSecurity.DataValueField = "securityQuestionID"
	'            ddlSecurity.DataTextField = "securityQuestion"
	'            ddlSecurity.DataBind()

	'            txtWP1.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtWP2.focus(); ")
	'            txtWP2.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtWP3.focus(); ")
	'            txtWP3.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 4 ) document.aspnetForm.ctl00$mainContent$txtWP4.focus(); ")

	'            txtFax1.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtFax2.focus(); ")
	'            txtFax2.Attributes.Add("onKeyUp", "javascript: if ( this.value.length == 3 ) document.aspnetForm.ctl00$mainContent$txtFax3.focus(); ")

	'        End If
	'    Catch
	'        'retrieves error message
	'        Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
	'    End Try

	'End Sub

	'Protected Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
	'    If Trim(txtFirstName.Text) = "" Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP9")
	'        Exit Sub
	'    End If
	'    If Not common.ValidateNoSpecialChar(Trim(txtFirstName.Text)) Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP78")
	'        Exit Sub
	'    End If
	'    If Trim(txtLastName.Text) = "" Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP10")
	'        Exit Sub
	'    End If
	'    If Not common.ValidateNoSpecialChar(Trim(txtLastName.Text)) Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP79")
	'        Exit Sub
	'    End If
	'    If Trim(txtOrganization.Text) = "" Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP11")
	'        Exit Sub
	'    End If
	'    If Trim(txtPosition.Text) = "" Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP12")
	'        Exit Sub
	'    End If
	'    If Trim(txtEmail.Text) = "" Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP13")
	'        Exit Sub
	'    End If
	'    If Not common.ValidateEmail(Trim(txtEmail.Text)) Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP70")
	'        Exit Sub
	'    End If
	'    If Trim(txtWP1.Text) = "" Or Trim(txtWP2.Text) = "" Or Trim(txtWP3.Text) = "" Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP14")
	'        Exit Sub
	'    End If
	'    If Not common.ValidatePhoneFaxNumber(Trim(txtWP1.Text) + Trim(txtWP2.Text) + Trim(txtWP3.Text), Trim(txtWP4.Text)) Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP75")
	'        Exit Sub
	'    End If
	'    If Trim(txtAnswer.Text) = "" Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP15")
	'        Exit Sub
	'    End If
	'    If Not Trim(txtPostalCode.Text) = "" And Not common.ValidatePostalCode(Trim(txtPostalCode.Text)) Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP71")
	'        Exit Sub
	'    End If
	'    Dim faxNumber As String = Trim(txtFax1.Text) + Trim(txtFax2.Text) + Trim(txtFax3.Text)
	'    If Not faxNumber = "" And Not common.ValidatePhoneFaxNumber(faxNumber, "") Then
	'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP77")
	'        Exit Sub
	'    End If

	'    Try
	'        'setup database connection
	'        Dim con As New SqlClient.SqlConnection
	'        con.ConnectionString = PATMAP.Global_asax.connString
	'        con.Open()

	'        'check if the email is unique
	'        Dim query As New SqlClient.SqlCommand
	'        query.Connection = con

	'        'convert boolean to integer values for insert into database
	'        Dim model As Integer
	'        Dim boundary As Integer
	'        If cklRequest.Items(0).Selected = True Then
	'            model = 1
	'        Else
	'            model = 0
	'        End If
	'        If cklRequest.Items(1).Selected = True Then
	'            boundary = 1
	'        Else
	'            boundary = 0
	'        End If

	'        'remove any single quotes from fields
	'        txtFirstName.Text = txtFirstName.Text.Replace("'", "''")
	'        txtLastName.Text = txtLastName.Text.Replace("'", "''")
	'        txtOrganization.Text = txtOrganization.Text.Replace("'", "''")
	'        txtPosition.Text = txtPosition.Text.Replace("'", "''")
	'        txtAddress1.Text = txtAddress1.Text.Replace("'", "''")
	'        txtAddress2.Text = txtAddress2.Text.Replace("'", "''")
	'        txtMunicipality.Text = txtMunicipality.Text.Replace("'", "''")
	'        txtAnswer.Text = txtAnswer.Text.Replace("'", "''")

	'        'do any error checking of the form fields


	'        'setup the query to insert the new user            
	'        query.Connection = con
	'        Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
	'        Dim defaultPaswd As String = en.Encrypting(PATMAP.common.GeneratePassword())
	'        query.CommandText = "insert into users (loginName, loginPassword, firstName, lastname, organization, position, email, workphone, fax, address1, address2,municipality, provinceID, postalCode, interestModel, interestBoundary, securityQuestionID, securityAnswer) values ('" & Trim(txtFirstName.Text) & Trim(txtLastName.Text.Substring(0, 1)) & "','" & defaultPaswd & "','" & Trim(txtFirstName.Text) & "','" & Trim(txtLastName.Text) & "','" & txtOrganization.Text & "','" & Trim(txtPosition.Text) & "','" & Trim(txtEmail.Text) & "','" & Trim(txtWP1.Text) & Trim(txtWP2.Text) & Trim(txtWP3.Text) & Trim(txtWP4.Text) & "','" & Trim(txtFax1.Text) & Trim(txtFax2.Text) & Trim(txtFax3.Text) & "','" & Trim(txtAddress1.Text) & "','" & Trim(txtAddress2.Text) & "','" & Trim(txtMunicipality.Text) & "'," & ddlProvince.SelectedValue & ",'" & Trim(txtPostalCode.Text) & "'," & model & "," & boundary & "," & ddlSecurity.SelectedValue & ",'" & Trim(txtAnswer.Text) & "')"
	'        query.ExecuteNonQuery()
	'        con.Close()

	'        pnlForm.Visible = False

	'        lblSuccess.Visible = True

	'        'send email with username and new password
	'        'Dim Mail As New System.Net.Mail.MailMessage
	'        'Dim SMTP As New System.Net.Mail.SmtpClient(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport)
	'        Dim Mail As New OpenSmtp.Mail.MailMessage()
	'        'Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport) '***Inky commented this line out in Apr-2010
	'        Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPuserName, PATMAP.Global_asax.SMTPpassword) '***Inky's Addition: Apr-2010

	'        'build the email (new user)                
	'        Mail.GetBodyFromFile(Request.MapPath("/includes/Email.html"))
	'        Mail.AddImage(Request.MapPath("/includes/governmentLogoNew.jpg"), "patmap01")
	'        Mail.HtmlBody = Mail.Body.Replace("governmentLogoNew.jpg", "cid:patmap01")

	'        Dim mailMessage As String
	'        mailMessage = "Hello,<br><br>"
	'        mailMessage += "Your information has been successfully submitted.<br><br>"
	'        mailMessage += "A follow up email will be sent to your email address with your username and password after your request has been approved.<br><br>"
	'        mailMessage += "Your patience is truly appriciated.<br><br>"
	'        mailMessage += "Thank You."
	'        Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)

	'        Mail.Subject = "Your request has been submitted."
	'        Mail.To.Add(New OpenSmtp.Mail.EmailAddress(Trim(txtEmail.Text), txtFirstName.Text & " " & txtLastName.Text))
	'        Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailName)

	'        SMTP.SendMail(Mail)

	'        'build the email (approver)
	'        mailMessage = "Hello,<br><br>"
	'        mailMessage += "A new user request awaits your approval to access the PATMAP system.<br><br>"
	'        mailMessage += "First Name:- " & Trim(txtFirstName.Text) & "<br>"
	'        mailMessage += "Last Name:- " & Trim(txtLastName.Text) & "<br>"
	'        mailMessage += "Sign-Up Date:- " & DateTime.Now.ToString("M/d/yyyy h:mm:ss tt") & "<br><br>"
	'        mailMessage += "Thank You."
	'        Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)

	'        Mail.Subject = "A new user has been signed up - PATMAP."
	'        Mail.To.Clear()
	'        Mail.To.Add(New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.ApproverEmailAddress, PATMAP.Global_asax.ApproverEmailName))
	'        Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailName)

	'        SMTP.SendMail(Mail)

	'        'send to home page
	'        'Response.Redirect("index.aspx")
	'    Catch
	'        'retrieves error message
	'        Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
	'    End Try

	'End Sub
End Class
