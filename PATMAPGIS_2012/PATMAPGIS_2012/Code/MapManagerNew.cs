using System;
using System.Data;
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
using System.Xml.Linq;
using System.Linq;
using System.Collections.Specialized;
using System.Globalization;
using OSGeo.MapGuide;

public class MapManagerNew
{


    public static string[] statusIds = { "Taxable", "Exempt", "Provincial grant in lieu", "Federal grant in lieu" };
    public static string[] statusNames = { "Taxable", "Exempt", "Provincial grant in lieu", "Federal grant in lieu" };
    //private static string[] shiftNames = { "Municipal Tax", "School Tax", "Assessment Value", "PMR", "Levy", "Grant", "Tax Credit", "Total Impact", "Total Tax", "Phase-In Amount", "Minimum Tax", "Base Tax", }; Donna
    public static string[] shiftNames = { "Municipal Tax", "School Tax", "Assessment Value", "Levy", "Total Impact", "Total Tax", "Phase-In Amount", "Minimum Tax", "Base Tax", }; //Donna

    //public static string[,] baseColumns = { 
    //      /// VLAD inserted for test 
    //      ///{"(BasetaxableMunicipalTax + basepotash)", "", "BasePGILMunicipalTax", "BaseFGILMunicipalTax"},
    //      {"(BasetaxableMunicipalTax + basepotash)", "basepotash", "BasePGILMunicipalTax", "BaseFGILMunicipalTax"},
    //      {"BasetaxableSchoolTax", "", "BasePGILSchoolTax", "BaseFGILSchoolTax"},
    //      {"(BasetaxableMarketValue * POV.POV)", "(BaseexemptMarketValue * POV.POV)", "(BasePGILMarketValue * POV.POV)", "(BaseFGILMarketValue * POV.POV)"},
    //      {"BasetaxableProvincialSchoolTax", "", "BasePGILProvincialSchoolTax", "BaseFGILProvincialSchoolTax"},
    //      {"BasetaxableMunicipalTax", "", "BasePGILMunicipalTax", "BaseFGILMunicipalTax"},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""}
    //  };
    //modified for error resolution for boundarychange 24-sep-2013
    public static string[,] baseColumns = { 
				/// VLAD inserted for test 
        ///{"(BasetaxableMunicipalTax + basepotash)", "", "BasePGILMunicipalTax", "BaseFGILMunicipalTax"},
				{"(BasetaxableMunicipalTax + basepotash)", "basepotash", "BasePGILMunicipalTax", "BaseFGILMunicipalTax"},
        {"BasetaxableSchoolTax", "", "BasePGILSchoolTax", "BaseFGILSchoolTax"},
        {"(BasetaxableMarketValue * POV.POV)", "(BaseexemptMarketValue * POV.POV)", "(BasePGILMarketValue * POV.POV)", "(BaseFGILMarketValue * POV.POV)"},
        {"BasetaxableMunicipalTax", "", "BasePGILMunicipalTax", "BaseFGILMunicipalTax"},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""}
    };

    //public static string[,] baseColumnsLTT = { 
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"subjectStartTaxableAdValerumTax", "subjectStartExemptAdValerumTax", "subjectStartFGILAdValerumTax", "subjectStartPGILAdValerumTax"},
    //      {"(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)", "subjectStartExemptAdValerumTax", "subjectStartFGILAdValerumTax", "subjectStartPGILAdValerumTax"},
    //      {"eligiblePhaseInAmt","","",""},
    //      {"subjectTaxableMinTax", "subjectExemptMinTax", "subjectFGILMinTax", "subjectPGILMinTax"},
    //      {"subjectTaxableBaseTax","subjectExemptBaseTax","subjectFGILBaseTax","subjectPGILBaseTax"}
    //  };

    //Modified to place PGIL and FGIL columns in correct order 10-Jan-2014
    //public static string[,] baseColumnsLTT = { 
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"subjectStartTaxableAdValerumTax", "subjectStartExemptAdValerumTax", "subjectStartFGILAdValerumTax", "subjectStartPGILAdValerumTax"},
    //      {"(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)", "subjectStartExemptAdValerumTax", "subjectStartFGILAdValerumTax", "subjectStartPGILAdValerumTax"},
    //      {"eligiblePhaseInAmt","","",""},
    //      {"subjectTaxableMinTax", "subjectExemptMinTax", "subjectFGILMinTax", "subjectPGILMinTax"},
    //      {"subjectTaxableBaseTax","subjectExemptBaseTax","subjectFGILBaseTax","subjectPGILBaseTax"},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""}
    //  };

    public static string[,] baseColumnsLTT = { 
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"subjectStartTaxableAdValerumTax", "subjectStartExemptAdValerumTax", "subjectStartPGILAdValerumTax", "subjectStartFGILAdValerumTax" },
        {"(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)", "subjectStartExemptAdValerumTax", "subjectStartPGILAdValerumTax", "subjectStartFGILAdValerumTax"},
        {"eligiblePhaseInAmt","","",""},
        {"subjectTaxableMinTax", "subjectExemptMinTax", "subjectPGILMinTax", "subjectFGILMinTax"},
        {"subjectTaxableBaseTax","subjectExemptBaseTax", "subjectPGILBaseTax", "subjectFGILBaseTax"},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""}
    };

    //public static string[,] shiftColumns = { 
    //      /// VLAD inserted for test 
    //      /// {"(SubjecttaxableMunicipalTax + subjectpotash)", "", "SubjectPGILMunicipalTax", "SubjectFGILMunicipalTax"},
    //      {"(SubjecttaxableMunicipalTax + subjectpotash)", "subjectpotash", "SubjectPGILMunicipalTax", "SubjectFGILMunicipalTax"},
    //      {"SubjecttaxableSchoolTax", "", "SubjectPGILSchoolTax", "SubjectFGILSchoolTax"},
    //      {"(SubjecttaxableMarketValue * livePOV.POV)", "(SubjectexemptMarketValue * livePOV.POV)", "(SubjectPGILMarketValue * livePOV.POV)", "(SubjectFGILMarketValue * livePOV.POV)"},
    //      {"SubjecttaxableProvincialSchoolTax", "", "SubjectPGILProvincialSchoolTax", "SubjectFGILProvincialSchoolTax"},
    //      {"restructuredTaxableLevy", "", "restructuredPGILLevy", "restructuredFGILLevy"},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""}
    //  };

    //modified for error resolution for boundarychange 24-sep-2013
    public static string[,] shiftColumns = { 
				/// VLAD inserted for test 
				/// {"(SubjecttaxableMunicipalTax + subjectpotash)", "", "SubjectPGILMunicipalTax", "SubjectFGILMunicipalTax"},
        {"(SubjecttaxableMunicipalTax + subjectpotash)", "subjectpotash", "SubjectPGILMunicipalTax", "SubjectFGILMunicipalTax"},
        {"SubjecttaxableSchoolTax", "", "SubjectPGILSchoolTax", "SubjectFGILSchoolTax"},
        {"(SubjecttaxableMarketValue * livePOV.POV)", "(SubjectexemptMarketValue * livePOV.POV)", "(SubjectPGILMarketValue * livePOV.POV)", "(SubjectFGILMarketValue * livePOV.POV)"},
        {"restructuredTaxableLevy", "", "restructuredPGILLevy", "restructuredFGILLevy"},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""}
    };

    //public static string[,] shiftColumnsLTT = { 
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)", "subjectTaxableBaseTax", "subjectTaxableBaseTax", "subjectTaxableBaseTax"},
    //      {"(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)+(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)", "subjectTaxableBaseTax", "subjectTaxableBaseTax", "subjectTaxableBaseTax"},
    //      {"(eligiblePhaseInAmt+eligiblePhaseInAmt)","","",""},        
    //      {"(subjectTaxableMinTax+subjectTaxableMinTax)", "subjectExemptMinTax", "subjectFGILMinTax", "subjectPGILMinTax"},
    //      {"(subjectTaxableBaseTax+subjectTaxableBaseTax)","subjectExemptBaseTax","subjectFGILBaseTax","subjectPGILBaseTax"}
    //  };

    //Modified to place PGIL and FGIL columns in correct order 10-Jan-2014
    //public static string[,] shiftColumnsLTT = { 
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)", "subjectTaxableBaseTax", "subjectTaxableBaseTax", "subjectTaxableBaseTax"},
    //      {"(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)+(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)", "subjectTaxableBaseTax", "subjectTaxableBaseTax", "subjectTaxableBaseTax"},
    //      {"(eligiblePhaseInAmt+eligiblePhaseInAmt)","","",""},        
    //      {"(subjectTaxableMinTax+subjectTaxableMinTax)", "subjectExemptMinTax", "subjectFGILMinTax", "subjectPGILMinTax"},
    //      {"(subjectTaxableBaseTax+subjectTaxableBaseTax)","subjectExemptBaseTax","subjectFGILBaseTax","subjectPGILBaseTax"},
    //      {"", "", "", ""},
    //      {"", "", "", ""},
    //      {"", "", "", ""}
    //  };

    public static string[,] shiftColumnsLTT = { 
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""},
        {"(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)", "subjectTaxableBaseTax", "subjectTaxableBaseTax", "subjectTaxableBaseTax"},
        {"(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)+(subjectTaxableAdValerumTax + subjectTaxableBaseTax + subjectTaxableMinTax)", "subjectTaxableBaseTax", "subjectTaxableBaseTax", "subjectTaxableBaseTax"},
        {"(eligiblePhaseInAmt+eligiblePhaseInAmt)","","",""},        
        {"(subjectTaxableMinTax+subjectTaxableMinTax)", "subjectExemptMinTax", "subjectPGILMinTax", "subjectFGILMinTax" },
        {"(subjectTaxableBaseTax+subjectTaxableBaseTax)","subjectExemptBaseTax", "subjectPGILBaseTax", "subjectFGILBaseTax"},
        {"", "", "", ""},
        {"", "", "", ""},
        {"", "", "", ""}
    };


    /// <summary>
    /// Adds filters to the municipality layer
    /// </summary>
    /// <param name="style">The layer object to add the filter to</param>

    public static string addMunicipalityThemeDatasourceNew(bool isPercent)
    {
        string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
        StringBuilder str = new StringBuilder();
        //str.Append("(SELECT PPID AS [MunicipalityID], ISNULL(MAX([jurisdiction]), MAX(PATMAP_Roll_Up_Name)) AS Name, ");
        str.Append("SELECT PPID AS [MunicipalityID], ISNULL(MAX([jurisdiction]), MAX(PATMAP_Roll_Up_Name)) AS Name, ");

        if (isPercent)
        {
            str.Append(" CASE WHEN ");
            str.Append("SUM(");
            addShiftTotals(str);
            str.Append(") = 0 THEN 0 ELSE ");
        }
        str.Append(" CAST(SUM(");
        if (MapSettings.TaxShiftFilters != null)
        {
            addShiftFilters(str);
        }
        str.Append(")");
        if (isPercent)
        {
            str.Append("/SUM(");
            addShiftTotals(str);
            str.Append(") * 100");
            str.Append(" as decimal(38,5)) END");
        }
        else
        {
            str.Append(" as decimal(38,2))");
        }
        str.Append(" as value");
        str.Append(" FROM  MunicipalitiesMapLink");
        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE))
        {
            ///Work with this
            str.Append(" LEFT OUTER JOIN liveAssessmentTaxModelResultsSummary_");
        }
        else if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.USER))
        {
            ///No Work with this
            str.Append(" LEFT OUTER JOIN liveboundaryModelResultsSummary_");
        }
        else if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
        {
            str.Append(" LEFT OUTER JOIN (SELECT taxClassID, subjectMarketValue, subjectTaxableDA, subjectFGILDA, subjectPGILDA, subjectExemptDA, subjectStartTaxableAdValerumTax, subjectStartFGILAdValerumTax, subjectStartPGILAdValerumTax, subjectStartExemptAdValerumTax, subjectStartUMR, subjectTaxableAdValerumTax, subjectFGILAdValerumTax, subjectPGILAdValerumTax, subjectExemptAdValerumTax, subjectTaxableBaseTax, subjectFGILBaseTax, subjectPGILBaseTax, subjectTaxableMinTax,subjectExemptMinTax, subjectFGILMinTax, subjectPGILMinTax, subjectExemptBaseTax, subjectMillRateFactor,'");
            //str.Append(System.Web.HttpContext.Current.Session["LTTdropDownChoice"].ToString());
            str.Append(System.Web.HttpContext.Current.Session["CODE_LTTSubjectMunicipality"].ToString());
            str.Append("' as municipalityID FROM liveLTTSummary_");
        }
        //str.Append("82");
        str.Append(q_UserID);
        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
        {
            str.Append(") results ON [SAMA_Code] = '");
            //str.Append(System.Web.HttpContext.Current.Session["LTTdropDownChoice"].ToString());
            str.Append(System.Web.HttpContext.Current.Session["CODE_LTTSubjectMunicipality"].ToString());
            str.Append("'");
            if ((bool)System.Web.HttpContext.Current.Session["phaseInBaseYearAccess"])
            {
                str.Append(" INNER JOIN  liveLTTPhaseInSummary_");
                //str.Append("82");
                str.Append(q_UserID);
                str.Append(" resultsPH ON results.taxClassID = resultsPH.taxClassID");
            }
        }
        else
        {
            str.Append(" results ON [SAMA_Code] = MunicipalityID");
        }
        //str.AppendFormat(" LEFT OUTER JOIN liveAssessmentTaxModel ON liveAssessmentTaxModel.userID = 82 LEFT OUTER JOIN taxYearModelDescription ON liveAssessmentTaxModel.baseTaxYearModelID = taxYearModelDescription.taxYearModelID");
        str.AppendFormat(" LEFT OUTER JOIN liveAssessmentTaxModel ON liveAssessmentTaxModel.userID = {0} LEFT OUTER JOIN taxYearModelDescription ON liveAssessmentTaxModel.baseTaxYearModelID = taxYearModelDescription.taxYearModelID", q_UserID);
        str.Append(" LEFT OUTER JOIN POV ON taxYearModelDescription.POVID = POV.POVID AND POV.taxClassID = results.taxClassID");
        //str.AppendFormat(" LEFT OUTER JOIN livePOV ON livePOV.userID = {0} AND livePOV.taxClassID = results.taxClassID", "82");
        str.AppendFormat(" LEFT OUTER JOIN livePOV ON livePOV.userID = {0} AND livePOV.taxClassID = results.taxClassID", q_UserID);

        str.Append(" LEFT OUTER JOIN entities ON number = [SAMA_Code]");

        appendFilters(str);
        //if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
        //{
        //    str.Append("  and [SAMA_Code] = 'ESTEV'");
        //}
        str.Append(" GROUP BY PPID");



        string q_out = str.ToString();
        return q_out;
    }

    /// <summary>
    /// Adds filters to the school division layer (style)
    /// </summary>
    /// <param name="style">The style object to add the filter to</param>
    /// 




    public static string addSchoolThemeDatasourceNew(bool isPercent)
    {
        string q_UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
        StringBuilder str = new StringBuilder();
        //str.Append("SELECT SchoolID AS SchID, ISNULL(MAX(jurisdiction), '''') AS Name, ");
        str.Append("SELECT SchoolID AS SchID, ISNULL(MAX(jurisdiction), '') AS Name, ");
        if (isPercent)
        {
            str.Append(" CASE WHEN ");
            str.Append("SUM(");
            addShiftTotals(str);
            str.Append(") = 0 THEN 0 ELSE ");
        }
        str.Append("CAST(SUM(");
        if (MapSettings.TaxShiftFilters != null)
        {
            addShiftFilters(str);

        }
        str.Append(")");
        if (isPercent)
        {
            str.Append("/SUM(");
            addShiftTotals(str);
            str.Append(") * 100");
            str.Append(" as decimal(38,5)) END");
        }
        else
        {
            str.Append(" as decimal(38,2))");
        }
        str.Append(" AS value");
        str.Append(" FROM entities");
        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE))
        {
            str.Append(" LEFT OUTER JOIN liveAssessmentTaxModelResultsSummary_");
        }
        else if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.USER))
        {
            str.Append(" LEFT OUTER JOIN liveboundaryModelResultsSummary_");
        }
        //Donna - Note: this code does not work! TBD if any action is to be taken.
        else if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
        {
            str.Append(" FROM liveLTTResults_");
        }
        //str.Append("82");
        str.Append(q_UserID);
        str.Append(" results ON CONVERT(varchar(10), SchoolID) = number AND jurisdictionTypeID = 1");
        //str.AppendFormat(" LEFT OUTER JOIN liveAssessmentTaxModel ON liveAssessmentTaxModel.userID = 82 LEFT OUTER JOIN taxYearModelDescription ON liveAssessmentTaxModel.baseTaxYearModelID = taxYearModelDescription.taxYearModelID");
        str.AppendFormat(" LEFT OUTER JOIN liveAssessmentTaxModel ON liveAssessmentTaxModel.userID = {0} LEFT OUTER JOIN taxYearModelDescription ON liveAssessmentTaxModel.baseTaxYearModelID = taxYearModelDescription.taxYearModelID", q_UserID);
        str.Append(" LEFT OUTER JOIN POV ON taxYearModelDescription.POVID = POV.POVID AND POV.taxClassID = results.taxClassID");
        //str.AppendFormat(" LEFT OUTER JOIN livePOV ON livePOV.userID = 82 AND livePOV.taxClassID = results.taxClassID");
        str.AppendFormat(" LEFT OUTER JOIN livePOV ON livePOV.userID = {0} AND livePOV.taxClassID = results.taxClassID", q_UserID);
        //appendFilters(str);
        str.Append(" GROUP BY SchoolID");
        string q_out = str.ToString();
        return q_out;


    }

    /// <summary>

    /// <summary>
    /// Adds filters to school and municipal boundary layers in the form of a where clause
    /// </summary>
    /// <param name="str">A Stringbuilder to append the Where clause applying filters</param>
    public static void appendFilters(StringBuilder str)
    {
        bool isLTTRestrictedUser = false;   //Donna

        str.Append(" WHERE ");

        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
        {
        }
        else
        {
            str.Append(" CONVERT(varchar(10), SchoolID) not in (SELECT DISTINCT subschoolID FROM schoolApportionment) AND");
        }
        if (MapSettings.MapPropertyClassFilters != null && MapSettings.MapPropertyClassFilters.Count > 0)
        {
            if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE))
            {
                str.Append(" results.EfftaxClassID IN (");
            }
            //Donna start
            else if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
            {
                if ((bool)HttpContext.Current.Session["showFullTaxClasses"] == true)
                    str.Append(" results.taxClassID IN (");
                else
                {
                    str.Append(" results.taxClassID IN (SELECT taxClassID FROM LTTTaxClasses WHERE LTTMainTaxClassId IN (");
                    isLTTRestrictedUser = true;
                }
            }
            //Donna end
            else
                str.Append(" results.taxClassID IN (");

            List<string> filters = MapSettings.MapPropertyClassFilters;


            if (filters.Count > 0)
            {
                string qqq = "";
                qqq = filters.Aggregate((x, y) => x + "','" + y);
                qqq = "'" + qqq;
                str.Append(qqq.ToString());
            }
            //if (filters.Count > 0)
            //{
            //  string qqq = "";
            //  qqq = filters.Aggregate((x, y) => x + "'',''" + y);
            //  qqq = "''" + qqq;
            //  str.Append(qqq.ToString());
            //}


            //Vlad >>>
            //foreach (string filter in filters)
            //{
            //  //str.Append("'");
            //  //Vlad
            //  str.Append("''");
            //  str.Append(filter);
            //  //Vlad
            //  //str.Append("',");
            //  str.Append("'',");
            //}
            //Vlad <<<

            //Donna start - Close extra bracket.
            if (isLTTRestrictedUser)
                str.Append("')) AND ");
            else
                str.Append("') AND ");
            //Donna end
        }
        str.Append(" (");
        addShiftTotals(str);
        str.Append("+");
        addShiftedValues(str);
        str.Append(") > 0");
        //str.Append(" UserID = ");
        //str.Append("82");
    }

    public static void addShiftTotals(StringBuilder str)
    {
        List<string> shifts = MapSettings.TaxShiftFilters;

        foreach (string shift in shifts)
        {
            int indexShift = Array.IndexOf(shiftNames, shift);
            if (indexShift == -1)
            {
                //Unknown shift
                continue;
            }
            //Donna Start - Removed this.
            //else if (indexShift == 5)
            //{
            //    str.Append("SubjectK12 +");
            //}
            //else if (indexShift == 6)
            //{
            //    str.Append("BasetaxCredit +");
            //}
            //Donna End
            else
            {
                foreach (string taxStatus in MapSettings.TaxStatusFilters)
                {
                    int indexStatus = Array.IndexOf(statusIds, taxStatus);
                    if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
                    {
                        if (indexStatus != -1)
                        {
                            if ((bool)System.Web.HttpContext.Current.Session["phaseInBaseYearAccess"])
                            {
                                str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                str.Append("+");
                            }
                            else
                            {
                                //change column no from 9 to 6
                                if (indexShift != 6)    //Donna - Fixed bug. Was previously checking indexStatus variable.
                                {
                                    str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                    str.Append("+");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (indexStatus != -1)
                        {
                            str.Append(baseColumns[indexShift, indexStatus]);
                            str.Append("+");
                        }
                    }
                }
            }
        }
        str.Append("0");
    }

    /// <summary>
    /// Adds the shift filters.
    /// </summary>
    /// <param name="str">The STR.</param>
    public static void addShiftFilters(StringBuilder str)
    {
        List<string> shifts = MapSettings.TaxShiftFilters;

        foreach (string shift in shifts)
        {
            int indexShift = Array.IndexOf(shiftNames, shift);
            if (indexShift == -1)
            {
                //Unknown shift
                continue;
            }
            //Donna Start - Removed this.
            //else if (indexShift == 5)
            //{
            //    str.Append("BaseK12 - SubjectK12 +");
            //}
            //else if (indexShift == 6)
            //{
            //    str.Append("SubjecttaxCredit - BasetaxCredit +");
            //}
            //Donna End
            else
            {
                foreach (string taxStatus in MapSettings.TaxStatusFilters)
                {
                    int indexStatus = Array.IndexOf(statusIds, taxStatus);
                    if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
                    {
                        if (indexStatus != -1)
                        {
                            if ((bool)System.Web.HttpContext.Current.Session["phaseInBaseYearAccess"])
                            {
                                str.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                str.Append("-");
                                str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                str.Append("+");
                            }
                            else
                            {
                                //change column no from 9 to 6
                                if (indexShift != 6)    //Donna - Fixed bug. Was previously checking indexStatus variable.
                                {
                                    str.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                    str.Append("-");
                                    str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                    str.Append("+");
                                }
                            }
                        }

                    }
                    else
                    {
                        if (indexStatus != -1)
                        {
                            str.Append(shiftColumns[indexShift, indexStatus]);
                            str.Append("-");
                            str.Append(baseColumns[indexShift, indexStatus]);
                            str.Append("+");
                        }
                    }
                }
            }
        }
        str.Append("0");
    }

    public static void addShiftedValues(StringBuilder str)
    {
        List<string> shifts = MapSettings.TaxShiftFilters;

        foreach (string shift in shifts)
        {
            int indexShift = Array.IndexOf(shiftNames, shift);
            if (indexShift == -1)
            {
                //Unknown shift
                continue;
            }
            //Donna Start - Removed this.
            //else if (indexShift == 5)
            //{
            //    str.Append("BaseK12 +");
            //}
            //else if (indexShift == 6)
            //{
            //    str.Append("SubjecttaxCredit +");
            //}
            //Donna End
            else
            {
                foreach (string taxStatus in MapSettings.TaxStatusFilters)
                {
                    int indexStatus = Array.IndexOf(statusIds, taxStatus);
                    if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
                    {
                        if (indexStatus != -1)
                        {
                            if ((bool)System.Web.HttpContext.Current.Session["phaseInBaseYearAccess"])
                            {
                                str.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                str.Append("+");
                            }
                            else
                            {
                                //change column no from 9 to 6
                                if (indexShift != 6)    //Donna - Fixed bug. Was previously checking indexStatus variable.
                                {
                                    str.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                    str.Append("+");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (indexStatus != -1)
                        {
                            str.Append(shiftColumns[indexShift, indexStatus]);
                            str.Append("+");
                        }
                    }
                }
            }
        }
        str.Append("0");
    }

    /// <summary>
    /// Converts a MapGuide 6.5 colour index into the corresponding Web colour
    /// </summary>
    /// <param name="index">A MapGuide 6.5 colour index (1-256)</param>
    /// <returns>A web colour</returns>
    public static string convertToColour(int index)
    {
        switch (index)
        {
            case 1: return "#FFFFFF";
            case 2: return "#C0C0C0";
            case 3: return "#808080";
            case 4: return "#000000";
            case 5: return "#FF0000";
            case 6: return "#FFFF00";
            case 7: return "#00FF00";
            case 8: return "#00FFFF";
            case 9: return "#0000FF";
            case 10: return "#FF00FF";
            case 11: return "#800000";
            case 12: return "#808000";
            case 13: return "#008000";
            case 14: return "#008080";
            case 15: return "#000080";
            case 16: return "#800080";
            case 17: return "#500000";
            case 18: return "#700000";
            case 19: return "#900000";
            case 20: return "#C00000";
            case 21: return "#E00000";
            case 22: return "#FF0000";
            case 23: return "#FF2020";
            case 24: return "#FF4040";
            case 25: return "#FF5050";
            case 26: return "#FF6060";
            case 27: return "#FF8080";
            case 28: return "#FF9090";
            case 29: return "#FFA0A0";
            case 30: return "#FFB0B0";
            case 31: return "#FFD0D0";
            case 32: return "#501400";
            case 33: return "#701C00";
            case 34: return "#902400";
            case 35: return "#A02800";
            case 36: return "#C03000";
            case 37: return "#E03800";
            case 38: return "#FF4000";
            case 39: return "#FF5820";
            case 40: return "#FF7040";
            case 41: return "#FF7C50";
            case 42: return "#FF9470";
            case 43: return "#FFA080";
            case 44: return "#FFB8A0";
            case 45: return "#FFC4B0";
            case 46: return "#FFDCD0";
            case 47: return "#502800";
            case 48: return "#603000";
            case 49: return "#804000";
            case 50: return "#A05000";
            case 51: return "#B05800";
            case 52: return "#D06800";
            case 53: return "#E07000";
            case 54: return "#FF8000";
            case 55: return "#FF9830";
            case 56: return "#FFA850";
            case 57: return "#FFB060";
            case 58: return "#FFC080";
            case 59: return "#FFD0A0";
            case 60: return "#FFD8B0";
            case 61: return "#FFE8D0";
            case 62: return "#503C00";
            case 63: return "#604800";
            case 64: return "#806000";
            case 65: return "#906C00";
            case 66: return "#A07800";
            case 67: return "#C09000";
            case 68: return "#D09C00";
            case 69: return "#F0B400";
            case 70: return "#FFC000";
            case 71: return "#FFD040";
            case 72: return "#FFD860";
            case 73: return "#FFDC70";
            case 74: return "#FFE490";
            case 75: return "#FFECB0";
            case 76: return "#FFF4D0";
            case 77: return "#505000";
            case 78: return "#606000";
            case 79: return "#707000";
            case 80: return "#909000";
            case 81: return "#A0A000";
            case 82: return "#B0B000";
            case 83: return "#C0C000";
            case 84: return "#F4F400";
            case 85: return "#F0F000";
            case 86: return "#FFFF00";
            case 87: return "#FFFF40";
            case 88: return "#FFFF70";
            case 89: return "#FFFF90";
            case 90: return "#FFFFB0";
            case 91: return "#FFFFD0";
            case 92: return "#305000";
            case 93: return "#3A6000";
            case 94: return "#4D8000";
            case 95: return "#569000";
            case 96: return "#60A000";
            case 97: return "#73C000";
            case 98: return "#7DD000";
            case 99: return "#90F000";
            case 100: return "#9AFF00";
            case 101: return "#B3FF40";
            case 102: return "#C0FF60";
            case 103: return "#CDFF80";
            case 104: return "#D3FF90";
            case 105: return "#E0FFB0";
            case 106: return "#EDFFD0";
            case 107: return "#005000";
            case 108: return "#006000";
            case 109: return "#008000";
            case 110: return "#00A000";
            case 111: return "#00B000";
            case 112: return "#00D000";
            case 113: return "#00E000";
            case 114: return "#00FF00";
            case 115: return "#50FF50";
            case 116: return "#60FF60";
            case 117: return "#70FF70";
            case 118: return "#90FF90";
            case 119: return "#A0FFA0";
            case 120: return "#B0FFB0";
            case 121: return "#D0FFD0";
            case 122: return "#005028";
            case 123: return "#006030";
            case 124: return "#008040";
            case 125: return "#00A050";
            case 126: return "#00B058";
            case 127: return "#00D068";

            case 128: return "#00F470";
            case 129: return "#00FF80";
            case 130: return "#50FFA8";
            case 131: return "#60FFB0";
            case 132: return "#70FFB8";
            case 133: return "#90FFC8";
            case 134: return "#A0FFD0";
            case 135: return "#B0FFD8";
            case 136: return "#D0FFE8";
            case 137: return "#005050";
            case 138: return "#006060";
            case 139: return "#008080";
            case 140: return "#009090";
            case 141: return "#00A0A0";
            case 142: return "#00C0C0";
            case 143: return "#00D0D0";
            case 144: return "#00F0F0";
            case 145: return "#00FFFF";
            case 146: return "#50FFFF";
            case 147: return "#70FFFF";
            case 148: return "#80FFFF";
            case 149: return "#A0FFFF";
            case 150: return "#B0FFFF";
            case 151: return "#D0FFFF";
            case 152: return "#003550";
            case 153: return "#004B70";
            case 154: return "#006090";
            case 155: return "#006BA0";
            case 156: return "#0080C0";
            case 157: return "#0095E0";
            case 158: return "#00ABFF";
            case 159: return "#40C0FF";
            case 160: return "#50C5FF";
            case 161: return "#60CBFF";
            case 162: return "#80D5FF";
            case 163: return "#90DBFF";
            case 164: return "#A0E0FF";
            case 165: return "#B0E5FF";
            case 166: return "#D0F0FF";
            case 167: return "#001B50";
            case 168: return "#002570";
            case 169: return "#001C90";
            case 170: return "#0040C0";
            case 171: return "#004BE0";
            case 172: return "#0055FF";
            case 173: return "#3075FF";
            case 174: return "#4080FF";
            case 175: return "#508BFF";
            case 176: return "#70A0FF";
            case 177: return "#80ABFF";
            case 178: return "#90B5FF";
            case 179: return "#A0C0FF";
            case 180: return "#C0D5FF";
            case 181: return "#D0E0FF";
            case 182: return "#000050";
            case 183: return "#000080";
            case 184: return "#0000A0";
            case 185: return "#0000D0";
            case 186: return "#0000FF";
            case 187: return "#2020FF";
            case 188: return "#1C1CFF";
            case 189: return "#5050FF";
            case 190: return "#6060FF";
            case 191: return "#7070FF";
            case 192: return "#8080FF";
            case 193: return "#9090FF";
            case 194: return "#A0A0FF";
            case 195: return "#C0C0FF";
            case 196: return "#D0D0FF";
            case 197: return "#280050";
            case 198: return "#380070";
            case 199: return "#480090";
            case 200: return "#6000C0";
            case 201: return "#7000E0";
            case 202: return "#8000FF";
            case 203: return "#9020FF";
            case 204: return "#A040FF";
            case 205: return "#A850FF";
            case 206: return "#B060FF";
            case 207: return "#C080FF";
            case 208: return "#C890FF";
            case 209: return "#D0A0FF";
            case 210: return "#D8B0FF";
            case 211: return "#E8D0FF";
            case 212: return "#500050";
            case 213: return "#700070";
            case 214: return "#900090";
            case 215: return "#A000A0";
            case 216: return "#C000C0";
            case 217: return "#E000E0";
            case 218: return "#FF00FF";
            case 219: return "#FF20FF";
            case 220: return "#FF40FF";
            case 221: return "#FF50FF";
            case 222: return "#FF70FF";
            case 223: return "#FF80FF";
            case 224: return "#FFA0FF";
            case 225: return "#FFB0FF";
            case 226: return "#FFD0FF";
            case 227: return "#500028";
            case 228: return "#700060";
            case 229: return "#900048";
            case 230: return "#C00060";
            case 231: return "#E00070";
            case 232: return "#FF008A";
            case 233: return "#FF2090";
            case 234: return "#FF40A0";
            case 235: return "#FF50A8";
            case 236: return "#FF60B0";
            case 237: return "#FF80C0";
            case 238: return "#FF90C8";
            case 239: return "#FFA0D0";
            case 240: return "#FFB0D8";
            case 241: return "#FFD0E8";
            case 242: return "#000000";
            case 243: return "#101010";
            case 244: return "#202020";
            case 245: return "#303030";
            case 246: return "#404040";
            case 247: return "#505050";
            case 248: return "#606060";
            case 249: return "#707070";
            case 250: return "#909090";
            case 251: return "#A0A0A0";
            case 252: return "#B0B0B0";
            case 253: return "#C0C0C0";
            case 254: return "#D0D0D0";
            case 255: return "#E0E0E0";
            case 256: return "#F0F0F0";
            default: return "";
        }

    }

    /// P A R C E L S >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    //Adds filters (SQL Where clause) to the parcel layer
    ///private static void filterParcelLayer(IMapLayer layer, bool isPercent)
    ///XXX

    public static string filterParcelLayer(bool isPercent)
    {
        //Vlad check
        //layer.DataSource.NameColumn = "mouseover";
        StringBuilder str = new StringBuilder();
        bool isLTTRestrictedUser = false;   //Donna

        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE))
        {
            //str.Append("(SELECT MapParcelID, CASE WHEN COUNT( DISTINCT results.EfftaxClassID ) > 1 THEN CAST(COUNT( DISTINCT results.EfftaxClassID) as varchar(3)) ELSE MAX(results.EfftaxClassID) END taxClassID, ");
            str.Append("SELECT MapParcelID, CASE WHEN COUNT( DISTINCT results.EfftaxClassID ) > 1 THEN CAST(COUNT( DISTINCT results.EfftaxClassID) as varchar(3)) ELSE MAX(results.EfftaxClassID) END taxClassID, ");
        }
        else
        {
            //str.Append("(SELECT MapParcelID, CASE WHEN COUNT( DISTINCT results.taxClassID ) > 1 THEN CAST(COUNT( DISTINCT results.taxClassID) as varchar(3)) ELSE MAX(results.taxClassID) END taxClassID, ");
            str.Append("SELECT MapParcelID, CASE WHEN COUNT( DISTINCT results.taxClassID ) > 1 THEN CAST(COUNT( DISTINCT results.taxClassID) as varchar(3)) ELSE MAX(results.taxClassID) END taxClassID, ");
        }

        str.Append("(");
        str.Append("'Assessment #: ' + CASE WHEN COUNT( DISTINCT results.alternate_parcelID) = 1 THEN CAST(MAX(results.alternate_parcelID) as varchar(50)) ELSE 'Multiple Numbers' END + '\\n' + ");
        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE))
        {
            str.Append("CASE WHEN COUNT( DISTINCT results.EfftaxClassID ) > 2 THEN CAST(COUNT( DISTINCT results.EfftaxClassID) as varchar(3)) + ' tax classes' WHEN COUNT( DISTINCT results.EfftaxClassID ) = 2 THEN 'Tax Classes: ' +  MIN(results.EfftaxClassID) + ', ' + MAX(results.EfftaxClassID) ELSE 'Tax Class: ' + MAX(results.EfftaxClassID) END");
        }
        else
        {
            str.Append("CASE WHEN COUNT( DISTINCT results.taxClassID ) > 2 THEN CAST(COUNT( DISTINCT results.taxClassID) as varchar(3)) + ' tax classes' WHEN COUNT( DISTINCT results.taxClassID ) = 2 THEN 'Tax Classes: ' +  MIN(results.taxClassID) + ', ' + MAX(results.taxClassID) ELSE 'Tax Class: ' + MAX(results.taxClassID) END");
        }

        //str.Append(" + '\n---Shifts---'");
        str.Append(" + '\\n---Shifts---'");

        AddParcelMouseover(isPercent, str);
        str.Append(") mouseover,");
        AddParcelValues(isPercent, str);
        str.Append(" value");
        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE))
        {
            str.Append(" FROM liveAssessmentTaxModelResults_");
        }
        else if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.USER))
        {
            str.Append(" FROM liveboundaryModelResults_");
        }
        else if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
        {
            str.Append(" FROM liveLTTResults_");
        }



        str.Append(System.Web.HttpContext.Current.Session["UserID"].ToString());
        str.Append(" results INNER JOIN ParcelToAssessment ON assessmentNumber = CAST(results.alternate_parcelID as varchar(50)) ");
        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
        {
            if ((bool)System.Web.HttpContext.Current.Session["phaseInBaseYearAccess"])
            {
                str.Append(" INNER JOIN  liveLTTPhaseIn_");
                str.Append(System.Web.HttpContext.Current.Session["UserID"].ToString());
                str.Append(" resultsPH ON results.alternate_parcelID = resultsPH.alternate_parcelID AND results.taxClassID = resultsPH.taxClassID");
            }
        }
        else
        {
            str.AppendFormat(" INNER JOIN liveAssessmentTaxModel ON liveAssessmentTaxModel.userID = {0} INNER JOIN taxYearModelDescription ON liveAssessmentTaxModel.baseTaxYearModelID = taxYearModelDescription.taxYearModelID", System.Web.HttpContext.Current.Session["UserID"].ToString());
            str.Append(" INNER JOIN POV ON taxYearModelDescription.POVID = POV.POVID AND POV.taxClassID = results.taxClassID");
            str.AppendFormat(" INNER JOIN livePOV ON livePOV.userID = {0} AND livePOV.taxClassID = results.taxClassID", System.Web.HttpContext.Current.Session["UserID"].ToString());
        }
        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE))
        {
            str.Append(" AND municipalityID_orig = munID");
        }
        else if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.USER))
        {
            str.Append(" AND origMunicipalityID = munID");
        }
        str.Append(" WHERE ");
        if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
        {
        }
        else
        {
            str.Append(" CONVERT(varchar(10), SchoolID) not in (SELECT DISTINCT subschoolID FROM schoolApportionment) AND ");
        }

        if (MapSettings.MapPropertyClassFilters != null)
        {
            if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE))
            {
                str.Append("results.EfftaxClassID IN (");
                //Donna start - Moved this code further down.
                //List<string> filters = MapSettings.MapPropertyClassFilters;
                //foreach (string filter in filters)
                //{
                //    str.Append("'");
                //    str.Append(filter);
                //    str.Append("',");
                //}
                //str.Append("'') AND");
                //Donna end
            }
            //Donna start
            else if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
            {
                if ((bool)HttpContext.Current.Session["showFullTaxClasses"] == true)
                    str.Append("results.taxClassID IN (");
                else
                {
                    str.Append("results.taxClassID IN (SELECT taxClassID FROM LTTTaxClasses WHERE LTTMainTaxClassId IN (");
                    isLTTRestrictedUser = true;
                }
            }
            //Donna end
            else
            {
                str.Append("results.taxClassID IN (");
                //Donna start - Removed this.
                //List<string> filters = MapSettings.MapPropertyClassFilters;
                //foreach (string filter in filters)
                //{
                //    str.Append("'");
                //    str.Append(filter);
                //    str.Append("',");
                //}
                //str.Append("'') AND");
                //Donna end
            }

            //Donna start
            List<string> filters = MapSettings.MapPropertyClassFilters;

            ///Vlad changed
            if (filters.Count > 0)
            {
                string qqq = "";
                qqq = filters.Aggregate((x, y) => x + "','" + y);
                qqq = "'" + qqq;
                str.Append(qqq.ToString());
            }

            // Vlad check

            //if (filters.Count > 0)
            //{
            //  string qqq = "";
            //  qqq = filters.Aggregate((x, y) => x + "'',''" + y);
            //  qqq = "''" + qqq;
            //  str.Append(qqq.ToString());
            //}

            //foreach (string filter in filters)
            //{
            //  str.Append("'");
            //  str.Append(filter);
            //  str.Append("',");
            //}

            //Close extra bracket.
            if (isLTTRestrictedUser)
                str.Append("')) AND ");
            else
                str.Append("') AND");

            //if (isLTTRestrictedUser)
            //  str.Append("'')) AND ");
            //else
            //  str.Append("'') AND");
            //Donna end
        }
        str.Append(" (");
        addShiftTotals(str);
        str.Append("+");
        addShiftedValues(str);
        str.Append(") > 0");

        //modified on 12-sep-2013
        //if ( BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT ))
        //modified to apply only assessment map this is not present in old mapmanager class ????
        // not required 23-oct-2013
        //if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE))
        //{
        //    str.Append(" AND ((SubjecttaxableMunicipalTax + subjectpotash)-(BasetaxableMunicipalTax + basepotash) + (SubjecttaxableSchoolTax-BasetaxableSchoolTax))>0 and ((BasetaxableMunicipalTax + basepotash) + BasetaxableSchoolTax)>0");
        //}

        //str.Append(" UserID = ");
        //str.Append(System.Web.HttpContext.Current.Session["UserID"].ToString());

        str.Append(" GROUP BY MapParcelID");

        //Vlad
        //layer.LayerStyles[0].ThemeDataSource.Table = str.ToString();
        //layer.SecondaryDataSource.Table = str.ToString();

        //layer.DataSource.SQLWhereClause = "MapParcelID IS NOT NULL";
        string q_str = str.ToString();
        return q_str;

    }

    private static void AddParcelMouseover(bool isPercent, StringBuilder str)
    {
        if (isPercent)
        {
            List<string> shifts = MapSettings.TaxShiftFilters;
            foreach (string shift in shifts)
            {
                int indexShift = Array.IndexOf(shiftNames, shift);
                if (indexShift == -1)
                {
                    //Unknown shift
                    continue;
                }
                //Donna Start - Removed this.
                //else if (indexShift == 5)
                //{
                //    str.Append(" + '\n K-12 Grant Tax: ' + CAST(CAST(CASE WHEN SUM(SubjectK12) = 0 THEN 0 ELSE SUM(BaseK12 - SubjectK12) / SUM(SubjectK12) END * 100 as decimal(38, 2)) as varchar(44)) + '%'");
                //}
                //else if (indexShift == 6)
                //{
                //    str.Append(" + '\n Tax Credit: ' + CAST(CAST(CASE WHEN S(BasetaxCredit) = 0 THEN 0 ELSE SUM(SubjecttaxCredit - BasetaxCredit) / SUM(BasetaxCredit) END * 100 as decimal(38, 2)) as varchar(44)) + '%'");
                //}
                //Donna End
                else
                {
                    foreach (string taxStatus in MapSettings.TaxStatusFilters)
                    {
                        int indexStatus = Array.IndexOf(statusIds, taxStatus);
                        if (indexStatus != -1)
                        {

                            if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
                            {
                                if (baseColumnsLTT[indexShift, indexStatus].Length > 0)
                                {
                                    if ((bool)System.Web.HttpContext.Current.Session["phaseInBaseYearAccess"])  //Donna
                                    {
                                        str.Append(" + '\\n ");
                                        str.Append(statusNames[indexStatus]);
                                        str.Append(" ");
                                        str.Append(shift);
                                        str.Append(": ' + CAST(CASE WHEN SUM(");
                                        str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                        str.Append(") <> 0 THEN CAST(CAST(SUM(");
                                        str.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                        str.Append("-");
                                        str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                        str.Append(") / SUM(");
                                        str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                        str.Append(") * 100 as decimal(38, 2)) as varchar) ELSE '--' END as varchar(44)) + '%'");
                                    }
                                    // Donna start - Don't include PhaseIn column if user does not see PhaseIn info.
                                    else
                                    {
                                        //change column no from 9 to 6
                                        if (indexShift != 6)
                                        {
                                            str.Append(" + '\\n ");
                                            str.Append(statusNames[indexStatus]);
                                            str.Append(" ");
                                            str.Append(shift);
                                            str.Append(": ' + CAST(CASE WHEN SUM(");
                                            str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                            str.Append(") <> 0 THEN CAST(CAST(SUM(");
                                            str.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                            str.Append("-");
                                            str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                            str.Append(") / SUM(");
                                            str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                            str.Append(") * 100 as decimal(38, 2)) as varchar) ELSE '--' END as varchar(44)) + '%'");
                                        }
                                    }
                                    //Donna end
                                }
                            }
                            else
                            {
                                if (baseColumns[indexShift, indexStatus].Length > 0)
                                {
                                    str.Append(" + '\\n ");
                                    str.Append(statusNames[indexStatus]);
                                    str.Append(" ");
                                    str.Append(shift);
                                    str.Append(": ' + CAST(CASE WHEN SUM(");
                                    str.Append(baseColumns[indexShift, indexStatus]);
                                    str.Append(") <> 0 THEN CAST(CAST(SUM(");
                                    str.Append(shiftColumns[indexShift, indexStatus]);
                                    str.Append("-");
                                    str.Append(baseColumns[indexShift, indexStatus]);
                                    str.Append(") / SUM(");
                                    str.Append(baseColumns[indexShift, indexStatus]);
                                    str.Append(") * 100 as decimal(38, 2)) as varchar) ELSE '--' END as varchar(44)) + '%'");
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            List<string> shifts = MapSettings.TaxShiftFilters;
            foreach (string shift in shifts)
            {
                int indexShift = Array.IndexOf(shiftNames, shift);
                if (indexShift == -1)
                {
                    //Unknown shift
                    continue;
                }
                //Donna Start - Removed this.
                //else if (indexShift == 5)
                //{
                //    str.Append(" + '\n K-12 Grant Tax: $' + CAST(CAST(SUM(BaseK12 - SubjectK12) as decimal(38, 2)) as varchar(44))");
                //}
                //else if (indexShift == 6)
                //{
                //    str.Append(" + '\n Tax Credit: $' + CAST(CAST(SUM(SubjecttaxCredit - BasetaxCredit) as decimal(38, 2)) as varchar(44))");
                //}
                //Donna End
                else
                {
                    foreach (string taxStatus in MapSettings.TaxStatusFilters)
                    {
                        int indexStatus = Array.IndexOf(statusIds, taxStatus);
                        if (indexStatus != -1)
                        {
                            if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
                            {
                                if (baseColumnsLTT[indexShift, indexStatus].Length > 0)
                                {
                                    if ((bool)System.Web.HttpContext.Current.Session["phaseInBaseYearAccess"])  //Donna
                                    {
                                        str.Append(" + '\\n ");
                                        str.Append(statusNames[indexStatus]);
                                        str.Append(" ");
                                        str.Append(shift);
                                        str.Append(": ' + CASE WHEN SUM(");
                                        str.AppendFormat("{0}+{1}", baseColumnsLTT[indexShift, indexStatus], shiftColumnsLTT[indexShift, indexStatus]);
                                        str.Append(") <> 0 THEN ': $' + CAST(CAST(SUM(");
                                        //str.Append(": $' + CAST(CAST(SUM(");
                                        str.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                        str.Append("-");
                                        str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                        str.Append(") as decimal(38, 2)) as varchar(44)) ELSE '--' END ");
                                    }
                                    // Donna start - Don't include PhaseIn column if user does not see PhaseIn info.
                                    else
                                    {
                                        //change column no from 9 to 6
                                        if (indexShift != 6)
                                        {
                                            str.Append(" + '\\n ");
                                            str.Append(statusNames[indexStatus]);
                                            str.Append(" ");
                                            str.Append(shift);
                                            str.Append(": ' + CASE WHEN SUM(");
                                            str.AppendFormat("{0}+{1}", baseColumnsLTT[indexShift, indexStatus], shiftColumnsLTT[indexShift, indexStatus]);
                                            str.Append(") <> 0 THEN ': $' + CAST(CAST(SUM(");
                                            str.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                            str.Append("-");
                                            str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                            str.Append(") as decimal(38, 2)) as varchar(44)) ELSE '--' END ");
                                        }
                                    }
                                    //Donna end
                                }
                            }
                            else
                            {
                                if (baseColumns[indexShift, indexStatus].Length > 0)
                                {
                                    str.Append(" + '\\n ");
                                    str.Append(statusNames[indexStatus]);
                                    str.Append(" ");
                                    str.Append(shift);
                                    str.Append(": ' + CASE WHEN SUM(");
                                    str.AppendFormat("{0}+{1}", baseColumns[indexShift, indexStatus], shiftColumns[indexShift, indexStatus]);
                                    str.Append(") <> 0 THEN ': $' + CAST(CAST(SUM(");
                                    //str.Append(": $' + CAST(CAST(SUM(");
                                    str.Append(shiftColumns[indexShift, indexStatus]);
                                    str.Append("-");
                                    str.Append(baseColumns[indexShift, indexStatus]);
                                    str.Append(") as decimal(38, 2)) as varchar(44)) ELSE '--' END ");
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    public static void AddParcelValues(bool isPercent, StringBuilder str)
    {
        if (isPercent)
        {
            //as decimal(38, 2)) as varchar) ELSE '--' END as varchar(44))

            StringBuilder strBase = new StringBuilder("0");
            StringBuilder strTotal = new StringBuilder("0");

            List<string> shifts = MapSettings.TaxShiftFilters;
            foreach (string shift in shifts)
            {
                int indexShift = Array.IndexOf(shiftNames, shift);
                if (indexShift == -1)
                {
                    //Unknown shift
                    continue;
                }
                //Donna Start - Removed this.
                //else if (indexShift == 5)
                //{
                //    strBase.Append(" + SubjectK12 ");
                //    strTotal.Append(" + BaseK12 - SubjectK12 ");
                //}
                //else if (indexShift == 6)
                //{
                //    strBase.Append(" + BasetaxCredit");
                //    strTotal.Append(" + SubjecttaxCredit - BasetaxCredit ");
                //}
                //Donna End
                else
                {
                    foreach (string taxStatus in MapSettings.TaxStatusFilters)
                    {
                        int indexStatus = Array.IndexOf(statusIds, taxStatus);
                        if (indexStatus != -1)
                        {
                            if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
                            {
                                if (baseColumnsLTT[indexShift, indexStatus].Length > 0)
                                {
                                    if ((bool)System.Web.HttpContext.Current.Session["phaseInBaseYearAccess"])  //Donna
                                    {
                                        strBase.Append(" + ");
                                        strBase.Append(baseColumnsLTT[indexShift, indexStatus]);
                                        strTotal.Append(" + ");
                                        strTotal.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                        strTotal.Append("-");
                                        strTotal.Append(baseColumnsLTT[indexShift, indexStatus]);
                                    }
                                    // Donna start - Don't include PhaseIn column if user does not see PhaseIn info.
                                    else
                                    {
                                        //change column no from 9 to 6
                                        if (indexShift != 6)
                                        {
                                            strBase.Append(" + ");
                                            strBase.Append(baseColumnsLTT[indexShift, indexStatus]);
                                            strTotal.Append(" + ");
                                            strTotal.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                            strTotal.Append("-");
                                            strTotal.Append(baseColumnsLTT[indexShift, indexStatus]);
                                        }
                                    }
                                    //Donna end
                                }
                            }
                            else
                            {
                                if (baseColumns[indexShift, indexStatus].Length > 0)
                                {
                                    strBase.Append(" + ");
                                    strBase.Append(baseColumns[indexShift, indexStatus]);

                                    strTotal.Append(" + ");
                                    strTotal.Append(shiftColumns[indexShift, indexStatus]);
                                    strTotal.Append("-");
                                    strTotal.Append(baseColumns[indexShift, indexStatus]);
                                }
                            }
                        }
                    }
                }
            }

            //str.Append(" CASE WHEN SUM(");
            //str.Append(strBase);
            //str.Append(") <> 0 THEN SUM(");
            //str.Append(strTotal);
            //str.Append(") / SUM(");
            //str.Append(strBase);
            //str.Append(") * 100 ELSE '--' END ");

            str.Append(" CASE WHEN SUM(");
            str.Append(strBase);
            str.Append(") <> 0 THEN SUM(");
            str.Append(strTotal);
            str.Append(") / SUM(");
            str.Append(strBase);
            str.Append(") * 100 ELSE 0 END ");

        }
        else
        {
            str.Append("0");
            List<string> shifts = MapSettings.TaxShiftFilters;
            foreach (string shift in shifts)
            {
                int indexShift = Array.IndexOf(shiftNames, shift);
                if (indexShift == -1)
                {
                    //Unknown shift
                    continue;
                }
                //Donna Start - Removed this.
                //else if (indexShift == 5)
                //{
                //    str.Append(" + SUM(BaseK12 - SubjectK12)");
                //}
                //else if (indexShift == 6)
                //{
                //    str.Append(" + SUM(SubjecttaxCredit - BasetaxCredit)");
                //}
                //Donna End
                else
                {
                    foreach (string taxStatus in MapSettings.TaxStatusFilters)
                    {
                        int indexStatus = Array.IndexOf(statusIds, taxStatus);
                        if (indexStatus != -1)
                        {
                            if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
                            {
                                if (baseColumnsLTT[indexShift, indexStatus].Length > 0)
                                {
                                    if ((bool)System.Web.HttpContext.Current.Session["phaseInBaseYearAccess"])  //Donna
                                    {
                                        str.Append(" + SUM(");
                                        str.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                        str.Append("-");
                                        str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                        str.Append(")");
                                    }
                                    // Donna start - Don't include PhaseIn column if user does not see PhaseIn info.
                                    else
                                    {
                                        //change column no from 9 to 6
                                        if (indexShift != 6)
                                        {
                                            str.Append(" + SUM(");
                                            str.Append(shiftColumnsLTT[indexShift, indexStatus]);
                                            str.Append("-");
                                            str.Append(baseColumnsLTT[indexShift, indexStatus]);
                                            str.Append(")");
                                        }
                                    }
                                    //Donna end
                                }
                            }
                            else
                            {
                                if (baseColumns[indexShift, indexStatus].Length > 0)
                                {
                                    str.Append(" + SUM(");
                                    str.Append(shiftColumns[indexShift, indexStatus]);
                                    str.Append("-");
                                    str.Append(baseColumns[indexShift, indexStatus]);
                                    str.Append(")");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public static string BA_addMunicipalityThemeDatasourceNew(bool isPercent)
    {
        int UserID = (int)System.Web.HttpContext.Current.Session["UserID"];
        StringBuilder str = new StringBuilder();
        str.Append("SELECT PPID AS [MunicipalityID], ISNULL(MAX([jurisdiction]), MAX(PATMAP_Roll_Up_Name)) AS Name, 0 as value");
        str.Append(" FROM MunicipalitiesMapLink left outer join entities ON number = [SAMA_Code]");
        str.Append(" GROUP BY PPID");
        string q_str = str.ToString();
        return q_str;
    }

    public static string filterBASourceParcelLayer(bool isPercent)
    {
        int UserID = (int)System.Web.HttpContext.Current.Session["UserID"];
        StringBuilder str = new StringBuilder();
        str.Append("SELECT MapParcelID, AssessmentNumber, MunID FROM ParcelToAssessment");
        str.Append(" WHERE NOT EXISTS(SELECT * FROM boundaryGroups INNER JOIN boundaryTransfers ON boundaryGroups.boundaryGroupID = boundaryTransfers.boundaryGroupID WHERE boundaryTransfers.alternate_parcelID = assessmentNumber AND boundaryGroups.OriginMunicipalityID = munid AND boundaryGroups.UserID = " + UserID.ToString() + ")");
        str.Append(" AND MunID = '" + BoundaryChangeSettings.Source + "'");
        string q_str = str.ToString();
        return q_str;
    }

    public static string filterBADestinationParcelLayer(bool isPercent)
    {
        int UserID = (int)System.Web.HttpContext.Current.Session["UserID"];
        StringBuilder str = new StringBuilder();
        str.Append("SELECT MapParcelID, AssessmentNumber, MunID FROM ParcelToAssessment");
        str.Append(" WHERE NOT EXISTS(SELECT * FROM boundaryGroups INNER JOIN boundaryTransfers ON boundaryGroups.boundaryGroupID = boundaryTransfers.boundaryGroupID WHERE boundaryTransfers.alternate_parcelID = assessmentNumber AND boundaryGroups.OriginMunicipalityID = munid AND boundaryGroups.UserID = " + UserID.ToString() + ")");
        str.Append(" AND MunID = '" + BoundaryChangeSettings.Destination + "'");
        string q_str = str.ToString();
        return q_str;
    }

    public static string filterTESTParcelLayer(bool isPercent)
    {
        int UserID = (int)System.Web.HttpContext.Current.Session["UserID"];
        StringBuilder str = new StringBuilder();
        str.Append("SELECT * FROM test_data");
        string q_str = str.ToString();
        return q_str;
    }

    //Original
    public static void getAnalysisMap(Boolean IsRequiredTableCreate)
    {
        try
        {
            //if (MapSettings.MapStale) {
            //modified on 12-sep-2013
            if (BoundaryChangeSettings.BoundaryChangeState.Equals(BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT))
            {
                //nothing
            }
            else
            {
                Ut_SQL2TT.qF_SetTTforSchoolDivisions(IsRequiredTableCreate);
            }
            Ut_SQL2TT.qF_SetTTforMunicipality(IsRequiredTableCreate);

            Ut_SQL2TT.qF_SetTTforMunicipality_Parcels(IsRequiredTableCreate);

            if (MapSettings.MapAnalysisLayer == "SchoolDivisions")
                Ut_SQL2TT.turnOnSchoolDistricts();
            else if (MapSettings.MapAnalysisLayer == "Municipalities")
                Ut_SQL2TT.turnOnMunicipalities();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    ////test code
    //public static void getAnalysisMap(Boolean IsRequiredTableCreate)
    //{
    //    try
    //    {
    //        Ut_SQL2TT.qF_SetTTforMunicipality(IsRequiredTableCreate);
    //        //do nothing
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}

    public static void getBoundaryAdjustmentMap(Boolean IsRequiredTableCreate)
    {
        try
        {
            Ut_SQL2TT.qF_BA_SetTTforMunicipality(IsRequiredTableCreate);
            Ut_SQL2TT.qF_BASource_SetTTforMunicipality_Parcels(IsRequiredTableCreate);
            Ut_SQL2TT.qF_BADestination_SetTTforMunicipality_Parcels(IsRequiredTableCreate);

            //Ut_SQL2TT.qF_TEST_Parcels(IsRequiredTableCreate);
            //if (MapSettings.MapAnalysisLayer == "SchoolDivisions")
            //  Ut_SQL2TT.turnOnSchoolDistricts();
            //else if (MapSettings.MapAnalysisLayer == "Municipalities")
            //  Ut_SQL2TT.turnOnMunicipalities();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}