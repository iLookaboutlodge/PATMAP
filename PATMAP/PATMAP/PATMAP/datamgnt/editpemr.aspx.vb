Partial Public Class editpemr
    Inherits System.Web.UI.Page

    Private tableCopied As Boolean = False
    Private originalDataTable As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim helpButtons As ArrayList

        Try
            Master.errorMsg = ""

            If Not IsPostBack Then
                subMenu.setStartNode(menu.editData)

                'Handle populating Year dropdown.
                ddlYear.DataSource = common.GetYears()
                ddlYear.DataBind()

                GetDataSetInfo()

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

    'Assessment Tier header should span two columns.
    Private Sub grdPEMR_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdPEMR.PreRender
        If grdPEMR.Rows.Count > 0 Then
            grdPEMR.HeaderRow.Cells(2).Attributes.Add("colspan", "2")
            grdPEMR.HeaderRow.Cells.RemoveAt(3)
        End If
    End Sub

    Private Sub GetDataSetInfo()
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand()
        Dim dr As SqlClient.SqlDataReader = Nothing

        Try
            con.ConnectionString = PATMAP.Global_asax.connString
            cmd.Connection = con
            cmd.CommandText = "SELECT dataSetName, year FROM PEMRDescription WHERE PEMRID = @PEMRID"
            cmd.Parameters.AddWithValue("@PEMRID", Request.QueryString("PEMRID"))
            con.Open()

            dr = cmd.ExecuteReader()

            If dr.Read() Then
                If Not IsDBNull(dr.Item(0)) Then
                    txtDataSet.Text = dr.Item(0)
                End If

                If Not IsDBNull(dr.Item(1)) Then
                    ddlYear.SelectedValue = dr.Item(1)
                End If
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        Finally
            dr.Close()
            con.Close()
        End Try
    End Sub

    Private Sub ibCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibCancel.Click
        Response.Redirect("viewpemr.aspx")
    End Sub

    'Save a copy of the original values so that we can compare them later when updating.
    Private Sub grdPEMR_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPEMR.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Not tableCopied Then
                originalDataTable = CType(e.Row.DataItem, System.Data.DataRowView).Row.Table.Copy()
                ViewState("originalValues") = originalDataTable
                tableCopied = True
            End If
        End If
    End Sub

    Private Sub ibSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibSave.Click
        Dim mainTaxClassID As Integer
        Dim tier As Integer
        Dim originalRow As DataRow

        Try
            If Page.IsValid Then
                originalDataTable = CType(ViewState("originalValues"), DataTable)

                'Save only those rows that have changed.
                For Each row As GridViewRow In grdPEMR.Rows
                    'Get primary key field for the row.
                    mainTaxClassID = CInt(grdPEMR.DataKeys(row.RowIndex).Item(0))
                    tier = CType(row.FindControl("lblTier"), Label).Text

                    'Get what the original values were for the row.
                    originalRow = originalDataTable.Select(String.Format("mainTaxClassID = '{0}' AND tier = {1}", mainTaxClassID, tier))(0)

                    If IsRowModified(row, originalRow) Then
                        grdPEMR.EditIndex = row.RowIndex
                        grdPEMR.UpdateRow(row.RowIndex, False)
                    End If
                Next

                Response.Redirect("viewpemr.aspx")
            End If
        Catch ex As Exception
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    'Validate mill rates.
    Protected Sub ValidateMillRate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim millRate As String

        For Each row As GridViewRow In grdPEMR.Rows
            millRate = CType(row.FindControl("txtMillRate"), TextBox).Text

            If Not IsNumeric(millRate) Then
                Master.errorMsg = common.GetErrorMessage("PATMAP72")
                args.IsValid = False

                Exit Sub
            End If
        Next
    End Sub

    'Compare the current row to the corresponding row in the saved DataTable object, and return true if the row has changed.
    Protected Function IsRowModified(ByVal currentRow As GridViewRow, ByVal originalRow As DataRow) As Boolean
        Dim millRate As Double

        'Mill Rate field.
        millRate = CDbl(CType(currentRow.FindControl("txtMillRate"), TextBox).Text)

        'Check if the Mill Rate value has changed.
        If Not millRate.Equals(CDbl(originalRow("PEMR"))) Then Return True

        Return False
    End Function
End Class