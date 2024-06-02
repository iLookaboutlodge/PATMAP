<%@ Page Language="vb" ValidateRequest="False" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewtaxyearmodel.aspx.vb" Inherits="PATMAP.viewtaxyearmodel" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="dataTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:dataTabMenu id="subMenu" runat="server"></patmap:dataTabMenu> 
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">View Tax Year Model</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
           <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddTaxYr.gif" /></div>
           <table cellpadding="0" cellspacing="0" border="0">
                 <tr>
                    <td class="label"><asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlYear" runat="server">
                        </asp:DropDownList> <asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    <td rowspan="2">&nbsp;</td>
                 </tr>
                 <tr>
                    <td class="label"><asp:Label ID="lblTaxYrModel" runat="server" Text="Tax Year Model"></asp:Label></td>
                    <td class="field">   
                        <asp:TextBox ID="txtTaxYrModel" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHTaxYrModel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp" />
                    </td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label></td>
                    <td class="field" valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:RadioButtonList ID="rdlStatus" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem>Active</asp:ListItem>
                                <asp:ListItem>History</asp:ListItem>
                                </asp:RadioButtonList></td>
                                <td><asp:ImageButton ID="btnHStatus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                     </td>                    
                    <td class="btns" style="padding-top:4px;"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>
           </table>
        </div> 
        <div class="subHeader">List</div>         
         <asp:UpdatePanel ID="uplTaxYrModel" runat="server">
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Tax Year Models: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdTaxYrModel" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="taxYearModelID">
                     <Columns>
                        <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editTaxYearModel">
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteTaxYearModel">
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>                  
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmHistory.gif&quot; /&gt;" >
                             <ItemStyle CssClass="colHistory" />
                         </asp:ButtonField>                               
                         <asp:BoundField HeaderText="Tax Year Model" DataField="taxYearModelName" SortExpression="taxYearModelName" />  
                         <asp:BoundField HeaderText="Year" DataField="year" SortExpression="year" />                              
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                      <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView>  
            </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxYrModel" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxYrModel" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxYrModel" EventName="Sorting" />
            </Triggers>
         </asp:UpdatePanel>                      
    </div>         
</asp:Content>

