Public Partial Class editpropcode
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            'check if its the first load or a post back
            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                'get present use code ID to edit
                Dim editPropCode As Integer
                editPropCode = Session("editPropCode")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader
                con.Open()

                If editPropCode <> -1 Then
                    'get present use code details from database
                    query.CommandText = "select shortDescription, description, notes from presentUseCodes where presentUseCodeID = " & editPropCode
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill present use code details into the form fields
                    txtPropCode.Text = editPropCode
                    txtShortDesc.Text = dr.GetValue(0)

                    If Not IsDBNull(dr.GetValue(1)) Then
                        txtDescription.Text = dr.GetValue(1)
                    Else
                        txtDescription.Text = ""
                    End If

                    If Not IsDBNull(dr.GetValue(2)) Then
                        txtNotes.Text = dr.GetValue(2)
                    Else
                        txtNotes.Text = ""
                    End If

                    'cleanup
                    dr.Close()
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
            If Trim(txtPropCode.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP22")
                Exit Sub
            End If
            If Not Regex.IsMatch(Trim(txtPropCode.Text), "^[0-9]*$") Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP93")
                Exit Sub
            End If
            If Trim(txtShortDesc.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP23")
                Exit Sub
            End If

            'remove any single quotes from fields
            txtPropCode.Text = txtPropCode.Text.Replace("'", "")
            txtShortDesc.Text = txtShortDesc.Text.Replace("'", "''")
            txtDescription.Text = txtDescription.Text.Replace("'", "''")
            txtNotes.Text = txtNotes.Text.Replace("'", "''")

            'get present Use Code to edit
            Dim editPropCode As Integer
            editPropCode = Session("editPropCode")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()
            Dim dr As SqlClient.SqlDataReader

            If editPropCode <> CType(txtPropCode.Text, Integer) Then
                'check if the present use code exists in the database
                query.CommandText = "select presentUseCodeID from presentUseCodes where presentUseCodeID=" & Trim(txtPropCode.Text)
                dr = query.ExecuteReader()

                If dr.Read() Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP109")
                    dr.Close()
                    con.Close()
                    Exit Sub
                End If

                dr.Close()
            End If
            'build add/update string
            If editPropCode = -1 Then
                query.CommandText = "insert into presentUseCodes (presentUseCodeID, shortDescription, description, notes) values ('" & Trim(txtPropCode.Text) & "','" & Trim(txtShortDesc.Text) & "','" & Trim(txtDescription.Text) & "','" & Trim(txtNotes.Text) & "')"
            Else
                query.CommandText = "update presentUseCodes set presentUseCodeID = '" & Trim(txtPropCode.Text) & "', shortDescription = '" & Trim(txtShortDesc.Text) & "', description = '" & Trim(txtDescription.Text) & "', notes = '" & Trim(txtNotes.Text) & "' where presentUseCodeID = '" & editPropCode & "'"
            End If
            query.ExecuteNonQuery()

            'clean up
            con.Close()
            Session.Remove("editPropCode")

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewpropcode.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("editPropCode")
        Response.Redirect("viewpropcode.aspx")
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("editPropCode"), lblTitle)
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