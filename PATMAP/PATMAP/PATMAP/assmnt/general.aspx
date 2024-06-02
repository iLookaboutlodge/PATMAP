<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="general.aspx.vb" Inherits="PATMAP.general" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="assmntTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:assmntTabMenu ID="subMenu" runat="server" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">
        <table cellpadding="0" cellspacing="0" border="0" width="92%">
            <tr>
                <td colspan="3" style="padding-left:10px;padding-bottom:15px;">
                    <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
                </td>
            </tr>
            <tr>                                
                <td class="label" style="padding-left:10px;width:100px;"><asp:Label ID="lblScenarioName" runat="server" Text="Scenario Name:"></asp:Label><span class="requiredField">*</span></td>
                <td class="field">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                 <asp:UpdatePanel ID="uplName" runat="server">
                                    <ContentTemplate><asp:TextBox ID="txtScenarioName" runat="server" CssClass="txtLong" OnTextChanged="changeName" AutoPostBack="True"></asp:TextBox></ContentTemplate>
                                 </asp:UpdatePanel>
                            </td>
                            <td><asp:ImageButton ID="btnHScenarioName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                        </tr>
                    </table>
                </td>         
                <td align="right" style="width:58px;"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /></td>
            </tr>
        </table>
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">General</p>
                </div>
            </div>
        </div>     
        <div class="commonForm">           
           <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td class="labelBase"><asp:Label ID="lblBase" runat="server" Text="Base Tax Year Model:" CssClass="labelTaxModel"></asp:Label></td>
                    <td style="width: 30%;padding-left:10px;"><asp:Label ID="txtBaseTaxYrModel" runat="server" Text=""></asp:Label></td>
                    <td class="labelSubject"><asp:Label ID="lblSubject" runat="server" Text="Subject Tax Year Model:" CssClass="labelTaxModel"></asp:Label></td>
                    <td style="width: 30%;padding-left:10px;"><asp:Label ID="txtSubjectTaxYrModel" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td class="label"><asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label></td>
                                <td class="field"><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox></td>
                                <td valign="top"><asp:ImageButton ID="btnHDescription" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                            <tr>
                                <td class="label"><asp:Label ID="lblBaseModel" runat="server" Text="Base Tax Year Model"></asp:Label> <span class="requiredField">*</span></td>
                                <td class="field" colspan="2"><asp:DropDownList ID="ddlBaseModel" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHBaseModel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                            <tr>
                                <td class="label"><asp:Label ID="lblSubjectModel" runat="server" Text="Subject Tax Year Model"></asp:Label> <span class="requiredField">*</span></td>
                                <td class="field" colspan="2"><asp:DropDownList ID="ddlSubjectModel" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHSubjectModel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
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
                                <td class="label"><asp:Label ID="lblAttachFile" runat="server" Text="Attach File"></asp:Label></td>
                                <td class="field" colspan="2"><asp:FileUpload ID="fpAttachFile" runat="server" /> <asp:ImageButton ID="btnHAttachFile" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton> <asp:ImageButton ID="btnAttach" runat="server" ImageUrl="~/images/btnAttach.gif" /></td>                    
                            </tr>
                            <tr>                
                                <td class="label">&nbsp;</td>
                                <td class="field" colspan="2">
                                    <asp:GridView ID="grdFiles" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" DataKeyNames="fileID,filename">
                                         <Columns>                                                                                          
                                             <asp:HyperLinkField HeaderText="Attached Files" DataTextField="filename" />                                
                                              <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteFile" >
                                                 <ItemStyle CssClass="colDelete" />
                                             </asp:ButtonField>    
                                         </Columns>
                                         <HeaderStyle CssClass="colHeader" />
                                         <AlternatingRowStyle CssClass="alertnateRow" />
                                         <PagerSettings Position="Top" />
                                     </asp:GridView> 
                                </td>
                            </tr> 
                            <tr>
                                <td class="label"><asp:Label ID="lblAudience" runat="server" Text="Audience"></asp:Label></td>
                                <td class="field" colspan="2">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="cblAudience" runat="server">
                                                    <asp:ListItem>Presentation</asp:ListItem>
                                                    <asp:ListItem>External Users</asp:ListItem>
                                                </asp:CheckBoxList></td>
                                            <td class="btnHelp"><asp:ImageButton ID="btnHAudience" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>               
                <tr>
                    <td class="btnsBottom" colspan="4"><asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" /> <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif" /></td>
                </tr>
           </table>
        </div> 
                              
    </div>         
</asp:Content>
