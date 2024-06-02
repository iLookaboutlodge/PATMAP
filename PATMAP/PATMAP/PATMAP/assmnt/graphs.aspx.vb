Imports Microsoft.Reporting.WebForms

Partial Public Class graphs
    Inherits System.Web.UI.Page

	Public ReadOnly Property PContainer()
		Get
			Return Master.FindControl("progressContainer").ClientID
		End Get
	End Property

	Public ReadOnly Property PMessage()
		Get
			Return Master.FindControl("lblCalcMessage").ClientID
		End Get
	End Property


	Protected Sub RegisterPostBack()
		'If Not ClientScript.IsStartupScriptRegistered("JScriptBlock1") Then
		'	Dim sbScript As New StringBuilder()
		'	sbScript.Append("<script language='javascript' type='text/javascript'>" + ControlChars.Lf)
		'	''sbScript.Append(ClientScript.GetPostBackEventReference(Me, "") + ";" + ControlChars.Lf)
		'	sbScript.Append(ClientScript.GetPostBackEventReference(LinkButton1, "") + ";" + ControlChars.Lf)
		'	sbScript.Append("</script>" + ControlChars.Lf)
		'	ClientScript.RegisterStartupScript(Me.GetType(), "JScriptBlock1", sbScript.ToString())
		'End If

		If Not ClientScript.IsClientScriptBlockRegistered("ClientScriptBlock1") Then
			Dim sbScript As New StringBuilder()
			sbScript.Append("<script language='javascript' type='text/javascript'>" + ControlChars.Lf)
			sbScript.Append("function forceAutoPostBack() {" + ControlChars.Lf)
			''sbScript.Append(ClientScript.GetPostBackEventReference(Me, "") + ";" + ControlChars.Lf)
			sbScript.Append(ClientScript.GetPostBackEventReference(lbStep1, "") + ";" + ControlChars.Lf)
			sbScript.Append("};" + ControlChars.Lf)
			sbScript.Append("</script>" + ControlChars.Lf)
			ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClientScriptBlock1", sbScript.ToString())
		End If
	End Sub

	Protected Sub ForcePostBack()
		If Not ClientScript.IsStartupScriptRegistered("JScriptBlock1") Then
			ClientScript.RegisterStartupScript(Me.GetType(), "JScriptBlock1", "forceAutoPostBack();", True)
		End If
	End Sub

	Protected Function CheckAutoPostBack(ByRef ControlID As String)
		Dim IsAutoPostBack As Boolean = False
		Dim lnkButton As LinkButton = Nothing
		Dim controlName As String = Page.Request.Params("__EVENTTARGET")
		If Not String.IsNullOrEmpty(controlName) Then
			If Not IsNothing(Page.FindControl(controlName)) AndAlso TypeOf (Page.FindControl(controlName)) Is LinkButton Then
				lnkButton = CType(Page.FindControl(controlName), LinkButton)
				'If lnkButton.ID = "lbStep1" Or lnkButton.ID = "lbStep2" Then
				If lnkButton.ID.IndexOf("lbStep") <> -1 Then
					ControlID = lnkButton.ID
					IsAutoPostBack = True
				End If
			End If
		End If
		Return IsAutoPostBack
	End Function

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		Master.errorMsg = ""

		If Not IsPostBack Then
			'Sets submenu to be displayed
			subMenu.setStartNode(menu.assmnt)

			Try
				If CalculateAssmnt(False, "") Then
					RegisterPostBack()
					ForcePostBack()
				End If
			Catch ex As Exception
				'retrieves error message
				Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
			End Try
		Else
			Try
				Dim IsAutoPostBack As Boolean = False
				Dim ControlID As String = ""
				IsAutoPostBack = CheckAutoPostBack(ControlID)
				If IsAutoPostBack Then
					CalculateAssmnt(True, ControlID)
				End If
			Catch ex As Exception
				'retrieves error message
				Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
			End Try
		End If

	End Sub

	Protected Function CalculateAssmnt(ByVal ShowCalculating As Boolean, ByVal ControlID As String) As Boolean
		Dim IsCalculationRequired As Boolean = True

		Try

			'get current user
			Dim userID As Integer = Session("userID")

			'setup database connection
			Dim con As New SqlClient.SqlConnection
			con.ConnectionString = PATMAP.Global_asax.connString
			con.Open()
			Dim da As New SqlClient.SqlDataAdapter
			Dim dt As New DataTable
			Dim query As New SqlClient.SqlCommand
			query.Connection = con
			query.CommandTimeout = 60000
			Dim dr As SqlClient.SqlDataReader

			'check if K-12 OG data set is entered by the user
			query.CommandText = "select SubjectK12ID from liveAssessmentTaxModel where userid = " & userID
			dr = query.ExecuteReader
			dr.Read()

			If dr.GetValue(0) = 0 Then
				Session.Add("missingK12DataSet", "true")
				'con.Close()    Donna - Keep connection open.
				'''''Response.Redirect("kog.aspx", False)  '***Inky commented-out this line
				'''''Exit Sub  '***Inky commented-out this line
			End If

			dr.Close()

			'Dim BaseTaxYearModelID As Integer
			'Dim BaseStale As Boolean
			'Dim SubjectTaxYearModelID As Integer
			'Dim SubjectStale As Boolean


			Dim baseAssessmentID As Integer
			Dim subjectAssessmentID As Integer

			query.CommandText = "select assessmentID from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
			dr = query.ExecuteReader
			dr.Read()
			baseAssessmentID = dr.GetValue(0)

			dr.Close()
			query.CommandText = "select assessmentID from taxYearModelDescription where taxYearModelID = (select subjectTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
			dr = query.ExecuteReader
			dr.Read()
			subjectAssessmentID = dr.GetValue(0)

			'dr.Close()
			'query.CommandText = "exec compareBaseandSubject " & userID.ToString & "," & subjectAssessmentID.ToString & "," & baseAssessmentID.ToString & ",1"
			'query.ExecuteNonQuery()

			'Dim BaseTaxYearModelID As Integer
			'Dim SubjectTaxYearModelID As Integer
			'Dim dataStale As Boolean
			'Dim basedataStale As Boolean

			''check if data is stale
			'query.CommandText = "select baseTaxYearModelID, subjectTaxYearModelID, dataStale from liveassessmenttaxmodel where userid = " & userID
			'dr = query.ExecuteReader
			'dr.Read()
			'BaseTaxYearModelID = dr.GetValue(0)
			'SubjectTaxYearModelID = dr.GetValue(1)
			'dataStale = dr.GetValue(2)

			'dr.Close()
			'query.CommandText = "select dataStale from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
			'dr = query.ExecuteReader
			'dr.Read()
			'basedataStale = dr.GetValue(0)

			dr.Close()
			Dim doCompare As Integer
			Dim tmpDataStale As Integer
			query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[assessmentBase_" & subjectAssessmentID & "_" & baseAssessmentID & "]') AND type in (N'U')"
			dr = query.ExecuteReader
			If Not dr.Read() Then
				doCompare = 1
			Else
				dr.Close()
				query.CommandText = "SELECT dataStale from assessmentDescription where assessmentID = " & baseAssessmentID
				dr = query.ExecuteReader
				dr.Read()
				tmpDataStale = dr.GetValue(0)
				If tmpDataStale = True Then
					doCompare = 1
				End If
			End If

			dr.Close()
			query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[assessmentSubject_" & subjectAssessmentID & "_" & baseAssessmentID & "]') AND type in (N'U')"
			dr = query.ExecuteReader
			If Not dr.Read() Then
				doCompare = 1
			Else
				dr.Close()
				query.CommandText = "SELECT dataStale from assessmentDescription where assessmentID = " & subjectAssessmentID
				dr = query.ExecuteReader
				dr.Read()
				tmpDataStale = dr.GetValue(0)
				If tmpDataStale = True Then
					doCompare = 1
				End If
			End If

			dr.Close()

			Dim sm As ScriptManager = DirectCast(Master.FindControl("scmControl"), ScriptManager)


			If ShowCalculating And ControlID = "lbStep1" Then
				If doCompare = 1 Then
					query.CommandText = "exec compareBaseandSubject " & userID.ToString & "," & subjectAssessmentID.ToString & "," & baseAssessmentID.ToString & ",1," & doCompare
					query.ExecuteNonQuery()
				End If
				sm.RegisterDataItem(lbStep1, "WillCausePostBack")
				Return IsCalculationRequired
			Else
				If Not ShowCalculating Then
					If doCompare = 1 Then
						Return IsCalculationRequired
					End If
				End If
			End If

			Dim BaseTaxYearModelID As Integer
			Dim SubjectTaxYearModelID As Integer
			Dim dataStale As Boolean
			Dim enterPEMR As Boolean		'Donna
			Dim PEMRByTotalLevy As Boolean	'Donna
			Dim basedataStale As Boolean

			'check if data is stale
			'Donna - Added enterPEMR and PEMRByTotalLevy.
			query.CommandText = "select baseTaxYearModelID, subjectTaxYearModelID, dataStale, enterPEMR, PEMRByTotalLevy from liveassessmenttaxmodel where userid = " & userID
			dr = query.ExecuteReader
			dr.Read()
			BaseTaxYearModelID = dr.GetValue(0)
			SubjectTaxYearModelID = dr.GetValue(1)
			dataStale = dr.GetValue(2)
			enterPEMR = dr("enterPEMR")	 'Donna

			'Donna start
			If dr("PEMRByTotalLevy").Equals(DBNull.Value) Then
				PEMRByTotalLevy = False
			Else
				PEMRByTotalLevy = dr("PEMRByTotalLevy")
			End If
			'Donna end

			dr.Close()

			query.CommandText = "select dataStale from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
			dr = query.ExecuteReader
			dr.Read()
			basedataStale = dr.GetValue(0)

			If ShowCalculating And ControlID = "lbStep2" Then
				'If BaseStale Or SubjectStale Or IsNothing(Session("calculated")) Then
				If dataStale Or basedataStale Or IsNothing(Session("calculated")) Then
					Try
						'check if base year is stale
						If basedataStale Then
							common.calculateTaxYearModel(1, BaseTaxYearModelID, userID, SubjectTaxYearModelID)
						End If

					Catch
						'retrieves error message
						'Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
						Throw
					End Try
				End If
				sm.RegisterDataItem(lbStep2, "WillCausePostBack")
				Return IsCalculationRequired
			Else
				If Not ShowCalculating Then
					If dataStale Or basedataStale Or IsNothing(Session("calculated")) Then
						Return IsCalculationRequired
					End If
				End If
			End If

			If ShowCalculating And ControlID = "lbStep3" Then
				If dataStale Or basedataStale Or IsNothing(Session("calculated")) Then
					Try
						'check if subject year is stale
						If dataStale Then
							common.calculateTaxYearModel(0, SubjectTaxYearModelID, userID)
						End If

						'Donna start
						If Not enterPEMR Then
							If PEMRByTotalLevy Then
								common.calcRevenueNeutralByTotalLevy(userID, BaseTaxYearModelID)
							Else
								common.calcRevenueNeutralByClassLevy(userID, BaseTaxYearModelID)
							End If
						End If
						'Donna end
					Catch
						'retrieves error message
						'Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
						Throw
					End Try
				End If
				sm.RegisterDataItem(lbStep3, "WillCausePostBack")
				Return IsCalculationRequired
			Else
				If Not ShowCalculating Then
					If dataStale Or basedataStale Or IsNothing(Session("calculated")) Then
						Return IsCalculationRequired
					End If
				End If
			End If

			If ShowCalculating And ControlID = "lbStep4" Then
				If dataStale Or basedataStale Or IsNothing(Session("calculated")) Then
					Try
						common.calcAssessmentSummary(userID, 0)
					Catch
						'retrieves error message
						'Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
						Throw
					End Try
				End If
				sm.RegisterDataItem(lbStep4, "WillCausePostBack")
				Return IsCalculationRequired
			Else
				If Not ShowCalculating Then
					If dataStale Or basedataStale Or IsNothing(Session("calculated")) Then
						Return IsCalculationRequired
					End If
				End If
			End If

			If ShowCalculating And ControlID = "lbStep5" Then
				If dataStale Or basedataStale Or IsNothing(Session("calculated")) Then
					Try
						'create School Municipality and Parcels tables for Map viewing
						common.Create_School_Mun_Parcel_Tables()
						Session.Add("calculated", "true")
					Catch
						'retrieves error message
						'Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
						Throw
					End Try
				End If
				sm.RegisterDataItem(lbStep5, "WillCausePostBack")
				Return IsCalculationRequired
			Else
				If Not ShowCalculating Then
					If dataStale Or basedataStale Or IsNothing(Session("calculated")) Then
						Return IsCalculationRequired
					End If
				End If
			End If

			'set the calculation required to false when all calculation is done
			IsCalculationRequired = False

			If ShowCalculating And ControlID = "lbStepFinal" Then
				Response.Redirect("graphs.aspx", True)
				Return IsCalculationRequired
			End If

			Try

				'Keeps session for the reportviewer. ReportViewer is in a frame so session
				'isn't normally tracked.
				Response.AddHeader("P3P", "CP=""CAO PSA OUR""")

				Dim levelID As Integer = Session("levelID")

				'Provincial Analyst, Sys Admininstrator, Presentation users 
				'only has the permission to change scenario parameters
				If levelID = 49 Then
					btnSave.ImageUrl = "~/images/btnSaveAs.gif"
				ElseIf levelID > 3 Then
					btnSave.Visible = False
					txtScenarioName.Enabled = False
				ElseIf levelID = 3 Then
					'Presentation users has to re-save current scenario in 
					'in a different scenario name
					btnSave.ImageUrl = "~/images/btnSaveAs.gif"
				End If

				'Gets Tax Year Model names used by the scenario 
				common.GetModelNames(txtBaseTaxYrModel.Text, txtSubjectTaxYrModel.Text, Session("userID"), txtScenarioName.Text)

				'Set Tax Classes Filters for map
				common.SetTaxClassFilters()

				'check if K-12 OG data set is entered by the user
				dr.Close()
				query.CommandText = "select SubjectK12ID from liveAssessmentTaxModel where userid = " & userID
				dr = query.ExecuteReader
				dr.Read()

				If dr.GetValue(0) = 0 Then
					Session.Add("missingK12DataSet", "true")
					'con.Close()    Donna - Keep connection open.
					'''''Response.Redirect("kog.aspx", False) '***Inky commented-out this line
					'''''Exit Sub '***Inky commented-out this line
				End If

				dr.Close()

				'jurisdiction type group
				da.SelectCommand = New SqlClient.SqlCommand()
				da.SelectCommand.Connection = con
				da.SelectCommand.CommandText = "select 0 as JurisdictionGroupID, '--Jurisdiction Type Group--' as JurisdictionGroup union all select JurisdictionGroupID, JurisdictionGroup from jurisdictionGroups"
				da.Fill(dt)
				ddlJurisTypeGroup.DataSource = dt
				ddlJurisTypeGroup.DataValueField = "JurisdictionGroupID"
				ddlJurisTypeGroup.DataTextField = "JurisdictionGroup"
				ddlJurisTypeGroup.DataBind()

				If Not IsNothing(Session("JurisTypeGroup")) Then
					ddlJurisTypeGroup.SelectedValue = Session("JurisTypeGroup")
				End If

				'jurisdiction type
				dt.Clear()
				da.SelectCommand = New SqlClient.SqlCommand()
				da.SelectCommand.Connection = con
				da.SelectCommand.CommandText = "select 0 as jurisdictionTypeID, '--Jurisdiction Type--' as jurisdictionType union all select jurisdictionTypeID, jurisdictionType from jurisdictionTypes"
				da.Fill(dt)
				ddlJurisType.DataSource = dt
				ddlJurisType.DataValueField = "jurisdictionTypeID"
				ddlJurisType.DataTextField = "jurisdictionType"
				ddlJurisType.DataBind()

				If Not IsNothing(Session("JurisType")) Then
					If Not IsNothing(ddlJurisType.Items.FindByValue(Session("JurisType"))) Then
						ddlJurisType.SelectedValue = Session("JurisType")
					End If
				End If

				'municipality
				dt = Nothing
				dt = common.FillMunicipality(ddlJurisType.SelectedValue, , ddlJurisTypeGroup.SelectedValue)
				ddlMunicipality.DataSource = dt
				ddlMunicipality.DataValueField = "number"
				ddlMunicipality.DataTextField = "jurisdiction"
				ddlMunicipality.DataBind()

				If Not IsNothing(Session("Municipalities")) Then
					If Not IsNothing(ddlMunicipality.Items.FindByValue(Session("Municipalities"))) Then
						ddlMunicipality.SelectedValue = Session("Municipalities")
					End If
				End If

				'school division
				dt.Clear()
				da.SelectCommand.CommandText = "select 0 as number, '--School Division--' as jurisdiction  union all select e.number, dbo.ProperCase(e.jurisdiction) as jurisdiction from entities e where e.jurisdictionTypeID=1"
				da.Fill(dt)
				ddlSchoolDivision.DataSource = dt
				ddlSchoolDivision.DataValueField = "number"
				ddlSchoolDivision.DataTextField = "jurisdiction"
				ddlSchoolDivision.DataBind()

				If Not IsNothing(Session("SchoolDistricts")) Then
					If IsNumeric(Session("SchoolDistricts")) Then
						ddlSchoolDivision.SelectedValue = Session("SchoolDistricts")
					End If
				End If

				'tax status
				dt.Clear()
				da.SelectCommand.CommandText = "select 0 as taxStatusID, '--Tax Status--' as taxStatus  union all select taxStatusID, taxStatus from taxStatus"
				da.Fill(dt)
				ddlTaxStatus.DataSource = dt
				ddlTaxStatus.DataValueField = "taxStatusID"
				ddlTaxStatus.DataTextField = "taxStatus"
				ddlTaxStatus.DataBind()

				Dim selectedTaxStatus As New List(Of String)
				Dim counter As Integer

				If Not IsNothing(Session("MapTaxStatusFilters")) Then
					selectedTaxStatus = CType(Session("MapTaxStatusFilters"), List(Of String))

					For counter = 0 To selectedTaxStatus.Count - 1

						Select Case Trim(selectedTaxStatus(counter))
							Case "Provincial grant in lieu"
								ddlTaxStatus.SelectedValue = 6
								Exit For
							Case "Federal grant in lieu"
								ddlTaxStatus.SelectedValue = 5
								Exit For
							Case "Exempt"
								ddlTaxStatus.SelectedValue = 4
								Exit For
							Case "Taxable"
								ddlTaxStatus.SelectedValue = 1
								Exit For
							Case Else
								If Not IsNothing(ddlTaxStatus.Items.FindByValue(Trim(selectedTaxStatus(counter)))) Then
									ddlTaxStatus.SelectedValue = ddlTaxStatus.Items.FindByValue(Trim(selectedTaxStatus(counter))).Value
									Exit For
								End If
						End Select
					Next

					If ddlTaxStatus.SelectedValue > 0 Then
						Session("TaxStatus") = ddlTaxStatus.SelectedValue
					End If

				End If

				'added on to display default tax status 17-jun-2013
				If selectedTaxStatus.Count <= 0 Then
					selectedTaxStatus.Add("Taxable")
					ddlTaxStatus.SelectedValue = 1
					Session("TaxStatus") = ddlTaxStatus.SelectedValue
					Session("MapTaxStatusFilters") = selectedTaxStatus
				End If

				'tax classes
				dt.Clear()
				da.SelectCommand.CommandText = "select ' ' as taxClassID, '--Tax Classes--' as taxClass  union all select t.taxClassID, t.taxClass from taxClasses t, liveTaxClasses l where t.taxClassID = l.taxClassID AND l.show=1 AND userID=" & Session("userID")
				da.Fill(dt)
				ddlTaxClasses.DataSource = dt
				ddlTaxClasses.DataValueField = "taxClassID"
				ddlTaxClasses.DataTextField = "taxClass"
				ddlTaxClasses.DataBind()

				If Not IsNothing(Session("TaxClass")) Then
					ddlTaxClasses.SelectedValue = Session("TaxClass")
				End If

				If Not IsNothing(Session("ParcelID")) Then
					txtParcelNo.Text = Session("ParcelID")
				End If

				txtParcelNo.Attributes.Add("onBlur", "javascript: if (this.value == '') this.value = '-- Enter Parcel ID --';")
				txtParcelNo.Attributes.Add("onClick", "javascript: if (this.value == '-- Enter Parcel ID --') this.value = '';")

				Dim selectedTaxShift As New List(Of String)

				If Not IsNothing(Session("MapTaxShiftFilters")) Then

					selectedTaxShift = CType(Session("MapTaxShiftFilters"), List(Of String))

					For counter = 0 To selectedTaxShift.Count - 1
						Select Case Trim(selectedTaxShift(counter))
							Case "Municipal Tax"
								ddlTaxType.SelectedValue = 1
								Exit For
							Case "School Tax"
								ddlTaxType.SelectedValue = 2
								Exit For
							Case "Grant"
								ddlTaxType.SelectedValue = 3
								Exit For
							Case Else
								If Not IsNothing(ddlTaxType.Items.FindByValue(Trim(selectedTaxShift(counter)))) Then
									ddlTaxType.SelectedValue = ddlTaxType.Items.FindByValue(Trim(selectedTaxShift(counter))).Value
									Session("TaxShift") = ddlTaxType.Items.FindByValue(Trim(selectedTaxShift(counter))).Value
									Exit For
								End If
						End Select
					Next

					If ddlTaxType.SelectedValue > 0 Then
						Session("TaxShift") = ddlTaxType.SelectedValue
					End If
				End If

				'added on to display default tax type 17-jun-2013
				If selectedTaxShift.Count <= 0 Then
					selectedTaxShift.Add("Municipal Tax")
					ddlTaxType.SelectedValue = 1
					Session("TaxShift") = ddlTaxType.SelectedValue
					Session("MapTaxShiftFilters") = selectedTaxShift
				End If

				If IsNothing(Session("taxStatusPageID")) Then
					query.CommandText = "Delete from liveTaxStatus where userID = " & userID
					query.ExecuteNonQuery()

					query.CommandText = "Insert into liveTaxStatus values(" & userID & ",1,1) "
					query.CommandText += "Insert into liveTaxStatus values(" & userID & ",5,1) "
					query.CommandText += "Insert into liveTaxStatus values(" & userID & ",6,1) "
					query.CommandText += "Insert into liveTaxStatus values(" & userID & ",4,0) "
					query.ExecuteNonQuery()
				End If

				'clean up
				con.Close()

				'report types
				'create an instance of our web service
				Dim ws As New PATMAPWebService.ReportingService2005
				'pass in the default credentials - meaning currently logged in user
				'ws.Credentials = System.Net.CredentialCache.DefaultCredentials
				'pass in the network credentials to access the reporting service
				ws.Credentials = New reportServerCredentials().NetworkCredentials

				'checks if the folder exists in the report server
				If ws.GetItemType(PATMAP.Global_asax.ReportGraphsFolder) = PATMAPWebService.ItemTypeEnum.Folder Then

					Dim items As PATMAPWebService.CatalogItem() = ws.ListChildren(PATMAP.Global_asax.ReportGraphsFolder, True)
					Dim catItem As PATMAPWebService.CatalogItem

					Dim ddlFirstItem = New ListItem("--Report Type--", "0")
					ddlReport.Items.Add(ddlFirstItem)
					For Each catItem In items
						Dim ddlItem = New ListItem(catItem.Name, catItem.Path)
						ddlReport.Items.Add(ddlItem)
					Next

				End If

				Dim reportPath As String = ""

				If Not IsNothing(Session("reportPath")) Then

					reportPath = Replace(Session("reportPath"), PATMAP.Global_asax.ReportTablesFolder, PATMAP.Global_asax.ReportGraphsFolder)

					If Not IsNothing(ddlReport.Items.FindByValue(reportPath)) Then
						ddlReport.SelectedValue = reportPath
					End If
				End If

			Catch
				'retrieves error message
				'Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
				Throw
			End Try

		Catch ex As Exception
			Throw
		End Try
		Return IsCalculationRequired
	End Function


	'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	'	Try

	'		Master.errorMsg = ""

	'		If Not IsPostBack Then

	'			'Sets submenu to be displayed
	'			subMenu.setStartNode(menu.assmnt)

	'			'get current user
	'			Dim userID As Integer = Session("userID")

	'			'setup database connection
	'			Dim con As New SqlClient.SqlConnection
	'			con.ConnectionString = PATMAP.Global_asax.connString
	'			con.Open()
	'			Dim da As New SqlClient.SqlDataAdapter
	'			Dim dt As New DataTable
	'			Dim query As New SqlClient.SqlCommand
	'			query.Connection = con
	'			query.CommandTimeout = 60000
	'			Dim dr As SqlClient.SqlDataReader

	'			'check if K-12 OG data set is entered by the user
	'			query.CommandText = "select SubjectK12ID from liveAssessmentTaxModel where userid = " & userID
	'			dr = query.ExecuteReader
	'			dr.Read()

	'			If dr.GetValue(0) = 0 Then
	'				Session.Add("missingK12DataSet", "true")
	'				'con.Close()    Donna - Keep connection open.
	'				'''''Response.Redirect("kog.aspx", False)  '***Inky commented-out this line
	'				'''''Exit Sub  '***Inky commented-out this line
	'			End If

	'			dr.Close()

	'			'Dim BaseTaxYearModelID As Integer
	'			'Dim BaseStale As Boolean
	'			'Dim SubjectTaxYearModelID As Integer
	'			'Dim SubjectStale As Boolean


	'			Dim baseAssessmentID As Integer
	'			Dim subjectAssessmentID As Integer

	'			query.CommandText = "select assessmentID from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
	'			dr = query.ExecuteReader
	'			dr.Read()
	'			baseAssessmentID = dr.GetValue(0)

	'			dr.Close()
	'			query.CommandText = "select assessmentID from taxYearModelDescription where taxYearModelID = (select subjectTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
	'			dr = query.ExecuteReader
	'			dr.Read()
	'			subjectAssessmentID = dr.GetValue(0)

	'			'dr.Close()
	'			'query.CommandText = "exec compareBaseandSubject " & userID.ToString & "," & subjectAssessmentID.ToString & "," & baseAssessmentID.ToString & ",1"
	'			'query.ExecuteNonQuery()

	'			'Dim BaseTaxYearModelID As Integer
	'			'Dim SubjectTaxYearModelID As Integer
	'			'Dim dataStale As Boolean
	'			'Dim basedataStale As Boolean

	'			''check if data is stale
	'			'query.CommandText = "select baseTaxYearModelID, subjectTaxYearModelID, dataStale from liveassessmenttaxmodel where userid = " & userID
	'			'dr = query.ExecuteReader
	'			'dr.Read()
	'			'BaseTaxYearModelID = dr.GetValue(0)
	'			'SubjectTaxYearModelID = dr.GetValue(1)
	'			'dataStale = dr.GetValue(2)

	'			'dr.Close()
	'			'query.CommandText = "select dataStale from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
	'			'dr = query.ExecuteReader
	'			'dr.Read()
	'			'basedataStale = dr.GetValue(0)

	'			dr.Close()
	'			Dim doCompare As Integer
	'			Dim tmpDataStale As Integer
	'			query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[assessmentBase_" & subjectAssessmentID & "_" & baseAssessmentID & "]') AND type in (N'U')"
	'			dr = query.ExecuteReader
	'			If Not dr.Read() Then
	'				doCompare = 1
	'			Else
	'				dr.Close()
	'				query.CommandText = "SELECT dataStale from assessmentDescription where assessmentID = " & baseAssessmentID
	'				dr = query.ExecuteReader
	'				dr.Read()
	'				tmpDataStale = dr.GetValue(0)
	'				If tmpDataStale = True Then
	'					doCompare = 1
	'				End If
	'			End If

	'			dr.Close()
	'			query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[assessmentSubject_" & subjectAssessmentID & "_" & baseAssessmentID & "]') AND type in (N'U')"
	'			dr = query.ExecuteReader
	'			If Not dr.Read() Then
	'				doCompare = 1
	'			Else
	'				dr.Close()
	'				query.CommandText = "SELECT dataStale from assessmentDescription where assessmentID = " & subjectAssessmentID
	'				dr = query.ExecuteReader
	'				dr.Read()
	'				tmpDataStale = dr.GetValue(0)
	'				If tmpDataStale = True Then
	'					doCompare = 1
	'				End If
	'			End If

	'			dr.Close()
	'			If doCompare = 1 Then
	'				Response.Write(common.JavascriptFunctions())
	'				Response.Flush()

	'				Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
	'				Response.Flush()

	'				query.CommandText = "exec compareBaseandSubject " & userID.ToString & "," & subjectAssessmentID.ToString & "," & baseAssessmentID.ToString & ",1," & doCompare
	'				query.ExecuteNonQuery()
	'			End If


	'			Dim BaseTaxYearModelID As Integer
	'			Dim SubjectTaxYearModelID As Integer
	'			Dim dataStale As Boolean
	'			Dim enterPEMR As Boolean		'Donna
	'			Dim PEMRByTotalLevy As Boolean	'Donna
	'			Dim basedataStale As Boolean

	'			'check if data is stale
	'			'Donna - Added enterPEMR and PEMRByTotalLevy.
	'			query.CommandText = "select baseTaxYearModelID, subjectTaxYearModelID, dataStale, enterPEMR, PEMRByTotalLevy from liveassessmenttaxmodel where userid = " & userID
	'			dr = query.ExecuteReader
	'			dr.Read()
	'			BaseTaxYearModelID = dr.GetValue(0)
	'			SubjectTaxYearModelID = dr.GetValue(1)
	'			dataStale = dr.GetValue(2)
	'			enterPEMR = dr("enterPEMR")	 'Donna

	'			'Donna start
	'			If dr("PEMRByTotalLevy").Equals(DBNull.Value) Then
	'				PEMRByTotalLevy = False
	'			Else
	'				PEMRByTotalLevy = dr("PEMRByTotalLevy")
	'			End If
	'			'Donna end

	'			dr.Close()

	'			query.CommandText = "select dataStale from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
	'			dr = query.ExecuteReader
	'			dr.Read()
	'			basedataStale = dr.GetValue(0)

	'			'If BaseStale Or SubjectStale Or IsNothing(Session("calculated")) Then
	'			If dataStale Or basedataStale Or IsNothing(Session("calculated")) Then


	'				If doCompare = 0 Then
	'					Response.Write(common.JavascriptFunctions())
	'					Response.Flush()

	'					Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
	'					Response.Flush()
	'				End If


	'				'check if base year is stale
	'				If basedataStale Then
	'					common.calculateTaxYearModel(1, BaseTaxYearModelID, userID, SubjectTaxYearModelID)
	'				End If

	'				'check if subject year is stale
	'				If dataStale Then
	'					common.calculateTaxYearModel(0, SubjectTaxYearModelID, userID)
	'				End If

	'				'Donna start
	'				If Not enterPEMR Then
	'					If PEMRByTotalLevy Then
	'						common.calcRevenueNeutralByTotalLevy(userID, BaseTaxYearModelID)
	'					Else
	'						common.calcRevenueNeutralByClassLevy(userID, BaseTaxYearModelID)
	'					End If
	'				End If
	'				'Donna end

	'				Response.Write("&nbsp;")
	'				Response.Flush()

	'				'modified split the stored procedure	03-jul-2014
	'				'common.calcAssessmentSummary(userID, 0)
	'				common.calcAssessmentSummary_A(userID, 0)

	'				Response.Write("&nbsp;")
	'				Response.Flush()

	'				common.calcAssessmentSummary_B(userID, 0)

	'				Response.Write("&nbsp;")
	'				Response.Flush()

	'				'create School Municipality and Parcels tables for Map viewing
	'				common.Create_School_Mun_Parcel_Tables()

	'				Session.Add("calculated", "true")

	'				Response.Write("Calculation Finished")
	'				Response.Flush()

	'				Response.Write(common.HideDiv())
	'				Response.Flush()

	'				'Response.Write("<script language=javascript>window.navigate('graphs.aspx');</script>")
	'				Response.Write("<script language='javascript'>window.location.href='graphs.aspx';</script>")
	'				Response.End()

	'			Else

	'				'Keeps session for the reportviewer. ReportViewer is in a frame so session
	'				'isn't normally tracked.
	'				Response.AddHeader("P3P", "CP=""CAO PSA OUR""")

	'				Dim levelID As Integer = Session("levelID")

	'				'Provincial Analyst, Sys Admininstrator, Presentation users 
	'				'only has the permission to change scenario parameters
	'				If levelID = 49 Then
	'					btnSave.ImageUrl = "~/images/btnSaveAs.gif"
	'				ElseIf levelID > 3 Then
	'					btnSave.Visible = False
	'					txtScenarioName.Enabled = False
	'				ElseIf levelID = 3 Then
	'					'Presentation users has to re-save current scenario in 
	'					'in a different scenario name
	'					btnSave.ImageUrl = "~/images/btnSaveAs.gif"
	'				End If

	'				'Gets Tax Year Model names used by the scenario 
	'				common.GetModelNames(txtBaseTaxYrModel.Text, txtSubjectTaxYrModel.Text, Session("userID"), txtScenarioName.Text)

	'				'Set Tax Classes Filters for map
	'				common.SetTaxClassFilters()

	'				'check if K-12 OG data set is entered by the user
	'				dr.Close()
	'				query.CommandText = "select SubjectK12ID from liveAssessmentTaxModel where userid = " & userID
	'				dr = query.ExecuteReader
	'				dr.Read()

	'				If dr.GetValue(0) = 0 Then
	'					Session.Add("missingK12DataSet", "true")
	'					'con.Close()    Donna - Keep connection open.
	'					'''''Response.Redirect("kog.aspx", False) '***Inky commented-out this line
	'					'''''Exit Sub '***Inky commented-out this line
	'				End If

	'				dr.Close()

	'				'jurisdiction type group
	'				da.SelectCommand = New SqlClient.SqlCommand()
	'				da.SelectCommand.Connection = con
	'				da.SelectCommand.CommandText = "select 0 as JurisdictionGroupID, '--Jurisdiction Type Group--' as JurisdictionGroup union all select JurisdictionGroupID, JurisdictionGroup from jurisdictionGroups"
	'				da.Fill(dt)
	'				ddlJurisTypeGroup.DataSource = dt
	'				ddlJurisTypeGroup.DataValueField = "JurisdictionGroupID"
	'				ddlJurisTypeGroup.DataTextField = "JurisdictionGroup"
	'				ddlJurisTypeGroup.DataBind()

	'				If Not IsNothing(Session("JurisTypeGroup")) Then
	'					ddlJurisTypeGroup.SelectedValue = Session("JurisTypeGroup")
	'				End If

	'				'jurisdiction type
	'				dt.Clear()
	'				da.SelectCommand = New SqlClient.SqlCommand()
	'				da.SelectCommand.Connection = con
	'				da.SelectCommand.CommandText = "select 0 as jurisdictionTypeID, '--Jurisdiction Type--' as jurisdictionType union all select jurisdictionTypeID, jurisdictionType from jurisdictionTypes"
	'				da.Fill(dt)
	'				ddlJurisType.DataSource = dt
	'				ddlJurisType.DataValueField = "jurisdictionTypeID"
	'				ddlJurisType.DataTextField = "jurisdictionType"
	'				ddlJurisType.DataBind()

	'				If Not IsNothing(Session("JurisType")) Then
	'					If Not IsNothing(ddlJurisType.Items.FindByValue(Session("JurisType"))) Then
	'						ddlJurisType.SelectedValue = Session("JurisType")
	'					End If
	'				End If

	'				'municipality
	'				dt = Nothing
	'				dt = common.FillMunicipality(ddlJurisType.SelectedValue, , ddlJurisTypeGroup.SelectedValue)
	'				ddlMunicipality.DataSource = dt
	'				ddlMunicipality.DataValueField = "number"
	'				ddlMunicipality.DataTextField = "jurisdiction"
	'				ddlMunicipality.DataBind()

	'				If Not IsNothing(Session("Municipalities")) Then
	'					If Not IsNothing(ddlMunicipality.Items.FindByValue(Session("Municipalities"))) Then
	'						ddlMunicipality.SelectedValue = Session("Municipalities")
	'					End If
	'				End If

	'				'school division
	'				dt.Clear()
	'				da.SelectCommand.CommandText = "select 0 as number, '--School Division--' as jurisdiction  union all select e.number, dbo.ProperCase(e.jurisdiction) as jurisdiction from entities e where e.jurisdictionTypeID=1"
	'				da.Fill(dt)
	'				ddlSchoolDivision.DataSource = dt
	'				ddlSchoolDivision.DataValueField = "number"
	'				ddlSchoolDivision.DataTextField = "jurisdiction"
	'				ddlSchoolDivision.DataBind()

	'				If Not IsNothing(Session("SchoolDistricts")) Then
	'					If IsNumeric(Session("SchoolDistricts")) Then
	'						ddlSchoolDivision.SelectedValue = Session("SchoolDistricts")
	'					End If
	'				End If

	'				'tax status
	'				dt.Clear()
	'				da.SelectCommand.CommandText = "select 0 as taxStatusID, '--Tax Status--' as taxStatus  union all select taxStatusID, taxStatus from taxStatus"
	'				da.Fill(dt)
	'				ddlTaxStatus.DataSource = dt
	'				ddlTaxStatus.DataValueField = "taxStatusID"
	'				ddlTaxStatus.DataTextField = "taxStatus"
	'				ddlTaxStatus.DataBind()

	'				Dim selectedTaxStatus As New List(Of String)
	'				Dim counter As Integer

	'				If Not IsNothing(Session("MapTaxStatusFilters")) Then
	'					selectedTaxStatus = CType(Session("MapTaxStatusFilters"), List(Of String))

	'					For counter = 0 To selectedTaxStatus.Count - 1

	'						Select Case Trim(selectedTaxStatus(counter))
	'							Case "Provincial grant in lieu"
	'								ddlTaxStatus.SelectedValue = 6
	'								Exit For
	'							Case "Federal grant in lieu"
	'								ddlTaxStatus.SelectedValue = 5
	'								Exit For
	'							Case "Exempt"
	'								ddlTaxStatus.SelectedValue = 4
	'								Exit For
	'							Case "Taxable"
	'								ddlTaxStatus.SelectedValue = 1
	'								Exit For
	'							Case Else
	'								If Not IsNothing(ddlTaxStatus.Items.FindByValue(Trim(selectedTaxStatus(counter)))) Then
	'									ddlTaxStatus.SelectedValue = ddlTaxStatus.Items.FindByValue(Trim(selectedTaxStatus(counter))).Value
	'									Exit For
	'								End If
	'						End Select
	'					Next

	'					If ddlTaxStatus.SelectedValue > 0 Then
	'						Session("TaxStatus") = ddlTaxStatus.SelectedValue
	'					End If

	'				End If

	'				'added on to display default tax status 17-jun-2013
	'				If selectedTaxStatus.Count <= 0 Then
	'					selectedTaxStatus.Add("Taxable")
	'					ddlTaxStatus.SelectedValue = 1
	'					Session("TaxStatus") = ddlTaxStatus.SelectedValue
	'					Session("MapTaxStatusFilters") = selectedTaxStatus
	'				End If

	'				'tax classes
	'				dt.Clear()
	'				da.SelectCommand.CommandText = "select ' ' as taxClassID, '--Tax Classes--' as taxClass  union all select t.taxClassID, t.taxClass from taxClasses t, liveTaxClasses l where t.taxClassID = l.taxClassID AND l.show=1 AND userID=" & Session("userID")
	'				da.Fill(dt)
	'				ddlTaxClasses.DataSource = dt
	'				ddlTaxClasses.DataValueField = "taxClassID"
	'				ddlTaxClasses.DataTextField = "taxClass"
	'				ddlTaxClasses.DataBind()

	'				If Not IsNothing(Session("TaxClass")) Then
	'					ddlTaxClasses.SelectedValue = Session("TaxClass")
	'				End If

	'				If Not IsNothing(Session("ParcelID")) Then
	'					txtParcelNo.Text = Session("ParcelID")
	'				End If

	'				txtParcelNo.Attributes.Add("onBlur", "javascript: if (this.value == '') this.value = '-- Enter Parcel ID --';")
	'				txtParcelNo.Attributes.Add("onClick", "javascript: if (this.value == '-- Enter Parcel ID --') this.value = '';")

	'				Dim selectedTaxShift As New List(Of String)

	'				If Not IsNothing(Session("MapTaxShiftFilters")) Then

	'					selectedTaxShift = CType(Session("MapTaxShiftFilters"), List(Of String))

	'					For counter = 0 To selectedTaxShift.Count - 1
	'						Select Case Trim(selectedTaxShift(counter))
	'							Case "Municipal Tax"
	'								ddlTaxType.SelectedValue = 1
	'								Exit For
	'							Case "School Tax"
	'								ddlTaxType.SelectedValue = 2
	'								Exit For
	'							Case "Grant"
	'								ddlTaxType.SelectedValue = 3
	'								Exit For
	'							Case Else
	'								If Not IsNothing(ddlTaxType.Items.FindByValue(Trim(selectedTaxShift(counter)))) Then
	'									ddlTaxType.SelectedValue = ddlTaxType.Items.FindByValue(Trim(selectedTaxShift(counter))).Value
	'									Session("TaxShift") = ddlTaxType.Items.FindByValue(Trim(selectedTaxShift(counter))).Value
	'									Exit For
	'								End If
	'						End Select
	'					Next

	'					If ddlTaxType.SelectedValue > 0 Then
	'						Session("TaxShift") = ddlTaxType.SelectedValue
	'					End If
	'				End If

	'				'added on to display default tax type 17-jun-2013
	'				If selectedTaxShift.Count <= 0 Then
	'					selectedTaxShift.Add("Municipal Tax")
	'					ddlTaxType.SelectedValue = 1
	'					Session("TaxShift") = ddlTaxType.SelectedValue
	'					Session("MapTaxShiftFilters") = selectedTaxShift
	'				End If

	'				If IsNothing(Session("taxStatusPageID")) Then
	'					query.CommandText = "Delete from liveTaxStatus where userID = " & userID
	'					query.ExecuteNonQuery()

	'					query.CommandText = "Insert into liveTaxStatus values(" & userID & ",1,1) "
	'					query.CommandText += "Insert into liveTaxStatus values(" & userID & ",5,1) "
	'					query.CommandText += "Insert into liveTaxStatus values(" & userID & ",6,1) "
	'					query.CommandText += "Insert into liveTaxStatus values(" & userID & ",4,0) "
	'					query.ExecuteNonQuery()
	'				End If

	'				'clean up
	'				con.Close()

	'				'report types
	'				'create an instance of our web service
	'				Dim ws As New PATMAPWebService.ReportingService2005
	'				'pass in the default credentials - meaning currently logged in user
	'				'ws.Credentials = System.Net.CredentialCache.DefaultCredentials
	'				'pass in the network credentials to access the reporting service
	'				ws.Credentials = New reportServerCredentials().NetworkCredentials

	'				'checks if the folder exists in the report server
	'				If ws.GetItemType(PATMAP.Global_asax.ReportGraphsFolder) = PATMAPWebService.ItemTypeEnum.Folder Then

	'					Dim items As PATMAPWebService.CatalogItem() = ws.ListChildren(PATMAP.Global_asax.ReportGraphsFolder, True)
	'					Dim catItem As PATMAPWebService.CatalogItem

	'					Dim ddlFirstItem = New ListItem("--Report Type--", "0")
	'					ddlReport.Items.Add(ddlFirstItem)
	'					For Each catItem In items
	'						Dim ddlItem = New ListItem(catItem.Name, catItem.Path)
	'						ddlReport.Items.Add(ddlItem)
	'					Next

	'				End If

	'				Dim reportPath As String = ""

	'				If Not IsNothing(Session("reportPath")) Then

	'					reportPath = Replace(Session("reportPath"), PATMAP.Global_asax.ReportTablesFolder, PATMAP.Global_asax.ReportGraphsFolder)

	'					If Not IsNothing(ddlReport.Items.FindByValue(reportPath)) Then
	'						ddlReport.SelectedValue = reportPath
	'					End If
	'				End If

	'			End If

	'		End If
	'	Catch
	'		'retrieves error message
	'		Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
	'	End Try
	'End Sub

    Private Sub btnClasses_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClasses.Click
        Session.Add("pageID", 0)
        Response.Redirect("classes.aspx")
    End Sub

    Private Sub graphs_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If IsNothing(Session("assessmentTaxModelID")) Then
            Response.Redirect("start.aspx")
        End If
    End Sub

    Public Sub changeName(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim errCode As String

            errCode = common.UpdateScenarioName(Session("userID"), Session("assessmentTaxModelID"), txtScenarioName.Text)

            If errCode <> "" Then
                Master.errorMsg = common.GetErrorMessage(errCode)
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            Dim errCode As String

            If Trim(txtScenarioName.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP99")
                Exit Sub
            End If

            errCode = common.UpdateScenarioName(Session("userID"), Session("assessmentTaxModelID"), txtScenarioName.Text)

            If errCode <> "" Then
                Master.errorMsg = common.GetErrorMessage(errCode)
                Exit Sub
            End If

            common.saveLiveModelTables(Session("userID"))

        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("start.aspx")
    End Sub

    Private Sub ddlJurisType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJurisType.SelectedIndexChanged
        Try
            If ddlJurisType.SelectedValue = 0 Then
                Session.Remove("JurisType")
                Session.Remove("MapAnalysisLayer")
            Else
                Session("JurisType") = ddlJurisType.SelectedValue

                Select Case ddlJurisType.SelectedValue
                    Case 1
                        Session("MapAnalysisLayer") = "SchoolDivisions"

                        If ddlSchoolDivision.SelectedIndex <> 0 Then
                            Session("MapZoom") = ddlSchoolDivision.SelectedValue
                        Else
                            Session.Remove("MapZoom")
                        End If

                    Case Else
                        Session("MapAnalysisLayer") = "Municipalities"

                        If ddlMunicipality.SelectedValue <> " " Then
                            Session("MapZoom") = ddlMunicipality.SelectedValue
                        Else
                            Session.Remove("MapZoom")
                        End If
                End Select
            End If

            If ddlJurisType.SelectedValue = 0 Or ddlJurisType.SelectedValue > 1 Then
                'municipality
                Dim dt As New DataTable()

                dt = common.FillMunicipality(ddlJurisType.SelectedValue, , ddlJurisTypeGroup.SelectedValue)
                ddlMunicipality.DataSource = dt
                ddlMunicipality.DataValueField = "number"
                ddlMunicipality.DataTextField = "jurisdiction"
                ddlMunicipality.DataBind()

                If Not IsNothing(Session("Municipalities")) Then
                    If Not IsNothing(ddlMunicipality.Items.FindByValue(Session("Municipalities"))) Then
                        ddlMunicipality.SelectedValue = Session("Municipalities")
                    Else
                        Session.Remove("Municipalities")
                    End If
                End If
            End If

        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ddlSchoolDivision_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSchoolDivision.SelectedIndexChanged
        Try
            If ddlSchoolDivision.SelectedValue = 0 Then
                Session.Remove("SchoolDistricts")
            Else
                If ddlJurisType.SelectedValue = 1 Or ddlJurisType.SelectedValue = 0 Then
                    Session("MapAnalysisLayer") = "SchoolDivisions"
                    Session("MapZoom") = ddlSchoolDivision.SelectedValue
                End If

                Session("SchoolDistricts") = ddlSchoolDivision.SelectedValue
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ddlTaxStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaxStatus.SelectedIndexChanged
        Try
            If ddlTaxStatus.SelectedValue = 0 Then
                Session.Remove("MapTaxStatusFilters")
                Session.Remove("TaxStatus")
            Else
                Dim selectedTaxStatus As New List(Of String)

                Select Case Trim(ddlTaxStatus.SelectedValue)
                    Case 6
                        selectedTaxStatus.Add("Provincial grant in lieu")
                    Case 5
                        selectedTaxStatus.Add("Federal grant in lieu")
                    Case 4
                        selectedTaxStatus.Add("Exempt")
                    Case 1
                        selectedTaxStatus.Add("Taxable")
                    Case Else
                        selectedTaxStatus.Add(ddlTaxStatus.SelectedItem.Text)
                End Select

                Session("TaxStatus") = ddlTaxStatus.SelectedValue
                Session("MapTaxStatusFilters") = selectedTaxStatus
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ddlMunicipality_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMunicipality.SelectedIndexChanged
        Try
            If ddlMunicipality.SelectedValue = " " Then
                Session.Remove("Municipalities")
            Else
                If ddlJurisType.SelectedValue > 1 Or ddlJurisType.SelectedValue = 0 Then
                    Session("MapAnalysisLayer") = "Municipalities"
                    Session("MapZoom") = ddlMunicipality.SelectedValue
                End If

                Session("Municipalities") = ddlMunicipality.SelectedValue
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub txtParcelNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtParcelNo.TextChanged
        Try
            If txtParcelNo.Text = "-- Enter Parcel ID --" Then
                Session.Remove("ParcelID")
            Else
                Session("ParcelID") = txtParcelNo.Text
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ddlTaxClasses_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaxClasses.SelectedIndexChanged
        Try
            If ddlTaxClasses.SelectedValue = " " Then
                Session.Remove("TaxClass")
            Else
                Session("TaxClass") = ddlTaxClasses.SelectedValue
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ddlTaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaxType.SelectedIndexChanged
        Try
            If ddlTaxType.SelectedValue = 0 Then
                Session.Remove("MapTaxShiftFilters")
                Session.Remove("TaxShift")
            Else

                Dim selectedTaxShift As New List(Of String)

                Select Case ddlTaxType.SelectedValue
                    Case 1
                        selectedTaxShift.Add("Municipal Tax")
                    Case 2
                        selectedTaxShift.Add("School Tax")
                    Case 3
                        selectedTaxShift.Add("Grant")
                End Select

                Session("TaxShift") = ddlTaxType.SelectedValue
                Session("MapTaxShiftFilters") = selectedTaxShift
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ddlJurisTypeGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJurisTypeGroup.SelectedIndexChanged
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim filter As String = ""

        If ddlJurisTypeGroup.SelectedValue <> 0 Then
            Session("JurisTypeGroup") = ddlJurisTypeGroup.SelectedValue
            filter = " where jurisdictionGroupID = " & ddlJurisTypeGroup.SelectedValue
        Else
            Session.Remove("JurisTypeGroup")
        End If

        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandText = "select 0 as jurisdictionTypeID, '--Jurisdiction Type--' as jurisdictionType union all select jurisdictionTypeID, jurisdictionType from jurisdictionTypes " & filter
        da.Fill(dt)
        ddlJurisType.DataSource = dt
        ddlJurisType.DataValueField = "jurisdictionTypeID"
        ddlJurisType.DataTextField = "jurisdictionType"
        ddlJurisType.DataBind()

        dt.Clear()
        dt = common.FillMunicipality(ddlJurisType.SelectedValue, , ddlJurisTypeGroup.SelectedValue)
        ddlMunicipality.DataSource = dt
        ddlMunicipality.DataValueField = "number"
        ddlMunicipality.DataTextField = "jurisdiction"
        ddlMunicipality.DataBind()

        Session("MapAnalysisLayer") = "Municipalities"

        If Not IsNothing(Session("Municipalities")) Then
            If Not IsNothing(ddlMunicipality.Items.FindByValue(Session("Municipalities"))) Then
                ddlMunicipality.SelectedValue = Session("Municipalities")
            Else
                Session.Remove("Municipalities")
            End If
        End If
    End Sub

    Private Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
        If ddlReport.SelectedIndex <> 0 Then
            Session("reportPath") = ddlReport.SelectedValue
        Else
            Session.Remove("reportPath")
        End If
    End Sub
    Private Sub btnTaxStaus_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnTaxStaus.Click
        Session.Add("pageID", 0)
        Response.Redirect("taxstatus.aspx")
    End Sub
End Class