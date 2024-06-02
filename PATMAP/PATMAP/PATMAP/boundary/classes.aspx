<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="classes.aspx.vb" Inherits="PATMAP.classes1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Register TagPrefix="patmap" TagName="boundaryTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:boundaryTabMenu ID="subMenu" runat="server" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">        
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Classes"></asp:Label></p>
                </div>
            </div>
        </div>     
        <div class="commonForm">           
           <table cellpadding="0" cellspacing="0" border="0" width="100%">                              
                <tr>
                    <td>
                         <br />  
                        <table cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td valign="top">
                                    <div class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 45px"><asp:CheckBoxList ID="cklTaxClass1" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top" style="height: 45px"><asp:ImageButton ID="btnHTaxClass1" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                      
                                    </div>
                                </td>
                                <td valign="top">
                                    <div class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList ID="cklTaxClass2" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass2" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                                <td valign="top">
                                    <div class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList ID="cklTaxClass3" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass3" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                                <td valign="top">
                                    <div class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 45px"><asp:CheckBoxList ID="cklTaxClass4" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top" style="height: 45px"><asp:ImageButton ID="btnHTaxClass4" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                      
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 64px" valign="top">
                                    <div class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList ID="cklTaxClass5" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass5" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                      
                                    </div>
                                </td>
                                <td style="height: 64px" valign="top">
                                    <div class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList ID="cklTaxClass6" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass6" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                     
                                    </div>
                                </td>
                                <td style="height: 64px" valign="top">
                                    <div class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList ID="cklTaxClass7" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass7" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                                <td style="height: 64px" valign="top">
                                    <div class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList ID="cklTaxClass8" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass8" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                         
                                    </div>
                                </td>
                                <td colspan="2" style="height: 64px">
                                    &nbsp;
                                </td>                                
                            </tr>         
                             <tr>
                                <td style="height:64px" valign="top">
                                    <div id="divClass9" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList Visible="false" ID="cklTaxClass9" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass9" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                      
                                    </div>
                                </td>
                                <td style="height:64px" valign="top">
                                    <div id="divClass10" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList Visible="false" ID="cklTaxClass10" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass10" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                     
                                    </div>
                                </td>
                                <td style="height: 64px;" valign="top">
                                    <div id="divClass11" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList Visible="false" ID="cklTaxClass11" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass11" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                                <td style="height:64px" valign="top">
                                    <div id="divClass12" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList Visible="false" ID="cklTaxClass12" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass12" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                         
                                    </div>
                                </td>
                                <td colspan="2" style="height: 64px">
                                    &nbsp;
                                </td>                                
                            </tr>                      
                        </table>
                    </td>
                </tr> 
                <tr>
                    <td class="btnsBottom" colspan="4"><asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" /> <asp:ImageButton ID="btnReset" runat="server" ImageUrl="~/images/btnReset.gif" /></td>
                </tr>
           </table>
        </div> 
                              
    </div>         
</asp:Content>



