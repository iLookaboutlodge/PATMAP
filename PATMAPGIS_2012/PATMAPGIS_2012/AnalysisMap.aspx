﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalysisMap.aspx.cs" Inherits="AnalysisMapPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Analysis Map</title>
<%--    <link rel="stylesheet" type="text/css" href="css/base.css" />
    <link rel="stylesheet" type="text/css" href="css/mapping.css" />
    <link rel="shortcut icon" href="../images/favicon.ico" type="image/x-icon" />--%>
</head>

    <frameset >
  
 <frame id ="myMap" name="myMap" src="http://localhost/mapserver2012/mapviewerajax/?WEBLAYOUT=<%=WebLayout%>&SESSION=<%=Session%>&LOCALE=en"></frame>
    
        </frameset>
</html>
