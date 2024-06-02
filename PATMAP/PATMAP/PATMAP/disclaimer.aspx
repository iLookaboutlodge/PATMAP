<%@ Page Language="vb" MasterPageFile="~/NoLogin.master" AutoEventWireup="false" CodeBehind="disclaimer.aspx.vb" Inherits="PATMAP.disclaimer" %>
<%@ MasterType VirtualPath="~/NoLogin.master" %>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">   
    <script language="javascript" type="text/javascript" src="js/general.js"></script>              
    <div class="commonContent">
       <div id="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Disclaimer</p>
                </div>
            </div>
        </div>
         <div class="commonForm">
            <div class="pHead">Lorem Ipsum</div>
            <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Nunc sed est ut ipsum viverra scelerisque. Mauris eu erat. Aliquam pulvinar urna. Ut ac arcu. Etiam suscipit tempor ligula. Aenean et augue. Vivamus tristique. Aliquam mollis tincidunt odio. Curabitur pharetra tellus nec sapien. Integer erat erat, tincidunt sit amet, gravida quis, malesuada sed, eros. Vivamus venenatis leo sit amet mi. Pellentesque vel leo eget ipsum mollis sagittis. Curabitur aliquam justo et augue porta egestas. Fusce consequat ullamcorper enim. Integer id mi. Fusce fermentum. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos hymenaeos. Donec eu magna. Phasellus pharetra, neque blandit accumsan imperdiet, urna velit adipiscing enim, quis pretium dolor lectus quis velit. </p>

            <p>Mauris vitae sem eu pede mattis placerat. Nullam in orci ut purus egestas interdum. Phasellus tincidunt diam vitae augue. Maecenas cursus sapien a est mattis malesuada. Pellentesque nibh neque, elementum egestas, dictum ut, condimentum non, ligula. Fusce faucibus. Donec sit amet sem non turpis fringilla laoreet. Suspendisse potenti. Donec laoreet rutrum augue. Quisque erat lorem, imperdiet ac, sodales vitae, blandit sed, lectus. Vestibulum sed eros. Nullam ut nisl. Vestibulum pellentesque nibh non dolor. Proin ac nulla at lacus suscipit euismod. Aenean feugiat ultricies augue. Etiam sollicitudin accumsan quam. Quisque vestibulum nulla non ante. Maecenas sit amet purus sit amet purus consectetuer accumsan. Phasellus sodales. </p>

            <p>In dignissim pellentesque tellus. Nullam velit. Donec eget orci. Curabitur a massa sed felis posuere ultricies. Phasellus ligula mi, eleifend vitae, pretium quis, vulputate quis, metus. Morbi vel nisl. Aenean malesuada. Suspendisse a odio nec neque consequat imperdiet. Nulla quis quam sed nibh molestie bibendum. Nulla vestibulum dictum dui. Fusce velit sapien, dapibus eget, hendrerit eu, tincidunt vulputate, mauris. Donec lorem. Mauris tortor lorem, pulvinar in, interdum eu, accumsan ac, pede. Aliquam augue. Donec at odio. Suspendisse mattis nunc vel sem. Pellentesque mollis dignissim ligula. Donec sodales. </p>
            
            <div id="print"><div id="pringImg"><a href="" onclick="return printPage();"><asp:Image ID="btnPrint" ImageUrl="~/images/btnPrint.gif" runat="server" /></a></div><div id="printLink"><a href="" onclick="return printPage();">Print this page</a></div></div>
         </div>
    </div>    
</asp:Content>

