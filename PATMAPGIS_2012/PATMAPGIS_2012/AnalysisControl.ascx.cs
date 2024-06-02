using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class AnalysisControl : System.Web.UI.UserControl
{
	bool isBoundaryChange = false;
	bool isLTT = false;
	bool firestTime = true;

	protected void Page_Load(object sender, EventArgs e)
	{

		if (!this.IsPostBack)
		{
			//Default first tab to highlighted
			tblNavigation.Rows[0].Cells[0].Attributes.Add("class", "navigationSelected");
		}

		int levelID = int.Parse(Session["levelID"].ToString());

		//  DataTable permissions = PATMAP.common.GetPermission(levelID);

		// 
		//Donna - Removed this as Provincial School Tax shift has been removed.
		//if (permissions.Compute("SUM(ScreenNameID)", "ScreenNameID = 48") == DBNull.Value)
		//{
		//    this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("PMR"));
		//}

		//if (this.Request.Params["IsBoundaryChange"] != null && this.Request.Params["IsBoundaryChange"].Equals("true"))
		if (System.Web.HttpContext.Current.Session["BoundaryChangeStale"] != null )
		{
			isBoundaryChange = true;
		}
		//changes made for LTTMap	11-sep-2013
		//if (this.Request.Params["IsLTT"] != null && this.Request.Params["IsLTT"].Equals("true"))
		if (System.Web.HttpContext.Current.Session["LTTMap"] != null && (bool)System.Web.HttpContext.Current.Session["LTTMap"] == true)
		{
			isLTT = true;
		}

		//Donna - Start        
		if (!IsPostBack)
		{
			if (isLTT)
				MapSettings.MapAnalysisLayer = "Municipalities";    //Reset layer in case it was set to School Divisions.                                            
			else
			//Add 'School Divisions' as an analysis layer for all maps except Local Tax Tools.
			//ddBoundaryType.Items.Add(new ListItem("School Divisions", "SchoolDivisions"));
			//ddBoundaryType.Items.Add(new ListItem("School Divisions", "SchoolDivisions"));
			{ int kkk = 1; }
		}
		//Donna - End

		if (isBoundaryChange)
		{
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Municipal Tax"));
			//this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("PMR")); Donna
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("School Tax"));
			//this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Tax Credit"));  Donna
			//this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Grant"));   Donna
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Total Impact"));
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Total Tax"));
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Base Tax"));
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Minimum Tax"));
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Phase-In Amount"));
		}
		else if (isLTT)
		{
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Levy"));
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Assessment Value"));
			//this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("PMR")); Donna
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("School Tax"));
			//this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Tax Credit"));  Donna
			//this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Grant"));   Donna
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Municipal Tax"));
			if ((bool)Session["phaseInBaseYearAccess"] == false)
			{
				this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Phase-In Amount"));
			}

		}
		else
		{
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Levy"));
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Total Impact"));
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Total Tax"));
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Base Tax"));
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Minimum Tax"));
			this.chkShifts.Items.Remove(this.chkShifts.Items.FindByValue("Phase-In Amount"));
		}
	}

	protected void Page_PreRender(object sender, EventArgs e)
	{
		if (this.chkShifts.Items.Count > 0)
		{
			setFilters();
		}

		if (MapSettings.MapAnalysisLayer != null)
		{
			this.ddBoundaryType.SelectedValue = MapSettings.MapAnalysisLayer;
		}

		Boolean q_TempoFirstTime = false;
		if (this.ddMapThemeSet.SelectedIndex == -1) q_TempoFirstTime = true;

		//modified to show added theme sets 13-May-2014
		String PrevValue = "-1";
		if (this.ddMapThemeSet.SelectedIndex > -1)
		{
			PrevValue = this.ddMapThemeSet.SelectedValue;
		}
		int userid = int.Parse(Session["UserID"].ToString());
		MapThemeDAL dal = new MapThemeDAL();
		this.ddMapThemeSet.DataSource = dal.getMapThemeSets(userid);
		this.ddMapThemeSet.DataBind();
		if (Int32.Parse(PrevValue) > -1)
		{
			ListItem li = ddMapThemeSet.Items.FindByValue(PrevValue.ToString());
			if (li != null)
			{
				this.ddMapThemeSet.SelectedValue = PrevValue;
			} else {
				q_TempoFirstTime = true;
			}
		} else {
			q_TempoFirstTime = true;
		}


		if (q_TempoFirstTime)
		{
			//moved up
			//int userid = int.Parse(Session["UserID"].ToString());
			//MapThemeDAL dal = new MapThemeDAL();
			//this.ddMapThemeSet.DataSource = dal.getMapThemeSets(userid);
			//this.ddMapThemeSet.DataBind();


			//this.ddMapThemeSet.SelectedIndex   = 13;
			//this.ddMapThemeSet.SelectedItem.Text = "Default";
			MapSettings.MapThemeID = -1;
			//this.ddMapThemeSet.SelectedIndex = this.ddMapThemeSet.Items.IndexOf(this.ddMapThemeSet.Items.FindByText("default"));
			this.ddMapThemeSet.Items.FindByText("Default").Selected = true;

		}
		this.ddMapThemeSet.Items.FindByText("Default");

		// Donna - Set to default theme.
		//if (MapSettings.MapThemeID != -1)
		if (this.ddMapThemeSet.Items.Count > 0)
		{
			MapSettings.MapThemeID = int.Parse(this.ddMapThemeSet.SelectedValue);
			MapSettings.MapThemeID = int.Parse(this.ddMapThemeSet.SelectedValue);
		}

		//BigMove
		//if (this.chkShifts.Items.Count > 0)
		//{
		//    setFilters();
		//}

		//if (MapSettings.MapAnalysisLayer != null)
		//{
		//    this.ddBoundaryType.SelectedValue = MapSettings.MapAnalysisLayer;
		//}
		//int userid = int.Parse(Session["UserID"].ToString()) ;
		//MapThemeDAL dal = new MapThemeDAL();
		//this.ddMapThemeSet.DataSource = dal.getMapThemeSets(userid);
		//this.ddMapThemeSet.DataBind();

		//// Donna - Set to default theme.
		////if (MapSettings.MapThemeID != -1)
		//if (this.ddMapThemeSet.Items.Count > 0)        
		//{
		//    this.ddMapThemeSet.SelectedIndex = 0;
		//    MapSettings.MapThemeID = int.Parse(this.ddMapThemeSet.SelectedValue);
		//    //this.ddMapThemeSet.SelectedValue = MapSettings.MapThemeID.ToString();
		//}
		//if (firestTime)
		//{
		//  MapManagerNew.getAnalysisMap();
		//  //Ut_SQL2TT.RefreshMap(this.Page);
		//  //UtilityCl2.ProgressIconOff(this.Page);
		//  //firestTime = false;
		//}

		//UtilityCl2.ProgressIconOn (this.Page );
		////string q_orText=btnRefreshMap.Text;
		//btnRefreshMap.Text = "DATA FETCHING IN PROGRESS...";
		//btnRefreshMap.Enabled = false;
	}

	private void setFilters()
	{
		if (MapSettings.TaxShiftFilters != null)
		{
			foreach (ListItem item in this.chkShifts.Items)
			{
				item.Selected = false;
			}
			List<string> taxShiftFilters = MapSettings.TaxShiftFilters;
			foreach (string taxShiftFilter in taxShiftFilters)
			{
				ListItem item = this.chkShifts.Items.FindByValue(taxShiftFilter);
				if (item != null)
				{
					item.Selected = true;
				}
			}
		}
		else
		{
			foreach (ListItem item in this.chkShifts.Items)
			{
				item.Selected = true;
			}
		}

		if (MapSettings.TaxStatusFilters != null)
		{
			foreach (ListItem item in this.chkTaxStatuses.Items)
			{
				item.Selected = false;
			}
			List<string> taxStatusFilters = MapSettings.TaxStatusFilters;
			foreach (string taxStatusFilter in taxStatusFilters)
			{
				ListItem item = this.chkTaxStatuses.Items.FindByValue(taxStatusFilter);
				if (item != null)
				{
					item.Selected = true;
				}
			}
		}
		else
		{
			foreach (ListItem item in this.chkTaxStatuses.Items)
			{
				item.Selected = true;
			}
		}
	}

	protected void tabControls_MenuItemClick(object sender, EventArgs e)
	{
		LinkButton btn = (LinkButton)sender;
		MultiView1.ActiveViewIndex = int.Parse(btn.CommandName);

		for (int i = 0; i < tblNavigation.Rows[0].Cells.Count; i++)
		{
			if (MultiView1.ActiveViewIndex == i)
			{
				tblNavigation.Rows[0].Cells[i].Attributes.Add("class", "navigationSelected");
			}
			else
			{
				tblNavigation.Rows[0].Cells[i].Attributes.Remove("class");
			}
		}
	}

	protected void ddBoundaryType_SelectedIndexChanged(object sender, EventArgs e)
	{
		switch (this.ddBoundaryType.SelectedIndex)
		{
			case 0:
				Ut_SQL2TT.turnOnMunicipalities();
				break;
			case 1:
				Ut_SQL2TT.turnOnSchoolDistricts();
				break;
		}
		Ut_SQL2TT.RefreshMap(this.Page);
		//bigmove
		//switch (this.ddBoundaryType.SelectedIndex)
		//{
		//    case 0:
		//        turnOnMunicipalities();
		//        break;
		//    case 1:
		//        turnOnSchoolDistricts();                
		//        break;
		//}
	}

	private void turnOnSchoolDistricts()
	{
		MapSettings.MapAnalysisLayer = "SchoolDivisions";
		this.Page.ClientScript.RegisterStartupScript(this.GetType(), "TurnOnSchoolDistricts", "switchLayers('" + ConfigurationManager.AppSettings["SchoolDivisionsLayerName"] + "', '" + ConfigurationManager.AppSettings["MunicipalitiesLayerName"] + "');", true);
	}

	private void turnOnMunicipalities()
	{
		MapSettings.MapAnalysisLayer = "Municipalities";
		this.Page.ClientScript.RegisterStartupScript(this.GetType(), "TurnOnMunicipalities", "switchLayers('" + ConfigurationManager.AppSettings["MunicipalitiesLayerName"] + "', '" + ConfigurationManager.AppSettings["SchoolDivisionsLayerName"] + "');", true);
	}

	//do not use this function
	private void reloadMap()
	{
		if (MapSettings.MapStale)
		{
			Ut_SQL2TT.RefreshMap(this.Page);
		}
		//bigmove
		if (MapSettings.MapStale)
		{
			this.Page.ClientScript.RegisterStartupScript(this.GetType(), "RefreshShifts", "parent.parent.GetMapFrame().Refresh();", true);
		}
	}

    protected void btnRefreshMap_Click(object sender, EventArgs e)
    {
        //UtilityCl2.ProgressIconOn (this.Page );
        ////string q_orText=btnRefreshMap.Text;
        //btnRefreshMap.Text = "DATA FETCHING IN PROGRESS...";
        //btnRefreshMap.Enabled = false;

        MapManagerNew.getAnalysisMap(MapSettings.IsRequiredTableCreate);
        Ut_SQL2TT.RefreshMap(this.Page);

        //btnRefreshMap.Enabled = true;
        //btnRefreshMap.Text = q_orText;
        //UtilityCl2.ProgressIconOff(this.Page);

        MapSettings.IsRequiredTableCreate = false;
        //bigmove
        //temporarily remared as running twice	08-jul-13
        //reloadMap();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        MapSettings.IsRequiredTableCreate = false;
        MapManagerNew.getAnalysisMap(MapSettings.IsRequiredTableCreate);

        //Ut_SQL2TT.RefreshMap(this.Page);
        //UtilityCl2.ProgressIconOff(this.Page);
        //System.Threading.Thread.Sleep(5000);
    }

	protected void chkShifts_SelectedIndexChanged(object sender, EventArgs e)
	{
		List<string> filters = new List<string>();
		foreach (ListItem item in this.chkShifts.Items)
		{
			if (item.Selected)
			{
				filters.Add(item.Value);
			}
		}
		//if ((MapSettings.TaxShiftFilters == null && filters.Count > 0) || (MapSettings.TaxShiftFilters != null && !filters.Count.Equals(MapSettings.TaxShiftFilters.Count)))
		{
			MapSettings.TaxShiftFilters = filters;
			MapSettings.MapStale = true;
			MapSettings.IsRequiredTableCreate = true;
		}
	}

	protected void chkTaxStatuses_SelectedIndexChanged(object sender, EventArgs e)
	{
		List<string> filters = new List<string>();
		foreach (ListItem item in this.chkTaxStatuses.Items)
		{
			if (item.Selected)
			{
				filters.Add(item.Value);
			}
		}
		if ((MapSettings.TaxStatusFilters == null && filters.Count > 0) || (MapSettings.TaxStatusFilters != null && !filters.Count.Equals(MapSettings.TaxStatusFilters.Count)))
		{
			MapSettings.TaxStatusFilters = filters;
			MapSettings.MapStale = true;
			MapSettings.IsRequiredTableCreate = true;
		}
	}

	protected void ddMapThemeSet_SelectedIndexChanged(object sender, EventArgs e)
	{
		MapSettings.MapThemeID = int.Parse(this.ddMapThemeSet.SelectedValue);
		MapSettings.MapStale = true;
	}

}
