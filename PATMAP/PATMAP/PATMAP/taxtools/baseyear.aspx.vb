
Partial Public Class baseyear
    Inherits System.Web.UI.Page

    Private bckgColor As System.Drawing.Color

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Clears out the error message
			Master.errorMsg = ""

			'added for calculating LTT tables when Map is first time loaded	17-sep-2013
			System.Web.HttpContext.Current.Session.Remove("LTTcalculated")

			If Not IsPostBack Then
				'Sets submenu to be displayed
				subMenu.setStartNode(menu.taxtools)


				'applies Subject Municipality and Subject Year to labels
				If Not IsNothing(Session("LTTSubjectMunicipality")) Then
					lblLiveSubjMun.Text = Replace(StrConv(Session("LTTSubjectMunicipality"), VbStrConv.ProperCase), "Of ", "of ")
				Else
					'hides Subject Municipality label if no Subject has been selected (precationary - shouldn't happen)
					lblSubjMun.Visible = False
					lblLiveSubjMun.Visible = False
				End If
				lblLiveSubjYr.Text = Session("LTTsubjYear")

				'fill datagrid
				fillLTTScheme()

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
				Dim subjmillRateSurveyID As Integer
				Dim basePOVID As Integer
				Dim taxableDrivedAssessment As Double
				Dim UMR As Double

				'get IDs
				query.CommandText = "select basePOVID, millRateSurveyID from boundaryModel where status = 1"
				dr = query.ExecuteReader()
				dr.Read()
				basePOVID = dr.GetValue(0)
				subjmillRateSurveyID = dr.GetValue(1)
				dr.Close()

				'If the liveLLTBase tbl already exists then just get the umr and levy to be displayed from it
				'Otherwise calculate the umr using the levy obtained from the mill rate survey tbl
				query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLLTBase_" & Session("userID").ToString & "]') AND type in (N'U')"
				dr = query.ExecuteReader
				If Not dr.Read() Then
					'get levy
					dr.Close()
					query.CommandText = "select levy from millRateSurvey where millRateSurveyID = " & subjmillRateSurveyID & " and MunicipalityID = (select number from entities where jurisdiction = '" & Replace(Session("LTTSubjectMunicipality").ToString, "'", "''") & "')"
					dr = query.ExecuteReader
					dr.Read()
					levy = dr.GetValue(0)
					dr.Close()

					'get taxable drived assment
					query.CommandText = "select sum(((marketValue)*(taxable/(taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption)))*POV) as taxableDrivedAssessment from assessmentLTTBase_" & Session("userID") & " a inner join POV on POV.taxClassID = a.taxClassID and POVID = " & basePOVID.ToString
					dr = query.ExecuteReader()
					dr.Read()
					taxableDrivedAssessment = dr.GetValue(0)
					dr.Close()

					'calculate UMR
					UMR = (levy / taxableDrivedAssessment)

					query.CommandText = "execute populateLiveLTTBase " & Session("userID").ToString & "," & basePOVID.ToString & "," & subjmillRateSurveyID.ToString
					query.ExecuteNonQuery()
				Else
					dr.Close()
					query.CommandText = "SELECT UMR, Levy from liveLLTBase_" & Session("userID").ToString & " group by UMR, Levy"
					dr = query.ExecuteReader
					dr.Read()
					UMR = dr.GetValue(0)
					levy = dr.GetValue(1)
				End If

				txtBaseYrUniformMillRate.Text = FormatNumber((UMR * 1000), 4)
				txtBaseYrMunicipalRevenue.Text = FormatNumber(levy, 2)

				Session.Add("LTToriginalUMR", UMR)
				Session.Add("LTToriginalLevy", levy)

				con.Close()

			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub
    Private Sub fillLTTScheme()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim query As String
        Dim userID As Integer = Session("userID")

        If Session("showFullTaxClasses") Then
            query = "SELECT liveLTTValues.taxClassID, taxClass, baseBaseTax, baseMinTax, baseMRF FROM liveLTTValues INNER JOIN LTTtaxClasses ON LTTtaxClasses.taxClassID = liveLTTValues.taxClassID INNER JOIN liveLTTtaxClasses_" & userID & " ON liveLTTtaxClasses_" & userID & ".taxClassID = LTTtaxClasses.taxClassID WHERE LTTtaxClasses.active = 1 AND liveLTTtaxClasses_" & userID & ".show = 1 AND userID = '" & userID & "'"

        Else
            query = "SELECT DISTINCT LTTmainTaxClasses.taxClassID, LTTmainTaxClasses.taxClass, baseBaseTax, baseMinTax, baseMRF FROM liveLTTValues INNER JOIN LTTtaxClasses ON LTTtaxClasses.taxClassID = liveLTTValues.taxClassID INNER JOIN LTTmainTaxClasses ON LTTmainTaxClasses.taxClassID = LTTtaxClasses.LTTmainTaxClassID INNER JOIN liveLTTtaxClasses_" & userID & " ON liveLTTtaxClasses_" & userID & ".taxClassID = LTTtaxClasses.taxClassID WHERE LTTtaxClasses.active = 1 AND liveLTTtaxClasses_" & userID & ".show = 1 AND userID = '" & userID & "'"
        End If

        'fill in the tax class table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        con.Close()

        Session.Add("BaseYrValues", dt)
        grdLTTScheme.DataSource = dt
        grdLTTScheme.DataBind()

    End Sub

	Protected Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click

		validateGrid()

		updateLiveLTTValues()

		Dim UMR As Double = Trim(txtBaseYrUniformMillRate.Text)
		UMR = (UMR / 1000)

		Dim Levy As Double = Trim(txtBaseYrMunicipalRevenue.Text)

		If Session("validBaseYrValues") Then
			'setup database connection
			Dim con As New SqlClient.SqlConnection
			Dim query As New SqlClient.SqlCommand
			Dim dr As SqlClient.SqlDataReader

			con.ConnectionString = PATMAP.Global_asax.connString
			query.Connection = con
			query.CommandTimeout = 60000
			con.Open()

			Dim prevUMR As Double = 0.0
			Dim currUMR As Double = 0.0
			'calculate and update the liveLLTBase tbl with the new levy/UMR (if the user edited any of those values) plus base, min MRF parameters
			If rdoEditUniformMillRate.Checked = True Then
				'calculate new levy

				'update the levy in liveLLTBase
				query.CommandText = "execute updateLiveLTTBase " & Session("userID").ToString & "," & UMR & ",0,-1,0"
				query.ExecuteNonQuery()

			ElseIf rdoEditMunicipalRevenue.Checked = True Then
				'calculate new UMR

				'get taxableDrivedAssessment
				Dim taxableDrivedAssessment As Double
				query.CommandText = "select sum(taxableDA) from liveLLTBase_" & Session("userID").ToString
				dr = query.ExecuteReader
				dr.Read()
				taxableDrivedAssessment = dr.GetValue(0)
				dr.Close()

				Dim newUmr As Double = 0.0
				'Inky's Code -- ADDED double.parse
				newUmr = FormatNumber(Levy / taxableDrivedAssessment)

				'update the umr in liveLLTBase
				query.CommandText = "execute updateLiveLTTBase " & Session("userID").ToString & "," & newUmr & "," & Levy & ",-1,0"
				query.ExecuteNonQuery()

				currUMR = newUmr
			Else
				'update the umr in liveLLTBase
				query.CommandText = "execute updateLiveLTTBase " & Session("userID").ToString & ",0,0,-1,0"
				query.ExecuteNonQuery()

				query.CommandText = "select UMR from liveLLTBase_" & Session("userID").ToString & " group by UMR"
				dr = query.ExecuteReader
				dr.Read()
				currUMR = dr.GetValue(0)
				dr.Close()
			End If

			If rdoEditUniformMillRate.Checked <> True Then

				'continue the iteration until
				'UMR from the prev iter is the same as the current one OR
				'UMR is 0 => base + min tax > levy

				'just set the currUMR to some odd number so that the iteration will be triggered
				prevUMR = -99.9999

				Dim iterNum = 0
				While prevUMR <> currUMR Or currUMR = 0
					iterNum = iterNum + 1
					query.CommandText = "execute updateLiveLTTBase " & Session("userID").ToString & ",0,0," & iterNum & ",0"
					query.ExecuteNonQuery()

					prevUMR = currUMR
					query.CommandText = "select UMR from liveLLTBase_" & Session("userID").ToString & " group by UMR"
					dr = query.ExecuteReader
					dr.Read()
					currUMR = dr.GetValue(0)
					dr.Close()
				End While

				'If currUMR = 0 Then
				'    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP323")
				'    Exit Sub
				'End If

				'after calculation is completed, update the total tax
				query.CommandText = "execute updateLiveLTTBase " & Session("userID").ToString & ",0,0,0,0"
				query.ExecuteNonQuery()
			Else

				'after calculation is completed, update the total tax
				query.CommandText = "execute updateLiveLTTBase " & Session("userID").ToString & ",0,0,0,1"
				query.ExecuteNonQuery()

			End If

			con.Close()

		End If

		If Session("updateComplete") Then

			'remove unnecessary session variables
			Session.Remove("validBaseYrValues")
			Session.Remove("updateComplete")
			Session.Remove("LTToriginalUMR")
			Session.Remove("LTToriginalLevy")

			'redirect to Start Tax page
			Response.Redirect("starttax.aspx")
		End If
	End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click

        'Reset radio buttons
        rdoEditMunicipalRevenue.Checked = False
        rdoEditUniformMillRate.Checked = False

        'Reset UMR and Municipal Revenue (aka Levy) to original values
        txtBaseYrUniformMillRate.Text = FormatNumber(Session("LTToriginalUMR"), 4)
        txtBaseYrMunicipalRevenue.Text = FormatNumber(Session("LTToriginalLevy"), 2)
        txtBaseYrMunicipalRevenue.Enabled = False
        txtBaseYrUniformMillRate.Enabled = False

        'reset grdLTTScheme (textboxes and radio buttons)
        Dim i As Integer = 0

        For i = 0 To grdLTTScheme.Rows.Count - 1
            If (grdLTTScheme.Rows(i).RowType = DataControlRowType.DataRow) Then
                CType(grdLTTScheme.Rows(i).Cells(1).FindControl("txtMinTax"), TextBox).Text = "0"
                CType(grdLTTScheme.Rows(i).Cells(2).FindControl("txtBaseTax"), TextBox).Text = "0"
                CType(grdLTTScheme.Rows(i).Cells(3).FindControl("txtMRF"), TextBox).Text = FormatNumber(1, 4)
            End If
        Next

    End Sub

    Private Sub rdoEditMunicipalRevenue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoEditMunicipalRevenue.CheckedChanged
        Try
            txtBaseYrMunicipalRevenue.Enabled = True
            txtBaseYrMunicipalRevenue.Focus()
            txtBaseYrUniformMillRate.Text = FormatNumber(Session("LTToriginalUMR"), 4)
            txtBaseYrUniformMillRate.Enabled = False
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub rdoEditUniformMillRate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoEditUniformMillRate.CheckedChanged
        Try
            txtBaseYrUniformMillRate.Enabled = True
            txtBaseYrUniformMillRate.Focus()
            txtBaseYrMunicipalRevenue.Text = FormatNumber(Session("LTToriginalLevy"), 2)
            txtBaseYrMunicipalRevenue.Enabled = False
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub updateLiveLTTValues()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        con.Open()

        Dim userID As Integer = Session("userID")
        Dim i As Integer
        Dim minTax, baseTax, MRF, taxClass As String
        Session.Add("updateComplete", False)

        'performs validation if not already completed
        If IsNothing(Session("validBaseYrValues")) Then
            validateGrid()
        End If

        'grab values from text boxes
        If Session("validBaseYrValues") Then

            For i = 0 To grdLTTScheme.Rows.Count - 1
                If (grdLTTScheme.Rows(i).RowType = DataControlRowType.DataRow) Then

                    baseTax = Double.Parse(Trim(CType(grdLTTScheme.Rows(i).FindControl("txtBaseTax"), TextBox).Text)).ToString
                    minTax = Double.Parse(Trim(CType(grdLTTScheme.Rows(i).FindControl("txtMinTax"), TextBox).Text)).ToString
                    MRF = Double.Parse(Trim(CType(grdLTTScheme.Rows(i).FindControl("txtMRF"), TextBox).Text)).ToString
                    taxClass = grdLTTScheme.DataKeys(i).Values("taxClassID").ToString()

                    If Session("showFullTaxClasses") Then
                        query.CommandText += "UPDATE liveLTTValues SET baseBaseTax = " & baseTax & ", baseMinTax = " & minTax & ", baseMRF = " & MRF & " WHERE userID = '" & userID & "' and taxClassID = '" & taxClass & "'" & vbCrLf

                    Else
                        query.CommandText += "UPDATE liveLTTValues SET baseBaseTax = " & baseTax & ", baseMinTax = " & minTax & ", baseMRF = " & MRF & vbCrLf & _
                                             "WHERE userID = '" & userID & "' and taxClassID IN (select taxClassID from liveLTTtaxClasses_" & userID & " where LTTmainTaxClassID = '" & taxClass & "') " & vbCrLf & _
                                             "OR taxClassID IN (select taxClassID from LTTtaxClasses where LTTmainTaxClassID = " & taxClass & " and active = 0)" & vbCrLf
                    End If
                End If
            Next
            query.ExecuteNonQuery()

            'update inactive subclasses to be assigned parent class values, if applicable...
            If Session("showFullTaxClasses") Then
                Dim parentID As String = ""
                Dim childID As String = ""
                Dim dr As SqlClient.SqlDataReader

                query.CommandText = "SELECT parentTaxClassID, taxClassID FROM LTTtaxClasses WHERE active = 0 AND parentTaxClassID <> 'none'"
                dr = query.ExecuteReader()
                query.CommandText = ""

                If dr.HasRows Then
                    While dr.Read
                        parentID = dr.GetValue(0)
                        childID = dr.GetValue(1)
                        query.CommandText += "UPDATE t1 SET t1.baseBaseTax = t2.baseBaseTax, t1.baseMinTax = t2.baseMinTax, t1.baseMRF = t2.baseMRF" & vbCrLf & _
                                             "FROM liveLTTValues t1 JOIN liveLTTValues t2 on t2.userID = t1.userID" & vbCrLf & _
                                             "WHERE t1.taxClassID = '" & childID & "' AND t2.taxClassID = '" & parentID & "' AND t1.userID = " & userID & vbCrLf
                    End While
                    dr.Close()
                    query.ExecuteNonQuery()
                End If
            End If

            Session("updateComplete") = True
        Else
            Session("updateComplete") = False
        End If
    End Sub


    Protected Sub validateGrid()
        Try
            Dim minTax, baseTax, MRF As String
            Dim i As Integer
            Session.Add("validBaseYrValues", False)

            'grab values from text boxes
            For i = 0 To grdLTTScheme.Rows.Count - 1
                If (grdLTTScheme.Rows(i).RowType = DataControlRowType.DataRow) Then

                    baseTax = Trim(CType(grdLTTScheme.Rows(i).FindControl("txtBaseTax"), TextBox).Text)
                    minTax = Trim(CType(grdLTTScheme.Rows(i).FindControl("txtMinTax"), TextBox).Text)
                    MRF = Trim(CType(grdLTTScheme.Rows(i).FindControl("txtMRF"), TextBox).Text)

                    'validation
                    If Not IsNumeric(baseTax) Then
                        Master.errorMsg = common.GetErrorMessage("PATMAP72")
                        CType(grdLTTScheme.Rows(i).FindControl("txtBaseTax"), TextBox).Focus()
                        CType(grdLTTScheme.Rows(i).FindControl("txtBaseTax"), TextBox).Attributes.Add("onFocusIn", "select();")
                        Session("validBaseYrValues") = False
                        Exit Sub

                    ElseIf Not IsNumeric(minTax) Then
                        Master.errorMsg = common.GetErrorMessage("PATMAP72")
                        CType(grdLTTScheme.Rows(i).FindControl("txtMinTax"), TextBox).Focus()
                        CType(grdLTTScheme.Rows(i).FindControl("txtMinTax"), TextBox).Attributes.Add("onFocusIn", "select();")
                        Session("validBaseYrValues") = False
                        Exit Sub

                    ElseIf Not IsNumeric(MRF) Then
                        Master.errorMsg = common.GetErrorMessage("PATMAP72")
                        CType(grdLTTScheme.Rows(i).FindControl("txtMRF"), TextBox).Focus()
                        CType(grdLTTScheme.Rows(i).FindControl("txtMRF"), TextBox).Attributes.Add("onFocusIn", "select();")
                        Session("validBaseYrValues") = False
                        Exit Sub
                    Else
                        If IsNumeric(MRF) And MRF < 0.01 Then
                            Master.errorMsg = common.GetErrorMessage("PATMAP147")
                            CType(grdLTTScheme.Rows(i).FindControl("txtMRF"), TextBox).Focus()
                            CType(grdLTTScheme.Rows(i).FindControl("txtMRF"), TextBox).Attributes.Add("onFocusIn", "select();")
                            Session("validBaseYrValues") = False
                            Exit Sub
                        End If

                    End If
                    Session("validBaseYrValues") = True
                End If
            Next

            'validate UMR and Municipal Revenue textboxes
            If Not IsNumeric(txtBaseYrUniformMillRate.Text) Then
                Master.errorMsg = common.GetErrorMessage("PATMAP141")
                txtBaseYrUniformMillRate.Focus()
                txtBaseYrUniformMillRate.Attributes.Add("onFocusIn", "select();")
                Session("validBaseYrValues") = False
                Exit Sub
            End If

            If Not IsNumeric(txtBaseYrMunicipalRevenue.Text) Then
                Master.errorMsg = common.GetErrorMessage("PATMAP142")
                txtBaseYrMunicipalRevenue.Focus()
                txtBaseYrMunicipalRevenue.Attributes.Add("onFocusIn", "select();")
                Session("validBaseYrValues") = False
                Exit Sub
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

End Class