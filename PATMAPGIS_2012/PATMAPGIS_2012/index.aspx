<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="PATMAPGIS_2012.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
    <title>Analysis Map</title>
    <link rel="stylesheet" type="text/css" href="css/base.css" />
    <link rel="stylesheet" type="text/css" href="css/mapping.css" />
    <link rel="shortcut icon" href="../images/favicon.ico" type="image/x-icon" />

</head>

<frameset cols="*,300px">
    <frameset rows="47px,*">
        <frame id="title" src="MapControlsh.aspx" scrolling=no frameborder="0"/>
        <frame id="map" src="MapContainer.aspx" MARGINWIDTH="0" MARGINHEIGHT="0" scrolling="no"/>
    </frameset>
    <%--<frame src="ControlPanel.aspx?IsAssmnt=true" />--%>
</frameset>

</body>
</html>
