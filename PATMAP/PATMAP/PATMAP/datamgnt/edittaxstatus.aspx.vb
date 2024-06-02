Public Partial Class edittaxstatus
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            'check if its the first load or a post back
            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                'get tax status ID to edit
                Dim editTaxStatusID As Integer
                editTaxStatusID = Session("editTaxStatusID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader
                con.Open()

                If editTaxStatusID <> 0 Then
                    'get tax status details from database
                    query.CommandText = "select taxStatus, description, notes from taxStatus where taxStatusID = " & editTaxStatusID
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill tax status details into the form fields
                    txtTaxStatus.Text = dr.GetValue(0)
                    txtDescription.Text = dr.GetValue(1)
                    txtNotes.Text = dr.GetValue(2)

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
            If Trim(txtTaxStatus.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP24")
                Exit Sub
            End If

            'If Not common.ValidateNoSpecialChar(Trim(txtTaxStatus.Text)) Then
            '    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP95")
            '    Exit Sub
            'End If

            'remove any single quotes from fields
            txtTaxStatus.Text = txtTaxStatus.Text.Replace("'", "''")
            txtDescription.Text = txtDescription.Text.Replace("'", "''")
            txtNotes.Text = txtNotes.Text.Replace("'", "''")

            'get groupID to edit
            Dim editTaxStatusID As Integer
            editTaxStatusID = Session("editTaxStatusID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            'check if the tax status name already exists
            Dim dr As SqlClient.SqlDataReader
            query.CommandText = "select taxStatus from taxStatus where taxStatus='" & Trim(txtTaxStatus.Text) & "' AND taxStatusID <> " & editTaxStatusID.ToString
            dr = query.ExecuteReader()

            If dr.Read() Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP116")
                dr.Close()
                con.Close()
                Exit Sub
            End If

            'build add/update string
            dr.Close()
            If editTaxStatusID = 0 Then
                query.CommandText = "insert into taxStatus (taxStatus, description, notes) values ('" & Trim(txtTaxStatus.Text) & "','" & Trim(txtDescription.Text) & "','" & Trim(txtNotes.Text) & "')"
            Else
                query.CommandText = "update taxStatus set taxStatus = '" & Trim(txtTaxStatus.Text) & "', description = '" & Trim(txtDescription.Text) & "', notes = '" & Trim(txtNotes.Text) & "' where taxStatusID = " & editTaxStatusID
            End If
            query.ExecuteNonQuery()

            'clean up
            con.Close()
            Session.Remove("editTaxStatusID")

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewtaxstatus.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("editTaxStatusID")
        Response.Redirect("viewtaxstatus.aspx")
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("editTaxStatusID"), lblTitle)
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