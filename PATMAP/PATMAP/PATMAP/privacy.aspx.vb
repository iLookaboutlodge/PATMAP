Public Partial Class privacy
    Inherits System.Web.UI.Page

    Private Sub privacy_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not IsNothing(Session("userID")) Then
            Page.MasterPageFile = "MasterPage.master"
        End If
    End Sub
End Class