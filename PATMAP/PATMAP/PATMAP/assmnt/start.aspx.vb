Imports System.IO

Partial Public Class start
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
			Master.errorMsg = ""

			If Not IsPostBack Then
				'Sets submenu to be displayed
				subMenu.setStartNode(menu.assmnt)

				Dim levelID As Integer = Session("levelID")
				Dim presentationUserQuery As String = ""

				'Provincial Analyst only has the permission to create
				'new scenario
				If ((levelID <> 1) And Not (levelID >= 46 And levelID <= 51)) Or levelID = 49 Then
					rdlScenarioType.Items(0).Selected = False
					pnlChoice.Visible = False
					pnlNew.Visible = False
					pnlSaved.Visible = True
					btnContinue.Visible = False

					If levelID = 3 Or levelID = 49 Then
						ddlAudience.SelectedValue = 1
						presentationUserQuery = " WHERE assessmentTaxModel.audiencePresentation = 1"
					Else
						ddlAudience.SelectedValue = 2
					End If

					ddlAudience.Enabled = False
				End If

				'setup database connection
				Dim con As New SqlClient.SqlConnection
				con.ConnectionString = PATMAP.Global_asax.connString
				Dim query As New SqlClient.SqlCommand
				query.Connection = con
				con.Open()

				'fill the users drop down
				Dim da As New SqlClient.SqlDataAdapter
				da.SelectCommand = New SqlClient.SqlCommand
				da.SelectCommand.Connection = con
				da.SelectCommand.CommandText = "SELECT 0 AS userID, '<Please Select>' AS [name] UNION SELECT userID, isNull(users.firstName,'Unknown') + ' ' + isNull(users.lastName,'User') AS [name] FROM users INNER JOIN assessmentTaxModel ON createdByUserID = userID INNER JOIN taxYearModelDescription TYMD1 ON assessmentTaxModel.BaseTaxYearModelID = TYMD1.taxYearModelID INNER JOIN taxYearModelDescription TYMD2 ON assessmentTaxModel.SubjectTaxYearModelID = TYMD2.taxYearModelID" & presentationUserQuery
				'da.SelectCommand.CommandText = "select 0 as userID, '<Please Select>' as name union select userID, isNull(users.firstName,'Unknown') + ' ' + isNull(users.lastName,'User') as [name] from users inner join assessmentTaxModel on createdByUserID = userID"
				Dim dt As New DataTable
				da.Fill(dt)
				ddlUser.DataSource = dt
				ddlUser.DataValueField = "userID"
				ddlUser.DataTextField = "name"
				ddlUser.DataBind()

				'fill the base year drop down
				da = New SqlClient.SqlDataAdapter
				da.SelectCommand = New SqlClient.SqlCommand
				da.SelectCommand.Connection = con
				'Donna - Added PEMRID > 0.
				da.SelectCommand.CommandText = "select 0 as taxYearModelID,'<Please Select>' as taxYearModelName union select taxYearModelID, taxYearModelName from taxYearModelDescription where taxYearStatusID = 1 and assessmentID > 0 and millRateSurveyID > 0 and POVID > 0 and potashID > 0 and PEMRID > 0"
				dt = New DataTable
				da.Fill(dt)
				ddlBaseModel.DataSource = dt
				ddlBaseModel.DataValueField = "taxYearModelID"
				ddlBaseModel.DataTextField = "taxYearModelName"
				ddlBaseModel.DataBind()

				'fill the other base year drop down
				ddlSavedBase.DataSource = dt
				ddlSavedBase.DataValueField = "taxYearModelID"
				ddlSavedBase.DataTextField = "taxYearModelName"
				ddlSavedBase.DataBind()

				'fill the subject year drop down
				da = New SqlClient.SqlDataAdapter
				da.SelectCommand = New SqlClient.SqlCommand
				da.SelectCommand.Connection = con
				da.SelectCommand.CommandText = "select 0 as taxYearModelID,'<Please Select>' as taxYearModelName union select taxYearModelID, taxYearModelName from taxYearModelDescription where taxYearStatusID = 1 and assessmentID > 0 and potashID > 0"
				dt = New DataTable
				da.Fill(dt)
				ddlSubjectModel.DataSource = dt
				ddlSubjectModel.DataValueField = "taxYearModelID"
				ddlSubjectModel.DataTextField = "taxYearModelName"
				ddlSubjectModel.DataBind()

				'fill the other subject year drop down
				ddlSavedSubject.DataSource = dt
				ddlSavedSubject.DataValueField = "taxYearModelID"
				ddlSavedSubject.DataTextField = "taxYearModelName"
				ddlSavedSubject.DataBind()

				con.Close()
			End If
			txtDateTo.Text = Request(txtDateTo.UniqueID)
			txtDateFrom.Text = Request(txtDateFrom.UniqueID)

		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
    End Sub

    Private Sub btnContinue_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnContinue.Click
        Try

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            query.CommandText = "exec dropUserModelTables " & Session("userID").ToString
            query.ExecuteNonQuery()

            con.Close()


            If rdlScenarioType.Items(0).Selected = True Then
                If ddlBaseModel.SelectedValue = 0 Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP50")
                    Exit Sub
                End If
                If ddlSubjectModel.SelectedValue = 0 Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP51")
                    Exit Sub
                End If

                CreateAssessmentTaxModel()

                Session.Add("assessmentTaxModelID", 0)
            Else
                If String.IsNullOrEmpty(grdStart.SelectedValue) Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP52")
                    Exit Sub
                End If

                Session.Add("assessmentTaxModelID", grdStart.SelectedValue)
            End If

            Session.Remove("assessmentTaxModel")

            'initSessionVariables according to map	19-sep-2013
            common.ASSMNT_InitRequiredSessionVars()
            common.LoadLiveTaxYearModel(ddlSubjectModel.SelectedValue, ddlBaseModel.SelectedValue)

            ''added to display default tax status and default tax shifts arv-12-jul-2013
            ''Set Tax Classes Filters for map
            'common.SetTaxClassFilters()
            ''TaxStatus
            'Dim TmpSelTaxStatus As New List(Of String)
            'TmpSelTaxStatus.Add("Taxable")
            'Session("TaxStatus") = 1
            'Session("MapTaxStatusFilters") = TmpSelTaxStatus
            ''TaxShift
            'Dim TmpSelTaxShift As New List(Of String)
            'TmpSelTaxShift.Add("Municipal Tax")
            'Session("TaxShift") = 1
            'Session("MapTaxShiftFilters") = TmpSelTaxShift
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Dim levelID As Integer = Session("levelID")

        'Provincial Analyst, Sys Admininstrator, Presentation users 
        'only has the permission to change scenario parameters
        'If (levelID < 4) Or (levelID >= 46 And levelID <= 51) Then
        '    Response.Redirect("kog.aspx")
        'Else
        '    Response.Redirect("tables.aspx")
        'End If

        common.gotoNextPage(3, 5, levelID)

    End Sub

    ''test
    'Private Sub btnContinue_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnContinue.Click
    '    Try
    '        Session.Add("assessmentTaxModelID", grdStart.SelectedValue)
    '        HttpContext.Current.Session.Add("calculated", "true")
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    '    Dim levelID As Integer = Session("levelID")
    '    common.gotoNextPage(3, 5, levelID)

    'End Sub

    Protected Sub rdlScenarioType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdlScenarioType.SelectedIndexChanged
        Try
            If rdlScenarioType.Items(0).Selected = True Then
                pnlNew.Visible = True
                pnlSaved.Visible = False
                btnContinue.Visible = True
            Else
                pnlNew.Visible = False
                pnlSaved.Visible = True

                If grdStart.Rows.Count = 0 Then
                    btnContinue.Visible = False
                End If

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            FillAssessmentGrid()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub FillAssessmentGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection

        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim query As String

        query = "select assessmentTaxModel.assessmentTaxModelID, isNull(users.firstName,'Uknown') + ' ' + isNull(users.lastName,'User') as [name],assessmentTaxModelName, TYMD1.taxYearModelName as BaseTaxYearModelName, TYMD2.taxYearModelName as SubjectTaxYearModelName, assessmentTaxModel.[description], assessmentTaxModel.dateCreated from assessmentTaxModel left join users on assessmentTaxModel.createdByUserID = users.userID inner join taxYearModelDescription TYMD1 on assessmentTaxModel.BaseTaxYearModelID = TYMD1.taxYearModelID inner join taxYearModelDescription TYMD2 on assessmentTaxModel.SubjectTaxYearModelID = TYMD2.taxYearModelID where 1=1 "

        If ddlUser.SelectedValue > 0 Then
            query += " and assessmentTaxModel.createdByUserID = " & ddlUser.SelectedValue
        End If

        If Trim(txtDateFrom.Text) <> "" Then
            query += " and datediff(d, assessmentTaxModel.dateCreated,'" & txtDateFrom.Text & "') <= 0 "
        End If

        If Trim(txtDateTo.Text) <> "" Then
            query += " and datediff(d, assessmentTaxModel.dateCreated,'" & txtDateTo.Text & "') >= 0 "
        End If

        If Trim(txtScenarioName.Text) <> "" Then
            query += " and charindex('" & Trim(txtScenarioName.Text) & "',assessmentTaxModelName) > 0"
        End If

        If ddlSavedBase.SelectedValue > 0 Then
            query += " and assessmentTaxModel.BaseTaxYearModelID = " & ddlSavedBase.SelectedValue
        End If

        If ddlSavedSubject.SelectedValue > 0 Then
            query += " and assessmentTaxModel.SubjectTaxYearModelID = " & ddlSavedSubject.SelectedValue
        End If

        If ddlAudience.SelectedValue = 1 Then
            query += " and assessmentTaxModel.audiencePresentation = 1"
        End If

        If ddlAudience.SelectedValue = 2 Then
            query += " and assessmentTaxModel.audienceExternalUser = 1"
        End If

        'fill in the scenario table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        con.Close()

        grdStart.DataSource = dt
        grdStart.DataBind()

        Session("assessmentTaxModel") = dt

        txtTotal.Text = dt.Rows.Count

        If grdStart.Rows.Count > 0 Then
            btnContinue.Visible = True
        Else
            btnContinue.Visible = False
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If
    End Sub

    Private Sub CreateAssessmentTaxModel()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        con.Open()

        Dim userID As Integer = Session("userID")

        'create an entry in the liveAssessmentTaxModel table
        query.CommandText = "delete from liveAssessmentTaxModel where userID = " & userID.ToString & vbCrLf
        query.ExecuteNonQuery()

        'query.CommandText += "insert into liveAssessmentTaxModel select " & userID.ToString & " as userID, 0, '', '', " & ddlBaseModel.SelectedValue & " as baseTaxYearModelID, " & ddlSubjectModel.SelectedValue & " as subjectTaxYearModelID,'',0,0,0,0.0,0,(select dataStale from taxYearModelDescription where taxYearModelID = " & ddlSubjectModel.SelectedValue & "),''"
        'Donna - Added 0 for PEMRDescriptionID, 1 for enterPEMR and NULL for PEMRByTotalLevy.
        query.CommandText += "insert into liveAssessmentTaxModel select " & userID.ToString & " as userID, 0, '', '', " & ddlBaseModel.SelectedValue & " as baseTaxYearModelID, " & ddlSubjectModel.SelectedValue & " as subjectTaxYearModelID,'',0,0,0,0.0,0,1,'',assessmentID,millRateSurveyID,uniformMunicipalMillRateID,uniformPotashMillRateID,uniformK12MillRateID,uniformSchoolMillRateID,0,0,0,0,PotashID,0,1, NULL from taxYearModelDescription where taxYearModelID = " & ddlSubjectModel.SelectedValue
        query.ExecuteNonQuery()

        Dim trans As SqlClient.SqlTransaction
        trans = con.BeginTransaction()
        query.Transaction = trans
        Try
            query.ExecuteNonQuery()
            trans.Commit()
        Catch
            trans.Rollback()
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
            con.Close()
            Exit Sub
        End Try

        'clean up
        con.Close()
    End Sub

    Protected Sub gridRowChecked(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim counter As Integer = 0
            Dim rb As System.Web.UI.WebControls.RadioButton
            While counter < grdStart.Rows.Count()
                rb = grdStart.Rows(counter).Cells(0).FindControl("selectScenario")
                If rb.Equals(sender) Then
                    grdStart.SelectedIndex = counter
                Else
                    rb.Checked = False
                End If
                counter += 1
            End While
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdStart_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdStart.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdStart.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Session("assessmentTaxModel")) Then
                dt = CType(Session("assessmentTaxModel"), DataTable)
                grdStart.DataSource = dt
                grdStart.DataBind()
            Else
                FillAssessmentGrid()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdStart_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdStart.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in session
            If IsNothing(Session("assessmentTaxModel")) Then
                FillAssessmentGrid()
            End If

            dt = CType(Session("assessmentTaxModel"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdStart.DataSource = dt
            grdStart.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
        Try
            Dim levelID As Integer = Session("levelID")

            txtScenarioName.Text = ""
            txtDateTo.Text = ""
            txtDateFrom.Text = ""
            ddlUser.SelectedItem.Selected = False
            ddlUser.Items(0).Selected = True
            ddlSavedBase.SelectedItem.Selected = False
            ddlSavedBase.Items(0).Selected = True
            ddlSavedSubject.SelectedItem.Selected = False
            ddlSavedSubject.Items(0).Selected = True

            If (levelID = 1) Or (levelID >= 46 And levelID <= 51) Then
                ddlAudience.SelectedItem.Selected = False
                ddlAudience.Items(0).Selected = True
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
    
End Class