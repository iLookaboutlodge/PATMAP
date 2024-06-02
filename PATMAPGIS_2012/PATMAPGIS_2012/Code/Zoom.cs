using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

namespace PATMAPCGIS
{
    public class Zoom
    {


        public static void zoomToBoundaryChange(int userid, Page page)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT (MAX(ST.MaxY(ST.Transform(the_geom, 4043))) + MIN(ST.MinY(ST.Transform(the_geom, 4043)))) / 2 as lat, (MAX(ST.MaxX(ST.Transform(the_geom, 4043))) + MIN(ST.MinX(ST.Transform(the_geom, 4043)))) / 2 as lon, MAX(ST.MaxX(the_geom)) - MIN(ST.MinX(the_geom)) as width, MAX(ST.MaxY(the_geom)) - MIN(ST.MinY(the_geom)) as height FROM [MunicipalitiesChanges] INNER JOIN [MunicipalitiesMapLink] ON MunID = [PPID] WHERE [UserID] = @UserID", conn);
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;
            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            double lat = 0;
            double lon = 0;
            double width = 0;
            double height = 0;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["lat"] != DBNull.Value)
                    {
                        lat = (double)(reader["lat"]);
                    }
                    if (reader["lon"] != DBNull.Value)
                    {
                        lon = (double)(reader["lon"]);
                    }
                    if (reader["width"] != DBNull.Value)
                    {
                        width = (double)(reader["width"]);
                    }
                    if (reader["height"] != DBNull.Value)
                    {
                        height = (double)(reader["height"]);
                    }
                }
                reader.Close();
            }

            page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "switchLayers('" + ConfigurationManager.AppSettings["MunicipalitiesLayerName"] + "', '" + ConfigurationManager.AppSettings["SchoolDivisionsLayerName"] + "'); zoomBox(" + lat.ToString() + ", " + lon.ToString() + ", " + width.ToString() + ", " + height.ToString() + ");", true);
        }
        public static void zoomToLTT(string mun, Page page)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT MunID AS MunicipalityID, (ST.MaxY(ST.Transform(Municipalities.the_geom, 4043)) + ST.MinY(ST.Transform(Municipalities.the_geom, 4043))) / 2 AS lat, (ST.MaxX(ST.Transform(Municipalities.the_geom, 4043)) + ST.MinX(ST.Transform(Municipalities.the_geom, 4043))) / 2 AS lon, ST.MaxX(Municipalities.the_geom) - ST.MinX(Municipalities.the_geom) AS width, ST.MaxY(Municipalities.the_geom) - ST.MinY(Municipalities.the_geom) AS height FROM Municipalities INNER JOIN MunicipalitiesMapLink ON Municipalities.MunID = MunicipalitiesMapLink.PPID INNER JOIN entities ON MunicipalitiesMapLink.PATMAP_Code = entities.number WHERE (entities.jurisdiction = @mun)", conn);
            cmd.Parameters.Add("@mun", SqlDbType.VarChar, 50).Value = mun;
            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            double lat = 0;
            double lon = 0;
            double width = 0;
            double height = 0;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["lat"] != DBNull.Value)
                    {
                        lat = (double)(reader["lat"]);
                    }
                    if (reader["lon"] != DBNull.Value)
                    {
                        lon = (double)(reader["lon"]);
                    }
                    if (reader["width"] != DBNull.Value)
                    {
                        width = (double)(reader["width"]);
                    }
                    if (reader["height"] != DBNull.Value)
                    {
                        height = (double)(reader["height"]);
                    }
                }
                reader.Close();
            }

            page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "switchLayers('" + ConfigurationManager.AppSettings["MunicipalitiesLayerName"] + "', '" + ConfigurationManager.AppSettings["SchoolDivisionsLayerName"] + "'); zoomBox(" + lat.ToString() + ", " + lon.ToString() + ", " + width.ToString() + ", " + height.ToString() + ");", true);
        }


        public static void zoomToSchool_OLD(string key, Page page)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT SD_NUM as SchoolID, (ST.MaxY(ST.Transform(the_geom, 4043)) + ST.MinY(ST.Transform(the_geom, 4043))) / 2 as lat, (ST.MaxX(ST.Transform(the_geom, 4043)) + ST.MinX(ST.Transform(the_geom, 4043))) / 2 as lon, ST.MaxX(the_geom) - ST.MinX(the_geom) as width, ST.MaxY(the_geom) - ST.MinY(the_geom) as height FROM SchoolDivisions WHERE SD_NUM = @SchoolID", conn);
            //SqlCommand cmd = new SqlCommand("SELECT [CON_NUM], (ST.MaxY(ST.Transform(the_geom, 4043)) + ST.MinY(ST.Transform(the_geom, 4043))) / 2 as lat, (ST.MaxX(ST.Transform(the_geom, 4043)) + ST.MinX(ST.Transform(the_geom, 4043))) / 2 as lon, ST.MaxX(the_geom) - ST.MinX(the_geom) as width, ST.MaxY(the_geom) - ST.MinY(the_geom) as height FROM ConstituencyBoundaries WHERE CON_NUM = @ConID", conn);
            cmd.Parameters.Add("@SchoolID", SqlDbType.VarChar, 50).Value = key;
            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            double lat = 0;
            double lon = 0;
            double width = 0;
            double height = 0;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["lat"] != DBNull.Value)
                    {
                        lat = (double)(reader["lat"]);
                    }
                    if (reader["lon"] != DBNull.Value)
                    {
                        lon = (double)(reader["lon"]);
                    }
                    if (reader["width"] != DBNull.Value)
                    {
                        width = (double)(reader["width"]) * 2;
                    }
                    if (reader["height"] != DBNull.Value)
                    {
                        height = (double)(reader["height"]) * 2;
                    }
                }
                reader.Close();
            }

            page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "switchLayers('" + ConfigurationManager.AppSettings["SchoolDivisionsLayerName"] + "', '" + ConfigurationManager.AppSettings["MunicipalitiesLayerName"] + "'); zoomBox(" + lat.ToString() + ", " + lon.ToString() + ", " + width.ToString() + ", " + height.ToString() + ");", true);
        }


        public static void zoomToParcel(string key, Page page)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT [ParcelID], [lat], [lon] FROM [ParcelLocations] WHERE ParcelID = @ParcelID", conn);
            cmd.Parameters.Add("@ParcelID", SqlDbType.VarChar, 50).Value = key;
            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            double lat = 0;
            double lon = 0;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["lat"] != DBNull.Value)
                    {
                        lat = (double)(reader["lat"]);
                    }
                    if (reader["lon"] != DBNull.Value)
                    {
                        lon = (double)(reader["lon"]);
                    }
                }
                reader.Close();
            }
            //mgviewr
            //page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "selectObject('" + key + "', '" + ConfigurationManager.AppSettings["ParcelLayerName"] + "', " + lat + ", " + lon + ");", true);
            //page.ClientScript.RegisterStartupScript(.GetType("mgviewr"), "Zoom", "selectObject('" + key + "', '" + ConfigurationManager.AppSettings["ParcelLayerName"] + "', " + lat + ", " + lon + ");", true);

            page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "zoomBox(0,0,10000,1000);", true);

        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        public static void zoomToConstituency(string key, Page page)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            //Vlad  Original STAFF;
            //SqlCommand cmd = new SqlCommand("SELECT [CON_NUM], (ST.MaxY(ST.Transform(the_geom, 4043)) + ST.MinY(ST.Transform(the_geom, 4043))) / 2 as lat, (ST.MaxX(ST.Transform(the_geom, 4043)) + ST.MinX(ST.Transform(the_geom, 4043))) / 2 as lon, ST.MaxX(the_geom) - ST.MinX(the_geom) as width, ST.MaxY(the_geom) - ST.MinY(the_geom) as height FROM ConstituencyBoundaries WHERE CON_NUM = @ConID", conn);

            SqlCommand cmd = new SqlCommand("SELECT [CON_NUM], (ST.MaxY(the_geom) + ST.MinY(the_geom)) / 2 as Y, (ST.MaxX(the_geom) + ST.MinX(the_geom)) / 2 as X, ST.MaxX(the_geom) - ST.MinX(the_geom) as width, ST.MaxY(the_geom) - ST.MinY(the_geom) as height FROM ConstituencyBoundaries WHERE CON_NUM = @ConID", conn);
            cmd.Parameters.Add("@ConID", SqlDbType.VarChar, 50).Value = key;
            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            double Y = 0;
            double X = 0;
            double width = 0;
            double height = 0;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["Y"] != DBNull.Value)
                    {
                        Y = (double)(reader["Y"]);
                    }
                    if (reader["X"] != DBNull.Value)
                    {
                        X = (double)(reader["X"]);
                    }
                    if (reader["width"] != DBNull.Value)
                    {
                        //width = (double)(reader["width"]) * 2;
                        width = (double)(reader["width"]);
                    }
                    if (reader["height"] != DBNull.Value)
                    {
                        //height = (double)(reader["height"]) * 2;
                        height = (double)(reader["height"]);
                    }
                }
                reader.Close();
            }
            page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "zoomBox(" + X.ToString() + ", " + Y.ToString() + ", " + width.ToString() + ", " + height.ToString() + ");", true);
        }

        public static void zoomToMunicipality(string key, Page page)
        {

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT MunID as [MunicipalityID],(ST.MaxY(the_geom) + ST.MinY(the_geom)) / 2 as Y, (ST.MaxX(the_geom) + ST.MinX(the_geom)) / 2 as X, ST.MaxX(the_geom) - ST.MinX(the_geom) as width, ST.MaxY(the_geom) - ST.MinY(the_geom) as height FROM [Municipalities] INNER JOIN [MunicipalitiesMapLink] ON MunID = [PPID] WHERE [SAMA_Code] = @MunicipalityID", conn);

            cmd.Parameters.Add("@municipalityID", SqlDbType.VarChar, 50).Value = key;

            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);



            double Y = 0;
            double X = 0;
            double width = 0;
            double height = 0;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["Y"] != DBNull.Value)
                    {
                        Y = (double)(reader["Y"]);
                    }
                    if (reader["X"] != DBNull.Value)
                    {
                        X = (double)(reader["X"]);
                    }
                    if (reader["width"] != DBNull.Value)
                    {
                        //width = (double)(reader["width"]) * 2;
                        width = (double)(reader["width"]);
                    }
                    if (reader["height"] != DBNull.Value)
                    {
                        //height = (double)(reader["height"]) * 2;
                        height = (double)(reader["height"]);
                    }
                }
                reader.Close();


                //good one uncommented later
                page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "zoomBox(" + X.ToString() + ", " + Y.ToString() + ", " + width.ToString() + ", " + height.ToString() + ");", true);
                page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "RefreshMap();", true);


                //original
                //page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "switchLayers('" + ConfigurationManager.AppSettings["MunicipalitiesLayerName"] + "', '" + ConfigurationManager.AppSettings["SchoolDivisionsLayerName"] + "'); zoomBox(" + X.ToString() + ", " + Y.ToString() + ", " + width.ToString() + ", " + height.ToString() + ");", true);

            }
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<




        }

        public static string getzoomCoordinate(string key)
        {

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT MunID as [MunicipalityID],(ST.MaxY(the_geom) + ST.MinY(the_geom)) / 2 as Y, (ST.MaxX(the_geom) + ST.MinX(the_geom)) / 2 as X, ST.MaxX(the_geom) - ST.MinX(the_geom) as width, ST.MaxY(the_geom) - ST.MinY(the_geom) as height FROM [Municipalities] INNER JOIN [MunicipalitiesMapLink] ON MunID = [PPID] WHERE [SAMA_Code] = @MunicipalityID", conn);

            cmd.Parameters.Add("@municipalityID", SqlDbType.VarChar, 50).Value = key;

            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);



            double Y = 0;
            double X = 0;
            double width = 0;
            double height = 0;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["Y"] != DBNull.Value)
                    {
                        Y = (double)(reader["Y"]);
                    }
                    if (reader["X"] != DBNull.Value)
                    {
                        X = (double)(reader["X"]);
                    }
                    if (reader["width"] != DBNull.Value)
                    {
                        //width = (double)(reader["width"]) * 2;
                        width = (double)(reader["width"]);
                    }
                    if (reader["height"] != DBNull.Value)
                    {
                        //height = (double)(reader["height"]) * 2;
                        height = (double)(reader["height"]);
                    }
                }
                reader.Close();

            }
            return X.ToString() + ", " + Y.ToString() + ", " + width.ToString() + ", " + height.ToString();
        }


        public static string getzoomBoundaryChangeCoordinate(int userid)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            //SqlCommand cmd = new SqlCommand("SELECT (MAX(ST.MaxY(ST.Transform(the_geom, 4043))) + MIN(ST.MinY(ST.Transform(the_geom, 4043)))) / 2 as lat, (MAX(ST.MaxX(ST.Transform(the_geom, 4043))) + MIN(ST.MinX(ST.Transform(the_geom, 4043)))) / 2 as lon, MAX(ST.MaxX(the_geom)) - MIN(ST.MinX(the_geom)) as width, MAX(ST.MaxY(the_geom)) - MIN(ST.MinY(the_geom)) as height FROM [MunicipalitiesChanges] INNER JOIN [MunicipalitiesMapLink] ON MunID = [PPID] WHERE [UserID] = @UserID", conn);
            SqlCommand cmd = new SqlCommand("SELECT (MAX(ST.MaxY(the_geom)) + MIN(ST.MinY(the_geom))) / 2 as lat, (MAX(ST.MaxX(the_geom)) + MIN(ST.MinX(the_geom))) / 2 as lon, MAX(ST.MaxX(the_geom)) - MIN(ST.MinX(the_geom)) as width, MAX(ST.MaxY(the_geom)) - MIN(ST.MinY(the_geom)) as height FROM [MunicipalitiesChanges] INNER JOIN [MunicipalitiesMapLink] ON MunID = [PPID] WHERE [UserID] = @UserID", conn);

            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;
            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            double lat = 0;
            double lon = 0;
            double width = 0;
            double height = 0;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["lat"] != DBNull.Value)
                    {
                        lat = (double)(reader["lat"]);
                    }
                    if (reader["lon"] != DBNull.Value)
                    {
                        lon = (double)(reader["lon"]);
                    }
                    if (reader["width"] != DBNull.Value)
                    {
                        width = (double)(reader["width"]);
                    }
                    if (reader["height"] != DBNull.Value)
                    {
                        height = (double)(reader["height"]);
                    }
                }
                reader.Close();
            }
            return lon.ToString() + ", " + lat.ToString() + ", " + width.ToString() + ", " + height.ToString();
        }

        public static string getzoomToLTTCoordinate(string mun)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT MunID AS MunicipalityID, (ST.MaxY(Municipalities.the_geom) + ST.MinY(Municipalities.the_geom)) / 2 AS lat, (ST.MaxX(Municipalities.the_geom) + ST.MinX(Municipalities.the_geom)) / 2 AS lon, ST.MaxX(Municipalities.the_geom) - ST.MinX(Municipalities.the_geom) AS width, ST.MaxY(Municipalities.the_geom) - ST.MinY(Municipalities.the_geom) AS height FROM Municipalities INNER JOIN MunicipalitiesMapLink ON Municipalities.MunID = MunicipalitiesMapLink.PPID INNER JOIN entities ON MunicipalitiesMapLink.PATMAP_Code = entities.number WHERE (entities.jurisdiction = @mun)", conn);
            cmd.Parameters.Add("@mun", SqlDbType.VarChar, 50).Value = mun;
            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            double lat = 0;
            double lon = 0;
            double width = 0;
            double height = 0;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["lat"] != DBNull.Value)
                    {
                        lat = (double)(reader["lat"]);
                    }
                    if (reader["lon"] != DBNull.Value)
                    {
                        lon = (double)(reader["lon"]);
                    }
                    if (reader["width"] != DBNull.Value)
                    {
                        width = (double)(reader["width"]);
                    }
                    if (reader["height"] != DBNull.Value)
                    {
                        height = (double)(reader["height"]);
                    }
                }
                reader.Close();
            }

            return lon.ToString() + ", " + lat.ToString() + ", " + width.ToString() + ", " + height.ToString();
        }


        public static void zoomToSchool(string key, Page page)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            //SqlCommand cmd = new SqlCommand("SELECT SD_NUM as SchoolID, (ST.MaxY(ST.Transform(the_geom, 4043)) + ST.MinY(ST.Transform(the_geom, 4043))) / 2 as lat, (ST.MaxX(ST.Transform(the_geom, 4043)) + ST.MinX(ST.Transform(the_geom, 4043))) / 2 as lon, ST.MaxX(the_geom) - ST.MinX(the_geom) as width, ST.MaxY(the_geom) - ST.MinY(the_geom) as height FROM SchoolDivisions WHERE SD_NUM = @SchoolID", conn);

            SqlCommand cmd = new SqlCommand("SELECT SD_NUM as SchoolID, (ST.MaxY(the_geom) + ST.MinY(the_geom)) / 2 as Y,(ST.MaxX(the_geom) + ST.MinX(the_geom)) / 2 as X, ST.MaxX(the_geom) - ST.MinX(the_geom) as width, ST.MaxY(the_geom) - ST.MinY(the_geom) as height FROM SchoolDivisions WHERE SD_NUM = @SchoolID", conn);

            cmd.Parameters.Add("@SchoolID", SqlDbType.VarChar, 50).Value = key;
            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


            double Y = 0;
            double X = 0;
            double width = 0;
            double height = 0;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["Y"] != DBNull.Value)
                    {
                        Y = (double)(reader["Y"]);
                    }
                    if (reader["X"] != DBNull.Value)
                    {
                        X = (double)(reader["X"]);
                    }
                    if (reader["width"] != DBNull.Value)
                    {
                        //width = (double)(reader["width"]) * 2;
                        width = (double)(reader["width"]);
                    }
                    if (reader["height"] != DBNull.Value)
                    {
                        //height = (double)(reader["height"]) * 2;
                        height = (double)(reader["height"]);
                    }
                }
                reader.Close();
            }

            //page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "switchLayers('" + ConfigurationManager.AppSettings["SchoolDivisionsLayerName"] + "', '" + ConfigurationManager.AppSettings["MunicipalitiesLayerName"] + "'); zoomBox(" + lat.ToString() + ", " + lon.ToString() + ", " + width.ToString() + ", " + height.ToString() + ");", true);
            page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "zoomBox(" + X.ToString() + ", " + Y.ToString() + ", " + width.ToString() + ", " + height.ToString() + ");", true);
        }


        //''''''''


    }
}
