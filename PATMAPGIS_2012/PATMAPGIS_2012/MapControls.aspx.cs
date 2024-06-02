using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;

public partial class MapControls : MappingBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["Referrer"] != null)
        {
            this.btnClose.Visible = true;
            this.btnClose.Attributes.Add("onclick", "parent.document.location='" + this.Request.QueryString["Referrer"] + "'");
            this.btnClose.NavigateUrl = "#";


            int userID = (int)Session["UserID"];

            if (BoundaryChangeSettings.BoundaryChangeState == BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
                SqlCommand query = new SqlCommand();
                query.Connection = conn;
                query.CommandText = "select assessmentTaxModelName from liveAssessmentTaxModel where userid = @userID";
                query.Parameters.Add("@userid", SqlDbType.Int).Value = userID;

                conn.Open();
                string name = (string)query.ExecuteScalar();

                //this.lblModelName.Text = "Scenario: " + (name.Length > 0 ? name : "[NO SCENARIO NAME]");
                //this.lblModelName.Text = Session["LTTSubjectMunicipality"].ToString();
                conn.Close();
            }
            else if (BoundaryChangeSettings.BoundaryChangeState == BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT)
            {
                this.lblModelName.Text = Session["LTTSubjectMunicipality"].ToString();
            }
            else
            {
                this.lblModelName.Text = "Scenario: [NO SCENARIO NAME]";
            }

            StringBuilder str = new StringBuilder();
            if (MapSettings.TaxShiftFilters.Count > 0)
            {
                str.Append("Shifts: ");

                foreach (string shift in MapSettings.TaxShiftFilters)
                {
                    str.Append(shift);
                    str.Append(",");
                }
                str.Remove(str.Length - 1, 1);
            }

            if (MapSettings.TaxStatusFilters.Count > 0)
            {
                str.Append(" Statuses: ");

                foreach (string status in MapSettings.TaxStatusFilters)
                {
                    str.Append(status);
                    str.Append(",");
                }
                str.Remove(str.Length - 1, 1);
            }

            if (MapSettings.MapPropertyClassFilters != null && MapSettings.MapPropertyClassFilters.Count > 0)
            {
                str.Append(" Classes: ");

                //Donna start
                List<string> classFilters = new List<string>();

                if (BoundaryChangeSettings.BoundaryChangeState == BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT)
                {
                    //Show the first character of the tax class for restricted LTT users.
                    if ((bool)HttpContext.Current.Session["showFullTaxClasses"] == false)
                    {
                        StringBuilder query = new StringBuilder();
                        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        query.Append("SELECT SUBSTRING(taxClass, 1, 1) FROM LTTmainTaxClasses WHERE taxClassID IN (");

                        List<string> filters = MapSettings.MapPropertyClassFilters;

                        foreach (string filter in filters)
                        {
                            query.Append("'");
                            query.Append(filter);
                            query.Append("',");
                        }

                        query.Append("'')");
                        cmd.CommandText = query.ToString();
                        conn.Open();

                        IDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            classFilters.Add((string)reader[0]);
                        }

                        reader.Close();
                        conn.Close();
                    }
                    else
                        classFilters = MapSettings.MapPropertyClassFilters;
                }
                else
                    classFilters = MapSettings.MapPropertyClassFilters;

                foreach (string taxclass in classFilters)
                {
                    str.Append(taxclass);
                    str.Append(",");
                }
                //Donna end

                str.Remove(str.Length - 1, 1);
            }
            else
            {
                str.Append(" Classes: All");
            }

            this.lblMapTitle.Text = str.ToString();
        }
        else
        {
            this.lblModelName.Text = "Group: " + BoundaryChangeSettings.GroupName;
        }
    }
}
