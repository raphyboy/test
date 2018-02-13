Imports System.Data
Partial Class edit_list
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("menu") IsNot Nothing Then
                Session("menu") = Request.QueryString("menu")
            End If
            Dim DT_List As DataTable
            If Session("Login_permission") = "user" Then
                DT_List = C.GetDataTable("select Document_No, CONVERT(varchar(10),Document_Date,103) Document_Date, Area, Cluster, Customer_Name from FeasibilityDocument where request_status = 110 ")
                GridView1.DataSource = DT_List
                GridView1.DataBind()
            ElseIf Session("Login_permission") = "inspector" Then
                DT_List = C.GetDataTable("select Document_No, CONVERT(varchar(10),Document_Date,103) Document_Date, Area, Cluster, Customer_Name from FeasibilityDocument where request_status = 55 ")
                GridView1.DataSource = DT_List
                GridView1.DataBind()
            End If

        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(0).FindControl("Link_DocumentID"), HyperLink).NavigateUrl = "http://posweb.triplet.co.th/feasibility/edit_capex.aspx?request_id=" + CType(e.Row.Cells(0).FindControl("Link_DocumentID"), HyperLink).Text
            'CType(e.Row.Cells(0).FindControl("Link_DocumentID"), HyperLink).Target = ""
        End If
    End Sub


End Class
