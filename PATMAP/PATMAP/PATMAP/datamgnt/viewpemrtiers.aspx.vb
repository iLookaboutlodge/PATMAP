Partial Public Class viewpemrtiers
    Inherits System.Web.UI.Page

    Private tableCopied As Boolean = False
    Private originalDataTable As DataTable
    Private minAssessment, maxAssessment As Integer
    Private maxAssessmentText As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim helpButtons As ArrayList

        Try
            Master.errorMsg = ""

            If Not IsPostBack Then
                subMenu.setStartNode(menu.editData)

                'Set text for help buttons.
                helpButtons = New ArrayList
                helpButtons.Add(ibTiersHelp)
                Master.HelpButtons = helpButtons
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    'Assessment Tier header should span two columns.
    Private Sub grdTiers_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdTiers.PreRender
        If grdTiers.Rows.Count > 0 Then
            grdTiers.HeaderRow.Cells(3).Attributes.Add("colspan", "2")
            grdTiers.HeaderRow.Cells.RemoveAt(4)
        End If
    End Sub

    'Save a copy of the original values so that we can compare them later when updating.
    Private Sub grdTiers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTiers.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Not tableCopied Then
                originalDataTable = CType(e.Row.DataItem, System.Data.DataRowView).Row.Table.Copy()
                ViewState("originalValues") = originalDataTable
                tableCopied = True
            End If
        End If
    End Sub

    Private Sub grdTiers_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles grdTiers.RowUpdating
        e.NewValues("minAssessment") = minAssessment
        e.NewValues("maxAssessment") = maxAssessment
        e.NewValues("maxAssessmentText") = maxAssessmentText
    End Sub

    Private Sub ibSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibSaveTop.Click, ibSaveBottom.Click
        Dim mainTaxClassID As Integer
        Dim tier As Integer
        Dim originalRow As DataRow

        Try
            If Page.IsValid Then
                originalDataTable = CType(ViewState("originalValues"), DataTable)

                'Save only those rows that have changed.
                For Each row As GridViewRow In grdTiers.Rows
                    'Get primary key field for the row.
                    mainTaxClassID = CInt(grdTiers.DataKeys(row.RowIndex).Item(0))
                    tier = grdTiers.DataKeys(row.RowIndex).Item(1).ToString

                    'Get what the original values were for the row.
                    originalRow = originalDataTable.Select(String.Format("mainTaxClassID = '{0}' AND tier = {1}", mainTaxClassID, tier))(0)

                    If IsRowModified(row, originalRow) Then
                        grdTiers.EditIndex = row.RowIndex
                        grdTiers.UpdateRow(row.RowIndex, False)
                    End If
                Next

                'Rebind the grid to repopulate the original values table.
                tableCopied = False
                grdTiers.DataBind()
            End If
        Catch ex As Exception
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ibCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibCancelTop.Click, ibCancelBottom.Click
        Response.Redirect("viewpemrtiers.aspx")
    End Sub

    Protected Sub ValidateTiers(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim rowIndex As Integer
        Dim active As Boolean
        Dim tier As Integer
        Dim activeCount As Integer
        Dim minAssessment, maxAssessment As String
        Dim previousMinAssessment As Integer

        'Iterate backwards over every row in the grid.
        For rowIndex = grdTiers.Rows.Count - 1 To 0 Step -1
            active = CType(grdTiers.Rows(rowIndex).FindControl("cbActive"), CheckBox).Checked
            tier = CInt(CType(grdTiers.Rows(rowIndex).FindControl("lblTier"), Label).Text)

            If active Then
                activeCount += 1
                minAssessment = CType(grdTiers.Rows(rowIndex).FindControl("txtMinAssessment"), TextBox).Text
                maxAssessment = CType(grdTiers.Rows(rowIndex).FindControl("txtMaxAssessment"), TextBox).Text

                'Check that only valid characters have been entered for the assessment tier parameters.
                'This will also ensure that both parameters have values.
                If Not IsNumeric(minAssessment) Or Not IsNumeric(Replace(maxAssessment, ">", "")) Then
                    Master.errorMsg = common.GetErrorMessage("PATMAP72")
                    args.IsValid = False

                    Exit For
                End If

                'The minimum assessment tier parameter cannot be greater than the maximum assessment tier parameter.
                If CInt(minAssessment) > CInt(Replace(maxAssessment, ">", "")) Then
                    Master.errorMsg = common.GetErrorMessage("PATMAP156")
                    args.IsValid = False

                    Exit For
                End If

                'Gaps are not allowed between active tiers of a tax class.
                If activeCount > 1 AndAlso CInt(Replace(maxAssessment, ">", "")) <> previousMinAssessment - 1 Then
                    Master.errorMsg = common.GetErrorMessage("PATMAP157")
                    args.IsValid = False

                    Exit For
                Else
                    previousMinAssessment = CInt(minAssessment)
                End If

                If activeCount = 1 Then 'This is the last active tier for a tax class, as we are moving backwards through the tiers.
                    'If there is no > for the last active value of a tax class, then add a >.
                    If InStr(maxAssessment, ">") <= 0 Then
                        CType(grdTiers.Rows(rowIndex).FindControl("txtMaxAssessment"), TextBox).Text = maxAssessment & " >"
                    End If
                Else
                    'Remove > if the tier is not the last tier in a tax class.
                    If InStr(maxAssessment, ">") > 0 Then
                        CType(grdTiers.Rows(rowIndex).FindControl("txtMaxAssessment"), TextBox).Text = Replace(maxAssessment, ">", "")
                    End If
                End If

                'The addition or removal of tiers must be done in succession.
                If tier > 1 Then    'Not applicable to first tier.                    
                    'Previous row must also be active.
                    If Not CType(grdTiers.Rows(rowIndex - 1).FindControl("cbActive"), CheckBox).Checked Then
                        Master.errorMsg = common.GetErrorMessage("PATMAP153")
                        args.IsValid = False

                        Exit For
                    End If
                Else
                    If activeCount = 1 Then 'Tier 1 is the only active tier.
                        'Check if parameters are 0 - 0>.
                        minAssessment = CType(grdTiers.Rows(rowIndex).FindControl("txtMinAssessment"), TextBox).Text
                        maxAssessment = Replace(CType(grdTiers.Rows(rowIndex).FindControl("txtMaxAssessment"), TextBox).Text, " ", "")

                        If minAssessment <> "0" Or maxAssessment <> "0>" Then
                            Master.errorMsg = common.GetErrorMessage("PATMAP154")
                            args.IsValid = False

                            Exit For
                        End If
                    End If
                End If
            Else
                'All classes must have at least one active assessment tier.
                If tier = 1 Then
                    Master.errorMsg = common.GetErrorMessage("PATMAP155")
                    args.IsValid = False

                    Exit For
                End If
            End If

            'Reset tax class specific counters.
            If tier = 1 Then
                activeCount = 0
                previousMinAssessment = 0
            End If
        Next
    End Sub

    'Compare the current row to the corresponding row in the saved DataTable object, and return true if the row has changed.
    Protected Function IsRowModified(ByVal currentRow As GridViewRow, ByVal originalRow As DataRow) As Boolean
        Dim active As Boolean
        Dim txtMaxAssessment As TextBox

        active = CType(currentRow.FindControl("cbActive"), CheckBox).Checked
        minAssessment = CInt(CType(currentRow.FindControl("txtMinAssessment"), TextBox).Text)
        maxAssessment = 0.0
        maxAssessmentText = ""

        'Handle maxAssessment separately as it is a special case that may contain the > character.
        txtMaxAssessment = CType(currentRow.FindControl("txtMaxAssessment"), TextBox)

        If InStr(txtMaxAssessment.Text, ">") > 0 Then
            maxAssessment = CInt(Replace(txtMaxAssessment.Text, ">", ""))
            maxAssessmentText = ">"
        Else
            maxAssessment = CInt(txtMaxAssessment.Text)
            maxAssessmentText = ""
        End If

        'Check if any of the values have changed.
        If Not active.Equals(CBool(originalRow("active"))) Then Return True
        If Not minAssessment.Equals(CInt(originalRow("minAssessment"))) Then Return True
        If Not maxAssessment.Equals(CInt(originalRow("maxAssessment"))) Then Return True
        If Not maxAssessmentText.Equals(CStr(originalRow("maxAssessmentText"))) Then Return True

        Return False
    End Function
End Class