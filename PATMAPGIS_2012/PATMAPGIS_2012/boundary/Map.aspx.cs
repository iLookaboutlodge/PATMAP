using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using OSGeo.MapGuide;
using System.Collections.Specialized;
using System.Collections.Generic;

public partial class MapBoundaryChange : System.Web.UI.Page
{
	protected string referrer = "";

	public string WebLayout, Session;
	public String mgSessionId, mgLocale, mgMapName;

	public void Page_Load(object sender, EventArgs e)
	{
		//required properties 
		//AnalysisMap
		MapSettings.CurrentMapName = ConfigurationManager.AppSettings["AutodeskAnalysisMapName"];
		MapSettings.CurrentWebLayout = ConfigurationManager.AppSettings["AutodeskAnalysisMapWebLayout"];

		int UserID =(int)System.Web.HttpContext.Current.Session["UserID"];

		//if BoundaryChangeMap remove LTTMap and set it to non stale first
		if (System.Web.HttpContext.Current.Session["LTTMap"] != null)
		{
			System.Web.HttpContext.Current.Session.Remove("LTTMap");
		}
		if (System.Web.HttpContext.Current.Session["BoundaryChangeStale"] == null)
		{
			System.Web.HttpContext.Current.Session.Add("BoundaryChangeStale",false);
		}
		System.Web.HttpContext.Current.Session["BoundaryChangeStale"]=false;
		BoundaryChangeSettings.BoundaryChangeState = BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.USER;

		if (!IsPostBack)
		{
			try
			{
				PATMAP.common.calculateBOUNDARYCHANGETableCreation(Response);
				//BuildBoundary.buildBoundary(UserID);
		}
			catch (Exception ex)
			{
				Response.Write(ex.Message);
			}
		}
		//after calculation set boundarychangestale to false
		System.Web.HttpContext.Current.Session["BoundaryChangeStale"] = false;
		BoundaryChangeSettings.BoundaryChangeState = BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.USER;

		if (this.Request.QueryString["Referrer"] != null)
		{
			this.referrer = this.Request.QueryString["Referrer"];
		}
		else if (this.Request.UrlReferrer != null)
		{
			this.referrer = this.Request.UrlReferrer.AbsolutePath.ToString();
		}

		if (System.Web.HttpContext.Current.Session["IsPrintStale"] != null)
		{
			System.Web.HttpContext.Current.Session.Remove("IsPrintStale");
		}

		Session = Ut_SQL2TT.Create_Get_Session();

		//WebLayout = @"Library://PATMAP/Layouts/AnalysisMap.WebLayout";
		WebLayout = MapSettings.CurrentWebLayout;

		NameValueCollection requestParams = Request.HttpMethod == "GET" ? Request.QueryString : Request.Form;
		//String mgSessionId = requestParams["SESSION"];
		//String mgLocale = requestParams["LOCALE"];
		//String mgMapName = requestParams["MAPNAME"];
		//Session["MySession"] = "This is my session value";

		//Session["mgMapName"] = "dqeweqw";

		System.Web.HttpContext.Current.Session.Add(("WebLayout"), WebLayout);
		System.Web.HttpContext.Current.Session.Add(("Session"), Session);
		string type = System.Web.HttpContext.Current.Session["Session"].ToString();
		//System.Web.HttpContext.Current.Session.Add(("levelID"), LevelId);
		//System.Web.HttpContext.Current.Session.Add(("UserId"), UserId);

		////added to show map 12-jul-2013
		//PATMAP.common.Set_TaxClass_TaxStatus_TaxShift_Filters();
		//modified on 10-sep-2013
		PATMAP.common.SetBOUNDARYCHANGE_TaxClass_TaxStatus_TaxShift_Filters();
	}

}
