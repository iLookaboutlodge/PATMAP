using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Globalization;
using OSGeo.MapGuide;
using AIMS2012ToolLib;
//using CGIS.MapTools;
//using CGIS.MapTools.MapGuide6_5;

public partial class BaseLayersControl : System.Web.UI.UserControl
{
    private MgMap _map;

    //IMap map = MapManager.getAnalysisMap();

	  //string session = System.Web.HttpContext.Current.Session["Session"].ToString();
    //string weblayout = System.Web.HttpContext.Current.Session["WebLayout"].ToString();
		//string weblayout = @"Library://PATMAP/Layouts/AnalysisMap.WebLayout";
		//string mapName = "AnalysisMap";

		string weblayout = MapSettings.CurrentWebLayout;
		string mapName = MapSettings.CurrentMapName;
		
    protected void Page_Load(object sender, EventArgs e)
    {
			//string session = System.Web.HttpContext.Current.Session["Session"].ToString();
			//UtilityClass utility = new UtilityClass();
			//utility.InitializeWebTier(Request);
			//utility.ConnectToServer(System.Web.HttpContext.Current.Session["Session"].ToString());
			//MgSiteConnection siteConnection = utility.GetSiteConnection();
			//MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

			//MgMap map = Ut_SQL2TT.GetMapObject(resourceService);

			string session = System.Web.HttpContext.Current.Session["Session"].ToString();
			UtilityCl2 utility = new UtilityCl2();
			utility.ConnectToServer(session);
			MgSiteConnection siteConnection = utility.GetSiteConnection();
			MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

			MgMap map = Ut_SQL2TT.GetMapObject(resourceService);

			MgLayerCollection mgLayers = map.GetLayers();
			MgLayerGroupCollection mgGroups = map.GetLayerGroups();

			try
			{
				addLayers(mgLayers, tblLayers);
				foreach (MgLayerGroup group in mgGroups)
				{
					Table tblGroup = createGroupPanel(group);
					addLayers(mgLayers, tblGroup);
				}

			}
			catch (Exception ex)
			{
				this.Response.Write(ex);
				this.Response.End();
			}
    }

    public MgMap map
    {
        set
        {
            this._map = value;
        }
    }

    private  void addLayers(MgLayerCollection mgLayers, Table tblGroup)
    {
        foreach (MgLayer layer in mgLayers)
        {
            if (layer.DisplayInLegend 
                && !layer.Name.Equals(ConfigurationManager.AppSettings["MunicipalitiesLayerName"]) 
                && !layer.Name.Equals(ConfigurationManager.AppSettings["SchoolDivisionsLayerName"]) 
                && !layer.Name.Equals(ConfigurationManager.AppSettings["ParcelLayerName"])
								&& !layer.Name.Equals("Source")
								&& !layer.Name.Equals("Destination")
								&& !layer.Name.ToUpper().Contains("FILTERED")
							)
            {
                CheckBox chkLayer = new CheckBox();
                chkLayer.Text = layer.LegendLabel;
                chkLayer.ToolTip = layer.Name;
                chkLayer.Checked = layer.Visible;
               // chkLayer.Attributes.Add("onclick", "toggleLayer('" + layer.Name + "');");
                chkLayer.AutoPostBack = true;
                chkLayer.CheckedChanged += new EventHandler(this.CheckedChanged_chkLayer);
               // PlaceHolder1.Controls.Add(box);

                TableCell cellChk = new TableCell();
                cellChk.Controls.Add(chkLayer);

                Image img = new Image();
                img.AlternateText = layer.LegendLabel;
                img.ImageUrl = "images/" + layer.Name + ".gif";

                TableCell cellImg = new TableCell();
                cellImg.Controls.Add(img);

                TableRow layerRow = new TableRow();
                layerRow.Cells.Add(cellChk);
                layerRow.Cells.Add(cellImg);

                tblGroup.Rows.Add(layerRow);
            }
        }

    }

    private void CheckedChanged_chkLayer(object sender, EventArgs e)
    {
        CheckBox x = (CheckBox)sender;

				//string session = System.Web.HttpContext.Current.Session["Session"].ToString();
				//UtilityClass utility = new UtilityClass();
				//utility.InitializeWebTier(Request);
				//utility.ConnectToServer(session);
				//MgSiteConnection siteConnection = utility.GetSiteConnection();
				//MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

				//MgMap map = Ut_SQL2TT.GetMapObject(resourceService);

				string session = System.Web.HttpContext.Current.Session["Session"].ToString();
				UtilityCl2 utility = new UtilityCl2();
				utility.ConnectToServer(session);
				MgSiteConnection siteConnection = utility.GetSiteConnection();
				MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

				MgMap map = Ut_SQL2TT.GetMapObject(resourceService);

        MgLayerCollection mgLayers = map.GetLayers();
        MgLayer layer = mgLayers.GetItem(x.ToolTip) as MgLayer;

        layer.Visible = !layer.Visible;
        map.Save(resourceService);
        //test
        map.Save();

        Page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "RefreshMap();", true);
      //  Response.Write("<script language='Javascript'>parent.parent.mapFrame.Refresh()</script>");


    }

    private Table createGroupPanel(MgLayerGroup group)
    {
        Panel pnlGroup = new Panel();
        pnlGroup.BorderWidth = 1;   

        CheckBox chkGroup = new CheckBox();
        chkGroup.Text = group.LegendLabel;
        chkGroup.Checked = group.Visible;
        chkGroup.Attributes.Add("onclick", "toggleGroup('" + group.Name + "');");
        pnlGroup.Controls.Add(chkGroup);

        Image imgGroup = new Image();
        imgGroup.ImageUrl = "images/arrow.gif";
        pnlGroup.Controls.Add(imgGroup);

        Table tblGroup = new Table();
        tblGroup.Style.Add("display", "none");
        pnlGroup.Controls.Add(tblGroup);

        TableCell cellGroup = new TableCell();
        cellGroup.Controls.Add(pnlGroup);
        TableRow row = new TableRow();
        row.Cells.Add(cellGroup);
        tblGroups.Rows.Add(row);

        imgGroup.Attributes.Add("onclick", "toggleGroupPanel('" + tblGroup.ClientID + "');");
        return tblGroup;
    }
	
}
