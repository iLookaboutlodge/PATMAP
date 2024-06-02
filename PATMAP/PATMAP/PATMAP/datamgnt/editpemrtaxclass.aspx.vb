Imports System.Data.SqlClient

Partial Public Class editpemrtaxclass
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            'check if its the first load or a post back
            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                'fill the lonely text box... :(
                getData()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub getData()
        Dim con As New SqlConnection
        Dim query As New SqlCommand
        Dim dr As SqlDataReader = Nothing

        Try
            If Not IsNothing(Request.QueryString("mainTaxClassID")) Then
                'setup database connection
                con.ConnectionString = PATMAP.Global_asax.connString
                query.Connection = con
                con.Open()

                query.CommandText = "select mainTaxClass from PEMRMainTaxClasses where mainTaxClassID = @mainTaxClassID"
                query.Parameters.AddWithValue("@mainTaxClassID", Request.QueryString("mainTaxClassID"))
                dr = query.ExecuteReader

                'fill textbox with PEMR taxclass name, if editing, or leave blank if adding new class
                If dr.Read() Then
                    If Not IsDBNull(dr.Item(0)) Then
                        txtTaxClass.Text = dr.Item(0)
                    End If
                End If
            Else
                txtTaxClass.Text = ""
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        Finally
            If dr IsNot Nothing Then
                dr.Close()
            End If

            If con IsNot Nothing Then
                con.Close()
            End If
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Dim connection As SqlConnection = Nothing
        Dim command As SqlCommand = Nothing
        Dim result As Integer = 0

        Try
            'PEMR Tax Class is required.
            If txtTaxClass.Text.Trim = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP161")
                Exit Sub
            End If

            connection = New SqlConnection(PATMAP.Global_asax.connString)
            connection.Open()

            If Request.QueryString("mainTaxClassID") IsNot Nothing Then 'Edit
                command = New SqlCommand("PEMRMainTaxClassesUpdate", connection)

                With command.Parameters
                    .AddWithValue("@mainTaxClassID", CInt(Request.QueryString("mainTaxClassID")))
                    .AddWithValue("@mainTaxClass", txtTaxClass.Text.Trim)
                End With
            Else    'Add
                command = New SqlCommand("PEMRMainTaxClassesInsert", connection)
                command.Parameters.AddWithValue("@mainTaxClass", txtTaxClass.Text.Trim)
            End If

            command.CommandType = CommandType.StoredProcedure
            result = command.ExecuteNonQuery()

            If result < 1 Then
                Master.errorMsg = "Unable to save PEMR tax class."
            Else
                Response.Redirect("viewpemrtaxclass.aspx")
            End If
        Catch ex As Exception
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        Finally
            If connection IsNot Nothing Then
                connection.Close()
            End If
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Response.Redirect("viewpemrtaxclass.aspx")
    End Sub
End Class