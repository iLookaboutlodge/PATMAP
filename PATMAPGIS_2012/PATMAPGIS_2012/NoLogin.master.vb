
Partial Class NoLogin
    Inherits System.Web.UI.MasterPage

    'ErrorMsg Property
    'Retrieves or Sets text displayed from
    'lblErrorText control
    Public Property errorMsg() As String
        Get
            Return lblErrorText.Text
        End Get
        Set(ByVal value As String)
            lblErrorText.Text = value
        End Set
    End Property

    'helpText Property
    'Retrieves or Sets text displayed from
    'lblHelp control
    Public Property helpText() As String
        Get
            Return lblHelp.Text
        End Get
        Set(ByVal value As String)
            lblHelp.Text = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim currentdate As System.DateTime

        currentdate = Today()
        lblDate.Text = currentdate.ToString("dddd, MMMM d, yyyy")

        Dim helpText As String

        If Not IsPostBack Then
            Try
                'retrieves form field help text
                helpText = common.GetHelpText(Page.AppRelativeVirtualPath, Page)
                lblHelp.Text = helpText
            Catch
                'retrieves error message
                lblErrorText.Text = common.GetErrorMessage(Err.Number, Err)
            End Try
        End If
    End Sub
End Class

