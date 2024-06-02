Public Partial Class edithelperr
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.helpText)

                'get Help screen ID
                Dim editHelpErrorID As String
                editHelpErrorID = Session("editHelpErrorID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader

                con.Open()

                If editHelpErrorID <> "" Then
                    'get help screen details from database
                    query.CommandText = "select errorName, errorText, description, notes from errorcodes where errorCode = '" & editHelpErrorID & "'"
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill user detail into first section form fields
                    txtCode.Text = editHelpErrorID
                    txtErrName.Text = Server.HtmlDecode(dr.GetValue(0))

                    If Not IsDBNull(dr.GetValue(1)) Then
                        ftbHelpText.Text = Server.HtmlDecode(dr.GetValue(1))
                    End If

                    If Not IsDBNull(dr.GetValue(2)) Then
                        txtDescription.Text = Server.HtmlDecode(dr.GetValue(2))
                    End If

                    If Not IsDBNull(dr.GetValue(3)) Then
                        txtNotes.Text = Server.HtmlDecode(dr.GetValue(3))
                    End If


                    'cleanup
                    dr.Close()

                End If

                'check if should show reset button
                query.CommandText = "select errorCode from errorCodesReset where errorCode = '" & editHelpErrorID & "'"
                dr = query.ExecuteReader
                If dr.Read() Then
                    btnReset.Visible = True
                Else
                    btnReset.Visible = False
                End If
                dr.Close()

                'clean up
                con.Close()

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
            common.ChangeTitle(Session("editHelpErrorID"), lblTitle)
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
        Session.Remove("editHelpErrorID")
        Response.Redirect("viewhelperr.aspx")

    End Sub

    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            'make sure required fields are filled out
            If String.IsNullOrEmpty(Trim(txtErrName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP33")
                Exit Sub
            End If
            If Not common.ValidateNoSpecialChar(Trim(txtErrName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP87")
                Exit Sub
            End If
            If String.IsNullOrEmpty(Trim(txtCode.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP34")
                Exit Sub
            End If
            If Not common.ValidateNoSpecialChar(Trim(txtCode.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP92")
                Exit Sub
            End If

            'remove any single quotes from fields
            txtErrName.Text = Server.HtmlEncode(Trim(txtErrName.Text.Replace("'", "''")))
            txtCode.Text = Server.HtmlEncode(Trim(txtCode.Text.Replace("'", "''")))
            txtDescription.Text = Server.HtmlEncode(Trim(txtDescription.Text.Replace("'", "''")))
            txtNotes.Text = Server.HtmlEncode(Trim(txtNotes.Text.Replace("'", "''")))
            ftbHelpText.Text = Server.HtmlEncode(Trim(ftbHelpText.Text.Replace("'", "''")))


            'get Help screen ID
            Dim editHelpErrorID As String
            editHelpErrorID = Session("editHelpErrorID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            Dim dr As SqlClient.SqlDataReader
            con.Open()

            'make sure error code has not already been used
            If Trim(txtCode.Text) <> editHelpErrorID Then
                query.CommandText = "select errorCode from errorCodes where errorCode = '" & Trim(txtCode.Text) & "'"
                dr = query.ExecuteReader
                If dr.Read() Then
                    'cleanup
                    dr.Close()

                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP35")
                    Exit Sub
                End If

                'cleanup
                dr.Close()
            End If

            'save the help screen details
            If editHelpErrorID = "" Then
                'insert help screen 
                query.CommandText = "insert into errorcodes (errorCode, errorName, errorText, description, notes) values ('" & Trim(txtCode.Text) & "','" & Trim(txtErrName.Text) & "','" & ftbHelpText.Text & "','" & Trim(txtDescription.Text) & "','" & Trim(txtNotes.Text) & "')" & vbCrLf
            Else
                'update help screen
                query.CommandText = "update errorcodes set errorCode = '" & Trim(txtCode.Text) & "', errorName = '" & Trim(txtErrName.Text) & "', description = '" & Trim(txtDescription.Text) & "', notes = '" & Trim(txtNotes.Text) & "', errorText = '" & ftbHelpText.Text & "' where errorcode = '" & editHelpErrorID & "'" & vbCrLf
            End If

            'check if this has been select to be a restore point
            If ckbReset.Checked = True Then
                query.CommandText += "delete from errorCodesReset where errorCode = '" & Trim(txtCode.Text) & "'" & vbCrLf
                query.CommandText = "insert into errorCodesReset select * from errorCodes where errorCode = '" & Trim(txtCode.Text) & "'" & vbCrLf
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
            Session.Remove("editHelpErrorID")
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        'return to home page
        Response.Redirect("viewhelperr.aspx")

    End Sub

    Protected Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReset.Click
        Try
            'get Help screen ID
            Dim editHelpErrorID As String
            editHelpErrorID = Session("editHelpErrorID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            Dim dr As SqlClient.SqlDataReader

            'get help screen details from database
            con.Open()
            query.CommandText = "select errorName, errorText, description, notes from errorcodesreset where errorCode = '" & editHelpErrorID & "'"
            dr = query.ExecuteReader
            dr.Read()

            'fill user detail into first section form fields
            txtCode.Text = Server.HtmlDecode(editHelpErrorID)
            txtErrName.Text = Server.HtmlDecode(dr.GetValue(0))
            ftbHelpText.Text = Server.HtmlDecode(dr.GetValue(1))
            txtDescription.Text = Server.HtmlDecode(dr.GetValue(2))
            txtNotes.Text = Server.HtmlDecode(dr.GetValue(3))

            'cleanup
            dr.Close()
            con.Close()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class