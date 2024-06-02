
Partial Class index
    Inherits System.Web.UI.Page

	Protected Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnLogin.Click

		Using con As SqlClient.SqlConnection = New SqlClient.SqlConnection(PATMAP.Global_asax.connString)
			'first open the connection if not already opened
			If Not con.State = ConnectionState.Open Then
				con.Open()
			End If

			Master.errorMsg = ""

			'remove any single quotes from the username and password
			txtUsername.Text = txtUsername.Text.Replace("'", "")
			txtPassword.Text = txtPassword.Text.Replace("'", "")

			Dim flgInvalidUserStatusFound As Boolean = False

			Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
				query.Connection = con
				'verify if account is active
				query.CommandText = "Select userStatusID from users where userStatusID <> 1 and loginName = @loginName"
				query.Parameters.Add(New SqlClient.SqlParameter("@loginName", SqlDbType.VarChar, 50)).Value = txtUsername.Text

				Using dr As SqlClient.SqlDataReader = query.ExecuteReader
					If dr.Read() Then
						flgInvalidUserStatusFound = True
						'the account is not active
						Select Case dr.GetValue(0)
							Case 2
								'account has been disabled
								Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP4")
								Exit Sub
							Case 3
								'account has not yet been approved
								Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP5")
								Exit Sub
							Case 4
								'account request has been rejected
								Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP6")
								Exit Sub
						End Select
					End If
				End Using
			End Using

			If flgInvalidUserStatusFound Then
				Exit Sub
			End If

			'verify username and password are correct 
			Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
			Dim enPswd = en.Encrypting(txtPassword.Text)

			Dim flgUserIDFound As Boolean = False
			Dim userID As Integer

			Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
				query.Connection = con

				query.CommandText = "Select userID from users where userStatusID = 1 and loginName = @loginName and loginPassword = @loginPassword"
				query.Parameters.Add(New SqlClient.SqlParameter("@loginName", SqlDbType.VarChar, 50)).Value = txtUsername.Text
				query.Parameters.Add(New SqlClient.SqlParameter("@loginPassword", SqlDbType.NVarChar, 50)).Value = enPswd

				Using dr As SqlClient.SqlDataReader = query.ExecuteReader
					If dr.Read() Then
						flgUserIDFound = True
						'correct                    
						userID = dr.GetValue(0)
					End If
				End Using
			End Using

			If flgUserIDFound Then

				Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
					query.Connection = con
					query.CommandText = "select count(*) from userlevels where userID = @userID"
					query.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID
					Using dr As SqlClient.SqlDataReader = query.ExecuteReader
						If dr.Read() Then
							If dr.Item(0) = 0 Then
								Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP135")
								Exit Sub
							End If
						End If
					End Using
				End Using

				Master.errorMsg = ""

				'add required session data
				Session.Add("userID", userID)

				Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
					query.Connection = con

					Dim sbSQL As New StringBuilder
					sbSQL.AppendLine("update users set numBadLoginPasswords = 0 where userStatusID = 1 and loginName = @loginName")
					sbSQL.AppendLine("insert into userStatistics select @userID, getdate()")

					query.CommandText = sbSQL.ToString()
					query.Parameters.Add(New SqlClient.SqlParameter("@loginName", SqlDbType.VarChar, 50)).Value = txtUsername.Text
					query.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID

					Dim trans As SqlClient.SqlTransaction
					trans = con.BeginTransaction()
					query.Transaction = trans
					Try
						query.ExecuteNonQuery()
						trans.Commit()
					Catch
						trans.Rollback()
						Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
						Exit Sub
					End Try


				End Using

				'transfer to home page
				Response.Redirect("home.aspx", False)

			Else
				'UserID not found

				Dim flgLessThan3BadLogin As Boolean = False
				'failed login
				Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
					query.Connection = con

					'check if 3 bad login attemps has been made and if so disable account
					query.CommandText = "update users set userStatusID = 2 where userStatusID = 1 and numBadLoginPasswords >= 2 and loginName = @loginName"
					query.Parameters.Add(New SqlClient.SqlParameter("@loginName", SqlDbType.VarChar, 50)).Value = txtUsername.Text

					If query.ExecuteNonQuery() = 0 Then
						flgLessThan3BadLogin = True
					Else
						'3 bad login attempts have been made and account has been disabled
						Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP4")
						Exit Sub
					End If
				End Using

				If flgLessThan3BadLogin Then
					'less then 3 bad login attempts have been made
					Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
						query.Connection = con
						'increment bad login counts
						query.CommandText = "update users set numBadLoginPasswords = numBadLoginPasswords + 1 where userStatusID = 1 and loginName = @loginName"
						query.Parameters.Add(New SqlClient.SqlParameter("@loginName", SqlDbType.VarChar, 50)).Value = txtUsername.Text
						query.ExecuteNonQuery()
						'return bad password error message
						Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP1")
						Exit Sub
					End Using
				End If
			End If

		End Using


		''Try
		''create connection and querying variables
		'Dim con As New SqlClient.SqlConnection
		'con.ConnectionString = PATMAP.Global_asax.connString
		'con.Open()
		'Dim query As New SqlClient.SqlCommand
		'query.Connection = con
		'Dim dr As SqlClient.SqlDataReader

		''remove any single quotes from the username and password
		'txtUsername.Text = txtUsername.Text.Replace("'", "")
		'txtPassword.Text = txtPassword.Text.Replace("'", "")

		''verify if account is active
		'query.CommandText = "Select userStatusID from users where userStatusID <> 1 and loginName = '" & txtUsername.Text & "'"
		'dr = query.ExecuteReader
		'If dr.Read() Then
		'	'the account is not active
		'	Select Case dr.GetValue(0)
		'		Case 2
		'			'account has been disabled
		'			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP4")
		'			'clean up
		'			dr.Close()
		'			con.Close()
		'		Case 3
		'			'account has not yet been approved
		'			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP5")
		'			'clean up
		'			dr.Close()
		'			con.Close()
		'		Case 4
		'			'account request has been rejected
		'			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP6")
		'			'clean up
		'			dr.Close()
		'			con.Close()
		'	End Select
		'Else
		'	'the account is active

		'	'clean up
		'	dr.Close()

		'	'verify username and password are correct 
		'	Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
		'	Dim enPswd = en.Encrypting(txtPassword.Text)

		'	query.CommandText = "Select userID from users where userStatusID = 1 and loginName = '" & txtUsername.Text & "' and loginPassword = '" & enPswd & "'"
		'	dr = query.ExecuteReader
		'	If dr.Read() Then

		'		'correct                    
		'		Dim userID As Integer
		'		userID = dr.GetValue(0)
		'		dr.Close()

		'		query.CommandText = "select count(*) from userlevels where userID = " & userID
		'		dr = query.ExecuteReader()

		'		If dr.Read() Then
		'			If dr.Item(0) = 0 Then
		'				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP135")
		'				dr.Close()
		'				con.Close()
		'				Exit Sub
		'			End If
		'		End If

		'		dr.Close()

		'		'verify if the user has the presentaion user level privilege in satellite mode
		'		'If PATMAP.Global_asax.satellite Then
		'		'    query.CommandText = "Select userID from userLevels l where l.levelID=3 and l.userID=(select userID from users u where u.loginName = '" & txtUsername.Text & "')"
		'		'    dr = query.ExecuteReader
		'		'    If Not dr.Read() Then
		'		'        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP133")
		'		'        dr.Close()
		'		'        con.Close()
		'		'        Exit Sub
		'		'    End If
		'		'    dr.Close()
		'		'End If

		'		Master.errorMsg = ""

		'		'add required session data
		'		Session.Add("userID", userID)

		'		'clear out any counts for bad login attemps
		'		query.CommandText = "update users set numBadLoginPasswords = 0 where userStatusID = 1 and loginName = '" & txtUsername.Text & "'" & vbCrLf

		'		'add entry to system statistics table
		'		query.CommandText += "insert into userStatistics select " & userID.ToString & ",getdate()"

		'		Dim trans As SqlClient.SqlTransaction
		'		trans = con.BeginTransaction()
		'		query.Transaction = trans
		'		Try
		'			query.ExecuteNonQuery()
		'			trans.Commit()
		'		Catch
		'			trans.Rollback()
		'			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
		'			con.Close()
		'			Exit Sub
		'		End Try

		'		'transfer to home page
		'		con.Close()
		'		Response.Redirect("home.aspx", False)
		'		Exit Sub
		'	Else
		'		'failed login
		'		dr.Close()

		'		'check if 3 bad login attemps has been made and if so disable account
		'		query.CommandText = "update users set userStatusID = 2 where userStatusID = 1 and numBadLoginPasswords >= 2 and loginName = '" & txtUsername.Text & "'"
		'		If query.ExecuteNonQuery() = 0 Then
		'			'less then 3 bad login attempts have been made

		'			'increment bad login counts
		'			query.CommandText = "update users set numBadLoginPasswords = numBadLoginPasswords + 1 where userStatusID = 1 and loginName = '" & txtUsername.Text & "'"
		'			query.ExecuteNonQuery()

		'			'return bad password error message
		'			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP1")
		'			con.Close()
		'		Else
		'			'3 bad login attempts have been made and account has been disabled
		'			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP4")
		'			con.Close()
		'		End If

		'		con.Close()
		'	End If
		'End If

		''Catch
		''retrieves error message
		''Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		''End Try
	End Sub

	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Master.errorMsg = ""

		'make sure levelID is not from the previous session		17-jul-2014
		Session.Remove("levelID")

		If Not IsPostBack Then
			txtUsername.Focus()
		End If

		'If PATMAP.Global_asax.satellite Then
		'    panelLoginLinks.Visible = False
		'    pnlMoreLinks.Visible = False

		'    PATMAP.Global_asax.connString = "Data Source=" & PATMAP.Global_asax.SQLEngineServer & ";Initial Catalog=" & PATMAP.Global_asax.DBName & ";Persist Security Info=True;User ID=" & PATMAP.Global_asax.DBUser & ";Password=" & PATMAP.Global_asax.DBPassword & ";"
		'    'PATMAP.Global_asax.SQLEngineServer = "(local)"
		'    'PATMAP.Global_asax.SQLIntegrationServer = "(local)"
		'    'PATMAP.Global_asax.SQLReportingServer = "http://localhost/ReportServer"
		'    'PATMAP.Global_asax.domainName = "."            

		'    ''setup database connection
		'    'Dim con As New SqlClient.SqlConnection
		'    'con.ConnectionString = PATMAP.Global_asax.connString
		'    'con.Open()

		'    'Dim query As New SqlClient.SqlCommand
		'    'Dim dr As SqlClient.SqlDataReader
		'    'query.Connection = con
		'    'query.CommandText = "select destination, userName, password from satelliteInfo where machineName='" & System.Configuration.ConfigurationManager.AppSettings("MachineName") & "'"
		'    'dr = query.ExecuteReader
		'    'If dr.Read() Then

		'    '    Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
		'    '    Dim enPswd As String


		'    '    If dr.GetValue(2) <> "" Then
		'    '        enPswd = en.Decrypting(dr.GetValue(2))
		'    '    Else
		'    '        enPswd = dr.GetValue(2)
		'    '    End If


		'    '    PATMAP.Global_asax.domainUser = dr.GetValue(1)
		'    '    PATMAP.Global_asax.domainPassword = enPswd
		'    '    PATMAP.Global_asax.FileRootPath = dr.GetValue(0)
		'    'Else
		'    '    PATMAP.Global_asax.domainUser = ""
		'    '    PATMAP.Global_asax.domainPassword = ""
		'    '    PATMAP.Global_asax.FileRootPath = ""
		'    'End If

		'    'dr.Close()
		'    'con.Close()
		'End If
	End Sub

End Class
