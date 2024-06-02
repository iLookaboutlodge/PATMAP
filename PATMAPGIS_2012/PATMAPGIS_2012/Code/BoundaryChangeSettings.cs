using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for BoundaryChangeSettings
/// </summary>
public class BoundaryChangeSettings
{
    public BoundaryChangeSettings()
    {
    }

    public static string Subject
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["MapBoundaryChangeSubject"] != null)
                {
                    return (string)HttpContext.Current.Session["MapBoundaryChangeSubject"];
                }
            }
            catch { }

            return null;
        }
        set
        {
            HttpContext.Current.Session.Add("MapBoundaryChangeSubject", value);
        }
    }

    public static string Source
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["MapBoundaryChangeSource"] != null)
                {
                    return (string)HttpContext.Current.Session["MapBoundaryChangeSource"];
                }
            }
            catch { }

            return null;
        }
        set
        {
            HttpContext.Current.Session.Add("MapBoundaryChangeSource", value);
        }
    }

    public static string Destination
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["MapBoundaryChangeDestination"] != null)
                {
                    return (string)HttpContext.Current.Session["MapBoundaryChangeDestination"];
                }
            }
            catch { }

            return null;
        }
        set
        {
            HttpContext.Current.Session.Add("MapBoundaryChangeDestination", value);
        }
    }

    public static int GroupID
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["MapBoundaryChangeGroupID"] != null)
                {
                    return (int)HttpContext.Current.Session["MapBoundaryChangeGroupID"];
                }
            }
            catch { }

            return -1;
        }
        set
        {
            HttpContext.Current.Session.Add("MapBoundaryChangeGroupID", value);
        }
    }

    public static string GroupName
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["MapGroupName"] != null)
                {
                    return (string)HttpContext.Current.Session["MapGroupName"];
                }
            }
            catch { }

            return null;
        }
        set
        {
            HttpContext.Current.Session.Add("MapGroupName", value);
        }
    }

    public static bool RestructuredLevy
    {
        get
        {
            try
            {
                if (HttpContext.Current.Session["MapRestructuredLevy"] != null)
                {
                    return (bool)HttpContext.Current.Session["MapRestructuredLevy"];
                }
            }
            catch { }

            return false;
        }
        set
        {
            HttpContext.Current.Session.Add("MapRestructuredLevy", value);
        }
    }

    public enum BOUNDARY_CHANGE_STATE
    {
        NONE = 0,
        STALE = 1,
        USER = 2,
        LTT = 3
    }

    public static BOUNDARY_CHANGE_STATE BoundaryChangeState
    {
        get
        {
            if (HttpContext.Current.Session["BoundaryChangeStale"] == null)
            {
                if (HttpContext.Current.Session["LTTMap"] == null)
                {
                    return BOUNDARY_CHANGE_STATE.NONE;
                }
                else
                {
                    return BOUNDARY_CHANGE_STATE.LTT;
                }
            }
            if ((bool)HttpContext.Current.Session["BoundaryChangeStale"] == true)
            {
                return BOUNDARY_CHANGE_STATE.STALE;
            }
            return BOUNDARY_CHANGE_STATE.USER;
        }
        set
        {
            if (value == BOUNDARY_CHANGE_STATE.NONE)
            {
                HttpContext.Current.Session.Remove("BoundaryChangeStale");
            }
            else if (value == BOUNDARY_CHANGE_STATE.LTT)
            {
                HttpContext.Current.Session.Remove("BoundaryChangeStale");
            }
            else if (value == BOUNDARY_CHANGE_STATE.STALE)
            {
                HttpContext.Current.Session.Add("BoundaryChangeStale", true);
            }
            else if (value == BOUNDARY_CHANGE_STATE.USER)
            {
                HttpContext.Current.Session.Add("BoundaryChangeStale", false);

            }
        }

    }
}
