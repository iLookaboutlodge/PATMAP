Imports System.IO

Partial Public Class viewscenario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.taxYearModel)

                'sets the page size - number of items to be displayed per page
                grdStart.PageSize = PATMAP.Global_asax.pageSize

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                con.Open()

                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand()
                Dim dt As New DataTable
                Dim constr As String

                da.SelectCommand.Connection = con

                'populate the user drop down list                
                Bind_UserDLL()

                'populate the base tax year list   
                dt.Clear()
                constr = "select 0 as taxYearModelID,'<Please Select>' as taxYearModelName union select taxYearModelID, taxYearModelName from taxYearModelDescription where taxYearStatusID = 1 and assessmentID > 0 and millRateSurveyID > 0 and POVID > 0 and taxCreditID > 0 and K12ID > 0 and potashID > 0"
                da.SelectCommand.CommandText = constr
                da.Fill(dt)
                ddlSavedBase.DataSource = dt
                ddlSavedBase.DataValueField = "taxYearModelID"
                ddlSavedBase.DataTextField = "taxYearModelName"
                ddlSavedBase.DataBind()

                'populate the subject tax year list 
                dt.Clear()
                constr = "select 0 as taxYearModelID,'<Please Select>' as taxYearModelName union select taxYearModelID, taxYearModelName from taxYearModelDescription where taxYearStatusID = 1 and assessmentID > 0 and potashID > 0"
                da.SelectCommand.CommandText = constr
                da.Fill(dt)
                ddlSavedSubject.DataSource = dt
                ddlSavedSubject.DataValueField = "taxYearModelID"
                ddlSavedSubject.DataTextField = "taxYearModelName"
                ddlSavedSubject.DataBind()

                'clean up
                con.Close()

            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try


        txtDateFrom.Text = Request(txtDateFrom.UniqueID)
        txtDateTo.Text = Request(txtDateTo.UniqueID)

    End Sub

    Private Sub Bind_UserDLL()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand()
        Dim dt As New DataTable
        Dim constr As String

        da.SelectCommand.Connection = con

        'populate the user drop down list    
        'select only users who have  created any scenarios
        constr = "SELECT 0 AS userID, '<Please Select>' AS [userName] UNION SELECT userID, isNull(users.firstName,'Unknown') + ' ' + isNull(users.lastName,'User') AS [userName] FROM users INNER JOIN assessmentTaxModel ON createdByUserID = userID INNER JOIN taxYearModelDescription TYMD1 ON assessmentTaxModel.BaseTaxYearModelID = TYMD1.taxYearModelID INNER JOIN taxYearModelDescription TYMD2 ON assessmentTaxModel.SubjectTaxYearModelID = TYMD2.taxYearModelID"
        'constr = "select 0 as userID, '<Please Select>' as userName union select userID, isNull(users.firstName,'Unknown') + ' ' + isNull(users.lastName,'User') as [userName] from users right join assessmentTaxModel on createdByUserID = userID"
        da.SelectCommand.CommandText = constr
        da.Fill(dt)
        ddlUser.DataSource = dt
        ddlUser.DataValueField = "userID"
        ddlUser.DataTextField = "userName"
        ddlUser.DataBind()

        con.Close()
    End Sub



    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            'fill grid with search results
            performScenarioSearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub performScenarioSearch()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'setup the query to get entity details
        Dim da As New SqlClient.SqlDataAdapter
        Dim conStr As String

        'base query
        conStr = "select a.assessmentTaxModelID, isNull(u.firstName,'Uknown') + ' ' + isNull(u.lastName,'User') as userName, a.assessmentTaxModelName, t1.taxYearModelName as baseTaxYear, t2.taxYearModelName as subjectTaxYear, a.description, a.dateCreated  "
        conStr += "from assessmentTaxModel a "
        conStr += "left join users u on a.createdByUserID = u.userID "
        conStr += "join taxYearModelDescription t1 on a.BaseTaxYearModelID = t1.taxYearModelID "
        conStr += "join taxYearModelDescription t2 on a.SubjectTaxYearModelID = t2.taxYearModelID where 1=1"

        Dim userID As Integer
        userID = CType(ddlUser.SelectedValue, Integer)
        If userID <> 0 Then
            conStr += " AND a.createdByUserID=" & userID & " AND u.userID=" & userID
        End If

        Dim baseYearID As Integer
        baseYearID = CType(ddlSavedBase.SelectedValue, Integer)
        If baseYearID <> 0 Then
            conStr += " AND a.BaseTaxYearModelID=" & baseYearID & " AND t1.taxYearModelID=" & baseYearID
        End If

        Dim subjectYearID As Integer
        subjectYearID = CType(ddlSavedSubject.SelectedValue, Integer)
        If subjectYearID <> 0 Then
            conStr += " AND a.SubjectTaxYearModelID=" & subjectYearID & " AND t2.taxYearModelID=" & subjectYearID
        End If

        Dim audienceID As Integer
        audienceID = CType(ddlAudience.SelectedValue, Integer)
        If audienceID <> 0 Then
            If audienceID = 1 Then
                conStr += " AND a.audiencePresentation=1"
            ElseIf audienceID = 2 Then
                conStr += " AND a.audienceExternalUser=1"
            End If
        End If

        Dim fromDate As String
        fromDate = Trim(txtDateFrom.Text)
        Dim toDate As String
        toDate = Trim(txtDateTo.Text)
        If fromDate <> "" Then
            conStr += " AND a.dateCreated >='" & fromDate & "'"
        End If

        If toDate <> "" Then
            conStr += " AND a.dateCreated < DATEADD(day,1,'" & toDate & "')"
        End If

        If Trim(txtScenarioName.Text) <> "" Then
            conStr += " and a.assessmentTaxModelName like '%" & Trim(txtScenarioName.Text) & "%'"
        End If


        da.SelectCommand = New SqlClient.SqlCommand(conStr, con)

        'get the data for the entity grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdStart.DataSource = dt
        grdStart.DataBind()

        If IsNothing(Cache("scenario")) Then
            Cache.Add("scenario", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("scenario") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If

    End Sub

    Private Sub grdStart_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdStart.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdStart.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("scenario")) Then
                dt = CType(Cache("scenario"), DataTable)
                grdStart.DataSource = dt
                grdStart.DataBind()
            Else
                performScenarioSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub


    Private Sub grdStart_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdStart.RowCommand
        Try
            If e.CommandName = "deleteScenario" Then

                'find the row index which to be deleted
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim assessmentTaxModelID As String = grdStart.DataKeys(index).Values("assessmentTaxModelID")


                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                con.Open()
                Dim query As New SqlClient.SqlCommand
                query.Connection = con

                'delete function
                query.CommandText = "delete from assessmentTaxModel where assessmentTaxModelID = " & assessmentTaxModelID & vbCrLf
                query.CommandText += "delete from assessmentTaxModelFile where assessmentTaxModelID = " & assessmentTaxModelID & vbCrLf
                query.CommandText += "delete from assessmentTaxModelResults where assessmentTaxModelID = " & assessmentTaxModelID & vbCrLf
                query.CommandText += "delete from assessmentTaxModelResultsSummary where assessmentTaxModelID = " & assessmentTaxModelID & vbCrLf

                query.CommandText += "IF (select count(*) from sysobjects where name = 'liveAssessmentTaxModelResultsModel_" & assessmentTaxModelID & "') > 0 " & vbCrLf & _
                                     "BEGIN" & vbCrLf & _
                                     "     drop table liveAssessmentTaxModelResultsModel_" & assessmentTaxModelID & vbCrLf & _
                                     "End" & vbCrLf

                query.CommandText += "IF (select count(*) from sysobjects where name = 'liveAssessmentTaxModelResultsSummaryModel_" & assessmentTaxModelID & "') > 0" & vbCrLf & _
                                     "BEGIN" & vbCrLf & _
                                     "     drop Table liveAssessmentTaxModelResultsSummaryModel_" & assessmentTaxModelID & vbCrLf & _
                                     "End"

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

                'delete files from the folder
                Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, Master.errorMsg)
                Dim strPath As String = ""
                strPath = PATMAP.Global_asax.FileRootPath & general.subFolder & "\" & assessmentTaxModelID & "\"
                If Directory.Exists(strPath) Then
                    Directory.Delete(strPath, True)
                End If
                Impersonate.undoImpersonation()

                con.Close()

                If grdStart.Rows.Count > 1 Then
                    'update function search grid
                    performScenarioSearch()
                Else
                    Bind_UserDLL()
                    grdStart.DataSource = Nothing
                    grdStart.DataBind()
                    txtTotal.Text = ""
                End If

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub


    Private Sub grdStart_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdStart.RowDataBound
        Try
            'attaches confirm script to button
            common.ConfirmDel(e, 0, DataBinder.Eval(e.Row.DataItem, "assessmentTaxModelName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub


    Private Sub grdStart_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdStart.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("scenario")) Then
                performScenarioSearch()
            End If

            dt = CType(Cache("scenario"), DataTable)
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
            txtScenarioName.Text = ""
            txtDateFrom.Text = ""
            txtDateTo.Text = ""
            ddlUser.SelectedIndex = 0
            ddlSavedBase.SelectedIndex = 0
            ddlSavedSubject.SelectedIndex = 0
            ddlAudience.SelectedIndex = 0
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class