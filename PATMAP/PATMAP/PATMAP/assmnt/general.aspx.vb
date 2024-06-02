Imports System.IO

Partial Public Class general
    Inherits System.Web.UI.Page

    Public Shared liveSubFolder As String = "LiveAssessmentModels"
    Public Shared subFolder As String = "AssessmentModels"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.assmnt)

                Dim levelID As Integer = Session("levelID")

                'Presentation users has to re-save current scenario in 
                'in a different scenario name
                If levelID = 3 Then
                    btnSave.ImageUrl = "~/images/btnSaveAs.gif"
                End If


                txtScenarioName.Text = Request(txtScenarioName.UniqueID)

                'get userid
                Dim userID As Integer = Session("userID")
                Dim assessmentTaxModelID As Integer = Session("assessmentTaxModelID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                con.Open()

                'fill the users drop down
                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable

                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con

                'fill the base year drop down                
                da.SelectCommand.CommandText = "select 0 as taxYearModelID,'<Please Select>' as taxYearModelName union select taxYearModelID, taxYearModelName from taxYearModelDescription where taxYearStatusID = 1 and assessmentID > 0 and millRateSurveyID > 0 and POVID > 0 and potashID > 0"
                dt = New DataTable
                da.Fill(dt)
                ddlBaseModel.DataSource = dt
                ddlBaseModel.DataValueField = "taxYearModelID"
                ddlBaseModel.DataTextField = "taxYearModelName"
                ddlBaseModel.DataBind()


                'fill the subject year drop down               
                da.SelectCommand.CommandText = "select 0 as taxYearModelID,'<Please Select>' as taxYearModelName union select taxYearModelID, taxYearModelName from taxYearModelDescription where taxYearStatusID = 1 and assessmentID > 0 and potashID > 0"
                dt = New DataTable
                da.Fill(dt)
                ddlSubjectModel.DataSource = dt
                ddlSubjectModel.DataValueField = "taxYearModelID"
                ddlSubjectModel.DataTextField = "taxYearModelName"
                ddlSubjectModel.DataBind()


                Dim dr As SqlClient.SqlDataReader
                Dim query As New SqlClient.SqlCommand
                query.Connection = con

                'populate the basetaxyrmodel and subjecttaxyrmodel fields whether it is a new or old scenario
                Dim constr As String
                constr = "select baseDesc.taxYearModelName as baseTaxYearModelName, subjectDesc.taxYearModelName as subjectTaxYearModelName, model.[description], model.notes, model.audiencePresentation, model.audienceExternalUser, model.baseTaxYearModelID, model.subjectTaxYearModelID, model.assessmentTaxModelName" & vbCrLf & _
                        "from liveAssessmentTaxModel model" & vbCrLf & _
                        "left join taxYearModelDescription baseDesc on model.baseTaxYearModelID = baseDesc.taxYearModelID" & vbCrLf & _
                        "left join taxYearModelDescription subjectDesc on model.subjectTaxYearModelID = subjectDesc.taxYearModelID" & vbCrLf & _
                        "where model.userID = " & userID
                query.CommandText = constr
                dr = query.ExecuteReader()
                dr.Read()

                txtBaseTaxYrModel.Text = dr.GetValue(0)
                txtSubjectTaxYrModel.Text = dr.GetValue(1)
                txtDescription.Text = dr.GetValue(2)
                txtNotes.Text = dr.GetValue(3)
                cblAudience.Items(0).Selected = dr.GetValue(4)
                cblAudience.Items(1).Selected = dr.GetValue(5)

                ddlBaseModel.SelectedValue = dr.GetValue(6)
                ddlSubjectModel.SelectedValue = dr.GetValue(7)

                txtScenarioName.Text = dr.GetValue(8)

                'clean up 
                dr.Close()
                con.Close()

                'If assessmentTaxModelID <> 0 Then
                'moveAssessmentFiles(assessmentTaxModelID, userID)
                loadFileGrid(userID)
                'End IF

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub general_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If IsNothing(Session("assessmentTaxModelID")) Then
            Response.Redirect("start.aspx")
        End If
    End Sub

    Private Sub btnAttach_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAttach.Click
        Try

            Dim userID As Integer = Session("userID")

            Dim serverLocation(2) As String

            serverLocation = common.GetFilename(fpAttachFile, userID, liveSubFolder)

            If Not IsNothing(serverLocation) Then

                If serverLocation(0) = "Invalid File Name" Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP119")
                    Exit Sub
                End If

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                Dim dr As SqlClient.SqlDataReader
                Dim fileID As Integer
                query.Connection = con

                con.Open()

                query.CommandText = "select max(fileID) As fileID from liveAssessmentTaxModelFile"

                dr = query.ExecuteReader()

                If dr.Read() Then
                    If Not IsDBNull(dr.Item(0)) Then
                        fileID = dr.Item(0) + 1
                    Else
                        fileID = 1
                    End If
                End If

                dr.Close()

                'get current audit trail text
                query.CommandText = "select auditTrailText from liveAssessmentTaxModel where userID=" & userID.ToString
                dr = query.ExecuteReader()
                dr.Read()
                Dim currentAuditTrailText As New StringBuilder(dr.GetValue(0).ToString)
                dr.Close()

                'update file info and audit trail text
                query.CommandText = "insert into liveAssessmentTaxModelFile (fileID, userID, filename, dateLoaded) values (" & fileID & "," & userID & ",'" & serverLocation(1) & "','" & Now() & "')" & vbCrLf
                query.CommandText += "update liveAssessmentTaxModel set auditTrailText='[" & Now.ToString("MM/dd/yyyy") & "]File attached - " & serverLocation(1) & vbCrLf & currentAuditTrailText.ToString.Replace("'", "''") & "'"
                Dim trans As SqlClient.SqlTransaction
                trans = con.BeginTransaction()
                query.Transaction = trans
                Try
                    query.ExecuteNonQuery()
                    trans.Commit()
                    Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, Master.errorMsg)
                    fpAttachFile.PostedFile.SaveAs(serverLocation(0))
                    Impersonate.undoImpersonation()
                Catch
                    trans.Rollback()
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
                    con.Close()
                    Exit Sub
                End Try

                con.Close()

                'fill file data grid
                loadFileGrid(userID)
            Else
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP37")
            End If


        Catch
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP36")
        End Try
    End Sub

    'loadFileGrid()
    'Accepts one parameter
    'ID of the user
    Private Sub loadFileGrid(ByVal userID As Integer)

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        con.Open()

        'gets all uploaded files for current tax year model
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandText = "select fileID, [filename] from liveAssessmentTaxModelFile where userID = " & userID

        Dim dt As New DataTable
        da.Fill(dt)

        con.Close()

        grdFiles.DataSource = dt
        grdFiles.DataBind()

        If IsNothing(Cache("assessmentModelFiles")) Then
            Cache.Add("assessmentModelFiles", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("assessmentModelFiles") = dt
        End If
    End Sub

    Private Sub grdFiles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdFiles.RowCommand
        Try
            Select Case e.CommandName

                Case "deleteFile"

                    'get user ID
                    Dim userID As Integer = Session("userID")

                    'get selected row index and corresponding fileID and filename to that row
                    Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                    Dim fileID As String = grdFiles.DataKeys(index).Values("fileID")
                    Dim fileName As String = grdFiles.DataKeys(index).Values("filename")

                    'setup database connection
                    Dim con As New SqlClient.SqlConnection
                    con.ConnectionString = PATMAP.Global_asax.connString
                    con.Open()
                    Dim query As New SqlClient.SqlCommand
                    query.Connection = con
                    Dim dr As SqlClient.SqlDataReader

                    'get current audit trail text
                    query.CommandText = "select auditTrailText from liveAssessmentTaxModel where userID=" & userID.ToString
                    dr = query.ExecuteReader()
                    dr.Read()
                    Dim currentAuditTrailText As New StringBuilder(dr.GetValue(0).ToString)
                    dr.Close()

                    'update file info and audit trail text
                    query.CommandText = "delete from liveAssessmentTaxModelFile where fileID = " & fileID & vbCrLf
                    query.CommandText += "update liveAssessmentTaxModel set auditTrailText='[" & Now.ToString("MM/dd/yyyy") & "]File deleted - " & fileName & vbCrLf & currentAuditTrailText.ToString.Replace("'", "''") & "'"
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

                    con.Close()

                    'deletes file
                    common.DeleteFile(userID, fileName, liveSubFolder)

                    'update file grid
                    loadFileGrid(userID)

            End Select

        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub grdFiles_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdFiles.RowDataBound
        Try
            'if current row is a datarow type
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim queryString As String
                Dim dt As DataTable

                dt = grdFiles.DataSource

                queryString = "type=" & liveSubFolder & "&id=" & Session("userID") & "&file=" & dt.Rows(e.Row.RowIndex).Item("fileID")

                'Sets url for linkbutton
                common.SetButtonLink(e, 0, DataBinder.Eval(e.Row.DataItem, "filename"), queryString)

                'attaches confirm script to button
                common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "filename"))
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Try
            'make sure required fields are filled out
            If ddlBaseModel.SelectedValue = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP50")
                Exit Sub
            End If
            If ddlSubjectModel.SelectedValue = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP51")
                Exit Sub
            End If

            'remove any single quotes from fields
            txtDescription.Text = txtDescription.Text.Replace("'", "''")
            txtNotes.Text = txtNotes.Text.Replace("'", "''")

            'do any error checking of the form fields


            'get user ID
            Dim userID As Integer = Session("userID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()
            Dim query As New SqlClient.SqlCommand
            query.Connection = con

            Dim presentation, externalUser As Integer

            If cblAudience.Items(0).Selected Then
                presentation = 1
            Else
                presentation = 0
            End If

            If cblAudience.Items(1).Selected Then
                externalUser = 1
            Else
                externalUser = 0
            End If

            Dim subjectTaxYearModelID As Integer
            Dim dr As SqlClient.SqlDataReader

            query.CommandText = "select subjectTaxYearModelID from liveAssessmentTaxModel where userID = " & userID
            dr = query.ExecuteReader()

            If dr.Read() Then
                subjectTaxYearModelID = dr.Item(0)
            End If

            dr.Close()

            'update assessment model - build query
            Dim sql As String = "update liveAssessmentTaxModel set description = '" & Trim(txtDescription.Text) & "', baseTaxYearModelID = " & ddlBaseModel.SelectedValue & ", subjectTaxYearModelID = " & ddlSubjectModel.SelectedValue & ", notes = '" & Trim(txtNotes.Text) & "', audiencePresentation = " & presentation & ", audienceExternalUser = " & externalUser & " where userID = " & userID & vbCrLf

            query.CommandText = "select l.baseTaxYearModelID, l.subjectTaxYearModelID from liveAssessmentTaxModel l join taxYearModelDescription t1 on t1.taxYearModelID=l.baseTaxYearModelID join taxYearModelDescription t2 on t2.taxYearModelID=l.subjectTaxYearModelID where l.userID=" & userID
            dr = query.ExecuteReader
            dr.Read()
            If dr.GetValue(0) <> ddlBaseModel.SelectedItem.Value Or dr.GetValue(1) <> ddlSubjectModel.SelectedItem.Value Then
                dr.Close()
                sql = sql & "update liveAssessmentTaxModel set dataStale = 1 where userID = " & userID & ")" & vbCrLf
            End If
            dr.Close()


            'update audit trail text - build query
            Dim datsetsUpdated As Boolean = 0
            Dim updateAuditTrailSQL As StringBuilder = getUpdateAuditTrailSQL(Session("userID").ToString, ddlBaseModel.SelectedItem.Value, ddlBaseModel.SelectedItem.Text.Replace("'", "''"), ddlSubjectModel.SelectedItem.Value, ddlSubjectModel.SelectedItem.Text.Replace("'", "''"), cblAudience.Items(0).Selected, cblAudience.Items(1).Selected, datsetsUpdated)

            If IsNothing(updateAuditTrailSQL) Then
                query.CommandText = sql
            Else
                query.CommandText = sql & updateAuditTrailSQL.ToString()
            End If

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

            If subjectTaxYearModelID <> ddlSubjectModel.SelectedValue Then
                common.LoadLiveTaxYearModel(ddlSubjectModel.SelectedValue, ddlBaseModel.SelectedValue)
            End If

            'clean up
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

		'Response.Redirect("pov.aspx")

		common.gotoNextPage(3, 45, Session("levelID"))
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
        Try
            txtDescription.Text = ""
            txtNotes.Text = ""
            cblAudience.Items(0).Selected = False
            cblAudience.Items(1).Selected = False
            ddlBaseModel.SelectedItem.Selected = False
            ddlBaseModel.Items(0).Selected = True
            ddlSubjectModel.SelectedItem.Selected = False
            ddlSubjectModel.Items(0).Selected = True
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
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


    Private Function getUpdateAuditTrailSQL(ByVal userID As String, ByVal baseTaxYrModelID As String, ByVal baseTaxYrModelNm As String, ByVal subjectTaxYrModelID As String, ByVal subjectTaxYrModelNm As String, ByVal presentation As Boolean, ByVal externalUser As Boolean, ByRef datsetsUpdated As Boolean) As StringBuilder

        'setup database connection            
        Dim con As New SqlClient.SqlConnection
        Dim com As New SqlClient.SqlCommand
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()
        com.Connection = con
        Dim dr As SqlClient.SqlDataReader

        com.CommandText = "select t1.taxYearModelName as oldbaseTaxYearModelNm, l.baseTaxYearModelID, t2.taxYearModelName as oldsubjectTaxYearModelNm, l.subjectTaxYearModelID, l.audiencePresentation, l.audienceExternalUser, l.auditTrailText from liveAssessmentTaxModel l join taxYearModelDescription t1 on t1.taxYearModelID=l.baseTaxYearModelID join taxYearModelDescription t2 on t2.taxYearModelID=l.subjectTaxYearModelID where l.userID=" & userID
        dr = com.ExecuteReader
        dr.Read()

        Dim sql As String = ""
        Dim auditTrail As String = ""
        If dr.GetInt32(1) <> CType(baseTaxYrModelID, Integer) Then
            auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]Base Tax Year Model changed from " & dr.GetValue(0) & " to " & baseTaxYrModelNm & vbCrLf
            datsetsUpdated = 1
        End If
        If dr.GetInt32(3) <> CType(subjectTaxYrModelID, Integer) Then
            auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]Subject Tax Year Model changed from " & dr.GetValue(2) & " to " & subjectTaxYrModelNm & vbCrLf
            datsetsUpdated = 1
        End If
        If dr.GetBoolean(4) <> presentation Then
            If presentation Then
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]Audience is updated to include presentation" & vbCrLf
            Else
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]Audience is updated to exclude presentation" & vbCrLf
            End If
        End If
        If dr.GetBoolean(5) <> externalUser Then
            If externalUser Then
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]Audience is updated to include external users" & vbCrLf
            Else
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]Audience is updated to exclude external users" & vbCrLf
            End If
        End If

        Dim updateAuditTrailSQL As StringBuilder = Nothing
        updateAuditTrailSQL = New StringBuilder("update liveAssessmentTaxModel set auditTrailText='" & auditTrail & dr.GetValue(6).ToString.Replace("'", "''") & "' where userID=" & userID.ToString)

        dr.Close()
        con.Close()

        Return updateAuditTrailSQL
    End Function
End Class