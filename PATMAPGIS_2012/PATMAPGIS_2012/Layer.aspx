<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Layer.aspx.cs" Inherits="PATMAPGIS_2012.Layer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
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
    </div>
    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_OnCheckedChanged" Visible="False" />
    <asp:CheckBox ID="CheckBox2" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox3" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox4" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox5" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox6" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox7" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox8" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox9" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox10" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox11" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox12" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox13" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox14" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox15" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox17" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox16" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox18" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox19" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox20" runat="server" Visible="False" />
    <asp:CheckBox ID="CheckBox21" runat="server" Visible="False" />
    

    </form>
</body>
</html>
