﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MapInformation.aspx.vb" Inherits="PATMAP.MapInformation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods ="true" ScriptMode ="Auto" ></asp:ScriptManager>
    <ajaxToolkit:TabContainer ID="tc" runat="server">
        <ajaxToolkit:TabPanel ID="tpAnalysis" runat ="server" HeaderText ="AnalysisLayers" TabIndex ="0">
        <HeaderTemplate >Analysis</HeaderTemplate>
        <ContentTemplate >
        
        </ContentTemplate>
        </ajaxToolkit:TabPanel>
      
    </ajaxToolkit:TabContainer>
    
    </div>
    </form>
</body>
</html>
