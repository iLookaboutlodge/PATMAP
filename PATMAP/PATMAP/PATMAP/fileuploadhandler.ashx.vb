Imports System.Web
Imports System.Web.Services
Imports System.IO

Public Class fileuploadhandler
    Implements System.Web.IHttpHandler, IRequiresSessionState

    Dim _LocalFileSubFolderPath As String = "/Assessment/"
    Dim _Source_File_Extantion As String = "mdb"
    Dim _typeofData As String = "assessment"

    Dim _LocalFileRootPath As String = ConfigurationSettings.AppSettings("LocalFileRootPath").ToString

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/plain"
        Try
            Dim fpFile As HttpPostedFile = context.Request.Files(0)
            FileUpload(context, fpFile)

            context.Response.StatusCode = 200
            context.Response.Write("File Uploaded Successfully!")
        Catch ex As Exception
            context.Response.StatusCode = 417
            context.Response.Write(PATMAP.common.GetErrorMessage("PATMAP301") + ex.ToString + "'")
        End Try

    End Sub

    Protected Function FileUpload(ByVal context As HttpContext, ByVal fpFile As HttpPostedFile) As Boolean
        Dim file_Correct As Boolean = True
        Dim file_ShortName As String = ""
        Dim file_PathName As String = ""
        Dim file_Extension As String = ""
        Dim file_ContentType As String = ""
        Dim file_Size As Decimal = 0
        Try
            file_ShortName = fpFile.FileName
            'file_PathName = fpFile.PostedFile.FileName
            file_Extension = fpFile.FileName.Substring((fpFile.FileName.LastIndexOf(".") + 1))
            'file_ContentType = fpFile.PostedFile.ContentType
            'file_Size = Convert.ToDecimal(fpFile.PostedFile.ContentLength)

            file_ContentType = fpFile.ContentType
            file_Size = Convert.ToDecimal(fpFile.ContentLength)
        Catch ex As Exception
            file_Correct = False
            'Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP301") + ex.ToString + "'"
            Throw New Exception(PATMAP.common.GetErrorMessage("PATMAP301") + ex.ToString + "'")
        End Try
        'If Not file_Extension.ToLower.Equals(_Source_File_Extantion) Then
        If Not (file_Extension.ToLower.Equals("mdb") OrElse file_Extension.ToLower.Equals("accdb")) Then
            file_Correct = False
            'Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP302") + _Source_File_Extantion + "'"
            Throw New Exception(PATMAP.common.GetErrorMessage("PATMAP302") + _Source_File_Extantion + "'")
        End If
        If file_Correct Then
            Dim Unic As String = DateTime.Today.Year.ToString _
            + "_" + DateTime.Today.Month.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
            + "_" + DateTime.Today.Day.ToString.PadLeft(2, Microsoft.VisualBasic.ChrW(48)) _
            + "_" + Guid.NewGuid.ToString()

            context.Session("UnicAssessmentFile") = Unic

            Dim RepositoryUnicFileName As String = (_LocalFileRootPath _
                 + (_LocalFileSubFolderPath _
                 + (Unic + ("." + _Source_File_Extantion))))
            fpFile.SaveAs(RepositoryUnicFileName)
            file_Correct = FileExist(RepositoryUnicFileName)
        End If
        Return file_Correct
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Protected Function FileExist(ByVal fileName As String) As Boolean
        Dim status As Boolean = False
        Dim dt0 As DateTime = DateTime.Now
        Dim dt1 As DateTime
        Dim dt2 As DateTime
        Dim dt3 As DateTime = dt0.AddSeconds(90)
        'TimeOUT time
FileNotClosed:
        dt0 = DateTime.Now
        dt1 = DateTime.Now
        dt2 = dt1.AddSeconds(10)
        If Not File.Exists(fileName) Then
            If (dt3 > dt0) Then

                While (dt2 > dt1)
                    dt1 = DateTime.Now

                End While
                GoTo FileNotClosed

            Else
                'Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP307")
                Throw New Exception(PATMAP.common.GetErrorMessage("PATMAP307"))
            End If
        Else
            status = True
        End If
        Return status
    End Function

End Class