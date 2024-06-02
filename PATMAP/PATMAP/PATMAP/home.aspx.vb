Imports Microsoft.SqlServer.Dts.Runtime
Partial Class home
    Inherits System.Web.UI.Page

    Protected Sub btnUserProfile_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnUserProfile.Click
        Response.Redirect("userprofile.aspx")
    End Sub

	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			If Not IsPostBack Then

				'get userID
				Dim userID As Integer
				userID = Session("userID")

				Using con As SqlClient.SqlConnection = New SqlClient.SqlConnection(PATMAP.Global_asax.connString)
					If Not con.State = ConnectionState.Open Then
						con.Open()
					End If

					'setup the query to get user details
					Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
						query.Connection = con
						query.CommandText = "select loginName from users where userID = @userID"
						query.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID
						Using dr As SqlClient.SqlDataReader = query.ExecuteReader
							dr.Read()
							lblUsername.Text = dr.GetValue(0)
							If IsNothing(Session("username")) Then
								Session.Add("username", dr.GetValue(0))
							End If
						End Using
					End Using

					'get user group
					Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
						query.Connection = con
						query.CommandText = "select groupName from users inner join groups on users.groupID = groups.groupID where userID = @userID"
						query.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID
						Using dr As SqlClient.SqlDataReader = query.ExecuteReader
							If dr.Read() Then
								lblUserGroup.Text = dr.GetValue(0)
							Else
								lblUserGroup.Text = ""
							End If
						End Using
					End Using

					'get user levels
					Using da As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter
						da.SelectCommand = New SqlClient.SqlCommand
						da.SelectCommand.Connection = con
						da.SelectCommand.CommandText = "select levels.levelID as levelID, levelName from userlevels inner join levels on userlevels.levelID = levels.levelID where userID = @userID order by levels.levelID"
						da.SelectCommand.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID

						Using dt As DataTable = New DataTable
							da.Fill(dt)
							ddlUserLevel.DataSource = dt
							ddlUserLevel.DataValueField = "levelID"
							ddlUserLevel.DataTextField = "levelName"
							ddlUserLevel.DataBind()
						End Using
					End Using

					If IsNothing(Session("levelID")) Then
						Session.Add("levelID", ddlUserLevel.SelectedValue)
					Else
						ddlUserLevel.SelectedValue = Session("levelID")
					End If

					If IsNothing(Session("userlevel")) Then
						Session.Add("userlevel", ddlUserLevel.SelectedItem.Text)
					End If

					'get last login time
					Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
						query.Connection = con
						query.CommandText = "select isnull(max(logindatetime),'') from userstatistics where userID = @userID and logindatetime <> (select max(logindatetime) from userstatistics where userID = @userID)"
						query.Parameters.Add(New SqlClient.SqlParameter("@userID", SqlDbType.Int)).Value = userID
						Using dr As SqlClient.SqlDataReader = query.ExecuteReader
							dr.Read()
							lblLastLogin.Text = dr.GetValue(0)
						End Using
					End Using

					'get synchronize dates
					Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
						query.Connection = con
						query.CommandText = "select (select max(dateLoaded) from assessmentDescription) as assessmentLastLoaded, (select max(dateLoaded) from millRateSurveyDescription) as millRateLastLoaded"
						Using dr As SqlClient.SqlDataReader = query.ExecuteReader
							If dr.Read() Then
								If Not IsDBNull(dr.Item(0)) Then
									lblAssmntUpdt.Text = dr.Item(0)
								End If
								If Not IsDBNull(dr.Item(1)) Then
									lblMillRateUpdt.Text = dr.Item(1)
								End If
							End If
						End Using
					End Using

				End Using

			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

	'Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	'    Try
	'        If Not IsPostBack Then

	'            '*** Inky's Update (May-2010): This section removed during re-tooling project as Satellite function no longer part of PATMAP
	'            '''''btnSynchronize.Visible = True
	'            '***Inky: End***

	'            'get userID
	'            Dim userID As Integer
	'            userID = Session("userID")

	'            'setup database connection
	'            Dim con As New SqlClient.SqlConnection
	'            con.ConnectionString = PATMAP.Global_asax.connString

	'            'setup the query to get user details
	'            Dim query As New SqlClient.SqlCommand
	'            Dim dr As SqlClient.SqlDataReader
	'            query.Connection = con
	'            con.Open()

	'            'get user name 
	'            query.CommandText = "select loginName from users where userID = " & userID.ToString
	'            dr = query.ExecuteReader
	'            dr.Read()
	'            lblUsername.Text = dr.GetValue(0)

	'            If IsNothing(Session("username")) Then
	'                Session.Add("username", dr.GetValue(0))
	'            End If

	'            dr.Close()

	'            'get user group
	'            query.CommandText = "select groupName from users inner join groups on users.groupID = groups.groupID where userID = " & userID.ToString
	'            dr = query.ExecuteReader
	'            If dr.Read() Then
	'                lblUserGroup.Text = dr.GetValue(0)
	'            Else
	'                lblUserGroup.Text = ""
	'            End If
	'            dr.Close()

	'            'get user levels
	'            Dim da As New SqlClient.SqlDataAdapter
	'            da.SelectCommand = New SqlClient.SqlCommand("select levels.levelID as levelID, levelName from userlevels inner join levels on userlevels.levelID = levels.levelID where userID = " & userID.ToString & " order by levels.levelID", con)
	'            Dim dt As New DataTable
	'            da.Fill(dt)
	'            ddlUserLevel.DataSource = dt
	'            ddlUserLevel.DataValueField = "levelID"
	'            ddlUserLevel.DataTextField = "levelName"
	'            ddlUserLevel.DataBind()

	'            If IsNothing(Session("levelID")) Then
	'                Session.Add("levelID", ddlUserLevel.SelectedValue)
	'            Else
	'                ddlUserLevel.SelectedValue = Session("levelID")
	'            End If

	'            If IsNothing(Session("userlevel")) Then
	'                Session.Add("userlevel", ddlUserLevel.SelectedItem.Text)
	'            End If

	'            'get last login time
	'            query.CommandText = "select isnull(max(logindatetime),'') from userstatistics where userID = " & userID.ToString & " and logindatetime <> (select max(logindatetime) from userstatistics where userID = " & userID.ToString & ")"
	'            dr = query.ExecuteReader
	'            dr.Read()
	'            lblLastLogin.Text = dr.GetValue(0)
	'            dr.Close()

	'            '*** Inky's Update (May-2010): This section removed during re-tooling project as Satellite function no longer part of PATMAP
	'            '''''''get permission to synchronize button
	'            '''''query.CommandText = "select count(*) from levelsPermission where screenNameID = 90 and access = 1 and levelID = " & Session("levelID")
	'            '''''dr = query.ExecuteReader

	'            '''''If dr.Read() Then
	'            '''''    If dr.Item(0) = 0 Then
	'            '''''        btnSynchronize.Visible = False
	'            '''''    End If
	'            '''''End If

	'            '''''dr.Close()
	'            '*** Inky: End ***

	'            'get synchronize dates
	'            query.CommandText = "select (select max(dateLoaded) from assessmentDescription) as assessmentLastLoaded, (select max(dateLoaded) from millRateSurveyDescription) as millRateLastLoaded"
	'            dr = query.ExecuteReader

	'            If dr.Read() Then
	'                If Not IsDBNull(dr.Item(0)) Then
	'                    lblAssmntUpdt.Text = dr.Item(0)
	'                End If
	'                If Not IsDBNull(dr.Item(1)) Then
	'                    lblMillRateUpdt.Text = dr.Item(1)
	'                End If
	'            End If

	'            dr.Close()

	'            '*** Inky's Update (May-2010): This section removed during re-tooling project as Satellite function no longer part of PATMAP
	'            '''''query.CommandText = "select lastUpdate from satelliteInfo where machineName='" & System.Configuration.ConfigurationManager.AppSettings("MachineName") & "'"
	'            '''''dr = query.ExecuteReader

	'            '''''If dr.Read() Then
	'            '''''SatelliteUpdt.Text &= dr.Item(0)
	'            '''''End If

	'            '''''dr.Close()
	'            '*** Inky: End ***

	'            '*** Inky's Update (May-2010): This section removed during re-tooling project as Satellite function no longer part of PATMAP
	'            'Disable the drop down list when the user in satellite mode                
	'            '''''If PATMAP.Global_asax.satellite Then

	'            '''''    btnSynchronize.Visible = False

	'            '''''    ddlUserLevel.SelectedValue = 3
	'            '''''    ddlUserLevel.Enabled = False
	'            '''''    Session("userlevel") = ddlUserLevel.SelectedItem.Text
	'            '''''    Session("levelID") = ddlUserLevel.SelectedValue
	'            '''''End If
	'            '*** Inky: End ***

	'            'cleanup
	'            con.Close()

	'        End If
	'    Catch
	'        'retrieves error message
	'        Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
	'    End Try
	'End Sub

    Private Sub ddlUserLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserLevel.SelectedIndexChanged
        Try
            Session("userlevel") = ddlUserLevel.SelectedItem.Text
            Session("levelID") = ddlUserLevel.SelectedValue

            'sets table for Local Tax Tools Module
            If HttpContext.Current.Session("liveLTTtableCreated") Then
                common.deleteLiveLTTtable()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("home.aspx")

    End Sub

    '*** Inky's Update (May-2010): This section removed during re-tooling project as Satellite function no longer part of PATMAP
    '''''Private Sub btnSynchronize_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSynchronize.Click
    '''''    Response.Redirect("synchronize.aspx")
    '''''End Sub
    '*** Inky: End ***
End Class


