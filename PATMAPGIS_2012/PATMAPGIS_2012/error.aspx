<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="error.aspx.vb" Inherits="PATMAP._error" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">  
    <script language="javascript" type="text/javascript" src="js/general.js"></script>               
    <div class="commonContent">
       <div id="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Error</p>
                </div>
            </div>
        </div>
         <div class="commonForm">            
            <div><asp:Label ID="lblErrorDesc" runat="server" Text="Label"></asp:Label></div>        
         </div>
    </div>    
</asp:Content>