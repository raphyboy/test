Imports System.Data
Partial Class edit_capex
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim trow As New HtmlTableRow
    Dim tcell As New HtmlTableCell
    Dim table As New HtmlTable
    Dim xRequest_id
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            xRequest_id = Request.QueryString("request_id")
            menu_opex.HRef = "edit_opex.aspx?request_id=" + xRequest_id
            menu_service.HRef = "edit_service.aspx?request_id=" + xRequest_id
            menu_summary.HRef = "edit_Summary.aspx?request_id=" + xRequest_id
            If Request.QueryString("menu") IsNot Nothing Then
                Session("menu") = Request.QueryString("menu")
            End If
            Dim dt_check As DataTable
            dt_check = C.GetDataTable("select * from dbo.FeasibilityDocument where (request_status = 55 or request_status = 110) and Document_No = '" + xRequest_id + "'")
            If dt_check.Rows.Count > 0 Then
                If ((dt_check.Rows(0).Item("request_status").ToString = "55" And Session("Login_permission") = "inspector") Or (dt_check.Rows(0).Item("request_status").ToString = "110" And dt_check.Rows(0).Item("CreateBy").ToString = Session("uemail"))) Then
                    Dim DT_List As DataTable
                    Dim r As String = ""
                    C.SetDropDownList(DropDownList1, "select * from CAPEX", "CAPEX_Name", "CAPEX_id")
                    DT_List = C.GetDataTable("select * from List_CAPEX where CreateBy = '" + Session("uemail") + "' and Document_No = '" + xRequest_id + "' ")
                    GridView1.DataSource = DT_List
                    GridView1.DataBind()
                Else
                    Button3.Visible = False
                    Button2.Visible = False
                End If

            Else
                Button3.Visible = False
                Button2.Visible = False
            End If

            'If DT_List.Rows.Count > 0 Then
            '    Dim i As Integer
            '    r = "<table id='table1'>"
            '    For i = 0 To DT_List.Rows.Count - 1
            '        r += "<tr>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("CAPEX_Name").ToString + "</td>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("Asset_Type").ToString + "</td>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("Cost").ToString + "</td>"
            '        r += "</tr>"
            '        'trow = New HtmlTableRow
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("CAPEX_Name").ToString
            '        'trow.Cells.Add(tcell)
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("Asset_Type").ToString
            '        'trow.Cells.Add(tcell)
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("Equipment_Cost").ToString
            '        'trow.Cells.Add(tcell)
            '        'table.Rows.Add(trow)
            '        'test.Controls.Add(table)
            '    Next
            '    r += "</table>"
            '    test.InnerHtml = r

            'End If
        End If
        'Label1.Text = test.InnerText

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim DT As DataTable
        Dim DT_List As DataTable
        Dim strSql As String
        Dim r As String = ""
        DT = C.GetDataTable("select * from CAPEX where CAPEX_id='" + DropDownList1.SelectedValue + "'")

        If DT.Rows.Count > 0 Then
            strSql = "insert into List_CAPEX (CAPEX_Name, Asset_Type, Cost, CreateBy, CreateDate, Document_No) values('" + DT.Rows(0).Item("CAPEX_Name").ToString + "','" + DT.Rows(0).Item("Asset_Type").ToString + "','" + DT.Rows(0).Item("Equipment_Cost").ToString + "','weraphon.r',getdate(),'" + xRequest_id + "') "
            C.ExecuteNonQuery(strSql)
            DT_List = C.GetDataTable("select * from List_CAPEX where CreateBy = '" + Session("uemail") + "'  and Document_No = '" + xRequest_id + "' ")
            GridView1.DataSource = DT_List
            GridView1.DataBind()
            'If DT_List.Rows.Count > 0 Then
            '    Dim i As Integer
            '    r = "<table id='table1' runat='server'>"
            '    For i = 0 To DT_List.Rows.Count - 1
            '        r += "<tr>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("CAPEX_Name").ToString + "</td>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("Asset_Type").ToString + "</td>"
            '        r += "<td style='border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid'>" + DT_List.Rows(i).Item("Cost").ToString + "</td>"
            '        r += "</tr>"
            '        'trow = New HtmlTableRow
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("CAPEX_Name").ToString
            '        'trow.Cells.Add(tcell)
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("Asset_Type").ToString
            '        'trow.Cells.Add(tcell)
            '        'tcell = New HtmlTableCell
            '        'tcell.InnerText = dt.Rows(0).Item("Equipment_Cost").ToString
            '        'trow.Cells.Add(tcell)
            '        'table.Rows.Add(trow)
            '        'test.Controls.Add(table)
            '    Next
            '    r += "</table>"
            '    test.InnerHtml = r
            'End If

        End If


    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
        End If
    End Sub

    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        Dim id_list As String
        Dim strSql As String

        id_list = CType(GridView1.Rows(e.RowIndex).Cells(3).FindControl("lblID"), Label).Text
        strSql = "delete from List_CAPEX where id_List = '" + id_list + "' "
        C.ExecuteNonQuery(strSql)


        Dim DT_List As DataTable
        Dim r As String = ""
        C.SetDropDownList(DropDownList1, "select * from CAPEX", "CAPEX_Name", "CAPEX_id")
        DT_List = C.GetDataTable("select * from List_CAPEX where CreateBy = '" + Session("uemail") + "' and Document_No = '" + xRequest_id + "' ")
        GridView1.DataSource = DT_List
        GridView1.DataBind()

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim strSql As String
        Dim i As Integer
        If GridView1.Rows.Count > 0 Then
            For i = 0 To GridView1.Rows.Count - 1
                strSql = "Update List_CAPEX Set CAPEX_Name='" + GridView1.Rows(i).Cells(0).Text + "', Asset_Type='" + CType(GridView1.Rows(i).Cells(1).FindControl("TextBox2"), TextBox).Text + "', Cost='" + CType(GridView1.Rows(i).Cells(2).FindControl("TextBox3"), TextBox).Text + "', CreateBy='" + Session("uemail") + "', CreateDate = getdate() where id_List='" + CType(GridView1.Rows(i).Cells(3).FindControl("lblID"), Label).Text + "' "
                C.ExecuteNonQuery(strSql)
            Next
        End If
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        Response.Redirect("edit_opex.aspx?request_id=" + Request.QueryString("request_id"))
    End Sub
End Class
