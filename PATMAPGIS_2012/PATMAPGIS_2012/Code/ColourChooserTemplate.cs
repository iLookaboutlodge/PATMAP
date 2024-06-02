using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

/// <summary>
/// The template for the colour chooser control in the change themes panel
/// </summary>
public class ColourChooserTemplate : System.Web.UI.Page, IBindableTemplate
{
    protected Panel pnlFillColorDisplay;        //The main panel to be filled with colour
    protected string column;                    //The column to reference
    protected HiddenField hdnFillColorIndex;    //The hidden field which contains the colour index

    public ColourChooserTemplate(string column)
    {
        this.column = column;
    }

    //When updating a row the colour index is returned through this method
    public System.Collections.Specialized.IOrderedDictionary ExtractValues(Control container)
    {
        OrderedDictionary dict = new OrderedDictionary();
        string value = hdnFillColorIndex.Value.ToString();
        dict.Add(this.column, value);
        return dict;
    }

    //Builds the control
    public void InstantiateIn(Control container)
    {
        hdnFillColorIndex = new HiddenField();
        hdnFillColorIndex.DataBinding += new EventHandler(hdnFillColorIndex_DataBinding);
        hdnFillColorIndex.PreRender += new EventHandler(hdnFillColorIndex_PreRender);
        hdnFillColorIndex.ID = "hdnFillColorIndex";
        hdnFillColorIndex.EnableViewState = true;
        container.Controls.Add(hdnFillColorIndex);

        pnlFillColorDisplay = new Panel();
        //added to fix browser issue with ie 8+ 13-May-2014
        pnlFillColorDisplay.Controls.Add(new LiteralControl("&nbsp"));
        pnlFillColorDisplay.Style.Add("width", "20px");
        pnlFillColorDisplay.Style.Add("border", "solid 1px black");
        pnlFillColorDisplay.DataBinding += new EventHandler(pnlFillColorDisplay_DataBinding);
        pnlFillColorDisplay.PreRender += new EventHandler(pnlFillColorDisplay_PreRender);
        pnlFillColorDisplay.Attributes.Add("onclick", "openColourWindow();");
        pnlFillColorDisplay.Style.Add("Cursor", "hand");
        pnlFillColorDisplay.EnableViewState = true;
        container.Controls.Add(pnlFillColorDisplay);
    }

    //Fills the panel with the correct colour while updating by reading from the form variables
    void pnlFillColorDisplay_PreRender(object sender, EventArgs e)
    {
        if (System.Web.HttpContext.Current.Request.Form["hdnFillColorIndex"] != null && System.Web.HttpContext.Current.Request.Form["hdnFillColorIndex"].Length > 0)
        {
            pnlFillColorDisplay.Style.Add("background", MapManagerNew.convertToColour(int.Parse(System.Web.HttpContext.Current.Request.Form["hdnFillColorIndex"])));
        }
    }

    //Fills the hidden field with the colour index while updating by reading from the form variables
    void hdnFillColorIndex_PreRender(object sender, EventArgs e)
    {
        if (System.Web.HttpContext.Current.Request.Form["hdnFillColorIndex"] != null && System.Web.HttpContext.Current.Request.Form["hdnFillColorIndex"].Length > 0)
        {
            hdnFillColorIndex.Value = System.Web.HttpContext.Current.Request.Form["hdnFillColorIndex"];
        }
    }

    //Fills the hidden field with the colour index
    void hdnFillColorIndex_DataBinding(object sender, EventArgs e)
    {
        hdnFillColorIndex = (HiddenField)sender;
        GridViewRow row = (GridViewRow)hdnFillColorIndex.NamingContainer;
        hdnFillColorIndex.Value = DataBinder.Eval(row.DataItem, column).ToString();
    }

    //Fills the panel with the colour
    void pnlFillColorDisplay_DataBinding(object sender, EventArgs e)
    {
        pnlFillColorDisplay = (Panel)sender;
        GridViewRow row = (GridViewRow)pnlFillColorDisplay.NamingContainer;
        pnlFillColorDisplay.Style.Add("background", MapManagerNew.convertToColour((int)DataBinder.Eval(row.DataItem, column)));
    }

}
