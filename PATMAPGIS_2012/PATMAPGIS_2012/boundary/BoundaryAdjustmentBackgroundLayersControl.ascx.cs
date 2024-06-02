using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using CGIS.MapTools;
//using CGIS.MapTools.MapGuide6_5;

public partial class BoundaryAdjustmentBackgroundLayersControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
        //    IMap map = MapManager.getBoundaryAdjustmentMap();

        //    addLayers(map.MapLayers, tblLayers);
            
        //    foreach (IMapGroup group in map.MapGroups)
        //    {
        //        Table tblGroup = createGroupPanel(group);
        //        addLayers(group.MapLayers, tblGroup);
        //    }

        //}
        //catch (Exception ex)
        //{
        //    this.Response.Write(ex);
        //    this.Response.End();
        //}
    }

    //private static void addLayers(List<IMapLayer> layers, Table tblGroup)
    //{
    //    foreach (IMapLayer layer in layers)
    //    {
    //        if (layer.ShowInLegend)
    //        {
    //            CheckBox chkLayer = new CheckBox();
    //            chkLayer.Text = layer.LegendLabel;
    //            chkLayer.Checked = layer.Visible;
    //            chkLayer.Attributes.Add("onclick", "toggleLayer('" + layer.Name + "');");

    //            TableCell cellChk = new TableCell();
    //            cellChk.Controls.Add(chkLayer);

    //            TableCell cellImg = new TableCell();
    //            TableRow layerRow = new TableRow();
    //            layerRow.Cells.Add(cellChk);
    //            layerRow.Cells.Add(cellImg);

    //            tblGroup.Rows.Add(layerRow);
    //        }
    //    }
    //}

    //private Table createGroupPanel(IMapGroup group)
    //{
    //    Panel pnlGroup = new Panel();
    //    pnlGroup.BorderWidth = 1;   

    //    CheckBox chkGroup = new CheckBox();
    //    chkGroup.Text = group.LegendLabel;
    //    chkGroup.Checked = group.Visible;
    //    chkGroup.Attributes.Add("onclick", "toggleGroup('" + group.Name + "');");
    //    pnlGroup.Controls.Add(chkGroup);

    //    Image imgGroup = new Image();
    //    imgGroup.ImageUrl = "images/arrow.gif";
    //    pnlGroup.Controls.Add(imgGroup);

    //    Table tblGroup = new Table();
    //    tblGroup.Style.Add("display", "none");
    //    pnlGroup.Controls.Add(tblGroup);

    //    TableCell cellGroup = new TableCell();
    //    cellGroup.Controls.Add(pnlGroup);
    //    TableRow row = new TableRow();
    //    row.Cells.Add(cellGroup);
    //    tblGroups.Rows.Add(row);

    //    imgGroup.Attributes.Add("onclick", "toggleGroupPanel('" + tblGroup.ClientID + "');");
    //    return tblGroup;
    //}
}
