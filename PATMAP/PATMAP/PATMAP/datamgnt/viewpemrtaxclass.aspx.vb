Public Partial Class viewpemrtaxclass
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)
                grdTaxCLass.PageSize = PATMAP.Global_asax.pageSize

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString

                'get main tax classes for drop down
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select mainTaxClassID, mainTaxClass from PEMRMainTaxClasses", con)
                Dim dt As New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlTaxClass.DataSource = dt
                ddlTaxClass.DataValueField = "mainTaxClassID"
                ddlTaxClass.DataTextField = "mainTaxClass"
                ddlTaxClass.DataBind()
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        PerformTaxClassSearch()
    End Sub

    Private Sub PerformTaxClassSearch()
        Try
            grdTaxClass.DataSourceID = "sdsPEMRMainTaxClasses"
            grdTaxClass.DataBind()

            If grdTaxClass.Rows.Count = 0 Then
                lblTotal.Text = "PEMR Tax Classes: " & grdTaxClass.Rows.Count
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxClass_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTaxClass.RowDataBound
        Dim ibDelete As ImageButton
        Dim rowView As DataRowView
        Dim mainTaxClass As String

        If e.Row.RowType = DataControlRowType.DataRow Then
            ibDelete = CType(e.Row.FindControl("ibDelete"), ImageButton)

            'Add confirmation to delete button.
            If ibDelete.Visible Then
                rowView = CType(e.Row.DataItem, DataRowView)
                mainTaxClass = rowView("mainTaxClass").ToString()
                ibDelete.OnClientClick = String.Format("return confirmPrompt('Are you sure you want to delete {0}?');", Replace(mainTaxClass, "'", "\'"))
            End If
        End If
    End Sub

    Private Sub grdTaxClass_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grdTaxClass.RowEditing
        Response.Redirect("editpemrtaxclass.aspx?mainTaxClassID=" & grdTaxClass.DataKeys(e.NewEditIndex).Values("mainTaxClassID").ToString)
    End Sub

    Private Sub grdTaxClass_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdTaxClass.RowDeleting
        sdsPEMRMainTaxClasses.DeleteParameters("mainTaxClassID").DefaultValue = grdTaxClass.DataKeys(e.RowIndex).Values("mainTaxClassID").ToString
    End Sub
End Class