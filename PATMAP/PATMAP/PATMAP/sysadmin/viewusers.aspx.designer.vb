'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.312
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On



'''<summary>
'''viewusers class.
'''</summary>
'''<remarks>
'''Auto-generated class.
'''</remarks>
Partial Public Class viewusers

    '''<summary>
    '''subMenu control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents subMenu As Global.PATMAP.tabmenu

    '''<summary>
    '''btnAdd control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents btnAdd As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''lblName control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents lblName As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtName control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtName As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''btnHName control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents btnHName As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''lblUsername control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents lblUsername As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtUsername control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtUsername As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''btnHUsername control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents btnHUsername As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''lblUserGroup control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents lblUserGroup As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''ddlUserGroup control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents ddlUserGroup As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''btnHUserGroup control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents btnHUserGroup As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''lblUserLevel control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents lblUserLevel As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''ddlUserLevel control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents ddlUserLevel As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''btnHUserLevel control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents btnHUserLevel As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''btnSearch control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents btnSearch As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''uplUsers control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents uplUsers As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''lblTotalUser control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents lblTotalUser As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtTotalUser control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtTotalUser As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''grdUsers control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents grdUsers As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''uplPending control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents uplPending As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''lblPendingRequest control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents lblPendingRequest As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtPendingRequest control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtPendingRequest As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''grdRequests control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents grdRequests As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Master property.
    '''</summary>
    '''<remarks>
    '''Auto-generated property.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As PATMAP.MasterPage
        Get
            Return CType(MyBase.Master, PATMAP.MasterPage)
        End Get
    End Property
End Class
