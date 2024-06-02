using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using GisSharpBlog.NetTopologySuite.IO;
using System.Collections.Generic;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Features;
using System.Text;

public class BuildBoundary
{
    public static void buildBoundary(int userID)
    {
        StringBuilder str = new StringBuilder();
        str.Append("INSERT INTO MunicipalitiesChanges (MunID, UserID, the_geom) ");
        str.Append(System.Environment.NewLine);
        str.Append("SELECT  MunID, @UserID, ST.GeomUnion(the_geom, ST.Buffer(parcelGeoms, 0))  ");
        str.Append(System.Environment.NewLine);
        str.Append("FROM Municipalities INNER JOIN MunicipalitiesMapLink ON MunID = PPID ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN ");
        str.Append(System.Environment.NewLine);
        str.Append("( ");
        str.Append(System.Environment.NewLine);
        str.Append("SELECT DestinationMunicipalityID as targetMunicipalityID, ST.CollectAggregate(the_geom) as parcelGeoms ");
        str.Append(System.Environment.NewLine);
        str.Append("FROM ");
        str.Append(System.Environment.NewLine);
        str.Append("boundaryGroups ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN boundaryTransfers ON  ");
        str.Append(System.Environment.NewLine);
        str.Append("boundaryGroups.boundaryGroupID = boundaryTransfers.boundaryGroupID ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN ParcelToAssessment ON  originMunicipalityID = MunID AND ");
        str.Append(System.Environment.NewLine);
        str.Append("alternate_parcelID = assessmentNumber  ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN AssessmentParcels ON mapparcelID = P_ID ");
        str.Append(System.Environment.NewLine);
        str.Append("WHERE UserID = @UserID ");
        str.Append(System.Environment.NewLine);
        str.Append("GROUP BY DestinationMunicipalityID ");
        str.Append(System.Environment.NewLine);
        str.Append(") geom ");
        str.Append(System.Environment.NewLine);
        str.Append("ON targetMunicipalityID = [SAMA_Code] ");
        str.Append(System.Environment.NewLine);

        str.Append(System.Environment.NewLine);
        str.Append("INSERT INTO MunicipalitiesChanges (MunID, UserID, the_geom) ");
        str.Append(System.Environment.NewLine);
        str.Append("SELECT MunID, @UserID, ST.Difference(ST.Buffer(ST.Simplify(the_geom, 10), 1), ST.Buffer(parcelGeoms, 0)) ");
        str.Append(System.Environment.NewLine);
        str.Append("FROM Municipalities INNER JOIN MunicipalitiesMapLink ON MunID = PPID ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN ");
        str.Append(System.Environment.NewLine);
        str.Append("( ");
        str.Append(System.Environment.NewLine);
        str.Append("SELECT originMunicipalityID, ST.CollectAggregate(the_geom) as parcelGeoms ");
        str.Append(System.Environment.NewLine);
        str.Append("FROM ");
        str.Append(System.Environment.NewLine);
        str.Append("boundaryGroups ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN boundaryTransfers ON  ");
        str.Append(System.Environment.NewLine);
        str.Append("boundaryGroups.boundaryGroupID = boundaryTransfers.boundaryGroupID ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN ParcelToAssessment ON  originMunicipalityID = MunID AND ");
        str.Append(System.Environment.NewLine);
        str.Append("alternate_parcelID = assessmentNumber  ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN AssessmentParcels ON mapparcelID = P_ID ");
        str.Append(System.Environment.NewLine);
        str.Append("WHERE UserID = @UserID ");
        str.Append(System.Environment.NewLine);
        str.Append("GROUP BY originMunicipalityID ");
        str.Append(System.Environment.NewLine);
        str.Append(") geom ");
        str.Append(System.Environment.NewLine);
        str.Append("ON originMunicipalityID = [SAMA_Code] ");
        str.Append(System.Environment.NewLine);
        str.Append("WHERE NOT EXISTS( ");
        str.Append(System.Environment.NewLine);
        str.Append("SELECT MunID FROM MunicipalitiesChanges changes WHERE changes.UserID = @UserID AND changes.MunID = Municipalities.MunID ");
        str.Append(System.Environment.NewLine);
        str.Append(") ");
        str.Append(System.Environment.NewLine);

        str.Append(System.Environment.NewLine);
        str.Append("INSERT INTO MunicipalitiesChanges (MunID, UserID, the_geom) ");
        str.Append(System.Environment.NewLine);
        str.Append("SELECT MunID, @UserID, ST.Difference(ST.Buffer(ST.Simplify(the_geom, 10), 1), ST.Buffer(parcelGeoms, 0))  ");
        str.Append(System.Environment.NewLine);
        str.Append("FROM MunicipalitiesChanges INNER JOIN MunicipalitiesMapLink ON MunID = PPID ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN ");
        str.Append(System.Environment.NewLine);
        str.Append("( ");
        str.Append(System.Environment.NewLine);
        str.Append("SELECT originMunicipalityID, ST.CollectAggregate(the_geom) as parcelGeoms ");
        str.Append(System.Environment.NewLine);
        str.Append("FROM ");
        str.Append(System.Environment.NewLine);
        str.Append("boundaryGroups ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN boundaryTransfers ON  ");
        str.Append(System.Environment.NewLine);
        str.Append("boundaryGroups.boundaryGroupID = boundaryTransfers.boundaryGroupID ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN ParcelToAssessment ON  originMunicipalityID = MunID AND ");
        str.Append(System.Environment.NewLine);
        str.Append("alternate_parcelID = assessmentNumber  ");
        str.Append(System.Environment.NewLine);
        str.Append("INNER JOIN AssessmentParcels ON mapparcelID = P_ID ");
        str.Append(System.Environment.NewLine);
        str.Append("WHERE UserID = @UserID ");
        str.Append(System.Environment.NewLine);
        str.Append("GROUP BY originMunicipalityID ");
        str.Append(System.Environment.NewLine);
        str.Append(") geom ");
        str.Append(System.Environment.NewLine);
        str.Append("ON originMunicipalityID = [SAMA_Code] ");
        str.Append(System.Environment.NewLine);
        str.Append("WHERE UserID = @userID ");
        str.Append(System.Environment.NewLine);

        //Select data for the current user only, changes are selected from the changes table
        str.Append(System.Environment.NewLine);
        str.Append("SELECT MunID, the_geom FROM Municipalities WHERE MunID IS NOT NULL AND NOT EXISTS ");
        str.Append(System.Environment.NewLine);
        str.Append("(SELECT * FROM MunicipalitiesChanges WHERE UserID = @userID ");
        str.Append(System.Environment.NewLine);
        str.Append("AND Municipalities.MunID = MunicipalitiesChanges.MunID) ");
        str.Append(System.Environment.NewLine);
        str.Append("UNION ");
        str.Append(System.Environment.NewLine);
        str.Append("SELECT MunID, the_geom ");
        str.Append(System.Environment.NewLine);
        str.Append("FROM MunicipalitiesChanges ");
        str.Append(System.Environment.NewLine);
        str.Append("WHERE UserID = @userID ");



        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PATMAPConnection"].ConnectionString);
        SqlCommand cmd = new SqlCommand(str.ToString(), conn);
        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
        cmd.CommandTimeout = 600;
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        writeToShapefile(reader, userID);
        reader.Close();
    }

    public static void writeToShapefile(SqlDataReader reader, int userID)
    {
        MsSqlSpatialReader spatialReader = new MsSqlSpatialReader();

        List<Feature> featureList = new List<Feature>();

        while (reader.Read())
        {
            string munid = "";
            if (!reader[0].Equals(DBNull.Value))
            {
                munid = reader.GetString(0);
            }
            byte[] geomBuffer = reader.GetSqlBinary(1).Value;
            IGeometry geom = spatialReader.Read(geomBuffer);

            //GeoAPI.Geometries.IGeometry geom = spatialReader.Read(geomBuffer);
            IAttributesTable table = new AttributesTable();
            table.AddAttribute("munid", munid);
            Feature feature = new Feature(geom, table);
            featureList.Add(feature);
        }

        string exportShapefilePath = ConfigurationManager.AppSettings["WebServerShapefilePath"];
        if (!(exportShapefilePath.EndsWith("\\") || exportShapefilePath.EndsWith("/")))
        {
            exportShapefilePath += "\\";
        }
        exportShapefilePath += "Municipalities" + userID.ToString();
        ShapefileDataWriter shp = new ShapefileDataWriter(exportShapefilePath);
        shp.Header = new DbaseFileHeader();
        shp.Header.NumRecords = featureList.Count;
        shp.Header.NumFields = 1;
        shp.Header.AddColumn("munid", 'c', 50, 0);
        shp.Write(featureList);
    }
}
