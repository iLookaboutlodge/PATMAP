<%@ Page Language="vb" ValidateRequest="false" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="editpov.aspx.vb" Inherits="PATMAP.editpov" %>
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
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit POV"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblDSN" runat="server" Text="Data Set Name"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field">
                       <asp:TextBox ID="txtNewDSN" runat="server" CssClass="txtLong"></asp:TextBox> <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr> 
                <tr>
                    <td class="label"><asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field"><asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>    
            </table>
            <asp:UpdatePanel ID="uplPOV" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdPOV" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdSmStyle" DataKeyNames="taxClassID">
                 <Columns>
                     <asp:ButtonField Text="&lt;img src=&quot;../images/btnExpand.gif&quot; &gt;" CommandName="expandClass" ItemStyle-HorizontalAlign="Center"/>
                     <asp:BoundField HeaderText="Tax Class" DataField="taxClass" />
                     <asp:TemplateField HeaderText ="Base Year POV (%)" ItemStyle-HorizontalAlign="Center">
                         <ItemTemplate>                                            
                             <asp:TextBox ID="txtBasePOV" runat="server" Width="75" Text='<%# DataBinder.Eval(Container.DataItem,"BasePOV") & "%" %>' AutoPostBack="true" OnTextChanged="valueChanged"></asp:TextBox>
                         </ItemTemplate>                                 
                     </asp:TemplateField>                                                      
                 </Columns>
                 <HeaderStyle CssClass="colHeader" />                                                     
                </asp:GridView> 
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>              
    </div>         
</asp:Content>
