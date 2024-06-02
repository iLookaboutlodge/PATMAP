using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class FiltersControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    void Page_PreRender(object sender, EventArgs e)
    {
        if (this.chkPropertyClasses.Items.Count > 0)
        {
            setFilters();
        }
    }

    private void setFilters()
    {
        if (MapSettings.MapPropertyClassFilters != null)
        {
            foreach (ListItem item in this.chkPropertyClasses.Items)
            {
                item.Selected = false;
            }
            List<string> propertyClassFilters = MapSettings.MapPropertyClassFilters;
            foreach (string propertyClassFilter in propertyClassFilters)
            {
                ListItem item = this.chkPropertyClasses.Items.FindByValue(propertyClassFilter);
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }
        else
        {
            foreach (ListItem item in this.chkPropertyClasses.Items)
            {
                item.Selected = true;
            }
        }
    }

    protected void chkPropertyClasses_DataBound(object sender, EventArgs e)
    {
        setFilters();
    }

    protected void chkPropertyClasses_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<string> filters = new List<string>();
        foreach (ListItem item in this.chkPropertyClasses.Items)
        {
            if (item.Selected)
            {
                filters.Add(item.Value);
            }
        }
        if ((MapSettings.MapPropertyClassFilters == null && filters.Count > 0) || (MapSettings.MapPropertyClassFilters != null && !filters.Count.Equals(MapSettings.MapPropertyClassFilters.Count)))
        {
            MapSettings.MapPropertyClassFilters = filters;
            MapSettings.MapStale = true;
						MapSettings.IsRequiredTableCreate = true;
        }
    }
}
