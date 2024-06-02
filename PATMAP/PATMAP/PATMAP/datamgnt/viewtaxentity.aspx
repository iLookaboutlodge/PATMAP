<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewtaxentity.aspx.vb" Inherits="PATMAP.viewtaxentity" %>
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
                    <p class="Title">View Tax Entity</p>
                </div>
            </div>
        </div>
        <div class="commonForm">            
            <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddEntity.gif" /></div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblJurisType" runat="server" Text="Jurisdiction Type"></asp:Label></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlJurisType" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHJurisType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                       
                </tr>   
                <tr>
                    <td class="label"><asp:Label ID="lblJurisdiction" runat="server" Text="Jurisdiction"></asp:Label></td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="uplJurisdiction" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlJurisdiction" runat="server"></asp:DropDownList> 
                                        </ContentTemplate>
                                         <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlJurisType" EventName="SelectedIndexChanged" />                               
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td><asp:ImageButton ID="btnHJurisdiction" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                         
                    </td>                   
                    <td class="btns"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>
            </table>            
        </div>
        <div class="subHeader">LIST</div>
        <asp:UpdatePanel ID="uplEntity" runat="server">
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Tax Entities: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdTaxEntity" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="number">
                     <Columns>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editEntity">
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteEntity" >
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>
                         <asp:BoundField HeaderText="Jurisdiction" DataField="jurisdiction" SortExpression="jurisdiction" ItemStyle-Width="50%"/>
                         <asp:BoundField HeaderText="Number" DataField="number" SortExpression="number" ItemStyle-Width="25%"/>                 
                         <asp:BoundField HeaderText="Jurisdiction Type " DataField="jurisdictionType" SortExpression="jurisdictionType" ItemStyle-Width="25%"/>                 
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                     <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView>   
            </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxEntity" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxEntity" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxEntity" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel>                     
    </div>         
</asp:Content>
