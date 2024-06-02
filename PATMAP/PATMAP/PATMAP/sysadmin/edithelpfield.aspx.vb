Public Partial Class edithelpfield
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.helpText)

                'get Help screen ID
                Dim editHelpControlID As Integer
                editHelpControlID = Session("editHelpControlID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader
                con.Open()

                'fill the sections drop down
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select 0 as sectionID, '<Select>' as sectionName union select sectionID, sectionName from sections"
                Dim dt As New DataTable
                da.Fill(dt)
                ddlSection.DataSource = dt
                ddlSection.DataValueField = "sectionID"
                ddlSection.DataTextField = "sectionName"
                ddlSection.DataBind()

                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                If editHelpControlID <> 0 Then
                    da.SelectCommand.CommandText = "select screenNameID, screenName from screennames"
                Else
                    da.SelectCommand.CommandText = "select 0 as screenNameID,'<Select>' as screenName"
                End If
                dt = New DataTable
                da.Fill(dt)
                ddlScreen.DataSource = dt
                ddlScreen.DataValueField = "screenNameID"
                ddlScreen.DataTextField = "screenName"
                ddlScreen.DataBind()

                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                If editHelpControlID <> 0 Then
                    da.SelectCommand.CommandText = "select controlTypeID, controlType from controltypes"
                Else
                    da.SelectCommand.CommandText = "select 0 as controlTypeID,'<Select>' as controlType"
                End If
                dt = New DataTable
                da.Fill(dt)
                ddlType.DataSource = dt
                ddlType.DataValueField = "controlTypeID"
                ddlType.DataTextField = "controlType"
                ddlType.DataBind()

                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                If editHelpControlID <> 0 Then
                    da.SelectCommand.CommandText = "select controlID, controlName from controls"
                Else
                    da.SelectCommand.CommandText = "select 0 as controlID,'<Select>' as controlName"
                End If
                dt = New DataTable
                da.Fill(dt)
                ddlFieldName.DataSource = dt
                ddlFieldName.DataValueField = "controlID"
                ddlFieldName.DataTextField = "controlName"
                ddlFieldName.DataBind()

                If editHelpControlID <> 0 Then
                    'don't allow user to change the section and screen name dropdown



                    'get help screen details from database
                    query.CommandText = "select sectionID, screenNameID, controlTypeID, helpText, description, notes from helpcontrols inner join controls on helpcontrols.controlID = controls.controlID inner join screennames on screennames.screenNameID = controls.screenID where helpcontrols.controlID = " & editHelpControlID
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill user detail into first section form fields
                    ddlSection.SelectedValue = dr.GetValue(0)
                    ddlScreen.SelectedValue = dr.GetValue(1)
                    ddlType.SelectedValue = dr.GetValue(2)
                    ddlFieldName.SelectedValue = editHelpControlID

                    If Not IsDBNull(dr.GetValue(3)) Then
                        ftbHelpText.Text = Server.HtmlDecode(dr.GetValue(3))
                    End If

                    If Not IsDBNull(dr.GetValue(4)) Then
                        txtDescription.Text = Server.HtmlDecode(dr.GetValue(4))
                    End If

                    If Not IsDBNull(dr.GetValue(5)) Then
                        txtNotes.Text = Server.HtmlDecode(dr.GetValue(5))
                    End If


                    'cleanup
                    dr.Close()

                End If

                'check if should show reset button
                query.CommandText = "select controlID from helpControlsReset where controlID = " & editHelpControlID
                dr = query.ExecuteReader
                If dr.Read() Then
                    btnReset.Visible = True
                Else
                    btnReset.Visible = False
                End If
                dr.Close()

                If editHelpControlID <> 0 Then
                    'don't allow user to change the section and screen name dropdown
                    ddlSection.Enabled = False
                    ddlScreen.Enabled = False
                    ddlType.Enabled = False
                    ddlFieldName.Enabled = False
                Else
                    ddlSection.Enabled = True
                    ddlScreen.Enabled = True
                    ddlType.Enabled = True
                    ddlFieldName.Enabled = True
                End If

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
            common.ChangeTitle(Session("editHelpControlID"), lblTitle)
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
        Session.Remove("editHelpControlID")
        Response.Redirect("viewhelpfield.aspx")

    End Sub

    Protected Sub ddlSection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSection.SelectedIndexChanged
        Try
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString

            If ddlSection.SelectedValue <> 0 Then

                'get screen name for drop down
                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as screenNameID,'<Select>' as screenName union select screennames.screenNameID,screennames.screenName from controls inner join screennames on screennames.screenNameID = controls.screenID where controlID not in (select controlID from helpcontrols) and screennames.sectionID = " & ddlSection.SelectedValue & " group by screennames.screenNameID,screennames.screenName", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlScreen.DataSource = dt
                ddlScreen.DataValueField = "screenNameID"
                ddlScreen.DataTextField = "screenName"
                ddlScreen.DataBind()

                'get type for drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlTypeID,'<Select>' as controlType", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlType.DataSource = dt
                ddlType.DataValueField = "controlTypeID"
                ddlType.DataTextField = "controlType"
                ddlType.DataBind()

                'get sections for drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlNameID,'<Select>' as controlName", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlFieldName.DataSource = dt
                ddlFieldName.DataValueField = "controlNameID"
                ddlFieldName.DataTextField = "controlName"
                ddlFieldName.DataBind()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Protected Sub ddlScreen_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlScreen.SelectedIndexChanged
        Try
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString

            If ddlScreen.SelectedValue <> 0 Then

                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable

                'get type for drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlTypeID,'<Select>' as controlType union select controltypes.controlTypeID, controltypes.controlType from controls inner join screennames on screennames.screenNameID = controls.screenID inner join controltypes on controls.controlTypeID = controltypes.controlTypeID where controlID not in (select controlID from helpcontrols) and screenNames.sectionID = " & ddlSection.SelectedValue & " and controls.screenID = " & ddlScreen.SelectedValue & " group by controltypes.controlTypeID, controltypes.controlType", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlType.DataSource = dt
                ddlType.DataValueField = "controlTypeID"
                ddlType.DataTextField = "controlType"
                ddlType.DataBind()

                'get sections for drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlNameID,'<Select>' as controlName", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlFieldName.DataSource = dt
                ddlFieldName.DataValueField = "controlNameID"
                ddlFieldName.DataTextField = "controlName"
                ddlFieldName.DataBind()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        Try
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString

            If ddlType.SelectedValue <> 0 Then

                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable

                'get sections for drop down
                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select 0 as controlID,'<Select>' as controlName union select controls.controlID, controlName from controls inner join screennames on screennames.screenNameID = controls.screenID where controlID not in (select controlID from helpcontrols) and screennames.sectionID = " & ddlSection.SelectedValue & " and controls.screenID = " & ddlScreen.SelectedValue & " and controls.controlTypeID = " & ddlType.SelectedValue & " group by controls.controlID, controlName", con)
                dt = New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlFieldName.DataSource = dt
                ddlFieldName.DataValueField = "controlID"
                ddlFieldName.DataTextField = "controlName"
                ddlFieldName.DataBind()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReset.Click
        Try
            'get Help screen ID
            Dim editHelpControlID As Integer
            editHelpControlID = Session("editHelpControlID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            Dim dr As SqlClient.SqlDataReader

            'get help screen details from database
            con.Open()
            query.CommandText = "select description, notes, helpText from helpcontrolsReset where controlID = " & editHelpControlID
            dr = query.ExecuteReader
            dr.Read()

            'fill user detail into first section form fields
            txtDescription.Text = Server.HtmlDecode(dr.GetValue(0))
            txtNotes.Text = Server.HtmlDecode(dr.GetValue(1))
            ftbHelpText.Text = Server.HtmlDecode(dr.GetValue(2))

            'cleanup
            dr.Close()
            con.Close()
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
            If String.IsNullOrEmpty(ddlType.SelectedValue) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP31")
                Exit Sub
            End If
            If String.IsNullOrEmpty(ddlFieldName.SelectedValue) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP32")
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
            If ddlType.SelectedValue = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP31")
                Exit Sub
            End If
            If ddlFieldName.SelectedValue = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP32")
                Exit Sub
            End If

            'remove any single quotes from fields
            txtDescription.Text = Server.HtmlEncode(Trim(txtDescription.Text.Replace("'", "''")))
            txtNotes.Text = Server.HtmlEncode(Trim(txtNotes.Text.Replace("'", "''")))
            ftbHelpText.Text = Server.HtmlEncode(ftbHelpText.Text.Replace("'", "''"))

            'get Help screen ID
            Dim editHelpControlID As Integer
            editHelpControlID = Session("editHelpControlID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            'save the help screen details
            If editHelpControlID = 0 Then
                'insert help screen 
                query.CommandText = "insert into helpControls (controlID, helpText, description, notes) values (" & ddlFieldName.SelectedValue & ",'" & ftbHelpText.Text & "','" & Trim(txtDescription.Text) & "','" & Trim(txtNotes.Text) & "')" & vbCrLf
            Else
                'update help screen
                query.CommandText = "update helpControls set description = '" & Trim(txtDescription.Text) & "', notes = '" & Trim(txtNotes.Text) & "', helpText = '" & ftbHelpText.Text & "' where controlID = " & editHelpControlID & vbCrLf
            End If

            'check if this has been select to be a restore point
            If ckbReset.Checked = True Then
                query.CommandText += "delete from helpControlsReset where controlID = " & ddlFieldName.SelectedValue & vbCrLf
                query.CommandText += "insert into helpControlsReset select * from helpControls where controlID = " & ddlFieldName.SelectedValue & vbCrLf
            End If

            If ckbAll.Checked = True Then
                query.CommandText += "update helpControls set description = '" & Trim(txtDescription.Text) & "', notes = '" & Trim(txtNotes.Text) & "', helpText = '" & ftbHelpText.Text & "' where controlID in (select controlID from controls where controlNameID in (select controlNameID from controls where controlid = " & ddlFieldName.SelectedValue & "))" & vbCrLf
                If ckbReset.Checked = True Then
                    query.CommandText += "delete from helpControlsReset where controlID in (select controlID from controls where controlNameID in (select controlNameID from controls where controlid = " & ddlFieldName.SelectedValue & ")"
                    query.CommandText += "insert into helpControlsReset select * from helpControls where controlID in (select controlID from controls where controlNameID in (select controlNameID from controls where controlid = " & ddlFieldName.SelectedValue & ")"
                End If
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
            Session.Remove("editHelpControlID")
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        'return to home page
        Response.Redirect("viewhelpfield.aspx")

    End Sub
End Class