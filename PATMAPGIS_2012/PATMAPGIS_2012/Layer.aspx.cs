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
using OSGeo.MapGuide;
using System.Collections.Specialized;
//using CGIS.MapTools;
//using CGIS.MapTools.MapGuide6_5;

namespace PATMAPGIS_2012
{
    public partial class Layer : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection requestParams = Request.HttpMethod == "GET" ? Request.QueryString : Request.Form;
            String mgSessionId = requestParams["SESSION"];
            String mgLocale = requestParams["LOCALE"];
            String mgMapName = requestParams["MAPNAME"];
            mgMapName = "AnalysisMap";

            // Initialize the web-tier.

            String realPath = Request.ServerVariables["APPL_PHYSICAL_PATH"];
            String configPath = realPath + "..\\webconfig.ini";
            MapGuideApi.MgInitializeWebTier(configPath);

            // Connect to the site.
            MgUserInformation userInfo = new MgUserInformation(mgSessionId);
            MgSiteConnection siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);

            // Get an instance of the required service(s).
            MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

						MgMap map = Ut_SQL2TT.GetMapObject(resourceService);

            // Get map layers.
            MgLayerCollection mgLayers = map.GetLayers();
            MgLayer roadsLayer = mgLayers.GetItem("Wells_Background") as MgLayer;

            int i = 0;
            int count = map.GetLayers().Count;
            for ( i= 1; i <12; i++) 


            {
                if (i == 1)
                {
                    this.CheckBox1.Visible = true;
                    this.CheckBox1.Checked = map.GetLayers()[i].Visible;
                    this.CheckBox1.Text = map.GetLayers()[i].Name;

                }
                if (i == 2)
                {
                    this.CheckBox2.Visible = true;
                    this.CheckBox2.Checked = map.GetLayers()[i].Visible;
                    this.CheckBox2.Text = map.GetLayers()[i].Name;

                }

                //if (map.GetLayers()[1].sh.ShowInLegend
                //    && !layer.Name.Equals(ConfigurationManager.AppSettings["MunicipalitiesLayerName"])
                //    && !layer.Name.Equals(ConfigurationManager.AppSettings["SchoolDivisionsLayerName"])
                //    && !layer.Name.Equals(ConfigurationManager.AppSettings["ParcelLayerName"]))
                //{

                //////  CheckBox chkLayer = new  CheckBox();
                //////  chkLayer.Text = map.GetLayers()[i].Name;
                //////  chkLayer.Checked = map.GetLayers()[i].Visible;
                //////  chkLayer.Attributes.Add("onclick", "toggleLayer('" + map.GetLayers()[i].Name + "');");

                //////  TableCell cellChk = new TableCell();
                //////  cellChk.Controls.Add(chkLayer);

                //////  Image img = new Image();
                //////  img.AlternateText = map.GetLayers()[i].LegendLabel;
                //////  img.ImageUrl = "images/" + map.GetLayers()[i].Name + ".gif";

                //////  TableCell cellImg = new TableCell();
                //////  cellImg.Controls.Add(img);


                //////  TableRow layerRow = new TableRow();
                //////  layerRow.Cells.Add(cellChk);
                //////  layerRow.Cells.Add(cellImg);

                ////////  tblGroup.Rows.Add(layerRow);
                //////  Panel pnlGroup = new Panel();
                //////  pnlGroup.BorderWidth = 1;

                //////  CheckBox chkGroup = new CheckBox();
                //////  //chkGroup.Text = group.LegendLabel;
                //////  //chkGroup.Checked = group.Visible;
                //////  chkGroup.Attributes.Add("onclick", "toggleGroup('" + map.GetLayers()[i].Name + "');");
                //////  pnlGroup.Controls.Add(chkGroup);

                //////  Image imgGroup = new Image();
                //////  //imgGroup.ImageUrl = "images/arrow.gif";
                //////  pnlGroup.Controls.Add(imgGroup);

                //////  Table tblGroup = new Table();
                //////  tblGroup.Style.Add("display", "none");
                //////  pnlGroup.Controls.Add(tblGroup);

                //////  TableCell cellGroup = new TableCell();
                //////  cellGroup.Controls.Add(pnlGroup);
                //////  TableRow row = new TableRow();
                //////  row.Cells.Add(cellGroup);
                //////  tblGroups.Rows.Add(row);

                //////  imgGroup.Attributes.Add("onclick", "toggleGroupPanel('" + tblGroup.ClientID + "');");
                //}
                //   }

                //toggle the visibility.
               // roadsLayer.Visible = !roadsLayer.Visible;

                // Save the updated map to apply the change
              //  map.Save(resourceService);
                //Response.Write("Parcels Layers visible togged!");

                //refresh the map using the viewer API
                //Response.Write("<script>parent.parent.Refresh();</script>");

            }


        }

        public void CheckBox1_OnCheckedChanged(object sender, EventArgs e)
        {
            NameValueCollection requestParams = Request.HttpMethod == "GET" ? Request.QueryString : Request.Form;
            String mgSessionId = requestParams["SESSION"];
            String mgLocale = requestParams["LOCALE"];
            String mgMapName = requestParams["MAPNAME"];
            mgMapName = "AnalysisMap";

            // Initialize the web-tier.

            String realPath = Request.ServerVariables["APPL_PHYSICAL_PATH"];
            String configPath = realPath + "..\\webconfig.ini";
            MapGuideApi.MgInitializeWebTier(configPath);

            // Connect to the site.
            MgUserInformation userInfo = new MgUserInformation(mgSessionId);
            MgSiteConnection siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);

            // Get an instance of the required service(s).
            MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

						MgMap map = Ut_SQL2TT.GetMapObject(resourceService);

            // Get map layers.
            MgLayerCollection mgLayers = map.GetLayers();
            MgLayer roadsLayer = mgLayers.GetItem("Wells_Background") as MgLayer;
            map.GetLayers()[1].Visible = true;
            // Save the updated map to apply the change
            //  map.Save(resourceService);
            //Response.Write("Parcels Layers visible togged!");

            //refresh the map using the viewer API
            //Response.Write("<script>parent.parent.Refresh();</script>");
        
        }
    }
}

