Public Partial Class logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        common.deleteLiveLTTtable() ' delete liveLTTtable
        Session.RemoveAll()
    End Sub
End Class