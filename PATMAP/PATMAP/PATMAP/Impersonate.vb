Public Class Impersonate

    '----------------------- IMPERSONATION CODE ----------------------------

    Private Const LOGON32_LOGON_INTERACTIVE As Integer = 2
    Private Const LOGON32_LOGON_NETWORK As Integer = 3
    Private Const LOGON32_PROVIDER_DEFAULT As Integer = 0

    Shared impersonationContext As System.Security.Principal.WindowsImpersonationContext

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

    Shared Function impersonateValidUser(ByVal userName As String, ByVal domain As String, ByVal password As String, ByRef ErrorMsg As String) As Boolean

        Dim tempWindowsIdentity As System.Security.Principal.WindowsIdentity
        Dim token As IntPtr = IntPtr.Zero
        Dim tokenDuplicate As IntPtr = IntPtr.Zero

        impersonateValidUser = False

        If RevertToSelf() Then
            If LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, token) <> 0 Then
                If DuplicateToken(token, 2, tokenDuplicate) <> 0 Then
                    tempWindowsIdentity = New System.Security.Principal.WindowsIdentity(tokenDuplicate)
                    impersonationContext = tempWindowsIdentity.Impersonate()
                    If Not impersonationContext Is Nothing Then
                        impersonateValidUser = True
                    End If
                Else
                    ErrorMsg = "Impersonation - Unable to Duplicate Token"
                End If
            Else
                ErrorMsg = "Impersonation - Login Failed"
            End If
        Else
            ErrorMsg = "Impersonation - Unable to Revert to Self"
        End If
        If Not tokenDuplicate.Equals(IntPtr.Zero) Then
            CloseHandle(tokenDuplicate)
        End If
        If Not token.Equals(IntPtr.Zero) Then
            CloseHandle(token)
        End If
    End Function

    Shared Sub undoImpersonation()
        If Not IsNothing(impersonationContext) Then
            impersonationContext.Undo()
        End If
    End Sub


End Class
