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



namespace PATMAPGIS_2012
{
    public partial class _Default : System.Web.UI.Page
    {
        //protected string referrer = "";


        //public string WebLayout, Session;
        //public String mgSessionId, mgLocale, mgMapName;

        private void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["Referrer"] != null)
            {
                this.referrer = this.Request.QueryString["Referrer"];
            }
            else if (this.Request.UrlReferrer != null)
            {
                this.referrer = this.Request.UrlReferrer.AbsolutePath.ToString();
            }
            MapGuideApi.MgInitializeWebTier(@"C:\Program Files\Autodesk\Autodesk Infrastructure Web Server Extension 2012\www\webconfig.ini");
            MgUserInformation userInfo = new MgUserInformation("Administrator", "admin");
            MgSite site = new MgSite();
            site.Open(userInfo);

            Session = site.CreateSession();
            WebLayout = @"Library://PATMAP/Layouts/AnalysisMap.WebLayout";

            NameValueCollection requestParams = Request.HttpMethod == "GET" ? Request.QueryString : Request.Form;
            //String mgSessionId = requestParams["SESSION"];
            //String mgLocale = requestParams["LOCALE"];
            //String mgMapName = requestParams["MAPNAME"];
            //Session["MySession"] = "This is my session value";




            //Session["mgMapName"] = "dqeweqw";
            string UserId = "82";
            string LevelId = "1";

            System.Web.HttpContext.Current.Session.Add(("Session"), Session);
            System.Web.HttpContext.Current.Session.Add(("LevelId"), LevelId);
            System.Web.HttpContext.Current.Session.Add(("UserId"), UserId);

        }
    }
}