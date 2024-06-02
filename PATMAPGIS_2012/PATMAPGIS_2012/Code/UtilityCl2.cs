using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using OSGeo.MapGuide;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;
using AIMS2012ToolLib;

public class UtilityCl2
{

    // the mapguide connection site object 
    MgSiteConnection siteConnection;
    string currentPath;



    string selectionResult;
    string selectString;


    public string SelectString
    {
        get { return selectString; }
        set { selectString = value; }
    }

    public string SelectionResult
    {
        get { return selectionResult; }
        set { selectionResult = value; }
    }

    public MgSiteConnection GetSiteConnection()
    {
        return siteConnection;
    }

    // initialize the web tier, "webconfig.ini" file is supposed to be copied to current folder of web application 
    // from C:\Program Files\Autodesk\MapGuideEnterprise2011\WebServerExtensions\www\webconfig.ini
    public void InitializeWebTier(HttpRequest Request)
    {
        string realPath = Request.ServerVariables["APPL_PHYSICAL_PATH"];
        String configPath = realPath + "webconfig.ini";
        MapGuideApi.MgInitializeWebTier(configPath);

        //Save the current Path
        currentPath = realPath;
    }

    // connect to server with specifed session string
    public void ConnectToServer(String sessionID)
    {
        MgUserInformation userInfo = new MgUserInformation(sessionID);
        siteConnection = new MgSiteConnection();
        siteConnection.Open(userInfo);
    }

    //Connect to MapGuide server with specifed username/pwd
    public void ConnectToServer()
    {
        MgUserInformation userInfo = new MgUserInformation("Administrator", "admin");
        siteConnection = new MgSiteConnection();
        siteConnection.Open(userInfo);
    }

    public void OpenMap(string mapName)
    {

        MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        MgMap map = Ut_SQL2TT.GetMapObject(resSvc);
    }

    public string GetMapInformation(string mapName)
    {
        StringBuilder sb = new StringBuilder();

        MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        MgMap map = Ut_SQL2TT.GetMapObject(resSvc);

        sb.Append("Map Name :" + mapName + "<hr>");
        sb.Append("Coordinate System: " + map.GetMapSRS().ToString() + "<br><br>");

        sb.Append("Map layers count: " + map.GetLayers().Count.ToString() + "<br>");
        for (int i = 0; i < map.GetLayers().Count; i++)
        {
            //get layer name
            sb.Append(map.GetLayers()[i].Name + "   -    " + "<br>");


        }



        return sb.ToString();
    }





    public MgByteSource ByteSourceFromXMLDoc(XmlDocument xmlDoc)
    {
        //Save the amended DOM object to a memory stream
        MemoryStream XmlStream = new MemoryStream(); xmlDoc.Save(XmlStream);

        //Now get the memory stream into a byte array 
        //that can be read into an MgByteSource
        byte[] byteNewDef = XmlStream.ToArray(); String sNewDef = new String(Encoding.UTF8.GetChars(byteNewDef));
        byte[] byteNewDef2 = new byte[byteNewDef.Length - 1];
        int iNewByteCount = Encoding.UTF8.GetBytes(sNewDef, 1, sNewDef.Length - 1, byteNewDef2, 0);
        MgByteSource byteSource = new MgByteSource(byteNewDef2, byteNewDef2.Length);
        byteSource.SetMimeType(MgMimeType.Xml);
        return byteSource;
    }

    public static string GetStringFromMemoryStream(MemoryStream m)
    {
        if (m == null || m.Length == 0)
            return null;

        m.Flush();
        m.Position = 0;
        StreamReader sr = new StreamReader(m);
        string s = sr.ReadToEnd();

        return s;
    }

    public static MemoryStream
        GetMemoryStreamFromString(string s)
    {
        if (s == null || s.Length == 0)
            return null;

        MemoryStream m = new MemoryStream();
        StreamWriter sw = new StreamWriter(m);
        sw.Write(s);
        sw.Flush();

        return m;
    }




    private static string GetXmlFromByteReader(MgByteReader reader)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        byte[] buf = new byte[8 * 1024];
        int read = 1;
        while (read != 0)
        {
            read = reader.Read(buf, buf.Length);
            ms.Write(buf, 0, read);
        }

        string layoutXml = GetStringFromMemoryStream(ms);
        return layoutXml;
    }

    /// <summary>
    /// use a layer in reposorty as template, 
    /// to create a temp layer 
    /// with filter, and set resource in repository
    /// </summary>
    /// <param name="map"></param>
    /// <param name="layerResId">the resource id 
    ///     of template layer defination </param>
    /// <param name="strFilter">filter string</param>
    /// <returns>resource id of temp layer in 
    ///     repository</returns>
    public string SetFilterForLayer(string sessionId, string layerResId, string strFilter)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo = new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceIdentifier templateLayerId = new MgResourceIdentifier(layerResId);

        MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;
        //the resource content of LayerDefinition

        MgByteReader reader = resSvc.GetResourceContent(templateLayerId);
        string layoutXml = GetXmlFromByteReader(reader);

        //Edit the map resource in XmlDocument in DOM

        XmlDocument doc = new XmlDocument();

        doc.LoadXml(layoutXml);

        XmlNodeList objNodeList = doc.SelectNodes("//VectorLayerDefinition/Filter");

        if (objNodeList.Count > 0)
        {
            objNodeList.Item(0).InnerXml = strFilter;
        }
        else
        {
            XmlNode filterNode;
            filterNode = doc.CreateElement("Filter");
            filterNode.InnerText = strFilter;
            doc.GetElementsByTagName("VectorLayerDefinition")[0].AppendChild(filterNode);
        }

        MgByteSource byteSource = ByteSourceFromXMLDoc(doc);
        string sessionLayerName = templateLayerId.GetName();
        string sessionLayer = "Session:" + sessionId + @"//" + sessionLayerName + ".LayerDefinition";
        MgResourceIdentifier sessionLayerResId = new MgResourceIdentifier(sessionLayer);

        //resSvc.SetResource(sessionLayerResId, byteSource.GetReader(), null);
        Ut_SQL2TT.CustomSetResource(resSvc, sessionLayerResId, byteSource);

        return sessionLayer;
    }

    /// <summary>
    /// Create a MgLayer object using the session layer definition,
    /// Add it to MgMap and remove the original one
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="sessionLayerResId"></param>
    /// <param name="originalLayerName"></param>
    /// <param name="mapName"></param>
    public void ReplaceWithFilteredLayer(string sessionId, string sessionLayerResId, string originalLayerName, string mapName)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo = new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        MgResourceIdentifier filterLayerId = new MgResourceIdentifier(sessionLayerResId);
        MgLayer filteredLayer = new MgLayer(filterLayerId, resSvc);

        MgMap map = Ut_SQL2TT.GetMapObject(resSvc);

        MgLayer oriLayer;
        try
        {
            oriLayer = map.GetLayers().GetItem(originalLayerName) as MgLayer;
        }
        catch (MgObjectNotFoundException)
        {
            oriLayer = map.GetLayers().GetItem(originalLayerName + "_filtered") as MgLayer;
            originalLayerName = originalLayerName + "_filtered";
        }

        filteredLayer.LegendLabel = oriLayer.LegendLabel.Contains("_filtered") ? oriLayer.LegendLabel : oriLayer.LegendLabel + "_filtered";
        filteredLayer.Selectable = oriLayer.Selectable;
        filteredLayer.DisplayInLegend = oriLayer.DisplayInLegend;
        filteredLayer.Name = oriLayer.Name.Contains("_filtered") ? oriLayer.Name : oriLayer.Name + "_filtered";
        filteredLayer.Group = oriLayer.Group;

        int index = map.GetLayers().IndexOf(originalLayerName);
        map.GetLayers().RemoveAt(index);
        map.GetLayers().Insert(index, filteredLayer);

        map.Save(resSvc);
        //test
        map.Save();
    }

    public string GetLayerDefinitionResourceId(string layerName, string sessionId, string mapName)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo = new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        MgMap map = Ut_SQL2TT.GetMapObject(resSvc);

        // MgCoordinateXY mXY = map.GetMapDefinition();
        MgLayerCollection layerColl = map.GetLayers();
        foreach (MgLayerBase ly in layerColl)
        {
            if (ly.Name == layerName
                || ly.Name == layerName + "_filtered")
            {
                return ly.GetLayerDefinition().ToString();
            }
        }

        return string.Empty;
    }

    // public string GetMapDefinitionResourceId(string sessionId, string mapName)
    // {
    //if (siteConnection == null)
    //{
    //    MgUserInformation userInfo = new MgUserInformation(sessionId);
    //    siteConnection = new MgSiteConnection();
    //    siteConnection.Open(userInfo);
    //}

    //MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

    ////MgMapCollection mapCol = map.MapDefinition();
    //// MgCoordinateXY mXY = map.GetMapDefinition();
    //MgLayerCollection layerColl = map.GetLayers();
    //foreach (MgLayerBase ly in layerColl)
    //{
    //    if (ly.Name == layerName
    //        || ly.Name == layerName + "_filtered")
    //    {
    //        return ly.GetLayerDefinition().ToString();
    //    }
    //}

    //return string.Empty;
    //  }

    public bool ListSelections()
    {
        MgResourceService resService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
        MgFeatureService featService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);

        MgMap map = Ut_SQL2TT.GetMapObject(resService);

        MgSelection mapSelection = new MgSelection(map);
        mapSelection.Open(resService, MapSettings.CurrentMapName);

        MgLayerCollection mgLayers = map.GetLayers();
        MgLayer roadsLayer = mgLayers.GetItem(1) as MgLayer;
        if (mgLayers != null)
        {
            OutputSelectionInHTML(mgLayers, featService);
            return true;
        }
        else
        {
            return false;
        }
    }

    public string OutputSelectionInHTML(MgLayerCollection layers, MgFeatureService featureService)
    {
        //MgReadOnlyLayerCollection layers = selection.GetLayers();
        String outString = null;

        if (layers != null)
        {
            for (int i = 0; i < layers.GetCount(); i++)
            {
                MgLayer layer = layers.GetItem(i) as MgLayer;
                if ((layer != null))
                {
                    String layerClassName = layer.GetFeatureClassName();
                    //selectString = selection.GenerateFilter(layer, layerClassName);

                    String layerFeatureIdString = layer.GetFeatureSourceId();
                    MgResourceIdentifier layerResId = new MgResourceIdentifier(layerFeatureIdString);

                    //MgFeatureQueryOptions queryOptions = new MgFeatureQueryOptions();
                    //queryOptions.SetFilter(selectString);
                    //MgFeatureReader featReader = featureService.SelectFeatures(layerResId, layerClassName, queryOptions);

                    outString = outString + "<table border=\"1\">\n";

                    double acre = 0;

                    //while (featReader.ReadNext())
                    //{
                    outString = outString + "<tr>\n";

                    outString = outString + "<td>";
                    outString = outString + layer.LegendLabel;
                    outString = outString + "</td>\n";
                    //outString = outString + "<td>";
                    //outString = outString + featReader.GetString("RPROPAD");
                    //outString = outString + "</td>\n";
                    //outString = outString + "<td>";
                    //String acreString = featReader.GetString("RACRE");
                    //acre = acre + (acreString == "" ? 0 : Convert.ToDouble(acreString));
                    //outString = outString + acreString;
                    outString = outString + "</tr>\n";

                    //}
                    outString = outString + "</table>\n";

                    //important
                    // featReader.Close();
                }
            }
        }

        selectionResult = outString;
        return selectionResult;
    }
    //
    //
    // get the layer by its name 
    //
    //
    public static MgLayerBase getLayerByName(MgMap map, String layerName)
    {
        MgLayerBase layer = null;
        for (int i = 0; i < map.GetLayers().GetCount(); i++)
        {
            MgLayerBase nextLayer = map.GetLayers().GetItem(i);
            if (nextLayer.GetName() == layerName)
            {
                layer = nextLayer;
                break;
            }

        }
        return layer;
    }

    public static MgLayerBase getLayerByName(string mapName, String layerName)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        UtilityClass utility = new UtilityClass();
        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        MgMap map = Ut_SQL2TT.GetMapObject(resourceService);

        MgLayerBase layer = null;
        for (int i = 0; i < map.GetLayers().GetCount(); i++)
        {
            MgLayerBase nextLayer = map.GetLayers().GetItem(i);
            if (nextLayer.GetName() == layerName)
            {
                layer = nextLayer;
                break;
            }

        }
        return layer;
    }
    public static void ProgressIconOn(Page page)
    {
        page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Zoom", "screenBusyOn();", true);
    }
    public static void ProgressIconOff(Page page)
    {
        page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Zoom1", "screenBusyOff();", true);
    }


}
