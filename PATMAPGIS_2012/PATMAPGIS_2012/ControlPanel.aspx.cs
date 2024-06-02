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

public partial class ControlPanel : MappingBasePage
    //    : System.Web.UI.Page
    //
{
    //const string viewerPathSchema = "http://localhost/mapserver2012/mapviewernet/" + "ajaxviewer.aspx?SESSION={0}&WEBLAYOUT={1}";
    //UtilityClass utility = new UtilityClass();
    //string mapName = "AnalysisMap";

    //public string WebLayout, SessionID;

    protected void Page_Load(object sender, EventArgs e)
    {
				int UserID = (int)System.Web.HttpContext.Current.Session["UserID"];

                //added for Parcel Tooltip display
                HiddenField hdnCurrentUserID = (HiddenField)this.FindControl("hdnCurrentUserID");
                if (hdnCurrentUserID != null)
                {
                    hdnCurrentUserID.Value = UserID.ToString();
                }

				if (System.Web.HttpContext.Current.Session["LTTMap"] != null && (bool)System.Web.HttpContext.Current.Session["LTTMap"] == true)
				{
					HiddenField ctrl = (HiddenField)this.FindControl("hdnLTTMap");
					if (ctrl != null)
					{
						//string coord = PATMAPCGIS.Zoom.getzoomToLTTCoordinate(Session["LTTSubjectMunicipality"].ToString());
						string coord = PATMAPCGIS.Zoom.getzoomCoordinate(Session["CODE_LTTSubjectMunicipality"].ToString());
						ctrl.Value = "LTTMap;" + coord.Trim();
					}
				}
				else if (System.Web.HttpContext.Current.Session["BoundaryChangeStale"] != null )
				{
					HiddenField ctrl = (HiddenField)this.FindControl("hdnLTTMap");
					if (ctrl != null)
					{
						//string coord = PATMAPCGIS.Zoom.getzoomCoordinate(Session["CODE_DestinationMunicipality"].ToString());
						//string coord = PATMAPCGIS.Zoom.getzoomCoordinate("005");
						string coord = PATMAPCGIS.Zoom.getzoomBoundaryChangeCoordinate(UserID);
						ctrl.Value = "BOUNDARYCHANGEMap;" + coord.Trim();
					}
				}

				//process to auto load map when load completes
				Page.LoadComplete += new EventHandler(Page_LoadComplete);

				if (!this.IsPostBack)
				{
					//Default first tab to highlighted
					this.tblAnalysis.Rows[0].Cells[0].Attributes.Add("class", "navigationSelected");

					//moved up not required now
					//if (this.Page.Request.QueryString["IsBoundaryChange"] != null && this.Page.Request.QueryString["IsBoundaryChange"] == "true")
					//{
					//  PATMAPCGIS.Zoom.zoomToBoundaryChange((int)Session["UserID"], this.Page);
					//}

					//changes made for LTTMap	11-sep-2013
					//if (this.Page.Request.QueryString["IsLTT"] != null && this.Page.Request.QueryString["IsLTT"] == "true")
					if (System.Web.HttpContext.Current.Session["LTTMap"] != null && (bool)System.Web.HttpContext.Current.Session["LTTMap"] == true)
					{
						//PATMAPCGIS.Zoom.zoomToLTT(Session["LTTSubjectMunicipality"].ToString(), this.Page);
						//PATMAPCGIS.Zoom.zoomToMunicipality(Session["LTTSubjectMunicipality"].ToString(), this.Page);
						//not required not moved
						//PATMAPCGIS.Zoom.zoomToMunicipality(Session["CODE_LTTSubjectMunicipality"].ToString(), this.Page);
					}
				}
				if (this.Page.Request.QueryString["IsAssmnt"] != null && this.Page.Request.QueryString["IsAssmnt"] == "true")
				{
					BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE);
				}

				if (Session["MapZoom"] != null)
				{
					if (MapSettings.MapAnalysisLayer.Equals("SchoolDivisions"))
					{
						PATMAPCGIS.Zoom.zoomToSchool(Session["MapZoom"].ToString(), this.Page);
					}
					else if (MapSettings.MapAnalysisLayer.Equals("Parcel"))
					{
						PATMAPCGIS.Zoom.zoomToParcel(Session["MapZoom"].ToString(), this.Page);
						MapSettings.MapAnalysisLayer = "Municipalities";
					}
					else
					{
						PATMAPCGIS.Zoom.zoomToMunicipality(Session["MapZoom"].ToString(), this.Page);
						MapSettings.MapAnalysisLayer = "Municipalities";
					}
					Session.Remove("MapZoom");
				}
    }

	  protected void Button1_Click(object sender, EventArgs e)
    {
        // string layerName = this.tbLayer.Text;
        // string strFilter = Server.HtmlEncode(tbFilter.Text);

        // string session = Session["MgSession"].ToString();
        // UtilityClass utility = new UtilityClass();
        // utility.InitializeWebTier(Request);
        // utility.ConnectToServer(session);
        // MgSiteConnection siteConnection = utility.GetSiteConnection();

        // string webLayout = Session["InitWebLayout"].ToString();


        // string layerDefId = utility.GetLayerDefinitionResourceId(layerName, session, mapName);

        // //create a filtered session layer defination 
        // string sessionLayerId = utility.SetFilterForLayer(session, layerDefId, strFilter);

        // //create a session layer object, 
        // //replace it with the library layer
        // utility.ReplaceWithFilteredLayer(session, sessionLayerId, layerName, mapName);
        //// RenderMap(map, selection, format, bKeepSelection, bClip);
    //    Response.Write("<SCRIPT>parent.parent.mapFrame.ZoomToView(644634.109999999, 5442435.63, 500000, true);</SCRIPT>");
    }

    protected void tabControls_MenuItemClick(object sender, EventArgs e)
    {
				//Response.Write("<SCRIPT>top.parent.parent.parent.parent.parent.parent.parent.mapFrame.ZoomToView(644634.109999999, 5442435.63, 500000, true);</SCRIPT>");

        LinkButton btn = (LinkButton)sender;
        MultiView1.ActiveViewIndex = int.Parse(btn.CommandName);

        for (int i = 0; i < this.tblAnalysis.Rows[0].Cells.Count; i++)
        {
            if (MultiView1.ActiveViewIndex == i)
            {
                this.tblAnalysis.Rows[0].Cells[i].Attributes.Add("class", "navigationSelected");
            }
            else
            {
                this.tblAnalysis.Rows[0].Cells[i].Attributes.Remove("class");
            }
        }
    }

		protected void Page_LoadComplete(object sender, EventArgs e)
		{
			//process to auto load map when load completes
			if (!IsPostBack)
			{
				//only do auto postback once
				if (System.Web.HttpContext.Current.Session["IsPrintStale"] == null)
				{
					System.Web.HttpContext.Current.Session.Add(("IsPrintStale"), 1);
					this.Page.ClientScript.RegisterStartupScript(this.GetType(), "AutoScript", "execAutoRefresh();", true);
				}
			}
		}

}
