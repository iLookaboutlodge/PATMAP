Partial Public Class LTTviewReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                Dim filter As String = ""

                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString

                Dim com As New SqlClient.SqlCommand
                Dim dr As SqlClient.SqlDataReader

                con.Open()
                com.Connection = con

                Dim reportPath As String
                reportPath = Session("LTTgraphreportPath")

                Dim reportParamArray(3) As Microsoft.Reporting.WebForms.ReportParameter

                Dim userID As String = Session("userID")
                Dim param0 = New Microsoft.Reporting.WebForms.ReportParameter("userID", userID)
                reportParamArray(0) = param0

                com.CommandText = "select taxStatus from liveTaxStatus join taxStatus on liveTaxStatus.taxstatusID=taxStatus.taxstatusID where userID = " & Session("userID") & " and selected = 1"
                dr = com.ExecuteReader
                If dr.Read() Then
                    filter &= " Tax Status = " & dr.GetValue(0) & ","
                    While (dr.Read())
                        filter &= dr.GetValue(0) & ","
                    End While
                    filter = filter.Substring(0, (filter.Length - 1)) & ";"
                End If
                dr.Close()

                com.CommandText = "select taxClassID from liveTaxClasses where userID = " & Session("userID") & " and show = 1"
                dr.Close()
                dr = com.ExecuteReader
                If dr.Read() Then
                    filter &= " Tax Class = " & dr.GetValue(0) & ","
                    While (dr.Read())
                        filter &= dr.GetValue(0) & ","
                    End While
                    filter = filter.Substring(0, (filter.Length - 1))
                End If
                dr.Close()

                Dim param1 = New Microsoft.Reporting.WebForms.ReportParameter("filter", filter)
                reportParamArray(1) = param1

                If Session("showFullTaxClasses") Then
                    Dim param2 = New Microsoft.Reporting.WebForms.ReportParameter("showFullTaxClasses", 1)
                    reportParamArray(2) = param2
                Else
                    Dim param2 = New Microsoft.Reporting.WebForms.ReportParameter("showFullTaxClasses", 0)
                    reportParamArray(2) = param2
                End If

                Dim param3 = New Microsoft.Reporting.WebForms.ReportParameter("municipality", Session("LTTSubjectMunicipality").ToString)
                reportParamArray(3) = param3

                rpvReports.ShowCredentialPrompts = False
                rpvReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote
                rpvReports.ServerReport.ReportServerUrl = New Uri(PATMAP.Global_asax.SQLReportingServer)
                rpvReports.ServerReport.ReportServerCredentials = New reportServerCredentials()
                rpvReports.ServerReport.ReportPath = reportPath
                rpvReports.ServerReport.SetParameters(reportParamArray)
                rpvReports.ZoomMode = Microsoft.Reporting.WebForms.ZoomMode.PageWidth
                rpvReports.ServerReport.Refresh()

                con.Close()

            End If

        Catch
            common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

End Class