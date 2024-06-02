Partial Public Class mintax
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

				'applies Subject Municipality and Subjust Year to labels
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
				fillMinTaxGrid()

				'Hides BaseYear (StatusQuo) column if user doesn't have access to Phase In screen
				If Session("phaseInBaseYearAccess") = False Then
					grdMinTax.Columns(1).Visible = False
					grdMinTax.Width = Unit.Percentage(70)
				End If

				common.populateSubjectModelFields(txtRevisedUniformMillRateValue, txtRevisedModelRevenueValue)

			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

    Protected Sub fillMinTaxGrid()
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
            query = "SELECT liveLTTValues.taxClassID, taxClass, baseMinTax, modelMinTax, modelMinTaxRN FROM liveLTTValues INNER JOIN LTTtaxClasses ON LTTtaxClasses.taxClassID = liveLTTValues.taxClassID INNER JOIN liveLTTtaxClasses_" & userID & " ON liveLTTtaxClasses_" & userID & ".taxClassID = LTTtaxClasses.taxClassID WHERE LTTtaxClasses.active = 1 AND liveLTTtaxClasses_" & userID & ".show = 1 AND userID = '" & userID & "'"

        Else
            query = "SELECT DISTINCT LTTmainTaxClasses.taxClassID, LTTmainTaxClasses.taxClass, baseMinTax, modelMinTax, modelMinTaxRN FROM liveLTTValues INNER JOIN LTTtaxClasses ON LTTtaxClasses.taxClassID = liveLTTValues.taxClassID INNER JOIN LTTmainTaxClasses ON LTTmainTaxClasses.taxClassID = LTTtaxClasses.LTTmainTaxClassID INNER JOIN liveLTTtaxClasses_" & userID & " ON liveLTTtaxClasses_" & userID & ".taxClassID = LTTtaxClasses.taxClassID WHERE LTTtaxClasses.active = 1 AND liveLTTtaxClasses_" & userID & ".show = 1 AND userID = '" & userID & "'"
        End If

        'fill in the tax class table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        grdMinTax.DataSource = dt
        grdMinTax.DataBind()

        'set radio button options
        For i = 0 To dt.Rows.Count - 1
            CType(grdMinTax.Rows(i).FindControl("rdlRevenueNeutral"), RadioButtonList).SelectedValue = dt.Rows(i).Item("modelMinTaxRN")
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

                'Redirect user to the next available LTT screen. Passes the current pages screenID and the current sessionID
                common.gotoNextPage(9, 109, Session("levelID"))
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
        Dim i As Integer
        Dim modelMinTax, taxClass As String
        Dim revNeutral As Boolean
        Dim selected As Integer
        Session.Add("updateComplete", False)

        'performs validation if not already completed
        If IsNothing(Session("validLTTValues")) Then
            validateGrid()
        End If

        'grab values from text boxes
        If Session("validLTTValues") Then

            For i = 0 To grdMinTax.Rows.Count - 1
                If (grdMinTax.Rows(i).RowType = DataControlRowType.DataRow) Then

                    modelMinTax = Double.Parse(Trim(CType(grdMinTax.Rows(i).FindControl("txtModelMinTax"), TextBox).Text)).ToString
                    revNeutral = CType(grdMinTax.Rows(i).FindControl("rdlRevenueNeutral"), RadioButtonList).SelectedValue
                    taxClass = grdMinTax.DataKeys(i).Values("taxClassID").ToString()

                    If revNeutral = True Then
                        selected = 1
                    Else
                        selected = 0
                    End If

                    If Session("showFullTaxClasses") Then
                        query.CommandText += "UPDATE liveLTTValues SET modelMinTax = " & modelMinTax & ", modelMinTaxRN = " & selected & " WHERE userID = '" & userID & "' and taxClassID = '" & taxClass & "'" & vbCrLf

                    Else
                        query.CommandText += "UPDATE liveLTTValues SET modelMinTax = " & modelMinTax & ", modelMinTaxRN = " & selected & vbCrLf & _
                                             "WHERE userID = '" & userID & "' AND taxClassID IN (select taxClassID from liveLTTtaxClasses_" & userID & _
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
                        query.CommandText += "UPDATE t1 SET t1.modelMinTax = t2.modelMinTax, t1.modelMinTaxRN = t2.modelMinTaxRN" & vbCrLf & _
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
            Dim modelMinTax As String
            Dim i As Integer
            Session.Add("validLTTValues", False)

            'grab values from text box
            For i = 0 To grdMinTax.Rows.Count - 1
                If (grdMinTax.Rows(i).RowType = DataControlRowType.DataRow) Then

                    modelMinTax = Trim(CType(grdMinTax.Rows(i).FindControl("txtModelMinTax"), TextBox).Text)

                    'validation
                    If Not IsNumeric(modelMinTax) Then
                        Master.errorMsg = common.GetErrorMessage("PATMAP72")
                        CType(grdMinTax.Rows(i).FindControl("txtModelMinTax"), TextBox).Focus()
                        CType(grdMinTax.Rows(i).FindControl("txtModelMinTax"), TextBox).Attributes.Add("onFocusIn", "select();")
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

    Protected Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReset.Click
        Try
            Dim i As Integer = 0

            'resets textbox and radio buttons
            For i = 0 To grdMinTax.Rows.Count - 1
                If (grdMinTax.Rows(i).RowType = DataControlRowType.DataRow) Then
                    CType(grdMinTax.Rows(i).Cells(2).FindControl("txtModelMinTax"), TextBox).Text = ""
                    CType(grdMinTax.Rows(i).Cells(3).FindControl("rdlRevenueNeutral"), RadioButtonList).SelectedIndex = 0
                End If
            Next

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub
End Class