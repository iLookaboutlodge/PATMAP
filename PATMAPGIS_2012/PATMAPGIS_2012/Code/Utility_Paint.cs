using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

using OSGeo.MapGuide;


/// <summary>
/// Summary description for Utility.
/// Created by Daniel Du, DevTech
/// </summary>
public class Utility_Paint
{

    MgSiteConnection siteConnection;

    public void InitializeWebTier(HttpRequest Request)
    {
        string realPath =
            Request.ServerVariables["APPL_PHYSICAL_PATH"];
        String configPath = realPath + "../webconfig.ini";
        MapGuideApi.MgInitializeWebTier(configPath);
    }

    public void ConnectToServer(String sessionID)
    {
        MgUserInformation userInfo =
            new MgUserInformation(sessionID);
        siteConnection = new MgSiteConnection();
        siteConnection.Open(userInfo);
    }

    public MgSiteConnection GetSiteConnection()
    {
        return siteConnection;
    }


    public static MgByteSource ByteSourceFromXMLDoc(
                                XmlDocument xmlDoc)
    {
        //Save the amended DOM object to a memory stream
        MemoryStream XmlStream = new MemoryStream();
        xmlDoc.Save(XmlStream);

        //Now get the memory stream into a byte array
        //that can be read into an MgByteSource
        byte[] byteNewDef = XmlStream.ToArray();
        String sNewDef = new String(Encoding.UTF8
                            .GetChars(byteNewDef));
        byte[] byteNewDef2 =
                    new byte[byteNewDef.Length - 1];
        int iNewByteCount = Encoding.UTF8
            .GetBytes(sNewDef, 1, sNewDef.Length - 1,
                        byteNewDef2, 0);
        MgByteSource byteSource =
            new MgByteSource(byteNewDef2,
                            byteNewDef2.Length);
        byteSource.SetMimeType(MgMimeType.Xml);
        return byteSource;
    }

    public static string GetStringFromMemoryStream(
                                MemoryStream m)
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





    public static string GetXmlFromByteReader(
                            MgByteReader reader)
    {
        System.IO.MemoryStream ms =
                    new System.IO.MemoryStream();
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

    //TEST
    public void UpdateFilterForLayer(string sessionId, string layerResId )
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo =
                new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceIdentifier templateLayerId
            = new MgResourceIdentifier(layerResId);

        MgResourceService resSvc = siteConnection
            .CreateService(MgServiceType.ResourceService)
            as MgResourceService;
        //the resource content of LayerDefinition

        MgByteReader reader = resSvc
            .GetResourceContent(templateLayerId);
        string layoutXml = GetXmlFromByteReader(reader);

        XmlDocument doc = new XmlDocument();

        doc.LoadXml(layoutXml);

        MgByteSource byteSource = ByteSourceFromXMLDoc(doc);
        Ut_SQL2TT.CustomSetResource(resSvc, templateLayerId, byteSource);
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
    public string SetFilterForLayer(string sessionId,
                                    string layerResId,
                                    Dictionary<string, string> q_CondColor)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo =
                new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceIdentifier templateLayerId
            = new MgResourceIdentifier(layerResId);

        MgResourceService resSvc = siteConnection
            .CreateService(MgServiceType.ResourceService)
            as MgResourceService;
        //the resource content of LayerDefinition

        MgByteReader reader = resSvc
            .GetResourceContent(templateLayerId);
        string layoutXml = GetXmlFromByteReader(reader);

        //Edit the map resource in XmlDocument in DOM

        XmlDocument doc = new XmlDocument();

        doc.LoadXml(layoutXml);

        XmlNodeList objNodeList = doc.SelectNodes("//VectorLayerDefinition/VectorScaleRange/CompositeTypeStyle/CompositeRule");

        doc.GetElementsByTagName("CompositeTypeStyle")[0].RemoveAll();

        foreach (string q_key in q_CondColor.Keys)
        {

            string q_ProtoForClass = "<LegendLabel></LegendLabel><Filter>qz_VL_FILTER_FORCHJANGE_#123</Filter><CompositeSymbolization><SymbolInstance><SimpleSymbolDefinition><Name>Solid Fill</Name><Description>Default Area Symbol</Description><Graphics><Path><Geometry>M 0.0,0.0 h 100.0 v 100.0 h -100.0 z</Geometry><FillColor>%FILLCOLOR%</FillColor></Path></Graphics><AreaUsage><RepeatX>100.0</RepeatX><RepeatY>100.0</RepeatY></AreaUsage><ParameterDefinition><Parameter><Identifier>FILLCOLOR</Identifier><DefaultValue>0xffbfbfbf</DefaultValue><DisplayName>&amp;Fill Color</DisplayName><Description>Fill Color</Description><DataType>FillColor</DataType></Parameter></ParameterDefinition></SimpleSymbolDefinition><ParameterOverrides><Override><SymbolName>Solid Fill</SymbolName><ParameterIdentifier>FILLCOLOR</ParameterIdentifier><ParameterValue>q_Stryapo_Color_ForChane_@#$123456</ParameterValue></Override></ParameterOverrides><GeometryContext>Polygon</GeometryContext></SymbolInstance><SymbolInstance><SimpleSymbolDefinition><Name>Solid Line</Name><Description>Default Line Symbol</Description><Graphics><Path><Geometry>M 0.0,0.0 L 1.0,0.0</Geometry><LineColor>%LINECOLOR%</LineColor><LineWeight>%LINEWEIGHT%</LineWeight><LineWeightScalable>false</LineWeightScalable></Path></Graphics><LineUsage><Repeat>1.0</Repeat></LineUsage><ParameterDefinition><Parameter><Identifier>LINECOLOR</Identifier><DefaultValue>0xff000000</DefaultValue><DisplayName>Line &amp;Color</DisplayName><Description>Line Color</Description><DataType>LineColor</DataType></Parameter><Parameter><Identifier>LINEWEIGHT</Identifier><DefaultValue>0.0</DefaultValue><DisplayName>Line &amp;Thickness</DisplayName><Description>Line Thickness</Description><DataType>LineWeight</DataType></Parameter></ParameterDefinition></SimpleSymbolDefinition><ParameterOverrides><Override><SymbolName>Solid Line</SymbolName><ParameterIdentifier>LINECOLOR</ParameterIdentifier><ParameterValue>0xff000000</ParameterValue></Override><Override><SymbolName>Solid Line</SymbolName><ParameterIdentifier>LINEWEIGHT</ParameterIdentifier><ParameterValue>0.0</ParameterValue></Override></ParameterOverrides><GeometryContext>Polygon</GeometryContext></SymbolInstance></CompositeSymbolization>";
            q_ProtoForClass = q_ProtoForClass.Replace("qz_VL_FILTER_FORCHJANGE_#123", q_key);
            q_ProtoForClass = q_ProtoForClass.Replace("q_Stryapo_Color_ForChane_@#$123456", q_CondColor[q_key]);

            XmlNode childElement = doc.CreateElement("CompositeRule");
            childElement.InnerXml = q_ProtoForClass;
            //childElement.InnerText = q_ProtoForClass;
            doc.GetElementsByTagName("CompositeTypeStyle")[0].AppendChild(childElement);

        }

        MgByteSource byteSource = ByteSourceFromXMLDoc(doc);
        string sessionLayerName = templateLayerId.GetName();
        string sessionLayer = "Session:" + sessionId + @"//" + sessionLayerName + ".LayerDefinition";
        MgResourceIdentifier sessionLayerResId = new MgResourceIdentifier(sessionLayer);
        //resSvc.SetResource(sessionLayerResId,byteSource.GetReader(),null);
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
    public void ReplaceWithFilteredLayer(
                    string sessionId,
                    string sessionLayerResId,
                    string originalLayerName,
                    string mapName)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo =
                new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceService resSvc = siteConnection
            .CreateService(MgServiceType.ResourceService)
            as MgResourceService;

        MgResourceIdentifier filterLayerId =
            new MgResourceIdentifier(sessionLayerResId);
        MgLayer filteredLayer =
            new MgLayer(filterLayerId, resSvc);

        MgMap map = Ut_SQL2TT.GetMapObject(resSvc);

        MgLayer oriLayer;
        try
        {
            oriLayer = map.GetLayers()
                .GetItem(originalLayerName) as MgLayer;

        }
        catch (MgObjectNotFoundException)
        {
            oriLayer = map.GetLayers()
                .GetItem(originalLayerName + "_filtered")
                as MgLayer;
            originalLayerName = originalLayerName
                + "_filtered";
        }

        filteredLayer.LegendLabel =
             oriLayer.LegendLabel.Contains("_filtered")
            ? oriLayer.LegendLabel
            : oriLayer.LegendLabel + "_filtered";
        filteredLayer.Selectable = oriLayer.Selectable;
        filteredLayer.DisplayInLegend =
                        oriLayer.DisplayInLegend;
        filteredLayer.Name =
            oriLayer.Name.Contains("_filtered")
            ? oriLayer.Name
            : oriLayer.Name + "_filtered";
        filteredLayer.Group = oriLayer.Group;

        int index = map.GetLayers()
                    .IndexOf(originalLayerName);
        map.GetLayers().RemoveAt(index);
        map.GetLayers().Insert(index, filteredLayer);

        map.Save(resSvc);
        //test
        map.Save();

    }

    public string GetLayerDefinitionResourceId(
                          string layerName,
                          string sessionId,
                          string mapName)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo
                = new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceService resSvc = siteConnection
            .CreateService(MgServiceType.ResourceService)
            as MgResourceService;

        MgMap map = Ut_SQL2TT.GetMapObject(resSvc);

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

    public static void qF_PaintIt(string mapName, string layerName, Dictionary<string, string> q_CondColor)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        //string session = Session["Session"].ToString();

        Utility_Paint utility = new Utility_Paint();
        //utility.InitializeWebTier(Request);
        utility.ConnectToServer(session);
        MgSiteConnection siteConnection =
                utility.GetSiteConnection();

        //string webLayout =1Session["WebLayout"].ToString();

        string layerDefId = utility
                .GetLayerDefinitionResourceId(
                                                layerName,
                                                session,
                                                mapName);

        //create a filtered session layer defination
        string sessionLayerId = utility
                .SetFilterForLayer(session,
                                                        layerDefId,
                                                        q_CondColor);

        //create a session layer object,
        //replace it with the library layer
        utility.ReplaceWithFilteredLayer(
                                                        session,
                                                        sessionLayerId,
                                                        layerName,
                                                        mapName);

    }
    public static string CorrelationForTaxShift(string q_Criterion, string q_strRaw1, string q_strRaw2, Boolean q_IsPercent)
    {
        if (q_IsPercent)
        {
            int j_dTempo = 1;
        }
        else
        {
            int j_dTempo = 0;
        }

        //string q_cond = q_Criterion + " &gt; " + q_strRaw1 + " and " + q_Criterion + " &lt; " + q_strRaw2;
        string q_cond = q_Criterion + " &gt;= " + q_strRaw1 + " and " + q_Criterion + " &lt; " + q_strRaw2;
        //string q_cond = q_Criterion + " &gte; " + q_strRaw1 + " and " + q_Criterion + " &lt; " + q_strRaw2;

        return q_cond;
    }
    public static string ConvertIndexColorsToMapColors(string q_str)
    {
        return MapManagerNew.convertToColour(Convert.ToInt32(q_str)).Replace("#", "0xFF");
    }
    public static Dictionary<string, string> RawDictionaryToOfficial(Dictionary<string, string> q_RawDict, string q_Criterion, Boolean q_IsPercent)
    {
        Dictionary<string, string> q_CondColor = new Dictionary<string, string>();
        foreach (string q_key in q_RawDict.Keys)
        {
            string[] q_strRaw = q_key.Split(' ');
            q_CondColor.Add(CorrelationForTaxShift(q_Criterion, q_strRaw[0], q_strRaw[1], q_IsPercent),
            ConvertIndexColorsToMapColors(q_RawDict[q_key]));
        }
        return q_CondColor;
    }

    //test change colors
    public static Dictionary<string, string> TEST_RawDictionaryToOfficial(Dictionary<string, string> q_RawDict)
    {
        Dictionary<string, string> q_CondColor = new Dictionary<string, string>();
        q_CondColor.Add("", ConvertIndexColorsToMapColors("110"));
        return q_CondColor;
    }

    public static Dictionary<string, string> BASource_RawDictionaryToOfficial(Dictionary<string, string> q_RawDict, string q_Criterion, Boolean q_IsPercent)
    {
        Dictionary<string, string> q_CondColor = new Dictionary<string, string>();
        //q_CondColor.Add("", ConvertIndexColorsToMapColors("255"));
        q_CondColor.Add("", ConvertIndexColorsToMapColors("110"));
        return q_CondColor;
    }

    public static Dictionary<string, string> BADestination_RawDictionaryToOfficial(Dictionary<string, string> q_RawDict, string q_Criterion, Boolean q_IsPercent)
    {
        Dictionary<string, string> q_CondColor = new Dictionary<string, string>();
        //q_CondColor.Add("", ConvertIndexColorsToMapColors("255"));
        q_CondColor.Add("", ConvertIndexColorsToMapColors("19"));
        return q_CondColor;
    }

    public string SetFeatureNameForLayer(string sessionId,
                                                                    string layerResId,
                                                                    string q_FeatureName)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo =
                    new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceIdentifier templateLayerId
                = new MgResourceIdentifier(layerResId);

        MgResourceService resSvc = siteConnection
                .CreateService(MgServiceType.ResourceService)
                as MgResourceService;
        //the resource content of LayerDefinition

        MgByteReader reader = resSvc
                .GetResourceContent(templateLayerId);
        string layoutXml = GetXmlFromByteReader(reader);

        //Edit the map resource in XmlDocument in DOM

        XmlDocument doc = new XmlDocument();

        doc.LoadXml(layoutXml);

        XmlNodeList objNodeList = doc.SelectNodes("//VectorLayerDefinition");

        doc.GetElementsByTagName("FeatureName").Item(0).InnerText = q_FeatureName;
        doc.GetElementsByTagName("FeatureName").Item(0).InnerXml = q_FeatureName;

        //doc.SelectNodes("//VectorLayerDefinition/FeatureName")

        MgByteSource byteSource = ByteSourceFromXMLDoc(doc);
        string sessionLayerName = templateLayerId.GetName();
        string sessionLayer = "Session:" + sessionId + @"//" + sessionLayerName + ".LayerDefinition";
        MgResourceIdentifier sessionLayerResId = new MgResourceIdentifier(sessionLayer);
        //resSvc.SetResource(sessionLayerResId, byteSource.GetReader(), null);
        Ut_SQL2TT.CustomSetResource(resSvc, sessionLayerResId, byteSource);
        return sessionLayer;
    }
    public static void qF_SetFeatureName(string mapName, string layerName, string q_FeatureName)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        //string session = Session["Session"].ToString();

        Utility_Paint utility = new Utility_Paint();
        //utility.InitializeWebTier(Request);
        utility.ConnectToServer(session);
        MgSiteConnection siteConnection =
                utility.GetSiteConnection();

        //string webLayout =1Session["WebLayout"].ToString();

        string layerDefId = utility
                .GetLayerDefinitionResourceId(
                                                layerName,
                                                session,
                                                mapName);

        //create a filtered session layer defination
        string sessionLayerId = utility
                .SetFeatureNameForLayer(session,
                                                        layerDefId,
                                                        q_FeatureName);

        //create a session layer object,
        //replace it with the library layer
        utility.ReplaceWithFilteredLayer(
                                                        session,
                                                        sessionLayerId,
                                                        layerName,
                                                        mapName);

    }
    public static void qF_SetLayerFeatureInXML(string mapName, string layerName, string q_FeatureName, string q_FeatureContent)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        //string session = Session["Session"].ToString();

        Utility_Paint utility = new Utility_Paint();
        //utility.InitializeWebTier(Request);
        utility.ConnectToServer(session);
        MgSiteConnection siteConnection =
                utility.GetSiteConnection();

        //string webLayout =1Session["WebLayout"].ToString();

        string layerDefId = utility
                .GetLayerDefinitionResourceId(
                                                layerName,
                                                session,
                                                mapName);

        //create a filtered session layer defination
        string sessionLayerId = utility
                .SetFeatureForLayer(session,
                                                        layerDefId,
                                                        q_FeatureName,
                                                        q_FeatureContent);

        //create a session layer object,
        //replace it with the library layer
        utility.ReplaceWithFilteredLayer(
                                                        session,
                                                        sessionLayerId,
                                                        layerName,
                                                        mapName);

    }

    public string SetFeatureForLayer(string sessionId,
                                                                    string layerResId,
                                                                    string q_FeatureName,
                                                                    string q_FeatureContent)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo =
                    new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceIdentifier templateLayerId
                = new MgResourceIdentifier(layerResId);

        MgResourceService resSvc = siteConnection
                .CreateService(MgServiceType.ResourceService)
                as MgResourceService;
        //the resource content of LayerDefinition

        MgByteReader reader = resSvc
                .GetResourceContent(templateLayerId);
        string layoutXml = GetXmlFromByteReader(reader);

        //Edit the map resource in XmlDocument in DOM

        XmlDocument doc = new XmlDocument();

        doc.LoadXml(layoutXml);
        MgByteSource byteSource = ByteSourceFromXMLDoc(doc);
        string sessionLayerName = templateLayerId.GetName();
        string sessionLayer = "Session:" + sessionId + @"//" + sessionLayerName + ".LayerDefinition";
        MgResourceIdentifier sessionLayerResId = new MgResourceIdentifier(sessionLayer);
        //resSvc.SetResource(sessionLayerResId, byteSource.GetReader(), null);
        Ut_SQL2TT.CustomSetResource(resSvc, sessionLayerResId, byteSource);
        return sessionLayer;
    }




    public string GetSHP(string sessionId,
                                                                string layerResId,
                                                                string q_FeatureName)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo =
                    new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceIdentifier templateLayerId
                = new MgResourceIdentifier(layerResId);

        MgResourceService resSvc = siteConnection
                .CreateService(MgServiceType.ResourceService)
                as MgResourceService;
        //the resource content of LayerDefinition

        MgByteReader reader = resSvc
                .GetResourceContent(templateLayerId);
        string layoutXml = GetXmlFromByteReader(reader);

        //Edit the map resource in XmlDocument in DOM

        XmlDocument doc = new XmlDocument();

        doc.LoadXml(layoutXml);

        doc.GetElementsByTagName("ToolTip")[0].InnerText = "SELECT TOP 1 UMNM from MunicipalitiesMapLink";
        doc.GetElementsByTagName("ToolTip")[0].InnerXml = "SELECT TOP 1 UMNM from MunicipalitiesMapLink";

        MgByteSource byteSource = ByteSourceFromXMLDoc(doc);
        string sessionLayerName = templateLayerId.GetName();
        string sessionLayer = "Session:" + sessionId + @"//" + sessionLayerName + ".LayerDefinition";
        MgResourceIdentifier sessionLayerResId = new MgResourceIdentifier(sessionLayer);
        //resSvc.SetResource(sessionLayerResId, byteSource.GetReader(), null);
        Ut_SQL2TT.CustomSetResource(resSvc, sessionLayerResId, byteSource);
        return sessionLayer;
    }
    public static string GetValueFromInnerXml(string q_OriString, string q_Pattern)
    {
        string q_From = "<" + q_Pattern + ">";
        string q_To = "</" + q_Pattern + ">";
        string q_RetVal = "";
        if (q_OriString.ToUpper().Contains(q_From.ToUpper()) && q_OriString.ToUpper().Contains(q_To.ToUpper()))
        {
            int q_i1 = q_OriString.ToUpper().IndexOf(q_From.ToUpper());
            int q_i2 = q_OriString.ToUpper().IndexOf(q_To.ToUpper());
            if (q_i2 >= q_i1)
            {
                q_RetVal = q_OriString.Substring(q_i1 + q_From.Length, q_i2 - q_i1 - q_From.Length);
            }
        }
        return q_RetVal;
    }


    public string SetFilterForLayerSimlpe(string sessionId, string layerResId)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo =
                    new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceIdentifier templateLayerId
                = new MgResourceIdentifier(layerResId);

        MgResourceService resSvc = siteConnection
                .CreateService(MgServiceType.ResourceService)
                as MgResourceService;
        //the resource content of LayerDefinition

        MgByteReader reader = resSvc
                .GetResourceContent(templateLayerId);
        string layoutXml = GetXmlFromByteReader(reader);

        //Edit the map resource in XmlDocument in DOM

        XmlDocument doc = new XmlDocument();

        doc.LoadXml(layoutXml);


        MgByteSource byteSource = ByteSourceFromXMLDoc(doc);
        string sessionLayerName = templateLayerId.GetName();
        string sessionLayer = "Session:" + sessionId + @"//" + sessionLayerName + ".LayerDefinition";
        MgResourceIdentifier sessionLayerResId = new MgResourceIdentifier(sessionLayer);
        //resSvc.SetResource(sessionLayerResId, byteSource.GetReader(), null);
        Ut_SQL2TT.CustomSetResource(resSvc, sessionLayerResId, byteSource);
        return sessionLayer;
    }
    public string SetFilterForLayerSimlpe(string sessionId, string layerResId, string tooltip)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo =
                    new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceIdentifier templateLayerId
                = new MgResourceIdentifier(layerResId);

        MgResourceService resSvc = siteConnection
                .CreateService(MgServiceType.ResourceService)
                as MgResourceService;
        //the resource content of LayerDefinition

        MgByteReader reader = resSvc
                .GetResourceContent(templateLayerId);
        string layoutXml = GetXmlFromByteReader(reader);

        //Edit the map resource in XmlDocument in DOM

        XmlDocument doc = new XmlDocument();

        doc.LoadXml(layoutXml);
        doc.GetElementsByTagName("ToolTip")[0].InnerText = tooltip;

        MgByteSource byteSource = ByteSourceFromXMLDoc(doc);
        string sessionLayerName = templateLayerId.GetName();
        string sessionLayer = "Session:" + sessionId + @"//" + sessionLayerName + ".LayerDefinition";
        MgResourceIdentifier sessionLayerResId = new MgResourceIdentifier(sessionLayer);
        //resSvc.SetResource(sessionLayerResId, byteSource.GetReader(), null);
        Ut_SQL2TT.CustomSetResource(resSvc, sessionLayerResId, byteSource);
        return sessionLayer;
    }

    //new added on 24-mar-2017
    public void SaveMap(string sessionId, string mapName)
    {
        if (siteConnection == null)
        {
            MgUserInformation userInfo =
                new MgUserInformation(sessionId);
            siteConnection = new MgSiteConnection();
            siteConnection.Open(userInfo);
        }

        MgResourceService resSvc = siteConnection
            .CreateService(MgServiceType.ResourceService)
            as MgResourceService;

        MgMap map = Ut_SQL2TT.GetMapObject(resSvc);
        
        map.Save(resSvc);
        //test
        map.Save();
    }

}

