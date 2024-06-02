Public Partial Class editboundarymodel
    Inherits System.Web.UI.Page

    Public Shared subFolder As String = "BoundaryModels"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then

                'get Boundary Model ID
                Dim boundaryModelID As Integer
                boundaryModelID = Session("boundaryModelID")

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
                da.SelectCommand.CommandText = "select assessmentID, dataSetName from assessmentDescription where statusID = 1" & vbCrLf & _
                                                "select millRateSurveyID, dataSetName from millRateSurveyDescription where statusID = 1" & vbCrLf & _
                                                "select POVID, dataSetName from POVDescription where statusID = 1"

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

                ddlBaseAssessmentDS.DataSource = ds.Tables(0)
                ddlBaseAssessmentDS.DataValueField = "assessmentID"
                ddlBaseAssessmentDS.DataTextField = "dataSetName"
                ddlBaseAssessmentDS.DataBind()

                ddlMillRateDS.DataSource = ds.Tables(1)
                ddlMillRateDS.DataValueField = "millRateSurveyID"
                ddlMillRateDS.DataTextField = "dataSetName"
                ddlMillRateDS.DataBind()

                ddlPOVDS.DataSource = ds.Tables(2)
                ddlPOVDS.DataValueField = "POVID"
                ddlPOVDS.DataTextField = "dataSetName"
                ddlPOVDS.DataBind()

                ddlBasePOVDS.DataSource = ds.Tables(2)
                ddlBasePOVDS.DataValueField = "POVID"
                ddlBasePOVDS.DataTextField = "dataSetName"
                ddlBasePOVDS.DataBind()

                con.Open()

                If boundaryModelID <> 0 Then

                    'get tax year model details from database
                    query.CommandText = "select boundaryModelName, [year], status.status, notes, assessmentID, millRateSurveyID, POVID, baseAssessmentID, basePOVID" & vbCrLf & _
                                        "from boundaryModel inner join status on status.statusID = boundaryModel.status where boundaryModelID = " & boundaryModelID

                    dr = query.ExecuteReader

                    If dr.Read() Then
                        'fill detail into form fields
                        If Not IsDBNull(dr.GetValue(0)) Then
                            txtBoundaryModel.Text = dr.GetValue(0)
                        End If

                        If Not IsDBNull(dr.GetValue(1)) Then
                            ddlYear.SelectedValue = dr.GetValue(1)
                        End If

                        If Not IsDBNull(dr.GetValue(2)) Then
                            ddlStatus.SelectedValue = dr.GetValue(2)
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

                        If Not IsDBNull(dr.GetValue(7)) Then
                            ddlBaseAssessmentDS.SelectedValue = dr.GetValue(7)
                        End If

                        If Not IsDBNull(dr.GetValue(8)) Then
                            ddlBasePOVDS.SelectedValue = dr.GetValue(8)
                        End If


                        'If current status is history, disable all fields
                        If dr.GetValue(2).ToString = "3" Then
                            txtBoundaryModel.Enabled = False
                            ddlYear.Enabled = False
                            ddlStatus.Enabled = False
                            txtNotes.Enabled = False
                            ddlAssessmentDS.Enabled = False
                            ddlMillRateDS.Enabled = False
                            ddlPOVDS.Enabled = False
                            fpAttachFile.Enabled = False
                            btnSave.Enabled = False
                            btnAttach.Enabled = False
                        End If
                    End If

                    loadFileGrid(boundaryModelID)

                    'cleanup
                    dr.Close()

                Else

                    query.CommandText = "insert into boundaryModel (boundaryModelName, [year], status, notes,assessmentID,millRateSurveyID,POVID,dataStale,baseAssessmentID,basePOVID) values (''," & Year(Now()) & ",2,'',0,0,0,1,0,0)" & vbCrLf & _
                                        "select max(boundaryModelID) from boundaryModel"

                    dr = query.ExecuteReader()

                    dr.Read()

                    boundaryModelID = dr.Item(0)

                    Session("boundaryModelID") = boundaryModelID

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
            common.ChangeTitle(Session("boundaryModelID"), lblTitle)
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
        Session.Remove("boundaryModelID")
        Response.Redirect("viewboundarymodel.aspx")
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            'make sure required fields are filled out
            If String.IsNullOrEmpty(Trim(txtBoundaryModel.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP57")
                Exit Sub
            End If
            If Not common.ValidateNoSpecialChar(Trim(txtBoundaryModel.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP89")
                Exit Sub
            End If
            If ddlYear.SelectedItem.Text = "<Select>" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP86")
                Exit Sub
            End If
            If ddlStatus.SelectedIndex = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP62")
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
            If ddlPOVDS.SelectedValue = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP40")
                Exit Sub
            End If

            'remove any single quotes from fields
            txtNotes.Text = txtNotes.Text.Replace("'", "''")
            txtBoundaryModel.Text = txtBoundaryModel.Text.Replace("'", "''")

            'do any error checking of the form fields


            'get boundary model ID
            Dim boundaryModelID As Integer
            boundaryModelID = Session("boundaryModelID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            Dim dr As SqlClient.SqlDataReader

            con.Open()

            'check if the boundary model name alreday exists in the database
            query.CommandText = "select boundaryModelID from boundaryModel where boundaryModelID <> " & boundaryModelID & " AND boundaryModelName='" & txtBoundaryModel.Text & "' AND status in (1,3)"
            dr = query.ExecuteReader
            If dr.Read() Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP136")
                dr.Close()
                con.Close()
                Exit Sub
            End If

            dr.Close()

            'if current boundary model is active, makes other active model inactive
            If ddlStatus.SelectedIndex = 1 Then
                query.CommandText = "update boundaryModel set status = 4 where status = 1"
                query.ExecuteNonQuery()
            End If

            'update boundary model
            query.CommandText = "update boundaryModel set status = (select StatusID from status where Status = '" & ddlStatus.SelectedValue & "') , boundaryModelName = '" & Trim(txtBoundaryModel.Text) & "', year = " & ddlYear.SelectedValue & ", notes = '" & Trim(txtNotes.Text) & "', assessmentID = " & ddlAssessmentDS.SelectedValue & ", millRateSurveyID = " & ddlMillRateDS.SelectedValue & ", POVID = " & ddlPOVDS.SelectedValue & ", baseAssessmentID = " & ddlBaseAssessmentDS.SelectedValue & ", basePOVID = " & ddlBasePOVDS.SelectedValue & " where boundaryModelID = " & boundaryModelID
            query.ExecuteNonQuery()

            'reload datasets
            If ddlStatus.SelectedIndex = 1 Then
                common.calculateBoundaryModel(0)
            End If



            'clean up
            Session.Remove("boundaryModelID")
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        'return to home page
        Response.Redirect("viewboundarymodel.aspx")
    End Sub

    Private Sub btnAttach_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAttach.Click
        Try
            'get boundary model ID
            Dim boundaryModelID As Integer

            If Not IsNothing(Session("boundaryModelID")) Then
                boundaryModelID = Session("boundaryModelID")
            End If

            Dim serverLocation(2) As String

            serverLocation = common.GetFilename(fpAttachFile, boundaryModelID, subFolder)

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

                query.CommandText = "insert into boundaryModelFile (boundaryModelID, filename, dateLoaded) values (" & boundaryModelID & ", '" & serverLocation(1) & "','" & Now() & "')"
                query.ExecuteNonQuery()

                con.Close()

                'fill file data grid
                loadFileGrid(boundaryModelID)
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
    Private Sub loadFileGrid(ByVal boundaryModelID As Integer)

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        con.Open()

        'gets all uploaded files for current boundary model
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandText = "select fileID, [filename] from boundaryModelFile where boundaryModelID = " & boundaryModelID

        Dim dt As New DataTable
        da.Fill(dt)

        con.Close()

        grdFiles.DataSource = dt
        grdFiles.DataBind()

        If IsNothing(Cache("boundaryFiles")) Then
            Cache.Add("boundaryFiles", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("boundaryFiles") = dt
        End If
    End Sub

    Private Sub grdFiles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdFiles.RowCommand
        Try
            Select Case e.CommandName

                Case "deleteFile"

                    'get boundary model ID
                    Dim boundaryModelID As Integer
                    boundaryModelID = Session("boundaryModelID")

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
                    query.CommandText = "delete from boundaryModelFile where fileID = " & fileID

                    con.Open()
                    query.ExecuteNonQuery()
                    con.Close()

                    'deletes file
                    common.DeleteFile(boundaryModelID, fileName, subFolder)

                    'update file grid
                    loadFileGrid(boundaryModelID)

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

                queryString = "type=" & subFolder & "&id=" & Session("boundaryModelID") & "&file=" & dt.Rows(e.Row.RowIndex).Item("fileID")

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