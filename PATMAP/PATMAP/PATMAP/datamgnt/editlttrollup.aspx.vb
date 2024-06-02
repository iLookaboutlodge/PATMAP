Partial Public Class editlttrollup
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
                Dim editRollupClassID As Integer
                editRollupClassID = Session("editRollupClassID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader
                con.Open()

                '' '' '' '' ''Dim jurisGroup As Integer
                If editRollupClassID <> 0 Then
                    'get rollup class details from database
                    query.CommandText = "select taxClass, description, from LTTmainTaxClasses where taxClassID = " & editRollupClassID
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill ltt rollup details into the form fields
                    txtRollupName.Text = dr.GetValue(0)
                    If Not dr.IsDBNull(1) Then
                        txtDescription.Text = dr.GetValue(1)
                    Else
                        txtDescription.Text = ""
                    End If

                    'cleanup
                    dr.Close()
                End If

                ' '' '' '' '' ''get jurisdiction group info from the database (for new or existing jurisType)
                ' '' '' '' '' ''if juris = school division, then no need for displaying jurisGroup
                '' '' '' '' ''If editJurisTypeID = 1 Then
                '' '' '' '' ''    ddlJurisGroup.Visible = False
                '' '' '' '' ''    lblNoGroup.Visible = True
                '' '' '' '' ''Else
                '' '' '' '' ''    'first bind the list of jurisGroups to the ddl
                '' '' '' '' ''    Dim da As New SqlClient.SqlDataAdapter
                '' '' '' '' ''    da.SelectCommand = New SqlClient.SqlCommand
                '' '' '' '' ''    da.SelectCommand.Connection = con
                '' '' '' '' ''    da.SelectCommand.CommandText = "select 0 as JurisdictionGroupID, '<Select>' as JurisdictionGroup union all select JurisdictionGroupID, JurisdictionGroup from jurisdictionGroups"
                '' '' '' '' ''    Dim dt As New DataTable
                '' '' '' '' ''    da.Fill(dt)
                '' '' '' '' ''    ddlJurisGroup.DataSource = dt
                '' '' '' '' ''    ddlJurisGroup.DataValueField = "JurisdictionGroupID"
                '' '' '' '' ''    ddlJurisGroup.DataTextField = "JurisdictionGroup"
                '' '' '' '' ''    ddlJurisGroup.DataBind()

                '' '' '' '' ''    'if the jurisGroup is a valid opt, then select the group from the ddl
                '' '' '' '' ''    If Not IsNothing(jurisGroup) And jurisGroup <> 0 Then
                '' '' '' '' ''        ddlJurisGroup.SelectedValue = jurisGroup
                '' '' '' '' ''    End If
                '' '' '' '' ''End If

                '' '' '' '' ''Session.Add("jurisGroup", jurisGroup)

                ' '' '' '' '' ''clean up
                '' '' '' '' ''con.Close()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            ' '' '' '' '' ''get jurisdiction type ID to edit
            '' '' '' '' ''Dim jurisGroup As Integer
            '' '' '' '' ''jurisGroup = Session("jurisGroup")

            'get LTT rollup taxClass ID to edit
            Dim editRollupClassID As Integer
            editRollupClassID = Session("editRollupClassID")

            'make sure required fields are filled out
            If Trim(txtRollupName.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP159")
                Exit Sub
            End If
            '' '' '' '' ''If (editJurisTypeID = 0 And ddlJurisGroup.SelectedValue = "0") Or (jurisGroup <> 0 And ddlJurisGroup.SelectedValue = "0") Then
            '' '' '' '' ''    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP130")
            '' '' '' '' ''    Exit Sub
            '' '' '' '' ''End If

            'If Not common.ValidateNoSpecialChar(Trim(txtJurisType.Text)) Then
            '    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP94")
            '    Exit Sub
            'End If

            'remove any single quotes from fields
            txtRollupName.Text = txtRollupName.Text.Replace("'", "''")
            txtDescription.Text = txtDescription.Text.Replace("'", "''")
            '' '' '' '' ''txtNotes.Text = txtNotes.Text.Replace("'", "''")

            ' '' '' '' '' ''if jurisType is new or existing one except shcool division
            '' '' '' '' ''If editJurisTypeID = 0 Or jurisGroup <> 0 Then
            '' '' '' '' ''    jurisGroup = ddlJurisGroup.SelectedValue
            '' '' '' '' ''End If

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            'check if the Rollup class already exists
            Dim reactivateRollup As Boolean = False
            Dim dr As SqlClient.SqlDataReader
            query.CommandText = "select taxclassID, active from LTTmainTaxClasses where taxclass ='" & Trim(txtRollupName.Text) & "' AND taxClassID <> '" & editRollupClassID.ToString & "'"
            dr = query.ExecuteReader()

            If dr.Read() Then
                If dr.GetValue(1) <> 0 Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP160")
                    dr.Close()
                    con.Close()
                    Exit Sub
                Else
                    editRollupClassID = dr.GetValue(0)
                    'reactivateRollup = True
                End If
            End If

            'build add/update string
            dr.Close()
            If editRollupClassID = 0 Then
                query.CommandText = "insert into LTTmainTaxClasses select max(taxClassID) + 1,'" & Trim(txtRollupName.Text) & "','" & Trim(txtDescription.Text) & "','1' from LTTmainTaxClasses"
            Else
                query.CommandText = "update LTTmainTaxClasses set taxClass = '" & Trim(txtRollupName.Text) & "', description = '" & Trim(txtDescription.Text) & "', active = '1' where taxClassID = '" & editRollupClassID & "'"
            End If
            query.ExecuteNonQuery()

            'clean up
            con.Close()
            Session.Remove("editRollupClassID")
            '' '' '' '' ''Session.Remove("jurisGroup")
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewlttrollup.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("editRollupClassID")
        '' '' '' '' ''Session.Remove("jurisGroup")
        Response.Redirect("viewlttrollup.aspx")
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("editRollupClassID"), lblTitle)
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