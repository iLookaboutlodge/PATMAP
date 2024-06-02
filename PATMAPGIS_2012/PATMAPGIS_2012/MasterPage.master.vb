
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Private _helpButtons As ArrayList   'Donna

    'ErrorMsg Property
    'Retrieves or Sets text displayed from
    'lblErrorText control
    Public Property errorMsg() As String
        Get
            Return lblErrorText.Text
        End Get
        Set(ByVal value As String)
            lblErrorText.Text = value
        End Set
    End Property

    'curUser Property
    'Retrieves or Sets username displayed from
    'lblCurUser control
    Public Property curUser() As String
        Get
            Return lblCurUser.Text
        End Get
        Set(ByVal value As String)
            lblCurUser.Text = value
        End Set
    End Property

    'curRole Property
    'Retrieves or Sets user level displayed from
    'lblCurRole control
    Public Property curRole() As String
        Get
            Return lblCurRole.Text
        End Get
        Set(ByVal value As String)
            lblCurRole.Text = value
        End Set
    End Property

    'helpText Property
    'Retrieves or Sets text displayed from
    'lblHelp control
    Public Property helpText() As String
        Get
            Return lblHelp.Text
        End Get
        Set(ByVal value As String)
            lblHelp.Text = value
        End Set
    End Property

    'Donna start
    Public Property HelpButtons() As ArrayList
        Get
            Return _helpButtons
        End Get
        Set(ByVal value As ArrayList)
            _helpButtons = value
        End Set
    End Property
    'Donna end

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'Redirects user to login page 
        'if he hasn't successfully logged in or
        'his session expires

        If IsNothing(Session("userID")) Then
            Response.Redirect("~/index.aspx")
        Else

            Dim errorCode As String = ""

            Try

                Dim url As String
                Dim node As SiteMapNode

                node = SiteMap.CurrentNode

                If InStr(node.Url, "edit") Then
                    url = node.ParentNode.Url
                Else
                    url = node.Url
                End If

                If InStr(node.Url, "datamgnt/view") Then

                    '*****Inky's Code: assign "Edit Data" startpage URL dynamically...
                    'setup database connection
                    Dim con As New SqlClient.SqlConnection
                    con.ConnectionString = PATMAP.Global_asax.connString
                    con.Open()

                    Dim query As New SqlClient.SqlCommand
                    Dim dataReader As SqlClient.SqlDataReader
                    query.Connection = con

                    query.CommandText = "SELECT screenNameID, access FROM levelsPermission WHERE levelID = " & HttpContext.Current.Session("levelID") & "  AND screenNameID IN (55, 56, 58, 61, 63, 77, 25, 8, 31, 32)" '''', 124 *** Page 124 not needed anylonger.
                    'query.CommandText = "SELECT screenNameID, access FROM levelsPermission WHERE levelID = " & HttpContext.Current.Session("levelID") & "  AND screenNameID IN (55, 56, 58, 61, 63, 77) order by screenNameID"
                    dataReader = query.ExecuteReader

                    Dim screenID As Integer = 0
                    Dim editDataAccess As Boolean = False

                    'Assign URL based on level selection
                    If dataReader.HasRows Then
                        While dataReader.Read()
                            screenID = dataReader.GetValue(0)
                            editDataAccess = dataReader.GetValue(1)

                            If (screenID = 55) And (editDataAccess = True) Then
                                url = "/datamgnt/viewassessment.aspx"
                                Exit While

                                ''''**** Inky (Apr-2010) Tax Credit page removed as per 2010 PATMAP retooling
                                ''''''ElseIf (screenID = 56) And (editDataAccess = True) Then
                                ''''''    url = "/datamgnt/viewtaxcredit.aspx"
                                ''''''    Exit While

                            ElseIf (screenID = 58) And (editDataAccess = True) Then
                                url = "/datamgnt/viewmillrate.aspx"
                                Exit While

                            ElseIf (screenID = 61) And (editDataAccess = True) Then
                                url = "/datamgnt/viewpov.aspx"
                                Exit While

                            ElseIf (screenID = 63) And (editDataAccess = True) Then
                                url = "/datamgnt/viewpotash.aspx"
                                Exit While

                                ''''**** Inky (Apr-2010) K-12 page removed as per 2010 PATMAP retooling
                                ''''''ElseIf (screenID = 77) And (editDataAccess = True) Then
                                ''''''    url = "/datamgnt/viewkog.aspx"
                                ''''''    Exit While
                            Else

                                If (screenID = 8) And (editDataAccess = True) Then
                                    url = "/datamgnt/viewtaxclass.aspx"
                                    Exit While

                                ElseIf (screenID = 25) And (editDataAccess = True) Then
                                    url = "/datamgnt/viewtaxentity.aspx"
                                    Exit While

                                ElseIf (screenID = 31) And (editDataAccess = True) Then
                                    url = "/datamgnt/viewjuristype.aspx"
                                    Exit While

                                ElseIf (screenID = 32) And (editDataAccess = True) Then
                                    url = "/datamgnt/viewpropcode.aspx"
                                    Exit While

                                    ''''''ElseIf (screenID = 124) And (editDataAccess = True) Then '***Inky's Addition: Apr-2010 *** NOT NECESSARY ANYLONGER - PAGE NOT USED ***
                                    ''''''    url = "/datamgnt/viewlttrollup.aspx"
                                    ''''''    Exit While
                                End If

                            End If
                        End While
                    End If

                    dataReader.Close()
                    con.Close()

                    '**** End of Inky's code

                End If

                url = Replace(url, "?id=def", "")

                'This code block was uncommented on Nov-19-2008
                If url.LastIndexOf("/") > -1 Then
                    url = url.Substring(url.LastIndexOf("/"))
                End If
                'End of uncommented code block

                errorCode = common.HasAccess(Session("levelID"), url)

            Catch
                'Displays error message and code
                errorMsg = common.GetErrorMessage(Err.Number, Err)
                Exit Sub
            End Try

            If errorCode <> "" Then
                Session("responseCode") = errorCode
                Response.Redirect("~/error.aspx")
            End If
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try

                'Dynamically loads items into menu control
                menu.loadMenu(mainMenu, Session)

                'sets username and role
                lblCurUser.Text = Session("username")
                lblCurRole.Text = Session("userlevel")

                Dim helpText As String = String.Empty

                'Donna start
                If _helpButtons Is Nothing Then
                    'retrieves form field help text
                    helpText = common.GetHelpText(Page.AppRelativeVirtualPath, Page)
                Else
                    helpText = common.GetHelpText(Page.AppRelativeVirtualPath, _helpButtons)
                End If

                lblHelp.Text = ""
                lblHelp.Text = helpText

            Catch
                'Displays error message and code
                errorMsg = common.GetErrorMessage(Err.Number, Err)
            End Try
        End If

    End Sub
    
End Class

