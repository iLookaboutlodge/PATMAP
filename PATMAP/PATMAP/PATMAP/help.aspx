<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="help.aspx.vb" Inherits="PATMAP.help" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title>Property Assessment and Tax Mapping and Analysis Program - Help Page</title>
     <link rel="shortcut icon" href="~/images/favicon.ico" type="image/x-icon" />
     <link rel="stylesheet" type="text/css" href="~/css/base.css" />
     <link rel="stylesheet" media="print" type="text/css" href="~/css/print.css" />
     <script language="javascript" type="text/javascript" src="js/general.js"></script>
</head>
<body>    
    <form id="form1" runat="server">                                
        <div class="commonForm">   
            <%--<div class="subHeader"><asp:Label runat="server" ID="lblsubHeader"></asp:Label></div>--%>            
            <div id="print" style="padding-top:10px;padding-bottom:10px;">
                <div id="pringImg">
                    <a href="" onclick="return printPage();">
                        <asp:Image ID="btnPrint" ImageUrl="~/images/btnPrint.gif" runat="server" />
                    </a>
                </div>
                <div id="printLink">
                    <a href="" onclick="return printPage();">Print this page</a>                    
                </div>
            </div>
            <asp:Label runat="server" ID="lablHelpContent"></asp:Label>             
        </div>
    </form>
</body>
</html>
