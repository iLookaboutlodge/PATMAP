Partial Public Class viewReport
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

                If Not IsNothing(Session("JurisTypeGroup")) Then
                    query &= "(select top 1 JurisdictionGroup from dbo.jurisdictionGroups where JurisdictionGroupID = " & Session("JurisTypeGroup") & ") as JurisTypeGroup, " & vbCrLf
                Else
                    query &= "(select top 1 JurisdictionGroup from dbo.jurisdictionGroups where JurisdictionGroupID = " & jurisdictionTypeGroup & ") as JurisTypeGroup, " & vbCrLf
                End If

                If Not IsNothing(Session("JurisType")) Then
                    query &= "(select top 1 jurisdictionType from dbo.jurisdictionTypes where jurisdictionTypeID = " & Session("JurisType") & ") as JurisType," & vbCrLf
                Else
                    query &= "(select top 1 jurisdictionType from dbo.jurisdictionTypes where jurisdictionTypeID = " & jurisdictionType & ") as JurisType," & vbCrLf
                End If

                If Not IsNothing(Session("Municipalities")) Then
                    query &= "(select top 1 dbo.ProperCase(jurisdiction) from dbo.entities where number = '" & Session("Municipalities") & "' and jurisdictionTypeID > 1) as Municipalities," & vbCrLf
                Else
                    query &= "(select top 1 dbo.ProperCase(jurisdiction) from dbo.entities where number = '" & municipality & "' and jurisdictionTypeID > 1) as Municipalities," & vbCrLf
                End If

                If Not IsNothing(Session("SchoolDistricts")) Then
                    query &= "(select top 1 dbo.ProperCase(jurisdiction) from dbo.entities where number = '" & Session("SchoolDistricts") & "' and jurisdictionTypeID = 1) as SchoolDistricts," & vbCrLf
                Else
                    query &= "(select top 1 dbo.ProperCase(jurisdiction) from dbo.entities where number = '" & schoolDivision & "' and jurisdictionTypeID = 1) as SchoolDistricts," & vbCrLf
                End If

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
                            jurisdictionTypeGroup = Trim(dr.Item(0))
                        End If

                        If Not IsDBNull(dr.Item(1)) Then
                            jurisdictionType = Trim(dr.Item(1))
                        End If

                        If Not IsDBNull(dr.Item(2)) Then
                            municipality = Trim(dr.Item(2))
                        End If

                        If Not IsDBNull(dr.Item(3)) Then
                            schoolDivision = Trim(dr.Item(3))
                        End If

                        If Not IsDBNull(dr.Item(4)) Then
                            taxStatus = Trim(dr.Item(4))
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

                Dim reportParamArray(9) As Microsoft.Reporting.WebForms.ReportParameter
                Dim userID As String = Session("userID")
                Dim param0 = New Microsoft.Reporting.WebForms.ReportParameter("userID", userID)
                reportParamArray(0) = param0

                If Not IsNothing(Session("Municipalities")) Then
                    filter &= "Municipality = " & municipality & ";"
                    municipality = Session("Municipalities")
                End If
                Dim param1 = New Microsoft.Reporting.WebForms.ReportParameter("municipality", municipality)
                reportParamArray(1) = param1

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
                Dim param2 = New Microsoft.Reporting.WebForms.ReportParameter("taxStatus", taxStatus)
                reportParamArray(2) = param2

                If Not IsNothing(Session("JurisTypeGroup")) Then
                    filter &= "Jurisdiction Type Group = " & jurisdictionTypeGroup & ";"
                    jurisdictionTypeGroup = Session("JurisTypeGroup")
                End If
                Dim param3 = New Microsoft.Reporting.WebForms.ReportParameter("jurisdictionTypeGroup", jurisdictionTypeGroup)
                reportParamArray(3) = param3

                If Not IsNothing(Session("JurisType")) Then
                    filter &= "Jurisdiction Type = " & jurisdictionType & ";"
                    jurisdictionType = Session("JurisType")
                End If
                Dim param4 = New Microsoft.Reporting.WebForms.ReportParameter("jurisdictionType", jurisdictionType)
                reportParamArray(4) = param4

                If Not IsNothing(Session("SchoolDistricts")) Then
                    filter &= "School Division = " & schoolDivision & ";"
                    schoolDivision = Session("SchoolDistricts")
                End If
                Dim param5 = New Microsoft.Reporting.WebForms.ReportParameter("schoolDivision", schoolDivision)
                reportParamArray(5) = param5

                If Not IsNothing(Session("ParcelID")) Then
                    parcelID = Session("ParcelID")
                    If parcelID <> "0" Then
                        filter &= "Parcel ID = " & parcelID & ";"
                    End If
                End If
                Dim param6 = New Microsoft.Reporting.WebForms.ReportParameter("parcelID", parcelID)
                reportParamArray(6) = param6

                If Not IsNothing(Session("TaxShift")) Then

                    Select Case Trim(Session("TaxShift"))
                        Case 1
                            taxType = "Municipal Tax"
                        Case 2
                            taxType = "School Tax"
                        Case 3
                            taxType = "Grant"
                    End Select

                    filter &= "Tax Type = " & taxType & ";"
                    taxType = Session("TaxShift")

                End If
                Dim param8 = New Microsoft.Reporting.WebForms.ReportParameter("taxType", taxType)
                reportParamArray(8) = param8

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
                Dim param9 = New Microsoft.Reporting.WebForms.ReportParameter("taxClass", taxClass)
                reportParamArray(9) = param9

                Dim param7 = New Microsoft.Reporting.WebForms.ReportParameter("filter", filter)
                reportParamArray(7) = param7

                'rpvReports.ShowCredentialPrompts = False
                'rpvReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote
                'rpvReports.ServerReport.ReportServerUrl = New Uri(PATMAP.Global_asax.SQLReportingServer)

                'rpvReports.ServerReport.ReportServerCredentials = New reportServerCredentials()

                'rpvReports.ServerReport.ReportPath = reportPath

                'rpvReports.ServerReport.SetParameters(reportParamArray)
                'rpvReports.ZoomMode = Microsoft.Reporting.WebForms.ZoomMode.PageWidth
                'rpvReports.ServerReport.Refresh()

                rpvReports.ShowCredentialPrompts = False
                rpvReports.ShowExportControls = True
                rpvReports.ShowPageNavigationControls = True
                rpvReports.ShowPrintButton = True
                'rpvReports.AsyncRendering = True
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