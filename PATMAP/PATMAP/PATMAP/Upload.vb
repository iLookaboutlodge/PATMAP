Imports System.Web

Public Class Upload
    Implements IHttpHandler

    'Public Sub ProcessRequest( _
    '         ByVal context As System.Web.HttpContext) _
    '         Implements _
    '         System.Web.IHttpHandler.ProcessRequest

    'End Sub

    Public Sub New()
        MyBase.New()

    End Sub

    'Public ReadOnly Property IsReusable() As Boolean
    '    Get
    '        Return True
    '    End Get
    'End Property

    Public ReadOnly Property IsReusable() _
      As Boolean _
      Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property


	Public Sub ProcessRequest(ByVal context As HttpContext) _
	 Implements _
		System.Web.IHttpHandler.ProcessRequest

		Dim tempFile As String = "C:/PATMAP root/_T_E_M_P.TMP"
		If (Not (context.Request.QueryString("FilePath")) Is Nothing) Then
			tempFile = context.Request.QueryString("FilePath")
		End If
		If (context.Request.Files.Count > 0) Then
			Dim j As Integer = 0
			Do While (j < context.Request.Files.Count)
				' get the current file
				Dim uploadFile As HttpPostedFile = context.Request.Files(j)
				' if there was a file uploded
				If (uploadFile.ContentLength > 0) Then
					' save the file to the upload directory
					uploadFile.SaveAs(tempFile)
					'uploadFile.SaveAs("C:\aaa\db3.mdb")
				End If
				j = (j + 1)
			Loop
		End If
		' Used as a fix for a bug in mac flash player that makes the 
		' onComplete event not fire
		HttpContext.Current.Response.Write(" ")
	End Sub

End Class
