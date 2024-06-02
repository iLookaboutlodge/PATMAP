Partial Public Class viewReportBoundary
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

                Dim query As String = ""
                Dim municipality As String = " "
                Dim taxStatus As String = "0"
                Dim jurisdictionTypeGroup As String = "0"
                Dim jurisdictionType As String = "0"
                Dim schoolDivision As String = "0"
                Dim taxType As String = "0"
                Dim parcelID As String = "0"
                Dim taxClass As String = " "

                query = "select" & vbCrLf

                If Not IsNothing(Session("TaxStatus")) Then
                    query &= "(select top 1 taxStatus from dbo.taxStatus where taxStatusID = " & Session("TaxStatus") & ") as TaxStatus"
                Else
                    query &= "(select top 1 taxStatus from dbo.taxStatus where taxStatusID = " & taxStatus & ") as TaxStatus"
                End If


                If query <> "select" Then
                    com.CommandText = query
                    dr = com.ExecuteReader()

                    If dr.Read() Then
                        If Not IsDBNull(dr.Item(0)) Then
                            taxStatus = Trim(dr.Item(0))
                        End If

                    End If
                    dr.Close()
                End If

                Dim reportPath As String
                If Not IsNothing(Session("reportPath")) Then
                    reportPath = Session("reportPath")
                Else
                    lblErrorText.Text = common.GetErrorMessage("PATMAP131")
                    Exit Sub
                End If

                Dim reportParamArray(5) As Microsoft.Reporting.WebForms.ReportParameter

                Dim userID As String = Session("userID")
                Dim param0 = New Microsoft.Reporting.WebForms.ReportParameter("userID", userID)
                reportParamArray(0) = param0

                If Not IsNothing(Session("TaxStatus")) Then
                    taxStatus = Session("TaxStatus")
                    If Session("TaxStatus") <> 0 Then
                        com.CommandText = "select taxStatus from taxStatus where taxStatusID = " & taxStatus
                        dr = com.ExecuteReader
                        dr.Read()
                        filter &= "Tax Status = " & dr.GetValue(0) & ";"
                        dr.Close()
                    Else
                        com.CommandText = "select taxStatus from liveTaxStatus join taxStatus on liveTaxStatus.taxstatusID=taxStatus.taxstatusID where userID = " & userID & " and selected = 1"
                        dr = com.ExecuteReader
                        If dr.Read() Then
                            filter &= "Tax Status = " & dr.GetValue(0) & ","
                            While (dr.Read())
                                filter &= dr.GetValue(0) & ","
                            End While
                            filter = filter.Substring(0, (filter.Length - 3)) & ";"
                        End If
                        dr.Close()
                    End If
                Else
                    com.CommandText = "select taxStatus from liveTaxStatus join taxStatus on liveTaxStatus.taxstatusID=taxStatus.taxstatusID where userID = " & userID & " and selected = 1"
                    dr = com.ExecuteReader
                    If dr.Read() Then
                        filter &= "Tax Status = " & dr.GetValue(0) & ","
                        While (dr.Read())
                            filter &= dr.GetValue(0) & ","
                        End While
                        filter = filter.Substring(0, (filter.Length - 3)) & ";"
                    End If
                    dr.Close()
                End If
                Dim param1 = New Microsoft.Reporting.WebForms.ReportParameter("taxStatus", taxStatus)
                reportParamArray(1) = param1

                If Not IsNothing(Session("TaxClass")) Then
                    taxClass = Session("TaxClass")
                    If Session("TaxClass") <> "" Then
                        filter &= "Tax Class = " & taxClass & ";"
                    Else
                        com.CommandText = "select taxClassID from liveTaxClasses where userID = " & userID & " and show = 1"
                        dr = com.ExecuteReader
                        If dr.Read() Then
                            filter &= "Tax Class = " & dr.GetValue(0) & ","
                            While (dr.Read())
                                filter &= dr.GetValue(0) & ","
                            End While
                            filter = filter.Substring(0, (filter.Length - 3)) & ";"
                        End If
                        dr.Close()
                    End If
                Else
                    com.CommandText = "select taxClassID from liveTaxClasses where userID = " & userID & " and show = 1"
                    dr.Close()
                    dr = com.ExecuteReader
                    If dr.Read() Then
                        filter &= "Tax Class = " & dr.GetValue(0) & ","
                        While (dr.Read())
                            filter &= dr.GetValue(0) & ","
                        End While
                        filter = filter.Substring(0, (filter.Length - 3)) & ";"
                    End If
                    dr.Close()
                End If
                Dim param2 = New Microsoft.Reporting.WebForms.ReportParameter("taxClass", taxClass)
                reportParamArray(2) = param2

                Dim subjectOriginalAssessment As Decimal = 0
                If Not IsNothing(Session("subjectOriginalDrivedAssessment")) Then
                    subjectOriginalAssessment = Session("subjectOriginalDrivedAssessment")
                End If
                Dim param3 = New Microsoft.Reporting.WebForms.ReportParameter("subjectAssessment", subjectOriginalAssessment)
                reportParamArray(3) = param3

                Dim subjectOriginalLevy As Decimal = 0
                If Not IsNothing(Session("subjectOriginalLevy")) Then
                    subjectOriginalLevy = Session("subjectOriginalLevy")
                End If
                Dim param4 = New Microsoft.Reporting.WebForms.ReportParameter("subjectLevy", subjectOriginalLevy)
                reportParamArray(4) = param4

                Dim param5 = New Microsoft.Reporting.WebForms.ReportParameter("filter", filter)
                reportParamArray(5) = param5

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