Imports System.Data
Imports Microsoft.VisualBasic
Partial Class Summary
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim CF As New Cls_RequestFlow

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strSql As String
        Dim DT_CAPEX As DataTable
        Dim DT_OPEX As DataTable
        Dim DT_Service As DataTable
        Dim DT_InternetBWCost As DataTable
        Dim DomesticCost As Double
        Dim InternationalCost As Double
        Dim Transit As Double
        Dim xDSL_FTTx As Double
        Dim r As String
        Dim total_capex As Double = 0
        Dim total_opex As Double = 0
        strSql = "select * from dbo.List_CAPEX where CreateBy='" + Session("uemail") + "' and Document_No is null "
        DT_CAPEX = C.GetDataTable(strSql)
        r = "<table style='width: 100%'>"
        For i As Integer = 0 To DT_CAPEX.Rows.Count - 1
            r += "<tr>"
            r += "<td>" + DT_CAPEX.Rows(i).Item("CAPEX_Name").ToString + "</td>"
            r += "<td>" + DT_CAPEX.Rows(i).Item("Asset_Type").ToString + "</td>"
            r += "<td>" + DT_CAPEX.Rows(i).Item("Cost").ToString + "</td>"
            r += "<td>บาท/เดือน</td>"
            r += "</tr>"
            total_capex += DT_CAPEX.Rows(i).Item("Cost")
        Next
        r += "<tr>"
        r += "<td>Total Investment</td>"
        r += "<td></td>"
        r += "<td><b>" + total_capex.ToString + "</b></td>"
        r += "<td></td>"
        r += "</tr>"
        r += "</table>"
        CAPEX.InnerHtml = r

        strSql = "select * from dbo.List_OPEX where CreateBy='" + Session("uemail") + "' and Document_No is null "
        DT_OPEX = C.GetDataTable(strSql)
        r = "<table style='width: 100%'>"
        For i As Integer = 0 To DT_OPEX.Rows.Count - 1
            r += "<tr>"
            r += "<td>" + DT_OPEX.Rows(i).Item("OPEX_Name").ToString + "</td>"
            r += "<td>" + DT_OPEX.Rows(i).Item("Cost").ToString + "</td>"
            r += "<td>บาท/เดือน</td>"
            r += "</tr>"
            total_opex += DT_OPEX.Rows(i).Item("Cost")
        Next
        r += "<tr>"
        r += "<td></td>"
        r += "<td><b>" + total_opex.ToString + "</b></td>"
        r += "<td></td>"
        r += "</tr>"
        r += "</table>"
        OPEX.InnerHtml = r

        strSql = "select * from dbo.List_Model where CreateBy='" + Session("uemail") + "' and Document_No is null "
        DT_Service = C.GetDataTable(strSql)
        If DT_Service.Rows.Count > 0 Then
            lblContract.Text = DT_Service.Rows(0).Item("Contract").ToString
            lblMonthly.Text = DT_Service.Rows(0).Item("Monthly").ToString
            lblOneTimePayment.Text = DT_Service.Rows(0).Item("OneTimePayment").ToString
            lblCAPEX.Text = total_capex.ToString

            ' Define money format.
            Dim MoneyFmt As String = "###,##0.00"
            ' Define percentage format.
            Dim PercentFmt As String = "#0.0"
            Dim values(lblContract.Text) As Double

            Dim Revenue As Double
            Dim MKTCost As Double
            Dim RevenueOneTime As Double
            Dim MKTCostOneTime As Double
            Dim CostOfInternet As Double
            Dim CostOfNetwork As Double
            Dim CostOfNOC As Double
            Dim Vas As Double
            Dim CashFlow As Double
            Dim CashFlowOneTime As Double

            DT_InternetBWCost = C.GetDataTable("select * from InternetBWCost where BWCost_Type='" + DT_Service.Rows(0).Item("Discount") + "' ")
            Revenue = DT_Service.Rows(0).Item("Monthly")
            RevenueOneTime = DT_Service.Rows(0).Item("Monthly") + DT_Service.Rows(0).Item("OneTimePayment")
            MKTCost = (Revenue * 3) / 100
            MKTCostOneTime = (RevenueOneTime * 3) / 100
            CostOfNetwork = ((DT_Service.Rows(0).Item("EthernetIPV") * (DT_InternetBWCost.Rows(0).Item("IPV_Discount") / 100)) + DT_Service.Rows(0).Item("EthernetINP")) * DT_InternetBWCost.Rows(0).Item("Network")
            CostOfNOC = (DT_Service.Rows(0).Item("TotalINLCurcuits") + DT_Service.Rows(0).Item("TotalIPVCurcuits") + DT_Service.Rows(0).Item("TotalINPCurcuits")) * DT_InternetBWCost.Rows(0).Item("NOC")
            Vas = C.GetDataTable("select SUM(Cost) 'VasCost' from dbo.List_OPEX where CreateBy='" + Session("uemail") + "' and Document_No is null ").Rows(0).Item("VasCost")

            'lblRevenue.Text = ((Revenue * DT_Service.Rows(0).Item("Contract")) + DT_Service.Rows(0).Item("OneTimePayment")).ToString
            'lblMKTCost.Text = (MKTCost * DT_Service.Rows(0).Item("Contract")).ToString
            lblRevenue.Text = (RevenueOneTime + (Revenue * (DT_Service.Rows(0).Item("Contract") - 1))).ToString
            lblMKTCost.Text = ((MKTCostOneTime) + (MKTCost * (DT_Service.Rows(0).Item("Contract") - 1))).ToString

            If DT_InternetBWCost.Rows.Count > 0 Then
                DomesticCost = (DT_Service.Rows(0).Item("Domestic") * DT_InternetBWCost.Rows(0).Item("Domestic")) * (DT_InternetBWCost.Rows(0).Item("Dom_Discount") / 100)
                InternationalCost = ((DT_Service.Rows(0).Item("International") - DT_Service.Rows(0).Item("Transit")) * DT_InternetBWCost.Rows(0).Item("All_International")) * (DT_InternetBWCost.Rows(0).Item("Inter_Discount") / 100)
                Transit = DT_Service.Rows(0).Item("Transit") * DT_InternetBWCost.Rows(0).Item("Transit")
                xDSL_FTTx = DT_Service.Rows(0).Item("ServicePrice")
                CostOfInternet = (DomesticCost + InternationalCost + Transit + xDSL_FTTx)
                lblCostOfInternet.Text = (CostOfInternet * DT_Service.Rows(0).Item("Contract")).ToString
                lblCostOfNetwork.Text = (CostOfNetwork * DT_Service.Rows(0).Item("Contract")).ToString
                lblCostOfNOC.Text = (CostOfNOC * DT_Service.Rows(0).Item("Contract")).ToString
            End If
            lblVas.Text = (Vas * DT_Service.Rows(0).Item("Contract")).ToString.ToString
            CashFlow = Revenue - MKTCost - CostOfInternet - CostOfNetwork - CostOfNOC - Vas
            CashFlowOneTime = RevenueOneTime - MKTCostOneTime - CostOfInternet - CostOfNetwork - CostOfNOC - Vas
            lblCashFlow.Text = (CashFlowOneTime + (CashFlow * (DT_Service.Rows(0).Item("Contract") - 1))).ToString
            'lblCashFlow.Text = (((DT_Service.Rows(0).Item("Monthly") + DT_Service.Rows(0).Item("OneTimePayment")) - (((DT_Service.Rows(0).Item("Monthly") + DT_Service.Rows(0).Item("OneTimePayment")) * 3) / 100) - CostOfInternet - CostOfNetwork - CostOfNOC - Vas) + ((DT_Service.Rows(0).Item("Monthly") * (DT_Service.Rows(0).Item("Contract") - 1)) - (((DT_Service.Rows(0).Item("Monthly") * (DT_Service.Rows(0).Item("Contract") - 1)) * 3) / 100) - ((CostOfInternet + CostOfNetwork + CostOfNOC + Vas) * (DT_Service.Rows(0).Item("Contract") - 1)))).ToString

            'CashFlow = DT_Service.Rows(0).Item("Monthly") - ((DT_Service.Rows(0).Item("Monthly") * 3) / 100) - CostOfInternet - CostOfNetwork - CostOfNOC - Vas

            If lblOneTimePayment.Text - lblCAPEX.Text > 0 Then
                lblPayBack.Text = "<1"
            Else
                lblPayBack.Text = Format(lblCAPEX.Text / CashFlow, "#0.0")
            End If

            For i As Integer = 0 To lblContract.Text - 1
                If i = 0 Then
                    values(i) = CashFlowOneTime - lblCAPEX.Text
                Else
                    values(i) = CashFlow
                End If
            Next

            Dim FixedRetRate As Double = 0.05
            ' Calculate net present value.
            Dim NetPVal As Double = NPV(FixedRetRate / 12, values)
            ' Display net present value.
            ' MsgBox("The net present value of these cash flows is " & Format(NetPVal, MoneyFmt) & ".")
            lblNPV.Text = Format(NetPVal, MoneyFmt)
            lblMargin.Text = Format((lblNPV.Text / lblRevenue.Text) * 100, PercentFmt).ToString & "%"

        End If

        If Not Page.IsPostBack Then
            Dim strRO As String
            Dim DTRO As DataTable
            Dim strCluster As String
            Dim DTCluster As DataTable

            strRO = "select distinct RO from Cluster where Status = '1' "
            C.SetDropDownList(ddlArea, strRO, "RO", "RO")

            strCluster = "select distinct Cluster, Cluster_email, Cluster_name from Cluster where Status = '1' and RO = '" + ddlArea.SelectedValue + "' "
            C.SetDropDownList(ddlCluster, strCluster, "Cluster", "Cluster")
            DTCluster = C.GetDataTable(strCluster)
            If DTCluster.Rows.Count > 0 Then
                lblPrepare.Text = "(" & DTCluster.Rows(0).Item("Cluster_name") & ")"
            End If

        End If

        '' Define money format.
        'Dim MoneyFmt As String = "###,##0.00"
        '' Define percentage format.
        'Dim PercentFmt As String = "#0.0"
        'Dim rr As Integer = 12
        'Dim values(rr) As Double
        '' Business start-up costs.
        ''values(0) = -70000
        ' '' Positive cash flows reflecting income for four successive years.
        ''values(1) = 22000
        ''values(2) = 25000
        ''values(3) = 28000
        ''values(4) = 31000

        'values(0) = -77994.67
        '' Positive cash flows reflecting income for four successive years.
        'values(1) = 12683.33
        'values(2) = 12683.33
        'values(3) = 12683.33
        'values(4) = 12683.33
        'values(5) = 12683.33
        'values(6) = 12683.33
        'values(7) = 12683.33
        'values(8) = 12683.33
        'values(9) = 12683.33
        'values(10) = 12683.33
        'values(11) = 12683.33

        '' Use the NPV function to calculate the net present value.
        '' Set fixed internal rate.
        'Dim FixedRetRate As Double = 0.05
        '' Calculate net present value.
        'Dim NetPVal As Double = NPV(FixedRetRate / 12, values)
        '' Display net present value.
        '' MsgBox("The net present value of these cash flows is " & Format(NetPVal, MoneyFmt) & ".")
        'lblNPV.Text = Format(NetPVal, MoneyFmt)
        'lblMargin.Text = Format((lblNPV.Text / lblRevenue.Text) * 100, PercentFmt).ToString & "%"
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim DT_CAPEX As New DataTable
        Dim DT_OPEX As New DataTable
        Dim DT_Model As New DataTable
        Dim strSql As String
        Dim DT As New DataTable
        Dim TypeService As String

        strSql = "select * from List_CAPEX where CreateBy = '" + Session("uemail") + "' and Document_No is NULL "
        DT_CAPEX = C.GetDataTable(strSql)
        strSql = "select * from List_OPEX where CreateBy = '" + Session("uemail") + "' and Document_No is NULL "
        DT_OPEX = C.GetDataTable(strSql)
        strSql = "select * from List_Model where CreateBy = '" + Session("uemail") + "' and Document_No is NULL "
        DT_Model = C.GetDataTable(strSql)

        If DT_CAPEX.Rows.Count <= 0 Then
            ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณากรอกข้อมูล CAPEX');focus();window.location.href='Default.aspx';", True)
        ElseIf DT_OPEX.Rows.Count <= 0 Then
            ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณากรอกข้อมูล OPEX');focus();window.location.href='add_opex.aspx';", True)
        ElseIf DT_Model.Rows.Count <= 0 Then
            ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณากรอกข้อมูล Service');focus();window.location.href='add_service.aspx';", True)
        Else
            If RadioButton2.Checked = True Then
                TypeService = RadioButton2.Text
            ElseIf RadioButton3.Checked = True Then
                TypeService = RadioButton3.Text
            Else
                TypeService = RadioButton1.Text
            End If
            strSql = "select convert(varchar(5),right(isnull(max(Document_No),'FES000000-00000'),5) + 1) 'Max_Document',  left(CONVERT(varchar(10),getdate(),111),4) + substring(CONVERT(varchar(10),getdate(),111),6,2) 'YearMonth' " + vbCr
            strSql += "from FeasibilityDocument " + vbCr
            strSql += "where substring(Document_No,4,6) = left(CONVERT(varchar(10),getdate(),111),4) + substring(CONVERT(varchar(10),getdate(),111),6,2) "
            DT = C.GetDataTable(strSql)
            'Dim a As Integer = 200
            'Dim doc As String = CInt(DT.Rows(0).Item("Max_Document")).ToString("00000")
            Dim doc_no As String = "FES" + DT.Rows(0).Item("YearMonth").ToString + "-" + CInt(DT.Rows(0).Item("Max_Document")).ToString("00000")
            strSql = "insert into FeasibilityDocument (Document_No,Document_Date,Service_Date,Area,Cluster,Customer_Name,Location_Name,Type_Service,Detail_Service,CreateBy,CreateDate,Status) values ('" + doc_no + "','" + C.CDateText(txtDocumentDate.Value) + "','" + C.CDateText(txtServiceDate.Value) + "','" + ddlArea.SelectedValue + "','" + ddlCluster.SelectedValue + "','" + txtCustomerName.Text + "','" + txtLocationName.Text + "','" + TypeService + "','" + txtDetailService.Text + "','" + Session("uemail") + "',getdate(),'1') "
            C.ExecuteNonQuery(strSql)
            strSql = "Update dbo.List_CAPEX set Document_No='" + doc_no + "', UpdateBy='weraphon.r', UpdateDate = getdate() where CreateBy = '" + Session("uemail") + "' and Document_No is NULL " + vbCr
            strSql += "Update dbo.List_OPEX set Document_No='" + doc_no + "', UpdateBy='weraphon.r', UpdateDate = getdate() where CreateBy = '" + Session("uemail") + "' and Document_No is NULL " + vbCr
            strSql += "Update dbo.List_Model set Document_No='" + doc_no + "', UpdateBy='weraphon.r', UpdateDate = getdate() where CreateBy = '" + Session("uemail") + "' and Document_No is NULL "
            C.ExecuteNonQuery(strSql)

            Dim vSqlIn As String = ""
            Dim flow_id As String = "555"
            vSqlIn += "INSERT INTO request_flow ( "
            vSqlIn += "request_id, flow_id, depart_id, flow_step, next_step, "
            vSqlIn += "send_uemail, uemail, approval, require_remark, require_file, add_next, back_step) "
            vSqlIn += "select '" + doc_no + "', fp.flow_id, fp.depart_id, fp.flow_step, fp.next_step, "
            vSqlIn += "dp.uemail, dp.uemail, fp.approval, fp.require_remark, fp.require_file, dp.add_next, fp.back_step "
            vSqlIn += "from flow_pattern fp "
            vSqlIn += "join ( "
            vSqlIn += " SELECT "
            vSqlIn += "      dm.depart_id "
            vSqlIn += "    , dm.depart_name "
            vSqlIn += "    , dm.add_next "
            vSqlIn += "    , uemail = STUFF(( "
            vSqlIn += "          SELECT ';' + du.uemail "
            vSqlIn += "          FROM depart_user du "
            vSqlIn += "          WHERE dm.depart_id = du.depart_id "
            vSqlIn += "          and start_date <= getdate() "
            vSqlIn += "          and (expired_date is null or expired_date >= getdate()) "
            vSqlIn += "          FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
            vSqlIn += "     FROM department dm "
            vSqlIn += ") dp on dp.depart_id = fp.depart_id "
            vSqlIn += "where flow_id = " + flow_id + " "
            vSqlIn += "order by fp.flow_step "
            C.ExecuteNonQuery(vSqlIn)

            Dim PreparedMail As String = "weraphon.r"
            vSqlIn = "Update request_flow set send_uemail = '" + PreparedMail + "', uemail = '" + PreparedMail + "' where request_id = '" + doc_no + "' and depart_id='2' " + vbCr
            vSqlIn += "Update request_flow set send_uemail = '" + Session("uemail") + "', uemail = '" + Session("uemail") + "' where request_id = '" + doc_no + "' and depart_id='0' " + vbCr
            vSqlIn += "Update FeasibilityDocument set uemail_verify = '" + PreparedMail + "' where Document_No = '" + doc_no + "' "
            C.ExecuteNonQuery(vSqlIn)

            CF.InsertRequestFile("", "", doc_no, "")
            ClientScript.RegisterStartupScript(Page.GetType, "", "alert('Ref No: " + doc_no + "');", True)
        End If


    End Sub

    Protected Sub ddlArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlArea.SelectedIndexChanged
        Dim strcluster As String
        Dim DTCluster As DataTable
        strcluster = "select distinct Cluster, Cluster_email, Cluster_name from Cluster where Status = '1' and RO = '" + ddlArea.SelectedValue + "' "
        C.SetDropDownList(ddlCluster, strcluster, "Cluster", "Cluster")
        DTCluster = C.GetDataTable(strcluster)
        If DTCluster.Rows.Count > 0 Then
            lblPrepare.Text = "(" & DTCluster.Rows(0).Item("Cluster_name") & ")"
        End If
    End Sub
End Class
