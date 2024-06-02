<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="tabmenu.ascx.vb" Inherits="PATMAP.tabmenu" %>
 <script language="javascript" type="text/javascript" src="../js/general.js"></script>
<div id="tabmenuContainer">
    <table cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td><asp:Image ID="imgNavLeft" runat="server" ImageUrl="~/images/subNavLeft.gif" /></td>
            <td class="subMenu"><asp:Menu ID="subMenu" runat="server" Orientation="Horizontal" DataSourceID="subSiteMapDataSource" MaximumDynamicDisplayLevels="1" ItemWrap="True" RenderingMode="Table" >
                <StaticMenuItemStyle CssClass="subMenuItem" />                
                <StaticSelectedStyle CssClass="subMenuSelected" />
                <DynamicMenuItemStyle CssClass="subMenuLevel2" />
                </asp:Menu></td>
            <td><asp:Image ID="imgNavRight" runat="server" ImageUrl="~/images/subNavRight.gif" /></td>
        </tr>
    </table>
   
    <asp:SiteMapDataSource ID="subSiteMapDataSource" runat="server" ShowStartingNode="False"
    StartingNodeOffset="-1" />                             
</div>
