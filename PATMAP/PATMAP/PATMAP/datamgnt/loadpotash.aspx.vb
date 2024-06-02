Imports System.IO
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.OleDb

Partial Public Class loadpotash
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Clears out the error message
			Master.errorMsg = ""

			If Not IsPostBack Then
				'Sets submenu to be displayed
				subMenu.setStartNode(menu.loadData)
				ddlYear_BindData()
			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
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
	Dim _PackageName As String = ConfigurationSettings.AppSettings("PackageName_PotashToSQL").ToString
	Dim _LocalFileSubFolderPath As String = "/Potash/"
	Dim _Source_File_Extantion As String = "xls"
	Dim _typeofData As String = "Potash"
	Dim OleDbConnectionExtended As String = "" '" ;Extended Properties=Excel 8.0;"
	Dim _TableAreas As String = "'Potash Areas$'"
	Dim _TableParcels As String = "'Potash Properties$'"
	Dim _TableRural As String = "'Rural Municipalities$'"
	Dim _TableUrban As String = "'Urban Municipalities$'"


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

	Protected Sub ddlYear_BindData()
		'ddlYear.Items.Insert(0, New ListItem("Select Year", "-1"))
		'ddlYear.SelectedIndex = 0
		'tr_txtNewDSN.Visible = False
		'Dim YearStart As Integer = 2000
		'Dim i As Integer = 1
		'Do While (i _
		'            <= ((DateTime.Now.Year - YearStart) _
		'            + 4))
		'    ddlYear.Items.Insert(i, CType((YearStart + i), Integer).ToString)
		'    i = (i + 1)
		'Loop
		tr_txtNewDSN.Visible = False
		ddlYear.DataSource = common.GetYears()
		ddlYear.DataBind()
	End Sub

	Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		If (ddlYear.SelectedIndex <> 0) Then
			tr_txtNewDSN.Visible = True
			txtNewDSN.Text = (_typeofData + (" Data for " + ddlYear.SelectedValue))
		Else
			tr_txtNewDSN.Visible = False
			txtNewDSN.Text = ""
		End If
		btnLoad_Visible()
	End Sub

	Protected Sub ddlDSN_BindData()
		Dim constr As String = "select " + _typeofData + "ID as ID, dataSetName from " + _typeofData + "Description order by dataSetName"
		ddlDSN.DataSource = SqlDbAccess.RunSqlReturnDataTable(constr)
		ddlDSN.DataTextField = "dataSetName"
		ddlDSN.DataValueField = "ID"
		ddlDSN.DataBind()
		ddlDSN.Items.Insert(0, New ListItem("Select Current Data Set", "-1"))
		ddlDSN.SelectedIndex = 0
	End Sub

	Protected Sub ddlDSN_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		If (ddlDSN.SelectedIndex <> 0) Then
			ddlDSN.Items.FindByValue("-1").Text = "Create New Data Set"
			tr_txtNewDSN.Visible = False
			tr_Year.Visible = False
		Else
			ddlDSN.Items.FindByValue("-1").Text = "Select Current Data Set"
			tr_Year.Visible = True
		End If
		btnLoad_Visible()
	End Sub

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
			Unic = (DateTime.Today.Year.ToString + ("_" _
				 + (DateTime.Today.Month.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) + ("_" _
				 + (DateTime.Today.Day.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) + ("_" + Guid.NewGuid.ToString))))))
			Dim RepositoryUnicFileName As String = (_LocalFileRootPath _
				+ (_LocalFileSubFolderPath _
				+ (Unic + ("." + _Source_File_Extantion))))
			fpFile.SaveAs(RepositoryUnicFileName)
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
			ddlDSN_BindData()
			ViewState("descriptionTableID") = Nothing
			ddlDSN.Enabled = True
			txtNewDSN.Enabled = True
			ddlYear.Enabled = True
			btnLoad.Visible = True
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
			'+ (Source_FilePath _
			'+ (Source_FileName + OleDbConnectionExtended))))

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
				If row("Table_Name").ToString.Equals(_TableAreas) Then
					ddlTableAreas.Items.Insert(0, row("Table_Name").ToString)
					ddlTableAreas.SelectedIndex = 0
					tr_ddlTableAreas.Visible = False
					row.Delete()
					'TODO: Warning!!! continue If
					dtTable.AcceptChanges()
				End If
			Next
			For Each row As DataRow In dtTable.Select
				If row("Table_Name").ToString.Equals(_TableParcels) Then
					ddlTableParcels.Items.Insert(0, row("Table_Name").ToString)
					ddlTableParcels.SelectedIndex = 0
					tr_ddlTableParcels.Visible = False
					row.Delete()
					'TODO: Warning!!! continue If
					dtTable.AcceptChanges()
				End If
			Next
			For Each row As DataRow In dtTable.Select

				If row("Table_Name").ToString.Equals(_TableRural) Then
					ddlTableRural.Items.Insert(0, row("Table_Name").ToString)
					ddlTableRural.SelectedIndex = 0
					tr_ddlTableRural.Visible = False
					row.Delete()
					'TODO: Warning!!! continue If
					dtTable.AcceptChanges()
				End If
			Next
			For Each row As DataRow In dtTable.Select

				If row("Table_Name").ToString.Equals(_TableUrban) Then
					ddlTableUrban.Items.Insert(0, row("Table_Name").ToString)
					ddlTableUrban.SelectedIndex = 0
					tr_ddlTableUrban.Visible = False
					row.Delete()
					'TODO: Warning!!! continue If
					dtTable.AcceptChanges()
				End If
			Next
			dtTable.AcceptChanges()
			ddlTableNames_collection(dtTable, tr_ddlTableAreas, ddlTableAreas)
			ddlTableNames_collection(dtTable, tr_ddlTableParcels, ddlTableParcels)
			ddlTableNames_collection(dtTable, tr_ddlTableRural, ddlTableRural)
			ddlTableNames_collection(dtTable, tr_ddlTableUrban, ddlTableUrban)
		End If
		DataSet_Visible()
		Return status
	End Function

	Protected Sub ddlTableNames_collection(ByVal dtColumnNames As DataTable, ByVal tr As HtmlTableRow, ByVal ddl As DropDownList)
		If tr.Visible Then
			ddl.DataSource = dtColumnNames
			ddl.DataTextField = dtColumnNames.Columns("Table_Name").ToString
			ddl.DataBind()
			ddl.Items.Insert(0, "Select Table Name")
			ddl.SelectedIndex = 0
		End If
	End Sub

	Protected Sub ddlTables_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		DataSet_Visible()
	End Sub

	Protected Sub DataSet_Visible()
		If (Not ddlTableAreas.SelectedValue.Equals("Select Table Name") _
			 AndAlso (Not ddlTableParcels.SelectedValue.Equals("Select Table Name") _
			 AndAlso (Not ddlTableRural.SelectedValue.Equals("Select Table Name") _
			 AndAlso Not ddlTableUrban.SelectedValue.Equals("Select Table Name")))) Then
			tr_DataSet.Visible = True
		Else
			tr_DataSet.Visible = False
		End If
	End Sub

	Protected Sub btnLoad_Visible()
		If (((ddlDSN.SelectedIndex <> 0) _
			 OrElse (Not txtNewDSN.Text.Equals("") _
			 AndAlso (ddlYear.SelectedIndex <> 0))) _
			 AndAlso (Not ddlTableAreas.SelectedValue.Equals("Select Table Name") _
			 AndAlso (Not ddlTableParcels.SelectedValue.Equals("Select Table Name") _
			 AndAlso (Not ddlTableRural.SelectedValue.Equals("Select Table Name") _
			 AndAlso Not ddlTableUrban.SelectedValue.Equals("Select Table Name"))))) Then
			tr_btnLoad.Visible = True
		Else
			tr_btnLoad.Visible = False
		End If
	End Sub

	Protected Sub ddlTableNames_Visible(ByVal visible As Boolean)
		tr_ddlTableAreas.Visible = visible
		tr_ddlTableParcels.Visible = visible
		tr_ddlTableRural.Visible = visible
		tr_ddlTableUrban.Visible = visible

	End Sub

	Protected Sub txtNewDSN_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		btnLoad_Visible()
	End Sub

	Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnLoad.Click
		Load_Click()
	End Sub

	Protected Sub Load_Click()
		Dim Source_Connection_Script_Areas As String = ""
		Dim Source_Connection_Script_Parcels As String = ""
		Dim Source_Connection_Script_Rural As String = ""
		Dim Source_Connection_Script_Urban As String = ""

		Source_Connection_Script_Areas = Make_Source_Script_Areas()
		Source_Connection_Script_Parcels = Make_Source_Script_Parcels()
		Source_Connection_Script_Rural = Make_Source_Script_Rural()
		Source_Connection_Script_Urban = Make_Source_Script_Urban()

		'If SSIS_Source_Connection_Script_Creation(Source_Connection_Script_Areas, Source_Connection_Script_Parcels, Source_Connection_Script_Rural, Source_Connection_Script_Urban) Then
		If RemoveFromTmpSSISTable_Areas() And RemoveFromTmpSSISTable_Parcels() And RemoveFromTmpSSISTable_Rural() And RemoveFromTmpSSISTable_Urban() Then
			If GetAndInsertExcelToSQLTable_Areas(Trim(ddlTableAreas.SelectedValue)) And _
			GetAndInsertExcelToSQLTable_Parcels(Trim(ddlTableParcels.SelectedValue)) And _
			GetAndInsertExcelToSQLTable_Rural(Trim(ddlTableRural.SelectedValue)) And _
			GetAndInsertExcelToSQLTable_Urban(Trim(ddlTableUrban.SelectedValue)) Then

				If SSIS_Package_Bind("file", Source_Connection_Script_Areas, Source_Connection_Script_Parcels, Source_Connection_Script_Rural, Source_Connection_Script_Urban) Then
					'Copy from _SSIS table to real table
					If SQL_Server_CopyData() Then
						ddlDSN.Enabled = False
						txtNewDSN.Enabled = False
						ddlYear.Enabled = False
						btnLoad.Visible = False
					End If
				End If
			End If
		End If
		'End If
	End Sub

	'Protected Function OLD_SSIS_Source_Connection_Script_Creation(ByRef Source_Connection_Script_Areas As String, ByRef Source_Connection_Script_Parcels As String, ByRef Source_Connection_Script_Rural As String, ByRef Source_Connection_Script_Urban As String) As Boolean
	'	Dim status As Boolean = True
	'	Source_Connection_Script_Areas = "SELECT '" + Unic + "' as SessionID, [Potash Area ID] as PotashAreaID,  [Potash Area] as  PotashArea,  [Administration Expense Factor] as administrationExpensePercentage FROM " _
	'		+ "[" + ddlTableAreas.SelectedValue + "]"
	'	Source_Connection_Script_Parcels = "SELECT '" + Unic + "' as SessionID, [Potash Area] AS potashAreaID, [RM] AS municipalityID, [Alternate Parcel ID] AS alternate_ParcelID, [Owner] AS owner FROM " _
	'			+ "[" + ddlTableParcels.SelectedValue + "]"
	'	Source_Connection_Script_Rural = "SELECT '" + Unic + "' as SessionID, [Potash Area ID] AS potashAreaID, [SAMA Code] AS municipalityID, [Area in Square Miles] AS areaInSquareMiles, [Statutory Discount Factor] AS statutoryDiscountPercentage, [Mill Rate Factor] AS millRateFactor, [Total Points] AS totalPoints, [Board Adjustments] AS boardAdjustments, [Total Grant] AS totalGrant FROM " _
	'		+ "[" + ddlTableRural.SelectedValue + "]"
	'	Source_Connection_Script_Urban = "SELECT '" + Unic + "' as SessionID, [Potash Area ID] AS potashAreaID, [SAMA Code] AS municipalityID, [Total Points] AS totalPoints, [Total Grant] AS totalGrant FROM " _
	'			+ "[" + ddlTableUrban.SelectedValue + "]"
	'	Return status
	'End Function

	Protected Function SSIS_Source_Connection_Script_Creation(ByRef Source_Connection_Script_Areas As String, ByRef Source_Connection_Script_Parcels As String, ByRef Source_Connection_Script_Rural As String, ByRef Source_Connection_Script_Urban As String) As Boolean
		Dim status As Boolean = True
		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
		Dim DataString As String = " OPENROWSET('Microsoft.ACE.OLEDB.12.0','Excel 8.0;HDR=Yes;Database=" + Source_FilePath + Source_FileName + "', "

		Source_Connection_Script_Areas = "SELECT '" + Unic + "' as SessionID, [Potash Area ID] as PotashAreaID,  [Potash Area] as  PotashArea,  [Administration Expense Factor] as administrationExpensePercentage FROM " _
		+ DataString + "[" + ddlTableAreas.SelectedValue + "]" + ")"
		Source_Connection_Script_Parcels = "SELECT '" + Unic + "' as SessionID, [Potash Area] AS potashAreaID, [RM] AS municipalityID, [Alternate Parcel ID] AS alternate_ParcelID, [Owner] AS owner FROM " _
		+ DataString + "[" + ddlTableParcels.SelectedValue + "]" + ")"
		Source_Connection_Script_Rural = "SELECT '" + Unic + "' as SessionID, [Potash Area ID] AS potashAreaID, [SAMA Code] AS municipalityID, [Area in Square Miles] AS areaInSquareMiles, [Statutory Discount Factor] AS statutoryDiscountPercentage, [Mill Rate Factor] AS millRateFactor, [Total Points] AS totalPoints, [Board Adjustments] AS boardAdjustments, [Total Grant] AS totalGrant FROM " _
		 + DataString + "[" + ddlTableRural.SelectedValue + "]" + ")"
		Source_Connection_Script_Urban = "SELECT '" + Unic + "' as SessionID, [Potash Area ID] AS potashAreaID, [SAMA Code] AS municipalityID, [Total Points] AS totalPoints, [Total Grant] AS totalGrant FROM " _
		 + DataString + "[" + ddlTableUrban.SelectedValue + "]" + ")"
		Return status
	End Function


	Protected Function SSIS_Package_Bind(ByVal TypeOfConnection As String, ByVal Source_Connection_Script_Areas As String, ByVal Source_Connection_Script_Parcels As String, ByVal Source_Connection_Script_Rural As String, ByVal Source_Connection_Script_Urban As String) As Boolean
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
					vars("Source_Connection_Script_Areas").Value = Source_Connection_Script_Areas
					vars("Source_Connection_Script_Parcels").Value = Source_Connection_Script_Parcels
					vars("Source_Connection_Script_Rural").Value = Source_Connection_Script_Rural
					vars("Source_Connection_Script_Urban").Value = Source_Connection_Script_Urban
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
		Dim CommandText As String = ""
		Dim descriptionTableID As String = ""
		If (ViewState("descriptionTableID") = Nothing) Then
			If (ddlDSN.SelectedIndex = 0) Then
				Dim dataSetName As String = ""
				dataSetName = txtNewDSN.Text
				CommandText = "INSERT INTO [" + _typeofData + "Description](" _
				+ "[year],[dataSetName],[notes]" _
				+ ",[statusID])" _
				+ "VALUES (" _
				+ "'" + ddlYear.SelectedValue + "'" _
				+ ", '" + dataSetName + "'" _
				+ ", 'Hello World'" _
				+ ",'1') select @@IDENTITY"
				SqlDbAccess.RunSqlReturnIdentity(CommandText, descriptionTableID)
				CommandText = "INSERT INTO [" + _typeofData + "File](" _
				 + "[" + _typeofData + "ID],[filename],[dateLoaded])" _
				 + " VALUES (" _
				 + "'" + descriptionTableID + "'" _
				 + ",'" + Unic + "." + _Source_File_Extantion + "'" _
				 + ",getdate()" _
				 + ") select @@IDENTITY"

				'                    + ",'" + DateTime.Now.ToString() + "'" _

				Dim fileTableID As String = ""
				SqlDbAccess.RunSqlReturnIdentity(CommandText, fileTableID)
			Else
				descriptionTableID = ddlDSN.SelectedValue
			End If
			ViewState("descriptionTableID") = descriptionTableID
		Else
			descriptionTableID = ViewState("descriptionTableID").ToString
		End If
		'1
		CommandText = " SELECT count (*) as count FROM [potashAreas_SSIS] where [SessionID]='" + Unic + "'"
		Dim dtAreas As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim countAreas As Integer = Integer.Parse(dtAreas.Rows(0)("count").ToString)
		If (countAreas > 0) Then
			CommandText = " DELETE FROM [potashAreas]"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " INSERT INTO [potashAreas] ([potashAreaID],[potashArea],[administrationExpensePercentage])" _
			 + " SELECT [potashAreaID],[potashArea],[administrationExpensePercentage] FROM [potashAreas_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [potashAreas_SSIS] where [SessionID]='" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countAreas.ToString + "' rows to 'potashAreas' table")))
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + countAreas.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'potashAreas'"
		Else
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP314") + "'potashAreas'"
		End If
		'2
		CommandText = " SELECT count (*) as count FROM [potashParcels_SSIS] where [SessionID]='" + Unic + "'"
		Dim dtParcels As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim countParcels As Integer = Integer.Parse(dtParcels.Rows(0)("count").ToString)
		If (countParcels > 0) Then
			CommandText = " DELETE FROM [potashParcels]"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " INSERT INTO [potashParcels] ([potashAreaID],[municipalityID],[alternate_ParcelID],[owner])" _
			 + " SELECT [potashAreaID],[municipalityID],[alternate_ParcelID],[owner] FROM [potashParcels_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [potashParcels_SSIS] where [SessionID]='" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countParcels.ToString + "' rows to 'potashParcels' table")))
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + countParcels.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'potashParcels'"
		Else
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP314") + "'potashParcels'"
		End If
		'3
		CommandText = " SELECT count (*) as count FROM [potashUrban_SSIS] where [SessionID]='" + Unic + "'"
		Dim dtUrban As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim countUrban As Integer = Integer.Parse(dtUrban.Rows(0)("count").ToString)
		If (countUrban > 0) Then
			CommandText = " INSERT INTO [potashUrban] ([potashID],[potashAreaID],[municipalityID],[totalPoints],[totalGrant])" _
			 + " SELECT " _
			 + "'" + descriptionTableID + "', [potashAreaID],[municipalityID],[totalPoints],[totalGrant] FROM [potashUrban_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [potashUrban_SSIS] where [SessionID]='" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countUrban.ToString + "' rows to 'potashUrban' table")))
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + countUrban.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'potashUrban'"
		Else
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP314") + "'potashUrban'"
		End If
		'4
		CommandText = " SELECT count (*) as count FROM [potashRural_SSIS] where [SessionID]='" + Unic + "'"
		Dim dtRural As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim countRural As Integer = Integer.Parse(dtRural.Rows(0)("count").ToString)
		If (countRural > 0) Then
			CommandText = " INSERT INTO [potashRural]([potashID],[potashAreaID],[municipalityID],[areaInSquareMiles],[statutoryDiscountPercentage],[millRateFactor],[totalPoints],[boardAdjustments],[totalGrant])" _
			 + " SELECT " _
			 + "'" + descriptionTableID + "',[potashAreaID],[municipalityID],[areaInSquareMiles],[statutoryDiscountPercentage],[millRateFactor],[totalPoints],[boardAdjustments],[totalGrant] FROM [potashRural_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [potashRural_SSIS] where [SessionID]='" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countRural.ToString + "' rows to 'potashRural' table")))
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + countRural.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'potashRural'"
		Else
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP314") + "'potashRural'"
		End If
		SourceFileMove(descriptionTableID)
		Return ((countParcels > 0) _
			 AndAlso ((countAreas > 0) _
			 AndAlso ((countUrban > 0) _
			 AndAlso (countRural > 0))))
	End Function

	Protected Sub SourceFileMove(ByVal ID As String)
		Dim Source_FilePath As String = _LocalFileRootPath + _LocalFileSubFolderPath
		Dim Source_FileName As String = Unic + "." + _Source_File_Extantion
		Dim Destin_FilePath As String = Source_FilePath + ID
		If Not Directory.Exists(Destin_FilePath) Then
			Directory.CreateDirectory(Destin_FilePath)
		End If
		If (File.Exists(Source_FilePath + Source_FileName) AndAlso Directory.Exists(Destin_FilePath)) Then
			File.Move(Source_FilePath + Source_FileName, Destin_FilePath + "/" + Source_FileName)
		End If
	End Sub

	Public Function GetAndInsertExcelToSQLTable_Areas(ByVal TableName As String) As Boolean
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
				.CommandText = OLEDB_TableScript_Areas(TableName)
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[potashAreas_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("PotashAreaID", "PotashAreaID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("PotashArea", "PotashArea")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("administrationExpensePercentage", "administrationExpensePercentage")
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

	Public Function GetAndInsertExcelToSQLTable_Parcels(ByVal TableName As String) As Boolean
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
				.CommandText = OLEDB_TableScript_Parcels(TableName)
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[potashParcels_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("potashAreaID", "potashAreaID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("municipalityID", "municipalityID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("alternate_ParcelID", "alternate_ParcelID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("owner", "owner")
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

	Public Function GetAndInsertExcelToSQLTable_Rural(ByVal TableName As String) As Boolean
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
				.CommandText = OLEDB_TableScript_Rural(TableName)
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[potashRural_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("potashAreaID", "potashAreaID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("municipalityID", "municipalityID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("areaInSquareMiles", "areaInSquareMiles")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("statutoryDiscountPercentage", "statutoryDiscountPercentage")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("millRateFactor", "millRateFactor")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("totalPoints", "totalPoints")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("boardAdjustments", "boardAdjustments")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("totalGrant", "totalGrant")
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

	Public Function GetAndInsertExcelToSQLTable_Urban(ByVal TableName As String) As Boolean
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
				.CommandText = OLEDB_TableScript_Urban(TableName)
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[potashUrban_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("potashAreaID", "potashAreaID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("municipalityID", "municipalityID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("totalPoints", "totalPoints")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("totalGrant", "totalGrant")
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

	Public Function RemoveFromTmpSSISTable_Areas() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [potashAreas_SSIS_TMP]")

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

	Public Function RemoveFromTmpSSISTable_Parcels() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [potashParcels_SSIS_TMP]")

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

	Public Function RemoveFromTmpSSISTable_Rural() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [potashRural_SSIS_TMP]")

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

	Public Function RemoveFromTmpSSISTable_Urban() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [potashUrban_SSIS_TMP]")

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

	Protected Function OLEDB_TableScript_Areas(ByVal TableName As String) As String
		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID, [Potash Area ID] as PotashAreaID,  [Potash Area] as  PotashArea,  [Administration Expense Factor] as administrationExpensePercentage " _
		+ " FROM " _
		+ " [" + TableName + "]"
		Return table_script
	End Function

	Protected Function OLEDB_TableScript_Parcels(ByVal TableName As String) As String
		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID, [Potash Area] AS potashAreaID, [RM] AS municipalityID, [Alternate Parcel ID] AS alternate_ParcelID, [Owner] AS owner " _
	+ " FROM " _
	+ " [" + TableName + "]"
		Return table_script
	End Function
	Protected Function OLEDB_TableScript_Rural(ByVal TableName As String) As String
		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID, [Potash Area ID] AS potashAreaID, [SAMA Code] AS municipalityID, [Area in Square Miles] AS areaInSquareMiles, [Statutory Discount Factor] AS statutoryDiscountPercentage, [Mill Rate Factor] AS millRateFactor, [Total Points] AS totalPoints, [Board Adjustments] AS boardAdjustments, [Total Grant] AS totalGrant " _
	+ " FROM " _
	+ " [" + TableName + "]"
		Return table_script
	End Function
	Protected Function OLEDB_TableScript_Urban(ByVal TableName As String) As String
		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID, [Potash Area ID] AS potashAreaID, [SAMA Code] AS municipalityID, [Total Points] AS totalPoints, [Total Grant] AS totalGrant " _
	+ " FROM " _
	+ " [" + TableName + "]"
		Return table_script
	End Function

	Protected Function Make_Source_Script_Areas() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,PotashAreaID,PotashArea,administrationExpensePercentage")
		Source_Connection_Script.Append(" FROM [potashAreas_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function

	Protected Function Make_Source_Script_Parcels() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,potashAreaID,municipalityID,alternate_ParcelID,owner")
		Source_Connection_Script.Append(" FROM [potashParcels_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function

	Protected Function Make_Source_Script_Rural() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,potashAreaID,municipalityID,areaInSquareMiles,statutoryDiscountPercentage,millRateFactor,totalPoints,boardAdjustments,totalGrant")
		Source_Connection_Script.Append(" FROM [potashRural_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function

	Protected Function Make_Source_Script_Urban() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,potashAreaID,municipalityID,totalPoints,totalGrant")
		Source_Connection_Script.Append(" FROM [potashUrban_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function

End Class