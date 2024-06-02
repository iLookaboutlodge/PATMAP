Imports OSGeo.MapGuide

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim sessionId As String = Create_Get_Session()
            Dim result As Boolean = IsSessionExists(sessionId)
            Label1.Text = result.ToString()
        Catch ex As Exception
            Label1.Text = ex.Message
        End Try
    End Sub

    Public Function IsSessionExists(ByVal sessionId As String) As Boolean
        Try
            If Not String.IsNullOrEmpty(sessionId) Then
                Dim cred As MgUserInformation = New MgUserInformation()
                cred.SetMgSessionId(sessionId)
                cred.SetMgUsernamePassword("Administrator", "admin")
                Dim site As MgSiteConnection = New MgSiteConnection()
                cred.SetLocale("en")
                cred.SetClientAgent("Ajax Viewer")
                site.Open(cred)
                Dim tmpidsessionId As String = site.GetSite().GetCurrentSession()
                If Not String.IsNullOrEmpty(tmpidsessionId) Then
                    Return True
                End If
            End If
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    Public Function Create_Get_Session() As String
        Dim sessionId As String = Nothing

        Try

            MgLocalizer.SetLocalizedFilesPath(ConfigurationManager.AppSettings("AutodeskWebServerPath") + "localized\\")

            MapGuideApi.MgInitializeWebTier(ConfigurationManager.AppSettings("AutodeskWebServerPath") + "webconfig.ini")
            'C:\Program Files\Autodesk\Autodesk Infrastructure Web Server Extension 2017\www\
            ''MapGuideApi.MgInitializeWebTier(Request.ServerVariables["APPL_PHYSICAL_PATH"] + "../webconfig.ini");


            Dim createSession As Boolean = True
            If System.Web.HttpContext.Current.Session("Session") IsNot Nothing Then
                sessionId = System.Web.HttpContext.Current.Session("Session").ToString()
            Else
                Dim cred As MgUserInformation = New MgUserInformation()
                cred.SetMgUsernamePassword("Administrator", "admin")

                Dim site As MgSiteConnection = New MgSiteConnection()
                cred.SetLocale("en")
                cred.SetClientAgent("Ajax Viewer")
                If (False) Then
                    cred.SetMgSessionId(sessionId)
                    createSession = False
                End If
                site.Open(cred)
                If (createSession) Then
                    Dim site1 As MgSite = site.GetSite()
                    sessionId = site1.CreateSession()
                End If
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return sessionId
    End Function

End Class