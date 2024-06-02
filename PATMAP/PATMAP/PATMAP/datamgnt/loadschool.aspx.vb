Imports System.IO
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.OleDb

Partial Public Class loadschool
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
	Dim _PackageName As String = ConfigurationSettings.AppSettings("PackageName_SchoolToSQL").ToString
	Dim _LocalFileSubFolderPath As String = "/School/"
	Dim _Source_File_Extantion As String = "xls"
	Dim OleDbConnectionExtended As String = "" '" ;Extended Properties=Excel 8.0;"
	Dim _TableSchKey As String = "schkey$"


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
			'tr_btnLoad.Visible = false;
			'ddlTableNames_Visible(false);
			If (ddlTableSchKey.Visible = False) Then
				Load_Click()
				tr_btnLoad.Visible = False
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
				row.BeginEdit()
				If row("Table_Name").ToString.Equals(_TableSchKey) Then
					ddlTableSchKey.Items.Insert(0, row("Table_Name").ToString)
					ddlTableSchKey.SelectedIndex = 0
					tr_ddlTableSchKey.Visible = False
					row.Delete()
				End If
			Next
			dtTable.AcceptChanges()
			ddlTableNames_collection(dtTable, tr_ddlTableSchKey, ddlTableSchKey)
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
		tr_ddlTableSchKey.Visible = visible
	End Sub

	Protected Sub ddlTableNames_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		btnLoad_Visible()
	End Sub

	Protected Sub btnLoad_Visible()
		If Not ddlTableSchKey.SelectedValue.Equals("Select Table Name") Then
			tr_btnLoad.Visible = True
		Else
			tr_btnLoad.Visible = False
		End If
	End Sub

	Protected Sub txtNewDSN_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		btnLoad_Visible()
	End Sub

	Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnLoad.Click
		Load_Click()
	End Sub

	Protected Sub Load_Click()
		Dim Source_Connection_Script As String = ""
		Source_Connection_Script = Make_Source_Script()
		'If SSIS_Source_Connection_Script_Creation(Source_Connection_Script) Then
		If RemoveFromTmpSSISTable() Then
			If GetAndInsertExcelToSQLTable(Trim(ddlTableSchKey.SelectedValue)) Then
				If SSIS_Package_Bind("file", Source_Connection_Script) Then
					'Copy from _SSIS table to real table
					If SQL_Server_CopyData() Then

					End If
				End If
			End If
		End If
		'End If
	End Sub

	'Protected Function OLD_SSIS_Source_Connection_Script_Creation(ByRef Source_Connection_Script As String) As Boolean
	'	Dim status As Boolean = True
	'	Source_Connection_Script = "SELECT '" + Unic + "' as SessionID, [F1] as A, [F2] as B, [F3] as C, [F4] as D, [F5] as E, [F6] as F, [F7] as G, [F8] as H, [F9] as I, [F10] as J, [F11] as K, [F12] as L, [F13] as M, [F14] as N from " _
	'		+ "[" + ddlTableSchKey.SelectedValue + "A2:N]"
	'	'[schkey$A2:N]";
	'	Return status
	'End Function

	Protected Function SSIS_Source_Connection_Script_Creation(ByRef Source_Connection_Script As String) As Boolean
		Dim status As Boolean = True
		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
		Dim DataString As String = " OPENROWSET('Microsoft.ACE.OLEDB.12.0','Excel 8.0;HDR=Yes;Database=" + Source_FilePath + Source_FileName + "', "

		Source_Connection_Script = "SELECT '" + Unic + "' as SessionID, [F1] as A, [F2] as B, [F3] as C, [F4] as D, [F5] as E, [F6] as F, [F7] as G, [F8] as H, [F9] as I, [F10] as J, [F11] as K, [F12] as L, [F13] as M, [F14] as N from " _
		+ DataString _
		+ "'" + "SELECT * FROM  [" + ddlTableSchKey.SelectedValue + "A2:N]" + "')"
		'[schkey$A2:N]";
		Return status
	End Function

	Protected Function SSIS_Package_Bind(ByVal TypeOfConnection As String, ByVal Source_Connection_Script As String) As Boolean
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
		Return status
	End Function

	Protected Function SQL_Server_CopyData() As Boolean
		'1 
		Dim CommandText As String = " SELECT count (*) as count FROM [entities_SSIS] where [SessionID]='" + Unic + "'"
		Dim countEntities As Integer = Integer.Parse(SqlDbAccess.RunSqlReturnDataTable(CommandText).Rows(0)("count").ToString)
		If (countEntities > 0) Then
			CommandText = " DELETE FROM [entities] where [jurisdictionTypeID] = '1'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [entities_SSIS] where [SessionID]='" + Unic + "' and [number] in (SELECT [number] FROM [entities] where jurisdictionTypeID = 1)"
			SqlDbAccess.RunSql(CommandText)
			SqlDbAccess.RunSql(CommandText)
			CommandText = " INSERT INTO [entities]([number],[jurisdiction],[jurisdictionTypeID]) " _
			+ " SELECT [number],[jurisdiction],[jurisdictionTypeID] FROM [entities_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = (" SELECT count (*) as count FROM [entities_SSIS] where [SessionID]='" _
			 + (Unic + "'"))
			countEntities = Integer.Parse(SqlDbAccess.RunSqlReturnDataTable(CommandText).Rows(0)("count").ToString)
			CommandText = (" DELETE FROM [entities_SSIS] where [SessionID]='" _
			 + (Unic + "'"))
			SqlDbAccess.RunSql(CommandText)
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countEntities.ToString + "' rows to 'entities' table")))
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + countEntities.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'entities'"
		Else
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP314") + "'entities'"
		End If
		'2
		CommandText = " SELECT count (*) as count FROM [schoolApportionment_SSIS] where [SessionID]='" + Unic + "'"
		Dim dtSchKey As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
		Dim countSchKey As Integer = Integer.Parse(dtSchKey.Rows(0)("count").ToString)
		If (countSchKey > 0) Then
			CommandText = " DELETE FROM [schoolApportionment]"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " INSERT INTO [PATMAP].[dbo].[schoolApportionment]([schoolID],[subSchoolID],[portion]) " _
			 + " SELECT [schoolID],[subSchoolID],[portion] FROM [schoolApportionment_SSIS] where [SessionID] = '" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			CommandText = " DELETE FROM [schoolApportionment_SSIS] where [SessionID]='" + Unic + "'"
			SqlDbAccess.RunSql(CommandText)
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
			'            + (countSchKey.ToString + "' rows to 'schoolApportionment' table")))
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + countSchKey.ToString + PATMAP.common.GetErrorMessage("PATMAP316") + "'schoolApportionment'"
		Else
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP314") + "'schoolApportionmen'"
		End If
		Return ((countSchKey > 0) _
		 AndAlso (countEntities > 0))
	End Function

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
				BulkCopyToSql.DestinationTableName = "dbo.[schoolUpload_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("A", "A")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("B", "B")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("C", "C")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("D", "D")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("E", "E")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("F", "F")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("G", "G")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("H", "H")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("I", "I")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("J", "J")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("K", "K")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("L", "L")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("M", "M")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("N", "N")
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
		query_str.Append("DELETE FROM [schoolUpload_SSIS_TMP]")

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

		table_script = "SELECT '" + Unic + "' as SessionID, [F1] as A, [F2] as B, [F3] as C, [F4] as D, [F5] as E, [F6] as F, [F7] as G, [F8] as H, [F9] as I, [F10] as J, [F11] as K, [F12] as L, [F13] as M, [F14] as N " _
		+ " FROM " _
		+ " [" + TableName + "A2:N]"
		Return table_script
	End Function

	Protected Function Make_Source_Script() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,A,B,C,D,E,F,G,H,I,J,K,L,M,N")
		Source_Connection_Script.Append(" FROM [schoolUpload_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function

End Class