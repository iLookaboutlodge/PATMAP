<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="loadGISData.aspx.cs" Inherits="PATMAPCGIS.loadGisData.loadGISData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Load GIS Data</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        This page will load the GIS data files into the database. The files need
        to be in
        <asp:Label ID="lblFileLocation" runat="server" Font-Bold="True" Text="Location"></asp:Label>
        as seen from the database server.<br />
        <asp:DropDownList ID="ddlImportType" runat="server" Width="218px">
            <asp:ListItem>Municipalities</asp:ListItem>
            <asp:ListItem>School Divisions</asp:ListItem>
            <asp:ListItem>Assessment Parcels</asp:ListItem>
            <asp:ListItem>Constituency Boundaries</asp:ListItem>
            <asp:ListItem>Parcel Linking Table</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Button ID="btnContinue" runat="server" Text="Load" OnClick="btnContinue_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" />&nbsp;<br />
        <asp:TextBox ID="importMessage" runat="server" Height="81px" ReadOnly="True" TextMode="MultiLine"
            Width="397px"></asp:TextBox><p class="MsoNormal" style="margin: 10pt 0in; line-height: 115%">
                <a name="_Toc185912913"><span class="Heading1Char"><span style="font-size: 14pt;
                    line-height: 115%; font-family: Arial; mso-bidi-font-size: 10.0pt; mso-bidi-font-family: 'Times New Roman'">
                    <strong>Loading Spatial Datasets into the PATMAP database</strong></span></span></a></p>
        <p class="MsoNormal" style="margin: 4pt 0in 0pt">
            <span style="font-size: 10pt; font-family: Arial">Some GIS datasets used by the PATMAP
                system must also be loaded into the SQL Server database for the purposes of advanced
                geospatial analysis.<span style="mso-spacerun: yes">&nbsp; </span>This page has
                been provided for the purposes of loading GIS datasets in into the SQL Server.</span></p>
        <p class="MsoNormal" style="margin: 4pt 0in 0pt">
            <?xml namespace="" ns="urn:schemas-microsoft-com:office:office" prefix="o" ?><o:p><SPAN 
style="FONT-SIZE: 10pt; FONT-FAMILY: Arial">&nbsp;</SPAN></o:p>
            &nbsp;</p>
        <p class="MsoNormal" style="margin: 4pt 0in 0pt">
            <b style="mso-bidi-font-weight: normal"><span style="font-size: 10pt"><span style="font-family: Arial">
                UPDATING GIS DATASETS SHOULD ONLY BE DONE WHILE THE PATMAP APPLICATION IS NOT IN
                USE.<span style="mso-spacerun: yes">&nbsp; </span>UNPREDICATBLE RESULTS, AND/OR
                ERRORS WILL OCCUR IF THE PATMAP APPLICATION IS IN USE DURING UPDATES.<o:p></o:p></span></span></b></p>
        <p class="MsoNormal" style="margin: 4pt 0in 0pt">
            <b style="mso-bidi-font-weight: normal">
                <o:p><SPAN 
style="FONT-SIZE: 10pt; FONT-FAMILY: Arial">&nbsp;</SPAN></o:p>
                &nbsp;</b>
        </p>
        <p class="MsoNormal" style="margin: 4pt 0in 0pt">
            <span style="font-size: 10pt; font-family: Arial">Steps to update GIS files in SQL Server
                database:</span></p>
        <ol start="1" style="margin-top: 0in" type="1">
            <li class="MsoNormal" style="margin: 10pt 0in; line-height: 115%; mso-list: l0 level1 lfo1">
                <span style="font-size: 10pt; font-family: Arial">From the “Updateable GIS Datasets”
                    drop-down list on this page choose the dataset you wish to update.</span></li>
            <li class="MsoNormal" style="margin: 10pt 0in; line-height: 115%; mso-list: l0 level1 lfo1">
                <span style="font-size: 10pt; font-family: Arial">Ensure that the updated GIS dataset
                    has been loaded into its “Storage Location” folder as defined in its “GIS Data Requirements”
                    section of the PATMAP Datasets &amp; Update Procedures document. </span></li>
            <ol start="1" style="margin-top: 0in" type="a">
                <li class="MsoNormal" style="margin: 10pt 0in; line-height: 115%; mso-list: l0 level2 lfo1">
                    <span style="font-size: 10pt; font-family: Arial">IMPORTANT NOTE: If updating the Assessment
                        linking table – the Assessment Linking table must be in .CSV format, with the first
                        column called “MapParcelID”, the second column called “AssessmentNumber”, and the
                        third column called “MunID”.<span style="mso-spacerun: yes">&nbsp; </span>The MapParcelID
                        column must match to the ID’s of the Assessment parcels GIS dataset.<span style="mso-spacerun: yes">&nbsp;
                        </span>The AssessmentNumber column must contain the Assessment Roll Number associated
                        with MapParcelID.<span style="mso-spacerun: yes">&nbsp; </span>The MunID column
                        identifies which municipality an AssessmentNumber belongs to since Assessment Roll
                        Numbers are not unique across Municipalities.</span></li>
            </ol>
            <li class="MsoNormal" style="margin: 10pt 0in; line-height: 115%; mso-list: l0 level1 lfo1">
                <span style="font-size: 10pt; font-family: Arial">Press the “Update” button.</span></li>
        </ol>
        <p class="MsoNormal" style="margin: 4pt 0in 0pt 0.5in">
            <b style="mso-bidi-font-weight: normal"><span style="font-size: 10pt"><span style="font-family: Arial">
                THIS PROCEDURE MAY RUN FOR A LONG PERIOD OF TIME DEPENDING ON THE SIZE OF THE GIS
                DATASET.<o:p></o:p></span></span></b></p>
    </div>
    </form>
</body>
</html>
