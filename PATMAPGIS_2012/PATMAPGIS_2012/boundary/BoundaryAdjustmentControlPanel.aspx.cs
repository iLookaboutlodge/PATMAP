using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;

//using CGIS.MapTools;
using GisSharpBlog.NetTopologySuite.IO;
using GisSharpBlog.NetTopologySuite.Features;
using GeoAPI.Geometries;
using PATMAPCGIS;

/// <summary>
/// The control panel for changing boundaries
/// </summary>
public partial class BoundaryAdjustmentControlPanel : MappingBasePage
{
    /// <summary>
    /// Handles the Load event of the Page control.
    /// Builds a zoom to to zoom into the source and destination boundaries
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
				int UserID = (int)System.Web.HttpContext.Current.Session["UserID"];
				int levelID = int.Parse(Session["levelID"].ToString());

				////process to auto load map when load completes
				Page.LoadComplete += new EventHandler(Page_LoadComplete);

        if (!this.IsPostBack)
        {
						MapSettings.MapAnalysisLayer = "Municipalities";
						MapSettings.MapThemeID = 13;

						string source = BoundaryChangeSettings.Source;
            string destination = BoundaryChangeSettings.Destination;
            //this.lblSource.Text = source;
            //this.lblDestination.Text = destination;
            string layerName = ConfigurationManager.AppSettings["MunicipalitiesLayerName"];

            StringBuilder cmdStr = new StringBuilder();
            cmdStr.Append("SELECT (SELECT jurisdiction FROM entities WHERE number = @sourceID) source,  (SELECT jurisdiction FROM entities WHERE number = @destID) destination, (ST.MaxY(the_geom) + ST.MinY(the_geom)) / 2 as lat, (ST.MaxX(the_geom) + ST.MinX(the_geom)) / 2 as lon, width, height");
            cmdStr.Append(" FROM (");
            //cmdStr.Append(" SELECT ST.Transform(ST.EnvelopeAggregate(the_geom), 4043) as the_geom, MAX(ST.MaxX(the_geom)) - MIN(ST.MinX(the_geom)) as width, MAX(ST.MaxY(the_geom)) - MIN(ST.MinY(the_geom)) as height");
						cmdStr.Append(" SELECT ST.EnvelopeAggregate(the_geom) as the_geom, MAX(ST.MaxX(the_geom)) - MIN(ST.MinX(the_geom)) as width, MAX(ST.MaxY(the_geom)) - MIN(ST.MinY(the_geom)) as height");
            cmdStr.Append(" FROM Municipalities INNER JOIN [MunicipalitiesMapLink] ON MunID = [PPID]");
            cmdStr.Append(" WHERE [SAMA_Code] = @destID OR [SAMA_Code] = @sourceID");
            cmdStr.Append(" ) Municipalities");            

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPCOnnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand(cmdStr.ToString(), conn);
            cmd.Parameters.Add("@destID", SqlDbType.VarChar, 50).Value = destination;
            cmd.Parameters.Add("@sourceID", SqlDbType.VarChar, 50).Value = source;

            conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            double lat = 0;
            double lon = 0;
            double width = 0;
            double height = 0;

            string sourceName = string.Empty;
            string destName = string.Empty;

            if (reader != null)
            {
                if (reader.Read())
                {
                    if (reader["lat"] != DBNull.Value)
                    {
                        lat = (double)(reader["lat"]);
                    }
                    if (reader["lon"] != DBNull.Value)
                    {
                        lon = (double)(reader["lon"]);
                    }
                    if (reader["width"] != DBNull.Value)
                    {
                        width = (double)(reader["width"]);
                    }
                    if (reader["height"] != DBNull.Value)
                    {
                        height = (double)(reader["height"]);
                    }
                    if (reader["source"] != DBNull.Value)
                    {
                        sourceName = (string)(reader["source"]);
                    }
                    if (reader["destination"] != DBNull.Value)
                    {
                        destName = (string)(reader["destination"]);
                    }
                }
                reader.Close();
            }

            this.lblSource.Text = sourceName;
            this.lblDestination.Text = destName;
					
						HiddenField ctrl = (HiddenField)this.FindControl("hdnLTTMap");
						if (ctrl != null)
						{
							//string coord = PATMAPCGIS.Zoom.getzoomCoordinate(Session["CODE_DestinationMunicipality"].ToString());
							//string coord = PATMAPCGIS.Zoom.getzoomCoordinate("005");
							//string coord = PATMAPCGIS.Zoom.getzoomBoundaryChangeCoordinate(UserID);
							string coord = lon.ToString() + ", " + lat.ToString() + ", " + width.ToString() + ", " + height.ToString();
							ctrl.Value = "BOUNDARYCHANGEMap;" + coord.Trim();
						}
	
						//IMap map = MapManager.getBoundaryAdjustmentMap();
						
						//foreach (IMapLayer layer in map.MapLayers)
						//{
						//    if (layer.Name.Equals("Municipalities"))
						//    {
						//        this.pnlDestColour.Style.Add("background", MapManager.convertToColour(layer.LayerStyles[0].LayerThemes[2].FillColorIndex));                               
						//    }
						//    else if (layer.Name.Equals("Source"))
						//    {
						//        this.pnlSourceColour.Style.Add("background", MapManager.convertToColour(layer.LayerStyles[0].FillColorIndex));
						//    }
                
						//}            
            
						//this.Page.ClientScript.RegisterStartupScript(Type.GetType("AnalysisBasePage"), "Zoom", "zoomBox(" + lat.ToString() + ", " + lon.ToString() + ", " + width.ToString() + ", " + height.ToString() + ");", true);
        }
    }


		protected void LinkButton1_Click(object sender, EventArgs e)
		{
			//MapSettings.IsRequiredTableCreate = false;
			MapSettings.IsRequiredTableCreate = true;
			//MapManagerNew.getAnalysisMap(MapSettings.IsRequiredTableCreate);
			MapManagerNew.getBoundaryAdjustmentMap(MapSettings.IsRequiredTableCreate);
		}

    /// <summary>
    /// Handles the MenuItemClick event of the tabControls control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void tabControls_MenuItemClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        MultiView1.ActiveViewIndex = int.Parse(btn.CommandName);

        for (int i = 0; i < tblNavigation.Rows[0].Cells.Count; i++)
        {
            if (MultiView1.ActiveViewIndex == i)
            {
                tblNavigation.Rows[0].Cells[i].Attributes.Add("class", "navigationSelected");
            }
            else
            {
                tblNavigation.Rows[0].Cells[i].Attributes.Remove("class");
            }
        }
    }

    /// <summary>
    /// Does the boundary change.
    /// </summary>
    private void doBoundaryChange()
    {
        if (this.Request.Form["SelectedParcels"] != null)
        {
            string[] parcels = this.Request.Form["SelectedParcels"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] iscParcels = this.Request.Form["SelectedISCParcels"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (parcels.Length > 0 || iscParcels.Length > 0)
            {
                int userID = (int)Session["UserID"];
                int groupID = (int)Session["MapboundaryGroupId"];

                List<string> parcelIDs = new List<string>(parcels);
                List<string> iscParcelIDs = new List<string>(iscParcels);

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
                                          
                
                //StringBuilder str = new StringBuilder();

                ////changes selected parcels into a new boundary
                //str.Append("DECLARE @newDestGeom varbinary(MAX) ");
                //str.Append("IF EXISTS (SELECT * FROM MunicipalitiesChanges ");
                //str.Append("WHERE UserID = @userID AND MunID = @destination) ");
                //str.Append("BEGIN ");
                //setNewDestGeom(parcelIDs, iscParcelIDs, str, true);

                //str.Append("UPDATE MunicipalitiesChanges ");
                //str.Append("SET the_geom = @newDestGeom WHERE MunID = @destination AND UserID = @userID ");
                //str.Append("END ");

                //str.Append("ELSE ");
                //str.Append("BEGIN ");
                //setNewDestGeom(parcelIDs, iscParcelIDs, str, false);
                //str.Append("INSERT INTO MunicipalitiesChanges (the_geom, MunID, UserID) ");
                //str.Append("VALUES(@newDestGeom, @destination, @userID) ");
                //str.Append("END ");

                ////str.Append("SELECT @newDestGeom = ST.Boundary(@newDestGeom) ");

                ////subtracting the modified parcels from the original boundary
                //str.Append("IF EXISTS (SELECT * FROM MunicipalitiesChanges ");
                //str.Append("WHERE UserID = @userID ");
                //str.Append("AND MunID = @source) ");
                //str.Append("BEGIN ");
                //str.Append("UPDATE MunicipalitiesChanges ");

                //str.Append("SET the_geom = CASE WHEN ST.Intersects(the_geom, @newDestGeom) = 1 THEN ST.Difference(the_geom, @newDestGeom) ELSE the_geom END ");
                //str.Append("WHERE MunID = @source ");
                //str.Append(" AND UserID = @userID ");
                //str.Append(" END ");
                //str.Append(" ELSE ");
                //str.Append(" BEGIN ");
                //str.Append("INSERT INTO MunicipalitiesChanges (the_geom, MunID, UserID) ");
                //str.Append(" SELECT ST.Difference(the_geom, @newDestGeom) as the_geom, @source, @userID ");
                //str.Append(" FROM Municipalities ");
                //str.Append("WHERE MunID = @source ");
                //str.Append(" END ");

                ////Select data for the current user only, changes are selected from the changes table
                //str.Append("SELECT MunID, the_geom FROM Municipalities WHERE MunID IS NOT NULL AND NOT EXISTS ");
                //str.Append("(SELECT * FROM MunicipalitiesChanges WHERE UserID = @userID ");
                //str.Append("AND Municipalities.MunID = MunicipalitiesChanges.MunID) ");
                //str.Append("UNION ");
                //str.Append("SELECT MunID, the_geom ");
                //str.Append("FROM MunicipalitiesChanges ");
                //str.Append("WHERE UserID = @userID ");

                //SqlCommand cmd = new SqlCommand(str.ToString(), conn);

                
                //string source = BoundaryChangeSettings.Source;
                //string destination = BoundaryChangeSettings.Destination;
                //cmd.Parameters.Add("@destination", SqlDbType.VarChar, 255).Value = destination;
                //cmd.Parameters.Add("@source", SqlDbType.VarChar, 255).Value = source;
                //cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                
                //conn.Open();
                //cmd.CommandTimeout = 600; // Wait ten minutes before timing out
                //SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //BuildBoundary.writeToShapefile(reader, userID);

                //reader.Close();

                StringBuilder writeAssessmentNumbers = new StringBuilder();
                writeAssessmentNumbers.Append("INSERT INTO mapBoundaryTransfers (UserID, alternate_parcelID, municipalityID, groupid) ");
                writeAssessmentNumbers.Append("SELECT DISTINCT @UserID, assessmentNumber, munID, @groupid FROM ParcelToAssessment WHERE MapParcelID = @ParcelID AND NOT EXISTS(SELECT * FROM mapBoundaryTransfers WHERE UserID = @UserID AND alternate_parcelID = assessmentNumber AND municipalityID = munID AND groupid = @groupid)");
                SqlCommand writeAssessmentNumbersCmd = new SqlCommand(writeAssessmentNumbers.ToString(), conn);
                writeAssessmentNumbersCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                writeAssessmentNumbersCmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = groupID;
                writeAssessmentNumbersCmd.Parameters.Add("@ParcelID", SqlDbType.VarChar, 50);
                conn.Open();
                foreach (string parcelID in parcelIDs)
                {
                	writeAssessmentNumbersCmd.Parameters["@ParcelID"].Value = parcelID;
                    writeAssessmentNumbersCmd.ExecuteNonQuery();
                }
                conn.Close();

                BoundaryChangeSettings.BoundaryChangeState = BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.USER;
            }

        }
    }

    

    private static void setNewDestGeom(List<string> parcelIDs, List<string> iscParcelIDs, StringBuilder str, bool alreadyChanged)
    {
        if (parcelIDs.Count > 0)
        {
            str.Append("SELECT @newDestGeom = ST.CollectAggregate(the_geom) ");
            str.Append("FROM AssessmentParcels ");
            str.Append("WHERE P_ID IN ('");
            foreach (string parcelID in parcelIDs)
            {
                str.Append(parcelID);
                str.Append("', '");
            }
            //delete the last comma and seal it off with a bracket
            str.Remove((str.Length - 4), 3);
            str.Append(") ");
        }
        /*
        if (iscParcelIDs.Count > 0)
        {
            str.Append("SELECT @newDestGeom = ST.CollectAggregate(the_geom) ");
            str.Append("FROM ISC_Parcels ");
            str.Append("WHERE PPID IN ('");
            foreach (string iscParcelID in iscParcelIDs)
            {
                str.Append(iscParcelID);
                str.Append("', '");
            }
            //delete the last comma and seal it off with a bracket
            str.Remove((str.Length - 4), 3);
            str.Append(") ");
        }*/

        str.Append("SELECT @newDestGeom = ST.GeomUnion(ST.Buffer(@newDestGeom, 0), the_geom) ");
        str.Append("FROM ");
        if (alreadyChanged)
        {
            str.Append("MunicipalitiesChanges ");
        }
        else
        {
            str.Append("Municipalities ");
        }
        str.Append("WHERE MunID = @destination ");
        if (alreadyChanged)
        {
            str.Append("AND UserID = @userID ");
        }
    }

    /// <summary>
    /// Handles the Click event of the btnBoundaryChange control.
    /// Performs a boundary change and reloads the map
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnBoundaryChange_Click(object sender, EventArgs e)
    {
        doBoundaryChange();
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "RefreshThemes", "top.location = 'main.aspx';", true);
    }

		protected void Page_LoadComplete(object sender, EventArgs e)
		{
			//process to auto load map when load completes
			if (!IsPostBack)
			{
				//only do auto postback once
				if (System.Web.HttpContext.Current.Session["IsPrintStale"] == null)
				{
					System.Web.HttpContext.Current.Session.Add(("IsPrintStale"), 1);
					this.Page.ClientScript.RegisterStartupScript(this.GetType(), "AutoScript", "execBAAutoRefresh();", true);
				}
			}
		}

}
