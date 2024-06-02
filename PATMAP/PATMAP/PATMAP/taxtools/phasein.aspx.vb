Partial Public Class phasein
    Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Clears out the error message
			Master.errorMsg = ""

			'added for calculating LTT tables when Map is first time loaded	17-sep-2013
			System.Web.HttpContext.Current.Session.Remove("LTTcalculated")

			If Not IsPostBack Then
				'Sets submenu to be displayed
				subMenu.setStartNode(menu.taxtools)

				'applies Subject Municipality to labels - formats as Proper Case
				If Not IsNothing(Session("LTTSubjectMunicipality")) Then
					lblLiveSubjMun.Text = Replace(StrConv(Session("LTTSubjectMunicipality"), VbStrConv.ProperCase), "Of ", "of ")

				Else
					'hides Subject Municipality label if no Subject has been selected (precationary - shouldn't happen)
					lblSubjMun.Visible = False
					lblLiveSubjMun.Visible = False
				End If
				lblLiveSubjYr.Text = Session("LTTsubjYear")
				lblLiveStartingRevenue.Text = Session("LTTSubjMunRev")
				lblLiveStartingUniformMillRate.Text = Session("LTTSubjUMR")

				'load Repeater control table
				fillPhaseInTable()

				'setup database connection
				Dim con As New SqlClient.SqlConnection
				con.ConnectionString = PATMAP.Global_asax.connString

				Dim query As New SqlClient.SqlCommand
				query.Connection = con
				con.Open()

				query.CommandText = "execute calcPhaseIn " & Session("userID").ToString	'creates and populates liveLTTPhaseIn_(userID) table
				query.ExecuteNonQuery()

				query.CommandText = "execute calcLTTPhaseInSummary " & Session("userID").ToString
				query.ExecuteNonQuery()

				query.CommandText = "execute calcLTTResult " & Session("userID").ToString
				query.ExecuteNonQuery()

				query.CommandText = "execute calcLTTSummary " & Session("userID").ToString
				query.ExecuteNonQuery()

				'load phase in Summary table
				fillPhaseInSummary()

			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub


    Protected Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Try

            validationSequence(sender, e)
            updatePhaseInValues()

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString

            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            query.CommandText = "execute calcPhaseIn " & Session("userID").ToString
            query.ExecuteNonQuery()

            query.CommandText = "execute calcLTTPhaseInSummary " & Session("userID").ToString
            query.ExecuteNonQuery()

            query.CommandText = "execute calcLTTResult " & Session("userID").ToString
            query.ExecuteNonQuery()

            query.CommandText = "execute calcLTTSummary " & Session("userID").ToString
            query.ExecuteNonQuery()

            If Session("updateComplete") Then
                Session.Remove("validLTTvalues")
                Session.Remove("updateComplete")

                'Redirect user to the next available LTT screen. Passes the current pages screenID and the current  user levelID
                common.gotoNextPage(9, 111, Session("levelID"))

                'Response.Redirect("tables.aspx")
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub updatePhaseInValues()

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        con.Open()

        Dim userID As Integer = Session("userID")

        'performs validation if not already completed
        If IsNothing(Session("validLTTValues")) Then
            validateGrid()
        End If

        Dim i As Integer
        Dim taxClassID As String
        Dim thresholdIncr, thresholdDecr As String
        Dim incrYr1, incrYr2, incrYr3, incrYr4 As String
        Dim decrYr1, decrYr2, decrYr3, decrYr4 As String
        Dim yrsSelectedIncr, yrsSelectedDecr As Integer

        Session.Add("updateComplete", False)

        'If phaseIn user-values are valid, grab data from text boxes and update table
        If Session("validLTTValues") Then

            'set grid values and update table
            For i = 0 To rptPhaseIn.Items.Count - 1

                'assign valid data to variables
                taxClassID = CType(rptPhaseIn.Items(i).FindControl("lblTaxClassID"), Label).Text 'hidden control in the repeater holds taxClassID value

                thresholdIncr = Double.Parse(Trim(CType(rptPhaseIn.Items(i).FindControl("txtThresholdIncr"), TextBox).Text)).ToString
                incrYr1 = (Double.Parse(Trim(CType(rptPhaseIn.Items(i).FindControl("txtIncrYr1"), TextBox).Text)) / 100).ToString
                incrYr2 = (Double.Parse(Trim(CType(rptPhaseIn.Items(i).FindControl("txtIncrYr2"), TextBox).Text)) / 100).ToString
                incrYr3 = (Double.Parse(Trim(CType(rptPhaseIn.Items(i).FindControl("txtIncrYr3"), TextBox).Text)) / 100).ToString
                incrYr4 = (Double.Parse(Trim(CType(rptPhaseIn.Items(i).FindControl("txtIncrYr4"), TextBox).Text)) / 100).ToString
                yrsSelectedIncr = CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue

                thresholdDecr = Double.Parse(Trim(CType(rptPhaseIn.Items(i).FindControl("txtThresholdDecr"), TextBox).Text)).ToString
                decrYr1 = (Double.Parse(Trim(CType(rptPhaseIn.Items(i).FindControl("txtDecrYr1"), TextBox).Text)) / 100).ToString
                decrYr2 = (Double.Parse(Trim(CType(rptPhaseIn.Items(i).FindControl("txtDecrYr2"), TextBox).Text)) / 100).ToString
                decrYr3 = (Double.Parse(Trim(CType(rptPhaseIn.Items(i).FindControl("txtDecrYr3"), TextBox).Text)) / 100).ToString
                decrYr4 = (Double.Parse(Trim(CType(rptPhaseIn.Items(i).FindControl("txtDecrYr4"), TextBox).Text)) / 100).ToString
                yrsSelectedDecr = CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue

                'write values to PhaseIn table
                If Session("showFullTaxClasses") Then
                    query.CommandText += "UPDATE liveLTTPhaseInValues SET thresholdIncrease = " & thresholdIncr & ", thresholdDecrease = " & thresholdDecr & _
                            ", Y1Increase = " & incrYr1 & ", Y1Decrease = " & decrYr1 & ",Y2Increase = " & incrYr2 & ", Y2Decrease = " & decrYr2 & _
                            ", Y3Increase = " & incrYr3 & ", Y3Decrease = " & decrYr3 & ",Y4Increase = " & incrYr4 & ", Y4Decrease = " & decrYr4 & _
                            ", phaseInYrsIncrease = " & yrsSelectedIncr & ", phaseInYrsDecrease = " & yrsSelectedDecr & _
                            " WHERE userID = '" & userID & "' and taxClassID = '" & taxClassID & "'" & vbCrLf
                Else
                    query.CommandText += "UPDATE liveLTTPhaseInValues SET thresholdIncrease = " & thresholdIncr & ", thresholdDecrease = " & thresholdDecr & _
                            ", Y1Increase = " & incrYr1 & ", Y1Decrease = " & decrYr1 & ",Y2Increase = " & incrYr2 & ", Y2Decrease = " & decrYr2 & _
                            ", Y3Increase = " & incrYr3 & ", Y3Decrease = " & decrYr3 & ",Y4Increase = " & incrYr4 & ", Y4Decrease = " & decrYr4 & _
                            ", phaseInYrsIncrease = " & yrsSelectedIncr & ", phaseInYrsDecrease = " & yrsSelectedDecr & _
                            " WHERE userID = '" & userID & "' and taxClassID IN (select taxClassID from liveLTTtaxClasses_" & userID & " where LTTmainTaxClassID = '" & taxClassID & "')" & vbCrLf & _
                            " OR taxClassID IN (select taxClassID from LTTtaxClasses where LTTmainTaxClassID = " & taxClassID & " and active = 0)" & vbCrLf
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
                        query.CommandText += "UPDATE p1 SET p1.thresholdIncrease = p2.thresholdIncrease, p1.thresholdDecrease = p2.thresholdDecrease, p1.Y1Increase = p2.Y1Increase, " & vbCrLf & _
                                             "p1.Y1Decrease = p2.Y1Decrease, p1.Y2Increase = p2.Y2Increase, p1.Y2Decrease = p2.Y2Decrease, p1.Y3Increase = p2.Y3Increase, p1.Y3Decrease = p2.Y3Decrease, " & vbCrLf & _
                                             "p1.Y4Increase = p2.Y4Increase, p1.Y4Decrease = p2.Y4Decrease, p1.phaseInYrsIncrease = p2.phaseInYrsIncrease, p1.phaseInYrsDecrease = p2.phaseInYrsDecrease " & vbCrLf & _
                                             "FROM liveLTTPhaseInValues p1 " & vbCrLf & _
                                             "JOIN liveLTTPhaseInValues p2 on p2.userID = p1.userID " & vbCrLf & _
                                             "WHERE p1.taxClassID = '" & childID & "' AND p2.taxClassID = '" & parentID & "' AND p1.userID = " & userID & vbCrLf
                    End While
                    dr.Close()
                    query.ExecuteNonQuery()
                End If
            End If

            con.Close()
            Session("updateComplete") = True
        Else
            Session("updateComplete") = False
        End If


    End Sub

    Public Sub validationSequence(ByVal sender As System.Object, ByVal e As System.EventArgs)
        validateGrid()
        calculateYrPercentage()
    End Sub
    Public Sub validateGrid()
        Try
            Dim i As Integer
            Session.Add("validLTTValues", False)

            'calls validation function for each textbox to confirm that values are numeric
            For i = 0 To rptPhaseIn.Items.Count - 1

                If Not validData(i, "txtThresholdIncr") Then
                    Session("validLTTValues") = False
                    Exit Sub
                ElseIf Not validData(i, "txtIncrYr1") Then
                    Session("validLTTValues") = False
                    Exit Sub
                ElseIf Not validData(i, "txtIncrYr2") Then
                    Session("validLTTValues") = False
                    Exit Sub
                ElseIf Not validData(i, "txtIncrYr3") Then
                    Session("validLTTValues") = False
                    Exit Sub
                ElseIf Not validData(i, "txtIncrYr4") Then
                    Session("validLTTValues") = False
                    Exit Sub
                ElseIf Not validData(i, "txtThresholdDecr") Then
                    Session("validLTTValues") = False
                    Exit Sub
                ElseIf Not validData(i, "txtDecrYr1") Then
                    Session("validLTTValues") = False
                    Exit Sub
                ElseIf Not validData(i, "txtDecrYr2") Then
                    Session("validLTTValues") = False
                    Exit Sub
                ElseIf Not validData(i, "txtDecrYr3") Then
                    Session("validLTTValues") = False
                    Exit Sub
                ElseIf Not validData(i, "txtDecrYr4") Then
                    Session("validLTTValues") = False
                    Exit Sub
                Else
                    Session("validLTTValues") = True
                End If
            Next
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Function validData(ByVal index As Integer, ByVal txtboxID As String) As Boolean
        Try
            If Not IsNumeric(Trim(CType(rptPhaseIn.Items(index).FindControl(txtboxID), TextBox).Text)) Then
                Master.errorMsg = common.GetErrorMessage("PATMAP72")
                SetTxtBoxFocus(index, txtboxID)
                validData = False
            Else
                If InStr(txtboxID, "Threshold") <> 0 Then
                    If Trim(CType(rptPhaseIn.Items(index).FindControl(txtboxID), TextBox).Text) < 0 Then
                        Master.errorMsg = common.GetErrorMessage("PATMAP150")
                        SetTxtBoxFocus(index, txtboxID)
                        validData = False
                    Else
                        validData = True
                    End If
                Else
                    validData = True
                End If
            End If
            Return validData
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Function

    Private Sub calculateYrPercentage()

        Dim i As Integer
        Dim incrYr1, incrYr2, incrYr3, incrYr4 As Double
        Dim decrYr1, decrYr2, decrYr3, decrYr4 As Double

        'validate input to ensure numeric values add up to 100%
        If Session("validLTTValues") Then
            For i = 0 To rptPhaseIn.Items.Count - 1
                incrYr1 = Double.Parse(CType(rptPhaseIn.Items(i).FindControl("txtIncrYr1"), TextBox).Text)
                incrYr2 = Double.Parse(CType(rptPhaseIn.Items(i).FindControl("txtIncrYr2"), TextBox).Text)
                incrYr3 = Double.Parse(CType(rptPhaseIn.Items(i).FindControl("txtIncrYr3"), TextBox).Text)
                incrYr4 = Double.Parse(CType(rptPhaseIn.Items(i).FindControl("txtIncrYr4"), TextBox).Text)

                decrYr1 = Double.Parse(CType(rptPhaseIn.Items(i).FindControl("txtDecrYr1"), TextBox).Text)
                decrYr2 = Double.Parse(CType(rptPhaseIn.Items(i).FindControl("txtDecrYr2"), TextBox).Text)
                decrYr3 = Double.Parse(CType(rptPhaseIn.Items(i).FindControl("txtDecrYr3"), TextBox).Text)
                decrYr4 = Double.Parse(CType(rptPhaseIn.Items(i).FindControl("txtDecrYr4"), TextBox).Text)

                If CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = 2 Then
                    incrYr2 = 100 - incrYr1
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr2"), TextBox).Text = incrYr2
                    If validateYrVal(i, "txtIncrYr1") And validateYrVal(i, "txtIncrYr2") Then

                        If validPercentTotal(i, "txtIncrYr1", "txtIncrYr2") Then
                            Session("validLTTValues") = True
                        Else
                            Session("validLTTValues") = False
                            Exit Sub
                        End If
                    Else
                        Session("validLTTValues") = False
                        Exit Sub
                    End If

                ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = 3 Then
                    incrYr3 = 100 - (incrYr1 + incrYr2)
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr3"), TextBox).Text = incrYr3

                    If validateYrVal(i, "txtIncrYr1") And validateYrVal(i, "txtIncrYr2") And validateYrVal(i, "txtIncrYr3") Then
                        'Validate new percentage totals
                        If validPercentTotal(i, "txtIncrYr1", "txtIncrYr2", "txtIncrYr3") Then
                            Session("validLTTValues") = True
                        Else
                            Session("validLTTValues") = False
                            Exit Sub
                        End If
                    Else
                        Session("validLTTValues") = False
                        Exit Sub
                    End If

                ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = 4 Then
                    incrYr4 = 100 - (incrYr1 + incrYr2 + incrYr3)
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr4"), TextBox).Text = incrYr4

                    If validateYrVal(i, "txtIncrYr1") And validateYrVal(i, "txtIncrYr2") And validateYrVal(i, "txtIncrYr3") And validateYrVal(i, "txtIncrYr4") Then
                        'Validate new percentage totals
                        If validPercentTotal(i, "txtIncrYr1", "txtIncrYr2", "txtIncrYr3", "txtIncrYr4") Then
                            Session("validLTTValues") = True
                        Else
                            Session("validLTTValues") = False
                            Exit Sub
                        End If
                    Else
                        Session("validLTTValues") = False
                        Exit Sub
                    End If
                End If

                'Decrease values
                If CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = 2 Then
                    decrYr2 = 100 - decrYr1
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr2"), TextBox).Text = decrYr2
                    If validateYrVal(i, "txtDecrYr1") And validateYrVal(i, "txtDecrYr2") Then
                        'Validate new percentage totals
                        If validPercentTotal(i, "txtDecrYr1", "txtDecrYr2") Then
                            Session("validLTTValues") = True
                        Else
                            Session("validLTTValues") = False
                            Exit Sub
                        End If
                    Else
                        Session("validLTTValues") = False
                        Exit Sub
                    End If

                ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = 3 Then

                    decrYr3 = 100 - (decrYr1 + decrYr2)
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr3"), TextBox).Text = decrYr3
                    If validateYrVal(i, "txtDecrYr1") And validateYrVal(i, "txtDecrYr2") And validateYrVal(i, "txtDecrYr3") Then

                        'Validate new percentage totals
                        If validPercentTotal(i, "txtDecrYr1", "txtDecrYr2", "txtDecrYr3") Then
                            Session("validLTTValues") = True
                        Else
                            Session("validLTTValues") = False
                            Exit Sub
                        End If
                    Else
                        Session("validLTTValues") = False
                        Exit Sub
                    End If

                ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = 4 Then
                    decrYr4 = 100 - (decrYr1 + decrYr2 + decrYr3)
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr4"), TextBox).Text = decrYr4

                    If validateYrVal(i, "txtDecrYr1") And validateYrVal(i, "txtDecrYr2") And validateYrVal(i, "txtDecrYr3") And validateYrVal(i, "txtDecrYr4") Then

                        'Validate new percentage totals
                        If validPercentTotal(i, "txtDecrYr1", "txtDecrYr2", "txtDecrYr3", "txtDecrYr4") Then
                            Session("validLTTValues") = True
                        Else
                            Session("validLTTValues") = False
                            Exit Sub
                        End If
                    Else
                        Session("validLTTValues") = False
                        Exit Sub
                    End If
                End If
            Next
        End If
    End Sub

    Private Function validPercentTotal(ByVal index As Integer, ByVal yr1txt As String, ByVal yr2txt As String, Optional ByVal yr3txt As String = "", Optional ByVal yr4txt As String = "") As Boolean
        Try
            Dim yr1, yr2 As Double
            Dim yr3 As Double = -1234567890.0123458 'initialize to 'dummy values'
            Dim yr4 As Double = -1234567890.0123458 'initialize to 'dummy values'

            yr1 = Double.Parse(CType(rptPhaseIn.Items(index).FindControl(yr1txt), TextBox).Text)
            yr2 = Double.Parse(CType(rptPhaseIn.Items(index).FindControl(yr2txt), TextBox).Text)

            If Not (yr3txt = "") Then
                yr3 = Double.Parse(CType(rptPhaseIn.Items(index).FindControl(yr3txt), TextBox).Text)
            End If

            If Not (yr4txt = "") Then
                yr4 = Double.Parse(CType(rptPhaseIn.Items(index).FindControl(yr4txt), TextBox).Text)
            End If


            If yr4 = -1234567890.0123458 Then
                If yr3 = -1234567890.0123458 Then
                    'only yr1 and yr2 have values
                    If Not validateYrVal(index, yr1txt) Or Not validateYrVal(index, yr2txt) Then
                        validPercentTotal = False

                    ElseIf (yr1 + yr2) <> 100 Then 'test for total equal to 100
                        Master.errorMsg = common.GetErrorMessage("PATMAP148")
                        SetTxtBoxFocus(index, yr1txt)
                        validPercentTotal = False

                    Else
                        validPercentTotal = True
                    End If
                Else
                    'yr1, yr2 & yr3 have values
                    If Not validateYrVal(index, yr1txt) Or Not validateYrVal(index, yr2txt) Or Not validateYrVal(index, yr3txt) Then
                        validPercentTotal = False

                    ElseIf (yr1 + yr2 + yr3) <> 100 Then 'test for total equal to 100
                        Master.errorMsg = common.GetErrorMessage("PATMAP148")
                        SetTxtBoxFocus(index, yr1txt)
                        validPercentTotal = False
                    Else
                        validPercentTotal = True
                    End If
                End If
            Else
                'all four years have values

                'test for zero values
                If Not validateYrVal(index, yr1txt) Or Not validateYrVal(index, yr2txt) Or _
                   Not validateYrVal(index, yr3txt) Or Not validateYrVal(index, yr4txt) Then
                    validPercentTotal = False

                ElseIf (yr1 + yr2 + yr3 + yr4) <> 100 Then 'test for total equal to 100
                    Master.errorMsg = common.GetErrorMessage("PATMAP148")
                    SetTxtBoxFocus(index, yr1txt)
                    validPercentTotal = False
                Else
                    validPercentTotal = True
                End If
            End If

            Return validPercentTotal
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Function

    Private Function validateYrVal(ByVal index As Integer, ByVal txtboxID As String) As Boolean
        Try

            If (Trim(CType(rptPhaseIn.Items(index).FindControl(txtboxID), TextBox).Text) <= 0) Or _
               (Trim(CType(rptPhaseIn.Items(index).FindControl(txtboxID), TextBox).Text) >= 100) Then
                Master.errorMsg = common.GetErrorMessage("PATMAP149")
                SetTxtBoxFocus(index, txtboxID)
                validateYrVal = False
            Else
                validateYrVal = True
            End If
            Return validateYrVal
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Function

    Public Sub updateYrAccess(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer
        Dim count As Integer

        'grab values from text boxes
        For i = 0 To rptPhaseIn.Items.Count - 1

            'Inrease Values
            If CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = 0 Then
                For count = 1 To 4
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr" & count), TextBox).Text = "0"
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr" & count), TextBox).Enabled = False
                Next

            ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = 2 Then
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr3"), TextBox).Text = "0"
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr4"), TextBox).Text = "0"
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr3"), TextBox).Enabled = False
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr4"), TextBox).Enabled = False
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr1"), TextBox).Enabled = True
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr2"), TextBox).Enabled = True
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr1"), TextBox).Text = "50"
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr2"), TextBox).Text = "50"

            ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = 3 Then
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr1"), TextBox).Enabled = True
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr2"), TextBox).Enabled = True
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr3"), TextBox).Enabled = True

                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr1"), TextBox).Text = "33"
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr2"), TextBox).Text = "33"
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr3"), TextBox).Text = "34"
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr4"), TextBox).Text = "0"
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr4"), TextBox).Enabled = False

            Else
                For count = 1 To 4
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr" & count), TextBox).Enabled = True
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr" & count), TextBox).Text = "25"
                Next
            End If

            'Decrease Values
            If CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = 0 Then
                For count = 1 To 4
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr" & count), TextBox).Text = "0"
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr" & count), TextBox).Enabled = False
                Next

            ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = 2 Then
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr3"), TextBox).Text = "0"
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr4"), TextBox).Text = "0"
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr3"), TextBox).Enabled = False
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr4"), TextBox).Enabled = False
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr1"), TextBox).Enabled = True
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr2"), TextBox).Enabled = True
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr1"), TextBox).Text = "50"
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr2"), TextBox).Text = "50"

            ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = 3 Then
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr1"), TextBox).Enabled = True
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr2"), TextBox).Enabled = True
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr3"), TextBox).Enabled = True
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr1"), TextBox).Text = "33"
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr2"), TextBox).Text = "33"
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr3"), TextBox).Text = "34"

                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr4"), TextBox).Text = "0"
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr4"), TextBox).Enabled = False

            Else
                For count = 1 To 4
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr" & count), TextBox).Enabled = True
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr" & count), TextBox).Text = "25"
                Next
            End If
        Next
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
        Try
            Dim i, count As Integer

            'reSet PhaseIn table
            For i = 0 To rptPhaseIn.Items.Count - 1

                CType(rptPhaseIn.Items(i).FindControl("txtThresholdIncr"), TextBox).Text = "0"
                CType(rptPhaseIn.Items(i).FindControl("txtThresholdDecr"), TextBox).Text = "0"

                'reset textboxes to 4 yrs and 25% for each class.
                For count = 1 To 4
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr" & count), TextBox).Enabled = True
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr" & count), TextBox).Enabled = True
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr" & count), TextBox).Text = "25"
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr" & count), TextBox).Text = "25"
                Next

                CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = "4"
                CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = "4"

            Next
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Protected Sub fillPhaseInTable()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim query As String
        Dim userID As Integer = Session("userID")
        Dim i, count As Integer

        If Session("showFullTaxClasses") Then
            query = "SELECT liveLTTPhaseInValues.taxClassID, taxClass, thresholdIncrease, thresholdDecrease, (Y1Increase * 100) AS Y1Increase, (Y1Decrease * 100) AS Y1Decrease, (Y2Increase * 100) AS Y2Increase, (Y2Decrease * 100) AS Y2Decrease, (Y3Increase * 100) AS Y3Increase, (Y3Decrease * 100) AS Y3Decrease, (Y4Increase * 100) AS Y4Increase, (Y4Decrease * 100) AS Y4Decrease, phaseInYrsIncrease, phaseInYrsDecrease FROM liveLTTPhaseInValues INNER JOIN LTTtaxClasses ON LTTtaxClasses.taxClassID = liveLTTPhaseInValues.taxClassID INNER JOIN liveLTTtaxClasses_" & userID & " ON liveLTTtaxClasses_" & userID & ".taxClassID = LTTtaxClasses.taxClassID WHERE LTTtaxClasses.active = 1 AND liveLTTtaxClasses_" & userID & ".show = 1 AND userID = '" & userID & "'"

        Else
            query = "SELECT DISTINCT LTTmainTaxClasses.taxClassID, LTTmainTaxClasses.taxClass, thresholdIncrease, thresholdDecrease, (Y1Increase * 100) AS Y1Increase, (Y1Decrease * 100) AS Y1Decrease, (Y2Increase * 100) AS Y2Increase, (Y2Decrease * 100) AS Y2Decrease, (Y3Increase * 100) AS Y3Increase, (Y3Decrease * 100) AS Y3Decrease, (Y4Increase * 100) AS Y4Increase, (Y4Decrease * 100) AS Y4Decrease, phaseInYrsIncrease, phaseInYrsDecrease FROM liveLTTPhaseInValues INNER JOIN LTTtaxClasses ON LTTtaxClasses.taxClassID = liveLTTPhaseInValues.taxClassID INNER JOIN LTTmainTaxClasses ON LTTmainTaxClasses.taxClassID = LTTtaxClasses.LTTmainTaxClassID INNER JOIN liveLTTtaxClasses_" & userID & " ON liveLTTtaxClasses_" & userID & ".taxClassID = LTTtaxClasses.taxClassID WHERE LTTtaxClasses.active = 1 AND liveLTTtaxClasses_" & userID & ".show = 1 AND userID = '" & userID & "'"
        End If

        'fill in the tax class table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        rptPhaseIn.DataSource = dt
        rptPhaseIn.DataBind()

        'set dropdownlist values
        For i = 0 To dt.Rows.Count - 1
            CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = dt.Rows(i).Item("phaseInYrsIncrease")
            CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = dt.Rows(i).Item("phaseInYrsDecrease")


            '*** Enable or disable textboxes based on dropdownlistselection ***

            'Increase Values
            If CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = 0 Then
                For count = 1 To 4
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr" & count), TextBox).Enabled = False
                Next

            ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = 2 Then
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr3"), TextBox).Enabled = False
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr4"), TextBox).Enabled = False

            ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsIncr"), DropDownList).SelectedValue = 3 Then
                CType(rptPhaseIn.Items(i).FindControl("txtIncrYr4"), TextBox).Enabled = False

            Else
                For count = 1 To 4
                    CType(rptPhaseIn.Items(i).FindControl("txtIncrYr" & count), TextBox).Enabled = True
                Next
            End If

            'Decrease Values
            If CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = 0 Then
                For count = 1 To 4
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr" & count), TextBox).Enabled = False
                Next

            ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = 2 Then
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr3"), TextBox).Enabled = False
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr4"), TextBox).Enabled = False

            ElseIf CType(rptPhaseIn.Items(i).FindControl("ddlYrsDecr"), DropDownList).SelectedValue = 3 Then
                CType(rptPhaseIn.Items(i).FindControl("txtDecrYr4"), TextBox).Enabled = False

            Else
                For count = 1 To 4
                    CType(rptPhaseIn.Items(i).FindControl("txtDecrYr" & count), TextBox).Enabled = True
                Next
            End If

        Next

    End Sub

    Protected Sub SetTxtBoxFocus(ByVal index As Integer, ByVal txtBoxID As String)
        'Sets the Focus to the indicated text box in the Repeater Control
        Dim sm As ScriptManager = ScriptManager.GetCurrent(Page)
        sm.SetFocus(CType(rptPhaseIn.Items(index).FindControl(txtBoxID), TextBox))
        CType(rptPhaseIn.Items(index).FindControl(txtBoxID), TextBox).Attributes.Add("onFocusIn", "select();")

    End Sub

    Private Sub fillPhaseInSummary()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        con.Open()

        Dim dr As SqlClient.SqlDataReader

        'Get Levy and UMR
        query.CommandText = "SELECT DISTINCT UMR, Levy FROM liveLLTSubjectModel_" & Session("userID").ToString
        dr = query.ExecuteReader()

        Dim revisedUMR, revisedLevy As Double

        If dr.HasRows Then
            dr.Read()
            revisedUMR = dr.GetValue(0) * 1000
            revisedLevy = dr.GetValue(1)
        End If
        dr.Close()

        'Get PhaseIn data
        Dim PhaseInY1, PhaseInY2, PhaseInY3, PhaseInY4 As Double
        query.CommandText = "SELECT SUM(Y1PhaseIn) AS Y1PhaseIn, SUM(Y2PhaseIn) AS Y2PhaseIn, SUM(Y3PhaseIn) AS Y3PhaseIn, SUM(Y4PhaseIn) AS Y4PhaseIn FROM liveLTTPhaseInSummary_" & Session("userID").ToString
        dr = query.ExecuteReader()

        If dr.HasRows Then
            dr.Read()
            PhaseInY1 = dr.GetValue(0)
            PhaseInY2 = dr.GetValue(1)
            PhaseInY3 = dr.GetValue(2)
            PhaseInY4 = dr.GetValue(3)
        End If
        dr.Close()

        'Fill PhaseIn Summary table 
        txtRevisedUMR.Text = FormatNumber(revisedUMR, 4)

        txtRevY1.Text = FormatNumber(revisedLevy, 0)
        txtRevY2.Text = FormatNumber(revisedLevy, 0)
        txtRevY3.Text = FormatNumber(revisedLevy, 0)
        txtRevY4.Text = FormatNumber(revisedLevy, 0)

        txtRevPhaseInY1.Text = FormatNumber(PhaseInY1, 0)
        txtRevPhaseInY2.Text = FormatNumber(PhaseInY2, 0)
        txtRevPhaseInY3.Text = FormatNumber(PhaseInY3, 0)
        txtRevPhaseInY4.Text = FormatNumber(PhaseInY4, 0)

        txtImpactY1.Text = FormatNumber(PhaseInY1 - revisedLevy, 0)
        txtImpactY2.Text = FormatNumber(PhaseInY2 - revisedLevy, 0)
        txtImpactY3.Text = FormatNumber(PhaseInY3 - revisedLevy, 0)
        txtImpactY4.Text = FormatNumber(PhaseInY4 - revisedLevy, 0)
    End Sub

    Protected Sub rptPhaseIn_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptPhaseIn.ItemCommand

    End Sub
End Class