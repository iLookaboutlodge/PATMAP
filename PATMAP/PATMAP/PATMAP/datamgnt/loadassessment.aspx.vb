Imports System.IO
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports PATMAP.common

Partial Public Class loadassessment
    Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Clears out the error message
			Master.errorMsg = ""

            Master.Response.AddHeader("X-UA-Compatible", "IE=Edge")

			'Response.Redirect("~\MyTestPage.aspx")

			If Not IsPostBack Then
				'Sets submenu to be displayed
				subMenu.setStartNode(menu.loadData)
				ddlYear_BindData()
				ddlDSN_BindData()
                Unic = DateTime.Today.Year.ToString _
                + "_" + DateTime.Today.Month.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
                + "_" + DateTime.Today.Day.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
                + "_" + Guid.NewGuid.ToString()


                'ViewFileUploadAssessmentData()
            End If
            'remarked as not needed 
            'PreLoadFlash()
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number)
		End Try
		'lb _ Message.Text = ""
		Master.errorMsg = ""
    End Sub

    'remarked as not needed
    'Protected Sub btnFileUpload_Click(sender As Object, e As ImageClickEventArgs)
    '    If (FileUpload()) Then
    '        Dim RepositoryUnicFileName As String = (_LocalFileRootPath _
    '                    + (_LocalFileSubFolderPath _
    '                    + (Unic + ("." + _Source_File_Extantion))))
    '        If FileExist(RepositoryUnicFileName) Then
    '            ddlTableNames_BindData()
    '            tr_Aggregate.Visible = False
    '            tr_DataSet.Visible = False
    '            tr_btnLoad_SSIS.Visible = False
    '            ddlTableColumns_Visible(False)
    '        End If
    '        FlashfileUpload.Visible = False
    '    End If
    'End Sub

    'Protected Function FileUpload() As Boolean
    '    Dim file_Correct As Boolean = True
    '    Dim file_ShortName As String = ""
    '    Dim file_PathName As String = ""
    '    Dim file_Extension As String = ""
    '    Dim file_ContentType As String = ""
    '    Dim file_Size As Decimal = 0
    '    Try
    '        file_ShortName = fpFile.FileName
    '        file_PathName = fpFile.PostedFile.FileName
    '        file_Extension = fpFile.FileName.Substring((fpFile.FileName.LastIndexOf(".") + 1))
    '        file_ContentType = fpFile.PostedFile.ContentType
    '        file_Size = Convert.ToDecimal(fpFile.PostedFile.ContentLength)
    '    Catch ex As Exception
    '        file_Correct = False
    '        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP301") + ex.ToString + "'"
    '    End Try
    '    If Not file_Extension.ToLower.Equals(_Source_File_Extantion) Then
    '        file_Correct = False
    '        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP302") + _Source_File_Extantion + "'"
    '    End If
    '    If file_Correct Then
    '        Unic = DateTime.Today.Year.ToString _
    '        + "_" + DateTime.Today.Month.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
    '        + "_" + DateTime.Today.Day.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
    '        + "_" + Guid.NewGuid.ToString()
    '        Dim RepositoryUnicFileName As String = (_LocalFileRootPath _
    '             + (_LocalFileSubFolderPath _
    '             + (Unic + ("." + _Source_File_Extantion))))
    '        fpFile.SaveAs(RepositoryUnicFileName)
    '        file_Correct = FileExist(RepositoryUnicFileName)
    '    End If
    '    Return file_Correct
    'End Function

    'Private Sub btnUploadAssessmentData_Command(sender As Object, e As CommandEventArgs) Handles btnUploadAssessmentData.Command
    '    Try

    '        If fileUploadAssessmentData.HasFile Then
    '            Dim fileExt As String = System.IO.Path.GetExtension(fileUploadAssessmentData.FileName)
    '            If fileExt.ToLower() <> ".mdb" Then
    '                Throw New Exception("Only 'mdb' files are allowed to upload.")
    '            End If

    '            'fileUploadGRAD.SaveAs(FullFilePath)

    '            'lblInfo.Text = "Success!"
    '            'lblInfo.Visible = True
    '        Else
    '            Throw New Exception("Please select file to upload.")
    '        End If

    '    Catch ex As Exception
    '        Master.errorMsg = ex.ToString
    '    End Try

    'End Sub

    'Public Function ViewFileUploadAssessmentData() As Boolean
    '    Try
    '        plViewFileUploadAssessmentData.Controls.Clear()

    '        Dim strBuilder As New StringBuilder()

    '        strBuilder.Append("var e = document.createElement('iframe');")
    '        strBuilder.Append("e.setAttribute('id', 'viewFileUploadGRADDocument');")
    '        strBuilder.Append("e.setAttribute('class', 'fileUploadGRADDocument');")
    '        strBuilder.Append("e.setAttribute('src', 'frmUploadGRAD.aspx?ie=" + System.DateTime.Now.ToString() + "');")
    '        strBuilder.Append("document.getElementById('" + plViewFileUploadAssessmentData.ClientID + "').innerHTML = e.outerHTML;")

    '        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ViewFileUploadAssessmentData", strBuilder.ToString(), True)

    '        upFileUploadAssessmentData.Update()

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    Dim SQL_DataSource As String = PATMAP.Global_asax.SQLEngineServer
    'System.Configuration.ConfigurationSettings.AppSettings("SQL_DataSource").ToString
    Dim SQL_InitialCatalog As String = PATMAP.Global_asax.DBName
    'System.Configuration.ConfigurationSettings.AppSettings("SQL_InitialCatalog").ToString

    Dim _LocalFileRootPath As String = ConfigurationSettings.AppSettings("LocalFileRootPath").ToString
    Dim _PackagePath As String = ConfigurationSettings.AppSettings("PackagePath").ToString
    Dim _PackageName As String = ConfigurationSettings.AppSettings("PackageName_Access2SQL").ToString
    ''TEST
    'Dim _PackageName As String = "TestPackage1"

    Dim _LocalFileSubFolderPath As String = "/Assessment/"
    Dim _Source_File_Extantion As String = "mdb"
    Dim _typeofData As String = "assessment"
    Dim OleDbConnectionExtended As String = ""

    Dim _ISCParcelNumber As String = "ISC_Parcel_Number"
    Dim _parcelID1 As String = "Provincial_Property_Number"
    Dim _parcelID2 As String = "Provincial_Property_NO"
    Dim _parcelID3 As String = "Provinciap_Property_Number"
    Dim _municipalityID_orig As String = "Municipality"
    Dim _municipalityID As String = "Municipality"
    Dim _alternate_parcelID1 As String = "Alternate_Property_Number"
    Dim _alternate_parcelID2 As String = "Alternate_Property_NO"
    Dim _LLD As String = "LLD"
    Dim _civicAddress As String = "Civic_Address"
    Dim _presentUseCodeID As String = "PU_Code"
    Dim _schoolID1 As String = "School_Division_Number"
    Dim _schoolID2 As String = "School_Division_NO"
    Dim _schoolID3 As String = "School_Division"
    Dim _taxClassID_orig As String = "Tax_Class"
    Dim _marketValue As String = "Market_Value_Assessment"
    Dim _taxable1 As String = "Taxable_Assessment"
    Dim _taxable2 As String = "Taxable_Assessement"
    Dim _otherExempt As String = "Other_Exempt"
    Dim _FGIL1 As String = "Federal_GIL"
    Dim _FGIL2 As String = "FEDERAL_GIL"
    Dim _PGIL1 As String = "Provincial_GIL"
    Dim _PGIL2 As String = "PROV_GIL"
    Dim _Section293 As String = "SECTION_293_2e"
    Dim _ByLawExemption As String = "Bylaw_Exemption"

    Protected Property Unic() As String
        Get
            If (Not (Session("UnicAssessmentFile")) Is Nothing) Then
                Return Session("UnicAssessmentFile").ToString
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            If (value = Nothing) Then
                Session("UnicAssessmentFile") = Nothing
            Else
                Session("UnicAssessmentFile") = value.ToString
            End If
        End Set
    End Property



    'protected string Unic
    '{
    '    get
    '    {
    '        if (ViewState["Unic"] != null)
    '        {
    '            return ViewState["Unic"].ToString();
    '        }
    '        else
    '        {
    '            return null;
    '        }
    '    }
    '    set { ViewState["Unic"] = value.ToString(); }
    '}
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

    'protected bool FileUpload()
    '{
    '    //ERROR - Database AccessTimeout expired.  
    '    //The timeout period elapsed prior to completion of the operation or the server is not responding
    '    //) Selected Tools -> Options
    '    //2) Expanded the "Query Execution" node
    '    //3) Clicked on "SQL Server"
    '    //4) Set value for "Execution time-out" to 0 and various numbers up to 1800
    '    //Also checked the following
    '    //1) In Object Explorer I right-clicked on the server and selected "Properties"
    '    //2) Selected the "Advanced" page
    '    //4) Set the value for "Query Wait" under "Parallelism" to various values from the default of -1 up to 1800.
    '    //I also stopped and restarted SQL after each change
    '    //Here's how I had to fix it.  In Tools -> Options then Designers -> Table and Database Designers I unchecked the Override connection string time-out value for table desinger.  This STILL didn't work... so I then turned the check on for it and up'd the transaction-timeout to 1600 seconds.  This fixed it which indicates to me that it's a bug with the Management Studio.  Hope this helps someone!  ;) 
    '    bool file_Correct = true;
    '    string file_ShortName = "";
    '    string file_PathName = "";
    '    string file_Extension = "";
    '    string file_ContentType = "";
    '    Decimal file_Size = 0;
    '    try
    '    {
    '        file_ShortName = fpFile.FileName;
    '        file_PathName = fpFile.PostedFile.FileName;
    '        file_Extension = fpFile.FileName.Substring((fpFile.FileName.LastIndexOf(".") + 1));
    '        file_ContentType = fpFile.PostedFile.ContentType;
    '        file_Size = Convert.ToDecimal(fpFile.PostedFile.ContentLength);
    '    }
    '    catch (Exception ex)
    '    {
    '        file_Correct = false;
    '        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP301") + ex.ToString + "'"
    '    }
    '    if (!file_Extension.ToLower().Equals(_Source_File_Extantion))
    '    {
    '        file_Correct = false;
    '        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP302") + _Source_File_Extantion + "'"
    '    }
    '    //Decimal MaxMegabyteSizeOfUploadFile =
    '    //    Decimal.Parse(System.Configuration.ConfigurationSettings.AppSettings["MaxMegabyteSizeOfUploadFile"].ToString()) * 1024 * 1024;
    '    //if (file_Correct && file_Size >= MaxMegabyteSizeOfUploadFile)
    '    //{
    '    //    file_Correct = false;
    '   //Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP311")+ file_Size.ToString() + ">" + MaxMegabyteSizeOfUploadFile.ToString()
    '    //}
    '    if (file_Correct)
    '    {
    'Unic = DateTime.Today.Year.ToString _
    '+ "_" + DateTime.Today.Month.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
    '+ "_" + DateTime.Today.Day.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
    '+ "_" + Guid.NewGuid.ToString()
    '        string RepositoryUnicFileName = _LocalFileRootPath + _LocalFileSubFolderPath + Unic + "."+_Source_File_Extantion;
    '        fpFile.SaveAs(RepositoryUnicFileName);
    '        file_Correct = FileExist(RepositoryUnicFileName);
    '    }
    '    return file_Correct;
    '}
    'protected void btnFileUpload_Click(object sender, ImageClickEventArgs e)
    '{
    '    if (FileUpload())
    '    {
    '        ddlTableNames_BindData();
    '        tr_Aggregate.Visible = false;
    '        tr_DataSet.Visible = false;
    '        tr_btnLoad_SSIS.Visible = false;
    '        ddlTableColumns_Visible(false);
    '    }
    '}    
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim RepositoryUnicFileName As String = (_LocalFileRootPath _
                    + (_LocalFileSubFolderPath _
                    + (Unic + ("." + _Source_File_Extantion))))

        If FileExist(RepositoryUnicFileName) Then
            ddlTableNames_BindData()
            tr_Aggregate.Visible = False
            tr_DataSet.Visible = False
            tr_btnLoad_SSIS.Visible = False
            ddlTableColumns_Visible(False)
        End If

        FlashfileUpload.Visible = False
        FlashfileUpload_p.Visible = False

        'Label2.Text = RepositoryUnicFileName
    End Sub

	Protected Sub PreLoadFlash()
		Dim jscript As String = "function UploadComplete(){"
		jscript = jscript + String.Format("__doPostBack('{0}','');", LinkButton1.ClientID.Replace("_", "$"))
		jscript = jscript + "};"
		Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "FileCompleteUpload", jscript, True)
	End Sub

    Protected Function GetFlashVars() As String
        Return ("?" + Server.UrlEncode(("FilePath=" _
                        + (_LocalFileRootPath _
                        + (_LocalFileSubFolderPath _
                        + (Unic + ("." + _Source_File_Extantion)))))))
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
			+ ";User ID=admin;"))

			OleConn.Open()
			dtTable = OleConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
			If (dtTable.Rows.Count > 0) Then
				ddlTableNames.DataSource = dtTable
				ddlTableNames.DataTextField = dtTable.Columns("Table_Name").ToString
				ddlTableNames.DataBind()
				ddlTableNames.Items.Insert(0, New ListItem("Select Table Name", "-1"))
				ddlTableNames.SelectedIndex = 0
				tr_ddlTableNames.Visible = True
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
                'tr_Aggregate.Visible = true;//Need to open for when start to use Aggregate
                tr_DataSet.Visible = True
            End If
        Else
            status = False
        End If
        If Not status Then
            tr_Aggregate.Visible = False
            tr_DataSet.Visible = False
            tr_btnLoad_SSIS.Visible = False
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

	Protected Sub btnLoad_Click_SSIS(ByVal sender As Object, ByVal e As ImageClickEventArgs)
		Dim Source_Connection_Script As String = ""
		Source_Connection_Script = Make_Source_Script()
		If (ddlTableNames.SelectedIndex <> 0) Then
			'If SSIS_Source_Connection_Script_Creation(ddlTableNames.SelectedValue, Source_Connection_Script) Then
			If RemoveFromTmpSSISTable() Then
				If GetAndInsertAccessToSQLTable() Then
					If SSIS_Package_Bind("file", Source_Connection_Script) Then
						tr_btnLoad_SSIS.Visible = False
						tr_btnLoad_SQL.Visible = True
						tr_ddlTableNames.Visible = False
						tr_DataSet.Visible = False
						Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP319")
					End If
				End If
			End If
			'End If
		Else
			Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP306")
		End If
	End Sub

    'Protected Sub btnLoad_SQL_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
    Protected Sub btnLoad_SQL_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnLoad_SQL.Click
        tr_btnLoad_SQL.Visible = False
        tr_ddlTableNames.Visible = True
        tr_DataSet.Visible = True

        'Copy from _SSIS table to real table
        'If SQL_Server_CopyData() Then
        '    ddlDSN_BindData()
        '    ddlTableNames.Items.RemoveAt(ddlTableNames.SelectedIndex)
        'End If
        'If (ddlTableNames.Items.Count <= 1) Then
        '    SourceFileDelete()
        '    Unic = Nothing
        'End If
        SQL_Server_CopyData()
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

					'Dim pkgConn As ConnectionManager = myPackage.Connections(0)
					'Dim connString As String = ""
					'Dim ApplicationName = "SSIS-" + Trim(myPackage.Name) + "-" + Trim(pkgConn.ID) + Trim(PATMAP.Global_asax.SQLEngineServer) + "." + Trim(PATMAP.Global_asax.DBName) + "." + Trim(PATMAP.Global_asax.DBUser)
					'connString = "Data Source=" + Trim(PATMAP.Global_asax.SQLEngineServer) + ";User ID=" + Trim(PATMAP.Global_asax.DBUser) + ";Password=" + Trim(PATMAP.Global_asax.DBPassword) + ";Initial Catalog=" + Trim(PATMAP.Global_asax.DBName) + ";Provider=SQLNCLI.1;Auto Translate=False;Application Name=" + Trim(ApplicationName) + ";"
					'pkgConn.ConnectionString = connString

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
			'lb _ Message.Text = (lb _ Message.Text + ("<br>Source_FilePath: " _
			'            + (Source_FilePath + ("<br>Source_FileName: " _
			'            + (Source_FileName + ("<br>Source_Connection_Script: " + Source_Connection_Script))))))
			Dim CommandText As String = ("delete FROM [" _
						+ (_typeofData + ("_SSIS] where [SessionID] = '" _
						+ (Unic + "'"))))
			SqlDbAccess.RunSql(CommandText)
		End If
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
			+ ";User ID=admin;"))

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
                If row("Column_Name").ToString.ToLower.Equals(_ISCParcelNumber.ToLower) Then
                    ddlISCParcelNumber.Items.Insert(0, row("Column_Name").ToString)
                    ddlISCParcelNumber.SelectedIndex = 0
                    tr_ddlISCParcelNumber.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If (row("Column_Name").ToString.ToLower.Equals(_parcelID1.ToLower) _
                            OrElse (row("Column_Name").ToString.ToLower.Equals(_parcelID2.ToLower) OrElse row("Column_Name").ToString.ToLower.Equals(_parcelID3.ToLower))) Then
                    ddlparcelID.Items.Insert(0, row("Column_Name").ToString)
                    ddlparcelID.SelectedIndex = 0
                    tr_ddlparcelID.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.ToLower.Equals(_municipalityID.ToLower) Then
                    ddlmunicipalityID.Items.Insert(0, row("Column_Name").ToString)
                    ddlmunicipalityID.SelectedIndex = 0
                    tr_ddlmunicipalityID.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If (row("Column_Name").ToString.ToLower.Equals(_alternate_parcelID1.ToLower) OrElse row("Column_Name").ToString.ToLower.Equals(_alternate_parcelID2.ToLower)) Then
                    ddlalternate_parcelID.Items.Insert(0, row("Column_Name").ToString)
                    ddlalternate_parcelID.SelectedIndex = 0
                    tr_ddlalternate_parcelID.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.ToLower.Equals(_LLD.ToLower) Then
                    ddlLLD.Items.Insert(0, row("Column_Name").ToString)
                    ddlLLD.SelectedIndex = 0
                    tr_ddlLLD.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.ToLower.Equals(_civicAddress.ToLower) Then
                    ddlcivicAddress.Items.Insert(0, row("Column_Name").ToString)
                    ddlcivicAddress.SelectedIndex = 0
                    tr_ddlcivicAddress.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.ToLower.Equals(_presentUseCodeID.ToLower) Then
                    ddlpresentUseCodeID.Items.Insert(0, row("Column_Name").ToString)
                    ddlpresentUseCodeID.SelectedIndex = 0
                    tr_ddlpresentUseCodeID.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If (row("Column_Name").ToString.ToLower.Equals(_schoolID1.ToLower) _
                            OrElse (row("Column_Name").ToString.ToLower.Equals(_schoolID2.ToLower) OrElse row("Column_Name").ToString.ToLower.Equals(_schoolID3.ToLower))) Then
                    ddlschoolID.Items.Insert(0, row("Column_Name").ToString)
                    ddlschoolID.SelectedIndex = 0
                    tr_ddlschoolID.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.ToLower.Equals(_taxClassID_orig.ToLower) Then
                    ddltaxClassID_orig.Items.Insert(0, row("Column_Name").ToString)
                    ddltaxClassID_orig.SelectedIndex = 0
                    tr_ddltaxClassID_orig.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.ToLower.Equals(_marketValue.ToLower) Then
                    ddlmarketValue.Items.Insert(0, row("Column_Name").ToString)
                    ddlmarketValue.SelectedIndex = 0
                    tr_ddlmarketValue.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If (row("Column_Name").ToString.ToLower.Equals(_taxable1.ToLower) OrElse row("Column_Name").ToString.ToLower.Equals(_taxable2.ToLower)) Then
                    ddltaxable.Items.Insert(0, row("Column_Name").ToString)
                    ddltaxable.SelectedIndex = 0
                    tr_ddltaxable.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.ToLower.Equals(_otherExempt.ToLower) Then
                    ddlotherExempt.Items.Insert(0, row("Column_Name").ToString)
                    ddlotherExempt.SelectedIndex = 0
                    tr_ddlotherExempt.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If (row("Column_Name").ToString.ToLower.Equals(_FGIL1.ToLower) OrElse row("Column_Name").ToString.ToLower.Equals(_FGIL2.ToLower)) Then
                    ddlFGIL.Items.Insert(0, row("Column_Name").ToString)
                    ddlFGIL.SelectedIndex = 0
                    tr_ddlFGIL.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If (row("Column_Name").ToString.ToLower.Equals(_PGIL1.ToLower) OrElse row("Column_Name").ToString.ToLower.Equals(_PGIL2.ToLower)) Then
                    ddlPGIL.Items.Insert(0, row("Column_Name").ToString)
                    ddlPGIL.SelectedIndex = 0
                    tr_ddlPGIL.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.ToLower.Equals(_Section293.ToLower) Then
                    ddlSection293.Items.Insert(0, row("Column_Name").ToString)
                    ddlSection293.SelectedIndex = 0
                    tr_ddlSection293.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
                If row("Column_Name").ToString.ToLower.Equals(_ByLawExemption.ToLower) Then
                    ddlByLawExemption.Items.Insert(0, row("Column_Name").ToString)
                    ddlByLawExemption.SelectedIndex = 0
                    tr_ddlByLawExemption.Visible = False
                    row.Delete()
                    row.BeginEdit()
                End If
            Next
            dtColumnNames.AcceptChanges()
            ddlTableColumns_collection(dtColumnNames, tr_ddlISCParcelNumber, ddlISCParcelNumber)
            ddlTableColumns_collection(dtColumnNames, tr_ddlparcelID, ddlparcelID)
            ddlTableColumns_collection(dtColumnNames, tr_ddlmunicipalityID, ddlmunicipalityID)
            ddlTableColumns_collection(dtColumnNames, tr_ddlalternate_parcelID, ddlalternate_parcelID)
            ddlTableColumns_collection(dtColumnNames, tr_ddlLLD, ddlLLD)
            ddlTableColumns_collection(dtColumnNames, tr_ddlcivicAddress, ddlcivicAddress)
            ddlTableColumns_collection(dtColumnNames, tr_ddlpresentUseCodeID, ddlpresentUseCodeID)
            ddlTableColumns_collection(dtColumnNames, tr_ddlschoolID, ddlschoolID)
            ddlTableColumns_collection(dtColumnNames, tr_ddltaxClassID_orig, ddltaxClassID_orig)
            ddlTableColumns_collection(dtColumnNames, tr_ddlmarketValue, ddlmarketValue)
            ddlTableColumns_collection(dtColumnNames, tr_ddltaxable, ddltaxable)
            ddlTableColumns_collection(dtColumnNames, tr_ddlotherExempt, ddlotherExempt)
            ddlTableColumns_collection(dtColumnNames, tr_ddlFGIL, ddlFGIL)
            ddlTableColumns_collection(dtColumnNames, tr_ddlPGIL, ddlPGIL)
            ddlTableColumns_collection(dtColumnNames, tr_ddlSection293, ddlSection293)
            ddlTableColumns_collection(dtColumnNames, tr_ddlByLawExemption, ddlByLawExemption)
        End If
        Return status
    End Function

    Protected Sub ddlTableColumns_Visible(ByVal visible As Boolean)
        tr_ddlISCParcelNumber.Visible = visible
        tr_ddlparcelID.Visible = visible
        tr_ddlmunicipalityID.Visible = visible
        tr_ddlalternate_parcelID.Visible = visible
        tr_ddlLLD.Visible = visible
        tr_ddlcivicAddress.Visible = visible
        tr_ddlpresentUseCodeID.Visible = visible
        tr_ddlschoolID.Visible = visible
        tr_ddltaxClassID_orig.Visible = visible
        tr_ddlmarketValue.Visible = visible
        tr_ddltaxable.Visible = visible
        tr_ddlotherExempt.Visible = visible
        tr_ddlFGIL.Visible = visible
        tr_ddlPGIL.Visible = visible
        tr_ddlSection293.Visible = visible
        tr_ddlByLawExemption.Visible = visible
    End Sub

    Protected Sub btnLoad_Visible()
        If (((ddlDSN.SelectedIndex <> 0) _
                    OrElse (Not txtNewDSN.Text.Equals("") _
                    AndAlso (ddlYear.SelectedIndex <> 0))) _
                    AndAlso (Not ddlISCParcelNumber.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlparcelID.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlmunicipalityID.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlalternate_parcelID.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlLLD.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlcivicAddress.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlpresentUseCodeID.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlschoolID.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddltaxClassID_orig.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlmarketValue.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddltaxable.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlotherExempt.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlFGIL.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlPGIL.SelectedValue.Equals("Select Column Name") _
                    AndAlso (Not ddlSection293.SelectedValue.Equals("Select Column Name") _
                    AndAlso Not ddlByLawExemption.SelectedValue.Equals("Select Column Name"))))))))))))))))) Then
            tr_btnLoad_SSIS.Visible = True
            tr_btnLoad_SQL.Visible = False
        Else
            tr_btnLoad_SSIS.Visible = False
            tr_btnLoad_SQL.Visible = False
        End If
    End Sub

	'Protected Function OLD_SSIS_Source_Connection_Script_Creation(ByVal TableName As String, ByRef Source_Connection_Script As String) As Boolean
	'	Dim status As Boolean = True
	'	Dim ISCParcelNumber As String = ddlISCParcelNumber.SelectedValue
	'	Dim parcelID As String = ddlparcelID.SelectedValue
	'	Dim municipalityID As String = ddlmunicipalityID.SelectedValue
	'	Dim municipalityID_orig As String = municipalityID
	'	Dim alternate_parcelID As String = ddlalternate_parcelID.SelectedValue
	'	Dim LLD As String = ddlLLD.SelectedValue
	'	Dim civicAddress As String = ddlcivicAddress.SelectedValue
	'	Dim presentUseCodeID As String = ddlpresentUseCodeID.SelectedValue
	'	Dim schoolID As String = ddlschoolID.SelectedValue
	'	Dim taxClassID_orig As String = ddltaxClassID_orig.SelectedValue
	'	Dim taxClassID As String = ddltaxClassID_orig.SelectedValue
	'	Dim marketValue As String = ddlmarketValue.SelectedValue
	'	Dim taxable As String = ddltaxable.SelectedValue
	'	Dim otherExempt As String = ddlotherExempt.SelectedValue
	'	Dim FGIL As String = ddlFGIL.SelectedValue
	'	Dim PGIL As String = ddlPGIL.SelectedValue
	'	Dim Section293 As String = ddlSection293.SelectedValue
	'	Dim ByLawExemption As String = ddlByLawExemption.SelectedValue
	'	Source_Connection_Script = "SELECT '" + Unic + "' as SessionID" _
	'		 + ", " + ISCParcelNumber + " AS ISCParcelNumber" _
	'		 + ", " + parcelID + " AS parcelID" _
	'		 + ", " + municipalityID + " AS municipalityID_orig" _
	'		 + ", " + municipalityID + " AS municipalityID" _
	'		 + ", " + alternate_parcelID + " AS alternate_parcelID" _
	'		 + ", " + LLD + " as LLD" _
	'		 + ", " + civicAddress + " AS civicAddress" _
	'		 + ", " + presentUseCodeID + " AS presentUseCodeID" _
	'		 + ", " + schoolID + " AS schoolID" _
	'		 + ", " + taxClassID_orig + " AS taxClassID_orig" _
	'		 + ", " + taxClassID + " AS taxClassID" _
	'		 + ", " + marketValue + " AS marketValue" _
	'		 + ", " + taxable + " AS taxable" _
	'		 + ", " + otherExempt + " AS otherExempt" _
	'		 + ", " + FGIL + " AS FGIL" _
	'		 + ", " + PGIL + " AS PGIL" _
	'		 + ", " + Section293 + " AS Section293" _
	'		 + ", " + ByLawExemption + " AS ByLawExemption" _
	'		 + " FROM  [" + TableName + "]"
	'	Return status
	'End Function

	Protected Function SSIS_Source_Connection_Script_Creation(ByVal TableName As String, ByRef Source_Connection_Script As String) As Boolean
		Dim status As Boolean = True
		Dim ISCParcelNumber As String = ddlISCParcelNumber.SelectedValue
		Dim parcelID As String = ddlparcelID.SelectedValue
		Dim municipalityID As String = ddlmunicipalityID.SelectedValue
		Dim municipalityID_orig As String = municipalityID
		Dim alternate_parcelID As String = ddlalternate_parcelID.SelectedValue
		Dim LLD As String = ddlLLD.SelectedValue
		Dim civicAddress As String = ddlcivicAddress.SelectedValue
		Dim presentUseCodeID As String = ddlpresentUseCodeID.SelectedValue
		Dim schoolID As String = ddlschoolID.SelectedValue
		Dim taxClassID_orig As String = ddltaxClassID_orig.SelectedValue
		Dim taxClassID As String = ddltaxClassID_orig.SelectedValue
		Dim marketValue As String = ddlmarketValue.SelectedValue
		Dim taxable As String = ddltaxable.SelectedValue
		Dim otherExempt As String = ddlotherExempt.SelectedValue
		Dim FGIL As String = ddlFGIL.SelectedValue
		Dim PGIL As String = ddlPGIL.SelectedValue
		Dim Section293 As String = ddlSection293.SelectedValue
		Dim ByLawExemption As String = ddlByLawExemption.SelectedValue

		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))
		Dim DataString As String = " OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source=" + Source_FilePath + Source_FileName + ";User ID=admin;')..."


		Source_Connection_Script = "SELECT '" + Unic + "' as SessionID" _
		 + ", " + ISCParcelNumber + " AS ISCParcelNumber" _
		 + ", " + parcelID + " AS parcelID" _
		 + ", " + municipalityID + " AS municipalityID_orig" _
		 + ", " + municipalityID + " AS municipalityID" _
		 + ", " + alternate_parcelID + " AS alternate_parcelID" _
		 + ", " + LLD + " as LLD" _
		 + ", " + civicAddress + " AS civicAddress" _
		 + ", " + presentUseCodeID + " AS presentUseCodeID" _
		 + ", " + schoolID + " AS schoolID" _
		 + ", " + taxClassID_orig + " AS taxClassID_orig" _
		 + ", " + taxClassID + " AS taxClassID" _
		 + ", " + marketValue + " AS marketValue" _
		 + ", " + taxable + " AS taxable" _
		 + ", " + otherExempt + " AS otherExempt" _
		 + ", " + FGIL + " AS FGIL" _
		 + ", " + PGIL + " AS PGIL" _
		 + ", " + Section293 + " AS Section293" _
		 + ", " + ByLawExemption + " AS ByLawExemption" _
		 + " FROM  " + DataString _
		 + "[" + TableName + "]"
		Return status
	End Function

	Protected Function SQL_Server_CopyData() As Boolean
		Dim CommandText As String = ""
		Dim descriptionTableID As String = ""
		If (ddlDSN.SelectedIndex = 0) Then
			Dim dataSetName As String = ""
			dataSetName = txtNewDSN.Text
			CommandText = "INSERT INTO [" _
			 + _typeofData + "Description]([year],[dataSetName],[notes],[dateLoaded],[statusID],[dataStale]) VALUES ('" _
			 + ddlYear.SelectedValue + "'" + ", '" _
			 + dataSetName + "'" + ", 'Hello World'" _
			 + ",getdate()" + ",'1',0) select @@IDENTITY"
			'+ DateTime.Now.ToString + "'" + ",'1',0) select @@IDENTITY"

			SqlDbAccess.RunSqlReturnIdentity(CommandText, descriptionTableID)
			CommandText = "INSERT INTO [" _
				+ _typeofData + "File]([" _
				+ _typeofData + "ID],[filename],[dateLoaded]) VALUES ('" _
				+ descriptionTableID + "'" + ",'" _
				+ Unic + "." _
				+ _Source_File_Extantion + "'" _
				+ ",getdate()" + ") select @@IDENTITY"
			'+ DateTime.Now.ToString + "'" + ") select @@IDENTITY"
			Dim fileTableID As String = ""
			SqlDbAccess.RunSqlReturnIdentity(CommandText, fileTableID)
		Else
			descriptionTableID = ddlDSN.SelectedValue
		End If
		'CommandText =
		'    "INSERT INTO [assessment]([assessmentID],[ISCParcelNumber],[parcelID],[municipalityID],[municipalityID_orig],[alternate_parcelID],[LLD],[civicAddress],[presentUseCodeID],[schoolID],[taxClassID_orig],[taxClassID],[marketValue],[taxable],[otherExempt],[FGIL],[PGIL],[Section293],[ByLawExemption])"
		'    + " SELECT "
		'    + "'" + descriptionTableID + "',[ISCParcelNumber],[parcelID],[municipalityID],[municipalityID_orig],[alternate_parcelID],[LLD],[civicAddress],[presentUseCodeID],[schoolID],[taxClassID_orig],[taxClassID],[marketValue],[taxable],[otherExempt],[FGIL],[PGIL],[Section293],[ByLawExemption]"
		'    + " FROM ["+_typeofData+"_SSIS] where [SessionID] = '" + Unic + "'";
		'SqlDbAccess.RunSql(CommandText);
		Dim stat As Integer = 0
		Try
			stat = Assessmet_InsertDataFromSSIStable(Integer.Parse(descriptionTableID), Unic)
			If (stat = 1) Then
				CommandText = "select count(*) as count FROM [" + _typeofData + "_SSIS] where [SessionID] = '" + Unic + "'"
				Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
				Dim count As String = dt.Rows(0)("count").ToString
				'lb _ Message.Text = (lb _ Message.Text + ("<br>Successfully transformed '" _
				'            + (count + ("' rows to '" _
				'            + (_typeofData + "' table")))))
				Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP315") + count + PATMAP.common.GetErrorMessage("PATMAP316") + "'" + _typeofData + "'"
			Else
				Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP317") + "'assessment'" + PATMAP.common.GetErrorMessage("PATMAP318") + "'assessment_SSIS'"
			End If
		Catch ex As Exception
			Master.errorMsg = Master.errorMsg + PATMAP.common.GetErrorMessage("PATMAP317") + "'assessment'" + PATMAP.common.GetErrorMessage("PATMAP318") + "'assessment_SSIS'"
		End Try
		CommandText = "delete FROM [" + _typeofData + "_SSIS] where [SessionID] = '" + Unic + "'"
		SqlDbAccess.RunSql(CommandText)

		Dim bStat As Boolean = False
		If (Integer.Parse(descriptionTableID) > 0) AndAlso (stat = 1) Then
			bStat = True
		End If

		If bStat Then
			ddlDSN_BindData()
			ddlTableNames.Items.RemoveAt(ddlTableNames.SelectedIndex)
		End If

		If (ddlTableNames.Items.Count <= 1) Then
			SourceFileMove(descriptionTableID)
			Unic = Nothing
		Else
			SourceFileCopy(descriptionTableID)
		End If


		Return bStat
	End Function

    Protected Function Assessmet_InsertDataFromSSIStable(ByVal descriptionTableID As Integer, ByVal SessionID As String) As Integer
        Dim Status As Integer = 0
        Dim spName As String = "Assessment_InsertDataFromSSIStable"
        Dim sqlParams() As SqlParameter = New SqlParameter() {New SqlParameter("@descriptionTableID", SqlDbType.Int), New SqlParameter("@SessionID", SqlDbType.VarChar, 50), New SqlParameter("@ReturnVal", SqlDbType.Int)}
        'assign values to the store proc params            
        sqlParams(0).Value = descriptionTableID
        sqlParams(1).Value = SessionID
        sqlParams(2).Direction = ParameterDirection.Output
        Try
            Status = SqlDbAccess.RunSp(spName, sqlParams)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return Status
    End Function

    Protected Sub SourceFileCopy(ByVal ID As String)
        Dim Source_FilePath As String = _LocalFileRootPath + _LocalFileSubFolderPath
        Dim Source_FileName As String = Unic + "." + _Source_File_Extantion
        Dim Destin_FilePath As String = Source_FilePath + ID
        If Not Directory.Exists(Destin_FilePath) Then
            Directory.CreateDirectory(Destin_FilePath)
        End If
        If (File.Exists(Source_FilePath + Source_FileName) AndAlso Directory.Exists(Destin_FilePath) AndAlso Not File.Exists(Destin_FilePath + "/" + Source_FileName)) Then
            File.Copy(Source_FilePath + Source_FileName, Destin_FilePath + "/" + Source_FileName)
        End If
    End Sub
    Protected Sub SourceFileMove(ByVal ID As String)
        Dim Source_FilePath As String = _LocalFileRootPath + _LocalFileSubFolderPath
        Dim Source_FileName As String = Unic + "." + _Source_File_Extantion
        Dim Destin_FilePath As String = Source_FilePath + ID
        If Not Directory.Exists(Destin_FilePath) Then
            Directory.CreateDirectory(Destin_FilePath)
        End If
        If (File.Exists(Source_FilePath + Source_FileName) AndAlso Directory.Exists(Destin_FilePath) AndAlso Not File.Exists(Destin_FilePath + "/" + Source_FileName)) Then
            File.Move(Source_FilePath + Source_FileName, Destin_FilePath + "/" + Source_FileName)
        End If
        If (File.Exists(Source_FilePath + Source_FileName)) Then
            File.Delete(Source_FilePath + Source_FileName)
        End If

    End Sub

	'Public Function GetAndInsertAccessToSQLTable() As Boolean
	'	Dim OleConn As New OleDbConnection
	'	Dim OleCmd As New OleDbCommand
	'	Dim OleDr As OleDbDataReader = Nothing

	'	Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
	'	Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))

	'	If Not IO.File.Exists(Source_FilePath + Source_FileName) Then
	'		Master.errorMsg = "File Not Found."
	'		Return False
	'	End If

	'	'open sqlserver connection
	'	Dim con As New SqlConnection
	'	con.ConnectionString = PATMAP.Global_asax.connString
	'	con.Open()

	'	Dim constr As String = ""
	'	constr += "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
	'	constr += Source_FilePath
	'	constr += Source_FileName + OleDbConnectionExtended
	'	constr += ";User ID=admin;"

	'	OleConn = New System.Data.OleDb.OleDbConnection(constr)

	'	OleConn.Open()

	'	Try
	'		'Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, strMsg)
	'		'con.Open()
	'		'Impersonate.undoImpersonation()

	'		With OleCmd
	'			.Connection = OleConn
	'			.CommandType = CommandType.Text
	'			.CommandText = OLEDB_TableScript(Trim(ddlTableNames.SelectedValue))
	'		End With

	'		Dim status As Boolean = False
	'		OleDr = OleCmd.ExecuteReader()
	'		While OleDr.Read()
	'			status = True
	'			If Not INSERTTOPATMAPTable(OleDr, con) Then
	'				status = False
	'				Exit While
	'			End If
	'		End While
	'		Return status
	'	Catch ex As Exception
	'		'Throw New Exception(ex.Message)
	'		Master.errorMsg = ex.Message
	'	Finally
	'		If OleConn.State = ConnectionState.Open Then
	'			OleConn.Close()
	'		End If
	'		If con.State = ConnectionState.Open Then
	'			con.Close()
	'		End If
	'		If OleDr IsNot Nothing AndAlso (Not OleDr.IsClosed) Then
	'			OleDr.Close()
	'		End If
	'	End Try
	'	Return False
	'End Function

	Public Function GetAndInsertAccessToSQLTable() As Boolean
		Dim OleConn As New OleDbConnection
		Dim OleCmd As New OleDbCommand
		Dim OleDr As OleDbDataReader = Nothing

		Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
		Dim Source_FileName As String = (Unic + ("." + _Source_File_Extantion))

		If Not IO.File.Exists(Source_FilePath + Source_FileName) Then
			Master.errorMsg = "File Not Found."
			Return False
		End If

		''open sqlserver connection
		'Dim con As New SqlConnection
		'con.ConnectionString = PATMAP.Global_asax.connString
		'con.Open()

		Dim constr As String = ""
		constr += "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
		constr += Source_FilePath
		constr += Source_FileName + OleDbConnectionExtended
		constr += ";User ID=admin;"

		OleConn = New System.Data.OleDb.OleDbConnection(constr)

		OleConn.Open()

		Try
			'Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, strMsg)
			'con.Open()
			'Impersonate.undoImpersonation()

			With OleCmd
				.Connection = OleConn
				.CommandType = CommandType.Text
				.CommandText = OLEDB_TableScript(Trim(ddlTableNames.SelectedValue))
			End With

			Dim status As Boolean = False
			OleDr = OleCmd.ExecuteReader()

			Using BulkCopyToSql As New SqlBulkCopy(PATMAP.Global_asax.connString)
				BulkCopyToSql.DestinationTableName = "dbo.[assessment_SSIS_TMP]"
				BulkCopyToSql.BulkCopyTimeout = 60000

				'Bulk copy column mappings 
				Dim MapCol As SqlBulkCopyColumnMapping = Nothing

				MapCol = New SqlBulkCopyColumnMapping("SessionID", "SessionID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("ISCParcelNumber", "ISCParcelNumber")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("parcelID", "parcelID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("municipalityID_orig", "municipalityID_orig")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("municipalityID", "municipalityID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("alternate_parcelID", "alternate_parcelID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("LLD", "LLD")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("civicAddress", "civicAddress")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("presentUseCodeID", "presentUseCodeID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("schoolID", "schoolID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("taxClassID_orig", "taxClassID_orig")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("taxClassID", "taxClassID")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("marketValue", "marketValue")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("taxable", "taxable")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("otherExempt", "otherExempt")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("FGIL", "FGIL")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("PGIL", "PGIL")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("Section293", "Section293")
				BulkCopyToSql.ColumnMappings.Add(MapCol)
				MapCol = New SqlBulkCopyColumnMapping("ByLawExemption", "ByLawExemption")
				BulkCopyToSql.ColumnMappings.Add(MapCol)

				BulkCopyToSql.WriteToServer(OleDr)

				status = True
			End Using

			'While OleDr.Read()
			'	status = True
			'	If Not INSERTTOPATMAPTable(OleDr, con) Then
			'		status = False
			'		Exit While
			'	End If
			'End While
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

	Protected Function OLEDB_TableScript(ByVal TableName As String) As String
		Dim ISCParcelNumber As String = ddlISCParcelNumber.SelectedValue
		Dim parcelID As String = ddlparcelID.SelectedValue
		Dim municipalityID As String = ddlmunicipalityID.SelectedValue
		Dim municipalityID_orig As String = municipalityID
		Dim alternate_parcelID As String = ddlalternate_parcelID.SelectedValue
		Dim LLD As String = ddlLLD.SelectedValue
		Dim civicAddress As String = ddlcivicAddress.SelectedValue
		Dim presentUseCodeID As String = ddlpresentUseCodeID.SelectedValue
		Dim schoolID As String = ddlschoolID.SelectedValue
		Dim taxClassID_orig As String = ddltaxClassID_orig.SelectedValue
		Dim taxClassID As String = ddltaxClassID_orig.SelectedValue
		Dim marketValue As String = ddlmarketValue.SelectedValue
		Dim taxable As String = ddltaxable.SelectedValue
		Dim otherExempt As String = ddlotherExempt.SelectedValue
		Dim FGIL As String = ddlFGIL.SelectedValue
		Dim PGIL As String = ddlPGIL.SelectedValue
		Dim Section293 As String = ddlSection293.SelectedValue
		Dim ByLawExemption As String = ddlByLawExemption.SelectedValue

		Dim table_script As String = ""

		table_script = "SELECT '" + Unic + "' as SessionID" _
	+ ", " + ISCParcelNumber + " AS ISCParcelNumber" _
	+ ", " + parcelID + " AS parcelID" _
	+ ", " + municipalityID + " AS municipalityID_orig" _
	+ ", " + municipalityID + " AS municipalityID" _
	+ ", " + alternate_parcelID + " AS alternate_parcelID" _
	+ ", " + LLD + " as LLD" _
	+ ", " + civicAddress + " AS civicAddress" _
	+ ", " + presentUseCodeID + " AS presentUseCodeID" _
	+ ", " + schoolID + " AS schoolID" _
	+ ", " + taxClassID_orig + " AS taxClassID_orig" _
	+ ", " + taxClassID + " AS taxClassID" _
	+ ", " + marketValue + " AS marketValue" _
	+ ", " + taxable + " AS taxable" _
	+ ", " + otherExempt + " AS otherExempt" _
	+ ", " + FGIL + " AS FGIL" _
	+ ", " + PGIL + " AS PGIL" _
	+ ", " + Section293 + " AS Section293" _
	+ ", " + ByLawExemption + " AS ByLawExemption" _
	+ " FROM  " _
	+ "[" + TableName + "]"

		Return table_script
	End Function

	'Public Function CreateDropPATMAPTable() As Boolean
	'	Dim con As New SqlConnection
	'	con.ConnectionString = PATMAP.Global_asax.connString

	'	Dim cmd As New SqlCommand
	'	Dim query_str As String = ""

	'	query_str += "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Assessment_SSIS_TMP]') AND type in (N'U')) " + _
	'	" DROP TABLE [dbo].[Assessment_SSIS_TMP]" + _
	'	" CREATE TABLE [dbo].[assessment_SSIS_TMP](" + _
	'	"	[SessionID] [varchar](255) NULL," + _
	'	"	[ISCParcelNumber] [float] NULL," + _
	'	"	[parcelID] [float] NULL," + _
	'	"	[municipalityID_orig] [nvarchar](255) NULL," + _
	'	"	[municipalityID] [nvarchar](255) NULL," + _
	'	"	[alternate_parcelID] [float] NULL," + _
	'	"	[LLD] [nvarchar](250) NULL," + _
	'	"	[civicAddress] [nvarchar](250) NULL," + _
	'	"	[presentUseCodeID] [float] NULL," + _
	'	"	[schoolID] [nvarchar](255) NULL," + _
	'	"	[taxClassID_orig] [nvarchar](255) NULL," + _
	'	"	[taxClassID] [nvarchar](255) NULL," + _
	'	"	[marketValue] [float] NULL," + _
	'	"	[taxable] [float] NULL," + _
	'	"	[otherExempt] [float] NULL," + _
	'	"	[FGIL] [float] NULL," + _
	'	"	[PGIL] [float] NULL," + _
	'	"	[Section293] [float] NULL," + _
	'	"	[ByLawExemption] [float] NULL" + _
	'	" ) ON [PRIMARY] "

	'	con.Open()

	'	Try
	'		With cmd
	'			.Connection = con
	'			.CommandType = CommandType.Text
	'			.CommandText = query_str
	'			.ExecuteNonQuery()
	'		End With
	'	Catch ex As Exception
	'		Throw New Exception(ex.Message)
	'	Finally
	'		con.Close()
	'	End Try
	'	Return True
	'End Function

	Public Function RemoveFromTmpSSISTable() As Boolean
		Dim con As New SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim cmd As New SqlCommand
		Dim query_str As New StringBuilder
		query_str.Append("DELETE FROM [assessment_SSIS_TMP]")

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
		Finally
			con.Close()
		End Try
		Return True
	End Function


	'Public Function INSERTTOPATMAPTable(ByVal OleDr As OleDbDataReader, ByVal con As SqlConnection) As Boolean
	'	Dim cmd As New SqlCommand
	'	Dim query_str As New StringBuilder

	'	Try

	'		'query_str.AppendFormat("'{0}',", SessionID)
	'		'query_str.AppendFormat("{0},", ISCParcelNumber)
	'		'query_str.AppendFormat("{0},", parcelID)
	'		'query_str.AppendFormat("'{0}',", municipalityID_orig)
	'		'query_str.AppendFormat("'{0}',", municipalityID)
	'		'query_str.AppendFormat("{0},", alternate_parcelID)
	'		'query_str.AppendFormat("'{0}'", LLD)
	'		'query_str.Append(" )")

	'		query_str.Append(" INSERT INTO assessment_SSIS_TMP ( ")
	'		query_str.Append(" [SessionID],[ISCParcelNumber],[parcelID],[municipalityID_orig],[municipalityID],[alternate_parcelID],[LLD],")
	'		query_str.Append(" [civicAddress],[presentUseCodeID],[schoolID],[taxClassID_orig],[taxClassID],[marketValue],[taxable],[otherExempt],")
	'		query_str.Append(" [FGIL],[PGIL],[Section293],[ByLawExemption]")
	'		query_str.Append(" ) VALUES ( ")
	'		query_str.Append(" @SessionID,@ISCParcelNumber,@parcelID,@municipalityID_orig,@municipalityID,@alternate_parcelID,@LLD,")
	'		query_str.Append(" @civicAddress,@presentUseCodeID,@schoolID,@taxClassID_orig,@taxClassID,@marketValue,@taxable,@otherExempt,")
	'		query_str.Append(" @FGIL,@PGIL,@Section293,@ByLawExemption")
	'		query_str.Append(" )")

	'		With cmd
	'			.Connection = con
	'			.CommandType = CommandType.Text
	'			.CommandText = query_str.ToString()
	'			.Parameters.AddWithValue("@SessionID", NullToStr(OleDr.Item("SessionID")))
	'			.Parameters.AddWithValue("@ISCParcelNumber", ToDouble(OleDr.Item("ISCParcelNumber")))
	'			.Parameters.AddWithValue("@parcelID", ToDouble(OleDr.Item("parcelID")))
	'			.Parameters.AddWithValue("@municipalityID_orig", NullToStr(OleDr.Item("municipalityID_orig")))
	'			.Parameters.AddWithValue("@municipalityID", NullToStr(OleDr.Item("municipalityID")))
	'			.Parameters.AddWithValue("@alternate_parcelID", ToDouble(OleDr.Item("alternate_parcelID")))
	'			.Parameters.AddWithValue("@LLD", NullToStr(OleDr.Item("LLD")))
	'			.Parameters.AddWithValue("@civicAddress", NullToStr(OleDr.Item("civicAddress")))
	'			.Parameters.AddWithValue("@presentUseCodeID", ToDouble(OleDr.Item("presentUseCodeID")))
	'			.Parameters.AddWithValue("@schoolID", NullToStr(OleDr.Item("schoolID")))
	'			.Parameters.AddWithValue("@taxClassID_orig", NullToStr(OleDr.Item("taxClassID_orig")))
	'			.Parameters.AddWithValue("@taxClassID", NullToStr(OleDr.Item("taxClassID")))
	'			.Parameters.AddWithValue("@marketValue", ToDouble(OleDr.Item("marketValue")))
	'			.Parameters.AddWithValue("@taxable", ToDouble(OleDr.Item("taxable")))
	'			.Parameters.AddWithValue("@otherExempt", ToDouble(OleDr.Item("otherExempt")))
	'			.Parameters.AddWithValue("@FGIL", ToDouble(OleDr.Item("FGIL")))
	'			.Parameters.AddWithValue("@PGIL", ToDouble(OleDr.Item("PGIL")))
	'			.Parameters.AddWithValue("@Section293", ToDouble(OleDr.Item("Section293")))
	'			.Parameters.AddWithValue("@ByLawExemption", ToDouble(OleDr.Item("ByLawExemption")))

	'			.ExecuteNonQuery()
	'		End With

	'	Catch ex As Exception
	'		'Throw New Exception(ex.Message)
	'		Master.errorMsg = ex.Message
	'	End Try
	'	Return True
	'End Function

	Protected Function Make_Source_Script() As String
		Dim Source_Connection_Script As New StringBuilder
		Source_Connection_Script.Append(" SELECT")
		Source_Connection_Script.Append(" SessionID,ISCParcelNumber,parcelID,municipalityID_orig,municipalityID,alternate_parcelID,LLD,civicAddress,presentUseCodeID,")
		Source_Connection_Script.Append(" schoolID,taxClassID_orig,taxClassID,marketValue,taxable,otherExempt,FGIL,PGIL,Section293,ByLawExemption")
		Source_Connection_Script.Append(" FROM [assessment_SSIS_TMP]")
		Return Source_Connection_Script.ToString()
	End Function


End Class

















