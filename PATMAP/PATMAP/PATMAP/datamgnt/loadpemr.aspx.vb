Public Partial Class loadpemr
    Inherits System.Web.UI.Page

    Private PEMRID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim helpButtons As ArrayList

        Try
            Master.errorMsg = ""

            If Not IsPostBack Then
                subMenu.setStartNode(menu.loadData)

                'Handle populating Year dropdown.
                ddlYear.DataSource = common.GetYears()
                ddlYear.DataBind()
                'Remove the first item so a proper default selection item can be added.
                ddlYear.Items.Remove(ddlYear.Items.FindByValue("<Select>"))
                'Add the default selection item with an empty string as its value.
                ddlYear.Items.Insert(0, New ListItem("<Select>", String.Empty))
                ddlYear.SelectedValue = String.Empty

                'Set text for help buttons.
                helpButtons = New ArrayList
                helpButtons.Add(ibNameHelp)
                helpButtons.Add(ibYearHelp)
                Master.HelpButtons = helpButtons
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    'Assessment Tier header should span two columns.
    Private Sub grdPEMR_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdPEMR.PreRender
        If grdPEMR.Rows.Count > 0 Then
            grdPEMR.HeaderRow.Cells(2).Attributes.Add("colspan", "2")
            grdPEMR.HeaderRow.Cells.RemoveAt(3)
        End If
    End Sub

    Private Sub grdPEMR_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles grdPEMR.RowUpdating
        e.NewValues("PEMRID") = PEMRID
        e.NewValues("PEMR") = CType(grdPEMR.Rows(e.RowIndex).FindControl("txtMillRate"), TextBox).Text
    End Sub

    Private Sub ibSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibSave.Click       
        Try
            If Page.IsValid Then
                PEMRID = AddDataSet()

                'Iterate through each row of the grid.
                For Each row As GridViewRow In grdPEMR.Rows
                    grdPEMR.EditIndex = row.RowIndex
                    grdPEMR.UpdateRow(row.RowIndex, False)
                Next

                'Clear fields for a new entry.
                txtDataSet.Text = ""
                ddlYear.SelectedValue = ""
            End If
        Catch ex As Exception
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub ValidatePage(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand()
        Dim millRate As String

        'Data Set Name and Year are required fields.
        If String.IsNullOrEmpty(Trim(txtDataSet.Text)) Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP135")
            args.IsValid = False

            Exit Sub
        End If

        If Not Common.ValidateNoSpecialChar(Trim(txtDataSet.Text)) Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP84")
            args.IsValid = False

            Exit Sub
        End If

        If ddlYear.SelectedValue = "" Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP86")
            args.IsValid = False

            Exit Sub
        End If

        'Check if this data set name already exists.
        Try
            con.ConnectionString = PATMAP.Global_asax.connString
            cmd.Connection = con
            cmd.CommandText = "PEMRDescriptionExists"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@dataSetName", txtDataSet.Text.Trim)
            con.Open()

            If CInt(cmd.ExecuteScalar()) = 1 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP113")
                args.IsValid = False

                Exit Sub
            End If
        Catch
            Master.errorMsg = Common.GetErrorMessage(Err.Number, Err)
        Finally
            con.Close()
        End Try

        'Validate mill rates.
        For Each row As GridViewRow In grdPEMR.Rows
            millrate = CType(row.FindControl("txtMillRate"), TextBox).Text

            If Not IsNumeric(millrate) Then
                Master.errorMsg = common.GetErrorMessage("PATMAP72")
                args.IsValid = False

                Exit Sub
            End If
        Next
    End Sub

    'Add the PEMR data set to the database and return the new PEMRID.
    Private Function AddDataSet() As Integer
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand()

        Try
            con.ConnectionString = PATMAP.Global_asax.connString
            cmd.Connection = con
            cmd.CommandText = "PEMRDescriptionInsert"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@dataSetName", txtDataSet.Text.Trim)
            cmd.Parameters.AddWithValue("@year", ddlYear.SelectedValue)
            con.Open()

            Return CInt(cmd.ExecuteScalar())
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        Finally
            con.Close()
        End Try
    End Function
End Class