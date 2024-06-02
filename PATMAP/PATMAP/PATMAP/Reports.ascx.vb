Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections.Specialized
Imports System.Text

Partial Public Class Reports
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim getMapValues As String = (("getMapValues('" + ConfigurationManager.AppSettings("MunicipalitiesLayerName") & "','") + ConfigurationManager.AppSettings("SchoolDivisionsLayerName") & "','") + ConfigurationManager.AppSettings("ParcelLayerName") & "');"
        Me.btnOpenTables.OnClientClick = getMapValues
        Me.btnOpenGraphs.OnClientClick = getMapValues
    End Sub

    Private Sub setSession()
        Dim formVars As NameValueCollection = Me.Page.Request.Form

        If formVars("MunicipalityID") IsNot Nothing AndAlso formVars("MunicipalityID").Length > 0 Then
            Session.Add("Municipalities", formVars("MunicipalityID"))
        End If

        If formVars("SchoolID") IsNot Nothing AndAlso formVars("SchoolID").Length > 0 Then
            Session.Add("SchoolDistricts", formVars("SchoolID"))
        End If

        If formVars("ParcelID") IsNot Nothing AndAlso formVars("ParcelID").Length > 0 Then
            Session.Add("ParcelID", formVars("ParcelID"))
        End If

        ''If MapSettings.MapPropertyClassFilters IsNot Nothing AndAlso MapSettings.MapPropertyClassFilters.Count = 1 Then
        ''    Session.Add("TaxClass", MapSettings.MapPropertyClassFilters(0))
        ''End If

        ''Session.Add("TaxType", If(MapSettings.MapAnalysisLayer.Equals("Municipalities"), "1", "2"))

        ''If MapSettings.TaxStatusFilters IsNot Nothing AndAlso MapSettings.TaxStatusFilters.Count = 1 Then
        ''    Dim taxStatusFilter As New StringBuilder()
        ''    For Each taxStatus As String In MapSettings.TaxStatusFilters
        ''        taxStatusFilter.Append(taxStatus)
        ''        taxStatusFilter.Append(",")
        ''    Next
        ''    Session.Add("TaxStatusFilters", taxStatusFilter.ToString())
        ''End If
    End Sub

    Protected Sub btnOpenTables_Click(ByVal sender As Object, ByVal e As EventArgs)
        setSession()
        ''If BoundaryChangeSettings.BoundaryChangeState = BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE Then
        ''    Me.Page.ClientScript.RegisterStartupScript(Me.[GetType](), "OpenReport", "parent.document.location='assmnt/tables.aspx'", True)
        ''ElseIf BoundaryChangeSettings.BoundaryChangeState = BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT Then
        ''    Me.Page.ClientScript.RegisterStartupScript(Me.[GetType](), "OpenReport", "parent.document.location='taxtools/tables.aspx'", True)
        ''Else
        ''    Me.Page.ClientScript.RegisterStartupScript(Me.[GetType](), "OpenReport", "parent.document.location='boundary/tables.aspx'", True)
        ''End If
    End Sub

    Protected Sub btnOpenGraphs_Click(ByVal sender As Object, ByVal e As EventArgs)
        setSession()
        ''If BoundaryChangeSettings.BoundaryChangeState = BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.NONE Then
        ''    Me.Page.ClientScript.RegisterStartupScript(Me.[GetType](), "OpenReport", "parent.document.location='assmnt/graphs.aspx'", True)
        ''ElseIf BoundaryChangeSettings.BoundaryChangeState = BoundaryChangeSettings.BOUNDARY_CHANGE_STATE.LTT Then
        ''    Me.Page.ClientScript.RegisterStartupScript(Me.[GetType](), "OpenReport", "parent.document.location='taxtools/graphs.aspx'", True)
        ''Else
        ''    Me.Page.ClientScript.RegisterStartupScript(Me.[GetType](), "OpenReport", "parent.document.location='boundary/graphs.aspx'", True)
        ''End If
    End Sub

End Class
