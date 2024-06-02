<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="editdataset.aspx.vb" Inherits="PATMAP.editdataset" %>
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
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Data Set Info"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblDataSource" runat="server" Text="Data Source"></asp:Label></td>
                    <td class="field" colspan="2"><asp:Label ID="txtDataSource" runat="server" Text=""></asp:Label></td>
                </tr> 
                <tr>
                    <td class="label"><asp:Label ID="lblDataSetName" runat="server" Text="Data Set Name"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtDataSetName" runat="server" CssClass="txtLong"></asp:TextBox> <asp:ImageButton ID="btnHDataSetName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr> 
                <tr>
                    <td class="label"><asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label></td>
                    <td class="field" colspan="2"><asp:Label ID="txtStatus" runat="server" Text=""></asp:Label></td>
                </tr>               
                 <tr>
                    <td class="label">
                        <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox>
                    </td>
                    <td valign="top"><asp:ImageButton ID="btnHNotes" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>  
                <tr>
                    <td class="label"><asp:Label ID="lblTaxYearModels" runat="server" Text="Used by Tax Year Models"></asp:Label></td>
                    <td class="field" colspan="2">
                         <asp:GridView ID="grdTaxYearModels" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle">
                             <Columns>                                                     
                                 <asp:BoundField HeaderText="Tax Year Model" DataField="taxYearModelName"/>   
                                 <asp:BoundField HeaderText="Year" DataField="year"/>                              
                             </Columns>
                             <HeaderStyle CssClass="colHeader"/>
                             <AlternatingRowStyle CssClass="alertnateRow" />
                             <PagerSettings Position="Top" />
                         </asp:GridView> 
                    </td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblAttachFile" runat="server" Text="Attach File"></asp:Label></td>
                    <td class="field"><asp:FileUpload ID="fpAttachFile" runat="server" /> <asp:ImageButton ID="btnHAttachFile" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton> <asp:ImageButton ID="btnAttach" runat="server" ImageUrl="~/images/btnAttach.gif" /> </td>                    
                </tr>
                <tr>                
                    <td class="label">&nbsp;</td>
                    <td class="field">
                         <asp:GridView ID="grdFiles" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" DataKeyNames="fileID,filename">
                             <Columns>                                                           
                                 <asp:HyperLinkField HeaderText="Attached Files" DataTextField="filename" />                                
                                  <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteFile">
                                     <ItemStyle CssClass="colDelete" />
                                 </asp:ButtonField>    
                             </Columns>
                             <HeaderStyle CssClass="colHeader" />
                             <AlternatingRowStyle CssClass="alertnateRow" />
                             <PagerSettings Position="Top" />
                         </asp:GridView> 
                    </td>
                </tr>
            </table>
        </div>              
    </div>         
</asp:Content>

