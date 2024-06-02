Imports System.IO
Imports Microsoft.SqlServer.Dts.Runtime
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports PATMAP.common

Public Class WebForm2
    Inherits System.Web.UI.Page

    Dim SQL_DataSource As String = PATMAP.Global_asax.SQLEngineServer
    Dim SQL_InitialCatalog As String = PATMAP.Global_asax.DBName

    Dim _LocalFileRootPath As String = ConfigurationSettings.AppSettings("LocalFileRootPath").ToString
    Dim _PackagePath As String = "SSIS/" 'ConfigurationSettings.AppSettings("PackagePath").ToString
    Dim _PackageName As String = "TestPackage1" 'ConfigurationSettings.AppSettings("PackageName_Access2SQL").ToString
    Dim _LocalFileSubFolderPath As String = "/Assessment/"
    Dim _Source_File_Extantion As String = "mdb"
    Dim _typeofData As String = "assessment"
    Dim OleDbConnectionExtended As String = ""

    Dim _ISCParcelNumber As String = "ISC_Parcel_Number"
    Dim _parcelID1 As String = "Provincial_Property_Number"
    Dim _parcelID2 As String = "Provincial_Property_NO"
    Dim _parcelID3 As String = "Provinciap_Property_Number"
    Dim _municipalityID_orig As String = "Municipality"
    Dim _municipalityID As String = "Municipality"
    Dim _alternate_parcelID1 As String = "Alternate_Property_Number"
    Dim _alternate_parcelID2 As String = "Alternate_Property_NO"
    Dim _LLD As String = "LLD"
    Dim _civicAddress As String = "Civic_Address"
    Dim _presentUseCodeID As String = "PU_Code"
    Dim _schoolID1 As String = "School_Division_Number"
    Dim _schoolID2 As String = "School_Division_NO"
    Dim _schoolID3 As String = "School_Division"
    Dim _taxClassID_orig As String = "Tax_Class"
    Dim _marketValue As String = "Market_Value_Assessment"
    Dim _taxable1 As String = "Taxable_Assessment"
    Dim _taxable2 As String = "Taxable_Assessement"
    Dim _otherExempt As String = "Other_Exempt"
    Dim _FGIL1 As String = "Federal_GIL"
    Dim _FGIL2 As String = "FEDERAL_GIL"
    Dim _PGIL1 As String = "Provincial_GIL"
    Dim _PGIL2 As String = "PROV_GIL"
    Dim _Section293 As String = "SECTION_293_2e"
    Dim _ByLawExemption As String = "Bylaw_Exemption"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim TypeOfConnection As String = "file"
        Dim Source_Connection_Script As String = "SELECT SessionID,ISCParcelNumber,parcelID,municipalityID_orig,municipalityID,alternate_parcelID,LLD,civicAddress,presentUseCodeID,schoolID,taxClassID_orig,taxClassID,marketValue,taxable,otherExempt,FGIL,PGIL,Section293,ByLawExemption FROM [assessment_SSIS_TMP]"

        SSIS_Package_Bind(TypeOfConnection, Source_Connection_Script)
    End Sub

    Protected Function SSIS_Package_Bind(ByVal TypeOfConnection As String, ByVal Source_Connection_Script As String) As Boolean
        Dim status As Boolean = False
        'Master.errorMsg = ""
        Dim Source_FilePath As String = (_LocalFileRootPath + _LocalFileSubFolderPath)
        Dim Source_FileName As String = "db2.mdb" '(Unic + ("." + _Source_File_Extantion))
        Dim PackageName As String = (Server.MapPath(_PackagePath) + _PackageName)
        Select Case (TypeOfConnection)
            Case "file"
                If (File.Exists((PackageName + ".dtsx"))) Then
                    Dim myPackage As Package
                    Dim app As Application = New Application
                    myPackage = app.LoadPackage((PackageName + ".dtsx"), Nothing)
                    Dim vars As Variables = myPackage.Variables
                    'vars("SQL_DataSource").Value = SQL_DataSource
                    'vars("SQL_InitialCatalog").Value = SQL_InitialCatalog
                    'vars("Source_FilePath").Value = Source_FilePath
                    'vars("Source_FileName").Value = Source_FileName
                    vars("Source_Connection_Script").Value = Source_Connection_Script
                    vars("SQL_UserName").Value = Trim(PATMAP.Global_asax.DBUser)
                    vars("SQL_Password").Value = Trim(PATMAP.Global_asax.DBPassword)

                    Dim result As DTSExecResult = myPackage.Execute
                    Debug.Print(result.ToString())
                    'Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP312") + result.ToString

                    If result.ToString.ToLower.Equals("success") Then
                        status = True
                    Else
                        status = False
                        If myPackage.Errors.Count > 0 Then
                            Debug.Print(myPackage.Errors(0).Description)
                            'Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP312") + myPackage.Errors(0).Description
                        End If
                    End If
                Else
                    'Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP308") + PackageName + PATMAP.common.GetErrorMessage("PATMAP309")
                End If
        End Select
        If Not status Then
            'Dim CommandText As String = ("delete FROM [" _
            '            + (_typeofData + ("_SSIS] where [SessionID] = '" _
            '            + (Unic + "'"))))
            'SqlDbAccess.RunSql(CommandText)
        End If
        Return status
    End Function

End Class