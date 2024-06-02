<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="main.aspx.vb" Inherits="PATMAP.main2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %> 
<%@ Register TagPrefix="patmap" TagName="taxtoolsTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:taxtoolsTabMenu ID="subMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
     <div class="commonContent">  
        <div class="commonHeader">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width:35%">
                        <asp:Label ID="lblSubjYr" runat="server" Text="Subject Year: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjYr" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblSubjMun" runat="server" Text="Subject Municipality: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjMun" runat="server" Text=""></asp:Label>
                    </td>         
                </tr>
            </table>
            <br /> 
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Main</p>
                </div>
            </div>
        </div>     
        <div class="commonForm">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td colspan="3" style="padding-left:10px;padding-bottom:15px; height: 41px;">
                        <asp:UpdatePanel ID="uplSubjMun" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblSelectSubjMun" runat="server" Text="Select Subject Municipality"></asp:Label>&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlSubjMun" runat="server" AutoPostBack="True"> </asp:DropDownList> <asp:ImageButton ID="btnHSubjMun" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                        <hr />
                        <p>Select Classes for Local Tax Tool Modelling</p>
                    </td>
                </tr>                      
                <tr>
                    <td colspan="4">
                        <table runat="server" id="tblTaxClasses" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td valign="top" style="height: 64px">
                                    <div class="groupClass">
                                        <table id="tblRollup1" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 25px" valign="top">
                                                    &nbsp;<asp:CheckBox ID="chkRollup1" runat="server" text="rollup1"></asp:CheckBox></td>
                                                <td valign="top" style="height:25px"><asp:ImageButton ID="btnHAgriculture" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>
                                        <table id="tblTaxC1ass1" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td id="checklistAgriculture" style="height:45px"><asp:CheckBoxList ID="cklTaxClass1" runat="server" ></asp:CheckBoxList></td>
                                                <td valign="top" style="height:45px"><asp:ImageButton ID="btnHTaxClass1" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                      
                                    </div>
                                </td>
                                <td valign="top" style="height: 64px">
                                    <div class="groupClass">
                                        <table id="tblRollup2" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 25px" valign="top">
                                                    &nbsp;<asp:CheckBox ID="chkRollup2" runat="server" text="rollup2"></asp:CheckBox></td>
                                                <td valign="top" style="height: 25px; width: 20px;"><asp:ImageButton ID="btnHResidential" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>
                                        <table id="tblTaxC1ass2" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td id="checklistResidential" style="height: 45px"><asp:CheckBoxList ID="cklTaxClass2" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top" style="height:45px; width: 20px;"><asp:ImageButton ID="btnHTaxClass2" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                                <td valign="top" style="height: 64px;">
                                    <div class="groupClass">
                                        <table id="tblRollup3" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 25px" valign="top">
                                                    &nbsp;<asp:CheckBox ID="chkRollup3" runat="server" text="rollup3"></asp:CheckBox></td>
                                                <td valign="top" style="height: 25px"><asp:ImageButton ID="btnHCommercial" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>
                                        <table id="tblTaxC1ass3" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td id="checklistCommerical"><asp:CheckBoxList ID="cklTaxClass3" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top" style="height:45px"><asp:ImageButton ID="btnHTaxClass3" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                            </tr>
                            <%--<tr>
                                <td valign="top" style="height: 64px;">
                                    <div class="groupClass">
                                        <table id="tblRollup4" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 25px" valign="top">
                                                    &nbsp;<asp:CheckBox ID="chkRollup4" runat="server" text="rollup4"></asp:CheckBox></td>
                                                <td valign="top" style="height: 25px"><asp:ImageButton ID="btnHRollup4" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>
                                        <table id="tblTaxC1ass4" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td id="checklistIndustrial"><asp:CheckBoxList ID="cklTaxClass4" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top" style="height:45px"><asp:ImageButton ID="btnHTaxClass4" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                                <td valign="top" style="height: 64px;">
                                    <div class="groupClass">
                                        <table id="tblRollup5" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 25px" valign="top">
                                                    &nbsp;<asp:CheckBox ID="chkRollup5" runat="server" text="rollup5"></asp:CheckBox></td>
                                                <td valign="top" style="height: 25px"><asp:ImageButton ID="btnHRollup5" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>
                                        <table id="tblTaxC1ass5" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td id="td1"><asp:CheckBoxList ID="cklTaxClass5" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top" style="height:45px"><asp:ImageButton ID="btnHTaxClass5" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td>
                                <td valign="top" style="height: 64px;">
                                    <div class="groupClass">
                                        <table id="tblRollup6" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 25px" valign="top">
                                                    &nbsp;<asp:CheckBox ID="chkRollup6" runat="server" text="rollup6"></asp:CheckBox></td>
                                                <td valign="top" style="height: 25px"><asp:ImageButton ID="btnHRollup6" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>
                                        <table id="tblTaxC1ass6" runat="server" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td id="td2"><asp:CheckBoxList ID="cklTaxClass6" runat="server"></asp:CheckBoxList></td>
                                                <td valign="top" style="height:45px"><asp:ImageButton ID="btnHTaxClass6" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>                                                                       
                                    </div>
                                </td> 
                            </tr>--%>
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
