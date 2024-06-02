<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm3.aspx.vb" Inherits="PATMAP.WebForm3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Property Assessment and Tax Mapping and Analysis Program</title>
    <link rel="stylesheet" type="text/css" href="~/css/base.css" />
    <link rel="stylesheet" media="print" type="text/css" href="~/css/print.css" />
    <link rel="stylesheet" type="text/css" href="~/ProgressStyle.css" />
    
    <!--[if IE 6]>
        <link rel="stylesheet" type="text/css" href="~/css/IE6style.css" />
    <![endif]-->
    <link rel="shortcut icon" href="~/images/favicon.ico" type="image/x-icon" />

   <script language="javascript" type="text/javascript" src="js/general.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:ImageButton ID="btnHUserLevel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="true" CssClass="btnHelp"></asp:ImageButton>

<%--        <div class='fieldhelp' style='visibility:visible;' id='testID'> <iframe src='' frameborder='0' scrolling='no' style='filter:alpha(opacity=0);z-index:-1;position:absolute;width:290px;height:30px;top:0;left:0;border:1px solid black;'></iframe>Testing text</div>--%>

        <asp:Label ID="lblHelp" runat="server" Text=""></asp:Label>

    </div>
    </form>
</body>
</html>
