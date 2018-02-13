Imports System.Data
Imports Microsoft.VisualBasic
Partial Class edit_Summary
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim CF As New Cls_RequestFlow
    Dim xRequest_id
    Dim request_status

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        xRequest_id = Request.QueryString("request_id")
        menu_capex.HRef = "edit_capex.aspx?request_id=" + xRequest_id
        menu_opex.HRef = "edit_opex.aspx?request_id=" + xRequest_id
        menu_service.HRef = "edit_service.aspx?request_id=" + xRequest_id

        Dim dt_check As DataTable
        dt_check = C.GetDataTable("select * from dbo.FeasibilityDocument where (request_status = 55 or request_status = 110) and Document_No = '" + xRequest_id + "'")
        If dt_check.Rows.Count > 0 Then
            If ((dt_check.Rows(0).Item("request_status").ToString = "55" And Session("Login_permission") = "inspector") Or (dt_check.Rows(0).Item("request_status").ToString = "110" And dt_check.Rows(0).Item("CreateBy").ToString = Session("uemail"))) Then
                request_status = dt_check.Rows(0).Item("request_status").ToString
                Dim DT_Document As DataTable
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
                strSql = "select * from dbo.List_CAPEX where CreateBy='" + Session("uemail") + "' and Document_No = '" + xRequest_id + "' "
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

                strSql = "select * from dbo.List_OPEX where CreateBy='" + Session("uemail") + "' and Document_No = '" + xRequest_id + "' "
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

                strSql = "select * from dbo.List_Model where CreateBy='" + Session("uemail") + "' and Document_No = '" + xRequest_id + "' "
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
                    Vas = C.GetDataTable("select SUM(Cost) 'VasCost' from dbo.List_OPEX where CreateBy='" + Session("uemail") + "' and Document_No = '" + xRequest_id + "' ").Rows(0).Item("VasCost")

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
                    strSql = "select Document_No, convert(varchar(10),Document_Date,103) 'Document_Date', convert(varchar(10),Service_Date,103) 'Service_Date' " + vbCr
                    strSql += ", Area, Cluster, Customer_Name, Location_Name, Type_Service, Detail_Service  " + vbCr
                    strSql += "from dbo.FeasibilityDocument where Document_No = '" + xRequest_id + "' and Status = '1' "
                    DT_Document = C.GetDataTable(strSql)
                    If DT_Document.Rows.Count > 0 Then
                        lblDocumentNo.Text = DT_Document.Rows(0).Item("Document_No")
                        txtDocumentDate.Text = DT_Document.Rows(0).Item("Document_Date")
                        txtArea.Text = DT_Document.Rows(0).Item("Area")
                        txtLocationName.Text = DT_Document.Rows(0).Item("Location_Name")
                        txtServiceDate.Text = DT_Document.Rows(0).Item("Service_Date")
                        txtCluster.Text = DT_Document.Rows(0).Item("Cluster")
                        txtCustomerName.Text = DT_Document.Rows(0).Item("Customer_Name")
                        'RadioButton4.Checked = False
                        If DT_Document.Rows(0).Item("Type_Service") = "Re-New Service" Then
                            RadioButton2.Checked = True
                        ElseIf DT_Document.Rows(0).Item("Type_Service") = "Maintenance" Then
                            RadioButton3.Checked = True
                        Else
                            RadioButton1.Checked = True
                        End If
                        txtDetailService.Text = DT_Document.Rows(0).Item("Detail_Service")
                    End If
                End If
            Else
                btnSave.Visible = False
            End If
        Else
            btnSave.Visible = False
        End If

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strSql As String
        Dim DT As New DataTable
        Dim TypeService As String
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
        'strSql = "insert into FeasbilityDocument (Document_No,Document_Date,Service_Date,Area,Cluster,Customer_Name,Location_Name,Type_Service,Detail_Service,CreateBy,CreateDate,Status) values ('" + doc_no + "','" + C.CDateText(txtDocumentDate.Text) + "','" + C.CDateText(txtServiceDate.Text) + "','" + txtArea.Text + "','" + txtCluster.Text + "','" + txtCustomerName.Text + "','" + txtLocationName.Text + "','" + TypeService + "','" + txtDetailService.Text + "','" + Session("uemail") + "',getdate(),'1') "
        strSql = "Update FeasibilityDocument set Document_Date = '" + C.CDateText(txtDocumentDate.Text) + "', Service_Date = '" + C.CDateText(txtServiceDate.Text) + "', Area = '" + txtArea.Text + "', Cluster = '" + txtCluster.Text + "', Customer_Name = '" + txtCustomerName.Text + "', Location_Name = '" + txtLocationName.Text + "', Type_Service = '" + TypeService + "', Detail_Service = '" + txtDetailService.Text + "', UpdateBy = '" + Session("uemail") + "', UpdateDate = getdate(), Status = '1'  "
        If request_status = "110" Then
            strSql += ", request_Status='0' " + vbCr
        End If
        strSql += "where Document_No = '" + xRequest_id + "' "
        C.ExecuteNonQuery(strSql)
        'strSql = "Update dbo.List_CAPEX set Document_No='" + doc_no + "', UpdateBy='weraphon.r', UpdateDate = getdate() where CreateBy = '" + Session("uemail") + "' and Document_No is NULL " + vbCr
        'strSql += "Update dbo.List_OPEX set Document_No='" + doc_no + "', UpdateBy='weraphon.r', UpdateDate = getdate() where CreateBy = '" + Session("uemail") + "' and Document_No is NULL " + vbCr
        'strSql += "Update dbo.List_Model set Document_No='" + doc_no + "', UpdateBy='weraphon.r', UpdateDate = getdate() where CreateBy = '" + Session("uemail") + "' and Document_No is NULL "
        'C.ExecuteNonQuery(strSql)

        'Dim vSqlIn As String = ""
        'Dim flow_id As String = "555"
        'vSqlIn += "INSERT INTO request_flow ( "
        'vSqlIn += "request_id, flow_id, depart_id, flow_step, next_step, "
        'vSqlIn += "send_uemail, uemail, approval, require_remark, require_file, add_next) "
        'vSqlIn += "select '" + doc_no + "', fp.flow_id, fp.depart_id, fp.flow_step, fp.next_step, "
        'vSqlIn += "dp.uemail, dp.uemail, fp.approval, fp.require_remark, fp.require_file, dp.add_next "
        'vSqlIn += "from flow_pattern fp "
        'vSqlIn += "join ( "
        'vSqlIn += " SELECT "
        'vSqlIn += "      dm.depart_id "
        'vSqlIn += "    , dm.depart_name "
        'vSqlIn += "    , dm.add_next "
        'vSqlIn += "    , uemail = STUFF(( "
        'vSqlIn += "          SELECT ';' + du.uemail "
        'vSqlIn += "          FROM depart_user du "
        'vSqlIn += "          WHERE dm.depart_id = du.depart_id "
        'vSqlIn += "          and start_date <= getdate() "
        'vSqlIn += "          and (expired_date is null or expired_date >= getdate()) "
        'vSqlIn += "          FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        'vSqlIn += "     FROM department dm "
        'vSqlIn += ") dp on dp.depart_id = fp.depart_id "
        'vSqlIn += "where flow_id = " + flow_id + " "
        'vSqlIn += "order by fp.flow_step "
        'C.ExecuteNonQuery(vSqlIn)

        'Dim PreparedMail As String = "sophida.t"
        'vSqlIn = "Update request_flow set send_uemail = '" + PreparedMail + "', uemail = '" + PreparedMail + "' where request_id = '" + doc_no + "' and depart_id='2'"
        'C.ExecuteNonQuery(vSqlIn)

        'CF.InsertRequestFile("", "", doc_no, "")
        If request_status = "110" Then
            CF.UpdateRequest(xRequest_id, "", "", "", "", Session("uemail"), Session("uemail"), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
        End If
        ClientScript.RegisterStartupScript(Page.GetType, "", "alert('Ref No: " + doc_no + "');", True)
    End Sub

    Sub Flow_Submit(ByVal Source As Object, ByVal E As EventArgs)
        Dim strSql As String
        strSql = ""

        Dim flow_file As String = "" 'rUpFile("flow_file", Request.QueryString("request_id") + "_F")
        CF.SaveFlow(hide_uemail.Value, hide_flow_no.Value, hide_flow_sub.Value, hide_next_step.Value, hide_back_step.Value, hide_department.Value, hide_flow_status.Value, hide_flow_remark.Value, flow_file)
    End Sub

    Protected Sub Add_Next(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_add_next_hidden.ServerClick

    End Sub
End Class
