<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LTTviewReport.aspx.vb" Inherits="PATMAP.LTTviewReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <title>View Reports</title>
     <link rel="shortcut icon" href="~/images/favicon.ico" type="image/x-icon" />
     <link rel="stylesheet" type="text/css" href="~/css/base.css" />
     <link rel="stylesheet" media="print" type="text/css" href="~/css/print.css" />
     <script language="javascript" type="text/javascript" src="../js/general.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="viewReportCommonForm">   
            <asp:ScriptManager ID="scmControl" runat="Server" EnableScriptGlobalization="true"
                EnableScriptLocalization="true" AsyncPostBackTimeout="60000" />

            <div class="viewReportSubHeader">
                <asp:Label runat="server" ID="lblsubHeader" Text="View Report"></asp:Label>
            </div>
            <div class="viewReportErrorText">
                <asp:Label ID="lblErrorText" runat="server" Height="40"></asp:Label>
            </div>


<%--            <rsweb:ReportViewer ID="rpvReports" runat="server" Height="1000px" ShowParameterPrompts="False"        
            Width="100%">  </rsweb:ReportViewer>--%>

            <rsweb:ReportViewer ID="rpvReports" runat="server" height="1000px" 
                ShowExportControls="true" ShowPageNavigationControls="true" ShowPrintButton="true" ShowRefreshButton="false" ShowFindControls="true" ShowParameterPrompts="False"        
                Width="100%" AsyncRendering="false">
            </rsweb:ReportViewer>

       
    </div>
    </form>
</body>
</html>
