Public Partial Class help
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Redirects user to login page 
			'if he hasn't successfully logged in or
			'his session expires
			If Not IsNothing(Session("userID")) Then
				Dim currentNode As String
				currentNode = Session("currentNode")
				'Dim currentNodeTittle As String
				'currentNodeTittle = Session("currentNodeTittle")
				'Session.Remove("currentNode")
				'Session.Remove("currentNodeTittle")

				'setup database connection
				Dim con As New SqlClient.SqlConnection
				con.ConnectionString = PATMAP.Global_asax.connString
				con.Open()

				Dim dr As SqlClient.SqlDataReader
				Dim query As New SqlClient.SqlCommand

				query.Connection = con
				query.CommandText = "select tbl1.* from helpScreens tbl1 join screenNames tbl2 on tbl1.screenNameID=tbl2.screenNameID where tbl2.pageAddress='" & currentNode & "'"
				dr = query.ExecuteReader

				If dr.Read() Then
					lablHelpContent.Text = Server.HtmlDecode(dr.GetValue(3))
				Else
					lablHelpContent.Text = "Sorry, No help is available"
				End If

				'lblsubHeader.Text = currentNodeTittle

				'clean up
				dr.Close()
				con.Close()

				'Remarked as was creating problem when help is viewed 15oct2014
				''sets table for Local Tax Tools Module
				'If HttpContext.Current.Session("liveLTTtableCreated") Then
				'	common.deleteLiveLTTtable()
				'End If

			End If
		Catch
			'Displays error message and code
			common.GetErrorMessage(Err.Number, Err)
			Exit Sub
		End Try

		If IsNothing(Session("userID")) Then
			Response.Redirect("~/index.aspx")
		End If
	End Sub
End Class