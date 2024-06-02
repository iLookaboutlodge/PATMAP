Public Partial Class linearproperty
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            Dim da As New SqlClient.SqlDataAdapter
            da.SelectCommand = New SqlClient.SqlCommand()
            Dim dt As New DataTable


            da.SelectCommand.Connection = con
            da.SelectCommand.CommandText = "select t.taxClassID, t.taxClass, sum(totalAssessmentValue) as totalAssessmentValue, percentageTransfer from ("
            da.SelectCommand.CommandText += " select c.taxClassID, c.taxClass, b.taxableAssessmentValue + b.FGILAssessmentValue + b.PGILAssessmentValue + b.otherExemptAssessmentValue + b.Section293AssessmentValue + b.ByLawExemptionAssessmentValue as totalAssessmentValue, b.percentageTransfer * 100 as percentageTransfer from boundaryLinearTransfers b join taxClasses c on b.taxClassID=c.taxClassID  where boundaryGroupID=" & Session("boundaryGroupID")
            da.SelectCommand.CommandText += " ) t group by taxClassID, taxClass, percentageTransfer"

            'populate the data grid
            da.Fill(dt)
            grdLinearProp.DataSource = dt
            grdLinearProp.DataBind()

            con.Close()
        End If

        lblErrorText.Text = ""

    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        query.CommandText = ""

        'for each class (pipeline & railway) update the percentage to transfer
        Dim percentageTransfer As System.Web.UI.WebControls.TextBox
        Dim taxClassID As String
        Dim row As GridViewRow
        For Each row In grdLinearProp.Rows
            If row.RowType = DataControlRowType.DataRow Then
                percentageTransfer = grdLinearProp.Rows(row.RowIndex).Cells(2).FindControl("txtPercentageTransfer")

                'validate the percentage value
                If Not IsNumeric(Replace(Trim(percentageTransfer.Text), "%", "")) Then
                    lblErrorText.Text = common.GetErrorMessage("PATMAP72")
                    con.Close()
                    Exit Sub
                End If

                If Not common.ValidateRange(Trim(percentageTransfer.Text), 0, 100) Then
                    lblErrorText.Text = common.RangeErrorMsg(0, 100)
                    con.Close()
                    Exit Sub
                End If

                taxClassID = grdLinearProp.DataKeys(row.RowIndex).Values("taxClassID")
                query.CommandText += "update boundaryLinearTransfers set percentageTransfer=" & (Trim(percentageTransfer.Text) / 100) & " where boundaryGroupID=" & Session("boundaryGroupID") & " and taxClassID='" & taxClassID & "'" & vbCrLf
            End If
        Next

        query.ExecuteNonQuery()
        con.Close()

        'close the popup
        Response.Write("<script language='javascript'> { window.close();}</script>")
    End Sub
End Class