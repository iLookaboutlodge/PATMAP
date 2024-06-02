<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BackgroundLayersControl.ascx.cs" Inherits="BaseLayersControl" %>



<table style="width:100%">
    <tr>
        <td align="center">
            <asp:Label ID="lblBackgroundLayers" runat="server" Text="Background Layers" CssClass="sectionHeader"></asp:Label></td>
    </tr>
</table>
<script lang="javascript" src="BackgroundLayersControl.js" ></script>
<asp:Table ID="tblGroups" runat="server">

</asp:Table>
<asp:Table ID="tblLayers" runat="server">

</asp:Table>
&nbsp;