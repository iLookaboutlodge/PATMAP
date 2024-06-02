<%@ Page Language="vb" MasterPageFile="MasterPage.master" AutoEventWireup="false" CodeBehind="synchronize.aspx.vb" Inherits="PATMAP.synchronize" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<asp:Content ContentPlaceHolderID="mainContent" runat="server"> 
    <script language="javascript" type="text/javascript" src="js/general.js"></script>      
    <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Synchronize</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <br/>           
            <asp:Panel runat="server" ID="panelForm" Visible="false">
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDestination" runat="server" Text="Destination"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtDestination" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHDestination" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHUsername" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>   
                <tr>
                    <td class="label">
                        <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="txtNormal" TextMode="Password"></asp:TextBox> <asp:ImageButton ID="btnHPassword" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr> 
                <tr>                
                    <td class="btnsBottom" colspan="4">
                        <%--<asp:ImageButton ID="btnSubmitToLaptop" runat="server" ImageUrl="~/images/btnSyncLaptop.gif" /> 
                        <asp:ImageButton ID="btnSubmitToServer" runat="server" ImageUrl="~/images/btnSyncServer.gif" />
                        <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif"/>--%>
                    </td>
                </tr>                                       
            </table>
            </asp:Panel> 
            <asp:Panel runat="server" ID="panelBtns"> 
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>                
                    <td class="btnsBottom" colspan="4">
                        <asp:ImageButton ID="btnSubmitToLaptop" runat="server" ImageUrl="~/images/btnSyncLaptop.gif" /> 
                        <asp:ImageButton ID="btnSubmitToServer" runat="server" ImageUrl="~/images/btnSyncServer.gif" />
                    </td>
                </tr>
            </table>
            </asp:Panel>    
            <asp:Panel runat="server" ID="panelResult" Visible="false"> 
                <asp:Label runat ="server" ID="lblSynProgressBar" Text="Synchronization is completed successfully"></asp:Label>              
            </asp:Panel>        
        </div>
    </div>                
</asp:Content>
