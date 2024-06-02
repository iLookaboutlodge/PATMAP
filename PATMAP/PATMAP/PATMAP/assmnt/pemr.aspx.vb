Imports System.Data.SqlClient

Partial Public Class pemr
    Inherits System.Web.UI.Page

    Private tableCopied As Boolean = False
    Private originalDataTable As DataTable

    Protected Property EnterPEMR() As Boolean
        Get
            If ViewState("EnterPEMR") IsNot Nothing Then
                Return ViewState("EnterPEMR")
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("EnterPEMR") = value
        End Set
    End Property

    Protected Property PEMRByTotalLevy() As Nullable(Of Boolean)
        Get
            If ViewState("PEMRByTotalLevy") IsNot Nothing Then
                Return ViewState("PEMRByTotalLevy")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            ViewState("PEMRByTotalLevy") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim helpButtons As ArrayList

        Try
            Master.errorMsg = ""

            If Not IsPostBack Then
                subMenu.setStartNode(menu.assmnt)

                'Set text for help buttons.
                helpButtons = New ArrayList
                helpButtons.Add(ucHeader.ScenarioHelp)
                helpButtons.Add(ibMillRateHelp)
                helpButtons.Add(ibLevyHelp)
                Master.HelpButtons = helpButtons

                BindData()
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub sdsPEMR_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles sdsPEMR.Selecting
        e.Command.Parameters("@userID").Value = Session("userID")
        e.Command.Parameters("@levelID").Value = Session("levelID")
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

    'Assessment Tier header should span two columns.
    Private Sub grdPEMR_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdPEMR.PreRender
        If grdPEMR.Rows.Count > 0 Then
            grdPEMR.HeaderRow.Cells(2).Attributes.Add("colspan", "2")
            grdPEMR.HeaderRow.Cells.RemoveAt(3)
        End If
    End Sub

    Protected Sub MillRateOptionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        SetControlState()
    End Sub

    Private Sub ibSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibSubmit.Click
        Dim mainTaxClassID As Integer
        Dim tier As Integer
        Dim originalRow As DataRow
        Dim auditTrailText As String = String.Empty

        Try
            If Page.IsValid Then
                'Save radio button state.
                UpdateData()

                'Construct audit trail string.
                If EnterPEMR <> rbEnter.Checked Then
                    If rbEnter.Checked Then
                        auditTrailText = "[" & Date.Today.ToString("MM/dd/yyyy") & "]PEMR changed from Calculate Revenue Neutral Education Mill Rates to Enter / Edit Education Mill Rates" & vbCrLf
                    Else
                        auditTrailText = "[" & Date.Today.ToString("MM/dd/yyyy") & "]PEMR changed from Enter / Edit Education Mill Rates to Calculate Revenue Neutral Education Mill Rates" & vbCrLf

                        If PEMRByTotalLevy.HasValue Then
                            If PEMRByTotalLevy.Value <> rbTotalLevy.Checked Then
                                If rbTotalLevy.Checked Then
                                    auditTrailText &= "[" & Date.Today.ToString("MM/dd/yyyy") & "]PEMR changed from Revenue Neutral Rates by Class Levy to Revenue Neutral Rates by Total Levy" & vbCrLf
                                Else
                                    auditTrailText &= "[" & Date.Today.ToString("MM/dd/yyyy") & "]PEMR changed from Revenue Neutral Rates by Total Levy to Revenue Neutral Rates by Class Levy" & vbCrLf
                                End If
                            End If
                        End If
                    End If
                ElseIf PEMRByTotalLevy.HasValue Then
                    If PEMRByTotalLevy.Value <> rbTotalLevy.Checked Then
                        If rbTotalLevy.Checked Then
                            auditTrailText = "[" & Date.Today.ToString("MM/dd/yyyy") & "]PEMR changed from Revenue Neutral Rates by Class Levy to Revenue Neutral Rates by Total Levy" & vbCrLf
                        Else
                            auditTrailText = "[" & Date.Today.ToString("MM/dd/yyyy") & "]PEMR changed from Revenue Neutral Rates by Total Levy to Revenue Neutral Rates by Class Levy" & vbCrLf
                        End If
                    End If
                End If

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

                        'Construct audit trail string.
                        auditTrailText &= "[" & Date.Today.ToString("MM/dd/yyyy") & "]PEMR changed for " & CType(row.FindControl("lblTaxClass"), Label).Text & _
                            " Tier " & tier & " from " & String.Format("{0:##0.00}", originalRow.Item("subjectPEMR")) & vbCrLf
                    End If
                Next

                'Update audit trail.
                If Not String.IsNullOrEmpty(auditTrailText) Then
                    UpdateAuditTrail(auditTrailText)
                End If

            End If
        Catch ex As Exception
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
        Response.Clear()


        common.gotoNextPage(3, 48, Session("levelID"))
    End Sub

    'Zero out the Subject Year PEMR values.
    Private Sub ibClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibClear.Click
        Dim txtSubjectPEMR As TextBox

        For index As Integer = 0 To grdPEMR.Rows.Count - 1
            txtSubjectPEMR = CType(grdPEMR.Rows(index).FindControl("txtSubjectPEMR"), TextBox)

            If txtSubjectPEMR IsNot Nothing Then
                txtSubjectPEMR.Text = "0.00"
            End If
        Next
    End Sub

    'Initialize the radio button state.
    Private Sub BindData()
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim dr As SqlDataReader = Nothing
        Dim value As Object = Nothing

        Try
            con.ConnectionString = PATMAP.Global_asax.connString
            cmd.Connection = con
            cmd.CommandText = "SELECT enterPEMR, PEMRByTotalLevy FROM liveAssessmentTaxModel WHERE userID = @userID"
            cmd.Parameters.AddWithValue("@userID", Session("userID"))
            con.Open()

            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

            If dr.Read Then
                value = dr("enterPEMR")

                If Not value.Equals(DBNull.Value) Then
                    EnterPEMR = value
                    rbEnter.Checked = value
                    rbCalculate.Checked = Not value
                End If

                'Levies only apply for calculated rates.
                If rbCalculate.Checked Then
                    value = dr("PEMRByTotalLevy")

                    If Not value.Equals(DBNull.Value) Then
                        PEMRByTotalLevy = value
                        rbTotalLevy.Checked = value
                        rbClassLevy.Checked = Not value
                    End If
                End If
            End If

            SetControlState()
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        Finally
            dr.Close()
            con.Close()
        End Try
    End Sub

    'Set the state of the controls on the page.
    Private Sub SetControlState()
        Dim txtSubjectPEMR As TextBox

        levy.Visible = rbCalculate.Checked

        If levy.Visible Then
            If PEMRByTotalLevy.HasValue Then
                rbTotalLevy.Checked = PEMRByTotalLevy.Value
                rbClassLevy.Checked = Not rbTotalLevy.Checked
            Else
                rbTotalLevy.Checked = True
                rbClassLevy.Checked = False
            End If
        End If

        'Subject Year PEMR is only editable when user is manually entering mill rates.
        For i As Integer = 0 To grdPEMR.Rows.Count - 1
            txtSubjectPEMR = CType(grdPEMR.Rows(i).FindControl("txtSubjectPEMR"), TextBox)
            txtSubjectPEMR.Enabled = rbEnter.Checked

            If IsPostBack AndAlso rbCalculate.Checked Then
                txtSubjectPEMR.Text = String.Empty
            End If
        Next
    End Sub

    'Validate subject PEMR.
    Protected Sub ValidateSubjectPEMR(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim subjectPEMR As String

        'Manually entered mill rates must have values.
        If rbEnter.Checked Then
            For Each row As GridViewRow In grdPEMR.Rows
                subjectPEMR = CType(row.FindControl("txtSubjectPEMR"), TextBox).Text

                If Not IsNumeric(subjectPEMR) Then
                    Master.errorMsg = common.GetErrorMessage("PATMAP72")
                    args.IsValid = False

                    Exit Sub
                End If
            Next
        End If
    End Sub

    'Compare the current row to the corresponding row in the saved DataTable object, and return true if the row has changed.
    Private Function IsRowModified(ByVal currentRow As GridViewRow, ByVal originalRow As DataRow) As Boolean
        Dim txtSubjectPEMR As TextBox
        Dim subjectYearPEMR As Double

        'Subject Year PEMR field.
        txtSubjectPEMR = CType(currentRow.FindControl("txtSubjectPEMR"), TextBox)

        If String.IsNullOrEmpty(txtSubjectPEMR.Text) Then
            subjectYearPEMR = 0
        Else
            subjectYearPEMR = CDbl(txtSubjectPEMR.Text)
        End If

        'Check if the Subject Year PEMR value has changed.
        If Not subjectYearPEMR.Equals(CDbl(originalRow("subjectPEMR"))) Then Return True

        Return False
    End Function

    'Save the state of the radio buttons.
    Private Sub UpdateData()
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand()

        Try
            con.ConnectionString = PATMAP.Global_asax.connString
            cmd.Connection = con
            cmd.CommandText = "liveAssessmentTaxModelUpdatePEMRData"
            cmd.CommandType = CommandType.StoredProcedure
            con.Open()

            With cmd.Parameters
                .AddWithValue("@userID", Session("userID"))
                .AddWithValue("@enterPEMR", rbEnter.Checked)
                .AddWithValue("@PEMRByTotalLevy", rbTotalLevy.Checked)
            End With

            cmd.ExecuteNonQuery()
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        Finally
            con.Close()
        End Try
    End Sub

    'Update the audit trail with any changes made to Subject Year PEMR.
    Private Sub UpdateAuditTrail(ByVal auditTrailText As String)
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand()

        Try
            con.ConnectionString = PATMAP.Global_asax.connString
            cmd.Connection = con
            cmd.CommandText = "liveAssessmentTaxModelUpdateAuditTrail"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@userID", Session("userID"))
            cmd.Parameters.AddWithValue("@auditTrailText", auditTrailText)
            con.Open()

            cmd.ExecuteNonQuery()
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        Finally
            con.Close()
        End Try
    End Sub
End Class