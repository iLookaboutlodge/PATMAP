<%@ Page Language="VB" MasterPageFile="MasterPage.master" AutoEventWireup="false" Inherits="PATMAP.home" Codebehind="home.aspx.vb" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<asp:Content ContentPlaceHolderID="mainContent" runat="server">       
    <script language="javascript" type="text/javascript" src="js/general.js"></script>
    <div class="patmapDisclaimer"><asp:LinkButton ID="patmapDisclaimer" runat="server" OnClientClick="openWindow('patmapdisclaimer.aspx','left=50,top=50,width=500,height=600,toolbar=0,resizable=1,scrollbars=1'); return false;">PATMAP Disclaimer</asp:LinkButton></div>
    <div class="commonContent">
        <div id="UserInfo">
            <div class="PageHeaderModule">
                <div class="Header">
	                <p class="Title">User Info</p>
                </div>
            </div>
            <div class="content">
                Username: <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label><br /><br />
                User Group: <asp:Label ID="lblUserGroup" runat="server" Text=""></asp:Label><br /><br />
                User Level: <asp:DropDownList ID="ddlUserLevel" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHUserLevel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton><br /><br />               
                Last Login: <asp:Label ID="lblLastLogin" runat="server" Text=""></asp:Label><br /><br />
                <div class="btnHome"><asp:ImageButton ID="btnUserProfile" runat="server" ImageUrl="~/images/btnUserProfile.gif" /></div>
            </div>
        </div>         
        <div id="SystemInfo">
            <div class="PageHeaderModule">
                <div class="Header">
	                <p class="Title">System Statistics</p>
                </div>
            </div>
            <div class="content">
                Assessment Last Loaded:
                <asp:Label ID="lblAssmntUpdt" runat="server" Text=""></asp:Label><br /><br />
                Mill Rate Last Loaded:
                <asp:Label ID="lblMillRateUpdt" runat="server" Text=""></asp:Label><br /><br />
                <asp:Label ID="SatelliteUpdt" runat="server" Text="Satellite Last Updated: " Visible="False"></asp:Label><br /><br />                
                <div class="btnHome" style="padding-top:35px;"><asp:ImageButton ID="btnSynchronize" runat="server" ImageUrl="~/images/btnSynchronize.gif" Visible="False" /></div>                                
            </div>            
        </div>
    </div>                
</asp:Content>
