Imports System.ComponentModel

Partial Public Class header
    Inherits System.Web.UI.UserControl

    <Browsable(True), _
    DefaultValue(""), _
    Description("The title of the header.")> _
    Public Property Title() As String
        Get
            Return lblTitle.Text
        End Get
        Set(ByVal value As String)
            lblTitle.Text = value
        End Set
    End Property

    Public ReadOnly Property ScenarioHelp() As ImageButton
        Get
            Return ibScenarioHelp
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim levelID As Integer

        If Not IsPostBack Then
            levelID = Session("levelID")

            'Presentation users have to re-save current scenario using a different scenario name.
            If levelID = 3 Then
                ibSave.ImageUrl = "~/images/btnSaveAs.gif"
            End If

            'Populate fields.
            common.GetModelNames(txtBaseTaxYrModel.Text, txtSubjectTaxYrModel.Text, Session("userID"), txtScenarioName.Text)
        End If
    End Sub

    'Update the scenario name.
    Protected Sub txtScenarioName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScenarioName.TextChanged
        Dim errCode As String

        Try
            errCode = common.UpdateScenarioName(Session("userID"), Session("assessmentTaxModelID"), txtScenarioName.Text)

            If errCode <> "" Then
                CType(Page.Master, MasterPage).errorMsg = common.GetErrorMessage(errCode)
            End If
        Catch
            CType(Page.Master, MasterPage).errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ibSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibSave.Click
        Dim errCode As String

        Try
            If Trim(txtScenarioName.Text) = "" Then
                CType(Page.Master, MasterPage).errorMsg = PATMAP.common.GetErrorMessage("PATMAP99")
                Exit Sub
            End If

            errCode = common.UpdateScenarioName(Session("userID"), Session("assessmentTaxModelID"), txtScenarioName.Text)

            If errCode <> "" Then
                CType(Page.Master, MasterPage).errorMsg = common.GetErrorMessage(errCode)
                Exit Sub
            End If

            common.saveLiveModelTables(Session("userID"))
        Catch
            CType(Page.Master, MasterPage).errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("start.aspx")
    End Sub
End Class