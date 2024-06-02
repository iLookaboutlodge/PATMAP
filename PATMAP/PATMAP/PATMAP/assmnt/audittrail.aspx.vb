Public Partial Class audittrail
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'sets submenu to be displayed
                subMenu.setStartNode(menu.assmnt)

                Dim levelID As Integer = Session("levelID")

                'Presentation users has to re-save current scenario using 
                'a different scenario name
                If levelID = 3 Then
                    btnSave.ImageUrl = "~/images/btnSaveAs.gif"
                End If

                'gets Tax Year Model names used by the scenario 
                common.GetModelNames(txtBaseTaxYrModel.Text, txtSubjectTaxYrModel.Text, Session("userID"), txtScenarioName.Text)

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                Dim com As New SqlClient.SqlCommand
                con.ConnectionString = PATMAP.Global_asax.connString
                con.Open()

                'retrieves audit trail information for current scenario
                com.Connection = con
                com.CommandText = "select auditTrailText from liveAssessmentTaxModel where userID=" & Session("userID").ToString
                Dim dr As SqlClient.SqlDataReader
                dr = com.ExecuteReader()
                dr.Read()
                txtAuditTrail.Text = dr.GetValue(0).ToString

                'clean up
                dr.Close()
                con.Close()

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub audittrail_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        'redirects user back to Start page if there's no recalled scenario selected or
        'not creating a new scenario
        'prevent user from going to this page by typing in the url directly on the browser 
        'without starting from the Start page
        If IsNothing(Session("assessmentTaxModelID")) Then
            Response.Redirect("start.aspx")
        End If
    End Sub

    Public Sub changeName(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim errCode As String

            'updates scenario name
            'validates scenario for invalid chars and duplicate name
            errCode = common.UpdateScenarioName(Session("userID"), Session("assessmentTaxModelID"), txtScenarioName.Text)

            'displays error message
            If errCode <> "" Then
                Master.errorMsg = common.GetErrorMessage(errCode)
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            Dim errCode As String

            'scenario name is required before saving scenario
            If Trim(txtScenarioName.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP99")
                Exit Sub
            End If

            'updates scenario name
            'validates scenario for invalid chars and duplicate name
            errCode = common.UpdateScenarioName(Session("userID"), Session("assessmentTaxModelID"), txtScenarioName.Text)

            'display error message
            If errCode <> "" Then
                Master.errorMsg = common.GetErrorMessage(errCode)
                Exit Sub
            End If

            'updates core DB table
            common.saveLiveModelTables(Session("userID"))

        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("start.aspx")
    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExport.Click
        Try
            'exports audit trail information into a .txt file
            Response.Clear()
            Response.AddHeader("content-disposition", "attachment;filename=auditTrail.txt")
            Response.Charset = ""
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.ContentType = "application/vnd.txt"

            Dim stringWrite As New System.IO.StringWriter()
            Dim htmlWrite As New HtmlTextWriter(stringWrite)
            htmlWrite.Write(txtAuditTrail.Text)
            Response.Write(stringWrite.ToString())
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.End()
    End Sub
End Class