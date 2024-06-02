Partial Public Class starttax
    Inherits System.Web.UI.Page

    Public validEntry As Boolean

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Clears out the error message
			Master.errorMsg = ""

			'added for calculating LTT tables when Map is first time loaded	17-sep-2013
			System.Web.HttpContext.Current.Session.Remove("LTTcalculated")

			If Not IsPostBack Then
				'Sets submenu to be displayed
				subMenu.setStartNode(menu.taxtools)

				'Set up textboxes for first-time display
				txtEditUniformMillRate.Text = FormatNumber(0.0, 4)
				txtEditUniformMillRate.Enabled = False

				txtEditMunicipalRevenue.Text = FormatCurrency("10000000", 0, TriState.True, TriState.True, TriState.True)
				txtEditMunicipalRevenue.Enabled = False

				If Not IsNothing(Session("LTTSubjectMunicipality")) Then
					lblLiveSubjMun.Text = Replace(StrConv(Session("LTTSubjectMunicipality"), VbStrConv.ProperCase), "Of ", "of ")

				Else
					'hides Subject Municipality label if no Subject has been selected (precationary - shouldn't happen)
					lblSubjMun.Visible = False
					lblLiveSubjMun.Visible = False
				End If
				lblLiveSubjYr.Text = Session("LTTsubjYear")

				'load Municipal Rev and UMR
				'setup database connection
				Dim con As New SqlClient.SqlConnection
				Dim query As New SqlClient.SqlCommand
				Dim dr As SqlClient.SqlDataReader

				con.ConnectionString = PATMAP.Global_asax.connString
				query.Connection = con
				query.CommandTimeout = 60000
				con.Open()

				Dim levy As Double
				Dim subjAssessmentID As Integer
				Dim baseAssessmentID As Integer
				Dim subjmillRateSurveyID As Integer
				Dim subjPOVID As Integer
				Dim taxableDrivedAssessment As Double
				Dim UMR As Double
				Dim revUMR As Double
				Dim revLevy As Double

				'get IDs
				query.CommandText = "select assessmentID, baseAssessmentID, POVID, millRateSurveyID from boundaryModel where status = 1"
				dr = query.ExecuteReader()
				dr.Read()
				subjAssessmentID = dr.GetValue(0)
				baseAssessmentID = dr.GetValue(1)
				subjPOVID = dr.GetValue(2)
				subjmillRateSurveyID = dr.GetValue(3)
				dr.Close()

				query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLLTSubject_" & Session("userID").ToString & "]') AND type in (N'U')"
				dr = query.ExecuteReader
				If dr.Read Then

					'get levy
					dr.Close()
					query.CommandText = "SELECT UMR, Levy from liveLLTSubject_" & Session("userID").ToString & " group by UMR, Levy"
					dr = query.ExecuteReader
					dr.Read()
					UMR = dr.GetValue(0)
					levy = dr.GetValue(1)
					dr.Close()

				Else

					'get levy - carried over from the base year or retrieved from the millratesurvey tbl
					dr.Close()
					If Session("phaseInBaseYearAccess") = True Then
						'levy is comming from the base year
						query.CommandText = "select Levy from liveLLTBase_" & Session("userID").ToString & " group by Levy"
						dr = query.ExecuteReader()
						dr.Read()
						levy = dr.GetValue(0)
						dr.Close()
					Else
						'levy is comming from millratesurvey tbl

						If Not IsNothing(Session("boundarySelection")) Then
							query.CommandText = "select restructuredLevy from boundaryGroups where boundaryGroupID = '" & Session("boundarySelection").ToString & "' and userID = " & Session("userID")
							dr = query.ExecuteReader
							dr.Read()
							levy = dr.GetValue(0)
							dr.Close()
						Else
							query.CommandText = "select levy from millRateSurvey where millRateSurveyID = " & subjmillRateSurveyID & " and MunicipalityID = (select number from entities where jurisdiction = '" & Replace(Session("LTTSubjectMunicipality").ToString, "'", "''") & "')"
							dr = query.ExecuteReader
							dr.Read()
							levy = dr.GetValue(0)
							dr.Close()
						End If
					End If

					If Not IsNothing(Session("boundarySelection")) Then
						query.CommandText = "select assessment from boundaryGroups where boundaryGroupID = '" & Session("boundarySelection").ToString & "' and userID = " & Session("userID")
						dr = query.ExecuteReader
						dr.Read()
						taxableDrivedAssessment = dr.GetValue(0)
						dr.Close()
					Else
						'get taxable drived assment
						query.CommandText = "select sum(((marketValue)*(taxable/(taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption)))*POV) as taxableDrivedAssessment from assessmentLTTSubject_" & Session("userID") & " a inner join POV on POV.taxClassID = a.taxClassID and POVID = " & subjPOVID.ToString
						dr = query.ExecuteReader()
						dr.Read()
						taxableDrivedAssessment = dr.GetValue(0)
						dr.Close()
					End If


					'calculate UMR
					If taxableDrivedAssessment > 0 Then
						UMR = (levy / taxableDrivedAssessment)
					Else
						UMR = 0
					End If

					If Not IsNothing(Session("boundarySelection")) Then
						query.CommandText = "select uniformMillRate, restructuredLevy from boundaryGroups where boundaryGroupID = '" & Session("boundarySelection").ToString & "' and userID = " & Session("userID")
						dr = query.ExecuteReader
						dr.Read()
						revUMR = dr.GetValue(0) / 1000
						revLevy = dr.GetValue(1)
						dr.Close()
					Else
						revUMR = UMR
						revLevy = levy
					End If


					Session.Add("orgSubjUMR", UMR)
					Session.Add("orgSubjLevy", levy)
					Session.Add("revSubjUMR", revUMR)
					Session.Add("revSubjLevy", revLevy)
					'Session.Add("revSubjUMR", UMR)
					'Session.Add("revSubjLevy", levy)

				End If

				'populate the fields on the page
				txtUniformMillRate.Text = FormatNumber((Session("orgSubjUMR") * 1000), 4)
				txtMunicipalRevenue.Text = FormatNumber(Session("orgSubjLevy"), 2)
				txtEditUniformMillRate.Text = FormatNumber((Session("revSubjUMR") * 1000), 4)
				txtEditMunicipalRevenue.Text = FormatNumber(Session("revSubjLevy"), 2)
				Session.Add("LTToriginalUMR", UMR)
				Session.Add("LTToriginalLevy", levy)

				con.Close()
			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

    Protected Sub rdoEditUniformMillRate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoEditUniformMillRate.CheckedChanged
        Try
            txtEditUniformMillRate.Enabled = True

            txtEditMunicipalRevenue.Text = FormatNumber(Session("LTToriginalLevy"), 2)
            txtEditMunicipalRevenue.Enabled = False
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub rdoEditMunicipalRevenue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoEditMunicipalRevenue.CheckedChanged
        Try
            txtEditMunicipalRevenue.Enabled = True

            txtEditUniformMillRate.Text = FormatNumber(Session("LTToriginalUMR"), 4)
            txtEditUniformMillRate.Enabled = False
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

	Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
		Dim strEntry As String = ""

		'Validates user entry to ensure values are numeric.
		If txtEditUniformMillRate.Enabled = True Then
			strEntry = Trim(txtEditUniformMillRate.Text)
			ValidateEntry(strEntry, "PATMAP141")

		ElseIf txtEditMunicipalRevenue.Enabled = True Then
			strEntry = Trim(txtEditMunicipalRevenue.Text)
			ValidateEntry(txtEditMunicipalRevenue.Text, "PATMAP142")

		Else
			'Edit feature was not used, data stored from the PATMAP Calculated function to be used in calulcations...
			validEntry = True
			'Val(lblUniformMillRate.Text)
			'Val(lblMunicipalRevenue.Text)
		End If

		If validEntry Then
			'setup database connection
			Dim con As New SqlClient.SqlConnection
			Dim query As New SqlClient.SqlCommand
			Dim dr As SqlClient.SqlDataReader

			con.ConnectionString = PATMAP.Global_asax.connString
			query.Connection = con
			query.CommandTimeout = 60000
			con.Open()

			Dim subjAssessmentID As Integer
			Dim baseAssessmentID As Integer
			Dim subjmillRateSurveyID As Integer
			Dim subjPOVID As Integer

			'get IDs
			query.CommandText = "select assessmentID, baseAssessmentID, POVID, millRateSurveyID from boundaryModel where status = 1"
			dr = query.ExecuteReader()
			dr.Read()
			subjAssessmentID = dr.GetValue(0)
			baseAssessmentID = dr.GetValue(1)
			subjPOVID = dr.GetValue(2)
			subjmillRateSurveyID = dr.GetValue(3)
			dr.Close()

			query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLLTSubject_" & Session("userID").ToString & "]') AND type in (N'U')"
			dr = query.ExecuteReader
			'If it the very first time the subject tax is being calculated or the user edited original values for the subject UMR/Levy,
			'then populate the subject table again with original values of the subject UMR & Levy first (ie: reset the values in the subject table)
			'Then update the subject table with the modified UMR/Levy
			If Not dr.Read Or rdoEditUniformMillRate.Checked = True Or rdoEditMunicipalRevenue.Checked = True Then

				dr.Close()
				query.CommandText = "execute populateLiveLTTSubject " & Session("userID").ToString & "," & subjPOVID.ToString & "," & Session("orgSubjUMR").ToString() & "," & Session("orgSubjLevy").ToString()
				query.ExecuteNonQuery()

			End If
			dr.Close()

			'calculate the new levy/UMR if the user edited any of those values
			If rdoEditUniformMillRate.Checked = True Then
				'calculate new levy

				'update the levy in liveLLTSubject
				query.CommandText = "execute updateLiveLTTSubject " & Session("userID").ToString & "," & (Double.Parse(txtEditUniformMillRate.Text) / 1000) & ",0"
				query.ExecuteNonQuery()

			ElseIf rdoEditMunicipalRevenue.Checked = True Then
				'calculate new UMR

				'get taxableDrivedAssessment
				Dim taxableDrivedAssessment As Double
				query.CommandText = "select sum(taxableDA) from liveLLTSubject_" & Session("userID").ToString
				dr = query.ExecuteReader
				dr.Read()
				taxableDrivedAssessment = dr.GetValue(0)
				dr.Close()

				Dim newUmr As Double
				newUmr = (Double.Parse(txtEditMunicipalRevenue.Text) / taxableDrivedAssessment)

				'update the umr in liveLLTSubject
				query.CommandText = "execute updateLiveLTTSubject " & Session("userID").ToString & "," & newUmr & "," & Double.Parse(txtEditMunicipalRevenue.Text)
				query.ExecuteNonQuery()
			Else
				'update the levy in liveLLTSubject
				query.CommandText = "execute updateLiveLTTSubject " & Session("userID").ToString & "," & Session("revSubjUMR").ToString & "," & Session("revSubjLevy").ToString
				query.ExecuteNonQuery()
			End If

			query.CommandText = "select UMR, Levy from liveLLTSubject_" & Session("userID").ToString
			dr = query.ExecuteReader()
			Dim revSubjUMR As Double = 0
			Dim revSubjLevy As Double = 0
			If dr.Read() Then
				revSubjUMR = common.ToDouble(dr.GetValue(0))
				revSubjLevy = common.ToDouble(dr.GetValue(1))
			End If
			dr.Close()

			Session.Remove("revSubjUMR")
			Session.Add("revSubjUMR", revSubjUMR)
			Session.Remove("revSubjLevy")
			Session.Add("revSubjLevy", revSubjLevy)

			'If the subject model tax is already calculated and the user edited the subject UMR/Levy on Subject Tax page,
			'Then recalculate the subject model tax 
			query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLLTSubjectModel_" & Session("userID").ToString & "]') AND type in (N'U')"
			dr = query.ExecuteReader
			If dr.Read() Then
				dr.Close()
				If rdoEditUniformMillRate.Checked = True Or rdoEditMunicipalRevenue.Checked = True Then
					common.calculateSubjectModelTax(revSubjUMR)
				End If
			Else
				common.calculateSubjectModelTax(revSubjUMR)
			End If
			dr.Close()


			'set session variables for subject values
			If IsNothing(Session("LTTsubjUMR")) Then
				Session.Add("LTTsubjUMR", FormatNumber(revSubjUMR * 1000, 4))
			Else
				Session("LTTsubjUMR") = FormatNumber(revSubjUMR * 1000, 4)
			End If

			If IsNothing(Session("LTTSubjMunRev")) Then
				Session.Add("LTTSubjMunRev", (FormatCurrency(revSubjLevy, 2)))
			Else
				Session("LTTSubjMunRev") = FormatCurrency(revSubjLevy, 2)
			End If

			Session.Remove("LTToriginalUMR")
			Session.Remove("LTToriginalLevy")

			'Redirect user to the next available LTT screen. Passes the current pages screenID and the current sessionID
			common.gotoNextPage(9, 106, Session("levelID"))

		End If

	End Sub
    Private Function ValidateEntry(ByVal editValue As String, ByVal errorCode As String) As Boolean

        If Not IsNumeric(editValue) Then 'then <> "" And Not Regex.IsMatch(Trim(editValue), "^\d*$|^\d*\.\d+$") Then
            Master.errorMsg = PATMAP.common.GetErrorMessage(errorCode)
            validEntry = False
            Exit Function
        Else
            validEntry = True
            Exit Function
        End If

    End Function

    Protected Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
        
        'reset radio buttons
        rdoEditMunicipalRevenue.Checked = False
        rdoEditUniformMillRate.Checked = False

        'reset textboxes
        txtEditMunicipalRevenue.Text = FormatNumber(0, 2)
        txtEditUniformMillRate.Text = FormatNumber(0, 4)
        txtEditMunicipalRevenue.Enabled = False
        txtEditUniformMillRate.Enabled = False

    End Sub

End Class
