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

public partial class MapPage : System.Web.UI.Page
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

					//if AnalysisMAP remove LTTMap and BoundaryChangeStale 11-sep-2013
					if (System.Web.HttpContext.Current.Session["LTTMap"] != null)
					{
						System.Web.HttpContext.Current.Session.Remove("LTTMap");
					}
					if (System.Web.HttpContext.Current.Session["BoundaryChangeStale"] != null)
					{
						System.Web.HttpContext.Current.Session.Remove("BoundaryChangeStale");
					}
					BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE);


					//not required now calculation is done in MapCalculate.aspx		17-jul-2014
					//if (!IsPostBack)
					//{
					//  try
					//  {
					//    PATMAP.common.calculateTableCreation(Response);
					//  }
					//  catch (Exception ex)
					//  {
					//    Response.Write(ex.Message);
					//  }
					//}




					//if (System.Web.HttpContext.Current.Session["TEST_calculated"] == null)
					//{
					//  Response.Write(PATMAP.common.JavascriptFunctions());
					//  Response.Flush();

					//  Response.Write(PATMAP.common.DisplayDiv("Please wait while the calculation is in progress...", -1));
					//  Response.Flush();
					//  System.Threading.Thread.Sleep(10000);

					//  System.Web.HttpContext.Current.Session.Add("TEST_calculated", true);

					//  Response.Write(PATMAP.common.HideDiv());
					//  Response.Flush();

					//  Response.Write("<script language=javascript>window.navigate('Map.aspx?Referrer=" + this.Request.UrlReferrer.AbsolutePath.ToString() + "');</script>");
					//  Response.End();
					//}
					
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

						//added to show map 12-jul-2013
						PATMAP.common.Set_TaxClass_TaxStatus_TaxShift_Filters();

						//PATMAP.common.SetTaxClassFilters();
						////TaxStatus
						//List<string> selectedTaxStatus = new List<string>();
						//if (System.Web.HttpContext.Current.Session["MapTaxStatusFilters"] != null)
						//{
						//  selectedTaxStatus = (List<string>)(System.Web.HttpContext.Current.Session["MapTaxStatusFilters"]);
						//}
						//if (selectedTaxStatus.Count <= 0)
						//{
						//  selectedTaxStatus.Add("Taxable");
						//  System.Web.HttpContext.Current.Session["TaxStatus"] = 1;
						//  System.Web.HttpContext.Current.Session["MapTaxStatusFilters"] = selectedTaxStatus;
						//}
						////TaxShift
						//List<string> selectedTaxShift = new List<string>();
						//if (System.Web.HttpContext.Current.Session["MapTaxShiftFilters"] != null)
						//{
						//  selectedTaxShift = (List<string>)(System.Web.HttpContext.Current.Session["MapTaxShiftFilters"]);
						//}
						//if (selectedTaxShift.Count <= 0)
						//{
						//  selectedTaxShift.Add("Municipal Tax");
						//  System.Web.HttpContext.Current.Session["TaxShift"] = 1;
						//  System.Web.HttpContext.Current.Session["MapTaxShiftFilters"] = selectedTaxShift;
						//}
				}

    }
