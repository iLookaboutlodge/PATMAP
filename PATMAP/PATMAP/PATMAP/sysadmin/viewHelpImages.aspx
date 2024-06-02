<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="viewHelpImages.aspx.vb" Inherits="PATMAP.viewHelpImages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <title>Help Images</title>
     <link rel="shortcut icon" href="~/images/favicon.ico" type="image/x-icon" />
     <link rel="stylesheet" type="text/css" href="~/css/base.css" />
     <link rel="stylesheet" media="print" type="text/css" href="~/css/print.css" />
     <script language="javascript" type="text/javascript" src="../js/general.js"></script>
</head>
<body>    
    <form id="form1" runat="server">    
    
        <div class="commonForm">   
            <div class="subHeader">
                <asp:Label runat="server" ID="lblsubHeader" Text="View Help Images"></asp:Label>
            </div>
            <div class="errorText">
                <asp:Label ID="lblErrorText" runat="server" Height="40"></asp:Label>
            </div>             
            <div class="totalCount">
                <asp:Label ID="lblTotal" runat="server" Text="Images: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label>
            </div>
            <asp:GridView ID="grdImages" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="imageName">
                 <Columns>
                    <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; &gt;" CommandName="deleteFunction">
                         <ItemStyle CssClass="colDelete" />
                    </asp:ButtonField>                  
                    <asp:HyperLinkField HeaderText="Name" SortExpression="imageName" DataTextField="imageName" ItemStyle-Width="25%"/>
                    <asp:BoundField HeaderText="Url" DataField="imageUrl" ItemStyle-Width="75%"/>                     
                 </Columns>
                 <HeaderStyle CssClass="colHeader" />
                 <AlternatingRowStyle CssClass="alertnateRow" />
                 <PagerSettings Position="Top" />
                 <PagerStyle CssClass="grdPage" />
            </asp:GridView> 
            <br />
            <br />
            <div class="subHeader">
                <asp:Label runat="server" ID="lblImagePreview" Text="Image Preview"></asp:Label>
            </div> 
            <div class="groupClass" style="width: 90%">
                <asp:Image runat="server" ID="image" ImageUrl="~/images/spacer.gif"/>
            </div>  
        </div>                                    
    </form>
</body>
</html>         


