<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="editpotash.aspx.vb" Inherits="PATMAP.editpotash" %>
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
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit Potash"></asp:Label></p>
                </div>
            </div>
        </div>
            <div class="commonForm">
                <div class="btnsTop">
                    <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif"
                        OnClick="btnCancel_Click" /></div>
                <br />
                <div class="clear">
                </div>
               <%-- <p>
                    Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>--%>
                <table cellpadding="0" cellspacing="0" border="0">
                    <%--<tr>
                        <td colspan="2">
                            <asp:Label ID="lb_Message" runat="server" Width="100%" ForeColor="#669966"></asp:Label></td>
                    </tr>--%>
                    <tr id="tr_DSN" runat="server">
                        <td class="label">
                            <asp:Label ID="lblDSN" runat="server" Text="Data Set Name"></asp:Label>
                            <span class="requiredField">*</span></td>
                        <td class="field" colspan="2">
                            <asp:DropDownList ID="ddlDSN" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDSN_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr id="tr_MunicipalityType" runat="server">
                        <td class="label">
                            <asp:Label ID="lblMunicipalityType" runat="server" Text="Municipality Type"></asp:Label></td>
                        <td class="field">
                            <asp:DropDownList ID="ddlMunicipalityType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMunicipalityType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHMunicipalityType" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblpotashAreaID" runat="server" Text="Potash AreaID"></asp:Label></td>
                        <td class="field">
                            <asp:TextBox ID="txtpotashAreaID" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHpotashAreaID" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidatortxtpotashAreaID" ControlToValidate="txtpotashAreaID"
                                Type="Integer" MinimumValue="0" MaximumValue="32767" Text="Only Int type less then 32767"
                                runat="server" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblMunicipalityID" runat="server" Text="Municipality ID"></asp:Label></td>
                        <td class="field">
                            <asp:DropDownList ID="ddlMunicipalityID" runat="server">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHMunicipalityID" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr id="tr_areaInSquareMiles" runat="server">
                        <td class="label">
                            <asp:Label ID="lblareaInSquareMiles" runat="server" Text="areaInSquareMiles"></asp:Label></td>
                        <td class="field">
                            <asp:TextBox ID="txtareaInSquareMiles" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHareaInSquareMiles" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidatorareaInSquareMiles" ControlToValidate="txtareaInSquareMiles"
                                Type="Double" MinimumValue="0" MaximumValue="99999999999999999999" Text="Only Float type"
                                runat="server" />
                        </td>
                    </tr>
                    <tr id="tr_statutoryDiscountPercentage" runat="server">
                        <td class="label">
                            <asp:Label ID="lblstatutoryDiscountPercentage" runat="server" Text="statutoryDiscountPercentage"></asp:Label></td>
                        <td class="field">
                            <asp:TextBox ID="txtstatutoryDiscountPercentage" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHstatutoryDiscountPercentage" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidatorstatutoryDiscountPercentage" ControlToValidate="txtstatutoryDiscountPercentage"
                                Type="Double" MinimumValue="0" MaximumValue="99999999999999999999" Text="Only Float type"
                                runat="server" />
                        </td>
                    </tr>
                    <tr id="tr_millRateFactor" runat="server">
                        <td class="label">
                            <asp:Label ID="lblmillRateFactor" runat="server" Text="millRateFactor"></asp:Label></td>
                        <td class="field">
                            <asp:TextBox ID="txtmillRateFactor" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHmillRateFactor" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidatormillRateFactor" ControlToValidate="txtmillRateFactor"
                                Type="Double" MinimumValue="0" MaximumValue="99999999999999999999" Text="Only Float type"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lbltotalPoints" runat="server" Text="totalPoints"></asp:Label></td>
                        <td class="field">
                            <asp:TextBox ID="txttotalPoints" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHtotalPoints" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidatortotalPoints" ControlToValidate="txttotalPoints"
                                Type="Double" MinimumValue="0" MaximumValue="99999999999999999999" Text="Only Float type"
                                runat="server" />
                        </td>
                    </tr>
                    <tr id="tr_boardAdjustments" runat="server">
                        <td class="label">
                            <asp:Label ID="lblboardAdjustments" runat="server" Text="boardAdjustments"></asp:Label></td>
                        <td class="field">
                            <asp:TextBox ID="txtboardAdjustments" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHboardAdjustments" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidatorboardAdjustments" ControlToValidate="txtboardAdjustments"
                                Type="Double" MinimumValue="0" MaximumValue="99999999999999999999" Text="Only Float type"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lbltotalGrant" runat="server" Text="totalGrant"></asp:Label></td>
                        <td class="field">
                            <asp:TextBox ID="txttotalGrant" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHtotalGrant" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidatortotalGrant" ControlToValidate="txttotalGrant"
                                Type="Double" MinimumValue="0" MaximumValue="99999999999999999999" Text="Only Float type"
                                runat="server" />
                        </td>
                    </tr>
                    
                </table>
            </div>
    </div>         
</asp:Content>

