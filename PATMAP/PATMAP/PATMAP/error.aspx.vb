Public Partial Class _error
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim errCode As String
       

        If Not IsNothing(Session("responseCode")) Then
            errCode = Session("responseCode")
            Session.Remove("responseCode")
            lblErrorDesc.Text = common.GetErrorMessage(errCode)
        Else
            If PATMAP.Global_asax.errorCode <> "" Then
                lblErrorDesc.Text = common.GetErrorMessage(PATMAP.Global_asax.errorCode)
            Else
                lblErrorDesc.Text = common.GetErrorMessage("500")
            End If

        End If

    End Sub

    Private Sub _error_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If IsNothing(Session("userID")) Or IsNothing(Session("levelID")) Or Not IsNumeric(Session("levelID")) Then
            Page.MasterPageFile = "NoLogin.master"
        End If
    End Sub
End Class