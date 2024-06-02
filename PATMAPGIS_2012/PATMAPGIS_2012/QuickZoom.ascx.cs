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
using CGIS.MapTools;
using CGIS.MapTools.MapGuide6_5;
using System.Collections.Specialized;
using System.Globalization;
using OSGeo.MapGuide;
//using AIMS2012ToolLib;



public partial class QuickZoom : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //changes made for LTTMap	11-sep-2013
            //if (this.Request.Params["IsLTT"] != null && this.Request.Params["IsLTT"].Equals("true"))
            if (System.Web.HttpContext.Current.Session["LTTMap"] != null && (bool)System.Web.HttpContext.Current.Session["LTTMap"] == true)
            {
                //Hide School Districts for Local Tax Tools.
                schoolDivision.Visible = false;
                ddSchoolDistricts.Visible = false;
            }
        }
    }

    protected void ddMunicipalities_SelectedIndexChanged(object sender, EventArgs e)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        UtilityCl2 utility = new UtilityCl2();
        //utility.InitializeWebTier(Request);
        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        MgMap map = Ut_SQL2TT.GetMapObject(resourceService);

        MgLayerCollection mgLayers = map.GetLayers();

        //string q_LayerOn = ConfigurationManager.AppSettings["MunicipalitiesLayerName"].ToString();
        //string q_LayerOff = ConfigurationManager.AppSettings["SchoolDivisionsLayerName"].ToString();

        string q_LayerOn = "Municipalities_Analysis_filtered";
        string q_LayerOn1 = "Municipalities_Analysis";
        string q_LayerOff = "SchoolDivisions_Analysis_filtered";
        string q_LayerOff1 = "SchoolDivisions_Analysis";

        MgLayerBase tmpLayer = null;
        tmpLayer = UtilityCl2.getLayerByName(map, q_LayerOn);
        if (tmpLayer == null)
        {
            tmpLayer = UtilityCl2.getLayerByName(map, q_LayerOn1);
        }
        tmpLayer.SetVisible(true);
        tmpLayer.ForceRefresh();
        map.Save(resourceService);
        //test
        map.Save();

        tmpLayer = null;
        tmpLayer = UtilityCl2.getLayerByName(map, q_LayerOff);
        if (tmpLayer == null)
        {
            tmpLayer = UtilityCl2.getLayerByName(map, q_LayerOff1);
        }
        if (tmpLayer != null)
        {
            tmpLayer.SetVisible(false);
            tmpLayer.ForceRefresh();
            map.Save(resourceService);
            //test
            map.Save();
        }

        ////added for refresh testing
        //tmpLayer = null;
        //tmpLayer = UtilityCl2.getLayerByName(map, "Assessment_Parcels_Analysis_filtered");
        //if (tmpLayer != null)
        //{
        //    tmpLayer.SetVisible(true);
        //    tmpLayer.ForceRefresh();
        //    map.Save(resourceService);
        //    //test
        //    map.Save();
        //}

        if (this.ddMunicipalities.SelectedIndex > 0)
        {
            MapSettings.MapAnalysisLayer = "Municipalities";
            PATMAPCGIS.Zoom.zoomToMunicipality(this.ddMunicipalities.SelectedValue, this.Page);
            this.ddMunicipalities.SelectedIndex = 0;

        }
    }

    protected void ddSchoolDistricts_SelectedIndexChanged(object sender, EventArgs e)
    {

        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        UtilityCl2 utility = new UtilityCl2();
        //utility.InitializeWebTier(Request);
        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        MgMap map = Ut_SQL2TT.GetMapObject(resourceService);

        MgLayerCollection mgLayers = map.GetLayers();

        string q_LayerOn = ConfigurationManager.AppSettings["SchoolDivisionsLayerName"].ToString();
        string q_LayerOff = ConfigurationManager.AppSettings["MunicipalitiesLayerName"].ToString();

        MgLayerBase tmpLayer = null;

        tmpLayer = UtilityCl2.getLayerByName(map, q_LayerOn);
        tmpLayer.SetVisible(true);
        tmpLayer.ForceRefresh();
        map.Save(resourceService);
        //test
        map.Save();

        tmpLayer = null;

        tmpLayer = UtilityCl2.getLayerByName(map, q_LayerOff);
        tmpLayer.SetVisible(false);
        tmpLayer.ForceRefresh();
        map.Save(resourceService);
        //test
        map.Save();

        if (this.ddSchoolDistricts.SelectedIndex > 0)
        {
            MapSettings.MapAnalysisLayer = "SchoolDivisions";
            PATMAPCGIS.Zoom.zoomToSchool(this.ddSchoolDistricts.SelectedValue, this.Page);
            this.ddSchoolDistricts.SelectedIndex = 0;
        }
    }

    protected void ddConstituencyBoundaries_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddConstituencyBoundaries.SelectedIndex > 0)
        {
            PATMAPCGIS.Zoom.zoomToConstituency(this.ddConstituencyBoundaries.SelectedValue, this.Page);
            this.ddConstituencyBoundaries.SelectedIndex = 0;
        }
    }

}
