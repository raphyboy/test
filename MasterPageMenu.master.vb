Imports System.Data
Partial Class MasterPageMenu
    Inherits System.Web.UI.MasterPage
    Dim C As New Cls_Data
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session("uemail") = "weraphon.r"
        If Session("uemail") Is Nothing Then
            Session("current_url") = HttpContext.Current.Request.Url.AbsoluteUri()
            Response.Redirect("~/index.aspx")
        Else
            lblUser.Text = Session("uemail") + " (Logout)"
        End If
        If Not Page.IsPostBack Then
            Dim strSql As String
            Dim DT As New DataTable
            strSql = "select * from dbo.UserLogin where login_name = '" & Session("uemail") & "' "
            DT = C.GetDataTable(strSql)
            If DT.Rows.Count > 0 Then
                Session("Login_permission") = DT.Rows(0).Item("Login_permission")
                If Session("Login_permission") = "administrator" Then
                    Create.Visible = True
                    Edit.Visible = True
                    Approve.Visible = True
                    Create.HRef = "Default.aspx?menu=create"
                    Edit.HRef = "edit_list.aspx?menu=edit"
                    Approve.HRef = "approve_list.aspx?menu=approve"
                    Create.Attributes("class") = "list-group-item active"
                ElseIf Session("Login_permission") = "inspector" Then
                    'Edit.Attributes("class") = "list-group-item active"
                    Create.Visible = False
                    Edit.Visible = True
                    Approve.Visible = False
                    Edit.HRef = "edit_list.aspx?menu=edit"
                    Edit.Attributes("class") = "list-group-item active"
                ElseIf Session("Login_permission") = "user" Then
                    'Create.Attributes("class") = "list-group-item active"
                    Create.Visible = True
                    Edit.Visible = True
                    Approve.Visible = False
                    Create.HRef = "Default.aspx?menu=create"
                    Edit.HRef = "edit_list.aspx?menu=edit"
                    Create.Attributes("class") = "list-group-item active"
                End If
            End If
            If Session("menu") = "create" Then
                Create.Attributes("class") = "list-group-item active"
                Edit.Attributes("class") = "list-group-item"
                Approve.Attributes("class") = "list-group-item"
            ElseIf Session("menu") = "edit" Then
                Create.Attributes("class") = "list-group-item"
                Edit.Attributes("class") = "list-group-item active"
                Approve.Attributes("class") = "list-group-item"
            ElseIf Session("menu") = "approve" Then
                Create.Attributes("class") = "list-group-item"
                Edit.Attributes("class") = "list-group-item"
                Approve.Attributes("class") = "list-group-item active"
            End If
        End If
    End Sub

    Protected Sub lblUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblUser.Click
        Session.Clear()
        Response.Redirect("~/index.aspx")
        'Response.Redirect("https://api.jasmine.com/authen1/jaslogout-page")
    End Sub
End Class

