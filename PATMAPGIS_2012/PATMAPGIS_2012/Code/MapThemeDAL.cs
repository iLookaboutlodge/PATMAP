using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

public class MapThemeDAL
{
    public MapThemeDAL()
    {

    }

    public bool isPercent(int themesetId)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SELECT isPercent FROM MapThemeSets WHERE ThemeSetID = @ThemeSetID", conn);
        cmd.Parameters.Add("@ThemeSetID", SqlDbType.Int).Value = themesetId;
        conn.Open();
        object ret = cmd.ExecuteScalar();
        conn.Close();
        if (ret != null)
        {
            return (bool)ret;
        }
        else
        {
            return true;
        }
    }

    public DataTable getMapThemeSets(int userid)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SELECT ThemeSetID, ThemeSetName FROM MapThemeSets WHERE UserID = @UserID OR UserID = -1", conn);
        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }

    public IDataReader getMapThemes(int themesetId)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SELECT * FROM MapThemes WHERE ThemeSetID = @ThemeSetID", conn);
        cmd.Parameters.Add("@ThemeSetID", SqlDbType.Int).Value = themesetId;
        conn.Open();
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }

    public void setMapThemesetType(int themesetid, bool isPercent)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        SqlCommand cmd = new SqlCommand("UPDATE MapThemeSets SET IsPercent = @IsPercent WHERE ThemeSetID = @ThemeSetID IF @IsPercent = 1 BEGIN UPDATE MapThemes SET LegendLabel = CAST(MinThemeValue as varchar(15)) + '% - ' + CAST(MaxThemeValue as varchar(15)) + '%' WHERE ThemeSetID = @ThemeSetID END ELSE BEGIN UPDATE MapThemes SET LegendLabel = '$' + CAST(MinThemeValue as varchar(15)) + ' - $' + CAST(MaxThemeValue as varchar(15)) WHERE ThemeSetID = @ThemeSetID END", conn);
        cmd.Parameters.Add("@IsPercent", SqlDbType.Bit).Value = isPercent;
        cmd.Parameters.Add("@ThemeSetID", SqlDbType.Int).Value = themesetid;
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public int GetThemeSetIdByUserIDAndThemSetName(string q_ThemSetName, string q_UserId)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SELECT TOP 1 ThemeSetID FROM MapThemeSets WHERE ThemeSetName ='" + q_ThemSetName + "' and UserID=" + q_UserId, conn);
        conn.Open();
        IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        int q_retval = -1;
        if (reader != null)
            if (reader.Read())
                if (reader["ThemeSetID"] != DBNull.Value)
                    q_retval = System.Convert.ToInt32(reader["ThemeSetID"]);
        reader.Close();
        return q_retval;
    }

    public Dictionary<string, string> GetRawDictionaryForPainting(int q_ThemSetId)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SELECT MinThemeValue,MaxThemeValue,FillColorIndex FROM MapThemes WHERE ThemeSetID =" + q_ThemSetId, conn);
        conn.Open();
        IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        Dictionary<string, string> q_retval = new Dictionary<string, string>();

        if (reader != null)
            while (reader.Read())
            {
                if ((reader["MinThemeValue"] != DBNull.Value) && (reader["MaxThemeValue"] != DBNull.Value) && (reader["FillColorIndex"] != DBNull.Value))
                {
                    string q_1 = reader["MinThemeValue"].ToString();
                    string q_2 = reader["MaxThemeValue"].ToString();
                    string q_3 = reader["FillColorIndex"].ToString();
                    q_retval.Add(q_1 + " " + q_2, q_3);
                }
            }
        reader.Close();
        return q_retval;
    }
}

