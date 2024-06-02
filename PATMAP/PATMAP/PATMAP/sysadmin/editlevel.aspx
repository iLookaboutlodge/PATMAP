<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="editlevel.aspx.vb" Inherits="PATMAP.editlevel" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="sysTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
   <patmap:sysTabMenu id="subMenu" runat="server"></patmap:sysTabMenu>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit User Level"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblUserLevel" runat="server" Text="User Level"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtUserLevel" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHUserLevel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                    </td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox></td>
                                <td valign="top" style="padding-left: 4px;"><asp:ImageButton ID="btnHDescription" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                          
                    </td>
                </tr>                               
                <tr>
                    <td class="label">
                        <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                    </td>
                    <td class="field">
                         <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox></td>
                                <td valign="top" style="padding-left:4px;"><asp:ImageButton ID="btnHNotes" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                         
                    </td>
                </tr>                                                  
            </table>
        </div>  
        <div class="subHeader">List</div>
        <asp:UpdatePanel ID="uplPermission" runat="server" >
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Screen Permissions: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>  
                 <asp:GridView ID="grdPermission" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="true" AllowSorting="true" DataKeyNames="screenNameID" >
                     <Columns>
                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%">
                             <ItemTemplate>                                            
                                 <asp:CheckBox ID="ckbAccess" Checked='<%# DataBinder.Eval(Container.DataItem,"access")%>' runat="server" />
                             </ItemTemplate>                                 
                         </asp:TemplateField>
                         <asp:BoundField HeaderText="Section" DataField="sectionName" SortExpression="sectionName" />
                         <asp:BoundField HeaderText="Screen" DataField="screenName" SortExpression="screenName" />                         
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                     <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView>                
            </ContentTemplate>
             <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="grdPermission" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdPermission" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdPermission" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel>   
         <br /> 
         <div class="totalCount"><asp:Label ID="lblTotalClass" runat="server" Text="Tax Class Permissions: "></asp:Label><asp:Label ID="txtTotalClass" runat="server" Text=""></asp:Label></div>          
          <asp:GridView ID="grdClasses" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdSmStyle" DataKeyNames="taxClassID" Width="50%">
             <Columns>
                 <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%">
                     <ItemTemplate>                                            
                         <asp:CheckBox ID="ckbAccess" Checked='<%# DataBinder.Eval(Container.DataItem,"access")%>' runat="server" />
                     </ItemTemplate>                                 
                 </asp:TemplateField>
                 <asp:BoundField HeaderText="Tax Class" DataField="taxClass" />
             </Columns>
             <HeaderStyle CssClass="colHeader" />
             <AlternatingRowStyle CssClass="alertnateRow" />
             <PagerSettings Position="Top" />
             <PagerStyle CssClass="grdPage" />
         </asp:GridView>        
         <br />
         <div class="totalCount">
            <asp:Label ID="lblTaxClassPermissionsLTT" runat="server" Text="Local Tax Tools Permissions: Tax Class View"></asp:Label></div>
            <br />
            <asp:CheckBox ID="chkShowLTTtaxClasses" runat="server" Text="Show Expanded View of Tax Classes in Local Tax Tools" />
    </div>         
</asp:Content>

