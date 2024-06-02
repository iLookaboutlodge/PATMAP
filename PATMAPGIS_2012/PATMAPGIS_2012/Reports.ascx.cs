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
using System.Collections.Specialized;
using System.Text;

public partial class Reports : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string getMapValues = "getMapValues('" + ConfigurationManager.AppSettings["MunicipalitiesLayerName"] + "','" + ConfigurationManager.AppSettings["SchoolDivisionsLayerName"] + "','" + ConfigurationManager.AppSettings["ParcelLayerName"] + "');";
        this.btnOpenTables.OnClientClick = getMapValues;
				this.btnOpenGraphs.OnClientClick = getMapValues;
				if (System.Web.HttpContext.Current.Session["BoundaryChangeStale"] != null)
				{
					this.btnOpenGraphs.Visible = false;
				} else {
					this.btnOpenGraphs.Visible = true;
				}
    }

    private void setSession()
    {
        NameValueCollection formVars = this.Page.Request.Form;

        if (formVars["MunicipalityID"] != null && formVars["MunicipalityID"].Length > 0)
        {
            Session.Add("Municipalities", formVars["MunicipalityID"]);
        }

        if (formVars["SchoolID"] != null && formVars["SchoolID"].Length > 0)
        {
            Session.Add("SchoolDistricts", formVars["SchoolID"]);
        }

        if(formVars["ParcelID"] != null && formVars["ParcelID"].Length > 0)
        {
             Session.Add("ParcelID", formVars["ParcelID"]);
        }

        if (MapSettings.MapPropertyClassFilters != null && MapSettings.MapPropertyClassFilters.Count == 1)
        {
            Session.Add("TaxClass", MapSettings.MapPropertyClassFilters[0]);
        }

        Session.Add("TaxType", MapSettings.MapAnalysisLayer.Equals("Municipalities") ? "1" : "2");

        if (MapSettings.TaxStatusFilters != null && MapSettings.TaxStatusFilters.Count == 1)
        {
            StringBuilder taxStatusFilter = new StringBuilder();
            foreach (string taxStatus in MapSettings.TaxStatusFilters)
            {
                taxStatusFilter.Append(taxStatus);
                taxStatusFilter.Append(",");
            }
            Session.Add("TaxStatusFilters", taxStatusFilter.ToString());
        }
    }

    protected void btnOpenTables_Click(object sender, EventArgs e)
    {
        setSession();
        if (BoundaryChangeSettings.BoundaryChangeState == BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE)
        {
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "parent.parent.document.location='assmnt/tables.aspx'", true);
					this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "openTablesLink('assmnt');", true);
        }
        else if (BoundaryChangeSettings.BoundaryChangeState == BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT)
        {
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "parent.document.location='taxtools/tables.aspx'", true);
					this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "openTablesLink('taxtools');", true);
        }
        else
        {
           //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "parent.document.location='boundary/tables.aspx'", true);
					this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "openTablesLink('boundary');", true);
        }
    }

    protected void btnOpenGraphs_Click(object sender, EventArgs e)
    {
        setSession();
        if (BoundaryChangeSettings.BoundaryChangeState == BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE)
        {
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "parent.document.location='assmnt/graphs.aspx'", true);
					this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "openGraphsLink('assmnt');", true);
        }
        else if (BoundaryChangeSettings.BoundaryChangeState == BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT)
        {
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "parent.document.location='taxtools/graphs.aspx'", true);
					this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "openGraphsLink('taxtools');", true);
        }
        else
        {
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "parent.document.location='boundary/graphs.aspx'", true);
					this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", "openGraphsLink('boundary');", true);
        }
    }

}
