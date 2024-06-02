Imports System
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text


'/// <summary>
'/// SymmCrypto is a wrapper of System.Security.Cryptography.SymmetricAlgorithm classes
'/// and simplifies the interface. It supports customized SymmetricAlgorithm as well.
'/// </summary>
Public Class SymmCrypto

    '/// <remarks>
    '/// Supported .Net intrinsic SymmetricAlgorithm classes.
    '/// </remarks>
    Public Enum SymmProvEnum As Integer
        DES
        RC2
        Rijndael
    End Enum

    Private mobjCryptoService As SymmetricAlgorithm
    Private Const Key As String = "!#%&0^$@"

    '/// <remarks>
    '/// Constructor for using an intrinsic .Net SymmetricAlgorithm class.
    '/// </remarks>
    Public Sub New(ByVal NetSelected As SymmProvEnum)
        Select Case NetSelected
            Case SymmProvEnum.DES
                mobjCryptoService = New DESCryptoServiceProvider()
                Exit Select
            Case SymmProvEnum.RC2
                mobjCryptoService = New RC2CryptoServiceProvider()
                Exit Select
            Case SymmProvEnum.Rijndael
                mobjCryptoService = New RijndaelManaged()
                Exit Select
        End Select
    End Sub

    '/// <remarks>
    '/// Constructor for using a customized SymmetricAlgorithm class.
    '/// </remarks>
    Public Sub New(ByVal ServiceProvider As SymmetricAlgorithm)
        mobjCryptoService = ServiceProvider
    End Sub

    '/// <remarks>
    '/// Depending on the legal key size limitations of a specific CryptoService provider
    '/// and length of the private key provided, padding the secret key with space character
    '/// to meet the legal size of the algorithm.
    '/// </remarks>
    Public Function GetLegalKey() As Byte()
        Dim sTemp As String

        If (mobjCryptoService.LegalKeySizes.Length > 0) Then
            Dim lessSize As Integer = 0
            Dim moreSize As Integer = mobjCryptoService.LegalKeySizes(0).MinSize
            '// key sizes are in bits
            While (Key.Length * 8 > moreSize)
                lessSize = moreSize
                moreSize += mobjCryptoService.LegalKeySizes(0).SkipSize
            End While

            sTemp = Key.PadRight(moreSize / 8, " ")
        Else
            sTemp = Key
        End If

        'convert the secret key to byte array
        GetLegalKey = ASCIIEncoding.ASCII.GetBytes(sTemp)
    End Function

    Public Function Encrypting(ByVal Source As String) As String
        Dim bytIn() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(Source)

        '// create a MemoryStream so that the process can be done without I/O files
        Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream()

        Dim bytKey() As Byte = GetLegalKey()

        '// set the private key
        mobjCryptoService.Key = bytKey
        mobjCryptoService.IV = bytKey

        '// create an Encryptor from the Provider Service instance
        Dim encrypto As ICryptoTransform = mobjCryptoService.CreateEncryptor()

        '// create Crypto Stream that transforms a stream using the encryption
        Dim cs As CryptoStream = New CryptoStream(ms, encrypto, CryptoStreamMode.Write)

        '// write out encrypted content into MemoryStream
        cs.Write(bytIn, 0, bytIn.Length)
        cs.FlushFinalBlock()

        '// get the output and trim the '\0' bytes
        Dim bytOut() As Byte = ms.GetBuffer()
        Dim i As Integer = 0

        For i = 0 To bytOut.Length - 1 Step 1
            If bytOut(i) = 0 Then
                Exit For
            End If
        Next

        '// convert into Base64 so that the result can be used in xml
        Encrypting = System.Convert.ToBase64String(bytOut, 0, i).Replace("'", "''")
    End Function


    Public Function Decrypting(ByVal Source As String) As String
        'convert from Base64 to binary
        Dim bytIn() As Byte = System.Convert.FromBase64String(Source)

        'create a MemoryStream with the input
        Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(bytIn, 0, bytIn.Length)

        Dim bytKey() As Byte = GetLegalKey()

        'set the private key
        mobjCryptoService.Key = bytKey
        mobjCryptoService.IV = bytKey


        'create a Decryptor from the Provider Service instance
        Dim encrypto As ICryptoTransform = mobjCryptoService.CreateDecryptor()

        'create Crypto Stream that transforms a stream using the decryption
        Dim cs As CryptoStream = New CryptoStream(ms, encrypto, CryptoStreamMode.Read)

        'read out the result from the Crypto Stream
        Dim sr As System.IO.StreamReader = New System.IO.StreamReader(cs)

        Decrypting = sr.ReadToEnd()
    End Function
End Class
