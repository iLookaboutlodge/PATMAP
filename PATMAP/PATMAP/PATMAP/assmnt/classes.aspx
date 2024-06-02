<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="classes.aspx.vb" Inherits="PATMAP.classes" %>
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
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Classes"></asp:Label></p>
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
                        <table cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td valign="top">
                                    <div id="divClass1" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 45px;"><asp:CheckBoxList Visible="false" ID="cklTaxClass1" runat="server"></asp:CheckBoxList></td><!--width: 83px;-->
                                                <td valign="top" style="height: 45px"><asp:ImageButton ID="btnHTaxClass1" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                      
                                    </div>
                                </td>
                                <td valign="top">
                                    <div id="divClass2" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList Visible="false" ID="cklTaxClass2" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass2" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                                <td valign="top">
                               
                                    <div id="divClass3" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 45px"><asp:CheckBoxList Visible="false" ID="cklTaxClass3" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top" style="height: 45px"><asp:ImageButton ID="btnHTaxClass3" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                                <td valign="top">
                                    <div id="divClass4" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height:45px"><asp:CheckBoxList Visible="false" ID="cklTaxClass4" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top" style="height: 45px"><asp:ImageButton ID="btnHTaxClass4" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                      
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:64px" valign="top">
                                    <div id="divClass5" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList Visible="false" ID="cklTaxClass5" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass5" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                      
                                    </div>
                                </td>
                                <td style="height:64px" valign="top">
                                    <div id="divClass6" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList Visible="false" ID="cklTaxClass6" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass6" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                     
                                    </div>
                                </td>
                                <td style="height: 64px;" valign="top">
                                    <div id="divClass7" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList Visible="false" ID="cklTaxClass7" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top"><asp:ImageButton ID="btnHTaxClass7" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                                <td style="height:64px" valign="top">
                                    <div id="divClass8" class="groupClass">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td><asp:CheckBoxList Visible="false" ID="cklTaxClass8" runat="server"></asp:CheckBoxList></td>
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


