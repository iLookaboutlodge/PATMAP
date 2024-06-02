<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BoundaryAdjustmentMap.aspx.cs" Inherits="BoundaryAdjustmentMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Boundary Adjustment Map</title>
<%--    <link rel="stylesheet" type="text/css" href="css/base.css" />
    <link rel="stylesheet" type="text/css" href="css/mapping.css" />
    <link rel="shortcut icon" href="../images/favicon.ico" type="image/x-icon" />--%>
</head>

<frameset>
 <frame id ="myMap" name="myMap" src="<%=ConfigurationManager.AppSettings["MapServerURL"]%>mapviewerajax/?WEBLAYOUT=<%=WebLayout%>&SESSION=<%=Session%>&LOCALE=en"></frame>
</frameset>

</html>
