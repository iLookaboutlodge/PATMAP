Imports System.IO

Partial Public Class edittaxyearmodel
    Inherits System.Web.UI.Page

    Public Shared subFolder As String = "TaxYearModels"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.taxYearModel)

                'get Tax Year Model ID
                Dim taxYearModelID As Integer
                taxYearModelID = Session("taxYearModelID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString

                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader

                'fill the all dropdowns
                ddlYear.DataSource = common.GetYears()
                ddlYear.DataBind()

                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                'Donna - Added PEMRDescription table.
                da.SelectCommand.CommandText = "select assessmentID, dataSetName from assessmentDescription where statusID = 1" & vbCrLf & _
                                                "select millRateSurveyID, dataSetName from millRateSurveyDescription where statusID = 1" & vbCrLf & _
                                                "select POVID, dataSetName from POVDescription where statusID = 1" & vbCrLf & _
                                                "select K12ID, dataSetName from K12Description where statusID = 1" & vbCrLf & _
                                                "select taxCreditID, dataSetName from taxCreditDescription where statusID = 1" & vbCrLf & _
                                                "select potashID, dataSetName from potashDescription where statusID = 1" & vbCrLf & _
                                                "SELECT PEMRID, dataSetName FROM PEMRDescription WHERE statusID = 1"

                Dim ds As New DataSet
                Dim currentRow As DataRow
                Dim counter As Integer

                da.Fill(ds)

                For counter = 0 To ds.Tables.Count - 1
                    currentRow = Nothing
                    currentRow = ds.Tables(counter).NewRow
                    currentRow.Item(0) = 0
                    currentRow.Item(1) = "<Select>"
                    ds.Tables(counter).Rows.InsertAt(currentRow, 0)
                Next

                ddlAssessmentDS.DataSource = ds.Tables(0)
                ddlAssessmentDS.DataValueField = "assessmentID"
                ddlAssessmentDS.DataTextField = "dataSetName"
                ddlAssessmentDS.DataBind()

                ddlMillRateDS.DataSource = ds.Tables(1)
                ddlMillRateDS.DataValueField = "millRateSurveyID"
                ddlMillRateDS.DataTextField = "dataSetName"
                ddlMillRateDS.DataBind()

                ddlPOVDS.DataSource = ds.Tables(2)
                ddlPOVDS.DataValueField = "POVID"
                ddlPOVDS.DataTextField = "dataSetName"
                ddlPOVDS.DataBind()

                'Donna - Remove Tax Credit and K-12 OG.
                'ddlKOGDS.DataSource = ds.Tables(3)
                'ddlKOGDS.DataValueField = "K12ID"
                'ddlKOGDS.DataTextField = "dataSetName"
                'ddlKOGDS.DataBind()

                'ddlTaxCreditDS.DataSource = ds.Tables(4)
                'ddlTaxCreditDS.DataValueField = "taxCreditID"
                'ddlTaxCreditDS.DataTextField = "dataSetName"
                'ddlTaxCreditDS.DataBind()

                ddlPotashDS.DataSource = ds.Tables(5)
                ddlPotashDS.DataValueField = "potashID"
                ddlPotashDS.DataTextField = "dataSetName"
                ddlPotashDS.DataBind()

                'Donna start
                ddlPEMR.DataSource = ds.Tables(6)
                ddlPEMR.DataValueField = "PEMRID"
                ddlPEMR.DataTextField = "dataSetName"
                ddlPEMR.DataBind()
                'Donna end

                con.Open()

                If taxYearModelID <> 0 Then

                    'get tax year model details from database
                    'Donna - Added PEMRID.
                    query.CommandText = "select taxYearModelName, [year], taxYearStatus, notes, assessmentID, millRateSurveyID, POVID, K12ID, taxCreditID, PotashID, PEMRID" & vbCrLf & _
                                        "from taxYearModelDescription" & vbCrLf & _
                                        "left outer join taxYearStatus on taxYearStatus.taxYearStatusID = taxYearModelDescription.taxYearStatusID" & vbCrLf & _
                                        "where taxYearModelID = " & taxYearModelID

                    dr = query.ExecuteReader

                    If dr.Read() Then
                        'fill detail into form fields
                        If Not IsDBNull(dr.GetValue(0)) Then
                            txtTaxYrModel.Text = dr.GetValue(0)
                        End If

                        If Not IsDBNull(dr.GetValue(1)) Then
                            ddlYear.SelectedValue = dr.GetValue(1)
                        End If

                        If Not IsDBNull(dr.GetValue(2)) Then
                            txtStatus.Text = dr.GetValue(2)
                        End If

                        If Not IsDBNull(dr.GetValue(3)) Then
                            txtNotes.Text = dr.GetValue(3)
                        End If

                        If Not IsDBNull(dr.GetValue(4)) Then
                            ddlAssessmentDS.SelectedValue = dr.GetValue(4)
                        End If

                        If Not IsDBNull(dr.GetValue(5)) Then
                            ddlMillRateDS.SelectedValue = dr.GetValue(5)
                        End If

                        If Not IsDBNull(dr.GetValue(6)) Then
                            ddlPOVDS.SelectedValue = dr.GetValue(6)
                        End If

                        'Donna - Remove Tax Credit and K-12 OG.
                        'If Not IsDBNull(dr.GetValue(7)) Then
                        '    ddlKOGDS.SelectedValue = dr.GetValue(7)
                        'End If

                        'If Not IsDBNull(dr.GetValue(8)) Then
                        '    ddlTaxCreditDS.SelectedValue = dr.GetValue(8)
                        'End If

                        If Not IsDBNull(dr.GetValue(9)) Then
                            ddlPotashDS.SelectedValue = dr.GetValue(9)
                        End If

                        'Donna start
                        If Not IsDBNull(dr.GetValue(10)) Then
                            ddlPEMR.SelectedValue = dr.GetValue(10)
                        End If
                        'Donna end

                        'If current status is history, disable all fields
                        If dr.GetValue(2) = "History" Then
                            txtTaxYrModel.Enabled = False
                            ddlYear.Enabled = False
                            txtNotes.Enabled = False
                            ddlAssessmentDS.Enabled = False
                            ddlMillRateDS.Enabled = False
                            ddlPOVDS.Enabled = False
                            'Donna - Remove Tax Credit and K-12 OG.
                            'ddlKOGDS.Enabled = False
                            'ddlTaxCreditDS.Enabled = False
                            ddlPotashDS.Enabled = False
                            ddlPEMR.Enabled = False 'Donna
                            fpAttachFile.Enabled = False
                            btnSave.Enabled = False
                            btnAttach.Enabled = False
                            grdFiles.Columns(1).Visible = False
                        End If
                    End If

                    loadFileGrid(taxYearModelID)

                    'cleanup
                    dr.Close()

                Else

                    query.CommandText = "insert into taxYearModelDescription (taxYearModelName, [year], taxYearStatusID, notes) values (''," & Year(Now()) & ",2,'')" & vbCrLf & _
                                        "select max(taxYearModelID) from taxYearModelDescription"

                    dr = query.ExecuteReader()

                    dr.Read()

                    taxYearModelID = dr.Item(0)

                    dr.Close()

                    Session("taxYearModelID") = taxYearModelID

                End If

                'clean up
                con.Close()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("taxYearModelID"), lblTitle)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            common.UndoChange()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("taxYearModelID")
        Response.Redirect("viewtaxyearmodel.aspx")
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            'make sure required fields are filled out
            If String.IsNullOrEmpty(Trim(txtTaxYrModel.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP47")
                Exit Sub
            End If
            If Not common.ValidateNoSpecialChar(Trim(txtTaxYrModel.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP81")
                Exit Sub
            End If
            If ddlYear.SelectedItem.Text = "<Select>" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP86")
                Exit Sub
            End If
            If ddlAssessmentDS.SelectedValue = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP46")
                Exit Sub
            End If
            If ddlMillRateDS.SelectedValue = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP45")
                Exit Sub
            End If
            If ddlPotashDS.SelectedValue = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP140")
                Exit Sub
            End If

            'remove any single quotes from fields
            txtNotes.Text = txtNotes.Text.Replace("'", "''")
            txtTaxYrModel.Text = txtTaxYrModel.Text.Replace("'", "''")

            'do any error checking of the form fields


            'get Tax Year Model ID
            Dim taxYearModelID As Integer
            taxYearModelID = Session("taxYearModelID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            Dim dr As SqlClient.SqlDataReader

            'update tax year model
            'check if the tax year model name alreday exists in the database
            query.CommandText = "select taxYearModelID from taxYearModelDescription where taxYearModelID <> " & taxYearModelID & " AND taxYearModelName='" & txtTaxYrModel.Text & "' AND taxYearStatusID in (1,3)"
            dr = query.ExecuteReader
            If dr.Read() Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP111")
                dr.Close()
                con.Close()
                Exit Sub
            End If

            dr.Close()
            'Donna - Added PEMRID. Removed K12ID and taxCreditID.
            'query.CommandText = "update taxYearModelDescription set dataStale = 1, taxYearStatusID = 1, taxYearModelName = '" & Trim(txtTaxYrModel.Text) & "', year = " & ddlYear.SelectedValue & ", notes = '" & Trim(txtNotes.Text) & "', assessmentID = " & ddlAssessmentDS.SelectedValue & ", millRateSurveyID = " & ddlMillRateDS.SelectedValue & ", POVID = " & ddlPOVDS.SelectedValue & ", taxCreditID = " & ddlTaxCreditDS.SelectedValue & ", K12ID = " & ddlKOGDS.SelectedValue & ", PotashID = " & ddlPotashDS.SelectedValue & ", PEMRID = " & ddlPEMR.SelectedValue & " where taxYearModelID = " & taxYearModelID & vbCrLf
            query.CommandText = "update taxYearModelDescription set dataStale = 1, taxYearStatusID = 1, taxYearModelName = '" & Trim(txtTaxYrModel.Text) & "', year = " & ddlYear.SelectedValue & ", notes = '" & Trim(txtNotes.Text) & "', assessmentID = " & ddlAssessmentDS.SelectedValue & ", millRateSurveyID = " & ddlMillRateDS.SelectedValue & ", POVID = " & ddlPOVDS.SelectedValue & ", PotashID = " & ddlPotashDS.SelectedValue & ", PEMRID = " & ddlPEMR.SelectedValue & " where taxYearModelID = " & taxYearModelID & vbCrLf
            query.CommandText += "update assessmentTaxModel set dataStale = 1 where BaseTaxYearModelID = " & taxYearModelID & vbCrLf
            query.CommandText += "update assessmentTaxModel set dataStale = 1, subjassessmentID = " & ddlAssessmentDS.SelectedValue & ", subjmillRateSurveyID = " & ddlMillRateDS.SelectedValue & ", subjPotashID = " & ddlPotashDS.SelectedValue & ", subjPEMRID = " & ddlPEMR.SelectedValue & " where SubjectTaxYearModelID = " & taxYearModelID & vbCrLf
            query.CommandText += "update assessmentDescription set dataStale = 1 where assessmentID = (select assessmentID from taxYearModelDescription where taxYearModelID = " & taxYearModelID & ")" & vbCrLf

            query.ExecuteNonQuery()

            'Dim povSQL As String = ""
            'If ddlPOVDS.SelectedValue <> 0 Then
            '    query.CommandText = "select subjPOVID from assessmentTaxModel where SubjectTaxYearModelID = " & taxYearModelID & vbCrLf
            '    dr = query.ExecuteReader
            '    While dr.Read() = True
            '        povSQL += "delete POV where POVID = " & dr.GetValue(0) & vbCrLf
            '        povSQL += "insert into POV select " & dr.GetValue(0) & ", p1.taxClassID, p1.POV from POV p1 where p1.POVID = " & ddlPOVDS.SelectedValue & vbCrLf
            '    End While
            '    dr.Close()
            '    query.CommandText = povSQL
            '    query.ExecuteNonQuery()
            'End If

            'Dim taxCreditSQL As String = ""
            'If ddlTaxCreditDS.SelectedValue <> 0 Then
            '    query.CommandText = "select subjtaxCreditID from assessmentTaxModel where SubjectTaxYearModelID = " & taxYearModelID & vbCrLf
            '    dr = query.ExecuteReader
            '    While dr.Read() = True
            '        taxCreditSQL += "delete taxCredit where taxCreditID = " & dr.GetValue(0) & vbCrLf
            '        taxCreditSQL += "insert into taxCredit select " & dr.GetValue(0) & ", t1.taxClassID, t1.taxCredit, t1.capped from taxCredit t1 where t1.taxCreditID = " & ddlTaxCreditDS.SelectedValue & vbCrLf
            '    End While
            '    dr.Close()
            '    query.CommandText = taxCreditSQL
            '    query.ExecuteNonQuery()
            'End If

            'If ddlKOGDS.SelectedValue <> 0 Then
            '    query.CommandText += "update assessmentTaxModel set SubjectK12ID = " & ddlKOGDS.SelectedValue & " where SubjectTaxYearModelID = " & taxYearModelID & vbCrLf
            'End If

            'clean up
            Session.Remove("taxYearModelID")
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        'return to home page
        Response.Redirect("viewtaxyearmodel.aspx")
    End Sub

    Private Sub btnAttach_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAttach.Click
        Try

            'get Tax Year Model ID
            Dim taxYearModelID As Integer

            If Not IsNothing(Session("taxYearModelID")) Then
                taxYearModelID = Session("taxYearModelID")
            End If

            Dim serverLocation(2) As String

            serverLocation = common.GetFilename(fpAttachFile, taxYearModelID, subFolder)

            If Not IsNothing(serverLocation) Then

                If serverLocation(0) = "Invalid File Name" Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP119")
                    Exit Sub
                End If

                Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, Master.errorMsg)
                fpAttachFile.PostedFile.SaveAs(serverLocation(0))
                Impersonate.undoImpersonation()

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con

                con.Open()

                query.CommandText = "insert into taxYearModelFile (taxYearModelID, filename, dateLoaded) values (" & taxYearModelID & ", '" & serverLocation(1) & "','" & Now() & "')"
                query.ExecuteNonQuery()

                con.Close()

                'fill file data grid
                loadFileGrid(taxYearModelID)
            Else
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP37")
            End If


        Catch
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP36")
            Exit Sub
        End Try
    End Sub

    'loadFileGrid()
    'Accepts one parameter
    'ID of the tax year model
    Private Sub loadFileGrid(ByVal taxYearModelID As Integer)

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        con.Open()

        'gets all uploaded files for current tax year model
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandText = "select fileID, [filename] from taxYearModelFile where taxYearModelID = " & taxYearModelID

        Dim dt As New DataTable
        da.Fill(dt)

        con.Close()

        grdFiles.DataSource = dt
        grdFiles.DataBind()

        If IsNothing(Cache("taxYrModelFiles")) Then
            Cache.Add("taxYrModelFiles", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("taxYrModelFiles") = dt
        End If
    End Sub

    Private Sub grdFiles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdFiles.RowCommand
        Try
            Select Case e.CommandName

                Case "deleteFile"

                    'get Tax Year Model ID
                    Dim taxYearModelID As Integer
                    taxYearModelID = Session("taxYearModelID")

                    'get selected row index and corresponding fileID and filename to that row
                    Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                    Dim fileID As String = grdFiles.DataKeys(index).Values("fileID")
                    Dim fileName As String = grdFiles.DataKeys(index).Values("filename")

                    'setup database connection
                    Dim con As New SqlClient.SqlConnection
                    con.ConnectionString = PATMAP.Global_asax.connString
                    Dim query As New SqlClient.SqlCommand
                    query.Connection = con

                    'delete file from database
                    query.CommandText = "delete from taxYearModelFile where fileID = " & fileID

                    con.Open()
                    query.ExecuteNonQuery()
                    con.Close()

                    'deletes file
                    common.DeleteFile(taxYearModelID, fileName, subFolder)

                    'update file grid
                    loadFileGrid(taxYearModelID)

            End Select

        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

    End Sub

    Private Sub grdFiles_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdFiles.RowDataBound
        Try
            'if current row is a datarow type
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim queryString As String
                Dim dt As DataTable

                dt = grdFiles.DataSource

                queryString = "type=" & subFolder & "&id=" & Session("taxYearModelID") & "&file=" & dt.Rows(e.Row.RowIndex).Item("fileID")

                'Sets url for linkbutton
                common.SetButtonLink(e, 0, DataBinder.Eval(e.Row.DataItem, "filename"), queryString)

                'attaches confirm script to button
                common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "filename"))
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try
    End Sub
End Class