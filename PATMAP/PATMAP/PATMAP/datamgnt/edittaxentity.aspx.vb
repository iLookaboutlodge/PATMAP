Public Partial Class edittaxentity
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            'check if its the first load or a post back
            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                'get entityID to edit
                Dim entityID As String
                entityID = Session("entityID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader
                con.Open()

                'fill the jurisdiction type list box
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select jurisdictionTypeID,jurisdictionType from jurisdictiontypes "
                Dim dt As New DataTable
                da.Fill(dt)
                ddlJurisType.DataSource = dt
                ddlJurisType.DataValueField = "jurisdictionTypeID"
                ddlJurisType.DataTextField = "jurisdictionType"
                ddlJurisType.DataBind()

                If entityID <> "-999A" Then
                    'get entity details from database
                    query.CommandText = "select number, jurisdiction, jurisdictionTypeID, revenue, expenditure, percentPublic, percentSeparate, notes from entities where number = '" & entityID & "'"
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill entity detail into the form fields
                    If Not IsDBNull(dr.GetValue(0)) Then
                        txtNumber.Text = dr.GetValue(0)
                    End If

                    If Not IsDBNull(dr.GetValue(1)) Then
                        txtJurisdiction.Text = dr.GetValue(1)
                    End If

                    Session.Add("jurisdiction", txtJurisdiction.Text)

                    ddlJurisType.SelectedValue = dr.GetValue(2)

                    If Not IsDBNull(dr.GetValue(3)) Then
                        txtRevenue.Text = dr.GetValue(3)
                    End If

                    If Not IsDBNull(dr.GetValue(4)) Then
                        txtExpenditure.Text = dr.GetValue(4)
                    End If

                    If Not IsDBNull(dr.GetValue(5)) Then
                        txtPublic.Text = dr.GetValue(5)
                    End If

                    If Not IsDBNull(dr.GetValue(6)) Then
                        txtSeparate.Text = dr.GetValue(6)
                    End If

                    If Not IsDBNull(dr.GetValue(7)) Then
                        txtNotes.Text = dr.GetValue(7)
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

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("entityID")
        Session.Remove("jurisdiction")
        Response.Redirect("viewtaxentity.aspx")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            'make sure required fields are filled out
            If Trim(txtJurisdiction.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP19")
                Exit Sub
            End If
            If Trim(txtNumber.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP20")
                Exit Sub
            End If

            'remove any single quotes from fields
            txtJurisdiction.Text = txtJurisdiction.Text.Replace("'", "''")
            txtNumber.Text = txtNumber.Text.Replace("'", "''")
            txtNotes.Text = txtNotes.Text.Replace("'", "''")

            'fill in some default values
            If Trim(txtRevenue.Text) = "" Then
                txtRevenue.Text = 0
            End If
            If Trim(txtExpenditure.Text) = "" Then
                txtExpenditure.Text = 0
            End If
            If Trim(txtPublic.Text) = "" Then
                txtPublic.Text = 0
            End If
            If Trim(txtSeparate.Text) = "" Then
                txtSeparate.Text = 0
            End If

            'get groupID to edit
            Dim entityID As String
            entityID = Session("entityID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()
            Dim dReader As SqlClient.SqlDataReader

            'check if number is uniqe within the same jurisType
            query.CommandText = "select count(number) from entities where number='" & Trim(txtNumber.Text) & "' and jurisdictionTypeID=" & ddlJurisType.SelectedValue
            dReader = query.ExecuteReader()
            dReader.Read()
            If entityID <> Trim(txtNumber.Text) Then
                If dReader.GetValue(0) >= 1 Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP128")
                    dReader.Close()
                    con.Close()
                    Exit Sub
                End If
                dReader.Close()
            Else
                If dReader.GetValue(0) > 1 Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP128")
                    dReader.Close()
                    con.Close()
                    Exit Sub
                End If
                dReader.Close()
            End If

            'check if the jurisdiction is uniqe within the same jurisType
            query.CommandText = "select count(number) from entities where jurisdictionTypeID=" & ddlJurisType.SelectedValue & " and jurisdiction='" & Trim(txtJurisdiction.Text) & "'"
            dReader = query.ExecuteReader()
            dReader.Read()
            If Session("jurisdiction") <> Trim(txtJurisdiction.Text) Then
                If dReader.GetValue(0) >= 1 Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP129")
                    dReader.Close()
                    con.Close()
                    Exit Sub
                End If
                dReader.Close()
            Else
                If dReader.GetValue(0) > 1 Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP129")
                    dReader.Close()
                    con.Close()
                    Exit Sub
                End If
                dReader.Close()
            End If

            'build add/update string
            If entityID = "-999A" Then
                query.CommandText = "insert into entities (number, jurisdiction, jurisdictionTypeID, revenue, expenditure, percentPublic, percentSeparate, notes) values ('" & Trim(txtNumber.Text) & "','" & Trim(txtJurisdiction.Text) & "'," & ddlJurisType.SelectedValue & "," & Trim(txtRevenue.Text) & "," & Trim(txtExpenditure.Text) & "," & Trim(txtPublic.Text) & "," & Trim(txtSeparate.Text) & ",'" & Trim(txtNotes.Text) & "')"
            Else
                query.CommandText = "update entities set number = '" & Trim(txtNumber.Text) & "', jurisdiction = '" & Trim(txtJurisdiction.Text) & "', jurisdictionTypeID = " & ddlJurisType.SelectedValue & ", revenue = " & Trim(txtRevenue.Text) & ", expenditure = " & Trim(txtExpenditure.Text) & ", percentPublic = " & Trim(txtPublic.Text) & ", percentSeparate = " & Trim(txtSeparate.Text) & ", notes = '" & Trim(txtNotes.Text) & "' where number = '" & entityID & "'"
            End If

            query.ExecuteNonQuery()

            'clean up
            con.Close()
            Session.Remove("entityID")
            Session.Remove("jurisdiction")

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewtaxentity.aspx")

    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("entityID"), lblTitle)
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