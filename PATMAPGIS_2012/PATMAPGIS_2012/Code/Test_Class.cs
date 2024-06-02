using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

using OSGeo.MapGuide;

public class Test_Class
{

    public static void Test_Mun()
    {

        string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
        MapThemeDAL dal = new MapThemeDAL();
        int q_themSetId = MapSettings.MapThemeID;
        Boolean isPer = dal.isPercent(q_themSetId);

        //string layerName = "Municipalities_Analysis";
        string layerName = "TEST_Mun";
        string mapName = MapSettings.CurrentMapName;

        int userid = Convert.ToInt32(q_UserID);
        string session = System.Web.HttpContext.Current.Session["Session"].ToString();
        Utility_Paint utility = new Utility_Paint();

        utility.ConnectToServer(session);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        string layerDefId = utility.GetLayerDefinitionResourceId(layerName, session, mapName);
        string sessionLayerId = utility.SetFilterForLayerSimlpe(session, layerDefId);

        utility.ReplaceWithFilteredLayer(session, sessionLayerId, layerName, mapName);
        string LayerName = layerName + "_filtered";

        string q_TableName = Ut_SQL2TT.GetMunTableName(userid);
        //if (IsRequiredTableCreate)
        //{
        //    Ut_SQL2TT.CreateMunTable(q_TableName, isPer);
        //}

        //Ut_SQL2TT.qF_changePrimeAndSecondaryPropOfFeatureSource(
        //    LayerName,
        //    mapName,
        //    "munid",
        //    "MunicipalityID",
        //    "dbo:" + q_TableName,
        //    "Library://PATMAP/Data/PATMAP_SQL_HGIS.FeatureSource",
        //    "Municipalities4",
        //    "Municipalities4Extended",
        //    "Default:Municipalities4"//from layer def
        //    );

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


}
