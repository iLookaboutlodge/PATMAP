Public Partial Class editjuristype
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            'check if its the first load or a post back
            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                'get jurisdiction type ID to edit
                Dim editJurisTypeID As Integer
                editJurisTypeID = Session("editJurisTypeID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader
                con.Open()

                Dim jurisGroup As Integer
                If editJurisTypeID <> 0 Then
                    'get jurisdiction type details from database
                    query.CommandText = "select jurisdictionType, description, notes, JurisdictionGroupID from jurisdictiontypes where jurisdictionTypeID = " & editJurisTypeID
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill jurisdiction type details into the form fields
                    txtJurisType.Text = dr.GetValue(0)
                    If Not dr.IsDBNull(1) Then
                        txtDescription.Text = dr.GetValue(1)
                    Else
                        txtDescription.Text = ""
                    End If
                    If Not dr.IsDBNull(2) Then
                        txtNotes.Text = dr.GetValue(2)
                    Else
                        txtNotes.Text = ""
                    End If

                    jurisGroup = dr.GetValue(3)

                    'cleanup
                    dr.Close()
                End If

                'get jurisdiction group info from the database (for new or existing jurisType)
                'if juris = school division, then no need for displaying jurisGroup
                If editJurisTypeID = 1 Then
                    ddlJurisGroup.Visible = False
                    lblNoGroup.Visible = True
                Else
                    'first bind the list of jurisGroups to the ddl
                    Dim da As New SqlClient.SqlDataAdapter
                    da.SelectCommand = New SqlClient.SqlCommand
                    da.SelectCommand.Connection = con
                    da.SelectCommand.CommandText = "select 0 as JurisdictionGroupID, '<Select>' as JurisdictionGroup union all select JurisdictionGroupID, JurisdictionGroup from jurisdictionGroups"
                    Dim dt As New DataTable
                    da.Fill(dt)
                    ddlJurisGroup.DataSource = dt
                    ddlJurisGroup.DataValueField = "JurisdictionGroupID"
                    ddlJurisGroup.DataTextField = "JurisdictionGroup"
                    ddlJurisGroup.DataBind()

                    'if the jurisGroup is a valid opt, then select the group from the ddl
                    If Not IsNothing(jurisGroup) And jurisGroup <> 0 Then
                        ddlJurisGroup.SelectedValue = jurisGroup
                    End If
                End If

                Session.Add("jurisGroup", jurisGroup)

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
            'get jurisdiction type ID to edit
            Dim jurisGroup As Integer
            jurisGroup = Session("jurisGroup")

            'get jurisdiction type ID to edit
            Dim editJurisTypeID As Integer
            editJurisTypeID = Session("editJurisTypeID")

            'make sure required fields are filled out
            If Trim(txtJurisType.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP21")
                Exit Sub
            End If
            If (editJurisTypeID = 0 And ddlJurisGroup.SelectedValue = "0") Or (jurisGroup <> 0 And ddlJurisGroup.SelectedValue = "0") Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP130")
                Exit Sub
            End If

            'If Not common.ValidateNoSpecialChar(Trim(txtJurisType.Text)) Then
            '    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP94")
            '    Exit Sub
            'End If

            'remove any single quotes from fields
            txtJurisType.Text = txtJurisType.Text.Replace("'", "''")
            txtDescription.Text = txtDescription.Text.Replace("'", "''")
            txtNotes.Text = txtNotes.Text.Replace("'", "''")

            'if jurisType is new or existing one except shcool division
            If editJurisTypeID = 0 Or jurisGroup <> 0 Then
                jurisGroup = ddlJurisGroup.SelectedValue
            End If

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            'check if the jurisdiction type already exists
            Dim dr As SqlClient.SqlDataReader
            query.CommandText = "select jurisdictionType from jurisdictiontypes where jurisdictionType='" & Trim(txtJurisType.Text) & "' AND jurisdictionTypeID <> " & editJurisTypeID.ToString
            dr = query.ExecuteReader()

            If dr.Read() Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP117")
                dr.Close()
                con.Close()
                Exit Sub
            End If

            'build add/update string
            dr.Close()
            If editJurisTypeID = 0 Then
                query.CommandText = "insert into jurisdictiontypes select max(jurisdictionTypeID) + 1,'" & Trim(txtJurisType.Text) & "','" & Trim(txtDescription.Text) & "','" & Trim(txtNotes.Text) & "'," & jurisGroup & " from jurisdictiontypes"
            Else
                query.CommandText = "update jurisdictiontypes set jurisdictionType = '" & Trim(txtJurisType.Text) & "', description = '" & Trim(txtDescription.Text) & "', notes = '" & Trim(txtNotes.Text) & "', jurisdictionGroupID = " & jurisGroup & " where jurisdictionTypeID = '" & editJurisTypeID & "'"
            End If
            query.ExecuteNonQuery()

            'clean up
            con.Close()
            Session.Remove("editJurisTypeID")
            Session.Remove("jurisGroup")
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewjuristype.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("editJurisTypeID")
        Session.Remove("jurisGroup")
        Response.Redirect("viewjuristype.aspx")
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("editJurisTypeID"), lblTitle)
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