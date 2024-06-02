Partial Public Class viewpemr
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim helpButtons As ArrayList

        Try
            Master.errorMsg = ""

            If Not IsPostBack Then
                subMenu.setStartNode(menu.editData)

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
                helpButtons.Add(ibDataSetHelp)
                helpButtons.Add(ibYearHelp)
                Master.HelpButtons = helpButtons
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ibSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibSearch.Click
        SearchPEMR()
    End Sub

    Private Sub SearchPEMR()
        Try
            grdPEMR.DataSourceID = "sdsPEMRDescriptions"
            grdPEMR.DataBind()

            If grdPEMR.Rows.Count = 0 Then
                lblTotal.Text = "PEMR: " & grdPEMR.Rows.Count
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdPEMR_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPEMR.RowDataBound
        Try
            common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "dataSetName"))

            If (e.Row.RowType = DataControlRowType.Footer) Then
                lblTotal.Text = "PEMR: " & grdPEMR.Rows.Count
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdPEMR_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grdPEMR.RowEditing
        Response.Redirect("editpemr.aspx?PEMRID=" & grdPEMR.DataKeys(e.NewEditIndex).Values("PEMRID").ToString)
    End Sub

    Private Sub grdPEMR_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdPEMR.RowDeleting
        sdsPEMRDescriptions.DeleteParameters("PEMRID").DefaultValue = grdPEMR.DataKeys(e.RowIndex).Values("PEMRID").ToString
    End Sub
End Class