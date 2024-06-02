<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ControlPanel.aspx.cs" Inherits="ControlPanel" %>

<%@ Register Src="QuickZoom.ascx" TagName="QuickZoom" TagPrefix="uc2" %>
<%@ Register Src="ChangeThemes.ascx" TagName="ChangeThemes" TagPrefix="uc4" %>
<%@ Register Src="Reports.ascx" TagName="Reports" TagPrefix="uc5" %>
<%@ Register Src="BackgroundLayersControl.ascx" TagName="BackgroundLayersControl" TagPrefix="uc3" %>
<%@ Register Src="AnalysisControl.ascx" TagName="AnalysisControl" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href="base.css" />
    <link rel="stylesheet" type="text/css" href="mapping.css" />
		<link href="ProgressStyle.css" rel="stylesheet" type="text/css" />
    <script src="RemoteMapFunctions.js"></script>
		<script language="Javascript">
			function MyZoomToCoord() {
				parent.parent.ZoomToView("11516002.190242026", "3400692.2790488531", 2000.0, true);
				parent.parent.refresh();
			}
		</script>
</head>
<body>

<%--    <input type="button" id="Submit" width="250" height="75" onclick="MyZoomToCoord();" value="Zoom to Coordinate">
--%>       
    <form id="form1" runat="server">
<%--           <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button"></asp:Button>
--%> 

<asp:HiddenField ID="hdnLTTMap" runat="server" Value="" />
<input type="hidden" id="ctrltestid" name="ctrltestid" value="Loaded" />
<asp:HiddenField ID="hdnCurrentUserID" runat="server" Value="" />

<div id="progressContainer" style="display:none;" >
        <div id="processMessage"> 
            <img src="../../Images/fadingballs_orange.gif" alt="Please Wait..." 
                 width="25px" height="25px"  align="middle"/>
        </div>
        <div id="progressBackgroundFilter"></div>
</div>

<script lang="javascript" src="BackgroundLayersControl.js" ></script>
<asp:Table ID="tblGroups" runat="server">
</asp:Table>
<asp:Table ID="tblLayers" runat="server">
</asp:Table>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <script type="text/javascript" language="javascript">
        	try {
        		var prm = Sys.WebForms.PageRequestManager.getInstance();
        		prm.add_initializeRequest(InitializeControlPageRequestHandler);
        		prm.add_endRequest(EndControlPageRequestHandler);
        	} catch (err) {
        		alert(err);
        	}
        </script>
        
        <table id="tblAnalysis" runat="server" class="navigation">
        <input type="button" id="Button1" width="250" height="75" onclick="MyZoomToCoord();" value="Zoom to Coordinate">
            <tr>
            <td  style="height: 23px">
                <asp:LinkButton ID="btnAnaysisLayersTab" Text="Analysis Layers" CommandName="0" runat="server" OnClick="tabControls_MenuItemClick"></asp:LinkButton>
                </td>
                <td style="height: 23px">
                <asp:LinkButton ID="btnBackgroundLayersTab" Text="Background Layers" CommandName="1" runat="server" OnClick="tabControls_MenuItemClick"></asp:LinkButton>
                </td>
                <td style="height: 23px">
                <asp:LinkButton ID="btnQuickZoomTab" Text="Quick Zooms" CommandName="2" runat="server" OnClick="tabControls_MenuItemClick"></asp:LinkButton>
                </td>
                <td style="height: 23px">
                <asp:LinkButton ID="btnChangeThemesTab" Text="Change Themes" CommandName="3" runat="server" OnClick="tabControls_MenuItemClick"></asp:LinkButton>
                </td>
                <td style="height: 23px">
                <asp:LinkButton ID="btnReportsTab" Text="Reports" CommandName="4" runat="server" OnClick="tabControls_MenuItemClick"></asp:LinkButton>
            </td>
            </tr>
        </table>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="AnaysisLayersTab" runat="server">
                <uc1:AnalysisControl id="AnalysisControl1" runat="server">
                </uc1:AnalysisControl>
            </asp:View>
            <asp:View ID="BackgroundLayersTab" runat="server">
                <uc3:BackgroundLayersControl id="BackgroundLayersControl1" runat="server">
                </uc3:BackgroundLayersControl>
            </asp:View>
            <asp:View ID="QuickZoomTab" runat="server">
                <uc2:QuickZoom ID="QuickZoom1" runat="server" />
            </asp:View>
            <asp:View ID="ChangeThemesTab" runat="server">
                <uc4:ChangeThemes ID="ChangeThemes1" runat="server" />
            </asp:View>
            <asp:View ID="ReportsTab" runat="server">
                <uc5:Reports ID="Reports1" runat="server" />
            </asp:View>
        </asp:MultiView>
        </div>
    </form>
</body>
</html>
