Imports System.IO
Imports Microsoft.SqlServer.Dts.Runtime

Partial Public Class synchronize
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            btnSubmitToLaptop.Attributes.Add("onClick", "javascript: return confirmPrompt('Are you sure you want to synchronize the laptop?')")
            btnSubmitToServer.Attributes.Add("onClick", "javascript: return confirmPrompt('Are you sure you want to synchronize the server?')")

            If IsNothing(Session("synFinsh")) Then
                panelBtns.Visible = True
                panelResult.Visible = False
            Else
                'lblSynProgressBar.Text = Session("newID") 'Session("pckgResults")                
                Session.Remove("synFinsh")
                panelResult.Visible = True
                panelBtns.Visible = False
            End If
        End If
    End Sub

    Private Sub btnSubmitToLaptop_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmitToLaptop.Click
        Try
            CopyFilesToLaptop(PATMAP.Global_asax.synchronizeFolder)
            CopyTablesToLaptop()
            CopySavedModelsTablesToLaptop(1)
            CopySavedModelsTablesToLaptop(2)
            UpdateLastDate()
            'CopyReportsToLaptop()

            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()
            Dim query As New SqlClient.SqlCommand

            query.Connection = con
            query.CommandText = "Insert into satelliteInfo (machineName) values('" & PATMAP.Global_asax.machineName & "')"
            query.ExecuteNonQuery()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
        Response.Write(common.HideDiv())
        Response.Flush()

        Session.Add("synFinsh", "true")

        Response.Write("<script language=javascript>window.navigate('synchronize.aspx');</script>")
        Response.End()

        Session.Remove("reportDS")
    End Sub

    Private Sub CopyFilesToLaptop(ByVal destination As String)

        Response.Write(common.JavascriptFunctions())
        Response.Flush()

        Response.Write(common.DisplayDiv("Copy directories and files...", -1))
        Response.Flush()

        Impersonate.impersonateValidUser(Global_asax.synchronizeUser, Global_asax.synchronizeDomainName, Global_asax.synchronizePassword, Master.errorMsg)

        'copy directories from the root
        Dim directoryPaths As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        directoryPaths = My.Computer.FileSystem.GetDirectories(PATMAP.Global_asax.synchronizeFileRootPath, FileIO.SearchOption.SearchTopLevelOnly)

        Dim directoryPath As String
        Dim directoryName As String
        For Each directoryPath In directoryPaths
            directoryName = My.Computer.FileSystem.GetDirectoryInfo(directoryPath).Name
            If directoryName <> "Assessment" Then 'Copies all directories EXCEPT the "Assessment" folder
                My.Computer.FileSystem.CopyDirectory(directoryPath, destination & directoryName, True)
            End If
        Next

        'copy files from the root
        Dim filePaths As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        filePaths = My.Computer.FileSystem.GetFiles(PATMAP.Global_asax.synchronizeFileRootPath, FileIO.SearchOption.SearchTopLevelOnly, "*.*")

        Dim filePath As String
        Dim fileName As String
        For Each filePath In filePaths
            fileName = My.Computer.FileSystem.GetFileInfo(filePath).Name
            My.Computer.FileSystem.CopyFile(filePath, destination & fileName, True)
        Next

        Impersonate.undoImpersonation()
    End Sub

    Private Sub CopyTablesToLaptop()

        Response.Write(common.ChangeText("Synchronize the database-step1..."))
        Response.Flush()

        'transfer tables
        Dim pkg As New Package
        Dim app As New Application
        Dim pkgResults As DTSExecResult
        Dim _PackagePath As String = System.Configuration.ConfigurationManager.AppSettings("PackagePath").ToString
        Dim _PackageName As String = System.Configuration.ConfigurationManager.AppSettings("PackageName_CopyTablesToLaptop").ToString
        Dim PackageName As String = (Server.MapPath(Replace(_PackagePath, "..", "")) + _PackageName)

        pkg = app.LoadPackage((PackageName + ".dtsx"), Nothing)
        Dim vars As Variables = pkg.Variables
        vars("localServer").Value = PATMAP.Global_asax.machineName
        vars("prodServer").Value = PATMAP.Global_asax.SQLEngineServer
        pkgResults = pkg.Execute()
        If pkgResults = DTSExecResult.Failure Then
            Master.errorMsg = pkg.Errors(0).Description & " Tables"
        End If

        Dim conLap As New SqlClient.SqlConnection
        conLap.ConnectionString = PATMAP.Global_asax.connStringLap
        conLap.Open()
        Dim queryLap As New SqlClient.SqlCommand
        queryLap.Connection = conLap
        queryLap.CommandTimeout = 60000

        queryLap.CommandText = "Exec addDefaultValues"
        queryLap.ExecuteNonQuery()

        conLap.Close()
    End Sub
    Private Sub CopySavedModelsTablesToLaptop(ByVal w As Integer)

        Response.Write(common.ChangeText("Synchronize the database-step2" & w & "..."))
        Response.Flush()


        'transfer tables
        Dim pkg As New Package
        Dim app As New Application
        Dim pkgResults As DTSExecResult
        Dim _PackagePath As String = System.Configuration.ConfigurationManager.AppSettings("PackagePath").ToString


        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        query.CommandTimeout = 60000
        Dim dr As SqlClient.SqlDataReader

        Dim conLap As New SqlClient.SqlConnection
        conLap.ConnectionString = PATMAP.Global_asax.connStringLap
        conLap.Open()
        Dim queryLap As New SqlClient.SqlCommand
        queryLap.Connection = conLap
        queryLap.CommandTimeout = 60000


        query.CommandText = "SELECT DISTINCT assessmentTaxModel.assessmentTaxModelID, assessmentTaxModel.BaseTaxYearModelID, assessmentTaxModel.SubjectTaxYearModelID, assessmentTaxModel.subjassessmentID, taxYearModelDescription.assessmentID " & vbCrLf & _
                            "FROM assessmentTaxModel " & vbCrLf & _
                            "INNER JOIN  taxYearModelDescription ON assessmentTaxModel.BaseTaxYearModelID = taxYearModelDescription.taxYearModelID " & vbCrLf & _
                            "WHERE (assessmentTaxModel.audiencePresentation = 1) AND (NOT (assessmentTaxModel.BaseTaxYearModelID IS NULL)) "
        dr = query.ExecuteReader


        If dr.HasRows Then

            While dr.Read()
                If w = 1 Then
                    Dim _PackageName As String = System.Configuration.ConfigurationManager.AppSettings("PackageName_CopySavedModelsTablesToLaptop").ToString
                    Dim PackageName As String = (Server.MapPath(Replace(_PackagePath, "..", "")) + _PackageName)
                    pkg = app.LoadPackage((PackageName + ".dtsx"), Nothing)
                    Dim vars As Variables = pkg.Variables
                    vars("localServer").Value = PATMAP.Global_asax.machineName
                    vars("prodServer").Value = PATMAP.Global_asax.SQLEngineServer
                    queryLap.CommandText = "exec creatCompareBaseandSubject " & "liveAssessmentTaxModelResultsModel_" & dr.GetValue(0) & "," & "liveAssessmentTaxModelResultsSummaryModel_" & dr.GetValue(0) & ",1"
                    queryLap.ExecuteNonQuery()
                    vars("tableNameR").Value = "liveAssessmentTaxModelResultsModel_" & dr.GetValue(0)
                    vars("tableNameS").Value = "liveAssessmentTaxModelResultsSummaryModel_" & dr.GetValue(0)
                    vars("tableNameR2").Value = "liveAssessmentTaxModelResultsModel_" & dr.GetValue(0)
                    vars("tableNameS2").Value = "liveAssessmentTaxModelResultsSummaryModel_" & dr.GetValue(0)
                    pkgResults = pkg.Execute()
                    If pkgResults.ToString.ToLower.Equals("success") Then
                    Else
                        'Response.Write(common.ChangeText(".. " & pkg.Errors(0).Description & ".."))
                        'Response.Flush()
                        If pkgResults = DTSExecResult.Failure Then
                            Master.errorMsg = pkg.Errors(0).Description & " Tables1"
                        End If
                    End If
                ElseIf w = 2 Then
                    Dim _PackageName As String = System.Configuration.ConfigurationManager.AppSettings("PackageName_CopySavedCompareTablesToLaptop").ToString
                    Dim PackageName As String = (Server.MapPath(Replace(_PackagePath, "..", "")) + _PackageName)
                    pkg = app.LoadPackage((PackageName + ".dtsx"), Nothing)
                    Dim vars As Variables = pkg.Variables
                    vars("localServer").Value = PATMAP.Global_asax.machineName
                    vars("prodServer").Value = PATMAP.Global_asax.SQLEngineServer
                    queryLap.CommandText = "exec creatCompareBaseandSubject " & "assessmentBase_" & dr.GetValue(3) & "_" & dr.GetValue(4) & "," & "assessmentSubject_" & dr.GetValue(3) & "_" & dr.GetValue(4) & ",2"
                    queryLap.ExecuteNonQuery()
                    vars("tableNameR").Value = "assessmentBase_" & dr.GetValue(3) & "_" & dr.GetValue(4)
                    vars("tableNameS").Value = "assessmentSubject_" & dr.GetValue(3) & "_" & dr.GetValue(4)
                    pkgResults = pkg.Execute()
                    If pkgResults.ToString.ToLower.Equals("success") Then
                    Else
                        'Response.Write(common.ChangeText(".. " & pkg.Errors(0).Description & ".."))
                        'Response.Flush()
                        If pkgResults = DTSExecResult.Failure Then
                            Master.errorMsg = pkg.Errors(0).Description & " Tables2"
                        End If
                    End If
                End If
                ' pkg.Dispose()
            End While
        End If


    End Sub
    'Private Sub CopyReportsToLaptop()

    '    'report types
    '    'create an instance of our main server's and local web service
    '    Dim ws As New MainServerWebService.ReportingService2005
    '    Dim wsCredential As New reportServerCredentials
    '    Dim localws As New DestinationWebService.ReportingService2005

    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()

    '    Dim query As New SqlClient.SqlCommand
    '    Dim dr As SqlClient.SqlDataReader
    '    query.Connection = con

    '    Dim assessmentTables, assessmentGraphs, export, systemStat, boundaryTables, boundaryGraphs As Boolean
    '    Dim levelID As Integer

    '    If Session("levelID") <> "" And Not IsNothing(Session("levelID")) Then
    '        levelID = Session("levelID")
    '    End If

    '    query.CommandText = "select" & vbCrLf & _
    '                        "(select count(*) from levelsPermission where access = 1 and levelID = " & levelID & " and screenNameID = 50) as assessmentTables," & vbCrLf & _
    '                        "(select count(*) from levelsPermission where access = 1 and levelID = " & levelID & " and screenNameID = 51) as assessmentGraphs," & vbCrLf & _
    '                        "(select count(*) from levelsPermission where access = 1 and levelID = " & levelID & " and screenNameID = 80) as boundaryTables," & vbCrLf & _
    '                        "(select count(*) from levelsPermission where access = 1 and levelID = " & levelID & " and screenNameID = 81) as boundaryGraphs," & vbCrLf & _
    '                        "(select count(*) from levelsPermission where access = 1 and levelID = " & levelID & " and screenNameID = 87) as export," & vbCrLf & _
    '                        "(select count(*) from levelsPermission where access = 1 and levelID = " & levelID & " and screenNameID = 89) as systemStat"
    '    dr = query.ExecuteReader

    '    If dr.Read() Then
    '        assessmentTables = dr.Item(0)
    '        assessmentGraphs = dr.Item(1)
    '        boundaryTables = dr.Item(2)
    '        boundaryGraphs = dr.Item(3)
    '        export = dr.Item(4)
    '        systemStat = dr.Item(5)
    '    End If

    '    con.Close()

    '    'pass in the default credentials - meaning currently logged in user
    '    'ws.Credentials = System.Net.CredentialCache.DefaultCredentials
    '    'pass in the network credentials to access the reporting service
    '    wsCredential.useMainServer = True
    '    ws.Credentials = wsCredential.NetworkCredentials()
    '    'localws.Credentials = New reportServerCredentials().NetworkCredentials
    '    localws.Credentials = wsCredential.NetworkCredentials()

    '    If assessmentTables Then
    '        Response.Write(common.ChangeText("Copy Assessment and Tax Modeling - Tables Reports..."))
    '        Response.Flush()

    '        'creates reports for Assessment and Tax Modeling - Tables page
    '        CreateReport(ws, localws, PATMAP.Global_asax.ReportTablesFolder)
    '    End If

    '    If assessmentGraphs Then
    '        Response.Write(common.ChangeText("Copy Assessment and Tax Modeling - Graphs Reports..."))
    '        Response.Flush()

    '        'creates reports for Assessment and Tax Modeling - Graphs page
    '        CreateReport(ws, localws, PATMAP.Global_asax.ReportGraphsFolder)
    '    End If

    '    If export Then
    '        Response.Write(common.ChangeText("Copy Export Reports..."))
    '        Response.Flush()

    '        'creates reports for Export page
    '        CreateReport(ws, localws, PATMAP.Global_asax.ReportExportFolder)
    '    End If

    '    If systemStat Then
    '        Response.Write(common.ChangeText("Copy System Statistic Reports..."))
    '        Response.Flush()

    '        'creates reports for System Statistic page
    '        CreateReport(ws, localws, PATMAP.Global_asax.ReportSystemFolder)
    '    End If

    'End Sub

    'Private Sub CreateReport(ByRef ws As MainServerWebService.ReportingService2005, ByRef localws As DestinationWebService.ReportingService2005, ByVal reportPath As String)
    '    Dim items() As MainServerWebService.CatalogItem
    '    Dim catItem As MainServerWebService.CatalogItem
    '    Dim reportDataSources() As MainServerWebService.DataSource
    '    Dim reportDataSource As MainServerWebService.DataSource
    '    Dim reportDataSourceRef As DestinationWebService.DataSourceReference
    '    Dim reportDefinition As Byte()

    '    Dim folders As String()
    '    Dim parentFolder As String = "/"
    '    Dim currentFolder As String
    '    Dim counter As Integer

    '    'checks if the folder exists in the report server
    '    If ws.GetItemType(reportPath) = MainServerWebService.ItemTypeEnum.Folder Then

    '        If localws.GetItemType(reportPath) = DestinationWebService.ItemTypeEnum.Folder Then
    '            localws.DeleteItem(reportPath)
    '        End If

    '        folders = reportPath.Split("/")

    '        For counter = 0 To folders.Length - 1
    '            If folders(counter) <> "" Then

    '                parentFolder &= folders(counter - 1)

    '                If folders(counter - 1) <> "" Then
    '                    currentFolder = parentFolder & "/" & folders(counter)
    '                Else
    '                    currentFolder = parentFolder & folders(counter)
    '                End If

    '                If localws.GetItemType(currentFolder) = DestinationWebService.ItemTypeEnum.Unknown Then
    '                    localws.CreateFolder(folders(counter), parentFolder, Nothing)
    '                End If
    '            End If
    '        Next

    '        'gets a list of reports from the main report server
    '        items = ws.ListChildren(reportPath, True)

    '        'gets the report definition and creates the report in the satellite's report server
    '        For Each catItem In items

    '            If ws.GetItemType(catItem.Path) = MainServerWebService.ItemTypeEnum.Report Then

    '                reportDefinition = ws.GetReportDefinition(catItem.Path)
    '                reportDataSources = ws.GetItemDataSources(catItem.Path)

    '                localws.CreateReport(catItem.Name, reportPath, False, reportDefinition, Nothing)

    '                Dim reportNewDataSources(reportDataSources.Length - 1) As DestinationWebService.DataSource

    '                counter = 0

    '                For Each reportDataSource In reportDataSources
    '                    If Session("reportDS") <> reportDataSource.Name Then
    '                        CreateReportDataSource(ws, localws, reportDataSource, PATMAP.Global_asax.ReportDataSourceFolder)
    '                        Session("reportDS") = reportDataSource.Name
    '                    End If

    '                    reportDataSourceRef = New DestinationWebService.DataSourceReference
    '                    reportDataSourceRef.Reference = PATMAP.Global_asax.ReportDataSourceFolder & "/" & reportDataSource.Name
    '                    reportNewDataSources(counter) = New DestinationWebService.DataSource
    '                    reportNewDataSources(counter).Item = CType(reportDataSourceRef, DestinationWebService.DataSourceDefinitionOrReference)
    '                    reportNewDataSources(counter).Name = reportDataSource.Name
    '                Next

    '                If reportNewDataSources.Length > 0 Then
    '                    localws.SetItemDataSources(catItem.Path, reportNewDataSources)
    '                End If

    '            End If

    '        Next

    '    End If

    'End Sub

    'Private Sub CreateReportDataSource(ByRef ws As MainServerWebService.ReportingService2005, ByRef localws As DestinationWebService.ReportingService2005, ByRef reportDS As MainServerWebService.DataSource, ByVal dataSourcePath As String)

    '    Dim mainDSDef As MainServerWebService.DataSourceDefinition
    '    Dim localDSDef As DestinationWebService.DataSourceDefinition
    '    Dim folders As String()
    '    Dim parentFolder As String = "/"
    '    Dim currentFolder As String
    '    Dim counter As Integer

    '    If localws.GetItemType(dataSourcePath & "/" & reportDS.Name) = DestinationWebService.ItemTypeEnum.DataSource Then
    '        localws.DeleteItem(dataSourcePath & "/" & reportDS.Name)
    '    End If

    '    folders = dataSourcePath.Split("/")

    '    For counter = 0 To folders.Length - 1
    '        If folders(counter) <> "" Then

    '            parentFolder &= folders(counter - 1)

    '            If folders(counter - 1) <> "" Then
    '                currentFolder = parentFolder & "/" & folders(counter)
    '            Else
    '                currentFolder = parentFolder & folders(counter)
    '            End If

    '            If localws.GetItemType(currentFolder) = DestinationWebService.ItemTypeEnum.Unknown Then
    '                localws.CreateFolder(folders(counter), parentFolder, Nothing)
    '            End If
    '        End If
    '    Next

    '    mainDSDef = ws.GetDataSourceContents(dataSourcePath & "/" & reportDS.Name)

    '    localDSDef = New DestinationWebService.DataSourceDefinition
    '    localDSDef.CredentialRetrieval = mainDSDef.CredentialRetrieval
    '    localDSDef.ConnectString = "Data Source=" & PATMAP.Global_asax.SQLEngineServer & ";Initial Catalog=" & PATMAP.Global_asax.DBName
    '    localDSDef.UserName = PATMAP.Global_asax.DBUser
    '    localDSDef.Password = PATMAP.Global_asax.DBPassword
    '    localDSDef.Enabled = mainDSDef.Enabled
    '    localDSDef.EnabledSpecified = mainDSDef.EnabledSpecified
    '    localDSDef.Extension = mainDSDef.Extension
    '    localDSDef.ImpersonateUserSpecified = mainDSDef.ImpersonateUserSpecified
    '    localDSDef.Prompt = mainDSDef.Prompt
    '    localDSDef.WindowsCredentials = mainDSDef.WindowsCredentials

    '    localws.CreateDataSource(reportDS.Name, dataSourcePath, False, localDSDef, Nothing)

    'End Sub

    Private Sub btnSubmitToServer_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmitToServer.Click
        Try
            CopyTablesToServer()
            CopyFilesToServer()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

        Response.Write(common.HideDiv())
        Response.Flush()

        Session.Add("synFinsh", "true")

        Response.Write("<script language=javascript>window.navigate('synchronize.aspx');</script>")
        Response.End()

    End Sub

    Private Sub CopyFilesToServer()

        Response.Write(common.ChangeText("Copy directories and files..."))
        Response.Flush()

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand
        da.SelectCommand.Connection = con

        Dim dt As New DataTable
        Dim dr As Data.DataRow()

        Impersonate.impersonateValidUser(Global_asax.synchronizeUser, Global_asax.synchronizeDomainName, Global_asax.synchronizePassword, Master.errorMsg)

        'Copy AssessmentModels folder to server
        Dim destDirectoryPaths As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        destDirectoryPaths = My.Computer.FileSystem.GetDirectories(PATMAP.Global_asax.synchronizeFileRootPath & general.subFolder, FileIO.SearchOption.SearchTopLevelOnly)

        Dim sourceDirectoryPaths As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        sourceDirectoryPaths = My.Computer.FileSystem.GetDirectories(PATMAP.Global_asax.synchronizeFolder & general.subFolder, FileIO.SearchOption.SearchTopLevelOnly)

        da.SelectCommand.CommandText = "select oldID, newID from mapIDs where typeAssess = 'AssessmentModels'"
        da.Fill(dt)

        Dim sourceDirectoryPath As String
        For Each sourceDirectoryPath In sourceDirectoryPaths
            If Not destDirectoryPaths.Contains(sourceDirectoryPath.Replace(PATMAP.Global_asax.synchronizeFolder, PATMAP.Global_asax.synchronizeFileRootPath)) Then
                dr = dt.Select("oldID = " & sourceDirectoryPath.Substring(sourceDirectoryPath.LastIndexOf("\") + 1))
                If dr.Length > 0 Then
                    Dim newID As String = dr(0).Item("newID").ToString
                    Dim oldID As String = dr(0).Item("oldID").ToString
                    My.Computer.FileSystem.CopyDirectory(sourceDirectoryPath, sourceDirectoryPath.Replace(PATMAP.Global_asax.synchronizeFolder, PATMAP.Global_asax.synchronizeFileRootPath).Replace(oldID, newID), True)
                End If
            End If
        Next

        'Copy K12OG folder to server        
        dt.Clear()
        destDirectoryPaths = My.Computer.FileSystem.GetDirectories(PATMAP.Global_asax.synchronizeFileRootPath & editdataset.K12OGSurveySubFolder, FileIO.SearchOption.SearchTopLevelOnly)

        sourceDirectoryPaths = My.Computer.FileSystem.GetDirectories(PATMAP.Global_asax.synchronizeFolder & editdataset.K12OGSurveySubFolder, FileIO.SearchOption.SearchTopLevelOnly)

        da.SelectCommand.CommandText = "select oldID, newID from mapIDs where typeAssess = 'POV'"
        da.Fill(dt)

        For Each sourceDirectoryPath In sourceDirectoryPaths
            If Not destDirectoryPaths.Contains(sourceDirectoryPath.Replace(PATMAP.Global_asax.synchronizeFolder, PATMAP.Global_asax.synchronizeFileRootPath)) Then
                dr = dt.Select("oldID = " & sourceDirectoryPath.Substring(sourceDirectoryPath.LastIndexOf("\") + 1))
                If dr.Length > 0 Then
                    Dim newID As String = dr(0).Item("newID").ToString
                    Dim oldID As String = dr(0).Item("oldID").ToString
                    My.Computer.FileSystem.CopyDirectory(sourceDirectoryPath, sourceDirectoryPath.Replace(PATMAP.Global_asax.synchronizeFolder, PATMAP.Global_asax.synchronizeFileRootPath).Replace(oldID, newID), True)
                End If
            End If
        Next

        'Copy POV folder to server 
        dt.Clear()
        destDirectoryPaths = My.Computer.FileSystem.GetDirectories(PATMAP.Global_asax.synchronizeFileRootPath & editdataset.POVSubFolder, FileIO.SearchOption.SearchTopLevelOnly)

        sourceDirectoryPaths = My.Computer.FileSystem.GetDirectories(PATMAP.Global_asax.synchronizeFolder & editdataset.POVSubFolder, FileIO.SearchOption.SearchTopLevelOnly)

        da.SelectCommand.CommandText = "select oldID, newID from mapIDs where typeAssess = 'POV'"
        da.Fill(dt)

        For Each sourceDirectoryPath In sourceDirectoryPaths
            If Not destDirectoryPaths.Contains(sourceDirectoryPath.Replace(PATMAP.Global_asax.synchronizeFolder, PATMAP.Global_asax.synchronizeFileRootPath)) Then
                dr = dt.Select("oldID = " & sourceDirectoryPath.Substring(sourceDirectoryPath.LastIndexOf("\") + 1))
                If dr.Length > 0 Then
                    Dim newID As String = dr(0).Item("newID").ToString
                    Dim oldID As String = dr(0).Item("oldID").ToString
                    My.Computer.FileSystem.CopyDirectory(sourceDirectoryPath, sourceDirectoryPath.Replace(PATMAP.Global_asax.synchronizeFolder, PATMAP.Global_asax.synchronizeFileRootPath).Replace(oldID, newID), True)
                End If
            End If
        Next

        'Copy TaxCredit folder to server     
        dt.Clear()
        destDirectoryPaths = My.Computer.FileSystem.GetDirectories(PATMAP.Global_asax.synchronizeFileRootPath & editdataset.taxCreditSubFolder, FileIO.SearchOption.SearchTopLevelOnly)

        sourceDirectoryPaths = My.Computer.FileSystem.GetDirectories(PATMAP.Global_asax.synchronizeFolder & editdataset.taxCreditSubFolder, FileIO.SearchOption.SearchTopLevelOnly)

        da.SelectCommand.CommandText = "select oldID, newID from mapIDs where typeAssess = 'TaxCredit'"
        da.Fill(dt)

        For Each sourceDirectoryPath In sourceDirectoryPaths
            If Not destDirectoryPaths.Contains(sourceDirectoryPath.Replace(PATMAP.Global_asax.synchronizeFolder, PATMAP.Global_asax.synchronizeFileRootPath)) Then
                dr = dt.Select("oldID = " & sourceDirectoryPath.Substring(sourceDirectoryPath.LastIndexOf("\") + 1))
                If dr.Length > 0 Then
                    Dim newID As String = dr(0).Item("newID").ToString
                    Dim oldID As String = dr(0).Item("oldID").ToString
                    My.Computer.FileSystem.CopyDirectory(sourceDirectoryPath, sourceDirectoryPath.Replace(PATMAP.Global_asax.synchronizeFolder, PATMAP.Global_asax.synchronizeFileRootPath).Replace(oldID, newID), True)
                End If
            End If
        Next

        Impersonate.undoImpersonation()

        dt.Clear()
        con.Close()
    End Sub

    Private Sub UpdateLastDate()

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim query As New SqlClient.SqlCommand
        query.Connection = con

        query.CommandText = "update satelliteInfo set lastUpdate = getDate() where machineName='" & PATMAP.Global_asax.machineName & "'"
        query.ExecuteNonQuery()

        con.Close()

    End Sub

    Private Sub CopyTablesToServer()

        Response.Write(common.JavascriptFunctions())
        Response.Flush()

        Response.Write(common.DisplayDiv("Synchronize the database...", -1))
        Response.Flush()

        'transfer tables
        Dim pkg As New Package
        Dim app As New Application
        Dim pkgResults As DTSExecResult
        Dim _PackagePath As String = System.Configuration.ConfigurationManager.AppSettings("PackagePath").ToString
        Dim _PackageName As String = System.Configuration.ConfigurationManager.AppSettings("PackageName_CopyTablesToServer").ToString
        Dim PackageName As String = (Server.MapPath(Replace(_PackagePath, "..", "")) + _PackageName)

        pkg = app.LoadPackage((PackageName + ".dtsx"), Nothing)
        Dim vars As Variables = pkg.Variables
        vars("localServer").Value = PATMAP.Global_asax.machineName
        vars("prodServer").Value = PATMAP.Global_asax.SQLEngineServer
        pkgResults = pkg.Execute()

        If pkgResults = DTSExecResult.Failure Then
            Master.errorMsg = pkg.Errors(0).Description
        End If

        'copy saved scenarios from laptop to server
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        query.CommandTimeout = 60000
        Dim dr As SqlClient.SqlDataReader

        Dim con2 As New SqlClient.SqlConnection
        con2.ConnectionString = PATMAP.Global_asax.connString
        con2.Open()
        Dim query2 As New SqlClient.SqlCommand
        query2.Connection = con2
        query2.CommandTimeout = 60000


        query.CommandText = "SELECT OldID, NewID from New_ID WHERE Description = 'AssessmentTaxModel'"
        dr = query.ExecuteReader


        If dr.HasRows Then
            While dr.Read()
                _PackageName = System.Configuration.ConfigurationManager.AppSettings("PackageName_CopySavedModelsTablesToLaptop").ToString
                PackageName = (Server.MapPath(Replace(_PackagePath, "..", "")) + _PackageName)
                pkg = app.LoadPackage((PackageName + ".dtsx"), Nothing)
                vars = pkg.Variables
                query2.CommandText = "exec creatCompareBaseandSubject " & "liveAssessmentTaxModelResultsModel_" & dr.GetValue(1) & "," & "liveAssessmentTaxModelResultsSummaryModel_" & dr.GetValue(1) & ",1"
                query2.ExecuteNonQuery()
                vars("localServer").Value = PATMAP.Global_asax.SQLEngineServer
                vars("prodServer").Value = PATMAP.Global_asax.machineName
                vars("tableNameR").Value = "liveAssessmentTaxModelResultsModel_" & dr.GetValue(0)
                vars("tableNameR2").Value = "liveAssessmentTaxModelResultsModel_" & dr.GetValue(1)
                vars("tableNameS").Value = "liveAssessmentTaxModelResultsSummaryModel_" & dr.GetValue(0)
                vars("tableNameS2").Value = "liveAssessmentTaxModelResultsSummaryModel_" & dr.GetValue(1)
                pkgResults = pkg.Execute()
                If pkgResults = DTSExecResult.Failure Then
                    Master.errorMsg = pkg.Errors(0).Description
                End If
                If pkgResults.ToString.ToLower.Equals("success") Then
                Else
                    Response.Write(common.ChangeText(".. " & pkg.Errors(0).Description & ".."))
                    Response.Flush()
                End If
            End While
        End If

    End Sub
End Class