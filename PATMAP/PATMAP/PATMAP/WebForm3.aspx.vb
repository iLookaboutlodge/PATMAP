Public Class WebForm3
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim helpText As String = String.Empty

        helpText = GetHelpTextTest(Page.AppRelativeVirtualPath, Page)

        lblHelp.Text = ""
        lblHelp.Text = helpText
    End Sub

    Public Shared Function GetHelpTextTest(ByRef pageAddr As String, ByRef currentPage As System.Web.UI.Page) As String
        Dim btnHelp As System.Web.UI.WebControls.ImageButton
        Dim counter As Integer
        Dim controlID, helpDiv, helpText As String
        Dim iframeHeight As Integer
        Dim helpTextContent As String

        helpText = ""

        'controlID = "ctl00$mainContent$btnHUserLevel"
        'helpDiv = "ctl00$mainContent$helpHUserLevel"

        controlID = "btnHUserLevel"
        helpDiv = "helpHUserLevel"

        'finds help button within the page
        btnHelp = currentPage.FindControl(controlID)

        If Not IsNothing(btnHelp) Then
            btnHelp.TabIndex = -1D
            btnHelp.Visible = True
            'assign mouseover and mouseout javascript function to button
            btnHelp.Attributes.Add("onmouseover", "javascript: showObj('" & helpDiv & "', 'show',this);")
            btnHelp.Attributes.Add("onmouseout", "javascript: hideObj('" & helpDiv & "');")

            iframeHeight = 25
            helpTextContent = "testing TExt testing TExttesting TExttesting TExttesting TExttesting TExttesting TExt"

            If helpTextContent.Length > 50 Then
                iframeHeight = 50
            End If

            'creates hidden div containers for all help text.                
            'helpText &= "<div class='fieldhelp' style='visibility:hidden;' id='" & helpDiv & "'> <iframe src='' frameborder='0' scrolling='no' style='filter:alpha(opacity=0);z-index:-1;position:absolute;width:290px;height:" & iframeHeight & "px;top:0;left:0;border:1px solid black;'></iframe>" & helpTextContent & "</div>"
            helpText &= "<div class='fieldhelp' style='visibility:hidden;' id='" & helpDiv & "'> <iframe src='' frameborder='0' scrolling='no' style='filter:alpha(opacity=0);z-index:-1;position:absolute;width:290px;height:" & iframeHeight & "px;top:0;left:0;'></iframe>" & helpTextContent & "</div>"
        End If

        Return helpText
    End Function

End Class