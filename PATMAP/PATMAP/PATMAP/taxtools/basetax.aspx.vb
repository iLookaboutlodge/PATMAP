Partial Public Class basetax
    Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'clears out the error message
			Master.errorMsg = ""

			'added for calculating LTT tables when Map is first time loaded	17-sep-2013
			System.Web.HttpContext.Current.Session.Remove("LTTcalculated")

			If Not IsPostBack Then
				'sets submenu to be displayed
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
				fillBaseTaxGrid()

				'Hides BaseYear (StatusQuo) column if user doesn't have access to PhaseIn screen
				If Session("phaseInBaseYearAccess") = False Then
					grdBaseTax.Columns(1).Visible = False
					grdBaseTax.Width = Unit.Percentage(70)
				End If

				common.populateSubjectModelFields(txtRevisedUniformMillRateValue, txtRevisedModelRevenueValue)

			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

    Protected Sub fillBaseTaxGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim query As String
        Dim userID As Integer = Session("userID")
        Dim i As Integer

        If Session("showFullTaxClasses") Then
            query = "SELECT liveLTTValues.taxClassID, taxClass, baseBaseTax, modelBaseTax, modelBaseTaxRN FROM liveLTTValues INNER JOIN LTTtaxClasses ON LTTtaxClasses.taxClassID = liveLTTValues.taxClassID INNER JOIN liveLTTtaxClasses_" & userID & " ON liveLTTtaxClasses_" & userID & ".taxClassID = LTTtaxClasses.taxClassID WHERE LTTtaxClasses.active = 1 AND liveLTTtaxClasses_" & userID & ".show = 1 AND userID = '" & userID & "'"

        Else
            query = "SELECT DISTINCT LTTmainTaxClasses.taxClassID, LTTmainTaxClasses.taxClass, baseBaseTax, modelBaseTax, modelBaseTaxRN FROM liveLTTValues INNER JOIN LTTtaxClasses ON LTTtaxClasses.taxClassID = liveLTTValues.taxClassID INNER JOIN LTTmainTaxClasses ON LTTmainTaxClasses.taxClassID = LTTtaxClasses.LTTmainTaxClassID INNER JOIN liveLTTtaxClasses_" & userID & " ON liveLTTtaxClasses_" & userID & ".taxClassID = LTTtaxClasses.taxClassID WHERE LTTtaxClasses.active = 1 AND liveLTTtaxClasses_" & userID & ".show = 1 AND userID = '" & userID & "'"
        End If

        'fill in the tax class table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        grdBaseTax.DataSource = dt
        grdBaseTax.DataBind()

        'set radio button options
        For i = 0 To dt.Rows.Count - 1
            CType(grdBaseTax.Rows(i).Cells(3).FindControl("rdlRevenueNeutral"), RadioButtonList).SelectedValue = dt.Rows(i).Item("modelBaseTaxRN")
        Next

        'clean up
        con.Close()


    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Try

            validateGrid()
            updateLiveLTTValues()

            common.calculateSubjectModelTax(Double.Parse(Session("revSubjUMR").ToString))

            If Session("updateComplete") Then
                Session.Remove("validLTTValues")
                Session.Remove("updateComplete")
                Response.Redirect("mintax.aspx")
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReset.Click
        Try
            Dim i As Integer = 0

            'resets textbox and radio buttons
            For i = 0 To grdBaseTax.Rows.Count - 1
                If (grdBaseTax.Rows(i).RowType = DataControlRowType.DataRow) Then
                    CType(grdBaseTax.Rows(i).FindControl("txtModelBaseTax"), TextBox).Text = "0"
                    CType(grdBaseTax.Rows(i).FindControl("rdlRevenueNeutral"), RadioButtonList).SelectedIndex = 0
                End If
            Next

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
        Dim baseTax, taxClass As String
        Dim revNeutral As Boolean
        Dim selected As Integer
        Session.Add("updateComplete", False)

        'performs validation if not already completed
        If IsNothing(Session("validLTTValues")) Then
            validateGrid()
        End If

        'grab values from text boxes
        If Session("validLTTValues") Then

            For i = 0 To grdBaseTax.Rows.Count - 1
                If (grdBaseTax.Rows(i).RowType = DataControlRowType.DataRow) Then

                    baseTax = Double.Parse(Trim(CType(grdBaseTax.Rows(i).FindControl("txtModelBaseTax"), TextBox).Text)).ToString
                    revNeutral = CType(grdBaseTax.Rows(i).FindControl("rdlRevenueNeutral"), RadioButtonList).SelectedValue
                    taxClass = grdBaseTax.DataKeys(i).Values("taxClassID").ToString()

                    If revNeutral = True Then
                        selected = 1
                    Else
                        selected = 0
                    End If

                    If Session("showFullTaxClasses") Then
                        query.CommandText += "UPDATE liveLTTValues SET modelBaseTax = " & baseTax & ", modelBaseTaxRN = " & selected & " WHERE userID = '" & userID & "' and taxClassID = '" & taxClass & "'" & vbCrLf

                    Else
                        query.CommandText += "UPDATE liveLTTValues SET modelBaseTax = " & baseTax & ", modelBaseTaxRN = " & selected & vbCrLf & _
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
                        query.CommandText += "UPDATE t1 SET t1.modelBaseTax = t2.modelBaseTax, t1.modelBaseTaxRN = t2.modelBaseTaxRN" & vbCrLf & _
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
            Dim modelBaseTax As String
            Dim i As Integer
            Session.Add("validLTTValues", False)

            'grab values from text boxes
            For i = 0 To grdBaseTax.Rows.Count - 1
                If (grdBaseTax.Rows(i).RowType = DataControlRowType.DataRow) Then

                    modelBaseTax = Trim(CType(grdBaseTax.Rows(i).FindControl("txtModelBaseTax"), TextBox).Text)

                    'validation
                    If Not IsNumeric(modelBaseTax) Then
                        Master.errorMsg = common.GetErrorMessage("PATMAP72")
                        CType(grdBaseTax.Rows(i).FindControl("txtModelBaseTax"), TextBox).Focus()
                        CType(grdBaseTax.Rows(i).FindControl("txtModelBaseTax"), TextBox).Attributes.Add("onFocusIn", "select();")
                        Session("validLTTValues") = False
                        Exit Sub
                    Else
                        Session("validLTTValues") = True
                    End If

                End If
            Next
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub
End Class