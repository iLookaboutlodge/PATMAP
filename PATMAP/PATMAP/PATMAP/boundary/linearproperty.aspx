<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="linearproperty.aspx.vb" Inherits="PATMAP.linearproperty" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Property Assessment and Tax Mapping and Analysis Program</title>
     <link rel="shortcut icon" href="~/images/favicon.ico" type="image/x-icon" />
     <link rel="stylesheet" type="text/css" href="~/css/base.css" />
    <link rel="stylesheet" media="print" type="text/css" href="~/css/print.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="commonForm">
        <div class="subHeader">Linear Property</div>
        <div class="errorText">
            <asp:Label ID="lblErrorText" runat="server" Height="40"></asp:Label>
        </div>        
        <asp:GridView ID="grdLinearProp" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" DataKeyNames="taxClassID">
             <Columns> 
                 <asp:BoundField HeaderText="Description" DataField="taxClass"/>
                 <asp:BoundField HeaderText="Assessment" DataField="totalAssessmentValue"/>
                 <asp:TemplateField HeaderText ="% to Transfer" ItemStyle-HorizontalAlign="Center">
                     <ItemTemplate>
                         <asp:TextBox ID="txtPercentageTransfer" runat="server" Width="100" Text='<%# Bind("percentageTransfer") %>'></asp:TextBox>
                     </ItemTemplate>
                 </asp:TemplateField>                                  
             </Columns>
             <HeaderStyle CssClass="colHeader" />
             <AlternatingRowStyle CssClass="alertnateRow" />
             <PagerSettings Position="Top" />
             <PagerStyle CssClass="grdPage" />
         </asp:GridView> 
         <div class="btnsBottom"><asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" CssClass="btns" /></div>
    </div>
    </form>
</body>
</html>
