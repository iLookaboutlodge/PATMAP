Public Partial Class viewtaxentity
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not Page.IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                grdTaxEntity.PageSize = PATMAP.Global_asax.pageSize

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString

                'get jurisdiction types for drop down
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select -999 as jurisdictionTypeID,'<Select>' as jurisdictionType union select jurisdictionTypeID,jurisdictionType from jurisdictiontypes ", con)
                Dim dt As New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlJurisType.DataSource = dt
                ddlJurisType.DataValueField = "jurisdictionTypeID"
                ddlJurisType.DataTextField = "jurisdictionType"
                ddlJurisType.DataBind()

                'load the jurisdiction drop down based on the jurisdiction type value selected
                ddlJurisdiction.DataSource = common.FillMunicipality(-999, True)
                ddlJurisdiction.DataValueField = "number"
                ddlJurisdiction.DataTextField = "jurisdiction"
                ddlJurisdiction.DataBind()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    'Private Sub loadJurisdictiondropdown()
    '    'setup database connection
    '    Dim con As New SqlClient.SqlConnection
    '    con.ConnectionString = PATMAP.Global_asax.connString

    '    'get jurisdictions for drop down
    '    Dim da As New SqlClient.SqlDataAdapter
    '    'check if a jurisdiction type has been selected
    '    If String.IsNullOrEmpty(ddlJurisType.SelectedValue) Or ddlJurisType.SelectedValue = -999 Then
    '        'no type selected
    '        da.SelectCommand = New SqlClient.SqlCommand("select '-999A' as number,'<Select>' as jurisdiction union select number,jurisdiction from entities ", con)
    '    Else
    '        'type selected
    '        da.SelectCommand = New SqlClient.SqlCommand("select '-999A' as number,'<Select>' as jurisdiction union select number,jurisdiction from entities where jurisdictionTypeID = " & ddlJurisType.SelectedValue, con)
    '    End If
    '    Dim dt As New DataTable
    '    con.Open()
    '    da.Fill(dt)
    '    con.Close()
    '    ddlJurisdiction.DataSource = dt
    '    ddlJurisdiction.DataValueField = "number"
    '    ddlJurisdiction.DataTextField = "jurisdiction"
    '    ddlJurisdiction.DataBind()
    'End Sub

    Protected Sub ddlJurisType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlJurisType.SelectedIndexChanged
        Try
            'load the jurisdiction drop down based on the jurisdiction type value selected            
            ddlJurisdiction.DataSource = common.FillMunicipality(ddlJurisType.SelectedValue, True)
            ddlJurisdiction.DataValueField = "number"
            ddlJurisdiction.DataTextField = "jurisdiction"
            ddlJurisdiction.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            'fill grid with search results
            performEntitySearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub performEntitySearch()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'setup the query to get entity details
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand("select e.jurisdiction, e.number, j.jurisdictionType from entities e join jurisdictionTypes j on e.jurisdictionTypeID = j.jurisdictionTypeID where 1 = 1 ", con)

        If Not (String.IsNullOrEmpty(ddlJurisType.SelectedValue) Or ddlJurisType.SelectedValue = -999) Then
            da.SelectCommand.CommandText += " and e.jurisdictionTypeID = " & ddlJurisType.SelectedValue
        End If
        If Not (String.IsNullOrEmpty(Trim(ddlJurisdiction.SelectedValue))) Then
            da.SelectCommand.CommandText += " and number = '" & ddlJurisdiction.SelectedValue & "'"
        End If

        'get the data for the entity grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdTaxEntity.DataSource = dt
        grdTaxEntity.DataBind()

        If IsNothing(Cache("entities")) Then
            Cache.Add("entities", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("entities") = dt
        End If
        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("entityID", "-999A")
        Response.Redirect("edittaxentity.aspx")
    End Sub

    Private Sub grdTaxEntity_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdTaxEntity.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdTaxEntity.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("entities")) Then
                dt = CType(Cache("entities"), DataTable)
                grdTaxEntity.DataSource = dt
                grdTaxEntity.DataBind()
            Else
                performEntitySearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxEntity_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdTaxEntity.RowCommand
        Try
            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding userID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim entityID As String = grdTaxEntity.DataKeys(index).Values("number")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editEntity"
                        Session.Add("entityID", entityID)
                    Case "deleteEntity"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'delete user
                        query.CommandText = "delete from entities where number = '" & entityID & "'"
                        con.Open()
                        query.ExecuteNonQuery()
                        con.Close()

                        'load the jurisdiction drop down based on the jurisdiction type value selected

                        ddlJurisdiction.DataSource = common.FillMunicipality(ddlJurisType.SelectedValue, True)
                        ddlJurisdiction.DataValueField = "number"
                        ddlJurisdiction.DataTextField = "jurisdiction"
                        ddlJurisdiction.DataBind()

                        'update user search grid
                        performEntitySearch()

                        Master.errorMsg = ""
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editEntity" Then
            Response.Redirect("edittaxentity.aspx")
        End If

    End Sub

    Private Sub grdTaxEntity_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTaxEntity.RowDataBound
        Try
            'attaches confirm script to button
            common.confirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "number") & " - " & DataBinder.Eval(e.Row.DataItem, "jurisdiction"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxEntity_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdTaxEntity.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("entities")) Then
                performEntitySearch()
            End If

            dt = CType(Cache("entities"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdTaxEntity.DataSource = dt
            grdTaxEntity.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class