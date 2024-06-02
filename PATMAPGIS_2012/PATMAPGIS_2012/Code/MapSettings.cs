using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
//using CGIS.MapTools;
using System.Text;

/// <summary>
/// Summary description for MapSettings
/// </summary>
public class MapSettings
{
    public MapSettings()
    {
    }

    public enum TaxShiftFilter
    {
        Tax = 0,
        Assessment = 1,
        //Grant = 2,
        Credit = 2,
        ProvincialSchoolTax = 3
    }

    public static bool MapStale
    {
        get
        {
            object mapStale = HttpContext.Current.Session["MapStale"];
            return mapStale != null ? (bool)mapStale : true;
        }
        set
        {
            HttpContext.Current.Session["MapStale"] = value;
        }
    }

    public static int MapThemeID
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["MapThemeID"] != null)
                {
                    return (int)HttpContext.Current.Session["MapThemeID"];
                }
            }
            catch
            { }

            return -1;
        }
        set
        {
            if (value == -1)
            {
                HttpContext.Current.Session.Remove("MapThemeID");
            }
            else
            {
                HttpContext.Current.Session.Add("MapThemeID", value);
            }
        }
    }

    public static List<string> MapPropertyClassFilters
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["MapPropertyClassFilters"] != null)
                {
                    return (List<string>)HttpContext.Current.Session["MapPropertyClassFilters"];
                }
            }
            catch { }

            return null;
        }
        set
        {
            HttpContext.Current.Session.Add("MapPropertyClassFilters", value);
        }
    }

    public static string MapAnalysisLayer
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["MapAnalysisLayer"] != null)
                {
                    return (string)HttpContext.Current.Session["MapAnalysisLayer"];
                }
            }
            catch { }

            return "Municipalities";
        }
        set
        {
            HttpContext.Current.Session.Add("MapAnalysisLayer", value);
        }
    }

    public static List<string> TaxShiftFilters
    {
        get
        {
            if (HttpContext.Current.Session["MapTaxShiftFilters"] == null)
            {
                HttpContext.Current.Session.Add("MapTaxShiftFilters", new List<string>());
            }
            return (List<string>)HttpContext.Current.Session["MapTaxShiftFilters"];
        }
        set
        {

            HttpContext.Current.Session.Add("MapTaxShiftFilters", value);
        }
    }

    public static List<string> TaxStatusFilters
    {
        get
        {
            if (HttpContext.Current.Session["MapTaxStatusFilters"] == null)
            {
                HttpContext.Current.Session.Add("MapTaxStatusFilters", new List<string>());
            }
            return (List<string>)HttpContext.Current.Session["MapTaxStatusFilters"];
        }
        set
        {
            HttpContext.Current.Session.Add("MapTaxStatusFilters", value);
        }
    }

    public static Boolean IsRequiredTableCreate
    {
        get
        {
            if (System.Web.HttpContext.Current.Session["IsRequiredTableCreate"] == null)
            {
                System.Web.HttpContext.Current.Session.Add(("IsRequiredTableCreate"), false);
            }
            object IsRequiredTableCreate = HttpContext.Current.Session["IsRequiredTableCreate"];
            return IsRequiredTableCreate != null ? (Boolean)IsRequiredTableCreate : false;
        }
        set
        {
            if (System.Web.HttpContext.Current.Session["IsRequiredTableCreate"] == null)
            {
                System.Web.HttpContext.Current.Session.Add(("IsRequiredTableCreate"), value);
            }
            else
            {
                System.Web.HttpContext.Current.Session["IsRequiredTableCreate"] = value;
            }
        }
    }

    public static string CurrentMapName
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["CurrentMapName"] != null)
                {
                    return (string)HttpContext.Current.Session["CurrentMapName"];
                }
            }
            catch { }
            return "";
        }
        set
        {
            HttpContext.Current.Session.Add("CurrentMapName", value);
        }
    }

    public static string CurrentWebLayout
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["CurrentWebLayout"] != null)
                {
                    return (string)HttpContext.Current.Session["CurrentWebLayout"];
                }
            }
            catch { }
            return "";
        }
        set
        {
            HttpContext.Current.Session.Add("CurrentWebLayout", value);
        }
    }

}
