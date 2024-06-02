Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization

Partial Public Class viewsysstat
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try

			'Clears out the error message
			Master.errorMsg = ""

			If Not IsPostBack Then

				'Keeps session for the reportviewer. ReportViewer is in a frame so session
				'isn't normally tracked.
				Response.AddHeader("P3P", "CP=""CAO PSA OUR""")

				'report types
				'create an instance of our web service
				Dim ws As New PATMAPWebService.ReportingService2005
				'pass in the default credentials - meaning currently logged in user
				'ws.Credentials = System.Net.CredentialCache.DefaultCredentials
				'pass in the network credentials to access the reporting service
				ws.Credentials = New reportServerCredentials().NetworkCredentials

				Dim items As PATMAPWebService.CatalogItem() = ws.ListChildren(PATMAP.Global_asax.ReportSystemFolder, True)
				Dim catItem As PATMAPWebService.CatalogItem

				Dim ddlFirstItem = New ListItem("--Report Type--", "0")
				ddlReport.Items.Add(ddlFirstItem)
				For Each catItem In items
					Dim ddlItem = New ListItem(catItem.Name, catItem.Path)
					ddlReport.Items.Add(ddlItem)
				Next
			Else
				txtDateFrom.Text = Request(txtDateFrom.UniqueID)
				txtDateTo.Text = Request(txtDateTo.UniqueID)
			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try

	End Sub

	Private Sub btnShow_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnShow.Click
		Try

			If ddlReport.SelectedIndex <> 0 Then
				Dim reportParamArray(1) As Microsoft.Reporting.WebForms.ReportParameter
				Dim dateFrom As Date = "1/1/0001"

				'String.Format(txtDateFrom.Text, "MM/dd/yyyy")
				'CultureInfo.InvariantCulture

				'If IsDate(txtDateFrom.Text) Then
				If IsValidDate(txtDateFrom.Text) Then
					'dateFrom = CDate(txtDateFrom.Text).ToShortDateString
					dateFrom = DateTime.ParseExact(txtDateFrom.Text, "d", CultureInfo.GetCultureInfo("en-US")).ToShortDateString()
				Else
					'dateFrom = CDate(Now().Month() & "/01/" & Now.Year())
					dateFrom = DateTime.ParseExact(Now().Month() & "/01/" & Now.Year(), "d", CultureInfo.GetCultureInfo("en-US")).ToShortDateString()
				End If

				Dim param0 = New Microsoft.Reporting.WebForms.ReportParameter("dateFrom", dateFrom)
				reportParamArray(0) = param0

				Dim dateTo As Date = "1/1/9999"

				'If IsDate(txtDateTo.Text) Then
				If IsValidDate(txtDateTo.Text) Then
					'dateTo = CDate(txtDateTo.Text).ToShortDateString
					dateTo = DateTime.ParseExact(txtDateTo.Text, "d", CultureInfo.GetCultureInfo("en-US")).ToShortDateString()
				Else
					'dateTo = CDate(dateFrom.Month() + 1 & "/01/" & dateFrom.Year())
					dateTo = DateTime.ParseExact(dateFrom.Month() + 1 & "/01/" & dateFrom.Year(), "d", CultureInfo.GetCultureInfo("en-US")).ToShortDateString()
				End If

				If dateTo < dateFrom Then
					Master.errorMsg = common.GetErrorMessage("PATMAP104")
					Exit Sub
				End If

				Dim param1 = New Microsoft.Reporting.WebForms.ReportParameter("dateTo", dateTo)
				reportParamArray(1) = param1
				'Dim param2 = New Microsoft.Reporting.WebForms.ReportParameter("dateFrom", dateFrom)
				'reportParamArray(2) = param2
				rpvReports.ShowCredentialPrompts = False
				rpvReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote
				rpvReports.ServerReport.ReportServerUrl = New Uri(PATMAP.Global_asax.SQLReportingServer)
				rpvReports.ServerReport.ReportServerCredentials = New reportServerCredentials()
				rpvReports.ServerReport.ReportPath = ddlReport.SelectedValue
				'If IsDate(txtDateFrom.Text) And IsDate(txtDateTo.Text) Then
				rpvReports.ServerReport.SetParameters(reportParamArray)
				'End If

				rpvReports.ZoomMode = Microsoft.Reporting.WebForms.ZoomMode.PageWidth
				rpvReports.ServerReport.Refresh()
			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try

	End Sub

	Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
		Try
			txtDateFrom.Text = ""
			txtDateTo.Text = ""
			ddlReport.SelectedIndex = 0
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try

	End Sub

	Private Sub purgeFiles()
		'setup database connection
		Dim con As New SqlClient.SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		con.Open()

		Dim query As New SqlClient.SqlCommand
		query.Connection = con

		Dim dr As SqlClient.SqlDataReader

		Dim strPath As String

		'delete TaxYearModel Files
		query.CommandText = "select taxYearModelID from taxYearModelDescription where taxYearStatusID = 2"
		dr = query.ExecuteReader()

		Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
		While dr.Read()
			strPath = PATMAP.Global_asax.FileRootPath & edittaxyearmodel.subFolder & "\" & dr.GetValue(0) & "\"
			If Directory.Exists(strPath) Then
				Directory.Delete(strPath, True)
			End If
		End While
		Impersonate.undoImpersonation()

		'delete Assessment Files
		dr.Close()
		query.CommandText = "select assessmentID from assessmentDescription where statusID = 2"
		dr = query.ExecuteReader()

		Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
		While dr.Read()
			strPath = PATMAP.Global_asax.FileRootPath & editdataset.assessmentSubFolder & "\" & dr.GetValue(0) & "\"
			If Directory.Exists(strPath) Then
				Directory.Delete(strPath, True)
			End If
		End While
		Impersonate.undoImpersonation()

		'delete TaxCredit Files
		dr.Close()
		query.CommandText = "select taxCreditID from taxCreditDescription where statusID = 2"
		dr = query.ExecuteReader()

		Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
		While dr.Read()
			strPath = PATMAP.Global_asax.FileRootPath & editdataset.taxCreditSubFolder & "\" & dr.GetValue(0) & "\"
			If Directory.Exists(strPath) Then
				Directory.Delete(strPath, True)
			End If
		End While
		Impersonate.undoImpersonation()

		'delete POV Files
		dr.Close()
		query.CommandText = "select POVID from POVDescription where statusID = 2"
		dr = query.ExecuteReader()

		Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
		While dr.Read()
			strPath = PATMAP.Global_asax.FileRootPath & editdataset.POVSubFolder & "\" & dr.GetValue(0) & "\"
			If Directory.Exists(strPath) Then
				Directory.Delete(strPath, True)
			End If
		End While
		Impersonate.undoImpersonation()

		'delete K12OG Files
		dr.Close()
		query.CommandText = "select K12ID from K12Description where statusID = 2"
		dr = query.ExecuteReader()

		Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
		While dr.Read()
			strPath = PATMAP.Global_asax.FileRootPath & editdataset.K12OGSurveySubFolder & "\" & dr.GetValue(0) & "\"
			If Directory.Exists(strPath) Then
				Directory.Delete(strPath, True)
			End If
		End While
		Impersonate.undoImpersonation()

		'delete MillRateSurvey Files
		dr.Close()
		query.CommandText = "select millRateSurveyID from millRateSurveyDescription where statusID = 2"
		dr = query.ExecuteReader()

		Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
		While dr.Read()
			strPath = PATMAP.Global_asax.FileRootPath & editdataset.millReateSurveySubFolder & "\" & dr.GetValue(0) & "\"
			If Directory.Exists(strPath) Then
				Directory.Delete(strPath, True)
			End If
		End While
		Impersonate.undoImpersonation()

		'delete Potash Files
		dr.Close()
		query.CommandText = "select potashID from potashDescription where statusID = 2"
		dr = query.ExecuteReader()

		Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
		While dr.Read()
			strPath = PATMAP.Global_asax.FileRootPath & editdataset.potashSurveySubFolder & "\" & dr.GetValue(0) & "\"
			If Directory.Exists(strPath) Then
				Directory.Delete(strPath, True)
			End If
		End While
		Impersonate.undoImpersonation()

		con.Close()
	End Sub

	Private Sub purgeDatabase()
		'make connection to database
		Dim con As New SqlClient.SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim query As New SqlClient.SqlCommand
		query.Connection = con
		query.CommandTimeout = 60000
		con.Open()

		'calculate assessment summary
		query.CommandText = "exec purgeDatabase"
		query.ExecuteNonQuery()

		con.Close()
	End Sub

	Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
		purgeFiles()
		purgeDatabase()
	End Sub

	Public Function IsValidDate(ByVal txtDate As String) As Boolean
		Dim dt As DateTime = Nothing
		Try
			dt = DateTime.ParseExact(txtDate, "d", CultureInfo.GetCultureInfo("en-US"))
			If dt <> Nothing Then
				Return True
			End If
		Catch ex As Exception
			Return False
		End Try
		Return False
	End Function

End Class