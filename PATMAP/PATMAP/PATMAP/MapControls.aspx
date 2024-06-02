<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapControls.aspx.cs" Inherits="MapControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Map Controls</title>
    <link rel="stylesheet" type="text/css" href="css/base.css" />
    <link rel="stylesheet" type="text/css" href="css/mapping.css" />
</head>
<body bottommargin="0px" topmargin="0px" rightmargin="0px" leftmargin="0px">
        <table id="tblMenu" cellpadding="0" align="center" style="width: 90%;">
            <tr align="center" valign="top">
                <td align="left" width="24">
                    <span style="cursor: hand">
                        <img name="pointers" onclick="top.map.selectMode();" border="0" src="images/pointer2.gif"
                            width="24" height="23" alt="Select"></span>
                </td>
                <td width="24">
                    <span style="cursor: hand">
                        <img name="pan" onclick="top.map.panMode();" border="0" src="images/buttons3.gif"
                            width="24" height="23" alt="Pan"></span>
                </td>
                <td width="24">
                    <span style="cursor: hand">
                        <img name="zoomIn" onclick="top.map.zoomInMode();" border="0" src="images/buttons1.gif"
                            width="24" height="23" alt="Zoom In"></span>
                </td>
                <td width="24">
                    <span style="cursor: hand">
                        <img name="zoomOut" onclick="top.map.zoomOutMode();" border="0" src="images/buttons2.gif"
                            width="24" height="23" alt="Zoom Out"></span>
                </td>
                <td align="left" width="24">
                    <span style="cursor: hand">
                        <img name="zoomPre" onclick="top.map.zoomPrevious();" border="0" src="images/zoompre2.gif"
                            width="24" height="23" alt="Zoom Previous"></span>
                </td>
                <td width="24">
                    <span style="cursor: hand">
                        <img name="unZoom" onclick="top.map.unzoom();" border="0" src="images/unzoom.gif"
                            width="24" height="23" alt="Unzoom"></span>
                </td>
                <td width="24">
                    <span style="cursor: hand">
                        <img name="Image45" onclick="top.map.clearSelection();" border="0" src="images/clearOn.jpg"
                            width="24" height="23" alt="Clear Selection"></span>
                </td>
                <td width="24">
                    <span style="cursor: hand">
                        <img name="Print" onclick="top.map.printMap();" border="0" src="images/print2.gif"
                            width="24" height="23" alt="Print"></span>
                </td>
                <td width="24">
                    <span style="cursor: hand">
                        <img name="Image46" onclick="top.map.copyMap();" border="0" src="images/copyOn.gif"
                            width="24" height="23" alt="Copy map onto Clipboard"></span>
                </td>
                <td width="24">
                    <span style="cursor: hand">
                        <img name="imgMeasure" onclick="top.map.measureMap();" border="0" src="images/measure.gif"
                            width="24" height="23" alt="View Distance"></span>
                </td>
                <td nowrap style="text-align:center; vertical-align:middle">
                    &nbsp;
                    <asp:Label ID="lblModelName" runat="server" Text=""></asp:Label>
                </td>
                <td align="right" valign="middle">
                    <asp:HyperLink ID="btnClose" runat="server" Visible=False>Close Map</asp:HyperLink>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMapTitle" runat="server" ></asp:Label>
</body>
</html>
