Public Partial Class editdataset
    Inherits System.Web.UI.Page
    Public Shared assessmentSubFolder As String = "Assessment"
    Public Shared taxCreditSubFolder As String = "TaxCredit"
    Public Shared POVSubFolder As String = "POV"
    Public Shared millReateSurveySubFolder As String = "MillReateSurvey"
    Public Shared K12OGSurveySubFolder As String = "K12OG"
    Public Shared potashSurveySubFolder As String = "Potash"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.taxYearModel)

                'get data set ID
                Dim editDataSetID As String
                editDataSetID = Session("editDataSetID")

                'get data source namne
                Dim dataSource As String
                dataSource = Session("dataSource")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                con.Open()

                Dim query As New SqlClient.SqlCommand
                query.Connection = con

                Dim dr As SqlClient.SqlDataReader

                If editDataSetID <> "" Then

                    'get dataset details from database
                    Select Case dataSource
                        Case "Assessment"
                            query.CommandText = "select a.assessmentID, a.dataSetName, a.notes, s.Status from assessmentDescription a join status s on a.statusID = s.statusID where a.assessmentID = " & editDataSetID

                        Case "Tax Credit"
                            query.CommandText = "select a.taxCreditID, a.dataSetName, a.notes, s.Status from taxCreditDescription a join status s on a.statusID = s.statusID where a.taxCreditID = " & editDataSetID

                        Case "POV"
                            query.CommandText = "select a.POVID, a.dataSetName, a.notes, s.Status from POVDescription a join status s on a.statusID = s.statusID where a.POVID = " & editDataSetID

                        Case "Mill Rate Survey"
                            query.CommandText = "select a.millRateSurveyID, a.dataSetName, a.notes, s.Status from millRateSurveyDescription a join status s on a.statusID = s.statusID where a.millRateSurveyID = " & editDataSetID

                        Case "K12OG"
                            query.CommandText = "select a.K12ID, a.dataSetName, a.notes, s.Status from K12Description a join status s on a.statusID = s.statusID where a.K12ID = " & editDataSetID

                        Case "Potash"
                            query.CommandText = "select a.potashID, a.dataSetName, a.notes, s.Status from potashDescription a join status s on a.statusID = s.statusID where a.potashID = " & editDataSetID
                    End Select

                    dr = query.ExecuteReader
                    dr.Read()

                    'fill the form details
                    txtDataSource.Text = dataSource

                    If IsDBNull(dr.GetValue(1)) Then
                        txtDataSetName.Text = ""
                    Else
                        txtDataSetName.Text = dr.GetValue(1)
                    End If

                    If IsDBNull(dr.GetValue(3)) Then
                        txtStatus.Text = ""
                    Else
                        txtStatus.Text = dr.GetValue(3)
                    End If

                    If IsDBNull(dr.GetValue(2)) Then
                        txtNotes.Text = ""
                    Else
                        txtNotes.Text = dr.GetValue(2)
                    End If

                    If dr.GetValue(3) = "History" Then
                        txtDataSetName.Enabled = False
                        txtNotes.Enabled = False
                        fpAttachFile.Enabled = False
                        btnAttach.Enabled = False
                        btnSave.Enabled = False
                        grdFiles.Columns(1).Visible = False
                    End If

                    'get tax year models
                    dr.Close()

                End If


                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand()
                da.SelectCommand.Connection = con

                Select Case dataSource
                    Case "Assessment"
                        da.SelectCommand.CommandText = "select t.taxYearModelName, t.year from taxYearModelDescription t where t.assessmentID=" & editDataSetID & " AND taxYearStatusID=1"

                    Case "Tax Credit"
                        da.SelectCommand.CommandText = "select t.taxYearModelName, t.year from taxYearModelDescription t where t.taxCreditID=" & editDataSetID & " AND taxYearStatusID=1"

                    Case "POV"
                        da.SelectCommand.CommandText = "select t.taxYearModelName, t.year from taxYearModelDescription t where t.POVID=" & editDataSetID & " AND taxYearStatusID=1"

                    Case "Mill Rate Survey"
                        da.SelectCommand.CommandText = "select t.taxYearModelName, t.year from taxYearModelDescription t where t.millRateSurveyID=" & editDataSetID & " AND taxYearStatusID=1"

                    Case "K12OG"
                        da.SelectCommand.CommandText = "select t.taxYearModelName, t.year from taxYearModelDescription t where t.K12ID=" & editDataSetID & " AND taxYearStatusID=1"

                    Case "Potash"
                        da.SelectCommand.CommandText = "select t.taxYearModelName, t.year from taxYearModelDescription t where t.PotashID=" & editDataSetID & " AND taxYearStatusID=1"
                End Select

                Dim dt As New DataTable
                da.Fill(dt)
                grdTaxYearModels.DataSource = dt
                grdTaxYearModels.DataBind()

                'Clean up                
                con.Close()

                'fill file data grid
                loadFileGrid(editDataSetID, dataSource)

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
            common.ChangeTitle(Session("editDataSetID"), lblTitle)
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
        Session.Remove("editDataSetID")
        Session.Remove("dataSource")
        Response.Redirect("viewdataset.aspx")
    End Sub


    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click

        Try

            'make sure required fields are filled out
            If String.IsNullOrEmpty(Trim(txtDataSetName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP85")
                Exit Sub
            End If

            If Not common.ValidateNoSpecialChar(Trim(txtDataSetName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP84")
                Exit Sub
            End If

            'get the values from the input fields
            Dim dataSetNm As String
            dataSetNm = Trim(txtDataSetName.Text.Replace("'", "''"))
            Dim notes As String
            notes = Trim(txtNotes.Text.Replace("'", "''"))

            'get data source name
            Dim dataSourceNm As String
            dataSourceNm = Session("dataSource")
            'get data set id
            Dim dataSetID As Integer
            dataSetID = CType(Session("editDataSetID"), Integer)

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()
            Dim dr As SqlClient.SqlDataReader

            'based on the data source name perform the update
            Dim dataSourceNmSql As String = ""
            Select Case dataSourceNm
                'If the data source is assessment
                Case "Assessment"
                    query.CommandText = "select dataSetName from assessmentDescription where assessmentID <> " & dataSetID & " AND statusID in (1,3) AND dataSetName='" & dataSetNm & "'"
                    dataSourceNmSql = "update assessmentDescription set dataSetName='" & dataSetNm & "', notes='" & notes & "' where assessmentID=" & dataSetID

                Case "Tax Credit"
                    query.CommandText = "select dataSetName from taxCreditDescription where taxCreditID <> " & dataSetID & " AND statusID in (1,3) AND dataSetName='" & dataSetNm & "'"
                    dataSourceNmSql = "update taxCreditDescription set dataSetName='" & dataSetNm & "', notes='" & notes & "' where taxCreditID=" & dataSetID

                Case "POV"
                    query.CommandText = "select dataSetName from POVDescription where POVID <> " & dataSetID & " AND statusID in (1,3) AND dataSetName='" & dataSetNm & "'"
                    dataSourceNmSql = "update POVDescription set dataSetName='" & dataSetNm & "', notes='" & notes & "' where POVID=" & dataSetID

                Case "Mill Rate Survey"
                    query.CommandText = "select dataSetName from millRateSurveyDescription where millRateSurveyID <> " & dataSetID & " AND statusID in (1,3) AND dataSetName='" & dataSetNm & "'"
                    dataSourceNmSql = "update millRateSurveyDescription set dataSetName='" & dataSetNm & "', notes='" & notes & "' where millRateSurveyID=" & dataSetID

                Case "K12OG"
                    query.CommandText = "select dataSetName from K12Description where K12ID <> " & dataSetID & " AND statusID in (1,3) AND dataSetName='" & dataSetNm & "'"
                    dataSourceNmSql = "update K12Description set dataSetName='" & dataSetNm & "', notes='" & notes & "' where K12ID=" & dataSetID

                Case "Potash"
                    query.CommandText = "select dataSetName from potashDescription where potashID <> " & dataSetID & " AND statusID in (1,3) AND dataSetName='" & dataSetNm & "'"
                    dataSourceNmSql = "update potashDescription set dataSetName='" & dataSetNm & "', notes='" & notes & "' where potashID=" & dataSetID
            End Select

            dr = query.ExecuteReader
            If dr.Read() Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP113")
                dr.Close()
                con.Close()
                Exit Sub
            End If

            dr.Close()
            query.CommandText = dataSourceNmSql
            query.ExecuteNonQuery()

            'clean up
            Session.Remove("editDataSetID")
            Session.Remove("dataSource")
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewdataset.aspx")

    End Sub

    Private Sub btnAttach_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAttach.Click
        Try
            'get data source name
            Dim dataSourceNm As String
            dataSourceNm = Session("dataSource")

            'get data set id
            Dim dataSetID As Integer
            dataSetID = CType(Session("editDataSetID"), Integer)

            'get user id


            Dim serverLocation(2) As String

            Dim subFolder As String = ""
            Select Case dataSourceNm
                Case "Assessment"
                    subFolder = "Assessment"

                Case "Tax Credit"
                    subFolder = "TaxCredit"

                Case "POV"
                    subFolder = "POV"

                Case "Mill Rate Survey"
                    subFolder = "MillRateSurvey"

                Case "K12OG"
                    subFolder = "K12OG"

                Case "Potash"
                    subFolder = "Potash"
            End Select

            serverLocation = common.GetFilename(fpAttachFile, dataSetID, subFolder)

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

                Select Case dataSourceNm
                    Case "Assessment"
                        query.CommandText = "insert into assessmentFile (assessmentID, [filename], dateLoaded) values (" & dataSetID & ", '" & serverLocation(1) & "','" & Now() & "')"

                    Case "Tax Credit"
                        query.CommandText = "insert into taxCreditFile (taxCreditID, [filename], dateLoaded) values (" & dataSetID & ", '" & serverLocation(1) & "','" & Now() & "')"

                    Case "POV"
                        query.CommandText = "insert into POVFile (POVID, [filename], dateLoaded) values (" & dataSetID & ", '" & serverLocation(1) & "','" & Now() & "')"

                    Case "Mill Rate Survey"
                        query.CommandText = "insert into millRateSurveyFile (millRateSurveyID, [filename], dateLoaded) values (" & dataSetID & ", '" & serverLocation(1) & "','" & Now() & "')"

                    Case "K12OG"
                        query.CommandText = "insert into K12File (K12ID, [filename], dateLoaded) values (" & dataSetID & ", '" & serverLocation(1) & "','" & Now() & "')"

                    Case "Potash"
                        query.CommandText = "insert into potashFile (potashID, [filename], dateLoaded) values (" & dataSetID & ", '" & serverLocation(1) & "','" & Now() & "')"

                End Select

                query.ExecuteNonQuery()

                con.Close()

                'fill file data grid
                loadFileGrid(dataSetID, dataSourceNm)
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
    Private Sub loadFileGrid(ByVal dataSetID As Integer, ByVal dataSourceNm As String)

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        con.Open()

        'gets all uploaded files for current tax year model
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand
        da.SelectCommand.Connection = con

        Select Case dataSourceNm
            Case "Assessment"
                da.SelectCommand.CommandText = "select fileID, [filename] from assessmentFile where assessmentID = " & dataSetID

            Case "Tax Credit"
                da.SelectCommand.CommandText = "select fileID, [filename] from taxCreditFile where taxCreditID = " & dataSetID

            Case "POV"
                da.SelectCommand.CommandText = "select fileID, [filename] from POVFile where POVID = " & dataSetID

            Case "Mill Rate Survey"
                da.SelectCommand.CommandText = "select fileID, [filename] from millRateSurveyFile where millRateSurveyID = " & dataSetID

            Case "K12OG"
                da.SelectCommand.CommandText = "select fileID, [filename] from K12File where K12ID = " & dataSetID

            Case "Potash"
                da.SelectCommand.CommandText = "select fileID, [filename] from potashFile where potashID = " & dataSetID
        End Select

        Dim dt As New DataTable
        da.Fill(dt)

        con.Close()

        grdFiles.DataSource = dt
        grdFiles.DataBind()

        'If IsNothing(Cache("taxYrModelFiles")) Then
        '    Cache.Add("taxYrModelFiles", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        'Else
        '    Cache("taxYrModelFiles") = dt
        'End If
    End Sub


    Private Sub grdFiles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdFiles.RowCommand
        Try

            Select Case e.CommandName

                Case "deleteFile"
                    'get data source name
                    Dim dataSourceNm As String
                    dataSourceNm = Session("dataSource")

                    'get data set id
                    Dim dataSetID As Integer
                    dataSetID = CType(Session("editDataSetID"), Integer)

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
                    Select Case dataSourceNm
                        Case "Assessment"
                            query.CommandText = "delete from assessmentFile where fileID = " & fileID

                        Case "Tax Credit"
                            query.CommandText = "delete from taxCreditFile where fileID = " & fileID

                        Case "POV"
                            query.CommandText = "delete from POVFile where fileID = " & fileID

                        Case "Mill Rate Survey"
                            query.CommandText = "delete from millRateSurveyFile where fileID = " & fileID

                        Case "K12OG"
                            query.CommandText = "delete from K12File where fileID = " & fileID

                        Case "Potash"
                            query.CommandText = "delete from potashFile where fileID = " & fileID
                    End Select

                    con.Open()
                    query.ExecuteNonQuery()
                    con.Close()

                    Dim subFolder As String = ""
                    Select Case dataSourceNm
                        Case "Assessment"
                            subFolder = "Assessment"

                        Case "Tax Credit"
                            subFolder = "TaxCredit"

                        Case "POV"
                            subFolder = "POV"

                        Case "Mill Rate Survey"
                            subFolder = "MillRateSurvey"

                        Case "K12OG"
                            subFolder = "K12OG"

                        Case "Potash"
                            subFolder = "Potash"
                    End Select

                    'deletes file
                    common.DeleteFile(dataSetID, fileName, subFolder)

                    'update file grid
                    loadFileGrid(dataSetID, dataSourceNm)

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

                'get data source name
                Dim dataSourceNm As String
                dataSourceNm = Session("dataSource")

                Dim subFolder As String = ""
                Select Case dataSourceNm
                    Case "Assessment"
                        subFolder = "Assessment"

                    Case "Tax Credit"
                        subFolder = "TaxCredit"

                    Case "POV"
                        subFolder = "POV"

                    Case "Mill Rate Survey"
                        subFolder = "MillRateSurvey"

                    Case "K12OG"
                        subFolder = "K12OG"

                    Case "Potash"
                        subFolder = "Potash"
                End Select

                queryString = "type=" & subFolder & "&id=" & Session("editDataSetID") & "&file=" & dt.Rows(e.Row.RowIndex).Item("fileID")

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