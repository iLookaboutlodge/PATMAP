using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Data.SqlClient;
using System.Text;

namespace PATMAPCGIS.loadGisData
{
    public partial class loadGISData : System.Web.UI.Page
    {
        private string folderPath = ConfigurationManager.AppSettings["DBServerShapefilePath"];
        private SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                Response.Redirect("~/index.aspx");
            }
            else
            {
                string errorCode = string.Empty;

                try
                {
                    string url;
                    url = this.Request.Url.AbsolutePath;
                    errorCode = PATMAP.common.HasAccess(int.Parse((string)Session["levelID"]), url);
                }
                catch (Exception ex)
                {
                    //Displays error message and code
                    errorCode = ex.Message;
                }

                if (errorCode != "")
                {
                    Session["responseCode"] = errorCode;
                    Response.Redirect("~/error.aspx");
                }
            }

            if (folderPath.EndsWith("\\") || folderPath.EndsWith("/"))
            {
                folderPath = folderPath.Substring(0, folderPath.Length - 1);
            }
            this.lblFileLocation.Text = folderPath;            

            this.importMessage.Text = ThreadResults.getMsgs() + Environment.NewLine + "Finished in: " + ThreadResults.getRuntime().ToString();
            ThreadResults.clear();
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            if (ThreadResults.isComplete())
            {
                ThreadResults.setComplete(false, TimeSpan.Zero);

                string filePath = String.Empty;
                ParameterizedThreadStart startThread = null;

                switch (this.ddlImportType.SelectedIndex)
                {
                    case 0:
                        filePath = folderPath + "\\Municipalities.shp";
                        startThread = new ParameterizedThreadStart(importMunicipalities);
                        break;
                    case 1:
                        filePath = folderPath + "\\School_Divisions.shp";
                        startThread = new ParameterizedThreadStart(importSchoolDivisions);
                        break;
                    case 2:
                        filePath = folderPath + "\\BaseLayer.shp";
                        startThread = new ParameterizedThreadStart(importAssessmentParcels);
                        break;
                    case 3:
                        filePath = folderPath + "\\Constituency_Boundaries.shp";
                        startThread = new ParameterizedThreadStart(importConstituencyBoundaries);
                        break;
                    case 4:
                        filePath = folderPath + "\\ParcelToAssessment.csv";
                        startThread = new ParameterizedThreadStart(importParcelLinkingTable);
                        break;
                }

                try
                {
                    if (startThread != null)
                    {
                        //starts process in a thread (i.e. backend process)
                        Thread importProcess = new Thread(startThread);
                        importProcess.Start(filePath);
                        Response.Redirect("Results.aspx?refreshRate=3&redirect=loadGISData.aspx");
                    }
                    else
                    {
                        ThreadResults.setComplete(true, TimeSpan.Zero);
                    }
                }
                catch (FormatException ex)
                {
                    importMessage.Text = "Invalid Format for file path! " + ex;
                }
            }
            else
            {
                importMessage.Text = "Import process already running";
            }
        }

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Page.Response.Redirect("/datamgnt/loadassessment.aspx");
		}

		private void importMunicipalities(object filePathObj)
		{
			DateTime start = DateTime.Now;
			//takes in file paths as one object, then split them up into two strings
			string filePath = (string)filePathObj;

			if (filePath is string)
			{
				doImportMunicipalities(filePath);
			}
			else
			{
				ThreadResults.addMsg("Error importing Municipalities! Input path is not a string!");
			}
			ThreadResults.setComplete(true, DateTime.Now.Subtract(start));
		}

        private void doImportMunicipalities(string filePath)
        {
            ThreadResults.addMsg("Importing Municipalities");

            SqlConnection conn = this.connection;

            StringBuilder importStatement = buildImportSQL("Municipalities", "MunID");

            //creates municipalities changes table
            importStatement.Append(" IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MunicipalitiesChanges]') AND type in (N'U')) ");
            importStatement.Append("DROP TABLE [dbo].[MunicipalitiesChanges] ");
            importStatement.Append("CREATE TABLE [dbo].[MunicipalitiesChanges]( ");
            importStatement.Append("[MunID] [varchar](50) NULL, ");
            importStatement.Append("[UserID] [int] NULL) ");
            importStatement.Append("EXEC ST.AddGeometryColumn '', 'MunicipalitiesChanges', 'the_geom', 26913 ");

            conn.Open();

            SqlTransaction transaction = conn.BeginTransaction();
            SqlCommand cmd = new SqlCommand(importStatement.ToString(), conn, transaction);
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = filePath;
            cmd.CommandTimeout = 0; //Never timeout

            //reports results for thread
            try
            {
                int num = cmd.ExecuteNonQuery();
                ThreadResults.addMsg("Finished importing Municipalities");
                ThreadResults.addMsg(num.ToString() + " municipalities imported");
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ThreadResults.addMsg("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        //private void TEST_doImportMunicipalities(string filePath)
        //{
        //    ThreadResults.addMsg("Importing Municipalities");

        //    SqlConnection conn = this.connection;

        //    StringBuilder importStatement = buildImportSQL("MunicipalitiesTEST", "MunID");

        //    //creates municipalities changes table
        //    importStatement.Append(" IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MunicipalitiesChangesTEST]') AND type in (N'U')) ");
        //    importStatement.Append("DROP TABLE [dbo].[MunicipalitiesChangesTEST] ");
        //    importStatement.Append("CREATE TABLE [dbo].[MunicipalitiesChangesTEST]( ");
        //    importStatement.Append("[MunID] [varchar](50) NULL, ");
        //    importStatement.Append("[UserID] [int] NULL) ");
        //    importStatement.Append("EXEC ST.AddGeometryColumn '', 'MunicipalitiesChangesTEST', 'the_geom', 26913 ");

        //    conn.Open();

        //    SqlTransaction transaction = conn.BeginTransaction();
        //    SqlCommand cmd = new SqlCommand(importStatement.ToString(), conn, transaction);
        //    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = filePath;
        //    cmd.CommandTimeout = 0; //Never timeout

        //    //reports results for thread
        //    try
        //    {
        //        int num = cmd.ExecuteNonQuery();
        //        ThreadResults.addMsg("Finished importing Municipalities");
        //        ThreadResults.addMsg(num.ToString() + " municipalities imported");
        //        transaction.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        ThreadResults.addMsg("Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        private void importSchoolDivisions(object filePath)
        {
            DateTime start = DateTime.Now;
            if (filePath is string)
            {
                doImportSchoolDivisions((string)filePath);
            }
            else
            {
                ThreadResults.addMsg("Error importing School Divisions! Input path is not a string!");
            }
            ThreadResults.setComplete(true, DateTime.Now.Subtract(start));
        }

        protected void doImportSchoolDivisions(string filePath)
        {
            ThreadResults.addMsg("Importing school divisions");
            string tableName = "SchoolDivisions";
            string colName = "SD_NUM";

            SqlConnection conn = this.connection;

            StringBuilder importStatement = buildImportSQL(tableName, colName);

            conn.Open();

            SqlTransaction transaction = conn.BeginTransaction();
            SqlCommand cmd = new SqlCommand(importStatement.ToString(), conn, transaction);
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = filePath;
            cmd.CommandTimeout = 0; //Never timeout

            try
            {
                int numSchools = cmd.ExecuteNonQuery();
                transaction.Commit();
                ThreadResults.addMsg("Finished importing school divisions");
                ThreadResults.addMsg(numSchools.ToString() + " school divisions imported");
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                ThreadResults.addMsg(ex.ToString());
            }
            conn.Close();
        }

        //protected void TEST_doImportSchoolDivisions(string filePath)
        //{
        //    ThreadResults.addMsg("Importing school divisions");
        //    string tableName = "SchoolDivisionsTEST";
        //    string colName = "SD_NUM";

        //    SqlConnection conn = this.connection;

        //    StringBuilder importStatement = buildImportSQL(tableName, colName);

        //    conn.Open();

        //    SqlTransaction transaction = conn.BeginTransaction();
        //    SqlCommand cmd = new SqlCommand(importStatement.ToString(), conn, transaction);
        //    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = filePath;
        //    cmd.CommandTimeout = 0; //Never timeout

        //    try
        //    {
        //        int numSchools = cmd.ExecuteNonQuery();
        //        transaction.Commit();
        //        ThreadResults.addMsg("Finished importing school divisions");
        //        ThreadResults.addMsg(numSchools.ToString() + " school divisions imported");
        //    }
        //    catch (SqlException ex)
        //    {
        //        transaction.Rollback();
        //        ThreadResults.addMsg(ex.ToString());
        //    }
        //    conn.Close();
        //}

   		private void importAssessmentParcels(object filePath)
        {
            DateTime start = DateTime.Now;
            if (filePath is string)
            {
                doImportAssessmentParcels((string)filePath);
            }
            else
            {
                ThreadResults.addMsg("Error importing Assessment Parcels! Input path is not a string!");
            }
            ThreadResults.setComplete(true, DateTime.Now.Subtract(start));
        }

        private void doImportAssessmentParcels(string filePath)
        {
            ThreadResults.addMsg("Importing Assessment Parcels");

            SqlConnection conn = this.connection;

            StringBuilder importBaselayer = buildImportSQL("AssessmentParcels", "P_ID");

            conn.Open();

            //SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.);
            SqlCommand cmd = new SqlCommand(importBaselayer.ToString(), conn);
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = filePath;
            cmd.CommandTimeout = 0; //Never timeout

            try
            {
                int numParcels = cmd.ExecuteNonQuery();
                //transaction.Commit();
                ThreadResults.addMsg("Finished importing Assessment Parcels");
                ThreadResults.addMsg(numParcels.ToString() + " parcels imported");
            }
            catch (SqlException ex)
            {
                //transaction.Rollback();
                ThreadResults.addMsg(ex.ToString());
            }
            conn.Close();
        }

        //private void TEST_doImportAssessmentParcels(string filePath)
        //{
        //    ThreadResults.addMsg("Importing Assessment Parcels");

        //    SqlConnection conn = this.connection;

        //    StringBuilder importBaselayer = buildImportSQL("AssessmentParcelsTEST", "P_ID");

        //    conn.Open();

        //    //SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.);
        //    SqlCommand cmd = new SqlCommand(importBaselayer.ToString(), conn);
        //    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = filePath;
        //    cmd.CommandTimeout = 0; //Never timeout

        //    try
        //    {
        //        int numParcels = cmd.ExecuteNonQuery();
        //        //transaction.Commit();
        //        ThreadResults.addMsg("Finished importing Assessment Parcels");
        //        ThreadResults.addMsg(numParcels.ToString() + " parcels imported");
        //    }
        //    catch (SqlException ex)
        //    {
        //        //transaction.Rollback();
        //        ThreadResults.addMsg(ex.ToString());
        //    }
        //    conn.Close();
        //}

        private void importConstituencyBoundaries(object filePath)
        {
            DateTime start = DateTime.Now;
            if (filePath is string)
            {
                doImportConstituencyBoundaries((string)filePath);
            }
            else
            {
                ThreadResults.addMsg("Error importing Assessment Parcels! Input path is not a string!");
            }
            ThreadResults.setComplete(true, DateTime.Now.Subtract(start));
        }

        private void doImportConstituencyBoundaries(string filePath)
        {
            ThreadResults.addMsg("Importing Constituency Boundaries");

            SqlConnection conn = this.connection;

            string tableName = "ConstituencyBoundaries";

            //import shapefile
            StringBuilder importStatement = new StringBuilder();

            importStatement.Append("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'new");
            importStatement.Append(tableName);
            importStatement.Append("') ");
            importStatement.Append("BEGIN DROP TABLE new");
            importStatement.Append(tableName);
            importStatement.Append(" END ");

            importStatement.Append("EXEC ST.ImportFromShapefile @filename, '', 'new");
            importStatement.Append(tableName);
            importStatement.Append("', 'the_geom', 26913 ");

            importStatement.Append("IF EXISTS (");
            importStatement.Append("SELECT * FROM INFORMATION_SCHEMA.COLUMNS ");
            importStatement.Append("WHERE TABLE_NAME = 'new");
            importStatement.Append(tableName);
            importStatement.Append("' AND COLUMN_NAME = '");
            importStatement.Append("CON_NAME");
            importStatement.Append("' ");
            importStatement.Append(")");
            importStatement.Append(" AND EXISTS (");
            importStatement.Append("SELECT * FROM INFORMATION_SCHEMA.COLUMNS ");
            importStatement.Append("WHERE TABLE_NAME = 'new");
            importStatement.Append(tableName);
            importStatement.Append("' AND COLUMN_NAME = '");
            importStatement.Append("CON_NUM");
            importStatement.Append("' ");
            importStatement.Append(") BEGIN ");
            importStatement.Append("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '");
            importStatement.Append(tableName);
            importStatement.Append("') ");
            importStatement.Append("BEGIN DROP TABLE ");
            importStatement.Append(tableName);
            importStatement.Append(" END ");
            importStatement.Append("CREATE TABLE ");
            importStatement.Append(tableName);
            importStatement.Append(" (");
            importStatement.Append("CON_NUM");
            importStatement.Append(" float,");
            importStatement.Append("CON_NAME");
            importStatement.Append(" varchar(4000)");
            importStatement.Append(") ");
            importStatement.Append("EXEC ST.AddGeometryColumn '', '");
            importStatement.Append(tableName);
            importStatement.Append("', 'the_geom', 26913 ");
            importStatement.Append("INSERT INTO ");
            importStatement.Append(tableName);
            importStatement.Append(" (");
            importStatement.Append("CON_NUM, CON_NAME");
            importStatement.Append(", the_geom) ");
            importStatement.Append("SELECT ");
            importStatement.Append("CON_NUM, CON_NAME");
            importStatement.Append(", the_geom FROM new");
            importStatement.Append(tableName);
            importStatement.Append(" ");
            importStatement.Append("END ");
            importStatement.Append("DROP TABLE new");
            importStatement.Append(tableName);
            importStatement.Append(" ");

            conn.Open();

            SqlTransaction transaction = conn.BeginTransaction();
            SqlCommand cmd = new SqlCommand(importStatement.ToString(), conn, transaction);
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = filePath;
            cmd.CommandTimeout = 0; //Never timeout

            try
            {
                int num = cmd.ExecuteNonQuery();
                transaction.Commit();
                ThreadResults.addMsg("Finished importing Constituency Boundaries");
                ThreadResults.addMsg(num.ToString() + " boundaries imported");
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                ThreadResults.addMsg(ex.ToString());
            }
            conn.Close();
        }

        //private void TEST_doImportConstituencyBoundaries(string filePath)
        //{
        //    ThreadResults.addMsg("Importing Constituency Boundaries");

        //    SqlConnection conn = this.connection;

        //    string tableName = "ConstituencyBoundariesTEST";

        //    //import shapefile
        //    StringBuilder importStatement = new StringBuilder();

        //    importStatement.Append("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'new");
        //    importStatement.Append(tableName);
        //    importStatement.Append("') ");
        //    importStatement.Append("BEGIN DROP TABLE new");
        //    importStatement.Append(tableName);
        //    importStatement.Append(" END ");

        //    importStatement.Append("EXEC ST.ImportFromShapefile @filename, '', 'new");
        //    importStatement.Append(tableName);
        //    importStatement.Append("', 'the_geom', 26913 ");

        //    importStatement.Append("IF EXISTS (");
        //    importStatement.Append("SELECT * FROM INFORMATION_SCHEMA.COLUMNS ");
        //    importStatement.Append("WHERE TABLE_NAME = 'new");
        //    importStatement.Append(tableName);
        //    importStatement.Append("' AND COLUMN_NAME = '");
        //    importStatement.Append("CON_NAME");
        //    importStatement.Append("' ");
        //    importStatement.Append(")");
        //    importStatement.Append(" AND EXISTS (");
        //    importStatement.Append("SELECT * FROM INFORMATION_SCHEMA.COLUMNS ");
        //    importStatement.Append("WHERE TABLE_NAME = 'new");
        //    importStatement.Append(tableName);
        //    importStatement.Append("' AND COLUMN_NAME = '");
        //    importStatement.Append("CON_NUM");
        //    importStatement.Append("' ");
        //    importStatement.Append(") BEGIN ");
        //    importStatement.Append("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '");
        //    importStatement.Append(tableName);
        //    importStatement.Append("') ");
        //    importStatement.Append("BEGIN DROP TABLE ");
        //    importStatement.Append(tableName);
        //    importStatement.Append(" END ");
        //    importStatement.Append("CREATE TABLE ");
        //    importStatement.Append(tableName);
        //    importStatement.Append(" (");
        //    importStatement.Append("CON_NUM");
        //    importStatement.Append(" float,");
        //    importStatement.Append("CON_NAME");
        //    importStatement.Append(" varchar(4000)");
        //    importStatement.Append(") ");
        //    importStatement.Append("EXEC ST.AddGeometryColumn '', '");
        //    importStatement.Append(tableName);
        //    importStatement.Append("', 'the_geom', 26913 ");
        //    importStatement.Append("INSERT INTO ");
        //    importStatement.Append(tableName);
        //    importStatement.Append(" (");
        //    importStatement.Append("CON_NUM, CON_NAME");
        //    importStatement.Append(", the_geom) ");
        //    importStatement.Append("SELECT ");
        //    importStatement.Append("CON_NUM, CON_NAME");
        //    importStatement.Append(", the_geom FROM new");
        //    importStatement.Append(tableName);
        //    importStatement.Append(" ");
        //    importStatement.Append("END ");
        //    importStatement.Append("DROP TABLE new");
        //    importStatement.Append(tableName);
        //    importStatement.Append(" ");

        //    conn.Open();

        //    SqlTransaction transaction = conn.BeginTransaction();
        //    SqlCommand cmd = new SqlCommand(importStatement.ToString(), conn, transaction);
        //    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = filePath;
        //    cmd.CommandTimeout = 0; //Never timeout

        //    try
        //    {
        //        int num = cmd.ExecuteNonQuery();
        //        transaction.Commit();
        //        ThreadResults.addMsg("Finished importing Constituency Boundaries");
        //        ThreadResults.addMsg(num.ToString() + " boundaries imported");
        //    }
        //    catch (SqlException ex)
        //    {
        //        transaction.Rollback();
        //        ThreadResults.addMsg(ex.ToString());
        //    }
        //    conn.Close();
        //}

        private void importParcelLinkingTable(object filePath)
        {
            DateTime start = DateTime.Now;
            if (filePath is string)
            {
                doImportParcelLinkingTable((string)filePath);
            }
            else
            {
                ThreadResults.addMsg("Error importing Parcel Linking Table! Input path is not a string!");
            }
            ThreadResults.setComplete(true, DateTime.Now.Subtract(start));
        }

        private void doImportParcelLinkingTable(string filePath)
        {
            ThreadResults.addMsg("Importing Parcel Linking Table");

            StringBuilder importStatement = new StringBuilder();

            importStatement.Append("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'newParcelToAssessment') ");
            importStatement.Append("BEGIN DROP TABLE newParcelToAssessment END ");

            importStatement.Append("CREATE TABLE newParcelToAssessment (  ");
            importStatement.Append("MapParcelID varchar(50),");
            importStatement.Append("AssessmentNumber varchar(50),");
            importStatement.Append("MunID varchar(5)");
            importStatement.Append(")");

            importStatement.Append(" BULK INSERT newParcelToAssessment FROM '");
            importStatement.Append(filePath);
            importStatement.Append("' WITH (FIELDTERMINATOR = ',') ");

            importStatement.Append("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ParcelToAssessment') ");
            importStatement.Append("BEGIN DROP TABLE ParcelToAssessment END ");

            importStatement.Append("CREATE TABLE ParcelToAssessment (  ");
            importStatement.Append("MapParcelID varchar(50),");
            importStatement.Append("AssessmentNumber varchar(50),");
            importStatement.Append("MunID varchar(5)");
            importStatement.Append(") ");

            importStatement.Append("INSERT INTO ParcelToAssessment SELECT * FROM newParcelToAssessment ");

            importStatement.Append("CREATE NONCLUSTERED INDEX [IX_MapParcelID] ON [dbo].[ParcelToAssessment] ( [MapParcelID] ASC ) ");
            importStatement.Append("CREATE NONCLUSTERED INDEX [IX_MunID_AssessmentNumber] ON [dbo].[ParcelToAssessment] ( [MunID] ASC, [AssessmentNumber] ASC ) ");

            importStatement.Append("DROP TABLE newParcelToAssessment ");

            this.connection.Open();

            SqlTransaction transaction = this.connection.BeginTransaction();
            SqlCommand cmd = new SqlCommand(importStatement.ToString(), this.connection, transaction);
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = filePath;
            cmd.CommandTimeout = 0; //Never timeout

            try
            {
                int num = cmd.ExecuteNonQuery();
                transaction.Commit();
                ThreadResults.addMsg("Finished importing Parcel Linking Table");
                ThreadResults.addMsg(num.ToString() + " parcels imported");
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                ThreadResults.addMsg(ex.ToString());
            }
            this.connection.Close();

        }

        //private void TEST_doImportParcelLinkingTable(string filePath)
        //{
        //    ThreadResults.addMsg("Importing Parcel Linking Table");

        //    StringBuilder importStatement = new StringBuilder();

        //    importStatement.Append("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'newParcelToAssessmentTEST') ");
        //    importStatement.Append("BEGIN DROP TABLE newParcelToAssessmentTEST END ");

        //    importStatement.Append("CREATE TABLE newParcelToAssessmentTEST (  ");
        //    importStatement.Append("MapParcelID varchar(50),");
        //    importStatement.Append("AssessmentNumber varchar(50),");
        //    importStatement.Append("MunID varchar(5)");
        //    importStatement.Append(")");

        //    importStatement.Append(" BULK INSERT newParcelToAssessmentTEST FROM '");
        //    importStatement.Append(filePath);
        //    importStatement.Append("' WITH (FIELDTERMINATOR = ',') ");

        //    importStatement.Append("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ParcelToAssessmentTEST') ");
        //    importStatement.Append("BEGIN DROP TABLE ParcelToAssessmentTEST END ");

        //    importStatement.Append("CREATE TABLE ParcelToAssessmentTEST (  ");
        //    importStatement.Append("MapParcelID varchar(50),");
        //    importStatement.Append("AssessmentNumber varchar(50),");
        //    importStatement.Append("MunID varchar(5)");
        //    importStatement.Append(") ");

        //    importStatement.Append("INSERT INTO ParcelToAssessmentTEST SELECT * FROM newParcelToAssessmentTEST ");

        //    importStatement.Append("CREATE NONCLUSTERED INDEX [IX_MapParcelID] ON [dbo].[ParcelToAssessmentTEST] ( [MapParcelID] ASC ) ");
        //    importStatement.Append("CREATE NONCLUSTERED INDEX [IX_MunID_AssessmentNumber] ON [dbo].[ParcelToAssessmentTEST] ( [MunID] ASC, [AssessmentNumber] ASC ) ");

        //    importStatement.Append("DROP TABLE newParcelToAssessmentTEST ");

        //    this.connection.Open();

        //    SqlTransaction transaction = this.connection.BeginTransaction();
        //    SqlCommand cmd = new SqlCommand(importStatement.ToString(), this.connection, transaction);
        //    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = filePath;
        //    cmd.CommandTimeout = 0; //Never timeout

        //    try
        //    {
        //        int num = cmd.ExecuteNonQuery();
        //        transaction.Commit();
        //        ThreadResults.addMsg("Finished importing Parcel Linking Table");
        //        ThreadResults.addMsg(num.ToString() + " parcels imported");
        //    }
        //    catch (SqlException ex)
        //    {
        //        transaction.Rollback();
        //        ThreadResults.addMsg(ex.ToString());
        //    }
        //    this.connection.Close();

        //}

        private static StringBuilder buildImportSQL(string tableName, string colName)
        {
            //import shapefile
            StringBuilder importStatement = new StringBuilder();

            importStatement.Append("EXEC ST.ImportFromShapefile @filename, '', 'new");
            importStatement.Append(tableName);
            importStatement.Append("', 'the_geom', 26913 ");

            importStatement.Append("IF EXISTS (");
            importStatement.Append("SELECT * FROM INFORMATION_SCHEMA.COLUMNS ");
            importStatement.Append("WHERE TABLE_NAME = 'new");
            importStatement.Append(tableName);
            importStatement.Append("' AND COLUMN_NAME = '");
            importStatement.Append(colName);
            importStatement.Append("' ");
            importStatement.Append(") BEGIN ");
            importStatement.Append("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '");
            importStatement.Append(tableName);
            importStatement.Append("') ");
            importStatement.Append("BEGIN DROP TABLE ");
            importStatement.Append(tableName);
            importStatement.Append(" END ");
            importStatement.Append("CREATE TABLE ");
            importStatement.Append(tableName);
            importStatement.Append(" (");
            importStatement.Append(colName);
            importStatement.Append(" varchar(50)");
            importStatement.Append(") ");
            importStatement.Append("EXEC ST.AddGeometryColumn '', '");
            importStatement.Append(tableName);
            importStatement.Append("', 'the_geom', 26913 ");
            importStatement.Append("INSERT INTO ");
            importStatement.Append(tableName);
            importStatement.Append(" (");
            importStatement.Append(colName);
            importStatement.Append(", the_geom) ");
            importStatement.Append("SELECT CONVERT(decimal(38,0), ");
            importStatement.Append(colName);
            importStatement.Append("), the_geom FROM new");
            importStatement.Append(tableName);
            importStatement.Append(" ");
            importStatement.Append("END ");
            importStatement.Append("ELSE BEGIN ");
            importStatement.Append("RAISERROR ('Column %s not found', 16, 1, '");
            importStatement.Append(colName);
            importStatement.Append("')");
            importStatement.Append("END ");
            importStatement.Append("DROP TABLE new");
            importStatement.Append(tableName);
            importStatement.Append(" ");

            return importStatement;
        }
    }
}
