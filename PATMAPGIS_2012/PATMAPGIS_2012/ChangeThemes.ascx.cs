using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using CGIS.MapTools;
//using CGIS.MapTools.MapGuide6_5;

public partial class ChangeThemes : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
       InitializeComponent();        
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.dsMapThemes.Insert();
        this.gvMapThemes.EditIndex = this.gvMapThemes.Rows.Count;
        MapSettings.MapStale = true;
    }

    protected void gvMapThemes_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        MapSettings.MapStale = true;
    }

    protected void gvMapThemes_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        MapSettings.MapStale = true;
    }

    protected void gvMapThemes_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        double max, min;
        if (!double.TryParse(e.NewValues["MaxThemeValue"].ToString(), out max))
        {
            e.Cancel = true;
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ParseError", "window.alert('Parse Error');", true);
        }
        if (!double.TryParse(e.NewValues["MinThemeValue"].ToString(), out min))
        {
            e.Cancel = true;
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ParseError", "window.alert('Parse Error');", true);
        }

        MapThemeDAL dal = new MapThemeDAL();
        this.rbPercentage.Checked = dal.isPercent(MapSettings.MapThemeID);
        if (dal.isPercent(MapSettings.MapThemeID))
        {
            e.NewValues["LegendLabel"] = min.ToString() + "% - " + max.ToString() + "%";
        }
        else
        {
            e.NewValues["LegendLabel"] = "$" + min.ToString() + " - $" + max.ToString();
        }
    }

    protected void gvMapThemes_Init(object sender, EventArgs e)
    {
        ((TemplateField)gvMapThemes.Columns[5]).EditItemTemplate = new ColourChooserTemplate("FillColorIndex");
    }

    protected void btnAddTheme_Click(object sender, EventArgs e)
    {
        this.dsMapThemeSets.InsertParameters["ThemeSetName"].DefaultValue = this.Request.Form["hdnThemeSetName"];
        this.dsMapThemeSets.InsertParameters["UserID"].DefaultValue = Session["UserID"].ToString();
        this.dsMapThemeSets.Insert();
			  //following added for showing newly added theme as selected theme	13-May-2014
				DataView dv = (DataView)this.dsMapThemeSets.Select(DataSourceSelectArguments.Empty);
				if (dv != null)
				{
					if (dv.Table.Rows.Count > 0)
					{
						int LastThemeSetID = (int)(dv.Table.Rows[dv.Table.Rows.Count - 1]["ThemeSetID"]);
						if ( LastThemeSetID > -1 )
						{
							//MapSettings.MapThemeID = LastThemeSetID;
							this.ddThemeSets.DataBind();
							ListItem li = ddThemeSets.Items.FindByValue(LastThemeSetID.ToString());
							if (li != null)
							{
								this.ddThemeSets.SelectedValue = LastThemeSetID.ToString();
								this.ddThemeSets_SelectedIndexChanged(this.ddThemeSets, new EventArgs());
							}
						}
					}
				}
        MapSettings.MapStale = true;
    }

		//do not use this function
    private void reloadMap()
    {
        if (MapSettings.MapStale)
        {
					this.Page.ClientScript.RegisterStartupScript(this.GetType(), "RefreshThemes", "parent.map.reload(); parent.title.location = parent.title.location;", true);
					//bigmove
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "RefreshThemes", "RefreshMap();", true);
        }
    }

    protected void ddThemeSets_SelectedIndexChanged(object sender, EventArgs e)
    {
        MapSettings.MapThemeID = int.Parse(this.ddThemeSets.SelectedValue);
        this.gvMapThemes.EditIndex = -1;
        MapSettings.MapStale = true;
    }

    protected void btnRename_Click(object sender, EventArgs e)
    {
        this.dsMapThemeSets.UpdateParameters["ThemeSetName"].DefaultValue = this.Request.Form["hdnThemeSetName"];
        this.dsMapThemeSets.UpdateParameters["ThemeSetID"].DefaultValue = this.ddThemeSets.SelectedValue;
        this.dsMapThemeSets.Update();
    }

    protected void btnDeleteTheme_Click(object sender, EventArgs e)
    {
        this.dsMapThemeSets.DeleteParameters["ThemeSetID"].DefaultValue = this.ddThemeSets.SelectedValue;
        this.dsMapThemeSets.Delete();
        MapSettings.MapThemeID = -1;
        this.ddThemeSets.SelectedIndex = -1;
        MapSettings.MapStale = true;
    }

    private void InitializeComponent()
    {
        this.PreRender += new System.EventHandler(this.ChangeThemes_PreRender);
    }

    private void ChangeThemes_PreRender(object sender, EventArgs e)
    {
        MapThemeDAL dal = new MapThemeDAL();
        this.rbPercentage.Checked = dal.isPercent(MapSettings.MapThemeID);
        this.rbValue.Checked = !this.rbPercentage.Checked;

        if (MapSettings.MapThemeID != -1)
        {
						this.ddThemeSets.SelectedValue = MapSettings.MapThemeID.ToString();
						this.gvMapThemes.DataBind();
        }

        if (this.gvMapThemes.EditIndex > -1)
        {
            foreach (string key in this.Page.Request.Form.AllKeys)
            {
                if (key.EndsWith("txtMinThemeValue"))
                {
                    ((TextBox)this.gvMapThemes.Rows[this.gvMapThemes.EditIndex].FindControl("txtMinThemeValue")).Text = this.Page.Request.Form[key];
                }
                else if (key.EndsWith("txtMaxThemeValue"))
                {
                    ((TextBox)this.gvMapThemes.Rows[this.gvMapThemes.EditIndex].FindControl("txtMaxThemeValue")).Text = this.Page.Request.Form[key];
                }
            }
        }

        if (this.ddThemeSets.SelectedIndex > -1 && this.ddThemeSets.SelectedItem.Text == "Default")
        {
            this.btnDeleteTheme.Visible = false;
            this.gvMapThemes.Enabled = false;
            this.btnAddThemeRow.Visible = false;
            this.rbPercentage.Enabled = false;
            this.rbValue.Enabled = false;
            this.btnRenameTheme.Visible = false;
            this.gvMapThemes.Columns[this.gvMapThemes.Columns.Count - 1].Visible = false;
        }
        else
        {
            this.btnDeleteTheme.Visible = true;
            this.gvMapThemes.Enabled = true;
            this.btnAddThemeRow.Visible = true;
            this.rbPercentage.Enabled = true;
            this.rbValue.Enabled = true;
            this.btnRenameTheme.Visible = true;
            this.gvMapThemes.Columns[this.gvMapThemes.Columns.Count - 1].Visible = true;
        }
    }

    protected void ddThemeSets_DataBound(object sender, EventArgs e)
    {
        if (this.ddThemeSets.Items.Count > 0)
        {
            this.gvMapThemes.Visible = true;
            this.ddThemeSets.SelectedIndex = 0;
            MapSettings.MapThemeID = int.Parse(this.ddThemeSets.SelectedValue);
            this.btnAddThemeRow.Visible = true;
            this.rbPercentage.Visible = true;
            this.rbValue.Visible = true;
        }
        else
        {
            this.gvMapThemes.Visible = false;
            this.btnAddThemeRow.Visible = false;
            this.rbPercentage.Visible = false;
            this.rbValue.Visible = false;
            MapSettings.MapThemeID = -1;
        }
    }
    protected void btnRefreshMap_Click(object sender, EventArgs e)
    {
        string q_UserID = Session["UserID"].ToString();
        MapThemeDAL dal = new MapThemeDAL();
        int q_themSetId = dal.GetThemeSetIdByUserIDAndThemSetName(ddThemeSets.SelectedItem.ToString(), q_UserID);
        Boolean q_IsPercent = dal.isPercent(q_themSetId);
        Dictionary<string, string> q_RawDict = dal.GetRawDictionaryForPainting(q_themSetId);


        //string q_Criterion = "FeatId";
        //string q_Layer = "Municipalities_Background";
        //string q_mapName = "AnalysisMap";

				//bigmove
        //string q_Criterion = "vw_MunGroupvalue";
				string q_Criterion = "FeatId";
        string q_Layer = "Municipalities_Analysis";
				//modified on 18-oct-2013
        //string q_mapName = "AnalysisMap";
				string q_mapName = MapSettings.CurrentMapName;

        Dictionary<string, string> q_CondColor = Utility_Paint.RawDictionaryToOfficial(q_RawDict, q_Criterion, q_IsPercent);
				

        Utility_Paint.qF_PaintIt(q_mapName, q_Layer, q_CondColor);
        q_CondColor.Clear();
        q_RawDict.Clear();
			//bigmove
        //reloadMap();
      
    }

    protected void rbValue_CheckedChanged(object sender, EventArgs e)
    {
        MapThemeDAL dal = new MapThemeDAL();
        dal.setMapThemesetType(MapSettings.MapThemeID, !this.rbValue.Checked);
        MapSettings.MapStale = true;
    }

    protected void rbPercentage_CheckedChanged(object sender, EventArgs e)
    {
        MapThemeDAL dal = new MapThemeDAL();
        dal.setMapThemesetType(MapSettings.MapThemeID, this.rbPercentage.Checked);
        MapSettings.MapStale = true;
    }

    protected void dsMapThemes_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
    }

		protected void gvMapThemes_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
}
