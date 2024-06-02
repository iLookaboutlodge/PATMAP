using System;
using System.Data;
using System.Data.SqlClient;
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
using System.Text;
using OSGeo.MapGuide;
using AIMS2012ToolLib;

public partial class ParcelThemes : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            IDataReader taxClasses = getTaxClasses();
            addLayers(taxClasses, tblParcelTypes);
            taxClasses.Close();
        }
        catch (Exception ex)
        {
            this.Response.Write(ex);
            this.Response.End();
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (MapSettings.MapPropertyClassFilters != null)
        {
            foreach (TableRow row in this.tblParcelTypes.Rows)
            {
                ((CheckBox)row.Cells[0].Controls[0]).Checked = MapSettings.MapPropertyClassFilters.Contains(((HiddenField)row.Cells[0].Controls[1]).Value);
                //if (((CheckBox)row.Cells[0].Controls[0]).Checked)
                {
										if (row.Cells.Count  > 2)
										{
											Table subTable = ((Table)row.Cells[2].Controls[0]);
											foreach (TableRow subRow in subTable.Rows)
											{
												((CheckBox)subRow.Cells[0].Controls[0]).Checked = MapSettings.MapPropertyClassFilters.Contains(((HiddenField)subRow.Cells[0].Controls[1]).Value);
											}
										}
                }
            }            
        }
        else
        {
            foreach (TableRow row in this.tblParcelTypes.Rows)
            {
                ((CheckBox)row.Cells[0].Controls[0]).Checked = true;
								if (row.Cells.Count > 2)
								{
									Table subTable = ((Table)row.Cells[2].Controls[0]);
									foreach (TableRow subRow in subTable.Rows)
									{
										((CheckBox)subRow.Cells[0].Controls[0]).Checked = true;
									}
								}
            }
        }
       
    }

    private void addLayers(IDataReader taxClasses, Table tblParcelTypes)
    {
        //MgMap map = MapManager.getAnalysisMap();
				MgMap map = Ut_SQL2TT.GetSessionMap();
        MgLayer maplayer = null;
        MgLayerCollection mgLayers = map.GetLayers();
        foreach (MgLayer layer in mgLayers)
        {
            if (layer.Name.Equals(ConfigurationManager.AppSettings["ParcelLayerName"]))
            {
                maplayer = layer;
            }
        }
        while (taxClasses.Read())
        {
            string id = (string)taxClasses["taxClassID"];
            string taxClass = (string)taxClasses["taxClass"];

            CheckBox chkLayer = new CheckBox();
            chkLayer.CheckedChanged += new EventHandler(chkLayer_CheckedChanged);
            chkLayer.AutoPostBack = true;
            chkLayer.Text = taxClass;
            chkLayer.Checked = false;
            HiddenField hdn = new HiddenField();
            hdn.Value = id;

            TableCell cellChk = new TableCell();
            cellChk.Controls.Add(chkLayer);
            cellChk.Controls.Add(hdn);

            
            TableCell cellImg = new TableCell();
            /*foreach (ILayerTheme theme in maplayer.LayerStyles[0].LayerThemes)
            {
                if (theme.MinThemeValue.Equals(id))
                {
                    Panel pnl = new Panel();
                    //pnl.Style.Add("width", "20px");
                    //pnl.Style.Add("background", MapManager.convertToColour(theme.FillColorIndex));
                    cellImg.Controls.Add(pnl);
                    break;
                }
            }*/

            TableRow layerRow = new TableRow();
            layerRow.CssClass = "parcelThemeRow";
            layerRow.Cells.Add(cellChk);
            layerRow.Cells.Add(cellImg);

						//Add sub classes
						IDataReader reader = getTaxClasses(id);
						TableCell cellSubClasses = new TableCell();
						Table newTable = new Table();
						cellSubClasses.Controls.Add(newTable);
						layerRow.Cells.Add(cellSubClasses);
						addSubLayers(reader, newTable);
            tblParcelTypes.Rows.Add(layerRow);
        }
        taxClasses.Close();
    }

    private void addSubLayers(IDataReader taxClasses, Table tblParcelSubTypes)
    {
        //MgMap map = MapManager.getAnalysisMap();
        //MgLayer maplayer = null;
        //MgLayerCollection mgLayers = map.GetLayers();
        //foreach (MgLayer layer in mgLayers)
        //{
        //    if (layer.Name.Equals(ConfigurationManager.AppSettings["ParcelLayerName"]))
        //    {
        //        maplayer = layer;
        //    }
        //}
        while (taxClasses.Read())
        {
            string id = (string)taxClasses["taxClassID"];
            string taxClass = (string)taxClasses["taxClass"];

            CheckBox chkLayer = new CheckBox();
            chkLayer.CheckedChanged += new EventHandler(chkLayer_CheckedChanged);
            chkLayer.AutoPostBack = true;
            chkLayer.Text = taxClass;
            chkLayer.Checked = true;
            HiddenField hdn = new HiddenField();
            hdn.Value = id;

            TableCell cellChk = new TableCell();
            cellChk.Controls.Add(chkLayer);
            cellChk.Controls.Add(hdn);
            
            TableCell cellImg = new TableCell();
            /*foreach (ILayerTheme theme in maplayer.LayerStyles[0].LayerThemes)
            {
                if (theme.MinThemeValue.Equals(id))
                {
                    Panel pnl = new Panel();
                    //pnl.Style.Add("width", "20px");
                    //pnl.Style.Add("background", MapManager.convertToColour(theme.FillColorIndex));
                    cellImg.Controls.Add(pnl);
                    break;
                }
            }*/

            TableRow layerRow = new TableRow();
            layerRow.Cells.Add(cellChk);
            layerRow.Cells.Add(cellImg);          

            tblParcelSubTypes.Rows.Add(layerRow);
        }
        taxClasses.Close();
    }

    protected void chkLayer_CheckedChanged(object sender, EventArgs e)
    {
        List<string> filters = new List<string>();
        foreach (TableRow row in this.tblParcelTypes.Rows)
        {
            Control ctrl = row.Cells[0].Controls[0];
            Control hdn = row.Cells[0].Controls[1];
            if (ctrl != null && hdn != null)
            {
                CheckBox chkbox = (CheckBox)ctrl;
                Table subTable = ((Table)row.Cells[2].Controls[0]);
                //subTable.Enabled = chkbox.Checked;
                if (chkbox.Checked)
                {
                    filters.Add(((HiddenField)hdn).Value);
                }
                    
                foreach (TableRow subRow in subTable.Rows)
                {
                    Control subCtrl = subRow.Cells[0].Controls[0];
                    Control subHdn = subRow.Cells[0].Controls[1];
                    if (subCtrl != null && subHdn != null)
                    {
                        CheckBox subChkbox = (CheckBox)subCtrl;
                        if (subChkbox.Checked)
                        {
                            filters.Add(((HiddenField)subHdn).Value);
                        }
                    }
                }
                
            }

            

        }
        if ((MapSettings.MapPropertyClassFilters == null && filters.Count > 0) || (MapSettings.MapPropertyClassFilters != null && !filters.Count.Equals(MapSettings.MapPropertyClassFilters.Count)))
        {
            MapSettings.MapPropertyClassFilters = filters;
            MapSettings.MapStale = true;
						MapSettings.IsRequiredTableCreate = true;
        }
    }

    private IDataReader getTaxClasses()
    {
        string levelID = Session["levelID"].ToString();
        string userID = Session["UserID"].ToString();

        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
        {
            bool showFullTaxClasses = (bool)Session["showFullTaxClasses"];
        }
        StringBuilder str = new StringBuilder();



        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
        {
            if ((bool)Session["showFullTaxClasses"] == true)
            {
                str.Append("select t.taxClassID, t.taxClass, l.show from liveTaxClasses l inner join taxClasses t on l.taxClassID = t.taxClassID INNER JOIN liveLTTtaxClasses_");
                //str.Append("82");
								str.Append(userID);
                str.Append(" ON liveLTTtaxClasses_");
                //str.Append("82");
								str.Append(userID);
                str.Append(".taxClassID = t.taxClassID WHERE t.active = 1 AND liveLTTtaxClasses_");
                //str.Append("82");
								str.Append(userID);
                str.Append(".show = 1 AND userID =");
                //str.Append("82");
								str.Append(userID);
								//following added to fix multiple class display problem	13-jan-2014
								str.Append(" AND NOT EXISTS ( SELECT * FROM taxClasses parent");
								str.Append(" WHERE t.parentTaxClassID = parent.taxClassID AND parent.active = 1");
								str.Append(")");
								str.Append("ORDER BY t.sort");
            }
            if ((bool)Session["showFullTaxClasses"] == false)
            {
                str.Append("SELECT DISTINCT m.taxClassID, m.taxClass FROM liveLTTValues v INNER JOIN LTTtaxClasses t ON t.taxClassID = v.taxClassID INNER JOIN LTTmainTaxClasses m ON m.taxClassID = t.LTTmainTaxClassID INNER JOIN liveLTTtaxClasses_");
                //str.Append("82");
								str.Append(userID);
                str.Append(" l ON l.taxClassID = t.taxClassID WHERE t.active = 1 AND l.show = 1 AND userID =");
                //str.Append("82");
								str.Append(userID);
            }
        }
        else
        {
            str.Append("SELECT child.[taxClassID], child.[taxClass] FROM taxClasses child");
            str.AppendFormat(" INNER JOIN taxClassesPermission perm ON perm.taxClassID = child.taxClassID AND levelID = {0}", levelID);
            str.Append(" WHERE NOT EXISTS (");
            str.Append(" SELECT * FROM taxClasses parent ");
            str.AppendFormat(" INNER JOIN taxClassesPermission permParent ON permParent.taxClassID = parent.taxClassID AND levelID = {0} ", levelID);
            str.Append(" WHERE child.parentTaxClassID = parent.taxClassID AND parent.active = 1 AND permParent.access = 1");
            str.Append(" ) AND child.active = 1 AND perm.access = 1");
            str.Append(" ORDER BY child.sort");
        }
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        SqlCommand cmd = new SqlCommand(str.ToString(), conn);
        //cmd.Parameters.Add("@parentTaxClassID", SqlDbType.VarChar, 5).Value = parent;
        conn.Open();
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }

    private IDataReader getTaxClasses(string parent)
    {
        string levelID = Session["levelID"].ToString();

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        StringBuilder str = new StringBuilder();
        str.Append("SELECT child.[taxClassID], child.[taxClass] FROM taxClasses child");
        str.AppendFormat(" INNER JOIN taxClassesPermission perm ON perm.taxClassID = child.taxClassID AND levelID = {0}", levelID);
        str.Append(" WHERE child.active = 1 AND perm.access = 1 AND child.ParentTaxClassID = @parentTaxClassID");
        str.Append(" ORDER BY child.sort");
        SqlCommand cmd = new SqlCommand(str.ToString(), conn);
        cmd.Parameters.Add("@parentTaxClassID", SqlDbType.VarChar, 5).Value = parent;
        conn.Open();
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }
}