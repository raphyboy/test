Imports System.Data
Partial Class add_opex
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim DT_List As DataTable
            
            C.SetDropDownList(DropDownList1, "select * from OPEX", "OPEX_Name", "OPEX_id")
            DT_List = C.GetDataTable("select * from List_OPEX where CreateBy = '" + Session("uemail") + "' and Document_No is null ")
            GridView1.DataSource = DT_List
            GridView1.DataBind()

        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim DT As DataTable
        Dim DT_List As DataTable
        Dim strSql As String
        Dim r As String = ""
        If IsNumeric(TextBox4.Text) = True Then
            DT = C.GetDataTable("select * from OPEX where OPEX_id='" + DropDownList1.SelectedValue + "'")

            If DT.Rows.Count > 0 Then
                strSql = "insert into List_OPEX (OPEX_Name, Number, Cost, CreateBy, CreateDate) values('" + DT.Rows(0).Item("OPEX_Name").ToString + "','" + TextBox4.Text + "','" + (CInt(DT.Rows(0).Item("OPEX_Cost")) * CInt(TextBox4.Text)).ToString + "','" + Session("uemail") + "',getdate()) "
                C.ExecuteNonQuery(strSql)
                DT_List = C.GetDataTable("select * from List_OPEX where CreateBy = '" + Session("uemail") + "' and Document_No is null ")
                GridView1.DataSource = DT_List
                GridView1.DataBind()

            End If
        Else
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('�кبӹǹ�繵���Ţ��ҹ��!');", True)
            TextBox4.Focus()
        End If

    End Sub

    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        Dim id_list As String
        Dim strSql As String

        id_list = CType(GridView1.Rows(e.RowIndex).Cells(2).FindControl("lblID"), Label).Text
        strSql = "delete from List_OPEX where id_List = '" + id_list + "' "
        C.ExecuteNonQuery(strSql)


        Dim DT_List As DataTable
        Dim r As String = ""
        C.SetDropDownList(DropDownList1, "select * from OPEX", "OPEX_Name", "OPEX_id")
        DT_List = C.GetDataTable("select * from List_OPEX where CreateBy = '" + Session("uemail") + "' and Document_No is null ")
        GridView1.DataSource = DT_List
        GridView1.DataBind()
    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        Response.Redirect("add_service.aspx")
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim strSql As String
        Dim i As Integer
        If GridView1.Rows.Count > 0 Then
            For i = 0 To GridView1.Rows.Count - 1
                strSql = "Update List_OPEX Set OPEX_Name='" + GridView1.Rows(i).Cells(0).Text + "', Number='" + GridView1.Rows(i).Cells(1).Text + "', Cost='" + CType(GridView1.Rows(i).Cells(2).FindControl("TextBox3"), TextBox).Text + "', CreateBy='" + Session("uemail") + "', CreateDate = getdate() where id_List='" + CType(GridView1.Rows(i).Cells(3).FindControl("lblID"), Label).Text + "' "
                C.ExecuteNonQuery(strSql)
            Next
        End If
    End Sub
End Class
