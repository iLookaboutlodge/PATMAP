Partial Public Class MapCalculate
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
				'''''Response.Redirect("kog.aspx", False) '***Inky commented-out this line
				'''''Exit Sub '***Inky commented-out this line
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
				Response.Redirect("~/Map.aspx", True)
				Return IsCalculationRequired
			Else
				If Not ShowCalculating Then
					Response.Redirect("~/Map.aspx", True)
					Return IsCalculationRequired
				End If
			End If

		Catch ex As Exception
			Throw
		End Try
		Return IsCalculationRequired
	End Function

End Class