
Partial Class forgotpassword
    Inherits System.Web.UI.Page

    Protected Sub btnESubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnESubmit.Click
        Try
            'create connection variable
            Using con As SqlClient.SqlConnection = New SqlClient.SqlConnection(PATMAP.Global_asax.connString)
                If Not con.State = ConnectionState.Open Then
                    con.Open()
                End If

                Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
                    query.Connection = con
                    'modified to show active first  13-apr-2017
                    'query.CommandText = "Select userStatusID, securityQuestion from users inner join securityQuestions on users.securityQuestionID = securityQuestions.securityQuestionID where email = @email"
                    query.CommandText = "Select userStatusID, securityQuestion from users inner join securityQuestions on users.securityQuestionID = securityQuestions.securityQuestionID where email = @email ORDER BY userStatusID"
                    query.Parameters.Add(New SqlClient.SqlParameter("@email", SqlDbType.VarChar, 100)).Value = Trim(txtEmail.Text)
                    Using dr As SqlClient.SqlDataReader = query.ExecuteReader
                        If dr.Read() Then
                            'account found
                            'check account user status
                            Select Case dr.GetValue(0)
                                Case 2
                                    'account has been disabled
                                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP4")
                                Case 3
                                    'account has not yet been approved
                                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP5")
                                Case 4
                                    'account request has been rejected
                                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP6")
                                Case Else
                                    Master.errorMsg = ""
                                    'hide/show appropriate panels to verify security question
                                    pnlEmail.Visible = False
                                    txtQuestion.Text = dr.GetValue(1)
                                    pnlQuestion.Visible = True
                            End Select
                        Else
                            'email address not found in user databse
                            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP2")
                        End If
                    End Using
                End Using
            End Using

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnQSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnQSubmit.Click
        Try

            'create connection variable
            Using con As SqlClient.SqlConnection = New SqlClient.SqlConnection(PATMAP.Global_asax.connString)
                If Not con.State = ConnectionState.Open Then
                    con.Open()
                End If

                Dim IsPasswordMatched As Boolean = False

                Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
                    query.Connection = con
                    query.CommandText = "Select userID, userStatusID from users where userStatusID = 1 and email = @email and securityAnswer = @securityAnswer"
                    query.Parameters.Add(New SqlClient.SqlParameter("@email", SqlDbType.VarChar, 100)).Value = Trim(txtEmail.Text)
                    query.Parameters.Add(New SqlClient.SqlParameter("@securityAnswer", SqlDbType.VarChar, 150)).Value = Trim(txtAnswer.Text)
                    Using dr As SqlClient.SqlDataReader = query.ExecuteReader
                        If dr.Read() Then
                            IsPasswordMatched = True
                        End If
                    End Using
                End Using

                If IsPasswordMatched Then
                    Master.errorMsg = ""

                    Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
                        query.Connection = con
                        'clear out any counts for bad security question attemps
                        query.CommandText = "update users set numBadSecurityAnswers = 0 where email = @email"
                        query.Parameters.Add(New SqlClient.SqlParameter("@email", SqlDbType.VarChar, 100)).Value = Trim(txtEmail.Text)
                        query.ExecuteNonQuery()
                    End Using

                    'hide/show appropriate panels for successful security question
                    pnlQuestion.Visible = False
                    pnlSuccess.Visible = True

                    'create new password
                    Dim NewPassword = common.GeneratePassword
                    Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
                    Dim enNewPassword = en.Encrypting(NewPassword)

                    Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
                        query.Connection = con
                        'reset password
                        query.CommandText = "update users set loginPassword = @loginPassword where email = @email"
                        query.Parameters.Add(New SqlClient.SqlParameter("@loginPassword", SqlDbType.NVarChar, 50)).Value = enNewPassword
                        query.Parameters.Add(New SqlClient.SqlParameter("@email", SqlDbType.VarChar, 100)).Value = Trim(txtEmail.Text)
                        query.ExecuteNonQuery()
                    End Using

                    'send email with username and new password
                    'Dim Mail As New System.Net.Mail.MailMessage
                    'Dim SMTP As New System.Net.Mail.SmtpClient(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport)
                    Dim Mail As New OpenSmtp.Mail.MailMessage()
                    'Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport) '***Inky commented this line out in Apr-2010
                    Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPuserName, PATMAP.Global_asax.SMTPpassword) '***Inky's Addition: Apr-2010

                    Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
                        query.Connection = con
                        'get the user name and login name for the entered email address
                        'modified to email only active user 13-apr-2017
                        'query.CommandText = "select firstname + ' ' + lastname, loginName from users where email = @email"
                        query.CommandText = "select firstname + ' ' + lastname, loginName from users where userStatusID = 1 and email = @email"
                        query.Parameters.Add(New SqlClient.SqlParameter("@email", SqlDbType.VarChar, 100)).Value = Trim(txtEmail.Text)
                        Using dr As SqlClient.SqlDataReader = query.ExecuteReader
                            If dr.Read() Then
                                'build the email                
                                Mail.GetBodyFromFile(Request.MapPath("/includes/Email.html"))

                                Dim mailMessage As String
                                mailMessage = "Hello,<br><br>"
                                mailMessage += "Your password has been reset.<br><br>"
                                mailMessage += "Here is your username and password to access the Property Assessment and Tax Mapping and Analysis Program:<br><br>"
                                mailMessage += "Username: " & dr.GetValue(1) & "<br>Password: " & NewPassword & "<br><br>"
                                mailMessage += "Thank You"

                                Mail.AddImage(Request.MapPath("/includes/governmentLogoNew.jpg"), "patmap01")
                                Mail.HtmlBody = Mail.Body.Replace("governmentLogoNew.jpg", "cid:patmap01")
                                Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)
                                Mail.Subject = "Your PATMAP password has been reset"
                                Mail.To.Add(New OpenSmtp.Mail.EmailAddress(Trim(txtEmail.Text), dr.GetValue(0)))
                                Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailName)

                                SMTP.SendMail(Mail)
                            End If
                        End Using
                    End Using

                Else

                    Dim flgLessThan3BadLogin As Boolean = False

                    Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
                        query.Connection = con
                        'check if 3 security question attemps has been made and if so disable account
                        query.CommandText = "update users set userStatusID = 2 where numBadSecurityAnswers >= 2 and email = @email"
                        query.Parameters.Add(New SqlClient.SqlParameter("@email", SqlDbType.VarChar, 100)).Value = Trim(txtEmail.Text)
                        If query.ExecuteNonQuery() = 0 Then
                            flgLessThan3BadLogin = True
                        End If
                    End Using

                    If flgLessThan3BadLogin Then
                        'less then 3 bad login attempts have been made
                        'increment security question counts
                        Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
                            query.Connection = con
                            'check if 3 security question attemps has been made and if so disable account
                            query.CommandText = "update users set numBadSecurityAnswers = numBadSecurityAnswers + 1 where userStatusID = 1 and email = @email"
                            query.Parameters.Add(New SqlClient.SqlParameter("@email", SqlDbType.VarChar, 100)).Value = Trim(txtEmail.Text)
                            query.ExecuteNonQuery()
                        End Using
                        'return bad password error message
                        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP7")
                    Else
                        '3 bad login attempts have been made and account has been disabled
                        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP8")
                    End If

                End If

            End Using

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    'Protected Sub btnESubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnESubmit.Click
    '    Try
    '        'create connection variable
    '        Dim con As New SqlClient.SqlConnection
    '        con.ConnectionString = PATMAP.Global_asax.connString

    '        'check if an account with that email address exists
    '        Dim query As New SqlClient.SqlCommand
    '        query.Connection = con
    '        query.CommandText = "Select userStatusID, securityQuestion from users inner join securityQuestions on users.securityQuestionID = securityQuestions.securityQuestionID where email = '" & Trim(txtEmail.Text) & "'"
    '        Dim dr As SqlClient.SqlDataReader
    '        con.Open()
    '        dr = query.ExecuteReader
    '        If dr.Read() Then
    '            'account found

    '            'check account user status
    '            Select Case dr.GetValue(0)
    '                Case 2
    '                    'account has been disabled
    '                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP4")
    '                    'clean up
    '                    dr.Close()
    '                    con.Close()
    '                Case 3
    '                    'account has not yet been approved
    '                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP5")
    '                    'clean up
    '                    dr.Close()
    '                    con.Close()
    '                Case 4
    '                    'account request has been rejected
    '                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP6")
    '                    'clean up
    '                    dr.Close()
    '                    con.Close()
    '                Case Else
    '                    Master.errorMsg = ""

    '                    'hide/show appropriate panels to verify security question
    '                    pnlEmail.Visible = False
    '                    txtQuestion.Text = dr.GetValue(1)
    '                    pnlQuestion.Visible = True

    '                    'clean up
    '                    dr.Close()
    '                    con.Close()
    '            End Select
    '        Else
    '            'email address not found in user databse
    '            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP2")
    '            'clean up
    '            dr.Close()
    '            con.Close()
    '        End If
    '    Catch
    '        'retrieves error message
    '        Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
    '    End Try
    'End Sub

    'Protected Sub btnQSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnQSubmit.Click
    '    Try
    '        'create connection variable
    '        Dim con As New SqlClient.SqlConnection
    '        con.ConnectionString = PATMAP.Global_asax.connString

    '        'check if answer to security question is correct
    '        Dim query As New SqlClient.SqlCommand
    '        query.Connection = con
    '        query.CommandText = "Select userID, userStatusID from users where userStatusID = 1 and email = '" & Trim(txtEmail.Text) & "' and securityAnswer = '" & Trim(txtAnswer.Text) & "'"
    '        Dim dr As SqlClient.SqlDataReader
    '        con.Open()
    '        dr = query.ExecuteReader
    '        If dr.Read() Then
    '            'correct answer

    '            'clean up
    '            dr.Close()
    '            Master.errorMsg = ""

    '            'clear out any counts for bad security question attemps
    '            query.CommandText = "update users set numBadSecurityAnswers = 0 where email = '" & Trim(txtEmail.Text) & "'"
    '            query.ExecuteNonQuery()

    '            'hide/show appropriate panels for successful security question
    '            pnlQuestion.Visible = False
    '            pnlSuccess.Visible = True

    '            'create new password
    '            Dim NewPassword = common.GeneratePassword
    '            Dim en As New SymmCrypto(SymmCrypto.SymmProvEnum.RC2)
    '            Dim enNewPassword = en.Encrypting(NewPassword)

    '            'reset password
    '            query.CommandText = "update users set loginPassword = '" & enNewPassword & "' where email = '" & Trim(txtEmail.Text) & "'"
    '            query.ExecuteNonQuery()

    '            'send email with username and new password
    '            'Dim Mail As New System.Net.Mail.MailMessage
    '            'Dim SMTP As New System.Net.Mail.SmtpClient(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport)
    '            Dim Mail As New OpenSmtp.Mail.MailMessage()
    '            'Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport) '***Inky commented this line out in Apr-2010
    '            Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPuserName, PATMAP.Global_asax.SMTPpassword) '***Inky's Addition: Apr-2010

    '            'get the user name and login name for the entered email address
    '            query.CommandText = "select firstname + ' ' + lastname, loginName from users where email = '" & Trim(txtEmail.Text) & "'"
    '            dr = query.ExecuteReader
    '            dr.Read()

    '            'build the email                
    '            Mail.GetBodyFromFile(Request.MapPath("/includes/Email.html"))

    '            Dim mailMessage As String
    '            mailMessage = "Hello,<br><br>"
    '            mailMessage += "Your password has been reset.<br><br>"
    '            mailMessage += "Here is your username and password to access the Property Assessment and Tax Mapping and Analysis Program:<br><br>"
    '            mailMessage += "Username: " & dr.GetValue(1) & "<br>Password: " & NewPassword & "<br><br>"
    '            mailMessage += "Thank You"

    '            Mail.AddImage(Request.MapPath("/includes/governmentLogoNew.jpg"), "patmap01")
    '            Mail.HtmlBody = Mail.Body.Replace("governmentLogoNew.jpg", "cid:patmap01")
    '            Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)
    '            Mail.Subject = "Your PATMAP password has been reset"
    '            Mail.To.Add(New OpenSmtp.Mail.EmailAddress(Trim(txtEmail.Text), dr.GetValue(0)))
    '            Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailName)

    '            SMTP.SendMail(Mail)
    '        Else
    '            'clean up
    '            dr.Close()

    '            'check if 3 security question attemps has been made and if so disable account
    '            query.CommandText = "update users set userStatusID = 2 where numBadSecurityAnswers >= 2 and email = '" & Trim(txtEmail.Text) & "'"
    '            If query.ExecuteNonQuery() = 0 Then
    '                'less then 3 bad login attempts have been made

    '                'increment security question counts
    '                query.CommandText = "update users set numBadSecurityAnswers = numBadSecurityAnswers + 1 where userStatusID = 1 and email = '" & Trim(txtEmail.Text) & "'"
    '                query.ExecuteNonQuery()

    '                'return bad password error message
    '                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP7")
    '            Else
    '                '3 bad login attempts have been made and account has been disabled
    '                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP8")
    '            End If
    '        End If

    '        'clean up
    '        con.Close()
    '    Catch
    '        'retrieves error message
    '        Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
    '    End Try
    'End Sub

End Class
