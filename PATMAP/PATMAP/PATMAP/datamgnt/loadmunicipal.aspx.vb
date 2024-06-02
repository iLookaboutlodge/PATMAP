Imports System.IO
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.OleDb

Partial Public Class loadmunicipal
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Clears out the error message
			Master.errorMsg = ""

			If Not IsPostBack Then
				'Sets submenu to be displayed
				subMenu.setStartNode(menu.loadData)
			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number)
		End Try
		'lb _ Message.Text = ""
		Master.errorMsg = ""
	End Sub


	Dim SQL_DataSource As String = PATMAP.Global_asax.SQLEngineServer
	'System.Configuration.ConfigurationSettings.AppSettings("SQL_DataSource").ToString
	Dim SQL_InitialCatalog As String = PATMAP.Global_asax.DBName
	'System.Configuration.ConfigurationSettings.AppSettings("SQL_InitialCatalog").ToString
	Dim _LocalFileRootPath As String = ConfigurationSettings.AppSettings("LocalFileRootPath").ToString
	Dim _PackagePath As String = ConfigurationSettings.AppSettings("PackagePath").ToString
	Dim _PackageName As String = ConfigurationSettings.AppSettings("PackageName_MunicipalToSQL").ToString
	Dim _PackageNameMap As String = ConfigurationSettings.AppSettings("PackageName_MappingToSQL").ToString
	Dim _LocalFileSubFolderPath As String = "/Municipal/"
	Dim _Source_File_Extantion As String = "xls"
	Dim OleDbConnectionExtended As String = "" '" ;Extended Properties=Excel 8.0;"
	Dim _TableJurisdGroups As String = "'Jurisdiction Grouping$'"
	Dim _TableJurisdTypes As String = "'Jurisdiction Look-Up$'"
	Dim _TableEntities As String = "'Mun by Type - Code$'"
	Dim _TableMunicRollup As String = "'SAMA Roll-UP$'"
	Dim _TableMunicipalitiesMapLink As String = "'MATCh Roll-UP$'"




	Protected Property Unic() As String
		Get
			If (Not (ViewState("Unic")) Is Nothing) Then
				Return ViewState("Unic").ToString
			Else
				Return Nothing
			End If
		End Get
		Set(ByVal value As String)
			ViewState("Unic") = value.ToString
		End Set
	End Property

	Protected Function FileUpload() As Boolean
		Dim file_Correct As Boolean = True
		Dim file_ShortName As String = ""
		Dim file_PathName As String = ""
		Dim file_Extension As String = ""
		Dim file_ContentType As String = ""
		Dim file_Size As Decimal = 0
		Try
			file_ShortName = fpFile.FileName
			file_PathName = fpFile.PostedFile.FileName
			file_Extension = fpFile.FileName.Substring((fpFile.FileName.LastIndexOf(".") + 1))
			file_ContentType = fpFile.PostedFile.ContentType
			file_Size = Convert.ToDecimal(fpFile.PostedFile.ContentLength)
		Catch ex As Exception
			file_Correct = False
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP301") + ex.ToString + "'"
		End Try
		If Not file_Extension.ToLower.Equals(_Source_File_Extantion) Then
			file_Correct = False
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP302") + _Source_File_Extantion + "'"
		End If
		If file_Correct Then
			Unic = DateTime.Today.Year.ToString _
			+ "_" + DateTime.Today.Month.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
			+ "_" + DateTime.Today.Day.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
			+ "_" + Guid.NewGuid.ToString()
			Dim RepositoryUnicFileName As String = (_LocalFileRootPath _
				+ (_LocalFileSubFolderPath _
				+ (Unic + ("." + _Source_File_Extantion))))
			fpFile.SaveAs(RepositoryUnicFileName)
			file_Correct = FileExist(RepositoryUnicFileName)
		End If
		Return file_Correct
	End Function
	Protected Function FileUploadMap() As Boolean
		Dim file_Correct As Boolean = True
		Dim file_ShortName As String = ""
		Dim file_PathName As String = ""
		Dim file_Extension As String = ""
		Dim file_ContentType As String = ""
		Dim file_Size As Decimal = 0
		Try
			file_ShortName = fpFileMap.FileName
			file_PathName = fpFileMap.PostedFile.FileName
			file_Extension = fpFileMap.FileName.Substring((fpFileMap.FileName.LastIndexOf(".") + 1))
			file_ContentType = fpFileMap.PostedFile.ContentType
			file_Size = Convert.ToDecimal(fpFileMap.PostedFile.ContentLength)
		Catch ex As Exception
			file_Correct = False
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP301") + ex.ToString + "'"
		End Try
		If Not file_Extension.ToLower.Equals(_Source_File_Extantion) Then
			file_Correct = False
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP302") + _Source_File_Extantion + "'"
		End If
		If file_Correct Then
			Unic = DateTime.Today.Year.ToString _
			+ "_" + DateTime.Today.Month.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
			+ "_" + DateTime.Today.Day.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
			+ "_" + Guid.NewGuid.ToString()
			Dim RepositoryUnicFileName As String = (_LocalFileRootPath _
				+ (_LocalFileSubFolderPath _
				+ (Unic + ("." + _Source_File_Extantion))))
			fpFileMap.SaveAs(RepositoryUnicFileName)
			file_Correct = FileExist(RepositoryUnicFileName)
		End If
		Return file_Correct
	End Function

	Protected Function FileExist(ByVal fileName As String) As Boolean
		Dim status As Boolean = False
		Dim dt0 As DateTime = DateTime.Now
		Dim dt1 As DateTime
		Dim dt2 As DateTime
		Dim dt3 As DateTime = dt0.AddSeconds(90)
		'TimeOUT time
FileNotClosed:
		dt0 = DateTime.Now
		dt1 = DateTime.Now
		dt2 = dt1.AddSeconds(10)
		If Not File.Exists(fileName) Then
			If (dt3 > dt0) Then

				While (dt2 > dt1)
					dt1 = DateTime.Now

				End While
				GoTo FileNotClosed

			Else
				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP307")
			End If
		Else
			status = True
		End If
		Return status
	End Function


	Protected Sub btnFileUpload_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
		If FileUpload() Then
			ddlTableNames_BindData()
			If ((ddlTableJurisdGroups.Visible = False) _
				AndAlso ((ddlTableJurisdTypes.Visible = False) _
				AndAlso ((ddlTableEntities.Visible = False) _
				AndAlso (ddlTableMunicRollup.Visible = False)))) Then
				Load_Click()
				tr_btnLoad.Visible = False
			End If
		End If
	End Sub

	Protected Sub btnFileUploadMap_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnFileUploadMAP.Click
		If FileUploadMap() Then
			ddlTableNames_BindDataMAP()
			If ((ddlTableMunicipalitiesMapLink.Visible = False)) Then
				LoadMAP_Click()
				tr_btnLoadMap.Visible = False
			End If
		End If
	End Sub


	Protected Function ddlTableNames_BindData() As Boolean
		Dim status As Boolean = True
		'string Source_FilePath = Server.MapPath(RepositoryPath);
		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
		Dim cnAccessConn As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection
		Dim dtTable As DataTable = New DataTable
		Try

			'cnAccessConn = New System.Data.OleDb.OleDbConnection(("Provider=Microsoft.Jet.OLEDB.4.0 ;Data Source=" _
			'								+ (Source_FilePath _
			'								+ (Source_FileName + OleDbConnectionExtended))))

			cnAccessConn = New System.Data.OleDb.OleDbConnection(("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" _
			 + (Source_FilePath _
			 + (Source_FileName + OleDbConnectionExtended)) _
			 + ";Extended Properties=""Excel 8.0;HDR=Yes;"""))

			cnAccessConn.Open()
			dtTable = cnAccessConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
			For Each row As DataRow In dtTable.Select
				If (Not row("Table_Name").ToString.ToLower.Contains("$") _
				 OrElse row("Table_Name").ToString.ToLower.Contains("print_area")) Then
					row.Delete()
					'row.BeginEdit();
				End If
			Next
			dtTable.AcceptChanges()
		Catch ex As Exception
			Master.errorMsg = ex.ToString
			status = False
		Finally
			cnAccessConn.Close()
		End Try
		If status Then
			ddlTableNames_Visible(True)
			For Each row As DataRow In dtTable.Select
				If row("Table_Name").ToString.Equals(_TableJurisdGroups) Then
					ddlTableJurisdGroups.Items.Insert(0, row("Table_Name").ToString)
					ddlTableJurisdGroups.SelectedIndex = 0
					tr_ddlTableJurisdGroups.Visible = False
					row.Delete()

					'TODO: Warning!!! continue If
					'row.BeginEdit()
					dtTable.AcceptChanges()
				End If
			Next
			For Each row As DataRow In dtTable.Select

				If row("Table_Name").ToString.Equals(_TableJurisdTypes) Then
					ddlTableJurisdTypes.Items.Insert(0, row("Table_Name").ToString)
					ddlTableJurisdTypes.SelectedIndex = 0
					tr_ddlTableJurisdTypes.Visible = False
					row.Delete()
					'TODO: Warning!!! continue If
					'row.BeginEdit()
					dtTable.AcceptChanges()

				End If
			Next
			For Each row As DataRow In dtTable.Select

				If row("Table_Name").ToString.Equals(_TableEntities) Then
					ddlTableEntities.Items.Insert(0, row("Table_Name").ToString)
					ddlTableEntities.SelectedIndex = 0
					tr_ddlTableEntities.Visible = False
					row.Delete()
					'TODO: Warning!!! continue If
					'row.BeginEdit()
					dtTable.AcceptChanges()

				End If
			Next
			For Each row As DataRow In dtTable.Select

				If row("Table_Name").ToString.Equals(_TableMunicRollup) Then
					ddlTableMunicRollup.Items.Insert(0, row("Table_Name").ToString)
					ddlTableMunicRollup.SelectedIndex = 0
					tr_ddlTableMunicRollup.Visible = False
					row.Delete()
					'TODO: Warning!!! continue If
					'row.BeginEdit()
					dtTable.AcceptChanges()
				End If
			Next
			dtTable.AcceptChanges()
			ddlTableNames_collection(dtTable, tr_ddlTableJurisdGroups, ddlTableJurisdGroups)
			ddlTableNames_collection(dtTable, tr_ddlTableJurisdTypes, ddlTableJurisdTypes)
			ddlTableNames_collection(dtTable, tr_ddlTableEntities, ddlTableEntities)
			ddlTableNames_collection(dtTable, tr_ddlTableMunicRollup, ddlTableMunicRollup)
		End If
		btnLoad_Visible()
		Return status
	End Function
	Protected Function ddlTableNames_BindDataMAP() As Boolean
		Dim status As Boolean = True
		'string Source_FilePath = Server.MapPath(RepositoryPath);
		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
		Dim cnAccessConn As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection
		Dim dtTable As DataTable = New DataTable
		Try

			'cnAccessConn = New System.Data.OleDb.OleDbConnection(("Provider=Microsoft.Jet.OLEDB.4.0 ;Data Source=" _
			'								+ (Source_FilePath _
			'								+ (Source_FileName + OleDbConnectionExtended))))

			cnAccessConn = New System.Data.OleDb.OleDbConnection(("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" _
			+ (Source_FilePath _
			+ (Source_FileName + OleDbConnectionExtended)) _
			+ ";Extended Properties=""Excel 8.0;HDR=Yes;"""))

			cnAccessConn.Open()
			dtTable = cnAccessConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
			For Each row As DataRow In dtTable.Select
				If (Not row("Table_Name").ToString.ToLower.Contains("$") _
				 OrElse row("Table_Name").ToString.ToLower.Contains("print_area")) Then
					row.Delete()
					'row.BeginEdit();
				End If
			Next
			dtTable.AcceptChanges()
		Catch ex As Exception
			Master.errorMsg = ex.ToString
			status = False
		Finally
			cnAccessConn.Close()
		End Try
		If status Then
			ddlTableNamesMap_Visible(True)
			For Each row As DataRow In dtTable.Select

				If row("Table_Name").ToString.Equals(_TableMunicipalitiesMapLink) Then
					ddlTableMunicipalitiesMapLink.Items.Insert(0, row("Table_Name").ToString)
					ddlTableMunicipalitiesMapLink.SelectedIndex = 0
					tr_ddlTableMunicipalitiesMapLink.Visible = False
					row.Delete()
					'TODO: Warning!!! continue If
					'row.BeginEdit()
					dtTable.AcceptChanges()

				End If
			Next

			dtTable.AcceptChanges()
			ddlTableNames_collection(dtTable, tr_ddlTableMunicipalitiesMapLink, ddlTableMunicipalitiesMapLink)
		End If
		btnLoad_Visible()
		Return status
	End Function

	Protected Sub ddlTableNames_collection(ByVal dtTable As DataTable, ByVal tr As HtmlTableRow, ByVal ddl As DropDownList)
		If tr.Visible Then
			ddl.DataSource = dtTable
			ddl.DataTextField = dtTable.Columns("Table_Name").ToString
			ddl.DataBind()
			'ddl.Items.Insert(0, new ListItem("Select Column Name", "-1"));
			ddl.Items.Insert(0, "Select Table Name")
			ddl.SelectedIndex = 0
		End If
	End Sub

	Protected Sub ddlTableNames_Visible(ByVal visible As Boolean)
		tr_ddlTableJurisdGroups.Visible = visible
		tr_ddlTableJurisdTypes.Visible = visible
		tr_ddlTableEntities.Visible = visible
		tr_ddlTableMunicRollup.Visible = visible
	End Sub
	Protected Sub ddlTableNamesMap_Visible(ByVal visible As Boolean)
		tr_ddlTableMunicipalitiesMapLink.Visible = visible
	End Sub

	Protected Sub ddlTableNames_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		btnLoad_Visible()
	End Sub
	Protected Sub ddlTableNamesMAP_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		btnLoadMAP_Visible()
	End Sub

	Protected Sub btnLoad_Visible()
		If (Not ddlTableJurisdGroups.SelectedValue.Equals("Select Table Name") _
			AndAlso (Not ddlTableJurisdTypes.SelectedValue.Equals("Select Table Name") _
			AndAlso (Not ddlTableEntities.SelectedValue.Equals("Select Table Name") _
			AndAlso Not ddlTableMunicRollup.SelectedValue.Equals("Select Table Name") _
			AndAlso Not ddlTableMunicipalitiesMapLink.SelectedValue.Equals("Select Table Name")))) Then
			tr_btnLoad.Visible = True
		Else
			tr_btnLoad.Visible = False
		End If
	End Sub
	Protected Sub btnLoadMAP_Visible()
		If (Not ddlTableMunicipalitiesMapLink.SelectedValue.Equals("Select Table Name")) Then
			tr_btnLoadMap.Visible = True
		Else
			tr_btnLoadMap.Visible = False
		End If
	End Sub

	Protected Sub txtNewDSN_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		btnLoad_Visible()
	End Sub

	Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnLoad.Click
		Load_Click()

	End Sub
	Protected Sub btnLoadMap_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnLoadMap.Click
		LoadMAP_Click()
	End Sub

	Protected Sub Load_Click()
		Dim Source_Connection_Script_Entities As String = ""
		Dim Source_Connection_Script_JurisdGroups As String = ""
		Dim Source_Connection_Script_JurisdTypes As String = ""
		Dim Source_Connection_Script_MunicRollup As String = ""
		Dim Source_Connection_Script_PPID As String = ""

		Source_Connection_Script_Entities = Make_Source_Script_Entities()
		Source_Connection_Script_JurisdGroups = Make_Source_Script_JurisdGroups()
		Source_Connection_Script_JurisdTypes = Make_Source_Script_JurisdTypes()
		Source_Connection_Script_MunicRollup = Make_Source_Script_MunicRollup()

		'If SSIS_Source_Connection_Script_Creation(Source_Connection_Script_Entities, Source_Connection_Script_JurisdGroups, Source_Connection_Script_JurisdTypes, Source_Connection_Script_MunicRollup, Source_Connection_Script_PPID) Then
		If RemoveFromTmpSSISTable_Entities() And RemoveFromTmpSSISTable_JurisdGroups() And RemoveFromTmpSSISTable_JurisdTypes() And RemoveFromTmpSSISTable_MunicRollup() Then
			If GetAndInsertExcelToSQLTable_Entities(Trim(ddlTableEntities.SelectedValue)) And _
			GetAndInsertExcelToSQLTable_JurisdGroups(Trim(ddlTableJurisdGroups.SelectedValue)) And _
			GetAndInsertExcelToSQLTable_JurisdTypes(Trim(ddlTableJurisdTypes.SelectedValue)) And _
			GetAndInsertExcelToSQLTable_MunicRollup(Trim(ddlTableMunicRollup.SelectedValue)) Then

				If SSIS_Package_Bind("file", Source_Connection_Script_Entities, Source_Connection_Script_JurisdGroups, Source_Connection_Script_JurisdTypes, Source_Connection_Script_MunicRollup) Then
					'Copy from _SSIS table to real table
					If SQL_Server_CopyData() Then

					End If
					'End If
				End If
			End If
		End If
	End Sub
	Protected Sub LoadMAP_Click()
		Dim Source_Connection_Script_Entities As String = ""
		Dim Source_Connection_Script_JurisdGroups As String = ""
		Dim Source_Connection_Script_JurisdTypes As String = ""
		Dim Source_Connection_Script_MunicRollup As String = ""
		Dim Source_Connection_Script_PPID As String = ""

		Source_Connection_Script_PPID = Make_Source_Script_PPID()

		'If SSIS_Source_Connection_Script_Creation(Source_Connection_Script_Entities, Source_Connection_Script_JurisdGroups, Source_Connection_Script_JurisdTypes, Source_Connection_Script_MunicRollup, Source_Connection_Script_PPID) Then
		If RemoveFromTmpSSISTable_PPID() Then
			If GetAndInsertExcelToSQLTable_PPID(Trim(ddlTableMunicipalitiesMapLink.SelectedValue)) Then
				If SSIS_Package_BindMAP("file", Source_Connection_Script_PPID) Then
					'Copy from _SSIS table to real table
					If SQL_Server_CopyDataMAP() Then

					End If
				End If
			End If
		End If
		'End If
	End Sub



	'Protected Function OLD_SSIS_Source_Connection_Script_Creation(ByRef Source_Connection_Script_Entities As String, ByRef Source_Connection_Script_JurisdGroups As String, ByRef Source_Connection_Script_JurisdTypes As String, ByRef Source_Connection_Script_MunicRollup As String, ByRef Source_Connection_Script_PPID As String) As Boolean
	'	Dim status As Boolean = True
	'	Source_Connection_Script_Entities = "SELECT '" + Unic + "' as SessionID, [Code] AS [number], [Municipality Name] AS jurisdiction, [Jurisdiction Type Code] AS jurisdictionTypeID FROM " _
	'			+ "[" + ddlTableEntities.SelectedValue + "]"
	'	'+ "['Mun by Type - Code$']";
	'	Source_Connection_Script_JurisdGroups = "SELECT '" + Unic + "' as SessionID, [Jurisdiction Grouping Code] as jurisdictionGroupID, [Jurisdiction Grouping] as jurisdictionGroup from " _
	'	+ "[" + ddlTableJurisdGroups.SelectedValue + "]"
	'	'+ ['Jurisdiction Grouping$']";
	'	Source_Connection_Script_JurisdTypes = "SELECT '" + Unic + "' as SessionID, [Jurisdiction Type Code] AS jurisdictionTypeID, [Jurisdiction Type] AS jurisdictionType, [Jurisdiction Grouping Code] AS jurisdictionGroupID FROM " _
	'			+ "[" + ddlTableJurisdTypes.SelectedValue + "]"
	'	'['Jurisdiction Look-Up$']";
	'	Source_Connection_Script_MunicRollup = "SELECT '" + Unic + "' as SessionID, [Survey] as newMunicipalityID,  [SAMA Info] as oldMunicipalityID from " _
	'			+ "[" + ddlTableMunicRollup.SelectedValue + "]"
	'	'['SAMA Roll-UP$']";
	'	Source_Connection_Script_PPID = "SELECT '" + Unic + "' as SessionID, [PPID] AS [PPID], [UMNM/RMNO] AS UMNM, [PATMAP Code] AS PATMAP_Code, [PATMAP Roll Up Name] AS PATMAP_Roll_Up_Name, [Sama Code] AS Sama_Code from " _
	'			+ "[" + ddlTableMunicipalitiesMapLink.SelectedValue + "]"
	'	'['Match Roll-Up$']";
	'	Return status
	'End Function

	Protected Function SSIS_Source_Connection_Script_Creation(ByRef Source_Connection_Script_Entities As String, ByRef Source_Connection_Script_JurisdGroups As String, ByRef Source_Connection_Script_JurisdTypes As String, ByRef Source_Connection_Script_MunicRollup As String, ByRef Source_Connection_Script_PPID As String) As Boolean
		Dim status As Boolean = True
		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
		Dim DataString As String = " OPENROWSET('Microsoft.ACE.OLEDB.12.0','Excel 8.0;HDR=Yes;Database=" + Source_FilePath + Source_FileName + "', "

		Source_Connection_Script_Entities = "SELECT '" + Unic + "' as SessionID, [Code] AS [number], [Municipality Name] AS jurisdiction, [Jurisdiction Type Code] AS jurisdictionTypeID FROM " _
		+ DataString + "[" + ddlTableEntities.SelectedValue + "]" + ")"
		'+ "['Mun by Type - Code$']";
		Source_Connection_Script_JurisdGroups = "SELECT '" + Unic + "' as SessionID, [Jurisdiction Grouping Code] as jurisdictionGroupID, [Jurisdiction Grouping] as jurisdictionGroup from " _
		+ DataString + "[" + ddlTableJurisdGroups.SelectedValue + "]" + ")"
		'+ ['Jurisdiction Grouping$']";
		Source_Connection_Script_JurisdTypes = "SELECT '" + Unic + "' as SessionID, [Jurisdiction Type Code] AS jurisdictionTypeID, [Jurisdiction Type] AS jurisdictionType, [Jurisdiction Grouping Code] AS jurisdictionGroupID FROM " _
		+ DataString + "[" + ddlTableJurisdTypes.SelectedValue + "]" + ")"
		'['Jurisdiction Look-Up$']";
		Source_Connection_Script_MunicRollup = "SELECT '" + Unic + "' as SessionID, [Survey] as newMunicipalityID,  [SAMA Info] as oldMunicipalityID from " _
		+ DataString + "[" + ddlTableMunicRollup.SelectedValue + "]" + ")"
		'['SAMA Roll-UP$']";
		Source_Connection_Script_PPID = "SELECT '" + Unic + "' as SessionID, [PPID] AS [PPID], [UMNM/RMNO] AS UMNM, [PATMAP Code] AS PATMAP_Code, [PATMAP Roll Up Name] AS PATMAP_Roll_Up_Name, [Sama Code] AS Sama_Code from " _
		+ DataString + "[" + ddlTableMunicipalitiesMapLink.SelectedValue + "]" + ")"
		'['Match Roll-Up$']";
		Return status
	End Function

	Protected Function SSIS_Package_Bind(ByVal TypeOfConnection As String, ByVal Source_Connection_Script_Entities As String, ByVal Source_Connection_Script_JurisdGroups As String, ByVal Source_Connection_Script_JurisdTypes As String, ByVal Source_Connection_Script_MunicRollup As String) As Boolean
		Dim status As Boolean = False
		Master.errorMsg = ""

		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
		Dim PackageName As String = (Server.MapPath(_PackagePath) + _PackageName)
		Select Case (TypeOfConnection)
			Case "file"
				'If (File.Exists((PackageName + ".dtsx")) AndAlso File.Exists((PackageName + ".dtsConfig"))) Then
				If (File.Exists((PackageName + ".dtsx"))) Then
					Dim myPackage As Package
					Dim app As Application = New Application
					myPackage = app.LoadPackage((PackageName + ".dtsx"), Nothing)
					'myPackage.ImportConfigurationFile((PackageName + ".dtsConfig"))
					Dim vars As Variables = myPackage.Variables
					vars("SQL_DataSource").Value = SQL_DataSource
					vars("SQL_InitialCatalog").Value = SQL_InitialCatalog
					vars("Source_FilePath").Value = Source_FilePath
					vars("Source_FileName").Value = Source_FileName
					vars("Source_Connection_Script_entities").Value = Source_Connection_Script_Entities
					vars("Source_Connection_Script_jurGroups").Value = Source_Connection_Script_JurisdGroups
					vars("Source_Connection_Script_jurTypes").Value = Source_Connection_Script_JurisdTypes
					vars("Source_Connection_Script_munRollup").Value = Source_Connection_Script_MunicRollup
					vars("SQL_UserName").Value = Trim(PATMAP.Global_asax.DBUser)
					vars("SQL_Password").Value = Trim(PATMAP.Global_asax.DBPassword)

					Dim result As DTSExecResult = myPackage.Execute
					Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP312") + result.ToString
					If result.ToString.ToLower.Equals("success") Then
						status = True
					Else
						status = False
						If myPackage.Errors.Count > 0 Then
							Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP312") + myPackage.Errors(0).Description
						End If
					End If
				Else
					Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP308") + PackageName + PATMAP.common.GetErrorMessage("PATMAP309")
				End If
			Case "WebService"
			Case "SQL"
			Case Else
				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP310")
		End Select
		'If Not status Then
		'    lb _ Message.Text = (lb _ Message.Text + ("<br>Source_FilePath: " _
		'                + (Source_FilePath + ("<br>Source_FileName: " + Source_FileName))))
		'End If
		Return status
	End Function
	Protected Function SSIS_Package_BindMAP(ByVal TypeOfConnection As String, ByVal Source_Connection_Script_PPID As String) As Boolean
		Dim status As Boolean = False
		Master.errorMsg = ""

		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
		Dim PackageName As String = (Server.MapPath(_PackagePath) + _PackageNameMap)
		Select Case (TypeOfConnection)
			Case "file"
				'If (File.Exists((PackageName + ".dtsx")) AndAlso File.Exists((PackageName + ".dtsConfig"))) Then
				If (File.Exists((PackageName + ".dtsx"))) Then
					Dim myPackage As Package
					Dim app As Application = New Application
					myPackage = app.LoadPackage((PackageName + ".dtsx"), Nothing)
					'myPackage.ImportConfigurationFile((PackageName + ".dtsConfig"))
					Dim vars As Variables = myPackage.Variables
					vars("SQL_DataSource").Value = SQL_DataSource
					vars("SQL_InitialCatalog").Value = SQL_InitialCatalog
					vars("Source_FilePath").Value = Source_FilePath
					vars("Source_FileName").Value = Source_FileName
					vars("Source_Connection_Script_PPID").Value = Source_Connection_Script_PPID
					vars("SQL_UserName").Value = Trim(PATMAP.Global_asax.DBUser)
					vars("SQL_Password").Value = Trim(PATMAP.Global_asax.DBPassword)

					Dim result As DTSExecResult = myPackage.Execute
					Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP312") + result.ToString
					If result.ToString.ToLower.Equals("success") Then
						status = True
					Else
						status = False
						If myPackage.Errors.Count > 0 Then
							Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP312") + myPackage.Errors(0).Description
						End If
					End If
				Else
					Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP308") + PackageName + PATMAP.common.GetErrorMessage("PATMAP309")
				End If
			Case "WebService"
			Case "SQL"
			Case Else
				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP310")
		End Select
		'If Not status Then
		'    lb _ Message.Text = (lb _ Message.Text + ("<br>Source_FilePath: " _
		'                + (Source_FilePath + ("<br>Source_FileName: " + Source_FileName))))
		'End If
		Return status
	End Function

	Protected Function SQL_Server_CopyData() As Boolean
		'1
		Dim CommandText As String = " SELECT count (*) as count FROM [jurisdictionGroups_SSIS] where [SessionID]='" + Unic + "'"
		Dim dtJurisdGroups As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim countJurisdGroups As Integer = Integer.Parse(dtJurisdGroups.Rows(0)("count").ToString)
		If (countJurisdGroups > 0) Then
			CommandText = " DELETE FROM [jurisdictionGroups]"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " INSERT INTO [jurisdictionGroups]([JurisdictionGroupID],[JurisdictionGroup])" _
			+ " SELECT [jurisdictionGroupID],[jurisdictionGroup] FROM [jurisdictionGroups_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [jurisdictionGroups_SSIS] where [SessionID]='" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countJurisdGroups.ToString + "' rows to 'jurisdictionGroups' table")))
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + countJurisdGroups.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'jurisdictionGroups'"
		Else
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP314") + "'jurisdictionGroups'"
		End If
		'2 
		CommandText = " SELECT count (*) as count FROM [jurisdictionTypes_SSIS] where [SessionID]='" + Unic + "'"
		Dim dtJurisdTypes As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim countJurisdTypes As Integer = Integer.Parse(dtJurisdTypes.Rows(0)("count").ToString)
		If (countJurisdTypes > 0) Then
			CommandText = " DELETE FROM [jurisdictionTypes]"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " INSERT INTO [jurisdictionTypes]([jurisdictionTypeID],[jurisdictionType],[jurisdictionGroupID]) " _
			+ " SELECT [jurisdictionTypeID],[jurisdictionType],[jurisdictionGroupID] FROM [jurisdictionTypes_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [jurisdictionTypes_SSIS] where [SessionID]='" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countJurisdTypes.ToString + "' rows to 'jurisdictionTypes' table")))
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + countJurisdTypes.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'jurisdictionTypes'"
		Else
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP314") + "'jurisdictionTypes'"
		End If
		'3 
		CommandText = " SELECT count (*) as count FROM [entities_SSIS] where [SessionID]='" + Unic + "'"
		Dim dtEntities As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim countEntities As Integer = Integer.Parse(dtEntities.Rows(0)("count").ToString)
		If (countEntities > 0) Then
			CommandText = " DELETE FROM [entities] where [jurisdictionTypeID] <> '1'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " INSERT INTO [entities]([number],[jurisdiction],[jurisdictionTypeID]) " _
			+ " SELECT [number],[jurisdiction],[jurisdictionTypeID] FROM [entities_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [entities_SSIS] where [SessionID]='" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countEntities.ToString + "' rows to 'entities' table")))
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + countEntities.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'entities'"
		Else
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP314") + "'entities'"
		End If
		'4
		CommandText = " SELECT count (*) as count FROM [municipalityRollup_SSIS] where [SessionID]='" + Unic + "'"
		Dim dtMunicipalityRollup As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim countMunicipalityRollup As Integer = Integer.Parse(dtMunicipalityRollup.Rows(0)("count").ToString)
		If (countMunicipalityRollup > 0) Then
			CommandText = " DELETE FROM [municipalityRollup]"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " INSERT INTO [PATMAP].[dbo].[municipalityRollup]([oldMunicipalityID] ,[newMunicipalityID])" _
			 + " SELECT [oldMunicipalityID],[newMunicipalityID] FROM [municipalityRollup_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [municipalityRollup_SSIS] where [SessionID]='" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)

			'setup database connection
			Dim con As New SqlClient.SqlConnection
			con.ConnectionString = PATMAP.Global_asax.connString
			con.Open()

			Dim query As New SqlClient.SqlCommand
			query.Connection = con

			query.CommandTimeout = 6000

			query.CommandText = "update assessmentDescription set dataStale = 1" & vbCrLf
			query.CommandText &= "update assessment set municipalityID = mr.newMunicipalityID  from municipalityRollup mr inner join assessment a on mr.oldMunicipalityID = a.municipalityID_orig"

			query.ExecuteNonQuery()
			con.Close()

			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countMunicipalityRollup.ToString + "' rows to 'municipalityRollup' table")))
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + countMunicipalityRollup.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'municipalityRollup'"
		Else
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP314") + "'municipalityRollup'"
		End If
		Return ((countJurisdGroups > 0) _
			AndAlso ((countJurisdTypes > 0) _
			AndAlso ((countEntities > 0) _
			AndAlso (countMunicipalityRollup > 0))))
	End Function

	Protected Function SQL_Server_CopyDataMAP() As Boolean
		Dim CommandText As String = " SELECT count (*) as count FROM [MunicipalitiesMapLink_SSIS] where [SessionID]='" + Unic + "'"
		Dim dtMunicipalitiesMapLink As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim countMunMapLink As Integer = Integer.Parse(dtMunicipalitiesMapLink.Rows(0)("count").ToString)
		If (countMunMapLink > 0) Then
			CommandText = " DELETE FROM [MunicipalitiesMapLink]"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " INSERT INTO [MunicipalitiesMapLink]([PPID],[UMNM],[PATMAP_Code],[PATMAP_Roll_Up_Name],[Sama_Code])" _
			+ " SELECT [PPID],[UMNM],[PATMAP_Code],[PATMAP_Roll_Up_Name],[Sama_Code] FROM [MunicipalitiesMapLink_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [MunicipalitiesMapLink_SSIS] where [SessionID]='" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countJurisdGroups.ToString + "' rows to 'jurisdictionGroups' table")))
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP315") + countMunMapLink.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'MunicipalitiesMapLink'"
		Else
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP314") + "'MunicipalitiesMapLink'"
		End If
		Return ((countMunMapLink > 0))
	End Function

	Public Function GetAndInsertExcelToSQLTable_Entities(ByVal TableName As String) As Boolean
		Dim OleConn As New OleDbConnection
		Dim OleCmd As New OleDbCommand
		Dim OleDr As OleDbDataReader = Nothing

		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))

		If Not IO.File.Exists(Source_FilePath + Source_FileName) Then
			Master.errorMsg = "File Not Found."
			Return False
		End If

		Dim constr As String = ""
		constr += "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
		constr += Source_FilePath
		constr += Source_FileName + OleDbConnectionExtended
		'constr += ";Extended Properties=Excel 8.0;HDR=Yes;"
		constr += ";Extended Properties=""Excel 8.0;HDR=Yes;"""

		OleConn = New System.Data.OleDb.OleDbConnection(constr)

		OleConn.Open()

		Try

			With OleCmd
				.Connection = OleConn
				.CommandType = CommandType.Text
				.CommandText = OLEDB_TableScript_Entities(TableName)
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[entities_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("number", "number")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("jurisdiction", "jurisdiction")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("jurisdictionTypeID", "jurisdictionTypeID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)

				BulkCopyToSql.WriteToServer(OleDr)

				status = True
			End Using

			Return status
		Catch ex As Exception
			'Throw New Exception(ex.Message)
			Master.errorMsg = ex.Message
		Finally
			If OleConn.State = ConnectionState.Open Then
				OleConn.Close()
			End If
			If OleDr IsNot Nothing AndAlso (Not OleDr.IsClosed) Then
				OleDr.Close()
			End If
		End Try
		Return False
	End Function

	Public Function GetAndInsertExcelToSQLTable_JurisdGroups(ByVal TableName As String) As Boolean
		Dim OleConn As New OleDbConnection
		Dim OleCmd As New OleDbCommand
		Dim OleDr As OleDbDataReader = Nothing

		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))

		If Not IO.File.Exists(Source_FilePath + Source_FileName) Then
			Master.errorMsg = "File Not Found."
			Return False
		End If

		Dim constr As String = ""
		constr += "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
		constr += Source_FilePath
		constr += Source_FileName + OleDbConnectionExtended
		'constr += ";Extended Properties=Excel 8.0;HDR=Yes;"
		constr += ";Extended Properties=""Excel 8.0;HDR=Yes;"""

		OleConn = New System.Data.OleDb.OleDbConnection(constr)

		OleConn.Open()

		Try

			With OleCmd
				.Connection = OleConn
				.CommandType = CommandType.Text
				.CommandText = OLEDB_TableScript_JurisdGroups(TableName)
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[jurisdictionGroups_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("jurisdictionGroupID", "jurisdictionGroupID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("jurisdictionGroup", "jurisdictionGroup")
				BulkCopyToSql.ColumnMappings.Add(MapCol)

				BulkCopyToSql.WriteToServer(OleDr)

				status = True
			End Using

			Return status
		Catch ex As Exception
			'Throw New Exception(ex.Message)
			Master.errorMsg = ex.Message
		Finally
			If OleConn.State = ConnectionState.Open Then
				OleConn.Close()
			End If
			If OleDr IsNot Nothing AndAlso (Not OleDr.IsClosed) Then
				OleDr.Close()
			End If
		End Try
		Return False
	End Function

	Public Function GetAndInsertExcelToSQLTable_JurisdTypes(ByVal TableName As String) As Boolean
		Dim OleConn As New OleDbConnection
		Dim OleCmd As New OleDbCommand
		Dim OleDr As OleDbDataReader = Nothing

		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))

		If Not IO.File.Exists(Source_FilePath + Source_FileName) Then
			Master.errorMsg = "File Not Found."
			Return False
		End If

		Dim constr As String = ""
		constr += "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
		constr += Source_FilePath
		constr += Source_FileName + OleDbConnectionExtended
		'constr += ";Extended Properties=Excel 8.0;HDR=Yes;"
		constr += ";Extended Properties=""Excel 8.0;HDR=Yes;"""

		OleConn = New System.Data.OleDb.OleDbConnection(constr)

		OleConn.Open()

		Try

			With OleCmd
				.Connection = OleConn
				.CommandType = CommandType.Text
				.CommandText = OLEDB_TableScript_JurisdTypes(TableName)
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[jurisdictionTypes_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("jurisdictionTypeID", "jurisdictionTypeID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("jurisdictionType", "jurisdictionType")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("jurisdictionGroupID", "jurisdictionGroupID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)

				BulkCopyToSql.WriteToServer(OleDr)

				status = True
			End Using

			Return status
		Catch ex As Exception
			'Throw New Exception(ex.Message)
			Master.errorMsg = ex.Message
		Finally
			If OleConn.State = ConnectionState.Open Then
				OleConn.Close()
			End If
			If OleDr IsNot Nothing AndAlso (Not OleDr.IsClosed) Then
				OleDr.Close()
			End If
		End Try
		Return False
	End Function

	Public Function GetAndInsertExcelToSQLTable_MunicRollup(ByVal TableName As String) As Boolean
		Dim OleConn As New OleDbConnection
		Dim OleCmd As New OleDbCommand
		Dim OleDr As OleDbDataReader = Nothing

		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))

		If Not IO.File.Exists(Source_FilePath + Source_FileName) Then
			Master.errorMsg = "File Not Found."
			Return False
		End If

		Dim constr As String = ""
		constr += "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
		constr += Source_FilePath
		constr += Source_FileName + OleDbConnectionExtended
		'constr += ";Extended Properties=Excel 8.0;HDR=Yes;"
		constr += ";Extended Properties=""Excel 8.0;HDR=Yes;"""

		OleConn = New System.Data.OleDb.OleDbConnection(constr)

		OleConn.Open()

		Try

			With OleCmd
				.Connection = OleConn
				.CommandType = CommandType.Text
				.CommandText = OLEDB_TableScript_MunicRollup(TableName)
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[municipalityRollup_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("newMunicipalityID", "newMunicipalityID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("oldMunicipalityID", "oldMunicipalityID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)

				BulkCopyToSql.WriteToServer(OleDr)

				status = True
			End Using

			Return status
		Catch ex As Exception
			'Throw New Exception(ex.Message)
			Master.errorMsg = ex.Message
		Finally
			If OleConn.State = ConnectionState.Open Then
				OleConn.Close()
			End If
			If OleDr IsNot Nothing AndAlso (Not OleDr.IsClosed) Then
				OleDr.Close()
			End If
		End Try
		Return False
	End Function

	Public Function GetAndInsertExcelToSQLTable_PPID(ByVal TableName As String) As Boolean
		Dim OleConn As New OleDbConnection
		Dim OleCmd As New OleDbCommand
		Dim OleDr As OleDbDataReader = Nothing

		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))

		If Not IO.File.Exists(Source_FilePath + Source_FileName) Then
			Master.errorMsg = "File Not Found."
			Return False
		End If

		Dim constr As String = ""
		constr += "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
		constr += Source_FilePath
		constr += Source_FileName + OleDbConnectionExtended
		'constr += ";Extended Properties=Excel 8.0;HDR=Yes;"
		constr += ";Extended Properties=""Excel 8.0;HDR=Yes;"""

		OleConn = New System.Data.OleDb.OleDbConnection(constr)

		OleConn.Open()

		Try

			With OleCmd
				.Connection = OleConn
				.CommandType = CommandType.Text
				.CommandText = OLEDB_TableScript_PPID(TableName)
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[MunicipalitiesMapLink_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("PPID", "PPID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("UMNM", "UMNM")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("PATMAP_Code", "PATMAP_Code")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("PATMAP_Roll_Up_Name", "PATMAP_Roll_Up_Name")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("Sama_Code", "Sama_Code")
				BulkCopyToSql.ColumnMappings.Add(MapCol)

				BulkCopyToSql.WriteToServer(OleDr)

				status = True
			End Using

			Return status
		Catch ex As Exception
			'Throw New Exception(ex.Message)
			Master.errorMsg = ex.Message
		Finally
			If OleConn.State = ConnectionState.Open Then
				OleConn.Close()
			End If
			If OleDr IsNot Nothing AndAlso (Not OleDr.IsClosed) Then
				OleDr.Close()
			End If
		End Try
		Return False
	End Function

	Public Function RemoveFromTmpSSISTable_Entities() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [entities_SSIS_TMP]")

		con.Open()

		Try
			With cmd
				.Connection = con
				.CommandType = CommandType.Text
				.CommandText = query_str.ToString()
				.ExecuteNonQuery()
			End With
		Catch ex As Exception
			'Throw New Exception(ex.Message)
			Master.errorMsg = ex.Message
			Return False
		Finally
			con.Close()
		End Try
		Return True
	End Function

	Public Function RemoveFromTmpSSISTable_JurisdGroups() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [jurisdictionGroups_SSIS_TMP]")

		con.Open()

		Try
			With cmd
				.Connection = con
				.CommandType = CommandType.Text
				.CommandText = query_str.ToString()
				.ExecuteNonQuery()
			End With
		Catch ex As Exception
			'Throw New Exception(ex.Message)
			Master.errorMsg = ex.Message
			Return False
		Finally
			con.Close()
		End Try
		Return True
	End Function
	Public Function RemoveFromTmpSSISTable_JurisdTypes() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [jurisdictionTypes_SSIS_TMP]")

		con.Open()

		Try
			With cmd
				.Connection = con
				.CommandType = CommandType.Text
				.CommandText = query_str.ToString()
				.ExecuteNonQuery()
			End With
		Catch ex As Exception
			'Throw New Exception(ex.Message)
			Master.errorMsg = ex.Message
			Return False
		Finally
			con.Close()
		End Try
		Return True
	End Function
	Public Function RemoveFromTmpSSISTable_MunicRollup() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [municipalityRollup_SSIS_TMP]")

		con.Open()

		Try
			With cmd
				.Connection = con
				.CommandType = CommandType.Text
				.CommandText = query_str.ToString()
				.ExecuteNonQuery()
			End With
		Catch ex As Exception
			'Throw New Exception(ex.Message)
			Master.errorMsg = ex.Message
			Return False
		Finally
			con.Close()
		End Try
		Return True
	End Function
	Public Function RemoveFromTmpSSISTable_PPID() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [MunicipalitiesMapLink_SSIS_TMP]")

		con.Open()

		Try
			With cmd
				.Connection = con
				.CommandType = CommandType.Text
				.CommandText = query_str.ToString()
				.ExecuteNonQuery()
			End With
		Catch ex As Exception
			'Throw New Exception(ex.Message)
			Master.errorMsg = ex.Message
			Return False
		Finally
			con.Close()
		End Try
		Return True
	End Function

	Protected Function OLEDB_TableScript_Entities(ByVal TableName As String) As String
		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID, [Code] AS [number], [Municipality Name] AS jurisdiction, [Jurisdiction Type Code] AS jurisdictionTypeID " _
	 + " FROM " _
	 + " [" + TableName + "]"
		Return table_script
	End Function

	Protected Function OLEDB_TableScript_JurisdGroups(ByVal TableName As String) As String
		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID, [Jurisdiction Grouping Code] as jurisdictionGroupID, [Jurisdiction Grouping] as jurisdictionGroup " _
	 + " FROM " _
	 + " [" + TableName + "]"
		Return table_script
	End Function
	Protected Function OLEDB_TableScript_JurisdTypes(ByVal TableName As String) As String
		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID, [Jurisdiction Type Code] AS jurisdictionTypeID, [Jurisdiction Type] AS jurisdictionType, [Jurisdiction Grouping Code] AS jurisdictionGroupID " _
	 + " FROM " _
	 + " [" + TableName + "]"
		Return table_script
	End Function
	Protected Function OLEDB_TableScript_MunicRollup(ByVal TableName As String) As String
		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID, [Survey] as newMunicipalityID,  [SAMA Info] as oldMunicipalityID " _
	 + " FROM " _
	 + " [" + TableName + "]"
		Return table_script
	End Function
	Protected Function OLEDB_TableScript_PPID(ByVal TableName As String) As String
		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID, [PPID] AS [PPID], [UMNM/RMNO] AS UMNM, [PATMAP Code] AS PATMAP_Code, [PATMAP Roll Up Name] AS PATMAP_Roll_Up_Name, [Sama Code] AS Sama_Code " _
	 + " FROM " _
	 + " [" + TableName + "]"
		Return table_script
	End Function

	Protected Function Make_Source_Script_Entities() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,[number],jurisdiction,jurisdictionTypeID")
		Source_Connection_Script.Append(" FROM [entities_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function
	Protected Function Make_Source_Script_JurisdGroups() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,jurisdictionGroupID,jurisdictionGroup")
		Source_Connection_Script.Append(" FROM [jurisdictionGroups_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function
	Protected Function Make_Source_Script_JurisdTypes() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,jurisdictionTypeID,jurisdictionType,jurisdictionGroupID")
		Source_Connection_Script.Append(" FROM [jurisdictionTypes_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function
	Protected Function Make_Source_Script_MunicRollup() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,newMunicipalityID,oldMunicipalityID")
		Source_Connection_Script.Append(" FROM [municipalityRollup_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function
	Protected Function Make_Source_Script_PPID() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,[PPID],UMNM,PATMAP_Code,PATMAP_Roll_Up_Name,Sama_Code")
		Source_Connection_Script.Append(" FROM [MunicipalitiesMapLink_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function

End Class