
Partial Class userprofile
    Inherits System.Web.UI.Page

	'Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
	'    Try
	'        If Trim(txtFirstName.Text) = "" Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP9")
	'            Exit Sub
	'        End If

	'        If Not common.ValidateNoSpecialChar(Trim(txtFirstName.Text)) Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP78")
	'            Exit Sub
	'        End If

	'        If Trim(txtLastName.Text) = "" Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP10")
	'            Exit Sub
	'        End If

	'        If Not common.ValidateNoSpecialChar(Trim(txtLastName.Text)) Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP79")
	'            Exit Sub
	'        End If

	'        If Trim(txtOrganization.Text) = "" Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP11")
	'            Exit Sub
	'        End If

	'        If Trim(txtPosition.Text) = "" Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP12")
	'            Exit Sub
	'        End If

	'        If Trim(txtEmail.Text) = "" Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP13")
	'            Exit Sub
	'        End If

	'        If Not common.ValidateEmail(Trim(txtEmail.Text)) Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP70")
	'            Exit Sub
	'        End If

	'        If Trim(txtWP1.Text) = "" Or Trim(txtWP2.Text) = "" Or Trim(txtWP3.Text) = "" Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP14")
	'            Exit Sub
	'        End If

	'        If Not common.ValidatePhoneFaxNumber(Trim(txtWP1.Text) + Trim(txtWP2.Text) + Trim(txtWP3.Text), Trim(txtWP4.Text)) Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP75")
	'            Exit Sub
	'        End If

	'        Dim faxNumber As String = Trim(txtFax1.Text) + Trim(txtFax2.Text) + Trim(txtFax3.Text)
	'        If Not faxNumber = "" And Not common.ValidatePhoneFaxNumber(faxNumber, "") Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP77")
	'            Exit Sub
	'        End If

	'        If Trim(txtPassword.Text) = "" Then
	'            txtPassword.Attributes.Remove("value")
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP16")
	'            Exit Sub
	'        End If

	'        If Trim(txtPassword.Text) <> "********" Then

	'            If Not common.ValidatePassword(Trim(txtPassword.Text)) Or txtPassword.Text.Length < 8 Then
	'                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP120")
	'                Exit Sub
	'            End If

	'            If Not common.ValidateNoSpecialChar(Trim(txtPassword.Text), "~`_-+={}[]:"";'<>?,./") Then
	'                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP120")
	'                Exit Sub
	'            End If

	'        End If

	'        If Trim(txtAnswer.Text) = "" Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP15")
	'            Exit Sub
	'        End If

	'        If Not Trim(txtPostalCode.Text) = "" And Not common.ValidatePostalCode(Trim(txtPostalCode.Text)) Then
	'            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP71")
	'            Exit Sub
	'        End If


	'        'remove any single quotes from fields
	'        txtFirstName.Text = txtFirstName.Text.Replace("'", "''")
	'        txtLastName.Text = txtLastName.Text.Replace("'", "''")
	'        txtOrganization.Text = txtOrganization.Text.Replace("'", "''")
	'        txtPosition.Text = txtPosition.Text.Replace("'", "''")
	'        txtEmail.Text = txtEmail.Text.Replace("'", "''")
	'        txtAddress1.Text = txtAddress1.Text.Replace("'", "''")
	'        txtAddress2.Text = txtAddress2.Text.Replace("'", "''")
	'        txtMunicipality.Text = txtMunicipality.Text.Replace("'", "''")
	'        txtPostalCode.Text = txtPostalCode.Text.Replace("'", "''")
	'        txtPassword.Text = txtPassword.Text.Replace("'", "''")
	'        txtAnswer.Text = txtAnswer.Text.Replace("'", "''")

	'        'do any error checking of the form fields


	'        'get userID
	'        Dim userID As Integer
	'        userID = Session("userID")

	'        'setup database connection
	'        Dim con As New SqlClient.SqlConnection
	'        con.ConnectionString = PATMAP.Global_asax.connString
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

	'        'build update query
	'        If Trim(txtPassword.Text) = "********" Then
	'            query.CommandText = "update users set firstName = '" & Trim(txtFirstName.Text) & "', lastName = '" & Trim(txtLastName.Text) & "', organization = '" & Trim(txtOrganization.Text) & "', position = '" & Trim(txtPosition.Text) & "', email = '" & Trim(txtEmail.Text) & "', workphone = '" & Trim(txtWP1.Text) & Trim(txtWP2.Text) & Trim(txtWP3.Text) & Trim(txtWP4.Text) & "', fax = '" & Trim(txtFax1.Text) & Trim(txtFax2.Text) & Trim(txtFax3.Text) & "', address1 = '" & Trim(txtAddress1.Text) & "', address2 = '" & Trim(txtAddress2.Text) & "', municipality = '" & Trim(txtMunicipality.Text) & "', provinceID = " & ddlProvince.SelectedValue & ", postalCode = '" & Trim(txtPostalCode.Text) & "', interestModel = " & model & ", interestBoundary = " & boundary & ", SecurityQuestionID = " & ddlSecurity.SelectedValue & ", securityAnswer = '" & Trim(txtAnswer.Text) & "' where userID = " & userID
	'        Else
	'            Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
	'            Dim enPswd = en.Encrypting(Trim(txtPassword.Text))
	'            query.CommandText = "update users set firstName = '" & Trim(txtFirstName.Text) & "', lastName = '" & Trim(txtLastName.Text) & "', organization = '" & Trim(txtOrganization.Text) & "', position = '" & Trim(txtPosition.Text) & "', email = '" & Trim(txtEmail.Text) & "', workphone = '" & Trim(txtWP1.Text) & Trim(txtWP2.Text) & Trim(txtWP3.Text) & Trim(txtWP4.Text) & "', fax = '" & Trim(txtFax1.Text) & Trim(txtFax2.Text) & Trim(txtFax3.Text) & "', address1 = '" & Trim(txtAddress1.Text) & "', address2 = '" & Trim(txtAddress2.Text) & "', municipality = '" & Trim(txtMunicipality.Text) & "', provinceID = " & ddlProvince.SelectedValue & ", postalCode = '" & Trim(txtPostalCode.Text) & "', interestModel = " & model & ", interestBoundary = " & boundary & ", loginPassword = '" & enPswd & "', SecurityQuestionID = " & ddlSecurity.SelectedValue & ", securityAnswer = '" & Trim(txtAnswer.Text) & "' where userID = " & userID
	'        End If
	'        con.Open()
	'        query.ExecuteNonQuery()
	'        con.Close()

	'    Catch
	'        'retrieves error message
	'        Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
	'        Exit Sub
	'    End Try

	'    'return to home page
	'    Response.Redirect("home.aspx")

	'End Sub

	'Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	'    Try

	'        'Clears out the error message
	'        Master.errorMsg = ""

	'        'check if its the first load or a post back
	'        If Not Page.IsPostBack Then

	'            'get userID
	'            Dim userID As Integer
	'            userID = Session("userID")

	'            'setup database connection
	'            Dim con As New SqlClient.SqlConnection
	'            con.ConnectionString = PATMAP.Global_asax.connString
	'            Dim query As New SqlClient.SqlCommand
	'            query.Connection = con
	'            Dim dr As SqlClient.SqlDataReader

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

	'            'get users profile details from database
	'            con.Open()
	'            query.CommandText = "select firstName, lastName, organization, position, email, workPhone, fax, address1, address2, municipality,provinceID, postalCode, interestModel, interestBoundary, loginName, loginPassword, securityQuestionID, securityAnswer, isnull(groupName,'') from users left join groups on users.groupID = groups.groupID where userID = " & userID.ToString
	'            dr = query.ExecuteReader
	'            dr.Read()

	'            'fill user detail into first section form fields
	'            txtFirstName.Text = dr.GetValue(0)
	'            txtLastName.Text = dr.GetValue(1)
	'            txtOrganization.Text = dr.GetValue(2)
	'            txtPosition.Text = dr.GetValue(3)
	'            txtEmail.Text = dr.GetValue(4)
	'            Dim wp As String = dr.GetValue(5)
	'            txtWP1.Text = wp.Substring(0, 3)
	'            txtWP2.Text = wp.Substring(3, 3)
	'            txtWP3.Text = wp.Substring(6, 4)
	'            If wp.Length > 10 Then
	'                txtWP4.Text = wp.Substring(10, (wp.Length - 10))
	'            End If
	'            Dim fax As String = dr.GetValue(6)
	'            If fax.Length >= 10 Then
	'                txtFax1.Text = fax.Substring(0, 3)
	'                txtFax2.Text = fax.Substring(3, 3)
	'                txtFax3.Text = fax.Substring(6, 4)
	'            End If

	'            If Not IsDBNull(dr.GetValue(7)) Then
	'                txtAddress1.Text = dr.GetValue(7)
	'            End If
	'            If Not IsDBNull(dr.GetValue(8)) Then
	'                txtAddress2.Text = dr.GetValue(8)
	'            End If
	'            If Not IsDBNull(dr.GetValue(9)) Then
	'                txtMunicipality.Text = dr.GetValue(9)
	'            End If
	'            If Not IsDBNull(dr.GetValue(10)) Then
	'                ddlProvince.SelectedValue = dr.GetValue(10)
	'            End If
	'            If Not IsDBNull(dr.GetValue(11)) Then
	'                txtPostalCode.Text = dr.GetValue(11)
	'            End If
	'            If Not IsDBNull(dr.GetValue(12)) Then
	'                cklRequest.Items(0).Selected = dr.GetValue(12)
	'            End If
	'            If Not IsDBNull(dr.GetValue(13)) Then
	'                cklRequest.Items(1).Selected = dr.GetValue(13)
	'            End If

	'            'fill user detail into second section form fields
	'            txtUsername.Text = dr.GetValue(14)
	'            txtPassword.Attributes.Add("value", "********")
	'            ddlSecurity.SelectedValue = dr.GetValue(16)
	'            txtAnswer.Text = dr.GetValue(17)
	'            txtGroup.Text = dr.GetValue(18)

	'            'cleanup
	'            dr.Close()

	'            'fill the user levels list box
	'            da = New SqlClient.SqlDataAdapter
	'            da.SelectCommand = New SqlClient.SqlCommand
	'            da.SelectCommand.Connection = con
	'            da.SelectCommand.CommandText = "select levels.levelID as levelID, levelName from levels inner join userlevels on levels.levelID = userlevels.levelID where userID = " & userID.ToString
	'            dt = New DataTable
	'            da.Fill(dt)
	'            lstUserLevel.DataSource = dt
	'            lstUserLevel.DataValueField = "levelID"
	'            lstUserLevel.DataTextField = "levelName"
	'            lstUserLevel.DataBind()

	'            'fill the section table
	'            fillSectionGrid()

	'            'clean up
	'            con.Close()

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


	Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
		Try
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

			If Not Trim(txtPostalCode.Text) = "" And Not common.ValidatePostalCode(Trim(txtPostalCode.Text)) Then
				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP71")
				Exit Sub
			End If


			'remove any single quotes from fields
			txtFirstName.Text = txtFirstName.Text.Replace("'", "''")
			txtLastName.Text = txtLastName.Text.Replace("'", "''")
			txtOrganization.Text = txtOrganization.Text.Replace("'", "''")
			txtPosition.Text = txtPosition.Text.Replace("'", "''")
			txtEmail.Text = txtEmail.Text.Replace("'", "''")
			txtAddress1.Text = txtAddress1.Text.Replace("'", "''")
			txtAddress2.Text = txtAddress2.Text.Replace("'", "''")
			txtMunicipality.Text = txtMunicipality.Text.Replace("'", "''")
			txtPostalCode.Text = txtPostalCode.Text.Replace("'", "''")
			txtPassword.Text = txtPassword.Text.Replace("'", "''")
			txtAnswer.Text = txtAnswer.Text.Replace("'", "''")

			'do any error checking of the form fields


			'get userID
			Dim userID As Integer
			userID = Session("userID")


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

				Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
					query.Connection = con

					If Trim(txtPassword.Text) = "********" Then
						Dim sbSQL As New StringBuilder
						sbSQL.Append("update users set firstName = @firstName, lastName = @lastname, organization = @organization")
						sbSQL.Append(", position = @position, email = @email, workphone = @workphone, fax = @fax")
						sbSQL.Append(", address1 = @address1, address2 = @address2, municipality = @municipality, provinceID = @provinceID")
						sbSQL.Append(", postalCode = @postalCode, interestModel = @interestModel, interestBoundary = @interestBoundary")
						sbSQL.Append(", SecurityQuestionID = @SecurityQuestionID, securityAnswer = @securityAnswer")
						sbSQL.Append(" where userID = @userID")

						query.CommandText = sbSQL.ToString()

						query.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID
						query.Parameters.Add(New SqlClient.SqlParameter("@firstName", SqlDbType.VarChar, 50)).Value = Trim(txtFirstName.Text)
						query.Parameters.Add(New SqlClient.SqlParameter("@lastname", SqlDbType.VarChar, 50)).Value = Trim(txtLastName.Text)
						query.Parameters.Add(New SqlClient.SqlParameter("@organization", SqlDbType.VarChar, 50)).Value = Trim(txtOrganization.Text)
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
					Else
						Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
						Dim enPswd = en.Encrypting(Trim(txtPassword.Text))

						Dim sbSQL As New StringBuilder
						sbSQL.Append("update users set firstName = @firstName, lastName = @lastname, organization = @organization")
						sbSQL.Append(", position = @position, email = @email, workphone = @workphone, fax = @fax")
						sbSQL.Append(", address1 = @address1, address2 = @address2, municipality = @municipality, provinceID = @provinceID")
						sbSQL.Append(", postalCode = @postalCode, interestModel = @interestModel, interestBoundary = @interestBoundary")
						sbSQL.Append(", SecurityQuestionID = @SecurityQuestionID, securityAnswer = @securityAnswer")
						sbSQL.Append(", loginPassword = @loginPassword")
						sbSQL.Append(" where userID = @userID")

						query.CommandText = sbSQL.ToString()

						query.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID
						query.Parameters.Add(New SqlClient.SqlParameter("@firstName", SqlDbType.VarChar, 50)).Value = Trim(txtFirstName.Text)
						query.Parameters.Add(New SqlClient.SqlParameter("@lastname", SqlDbType.VarChar, 50)).Value = Trim(txtLastName.Text)
						query.Parameters.Add(New SqlClient.SqlParameter("@organization", SqlDbType.VarChar, 50)).Value = Trim(txtOrganization.Text)
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
						query.Parameters.Add(New SqlClient.SqlParameter("@loginPassword", SqlDbType.NVarChar, 50)).Value = enPswd
					End If

					query.ExecuteNonQuery()
				End Using

			End Using

		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
			Exit Sub
		End Try

		'return to home page
		Response.Redirect("home.aspx")

	End Sub

	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try

			'Clears out the error message
			Master.errorMsg = ""

			'check if its the first load or a post back
			If Not Page.IsPostBack Then

				'get userID
				Dim userID As Integer
				userID = Session("userID")

				Using con As SqlClient.SqlConnection = New SqlClient.SqlConnection(PATMAP.Global_asax.connString)
					If Not con.State = ConnectionState.Open Then
						con.Open()
					End If

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

					'get users profile details from database
					Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
						query.Connection = con
						query.CommandText = "select firstName, lastName, organization, position, email, workPhone, fax, address1, address2, municipality,provinceID, postalCode, interestModel, interestBoundary, loginName, loginPassword, securityQuestionID, securityAnswer, isnull(groupName,'') from users left join groups on users.groupID = groups.groupID where userID = @userID"
						query.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID
						Using dr As SqlClient.SqlDataReader = query.ExecuteReader
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
							txtPassword.Attributes.Add("value", "********")
							ddlSecurity.SelectedValue = dr.GetValue(16)
							txtAnswer.Text = dr.GetValue(17)
							txtGroup.Text = dr.GetValue(18)

						End Using
					End Using

					'fill the user levels list box
					Using da As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter
						da.SelectCommand = New SqlClient.SqlCommand
						da.SelectCommand.Connection = con
						da.SelectCommand.CommandText = "select levels.levelID as levelID, levelName from levels inner join userlevels on levels.levelID = userlevels.levelID where userID = @userID"
						da.SelectCommand.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID
						Using dt As DataTable = New DataTable
							da.Fill(dt)
							lstUserLevel.DataSource = dt
							lstUserLevel.DataValueField = "levelID"
							lstUserLevel.DataTextField = "levelName"
							lstUserLevel.DataBind()
						End Using
					End Using

					'fill the section table
					fillSectionGrid()

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

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        'return to home page           
        Response.Redirect("home.aspx")
    End Sub

	Private Sub fillSectionGrid()
		Dim userID As Integer = Session("userID")

		Using con As SqlClient.SqlConnection = New SqlClient.SqlConnection(PATMAP.Global_asax.connString)
			If Not con.State = ConnectionState.Open Then
				con.Open()
			End If

			'fill in the section table
			Using da As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter
				Dim sbSQL As New StringBuilder
				sbSQL.AppendLine("select sections.sectionID, sectionName, case when access is null or access = 0 then 'No' else 'Yes' end  as access")
				sbSQL.AppendLine("from sections")
				sbSQL.AppendLine("left outer join (")
				sbSQL.AppendLine("select distinct(sections.sectionID), 1 as access")
				sbSQL.AppendLine("from sections")
				sbSQL.AppendLine("left join screenNames on screenNames.sectionID = sections.sectionID")
				sbSQL.AppendLine("left join levelsPermission on levelsPermission.screenNameID = screenNames.screenNameID")
				sbSQL.AppendLine("where sections.sectionID not in (1,2,7,8) and access = 1 and levelsPermission.levelID in (select levelID from userLevels where userID = @userID)")
				sbSQL.AppendLine(") results on results.sectionID = sections.sectionID")
				sbSQL.AppendLine("where sections.sectionID not in (1,2,7,8)")
				sbSQL.AppendLine("group by sections.sectionID, sectionName, results.access")

				da.SelectCommand = New SqlClient.SqlCommand
				da.SelectCommand.Connection = con
				da.SelectCommand.CommandText = sbSQL.ToString()
				da.SelectCommand.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID

				Dim dt As New DataTable
				da.Fill(dt)

				grdSection.DataSource = dt
				grdSection.DataBind()

				If IsNothing(Cache("sections")) Then
					Cache.Add("sections", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
				Else
					Cache("sections") = dt
				End If

			End Using

		End Using

	End Sub

	'Private Sub fillSectionGrid()
	'    'setup database connection
	'    Dim con As New SqlClient.SqlConnection
	'    con.ConnectionString = PATMAP.Global_asax.connString

	'    'fill in the section table
	'    Dim da As New SqlClient.SqlDataAdapter
	'    Dim dt As New DataTable
	'    Dim query As String

	'    da = New SqlClient.SqlDataAdapter
	'    da.SelectCommand = New SqlClient.SqlCommand()
	'    da.SelectCommand.Connection = con

	'    query = "select sections.sectionID, sectionName, case when access is null or access = 0 then 'No' else 'Yes' end  as access" & vbCrLf & _
	'            "from sections" & vbCrLf & _
	'            "left outer join (" & vbCrLf & _
	'            "	select distinct(sections.sectionID), 1 as access" & vbCrLf & _
	'            "	from sections" & vbCrLf & _
	'            "	left join screenNames on screenNames.sectionID = sections.sectionID" & vbCrLf & _
	'            "	left join levelsPermission on levelsPermission.screenNameID = screenNames.screenNameID" & vbCrLf & _
	'            "	where sections.sectionID not in (1,2,7,8) and access = 1 and levelsPermission.levelID in (select levelID from userLevels where userID = " & Session("userID") & ")" & vbCrLf & _
	'            ") results on results.sectionID = sections.sectionID" & vbCrLf & _
	'            "where sections.sectionID not in (1,2,7,8)" & vbCrLf & _
	'            "group by sections.sectionID, sectionName, results.access"

	'    da.SelectCommand.CommandText = query

	'    con.Open()
	'    da.Fill(dt)
	'    con.Close()

	'    grdSection.DataSource = dt
	'    grdSection.DataBind()

	'    If IsNothing(Cache("sections")) Then
	'        Cache.Add("sections", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
	'    Else
	'        Cache("sections") = dt
	'    End If

	'End Sub

End Class
