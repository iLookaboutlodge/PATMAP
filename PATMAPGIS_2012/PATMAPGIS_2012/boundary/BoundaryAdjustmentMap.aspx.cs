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
using System.Data.SqlClient;
using System.Text;

public partial class BoundaryAdjustmentMap : System.Web.UI.Page
{
	protected string referrer = "";

	public string WebLayout, Session;
	public String mgSessionId, mgLocale, mgMapName;

	protected void Page_Load(object sender, EventArgs e)
	{
		//required properties 
		//BoundaryChange
		MapSettings.CurrentMapName = ConfigurationManager.AppSettings["AutodeskBoundaryChangeMapName"];
		MapSettings.CurrentWebLayout = ConfigurationManager.AppSettings["AutodeskBoundaryChangeWebLayout"];

		int UserID = (int)System.Web.HttpContext.Current.Session["UserID"];
		if (BoundaryChangeSettings.Destination == null)
		{
			throw new Exception("Destination not set");
		}
		if (BoundaryChangeSettings.Source == null)
		{
			throw new Exception("Source not set");
		}

		//if BoundaryAdjustmentMap remove LTTMap and set it to non stale first
		if (System.Web.HttpContext.Current.Session["LTTMap"] != null)
		{
			System.Web.HttpContext.Current.Session.Remove("LTTMap");
		}
		if (System.Web.HttpContext.Current.Session["BoundaryChangeStale"] == null)
		{
			System.Web.HttpContext.Current.Session.Add("BoundaryChangeStale", false);
		}
		System.Web.HttpContext.Current.Session["BoundaryChangeStale"] = false;
		BoundaryChangeSettings.BoundaryChangeState = BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.USER;


		if (!IsPostBack)
		{
			try
			{
				//PATMAP.common.calculateBOUNDARYCHANGETableCreation(Response);
				calculateBOUNDARYADJUSTMENTTableCreation(Response);
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
		//WebLayout = @"Library://PATMAP/Layouts/TestMap.WebLayout";
		//WebLayout = @"Library://PATMAP/Layouts/BoundaryChange.WebLayout";
		WebLayout = MapSettings.CurrentWebLayout;

		NameValueCollection requestParams = Request.HttpMethod == "GET" ? Request.QueryString : Request.Form;

		System.Web.HttpContext.Current.Session.Add(("WebLayout"), WebLayout);
		System.Web.HttpContext.Current.Session.Add(("Session"), Session);
		string type = System.Web.HttpContext.Current.Session["Session"].ToString();

		//PATMAP.common.SetBOUNDARYCHANGE_TaxClass_TaxStatus_TaxShift_Filters();
	}

	public void calculateBOUNDARYADJUSTMENTTableCreation(System.Web.HttpResponse Response)
	{
		//setup database connection
		SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
		conn.Open();
		int userID = (int)System.Web.HttpContext.Current.Session["userID"];

		try
		{
			SqlDataAdapter da = new SqlDataAdapter();
			SqlCommand query = new SqlCommand();
			query.Connection = conn;
			query.CommandTimeout = 60000;
			SqlDataReader dr;
			if (HttpContext.Current.Session["MapStale"] == null)
			{
				HttpContext.Current.Session.Add("MapStale", true);
			}
			HttpContext.Current.Session["MapStale"] = true;

			query.CommandText = "select mapDataStale from liveBoundaryModel where userid = " + userID.ToString();
			dr = query.ExecuteReader();
			dr.Read();
			Boolean mapDataStale = false;
			mapDataStale = (Boolean)dr.GetValue(0);
			dr.Close();

			if (mapDataStale)
			{
				if (System.Web.HttpContext.Current.Session["BoundaryChangeStale"] == null)
				{
					System.Web.HttpContext.Current.Session.Add("BoundaryChangeStale", true);
				}
				System.Web.HttpContext.Current.Session["BoundaryChangeStale"] = true;

				StringBuilder delStr = new StringBuilder();
				delStr.Append("DELETE MunicipalitiesChanges WHERE UserID = @UserID DELETE mapBoundaryTransfers WHERE UserID = @UserID");
				SqlCommand clearChangesCmd = new SqlCommand(delStr.ToString(), conn);
				clearChangesCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
				clearChangesCmd.ExecuteNonQuery();

				//BuildBoundary.buildBoundary(userID);
				System.Web.HttpContext.Current.Session["BoundaryChangeStale"] = false;
				//BoundaryChangeSettings.BoundaryChangeState = BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.USER;
			}
		}
		finally
		{
			if (conn.State == ConnectionState.Open)
			{
				conn.Close();
			}
		}
	}

}