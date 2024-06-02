Imports System.IO
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Public Class loadkog
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.loadData)
                ddlYear_BindData()
                ddlDSN_BindData()

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
    Dim _PackageName As String = ConfigurationSettings.AppSettings("PackageName_K12ToSQL").ToString
    Dim _LocalFileSubFolderPath As String = "/K12OG/"
    Dim _Source_File_Extantion As String = "xls"
    Dim _typeofData As String = "K12"
	Dim OleDbConnectionExtended As String = "" '" ;Extended Properties=Excel 8.0;"

    'Mapping
    Dim _schoolID As String = "Division Name"
    Dim _divisionNumber As String = "Division #"
    Dim _divisionName As String = "Division Name"
    Dim _divisionType As String = "Division Type"
    Dim _totalRecogExp As String = "Total Recog Expenditure"
    Dim _assessment As String = "Assessment"
    Dim _derivedGILAssessment As String = "Derived GIL Assessment"
    Dim _totalAssessment As String = "Total Assessment"
    Dim _EQFactor As String = "Eq# Factor"
    Dim _locationRevenue As String = "Location Revenue"
    Dim _otherRevenue As String = "Other Revenue"
    Dim _totalRevenue As String = "Total Recog Revenue"
    Dim _totalGrant As String = "Total Grant"
    Dim _grantEntitlement As String = "Grant Entitlement (after prior year adjustments)"



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
        'Do While (i <= ((DateTime.Now.Year - YearStart) _
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
        Dim constr As String = ("select " _
                    + (_typeofData + ("ID as ID, dataSetName from " _
                    + (_typeofData + "Description order by dataSetName"))))
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
        'Decimal MaxMegabyteSizeOfUploadFile =
        '    Decimal.Parse(System.Configuration.ConfigurationSettings.AppSettings["MaxMegabyteSizeOfUploadFile"].ToString()) * 1024 * 1024;
        'if (file_Correct && file_Size >= MaxMegabyteSizeOfUploadFile)
        '{
        '    file_Correct = false;
        '   //Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP311")+ file_Size.ToString() + ">" + MaxMegabyteSizeOfUploadFile.ToString()        '}
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
            'tr_Aggregate.Visible = False
            tr_DataSet.Visible = False
            tr_btnLoad.Visible = False
            ddlTableColumns_Visible(False)
        End If
    End Sub

    Protected Function ddlTableNames_BindData() As Boolean
        Dim status As Boolean = True
        'string Source_FilePath = Server.MapPath(RepositoryPath);
        Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
        Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
        Dim OleConn As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection
        Dim dtTable As DataTable
		Try

			'OleConn = New System.Data.OleDb.OleDbConnection(("Provider=Microsoft.Jet.OLEDB.4.0 ;Data Source=" _
			'								+ (Source_FilePath _
			'								+ (Source_FileName + OleDbConnectionExtended))))

			OleConn = New System.Data.OleDb.OleDbConnection(("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" _
			+ (Source_FilePath _
			+ (Source_FileName + OleDbConnectionExtended)) _
			+ ";Extended Properties=""Excel 8.0;HDR=Yes;"""))

			OleConn.Open()
			dtTable = OleConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
			For Each row As DataRow In dtTable.Select
				If Not row("Table_Name").ToString.ToLower.Contains("$") Then
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
			OleConn.Close()
		End Try
        Return status
    End Function

    Protected Sub ddlTableNames_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim status As Boolean = True
        If (ddlTableNames.SelectedIndex <> 0) Then
            'try
            '{
            '    int Year = int.Parse(ddlTableNames.SelectedValue);
            '}
            'catch
            '{
            '    status = false;
            '    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP313")
            '}
            Dim TableName As String = ddlTableNames.SelectedValue
            If ddlTableColumns_BindData(TableName) Then
                'tr_Aggregate.Visible = True
                tr_DataSet.Visible = True
                tr_newFile.Visible = False
            End If
        Else
            status = False
        End If
        If Not status Then
            'tr_Aggregate.Visible = False
            tr_DataSet.Visible = False
            tr_btnLoad.Visible = False
            ddlTableColumns_Visible(False)
        Else
            'txtNewDSN.Text = _typeofData + " Data for " + ddlYear.SelectedValue;
            btnLoad_Visible()
        End If
    End Sub

    Protected Sub ddlTableColumns_collection(ByVal dtColumnNames As DataTable, ByVal tr As HtmlTableRow, ByVal ddl As DropDownList)
        If tr.Visible Then
            ddl.DataSource = dtColumnNames
            ddl.DataTextField = dtColumnNames.Columns("Column_Name").ToString
            ddl.DataBind()
            'ddl.Items.Insert(0, new ListItem("Select Column Name", "-1"));
            ddl.Items.Insert(0, "Select Column Name")
            ddl.SelectedIndex = 0
        End If
    End Sub

    Protected Sub ddlTableColumns_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        btnLoad_Visible()
    End Sub

    Protected Sub txtNewDSN_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        btnLoad_Visible()
    End Sub

    'Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
    Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnLoad.Click
        Dim Source_Connection_Script As String = ""
        If (ddlTableNames.SelectedIndex <> 0) Then
            If SSIS_Source_Connection_Script_Creation(ddlTableNames.SelectedValue, Source_Connection_Script) Then
                If SSIS_Package_Bind("file", Source_Connection_Script) Then
                    'Copy from _SSIS table to real table
                    If SQL_Server_CopyData() Then
                        ddlDSN_BindData()
                        ddlTableNames.Items.RemoveAt(ddlTableNames.SelectedIndex)
                    End If
                End If
            End If
        Else
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP306")
        End If
    End Sub


    Protected Function SSIS_Package_Bind(ByVal TypeOfConnection As String, ByVal Source_Connection_Script As String) As Boolean
        Dim status As Boolean = False
        'lb _ Message.Text = ""
        Master.errorMsg = ""
        Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
        'Server.MapPath(RepositoryPath);
        Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
        Dim PackageName As String = (Server.MapPath(_PackagePath) + _PackageName)
        Select Case (TypeOfConnection)
            Case "file"
                If (File.Exists((PackageName + ".dtsx")) AndAlso File.Exists((PackageName + ".dtsConfig"))) Then
                    Dim myPackage As Package
                    Dim app As Application = New Application
                    myPackage = app.LoadPackage((PackageName + ".dtsx"), Nothing)
                    myPackage.ImportConfigurationFile((PackageName + ".dtsConfig"))
                    Dim vars As Variables = myPackage.Variables
                    vars("SQL_DataSource").Value = SQL_DataSource
                    vars("SQL_InitialCatalog").Value = SQL_InitialCatalog
                    vars("Source_FilePath").Value = Source_FilePath
                    vars("Source_FileName").Value = Source_FileName
                    vars("Source_Connection_Script").Value = Source_Connection_Script
                    Dim result As DTSExecResult = myPackage.Execute
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP312") + result.ToString
                    If result.ToString.ToLower.Equals("success") Then
                        status = True
                    Else
                        status = False
                        Master.errorMsg = myPackage.Errors(0).Description
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
        '                + (Source_FilePath + ("<br>Source_FileName: " _
        '                + (Source_FileName + ("<br>Source_Connection_Script: " + Source_Connection_Script))))))
        'End If
        Return status
    End Function

    '=================
    Protected Function ddlTableColumns_BindData(ByVal TableName As String) As Boolean
        Dim status As Boolean = True
        Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
        'Server.MapPath(RepositoryPath);
        Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
        Dim OleConn As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection
        Dim dtTable As DataTable
        Dim dtColumnNames As DataTable = New DataTable
        Dim column As DataColumn = New DataColumn
        column.DataType = System.Type.GetType("System.String")
        column.ColumnName = "Column_Name"
        dtColumnNames.Columns.Add(column)
		Try

			'OleConn = New System.Data.OleDb.OleDbConnection(("Provider=Microsoft.Jet.OLEDB.4.0 ;Data Source=" _
			'								+ (Source_FilePath _
			'								+ (Source_FileName + OleDbConnectionExtended))))

			OleConn = New System.Data.OleDb.OleDbConnection(("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" _
	+ (Source_FilePath _
	+ (Source_FileName + OleDbConnectionExtended)) _
	+ ";Extended Properties=""Excel 8.0;HDR=Yes;"""))

			OleConn.Open()
			dtTable = OleConn.GetSchema(System.Data.OleDb.OleDbMetaDataCollectionNames.Columns)
			For Each row As DataRow In dtTable.Rows
				If row("Table_Name").Equals(TableName) Then
					Dim row2 As DataRow
					row2 = dtColumnNames.NewRow
					row2("Column_Name") = row("Column_Name")
					dtColumnNames.Rows.Add(row2)
				End If
			Next
		Catch ex As Exception
			Master.errorMsg = ex.ToString
			status = False
		Finally
			OleConn.Close()
		End Try
        If status Then
            ddlTableColumns_Visible(True)
            For Each row As DataRow In dtColumnNames.Select
                If row("Column_Name").ToString.Equals(_divisionNumber) Then
                    ddldivisionNumber.Items.Insert(0, row("Column_Name").ToString)
                    ddldivisionNumber.SelectedIndex = 0
                    tr_ddldivisionNumber.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_divisionName) Then
                    ddldivisionName.Items.Insert(0, row("Column_Name").ToString)
                    ddldivisionName.SelectedIndex = 0
                    tr_ddldivisionName.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_divisionType) Then
                    ddldivisionType.Items.Insert(0, row("Column_Name").ToString)
                    ddldivisionType.SelectedIndex = 0
                    tr_ddldivisionType.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_totalRecogExp) Then
                    ddltotalRecogExp.Items.Insert(0, row("Column_Name").ToString)
                    ddltotalRecogExp.SelectedIndex = 0
                    tr_ddltotalRecogExp.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_assessment) Then
                    ddlassessment.Items.Insert(0, row("Column_Name").ToString)
                    ddlassessment.SelectedIndex = 0
                    tr_ddlassessment.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_derivedGILAssessment) Then
                    ddlderivedGILAssessment.Items.Insert(0, row("Column_Name").ToString)
                    ddlderivedGILAssessment.SelectedIndex = 0
                    tr_ddlderivedGILAssessment.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_totalAssessment) Then
                    ddltotalAssessment.Items.Insert(0, row("Column_Name").ToString)
                    ddltotalAssessment.SelectedIndex = 0
                    tr_ddltotalAssessment.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_EQFactor) Then
                    ddlEQFactor.Items.Insert(0, row("Column_Name").ToString)
                    ddlEQFactor.SelectedIndex = 0
                    tr_ddlEQFactor.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_locationRevenue) Then
                    ddllocationRevenue.Items.Insert(0, row("Column_Name").ToString)
                    ddllocationRevenue.SelectedIndex = 0
                    tr_ddllocationRevenue.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_otherRevenue) Then
                    ddlotherRevenue.Items.Insert(0, row("Column_Name").ToString)
                    ddlotherRevenue.SelectedIndex = 0
                    tr_ddlotherRevenue.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_totalRevenue) Then
                    ddltotalRevenue.Items.Insert(0, row("Column_Name").ToString)
                    ddltotalRevenue.SelectedIndex = 0
                    tr_ddltotalRevenue.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_totalGrant) Then
                    ddltotalGrant.Items.Insert(0, row("Column_Name").ToString)
                    ddltotalGrant.SelectedIndex = 0
                    tr_ddltotalGrant.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.Equals(_grantEntitlement) Then
                    ddlgrantEntitlement.Items.Insert(0, row("Column_Name").ToString)
                    ddlgrantEntitlement.SelectedIndex = 0
                    tr_ddlgrantEntitlement.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
            Next
            dtColumnNames.AcceptChanges()
            ddlTableColumns_collection(dtColumnNames, tr_ddldivisionNumber, ddldivisionNumber)
            ddlTableColumns_collection(dtColumnNames, tr_ddldivisionName, ddldivisionName)
            ddlTableColumns_collection(dtColumnNames, tr_ddldivisionType, ddldivisionType)
            ddlTableColumns_collection(dtColumnNames, tr_ddltotalRecogExp, ddltotalRecogExp)
            ddlTableColumns_collection(dtColumnNames, tr_ddlassessment, ddlassessment)
            ddlTableColumns_collection(dtColumnNames, tr_ddlderivedGILAssessment, ddlderivedGILAssessment)
            ddlTableColumns_collection(dtColumnNames, tr_ddltotalAssessment, ddltotalAssessment)
            ddlTableColumns_collection(dtColumnNames, tr_ddlEQFactor, ddlEQFactor)
            ddlTableColumns_collection(dtColumnNames, tr_ddllocationRevenue, ddllocationRevenue)
            ddlTableColumns_collection(dtColumnNames, tr_ddlotherRevenue, ddlotherRevenue)
            ddlTableColumns_collection(dtColumnNames, tr_ddltotalRevenue, ddltotalRevenue)
            ddlTableColumns_collection(dtColumnNames, tr_ddltotalGrant, ddltotalGrant)
            ddlTableColumns_collection(dtColumnNames, tr_ddlgrantEntitlement, ddlgrantEntitlement)
        End If
        Return status
    End Function

    Protected Sub ddlTableColumns_Visible(ByVal visible As Boolean)
        tr_ddldivisionNumber.Visible = visible
        tr_ddldivisionName.Visible = visible
        tr_ddldivisionType.Visible = visible
        tr_ddltotalRecogExp.Visible = visible
        tr_ddlassessment.Visible = visible
        tr_ddlderivedGILAssessment.Visible = visible
        tr_ddltotalAssessment.Visible = visible
        tr_ddlEQFactor.Visible = visible
        tr_ddllocationRevenue.Visible = visible
        tr_ddlotherRevenue.Visible = visible
        tr_ddltotalRevenue.Visible = visible
        tr_ddltotalGrant.Visible = visible
        tr_ddlgrantEntitlement.Visible = visible
    End Sub

    Protected Sub btnLoad_Visible()
        If (((ddlDSN.SelectedIndex <> 0) _
                    OrElse (Not txtNewDSN.Text.Equals("") _
                    AndAlso (ddlYear.SelectedIndex <> 0))) _
                    AndAlso (Not ddldivisionNumber.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddldivisionName.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddldivisionType.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddltotalRecogExp.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlassessment.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlderivedGILAssessment.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddltotalAssessment.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlEQFactor.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddllocationRevenue.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlotherRevenue.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddltotalRevenue.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddltotalGrant.SelectedValue.Equals("Select Column Name") _
                    AndAlso Not ddlgrantEntitlement.SelectedValue.Equals("Select Column Name")))))))))))))) Then
            tr_btnLoad.Visible = True
        Else
            tr_btnLoad.Visible = False
        End If
    End Sub

    Protected Function SSIS_Source_Connection_Script_Creation(ByVal TableName As String, ByRef Source_Connection_Script As String) As Boolean
        Dim status As Boolean = True
        Dim schoolID As String = ddldivisionName.SelectedValue
        Dim divisionNumber As String = ddldivisionNumber.SelectedValue
        Dim divisionName As String = ddldivisionName.SelectedValue
        Dim divisionType As String = ddldivisionType.SelectedValue
        Dim totalRecogExp As String = ddltotalRecogExp.SelectedValue
        Dim assessment As String = ddlassessment.SelectedValue
        Dim derivedGILAssessment As String = ddlderivedGILAssessment.SelectedValue
        Dim totalAssessment As String = ddltotalAssessment.SelectedValue
        Dim EQFactor As String = ddlEQFactor.SelectedValue
        Dim locationRevenue As String = ddllocationRevenue.SelectedValue
        Dim otherRevenue As String = ddlotherRevenue.SelectedValue
        Dim totalRevenue As String = ddltotalRevenue.SelectedValue
        Dim totalGrant As String = ddltotalGrant.SelectedValue
        Dim grantEntitlement As String = ddlgrantEntitlement.SelectedValue
        Source_Connection_Script = "SELECT '" + Unic + "' as SessionID" _
            + ", [" + schoolID + "] AS schoolID" _
            + ", [" + divisionNumber + "] AS divisionNumber" _
            + ", [" + divisionName + "] AS divisionName" _
            + ", [" + divisionType + "] AS divisionType" _
            + ", [" + totalRecogExp + "] AS totalRecogExp" _
            + ", [" + assessment + "] AS assessment" _
            + ", [" + derivedGILAssessment + "] AS derivedGILAssessment" _
            + ", [" + totalAssessment + "] AS totalAssessment" _
            + ", [" + EQFactor + "] AS EQFactor" _
            + ", [" + locationRevenue + "] AS locationRevenue" _
            + ", [" + otherRevenue + "] AS otherRevenue" _
            + ", [" + totalRevenue + "] AS totalRevenue" _
            + ", [" + totalGrant + "] AS totalGrant" _
            + ", [" + grantEntitlement + "] AS grantEntitlement" _
            + " FROM  [" + TableName + "]"
        Return status
    End Function

    Protected Function SQL_Server_CopyData() As Boolean
        Dim CommandText As String = ""
        Dim descriptionTableID As String = ""
        If (ddlDSN.SelectedIndex = 0) Then
            Dim dataSetName As String = ""
            dataSetName = txtNewDSN.Text
            CommandText = "INSERT INTO [" + _typeofData + "Description](" _
            + "[year],[dataSetName],[notes],[dateLoaded],[statusID])" _
            + "VALUES (" _
            + "'" + ddlYear.SelectedValue + "'" _
            + ", '" + dataSetName + "'" _
            + ", 'Hello World'" _
            + ",'" + DateTime.Now.ToString() + "'" _
            + ",'1') select @@IDENTITY"
            SqlDbAccess.RunSqlReturnIdentity(CommandText, descriptionTableID)
            CommandText = "INSERT INTO [" + _typeofData + "File](" _
                + "[" + _typeofData + "ID],[filename],[dateLoaded])" _
                + " VALUES (" _
                + "'" + descriptionTableID + "'" _
                + ",'" + Unic + "." + _Source_File_Extantion + "'" _
                + ",'" + DateTime.Now.ToString() + "'" _
                + ") select @@IDENTITY"
            Dim fileTableID As String = ""
            SqlDbAccess.RunSqlReturnIdentity(CommandText, fileTableID)
        Else
            descriptionTableID = ddlDSN.SelectedValue
            CommandText = "DELETE FROM [K12] WHERE [K12ID]=" _
                                + "'" + descriptionTableID + "'"
            SqlDbAccess.RunSql(CommandText)
        End If
        CommandText = "INSERT INTO [K12](" _
            + "[K12ID],[schoolID],[divisionNumber],[divisionName],[divisionType],[totalRecogExp],[assessment],[derivedGILAssessment],[totalAssessment],[EQFactor],[locationRevenue],[otherRevenue],[totalRevenue],[totalGrant],[grantEntitlement])" _
            + "SELECT " _
            + "'" + descriptionTableID + "',[schoolID],[divisionNumber],[divisionName],[divisionType],[totalRecogExp],[assessment],[derivedGILAssessment],[totalAssessment],[EQFactor],[locationRevenue],[otherRevenue],[totalRevenue],[totalGrant],[grantEntitlement]" _
            + "FROM [" + _typeofData + "_SSIS] where [SessionID] = '" + Unic + "'"
        SqlDbAccess.RunSql(CommandText)
        CommandText = "select count(*) as count FROM [" + _typeofData + "_SSIS] where [SessionID] = '" + Unic + "'"
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        Dim count As String = dt.Rows(0)("count").ToString
        'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
        '            + (count + ("' rows to '" _
        '            + (_typeofData + "' table")))))
        Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + count + PATMAP.common.GetErrorMessage("PATMAP316") + "'" + _typeofData + "'"
        CommandText = "delete FROM [" + _typeofData + "_SSIS] where [SessionID] = '" + Unic + "'"
        SqlDbAccess.RunSql(CommandText)
        SourceFileMove(descriptionTableID)
        Return (Integer.Parse(descriptionTableID) > 0)
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




End Class