Imports System.IO
Imports System.Security.Cryptography
Imports System.Data.SqlClient

Public Class common

    'Private Shared blnChange As Boolean

    'Public Shared Function GetErrorMessage(ByVal errorCode As String, Optional ByVal Err As Microsoft.VisualBasic.ErrObject = Nothing) As String
    '    'make connection to database
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString

    '    'lookup error code 
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    query.CommandText = "Select errorText from errorCodes where errorCode = '" & errorCode & "'"
    '    Dim dr As SqlClient.SqlDataReader
    '    con.Open()
    '    dr = query.ExecuteReader
    '    If dr.Read() Then
    '        'found error code
    '        GetErrorMessage = HttpContext.Current.Server.HtmlDecode(dr.GetValue(0))
    '    Else
    '        'Syntax error found 
    '        If Not IsNothing(Err) Then

    '            If Err.GetException.Message <> "Thread was being aborted." Then

    '                'send email 
    '                Dim Mail As New OpenSmtp.Mail.MailMessage()
    '                'Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPport) '***Inky commented this line out in Apr-2010
    '                'Dim SMTP As New OpenSmtp.Mail.Smtp(PATMAP.Global_asax.SMTPserver, PATMAP.Global_asax.SMTPuserName, PATMAP.Global_asax.SMTPpassword) '***Inky's Addition: Apr-2010


    '                'build the email                
    '                Mail.GetBodyFromFile(HttpContext.Current.Request.MapPath("~/includes/Email.html"))

    '                Dim mailMessage As String
    '                mailMessage = "Message: " & "<br/>" & Err.GetException.Message & "<br/><br/>"
    '                mailMessage += "Stack Trace: " & "<br/>" & Err.GetException.StackTrace.ToString() & "<br/><br/>"
    '                mailMessage += "Host Address: " & "<br/>" & HttpContext.Current.Request.UserHostAddress & "<br/><br/>"
    '                mailMessage += "User Agent: " & "<br/>" & HttpContext.Current.Request.UserAgent

    '                Mail.AddImage(HttpContext.Current.Request.MapPath("~/includes/governmentLogoNew.jpg"), "patmap01")
    '                Mail.HtmlBody = Mail.Body.Replace("governmentLogoNew.jpg", "cid:patmap01")
    '                Mail.HtmlBody = Mail.Body.Replace("{CONTENT}", mailMessage)
    '                Mail.Subject = "Re: Page Error"
    '                Mail.To.Add(New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailAddress))
    '                Mail.From = New OpenSmtp.Mail.EmailAddress(PATMAP.Global_asax.SystemEmailAddress, PATMAP.Global_asax.SystemEmailName)

    '                'SMTP.SendMail(Mail)

    '                HttpContext.Current.Session.Add("responseCode", "500")
    '                HttpContext.Current.Response.Redirect("~/error.aspx", False)

    '            End If

    '            GetErrorMessage = ""
    '        Else
    '            'No error code found 
    '            GetErrorMessage = "Error code, " & errorCode & ", not found."
    '        End If

    '    End If
    '    con.Close()
    'End Function

    'Public Shared Function GeneratePassword() As String
    '    Dim randomBytes As Byte() = New Byte(3) {}
    '    Dim rng As RNGCryptoServiceProvider = New RNGCryptoServiceProvider()
    '    Dim seed As Integer
    '    Dim PasswordCharacters As String = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()"
    '    Dim PasswordLength As Integer = 8
    '    Dim NewPassword As String = ""
    '    Dim RandomNumber As Random
    '    Dim i As Integer = 0
    '    Dim NewChar As Char

    '    'use random number generator to seed the randomizer
    '    rng.GetBytes(randomBytes)
    '    seed = ((randomBytes(0) And &H7F) << 24 Or _
    '                            randomBytes(1) << 16 Or _
    '                            randomBytes(2) << 8 Or _
    '                            randomBytes(3))
    '    RandomNumber = New Random(seed)

    '    NewChar = PasswordCharacters.Substring(CType(Math.Round((PasswordCharacters.Length - 1) * RandomNumber.NextDouble(), 0), Integer), 1)

    '    While NewPassword.Length < PasswordLength
    '        'makes sure that the same character won't appear in the password
    '        While InStr(NewPassword, NewChar) > 0
    '            NewChar = PasswordCharacters.Substring(CType(Math.Round((PasswordCharacters.Length - 1) * RandomNumber.NextDouble(), 0), Integer), 1)
    '        End While
    '        NewPassword += NewChar
    '    End While

    '    'makes sure that the password has 1 number, and 1 special character
    '    While Not ValidatePassword(NewPassword)
    '        NewPassword = GeneratePassword()
    '    End While

    '    GeneratePassword = NewPassword
    'End Function

    ''New implementation of help functionality that works with user controls. Recommend using this going forward as the original 
    ''help functionality is not implemented well. This functionality could easily be changed to become a simple tooltip if styling 
    ''is not important, instead of having to dynamically create a div tag.
    'Public Shared Function GetHelpText(ByRef pageAddr As String, ByVal helpButtons As ArrayList) As String
    '    Dim con As New SqlConnection
    '    Dim query As New SqlDataAdapter
    '    Dim pageAddrParam As SqlParameter
    '    Dim queryResult As New Data.DataTable
    '    Dim i As Integer = 0
    '    Dim row As DataRow()
    '    Dim helpDiv As String = String.Empty
    '    Dim helpTextContent As String = String.Empty
    '    Dim iframeHeight As Integer = 0
    '    Dim helpText As String = String.Empty

    '    'Get help text.
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    query.SelectCommand = New SqlCommand
    '    query.SelectCommand.Connection = con
    '    query.SelectCommand.CommandText = "SELECT controls.controlID, controls.controlNameID, helpControls.helpText " & _
    '        "FROM helpControls INNER JOIN controls ON controls.controlID = helpControls.controlID " & _
    '        "INNER JOIN screenNames screens on screens.screenNameID = controls.screenID " & _
    '        "WHERE screens.pageAddress = @pageAddr AND helpControls.helpText IS NOT NULL"
    '    pageAddrParam = New SqlParameter("@pageAddr", SqlDbType.NVarChar)
    '    pageAddrParam.Value = Mid(pageAddr, 2)
    '    query.SelectCommand.Parameters.Add(pageAddrParam)
    '    query.Fill(queryResult)

    '    'Iterate over all help buttons.
    '    For Each helpButton As ImageButton In helpButtons
    '        row = queryResult.Select("controlID = " & helpButton.CommandArgument)
    '        helpDiv = queryResult.Rows(i).Item("controlNameID") & "Help"
    '        helpButton.TabIndex = -1D
    '        helpButton.Visible = True
    '        helpButton.Attributes.Add("onmouseover", "javascript: showObj('" & helpDiv & "', 'show',this);")
    '        helpButton.Attributes.Add("onmouseout", "javascript: hideObj('" & helpDiv & "');")
    '        helpTextContent = HttpContext.Current.Server.HtmlDecode(queryResult.Rows(i).Item("helpText"))

    '        If helpTextContent.Length > 50 Then
    '            iframeHeight = 50
    '        Else
    '            iframeHeight = 25
    '        End If

    '        'creates hidden div containers for all help text.                
    '        helpText &= "<div class='fieldhelp' style='visibility:hidden;' id='" & helpDiv & "'> <iframe src='' frameborder='0' scrolling='no' style='filter:alpha(opacity=0);z-index:-1;position:absolute;width:290px;height:" & iframeHeight & "px;top:0;left:0;border:1px solid black;'></iframe>" & helpTextContent & "</div>"
    '        i = i + 1
    '    Next

    '    Return helpText
    'End Function

    ''GetHelpText() Method
    ''Accepts string and page object as parameters
    ''Returns a string
    ''Makes corresponding help buttons visible for all fields
    ''that have help text and creates a container for help text
    ''which will be displayed on help buttons' mouseover events
    'Public Shared Function GetHelpText(ByRef pageAddr As String, ByRef currentPage As System.Web.UI.Page) As String
    '    'make connection to database
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString

    '    'lookup form field help text for current page
    '    Dim query As New SqlClient.SqlDataAdapter
    '    query.SelectCommand = New SqlClient.SqlCommand
    '    query.SelectCommand.Connection = con
    '    query.SelectCommand.CommandText = "select controls.controlNameID, helpControls.helpText from helpControls helpControls inner join controls controls on controls.controlID = helpControls.controlID inner join screenNames screens on screens.screenNameID = controls.screenID  where screens.pageAddress = '" & Mid(pageAddr, 2) & "' and (helpControls.helpText is not null)"

    '    Dim queryResult As New Data.DataTable
    '    query.Fill(queryResult)

    '    Dim btnHelp As System.Web.UI.WebControls.ImageButton
    '    Dim counter As Integer
    '    Dim controlID, helpDiv, helpText As String
    '    Dim iframeHeight As Integer
    '    Dim helpTextContent As String

    '    helpText = ""

    '    'iterates through the list of controls
    '    For counter = 0 To queryResult.Rows.Count - 1
    '        'Donna - This is not a reliable method of finding a control on a page and also doesn't allow for user controls.
    '        controlID = "ctl00$mainContent$btnH" & queryResult.Rows(counter).Item("controlNameID")
    '        helpDiv = "ctl00$mainContent$help" & queryResult.Rows(counter).Item("controlNameID")
    '        'finds help button within the page
    '        btnHelp = currentPage.FindControl(controlID)

    '        If Not IsNothing(btnHelp) Then
    '            btnHelp.TabIndex = -1D
    '            btnHelp.Visible = True
    '            'assign mouseover and mouseout javascript function to button
    '            btnHelp.Attributes.Add("onmouseover", "javascript: showObj('" & helpDiv & "', 'show',this);")
    '            btnHelp.Attributes.Add("onmouseout", "javascript: hideObj('" & helpDiv & "');")

    '            iframeHeight = 25
    '            helpTextContent = HttpContext.Current.Server.HtmlDecode(queryResult.Rows(counter).Item("helpText"))

    '            If helpTextContent.Length > 50 Then
    '                iframeHeight = 50
    '            End If

    '            'creates hidden div containers for all help text.                
    '            helpText &= "<div class='fieldhelp' style='visibility:hidden;' id='" & helpDiv & "'> <iframe src='' frameborder='0' scrolling='no' style='filter:alpha(opacity=0);z-index:-1;position:absolute;width:290px;height:" & iframeHeight & "px;top:0;left:0;border:1px solid black;'></iframe>" & helpTextContent & "</div>"
    '        End If
    '    Next

    '    Return helpText
    'End Function

    ''ChangeBreadcrumb() Method
    ''Accepts an object and SiteMapResolveEventArgs as parameters
    ''Returns a SiteMapNode
    ''Act as handler event for SiteMap.SiteMapResolve
    ''Changes page title in breadcrumb trail
    'Private Shared Function ChangeBreadcrumb(ByVal sender As Object, ByVal e As SiteMapResolveEventArgs) As SiteMapNode
    '    Dim currentNode As SiteMapNode = SiteMap.CurrentNode.Clone(True)

    '    If blnChange Then
    '        If currentNode.Title <> "Classes" Then
    '            currentNode.Title = Replace(currentNode.Title, "Edit", "Add")
    '        Else
    '            currentNode.ParentNode.Title = "Graphs"
    '        End If
    '    End If

    '    Return currentNode
    'End Function

    ''ChangeTitle()
    ''Accepts an integer and a label web control as parameters
    ''Return a boolean; true if page title was changed and false if it didn't
    ''Change page's title in the header and breadcrumb
    'Public Shared Function ChangeTitle(ByVal id As String, ByRef lblTitle As System.Web.UI.WebControls.Label) As Boolean
    '    AddHandler SiteMap.SiteMapResolve, AddressOf ChangeBreadcrumb

    '    'If there's no record to edit
    '    If (id = "0") Or (id = "none") Or IsNothing(id) Or (id = "-999A") Or (id = "-1") Then
    '        blnChange = True
    '        lblTitle.Text = Replace(lblTitle.Text, "Edit", "Add")
    '    Else
    '        blnChange = False
    '    End If

    '    Return blnChange

    'End Function

    ''ChangeTitle()
    ''Accepts no parameters
    ''Removes SiteMap.SiteMapResolve handler
    'Public Shared Sub UndoChange()
    '    RemoveHandler SiteMap.SiteMapResolve, AddressOf ChangeBreadcrumb
    'End Sub

    ''SortGrid()
    ''Accepts two strings as parameters
    ''Returns a string value
    ''Determines how grid will be sorted
    'Public Shared Function SortGrid(ByVal colName As String, ByVal curSort As String) As String
    '    Dim sort() As String
    '    Dim sortDirection As String

    '    sort = curSort.Split(" ")

    '    'If grid view is sorted for the first time
    '    If sort.Length = 1 Then
    '        sortDirection = colName & " desc"
    '    Else
    '        'If grid is being sorted by the same column
    '        If sort(0) = colName Then
    '            'If grid is sorted ascending
    '            If sort(1) = "asc" Then
    '                sort(1) = "desc"
    '            Else
    '                sort(1) = "asc"
    '            End If
    '        Else
    '            sort(0) = colName
    '            sort(1) = "desc"
    '        End If

    '        sortDirection = sort(0) & " " & sort(1)

    '    End If

    '    Return sortDirection

    'End Function

    ''ConfirmDel()
    ''Accepts GridViewRowEventArgs, Integer and String as parameters
    ''Attaches confirm script to gridview button
    'Public Shared Sub ConfirmDel(ByRef e As System.Web.UI.WebControls.GridViewRowEventArgs, ByVal colNo As Integer, ByVal data As String)
    '    Dim btnDel As System.Web.UI.WebControls.LinkButton

    '    'if current row is a datarow type
    '    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '        btnDel = CType(e.Row.Cells.Item(colNo).Controls.Item(0), LinkButton)
    '        data = Replace(data, "'", "\'")
    '        'when delete button is clicked, a prompt will ask for confirmation of deletion
    '        btnDel.Attributes.Add("onClick", "javascript: return confirmPrompt('Are you sure you want to delete " & data & "?')")
    '    End If
    'End Sub


    'Public Shared Sub calculateTaxYearModel(ByVal isBase As Integer, ByRef taxYearModelID As Integer, Optional ByRef userID As Integer = 0, Optional ByRef taxYearModelIDSubject As Integer = 0)
    '    'variables to hold ID's
    '    Dim assessmentID As Integer
    '    Dim assessmentIDSubject As Integer
    '    Dim millRateSurveyID As Integer
    '    Dim uniformMunicipalMillRateID As Integer
    '    Dim uniformPotashMillRateID As Integer
    '    Dim uniformK12MillRateID As Integer
    '    Dim uniformSchoolMillRateID As Integer
    '    Dim POVID As Integer
    '    Dim taxCreditID As Integer
    '    Dim K12ID As Integer
    '    Dim PMRID As Integer
    '    Dim potashID As Integer

    '    'make connection to database
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    'get ID values if not working on live tables
    '    If isBase = 0 Then
    '        'query.CommandText = "select assessmentID, PotashID from taxYearModelDescription where taxYearModelID = " & taxYearModelID.ToString
    '        query.CommandText = "select subjassessmentID, subjPotashID from liveAssessmentTaxModel where userID = " & userID.ToString
    '        Dim dr As SqlClient.SqlDataReader
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        assessmentID = dr.GetValue(0)
    '        potashID = dr.GetValue(1)
    '        dr.Close()

    '        'query.CommandText = "select millRateSurveyID from taxYearModelDescription where taxYearModelID = (select liveassessmenttaxmodel.baseTaxYearModelID from liveAssessmentTaxModel where userID = " & userID.ToString & ")"
    '        query.CommandText = "select subjmillRateSurveyID from liveAssessmentTaxModel where userID = " & userID.ToString
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        millRateSurveyID = dr.GetValue(0)
    '        dr.Close()

    '        query.CommandText = "select SubjectK12ID from liveassessmentTaxModel where userID = " & userID.ToString
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        K12ID = dr.GetValue(0)
    '        dr.Close()

    '    Else
    '        query.CommandText = "select assessmentID, millRateSurveyID, uniformMunicipalMillRateID, uniformPotashMillRateID, uniformK12MillRateID, uniformSchoolMillRateID, POVID, taxCreditID, K12ID, PMRID, PotashID from taxYearModelDescription where taxYearModelID = " & taxYearModelID.ToString
    '        Dim dr As SqlClient.SqlDataReader
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        assessmentID = ToInteger(dr.GetValue(0))
    '        millRateSurveyID = ToInteger(dr.GetValue(1))
    '        uniformMunicipalMillRateID = ToInteger(dr.GetValue(2))
    '        uniformPotashMillRateID = ToInteger(dr.GetValue(3))
    '        uniformK12MillRateID = ToInteger(dr.GetValue(4))
    '        uniformSchoolMillRateID = ToInteger(dr.GetValue(5))
    '        POVID = ToInteger(dr.GetValue(6))
    '        taxCreditID = ToInteger(dr.GetValue(7))
    '        K12ID = ToInteger(dr.GetValue(8))
    '        PMRID = ToInteger(dr.GetValue(9))
    '        potashID = ToInteger(dr.GetValue(10))
    '        dr.Close()

    '        query.CommandText = "select assessmentID, PotashID from taxYearModelDescription where taxYearModelID = " & taxYearModelIDSubject.ToString
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        assessmentIDSubject = dr.GetValue(0)
    '        dr.Close()
    '    End If

    '    'calculate assessment base and subject tables
    '    'If isBase = 1 Then
    '    'query.CommandText = "exec compareBaseandSubject " & userID.ToString & "," & assessmentIDSubject.ToString & "," & assessmentID.ToString & "," & isBase.ToString
    '    'query.ExecuteNonQuery()
    '    'End If

    '    'calculate uniform municipal mill Rates
    '    query.CommandText = "exec calcPotashShift " & taxYearModelID & "," & userID.ToString & "," & potashID.ToString & "," & assessmentID.ToString & "," & POVID.ToString & "," & uniformMunicipalMillRateID.ToString & "," & uniformPotashMillRateID.ToString & "," & millRateSurveyID.ToString & "," & isBase.ToString
    '    query.ExecuteNonQuery()

    '    'calculate uniform municipal mill Rates
    '    query.CommandText = "exec calcUniformMunicipalMillRateID " & taxYearModelID & "," & userID.ToString & "," & assessmentID.ToString & "," & POVID.ToString & "," & millRateSurveyID.ToString & "," & uniformMunicipalMillRateID.ToString & "," & uniformPotashMillRateID & "," & isBase.ToString
    '    query.ExecuteNonQuery()

    '    'calculate uniform K12 mill Rates 
    '    '** Stored procedure changed: Oct-03-2008. Isnull function added to prevent attempt to insert NULL values into "UniformK12MillRate" column of "liveUniformK12MillRate" tbl as this column cannot contain NULLS. See notes in calcUniformK12MillRateID stored procedure
    '    'Donna -- Removed this.
    '    'query.CommandText = "exec calcUniformK12MillRateID " & taxYearModelID & "," & userID.ToString & "," & assessmentID.ToString & "," & POVID.ToString & "," & K12ID.ToString & "," & uniformK12MillRateID.ToString & "," & isBase.ToString
    '    'query.ExecuteNonQuery()

    '    'calculate uniform school mill Rates
    '    'Donna -- Removed this.
    '    'query.CommandText = "exec calcUniformSchoolMillRateID " & taxYearModelID & "," & userID.ToString & "," & assessmentID.ToString & "," & POVID.ToString & "," & K12ID.ToString & "," & uniformSchoolMillRateID.ToString & "," & isBase.ToString
    '    'query.ExecuteNonQuery()

    '    'calculate tax year model
    '    query.CommandText = "exec calcTaxYearModel " & userID.ToString & "," & taxYearModelID.ToString & "," & assessmentID.ToString & "," & taxCreditID.ToString & "," & POVID.ToString & "," & potashID.ToString & "," & isBase.ToString
    '    query.ExecuteNonQuery()

    '    'Donna - Split assessments by tiers.
    '    query.CommandText = "exec calcPEMRModel " & userID.ToString & ", " & taxYearModelID.ToString & ", " & isBase.ToString
    '    query.ExecuteNonQuery()

    '    If (isBase = 0) Then
    '        HttpContext.Current.Session.Add("calcDone_" & userID, "True")
    '    End If

    '    'clean up
    '    con.Close()
    'End Sub

    'Public Shared Sub calcRevenueNeutralByTotalLevy(ByVal userID As Integer, ByVal taxYearModelID As Integer)
    '    Dim con As New SqlConnection
    '    Dim query As New SqlCommand

    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    query.CommandText = "exec calcRevenueNeutralByTotalLevy " & userID & ", " & taxYearModelID
    '    query.ExecuteNonQuery()

    '    con.Close()
    'End Sub

    'Public Shared Sub calcRevenueNeutralByClassLevy(ByVal userID As Integer, ByVal taxYearModelID As Integer)
    '    Dim con As New SqlConnection
    '    Dim query As New SqlCommand

    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    query.CommandText = "exec calcRevenueNeutralByClassLevy " & userID & ", " & taxYearModelID
    '    query.ExecuteNonQuery()

    '    con.Close()
    'End Sub

    'Public Shared Sub calcAssessmentSummary(ByVal userID As Integer, ByVal load As Integer)
    '    'make connection to database
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    'calculate assessment summary
    '    query.CommandText = "exec calcAssessmentSummary " & userID & ", " & load & "," & HttpContext.Current.Session("levelID").ToString
    '    query.ExecuteNonQuery()

    '    con.Close()
    'End Sub

    'Public Shared Sub calcAssessmentSummary_A(ByVal userID As Integer, ByVal load As Integer)
    '    'make connection to database
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    'calculate assessment summary
    '    query.CommandText = "exec calcAssessmentSummary_A " & userID & ", " & load & "," & HttpContext.Current.Session("levelID").ToString
    '    query.ExecuteNonQuery()

    '    con.Close()
    'End Sub

    'Public Shared Sub calcAssessmentSummary_B(ByVal userID As Integer, ByVal load As Integer)
    '    'make connection to database
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    'calculate assessment summary
    '    query.CommandText = "exec calcAssessmentSummary_B " & userID & ", " & load & "," & HttpContext.Current.Session("levelID").ToString
    '    query.ExecuteNonQuery()

    '    con.Close()
    'End Sub

    'Public Shared Sub createUserTables(ByVal assessmentTaxModelID As Integer, ByVal userID As Integer)
    '    'make connection to database
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    'calculate assessment summary
    '    query.CommandText = "exec createUserTables " & assessmentTaxModelID & ", " & userID & "," & HttpContext.Current.Session("levelID").ToString
    '    query.ExecuteNonQuery()

    '    con.Close()
    'End Sub

    'Public Shared Sub saveLiveModelTables(ByRef userID As Integer)

    '    'Remove all report filter sessions
    '    HttpContext.Current.Session.Remove("SchoolDistricts")
    '    HttpContext.Current.Session.Remove("MapPropertyClassFilters")
    '    HttpContext.Current.Session.Remove("TaxStatus")
    '    HttpContext.Current.Session.Remove("MapTaxStatusFilters")
    '    HttpContext.Current.Session.Remove("Municipalities")
    '    HttpContext.Current.Session.Remove("SchoolDistricts")
    '    HttpContext.Current.Session.Remove("ParcelID")
    '    HttpContext.Current.Session.Remove("MapAnalysisLayer")
    '    HttpContext.Current.Session.Remove("MapZoom")
    '    HttpContext.Current.Session.Remove("JurisType")
    '    HttpContext.Current.Session.Remove("JurisTypeGroup")
    '    HttpContext.Current.Session.Remove("MapTaxShiftFilters")
    '    HttpContext.Current.Session.Remove("TaxShift")
    '    HttpContext.Current.Session.Remove("TaxClass")
    '    HttpContext.Current.Session.Remove("calculated")

    '    'Remove sessions from POV/PMR/TaxCredit
    '    HttpContext.Current.Session.Remove("classFilter")
    '    HttpContext.Current.Session.Remove("taxClasses")

    '    'make connection to database
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    Dim dr As SqlClient.SqlDataReader
    '    Dim assessmentName As String = ""

    '    'variables to hold ID's
    '    Dim uniformMillRateID As Integer
    '    Dim POVID As Integer
    '    Dim taxCreditID As Integer
    '    Dim K12ID As Integer
    '    Dim PMRID As Integer
    '    Dim EDPOVID As Integer
    '    Dim PEMRID As Integer   'Donna
    '    Dim assessmentTaxModelID As Integer

    '    Dim oldPOVID As Integer
    '    Dim oldtaxCreditID As Integer
    '    Dim oldsubjectTaxYearModelID As Integer
    '    Dim subjectTaxYearModelID As Integer



    '    query.CommandText = "select assessmentTaxModelName, assessmentTaxModelID from liveAssessmentTaxModel where userID = " & userID
    '    dr = query.ExecuteReader()

    '    If dr.Read() Then
    '        assessmentName = NullToStr(dr.Item(0))
    '        assessmentTaxModelID = ToInteger(dr.GetValue(1))
    '    End If

    '    dr.Close()

    '    'get ID values if not working on live tables
    '    'query.CommandText = "select uniformMunicipalMillRateID, POVID, taxCreditID, K12ID, PMRID, taxYearModelID, EDPOVID from taxYearModelDescription where taxYearModelID = (select subjectTaxYearModelID from liveassessmenttaxmodel where userID = " & userID.ToString & ")"
    '    'Donna - Added subjPEMRID.
    '    query.CommandText = "select subjuniformMunicipalMillRateID, subjPOVID, subjtaxCreditID, SubjectK12ID, subjPMRID, subjectTaxYearModelID, subjEDPOVID, subjPEMRID from liveAssessmentTaxModel where userID = " & userID.ToString
    '    dr = query.ExecuteReader

    '    dr.Read()
    '    uniformMillRateID = ToInteger(dr.GetValue(0))
    '    POVID = ToInteger(dr.GetValue(1))
    '    taxCreditID = ToInteger(dr.GetValue(2))
    '    K12ID = ToInteger(dr.GetValue(3))
    '    PMRID = ToInteger(dr.GetValue(4))
    '    subjectTaxYearModelID = ToInteger(dr.GetValue(5))
    '    EDPOVID = ToInteger(dr.GetValue(6))
    '    PEMRID = ToInteger(dr.GetValue(7)) 'Donna

    '    dr.Close()

    '    'If PATMAP.Global_asax.satellite Or HttpContext.Current.Session("levelID") = 3 Or HttpContext.Current.Session("levelID") = 49 Then
    '    If HttpContext.Current.Session("levelID") = 3 Or HttpContext.Current.Session("levelID") = 49 Then
    '        oldPOVID = POVID
    '        oldtaxCreditID = taxCreditID
    '        oldsubjectTaxYearModelID = subjectTaxYearModelID

    '        POVID = 0
    '        taxCreditID = 0
    '        PMRID = 0
    '        EDPOVID = 0
    '        assessmentTaxModelID = 0

    '        query.CommandText = "insert into taxYearModelDescription select 'Auto generated " & assessmentName & "', year, taxYearStatusID, notes, assessmentID, millRateSurveyID, uniformMunicipalMillRateID, uniformPotashMillRateID, uniformK12MillRateID, uniformSchoolMillRateID, POVID, EDPOVID, taxCreditID, K12ID, PMRID, PotashID, baseTaxYearModelID, dataStale from taxYearModelDescription where taxYearModelID = " & subjectTaxYearModelID & " select @@IDENTITY"
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        subjectTaxYearModelID = dr.GetValue(0)

    '        dr.Close()

    '        query.CommandText = "update liveAssessmentTaxModel set subjectTaxYearModelID = " & subjectTaxYearModelID & " where userID = " & userID
    '        query.ExecuteNonQuery()


    '    End If


    '    'check if tax year model already has a POV ID
    '    If POVID > 0 Then
    '        'clear POV values to be updated
    '        query.CommandText = "delete from POV where POVID = " & POVID.ToString
    '        query.ExecuteNonQuery()
    '    Else
    '        'Get new POV ID number
    '        query.CommandText = "insert into POVDescription select '', 'Auto generated " & assessmentName & "', year(GETDATE()), 1 select @@IDENTITY"
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        POVID = dr.GetValue(0)
    '        dr.Close()

    '        'save new tax year model number
    '        'query.CommandText = "update taxYearModelDescription set POVID = " & POVID.ToString & " where taxYearModelID = (select subjectTaxYearModelID from liveassessmenttaxmodel where userID = " & userID.ToString & ")"
    '        query.CommandText = "update liveAssessmentTaxModel set subjPOVID = " & POVID.ToString & " where userID = " & userID.ToString
    '        query.ExecuteNonQuery()
    '    End If

    '    'insert new EDPOV values 
    '    query.CommandText = "insert into POV select " & POVID.ToString & ", taxclassID, POV from livePOV where userID = " & userID.ToString
    '    query.ExecuteNonQuery()

    '    'check if tax year model already has a EDPOV ID
    '    If EDPOVID > 0 Then
    '        'clear EDPOV values to be updated
    '        query.CommandText = "delete from EDPOV where EDPOVID = " & EDPOVID.ToString
    '        query.ExecuteNonQuery()
    '    Else
    '        'Get new EDPOV ID number
    '        query.CommandText = "insert into EDPOVDescription select '', 'Auto generated " & assessmentName & "', year(GETDATE()), 1 select @@IDENTITY"
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        EDPOVID = dr.GetValue(0)
    '        dr.Close()

    '        'save new tax year model number
    '        query.CommandText = "update liveAssessmentTaxModel set subjEDPOVID = " & EDPOVID.ToString & " where userID = " & userID.ToString
    '        query.ExecuteNonQuery()
    '    End If

    '    'insert new EDPOV values 
    '    query.CommandText = "insert into EDPOV select " & EDPOVID.ToString & ", taxclassID, EDPOV, EDPOVFactor from liveEDPOV where userID = " & userID.ToString
    '    query.ExecuteNonQuery()


    '    'check if tax year model already has a taxcredit ID
    '    If taxCreditID > 0 Then
    '        'clear taxcredit values to be updated
    '        query.CommandText = "delete from taxCredit where taxCreditID = " & taxCreditID.ToString
    '        query.ExecuteNonQuery()
    '    Else
    '        'Get new taxCredit ID number
    '        query.CommandText = "insert into taxCreditDescription select year(GETDATE()), '', 'Auto generated " & assessmentName & "', 1 select @@IDENTITY"
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        taxCreditID = dr.GetValue(0)
    '        dr.Close()

    '        'save new tax year model number
    '        query.CommandText = "update liveAssessmentTaxModel set subjtaxCreditID = " & taxCreditID.ToString & " where userID = " & userID.ToString
    '        query.ExecuteNonQuery()
    '    End If

    '    'insert new taxcredit values 
    '    query.CommandText = "insert into taxCredit select " & taxCreditID.ToString & ", taxclassID, taxCredit, capped  from livetaxCredit where userID = " & userID.ToString
    '    query.ExecuteNonQuery()

    '    'check if tax year model already has a PMR ID
    '    If PMRID > 0 Then
    '        'clear PMR values to be updated
    '        query.CommandText = "delete from PMR where PMRID = " & PMRID.ToString
    '        query.ExecuteNonQuery()
    '    Else
    '        'Get new PMR ID number
    '        query.CommandText = "insert into PMRDescription select '','Auto generated " & assessmentName & "', 1 select @@IDENTITY"
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        PMRID = dr.GetValue(0)
    '        dr.Close()

    '        'save new tax year model number
    '        query.CommandText = "update liveAssessmentTaxModel set subjPMRID = " & PMRID.ToString & " where userID = " & userID.ToString
    '        query.ExecuteNonQuery()
    '    End If

    '    'insert new PMR values 
    '    query.CommandText = "insert into PMR select " & PMRID.ToString & ", taxclassID, PMR, PMRReplacement, assessmentInclude from livePMR where userID = " & userID.ToString
    '    query.ExecuteNonQuery()

    '    'Donna start
    '    If PEMRID > 0 Then
    '        'Clear existing PEMR values.
    '        query.CommandText = "DELETE FROM PEMR WHERE PEMRID = " & PEMRID.ToString
    '        query.ExecuteNonQuery()
    '    Else
    '        'Get new PEMRID.
    '        query.CommandText = "INSERT INTO PEMRDescription (dataSetName, year, statusID) VALUES ('Auto generated " & assessmentName & "', year(GETDATE()), 1) SELECT @@IDENTITY"
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        PEMRID = dr.GetValue(0)
    '        dr.Close()

    '        query.CommandText = "UPDATE liveAssessmentTaxModel SET subjPEMRID = " & PEMRID.ToString & " WHERE userID = " & userID.ToString
    '        query.ExecuteNonQuery()
    '    End If

    '    'Insert new PEMR values.
    '    query.CommandText = "INSERT INTO PEMR SELECT " & PEMRID.ToString & ", taxClassID, tier, PEMR FROM livePEMR WHERE userID = " & userID.ToString
    '    query.ExecuteNonQuery()
    '    'Donna end


    '    'if it is a new Assessment Tax Model, then insert into real time assessment tax model table
    '    'otherwise update the real time assessment tax model table
    '    If assessmentTaxModelID <> 0 Then
    '        'Donna - Changed query to only return columns that are needed.
    '        query.CommandText = "select assessmentTaxModelName, description, baseTaxYearModelID, subjectTaxYearModelID, notes, " & _
    '            "audiencePresentation, audienceExternalUser, SubjectK12ID, K12Amount, K12Replacement, dataStale, auditTrailText, " & _
    '            "enterPEMR, PEMRByTotalLevy " & _
    '            "FROM liveAssessmentTaxModel " & _
    '            "WHERE userID = " & userID.ToString
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        Dim values(14) As Object
    '        dr.GetValues(values)
    '        dr.Close()
    '        'Donna - Added enterPEMR and PEMRByTotalLevy.
    '        query.CommandText = "UPDATE assessmentTaxModel SET assessmentTaxModelName = '" & values(0).ToString.Replace("'", "''") & _
    '            "', lastModified = GETDATE(), description = '" & values(1).ToString.Replace("'", "''") & "', BaseTaxYearModelID = " & _
    '            values(2).ToString & ", SubjectTaxYearModelID = " & values(3).ToString & ", notes = '" & values(4).ToString.Replace("'", "''") & _
    '            "', audiencePresentation = '" & values(5).ToString & "', audienceExternalUser = '" & values(6).ToString & _
    '            "', subjectK12ID = " & values(7).ToString & ", K12Amount = " & values(8).ToString & ", K12Replacement = '" & _
    '            values(9).ToString & "', dataStale = '" & values(10).ToString & "', auditTrailText = '" & values(11).ToString.Replace("'", "''") & _
    '            "', enterPEMR = '" & values(12) & "', PEMRByTotalLevy = '" & values(13) & _
    '            "' WHERE assessmentTaxModelID = " & assessmentTaxModelID.ToString
    '        query.ExecuteNonQuery()
    '    Else
    '        'Donna - Added subjPEMRID, enterPEMR and PEMRByTotalLevy.
    '        query.CommandText = "insert into assessmentTaxModel select l.assessmentTaxModelName," & userID.ToString & ", GETDATE(), GETDATE(), l.description, l.baseTaxYearModelID, l.subjectTaxYearModelID, l.notes, l.audiencePresentation, l.audienceExternalUser, l.SubjectK12ID, l.K12Amount, l.K12Replacement, l.dataStale, l.auditTrailText, l.subjassessmentID, l.subjmillRateSurveyID, l.subjuniformMunicipalMillRateID, l.subjuniformPotashMillRateID, l.subjuniformK12MillRateID, l.subjuniformSchoolMillRateID, l.subjPOVID, l.subjEDPOVID, l.subjtaxCreditID, l.subjPMRID, l.subjPotashID, subjPEMRID, enterPEMR, PEMRByTotalLevy from liveAssessmentTaxModel l where userID=" & userID.ToString & " select @@IDENTITY"
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        assessmentTaxModelID = dr.GetValue(0)
    '        dr.Close()
    '    End If


    '    'transfer(files) - table
    '    query.CommandText = "delete from assessmentTaxModelFile where assessmentTaxModelID=" & assessmentTaxModelID.ToString & " AND fileID NOT IN (select fileID from liveAssessmentTaxModelFile where userID=" & userID.ToString & ")"
    '    query.ExecuteNonQuery()
    '    query.CommandText = "insert into assessmentTaxModelFile select " & assessmentTaxModelID.ToString & ", l.filename, l.dateLoaded from liveAssessmentTaxModelFile l where userID=" & userID.ToString & " AND fileID NOT IN (select fileID from assessmentTaxModelFile where assessmentTaxModelID=" & assessmentTaxModelID.ToString & ")"
    '    query.ExecuteNonQuery()


    '    'check if data is stale
    '    'query.CommandText = "select liveassessmenttaxmodel.baseTaxYearModelID, taxYearModelDescription.datastale, liveassessmenttaxmodel.subjectTaxYearModelID,liveassessmenttaxmodel.dataStale from liveassessmenttaxmodel inner join taxYearModelDescription on taxYearModelID = liveassessmenttaxmodel.baseTaxYearModelID where userid = " & userID
    '    'dr = query.ExecuteReader
    '    'dr.Read()
    '    'BaseTaxYearModelID = dr.GetValue(0)
    '    'BaseStale = dr.GetValue(1)
    '    'subjectTaxYearModelID = dr.GetValue(2)
    '    'SubjectStale = dr.GetValue(3)
    '    'dr.Close()

    '    'If Not BaseStale Or Not SubjectStale Then
    '    '    'save subject tax year model data
    '    '    query.CommandText = "delete from assessmentTaxModelResults where assessmentTaxModelID = " & assessmentTaxModelID.ToString
    '    '    query.ExecuteNonQuery()
    '    '    query.CommandText = "insert into assessmentTaxModelResults select " & assessmentTaxModelID.ToString & " as assessmentTaxModelID, alternate_parcelID, municipalityID, schoolID, taxClassID, presentUseCodeID, BasetaxableMarketValue, BaseexemptMarketValue, BaseFGILMarketValue, BasePGILMarketValue, BasetaxableMunicipalTax, BaseFGILMunicipalTax, BasePGILMunicipalTax, BasetaxableSchoolTax, BaseFGILSchoolTax, BasePGILSchoolTax, BasetaxableProvincialSchoolTax, BaseFGILProvincialSchoolTax, BasePGILProvincialSchoolTax, BasetaxCredit, BaseK12, BasePotash, SubjecttaxableMarketValue, SubjectexemptMarketValue, SubjectFGILMarketValue, SubjectPGILMarketValue, SubjecttaxableMunicipalTax, SubjectFGILMunicipalTax, SubjectPGILMunicipalTax, SubjecttaxableSchoolTax, SubjectFGILSchoolTax, SubjectPGILSchoolTax, SubjecttaxableProvincialSchoolTax, SubjectFGILProvincialSchoolTax, SubjectPGILProvincialSchoolTax, SubjecttaxCredit, SubjectK12, SubjectPotash  from liveAssessmentTaxModelResults_" & userID.ToString
    '    '    query.ExecuteNonQuery()

    '    '    query.CommandText = "delete from assessmentTaxModelResultsSummary where assessmentTaxModelID = " & assessmentTaxModelID.ToString
    '    '    query.ExecuteNonQuery()
    '    '    query.CommandText = "insert into assessmentTaxModelResultsSummary select " & assessmentTaxModelID.ToString & " as assessmentTaxModelID, municipalityID, schoolID, taxClassID, BasetaxableMarketValue, BaseexemptMarketValue, BaseFGILMarketValue, BasePGILMarketValue, BasetaxableMunicipalTax, BaseFGILMunicipalTax, BasePGILMunicipalTax, BasetaxableSchoolTax, BaseFGILSchoolTax, BasePGILSchoolTax, BasetaxableProvincialSchoolTax, BaseFGILProvincialSchoolTax, BasePGILProvincialSchoolTax, BasetaxCredit, BaseK12, BasePotash, SubjecttaxableMarketValue, SubjectexemptMarketValue, SubjectFGILMarketValue, SubjectPGILMarketValue, SubjecttaxableMunicipalTax, SubjectFGILMunicipalTax, SubjectPGILMunicipalTax, SubjecttaxableSchoolTax, SubjectFGILSchoolTax, SubjectPGILSchoolTax, SubjecttaxableProvincialSchoolTax, SubjectFGILProvincialSchoolTax, SubjectPGILProvincialSchoolTax, SubjecttaxCredit, SubjectK12 , SubjectPotash from liveAssessmentTaxModelResultsSummary_" & userID.ToString
    '    '    query.ExecuteNonQuery()
    '    'End If


    '    'If PATMAP.Global_asax.satellite Or HttpContext.Current.Session("levelID") = 3 Or HttpContext.Current.Session("levelID") = 49 Then
    '    If HttpContext.Current.Session("levelID") = 3 Or HttpContext.Current.Session("levelID") = 49 Then

    '        query.CommandText = "insert into POVFile select " & POVID & " as POVID, filename, dateLoaded from POVFile where POVID = " & oldPOVID
    '        query.ExecuteNonQuery()

    '        query.CommandText = "insert into taxCreditFile select " & taxCreditID & " as taxCreditID, filename, dateLoaded from taxCreditFile where taxCreditID = " & oldtaxCreditID
    '        query.ExecuteNonQuery()


    '        copyLiveModelTableFiles(userID, assessmentTaxModelID, general.liveSubFolder)

    '        copyLiveModelTableFiles(oldsubjectTaxYearModelID, subjectTaxYearModelID, general.liveSubFolder)

    '        copyLiveModelTableFiles(oldPOVID, POVID, editdataset.POVSubFolder)

    '        copyLiveModelTableFiles(oldtaxCreditID, taxCreditID, editdataset.taxCreditSubFolder)
    '    Else
    '        'transfer(files) - phisical       
    '        common.moveAssessmentFiles(assessmentTaxModelID, userID)
    '    End If

    '    If Not IsNothing(HttpContext.Current.Session("calcDone_" & userID)) Then
    '        query.CommandText = "update assessmentTaxModel set dataStale = 0 where assessmentTaxModelID = " & assessmentTaxModelID
    '        query.ExecuteNonQuery()
    '        HttpContext.Current.Session.Remove("calcDone_" & userID)
    '    End If

    '    'transfer user tables to model tables
    '    query.CommandText = "exec createModelTables " & assessmentTaxModelID & ", " & userID
    '    query.ExecuteNonQuery()

    'End Sub

    'Public Shared Sub SetTaxClassFilters()
    '    'removes sessions
    '    HttpContext.Current.Session.Remove("MapPropertyClassFilters")

    '    Dim userID As Integer = HttpContext.Current.Session("userID")

    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()

    '    Dim dr As SqlClient.SqlDataReader
    '    Dim query As New SqlClient.SqlCommand
    '    Dim sql As String
    '    Dim filter As New List(Of String)

    '    'get selected main tax classes
    '    sql = "select taxClasses.taxClassID from livetaxClasses inner join taxClasses on livetaxClasses.taxClassID = taxClasses.taxClassID where taxClasses.parentTaxClassID = 'none' and livetaxClasses.show = 1 and livetaxClasses.userID=" & userID & " order by taxClasses.sort"

    '    query.Connection = con
    '    query.CommandText = sql
    '    dr = query.ExecuteReader()

    '    While dr.Read()
    '        filter.Add(dr.Item(0))
    '    End While

    '    dr.Close()

    '    'get selected sub tax classes
    '    sql = "select taxClasses.taxClassID from livetaxClasses inner join taxClasses on livetaxClasses.taxClassID = taxClasses.taxClassID where taxClasses.parentTaxClassID <> 'none' and livetaxClasses.show = 1 and livetaxClasses.userID=" & userID & " order by taxClasses.sort"

    '    query.Connection = con
    '    query.CommandText = sql
    '    dr = query.ExecuteReader()

    '    While dr.Read()
    '        filter.Add(dr.Item(0))
    '    End While

    '    dr.Close()

    '    If filter.Count > 0 Then
    '        HttpContext.Current.Session("MapPropertyClassFilters") = filter
    '    End If

    '    con.Close()
    'End Sub

    'Public Shared Sub SetLTT_TaxClassFilters()
    '    'removes sessions
    '    HttpContext.Current.Session.Remove("MapPropertyClassFilters")

    '    Dim userID As Integer = HttpContext.Current.Session("userID")
    '    Dim levelID As Integer = HttpContext.Current.Session("levelID")

    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()

    '    Dim dr As SqlClient.SqlDataReader
    '    Dim query As New SqlClient.SqlCommand
    '    Dim sql As String
    '    Dim filter As New List(Of String)
    '    Dim showFullTaxClasses As Boolean = False
    '    If Not IsNothing(System.Web.HttpContext.Current.Session("MapPropertyClassFilters")) Then
    '        filter = CType(System.Web.HttpContext.Current.Session("MapPropertyClassFilters"), List(Of String))
    '    End If

    '    If Not IsNothing(System.Web.HttpContext.Current.Session("showFullTaxClasses")) Then
    '        showFullTaxClasses = CType(System.Web.HttpContext.Current.Session("showFullTaxClasses"), Boolean)
    '    End If

    '    If filter.Count <= 0 Then

    '        'get selected main tax classes
    '        'sql = "select taxClasses.taxClassID from livetaxClasses inner join taxClasses on livetaxClasses.taxClassID = taxClasses.taxClassID where taxClasses.parentTaxClassID = 'none' and livetaxClasses.show = 1 and livetaxClasses.userID=" & userID & " order by taxClasses.sort"
    '        If showFullTaxClasses Then
    '            sql = "SELECT taxClasses.[taxClassID] FROM taxClasses INNER JOIN taxClassesPermission ON taxClasses.taxClassID = taxClassesPermission.taxClassID WHERE [default] = 1 AND levelID = " & levelID & " AND access = 1"
    '        Else
    '            sql = "SELECT DISTINCT m.[taxClassID] FROM taxclasses INNER JOIN LTTtaxClasses t ON t.taxClassID = taxclasses.taxClassID INNER JOIN LTTmainTaxClasses m ON m.taxClassID = t.LTTmainTaxClassID INNER JOIN taxClassesPermission ON taxClasses.taxClassID = taxClassesPermission.taxClassID WHERE taxclasses.[default] = 1 AND levelID =" & levelID & " AND access = 1"
    '        End If

    '        query.Connection = con
    '        query.CommandText = sql
    '        dr = query.ExecuteReader()

    '        While dr.Read()
    '            filter.Add(dr.Item(0))
    '        End While

    '        dr.Close()

    '        ''get selected main tax classes
    '        ''sql = "select taxClasses.taxClassID from livetaxClasses inner join taxClasses on livetaxClasses.taxClassID = taxClasses.taxClassID where taxClasses.parentTaxClassID = 'none' and livetaxClasses.show = 1 and livetaxClasses.userID=" & userID & " order by taxClasses.sort"
    '        'sql = "select taxClasses.taxClassID from liveLTTtaxClasses_" + userID.ToString().Trim() + " liveLTTtaxClasses inner join taxClasses on liveLTTtaxClasses.taxClassID = taxClasses.taxClassID where taxClasses.parentTaxClassID = 'none' and liveLTTtaxClasses.show = 1  order by taxClasses.sort"
    '        'query.Connection = con
    '        'query.CommandText = sql
    '        'dr = query.ExecuteReader()

    '        'While dr.Read()
    '        '	filter.Add(dr.Item(0))
    '        'End While

    '        'dr.Close()

    '        ''get selected sub tax classes
    '        ''sql = "select taxClasses.taxClassID from livetaxClasses inner join taxClasses on livetaxClasses.taxClassID = taxClasses.taxClassID where taxClasses.parentTaxClassID <> 'none' and livetaxClasses.show = 1 and livetaxClasses.userID=" & userID & " order by taxClasses.sort"
    '        'sql = "select taxClasses.taxClassID from liveLTTtaxClasses_" + userID.ToString().Trim() + " liveLTTtaxClasses inner join taxClasses on liveLTTtaxClasses.taxClassID = taxClasses.taxClassID where taxClasses.parentTaxClassID <> 'none' and liveLTTtaxClasses.show = 1 order by taxClasses.sort"

    '        'query.Connection = con
    '        'query.CommandText = sql
    '        'dr = query.ExecuteReader()

    '        'While dr.Read()
    '        '	filter.Add(dr.Item(0))
    '        'End While

    '        'dr.Close()

    '        If filter.Count > 0 Then
    '            HttpContext.Current.Session("MapPropertyClassFilters") = filter
    '        End If
    '    End If
    '    con.Close()
    'End Sub


    'Public Shared Sub calculateBoundaryModel(ByVal userID As Integer)
    '    'variables to hold ID's
    '    Dim assessmentID As Integer
    '    Dim millRateSurveyID As Integer
    '    Dim POVID As Integer

    '    'make connection to database
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    query.CommandText = "select assessmentID, millRateSurveyID, POVID from boundaryModel where status = 1"
    '    Dim dr As SqlClient.SqlDataReader
    '    dr = query.ExecuteReader
    '    dr.Read()
    '    assessmentID = dr.GetValue(0)
    '    millRateSurveyID = dr.GetValue(1)
    '    POVID = dr.GetValue(2)
    '    dr.Close()

    '    'calculate uniform municipal mill Rates

    '    If userID = 0 Then
    '        query.CommandText = "exec calcBoundaryUniformMunicipalMillRateID " & assessmentID & "," & POVID & "," & millRateSurveyID
    '        query.ExecuteNonQuery()
    '    End If

    '    query.CommandText = "exec calcBoundaryModel " & userID & ", " & assessmentID & "," & POVID
    '    query.ExecuteNonQuery()

    '    query.CommandText = "exec calcBoundaryModelForTransferedProperties " & userID & ", " & assessmentID & "," & POVID
    '    query.ExecuteNonQuery()

    '    If userID > 0 Then
    '        query.CommandText = "exec calcBoundaryModelSummary " & userID
    '        query.ExecuteNonQuery()
    '    End If

    '    'set the boundary model as NOT stale
    '    If userID = 0 Then
    '        query.CommandText = "update boundarymodel set dataStale = 0 where status = 1"
    '        query.ExecuteNonQuery()
    '    End If

    'End Sub

    'Public Shared Sub LoadLiveTaxYearModel(ByVal subjectTaxYrModelID As String, ByVal baseTaxYrModelID As String)
    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    'ID variables
    '    Dim userID As Integer = HttpContext.Current.Session("userID")
    '    Dim assessmentTaxModelID As Integer = HttpContext.Current.Session("AssessmentTaxModelID")
    '    Dim POVID As Integer
    '    Dim EDPOVID As Integer
    '    Dim taxCreditID As Integer
    '    Dim PMRID As Integer
    '    Dim PEMRID As Integer       'Donna

    '    Dim basePOVID As Integer
    '    Dim baseEDPOVID As Integer
    '    Dim basetaxCreditID As Integer
    '    Dim basePMRID As Integer
    '    Dim basePEMRID As Integer       'Donna
    '    'Dim BaseTaxYearModelID As Integer
    '    'Dim BaseStale As Boolean
    '    'Dim SubjectStale As Boolean
    '    'Dim subjectTaxYearModelID As Integer

    '    Dim dr As SqlClient.SqlDataReader

    '    'load assessment tax model into the live tables
    '    'existing model - recall
    '    If assessmentTaxModelID > 0 Then
    '        query.CommandText = "delete from liveAssessmentTaxModel where userID = " & userID.ToString
    '        query.ExecuteNonQuery()
    '        'Donna - Added subjPEMRID, enterPEMR and PEMRByTotalLevy.
    '        query.CommandText = "insert into liveAssessmentTaxModel select " & userID.ToString & " as userid, assessmentTaxModelID,assessmentTaxModelName,assessmentTaxModel.description,assessmentTaxModel.baseTaxYearModelID,assessmentTaxModel.subjectTaxYearModelID,assessmentTaxModel.notes,assessmentTaxModel.audiencepresentation, assessmentTaxModel.audienceExternalUser, assessmentTaxModel.subjectK12ID, assessmentTaxModel.K12Amount, assessmentTaxModel.K12Replacement, assessmentTaxModel.dataStale, assessmentTaxModel.auditTrailText, subjassessmentID, subjmillRateSurveyID, subjuniformMunicipalMillRateID, subjuniformPotashMillRateID, subjuniformK12MillRateID, subjuniformSchoolMillRateID, subjPOVID, subjEDPOVID, subjtaxCreditID, subjPMRID, subjPotashID, subjPEMRID, enterPEMR, PEMRByTotalLevy from assessmentTaxModel inner join taxYearModelDescription on assessmentTaxModel.subjectTaxYearModelID = taxYearModelID where assessmentTaxModel.assessmentTaxModelID = " & assessmentTaxModelID.ToString
    '        query.ExecuteNonQuery()

    '        'get ID values to be loaded for the live datasets
    '        'Donna - Added subjPEMRID.
    '        query.CommandText = "select subjPOVID, subjtaxCreditID, subjPMRID, subjEDPOVID, subjPEMRID from liveAssessmentTaxModel where userID = " & userID.ToString
    '        dr = query.ExecuteReader()
    '        dr.Read()
    '        POVID = dr.GetValue(0)
    '        taxCreditID = dr.GetValue(1)
    '        PMRID = dr.GetValue(2)
    '        EDPOVID = dr.GetValue(3)

    '        'Donna start
    '        If dr.IsDBNull(4) Then
    '            PEMRID = 0
    '        Else
    '            PEMRID = dr.GetValue(4)
    '        End If
    '        'Donna end

    '        dr.Close()
    '    Else
    '        'new model
    '        'get ID values to be loaded for the live datasets
    '        'query.CommandText = "select POVID, taxCreditID, PMRID, EDPOVID from taxYearModelDescription where taxYearModelID = " & subjectTaxYrModelID
    '        query.CommandText = "select isnull(POVID,0), isnull(taxCreditID,0), isnull(PMRID,0), isnull(EDPOVID,0) from taxYearModelDescription where taxYearModelID = " & subjectTaxYrModelID

    '        dr = query.ExecuteReader()
    '        dr.Read()
    '        POVID = dr.GetValue(0)
    '        taxCreditID = dr.GetValue(1)
    '        PMRID = dr.GetValue(2)
    '        EDPOVID = dr.GetValue(3)

    '        'Donna start
    '        'If dr.IsDBNull(4) Then
    '        '    PEMRID = 0
    '        'Else
    '        '    PEMRID = dr.GetValue(4)
    '        'End If
    '        'Donna end

    '        dr.Close()
    '        'Donna - Added PEMRID.
    '        'query.CommandText = "select POVID, taxCreditID, PMRID, EDPOVID, PEMRID from taxYearModelDescription where taxYearModelID = " & baseTaxYrModelID
    '        query.CommandText = "select isnull(POVID,0), isnull(taxCreditID,0), isnull(PMRID,0), isnull(EDPOVID,0), isnull(PEMRID,0) from taxYearModelDescription where taxYearModelID = " & baseTaxYrModelID

    '        dr = query.ExecuteReader()
    '        dr.Read()
    '        basePOVID = dr.GetValue(0)
    '        basetaxCreditID = dr.GetValue(1)
    '        basePMRID = dr.GetValue(2)
    '        baseEDPOVID = dr.GetValue(3)

    '        'Donna start
    '        If dr.IsDBNull(4) Then
    '            basePEMRID = 0
    '        Else
    '            basePEMRID = dr.GetValue(4)
    '        End If
    '        'Donna end

    '        dr.Close()
    '        If POVID = 0 Then
    '            POVID = basePOVID
    '        End If

    '        If taxCreditID = 0 Then
    '            taxCreditID = basetaxCreditID
    '        End If

    '        If PMRID = 0 Then
    '            PMRID = basePMRID
    '        End If

    '        If EDPOVID = 0 Then
    '            EDPOVID = baseEDPOVID
    '        End If

    '        'Donna start - For a new scenario, the base PEMR data will be used as the starting point for the subject data.
    '        PEMRID = basePEMRID
    '        'If PEMRID = 0 Then
    '        '    PEMRID = basePEMRID
    '        'End If
    '        'Donna end
    '    End If

    '    'get ID values to be loaded for the live datasets 
    '    'query.CommandText = "select case when isnull(STYM.POVID,0) = 0 then BTYM.POVID else STYM.POVID end as POVID,case when isnull(STYM.taxCreditID,0) = 0 then BTYM.taxCreditID else STYM.taxCreditID end as taxCreditID,case when isnull(STYM.PMRID,0) = 0 then isnull(BTYM.PMRID,0) else STYM.PMRID end as PMRID, case when isnull(STYM.EDPOVID,0) = 0 then isnull(BTYM.EDPOVID,0) else STYM.EDPOVID end as EDPOVID, assessmentTaxModel.SubjectTaxYearModelID from liveAssessmentTaxModel assessmentTaxModel inner join taxYearModelDescription BTYM on assessmentTaxModel.BaseTaxYearModelID = BTYM.TaxYearModelID left join taxYearModelDescription STYM on assessmentTaxModel.SubjectTaxYearModelID = STYM.TaxYearModelID where userID = " & userID
    '    'query.CommandText = "select subjPOVID, subjtaxCreditID, subjPMRID, subjEDPOVID, subjectTaxYearModelID from liveAssessmentTaxModel where userID = " & userID
    '    'dr = query.ExecuteReader()
    '    'dr.Read()
    '    'POVID = dr.GetValue(0)
    '    'taxCreditID = dr.GetValue(1)
    '    'PMRID = dr.GetValue(2)
    '    'EDPOVID = dr.GetValue(3)
    '    'subjectTaxYearModelID = dr.GetValue(4)

    '    'If PATMAP.Global_asax.satellite Or HttpContext.Current.Session("levelID") = 3 Or HttpContext.Current.Session("levelID") = 49 Then
    '    If HttpContext.Current.Session("levelID") = 3 Or HttpContext.Current.Session("levelID") = 49 Then

    '        Dim count As Integer = 0
    '        Dim assessmentName As String = ""

    '        query.CommandText = "select count(assessmentTaxModelName)-1, assessmentTaxModelName from liveAssessmentTaxModel where userID = " & userID & " group by assessmentTaxModelName"
    '        dr = query.ExecuteReader()

    '        If dr.Read() Then
    '            count = dr.Item(0) + 1
    '            assessmentName = Replace(dr.Item(1) & " " & count, "'", "''")
    '        End If

    '        dr.Close()

    '        query.CommandText = "update liveAssessmentTaxModel set assessmentTaxModelID = 0, assessmentTaxModelName = '" & assessmentName & "' where userID = " & userID
    '        query.ExecuteNonQuery()

    '        HttpContext.Current.Session("AssessmentTaxModelID") = 0
    '    End If


    '    'load POV data into live tables
    '    query.CommandText = "delete from livePOV where userID = " & userID.ToString
    '    query.ExecuteNonQuery()
    '    query.CommandText = "insert into livePOV select " & userID.ToString & " as userID, taxClassID,POV from POV where POVID = " & POVID.ToString
    '    query.ExecuteNonQuery()

    '    'load EDPOV data into live tables
    '    query.CommandText = "delete from liveEDPOV where userID = " & userID.ToString
    '    query.ExecuteNonQuery()
    '    If EDPOVID = 0 Then
    '        'by default the edpov has to be 100% of the pov - subject
    '        query.CommandText = "insert into liveEDPOV select " & userID.ToString & " as userID, taxClassID, 1.0, 0 from taxClasses"
    '    Else
    '        query.CommandText = "insert into liveEDPOV select " & userID.ToString & " as userID, taxClassID, EDPOV, EDPOVFactor from EDPOV where EDPOVID = " & EDPOVID.ToString
    '    End If
    '    query.ExecuteNonQuery()

    '    'load Tax credit data into live tables
    '    query.CommandText = "delete from liveTaxcredit where userID = " & userID.ToString
    '    query.ExecuteNonQuery()
    '    query.CommandText = "insert into liveTaxcredit select " & userID.ToString & " as userID, taxClassID,taxCredit, capped from taxcredit where taxCreditID = " & taxCreditID.ToString
    '    query.ExecuteNonQuery()

    '    'load PMR data into live tables
    '    query.CommandText = "delete from livepmr where userID = " & userID.ToString
    '    query.ExecuteNonQuery()
    '    If PMRID = 0 Then
    '        query.CommandText = "insert into livepmr select " & userID.ToString & " as userID, taxClassID, 0.0, 0, 1 from taxClasses"
    '    Else
    '        query.CommandText = "insert into livepmr select " & userID.ToString & " as userID, taxClassID,PMR, PMRReplacement, assessmentInclude from PMR where PMRID = " & PMRID.ToString
    '    End If
    '    query.ExecuteNonQuery()

    '    'Donna start - New scenario:  Load base PEMR data into live table, which is the starting point for the subject data.
    '    'Existing scenario:  Load subject PEMR data into live table.
    '    query.CommandText = "DELETE FROM livePEMR WHERE userID = " & userID.ToString
    '    query.ExecuteNonQuery()
    '    query.CommandText = "INSERT INTO livePEMR SELECT " & userID.ToString & " AS userID, taxClassID, tier, PEMR FROM PEMR WHERE PEMRID = " & PEMRID.ToString
    '    query.ExecuteNonQuery()
    '    'Donna end

    '    'load attached file data to live tables
    '    query.CommandText = "delete from liveAssessmentTaxModelFile where userID = " & userID.ToString
    '    query.ExecuteNonQuery()
    '    query.CommandText = "insert into liveassessmentTaxModelFile select fileID, " & userID.ToString & " as userID, filename, dateLoaded from assessmentTaxModelFile where assessmentTaxModelID = " & assessmentTaxModelID.ToString
    '    query.ExecuteNonQuery()

    '    'load active classes
    '    query.CommandText = "delete from liveTaxClasses where userID = " & userID.ToString
    '    query.ExecuteNonQuery()
    '    'query.CommandText = "insert into liveTaxClasses select " & userID.ToString & " as userID, taxClassID, [default] as show from taxClasses where active = 1"
    '    query.CommandText = "insert into liveTaxClasses select " & userID.ToString & " as userID, taxClasses.taxClassID, [default] as show from taxClasses inner join taxClassesPermission on taxClasses.taxClassID = taxClassesPermission.taxClassID where active = 1 and taxClassesPermission.levelID = " & HttpContext.Current.Session("levelID").ToString & " and taxClassesPermission.access = 1"
    '    query.ExecuteNonQuery()

    '    'check if data is stale
    '    'query.CommandText = "select liveassessmenttaxmodel.baseTaxYearModelID, taxYearModelDescription.datastale, liveassessmenttaxmodel.subjectTaxYearModelID,liveassessmenttaxmodel.dataStale from liveassessmenttaxmodel inner join taxYearModelDescription on taxYearModelID = liveassessmenttaxmodel.baseTaxYearModelID where userid = " & userID
    '    'dr = query.ExecuteReader
    '    'dr.Read()
    '    'BaseTaxYearModelID = dr.GetValue(0)
    '    'BaseStale = dr.GetValue(1)
    '    'SubjectTaxYearModelID = dr.GetValue(2)
    '    'SubjectStale = dr.GetValue(3)
    '    'dr.Close()

    '    'check if data is stale
    '    query.CommandText = "select dataStale from liveassessmenttaxmodel where userid = " & userID
    '    dr = query.ExecuteReader
    '    dr.Read()
    '    Dim dataStale As Boolean = dr.GetValue(0)


    '    dr.Close()
    '    query.CommandText = "select dataStale from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
    '    dr = query.ExecuteReader
    '    dr.Read()
    '    Dim basedataStale As Boolean = dr.GetValue(0)

    '    dr.Close()
    '    con.Close()

    '    If Not dataStale And Not basedataStale And assessmentTaxModelID <> 0 Then
    '        'loads summary result table
    '        common.createUserTables(assessmentTaxModelID, userID)
    '        Create_School_Mun_Parcel_Tables()
    '        HttpContext.Current.Session.Add("calculated", "true")
    '    Else
    '        HttpContext.Current.Session.Remove("calculated")
    '    End If

    '    Dim strPath As String

    '    If assessmentTaxModelID <> 0 Then
    '        copyAssessmentFiles(assessmentTaxModelID, userID)
    '    Else
    '        Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
    '        strPath = PATMAP.Global_asax.FileRootPath & general.liveSubFolder & "\" & userID & "\"
    '        If Directory.Exists(strPath) Then
    '            Directory.Delete(strPath, True)
    '        End If
    '        Impersonate.undoImpersonation()

    '    End If

    '    'clean up
    '    con.Close()

    'End Sub

    'Public Shared Sub copyAssessmentFiles(ByVal assessmentTaxModelID As Integer, ByVal userID As Integer)

    '    Dim destinationPath As String
    '    Dim originalPath As String
    '    Dim strPath As String

    '    Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
    '    strPath = PATMAP.Global_asax.FileRootPath & general.liveSubFolder & "\" & userID & "\"
    '    If Directory.Exists(strPath) Then
    '        Directory.Delete(strPath, True)
    '    End If
    '    Impersonate.undoImpersonation()

    '    originalPath = common.GetFilePath(assessmentTaxModelID, general.subFolder)
    '    destinationPath = common.GetFilePath(userID, general.liveSubFolder)

    '    Dim dir As New DirectoryInfo(originalPath)
    '    Dim dirFiles As FileInfo() = dir.GetFiles()
    '    Dim currentFile As IO.FileInfo
    '    Dim destinationFile As String
    '    Dim originationFile As String

    '    Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
    '    For Each currentFile In dirFiles
    '        originationFile = originalPath & currentFile.ToString()
    '        destinationFile = destinationPath & currentFile.ToString()
    '        File.Copy(originationFile, destinationFile)
    '    Next
    '    Impersonate.undoImpersonation()

    'End Sub

    ''DisplayExpand()
    ''Accepts GridViewRowEventArgs, DataTable, Color and String as parameters
    ''Returns Color; main class's color
    ''Hides or shows expand button.  Also sets background color for each grid view row
    'Public Shared Function DisplayExpand(ByRef e As System.Web.UI.WebControls.GridViewRowEventArgs, ByRef dt As DataTable, ByVal bckgColor As System.Drawing.Color, ByVal classFilter As String) As System.Drawing.Color
    '    Dim btnExpand As System.Web.UI.WebControls.LinkButton
    '    Dim parentID, state As String
    '    Dim dr As DataRow()

    '    'if current row is a datarow type
    '    If (e.Row.RowType = DataControlRowType.DataRow) Then

    '        'gets current row's parenTaxClassID and state (expand/collapse)
    '        parentID = DataBinder.Eval(e.Row.DataItem, "parentTaxClassID")
    '        state = DataBinder.Eval(e.Row.DataItem, "state")

    '        'gets main class and subclasses
    '        dr = dt.Select("groupClasss = '" & DataBinder.Eval(e.Row.DataItem, "groupClasss") & "'")

    '        'gets reference to expand button
    '        btnExpand = CType(e.Row.Cells.Item(0).Controls.Item(0), LinkButton)

    '        'if the tax class is a main class and it has subclasses, display expand/collapse button
    '        If (parentID = "none") And dr.Length > 1 Then
    '            If LCase(state) = "collapse" Then
    '                btnExpand.Text = "<img src='../images/btnExpand.gif'>"
    '            Else
    '                btnExpand.Text = "<img src='../images/btnCollapse.gif'>"
    '            End If

    '            btnExpand.Visible = True

    '            'sets the row's background color
    '            'main classes will have alternating color
    '            If bckgColor = Drawing.Color.White Or e.Row.RowIndex = 1 Then
    '                e.Row.BackColor = Drawing.Color.Moccasin
    '            Else
    '                e.Row.BackColor = Drawing.Color.White
    '            End If

    '            bckgColor = e.Row.BackColor

    '        Else
    '            'if current class doesn't have subclasses or it's a sub tax class
    '            If (parentID <> "none") Then
    '                'sub tax classes will have the same color as
    '                'its parent tax class
    '                e.Row.BackColor = bckgColor
    '            Else
    '                'sets the row's background color
    '                'main classes will have alternating color
    '                If bckgColor = Drawing.Color.White Then
    '                    e.Row.BackColor = Drawing.Color.Moccasin
    '                Else
    '                    e.Row.BackColor = Drawing.Color.White
    '                End If

    '                bckgColor = e.Row.BackColor
    '            End If

    '            'hide expand button
    '            btnExpand.Visible = False

    '        End If
    '    End If

    '    Return bckgColor

    'End Function

    ''ExpandCollapse()
    ''Accepts DataTable and Strings as parameters
    ''Returns String; filter to be applied
    ''Hides or show sub tax classes
    'Public Shared Function ExpandCollapse(ByRef dt As DataTable, ByVal classFilter As String, ByVal taxClass As String) As String
    '    Dim state As String
    '    Dim dr As DataRow()

    '    'gets main class
    '    dr = dt.Select("taxClassID = '" & taxClass & "'")

    '    If dr.Length > 0 Then
    '        state = dr(0).Item("state")

    '        'if current class's state is collpase, apply filter
    '        'to show subclasses
    '        If LCase(state) = "collapse" Then
    '            dr(0).Item("state") = "expand"
    '            classFilter &= " OR groupClasss = '" & taxClass & "'"
    '        Else
    '            'if tax class is expanded, remove filter
    '            'to hide sub classes
    '            dr(0).Item("state") = "collapse"
    '            classFilter = Replace(classFilter, " OR groupClasss = '" & taxClass & "'", "")
    '        End If

    '    End If

    '    Return classFilter

    'End Function

    ''GetYears()
    ''Accepts no parameter
    ''Returns an array string; present year to 4 years ago
    'Public Shared Function GetYears() As String()
    '    Dim years(9) As String
    '    Dim currentYear As Integer
    '    Dim counter As Integer

    '    currentYear = Year(Now())
    '    years(0) = "<Select>"
    '    counter = 1

    '    For currentYear = currentYear - 4 To currentYear + 4 Step +1
    '        years(counter) = CStr(currentYear)
    '        counter += 1
    '    Next

    '    Return years
    'End Function

    ''GetFilePath()
    ''Accepts an Integer and String as paramters
    ''Returns a string; directory path where file will be saved
    ''Sets the folder path where the file will be saved.  If the directory
    ''does not exists, automatically creates a folder
    'Public Shared Function GetFilePath(ByVal id As Integer, ByVal subFolder As String) As String
    '    Dim strPath As String

    '    Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")

    '    strPath = PATMAP.Global_asax.FileRootPath & subFolder & "\"

    '    If Not Directory.Exists(strPath) Then
    '        Directory.CreateDirectory(strPath)
    '    End If

    '    strPath &= id & "\"

    '    If Not Directory.Exists(strPath) Then
    '        Directory.CreateDirectory(strPath)
    '    End If

    '    Impersonate.undoImpersonation()

    '    Return strPath

    'End Function

    ''DeleteFile()
    ''Accepts an Integer and Two String as paramters
    ''Deletes the file from the directory
    'Public Shared Sub DeleteFile(ByVal id As Integer, ByVal filename As String, ByVal subFolder As String)
    '    Dim strPath As String

    '    strPath = GetFilePath(id, subFolder) & filename

    '    Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")

    '    If File.Exists(strPath) Then
    '        File.Delete(strPath)
    '    End If

    '    Impersonate.undoImpersonation()
    'End Sub

    ''GetFilename()
    ''Accepts a FileUpload control and Integer paramters
    ''Returns a string; filename
    ''Checks to make sure the filename is unique. 
    'Public Shared Function GetFilename(ByRef fileUpload As System.Web.UI.WebControls.FileUpload, ByVal id As Integer, ByVal subFolder As String) As String()

    '    'Check if the user selected a file
    '    If Not fileUpload.HasFile Then
    '        Return Nothing
    '    End If

    '    'Get the name and the type of the file
    '    Dim filePath As String = fileUpload.PostedFile.FileName
    '    Dim fileName As String = Path.GetFileName(filePath)
    '    Dim fileType As String = Path.GetExtension(filePath)


    '    If Not ValidateNoSpecialChar(fileName.Substring(0, (fileName.Length - fileType.Length)), "^!#$%&'*+-./\<>|:,;[]?{}""") Then
    '        Dim invalidFileName() As String = {"Invalid File Name"}
    '        Return invalidFileName
    '    End If

    '    Dim serverLocation(2) As String

    '    serverLocation(0) = common.GetFilePath(id, subFolder) & fileName
    '    serverLocation(1) = Mid(fileName, 1, fileName.IndexOf("."))

    '    Dim counter As Integer = 1

    '    'If there's a duplicate filename, change the name
    '    While File.Exists(serverLocation(0))
    '        counter += 1
    '        serverLocation(1) = serverLocation(1) & "(" & counter & ")" & fileType
    '        serverLocation(0) = common.GetFilePath(id, subFolder) & serverLocation(1)
    '    End While

    '    'If there's no duplicate
    '    If counter = 1 Then
    '        serverLocation(1) = fileName
    '    End If

    '    serverLocation(1) = Replace(serverLocation(1), "'", "''")

    '    Return serverLocation

    'End Function

    ''SetButtonLink()
    ''Accepts GridViewRowEventArgs, Integer and String as parameters
    ''Attaches script to open a new window for file download
    'Public Shared Function SetButtonLink(ByRef e As System.Web.UI.WebControls.GridViewRowEventArgs, ByVal colNo As Integer, ByVal text As String, ByVal queryString As String) As System.Web.UI.WebControls.HyperLink
    '    Dim currentButton As System.Web.UI.WebControls.HyperLink = Nothing

    '    'if current row is a datarow type
    '    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '        currentButton = CType(e.Row.Cells.Item(colNo).Controls.Item(0), HyperLink)
    '        currentButton.NavigateUrl = "javascript: openWindow('../downloadfile.aspx?" & queryString & "','left=20,top=20,width=100,height=100,toolbar=0,resizable=0')"
    '    End If

    '    Return currentButton
    'End Function

    ''FormatDecimal()
    ''Accepts a DataTable, Two Strings, and a Boolean (Optional)
    ''Formats the decimal places of DataTable columns
    'Public Shared Sub FormatDecimal(ByRef dr() As DataRow, ByVal columns As String, ByVal decimalPlaces As String, Optional ByVal keepZeros As Boolean = False)
    '    Dim i, j As Integer
    '    Dim value As Decimal
    '    Dim colNames() As String = columns.Split(",")           'Column names in the DataTable
    '    Dim decPlace() As String = decimalPlaces.Split(",")     'Number of decimal places for each column
    '    Dim decPlaces As Integer
    '    Dim currentDecPlace As Integer

    '    If colNames.Length > 0 Then

    '        'Iterates through the DataTable column listed
    '        For j = 0 To colNames.Length - 1

    '            'If there's number of decimal places provided per column
    '            'If not, use the last decimal place format for the remaining columns
    '            If j < decPlace.Length Then
    '                currentDecPlace = CInt(decPlace(j))
    '            End If

    '            decPlaces = 10 ^ currentDecPlace

    '            'Iterates through each record for current column
    '            For i = 0 To dr.Length - 1
    '                value = dr(i).Item(colNames(j))

    '                If currentDecPlace > 0 Then
    '                    'If trailing zeros are kept, just round the value to the nearest decimal place
    '                    If Not keepZeros Then
    '                        value = Math.Truncate(Math.Round(value, currentDecPlace) * decPlaces) / decPlaces
    '                    Else
    '                        value = Math.Round(value, currentDecPlace)
    '                    End If
    '                Else
    '                    value = Math.Truncate(Math.Round(value, currentDecPlace))
    '                End If


    '                dr(i).Item(colNames(j)) = value
    '            Next
    '        Next
    '    End If

    'End Sub

    ''GetPermission()
    ''Accepts an Integer; user level ID
    ''Returns a DataTable
    ''Gets access permission for currently logged in user
    'Public Shared Function GetPermission(ByVal levelID As Integer) As DataTable
    '    Dim con As New SqlClient.SqlConnection
    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim query As String

    '    If levelID <> 0 Then
    '        con.ConnectionString = PATMAP.Global_asax.connString

    '        con.Open()

    '        query = "select screenName, pageAddress, screenNames.sectionID, screenNames.screenNameID" & vbCrLf & _
    '                    "from screenNames" & vbCrLf & _
    '                    "inner join sections on sections.sectionID = screenNames.sectionID" & vbCrLf & _
    '                    "inner join levelsPermission on levelsPermission.screenNameID = screenNames.screenNameID" & vbCrLf

    '        'check to see if user has entered LTT module from Boundary 
    '        ' if not - query continues as usual.  If so, the WHERE clause is updated
    '        'to excluse access to the Phase-In (screenNameID:111) and Base Year (screenNameID:106) pages
    '        If IsNothing(HttpContext.Current.Session("boundarySelection")) Then
    '            query += "where access = 1 and levelID = " & levelID & " and sections.sectionID <> 1" & vbCrLf
    '        Else
    '            query += "where access = 1 and levelID = " & levelID & " and sections.sectionID <> 1 and levelsPermission.screenNameID not in (106,111)" & vbCrLf
    '        End If

    '        '***Inky's Addition: Apr-2010 ***
    '        'screens 6 & 47 represent K12 and TaxCredits, respectively. Both have been removed from the program.
    '        query += "and screenNames.screenNameID not in ('6','47', '90')" & vbCrLf '***Inky (May-2010) Added ID 90 to the mix to remove access to Satellize screen as per re-Tooling
    '        '***Inky: End ***

    '        query += "union" & vbCrLf & _
    '                    "select sectionName As screenName, '' As pageAddress, sectionID, 0 As screenNameID" & vbCrLf & _
    '                    "from sections" & vbCrLf & _
    '                    "where sectionID <> 1" & vbCrLf & _
    '                    "order by sectionID, screenNameID"

    '        da.SelectCommand = New SqlClient.SqlCommand()
    '        da.SelectCommand.Connection = con
    '        da.SelectCommand.CommandText = query
    '        da.Fill(dt)

    '        con.Close()
    '    End If

    '    Return dt

    'End Function

    ''HasAccess()
    ''Accepts an Integer and a String as parameters
    ''Returns a string; error code
    ''Determines whether the user has access to a page
    'Public Shared Function HasAccess(ByVal levelID As Integer, ByVal url As String) As String
    '    Dim con As New SqlClient.SqlConnection
    '    Dim com As New SqlClient.SqlCommand
    '    Dim dr As SqlClient.SqlDataReader
    '    Dim access As String = ""
    '    Dim query As String

    '    'added for tweaking security for MapCalculate.aspx page same like Map.aspx security 17-jul-2014
    '    If url.IndexOf("MapCalculate") > -1 Then
    '        url = Replace(url, "MapCalculate", "Map")
    '    End If

    '    If levelID <> 0 Then
    '        con.ConnectionString = PATMAP.Global_asax.connString

    '        con.Open()

    '        query = "select count(*), screenNames.sectionID" & vbCrLf & _
    '                "from levelsPermission" & vbCrLf & _
    '                "inner join screenNames on screenNames.screenNameID = levelsPermission.screenNameID" & vbCrLf & _
    '                "where screenNames.pageAddress like '%" & url & "%' and levelsPermission.access =  1 and levelsPermission.levelID = " & levelID & vbCrLf & _
    '                "group by screenNames.sectionID"

    '        com.Connection = con
    '        com.CommandText = query
    '        dr = com.ExecuteReader

    '        If dr.Read() Then
    '            'Checks to see if there's an active boundary model
    '            If dr.Item(1) = 4 Then
    '                dr.Close()

    '                query = "select count(*) from boundaryModel where status = 1"
    '                com.CommandText = query
    '                dr = com.ExecuteReader

    '                If dr.Read() Then
    '                    If dr.Item(0) = 0 Then
    '                        access = "PATMAP63"
    '                    End If
    '                End If
    '            End If
    '        Else
    '            access = "403"
    '        End If

    '        dr.Close()
    '        con.Close()
    '    End If

    '    Return access

    'End Function

    ''GetModelNames()
    ''Accepts Two Strings and an Integer parameters
    ''Gets Tax Year Model names used by the scenario 
    'Public Shared Sub GetModelNames(ByRef baseModelName As String, ByRef subjectModelName As String, ByVal userID As Integer, ByRef txtScenarioName As String)

    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()

    '    Dim query As String
    '    Dim com As New SqlClient.SqlCommand
    '    Dim dr As SqlClient.SqlDataReader

    '    com.Connection = con

    '    'get base tax year model and subject tax year model names
    '    query = "select t.taxYearModelName as baseTaxYearModeName, t1.taxYearModelName as subjectTaxYearModelName, l.assessmentTaxModelName "
    '    query += "from liveAssessmentTaxModel l "
    '    query += "join taxYearModelDescription t on l.baseTaxYearModelID = t.taxYearModelID "
    '    query += "join taxYearModelDescription t1 on l.subjectTaxYearModelID = t1.taxYearModelID "
    '    query += "where l.userID=" & userID

    '    com.CommandText = query
    '    dr = com.ExecuteReader()

    '    If dr.Read() Then
    '        baseModelName = dr.Item(0)
    '        subjectModelName = dr.Item(1)
    '        If IsDBNull(dr.Item(2)) Then
    '            txtScenarioName = ""
    '        Else
    '            txtScenarioName = dr.Item(2)
    '        End If
    '    End If

    '    'clean up
    '    dr.Close()
    '    con.Close()
    'End Sub

    ''ValidateEmail  
    ''--The user name (befor @) can have alphanumeric, _, ., -
    ''--The valid email will have '@' after the user name    
    'Public Shared Function ValidateEmail(ByVal email As String) As Boolean
    '    Return Regex.IsMatch(email, "^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$")
    'End Function

    'Public Shared Function ValidatePassword(ByVal password As String) As Boolean
    '    Return Regex.IsMatch(password, "^(([A-Za-z]+[^0-9]*)([0-9]+[^\W]*)([\W]+[\W0-9A-Za-z]*))|(([A-Za-z]+[^\W]*)([\W]+[^0-9]*)([0-9]+[\W0-9A-Za-z]*))|(([\W]+[^A-Za-z]*)([A-Za-z]+[^0-9]*)([0-9]+[\W0-9A-Za-z]*))|(([\W]+[^0-9]*)([0-9]+[^A-Za-z]*)([A-Za-z]+[\W0-9A-Za-z]*))|(([0-9]+[^A-Za-z]*)([A-Za-z]+[^\W]*)([\W]+[\W0-9A-Za-z]*))|(([0-9]+[^\W]*)([\W]+[^A-Za-z]*)([A-Za-z]+[\W0-9A-Za-z]*))$")
    'End Function

    ''ValidatePostalCode   
    'Public Shared Function ValidatePostalCode(ByVal postalCode As String) As Boolean
    '    Return Regex.IsMatch(postalCode, "^[A-Z][0-9][A-Z][\s][0-9][A-Z][0-9]$")
    'End Function

    ''ValidateRange
    'Public Shared Function ValidateRange(ByVal validateNum As Object, Optional ByVal minValue As Object = Nothing, Optional ByVal maxValue As Object = Nothing) As Boolean
    '    If Not IsNothing(minValue) And Not IsNothing(maxValue) Then
    '        Return validateNum >= minValue And validateNum <= maxValue
    '    ElseIf Not IsNothing(minValue) Then
    '        Return validateNum >= minValue
    '    ElseIf Not IsNothing(maxValue) Then
    '        Return validateNum <= maxValue
    '    End If

    'End Function

    ''ValidatePhoneFaxNumber
    'Public Shared Function ValidatePhoneFaxNumber(ByVal phoneFaxNumber As String, ByVal extension As String) As Boolean
    '    If Regex.IsMatch(phoneFaxNumber, "^\d{3}\d{3}\d{4}$") Then
    '        If extension = "" Or Regex.IsMatch(extension, "^[0-9]*$") Then
    '            Return True
    '        End If
    '    End If
    '    Return False
    'End Function

    ''ValidateNoSpecialChar
    'Public Shared Function ValidateNoSpecialChar(ByVal input As String, Optional ByVal invalidChars As String = "^~`!@#$%&*_+={}|[]\:"";<>?,./*") As Boolean
    '    Dim index As Integer
    '    index = input.IndexOfAny(invalidChars.ToCharArray())

    '    If index = -1 Then
    '        Return True
    '    End If

    '    Return False
    'End Function

    'Public Shared Function RangeErrorMsg(Optional ByVal minValue As Object = Nothing, Optional ByVal maxValue As Object = Nothing) As String
    '    Dim errorMsg As String = ""

    '    If Not IsNothing(minValue) And Not IsNothing(maxValue) Then
    '        errorMsg = GetErrorMessage("PATMAP74")
    '        errorMsg = Replace(errorMsg, "@minValue", minValue)
    '        errorMsg = Replace(errorMsg, "@maxValue", maxValue)
    '    Else
    '        If Not IsNothing(minValue) Then
    '            errorMsg = Replace(GetErrorMessage("PATMAP82"), "@minValue", minValue)
    '        End If

    '        If Not IsNothing(maxValue) Then
    '            errorMsg = Replace(GetErrorMessage("PATMAP83"), "@maxValue", maxValue)
    '        End If

    '    End If

    '    Return errorMsg
    'End Function

    'Public Shared Sub moveAssessmentFiles(ByVal assessmentTaxModelID As Integer, ByVal userID As Integer)

    '    Dim destinationPath As String
    '    Dim originalPath As String
    '    Dim strPath As String

    '    Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
    '    strPath = PATMAP.Global_asax.FileRootPath & general.subFolder & "\" & assessmentTaxModelID & "\"
    '    If Directory.Exists(strPath) Then
    '        Directory.Delete(strPath, True)
    '    End If
    '    Impersonate.undoImpersonation()

    '    originalPath = common.GetFilePath(userID, general.liveSubFolder)
    '    destinationPath = common.GetFilePath(assessmentTaxModelID, general.subFolder)

    '    Dim dir As New DirectoryInfo(originalPath)
    '    Dim dirFiles As FileInfo() = dir.GetFiles()
    '    Dim currentFile As IO.FileInfo
    '    Dim destinationFile As String
    '    Dim originationFile As String

    '    Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
    '    For Each currentFile In dirFiles
    '        originationFile = originalPath & currentFile.ToString()
    '        destinationFile = destinationPath & currentFile.ToString()
    '        File.Move(originationFile, destinationFile)
    '    Next
    '    Impersonate.undoImpersonation()

    'End Sub

    'Public Shared Function UpdateScenarioName(ByVal userID As Integer, ByVal assessmentID As Integer, ByVal newName As String) As String
    '    Dim auditTrail As String = ""
    '    Dim errorCode As String = ""

    '    newName = Trim(newName.Replace("'", "''"))

    '    If newName = "" Then
    '        errorCode = ""
    '        Return errorCode
    '    End If

    '    If Not ValidateNoSpecialChar(newName) Then
    '        errorCode = "PATMAP100"
    '        Return errorCode
    '    End If

    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()

    '    Dim trans As SqlClient.SqlTransaction
    '    trans = con.BeginTransaction

    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    query.Transaction = trans

    '    Dim dr As SqlClient.SqlDataReader

    '    query.CommandText = "select count(*) from liveAssessmentTaxModel latm inner join AssessmentTaxModel atm on atm.assessmentTaxModelName = latm.assessmentTaxModelName and atm.assessmentTaxModelID <> " & assessmentID & " where (latm.assessmentTaxModelName = '" & newName & "' and latm.userID <> " & userID & ") or (atm.assessmentTaxModelName = '" & newName & "')"

    '    dr = query.ExecuteReader()
    '    dr.Read()

    '    If dr.Item(0) <> 0 Then
    '        dr.Close()
    '        errorCode = "PATMAP107"
    '        Return errorCode
    '    End If

    '    dr.Close()

    '    query.CommandText = "select count(*) from AssessmentTaxModel atm where atm.assessmentTaxModelName = '" & newName & "' and atm.assessmentTaxModelID <> " & assessmentID

    '    dr = query.ExecuteReader()
    '    dr.Read()

    '    If dr.Item(0) <> 0 Then
    '        dr.Close()
    '        errorCode = "PATMAP107"
    '        Return errorCode
    '    End If

    '    dr.Close()

    '    query.CommandText = "select assessmentTaxModelName, auditTrailText from liveAssessmentTaxModel where userID = " & userID
    '    dr = query.ExecuteReader()
    '    dr.Read()

    '    auditTrail = "[" & Now.ToString("MM/dd/yyyy") & "]"

    '    If Not IsDBNull(dr.Item(0)) Then
    '        If dr.Item(0) = "" Then
    '            auditTrail &= "Set Scenario Name to " & newName
    '        Else
    '            auditTrail &= "Scenario Name changed from " & dr.Item(0).ToString.Replace("'", "''") & " to " & newName
    '        End If

    '        query.CommandText = "update liveAssessmentTaxModel set auditTrailText='" & auditTrail & vbCrLf & dr.Item(1).ToString.Replace("'", "''") & "', assessmentTaxModelName = '" & newName & "' where userID=" & userID.ToString & vbCrLf


    '        dr.Close()

    '        Try
    '            query.ExecuteNonQuery()
    '            trans.Commit()
    '        Catch
    '            trans.Rollback()
    '            errorCode = "PATMAP102"
    '        End Try
    '    End If

    '    con.Close()

    '    Return errorCode
    'End Function

    'Public Shared Function FillMunicipality(ByVal JurisType As Integer, Optional ByVal includeSchool As Boolean = False, Optional ByVal jurisGroup As Integer = 0)
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString

    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim filter As String = ""
    '    Dim selectName As String = ""

    '    da.SelectCommand = New SqlClient.SqlCommand()
    '    da.SelectCommand.Connection = con

    '    'list includes municipality and school division
    '    If includeSchool Then
    '        selectName = "'<Select>'"
    '        If JurisType > 0 Then
    '            filter += " and j.jurisdictionTypeID = " & JurisType
    '        End If
    '    Else
    '        selectName = "'--Municipality--'"
    '        'list contains municipality only
    '        If JurisType > 1 Then
    '            filter += " and j.jurisdictionTypeID = " & JurisType
    '        Else
    '            filter = " and j.jurisdictionTypeID > 1"
    '        End If
    '    End If

    '    'user selected a municipality jurisdiction type and a jurisdiction type group
    '    'school division doesn't have a jurisdiction type group
    '    If jurisGroup > 0 Then
    '        filter += " and j.jurisdictionGroupID = " & jurisGroup
    '    End If

    '    dt.Clear()
    '    da.SelectCommand.CommandText = "select 0 as jurisdictionTypeID, ' ' as number, " & selectName & " as jurisdiction "
    '    da.SelectCommand.CommandText += "union all select j.jurisdictionTypeID, ' ', '--' + jurisdictionType + '--' from jurisdictionTypes j where 1=1 " + filter
    '    da.SelectCommand.CommandText += "union select e.jurisdictionTypeID, e.number, '[' + e.number + '] ' + dbo.ProperCase(e.jurisdiction) as jurisdiction from entities e, jurisdictionTypes j where e.jurisdictionTypeID = j.jurisdictionTypeID " + filter + " order by jurisdictionTypeID"
    '    da.Fill(dt)


    '    Return dt
    'End Function


    'Public Shared Sub copyLiveModelTableFiles(ByVal ID As Integer, ByVal newID As Integer, ByVal subFolder As String)

    '    Dim destinationPath As String
    '    Dim originalPath As String
    '    Dim strPath As String

    '    Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
    '    strPath = PATMAP.Global_asax.FileRootPath & subFolder & "\" & newID & "\"
    '    If Directory.Exists(strPath) Then
    '        Directory.Delete(strPath, True)
    '    End If
    '    Impersonate.undoImpersonation()

    '    originalPath = common.GetFilePath(ID, subFolder)
    '    destinationPath = common.GetFilePath(newID, subFolder)

    '    Dim dir As New DirectoryInfo(originalPath)
    '    Dim dirFiles As FileInfo() = dir.GetFiles()
    '    Dim currentFile As IO.FileInfo
    '    Dim destinationFile As String
    '    Dim originationFile As String

    '    Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
    '    For Each currentFile In dirFiles
    '        originationFile = originalPath & currentFile.ToString()
    '        destinationFile = destinationPath & currentFile.ToString()
    '        File.Copy(originationFile, destinationFile)
    '    Next
    '    Impersonate.undoImpersonation()

    'End Sub
    'Public Shared Sub transferParcels(ByVal userID As Integer, ByVal ddlSubjMun As String, ByVal ddlDestinationMun As String, ByVal ddlOriginMun As String, ByVal boundaryGroupId As Integer, ByVal txtGroupName As String, ByVal txtParcelNo As String, ByVal rblRestructuredLevy As Boolean, ByVal map As Boolean, ByRef grdProperties As GridView, ByRef grdAssessment As GridView, ByRef grdDuplicateProp As GridView, ByRef errorMsg As String)
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    Dim dr As SqlClient.SqlDataReader
    '    'variables to hold ID's
    '    Dim assessmentID As Integer
    '    Dim millRateSurveyID As Integer
    '    Dim POVID As Integer
    '    Dim taxFormula As String
    '    Dim assessmentFormula As String

    '    query.CommandText = "select assessmentID, millRateSurveyID, POVID, (select formula from dbo.[functions] where functionID = 94) as taxFormula, (select formula from dbo.[functions] where functionID = 91) as assessmentFormula from boundaryModel where status = 1"
    '    dr = query.ExecuteReader
    '    dr.Read()
    '    assessmentID = dr.GetValue(0)
    '    millRateSurveyID = dr.GetValue(1)
    '    POVID = dr.GetValue(2)
    '    If Not IsDBNull(dr.GetValue(3)) Then
    '        taxFormula = dr.GetValue(3)
    '    Else
    '        taxFormula = "0"
    '    End If
    '    If Not IsDBNull(dr.GetValue(4)) Then
    '        assessmentFormula = dr.GetValue(4)
    '    Else
    '        assessmentFormula = "0"
    '    End If
    '    dr.Close()

    '    'check if this is the first group entered
    '    query.CommandText = "select userID from boundaryGroups where userid = " & userID.ToString
    '    dr = query.ExecuteReader()
    '    If Not dr.Read() Then
    '        dr.Close()

    '        'get original levy for subject municipality
    '        query.CommandText = "  select " & assessmentFormula & " as marketValue, " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & " as originalLevy"
    '        'query.CommandText = "  select " & assessmentFormula & " as marketValue, " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & " as originalLevy, " & taxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate") & " as originLevy"
    '        query.CommandText += " from assessment"
    '        query.CommandText += "       inner join pov on assessment.taxclassID = pov.taxclassID "
    '        query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu1 on rtrim(ltrim(bu1.municipalityID)) = rtrim(ltrim('" & ddlSubjMun & "'))"
    '        'query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu2 on rtrim(ltrim(bu2.municipalityID)) = rtrim(ltrim('" & ddlOriginMun & "'))"
    '        query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = rtrim(ltrim('" & ddlSubjMun & "'))"
    '        dr = query.ExecuteReader()
    '        dr.Read()

    '        Dim marketValue As Double = dr.GetValue(0)
    '        Dim originalLevy As Double = dr.GetValue(1)
    '        'Dim originLevy As Double = dr.GetValue(2)
    '        dr.Close()

    '        'create group for subject municipality
    '        'for subject municipality the origin levy is same as the original levy (there is no two diferent millrates to calc the levy for the subject alone)
    '        query.CommandText = "insert into boundaryGroups (userID, boundaryGroupName, SubjectMunicipalityID, OriginMunicipalityID, DestinationMunicipalityID, restructuredLevyCombined, assessment, originalLevy, restructuredLevy,uniformMillRate,originLevy) VALUES (" & userID.ToString & ",'Subject','" & ddlSubjMun & "','" & ddlOriginMun & "','" & ddlDestinationMun & "',1," & marketValue & "," & originalLevy & " ," & originalLevy & "," & originalLevy / (marketValue / 1000) & "," & originalLevy & ")"
    '        query.ExecuteNonQuery()
    '    Else
    '        dr.Close()
    '    End If

    '    'check if this is a new boundary group
    '    If boundaryGroupId = 0 Then
    '        'create new boundary group
    '        If rblRestructuredLevy Then
    '            query.CommandText = "insert into boundaryGroups (userID, boundaryGroupName, SubjectMunicipalityID, OriginMunicipalityID, DestinationMunicipalityID, restructuredLevyCombined) VALUES (" & userID.ToString & ",'" & txtGroupName & "','" & ddlSubjMun & "','" & ddlOriginMun & "','" & ddlDestinationMun & "',1) select @@IDENTITY"
    '        Else
    '            query.CommandText = "insert into boundaryGroups (userID, boundaryGroupName, SubjectMunicipalityID, OriginMunicipalityID, DestinationMunicipalityID, restructuredLevyCombined) VALUES (" & userID.ToString & ",'" & txtGroupName & "','" & ddlSubjMun & "','" & ddlOriginMun & "','" & ddlDestinationMun & "',0) select @@IDENTITY"
    '        End If
    '        dr = query.ExecuteReader()
    '        dr.Read()
    '        boundaryGroupId = dr.GetValue(0)
    '        dr.Close()
    '    End If

    '    'add proporties to list of transfered properties
    '    query.CommandText = "  insert into boundarytransfers "
    '    If ddlSubjMun = ddlOriginMun Then
    '        query.CommandText += " select " & boundaryGroupId.ToString & ", alternate_parcelID, -1 * " & assessmentFormula & " , -1 * " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & ", -1 * " & taxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate")
    '    Else
    '        query.CommandText += " select " & boundaryGroupId.ToString & ", alternate_parcelID, " & assessmentFormula & " , " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & ", " & taxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate")
    '    End If
    '    query.CommandText += " from assessment"
    '    query.CommandText += "       inner join pov on assessment.taxclassID = pov.taxclassID"
    '    query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu1 on rtrim(ltrim(bu1.municipalityID)) = rtrim(ltrim('" & ddlSubjMun & "'))"
    '    query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu2 on rtrim(ltrim(bu2.municipalityID)) = rtrim(ltrim('" & ddlOriginMun & "'))"
    '    'query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = ltrim(rtrim('" & ddlOriginMun & "')) and alternate_parcelID like '" & txtParcelNo & "%' and alternate_parcelID not in (select alternate_parcelID from boundaryTransfers where boundaryGroupID = " & boundaryGroupId.ToString & ")"
    '    query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = ltrim(rtrim('" & ddlOriginMun & "')) and alternate_parcelID "
    '    If map = 0 Then
    '        query.CommandText += " like '" & txtParcelNo & "%'"
    '    Else
    '        query.CommandText += " in (" & txtParcelNo & ")"
    '    End If
    '    query.CommandText += "and alternate_parcelID not in (select alternate_parcelID from boundaryGroups bg inner join boundaryTransfers bt on bg.boundaryGroupID = bt.boundaryGroupID  where bg.userID = " & userID & " and bg.boundaryGroupName not in ('Subject'))"
    '    query.CommandText += " group by alternate_parcelID"

    '    query.ExecuteNonQuery()

    '    'update the data satle flag to true
    '    query.CommandText = "update liveBoundaryModel set mapDataStale=1 where userid=" & userID.ToString
    '    query.ExecuteNonQuery()

    '    'check the overlap
    '    common.populateDuplicateGrd(userID, assessmentID, txtGroupName, ddlOriginMun, txtParcelNo, map, grdDuplicateProp, errorMsg)

    '    'update the assessment and levy data of the group
    '    common.updateBoundaryGroup(boundaryGroupId, userID, "load", ddlSubjMun, ddlOriginMun, errorMsg)

    '    'clear editting value if set
    '    'grdAssessment.EditIndex = -1

    '    'populate assessment and tax shift table
    '    common.populateAssessmentTaxTable(userID, grdAssessment)

    '    common.updateLinearPropertyAdjustment(boundaryGroupId, userID, assessmentID, rblRestructuredLevy, ddlSubjMun, ddlOriginMun)
    '    'clear the parcel number or prefix
    '    'txtParcelNo.Text = ""

    'End Sub
    'Public Shared Sub updateBoundaryGroup(ByVal boundaryGroupID As Integer, ByVal userID As Integer, ByVal mode As String, ByVal SubjMunID As String, ByVal OriginMunID As String, ByRef errorMsg As String)

    '    Try
    '        'setup database connection
    '        Dim con As New SqlClient.SqlConnection
    '        con.ConnectionString = PATMAP.Global_asax.connString
    '        con.Open()
    '        Dim query As New SqlClient.SqlCommand
    '        query.Connection = con

    '        'update the groups assessment and levy totals
    '        query.CommandText = "  update boundaryGroups "
    '        query.CommandText += " set assessment = isnull((select sum(assessment) from boundarytransfers where boundaryGroupID = " & boundaryGroupID.ToString & "),0.0)"
    '        query.CommandText += " , originalLevy = isnull((select sum(levy) from boundarytransfers where boundaryGroupID = " & boundaryGroupID.ToString & "),0.0)"
    '        query.CommandText += " , originLevy = isnull((select sum(originlevy) from boundarytransfers where boundaryGroupID = " & boundaryGroupID.ToString & "),0.0)"

    '        If mode = "load" Or mode = "delete" Then
    '            query.CommandText += " , restructuredLevy = case when restructuredLevyCombined = 1 then isnull(restructuredLevy,0) else isnull((select sum(levy) from boundarytransfers where boundaryGroupID = " & boundaryGroupID.ToString & "),0.0) end"
    '        Else
    '            query.CommandText += " , restructuredLevy = case when restructuredLevyCombined = 1 then 0 else isnull(restructuredLevy,0) end"
    '        End If

    '        query.CommandText += " where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID.ToString
    '        query.ExecuteNonQuery()

    '        query.CommandText = "  update boundaryGroups "
    '        query.CommandText += " set uniformMillRate = case when assessment = 0 then 0 else restructuredLevy/(assessment/1000) end"
    '        query.CommandText += " where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID.ToString
    '        query.ExecuteNonQuery()

    '        'get levy adjustment for subject municipality
    '        query.CommandText = "  select isnull(sum(assessmentAdjustment),0.0) as assessmentAdjustment, isnull(sum(groupLevyAdjustment),0.0) as groupLevyAdjustment, isnull(sum(restructuredLevyAdjustment),0.0) as restructuredLevyAdjustment"
    '        query.CommandText += " from"
    '        query.CommandText += " ("
    '        query.CommandText += "      select case when SubjectMunicipalityID = OriginMunicipalityID then assessment else 0.0 end as assessmentAdjustment, case when SubjectMunicipalityID = OriginMunicipalityID then originalLevy else 0.0 end as groupLevyAdjustment, case when restructuredLevyCombined = 0 then case when SubjectMunicipalityID = OriginMunicipalityID then originalLevy else 0.0 end else originalLevy end as restructuredLevyAdjustment"
    '        query.CommandText += "      from boundaryGroups temp1"
    '        query.CommandText += "      where boundaryGroupID in (select boundaryGroupID from boundaryGroups where userid = " & userID.ToString & " and boundaryGroupName <> 'Subject'"
    '        If SubjMunID = OriginMunID And mode = "load" Then
    '            query.CommandText += " and boundaryGroupID = " & boundaryGroupID
    '        End If
    '        query.CommandText += " )) temp1"
    '        Dim dr As SqlClient.SqlDataReader = query.ExecuteReader()
    '        dr.Read()
    '        Dim assessmentAdjustment As Double = dr.GetValue(0)
    '        Dim groupLevyAdjustment As Double = dr.GetValue(1)
    '        Dim restructuredLevyAdjustment As Double = dr.GetValue(2)
    '        dr.Close()

    '        'if seperate gain
    '        If restructuredLevyAdjustment = 0 Then

    '            'variables to hold ID's
    '            Dim assessmentID As Integer
    '            Dim millRateSurveyID As Integer
    '            Dim POVID As Integer
    '            Dim taxFormula As String
    '            Dim assessmentFormula As String

    '            query.CommandText = "select assessmentID, millRateSurveyID, POVID, (select formula from dbo.[functions] where functionID = 94) as taxFormula, (select formula from dbo.[functions] where functionID = 91) as assessmentFormula from boundaryModel where status = 1"
    '            dr = query.ExecuteReader
    '            dr.Read()
    '            assessmentID = dr.GetValue(0)
    '            millRateSurveyID = dr.GetValue(1)
    '            POVID = dr.GetValue(2)
    '            If Not IsDBNull(dr.GetValue(3)) Then
    '                taxFormula = dr.GetValue(3)
    '            Else
    '                taxFormula = "0"
    '            End If
    '            If Not IsDBNull(dr.GetValue(4)) Then
    '                assessmentFormula = dr.GetValue(4)
    '            Else
    '                assessmentFormula = "0"
    '            End If
    '            dr.Close()


    '            'get original levy for subject municipality                
    '            query.CommandText = "  select " & assessmentFormula & " as marketValue, " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & " as originalLevy"
    '            'query.CommandText = "  select " & assessmentFormula & " as marketValue, " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & " as originalLevy, " & taxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate") & " as originLevy"
    '            query.CommandText += " from assessment"
    '            query.CommandText += "       inner join pov on assessment.taxclassID = pov.taxclassID "
    '            query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu1 on rtrim(ltrim(bu1.municipalityID)) = rtrim(ltrim('" & SubjMunID & "'))"
    '            'query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu2 on rtrim(ltrim(boundaryUniformMunicipalMillRate.municipalityID)) = rtrim(ltrim('" & ddlOriginMun.SelectedValue & "'))"
    '            query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = rtrim(ltrim('" & SubjMunID & "'))"
    '            dr = query.ExecuteReader()
    '            dr.Read()

    '            assessmentAdjustment = dr.GetValue(0)
    '            restructuredLevyAdjustment = dr.GetValue(1)
    '            'Dim originLevy As Double = dr.GetValue(2)
    '            dr.Close()

    '            'update the groups assessment and levy totals
    '            'for subject municipality the origin levy is same as the original levy (there is no two diferent millrates to calc the levy for the subject alone)
    '            query.CommandText = "  update boundaryGroups "
    '            query.CommandText += " set assessment = " & assessmentAdjustment & " ,originalLevy = " & restructuredLevyAdjustment & " ,restructuredLevy = " & restructuredLevyAdjustment & ",originLevy = " & restructuredLevyAdjustment
    '            query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
    '            query.ExecuteNonQuery()

    '        Else
    '            'if loss 
    '            If assessmentAdjustment <> 0 And groupLevyAdjustment <> 0 Then
    '                'update the groups assessment and levy totals
    '                query.CommandText = "  update boundaryGroups "
    '                query.CommandText += " set assessment = assessment + " & assessmentAdjustment & " ,originalLevy = originalLevy + " & groupLevyAdjustment & " ,restructuredLevy = restructuredLevy + " & restructuredLevyAdjustment & ",originLevy = originalLevy + " & groupLevyAdjustment
    '                query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
    '                query.ExecuteNonQuery()
    '            Else
    '                'if combined(gain)
    '                'update the groups assessment and levy totals
    '                query.CommandText = "  update boundaryGroups "
    '                query.CommandText += " set assessment = assessment + " & assessmentAdjustment & " ,originalLevy = originalLevy + " & groupLevyAdjustment & " ,restructuredLevy = originalLevy + " & restructuredLevyAdjustment & ",originLevy = originalLevy + " & groupLevyAdjustment
    '                query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
    '                query.ExecuteNonQuery()
    '            End If
    '        End If

    '        'update the uniform mill rate 
    '        query.CommandText = "  update boundaryGroups "
    '        query.CommandText += " set uniformMillRate = restructuredLevy/((select sum(assessment) from boundaryGroups where restructuredLevyCombined = 1 and userID = " & userID.ToString & ")/1000)"
    '        query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
    '        query.ExecuteNonQuery()
    '        query.ExecuteNonQuery()
    '    Catch
    '        'retrieves error message

    '        Dim Master As New PATMAP.MasterPage
    '        errorMsg = GetErrorMessage(Err.Number, Err)
    '    End Try

    'End Sub
    'Public Shared Sub populateAssessmentTaxTable(ByVal userID As Integer, ByRef grdAssessment As GridView)
    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()

    '    'fill the users drop down
    '    Dim da As New SqlClient.SqlDataAdapter
    '    da.SelectCommand = New SqlClient.SqlCommand
    '    da.SelectCommand.Connection = con
    '    da.SelectCommand.CommandText = "select rtrim(ltrim(originMunicipalityID)) as originMunicipalityID, rtrim(ltrim(DestinationMunicipalityID)) as DestinationMunicipalityID, boundaryGroupID, boundaryGroupName, assessment, round(originalLevy,2) as originalLevy, case when boundaryGroupName = 'Subject' then 'Subject' when subjectMunicipalityID = originMunicipalityID then 'Lost' when restructuredLevyCombined = 1 then 'Combined' else 'Separate' end as LevyStatus, round(restructuredLevy,2) as restructuredLevy, originLevy from boundaryGroups where userid = " & userID & " order by boundaryGroupID"
    '    Dim dt As New DataTable
    '    da.Fill(dt)

    '    grdAssessment.DataSource = dt
    '    grdAssessment.DataBind()
    '    con.Close()
    'End Sub
    'Public Shared Sub populateDuplicateGrd(ByVal userID As Integer, ByVal assessmentID As Integer, ByVal txtGroupName As String, ByVal ddlOriginMun As String, ByVal txtParcelNo As String, ByVal map As Boolean, ByRef grdDuplicateProp As GridView, ByVal errorMsg As String)
    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()

    '    Dim da As New SqlClient.SqlDataAdapter
    '    da.SelectCommand = New SqlClient.SqlCommand
    '    da.SelectCommand.Connection = con
    '    da.SelectCommand.CommandTimeout = 6000
    '    da.SelectCommand.CommandText = "select bg.boundaryGroupID, bg.boundaryGroupName, bt.assessment, alternate_parcelID"
    '    da.SelectCommand.CommandText += " from boundaryGroups bg inner join boundaryTransfers bt on bg.boundaryGroupID = bt.boundaryGroupID"
    '    da.SelectCommand.CommandText += " where bg.userID = " & userID & " and bg.boundaryGroupName not in ('Subject','" & txtGroupName & "') and OriginMunicipalityID + cast(alternate_parcelID as varchar(8000)) in"
    '    da.SelectCommand.CommandText += " ("
    '    da.SelectCommand.CommandText += " select municipalityID + cast(alternate_parcelID as varchar(8000))"
    '    da.SelectCommand.CommandText += " from assessment"
    '    da.SelectCommand.CommandText += " where assessmentID = " & assessmentID & " and alternate_parcelID"
    '    If map = 0 Then
    '        da.SelectCommand.CommandText += " like '" & txtParcelNo & "%'"
    '    Else
    '        da.SelectCommand.CommandText += " in (" & txtParcelNo & ")"
    '    End If
    '    da.SelectCommand.CommandText += "and municipalityID = '" & ddlOriginMun & "')"


    '    Dim dt As New DataTable
    '    da.Fill(dt)

    '    grdDuplicateProp.DataSource = dt
    '    grdDuplicateProp.DataBind()

    '    'Session("dupProperties") = dt

    '    'if there are overlap display the grid with the duplicates
    '    'Dim Master As New PATMAP.MasterPage
    '    If dt.Rows.Count > 0 Then
    '        grdDuplicateProp.Visible = True
    '        errorMsg = GetErrorMessage("PATMAP321")
    '    Else
    '        'otherwise hide the grid
    '        grdDuplicateProp.Visible = False
    '        errorMsg = ""
    '    End If

    'End Sub
    'Public Shared Sub updateLinearPropertyAdjustment(ByVal boundaryGroupID As Integer, ByVal userID As Integer, ByVal assessmentID As Integer, ByVal RestructuredLevy As Boolean, ByVal SubjMunID As String, ByVal OriginMunID As String)

    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con

    '    Dim linearIncludedInBoundaryAssessment As Decimal = 0
    '    Dim linearIncludedInBoundaryLevy As Decimal = 0
    '    Dim originIncludedInBoundaryLevy As Decimal = 0

    '    'how much of the assessment is already lost or gained for pipeline and railway
    '    query.CommandText = " Select isnull(sum(bt.assessment),0), isnull(sum(bt.levy),0), isnull(sum(bt.originLevy),0)" & vbCrLf & _
    '                        " from boundaryGroups bg inner join boundaryTransfers bt on bg.boundaryGroupID = bt.boundaryGroupID" & vbCrLf & _
    '                        " inner join" & vbCrLf & _
    '                        " (" & vbCrLf & _
    '                        " select municipalityID, alternate_parcelID " & vbCrLf & _
    '                        " from assessment" & vbCrLf & _
    '                        " where assessmentID = " & assessmentID & " and municipalityID=CONVERT(CHAR,(select OriginMunicipalityID from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ")) and  assessment.taxClassID in ('P','PP')" & vbCrLf & _
    '                        " ) temp on temp.municipalityID = bg.OriginMunicipalityID and temp.alternate_parcelID = bt.alternate_parcelID" & vbCrLf & _
    '                        " where(bg.boundaryGroupID = " & boundaryGroupID & " and userID = " & userID & ")"
    '    Dim dr As SqlClient.SqlDataReader = query.ExecuteReader()
    '    dr.Read()

    '    linearIncludedInBoundaryAssessment = dr.GetValue(0)
    '    linearIncludedInBoundaryLevy = dr.GetValue(1)
    '    originIncludedInBoundaryLevy = dr.GetValue(2)

    '    'calulate the linear assessement to be transfered based on the percentage
    '    dr.Close()
    '    Dim linearAssessment As Decimal = 0
    '    Dim linearLevy As Decimal = 0
    '    Dim originLevy As Decimal = 0
    '    query.CommandText = "select isnull(sum(taxableAssessmentValue * percentageTransfer),0) from boundaryLinearTransfers where boundaryGroupID = " & boundaryGroupID & " and taxClassID in ('P','PP')"
    '    dr = query.ExecuteReader()
    '    If dr.Read() Then

    '        linearAssessment = dr.GetValue(0)
    '        dr.Close()
    '        query.CommandText = "select uniformMunicipalMillRate * " & (linearAssessment / 1000) & " from boundaryGroups bg inner join boundaryUniformMunicipalMillRate bu on bg.SubjectMunicipalityID=bu.municipalityID where bg.boundaryGroupName = 'Subject' and bg.userID = " & userID
    '        dr = query.ExecuteReader()
    '        dr.Read()
    '        linearLevy = dr.GetValue(0)

    '        dr.Close()
    '        query.CommandText = "select uniformMunicipalMillRate * " & (linearAssessment / 1000) & " from boundaryGroups bg inner join boundaryUniformMunicipalMillRate bu on bg.OriginMunicipalityID=bu.municipalityID where bg.boundaryGroupName = 'Subject' and bg.userID = " & userID
    '        dr = query.ExecuteReader()
    '        dr.Read()
    '        originLevy = dr.GetValue(0)


    '        'adjust the assessment and the groupLevy for the group
    '        dr.Close()
    '        query.CommandText = "update boundaryGroups set assessment = ((select assessment from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ") - (" & linearIncludedInBoundaryAssessment & ")) + " & linearAssessment & vbCrLf & _
    '                            ", originalLevy = ((select originalLevy from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ") - (" & linearIncludedInBoundaryLevy & ")) + " & linearLevy & vbCrLf & _
    '                            ", originLevy = ((select originLevy from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ") - (" & originIncludedInBoundaryLevy & ")) + " & originLevy & vbCrLf & _
    '                            " where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID
    '        query.ExecuteNonQuery()

    '        If OriginMunID <> SubjMunID Then
    '            'GAIN
    '            'combine - adjust the subject
    '            If RestructuredLevy = 0 Then
    '                query.CommandText = "update boundaryGroups set restructuredLevy = ((select restructuredLevy from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & userID & ") - " & linearIncludedInBoundaryLevy & ") + " & linearLevy & " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
    '                query.ExecuteNonQuery()

    '                query.CommandText = "  update boundaryGroups "
    '                query.CommandText += " set uniformMillRate = restructuredLevy/((select sum(assessment) from boundaryGroups where restructuredLevyCombined = 1 and userID = " & userID.ToString & ")/1000)"
    '                query.CommandText += " where userID = " & userID.ToString & " and boundaryGroupName = 'Subject'"
    '                query.ExecuteNonQuery()

    '            Else
    '                'separate - adjust the group
    '                query.CommandText = "update boundaryGroups set restructuredLevy = ((select restructuredLevy from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ") - " & linearIncludedInBoundaryLevy & ") + " & linearLevy & " where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID.ToString
    '                query.ExecuteNonQuery()

    '                query.CommandText = "  update boundaryGroups "
    '                query.CommandText += " set uniformMillRate = case when assessment = 0 then 0 else restructuredLevy/(assessment/1000) end"
    '                query.CommandText += " where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID.ToString
    '                query.ExecuteNonQuery()
    '            End If
    '        Else
    '            'LOST
    '            'adjust the subject
    '            query.CommandText = "update boundaryGroups set assessment = ((select assessment from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & userID & ") - (" & linearIncludedInBoundaryAssessment & ")) + " & linearAssessment
    '            query.CommandText += ", originalLevy = ((select originalLevy from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & userID & ") - (" & linearIncludedInBoundaryLevy & ")) + " & linearLevy
    '            query.CommandText += ", originLevy = ((select originalLevy from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & userID & ") - (" & linearIncludedInBoundaryLevy & ")) + " & linearLevy
    '            query.CommandText += ", restructuredLevy = ((select restructuredLevy from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & userID & ") - (" & linearIncludedInBoundaryLevy & ")) + " & linearLevy
    '            query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
    '            query.ExecuteNonQuery()

    '            query.CommandText = "  update boundaryGroups "
    '            query.CommandText += " set uniformMillRate = restructuredLevy/((select sum(assessment) from boundaryGroups where restructuredLevyCombined = 1 and userID = " & userID.ToString & ")/1000)"
    '            query.CommandText += " where userID = " & userID.ToString & " and boundaryGroupName = 'Subject'"
    '            query.ExecuteNonQuery()

    '        End If


    '        'update the boundaryTransfer table
    '        query.CommandText = " delete from boundaryTransfers" & vbCrLf & _
    '                            " where alternate_parcelID in " & vbCrLf & _
    '                            " (" & vbCrLf & _
    '                            " select alternate_parcelID " & vbCrLf & _
    '                            " from assessment" & vbCrLf & _
    '                            " where assessmentID = " & assessmentID & " and municipalityID=CONVERT(CHAR,(select OriginMunicipalityID from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ")) and  assessment.taxClassID in ('P', 'PP')" & vbCrLf & _
    '                            " ) and boundaryGroupID=" & boundaryGroupID.ToString
    '        query.ExecuteNonQuery()

    '        query.CommandText = "insert into boundaryTransfers "
    '        query.CommandText += " select blt.boundaryGroupID, blt.alternate_parcelID, (blt.taxableAssessmentValue * blt.percentageTransfer), ((blt.taxableAssessmentValue * blt.percentageTransfer) / 1000) * bu1.uniformMunicipalMillRate, ((blt.taxableAssessmentValue * blt.percentageTransfer) / 1000) * bu2.uniformMunicipalMillRate"
    '        query.CommandText += " from boundaryLinearTransfers blt"
    '        query.CommandText += " inner join boundaryGroups bg on bg.boundaryGroupID = blt.boundaryGroupID and blt.percentageTransfer <> 0 and blt.boundaryGroupID = " & boundaryGroupID
    '        query.CommandText += " inner join boundaryUniformMunicipalMillRate bu1 on bu1.municipalityID = bg.SubjectMunicipalityID"
    '        query.CommandText += " inner join boundaryUniformMunicipalMillRate bu2 on bu2.municipalityID = bg.OriginMunicipalityID"
    '        query.ExecuteNonQuery()

    '    End If

    '    con.Close()


    'End Sub


    'Shared Function DisplayDiv(ByVal text As String, ByVal Zindex As Integer) As String
    '    Dim domainUrl As String
    '    Dim domainProtocol As String
    '    Dim ret As New System.Text.StringBuilder()

    '    domainUrl = HttpContext.Current.Request.Url.Authority
    '    domainProtocol = HttpContext.Current.Request.Url.Scheme.ToString 'assigns URL protocol eg. "http," "https"...
    '    domainProtocol += Uri.SchemeDelimiter.ToString 'assigns protocol delimeter eg "://"

    '    If HttpContext.Current.Request.ApplicationPath <> "/" Then
    '        domainUrl &= HttpContext.Current.Request.ApplicationPath
    '    End If

    '    ret.Append("<div id=""loading"" align=""center"" style=""position:absolute; top: 0;height:100%;z-index:" & Zindex & ";"">")
    '    ret.Append("<table cellspacing=""0"" cellpadding=""0"" border=""0"" height=""100%"" width=""100%"">")
    '    ret.Append("<tr>")
    '    ret.Append("<td valign=""center"" align=""center"">")
    '    ret.Append("<table cellspacing=""0"" cellpadding=""0"" border=""0"" height=""200"" width=""276"" bgcolor=""#FFFFFF""><tr><td valign=""center"" align=""center""><img src=""" & domainProtocol & domainUrl & "/images/wait-image.gif"" border=""0"" width=""236"" height=""72""><br>")
    '    ret.Append("<img src=""" & domainProtocol & domainUrl & "/images/wait.gif"" border=""0"" width=""236"" height=""15"">")
    '    ret.Append("<br><br><H2><font face=""Arial, Helvetica, sans-serif"">" & text & "</font></H2></td>")
    '    ret.Append("</tr></table></td></tr>")
    '    ret.Append("</table>")
    '    ret.Append("</div>")

    '    Return ret.ToString

    'End Function

    ''Javascript to hide the layer
    'Shared Function HideDiv()
    '    Return "<script language=""javascript"" type=""text/javascript"">HideLoad();</script>"
    'End Function

    ''Javascript functions for dealing with the 'progress' Div
    'Shared Function JavascriptFunctions() As String
    '    Return "<script language=""javascript"" type=""text/javascript"">		var IE = document.all;var NS6 = document.getElementById;function ShowLoad(text) { if (IE) {document.all.loading.innerHTML =LoadingInnerHTML(text);document.all.loading.style.visibility = 'visible';}else if (NS6) {document.getElementById('loading').innerHTML = LoadingInnerHTML(text);document.getElementById('loading').style.visibility = 'visible';}else {document.loading.innerHTML = LoadingInnerHTML(text);document.loading.visibility='show';}} function LoadingInnerHTML(text) {return '<table cellspacing=""0"" cellpadding=""0"" border=""0"" height=""100%"" width=""100%""><tr><td valign=""center"" align=""center""><table cellspacing=""0"" cellpadding=""0"" border=""0"" height=""200"" width=""276"" bgcolor=""#FFFFFF""><tr><td valign=""center"" align=""center""><img src=""images/wait-image.gif"" border=""0"" width=""236"" height=""72""><br><img src=""images/wait.gif"" border=""0"" width=""236"" height=""15""><br><br><H2><font face=""Arial, Helvetica, sans-serif"">' + text + '</font></H2></td><td></tr></table></td></tr></table>';} function HideLoad() {if (IE) {document.all.loading.style.visibility = 'hidden';}else if (NS6) {document.getElementById('loading').style.visibility = 'hidden';}else {document.loading.visibility='hide';}}</script>"

    'End Function

    'Shared Function ChangeText(ByRef text As String) As String
    '    Return "<script language=""javascript"" type=""text/javascript"">ShowLoad('" & text & "');</script>"
    'End Function

    'Public Shared Sub createLiveLTTtable()

    '    Dim userID As Integer = HttpContext.Current.Session("UserID")

    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    Dim query As New SqlClient.SqlCommand
    '    query.Connection = con
    '    con.Open()

    '    '***Load current Session liveTaxClasses for insertion into liveLTTtable***
    '    query.CommandText = "delete from liveTaxClasses where userID = " & userID.ToString & vbCrLf
    '    query.CommandText += "insert into liveTaxClasses select " & userID.ToString & " as userID, taxClasses.taxClassID, [default] as show from taxClasses inner join taxClassesPermission on taxClasses.taxClassID = taxClassesPermission.taxClassID where active = 1 and taxClassesPermission.levelID = " & HttpContext.Current.Session("levelID").ToString & " and taxClassesPermission.access = 1"
    '    query.ExecuteNonQuery()


    '    '***Re-load current taxClasses data into LTTtaxClasses table ***
    '    '1)delete any extra classes from LTTtaxClasses which are not in taxClasses table... 
    '    query.CommandText = "DELETE FROM LTTtaxClasses WHERE taxClassID NOT IN (select taxClassID from taxClasses)" & vbCrLf

    '    '2) insert any missing classes into LTTtaxClasses which are found in taxClasses table
    '    'this also sets the LTTmainTaxClassID to a default value of 3: "Commercial" 
    '    query.CommandText += "INSERT INTO LTTtaxClasses SELECT taxClassID, taxClass, description, parentTaxClassID, [default], active, notes, [sort], 3 " & vbCrLf & _
    '                         "FROM taxClasses WHERE taxClassID NOT IN (select taxClassID from LTTtaxClasses where taxClassID in (select taxClassID from taxClasses))" & vbCrLf

    '    '3) ensures all data within LTTtaxClasses are identical to data in taxClasses table...
    '    query.CommandText += "UPDATE LTTtaxClasses SET taxClass = taxClasses.taxClass,  description = taxClasses.description, parentTaxClassID = taxClasses.parentTaxClassID, [default] = taxClasses.[default], active = taxClasses.active, notes = taxClasses.notes, [sort] = taxClasses.[sort] " & vbCrLf & _
    '                         "FROM taxClasses WHERE LTTtaxClasses.taxClassID = taxClasses.taxClassID" & vbCrLf
    '    query.ExecuteNonQuery()


    '    '***Find liveLTTTaxClasses_(userID) table and delete if it exists in Database***
    '    deleteLiveLTTtable()

    '    '***(Re)create LiveLTTTaxClasses_(userID) table. Set default value of column "show" to True (aka 1)***
    '    query.CommandText = "create table liveLTTtaxClasses_" & userID & " (taxClassID varchar(2), show bit default 1, LTTmainTaxClassID varchar(2))" & vbCrLf
    '    query.CommandText += "insert into liveLTTtaxClasses_" & userID & " (taxClassID, LTTmainTaxClassID) select liveTaxClasses.taxClassID, LTTtaxClasses.LTTmainTaxClassID from LTTtaxClasses inner join liveTaxClasses on liveTaxClasses.taxClassID = LTTtaxClasses.taxClassID where LTTtaxClasses.active = 1 and liveTaxClasses.userid = '" & userID & "' order by sort"
    '    query.ExecuteNonQuery()

    '    'sets session variable
    '    HttpContext.Current.Session("liveLTTtableCreated") = True

    'End Sub

    'Public Shared Sub deleteLiveLTTtable()

    '    Using con As SqlClient.SqlConnection = New SqlClient.SqlConnection(PATMAP.Global_asax.connString)
    '        If Not con.State = ConnectionState.Open Then
    '            con.Open()
    '        End If

    '        Dim userID As Integer = HttpContext.Current.Session("UserID")

    '        Dim IsFound As Boolean = False

    '        Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
    '            query.Connection = con
    '            query.CommandText = "select 'found' from sysobjects where name = 'liveLTTtaxClasses_" & userID & "'"
    '            Using dr As SqlClient.SqlDataReader = query.ExecuteReader
    '                If dr.HasRows Then
    '                    IsFound = True
    '                End If
    '            End Using
    '        End Using

    '        If IsFound Then
    '            Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
    '                query.Connection = con
    '                query.CommandText = "drop table liveLTTtaxClasses_" & userID
    '                query.ExecuteNonQuery()
    '            End Using
    '        End If
    '    End Using

    '    HttpContext.Current.Session("liveLTTtableCreated") = False

    '    ''setup database connection
    '    'Dim con As New SqlClient.SqlConnection
    '    'con.ConnectionString = PATMAP.Global_asax.connString

    '    'Dim query As New SqlClient.SqlCommand
    '    'query.Connection = con
    '    'con.Open()

    '    ''find liveLTTTaxClasses_(userID) if it exists in Database
    '    'query.CommandText = "select 'found' from sysobjects where name = 'liveLTTtaxClasses_" & userID & "'"

    '    'Dim dr As SqlClient.SqlDataReader
    '    'dr = query.ExecuteReader
    '    'If dr.HasRows Then
    '    '	dr.Close()
    '    '	query.CommandText = "drop table liveLTTtaxClasses_" & userID
    '    '	query.ExecuteNonQuery()
    '    'End If

    '    'con.Close()
    '    'HttpContext.Current.Session("liveLTTtableCreated") = False
    'End Sub

    ''redirects the user to the next available screen the user has access to
    ''accepts sectionID, the current page the user is on (equivalent to screenNameID) and the userLevel (i.e. levelID) as parameters
    'Public Shared Sub gotoNextPage(ByVal sectionID As Integer, ByVal currentScreenID As Integer, ByVal levelID As Integer)
    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    Dim query As New SqlClient.SqlCommand
    '    Dim dr As SqlClient.SqlDataReader

    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    Dim parentPath As String = ""
    '    query.CommandText = "SET ROWCOUNT 1 SELECT pageAddress FROM levelsPermission p INNER JOIN screenNames s ON s.screenNameID = p.screenNameID" & vbCrLf & _
    '                               "WHERE s.sectionID = " & sectionID & " AND levelID = " & levelID & " AND access = 1 " & vbCrLf

    '    'Redirect user to the next available page...
    '    Select Case sectionID

    '        Case 3 'Assessment Section
    '            parentPath = "/assmnt/"


    '            If currentScreenID = 99 Then 'if current page is "EDPOV" page
    '                query.CommandText += "AND s.screenNameID NOT IN (6, 47, 52, 101) AND s.screenNameID > 46 AND s.screenNameID < " & currentScreenID & vbCrLf

    '            ElseIf currentScreenID > 46 And currentScreenID < 99 Then
    '                query.CommandText += "AND s.screenNameID NOT IN (6, 47, 52, 101, 99) AND s.screenNameID > " & currentScreenID & vbCrLf

    '            Else
    '                query.CommandText += "AND s.screenNameID NOT IN (6, 47, 52, 101) AND s.screenNameID > " & currentScreenID & vbCrLf
    '            End If

    '            '***Inky's Update: Apr-2010***
    '            query.CommandText += "ORDER BY " & vbCrLf
    '            query.CommandText += "  Case s.screenNameID" & vbCrLf
    '            query.CommandText += "    WHEN  5 THEN  1" & vbCrLf ' "Start" page (/assmnt/start.aspx)
    '            query.CommandText += "    WHEN 45 THEN  2" & vbCrLf ' "General" page (/assmnt/general.aspx)
    '            query.CommandText += "    WHEN 46 THEN  3" & vbCrLf ' "POV" page (/assmnt/pov.aspx)
    '            query.CommandText += "    WHEN 99 THEN  4" & vbCrLf ' "EDPOV" page (/assmnt/edpov.aspx)
    '            query.CommandText += "    WHEN 48 THEN  5" & vbCrLf ' "PEMR" page (/assmnt/pemr.aspx) 
    '            query.CommandText += "    WHEN 49 THEN  6" & vbCrLf ' "Audit Trail" page (/assmnt/audittrail.aspx)
    '            query.CommandText += "    WHEN 50 THEN  7" & vbCrLf ' "Tables" page (/assmnt/tables.aspx)
    '            query.CommandText += "    WHEN 51 THEN  8" & vbCrLf ' "Graphs" page (/assmnt/graphs.aspx)
    '            query.CommandText += "    WHEN 83 THEN  9" & vbCrLf ' "Map" page (/assmnt/AnalysisMap.aspx)
    '            query.CommandText += "  End" & vbCrLf

    '            '' '' '' '' '' ''query.CommandText += "ORDER BY " & vbCrLf & _
    '            '' '' '' '' '' ''                       "Case s.screenNameID" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN  5 THEN  1" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN  6 THEN  2" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN 45 THEN  3" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN 46 THEN  4" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN 99 THEN  5" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN 47 THEN  6" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN 48 THEN  7" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN 49 THEN  8" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN 50 THEN  9" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN 51 THEN  10" & vbCrLf & _
    '            '' '' '' '' '' ''                         "WHEN 83 THEN  11" & vbCrLf & _
    '            '' '' '' '' '' ''                       "End" & vbCrLf
    '            '***Inky:End***

    '        Case 4 'Boundary Section
    '        Case 5 'Data Management Section
    '        Case 6 'System Admin Section
    '        Case 9 'Local Tax Tools (LTT) Section

    '            parentPath = "/taxtools/"
    '            If Not IsNothing(HttpContext.Current.Session("boundarySelection")) Then
    '                'user may have full access to all LTT screens but if coming from boundary will not be able to 
    '                'access the BaseYear or PhaseIn screens
    '                query.CommandText += "AND s.screenNameID <> 111 AND s.screenNameID <> 106 AND s.screenNameID > " & currentScreenID & " ORDER BY s.screenNameID"
    '            Else
    '                'user does not have full access to baseYear/phaseIn screens
    '                query.CommandText += "AND s.screenNameID > " & currentScreenID & " ORDER BY s.screenNameID"
    '            End If

    '    End Select

    '    dr = query.ExecuteReader

    '    Dim nextPage As String = ""
    '    If dr.Read() Then
    '        nextPage = dr.GetValue(0)
    '    Else
    '        'throw error
    '    End If

    '    'clean up
    '    dr.Close()
    '    con.Close()

    '    nextPage = Replace(nextPage, parentPath, "")
    '    HttpContext.Current.Response.Redirect(nextPage)

    'End Sub


    ''Based on the new levy recalculate the umr
    'Public Shared Sub calculateSubjectModelTax(ByVal revisedUniformMillRateValue As Double)

    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    Dim query As New SqlClient.SqlCommand
    '    Dim dr As SqlClient.SqlDataReader

    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    query.CommandText = "execute populateLiveLTTSubjectModel " & HttpContext.Current.Session("userID").ToString
    '    query.ExecuteNonQuery()

    '    'update the base tax parameters
    '    query.CommandText = "execute updateLiveLTTSubjectModel " & HttpContext.Current.Session("userID").ToString & ",-1,-1"
    '    query.ExecuteNonQuery()

    '    Dim prevUMR As Double = 0.0
    '    Dim currUMR As Double = revisedUniformMillRateValue

    '    'continue the iteration until
    '    'UMR from the prev iter is the same as the current one OR
    '    'UMR is 0 => base + min tax > levy

    '    'just set the currUMR to some odd numbe so that the iteration will be triggered
    '    prevUMR = -99.9999

    '    'check if mill rate factor is RN or NRN
    '    query.CommandText = "select modelMRFRN from liveLTTValues where userID = " & HttpContext.Current.Session("userID").ToString & " group by modelMRFRN"
    '    dr = query.ExecuteReader
    '    dr.Read()
    '    Dim millRateFactorRN As Integer = dr.GetValue(0)
    '    dr.Close()

    '    Dim iterNum = 0
    '    While prevUMR <> currUMR Or currUMR = 0
    '        iterNum = iterNum + 1
    '        query.CommandText = "execute updateLiveLTTSubjectModel " & HttpContext.Current.Session("userID").ToString & "," & iterNum & "," & millRateFactorRN
    '        query.ExecuteNonQuery()

    '        prevUMR = currUMR
    '        query.CommandText = "select UMR from liveLLTSubjectModel_" & HttpContext.Current.Session("userID").ToString & " group by UMR"
    '        dr = query.ExecuteReader
    '        If dr.Read() Then
    '            currUMR = dr.GetValue(0)
    '        End If
    '        dr.Close()
    '    End While

    '    'If currUMR = 0 Then
    '    '    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP323")
    '    '    Exit Sub
    '    'End If

    '    'after calculation is completed, update the total tax
    '    query.CommandText = "execute updateLiveLTTSubjectModel " & HttpContext.Current.Session("userID").ToString & ",0,-1"
    '    query.ExecuteNonQuery()

    '    con.Close()

    'End Sub

    'Public Shared Sub populateSubjectModelFields(ByRef txtRevisedUniformMillRateValue As System.Web.UI.WebControls.TextBox, ByRef txtRevisedModelRevenueValue As System.Web.UI.WebControls.TextBox)

    '    'populate the fields on the page
    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    Dim query As New SqlClient.SqlCommand
    '    Dim dr As SqlClient.SqlDataReader

    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    query.Connection = con
    '    query.CommandTimeout = 60000
    '    con.Open()

    '    'If the subject model tax is not calculated yet,
    '    'then get the revised UMR and the Levy from the subject table.
    '    'Else get the revised UMR and the Levy from the subject model table.
    '    query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLLTSubjectModel_" & HttpContext.Current.Session("userID").ToString & "]') AND type in (N'U')"
    '    dr = query.ExecuteReader
    '    If dr.Read Then
    '        dr.Close()
    '        query.CommandText = "select UMR, Levy from liveLLTSubjectModel_" & HttpContext.Current.Session("userID").ToString & " group by UMR, Levy"
    '        dr = query.ExecuteReader()
    '        dr.Read()
    '    Else
    '        dr.Close()
    '        query.CommandText = "select UMR, Levy from liveLLTSubject_" & HttpContext.Current.Session("userID").ToString & " group by UMR, Levy"
    '        dr = query.ExecuteReader()
    '        dr.Read()
    '    End If

    '    txtRevisedUniformMillRateValue.Text = FormatNumber((dr.GetValue(0) * 1000), 4)
    '    txtRevisedModelRevenueValue.Text = FormatNumber(dr.GetValue(1), 2)

    '    con.Close()

    'End Sub

    ''Public Shared Sub calculateTableCreation(ByVal Response As System.Web.HttpResponse)
    ''	'setup database connection
    ''	Dim con As New SqlClient.SqlConnection
    ''	con.ConnectionString = PATMAP.Global_asax.connString
    ''	con.Open()
    ''	Try
    ''		'get current user
    ''		Dim userID As Integer = System.Web.HttpContext.Current.Session("userID")

    ''		Dim da As New SqlClient.SqlDataAdapter
    ''		Dim dt As New DataTable
    ''		Dim query As New SqlClient.SqlCommand
    ''		query.Connection = con
    ''		query.CommandTimeout = 60000
    ''		Dim dr As SqlClient.SqlDataReader

    ''		'check if K-12 OG data set is entered by the user
    ''		query.CommandText = "select SubjectK12ID from liveAssessmentTaxModel where userid = " & userID
    ''		dr = query.ExecuteReader
    ''		dr.Read()

    ''		If dr.GetValue(0) = 0 Then
    ''			System.Web.HttpContext.Current.Session.Add("missingK12DataSet", "true")
    ''			'con.Close()    Donna - Keep connection open.
    ''			'''''Response.Redirect("kog.aspx", False) '***Inky commented-out this line
    ''			'''''Exit Sub '***Inky commented-out this line
    ''		End If

    ''		dr.Close()


    ''		'Dim BaseTaxYearModelID As Integer
    ''		'Dim BaseStale As Boolean
    ''		'Dim SubjectTaxYearModelID As Integer
    ''		'Dim SubjectStale As Boolean

    ''		Dim baseAssessmentID As Integer
    ''		Dim subjectAssessmentID As Integer

    ''		query.CommandText = "select assessmentID from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
    ''		dr = query.ExecuteReader
    ''		dr.Read()
    ''		baseAssessmentID = dr.GetValue(0)

    ''		dr.Close()
    ''		query.CommandText = "select assessmentID from taxYearModelDescription where taxYearModelID = (select subjectTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
    ''		dr = query.ExecuteReader
    ''		dr.Read()
    ''		subjectAssessmentID = dr.GetValue(0)

    ''		dr.Close()
    ''		Dim doCompare As Integer
    ''		Dim tmpDataStale As Integer
    ''		query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[assessmentBase_" & subjectAssessmentID & "_" & baseAssessmentID & "]') AND type in (N'U')"
    ''		dr = query.ExecuteReader
    ''		If Not dr.Read() Then
    ''			doCompare = 1
    ''		Else
    ''			dr.Close()
    ''			query.CommandText = "SELECT dataStale from assessmentDescription where assessmentID = " & baseAssessmentID
    ''			dr = query.ExecuteReader
    ''			dr.Read()
    ''			tmpDataStale = dr.GetValue(0)
    ''			If tmpDataStale = True Then
    ''				doCompare = 1
    ''			End If
    ''		End If

    ''		dr.Close()
    ''		query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[assessmentSubject_" & subjectAssessmentID & "_" & baseAssessmentID & "]') AND type in (N'U')"
    ''		dr = query.ExecuteReader
    ''		If Not dr.Read() Then
    ''			doCompare = 1
    ''		Else
    ''			dr.Close()
    ''			query.CommandText = "SELECT dataStale from assessmentDescription where assessmentID = " & subjectAssessmentID
    ''			dr = query.ExecuteReader
    ''			dr.Read()
    ''			tmpDataStale = dr.GetValue(0)
    ''			If tmpDataStale = True Then
    ''				doCompare = 1
    ''			End If
    ''		End If

    ''		dr.Close()
    ''		If doCompare = 1 Then
    ''			Response.Write(common.JavascriptFunctions())
    ''			Response.Flush()

    ''			Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
    ''			Response.Flush()

    ''			query.CommandText = "exec compareBaseandSubject " & userID.ToString & "," & subjectAssessmentID.ToString & "," & baseAssessmentID.ToString & ",1," & doCompare
    ''			query.ExecuteNonQuery()
    ''		End If


    ''		Dim BaseTaxYearModelID As Integer
    ''		Dim SubjectTaxYearModelID As Integer
    ''		Dim dataStale As Boolean
    ''		Dim enterPEMR As Boolean		'Donna
    ''		Dim PEMRByTotalLevy As Boolean	'Donna
    ''		Dim basedataStale As Boolean

    ''		'check if data is stale
    ''		'Donna - Added enterPEMR and PEMRByTotalLevy.
    ''		query.CommandText = "select baseTaxYearModelID, subjectTaxYearModelID, dataStale, enterPEMR, PEMRByTotalLevy from liveassessmenttaxmodel where userid = " & userID
    ''		dr = query.ExecuteReader
    ''		dr.Read()
    ''		BaseTaxYearModelID = dr.GetValue(0)
    ''		SubjectTaxYearModelID = dr.GetValue(1)
    ''		dataStale = dr.GetValue(2)
    ''		enterPEMR = dr("enterPEMR")	 'Donna

    ''		'Donna start
    ''		If dr("PEMRByTotalLevy").Equals(DBNull.Value) Then
    ''			PEMRByTotalLevy = False
    ''		Else
    ''			PEMRByTotalLevy = dr("PEMRByTotalLevy")
    ''		End If
    ''		'Donna end

    ''		dr.Close()

    ''		query.CommandText = "select dataStale from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
    ''		dr = query.ExecuteReader
    ''		dr.Read()
    ''		basedataStale = dr.GetValue(0)


    ''		'If BaseStale Or SubjectStale Or IsNothing(Session("calculated")) Then
    ''		If dataStale Or basedataStale Or IsNothing(System.Web.HttpContext.Current.Session("calculated")) Then
    ''			'Try
    ''			If doCompare = 0 Then
    ''				Response.Write(common.JavascriptFunctions())
    ''				Response.Flush()

    ''				Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
    ''				Response.Flush()
    ''			End If

    ''			'check if base year is stale
    ''			If basedataStale Then
    ''				common.calculateTaxYearModel(1, BaseTaxYearModelID, userID, SubjectTaxYearModelID)
    ''			End If

    ''			'check if subject year is stale
    ''			If dataStale Then
    ''				common.calculateTaxYearModel(0, SubjectTaxYearModelID, userID)
    ''			End If

    ''			'Donna start
    ''			If Not enterPEMR Then
    ''				If PEMRByTotalLevy Then
    ''					common.calcRevenueNeutralByTotalLevy(userID, BaseTaxYearModelID)
    ''				Else
    ''					common.calcRevenueNeutralByClassLevy(userID, BaseTaxYearModelID)
    ''				End If
    ''			End If
    ''			'Donna end

    ''			'Catch
    ''			'	'retrieves error message
    ''			'	'Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
    ''			'End Try

    ''			Response.Write("&nbsp;")
    ''			Response.Flush()

    ''			common.calcAssessmentSummary(userID, 0)

    ''			Response.Write("&nbsp;")
    ''			Response.Flush()

    ''			'create School Municipality and Parcels tables for Map viewing
    ''			common.Create_School_Mun_Parcel_Tables()

    ''			System.Web.HttpContext.Current.Session.Add("calculated", "true")

    ''			Response.Write(common.HideDiv())
    ''			Response.Flush()

    ''			'Response.Write("<script language=javascript>window.navigate('Map.aspx');</script>")
    ''			Response.Write("<script language='javascript'>window.location.href='Map.aspx';</script>")
    ''			Response.End()
    ''		End If
    ''	Finally
    ''		If con.State = ConnectionState.Open Then
    ''			con.Close()
    ''		End If
    ''	End Try
    ''End Sub

    'Public Shared Sub calculateTableCreation(ByVal Response As System.Web.HttpResponse)
    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()
    '    Try
    '        'get current user
    '        Dim userID As Integer = System.Web.HttpContext.Current.Session("userID")

    '        Dim da As New SqlClient.SqlDataAdapter
    '        Dim dt As New DataTable
    '        Dim query As New SqlClient.SqlCommand
    '        query.Connection = con
    '        query.CommandTimeout = 60000
    '        Dim dr As SqlClient.SqlDataReader

    '        'check if K-12 OG data set is entered by the user
    '        query.CommandText = "select SubjectK12ID from liveAssessmentTaxModel where userid = " & userID
    '        dr = query.ExecuteReader
    '        dr.Read()

    '        If dr.GetValue(0) = 0 Then
    '            System.Web.HttpContext.Current.Session.Add("missingK12DataSet", "true")
    '            'con.Close()    Donna - Keep connection open.
    '            '''''Response.Redirect("kog.aspx", False) '***Inky commented-out this line
    '            '''''Exit Sub '***Inky commented-out this line
    '        End If

    '        dr.Close()


    '        'Dim BaseTaxYearModelID As Integer
    '        'Dim BaseStale As Boolean
    '        'Dim SubjectTaxYearModelID As Integer
    '        'Dim SubjectStale As Boolean

    '        Dim baseAssessmentID As Integer
    '        Dim subjectAssessmentID As Integer

    '        query.CommandText = "select assessmentID from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        baseAssessmentID = dr.GetValue(0)

    '        dr.Close()
    '        query.CommandText = "select assessmentID from taxYearModelDescription where taxYearModelID = (select subjectTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        subjectAssessmentID = dr.GetValue(0)

    '        dr.Close()
    '        Dim doCompare As Integer
    '        Dim tmpDataStale As Integer
    '        query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[assessmentBase_" & subjectAssessmentID & "_" & baseAssessmentID & "]') AND type in (N'U')"
    '        dr = query.ExecuteReader
    '        If Not dr.Read() Then
    '            doCompare = 1
    '        Else
    '            dr.Close()
    '            query.CommandText = "SELECT dataStale from assessmentDescription where assessmentID = " & baseAssessmentID
    '            dr = query.ExecuteReader
    '            dr.Read()
    '            tmpDataStale = dr.GetValue(0)
    '            If tmpDataStale = True Then
    '                doCompare = 1
    '            End If
    '        End If

    '        dr.Close()
    '        query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[assessmentSubject_" & subjectAssessmentID & "_" & baseAssessmentID & "]') AND type in (N'U')"
    '        dr = query.ExecuteReader
    '        If Not dr.Read() Then
    '            doCompare = 1
    '        Else
    '            dr.Close()
    '            query.CommandText = "SELECT dataStale from assessmentDescription where assessmentID = " & subjectAssessmentID
    '            dr = query.ExecuteReader
    '            dr.Read()
    '            tmpDataStale = dr.GetValue(0)
    '            If tmpDataStale = True Then
    '                doCompare = 1
    '            End If
    '        End If

    '        dr.Close()
    '        If doCompare = 1 Then
    '            Response.Write(common.JavascriptFunctions())
    '            Response.Flush()

    '            Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
    '            Response.Flush()

    '            query.CommandText = "exec compareBaseandSubject " & userID.ToString & "," & subjectAssessmentID.ToString & "," & baseAssessmentID.ToString & ",1," & doCompare
    '            query.ExecuteNonQuery()
    '        End If


    '        Dim BaseTaxYearModelID As Integer
    '        Dim SubjectTaxYearModelID As Integer
    '        Dim dataStale As Boolean
    '        Dim enterPEMR As Boolean        'Donna
    '        Dim PEMRByTotalLevy As Boolean  'Donna
    '        Dim basedataStale As Boolean

    '        'check if data is stale
    '        'Donna - Added enterPEMR and PEMRByTotalLevy.
    '        query.CommandText = "select baseTaxYearModelID, subjectTaxYearModelID, dataStale, enterPEMR, PEMRByTotalLevy from liveassessmenttaxmodel where userid = " & userID
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        BaseTaxYearModelID = dr.GetValue(0)
    '        SubjectTaxYearModelID = dr.GetValue(1)
    '        dataStale = dr.GetValue(2)
    '        enterPEMR = dr("enterPEMR")  'Donna

    '        'Donna start
    '        If dr("PEMRByTotalLevy").Equals(DBNull.Value) Then
    '            PEMRByTotalLevy = False
    '        Else
    '            PEMRByTotalLevy = dr("PEMRByTotalLevy")
    '        End If
    '        'Donna end

    '        dr.Close()

    '        query.CommandText = "select dataStale from taxYearModelDescription where taxYearModelID = (select baseTaxYearModelID from liveAssessmentTaxModel where userid = " & userID & ")"
    '        dr = query.ExecuteReader
    '        dr.Read()
    '        basedataStale = dr.GetValue(0)


    '        'If BaseStale Or SubjectStale Or IsNothing(Session("calculated")) Then
    '        If dataStale Or basedataStale Or IsNothing(System.Web.HttpContext.Current.Session("calculated")) Then
    '            'Try
    '            If doCompare = 0 Then
    '                Response.Write(common.JavascriptFunctions())
    '                Response.Flush()

    '                Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
    '                Response.Flush()
    '            End If

    '            'check if base year is stale
    '            If basedataStale Then
    '                common.calculateTaxYearModel(1, BaseTaxYearModelID, userID, SubjectTaxYearModelID)
    '            End If

    '            'check if subject year is stale
    '            If dataStale Then
    '                common.calculateTaxYearModel(0, SubjectTaxYearModelID, userID)
    '            End If

    '            'Donna start
    '            If Not enterPEMR Then
    '                If PEMRByTotalLevy Then
    '                    common.calcRevenueNeutralByTotalLevy(userID, BaseTaxYearModelID)
    '                Else
    '                    common.calcRevenueNeutralByClassLevy(userID, BaseTaxYearModelID)
    '                End If
    '            End If
    '            'Donna end

    '            'Catch
    '            '	'retrieves error message
    '            '	'Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
    '            'End Try

    '            Response.Write("&nbsp;")
    '            Response.Flush()

    '            common.calcAssessmentSummary(userID, 0)

    '            Response.Write("&nbsp;")
    '            Response.Flush()

    '            'create School Municipality and Parcels tables for Map viewing
    '            common.Create_School_Mun_Parcel_Tables()

    '            System.Web.HttpContext.Current.Session.Add("calculated", "true")

    '            Response.Write(common.HideDiv())
    '            Response.Flush()

    '            'Response.Write("<script language=javascript>window.navigate('Map.aspx');</script>")
    '            Response.Write("<script language='javascript'>window.location.href='Map.aspx';</script>")
    '            Response.End()
    '        End If
    '    Finally
    '        If con.State = ConnectionState.Open Then
    '            con.Close()
    '        End If
    '    End Try
    'End Sub


    'Public Shared Sub calculateLTTTableCreation(ByVal Response As System.Web.HttpResponse)
    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()
    '    Try
    '        'get current user
    '        Dim userID As Integer = System.Web.HttpContext.Current.Session("userID")

    '        Dim da As New SqlClient.SqlDataAdapter
    '        Dim dt As New DataTable
    '        Dim query As New SqlClient.SqlCommand
    '        query.Connection = con
    '        query.CommandTimeout = 60000
    '        Dim dr As SqlClient.SqlDataReader

    '        If IsNothing(System.Web.HttpContext.Current.Session("LTTcalculated")) Then

    '            Response.Write(common.JavascriptFunctions())
    '            Response.Flush()

    '            Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
    '            Response.Flush()

    '            Dim phaseInBaseYearAccess As Boolean = False
    '            If Not IsNothing(System.Web.HttpContext.Current.Session("phaseInBaseYearAccess")) Then
    '                phaseInBaseYearAccess = CType(System.Web.HttpContext.Current.Session("phaseInBaseYearAccess"), Boolean)
    '            End If

    '            If phaseInBaseYearAccess Then
    '                query.CommandText = "execute calcPhaseIn " & userID.ToString()
    '                query.ExecuteNonQuery()

    '                query.CommandText = "execute calcLTTPhaseInSummary " & userID.ToString()
    '                query.ExecuteNonQuery()
    '            End If

    '            query.CommandText = "execute calcLTTResult " & userID.ToString()
    '            query.ExecuteNonQuery()

    '            query.CommandText = "execute calcLTTSummary " & userID.ToString()
    '            query.ExecuteNonQuery()

    '            'create School Municipality and Parcels tables for Map viewing
    '            common.CreateLTT_School_Mun_Parcel_Tables()

    '            System.Web.HttpContext.Current.Session.Add("LTTcalculated", "true")

    '            Response.Write(common.HideDiv())
    '            Response.Flush()
    '            Response.Write("<script language=javascript>window.navigate('Map.aspx');</script>")
    '            Response.End()

    '        End If

    '    Finally
    '        If con.State = ConnectionState.Open Then
    '            con.Close()
    '        End If
    '    End Try
    'End Sub

    'Public Shared Sub calculateBOUNDARYCHANGETableCreation(ByVal Response As System.Web.HttpResponse)
    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString
    '    con.Open()
    '    Try
    '        'get current user
    '        Dim userID As Integer = System.Web.HttpContext.Current.Session("userID")

    '        Dim da As New SqlClient.SqlDataAdapter
    '        Dim dt As New DataTable
    '        Dim query As New SqlClient.SqlCommand
    '        query.Connection = con
    '        query.CommandTimeout = 60000
    '        Dim dr As SqlClient.SqlDataReader


    '        '//Ensure the map will be drawn fresh
    '        'MapSettings.MapStale = true;
    '        If IsNothing(HttpContext.Current.Session("MapStale")) Then
    '            HttpContext.Current.Session.Add("MapStale", True)
    '        End If
    '        HttpContext.Current.Session("MapStale") = True

    '        'check if data is stale (ie: if the user has transfered some new properties from/to the subject mun)
    '        query.CommandText = "select mapDataStale from liveBoundaryModel where userid = " & userID
    '        dr = query.ExecuteReader()
    '        dr.Read()
    '        Dim mapDataStale As Boolean = False
    '        mapDataStale = CType(dr.GetValue(0), Boolean)
    '        dr.Close()

    '        If mapDataStale Or IsNothing(System.Web.HttpContext.Current.Session("calculated")) Then
    '            'change the status of the BoundaryChangeStale to true
    '            If IsNothing(System.Web.HttpContext.Current.Session("BoundaryChangeStale")) Then
    '                System.Web.HttpContext.Current.Session.Add("BoundaryChangeStale", True)
    '            End If
    '            System.Web.HttpContext.Current.Session("BoundaryChangeStale") = True

    '            Response.Write(common.JavascriptFunctions())
    '            Response.Flush()

    '            Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
    '            Response.Flush()

    '            common.calculateBoundaryModel(userID)

    '            query.CommandText = "update liveBoundaryModel set mapDataStale = 0 where userid = " & userID
    '            query.ExecuteNonQuery()

    '            System.Web.HttpContext.Current.Session("BoundaryChangeStale") = False

    '            Dim delStr As New StringBuilder
    '            delStr.Append("DELETE MunicipalitiesChanges WHERE UserID = @UserID DELETE mapBoundaryTransfers WHERE UserID = @UserID")
    '            Dim clearChangesCmd As SqlCommand = New SqlCommand(delStr.ToString(), con)
    '            clearChangesCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID
    '            clearChangesCmd.ExecuteNonQuery()

    '            BuildBoundary.buildBoundary(userID)

    '            'create School Municipality and Parcels tables for Map viewing
    '            common.CreateBOUNDARYCHANGE_School_Mun_Parcel_Tables()


    '            System.Web.HttpContext.Current.Session.Add("calculated", "true")

    '            Response.Write(common.HideDiv())
    '            Response.Flush()
    '            Response.Write("<script language=javascript>window.navigate('Map.aspx');</script>")
    '            Response.End()
    '        End If

    '    Finally
    '        If con.State = ConnectionState.Open Then
    '            con.Close()
    '        End If
    '    End Try
    'End Sub

    ''Public Shared Sub calculateBOUNDARYADJUSTMENTTableCreation(ByVal Response As System.Web.HttpResponse)
    ''	setup database connection
    ''	Dim con As New SqlClient.SqlConnection
    ''	con.ConnectionString = PATMAP.Global_asax.connString
    ''	con.Open()
    ''	Try
    ''		get current user
    ''		Dim userID As Integer = System.Web.HttpContext.Current.Session("userID")

    ''		Dim da As New SqlClient.SqlDataAdapter
    ''		Dim dt As New DataTable
    ''		Dim query As New SqlClient.SqlCommand
    ''		query.Connection = con
    ''		query.CommandTimeout = 60000
    ''		Dim dr As SqlClient.SqlDataReader


    ''		//Ensure the map will be drawn fresh
    ''		MapSettings.MapStale = true;
    ''		If IsNothing(HttpContext.Current.Session("MapStale")) Then
    ''			HttpContext.Current.Session.Add("MapStale", True)
    ''		End If
    ''		HttpContext.Current.Session("MapStale") = True

    ''		check if data is stale (ie: if the user has transfered some new properties from/to the subject mun)
    ''			query.CommandText = "select mapDataStale from liveBoundaryModel where userid = " & userID
    ''			dr = query.ExecuteReader()
    ''			dr.Read()
    ''			Dim mapDataStale As Boolean = False
    ''			mapDataStale = CType(dr.GetValue(0), Boolean)
    ''			dr.Close()

    ''			If mapDataStale Or IsNothing(System.Web.HttpContext.Current.Session("adjustment_calculated")) Then
    ''			change the status of the BoundaryChangeStale to true
    ''				If IsNothing(System.Web.HttpContext.Current.Session("BoundaryChangeStale")) Then
    ''					System.Web.HttpContext.Current.Session.Add("BoundaryChangeStale", True)
    ''				End If
    ''				System.Web.HttpContext.Current.Session("BoundaryChangeStale") = True

    ''				Response.Write(common.JavascriptFunctions())
    ''				Response.Flush()

    ''				Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
    ''				Response.Flush()

    ''				common.calculateBoundaryModel(userID)

    ''				query.CommandText = "update liveBoundaryModel set mapDataStale = 0 where userid = " & userID
    ''				query.ExecuteNonQuery()

    ''				System.Web.HttpContext.Current.Session("BoundaryChangeStale") = False

    ''				Dim delStr As New StringBuilder
    ''				delStr.Append("DELETE MunicipalitiesChanges WHERE UserID = @UserID DELETE mapBoundaryTransfers WHERE UserID = @UserID")
    ''				Dim clearChangesCmd As SqlCommand = New SqlCommand(delStr.ToString(), con)
    ''				clearChangesCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID
    ''				clearChangesCmd.ExecuteNonQuery()

    ''				BuildBoundary.buildBoundary(userID)

    ''			create School Municipality and Parcels tables for Map viewing
    ''				common.CreateBOUNDARYCHANGE_School_Mun_Parcel_Tables()


    ''				System.Web.HttpContext.Current.Session.Add("adjustment_calculated", "true")

    ''				Response.Write(common.HideDiv())
    ''				Response.Flush()
    ''				Response.Write("<script language=javascript>window.navigate('Map.aspx');</script>")
    ''				Response.End()
    ''			End If

    ''	Finally
    ''		If con.State = ConnectionState.Open Then
    ''			con.Close()
    ''		End If
    ''	End Try
    ''End Sub

    'Public Shared Function ToInteger(ByVal value As Object) As Integer
    '    If value Is Nothing Then
    '        Return 0
    '    ElseIf value.Equals(System.DBNull.Value) Then
    '        Return 0
    '    ElseIf IsNumeric(value) Then
    '        Return Int(value)
    '    Else
    '        Return 0
    '    End If
    'End Function

    'Public Shared Function ToDouble(ByVal value As Object) As Decimal
    '    If value Is Nothing Then
    '        Return 0
    '    ElseIf value.Equals(System.DBNull.Value) Then
    '        Return 0
    '    ElseIf IsNumeric(value) Then
    '        Return CDbl(value)
    '    Else
    '        Return 0
    '    End If
    'End Function

    'Public Shared Function ToDecimal(ByVal value As Object) As Decimal
    '    If value Is Nothing Then
    '        Return 0
    '    ElseIf value.Equals(System.DBNull.Value) Then
    '        Return 0
    '    ElseIf IsNumeric(value) Then
    '        'Return Decimal.Parse(value)
    '        Return CDec(value)
    '    Else
    '        Return 0
    '    End If
    'End Function

    'Public Shared Function NullToDateTime(ByVal value As Object) As DateTime
    '    Dim tdt As DateTime
    '    If value Is Nothing Then
    '        Return System.DateTime.Now()
    '    ElseIf value.Equals(System.DBNull.Value) Then
    '        Return System.DateTime.Now()
    '    ElseIf DateTime.TryParse(value, tdt) Then
    '        Return tdt
    '    Else
    '        Return System.DateTime.Now()
    '    End If
    'End Function

    'Public Shared Function NullToStr(ByVal value As Object) As String
    '    If value Is Nothing Then
    '        Return ""
    '    ElseIf value.Equals(System.DBNull.Value) Then
    '        Return ""
    '    ElseIf Len(Trim(value)) > 0 Then
    '        Return value.ToString()
    '    Else
    '        Return ""
    '    End If
    'End Function

    'Public Shared Sub Set_TaxClass_TaxStatus_TaxShift_Filters()
    '    SetTaxClassFilters()

    '    'TaxStatus
    '    Dim selectedTaxStatus As List(Of String) = New List(Of String)
    '    If Not IsNothing(System.Web.HttpContext.Current.Session("MapTaxStatusFilters")) Then
    '        selectedTaxStatus = CType(System.Web.HttpContext.Current.Session("MapTaxStatusFilters"), List(Of String))
    '    End If
    '    If (selectedTaxStatus.Count <= 0) Then
    '        selectedTaxStatus.Add("Taxable")
    '        System.Web.HttpContext.Current.Session("TaxStatus") = 1
    '        System.Web.HttpContext.Current.Session("MapTaxStatusFilters") = selectedTaxStatus
    '    End If

    '    'TaxShift
    '    Dim selectedTaxShift As List(Of String) = New List(Of String)
    '    If Not IsNothing(System.Web.HttpContext.Current.Session("MapTaxShiftFilters")) Then
    '        selectedTaxShift = CType(System.Web.HttpContext.Current.Session("MapTaxShiftFilters"), List(Of String))
    '    End If
    '    If (selectedTaxShift.Count <= 0) Then
    '        selectedTaxShift.Add("Municipal Tax")
    '        System.Web.HttpContext.Current.Session("TaxShift") = 1
    '        System.Web.HttpContext.Current.Session("MapTaxShiftFilters") = selectedTaxShift
    '    End If
    'End Sub

    ''added on 10-sep-2013
    'Public Shared Sub SetLTT_TaxClass_TaxStatus_TaxShift_Filters()
    '    SetLTT_TaxClassFilters()

    '    'TaxStatus
    '    Dim selectedTaxStatus As List(Of String) = New List(Of String)
    '    If Not IsNothing(System.Web.HttpContext.Current.Session("MapTaxStatusFilters")) Then
    '        selectedTaxStatus = CType(System.Web.HttpContext.Current.Session("MapTaxStatusFilters"), List(Of String))
    '    End If
    '    If (selectedTaxStatus.Count <= 0) Then
    '        selectedTaxStatus.Add("Taxable")
    '        System.Web.HttpContext.Current.Session("TaxStatus") = 1
    '        System.Web.HttpContext.Current.Session("MapTaxStatusFilters") = selectedTaxStatus
    '    End If

    '    'TaxShift
    '    Dim selectedTaxShift As List(Of String) = New List(Of String)
    '    If Not IsNothing(System.Web.HttpContext.Current.Session("MapTaxShiftFilters")) Then
    '        selectedTaxShift = CType(System.Web.HttpContext.Current.Session("MapTaxShiftFilters"), List(Of String))
    '    End If
    '    Dim phaseInBaseYearAccess As Boolean = False
    '    If Not IsNothing(System.Web.HttpContext.Current.Session("phaseInBaseYearAccess")) Then
    '        phaseInBaseYearAccess = CType(System.Web.HttpContext.Current.Session("phaseInBaseYearAccess"), Boolean)
    '    End If
    '    If (Not phaseInBaseYearAccess) Then
    '        If selectedTaxShift.Contains("Phase-In Amount") Then
    '            selectedTaxShift.Remove("Phase-In Amount")
    '        End If
    '    End If
    '    If selectedTaxShift.Contains("Levy") Then
    '        selectedTaxShift.Remove("Levy")
    '    End If
    '    If (selectedTaxShift.Count <= 0) Then
    '        'selectedTaxShift.Add("Municipal Tax")
    '        selectedTaxShift.Add("Total Impact")
    '        selectedTaxShift.Add("Total Tax")
    '        If phaseInBaseYearAccess Then
    '            selectedTaxShift.Add("Phase-In Amount")
    '        End If
    '        'System.Web.HttpContext.Current.Session("TaxShift") = 1
    '        System.Web.HttpContext.Current.Session("MapTaxShiftFilters") = selectedTaxShift
    '    End If
    'End Sub

    'Public Shared Sub SetBOUNDARYCHANGE_TaxClass_TaxStatus_TaxShift_Filters()
    '    SetTaxClassFilters()

    '    'TaxStatus
    '    Dim selectedTaxStatus As List(Of String) = New List(Of String)
    '    If Not IsNothing(System.Web.HttpContext.Current.Session("MapTaxStatusFilters")) Then
    '        selectedTaxStatus = CType(System.Web.HttpContext.Current.Session("MapTaxStatusFilters"), List(Of String))
    '    End If
    '    If (selectedTaxStatus.Count <= 0) Then
    '        selectedTaxStatus.Add("Taxable")
    '        System.Web.HttpContext.Current.Session("TaxStatus") = 1
    '        System.Web.HttpContext.Current.Session("MapTaxStatusFilters") = selectedTaxStatus
    '    End If

    '    'TaxShift
    '    Dim selectedTaxShift As List(Of String) = New List(Of String)
    '    If Not IsNothing(System.Web.HttpContext.Current.Session("MapTaxShiftFilters")) Then
    '        selectedTaxShift = CType(System.Web.HttpContext.Current.Session("MapTaxShiftFilters"), List(Of String))
    '    End If
    '    If selectedTaxShift.Contains("Municipal Tax") Then
    '        selectedTaxShift.Remove("Municipal Tax")
    '    End If
    '    If selectedTaxShift.Contains("School Tax") Then
    '        selectedTaxShift.Remove("School Tax")
    '    End If
    '    If (selectedTaxShift.Count <= 0) Then
    '        selectedTaxShift.Add("Levy")
    '        'System.Web.HttpContext.Current.Session("TaxShift") = 1
    '        System.Web.HttpContext.Current.Session("MapTaxShiftFilters") = selectedTaxShift
    '    End If
    'End Sub

    'Public Shared Sub Create_School_Mun_Parcel_Tables()
    '    Dim q_UserID As String = System.Web.HttpContext.Current.Session("UserID").ToString()
    '    Dim userid As Integer = Convert.ToInt32(q_UserID)
    '    Dim dal As MapThemeDAL = New MapThemeDAL()
    '    Dim q_themSetId As Integer = -1 'MapSettings.MapThemeID
    '    Dim tblThemeSet As DataTable = dal.getMapThemeSets(userid)
    '    If tblThemeSet.Rows.Count <= 0 Then
    '        Throw New Exception("No ThemeSet Exists")
    '        Exit Sub
    '    End If
    '    For Each row As DataRow In tblThemeSet.Rows
    '        If NullToStr(row("ThemeSetName")) = "Default" Then
    '            q_themSetId = ToInteger(row("ThemeSetID"))
    '            Exit For
    '        End If
    '    Next
    '    If q_themSetId <= 0 Then
    '        Throw New Exception("No Mathing Theme Exists")
    '        Exit Sub
    '    End If

    '    Set_TaxClass_TaxStatus_TaxShift_Filters()

    '    'Create School Division Tables
    '    Dim isPer As Boolean = dal.isPercent(q_themSetId)
    '    Dim q_TableName As String = ""

    '    Try

    '        'Create School Division Tables
    '        q_TableName = Ut_SQL2TT.GetSchoolTableName(userid)
    '        Ut_SQL2TT.CreateSchoolTable(q_TableName, isPer)

    '        'Create Municipality Tables
    '        q_TableName = Ut_SQL2TT.GetMunTableName(userid)
    '        Ut_SQL2TT.CreateMunTable(q_TableName, isPer)

    '        'Create Municipality Parcels Tables
    '        q_TableName = Ut_SQL2TT.GetParcelTableName(userid)
    '        Ut_SQL2TT.CreateParselTable(q_TableName, isPer)

    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try

    'End Sub

    'Public Shared Sub CreateLTT_School_Mun_Parcel_Tables()
    '    Dim q_UserID As String = System.Web.HttpContext.Current.Session("UserID").ToString()
    '    Dim userid As Integer = Convert.ToInt32(q_UserID)
    '    Dim dal As MapThemeDAL = New MapThemeDAL()
    '    Dim q_themSetId As Integer = -1 'MapSettings.MapThemeID
    '    Dim tblThemeSet As DataTable = dal.getMapThemeSets(userid)
    '    If tblThemeSet.Rows.Count <= 0 Then
    '        Throw New Exception("No ThemeSet Exists")
    '        Exit Sub
    '    End If
    '    For Each row As DataRow In tblThemeSet.Rows
    '        If NullToStr(row("ThemeSetName")) = "Default" Then
    '            q_themSetId = ToInteger(row("ThemeSetID"))
    '            Exit For
    '        End If
    '    Next
    '    If q_themSetId <= 0 Then
    '        Throw New Exception("No Mathing Theme Exists")
    '        Exit Sub
    '    End If

    '    SetLTT_TaxClass_TaxStatus_TaxShift_Filters()

    '    'Create School Division Tables
    '    Dim isPer As Boolean = dal.isPercent(q_themSetId)
    '    Dim q_TableName As String = ""

    '    Try

    '        'remark school division for LTTMap
    '        ''Create School Division Tables
    '        'q_TableName = Ut_SQL2TT.GetSchoolTableName(userid)
    '        'Ut_SQL2TT.CreateSchoolTable(q_TableName, isPer)

    '        'Create Municipality Tables
    '        q_TableName = Ut_SQL2TT.GetMunTableName(userid)
    '        Ut_SQL2TT.CreateMunTable(q_TableName, isPer)

    '        'Create Municipality Parcels Tables
    '        q_TableName = Ut_SQL2TT.GetParcelTableName(userid)
    '        Ut_SQL2TT.CreateParselTable(q_TableName, isPer)

    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try

    'End Sub

    'Public Shared Sub CreateBOUNDARYCHANGE_School_Mun_Parcel_Tables()
    '    Dim q_UserID As String = System.Web.HttpContext.Current.Session("UserID").ToString()
    '    Dim userid As Integer = Convert.ToInt32(q_UserID)
    '    Dim dal As MapThemeDAL = New MapThemeDAL()
    '    Dim q_themSetId As Integer = -1 'MapSettings.MapThemeID
    '    Dim tblThemeSet As DataTable = dal.getMapThemeSets(userid)
    '    If tblThemeSet.Rows.Count <= 0 Then
    '        Throw New Exception("No ThemeSet Exists")
    '        Exit Sub
    '    End If
    '    For Each row As DataRow In tblThemeSet.Rows
    '        If NullToStr(row("ThemeSetName")) = "Default" Then
    '            q_themSetId = ToInteger(row("ThemeSetID"))
    '            Exit For
    '        End If
    '    Next
    '    If q_themSetId <= 0 Then
    '        Throw New Exception("No Mathing Theme Exists")
    '        Exit Sub
    '    End If

    '    SetBOUNDARYCHANGE_TaxClass_TaxStatus_TaxShift_Filters()

    '    'Create School Division Tables
    '    Dim isPer As Boolean = dal.isPercent(q_themSetId)
    '    Dim q_TableName As String = ""

    '    Try
    '        'Create School Division Tables
    '        q_TableName = Ut_SQL2TT.GetSchoolTableName(userid)
    '        Ut_SQL2TT.CreateSchoolTable(q_TableName, isPer)

    '        'Create Municipality Tables
    '        q_TableName = Ut_SQL2TT.GetMunTableName(userid)
    '        Ut_SQL2TT.CreateMunTable(q_TableName, isPer)

    '        'Create Municipality Parcels Tables
    '        q_TableName = Ut_SQL2TT.GetParcelTableName(userid)
    '        Ut_SQL2TT.CreateParselTable(q_TableName, isPer)

    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try

    'End Sub

    'Public Shared Sub ASSMNT_InitRequiredSessionVars()
    '    System.Web.HttpContext.Current.Session.Remove("MapTaxStatusFilters")
    '    System.Web.HttpContext.Current.Session.Remove("MapTaxShiftFilters")
    '    System.Web.HttpContext.Current.Session.Remove("TaxStatus")
    '    System.Web.HttpContext.Current.Session.Remove("TaxShift")

    '    HttpContext.Current.Session.Remove("LTTMap")
    '    HttpContext.Current.Session.Remove("BoundaryChangeStale")
    'End Sub

    'Public Shared Sub LTTMAP_InitRequiredSessionVars()
    '    System.Web.HttpContext.Current.Session.Remove("MapTaxStatusFilters")
    '    System.Web.HttpContext.Current.Session.Remove("MapTaxShiftFilters")
    '    System.Web.HttpContext.Current.Session.Remove("TaxStatus")
    '    System.Web.HttpContext.Current.Session.Remove("TaxShift")

    '    If IsNothing(System.Web.HttpContext.Current.Session("LTTMap")) Then
    '        System.Web.HttpContext.Current.Session.Add("LTTMap", True)
    '    End If
    '    HttpContext.Current.Session.Remove("BoundaryChangeStale")
    '    'If IsNothing(System.Web.HttpContext.Current.Session("BoundaryChangeStale")) Then
    '    '	System.Web.HttpContext.Current.Session.Add("BoundaryChangeStale", 3) 'BOUNDARY_CHANGE_STATE.LTT
    '    'Else
    '    '	System.Web.HttpContext.Current.Session("BoundaryChangeStale") = 3	'BOUNDARY_CHANGE_STATE.LTT
    '    'End If
    'End Sub

    'Public Shared Sub BOUNDARYCHANGE_InitRequiredSessionVars()
    '    System.Web.HttpContext.Current.Session.Remove("MapTaxStatusFilters")
    '    System.Web.HttpContext.Current.Session.Remove("MapTaxShiftFilters")
    '    System.Web.HttpContext.Current.Session.Remove("TaxStatus")
    '    System.Web.HttpContext.Current.Session.Remove("TaxShift")

    '    HttpContext.Current.Session.Remove("LTTMap")
    '    If IsNothing(System.Web.HttpContext.Current.Session("BoundaryChangeStale")) Then
    '        System.Web.HttpContext.Current.Session.Add("BoundaryChangeStale", False)
    '    End If
    '    System.Web.HttpContext.Current.Session("BoundaryChangeStale") = False
    'End Sub

End Class

