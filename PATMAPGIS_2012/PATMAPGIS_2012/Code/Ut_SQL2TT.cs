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
using System.Xml;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using OSGeo.MapGuide;


public class Ut_SQL2TT : AnalysisControl
{
    MgSiteConnection siteConnection;

    //test
    public static void Test_UpdateResource(string mapName, string layerName)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        Utility_Paint utility = new Utility_Paint();
        utility.ConnectToServer(session);
        MgSiteConnection siteConnection =
                utility.GetSiteConnection();

        string layerDefId = utility
                .GetLayerDefinitionResourceId(
                                                layerName,
                                                session,
                                                mapName);
        utility.UpdateFilterForLayer(session, layerDefId);
    }
    
    
    //new added on 24-mar-2017
    public static void qF_PaintItNew(string q_LayerName, string q_MapName, Dictionary<string, string> q_CondColor)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        Utility_Paint utility = new Utility_Paint();
        utility.ConnectToServer(session);

        MgSiteConnection siteConnection = utility.GetSiteConnection();
        MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        //MgFeatureService featuerSvc = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);

        MgMap map = GetMapObject(resSvc);
        string layerResId = utility.GetLayerDefinitionResourceId(q_LayerName, session, q_MapName);
        MgResourceIdentifier layerDefResId = new MgResourceIdentifier(layerResId);

        XmlDocument doc = new XmlDocument();

        MgByteReader reader = resSvc.GetResourceContent(layerDefResId);
        string layoutXml = Utility_Paint.GetXmlFromByteReader(reader);

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

        MgByteSource byteSource = Utility_Paint.ByteSourceFromXMLDoc(doc);

        CustomSetResource(resSvc, layerDefResId, byteSource);

        byteSource.Dispose();
        resSvc.Dispose();
    }

    //test changing xml doc
    public static void ChangeColorsTooltips(string mapName, string layerName, string tooltip, Dictionary<string, string> q_CondColor)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        Utility_Paint utility = new Utility_Paint();
        utility.ConnectToServer(session);

        MgSiteConnection siteConnection = utility.GetSiteConnection();
        MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        //MgFeatureService featuerSvc = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);

        MgMap map = GetMapObject(resSvc);
        //string layerResId = utility.GetLayerDefinitionResourceId(q_LayerName, session, q_MapName);

        string layerResId = null;
        MgLayerCollection layerColl = map.GetLayers();
        foreach (MgLayerBase ly in layerColl)
        {
            if (ly.Name == layerName)
            {
                layerResId = ly.GetLayerDefinition().ToString();
            }
        }

        if (string.IsNullOrEmpty(layerResId))
        {
            throw new Exception("Invalid Layer Name " + layerName);
        }

        
        MgResourceIdentifier layerDefResId = new MgResourceIdentifier(layerResId);

        XmlDocument doc = new XmlDocument();

        MgByteReader reader = resSvc.GetResourceContent(layerDefResId);
        string layoutXml = Utility_Paint.GetXmlFromByteReader(reader);

        doc.LoadXml(layoutXml);

        //change tooltip
        if (! string.IsNullOrEmpty(tooltip) ) {
            doc.GetElementsByTagName("ToolTip")[0].InnerText = tooltip;
        }

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

        MgByteSource byteSource = Utility_Paint.ByteSourceFromXMLDoc(doc);

        CustomSetResource(resSvc, layerDefResId, byteSource);

        byteSource.Dispose();
        resSvc.Dispose();
    }

    //TEST
    //separate procedure for Parcels    19-apr-2017
    public static void qF_changePrimeAndSecondaryPropOfFeatureSourceParcel(
        string q_LayerName, string q_MapName, string q_FeatureClassProperty_1, string q_AttributeClassProperty_1,
        string q_AttributeClass, string q_ResourceId, string q_NameInside, string q_Name, string q_FeatureClass)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        //string weblayout = System.Web.HttpContext.Current.Session["WebLayout"].ToString();

        UtilityCl2 utility = new UtilityCl2();

        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        MgFeatureService featuerSvc = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);

        MgMap map = GetMapObject(resourceService);

        MgLayerBase q_Layer = UtilityCl2.getLayerByName(map, q_LayerName);
        string resId = q_Layer.GetFeatureSourceId();
        MgResourceIdentifier layerFeatureIdString = new MgResourceIdentifier(resId);

        XmlDocument doc = new XmlDocument();
        MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;
        MgByteReader reader = resSvc.GetResourceContent(layerFeatureIdString);
        string layoutXml = Utility_Paint.GetXmlFromByteReader(reader);

        doc.LoadXml(layoutXml);

        // Delete all 'Extension' from FeatureSource
        while (doc.GetElementsByTagName("Extension").Count > 0)
        {
            XmlNodeList q_toDeleteLsit = doc.GetElementsByTagName("Extension");
            XmlNode root = doc.DocumentElement;
            root.RemoveChild(q_toDeleteLsit.Item(0));
        }

        //TODO because of lack of Time, later make normal Node way
        //change made to Inner join instead of left join for BoundaryChange Map 25-oct-2013
        string q_ProtoForClass1 = "";
        if (MapSettings.CurrentMapName == ConfigurationManager.AppSettings["AutodeskBoundaryChangeMapName"])
        {
            q_ProtoForClass1 = "<RelateProperty><FeatureClassProperty>q_FeatureClassProperty_1wewewe</FeatureClassProperty><AttributeClassProperty>q_AttributeClassProperty_1wewewe</AttributeClassProperty></RelateProperty><AttributeClass>q_AttributeClass_wewewe</AttributeClass><ResourceId>q_ResourceId_wewewe</ResourceId><Name>q_Name_wewewe</Name><AttributeNameDelimiter /><RelateType>Inner</RelateType><ForceOneToOne>true</ForceOneToOne>";
        }
        else
        {
            //q_ProtoForClass1 = "<RelateProperty><FeatureClassProperty>q_FeatureClassProperty_1wewewe</FeatureClassProperty><AttributeClassProperty>q_AttributeClassProperty_1wewewe</AttributeClassProperty></RelateProperty><AttributeClass>q_AttributeClass_wewewe</AttributeClass><ResourceId>q_ResourceId_wewewe</ResourceId><Name>q_Name_wewewe</Name><AttributeNameDelimiter /><RelateType>LeftOuter</RelateType><ForceOneToOne>true</ForceOneToOne>";
            q_ProtoForClass1 = "<RelateProperty><FeatureClassProperty>q_FeatureClassProperty_1wewewe</FeatureClassProperty><AttributeClassProperty>q_AttributeClassProperty_1wewewe</AttributeClassProperty></RelateProperty><RelateProperty><FeatureClassProperty>FeatId</FeatureClassProperty><AttributeClassProperty>FeatID</AttributeClassProperty></RelateProperty><AttributeClass>q_AttributeClass_wewewe</AttributeClass><ResourceId>q_ResourceId_wewewe</ResourceId><Name>q_Name_wewewe</Name><AttributeNameDelimiter /><RelateType>LeftOuter</RelateType><ForceOneToOne>true</ForceOneToOne>";
            //q_ProtoForClass1 = "<RelateProperty><FeatureClassProperty>FeatId</FeatureClassProperty><AttributeClassProperty>FeatID</AttributeClassProperty></RelateProperty><AttributeClass>q_AttributeClass_wewewe</AttributeClass><ResourceId>q_ResourceId_wewewe</ResourceId><Name>q_Name_wewewe</Name><AttributeNameDelimiter /><RelateType>LeftOuter</RelateType><ForceOneToOne>true</ForceOneToOne>";
        }
        q_ProtoForClass1 = q_ProtoForClass1.Replace("q_FeatureClassProperty_1wewewe", q_FeatureClassProperty_1);
        q_ProtoForClass1 = q_ProtoForClass1.Replace("q_AttributeClassProperty_1wewewe", q_AttributeClassProperty_1);
        q_ProtoForClass1 = q_ProtoForClass1.Replace("q_AttributeClass_wewewe", q_AttributeClass);
        q_ProtoForClass1 = q_ProtoForClass1.Replace("q_ResourceId_wewewe", q_ResourceId);
        q_ProtoForClass1 = q_ProtoForClass1.Replace("q_Name_wewewe", q_NameInside);

        XmlNode childElementAttributeRelate = doc.CreateElement("AttributeRelate");
        childElementAttributeRelate.InnerXml = q_ProtoForClass1;

        XmlNode childElementName = doc.CreateElement("Name");
        childElementName.InnerText = q_Name;

        XmlNode childElementFeatureClass = doc.CreateElement("FeatureClass");
        childElementFeatureClass.InnerText = q_FeatureClass;

        XmlNode childElementExtension = doc.CreateElement("Extension");
        childElementExtension.AppendChild(childElementAttributeRelate);
        childElementExtension.AppendChild(childElementName);
        childElementExtension.AppendChild(childElementFeatureClass);

        doc.GetElementsByTagName("FeatureSource")[0].AppendChild(childElementExtension);
        MgByteSource byteSource = Utility_Paint.ByteSourceFromXMLDoc(doc);

        CustomSetResource(resSvc, layerFeatureIdString, byteSource);

        byteSource.Dispose();
        resSvc.Dispose();
    }


    public static void qF_changePrimeAndSecondaryPropOfFeatureSource(
        string q_LayerName, string q_MapName, string q_FeatureClassProperty_1, string q_AttributeClassProperty_1,
        string q_AttributeClass, string q_ResourceId, string q_NameInside, string q_Name, string q_FeatureClass)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        //string weblayout = System.Web.HttpContext.Current.Session["WebLayout"].ToString();

        UtilityCl2 utility = new UtilityCl2();

        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        MgFeatureService featuerSvc = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);

        MgMap map = GetMapObject(resourceService);

        MgLayerBase q_Layer = UtilityCl2.getLayerByName(map, q_LayerName);
        string resId = q_Layer.GetFeatureSourceId();
        MgResourceIdentifier layerFeatureIdString = new MgResourceIdentifier(resId);

        XmlDocument doc = new XmlDocument();
        MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;
        MgByteReader reader = resSvc.GetResourceContent(layerFeatureIdString);
        string layoutXml = Utility_Paint.GetXmlFromByteReader(reader);

        doc.LoadXml(layoutXml);

        // Delete all 'Extension' from FeatureSource
        while (doc.GetElementsByTagName("Extension").Count > 0)
        {
            XmlNodeList q_toDeleteLsit = doc.GetElementsByTagName("Extension");
            XmlNode root = doc.DocumentElement;
            root.RemoveChild(q_toDeleteLsit.Item(0));
        }

        //TODO because of lack of Time, later make normal Node way
        //change made to Inner join instead of left join for BoundaryChange Map 25-oct-2013
        string q_ProtoForClass1 = "";
        if (MapSettings.CurrentMapName == ConfigurationManager.AppSettings["AutodeskBoundaryChangeMapName"])
        {
            q_ProtoForClass1 = "<RelateProperty><FeatureClassProperty>q_FeatureClassProperty_1wewewe</FeatureClassProperty><AttributeClassProperty>q_AttributeClassProperty_1wewewe</AttributeClassProperty></RelateProperty><AttributeClass>q_AttributeClass_wewewe</AttributeClass><ResourceId>q_ResourceId_wewewe</ResourceId><Name>q_Name_wewewe</Name><AttributeNameDelimiter /><RelateType>Inner</RelateType><ForceOneToOne>true</ForceOneToOne>";
        }
        else
        {
            q_ProtoForClass1 = "<RelateProperty><FeatureClassProperty>q_FeatureClassProperty_1wewewe</FeatureClassProperty><AttributeClassProperty>q_AttributeClassProperty_1wewewe</AttributeClassProperty></RelateProperty><AttributeClass>q_AttributeClass_wewewe</AttributeClass><ResourceId>q_ResourceId_wewewe</ResourceId><Name>q_Name_wewewe</Name><AttributeNameDelimiter /><RelateType>LeftOuter</RelateType><ForceOneToOne>true</ForceOneToOne>";
        }
        q_ProtoForClass1 = q_ProtoForClass1.Replace("q_FeatureClassProperty_1wewewe", q_FeatureClassProperty_1);
        q_ProtoForClass1 = q_ProtoForClass1.Replace("q_AttributeClassProperty_1wewewe", q_AttributeClassProperty_1);
        q_ProtoForClass1 = q_ProtoForClass1.Replace("q_AttributeClass_wewewe", q_AttributeClass);
        q_ProtoForClass1 = q_ProtoForClass1.Replace("q_ResourceId_wewewe", q_ResourceId);
        q_ProtoForClass1 = q_ProtoForClass1.Replace("q_Name_wewewe", q_NameInside);

        XmlNode childElementAttributeRelate = doc.CreateElement("AttributeRelate");
        childElementAttributeRelate.InnerXml = q_ProtoForClass1;

        XmlNode childElementName = doc.CreateElement("Name");
        childElementName.InnerText = q_Name;

        XmlNode childElementFeatureClass = doc.CreateElement("FeatureClass");
        childElementFeatureClass.InnerText = q_FeatureClass;

        XmlNode childElementExtension = doc.CreateElement("Extension");
        childElementExtension.AppendChild(childElementAttributeRelate);
        childElementExtension.AppendChild(childElementName);
        childElementExtension.AppendChild(childElementFeatureClass);

        doc.GetElementsByTagName("FeatureSource")[0].AppendChild(childElementExtension);
        MgByteSource byteSource = Utility_Paint.ByteSourceFromXMLDoc(doc);

        CustomSetResource(resSvc, layerFeatureIdString, byteSource);

        byteSource.Dispose();
        resSvc.Dispose();
    }
    public static string GetViewName(string LayerName, int userid)
    {
        return "vw_" + LayerName + "_" + userid.ToString();
    }
    public static void CreateUpdateView(string LayerName, int userid, string q_strSQL, string q_ViewName)
    {
        if (q_ViewName == "")
        {
            q_ViewName = GetViewName(LayerName, userid);
        }
        StringBuilder q_strSQL1 = new StringBuilder();
        q_strSQL1.Append("if EXISTS (SELECT * FROM dbo.sysobjects where name = '");
        q_strSQL1.Append(q_ViewName);
        q_strSQL1.Append("')begin execute('ALTER View [");
        q_strSQL1.Append(q_ViewName);
        q_strSQL1.Append("] AS ");
        q_strSQL1.Append(q_strSQL);
        q_strSQL1.Append("') end else begin execute('");
        q_strSQL1.Append("CREATE View [");
        q_strSQL1.Append(q_ViewName);
        q_strSQL1.Append("] AS ");
        q_strSQL1.Append(q_strSQL);
        q_strSQL1.Append("') end");
        string q_strSQL2 = q_strSQL1.ToString();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        conn.Open();

        SqlCommand cmd = new SqlCommand(q_strSQL2, conn);
        cmd.CommandTimeout = 60000; // Wait ten minutes before timing out
        string returnvalue = (string)cmd.ExecuteScalar();
        conn.Close();
    }

    public static void RefreshMap(Page page)
    {
        page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "RefreshMap();", true);
    }

    public static void LayerTurnOnOff(string mapName, string layerName, Boolean q_OnOff)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        UtilityCl2 utility = new UtilityCl2();
        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

        MgMap map = GetMapObject(resourceService);

        MgLayerCollection mgLayers = map.GetLayers();

        MgLayerBase q_LayerBase = UtilityCl2.getLayerByName(map, layerName);
        if (q_LayerBase != null)
        {
            if (q_OnOff)
                q_LayerBase.SetVisible(true);
            else
                q_LayerBase.SetVisible(false);
            q_LayerBase.ForceRefresh();
            map.Save(resourceService);
            //test
            map.Save();
        }
    }

    public static string GetAppSettingsForLayers(string MapName, string layerGlobalName)
    {
        string layerName1 = ConfigurationManager.AppSettings[layerGlobalName];
        MgLayerBase q_checkLayer = UtilityCl2.getLayerByName(MapName, layerName1);
        if (q_checkLayer == null) layerName1 = layerName1 + "_filtered";
        return layerName1;
    }

    public static void qF_SetTTforSchoolDivisions(Boolean IsRequiredTableCreate)
    {

        string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
        MapThemeDAL dal = new MapThemeDAL();
        int q_themSetId = MapSettings.MapThemeID;// dal.GetThemeSetIdByUserIDAndThemSetName(MapSettings.MapThemeID.ToString(), q_UserID);
        Boolean isPer = dal.isPercent(q_themSetId);


        string layerName = "SchoolDivisions_Analysis";
        //string mapName = "AnalysisMap";
        string mapName = MapSettings.CurrentMapName;

        //changes made to create for each user	arv-12-jul-2013
        //int userid = 82;
        int userid = Convert.ToInt32(q_UserID);
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        Utility_Paint utility = new Utility_Paint();

        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        string layerDefId = utility.GetLayerDefinitionResourceId(layerName, session, mapName);
        //string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId, qF_SetToolTips(true));
        string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId);//, qF_SetToolTips(true));
        utility.ReplaceWithFilteredLayer(session, sessionLayerId, layerName, mapName);
        string LayerName = layerName + "_filtered";

        string q_TableName = Ut_SQL2TT.GetSchoolTableName(userid);
        if (IsRequiredTableCreate)
        {
            Ut_SQL2TT.CreateSchoolTable(q_TableName, isPer);
        }

        Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
            LayerName,
            mapName,
            "SD_NUM",
            //"MunicipalityID",
            "SchID",
            "dbo:" + q_TableName,
            "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
            "School_Divisions",
            "School_DivisionsExtended",
            "Default:School_Divisions"//from layer def
            );

        Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, Ut_SQL2TT.qF_SetToolTipsSchoolDivision(isPer));

        Dictionary<string, string> q_RawDict = dal.GetRawDictionaryForPainting(q_themSetId);

        string q_Criterion = "School_Divisionsvalue";

        string q_Layer = LayerName;
        string q_mapName = mapName;

        Dictionary<string, string> q_CondColor = Utility_Paint.RawDictionaryToOfficial(q_RawDict, q_Criterion, isPer);

        Utility_Paint.qF_PaintIt(q_mapName, q_Layer, q_CondColor);
        q_CondColor.Clear();
        q_RawDict.Clear();
    }
    public static void qF_SetTTforMunicipality(Boolean IsRequiredTableCreate)
    {

        string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
        MapThemeDAL dal = new MapThemeDAL();
        int q_themSetId = MapSettings.MapThemeID;// dal.GetThemeSetIdByUserIDAndThemSetName(MapSettings.MapThemeID.ToString(), q_UserID);
        Boolean isPer = dal.isPercent(q_themSetId);

        string layerName = "Municipalities_Analysis";
        //string mapName = "AnalysisMap";
        string mapName = MapSettings.CurrentMapName;

        int userid = Convert.ToInt32(q_UserID);
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        Utility_Paint utility = new Utility_Paint();

        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        string layerDefId = utility.GetLayerDefinitionResourceId(layerName, session, mapName);
        string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId);//, qF_SetToolTips(true));

        utility.ReplaceWithFilteredLayer(session, sessionLayerId, layerName, mapName);
        string LayerName = layerName + "_filtered";

        string q_TableName = Ut_SQL2TT.GetMunTableName(userid);
        if (IsRequiredTableCreate)
        {
            Ut_SQL2TT.CreateMunTable(q_TableName, isPer);
        }

        Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
            LayerName,
            mapName,
            "munid",
            "MunicipalityID",
            "dbo:" + q_TableName,
            "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
            "Municipalities4",
            "Municipalities4Extended",
            "Default:Municipalities4"//from layer def
            );

        Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, Ut_SQL2TT.qF_SetToolTipsMun(isPer));
        //Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, "testTip");

        Dictionary<string, string> q_RawDict = dal.GetRawDictionaryForPainting(q_themSetId);

        string q_Criterion = "Municipalities4value";
        string q_Layer = LayerName;
        string q_mapName = mapName;

        Dictionary<string, string> q_CondColor = Utility_Paint.RawDictionaryToOfficial(q_RawDict, q_Criterion, isPer);

        Utility_Paint.qF_PaintIt(q_mapName, q_Layer, q_CondColor);

        q_CondColor.Clear();
        q_RawDict.Clear();

    }

    //original code
    //public static void qF_SetTTforMunicipality_Parcels(Boolean IsRequiredTableCreate)
    //{
    //    string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
    //    MapThemeDAL dal = new MapThemeDAL();
    //    int q_themSetId = MapSettings.MapThemeID;// dal.GetThemeSetIdByUserIDAndThemSetName(MapSettings.MapThemeID.ToString(), q_UserID);
    //    Boolean isPer = dal.isPercent(q_themSetId);
    //    string layerName = "Assessment_Parcels_Analysis";
    //    //string mapName = "AnalysisMap";
    //    string mapName = MapSettings.CurrentMapName;

    //    int userid = Convert.ToInt32(q_UserID);
    //    string session = System.Web.HttpContext.Current.Session["Session"].ToString();
    //    Utility_Paint utility = new Utility_Paint();

    //    utility.ConnectToServer(session);
    //    MgSiteConnection siteConnection = utility.GetSiteConnection();
    //    string layerDefId = utility.GetLayerDefinitionResourceId(layerName, session, mapName);
    //    string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId);//, qF_SetToolTips(true));
    //    utility.ReplaceWithFilteredLayer(session, sessionLayerId, layerName, mapName);
    //    string LayerName = layerName + "_filtered";
    //    string q_TableName = Ut_SQL2TT.GetParcelTableName(userid);
    //    if (IsRequiredTableCreate)
    //    {
    //        Ut_SQL2TT.CreateParselTable(q_TableName, isPer);
    //    }

    //    Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
    //        LayerName,
    //        mapName,
    //        "P_ID",
    //        "MapParcelID",
    //        "dbo:" + q_TableName,
    //        "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
    //        //"Library://PATMAP/Data/PATMAP_SQL_Mun.FeatureSource",
    //        q_TableName,
    //        "APA",
    //        "Default:BaseLayer"
    //        );

    //    Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, (q_TableName + "mouseover").ToString());
    //    Dictionary<string, string> q_RawDict = dal.GetRawDictionaryForPainting(q_themSetId);
    //    string q_Criterion = q_TableName + "value";
    //    string q_Layer = LayerName;
    //    string q_mapName = mapName;

    //    Dictionary<string, string> q_CondColor = Utility_Paint.RawDictionaryToOfficial(q_RawDict, q_Criterion, isPer);

    //    Utility_Paint.qF_PaintIt(q_mapName, q_Layer, q_CondColor);
    //    q_CondColor.Clear();
    //    q_RawDict.Clear();
    //}


    //OG_1
    //public static void qF_SetTTforMunicipality_Parcels(Boolean IsRequiredTableCreate)
    //{
    //    string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
    //    MapThemeDAL dal = new MapThemeDAL();
    //    int q_themSetId = MapSettings.MapThemeID;// dal.GetThemeSetIdByUserIDAndThemSetName(MapSettings.MapThemeID.ToString(), q_UserID);
    //    Boolean isPer = dal.isPercent(q_themSetId);
    //    string layerName = "Assessment_Parcels_Analysis";

    //    //string LayerName = layerName;

    //    //string mapName = "AnalysisMap";
    //    string mapName = MapSettings.CurrentMapName;

    //    int userid = Convert.ToInt32(q_UserID);
    //    string session = System.Web.HttpContext.Current.Session["Session"].ToString();
    //    Utility_Paint utility = new Utility_Paint();

    //    utility.ConnectToServer(session);
    //    MgSiteConnection siteConnection = utility.GetSiteConnection();
    //    string layerDefId = utility.GetLayerDefinitionResourceId(layerName, session, mapName);
    //    string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId);//, qF_SetToolTips(true));
    //    utility.ReplaceWithFilteredLayer(session, sessionLayerId, layerName, mapName);
    //    string LayerName = layerName + "_filtered";

    //    string q_TableName = Ut_SQL2TT.GetParcelTableName(userid);
    //    if (IsRequiredTableCreate)
    //    {
    //        Ut_SQL2TT.CreateParselTable(q_TableName, isPer);
    //    }

    //    Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
    //        LayerName,
    //        mapName,
    //        "P_ID",
    //        "MapParcelID",
    //        "dbo:" + q_TableName,
    //        "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
    //        //"Library://PATMAP/Data/PATMAP_SQL_Mun.FeatureSource",
    //        q_TableName,
    //        "APA",
    //        "Default:BaseLayer"
    //     );

    //    Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, (q_TableName + "mouseover").ToString());

    //    ////TEST
    //    //Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, "'Testing Tool Tip'");

    //    Dictionary<string, string> q_RawDict = dal.GetRawDictionaryForPainting(q_themSetId);

    //    string q_Criterion = q_TableName + "value";
    //    string q_Layer = LayerName;
    //    string q_mapName = mapName;

    //    Dictionary<string, string> q_CondColor = Utility_Paint.RawDictionaryToOfficial(q_RawDict, q_Criterion, isPer);

    //    //string tooltip = (q_TableName + "mouseover").ToString();
    //    //Ut_SQL2TT.ChangeColorsTooltips(q_mapName, q_Layer, tooltip, q_CondColor);

    //    Utility_Paint.qF_PaintIt(q_mapName, q_Layer, q_CondColor);

    //    utility.SaveMap(session, q_mapName);

    //    q_CondColor.Clear();
    //    q_RawDict.Clear();
    //}


    //test
    public static void qF_SetTTforMunicipality_Parcels(Boolean IsRequiredTableCreate)
    {
        string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
        MapThemeDAL dal = new MapThemeDAL();
        int q_themSetId = MapSettings.MapThemeID;// dal.GetThemeSetIdByUserIDAndThemSetName(MapSettings.MapThemeID.ToString(), q_UserID);
        Boolean isPer = dal.isPercent(q_themSetId);
        string layerName = "Assessment_Parcels_Analysis";

        //string LayerName = layerName;
        
        //string mapName = "AnalysisMap";
        string mapName = MapSettings.CurrentMapName;

        int userid = Convert.ToInt32(q_UserID);
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        Utility_Paint utility = new Utility_Paint();

        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        string layerDefId = utility.GetLayerDefinitionResourceId(layerName, session, mapName);
        string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId);//, qF_SetToolTips(true));
        utility.ReplaceWithFilteredLayer(session, sessionLayerId, layerName, mapName);
        string LayerName = layerName + "_filtered";

        string q_TableName = Ut_SQL2TT.GetParcelTableName(userid);
        if (IsRequiredTableCreate)
        {
            Ut_SQL2TT.CreateParselTable(q_TableName, isPer);
        }

        //Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSourceParcel(

        Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
            LayerName,
            mapName,
            "P_ID",
            "MapParcelID",
            "dbo:" + q_TableName,
            "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
            "BaseLayer",
            "APA",
            "Default:BaseLayer"
         );

        Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, ("BaseLayer" + "mouseover").ToString());

        ////TEST
        //Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, "'Testing Tool Tip'");

        Dictionary<string, string> q_RawDict = dal.GetRawDictionaryForPainting(q_themSetId);

        string q_Criterion = "BaseLayervalue";
        string q_Layer = LayerName;
        string q_mapName = mapName;

        Dictionary<string, string> q_CondColor = Utility_Paint.RawDictionaryToOfficial(q_RawDict, q_Criterion, isPer);

        //string tooltip = (q_TableName + "mouseover").ToString();
        //Ut_SQL2TT.ChangeColorsTooltips(q_mapName, q_Layer, tooltip, q_CondColor);

        Utility_Paint.qF_PaintIt(q_mapName, q_Layer, q_CondColor);

        utility.SaveMap(session, q_mapName);

        q_CondColor.Clear();
        q_RawDict.Clear();
    }



    public static string qF_SetToolTipsMun(Boolean isPer)
    {
        string tooltips = "";
        if (isPer)
        {
            tooltips = "concat( Municipalities4Name , concat('\\nShift : ', concat(  NullValue(ToString( Municipalities4value  ),' NO DATA')    , ('%'))))";
        }
        else
        {
            tooltips = "Concat (  Municipalities4Name ,   '\\n'  , 'Shift: $', NullValue(ToString( Municipalities4value  ),' NO DATA')) ";
            //tooltips = "Municipalities4Name";
        }
        return tooltips;
    }

    public static string qF_SetToolTipsSchoolDivision(Boolean isPer)
    {
        string tooltips = "";
        if (isPer)
        {
            tooltips = @"concat(  School_DivisionsName  , concat('\nShift : ', concat(  NullValue(ToString(  School_Divisionsvalue   ),' NO DATA')    , ('%'))))";
        }
        else
        {
            tooltips = @"Concat (   School_DivisionsName  ,   '\n'  , 'Shift: $', NullValue(ToString(  School_Divisionsvalue   ),' NO DATA'))";
        }
        return tooltips;
    }

    //this function is to be replaced by qF_ChangeTooltips modified to avoid duplicates in the process which creates filtered layer again
    //required to test after implementing	19-dec-2013
    public static void TESTqF_ChangeTooltips(string mapName, string layerName, string tooltip)
    {
        //Utility_Paint.qF_SetLayerFeatureInXML(mapName, layerName, "ToolTip", tooltip);
        MgSiteConnection siteConnection;

        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        MgUserInformation userInfo = new MgUserInformation(session);
        siteConnection = new MgSiteConnection();
        siteConnection.Open(userInfo);

        MgResourceService resourceService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);

        MgMap map = GetMapObject(resourceService);

        MgLayerCollection layers = map.GetLayers();
        MgLayer layer = (MgLayer)UtilityCl2.getLayerByName(mapName, layerName);

        MgResourceIdentifier layerDefResId = layer.GetLayerDefinition();
        MgByteReader byteReader = resourceService.GetResourceContent(layerDefResId);

        //MgResourceIdentifier q_LayerDef = layer.GetLayerDefinition();
        XmlDocument doc = new XmlDocument();
        String xmlLayerDef = byteReader.ToString();
        doc.LoadXml(xmlLayerDef);

        doc.GetElementsByTagName("ToolTip")[0].InnerText = tooltip;
        MgByteSource byteSource = Utility_Paint.ByteSourceFromXMLDoc(doc);

        CustomSetResource(resourceService, layerDefResId, byteSource);
    }

    //concat(  School_DivisionsName  , concat('\nShift : ', concat(  NullValue(ToString(  School_Divisionsvalue   ),' NO DATA')    , ('%'))))
    //Concat (   School_DivisionsName  ,   '\n'  , 'Shift: $', NullValue(ToString(  School_Divisionsvalue   ),' NO DATA'))
    /// <summary>
    /// </summary>
    /// <param name="mapName"></param>
    /// <param name="layerName"></param>
    /// <param name="tooltip"></param>
    public static void qF_ChangeTooltips(string mapName, string layerName, string tooltip)
    {
        Utility_Paint.qF_SetLayerFeatureInXML(mapName, layerName, "ToolTip", tooltip);
        
        MgSiteConnection siteConnection;

        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        MgUserInformation userInfo = new MgUserInformation(session);
        siteConnection = new MgSiteConnection();
        siteConnection.Open(userInfo);

        MgResourceService resourceService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
        MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);

        MgMap map = GetMapObject(resourceService);

        MgLayerCollection layers = map.GetLayers();
        MgLayer layer = (MgLayer)UtilityCl2.getLayerByName(mapName, layerName);

        MgResourceIdentifier resId = new MgResourceIdentifier(layer.GetFeatureSourceId());
        MgResourceIdentifier layerDefResId = layer.GetLayerDefinition();
        MgByteReader byteReader = resourceService.GetResourceContent(layerDefResId);

        // Load the Layer Definition and Navigate to the specified <VectorScaleRange>
        MgResourceIdentifier q_LayerDef = layer.GetLayerDefinition();
        XmlDocument doc = new XmlDocument();
        String xmlLayerDef = byteReader.ToString();
        doc.LoadXml(xmlLayerDef);

        doc.GetElementsByTagName("ToolTip")[0].InnerText = tooltip;
        MgByteSource byteSource = Utility_Paint.ByteSourceFromXMLDoc(doc);

        //resourceService.SetResource(q_LayerDef, byteSource.GetReader(), null);
        CustomSetResource(resourceService, q_LayerDef, byteSource);

        //string q_LayerName = layerName;

        //MgMap map = GetMapObject(resourceService);

        //MgLayerBase q_Layer = UtilityCl2.getLayerByName(map, q_LayerName);
        //string resId = q_Layer.GetFeatureSourceId();
        //MgResourceIdentifier layerFeatureIdString = new MgResourceIdentifier(resId);

        //XmlDocument doc = new XmlDocument();
        //MgResourceService resSvc = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;
        //MgByteReader reader = resSvc.GetResourceContent(layerFeatureIdString);
        //string layoutXml = Utility_Paint.GetXmlFromByteReader(reader);

        //doc.LoadXml(layoutXml);

        //doc.GetElementsByTagName("ToolTip")[0].InnerText = tooltip;

        //MgByteSource byteSource = Utility_Paint.ByteSourceFromXMLDoc(doc);

        //CustomSetResource(resSvc, layerFeatureIdString, byteSource);

        //map.Save(resourceService);

        //byteSource.Dispose();
        //resSvc.Dispose();
    }
    //concat(  School_DivisionsName  , concat('\nShift : ', concat(  NullValue(ToString(  School_Divisionsvalue   ),' NO DATA')    , ('%'))))
    //Concat (   School_DivisionsName  ,   '\n'  , 'Shift: $', NullValue(ToString(  School_Divisionsvalue   ),' NO DATA'))


    //drop mouse over tables if exists 
    public static void DropMouseOverTable(string tableName)
    {
        try
        {
            StringBuilder q_NewTableQuery = new StringBuilder();
            q_NewTableQuery.Append("if EXISTS (SELECT * FROM dbo.sysobjects where name = '");
            q_NewTableQuery.Append(tableName);
            q_NewTableQuery.Append("') begin drop table ");
            q_NewTableQuery.Append(tableName);
            q_NewTableQuery.Append(" end ");

            string q_strSQL2 = q_NewTableQuery.ToString();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (SqlCommand cmd = new SqlCommand(q_strSQL2, conn))
                {
                    cmd.CommandTimeout = 60000; // Wait ten minutes before timing out
                    string returnvalue = (string)cmd.ExecuteScalar();
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    public static void CreateParselTable(string tableName, Boolean isPer)
    {


        StringBuilder q_NewTableQuery = new StringBuilder();
        string q_strSQL1 = MapManagerNew.filterParcelLayer(isPer);
        //q_NewTableQuery.Append("if EXISTS (SELECT * FROM dbo.sysobjects where name = '");
        //q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append("') begin drop table ");
        //q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append(" end ");

        DropMouseOverTable(tableName);

        q_NewTableQuery.Append("CREATE TABLE ");
        q_NewTableQuery.Append(tableName);
        //changes back to integer 27-Mar-2017
        //q_NewTableQuery.Append(" (MapParcelID nvarchar(50),taxClassID nchar(10),mouseover nvarchar(2500),value float)");
        q_NewTableQuery.Append(" (MapParcelID int,taxClassID nchar(10),mouseover varchar(2500),value float)");
        
        q_NewTableQuery.Append(" INSERT INTO ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ");
        q_NewTableQuery.Append(q_strSQL1);

        //create index for the table	18-nov-2013
        q_NewTableQuery.Append(Environment.NewLine);
        q_NewTableQuery.Append(" CREATE NONCLUSTERED INDEX [IX_MapParcelID] ON ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ( [MapParcelID] ASC )");
        q_NewTableQuery.Append(" WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");

        string q_strSQL2 = q_NewTableQuery.ToString();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand(q_strSQL2, conn);
        cmd.CommandTimeout = 60000; // Wait ten minutes before timing out
        string returnvalue = (string)cmd.ExecuteScalar();
        conn.Close();
    }

    public static void CreateMunTable(string tableName, Boolean isPer)
    {
        StringBuilder q_NewTableQuery = new StringBuilder();
        string q_strSQL1 = MapManagerNew.addMunicipalityThemeDatasourceNew(isPer);
        //q_NewTableQuery.Append("if EXISTS (SELECT * FROM dbo.sysobjects where name = '");
        //q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append("') begin drop table ");
        //q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append(" end ");

        DropMouseOverTable(tableName);

        q_NewTableQuery.Append("CREATE TABLE ");
        q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append(" (MunicipalityID int,Name nchar(150),value float)");
        q_NewTableQuery.Append(" (MunicipalityID nvarchar(50),Name nchar(150),value float)");
        q_NewTableQuery.Append(" INSERT INTO ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ");
        q_NewTableQuery.Append(q_strSQL1);

        //create index for the table	18-nov-2013
        q_NewTableQuery.Append(Environment.NewLine);
        q_NewTableQuery.Append(" CREATE NONCLUSTERED INDEX [IX_MunicipalityID] ON ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ( [MunicipalityID] ASC )");
        q_NewTableQuery.Append(" WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");

        string q_strSQL2 = q_NewTableQuery.ToString();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand(q_strSQL2, conn);
        cmd.CommandTimeout = 60000; // Wait ten minutes before timing out
        string returnvalue = (string)cmd.ExecuteScalar();
        conn.Close();
    }


    public static void CreateSchoolTable(string tableName, Boolean isPer)
    {
        StringBuilder q_NewTableQuery = new StringBuilder();
        string q_strSQL1 = MapManagerNew.addSchoolThemeDatasourceNew(isPer);

        //q_NewTableQuery.Append("if EXISTS (SELECT * FROM dbo.sysobjects where name = '");
        //q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append("') begin drop table ");
        //q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append(" end ");

        DropMouseOverTable(tableName);

        q_NewTableQuery.Append("CREATE TABLE ");
        q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append(" (SchID int,Name nchar(250),value float)");
        q_NewTableQuery.Append(" (SchID nvarchar(3),Name nchar(250),value float)");
        q_NewTableQuery.Append(" INSERT INTO ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ");
        q_NewTableQuery.Append(q_strSQL1);

        //create index for the table	18-nov-2013
        q_NewTableQuery.Append(Environment.NewLine);
        q_NewTableQuery.Append(" CREATE NONCLUSTERED INDEX [IX_SchID] ON ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ( [SchID] ASC )");
        q_NewTableQuery.Append(" WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");

        string q_strSQL2 = q_NewTableQuery.ToString();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand(q_strSQL2, conn);
        cmd.CommandTimeout = 60000; // Wait ten minutes before timing out
        string returnvalue = (string)cmd.ExecuteScalar();
        conn.Close();
    }

    public static void CreateBAMunTable(string tableName, Boolean isPer)
    {
        StringBuilder q_NewTableQuery = new StringBuilder();
        string q_strSQL1 = MapManagerNew.BA_addMunicipalityThemeDatasourceNew(isPer);
        q_NewTableQuery.Append("if EXISTS (SELECT * FROM dbo.sysobjects where name = '");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append("') begin drop table ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" end ");
        q_NewTableQuery.Append("CREATE TABLE ");
        q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append(" (MunicipalityID int,Name nchar(150),value float)");
        q_NewTableQuery.Append(" (MunicipalityID nvarchar(50),Name nchar(150),value float)");
        q_NewTableQuery.Append(" INSERT INTO ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ");
        q_NewTableQuery.Append(q_strSQL1);

        //create index for the table	18-nov-2013
        q_NewTableQuery.Append(Environment.NewLine);
        q_NewTableQuery.Append(" CREATE NONCLUSTERED INDEX [IX_MunicipalityID] ON ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ( [MunicipalityID] ASC )");
        q_NewTableQuery.Append(" WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");

        string q_strSQL2 = q_NewTableQuery.ToString();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand(q_strSQL2, conn);
        cmd.CommandTimeout = 60000; // Wait ten minutes before timing out
        string returnvalue = (string)cmd.ExecuteScalar();
        conn.Close();
    }

    public static void CreateBASourceParselTable(string tableName, Boolean isPer)
    {
        StringBuilder q_NewTableQuery = new StringBuilder();
        string q_strSQL1 = MapManagerNew.filterBASourceParcelLayer(isPer);
        //q_NewTableQuery.Append("if EXISTS (SELECT * FROM dbo.sysobjects where name = '");
        //q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append("') begin drop table ");
        //q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append(" end ");

        DropMouseOverTable(tableName);

        q_NewTableQuery.Append("CREATE TABLE ");
        q_NewTableQuery.Append(tableName);
        //	q_NewTableQuery.Append(" (MapParcelID int,taxClassID nchar(10),mouseover varchar(2500),value float)");
        //q_NewTableQuery.Append(" (MapParcelID nvarchar(50),AssessmentNumber nvarchar(50),MunID nvarchar(5))");
        q_NewTableQuery.Append(" (MapParcelID int,AssessmentNumber nvarchar(50),MunID nvarchar(5))");
        q_NewTableQuery.Append(" INSERT INTO ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ");
        q_NewTableQuery.Append(q_strSQL1);

        //create index for the table	18-nov-2013
        q_NewTableQuery.Append(Environment.NewLine);
        q_NewTableQuery.Append(" CREATE NONCLUSTERED INDEX [IX_MapParcelID] ON ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ( [MapParcelID] ASC )");
        q_NewTableQuery.Append(" WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");

        string q_strSQL2 = q_NewTableQuery.ToString();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand(q_strSQL2, conn);
        cmd.CommandTimeout = 60000; // Wait ten minutes before timing out
        string returnvalue = (string)cmd.ExecuteScalar();
        conn.Close();
    }

    public static void CreateBADestinationParselTable(string tableName, Boolean isPer)
    {
        StringBuilder q_NewTableQuery = new StringBuilder();
        string q_strSQL1 = MapManagerNew.filterBADestinationParcelLayer(isPer);
        //q_NewTableQuery.Append("if EXISTS (SELECT * FROM dbo.sysobjects where name = '");
        //q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append("') begin drop table ");
        //q_NewTableQuery.Append(tableName);
        //q_NewTableQuery.Append(" end ");

        DropMouseOverTable(tableName);

        q_NewTableQuery.Append("CREATE TABLE ");
        q_NewTableQuery.Append(tableName);
        //	q_NewTableQuery.Append(" (MapParcelID int,taxClassID nchar(10),mouseover varchar(2500),value float)");
        //q_NewTableQuery.Append(" (MapParcelID nvarchar(50),AssessmentNumber nvarchar(50),MunID nvarchar(5))");
        q_NewTableQuery.Append(" (MapParcelID int,AssessmentNumber nvarchar(50),MunID nvarchar(5))");

        q_NewTableQuery.Append(" INSERT INTO ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ");
        q_NewTableQuery.Append(q_strSQL1);

        //create index for the table	18-nov-2013
        q_NewTableQuery.Append(Environment.NewLine);
        q_NewTableQuery.Append(" CREATE NONCLUSTERED INDEX [IX_MapParcelID] ON ");
        q_NewTableQuery.Append(tableName);
        q_NewTableQuery.Append(" ( [MapParcelID] ASC )");
        q_NewTableQuery.Append(" WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");

        string q_strSQL2 = q_NewTableQuery.ToString();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand(q_strSQL2, conn);
        cmd.CommandTimeout = 60000; // Wait ten minutes before timing out
        string returnvalue = (string)cmd.ExecuteScalar();
        conn.Close();
    }

    public static void qF_BA_SetTTforMunicipality(Boolean IsRequiredTableCreate)
    {

        string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
        MapThemeDAL dal = new MapThemeDAL();
        int q_themSetId = MapSettings.MapThemeID;// dal.GetThemeSetIdByUserIDAndThemSetName(MapSettings.MapThemeID.ToString(), q_UserID);
        Boolean isPer = dal.isPercent(q_themSetId);

        string layerName = "Municipalities_Analysis";

        //string mapName = "AnalysisMap";
        //string mapName = "BoundaryChange";
        string mapName = MapSettings.CurrentMapName;

        int userid = Convert.ToInt32(q_UserID);
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        Utility_Paint utility = new Utility_Paint();

        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        string layerDefId = utility.GetLayerDefinitionResourceId(layerName, session, mapName);
        //string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId, qF_SetToolTips(true));
        string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId);//, qF_SetToolTips(true));
        utility.ReplaceWithFilteredLayer(session, sessionLayerId, layerName, mapName);
        string LayerName = layerName + "_filtered";

        string q_TableName = Ut_SQL2TT.GetMunTableName(userid);
        if (IsRequiredTableCreate)
        {
            Ut_SQL2TT.CreateBAMunTable(q_TableName, isPer);
        }

        Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
            LayerName,
            mapName,
            "munid",
            "MunicipalityID",
            "dbo:" + q_TableName,
            "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
            "Municipalities4",
            "Municipalities4Extended",
            "Default:Municipalities4"//from layer def
            );

        Ut_SQL2TT.qF_ChangeTooltips(mapName, "Municipalities_Analysis_filtered", Ut_SQL2TT.qF_SetToolTipsMun(isPer));

        Dictionary<string, string> q_RawDict = dal.GetRawDictionaryForPainting(q_themSetId);

        string q_Criterion = "Municipalities4value";
        string q_Layer = "Municipalities_Analysis_filtered";
        string q_mapName = mapName;

        Dictionary<string, string> q_CondColor = Utility_Paint.RawDictionaryToOfficial(q_RawDict, q_Criterion, isPer);

        Utility_Paint.qF_PaintIt(q_mapName, q_Layer, q_CondColor);
        q_CondColor.Clear();
        q_RawDict.Clear();

    }

    public static void qF_BASource_SetTTforMunicipality_Parcels(Boolean IsRequiredTableCreate)
    {
        string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
        MapThemeDAL dal = new MapThemeDAL();
        int q_themSetId = MapSettings.MapThemeID;// dal.GetThemeSetIdByUserIDAndThemSetName(MapSettings.MapThemeID.ToString(), q_UserID);
        Boolean isPer = dal.isPercent(q_themSetId);
        //	string layerName = "Assessment_Parcels_Analysis";
        string layerName = "Source";

        //string mapName = "AnalysisMap";
        //string mapName = "BoundaryChange";
        string mapName = MapSettings.CurrentMapName;

        int userid = Convert.ToInt32(q_UserID);
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        Utility_Paint utility = new Utility_Paint();

        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        string layerDefId = utility.GetLayerDefinitionResourceId(layerName, session, mapName);
        string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId);//, qF_SetToolTips(true));

        utility.ReplaceWithFilteredLayer(session, sessionLayerId, layerName, mapName);
        string LayerName = layerName + "_filtered";

        string q_TableName = Ut_SQL2TT.GetBASourceParcelTableName(userid);
        if (IsRequiredTableCreate)
        {
            Ut_SQL2TT.CreateBASourceParselTable(q_TableName, isPer);
        }


        //31-jul-2019
        //Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
        //    LayerName,
        //    mapName,
        //    "P_ID",
        //    "MapParcelID",
        //    "dbo:" + q_TableName,
        //    "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
        //    q_TableName,
        //    "BASource",
        //    "Default:BaseLayer"
        //    );

        Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
            LayerName,
            mapName,
            "P_ID",
            "MapParcelID",
            "dbo:" + q_TableName,
            "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
            "BaseLayer",
            "BASource",
            "Default:BaseLayer"
         );

        //Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, (q_TableName + "mouseover").ToString());
        //31-jul-2019
        //Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, (q_TableName + "MapParcelID").ToString());
        Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, ("BaseLayer" + "MapParcelID").ToString());

        Dictionary<string, string> q_RawDict = dal.GetRawDictionaryForPainting(q_themSetId);
        //31-jul-2019
        //string q_Criterion = q_TableName + "value";
        string q_Criterion = "BaseLayervalue";

        string q_Layer = LayerName;
        string q_mapName = mapName;

        //Dictionary<string, string> q_CondColor = Utility_Paint.RawDictionaryToOfficial(q_RawDict, q_Criterion, isPer);
        Dictionary<string, string> q_CondColor = Utility_Paint.BASource_RawDictionaryToOfficial(q_RawDict, q_Criterion, isPer);

        Utility_Paint.qF_PaintIt(q_mapName, q_Layer, q_CondColor);
        //added 31-jul-2019
        utility.SaveMap(session, q_mapName);

        q_CondColor.Clear();
        q_RawDict.Clear();

    }

    public static void qF_BADestination_SetTTforMunicipality_Parcels(Boolean IsRequiredTableCreate)
    {
        string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
        MapThemeDAL dal = new MapThemeDAL();
        int q_themSetId = MapSettings.MapThemeID;// dal.GetThemeSetIdByUserIDAndThemSetName(MapSettings.MapThemeID.ToString(), q_UserID);
        Boolean isPer = dal.isPercent(q_themSetId);
        //string layerName = "Assessment_Parcels_Analysis";
        string layerName = "Destination";

        //string mapName = "AnalysisMap";
        //string mapName = "BoundaryChange";
        string mapName = MapSettings.CurrentMapName;

        int userid = Convert.ToInt32(q_UserID);
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        Utility_Paint utility = new Utility_Paint();

        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        string layerDefId = utility.GetLayerDefinitionResourceId(layerName, session, mapName);
        string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId);//, qF_SetToolTips(true));

        utility.ReplaceWithFilteredLayer(session, sessionLayerId, layerName, mapName);
        string LayerName = layerName + "_filtered";

        string q_TableName = Ut_SQL2TT.GetBADestinationParcelTableName(userid);
        if (IsRequiredTableCreate)
        {
            Ut_SQL2TT.CreateBADestinationParselTable(q_TableName, isPer);
        }

        //31-jul-2019
        //Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
        //    LayerName,
        //    mapName,
        //    "P_ID",
        //    "MapParcelID",
        //    "dbo:" + q_TableName,
        //    "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
        //    q_TableName,
        //    "BADest",
        //    "Default:BaseLayer"
        //    );

        Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
            LayerName,
            mapName,
            "P_ID",
            "MapParcelID",
            "dbo:" + q_TableName,
            "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
            "BaseLayer",
            "BADest",
            "Default:BaseLayer"
         );



        //Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, (q_TableName + "mouseover").ToString());
        //31-jul-2019
        //Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, (q_TableName + "MapParcelID").ToString());
        Ut_SQL2TT.qF_ChangeTooltips(mapName, LayerName, ("BaseLayer" + "MapParcelID").ToString());

        Dictionary<string, string> q_RawDict = dal.GetRawDictionaryForPainting(q_themSetId);
        //31-jul-2019
        //string q_Criterion = q_TableName + "value";
        string q_Criterion = "BaseLayervalue";
        string q_Layer = LayerName;
        string q_mapName = mapName;

        //Dictionary<string, string> q_CondColor = Utility_Paint.RawDictionaryToOfficial(q_RawDict, q_Criterion, isPer);
        Dictionary<string, string> q_CondColor = Utility_Paint.BADestination_RawDictionaryToOfficial(q_RawDict, q_Criterion, isPer);

        Utility_Paint.qF_PaintIt(q_mapName, q_Layer, q_CondColor);

        //added 31-jul-2019
        utility.SaveMap(session, q_mapName);

        q_CondColor.Clear();
        q_RawDict.Clear();
    }

    public static string GetParcelTableName(int UserId)
    {
        return "LiveParcelMouseOver_" + UserId.ToString();
    }
    public static string GetMunTableName(int UserId)
    {
        return "LiveMunMouseOver_" + UserId.ToString();
    }
    public static string GetSchoolTableName(int UserId)
    {
        return "LiveSchoolMouseOver_" + UserId.ToString();
    }

    public static string GetBASourceParcelTableName(int UserId)
    {
        return "LiveBASourceParcelMouseOver_" + UserId.ToString();
    }

    public static string GetBADestinationParcelTableName(int UserId)
    {
        return "LiveBADestinationParcelMouseOver_" + UserId.ToString();
    }

    public static void turnOnSchoolDistricts()
    {
        MapSettings.MapAnalysisLayer = "SchoolDivisions";
        Ut_SQL2TT.LayerTurnOnOff(MapSettings.CurrentMapName, Ut_SQL2TT.GetAppSettingsForLayers(MapSettings.CurrentMapName, "SchoolDivisionsLayerName"), true);
        Ut_SQL2TT.LayerTurnOnOff(MapSettings.CurrentMapName, Ut_SQL2TT.GetAppSettingsForLayers(MapSettings.CurrentMapName, "SchoolDivisions_Analysis_filtered"), true);
        Ut_SQL2TT.LayerTurnOnOff(MapSettings.CurrentMapName, Ut_SQL2TT.GetAppSettingsForLayers(MapSettings.CurrentMapName, "MunicipalitiesLayerName"), false);
        Ut_SQL2TT.LayerTurnOnOff(MapSettings.CurrentMapName, Ut_SQL2TT.GetAppSettingsForLayers(MapSettings.CurrentMapName, "Municipalities_Analysis_filtered"), false);
    }

    public static void turnOnMunicipalities()
    {
        MapSettings.MapAnalysisLayer = "Municipalities";

        Ut_SQL2TT.LayerTurnOnOff(MapSettings.CurrentMapName, Ut_SQL2TT.GetAppSettingsForLayers(MapSettings.CurrentMapName, "MunicipalitiesLayerName"), true);
        Ut_SQL2TT.LayerTurnOnOff(MapSettings.CurrentMapName, Ut_SQL2TT.GetAppSettingsForLayers(MapSettings.CurrentMapName, "Municipalities_Analysis_filtered"), true);
        Ut_SQL2TT.LayerTurnOnOff(MapSettings.CurrentMapName, Ut_SQL2TT.GetAppSettingsForLayers(MapSettings.CurrentMapName, "SchoolDivisionsLayerName"), false);
        Ut_SQL2TT.LayerTurnOnOff(MapSettings.CurrentMapName, Ut_SQL2TT.GetAppSettingsForLayers(MapSettings.CurrentMapName, "SchoolDivisions_Analysis_filtered"), false);
    }

    public static MgMap GetSessionMap()
    {
        try
        {
            //org 
            //string session = System.Web.HttpContext.Current.Session["Session"].ToString();
            //UtilityClass utility = new UtilityClass();
            //utility.InitializeWebTier(HttpContext.Current.Request);
            //utility.ConnectToServer(session);
            //MgSiteConnection siteConnection = utility.GetSiteConnection();
            //MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;
            //MgMap analysisMap = Ut_SQL2TT.GetMapObject(resourceService);

            string session = System.Web.HttpContext.Current.Session["Session"].ToString();
            UtilityCl2 utility = new UtilityCl2();
            utility.ConnectToServer(session);
            MgSiteConnection siteConnection = utility.GetSiteConnection();
            MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

            MgMap map = GetMapObject(resourceService);

            return map;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void CustomSetResource(MgResourceService resSvc, MgResourceIdentifier resourceID, MgByteSource byteSource)
    {
        //added to wait until resource is available		arv-02-aug-2013
        Boolean IsResvError = false;
        do
        {
            try
            {
                IsResvError = false;
                //resSvc.SetResource(layerFeatureIdString, byteSource.GetReader(), null);
                resSvc.SetResource(resourceID, byteSource.GetReader(), null);
            }
            catch (MgResourceBusyException rex)
            {
                IsResvError = true;
            }
            catch (Exception rex)
            {
                //System.Diagnostics.Debug.Print(rex.Message);
                throw new Exception(rex.Message);
            }
        } while (IsResvError);

    }

    public static MgMap OpenNewMap(MgResourceService resourceService, string MapName)
    {
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        UtilityCl2 utility = new UtilityCl2();
        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();

        MgMap map = new MgMap(siteConnection);

        //added to wait until resource is available		arv-19-jul-2013
        int tmp = 0;
        Boolean IsResvError = false;
        do
        {
            try
            {
                IsResvError = false;
                //map.Open(resourceService, MapSettings.CurrentMapName);
                map.Open(resourceService, MapName);
            }
            catch (MgResourceNotFoundException rex)
            {
                IsResvError = true;
            }
            catch (MgResourceDataNotFoundException rex)
            {
                IsResvError = true;
            }
            catch (Exception rex)
            {
                //System.Diagnostics.Debug.Print(rex.Message);
                throw new Exception(rex.Message);
            }
        } while (IsResvError);

        return map;
    }

    public static MgMap GetMapObject(MgResourceService resourceService)
    {
        try
        {
            string MapName = MapSettings.CurrentMapName;
            MgMap map = OpenNewMap(resourceService, MapName);
            return map;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static MgMap GetMapObject(MgResourceService resourceService, string MapName)
    {
        try
        {
            MgMap map = OpenNewMap(resourceService, MapName);
            return map;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static string Create_Get_Session()
    {
        string sessionId = null;

        try
        {
            //MgLocalizer.SetLocalizedFilesPath(Request.ServerVariables["APPL_PHYSICAL_PATH"] + "..\\localized\\");
            //MgLocalizer.SetLocalizedFilesPath(@"C:\Program Files\Autodesk\Autodesk Infrastructure Web Server Extension 2012\www\localized\");
            MgLocalizer.SetLocalizedFilesPath(ConfigurationManager.AppSettings["AutodeskWebServerPath"] + "localized\\");

            //MapGuideApi.MgInitializeWebTier(Request.ServerVariables["APPL_PHYSICAL_PATH"] + "../webconfig.ini");
            //MapGuideApi.MgInitializeWebTier(@"C:\Program Files\Autodesk\Autodesk Infrastructure Web Server Extension 2012\www\webconfig.ini");
            MapGuideApi.MgInitializeWebTier(ConfigurationManager.AppSettings["AutodeskWebServerPath"] + "webconfig.ini");

            bool createSession = true;
            if (System.Web.HttpContext.Current.Session["Session"] != null)
            {
                sessionId = (string)System.Web.HttpContext.Current.Session["Session"];
            }
            MgUserInformation cred = new MgUserInformation();
            cred.SetMgUsernamePassword("Administrator", "admin");

            //else if (null != username)
            //{
            //    cred.SetMgUsernamePassword(username, password);
            //}
            //else
            //{
            //    RequestAuthentication();
            //    return;
            //}

            MgSiteConnection site = new MgSiteConnection();
            cred.SetLocale("en");
            //cred.SetClientIp(GetClientIp(Request));
            cred.SetClientAgent("Ajax Viewer");
            if (Ut_SQL2TT.IsSessionExists(sessionId))
            {
                cred.SetMgSessionId(sessionId);
                createSession = false;
            }
            site.Open(cred);
            if (createSession)
            {
                MgSite site1 = site.GetSite();
                sessionId = site1.CreateSession();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
            //Response.Write(ex.Message);
        }
        return sessionId;
    }

    public static Boolean IsSessionExists(string sessionId)
    {
        try
        {
            if (null != sessionId && "" != sessionId)
            {
                MgUserInformation cred = new MgUserInformation();
                cred.SetMgSessionId(sessionId);
                cred.SetMgUsernamePassword("Administrator", "admin");
                MgSiteConnection site = new MgSiteConnection();
                cred.SetLocale("en");
                //cred.SetClientIp(GetClientIp(Request));
                cred.SetClientAgent("Ajax Viewer");
                site.Open(cred);
                string session = site.GetSite().GetCurrentSession();
                if (null != session && "" != session)
                {
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        return false;
    }
}
