<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="sorttaxclass.aspx.vb" Inherits="PATMAP.sorttaxclass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <title>Sort Tax Classes</title>
     <link rel="shortcut icon" href="~/images/favicon.ico" type="image/x-icon" />
     <link rel="stylesheet" type="text/css" href="~/css/base.css" />
     <link rel="stylesheet" media="print" type="text/css" href="~/css/print.css" />
     <script language="javascript" type="text/javascript" src="/js/general.js"></script>
</head>
<body>    
    <form id="sorttaxclass" runat="server">    
        <div class="commonForm">   
            <div class="subHeader">
                <asp:Label runat="server" ID="lblSortTaxClass" Text="Sort Tax Classes"></asp:Label>
            </div>
            <div class="errorText">
                <asp:Label ID="lblErrorText" runat="server" Height="40"></asp:Label>
            </div> 
            <table width="100%">
            <tr>
            <td align="center">
            <table cellpadding="0" cellspacing="0" border="0">                
                <tr>
                    <td align="right">
                        <asp:ListBox ID="lstTaxClass" runat="server" Height="350px" Width="300px" AutoPostBack="True"></asp:ListBox></td>
                    <td style="padding-left:20px;" align="left">
                        <asp:ImageButton ID="btnMoveUp" runat="server" ImageUrl="~/images/btnMoveUp.gif" />&nbsp;&nbsp;
                        <asp:ImageButton ID="btnMoveDown" runat="server" ImageUrl="~/images/btnMoveDown.gif" />
                    </td>
                </tr> 
                <tr>
                   <td align="left">
                        <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" />
                   </td> 
                </tr>               
            </table>                        
            </td>
            </tr>
            </table>
        </div>                                    
    </form>
</body>
</html>