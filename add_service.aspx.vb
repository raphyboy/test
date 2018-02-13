Imports System.Data
Partial Class add_service
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim DT As DataTable
            Dim DT_List As DataTable
            Dim strSql As String

            C.SetDropDownList(ddlDiscount, "select distinct BWCost_Type from InternetBWCost order by 1 desc", "BWCost_Type", "BWCost_Type")

            strSql = "select * from dbo.List_Model where CreateBy='" + Session("uemail") + "' and Document_No is null "
            DT_List = C.GetDataTable(strSql)
            If DT_List.Rows.Count > 0 Then
                txtContract.Text = DT_List.Rows(0).Item("Contract").ToString
                txtDom.Text = DT_List.Rows(0).Item("Domestic").ToString
                txtInter.Text = DT_List.Rows(0).Item("International").ToString
                txtTransit.Text = DT_List.Rows(0).Item("Transit").ToString
                txtTotalINL.Text = DT_List.Rows(0).Item("TotalINLCurcuits").ToString
                txtEthernetIPV.Text = DT_List.Rows(0).Item("EthernetIPV").ToString
                txtTotalIPV.Text = DT_List.Rows(0).Item("TotalIPVCurcuits").ToString
                txtEthernetINP.Text = DT_List.Rows(0).Item("EthernetINP").ToString
                txtPriceINP.Text = DT_List.Rows(0).Item("ServicePrice").ToString
                txtTotalINP.Text = DT_List.Rows(0).Item("TotalINPCurcuits").ToString
                txtMonthly.Text = DT_List.Rows(0).Item("Monthly").ToString
                lblTotalYealy.Text = (CInt(txtMonthly.Text) * 12).ToString
                txtOneTime.Text = DT_List.Rows(0).Item("OneTimePayment").ToString

                ddlDiscount.SelectedValue = DT_List.Rows(0).Item("Discount")
            Else
                txtContract.Text = "12"
                txtDom.Text = "0"
                txtInter.Text = "0"
                txtTransit.Text = "4"
                txtTotalINL.Text = "0"
                txtEthernetIPV.Text = "0"
                txtTotalIPV.Text = "0"
                txtEthernetINP.Text = "0"
                txtPriceINP.Text = "0"
                txtTotalINP.Text = "0"
                txtMonthly.Text = "0"
                lblTotalYealy.Text = (CInt(txtMonthly.Text) * 12).ToString
                txtOneTime.Text = "0"
            End If

            strSql = "select * from InternetBWCost where BWCost_Type = '" + ddlDiscount.SelectedValue + "' order by 1"
            DT = C.GetDataTable(strSql)
            If DT.Rows.Count > 0 Then
                lblDom.Text = DT.Rows(0).Item("Dom_Discount").ToString
                lblInterUtilization.Text = DT.Rows(0).Item("Inter_Discount").ToString
                lblIPVUtilization.Text = DT.Rows(0).Item("IPV_Discount").ToString
            End If
        End If
    End Sub

    Protected Sub ddlDiscount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDiscount.SelectedIndexChanged
        Dim DT As DataTable
        Dim strSql As String
        strSql = "select * from InternetBWCost where BWCost_Type = '" + ddlDiscount.SelectedValue + "' order by 1"
        DT = C.GetDataTable(strSql)
        If DT.Rows.Count > 0 Then
            lblDom.Text = DT.Rows(0).Item("Dom_Discount").ToString
            lblInterUtilization.Text = DT.Rows(0).Item("Inter_Discount").ToString
            lblIPVUtilization.Text = DT.Rows(0).Item("IPV_Discount").ToString
        End If
    End Sub

    Protected Sub txtMonthly_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMonthly.TextChanged
        If IsNumeric(txtMonthly.Text) Then
            lblTotalYealy.Text = (CDbl(txtMonthly.Text) * 12).ToString
            lblMonthly.Text = txtMonthly.Text
        Else
            'txtMonthly.Text = "0"
            lblTotalYealy.Text = "0"
            lblMonthly.Text = "0"
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ Monthly (รายได้)เป็นตัวเลขเท่านั้น!');", True)
            txtMonthly.Focus()
        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim DT As DataTable
        Dim DT_List As DataTable
        Dim strSql As String
        Dim r As String = ""
        If IsNumeric(txtContract.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุอายุสัญญา Contract เป็นตัวเลขเท่านั้น!');", True)
            txtContract.Focus()
        ElseIf CInt(txtContract.Text) < 0 Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุอายุสัญญา Contract ให้มากกว่า 0');", True)
            txtContract.Focus()
        ElseIf IsNumeric(txtDom.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ Domestic เป็นตัวเลขเท่านั้น!');", True)
            txtDom.Focus()
        ElseIf IsNumeric(txtInter.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ International เป็นตัวเลขเท่านั้น!');", True)
            txtInter.Focus()
        ElseIf IsNumeric(txtTransit.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ Transit เป็นตัวเลขเท่านั้น!');", True)
            txtTransit.Focus()
        ElseIf IsNumeric(txtTotalINL.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ Total INL Curcuits เป็นตัวเลขเท่านั้น!');", True)
            txtTotalINL.Focus()
        ElseIf IsNumeric(txtEthernetIPV.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ Ethernet เป็นตัวเลขเท่านั้น!');", True)
            txtEthernetIPV.Focus()
        ElseIf IsNumeric(txtTotalIPV.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ Total IPV Curcuits เป็นตัวเลขเท่านั้น!');", True)
            txtTotalIPV.Focus()
        ElseIf IsNumeric(txtEthernetINP.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ Ethernet เป็นตัวเลขเท่านั้น!');", True)
            txtEthernetINP.Focus()
        ElseIf IsNumeric(txtTotalINP.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ Total INP Curcuits เป็นตัวเลขเท่านั้น!');", True)
            txtTotalINP.Focus()
        ElseIf IsNumeric(txtMonthly.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ Monthly (รายได้) เป็นตัวเลขเท่านั้น!');", True)
            txtMonthly.Focus()
        ElseIf IsNumeric(txtOneTime.Text) = False Then
            ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุ One-Time Payment เป็นตัวเลขเท่านั้น!');", True)
            txtOneTime.Focus()
        Else
            strSql = "select * from List_Model where CreateBy='" + Session("uemail") + "' and Document_No is null "
            DT = C.GetDataTable(strSql)
            If DT.Rows.Count > 0 Then
                strSql = "Update List_Model set Contract='" + txtContract.Text + "', Discount='" + ddlDiscount.SelectedValue + "', Domestic = '" + txtDom.Text + "', DomUtilization = '" + lblDom.Text + "', International = '" + txtInter.Text + "', InterUtilization = '" + lblInterUtilization.Text + "', Transit = '" + txtTransit.Text + "', TotalINLCurcuits = '" + txtTotalINL.Text + "', EthernetIPV = '" + txtEthernetIPV.Text + "', IPVUtilization = '" + lblIPVUtilization.Text + "', TotalIPVCurcuits = '" + txtTotalIPV.Text + "', EthernetINP = '" + txtEthernetINP.Text + "', ServicePrice = '" + txtPriceINP.Text + "', TotalINPCurcuits = '" + txtTotalINP.Text + "', Monthly = '" + txtMonthly.Text + "', OneTimePayment = '" + txtOneTime.Text + "' where CreateBy='" + Session("uemail") + "' and Document_No is null  "
                C.ExecuteNonQuery(strSql)
            Else
                strSql = "insert into List_Model(Contract,Discount,Domestic,DomUtilization,International,InterUtilization,Transit,TotalINLCurcuits,EthernetIPV,IPVUtilization,TotalIPVCurcuits,EthernetINP,ServicePrice,TotalINPCurcuits,Monthly,OneTimePayment,CreateBy,CreateDate) values('" + txtContract.Text + "','" + ddlDiscount.SelectedValue + "','" + txtDom.Text + "','" + lblDom.Text + "','" + txtInter.Text + "','" + lblInterUtilization.Text + "','" + txtTransit.Text + "','" + txtTotalINL.Text + "','" + txtEthernetIPV.Text + "','" + lblIPVUtilization.Text + "','" + txtTotalIPV.Text + "','" + txtEthernetINP.Text + "','" + txtPriceINP.Text + "','" + txtTotalINP.Text + "','" + txtMonthly.Text + "','" + txtOneTime.Text + "','" + Session("uemail") + "',getdate()) "
                C.ExecuteNonQuery(strSql)
            End If
        End If
        'If IsNumeric(TextBox4.Text) = True Then
        '    DT = C.GetDataTable("select * from OPEX where OPEX_id='" + DropDownList1.SelectedValue + "'")

        '    If DT.Rows.Count > 0 Then
        '        strSql = "insert into List_OPEX (OPEX_Name, Number, Cost, CreateBy, CreateDate) values('" + DT.Rows(0).Item("OPEX_Name").ToString + "','" + TextBox4.Text + "','" + (CInt(DT.Rows(0).Item("OPEX_Cost")) * CInt(TextBox4.Text)).ToString + "','weraphon.r',getdate()) "
        '        C.ExecuteNonQuery(strSql)
        '        DT_List = C.GetDataTable("select * from List_OPEX where CreateBy = 'weraphon.r'")
        '        GridView1.DataSource = DT_List
        '        GridView1.DataBind()

        '    End If
        'Else
        '    ClientScript.RegisterStartupScript(Page.GetType, "Alert", "alert('ระบุจำนวนเป็นตัวเลขเท่านั้น!');", True)
        '    TextBox4.Focus()
        'End If
        Response.Redirect("Summary.aspx")
    End Sub
End Class
