Imports System.Web.SessionState

Public Class Global_asax
    Inherits HttpApplication

    Public Shared cacheExpiration As Double = 10
    Public Shared pageSize As Double = 20
    Public Shared errorCode As String = ""
    Public Shared satellite As Boolean = System.Configuration.ConfigurationManager.AppSettings("IsSatellite")
    Public Shared SQLEngineServer As String = System.Configuration.ConfigurationManager.AppSettings("SQLEngineServer")
    Public Shared DBName As String = System.Configuration.ConfigurationManager.AppSettings("DBName")
    Public Shared DBUser As String = System.Configuration.ConfigurationManager.AppSettings("DBUser")
    Public Shared DBPassword As String = System.Configuration.ConfigurationManager.AppSettings("DBPassword")
    Public Shared synchronizeUser As String = System.Configuration.ConfigurationManager.AppSettings("synchronizeUser")
    Public Shared machineName As String = System.Configuration.ConfigurationManager.AppSettings("MachineName")
    Public Shared synchronizePassword As String = System.Configuration.ConfigurationManager.AppSettings("SynchronizeUserPassword")
    Public Shared synchronizeDomainName As String = System.Configuration.ConfigurationManager.AppSettings("synchronizeDomainName")
    Public Shared connString As String = "Data Source=" & SQLEngineServer & ";Initial Catalog=" & DBName & ";Persist Security Info=True;User ID=" & DBUser & ";Password=" & DBPassword & ";"
    Public Shared connStringLap As String = "Data Source=" & machineName & ";Initial Catalog=" & DBName & ";Persist Security Info=True;User ID=" & DBUser & ";Password=" & DBPassword & ";"
    Public Shared SMTPserver As String = System.Configuration.ConfigurationManager.AppSettings("SMTPserver")
    Public Shared SMTPuserName As String = System.Configuration.ConfigurationManager.AppSettings("SMTPuserName") '***Inky's Addition: Apr-2010
    Public Shared SMTPpassword As String = System.Configuration.ConfigurationManager.AppSettings("SMTPpassword") '***Inky's Addition: Apr-2010
    Public Shared SMTPport As Integer = System.Configuration.ConfigurationManager.AppSettings("SMTPport")
    Public Shared SystemEmailName As String = System.Configuration.ConfigurationManager.AppSettings("SystemEmailName")
    Public Shared ApproverEmailAddress As String = System.Configuration.ConfigurationManager.AppSettings("ApproverEmailAddress")
    Public Shared ApproverEmailName As String = System.Configuration.ConfigurationManager.AppSettings("ApproverEmailName")
    Public Shared SystemEmailAddress As String = System.Configuration.ConfigurationManager.AppSettings("SystemEmailAddress")
    Public Shared domainUser As String = System.Configuration.ConfigurationManager.AppSettings("domainUser")
    Public Shared domainPassword As String = System.Configuration.ConfigurationManager.AppSettings("domainPassword")
    Public Shared domainName As String = System.Configuration.ConfigurationManager.AppSettings("domainName")
    Public Shared synchronizeFileRootPath As String = System.Configuration.ConfigurationManager.AppSettings("SynchronizeFileRootPath")
    Public Shared synchronizeFolder As String = System.Configuration.ConfigurationManager.AppSettings("SynchronizeFolder")
    Public Shared FileRootPath As String = System.Configuration.ConfigurationManager.AppSettings("FileRootPath")
    Public Shared imageFilePath As String = System.Configuration.ConfigurationManager.AppSettings("imageFilePath")
    Public Shared SQLIntegrationServer As String = System.Configuration.ConfigurationManager.AppSettings("SQLIntegrationServer")
    Public Shared SQLReportingServer As String = System.Configuration.ConfigurationManager.AppSettings("SQLReportingServer")
    Public Shared ReportDataSourceFolder = System.Configuration.ConfigurationManager.AppSettings("ReportDataSourceFolder")
    Public Shared ReportTablesFolder = System.Configuration.ConfigurationManager.AppSettings("ReportTablesFolder")
    Public Shared ReportEdPOVFolder = System.Configuration.ConfigurationManager.AppSettings("ReportEdPOVFolder") 'Inky's addition
    Public Shared ReportK12OGFolder = System.Configuration.ConfigurationManager.AppSettings("ReportK12OGFolder") 'Inky's addition
    Public Shared ReportPMRFolder = System.Configuration.ConfigurationManager.AppSettings("ReportPMRFolder") 'Inky's addition
    Public Shared ReportTaxCreditsFolder = System.Configuration.ConfigurationManager.AppSettings("ReportTaxCreditsFolder") 'Inky's addition
    Public Shared ReportGeneralFolder = System.Configuration.ConfigurationManager.AppSettings("ReportGeneralFolder") 'Inky's addition
    Public Shared ReportLTTGraphsFolder = System.Configuration.ConfigurationManager.AppSettings("ReportLTTGraphsFolder")
    Public Shared ReportLTTTablesPhaseInFolder = System.Configuration.ConfigurationManager.AppSettings("ReportLTTTablesPhaseInFolder")
    Public Shared ReportLTTTablesFolder = System.Configuration.ConfigurationManager.AppSettings("ReportLTTTablesFolder")
    Public Shared ReportGraphsFolder = System.Configuration.ConfigurationManager.AppSettings("ReportGraphsFolder")
    Public Shared ReportSystemFolder = System.Configuration.ConfigurationManager.AppSettings("ReportSystemFolder")
    Public Shared ReportBoundaryTablesFolder = System.Configuration.ConfigurationManager.AppSettings("ReportBoundaryTablesFolder")
    Public Shared ReportBoundaryGraphsFolder = System.Configuration.ConfigurationManager.AppSettings("ReportBoundaryGraphsFolder")
    Public Shared ReportExportFolder = System.Configuration.ConfigurationManager.AppSettings("ReportExportFolder")
    Public Shared PackageName_CopySavedModelsTablesToLaptop = System.Configuration.ConfigurationManager.AppSettings("PackageName_CopySavedModelsTablesToLaptop")
    Public Shared PackageName_CopySavedCompareTablesToLaptop = System.Configuration.ConfigurationManager.AppSettings("PackageName_CopySavedCompareTablesToLaptop")








    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

        ' Fires when application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started

        'sets Session variable for "liveLTTtaxClasses_(userID)" table
        Dim tableCreated As Boolean = False
        HttpContext.Current.Session.Add("liveLTTtableCreated", tableCreated)
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs 
        If InStr(Server.GetLastError.GetBaseException.Message, "file") > 0 Or InStr(Server.GetLastError.GetBaseException.Message, "File") > 0 Then
            errorCode = "404"
        ElseIf InStr(Server.GetLastError.GetBaseException.Message, "forbidden") > 0 Then
            errorCode = "404"
        Else
            errorCode = "500"
        End If

        'ReportViewer sends 'ASP.NET session has expired'
        ' If InStr(Server.GetLastError.StackTrace, "Reporting") = 0 And Not satellite Then
        If InStr(Server.GetLastError.StackTrace, "Reporting") = 0 Then
            'send email 

            Dim Mail As New OpenSmtp.Mail.MailMessage()
            Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPuserName, PATMAP.Global_asax.SMTPpassword) '***Inky's Addition: Apr-2010   '"hh_mte@atriamail.com", "bcltfp77"
            'Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport) '***Inky commented this line out in Apr-2010
            Dim errorMsg As String

            errorMsg = Server.GetLastError.GetBaseException.Message

            If InStr(errorMsg, ">") > 0 Or InStr(errorMsg, "<") > 0 Then
                errorMsg = Server.HtmlEncode(errorMsg)
            End If

            Dim mailMessage As String = ""
            mailMessage = "Message: " & "<br/>" & errorMsg & "<br/><br/>"
            mailMessage += "Stack Trace: " & "<br/>" & Server.GetLastError.StackTrace.ToString() & "<br/><br/>"
            mailMessage += "Host Address: " & "<br/>" & Request.UserHostAddress & "<br/><br/>"
            mailMessage += "User Agent: " & "<br/>" & Request.UserAgent

            'build the email                
            Mail.GetBodyFromFile(Request.MapPath("~/includes/Email.html"))
            Mail.AddImage(Request.MapPath("~/includes/governmentLogoNew.jpg"), "patmap01")
            Mail.HtmlBody = Mail.Body.Replace("governmentLogoNew.jpg", "cid:patmap01")
            Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)
            Mail.Subject = "Re: Application Error"
            Mail.To.Add(New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailAddress))
            Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailName)

            SMTP.SendMail(Mail)
        End If

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)

        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)

        ' Fires when the application ends
    End Sub

End Class