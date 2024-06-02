Imports System.Net
Imports System.Security.Principal
Imports Microsoft.Reporting.WebForms

Public NotInheritable Class reportServerCredentials
    Implements IReportServerCredentials

    Public useMainServer As Boolean
    Shared impersonationContext As System.Security.Principal.WindowsImpersonationContext

    Private Const LOGON32_LOGON_INTERACTIVE As Integer = 2
    Private Const LOGON32_LOGON_NETWORK As Integer = 3
    Private Const LOGON32_PROVIDER_DEFAULT As Integer = 0

    Declare Function LogonUserA Lib "advapi32.dll" (ByVal lpszUsername As String, _
                            ByVal lpszDomain As String, _
                            ByVal lpszPassword As String, _
                            ByVal dwLogonType As Integer, _
                            ByVal dwLogonProvider As Integer, _
                            ByRef phToken As IntPtr) As Integer

    Declare Auto Function DuplicateToken Lib "advapi32.dll" ( _
                            ByVal ExistingTokenHandle As IntPtr, _
                            ByVal ImpersonationLevel As Integer, _
                            ByRef DuplicateTokenHandle As IntPtr) As Integer

    Declare Auto Function RevertToSelf Lib "advapi32.dll" () As Long
    Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Long

    Public Function GetFormsCredentials(ByRef authCookie As System.Net.Cookie, ByRef userName As String, ByRef password As String, ByRef authority As String) As Boolean Implements Microsoft.Reporting.WebForms.IReportServerCredentials.GetFormsCredentials
        authCookie = Nothing
        userName = Nothing
        password = Nothing
        authority = Nothing

        'Not using form credentials
        Return False
    End Function

    Public ReadOnly Property ImpersonationUser() As System.Security.Principal.WindowsIdentity Implements Microsoft.Reporting.WebForms.IReportServerCredentials.ImpersonationUser
        Get
            'Use creditentials of PATMAP.Global_asax.domainUser.  

            Dim tempWindowsIdentity As System.Security.Principal.WindowsIdentity
            Dim token As IntPtr = IntPtr.Zero
            Dim tokenDuplicate As IntPtr = IntPtr.Zero

            tempWindowsIdentity = Nothing

            If RevertToSelf() Then
                If LogonUserA(PATMAP.Global_asax.domainUser, PATMAP.Global_asax.domainName, PATMAP.Global_asax.domainPassword, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, token) <> 0 Then
                    If DuplicateToken(token, 2, tokenDuplicate) <> 0 Then
                        tempWindowsIdentity = New System.Security.Principal.WindowsIdentity(tokenDuplicate)
                        impersonationContext = tempWindowsIdentity.Impersonate()
                    End If
                End If
            End If

            If Not tokenDuplicate.Equals(IntPtr.Zero) Then
                CloseHandle(tokenDuplicate)
            End If

            If Not token.Equals(IntPtr.Zero) Then
                CloseHandle(token)
            End If

            Return tempWindowsIdentity
        End Get
    End Property

    Shared Sub undoImpersonation()
        impersonationContext.Undo()
    End Sub

    Public ReadOnly Property NetworkCredentials() As System.Net.ICredentials Implements Microsoft.Reporting.WebForms.IReportServerCredentials.NetworkCredentials
		Get
			If Not useMainServer Then
				Return New System.Net.NetworkCredential(PATMAP.Global_asax.domainUser, PATMAP.Global_asax.domainPassword, PATMAP.Global_asax.domainName)
			Else
				Return New System.Net.NetworkCredential(PATMAP.Global_asax.synchronizeUser, PATMAP.Global_asax.synchronizePassword, PATMAP.Global_asax.synchronizeDomainName)
			End If
		End Get
    End Property
End Class
