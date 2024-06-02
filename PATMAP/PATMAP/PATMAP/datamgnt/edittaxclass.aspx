<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    Codebehind="edittaxclass.aspx.vb" Inherits="PATMAP.edittaxclass" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="dataTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">
    <patmap:dataTabMenu ID="subMenu" runat="server"></patmap:dataTabMenu>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <div class="commonContent">
        <div class="commonHeader">
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">
                        <asp:Label ID="lblTitle" runat="server" Text="Edit Tax Class"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop">
                <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" />
                <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div>
            <br />
            <div class="clear">
            </div>
            <p>
                Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblTaxClass" runat="server" Text="Tax Class"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtTaxClass" runat="server" CssClass="txtNormal"></asp:TextBox>
                        <asp:ImageButton ID="btnHTaxClass" runat="server" ImageUrl="~/images/btnHelp.gif"
                            CssClass="btnHelp" Visible="False"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblCode" runat="server" Text="Code"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtCode" runat="server" Width="50" MaxLength="2"></asp:TextBox>
                        <asp:ImageButton ID="btnHCode" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                            CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                    </td>
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox></td>
                                <td valign="top">
                                    <asp:ImageButton ID="btnHDescription" runat="server" ImageUrl="~/images/btnHelp.gif"
                                        Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="label" style="height: 45px">
                        <asp:Label ID="lblClassType" runat="server" Text="Type"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" style="width: 30%; height: 45px;">
                        <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem>Main</asp:ListItem>
                            <asp:ListItem>Subclass of</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="field" style="padding-left: 5px; height: 45px;">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td valign="top">
                                    <asp:DropDownList ID="ddlMainClasses" runat="server">
                                    </asp:DropDownList></td>
                                <td valign="top">
                                    <asp:ImageButton ID="btnHClassType" runat="server" ImageUrl="~/images/btnHelp.gif"
                                        Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="label" style="height: 40px">
                        <asp:Label ID="lblLTTRollup" runat="server" Text="LTT Rollup Classes"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" colspan="2" style="height: 40px">
                        <asp:Label ID="lblLTTselection" runat="server" Text="Apply to Local Tax Tools as Subclass of &nbsp;"></asp:Label>&nbsp;
                        <asp:DropDownList ID="ddlLTTRollup" runat="server">
                        </asp:DropDownList>&nbsp;<asp:ImageButton ID="btnHLTTClass" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label" style="height: 40px">
                        <asp:Label ID="lblPEMRRollup" runat="server" Text="PEMR Rollup Classes"></asp:Label>
                        <span class="requiredField">*</span>
                    </td>
                    <td class="field" colspan="2" style="height: 40px">
                        <asp:Label ID="lblPEMRselection" runat="server" Text="Apply to PEMR as Subclass of &nbsp;"></asp:Label>&nbsp;
                        <asp:DropDownList ID="ddlPEMRRollup" runat="server" DataSourceID="sdsPEMRRollup"
                            DataValueField="mainTaxClassID" DataTextField="mainTaxClass">
                        </asp:DropDownList>&nbsp;
                        <asp:SqlDataSource ID="sdsPEMRRollup" runat="server" SelectCommand="SELECT mainTaxClassID, mainTaxClass FROM PEMRMainTaxClasses WHERE active = 1 ORDER BY sort"
                            ConnectionString='<%$ ConnectionStrings:PATMAPConnection %>'></asp:SqlDataSource>
                        <asp:ImageButton ID="btnHPEMRClass" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblPropCode" runat="server" Text="Present Use Code"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:UpdatePanel ID="uplHelp" runat="server">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAllPropCode" runat="server" Text="Present Use Code"></asp:Label>
                                            <asp:ImageButton ID="btnHAllPropCode" runat="server" ImageUrl="~/images/btnHelp.gif"
                                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                        <td style="width: 20%;">
                                            &nbsp;</td>
                                        <td>
                                            <asp:Label ID="lblClassProp" runat="server" Text="Present Use Code (Tax Class)"></asp:Label>
                                            <asp:ImageButton ID="btnHClassProp" runat="server" ImageUrl="~/images/btnHelp.gif"
                                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:ListBox ID="lstPropCodeLeft" runat="server"></asp:ListBox></td>
                                        <td>
                                            <asp:ImageButton ID="btnAddMember" runat="server" ImageUrl="~/images/btnMoveRight.gif" /><br />
                                            <br />
                                            <asp:ImageButton ID="btnRemoveMember" runat="server" ImageUrl="~/images/btnMoveLeft.gif" />
                                        </td>
                                        <td>
                                            <asp:ListBox ID="lstPropCodeRight" runat="server"></asp:ListBox></td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddMember" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnRemoveMember" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDefault" runat="server" Text="Default?"></asp:Label></td>
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td valign="top">
                                    <asp:RadioButtonList ID="rblDefault" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td valign="top" style="width: 20px">
                                    <asp:ImageButton ID="btnHDefault" runat="server" ImageUrl="~/images/btnHelp.gif"
                                        Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblActive" runat="server" Text="Active?"></asp:Label></td>
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td valign="top">
                                    <asp:RadioButtonList ID="rblActive" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td valign="top">
                                    <asp:ImageButton ID="btnHActive" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                        CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                    </td>
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td valign="top">
                                    <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox></td>
                                <td valign="top">
                                    <asp:ImageButton ID="btnHNotes" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                        CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
