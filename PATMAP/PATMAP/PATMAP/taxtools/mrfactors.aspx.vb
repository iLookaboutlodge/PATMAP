Partial Public Class mrfactors
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

				'applies Subject Municipality and Subject Year to labels
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


				'load gridview
				fillMRFactorsGrid()

				'Hides BaseYear (StatusQuo) column if user doesn't have access to PhaseIn screen
				If Session("phaseInBaseYearAccess") = False Then
					grdMRFactor.Columns(1).Visible = False
					grdMRFactor.Width = Unit.Percentage(60)
				End If

				common.populateSubjectModelFields(txtRevisedUniformMillRateValue, txtRevisedModelRevenueValue)

			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub


    Protected Sub fillMRFactorsGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim query As String
        Dim userID As Integer = Session("userID")

        If Session("showFullTaxClasses") Then
            query = "SELECT liveLTTValues.taxClassID, taxClass, baseMRF, modelMRF, modelMRFRN FROM liveLTTValues INNER JOIN LTTtaxClasses ON LTTtaxClasses.taxClassID = liveLTTValues.taxClassID INNER JOIN liveLTTtaxClasses_" & userID & " ON liveLTTtaxClasses_" & userID & ".taxClassID = LTTtaxClasses.taxClassID WHERE LTTtaxClasses.active = 1 AND liveLTTtaxClasses_" & userID & ".show = 1 AND userID = '" & userID & "'"

        Else
            query = "SELECT DISTINCT LTTmainTaxClasses.taxClassID, LTTmainTaxClasses.taxClass, baseMRF, modelMRF, modelMRFRN FROM liveLTTValues INNER JOIN LTTtaxClasses ON LTTtaxClasses.taxClassID = liveLTTValues.taxClassID INNER JOIN LTTmainTaxClasses ON LTTmainTaxClasses.taxClassID = LTTtaxClasses.LTTmainTaxClassID INNER JOIN liveLTTtaxClasses_" & userID & " ON liveLTTtaxClasses_" & userID & ".taxClassID = LTTtaxClasses.taxClassID WHERE LTTtaxClasses.active = 1 AND liveLTTtaxClasses_" & userID & ".show = 1 AND userID = '" & userID & "'"
        End If

        'fill in the tax class table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        grdMRFactor.DataSource = dt
        grdMRFactor.DataBind()

        'set radio button
        If dt.Rows(0).Item("modelMRFRN") = True Then
            rdoRevNeutralYes.Checked = True
            rdoRevNeutralNo.Checked = False
        Else
            rdoRevNeutralYes.Checked = False
            rdoRevNeutralNo.Checked = True
        End If

        'clean up
        con.Close()

    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Try

            validateGrid()
            updateLiveLTTValues()

            common.calculateSubjectModelTax(Double.Parse(Session("revSubjUMR").ToString))

            If Session("updateComplete") Then
                Session.Remove("validLTTValues")
                Session.Remove("updateComplete")


                'Redirect user to the next available LTT screen. Passes the current pages screenID and the current  user levelID
                common.gotoNextPage(9, 110, Session("levelID"))

                'If Session("phaseInBaseYearAccess") Then
                '    Response.Redirect("phasein.aspx")
                'Else
                '    Response.Redirect("tables.aspx")
                'End If
            End If

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
        Dim i, selected As Integer
        Dim modelMRF, taxClass As String

        Session.Add("updateComplete", False)

        'performs validation if not already completed
        If IsNothing(Session("validLTTValues")) Then
            validateGrid()
        End If

        'grab values from text boxes
        If Session("validLTTValues") Then

            'set radio button values
            If rdoRevNeutralYes.Checked = True Then
                selected = 1
            ElseIf rdoRevNeutralNo.Checked = True Then
                selected = 0
            End If

            'set grid values and update table
            For i = 0 To grdMRFactor.Rows.Count - 1
                If (grdMRFactor.Rows(i).RowType = DataControlRowType.DataRow) Then

                    modelMRF = Double.Parse(Trim(CType(grdMRFactor.Rows(i).FindControl("txtModelMRF"), TextBox).Text)).ToString
                    taxClass = grdMRFactor.DataKeys(i).Values("taxClassID").ToString()

                    If Session("showFullTaxClasses") Then
                        query.CommandText += "UPDATE liveLTTValues SET modelMRF = " & modelMRF & ", modelMRFRN = " & selected & _
                                " WHERE userID = '" & userID & "' and taxClassID = '" & taxClass & "'" & vbCrLf

                    Else
                        query.CommandText += "UPDATE liveLTTValues SET modelMRF = " & modelMRF & ", modelMRFRN = " & selected & vbCrLf & _
                                             "WHERE userID = '" & userID & "' and taxClassID IN (select taxClassID from liveLTTtaxClasses_" & userID & _
                                             " where LTTmainTaxClassID = '" & taxClass & "')" & vbCrLf & _
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
                        query.CommandText += "UPDATE t1 SET t1.modelMRF = t2.modelMRF, t1.modelMRFRN = t2.modelMRFRN" & vbCrLf & _
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

    'Public Sub valueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        validateGrid()
    '    Catch
    '        'retrieves error message
    '        Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
    '    End Try
    'End Sub

    Protected Sub validateGrid()
        Try
            Dim modelMRF As String
            Dim i As Integer
            Dim isValid_ModelMRF As Boolean = True
            Dim min_ModelMRF As Double? = Nothing
            Session.Add("validLTTValues", False)

            'grab values from text box
            For i = 0 To grdMRFactor.Rows.Count - 1
                If (grdMRFactor.Rows(i).RowType = DataControlRowType.DataRow) Then

                    modelMRF = Trim(CType(grdMRFactor.Rows(i).FindControl("txtModelMRF"), TextBox).Text)

                    'validation
                    If Not IsNumeric(modelMRF) Then
                        Master.errorMsg = common.GetErrorMessage("PATMAP72")
                        CType(grdMRFactor.Rows(i).FindControl("txtModelMRF"), TextBox).Focus()
                        CType(grdMRFactor.Rows(i).FindControl("txtModelMRF"), TextBox).Attributes.Add("onFocusIn", "select();")
                        Session("validLTTValues") = False
                        isValid_ModelMRF = False
                        Exit Sub
                    Else
                        'If modelMRF < 0.01 Then
                        'If modelMRF < 0.01 Or modelMRF > 9 Then
                        If modelMRF < 0.01 Then
                            Master.errorMsg = common.GetErrorMessage("PATMAP147")
                            CType(grdMRFactor.Rows(i).FindControl("txtModelMRF"), TextBox).Focus()
                            CType(grdMRFactor.Rows(i).FindControl("txtModelMRF"), TextBox).Attributes.Add("onFocusIn", "select();")
                            Session("validLTTValues") = False
                            isValid_ModelMRF = False
                            Exit Sub
                        Else
                            If min_ModelMRF.HasValue AndAlso min_ModelMRF.Value < modelMRF Then
                            Else
                                min_ModelMRF = modelMRF
                            End If
                        End If
                        Session("validLTTValues") = True
                    End If

                End If
            Next

            If isValid_ModelMRF AndAlso min_ModelMRF.HasValue Then
                Dim max_Allowed_ModelMRF As Double = min_ModelMRF.Value * 9

                For i = 0 To grdMRFactor.Rows.Count - 1
                    If (grdMRFactor.Rows(i).RowType = DataControlRowType.DataRow) Then
                        modelMRF = Trim(CType(grdMRFactor.Rows(i).FindControl("txtModelMRF"), TextBox).Text)
                        Dim dbl_ModelMRF As Double = Double.Parse(modelMRF)

                        If dbl_ModelMRF > max_Allowed_ModelMRF Then
                            Master.errorMsg = common.GetErrorMessage("PATMAP147")
                            CType(grdMRFactor.Rows(i).FindControl("txtModelMRF"), TextBox).Focus()
                            CType(grdMRFactor.Rows(i).FindControl("txtModelMRF"), TextBox).Attributes.Add("onFocusIn", "select();")
                            Session("validLTTValues") = False
                            isValid_ModelMRF = False
                            Exit Sub
                        End If
                    End If
                Next
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
        Try
            Dim i As Integer = 0

            'resets textbox and radio buttons
            For i = 0 To grdMRFactor.Rows.Count - 1
                If (grdMRFactor.Rows(i).RowType = DataControlRowType.DataRow) Then
                    CType(grdMRFactor.Rows(i).Cells(2).FindControl("txtModelMRF"), TextBox).Text = FormatNumber(1, 4)
                End If
            Next
            rdoRevNeutralYes.Checked = True
            rdoRevNeutralNo.Checked = False

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class