using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using OSGeo.MapGuide;
using AIMS2012ToolLib;





namespace PATMAPGIS_2012
{
    public partial class MapInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UtilityClass util = new UtilityClass();
            
            //1. initialize web tier
            util.InitializeWebTier(Request);

            //2. connect to Mapguide Server
         
            string session = Request["Session"].ToString();
            util.ConnectToServer(session);

            //3. det the information map
            string mapName = Request["mapname"].ToString();
            string mapInfo = util.GetMapInformation(mapName);
            Response.Write(mapInfo);

        }
    }
}
