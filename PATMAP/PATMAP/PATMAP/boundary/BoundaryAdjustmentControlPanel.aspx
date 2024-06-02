<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BoundaryAdjustmentControlPanel.aspx.cs" Inherits="BoundaryAdjustmentControlPanel" %>

<%@ Register Src="../BackgroundLayersControl.ascx" TagName="BackgroundLayersControl" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href="../base.css" />
    <link rel="stylesheet" type="text/css" href="../mapping.css" />
		<link href="../ProgressStyle.css" rel="stylesheet" type="text/css" />
    <script src="../RemoteMapFunctions.js"></script>
    <script src="BoundaryAdjustmentControls.js"></script>
</head>
<body onload="init();">
    <form id="form1" runat="server">
		<asp:HiddenField ID="hdnLTTMap" runat="server" Value="" />
		<input type="hidden" id="ctrltestid" name="ctrltestid" value="Loaded" />
		<div id="progressContainer" style="display:none;" >
				    <div id="processMessage"> 
						    <img src="../../Images/fadingballs_orange.gif" alt="Please Wait..." 
								     width="25px" height="25px"  align="middle"/>
						</div>
						<div id="progressBackgroundFilter"></div>
		</div>
		
		<script lang="javascript" src="../BackgroundLayersControl.js" ></script>
    
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
    
    <div>
        <table class="navigation" runat="server" id="tblNavigation">
            <tr>
                <td>
                    <asp:LinkButton ID="btnBoundaryAdjustmentTab" Text="Boundary Adjustment" CommandName="0" runat="server" OnClick="tabControls_MenuItemClick"></asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="btnBackgroundLayersTab" Text="Background Layers" CommandName="1" runat="server" OnClick="tabControls_MenuItemClick"></asp:LinkButton>
                </td>
            </tr>
        </table>
        
     <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0" EnableTheming="True">
            <asp:View ID="BoundaryAdjustmentTab" runat="server">
            
							<asp:UpdatePanel ID="updAnalysis" runat="server">
								<ContentTemplate>
								<asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" style="display:none"
										>Test Map</asp:LinkButton>
								</ContentTemplate>
							</asp:UpdatePanel>
            
                <table style="width:100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblBoundaryAdjustment" runat="server" Text="Boundary Adjustment" CssClass="sectionHeader"></asp:Label></td>
                    </tr>
                </table>
                <table>
                    <tr><td>Source:</td><td><asp:Label ID="lblSource" runat="server" Text="Label"></asp:Label></td><td>
                        <asp:Panel ID="pnlSourceColour" runat="server" Height="20px" Width="20px">
                        </asp:Panel>
                    </td></tr>
                    <tr><td>Destination:</td><td><asp:Label ID="lblDestination" runat="server" Text="Label"></asp:Label></td><td>
                        <asp:Panel ID="pnlDestColour" runat="server" Height="20px" Width="20px">
                        </asp:Panel>
                    </td></tr>
                    <tr><td colspan="3"><asp:ListBox ID="lstSelectedParcels" runat="server" Height="386px" Width="181px"></asp:ListBox></td></tr>
                </table>
                <input ID="SelectedParcels" type="hidden" runat="server" />
                <input ID="SelectedISCParcels" type="hidden" runat="server" />
                <br />
                <asp:LinkButton ID="btnBoundaryChange" runat="server" OnClick="btnBoundaryChange_Click">Perform Boundary Change</asp:LinkButton><br />
                </asp:View>
            <asp:View ID="BackgroundLayersTab" runat="server">
            <uc1:BackgroundLayersControl ID="BackgroundLayersControl1" runat="server" />
            </asp:View>
        </asp:MultiView>
    </div>
        
    </form>
</body>
</html>
