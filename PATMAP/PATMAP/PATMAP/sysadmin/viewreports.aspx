<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewreports.aspx.vb" Inherits="PATMAP.viewreports" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">View Reports</p>
                </div>
            </div>
        </div>
        <div class="commonForm">            
            <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddReport.gif" /></div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblReportName" runat="server" Text="Name"></asp:Label></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtReportName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHReportName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblReportType" runat="server" Text="Type"></asp:Label></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlReportType" runat="server">
                        <asp:ListItem>&lt;select&gt;</asp:ListItem>
                        <asp:ListItem>Standard</asp:ListItem>
                        <asp:ListItem>Customized</asp:ListItem>
                    </asp:DropDownList> <asp:ImageButton ID="btnHReportType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                 <tr>
                    <td class="label"><asp:Label ID="lblDisplayType" runat="server" Text="Display Type"></asp:Label></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlDisplayType" runat="server">
                        <asp:ListItem>&lt;select&gt;</asp:ListItem>
                        <asp:ListItem>Tables</asp:ListItem>
                        <asp:ListItem>Graphs</asp:ListItem>
                    </asp:DropDownList> <asp:ImageButton ID="btnHDisplayType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                  <td class="label"><asp:Label ID="lblTaxType" runat="server" Text="Tax Type"></asp:Label></td>
                    <td class="field"><asp:DropDownList ID="ddlTaxType" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHTaxType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    <td class="btns"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>                 
            </table>            
        </div>
        <div class="subHeader">List</div>         
         <asp:UpdatePanel ID="uplReports" runat="server">            
         <ContentTemplate>
            <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Reports: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
             <asp:GridView ID="grdReports" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True">
                 <Columns>
                     <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; &gt;">
                         <ItemStyle CssClass="colEdit" />
                     </asp:ButtonField>
                     <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; &gt;">
                         <ItemStyle CssClass="colDelete" />
                     </asp:ButtonField>
                    <asp:BoundField HeaderText="Reports" />
                    <asp:BoundField HeaderText="Type" />
                    <asp:BoundField HeaderText="Display Type" /> 
                    <asp:BoundField HeaderText="Description" /> 
                 </Columns>
                 <HeaderStyle CssClass="colHeader" />
                 <AlternatingRowStyle CssClass="alertnateRow" />
                 <PagerSettings Position="Top" />
                 <PagerStyle CssClass="grdPage" />
             </asp:GridView>
         </ContentTemplate>
         <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdReports" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdReports" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdReports" EventName="Sorting" />
            </Triggers>
         </asp:UpdatePanel>                     
    </div>         
</asp:Content>


