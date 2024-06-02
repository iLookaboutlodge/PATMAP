Imports System.Data.SqlClient

Partial Public Class viewfunctions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                grdFunctions.PageSize = PATMAP.Global_asax.pageSize
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            'fill grid with search results
            performFunctionSearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub performFunctionSearch()
        'setup database connection
        Dim con As New SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'setup the query to get entity details
        Dim da As New SqlDataAdapter
        Dim conStr As String

        txtFunctionName.Text = Trim(txtFunctionName.Text.Replace("'", "''"))

        '**********************************************************************************************
        '* Inky's Addition: Apr-2010 (added new 'access' colum to PATMAP.dbo.functions table. 'access'*
        '* was set to FALSE (0) in the table wherever "K12" is found in a "formula" column. Restricts *
        '* user-access to formulas using K12 as per 2010 PATMAP retooling project requirements        *
        '**********************************************************************************************
        'conStr = "select functionID, functionName, description from functions"
        conStr = "select functionID, functionName, description from functions where access = 1"
        '***Inky: End***

        If txtFunctionName.Text <> "" Then
            conStr &= " where functionName like '%" & txtFunctionName.Text & "%'"
        End If

        da.SelectCommand = New SqlCommand(conStr, con)

        'get the data for the entity grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdFunctions.DataSource = dt
        grdFunctions.DataBind()

        If IsNothing(Cache("functions")) Then
            Cache.Add("functions", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("functions") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If

    End Sub

    Private Sub grdFunctions_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdFunctions.RowCommand
        Try
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'find the row index which to be edited/deleted
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim editFunctionID As String = grdFunctions.DataKeys(index).Values("functionID")


                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editFunction"
                        Session.Add("editFunctionID", editFunctionID)

                    Case "deleteFunction"
                        'setup database connection
                        Dim con As New SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        con.Open()
                        Dim query As New SqlCommand
                        query.Connection = con

                        'delete function
                        query.CommandText = "delete from functions where functionID = '" & editFunctionID & "'" & vbCrLf
                        query.CommandText += "delete from functionsReset where functionID = '" & editFunctionID & "'"

                        Dim trans As SqlClient.SqlTransaction
                        trans = con.BeginTransaction()
                        query.Transaction = trans
                        Try
                            query.ExecuteNonQuery()
                            trans.Commit()
                        Catch
                            trans.Rollback()
                            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
                            con.Close()
                            Exit Sub
                        End Try

                        con.Close()

                        'update function search grid
                        performFunctionSearch()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editFunction" Then
            Response.Redirect("editfunctions.aspx")
        End If

    End Sub

    Private Sub grdFunctions_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdFunctions.RowDataBound
        Try
            'attaches confirm script to button
            'common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "functionName"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdFunctions_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdFunctions.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdFunctions.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("functions")) Then
                dt = CType(Cache("functions"), DataTable)
                grdFunctions.DataSource = dt
                grdFunctions.DataBind()
            Else
                performFunctionSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdFunctions_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdFunctions.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("functions")) Then
                performFunctionSearch()
            End If

            dt = CType(Cache("functions"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdFunctions.DataSource = dt
            grdFunctions.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editFunctionID", "")
        Response.Redirect("editfunctions.aspx")
    End Sub
End Class