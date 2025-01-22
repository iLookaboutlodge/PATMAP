Public Partial Class logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        common.deleteLiveLTTtable() ' delete liveLTTtable
        'get session userLoginType first'
        Dim userLoginType As String = Session("userLoginType")
        'if userLoginType is not empty, then sso logout'
        Session.RemoveAll()
        If Not String.IsNullOrEmpty(userLoginType) Then
            'sso logout'
            Dim ssoServer As String = System.Configuration.ConfigurationManager.AppSettings("SSOURL")
            Dim ssoLogoutUrl As String = ssoServer & "/Saml2/Logout"
            Response.Redirect(ssoLogoutUrl)
        End If

    End Sub
End Class