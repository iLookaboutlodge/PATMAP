Imports System.IO
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.OleDb

Partial Public Class loadmillrate
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Clears out the error message
			Master.errorMsg = ""

			If Not IsPostBack Then
				'Sets submenu to be displayed
				subMenu.setStartNode(menu.loadData)
				txtXlsColumnsInRowNumber.Text = "3"
				'tr_rb.Visible = False
				ddlYear_BindData()
			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number)
		End Try
		'lb _ Message.Text = ""
		Master.errorMsg = ""
	End Sub

	Dim SQL_DataSource As String = PATMAP.Global_asax.SQLEngineServer
	Dim SQL_InitialCatalog As String = PATMAP.Global_asax.DBName

	Dim _LocalFileRootPath As String = ConfigurationSettings.AppSettings("LocalFileRootPath").ToString
	Dim _PackagePath As String = ConfigurationSettings.AppSettings("PackagePath").ToString
	Dim _PackageName As String = ConfigurationSettings.AppSettings("PackageName_MillRateSurveyToSQL").ToString

	Dim _LocalFileSubFolderPath As String = "/MillRateSurvey/"
	Dim _Source_File_Extantion As String = "xls"
	Dim _typeofData As String = "MillRateSurvey"
	Dim OleDbConnectionExtended As String = "" '" ;Extended Properties=Excel 8.0;"

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
		'Do While (i <= ((DateTime.Now.Year - YearStart) + 4))
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
			'tr_rb.Visible = False
			txtNewDSN.Text = (_typeofData + (" Data for " + ddlYear.SelectedValue))
		Else
			tr_txtNewDSN.Visible = False
			'tr_rb.Visible = True
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
			lblExisting.Text = "Replace&nbsp;&nbsp;"
		Else
			ddlDSN.Items.FindByValue("-1").Text = "Select Current Data Set"
			tr_Year.Visible = True
			lblExisting.Text = "Existing&nbsp;&nbsp;"
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
			tr_DataSet.Visible = False
			tr_btnLoad.Visible = False
			'ddlTableColumns_Visible(false);
			ddlDSN_BindData()
			ViewState("descriptionTableID") = Nothing
			ddlDSN.Enabled = True
			txtNewDSN.Enabled = True
			ddlYear.Enabled = True
		End If
	End Sub

	Protected Function ddlTableNames_BindData() As Boolean
		Dim status As Boolean = True
		'string Source_FilePath = Server.MapPath(RepositoryPath);
		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
		Dim cnAccessConn As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection
		Dim dtTable As DataTable
		Try

			'cnAccessConn = New System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0 ;Data Source=" + Source_FilePath + Source_FileName + OleDbConnectionExtended)

			cnAccessConn = New System.Data.OleDb.OleDbConnection(("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" _
			 + (Source_FilePath _
			 + (Source_FileName + OleDbConnectionExtended)) _
			 + ";Extended Properties=""Excel 8.0;HDR=Yes;"""))

			cnAccessConn.Open()
			dtTable = cnAccessConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
			For Each row As DataRow In dtTable.Select
				If (Not row("Table_Name").ToString.ToLower.Contains("$") _
					OrElse (row("Table_Name").ToString.ToLower.Contains("summary") OrElse row("Table_Name").ToString.ToLower.Contains("notes"))) Then
					row.Delete()
				End If
			Next
			dtTable.AcceptChanges()
			If (dtTable.Rows.Count > 0) Then
				ddlTableNames.DataSource = dtTable
				ddlTableNames.DataTextField = dtTable.Columns("Table_Name").ToString
				ddlTableNames.DataBind()
				ddlTableNames.Items.Insert(0, New ListItem("Select Table Name", "-1"))
				ddlTableNames.SelectedIndex = 0
				tr_ddlTableNames.Visible = True
				'tr_newFile.Visible = False
			End If
		Catch ex As Exception
			Master.errorMsg = ex.ToString
			status = False
		Finally
			cnAccessConn.Close()
		End Try
		Return status
	End Function

	Protected Sub ddlTableNames_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim status As Boolean = True
		If (ddlTableNames.SelectedIndex <> 0) Then
			Dim TableName As String = ddlTableNames.SelectedValue
			'if (ddlTableColumns_BindData(TableName))
			'{
			tr_DataSet.Visible = True
			tr_newFile.Visible = False
			'}
		Else
			status = False
		End If
		If Not status Then
			tr_DataSet.Visible = False
			tr_btnLoad.Visible = False
			'ddlTableColumns_Visible(false);
		Else
			'txtNewDSN.Text = _typeofData + " Data for " + ddlYear.SelectedValue;
			btnLoad_Visible()
		End If
	End Sub

	Protected Sub txtNewDSN_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		btnLoad_Visible()
	End Sub

	Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnLoad.Click
		Dim Source_Connection_Script As String = ""
		Source_Connection_Script = Make_Source_Script()
		If (ddlTableNames.SelectedIndex <> 0) Then
			'If SSIS_Source_Connection_Script_Creation(ddlTableNames.SelectedValue, Source_Connection_Script) Then
			If RemoveFromTmpSSISTable() Then
				If GetAndInsertExcelToSQLTable(Trim(ddlTableNames.SelectedValue)) Then
					If SSIS_Package_Bind("file", Source_Connection_Script) Then
						'Copy from _SSIS table to real table
						If SQL_Server_CopyData() Then
							ddlDSN.Enabled = False
							txtNewDSN.Enabled = False
							ddlYear.Enabled = False
							ddlTableNames.Items.RemoveAt(ddlTableNames.SelectedIndex)
							If (ddlTableNames.Items.Count = 1) Then
								SourceFileMove(_descriptionTableID)
							End If
						End If
					End If
				End If
			End If
			'End If
		Else
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP306")
		End If
	End Sub

	Protected Function SSIS_Package_Bind(ByVal TypeOfConnection As String, ByVal Source_Connection_Script As String) As Boolean
		Dim status As Boolean = False
		Master.errorMsg = ""
		tr_XlsColumnsInRowNumber.Visible = False
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
					vars("Source_Connection_Script").Value = Source_Connection_Script
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
		If Not status Then
			'lb _ Message.Text = lb _ Message.Text
			'    + "<br>Source_FilePath: " + Source_FilePath
			'    + "<br>Source_FileName: " + Source_FileName
			'    + "<br>Source_Connection_Script: " + Source_Connection_Script;
			tr_XlsColumnsInRowNumber.Visible = True
		End If
		Return status
	End Function

	'=============================
	Protected Sub btnLoad_Visible()
		If ((ddlDSN.SelectedIndex <> 0) _
			 OrElse (Not txtNewDSN.Text.Equals("") _
			 AndAlso (ddlYear.SelectedIndex <> 0))) Then
			tr_btnLoad.Visible = True
		Else
			tr_btnLoad.Visible = False
		End If
	End Sub

	'Protected Function OLD_SSIS_Source_Connection_Script_Creation(ByVal TableName As String, ByRef Source_Connection_Script As String) As Boolean
	'	Dim status As Boolean = True
	'	Source_Connection_Script = "SELECT '" + Unic + "' as SessionID" _
	'	 + ", [SAMA Code] as MunicipalityID, [Total Municipal Tax Levy] as Levy " _
	'	 + " FROM  [" + TableName + "A" + txtXlsColumnsInRowNumber.Text + ":Z]"
	'	Return status
	'End Function

	Protected Function SSIS_Source_Connection_Script_Creation(ByVal TableName As String, ByRef Source_Connection_Script As String) As Boolean
		Dim status As Boolean = True
		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
		Dim DataString As String = " OPENROWSET('Microsoft.ACE.OLEDB.12.0','Excel 8.0;HDR=Yes;Database=" + Source_FilePath + Source_FileName + "', "

		Source_Connection_Script = "SELECT '" + Unic + "' as SessionID" _
		+ ", [SAMA Code] as MunicipalityID, [Total Municipal Tax Levy] as Levy FROM " _
		+ DataString _
		+ "'" + "SELECT * FROM  [" + TableName + "A" + txtXlsColumnsInRowNumber.Text + ":Q]" + "')"
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
				+ "[year],[dataSetName],[notes],[dateLoaded],[statusID])" _
				+ "VALUES (" _
				+ "'" + ddlYear.SelectedValue + "'" _
				+ ", '" + dataSetName + "'" _
				+ ", 'Hello World'" _
				+ ",getdate()" + ",'1') select @@IDENTITY"
				'+ ",'" + DateTime.Now.ToString() + "'" _

				SqlDbAccess.RunSqlReturnIdentity(CommandText, descriptionTableID)
				CommandText = "INSERT INTO [" + _typeofData + "File](" _
				 + "[" + _typeofData + "ID],[filename],[dateLoaded])" _
				 + " VALUES (" _
				 + "'" + descriptionTableID + "'" _
				 + ",'" + Unic + "." + _Source_File_Extantion + "'" _
				 + ",getdate()" + ") select @@IDENTITY"
				'+ ",'" + DateTime.Now.ToString() + "'" _

				Dim fileTableID As String = ""
				SqlDbAccess.RunSqlReturnIdentity(CommandText, fileTableID)
			Else
				descriptionTableID = ddlDSN.SelectedValue
				CommandText = "DELETE FROM [millRateSurvey] WHERE [millRateSurveyID]=" _
					+ "'" + descriptionTableID + "'"
				SqlDbAccess.RunSql(CommandText)
			End If
			ViewState("descriptionTableID") = descriptionTableID
		Else
			descriptionTableID = ViewState("descriptionTableID").ToString
		End If

		CommandText = "INSERT INTO [millRateSurvey] ([millRateSurveyID],[MunicipalityID],[Levy]) " _
		+ " SELECT " _
		+ "'" + descriptionTableID + "', [MunicipalityID],[Levy]" _
		+ "FROM [" + _typeofData + "_SSIS] where [SessionID] = '" + Unic + "'"
		SqlDbAccess.RunSql(CommandText)
		CommandText = "select count(*) as count FROM [" + _typeofData + "_SSIS] where [SessionID] = '" + Unic + "'"
		Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim count As String = dt.Rows(0)("count").ToString
		'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
		'            + (count + ("' rows to '" _
		'            + (_typeofData + "' table")))))
		Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + count + PATMAP.common.GetErrorMessage("PATMAP316") + "'" + _typeofData + "'"
		CommandText = ("delete FROM [" _
		 + (_typeofData + ("_SSIS] where [SessionID] = '" _
		 + (Unic + "'"))))
		SqlDbAccess.RunSql(CommandText)
		_descriptionTableID = descriptionTableID
		Return (Integer.Parse(descriptionTableID) > 0)
	End Function



	Dim _descriptionTableID As String = ""
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

	Public Function GetAndInsertExcelToSQLTable(ByVal TableName As String) As Boolean
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
				.CommandText = OLEDB_TableScript(TableName)
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[millRateSurvey_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("MunicipalityID", "MunicipalityID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("Levy", "Levy")
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

	Public Function RemoveFromTmpSSISTable() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [millRateSurvey_SSIS_TMP]")

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

	Protected Function OLEDB_TableScript(ByVal TableName As String) As String
		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID" _
		+ ", [SAMA Code] as MunicipalityID, [Total Municipal Tax Levy] as Levy FROM " _
		+ " [" + TableName + "A" + txtXlsColumnsInRowNumber.Text + ":Q]"

		Return table_script
	End Function

	Protected Function Make_Source_Script() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,MunicipalityID,Levy")
		Source_Connection_Script.Append(" FROM [millRateSurvey_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function

End Class