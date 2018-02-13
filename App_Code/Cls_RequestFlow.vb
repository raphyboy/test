Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Net.IPAddress
Imports System.Threading

Public Class Cls_RequestFlow
    Inherits System.Web.UI.Page
    Dim DB105 As New Cls_Data
    Dim CP As New Cls_Panu

    Public Shared Dim global_path As String = "upload/"
    Public Shared Dim file_wait_request_end = "<span class='txt-gray'>รอปิดคำขอ</span>"
    Public Shared Dim file_dont_request_permiss = "<span class='txt-red'>เฉพาะผู้เกี่ยวข้อง</span>"

#Region "loadPage"
    Public Function rSqlDDArea() As String
        Dim vSql As String 
        vSql = "select f03 ro_value, 'RO' + f03 ro_title from m00030 group by f03 order by f03"

        Return vSql
    End Function
#End Region


#Region "loadInsertPage"
    Public Function rLoadSubject(ByVal subject_id As String) As DataTable
        Dim vSql As String 
        vSql = "select subject.project_id, flow_id, project_prefix+subject_prefix prefix_id, "
        vSql += "project_name, subject_name, subject_desc "
        vSql += "from subject "
        vSql += "join project on project.project_id = subject.project_id "
        vSql += "where subject_id = " & subject_id

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rSqlDDSubject(ByVal project_id As String) As String
        Dim vSql As String 
        vSql = "select subject_id, subject_name, subject_url  "
        vSql += "from subject "
        vSql += "where disable = 0 "
        vSql += "and project_id = " & project_id & " "
        vSql += "order by subject_prefix "

        Return vSql
    End Function

    Public Function rSqlDDTitle(ByVal subject_id As String) As String
        Dim vSql As String 
        vSql = "select request_title_id, request_title "
        vSql += "from request_title "
        vSql += "where disable = 0 "
        vSql += "and subject_id = " + subject_id + " "
        vSql += "order by request_title "

        Return vSql
    End Function
#End Region


#Region "loadUpdatePage"
    Public Sub checkPage(ByVal vRequest_id As String, ByVal pageUrl As String)
        Dim vSql As String 
        vSql = "select subject_url from request "
        vSql += "join subject on request.subject_id = subject.subject_id "
        vSql += "where request.request_id = '" + vRequest_id + "'"

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() = 0 Then
            CP.kickDefault("norequest")
        Else If vDT.Rows(0).Item("subject_url") <> pageUrl Then
            'Dim page As Page = HttpContext.Current.Handler
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('Page Failed!!'); window.location = 'default.aspx';", True)
            CP.kickDefault("pagefailed")
        End If
    End Sub

    Public Function rSqlLoadDetailRedebt(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select request.subject_id, subject_name, project_name, "
        vSql += "request_id, request_title_id, request_title, "
        vSql += "status_name, request_status, request_step, request_remark, request_file, "
        vSql += "account_number, account_name, account_number_to, account_name_to, "
        vSql += "coalesce(convert(varchar(10), dx01, 103), '') dx01, " 'dx01=ันที่ชำระผิด
        vSql += "coalesce(convert(varchar(10), dx02, 103), '') dx02, " 'dx02=วันที่ยกเลิก
        vSql += "coalesce(convert(varchar(10), dx03, 103), '') dx03, " 'dx03=วันที่ลูกค้าขอลดหนี้
        vSql += "doc_number, amount, bcs_number, redebt_number, area_ro, "
        vSql += "uemail_verify, uemail_approve, uemail_cc1, uemail_cc2, request.create_by, "
        vSql += "mx01, mx02, mx03, " 'mx01=คำนวณจาก, mx02=ลูกค้าต้องการคืนเงินเป็น, mx03=ใบเสร็จออกจากช่องทาง
        vSql += "request.redebt_cause_id, isnull(redebt_cause_title,'null') redebt_cause_title "
        vSql += "from request "

        vSql += "left join subject "
        vSql += "on subject.subject_id = request.subject_id "
        vSql += "left join project "
        vSql += "on project.project_id = subject.project_id "
        vSql += "left join request_status rs "
        vSql += "on rs.status_id = request.request_status "

        vSql += "left join redebt_cause_title rt "
        vSql += "on rt.redebt_cause_id = request.redebt_cause_id "

        vSql += "where request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rLoadFlowDT(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select rf.no flow_no, flow_step, flow_sub_step, next_step, back_step, "
        vSql += "CASE WHEN send_uemail <> '' THEN send_uemail + ';' END send_uemail, "
        vSql += "CASE WHEN uemail <> '' THEN uemail + ';' END uemail, "
        vSql += "rf.depart_id, depart_name, flow_status, status_name, flow_complete, "
        vSql += "rf.update_date, rf.update_by, flow_remark, flow_file, "
        vSql += "rf.approval, require_remark, require_file, flow_sub, rf.add_next  "
        vSql += "from ( "

        vSql += "   select no, flow_id, depart_id, flow_step, 0 flow_sub_step, next_step, back_step, "
        vSql += "   send_uemail, uemail, approval, flow_remark, flow_file, flow_status, flow_complete, "
        vSql += "   update_by, update_date, require_remark, require_file, '' flow_sub, add_next "
        vSql += "   from request_flow "
        vSql += "   where request_id = '" + vRequest_id + "' "
        vSql += "   and disable = '0' "
        vSql += "   union "
        vSql += "   select no, flow_id, depart_id, flow_step, flow_sub_step, next_step, back_step, "
        vSql += "   send_uemail, uemail, approval, flow_remark, flow_file, flow_status, flow_complete, "
        vSql += "   update_by, update_date, 0 require_remark, 0 require_file, '_sub' flow_sub, add_next "
        vSql += "   from request_flow_sub "
        vSql += "   where request_id = '" + vRequest_id + "' "
        vSql += "   and disable = '0' "

        vSql += ") rf "

        vSql += "left join request_status rs "
        vSql += "on rs.status_id = rf.flow_status "

        vSql += "left join department dm "
        vSql += "on dm.depart_id = rf.depart_id "

        vSql += "order by flow_step, next_step, flow_sub_step "

        Return DB105.GetDataTable(vSql)
    End Function

    'Public Function rLoadFlowBody(ByVal vRequest_id As String, ByVal vRequest_status As String, ByVal vRequest_step As String, ByVal hide_uemail_create As String, Optional ByVal vReqeust_permiss As Integer = 0) As String
    '    Dim vDT As New DataTable
    '    vDT = rLoadFlowDT(vRequest_id)

    '    Dim vTbody As String = ""
    '    Dim vShow_form As Integer = 0
    '    Dim vStep_Current As Integer = 0

    '    For i As Integer = 0 To vDT.Rows().Count() - 1
    '        Dim flow_step As String = vDT.Rows(i).Item("flow_step")

    '        If vDT.Rows(i).Item("flow_sub_step") <> 0 Then
    '            flow_step += "." & vDT.Rows(i).Item("flow_sub_step")
    '        End If

    '        vTbody += "<tr class='replace-class " & i & "'>"
    '        vTbody += "<td>" & (i+1) & "</td>"
    '        vTbody += "<td>" & flow_step & "</td>"
    '        vTbody += "<td>" & vDT.Rows(i).Item("next_step") & "</td>"
    '        vTbody += "<td>" + vDT.Rows(i).Item("depart_name") + "</td>"

    '        vTbody += "<td>" 
    '        vTbody += rSplit_uemail(vDT.Rows(i).Item("send_uemail"), vDT.Rows(i).Item("uemail"))
    '        vTbody += "</td>"

    '        Dim vLabel As String = "default"
    '        Dim vFile As String = ""

    '        If vDT.Rows(i).Item("flow_complete") = 1 Then
    '            vLabel = "complete"
    '            vFile = "-"

    '            vDT.Rows(i).Item("flow_remark") = CP.rNullHyphen(vDT.Rows(i).Item("flow_remark"))

    '            If vDT.Rows(i).Item("flow_file").Trim() <> "" Then
    '                If vReqeust_permiss = 0 Then
    '                    vFile = file_dont_request_permiss
    '                Else If Session("uemail") <> hide_uemail_create _
    '                Or (Session("uemail") = hide_uemail_create And (vRequest_status = 100 Or vDT.Rows(i).Item("flow_status") = 30 Or vDT.Rows(i).Item("flow_status") = 30))  Then
    '                    vFile = "<a href='" + global_path + vDT.Rows(i).Item("flow_file") + "' target='_blank'>เปิดไฟล์..</a>"
    '                Else
    '                    vFile = file_wait_request_end
    '                End If
    '            End If
    '        End If

    '        vTbody += "<td><span class='flow-sts label label-" + vLabel + "'>" + vDT.Rows(i).Item("status_name") + "</span></td>"
    '        'vTbody += "<td>" + Left(vDT.Rows(i).Item("update_date").ToString(), 16) + "</td>"
    '        vTbody += "<td>" + vDT.Rows(i).Item("update_date") + "</td>"
    '        vTbody += "<td>" + vDT.Rows(i).Item("update_by") + "</td>"

    '        If vShow_form = 0 And vStep_Current = 0 _
    '        And vRequest_status <> 55 And vRequest_status <> 100 And vRequest_status <> 110 _
    '        And vDT.Rows(i).Item("flow_complete") = 0 _
    '        And (vDT.Rows(i).Item("flow_step") = vRequest_step _
    '        Or (vDT.Rows(i).Item("flow_step") <= vRequest_step  And vDT.Rows(i).Item("next_step") = "-" )) _
    '        And vDT.Rows(i).Item("next_step") <> "-" _
    '        And vDT.Rows(i).Item("uemail") Like "*" + Session("Uemail") + ";*" Then

    '            vTbody += "<td colspan='2'>"

    '            If vDT.Rows(i).Item("add_next") = 1 Then
    '                Dim vSql3 As String
    '                vSql3 = "select depart_id, depart_name "
    '                vSql3 += "from department "
    '                vSql3 += "where disable = 0 and depart_id > 1 "

    '                Dim vDT3 As New DataTable
    '                vDT3 = DB105.GetDataTable(vSql3)

    '                vTbody += "<div class='panel panel-danger'>"
    '                vTbody += "    <div class='panel-heading panel-fonting'>แทรกลำดับถัดไป (Add Next) </div>"
    '                vTbody += "    <div class='panel-body'>"
    '                vTbody += "        <div class='form-horizontal'>"
    '                vTbody += "            <div class='form-group'>"
    '                vTbody += "            <label class='col-sm-12 txt-red'>- หากต้องการขอข้อมูลเพิ่ม กรุณาขอข้อมูลเพิ่มให้เสร็จสิ้น ก่อนแทรกลำดับถัดไป</label>"
    '                vTbody += "            <label class='col-sm-12 txt-red'>- หากต้องการแทรกลำดับถัดไป กรุณาแทรกลำดับถัดไป ก่อนทำการอนุมัติ</label>"
    '                vTbody += "            </div>"
    '                vTbody += "            <div class='form-group required-n'>"
    '                vTbody += "                <label class='col-sm-2 control-label'>ส่วนงาน</label>"
    '                vTbody += "                <div class='col-sm-10'>"
    '                vTbody += "                    <select id='sel_depart_id' class='form-control input-sm' >"
    '                vTbody += "                         <option value=''>เลือกส่วนงาน</option>"

    '                For i3 As Integer = 0 To vDT3.Rows().Count() - 1
    '                    vTbody += "<option value='" & vDT3.Rows(i3).Item("depart_id") & "'>" & vDT3.Rows(i3).Item("depart_name") & "</option>"
    '                Next

    '                vTbody += "                    </select>"
    '                vTbody += "                </div>"
    '                vTbody += "            </div>"
    '                vTbody += "            <div class='form-group'>"
    '                vTbody += "                <div class='col-sm-offset-2 col-sm-10'>"
    '                vTbody += "                    <button type='button' class='btn btn-sm btn-danger' id='btn_add_next_submit'>"
    '                vTbody += "                        <span class='glyphicon glyphicon-floppy-save' aria-hidden='true'></span> แทรก"
    '                vTbody += "                    </button>"
    '                vTbody += "                </div>"
    '                vTbody += "            </div>"
    '                vTbody += "        </div>"
    '                vTbody += "    </div>"
    '                vTbody += "</div>"
    '            End If

    '            Dim require_remark As String = ""
    '            Dim require_file As String = ""

    '            If vDT.Rows(i).Item("require_remark") = 1 Then
    '                require_remark = "required-f"
    '            End If
    '            If vDT.Rows(i).Item("require_file") = 1 Then
    '                require_file = "required-f"
    '            End If

    '            Dim vDT2 As New DataTable
    '            vDT2 = rLoadStatusFormApprove(vRequest_id, vDT.Rows(i).Item("flow_step"), vDT.Rows(i).Item("approval"))

    '            vTbody += "<div class='panel panel-danger'>"
    '            vTbody += "    <div class='panel-heading panel-fonting'>ฟอร์มอนุมัติ</div>"
    '            vTbody += "    <div class='panel-body'>"
    '            vTbody += "        <div class='form-horizontal'>"
    '            vTbody += "            <input id='flow_no' type='hidden' value='" & vDT.Rows(i).Item("flow_no") & "'>"
    '            vTbody += "            <input id='flow_sub' type='hidden' value='" & vDT.Rows(i).Item("flow_sub") & "'>"
    '            vTbody += "            <input id='next_step' type='hidden' value='" & vDT.Rows(i).Item("next_step") & "'>"
    '            vTbody += "            <input id='department' type='hidden' value='" & vDT.Rows(i).Item("depart_id") & "'>"
    '            vTbody += "            <div class='form-group required-f'>"
    '            vTbody += "                <label class='col-sm-2 control-label'>สถานะ</label>"
    '            vTbody += "                <div class='col-sm-10'>"
    '            vTbody += "                    <select id='sel_flow_status' class='form-control input-sm' >"
    '            vTbody += "                         <option value=''>เลือกสถานะ</option>"

    '            For i2 As Integer = 0 To vDT2.Rows().Count() - 1
    '                vTbody += "<option value='" & vDT2.Rows(i2).Item("status_id") & "'>" & vDT2.Rows(i2).Item("status_name") & "</option>"
    '            Next

    '            vTbody += "                    </select>"
    '            vTbody += "                </div>"
    '            vTbody += "            </div>"
    '            vTbody += "            <div class='form-group " + require_remark + "'>"
    '            vTbody += "                <label class='col-sm-2 control-label'>หมายเหตุ</label>"
    '            vTbody += "                <div class='col-sm-10'>"
    '            vTbody += "                    <input type='text' class='form-control input-sm' id='txt_flow_remark' placeholder='กรอกหมายเหตุ..'>"
    '            vTbody += "                </div>"
    '            vTbody += "            </div>"
    '            'vTbody += "            <div class='form-group " + require_file + "'>"
    '            'vTbody += "                <label class='col-sm-2 control-label'>เอกสารประกอบ</label>"
    '            'vTbody += "                <div class='col-sm-10'>"
    '            'vTbody += "                    <input id='flow_file' name='flow_file' type='file' class='form-control input-sm'>"
    '            'vTbody += "                </div>"
    '            'vTbody += "            </div>"
    '            vTbody += "            <div class='form-group'>"
    '            vTbody += "                <div class='col-sm-offset-2 col-sm-10'>"
    '            vTbody += "                    <button type='button' class='btn btn-sm btn-success' id='btn_flow_submit'>"
    '            vTbody += "                        <span class='glyphicon glyphicon-floppy-save' aria-hidden='true'></span> บันทึก"
    '            vTbody += "                    </button>"
    '            vTbody += "                </div>"
    '            vTbody += "            </div>"
    '            vTbody += "        </div>"
    '            vTbody += "    </div>"
    '            vTbody += "</div>"
    '            vTbody += "</td>"

    '            If vDT.Rows(i).Item("next_step") <> "-"
    '                vShow_form = 1
    '            End If
    '        Else
    '            vTbody += "<td>" + vDT.Rows(i).Item("flow_remark") + "</td>"
    '            vTbody += "<td>" + vFile + "</td>"
    '        End If

    '        If vStep_Current = 0 _
    '        And vDT.Rows(i).Item("flow_complete") = 0 And vDT.Rows(i).Item("flow_step") = vRequest_step And vDT.Rows(i).Item("next_step") <> "-" _
    '        Then
    '            vStep_Current = 1
    '            vTbody = vTbody.Replace("replace-class " & i, "current-flow")
    '        End If

    '        vTbody += "</tr>"
    '    Next

    '    If vReqeust_permiss = 0 Then
    '        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('No permission!!'); window.location = 'default.aspx';", True)
    '        'CP.kickDefault("nopermiss")
    '        'Session("request_permiss") = 1
    '    End If

    '    return vTbody
    'End Function

    Public Function rLoadFlowBody(ByVal vRequest_id As String, ByVal vRequest_status As String, ByVal vRequest_step As String, Optional ByVal vReqeust_permiss As Integer = 0) As String
        'Dim vDTU As New DataTable
        'vDTU = rGetRequestor(vRequest_id)
        'Dim create_by As String = vDTU.Rows(0).Item("create_by")
        'Dim uemail_cc1 As String = vDTU.Rows(0).Item("uemail_cc1")
        'Dim uemail_cc2 As String = vDTU.Rows(0).Item("uemail_cc2")

        Dim vDT As New DataTable
        vDT = rLoadFlowDT(vRequest_id)

        Dim vTbody As String = ""
        Dim vShow_form As Integer = 0
        Dim vStep_Current As Integer = 0

        For i As Integer = 0 To vDT.Rows().Count() - 1
            Dim flow_step As String = vDT.Rows(i).Item("flow_step")

            If vDT.Rows(i).Item("flow_sub_step") <> 0 Then
                flow_step += "." & vDT.Rows(i).Item("flow_sub_step")
            End If

            vTbody += "<tr class='replace-class " & i & "'>"
            vTbody += "<td>" & (i + 1) & "</td>"
            vTbody += "<td>" & flow_step & "</td>"
            vTbody += "<td>" & vDT.Rows(i).Item("next_step") & "</td>"
            vTbody += "<td>" + vDT.Rows(i).Item("depart_name") + "</td>"

            vTbody += "<td>"
            vTbody += rSplit_uemail(vDT.Rows(i).Item("send_uemail"), vDT.Rows(i).Item("uemail"))
            vTbody += "</td>"

            Dim vLabel As String = "default"
            Dim vFile As String = ""

            If vDT.Rows(i).Item("flow_complete") = 1 Then
                vLabel = "complete"
                vFile = "-"

                vDT.Rows(i).Item("flow_remark") = CP.rNullHyphen(vDT.Rows(i).Item("flow_remark"))

                'If vDT.Rows(i).Item("flow_file").Trim() <> "" Then
                '    If vReqeust_permiss = 0 Then
                '        vFile = file_dont_request_permiss

                '    ElseIf (rCheckLoginNotRequestor(create_by, uemail_cc1, uemail_cc2) = True) _
                '    Or (rCheckLoginIsRequestor(create_by, uemail_cc1, uemail_cc2) = True And (vRequest_status = 100 Or vDT.Rows(i).Item("flow_status") = 30 Or vDT.Rows(i).Item("flow_status") = 30)) Then
                '        vFile = "<a href='" + global_path + vDT.Rows(i).Item("flow_file") + "' target='_blank'>เปิดไฟล์..</a>"
                '    Else
                '        vFile = file_wait_request_end
                '    End If
                'End If
            End If

            vTbody += "<td><span class='flow-sts label label-" + vLabel + "'>" + vDT.Rows(i).Item("status_name") + "</span></td>"
            vTbody += "<td>" + vDT.Rows(i).Item("update_date") + "</td>"
            vTbody += "<td>" + vDT.Rows(i).Item("update_by") + "</td>"

            '***** สถานะล่าสุด ต้องไม่เท่ากับ ปิดคำขอ, ยกเลิกคำขอ, รอข้อมูล
            If vShow_form = 0 And vStep_Current = 0 _
            And vRequest_status <> 100 And vRequest_status <> 105 And vRequest_status <> 110 _
            And vDT.Rows(i).Item("flow_complete") = 0 _
            And (vDT.Rows(i).Item("flow_step") = vRequest_step _
            Or (vDT.Rows(i).Item("flow_step") <= vRequest_step And vDT.Rows(i).Item("next_step") = "-")) _
            And vDT.Rows(i).Item("next_step") <> "-" _
            And vDT.Rows(i).Item("uemail") Like "*" + Session("Uemail") + ";*" Then

                vTbody += "<td colspan='2'>"

                If vDT.Rows(i).Item("add_next") = 1 Then
                    Dim vSql3 As String
                    vSql3 = "select depart_id, depart_name "
                    vSql3 += "from department "
                    vSql3 += "where disable = 0 and depart_id > 1 "

                    Dim vDT3 As New DataTable
                    vDT3 = DB105.GetDataTable(vSql3)

                    vTbody += "<div class='panel panel-danger'>"
                    vTbody += "    <div class='panel-heading panel-fonting'>แทรกลำดับถัดไป (Add Next) </div>"
                    vTbody += "    <div class='panel-body'>"
                    vTbody += "        <div class='form-horizontal'>"
                    vTbody += "            <div class='form-group'>"
                    vTbody += "            <label class='col-sm-12 txt-red'>- หากต้องการขอข้อมูลเพิ่ม กรุณาขอข้อมูลเพิ่มให้เสร็จสิ้น ก่อนแทรกลำดับถัดไป</label>"
                    vTbody += "            <label class='col-sm-12 txt-red'>- หากต้องการแทรกลำดับถัดไป กรุณาแทรกลำดับถัดไป ก่อนทำการอนุมัติ</label>"
                    vTbody += "            </div>"
                    vTbody += "            <div class='form-group required-n'>"
                    vTbody += "                <label class='col-sm-2 control-label'>ส่วนงาน</label>"
                    vTbody += "                <div class='col-sm-10'>"
                    vTbody += "                    <select id='sel_depart_id' class='form-control input-sm' >"
                    vTbody += "                         <option value=''>เลือกส่วนงาน</option>"

                    For i3 As Integer = 0 To vDT3.Rows().Count() - 1
                        vTbody += "<option value='" & vDT3.Rows(i3).Item("depart_id") & "'>" & vDT3.Rows(i3).Item("depart_name") & "</option>"
                    Next

                    vTbody += "                    </select>"
                    vTbody += "                </div>"
                    vTbody += "            </div>"
                    vTbody += "            <div class='form-group'>"
                    vTbody += "                <div class='col-sm-offset-2 col-sm-10'>"
                    vTbody += "                    <button type='button' class='btn btn-sm btn-danger' id='btn_add_next_submit'>"
                    vTbody += "                        <span class='glyphicon glyphicon-floppy-save' aria-hidden='true'></span> แทรก"
                    vTbody += "                    </button>"
                    vTbody += "                </div>"
                    vTbody += "            </div>"
                    vTbody += "        </div>"
                    vTbody += "    </div>"
                    vTbody += "</div>"
                End If

                Dim require_remark As String = ""
                Dim require_file As String = ""

                If vDT.Rows(i).Item("require_remark") = 1 Then
                    require_remark = "required-f"
                End If
                If vDT.Rows(i).Item("require_file") = 1 Then
                    require_file = "required-f"
                End If

                Dim vDT2 As New DataTable
                vDT2 = rLoadStatusFormApprove(vRequest_id, vDT.Rows(i).Item("flow_step"), vDT.Rows(i).Item("approval"))

                vTbody += "<div class='panel panel-danger'>"
                vTbody += "    <div class='panel-heading panel-fonting'>ฟอร์มอนุมัติ</div>"
                vTbody += "    <div class='panel-body'>"
                vTbody += "        <div class='form-horizontal'>"
                vTbody += "            <input id='flow_no' type='hidden' value='" & vDT.Rows(i).Item("flow_no") & "'>"
                vTbody += "            <input id='flow_sub' type='hidden' value='" & vDT.Rows(i).Item("flow_sub") & "'>"
                vTbody += "            <input id='next_step' type='hidden' value='" & vDT.Rows(i).Item("next_step") & "'>"
                vTbody += "            <input id='back_step' type='hidden' value='" & vDT.Rows(i).Item("back_step") & "'>"
                vTbody += "            <input id='department' type='hidden' value='" & vDT.Rows(i).Item("depart_id") & "'>"
                vTbody += "            <div class='form-group required-f'>"
                vTbody += "                <label class='col-sm-2 control-label'>สถานะ</label>"
                vTbody += "                <div class='col-sm-10'>"
                vTbody += "                    <select id='sel_flow_status' class='form-control input-sm' >"
                vTbody += "                         <option value=''>เลือกสถานะ</option>"

                For i2 As Integer = 0 To vDT2.Rows().Count() - 1
                    vTbody += "<option value='" & vDT2.Rows(i2).Item("status_id") & "'>" & vDT2.Rows(i2).Item("status_name") & "</option>"
                Next

                vTbody += "                    </select>"
                vTbody += "                </div>"
                vTbody += "            </div>"
                vTbody += "            <div class='form-group " + require_remark + "'>"
                vTbody += "                <label class='col-sm-2 control-label'>หมายเหตุ</label>"
                vTbody += "                <div class='col-sm-10'>"
                vTbody += "                    <textarea type='text' id='txt_flow_remark' class='form-control input-sm' rows='2' placeholder='กรอกหมายเหตุ..'></textarea>"
                vTbody += "                </div>"
                vTbody += "            </div>"
                'vTbody += "            <div class='form-group " + require_file + "'>"
                'vTbody += "                <label class='col-sm-2 control-label'>เอกสารประกอบ</label>"
                'vTbody += "                <div class='col-sm-10'>"
                'vTbody += "                    <input id='flow_file' name='flow_file' type='file' class='form-control input-sm'>"
                'vTbody += "                </div>"
                'vTbody += "            </div>"
                vTbody += "            <div class='form-group'>"
                vTbody += "                <div class='col-sm-offset-2 col-sm-10'>"
                vTbody += "                    <button type='button' class='btn btn-sm btn-success' id='btn_flow_submit'>"
                vTbody += "                        <span class='glyphicon glyphicon-floppy-save' aria-hidden='true'></span> บันทึก"
                vTbody += "                    </button>"
                vTbody += "                </div>"
                vTbody += "            </div>"
                vTbody += "        </div>"
                vTbody += "    </div>"
                vTbody += "</div>"
                vTbody += "</td>"

                If vDT.Rows(i).Item("next_step") <> "-" Then
                    vShow_form = 1
                End If
            Else
                vTbody += "<td>" + vDT.Rows(i).Item("flow_remark") + "</td>"
                vTbody += "<td>" + vFile + "</td>"
            End If

            If vStep_Current = 0 _
            And vDT.Rows(i).Item("flow_complete") = 0 And vDT.Rows(i).Item("flow_step") = vRequest_step And vDT.Rows(i).Item("next_step") <> "-" _
            Then
                vStep_Current = 1
                vTbody = vTbody.Replace("replace-class " & i, "current-flow")
            End If

            vTbody += "</tr>"
        Next

        If vReqeust_permiss = 0 Then
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('No permission!!'); window.location = 'default.aspx';", True)
            'CP.kickDefault("nopermiss")
            'Session("request_permiss") = 1
        End If

        Return vTbody
    End Function

    Public Function rLoadStatusFormApprove(ByVal vRequest_id As String, ByVal vFlow_step As String, ByVal vApproval As String) As DataTable
        Dim vSql As String
        'vSql = "select status_id, status_name "
        'vSql += "from request_status "
        '***** แก้เป็น ถ้าหาก flow_step นั้นมี sub หรือมีการแทรก flow แล้ว
        '***** จะไม่มีสถานะ ขอข้อมูลเพิ่มให้เลือก

        vSql += "select status_id, status_name "
        vSql += "from ( "
        vSql += "    select *, 'jj' for_join "
        vSql += "    from request_status rs "
        vSql += ") rs "
        vSql += "join ( "
        vSql += "    select count(1) count_sub, 'jj' for_join "
        vSql += "    from request_flow_sub "
        vSql += "    where request_id = '" & vRequest_id & "' "
        vSql += "    and flow_step = '" & vFlow_step & "' "
        vSql += "    and flow_reply = 0 "
        vSql += "    and disable = 0 "
        vSql += ") cs "
        vSql += "on cs.for_join = rs.for_join "
        vSql += "and (count_sub = 0 or (count_sub > 0 and nexted <> 2)) "
        'vSql += "and count_sub > 0 and nexted <> 2 "

        vSql += "where disable = 0 "
        vSql += "and (approval = 0 or approval = " & vApproval & ") "

        '***** มี sql เงื่อนไขด้านบนเพิ่มเข้ามาแล้ว if นี้ก็ไม่ต้องใช้
        'If vDT.Rows(i).Item("flow_sub_step") > 0 Then
        'vSql += "and nexted <> 2 "
        'END If

        vSql += "order by status_id "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rViewDetail(ByVal vRequest_id As String, ByVal hide_uemail_create As String) As Integer
        Dim vDT As New DataTable
        vDT = rLoadFlowDT(vRequest_id)

        Dim vView As Integer = 0

        For i As Integer = 0 To vDT.Rows().Count() - 1

            If hide_uemail_create = Session("uemail") Or vDT.Rows(i).Item("uemail") Like "*" + Session("Uemail") + ";*"
                vView = 1
            End If
        Next

        For i2 As Integer = 0 To vDT.Rows().Count() - 1

            If vDT.Rows(i2).Item("flow_complete") = 0 And vDT.Rows(i2).Item("flow_status") = 110 _
            And (hide_uemail_create = Session("uemail") Or vDT.Rows(i2).Item("uemail") Like "*" + Session("Uemail") + ";*")
                vView = 2
            End If
        Next

        Return vView
    End Function


    Public Function rGetRequestor(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select create_by, uemail_cc1, uemail_cc2, request_status, last_update from request "
        vSql += "where request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rCheckLoginNotRequestor(ByVal create_by As String, ByVal uemail_cc1 As String, ByVal uemail_cc2 As String) As Boolean
        If Session("uemail") <> create_by And Session("uemail") <> uemail_cc1 And Session("uemail") <> uemail_cc2 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function rCheckLoginIsRequestor(ByVal create_by As String, ByVal uemail_cc1 As String, ByVal uemail_cc2 As String) As Boolean
        If Session("uemail") = create_by Or Session("uemail") = uemail_cc1 Or Session("uemail") = uemail_cc2 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function rSplit_uemail(ByVal vSend_uemail As String, ByVal vUemail As String) As String
            Dim vSplit_uemail As String() = Regex.Split(vUemail, ";")
            Dim vAll_email AS String = ""

            For Each eSplit As String In vSplit_uemail
                If eSplit.Trim() <> "" Then
                    vAll_email += "<p>" + eSplit + "@jasmine.com</p>"
                End If
            Next

            If vSend_uemail <> vUemail Then
                vSend_uemail = vSend_uemail.Replace(";", "") + "@jasmine.com"
                vAll_email = "<strong data-container='body' data-toggle='popover' data-placement='bottom' data-content='" + vAll_email + "'>" + vSend_uemail + "</strong>"
            End If
            
            Return vAll_email
    End Function

    Public Function rZeroRO(ByVal vRO As String) As String
        If vRO.Length = 1 Then
            Return "0" + vRO
        Else
            Return vRO
        End If
    End Function
#End Region


#Region "SaveRequest"
    Public Function rInsertRequest(ByVal subject_id As String, ByVal prefix_id As String _
        , ByVal flow_id As String, ByVal request_title_id As String, ByVal request_title As String _
        , ByVal request_remark As String, ByVal uemail_verify As String, ByVal uemail_approve As String _
        , ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal create_by As String _
        , Optional ByVal bcs_number As String = "", Optional ByVal doc_number As String = "", Optional ByVal amount As String = "" _
        , Optional ByVal account_number As String = "", Optional ByVal account_name As String = "" _
        , Optional ByVal account_number_to As String = "", Optional ByVal account_name_to As String = "" _
        , Optional ByVal redebt_cause_id As String = "", Optional ByVal area_ro As String = "", Optional ByVal shop_code As String = "" _
        , Optional ByVal dx01 As String = "", Optional ByVal dx02 As String = "", Optional ByVal dx03 As String = "" _
        , Optional ByVal mx01 As String = "", Optional ByVal mx02 As String = "", Optional ByVal mx03 As String = "" _
        , Optional ByVal fx01 As String = "", Optional ByVal fx02 As String = "", Optional ByVal fx03 As String = "" _
        , Optional ByVal tx01 As String = "", Optional ByVal tx02 As String = "", Optional ByVal tx03 As String = "" _
        ) As String

        Dim vSqlIn As String = ""
        vSqlIn += declareDX01(dx01)
        vSqlIn += declareDX02(dx02)
        vSqlIn += declareDX03(dx03)

        vSqlIn += "DECLARE @newid varchar(12) "
        vSqlIn += "SET @newid = '" + prefix_id + "' + RIGHT(LEFT(CONVERT(varchar, GETDATE(),112),6),4) + dbo.run4digit(COALESCE((select COUNT(1)+1 from request where LEFT(CONVERT(varchar, GETDATE(),112),6) = LEFT(CONVERT(varchar, create_date,112),6)), 0)) "
        
        '''''''''''''''''''''''''''''''''''''''''''''''' Insert Request ''''''''''''''''''''''''''''''''''''''''''''''''
        vSqlIn += "INSERT INTO request ("
        vSqlIn += "request_id"
        vSqlIn += ", subject_id"
        vSqlIn += ", request_title_id"
        vSqlIn += ", request_title"
        vSqlIn += ", request_remark"
        vSqlIn += ", uemail_verify"
        vSqlIn += ", uemail_approve"
        vSqlIn += ", uemail_cc1"
        vSqlIn += ", uemail_cc2"
        vSqlIn += ", create_by"
        vSqlIn += ", create_date "
        vSqlIn += ", bcs_number "
        vSqlIn += ", doc_number"
        vSqlIn += ", amount"
        vSqlIn += ", account_number"
        vSqlIn += ", account_name"
        vSqlIn += ", account_number_to"
        vSqlIn += ", account_name_to "
        vSqlIn += ", redebt_cause_id"
        vSqlIn += ", area_ro"
        vSqlIn += ", shop_code "
        vSqlIn += ", fx01 "
        vSqlIn += ", fx02 "
        vSqlIn += ", fx03 "
        vSqlIn += ", tx01 "
        vSqlIn += ", tx02 "
        vSqlIn += ", tx03 "
        vSqlIn += ", mx01 "
        vSqlIn += ", mx02 "
        vSqlIn += ", mx03 "
        vSqlIn += ", dx01 "
        vSqlIn += ", dx02 "
        vSqlIn += ", dx03 "
        vSqlIn += ") VALUES ("
        vSqlIn += "@newid"
        vSqlIn += ", " + subject_id + ""
        vSqlIn += ", '" + request_title_id + "'"
        vSqlIn += ", '" + request_title + "'"
        vSqlIn += ", '" + request_remark + "'"
        vSqlIn += ", '" + uemail_verify + "'"
        vSqlIn += ", '" + uemail_approve + "'"
        vSqlIn += ", '" + uemail_cc1 + "'"
        vSqlIn += ", '" + uemail_cc2 + "'"
        vSqlIn += ", '" + create_by + "'"
        vSqlIn += ", GETDATE()"
        vSqlIn += ", '" + bcs_number + "'"
        vSqlIn += ", '" + doc_number + "'"
        vSqlIn += ", '" + amount + "'"
        vSqlIn += ", '" + account_number + "'"
        vSqlIn += ", '" + account_name + "'"
        vSqlIn += ", '" + account_number_to + "'"
        vSqlIn += ", '" + account_name_to + "' "
        vSqlIn += ", '" + redebt_cause_id + "'"
        vSqlIn += ", '" + area_ro + "'"
        vSqlIn += ", '" + shop_code + "' "
        vSqlIn += ", '" + fx01 + "' "
        vSqlIn += ", '" + fx02 + "' "
        vSqlIn += ", '" + fx03 + "' "
        vSqlIn += ", '" + tx01 + "' "
        vSqlIn += ", '" + tx02 + "' "
        vSqlIn += ", '" + mx03 + "' "
        vSqlIn += ", '" + mx01 + "' "
        vSqlIn += ", '" + mx02 + "' "
        vSqlIn += ", '" + mx03 + "' "
        vSqlIn += ", @dx01 "
        vSqlIn += ", @dx02 "
        vSqlIn += ", @dx03 "
        vSqlIn += ") "
        '''''''''''''''''''''''''''''''''''''''''''''''' Insert Request ''''''''''''''''''''''''''''''''''''''''''''''''

        '''''''''''''''''''''''''''''''''''''''''''''''' Insert Request Flow ''''''''''''''''''''''''''''''''''''''''''''''''
        vSqlIn += "INSERT INTO request_flow ( "
        vSqlIn += "request_id, flow_id, depart_id, flow_step, next_step, "
        vSqlIn += "send_uemail, uemail, approval, require_remark, require_file, add_next) "
        vSqlIn += "select @newid, fp.flow_id, fp.depart_id, fp.flow_step, fp.next_step, "
        vSqlIn += "dp.uemail, dp.uemail, fp.approval, fp.require_remark, fp.require_file, dp.add_next "
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
        vSqlIn += "where flow_id = " + flow_id +" "
        vSqlIn += "order by fp.flow_step "
        '''''''''''''''''''''''''''''''''''''''''''''''' Insert Request Flow ''''''''''''''''''''''''''''''''''''''''''''''''

        '''''''''''''''''''''''''''''''''''''''''''''''' Set depart uemail ''''''''''''''''''''''''''''''''''''''''''''''''
        vSqlIn += "update request_flow set send_uemail = '" + uemail_verify + "', uemail = '" + rCheckGroupMail(uemail_verify) + "' "
        vSqlIn += "where depart_id = 2 and request_id = @newid "

        vSqlIn += "update request_flow set send_uemail = '" + uemail_approve + "', uemail = '" + rCheckGroupMail(uemail_approve) + "' "
        vSqlIn += "where depart_id = 1 and request_id = @newid "
        
        vSqlIn += "update request_flow set send_uemail = '" + create_by + "', uemail = '" + create_by + "' "
        vSqlIn += "where depart_id = 0 and request_id = @newid "
        '''''''''''''''''''''''''''''''''''''''''''''''' Set depart uemail ''''''''''''''''''''''''''''''''''''''''''''''''
        
        vSqlIn += "select @newid newid"

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSqlIn)

        Dim vNewID As String = vDT.Rows(0).Item("newid")

        return vNewID
    End Function

    Public Sub InsertRequestFile(ByVal pageSubject_id As String, ByVal pageUrl As String, ByVal vRequest_id As String, ByVal request_file As String)
        'Dim vSqlIn As String 
        'vSqlIn = "update request set request_file = '" + request_file + "' "
        'vSqlIn += "where request_id = '" & vRequest_id & "' "

        'If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
        Dim vAlert_Mss As String = "ส่งอีเมล์แจ้ง ไปยังผู้ดำเนินการท่านแรกแล้ว"
        Dim vUrl_Redirect = "update_" & pageUrl & ".aspx?request_id=" & vRequest_id

        sendMailAndRedirect("Open_Flow", vRequest_id, vAlert_Mss, vUrl_Redirect)
        'Else
        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        'End If
    End Sub

    Public Sub UpdateRequest(ByVal vRequest_id As String _
       , ByVal request_file As String, ByVal request_remark As String _
       , ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal update_by As String _
       , ByVal create_by As String, ByVal create_ro As String _
       , ByVal request_title_id As String, ByVal request_title As String _
       , Optional ByVal bcs_number As String = "", Optional ByVal doc_number As String = "", Optional ByVal amount As String = "" _
       , Optional ByVal account_number As String = "", Optional ByVal account_name As String = "" _
       , Optional ByVal account_number_to As String = "", Optional ByVal account_name_to As String = "" _
       , Optional ByVal redebt_cause_id As String = "", Optional ByVal area_ro As String = "", Optional ByVal shop_code As String = "" _
       , Optional ByVal dx01 As String = "", Optional ByVal dx02 As String = "", Optional ByVal dx03 As String = "" _
       , Optional ByVal mx01 As String = "", Optional ByVal mx02 As String = "", Optional ByVal mx03 As String = "" _
       , Optional ByVal fx01 As String = "", Optional ByVal fx02 As String = "", Optional ByVal fx03 As String = "" _
       , Optional ByVal tx01 As String = "", Optional ByVal tx02 As String = "", Optional ByVal tx03 As String = "" _
       )

        Dim vSqlIn As String

        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request '''''''''''''''''''''''''''''''''''''''''''''''' 
        'vSqlIn += declareDX01(dx01)
        'vSqlIn += declareDX02(dx02)
        'vSqlIn += declareDX03(dx03)

        'vSqlIn += "update request set "
        'vSqlIn += "request_status = 0 "
        'vSqlIn += ", request_title_id = '" + request_title_id + "'"
        'vSqlIn += ", request_title = '" + request_title + "'"
        'vSqlIn += ", request_remark = '" + request_remark + "'"
        'vSqlIn += ", uemail_cc1 = '" + uemail_cc1 + "'"
        'vSqlIn += ", uemail_cc2 = '" + uemail_cc2 + "'"
        'vSqlIn += ", create_ro = '" + create_ro + "'"
        'vSqlIn += ", update_by = '" + update_by + "'"
        'vSqlIn += ", update_date = getdate() "
        'vSqlIn += ", bcs_number = '" + bcs_number + "'"
        'vSqlIn += ", doc_number = '" + doc_number + "'"
        'vSqlIn += ", amount = '" + amount + "'"
        'vSqlIn += ", account_number = '" + account_number + "'"
        'vSqlIn += ", account_name = '" + account_name + "'"
        'vSqlIn += ", account_number_to = '" + account_number_to + "'"
        'vSqlIn += ", account_name_to = '" + account_name_to + "'"
        'vSqlIn += ", redebt_cause_id = '" + redebt_cause_id + "'"
        'vSqlIn += ", area_ro = '" + area_ro + "'"
        'vSqlIn += ", shop_code = '" + shop_code + "'"
        'vSqlIn += ", mx01 = '" + mx01 + "'"
        'vSqlIn += ", mx02 = '" + mx02 + "'"
        'vSqlIn += ", mx03 = '" + mx03 + "'"
        'vSqlIn += ", dx01 = @dx01 "
        'vSqlIn += ", dx02 = @dx02 "
        'vSqlIn += ", dx03 = @dx03 "

        'If request_file.Trim() <> "" Then
        '    vSqlIn += ", request_file = '" + request_file + "'"
        'End If

        'vSqlIn += "where request_id = '" + vRequest_id + "' "
        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request ''''''''''''''''''''''''''''''''''''''''''''''''

        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request Flow ''''''''''''''''''''''''''''''''''''''''''''''''
        vSqlIn += "update request_flow set "
        vSqlIn += "flow_status = 0 "
        vSqlIn += "where request_id = '" + vRequest_id + "' and flow_complete = 0 and flow_status = 110 "

        vSqlIn += "update request_flow_sub set "
        vSqlIn += "flow_status = 0 "
        vSqlIn += "where request_id = '" + vRequest_id + "' and flow_complete = 0 and flow_status = 110 "

        If uemail_cc1 <> "" Then
            create_by += ";" + uemail_cc1
        End If
        If uemail_cc2 <> "" Then
            create_by += ";" + uemail_cc2
        End If

        vSqlIn += "update request_flow set send_uemail = '" + create_by + "', uemail = '" + create_by + "' "
        vSqlIn += "where depart_id = 0 and request_id = '" + vRequest_id + "' "
        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request Flow ''''''''''''''''''''''''''''''''''''''''''''''''

        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            'Dim vAlert_Mss As String = "ส่งอีเมล์แจ้ง ผู้สร้างคำขอและ ผู้ที่ขอข้อมูลเพิ่ม"
            Dim vAlert_Mss As String = "ส่งอีเมล์แจ้ง มีการแก้ไขข้อมูลคำขอ"
            sendMailAndRedirect("Reply_2", vRequest_id, vAlert_Mss)

        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        End If
    End Sub
#End Region


#Region "FlowStep"

    Public Sub SaveFlow(ByVal uemail As String, ByVal flow_no As String, ByVal flow_sub As String _
        , ByVal next_step As String, ByVal back_step As String, ByVal department As String _
        , ByVal flow_status As String, ByVal flow_remark As String, ByVal flow_file As String _
    )
        Dim xRequest_id = HttpContext.Current.Request.QueryString("request_id")

        Dim vCase As String = ""
        Dim vAlert_Mss As String = ""
        Dim vSqlIn As String
        Dim vSql As String
        vSql = "select nexted "
        vSql += "from request_status "
        vSql += "where status_id = " + flow_status

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Dim vTable_flow As String = "request_flow" + flow_sub
        Dim vNexted As Integer = vDT.Rows(0).Item("nexted")

        If vNexted = 1 Then '***** Next_Flow

            If IsNumeric(next_step) = True Then
                Dim vSql2 As String = "select flow_step, next_step "
                vSql2 += "from request_flow "
                vSql2 += "where request_id = '" + xRequest_id + "' and flow_step = " & (CInt(next_step) - 1) & " "
                vSql2 += "and flow_complete = 0 and next_step <> '-' "
                vSql2 += "and disable = 0 "
                vSql2 += "union all "
                vSql2 += "select flow_step, next_step "
                vSql2 += "from request_flow_sub "
                vSql2 += "where request_id = '" + xRequest_id + "' and flow_step = " & (CInt(next_step) - 1) & " "
                vSql2 += "and flow_complete = 0 and next_step <> '-' "
                vSql2 += "and disable = 0 "

                Dim vDT2 As New DataTable
                vDT2 = DB105.GetDataTable(vSql2)

                If vDT2.Rows().Count() > 1 Then
                    next_step = vDT2.Rows(0).Item("flow_step")
                End If
            End If

            vSqlIn = "update " + vTable_flow + " set "
            vSqlIn += "flow_status = '" + flow_status + "', flow_remark = '" + flow_remark + "', flow_file = '" + flow_file + "', "
            vSqlIn += "flow_complete = 1, update_by = '" + uemail + "', update_date = getdate() "
            vSqlIn += "where no = '" & flow_no & "' "

            vSqlIn += "update FeasibilityDocument set "
            vSqlIn += "request_status = '" + flow_status + "', request_step = '" & next_step & "' "
            vSqlIn += ", last_update = getdate(), last_depart = '" & department & "' "
            vSqlIn += "where Document_No = '" + xRequest_id + "' "

            vCase = "Next_Flow"
            vAlert_Mss = "ส่งอีเมล์แจ้ง ส่วนงานใน Flow Step ถัดไปแล้ว"

        ElseIf vNexted = 3 Then '***** Cancle_Flow
            If back_step = 0 Then '***** back_step = 0 ให้ ยกเลิกคำขอ อัตโนมัติ
                vSqlIn = "update " + vTable_flow + " set "
                vSqlIn += "flow_status = '" + flow_status + "', flow_remark = '" + flow_remark + "', flow_file = '" + flow_file + "', "
                vSqlIn += "flow_complete = 1, update_by = '" + uemail + "', update_date = getdate() "
                vSqlIn += "where no = '" & flow_no & "' "

                vSqlIn += "update FeasibilityDocument set "
                vSqlIn += "request_status = '105' " '***** จบคำขอ ให้สถานะคำขอเป็น ยกเลิกคำขอ
                vSqlIn += ", last_update = getdate(), last_depart = '" & department & "' "
                vSqlIn += ", next_depart = '0' " '***** จบคำขอ ให้ลำดับถัดไปเป็น คนสร้างคำขอ
                vSqlIn += "where Document_No = '" + xRequest_id + "' "

                vCase = "Cancle_Flow"
                vAlert_Mss = "ส่งอีเมล์แจ้ง ยกเลิกคำขอ ให้ผู้สร้างคำขอแล้ว"
            Else
                Dim vDTF As New DataTable
                vDTF = rLoadRequestFlow(xRequest_id, back_step)

                vSqlIn = "DECLARE @newid varchar(50) "
                vSqlIn += "SET @newid = '" + xRequest_id + "' "
                vSqlIn += rSqlDisableRequestFlow(xRequest_id, vDTF.Rows(0).Item("flow_id"), back_step)
                vSqlIn += rSqlInsertRequestFlow(vDTF.Rows(0).Item("flow_id"), back_step)
                'vSqlIn += rSqlSetDepartRequestFlow( _
                '    vDTF.Rows(0).Item("uemail_verify"), vDTF.Rows(0).Item("uemail_approve"), _
                '    vDTF.Rows(0).Item("create_by"), vDTF.Rows(0).Item("uemail_cc1"), vDTF.Rows(0).Item("uemail_cc2"))
                vSqlIn += rSqlSetDepartRequestFlow( _
                    vDTF.Rows(0).Item("uemail_verify"), "", _
                    vDTF.Rows(0).Item("createBy"), "", "")

                '***** update request ให้ข้อมูลของเป็น back_step
                vSqlIn += "update FeasibilityDocument set "
                vSqlIn += "request_status = '" + flow_status + "', request_step = '" & back_step & "' "
                vSqlIn += ", last_update = getdate(), last_depart = '" & vDTF.Rows(0).Item("depart_id") & "' "
                vSqlIn += "where Document_No = '" + xRequest_id + "' "
                '***** update request ให้ข้อมูลของเป็น back_step

                vCase = "Back_Flow"
                vAlert_Mss = "ส่งอีเมล์แจ้งไม่อนุมัติให้ผู้สร้างคำขอ และผู้ที่เกี่ยวข้องเพื่อตรวจสอบข้อมูลใหม่อีกครั้ง"
            End If

        ElseIf vNexted = 2 Then '***** Reply_1 ขอเพิ่มข้อมูล
            vSqlIn = "DECLARE @flow_sub_step int select @flow_sub_step = count(rfs.no) + 1 "
            vSqlIn += "from request_flow_sub rfs "
            vSqlIn += "right join ( "
            vSqlIn += "    select request_id, flow_step "
            vSqlIn += "    from " + vTable_flow + " "
            vSqlIn += "    where no = '" & flow_no & "' "
            vSqlIn += ") rf "
            vSqlIn += "on rf.request_id = rfs.request_id and rf.flow_step = rfs.flow_step "

            vSqlIn += "update " + vTable_flow + " set "
            vSqlIn += "flow_status = '" + flow_status + "', flow_remark = '" + flow_remark + "', flow_file = '" + flow_file + "', "
            vSqlIn += "flow_complete = 1, update_by = '" + uemail + "', update_date = getdate() "
            vSqlIn += "where no = '" & flow_no & "' "

            vSqlIn += "insert request_flow_sub ( "
            vSqlIn += "flow_sub_step, request_id, flow_id, "
            vSqlIn += "depart_id, flow_step, next_step, back_step, "
            vSqlIn += "send_uemail, uemail, approval, flow_status, add_next, flow_reply) "
            vSqlIn += "select @flow_sub_step, request_id, flow_id, "
            vSqlIn += "depart_id, flow_step, next_step, back_step, "
            vSqlIn += "send_uemail, uemail, approval, 110, 0, 1 "
            vSqlIn += "from " + vTable_flow + " "
            vSqlIn += "where no = '" & flow_no & "' "

            vSqlIn += "update FeasibilityDocument set "
            vSqlIn += "request_status = 110 "
            vSqlIn += ", last_update = getdate(), last_depart = '" & department & "' "
            vSqlIn += ", next_depart = '0' " '***** ขอเพิ่มข้อมูล ให้ลำดับถัดไปเป็น คนสร้างคำขอ
            vSqlIn += "where Document_No = '" + xRequest_id + "' "

            vCase = "Reply_1"
            vAlert_Mss = "ส่งอีเมล์แจ้ง ผู้สร้างคำขอ ให้เพิ่มเติมข้อมูล"
        Else '***** ignore (รับเรื่อง, กำลังดำเนินการ)
            vSqlIn = "update " + vTable_flow + " set "
            vSqlIn += "flow_status = '" + flow_status + "', flow_remark = '" + flow_remark + "', flow_file = '" + flow_file + "', "

            If flow_status = 1 Then
                vSqlIn += "flow_complete = 1, "
            End If

            vSqlIn += "update_by = '" + uemail + "', update_date = getdate() "
            vSqlIn += "where no = '" & flow_no & "' "

            vSqlIn += "update request set "
            vSqlIn += "request_status = '" + flow_status + "', "
            vSqlIn += "last_update = getdate(), last_depart = '" & department & "' "
            vSqlIn += "where request_id = '" + xRequest_id + "' "

            vCase = "ignore"
        End If

        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            If checkEndFlow(xRequest_id) = 1 Then
                vCase = "End_Flow"
                vAlert_Mss = "ถึงลำดับสุดท้ายแล้ว ทำการปิดคำขออัตโนมัติ และส่งอีเมล์แจ้งผู้สร้างคำขอ"
            End If

            sendMailAndRedirect(vCase, xRequest_id, vAlert_Mss)
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        End If
    End Sub

    Public Sub update_Next_Flow(ByVal vRequest_id As String, ByVal next_depart As String)
        Dim vSqlIn As String = "update FeasibilityDocument set next_depart = '" + next_depart + "' "
        vSqlIn += "where Document_No = '" + vRequest_id + "' "

            DB105.ExecuteNonQuery(vSqlIn)
    End Sub

    Public Sub AddNext(ByVal uemail As String, ByVal flow_no As String, ByVal flow_sub As String, ByVal depart_id As String)
        Dim xRequest_id = HttpContext.Current.Request.QueryString("request_id")

        Dim vTable_flow As String = "request_flow" + flow_sub
        Dim vSql As String = "select * "
        vSql += "from " + vTable_flow + " "
        vSql += "where no = '" & flow_no & "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Dim vSql2 As String = "SELECT "
        vSql2 += "  dm.depart_id "
        vSql2 += ", dm.depart_name "
        vSql2 += ", dm.add_next "
        vSql2 += ", dm.group_email "
        vSql2 += ", uemail = STUFF(( "
        vSql2 += "      SELECT ';' + du.uemail "
        vSql2 += "      FROM depart_user du "
        vSql2 += "      WHERE dm.depart_id = du.depart_id "
        vSql2 += "      and start_date <= getdate() "
        vSql2 += "         and (expired_date is null or expired_date >= getdate()) "
        vSql2 += "      FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql2 += "FROM department dm "
        vSql2 += "where depart_id = " & depart_id & " "

        Dim vDT2 As New DataTable
        vDT2 = DB105.GetDataTable(vSql2)

        Dim vSend_uemail As String = vDT2.Rows(0).Item("uemail")

        If Not IsDBNull(vDT2.Rows(0).Item("group_email"))  Then
            vSend_uemail = vDT2.Rows(0).Item("group_email")
        End If

        Dim vSqlIn As String = "DECLARE @flow_sub_step int "
        vSqlIn += "select @flow_sub_step = count(1) + 1 from request_flow_sub "
        vSqlIn += "where request_id = '" + xRequest_id + "' and flow_step = '" & vDT.Rows(0).Item("flow_step") & "' "
        vSqlIn += "select @flow_sub_step "

        vSqlIn += "insert request_flow_sub ( "
        vSqlIn += "flow_sub_step, request_id, flow_id, "
        vSqlIn += "depart_id, flow_step, next_step, "
        vSqlIn += "send_uemail, uemail, approval, flow_status, add_next "
        vSqlIn += ") values ("
        vSqlIn += "@flow_sub_step, '" & xRequest_id & "',  '" & vDT.Rows(0).Item("flow_id") & "' "
        vSqlIn += ", '" & depart_id & "',  '" & vDT.Rows(0).Item("flow_step") & "',  '" & vDT.Rows(0).Item("next_step") & "' "
        vSqlIn += ", '" & vSend_uemail & "', '" & vDT2.Rows(0).Item("uemail") & "',  2,  0, '" & vDT2.Rows(0).Item("add_next") & "') "

        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            Dim vAlert_Mss As String = "แทรกลำดับถัดไป และส่งอีเมล์ให้ส่วนงานที่ถูกแทรกแล้ว"
            sendMailAndRedirect("Add_Next", xRequest_id, vAlert_Mss)
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        End If
    End Sub

    'Public Function checkEndFlow(ByVal vRequest_id As String) As String
    '    Dim check_end As String = 0

    '    Dim vSql As String = "select no, flow_step, next_step from request_flow "
    '    vSql += "where request_id = '" + vRequest_id + "' and flow_complete = 0 and next_step <> '-' "
    '    vSql += "union "
    '    vSql += "select no, flow_step, next_step from request_flow_sub "
    '    vSql += "where request_id = '" + vRequest_id + "' and flow_complete = 0 and next_step <> '-' "
    '    vSql += "order by flow_step "

    '    Dim vDT As New DataTable
    '    vDT = DB105.GetDataTable(vSql)

    '    If vDT.Rows().Count() > 0 Then
    '        If vDT.Rows(0).Item("next_step") = "end" Then

    '            Dim vSqlIn As String = ""

    '            For i As Integer = 0 To vDT.Rows().Count() - 1
    '                vSqlIn += "update request_flow set "
    '                vSqlIn += "flow_status = 100, flow_complete = 1, update_by = 'auto_end', update_date = getdate() "
    '                vSqlIn += "where no = '" & vDT.Rows(i).Item("no") & "' "
    '            Next

    '            vSqlIn += "update FeasibilityDocument set "
    '            vSqlIn += "request_status = 100 "
    '            vSqlIn += ", last_update = getdate(), last_depart = 0 "
    '            vSqlIn += ", next_depart = '0' " '***** จบคำขอ ให้ลำดับถัดไปเป็น คนสร้างคำขอ
    '            vSqlIn += "where Document_No = '" + vRequest_id + "' "

    '            If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
    '                'myredirect()
    '                check_end = 1
    '            Else
    '                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
    '            End If

    '        End If
    '    End If

    '    Return check_end
    'End Function

    Public Function checkEndFlow(ByVal vRequest_id As String) As String
        Dim check_end As String = 0

        Dim vSql As String = "select request_status from FeasibilityDocument "
        vSql += "where Document_No = '" + vRequest_id + "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows(0).Item("request_status") <> 100 And vDT.Rows(0).Item("request_status") <> 105 Then

            vSql = "select no, flow_step, next_step from request_flow "
            vSql += "where request_id = '" + vRequest_id + "' and flow_complete = 0 and next_step <> '-' "
            vSql += "and disable = 0 "
            vSql += "union "
            vSql += "select no, flow_step, next_step from request_flow_sub "
            vSql += "where request_id = '" + vRequest_id + "' and flow_complete = 0 and next_step <> '-' "
            vSql += "and disable = 0 "
            vSql += "order by flow_step "

            vDT = DB105.GetDataTable(vSql)

            If vDT.Rows().Count() > 0 Then
                If vDT.Rows(0).Item("next_step") = "end" Then

                    Dim vSqlIn As String = ""

                    For i As Integer = 0 To vDT.Rows().Count() - 1
                        vSqlIn += "update request_flow set "
                        vSqlIn += "flow_status = 100, flow_complete = 1, update_by = 'auto_end', update_date = getdate() "
                        vSqlIn += "where no = '" & vDT.Rows(i).Item("no") & "' "
                    Next

                    vSqlIn += "update FeasibilityDocument set "
                    vSqlIn += "request_status = 100 "
                    vSqlIn += ", last_update = getdate(), last_depart = 0 "
                    vSqlIn += ", next_depart = '0' " '***** จบคำขอ ให้ลำดับถัดไปเป็น คนสร้างคำขอ
                    vSqlIn += "where Document_No = '" + vRequest_id + "' "

                    If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
                        'myredirect()
                        check_end = 1
                    Else
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
                    End If

                End If
            End If

        End If

        Return check_end
    End Function

    Public Function rSqlInsertRequestFlow(ByVal flow_id As String, Optional ByVal flow_step As String = "0") As String
        Dim vSqlIn As String
        vSqlIn = "INSERT INTO request_flow ( "
        vSqlIn += "request_id, flow_id, depart_id, flow_step, next_step, back_step, "
        vSqlIn += "send_uemail, uemail, approval, require_remark, require_file, add_next) "
        vSqlIn += "select @newid, fp.flow_id, fp.depart_id, fp.flow_step, fp.next_step, fp.back_step, "
        vSqlIn += "dp.uemail, dp.uemail, fp.approval, fp.require_remark, fp.require_file, dp.add_next "
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
        vSqlIn += "where flow_id = '" + flow_id + "' "
        vSqlIn += "and fp.flow_step >= '" + flow_step + "' "
        vSqlIn += "order by fp.flow_step "

        Return vSqlIn
    End Function

    Public Function rSqlSetDepartRequestFlow(ByVal uemail_verify As String, ByVal uemail_approve As String _
        , ByVal create_by As String, ByVal uemail_cc1 As String, ByVal uemail_cc2 As String _
    ) As String
        Dim vSqlIn As String
        vSqlIn = "update request_flow set send_uemail = '" + uemail_verify + "', uemail = '" + rCheckGroupMail(uemail_verify) + "' "
        vSqlIn += "where depart_id = 2 and request_id = @newid "

        'vSqlIn += "update request_flow set send_uemail = '" + uemail_approve + "', uemail = '" + rCheckGroupMail(uemail_approve) + "' "
        'vSqlIn += "where depart_id = 1 and request_id = @newid "

        If uemail_cc1 <> "" Then
            create_by += ";" + uemail_cc1
        End If
        If uemail_cc2 <> "" Then
            create_by += ";" + uemail_cc2
        End If

        vSqlIn += "update request_flow set send_uemail = '" + create_by + "', uemail = '" + create_by + "' "
        vSqlIn += "where depart_id = 0 and request_id = @newid "

        Return vSqlIn
    End Function

    Public Function rSqlDisableRequestFlow(ByVal vRequest_id As String, ByVal flow_id As String, ByVal flow_step As String) As String
        Dim vSqlIn As String
        vSqlIn = "update request_flow set disable = 1 "
        vSqlIn += "where request_id = '" + vRequest_id + "' "
        vSqlIn += "and flow_id = '" + flow_id + "' "
        vSqlIn += "and flow_step >= '" + flow_step + "' "

        vSqlIn += "update request_flow_sub set disable = 1 "
        vSqlIn += "where request_id = '" + vRequest_id + "' "
        vSqlIn += "and flow_id = '" + flow_id + "' "
        vSqlIn += "and flow_step >= '" + flow_step + "' "

        Return vSqlIn
    End Function

    Public Function rLoadRequestFlow(ByVal vRequest_id As String, ByVal flow_step As String) As DataTable
        Dim vSql As String
        'vSql = "select flow_id, depart_id, uemail_verify, uemail_approve, create_by, uemail_cc1, uemail_cc2 "
        'vSql += "from request "
        'vSql += "join ( "
        'vSql += "    select top 1 request_id, flow_id, depart_id from request_flow "
        'vSql += "    where request_id = '" + vRequest_id + "' "
        'vSql += "    and flow_step = '" + flow_step + "' "
        'vSql += ") rf on rf.request_id = request.request_id "
        'vSql += "where request.request_id = '" + vRequest_id + "' "

        vSql = "select flow_id, depart_id, uemail_verify, createBy --, uemail_cc1, uemail_cc2 " + vbCr
        vSql += "from  FeasibilityDocument " + vbCr
        vSql += "join ( " + vbCr
        vSql += "    select top 1 request_id, flow_id, depart_id from request_flow " + vbCr
        vSql += "    where request_id = '" + vRequest_id + "' " + vbCr
        vSql += "    and flow_step = '" + flow_step + "' " + vbCr
        vSql += ") rf on rf.request_id = Document_No " + vbCr
        vSql += "where Document_No = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rCheckGroupMail(ByVal vUemail As String) As String
        Dim vSql As String
        vSql = "select dm.depart_id, dm.depart_name, dm.add_next "
        vSql += ", uemail = STUFF(( "
        vSql += "    SELECT ';' + du.uemail "
        vSql += "    FROM depart_user du "
        vSql += "    WHERE dm.depart_id = du.depart_id "
        vSql += "    and start_date <= getdate() "
        vSql += "    and (expired_date is null or expired_date >= getdate()) "
        vSql += "    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  "
        vSql += "from department dm "
        vSql += "where dm.disable = 0 and dm.group_email = '" + vUemail + "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() > 0 Then
            Return vDT.Rows(0).Item("uemail")
        Else
            Return vUemail
        End If
    End Function
#End Region


#Region "SendMail & Redirect"
    'Private 
    Public Sub sendMailAndRedirect(ByVal vCase As String, ByVal vRequest_id As String, ByVal vAlert_Mss As String, Optional ByVal vUrl_Redirect As String = "")
        If vCase = "ignore" Then
            CP.InteruptRefresh()
        End If

        SendMailSubmit(vCase, vRequest_id)
        'RedirectSubmit(vAlert_Mss, vUrl_Redirect)
    End Sub

    Public Sub RedirectSubmit(Optional ByVal vAlert_Mss As String = "", Optional ByVal vUrl_Redirect As String = "")
        Dim page As Page = HttpContext.Current.Handler

        If vUrl_Redirect.Trim() = "" Then
            vUrl_Redirect = HttpContext.Current.Request.Url.AbsoluteUri()
        End If

        If vAlert_Mss <> "" Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('" + vAlert_Mss + "'); window.location = '" + vUrl_Redirect + "';", True)
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "window.location = '" + vUrl_Redirect + "';", True)
        End If
    End Sub

    Public Sub SendMailSubmit(ByVal vCase As String, ByVal vRequest_id As String)
        Dim vDT As New DataTable
        Dim vMain_Point As String = "" '"รบกวนตรวจสอบคำขอเพื่อดำเนินการ"
        Dim vRequest_title As String = "" '"ลดหนี้กรณีชำระผิด Acc. โดยยอด APO และตัดบิลไปแล้ว  และมีการเพิ่มหนี้จากคุณ amornrat.ka เรียบร้อยแล้ว"
        Dim vSubject_name As String = "" '"ลดหนี้ชำระผิด Account"
        Dim vSubject_url As String = "" '"redebt555"
        Dim vProduct_name As String = "" '"ลดหนี้"
        Dim vUemail As String = "" '"panupong.pa;test.t;test.t;"

        Select Case vCase
            Case "Open_Flow"
                vMain_Point = "มีคำขอใหม่ รบกวนตรวจสอบคำขอเพื่อดำเนินการ"
                'vDT = loadDT_Open_Flow(vRequest_id)
                'update_Next_Flow(vRequest_id, vDT.Rows(0).Item("next_depart_id"))

                'vRequest_title = vDT.Rows(0).Item("request_title")
                'vSubject_name = vDT.Rows(0).Item("subject_name")
                'vSubject_url = vDT.Rows(0).Item("subject_url")
                'vProduct_name = vDT.Rows(0).Item("project_name")
                'vUemail = vDT.Rows(0).Item("send_uemail")
                Dim sql As String
                sql = "    SELECT rm.Document_No 'request_id', send_uemail = STUFF(( " + vbCr
                sql += "SELECT ';' + ru.send_uemail " + vbCr
                sql += "FROM request_flow ru " + vbCr
                sql += "WHERE(rm.Document_No = ru.request_id) " + vbCr
                sql += "and ru.flow_step = '1' " + vbCr
                sql += "FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') " + vbCr
                sql += "FROM dbo.FeasibilityDocument rm " + vbCr
                sql += "where rm.Document_No = '" + vRequest_id.ToString + "' "
                vDT = DB105.GetDataTable(sql)
                vRequest_title = "Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vSubject_name = "อนุมัติ Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vUemail = vDT.Rows(0).Item("send_uemail")
            Case "Cancle_Flow"
                vMain_Point = "แจ้งคำขอถูกยกเลิก (ไม่อนุมัติ)"
                'vDT = loadDT_Cancle_Flow(vRequest_id)
                'vRequest_title = vDT.Rows(0).Item("request_title")
                'vSubject_name = vDT.Rows(0).Item("subject_name")
                'vSubject_url = vDT.Rows(0).Item("subject_url")
                'vProduct_name = vDT.Rows(0).Item("project_name")
                'vUemail = vDT.Rows(0).Item("send_uemail")
                Dim sql As String
                sql = " select CreateBy 'send_uemail' " + vbCr
                sql += "from FeasibilityDocument " + vbCr
                sql += "where Document_No = '" + vRequest_id.ToString + "' " 
                vDT = DB105.GetDataTable(sql)
                vRequest_title = "Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vSubject_name = "ยกเลิก Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vUemail = vDT.Rows(0).Item("send_uemail")

            Case "Back_Flow"
                vMain_Point = "แจ้งคำขอ ไม่อนุมัติ กรูณาตรวจสอบข้อมูลีกครั้ง"
                'vDT = loadDT_Cancle_Flow(vRequest_id)
                vRequest_title = "test back"
                vSubject_name = "test back"
                vSubject_url = "test back"
                vProduct_name = "test back"
                vUemail = "panupong.pa"
            Case "Next_Flow"
                vMain_Point = "ถึงลำดับของท่านแล้ว รบกวนตรวจสอบคำขอเพื่อดำเนินการ"
                vDT = loadDT_Next_Flow(vRequest_id)
                'update_Next_Flow(vRequest_id, vDT.Rows(0).Item("next_depart_id"))

                'vRequest_title = vDT.Rows(0).Item("request_title")
                'vSubject_name = vDT.Rows(0).Item("subject_name")
                'vSubject_url = vDT.Rows(0).Item("subject_url")
                'vProduct_name = vDT.Rows(0).Item("project_name")
                'vUemail = vDT.Rows(0).Item("send_uemail")

                vRequest_title = "Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vSubject_name = "อนุมัติ Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vUemail = vDT.Rows(0).Item("send_uemail")

            Case "Add_Next"
                vMain_Point = "ท่านได้ถูกแทรกเข้าเป็นผู้ดำเนินการคำขอนี้"
                vDT = loadDT_Add_Next(vRequest_id)
                vRequest_title = vDT.Rows(0).Item("request_title")
                vSubject_name = vDT.Rows(0).Item("subject_name")
                vSubject_url = vDT.Rows(0).Item("subject_url")
                vProduct_name = vDT.Rows(0).Item("project_name")
                vUemail = vDT.Rows(0).Item("send_uemail")

            Case "Reply_1"
                vMain_Point = "คำขอต้องการให้แก้ไขข้อมูล กรุณาให้ข้อมูลภายใน7วัน ไม่เช่นนั้นคำขอจะถูกยกเลิก"
                'vDT = loadDT_Reply_1(vRequest_id)
                'vRequest_title = vDT.Rows(0).Item("request_title")
                'vSubject_name = vDT.Rows(0).Item("subject_name")
                'vSubject_url = vDT.Rows(0).Item("subject_url")
                'vProduct_name = vDT.Rows(0).Item("project_name")
                'vUemail = vDT.Rows(0).Item("send_uemail")

                Dim sql As String
                sql = " select CreateBy 'send_uemail' " + vbCr
                sql += "from FeasibilityDocument " + vbCr
                sql += "where Document_No = '" + vRequest_id.ToString + "' "
                vDT = DB105.GetDataTable(sql)
                vRequest_title = "Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vSubject_name = "แก้ไขข้อมูล Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vUemail = vDT.Rows(0).Item("send_uemail")
            Case "Reply_2"
                vMain_Point = "คำขอได้ข้อมูลเพิ่มเติมแล้ว ตรวจสอบคำขอเพื่อดำเนินการต่อไป"
                'vDT = loadDT_Reply_2(vRequest_id)
                'vRequest_title = vDT.Rows(0).Item("request_title")
                'vSubject_name = vDT.Rows(0).Item("subject_name")
                'vSubject_url = vDT.Rows(0).Item("subject_url")
                'vProduct_name = vDT.Rows(0).Item("project_name")
                Dim strSql As String
                strSql = "select rq.CreateBy + ';' + isnull(send_uemail,'') as send_uemail " + vbCr
                strSql += "from FeasibilityDocument rq  " + vbCr
                strSql += "join ( " + vbCr
                strSql += "    SELECT rm.Document_No, send_uemail = STUFF((  " + vbCr
                strSql += "        SELECT ';' + ru.send_uemail " + vbCr
                strSql += "        FROM ( " + vbCr
                strSql += "            select top 1 * from request_flow_sub " + vbCr
                strSql += "            where request_id = '" + vRequest_id.ToString + "' " + vbCr
                strSql += "            and flow_complete = 0 " + vbCr
                strSql += "            order by flow_step desc, flow_sub_step desc " + vbCr
                strSql += "        ) ru " + vbCr
                strSql += "        WHERE rm.Document_No = ru.request_id " + vbCr
                strSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') " + vbCr
                strSql += "    FROM FeasibilityDocument rm " + vbCr
                strSql += "    where rm.Document_No = '" + vRequest_id.ToString + "' " + vbCr
                strSql += ") rf " + vbCr
                strSql += "on rf.Document_No = rq.Document_No " + vbCr
                strSql += "where rq.Document_No = '" + vRequest_id.ToString + "' "
                vDT = DB105.GetDataTable(strSql)
                vRequest_title = "Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vSubject_name = "แก้ไขข้อมูลเพิ่มเติม Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vUemail = vDT.Rows(0).Item("send_uemail")

            Case "End_Flow"
                vMain_Point = "ปิดคำขอเรียบร้อยแล้ว (จบกระบวนการ)"
                'vDT = loadDT_End_Flow(vRequest_id)
                'vRequest_title = vDT.Rows(0).Item("request_title")
                'vSubject_name = vDT.Rows(0).Item("subject_name")
                'vSubject_url = vDT.Rows(0).Item("subject_url")
                'vProduct_name = vDT.Rows(0).Item("project_name")
                'vUemail = vDT.Rows(0).Item("send_uemail")
                Dim strSql As String
                strSql = "    SELECT rm.Document_No, send_uemail = STUFF(( "
                strSql += "        SELECT ';' + ru.send_uemail "
                strSql += "        FROM request_flow ru "
                strSql += "        WHERE rm.Document_No = ru.request_id "
                strSql += "        and ru.next_step = 'end' "
                strSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
                strSql += "    FROM FeasibilityDocument rm "
                strSql += "    where rm.Document_No = '" + vRequest_id + "' "
                vDT = DB105.GetDataTable(strSql)
                vRequest_title = "Feasibility เอกสารเลขที่ " + vRequest_id.ToString
                vSubject_name = "ปิดคำขอ Feasibility เอกสารเลขที่ " + vRequest_id.ToString + "เรียบร้อย (จบกระบวนการ) "
                vUemail = vDT.Rows(0).Item("send_uemail")
        End Select

        Try
            'vMain_Point += " *" + vCase

            Dim vSplit_uemail As String() = Regex.Split(vUemail, ";")

            Dim mail As New MailMessage()
            mail.From = New MailAddress("fesibility@jasmine.com")
            mail.CC.Add("weraphon.r@jasmine.com")

            For Each sMail As String In vSplit_uemail
                If sMail.Trim() <> "" Then
                    mail.To.Add(sMail + "@jasmine.com")
                End If
            Next

            'mail.Subject = "Follow Request " + vRequest_id + ": " + vMain_Point
            mail.Subject = vSubject_name

            mail.Body = rMailBody(vRequest_id, vRequest_title, vSubject_name, vSubject_url, vProduct_name, vMain_Point)

            mail.IsBodyHtml = True

            Dim smtp As New SmtpClient("smtp.jasmine.com")
            smtp.Credentials = New NetworkCredential("chancharas.w", "311227")

            smtp.Send(mail)

        Catch ex As Exception
            'CP.InteruptRefresh()
        End Try
    End Sub

    Public Function rMailBody(ByVal vRequest_id As String, ByVal vRequest_title As String, ByVal vSubject_name As String, ByVal vSubject_url As String, ByVal vProduct_name As String, ByVal vMain_Point As String) As String
        'Dim herf_url As String = "http://posweb.triplet.co.th/FollowRequest/update" + vSubject_url + ".aspx?request_id=" + vRequest_id
        Dim herf_url As String = "http://posweb.triplet.co.th/feasibility/Approve.aspx?request_id=" + vRequest_id

        Return _
        "<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' 'http://www.w3.org/TR/html4/loose.dtd'>" + _
        "<html lang='th'>" + _
        "<head>" + _
        "  <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>" + _
        "  <meta name='viewport' content='width=device-width, initial-scale=1'>" + _
        "  <meta http-equiv='X-UA-Compatible' content='IE=edge'>" + _
        "  <meta name='format-detection' content='telephone=no'>" + _
        "</head>" + _
        "<body style='margin:0; padding:0;' leftmargin='0' topmargin='0' marginwidth='0' marginheight='0'>" + _
        "  <table border='0' width='100%' height='100%' cellpadding='0' cellspacing='0'>" + _
        "    <tr>" + _
        "      <td align='center' valign='top'>" + _
        "        <br>" + _
        "        <table border='0' width='700' cellpadding='0' cellspacing='0' class='container' style='width:700px;max-width:700px;margin-top:-20px;'>" + _
        "          <tr>" + _
        "            <td class='container-padding header' align='left' style='font-family:sans-serif;font-size:24px;font-weight:bold;color:#0c86e6;padding-left:12px;padding-right:12px'>" + _
        "             Feasibility Project" + _
        "           </td>" + _
        "         </tr>" + _
        "         <tr>" + _
        "          <td class='container-padding content' align='left' style='padding:12px;background-color:#ffffff'>" + _
        "            <div class='title' style='font-family:tahoma;font-size:16px;color:#ff5501;font-weight:bold;'>" + vMain_Point + "</div>" + _
        "            <div class='body-text' style='font-family:tahoma;font-size:16px;line-height:12px;text-align:left;color:#333333'>" + _
        "              <p><span style='font-family:tahoma;font-size: 13px;'><b>ระบบ:</b> " + vProduct_name + "</p>" + _
        "              <p><span style='font-family:tahoma;font-size: 13px;'><b>หัวข้อ:</b> " + vSubject_name + "</p>" + _
        "              <p><span style='font-family:tahoma;font-size: 13px;'><b>เลขที่คำขอ:</b> </span><span style='font-family:tahoma;'>" + vRequest_id + "</span></p>" + _
        "              <p><span style='font-family:tahoma;font-size: 13px;'><b>เรื่องที่แจ้ง:</b> " + vRequest_title + "</p>" + _
        "              <br/>" + _
        "              <p><center><span style='font-family:tahoma;font-size: 13px;font-weight: bold;'>โปรดตรวจสอบและดำเนินการ</span></center></p>" + _
        "              <br/>" + _
        "              <center><a style='font-family:tahoma;font-size:35px;font-weight: bold;' href='" + herf_url + "'>Click link for more details</a></center>" + _
        "              <br/><center><span style='font-family:tahoma;font-size:13px;font-weight: bold;'>โปรดใช้โปรแกรมเปิดเว็บ Google Chrome หรือ Firefox เท่านั้น</span></center><br/>" + _
        "            </div>" + _
        "          </td>" + _
        "        </tr>" + _
        "        <tr>" + _
        "          <td class='container-padding footer-text' align='left' style='font-family:Helvetica, Arial, sans-serif;font-size: 12px;line-height:16px;color:#666;padding-left:12px;padding-right:12px'>" + _
        "            <span style='font-family:tahoma;font-size:11px;'>หากมีปัญหาการใช้งานโปรดติดต่อ:</span> support_pos@jasmine.com" + _
        "            <br><br><br><br>" + _
        "          </td>" + _
        "        </tr>" + _
        "      </table>" + _
        "    </td>" + _
        "  </tr>" + _
        "</table>" + _
        "</body>" + _
        "</html>"
    End Function
#End Region


#Region "loadDT Detail for SendMail"
    Public Function loadDT_Open_Flow(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, send_uemail, next_depart_id "
        vSql += "from request rq "
        vSql += "join subject on  subject.subject_id = rq.subject_id "
        vSql += "join project on  project.project_id = subject.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM request_flow ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        and ru.flow_step = '1' "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "

        '***** เอา department ถัดไปเพื่อไป stamp ที่ request
        vSql += "left join ( "
        vSql += "    select request_id, depart_id next_depart_id from request_flow "
        vSql += "    where request_id = '" + vRequest_id + "' and flow_step = '1' "
        vSql += ") nx "
        vSql += "on nx.request_id = rq.request_id "
        '***** เอา department ถัดไปเพื่อไป stamp ที่ request

        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_End_Flow(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, send_uemail "
        vSql += "from request rq "
        vSql += "join subject on  subject.subject_id = rq.subject_id "
        vSql += "join project on  project.project_id = subject.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM request_flow ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        and ru.next_step = 'end' "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Cancle_Flow(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, rq.create_by send_uemail "
        vSql += "from request rq "
        vSql += "join subject on  subject.subject_id = rq.subject_id "
        vSql += "join project on  project.project_id = subject.project_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Reply_1(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, "
        vSql += "rq.create_by + isnull(case when uemail_cc1 <> '' Then ';' + uemail_cc1 end + case when uemail_cc2 <> '' Then ';' + uemail_cc2 end ,'') send_uemail "
        vSql += "from request rq "
        vSql += "join subject on  subject.subject_id = rq.subject_id "
        vSql += "join project on  project.project_id = subject.project_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Reply_2(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, rq.create_by + ';' + isnull(send_uemail,'') as send_uemail "
        vSql += "from request rq "
        vSql += "join subject on  subject.subject_id = rq.subject_id "
        vSql += "join project on  project.project_id = subject.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM ( "
        vSql += "            select top 1 * from request_flow_sub "
        vSql += "            where request_id = '" + vRequest_id + "' "
        vSql += "            and flow_complete = 0 "
        vSql += "            order by flow_step desc, flow_sub_step desc "
        vSql += "        ) ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "
    
        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Add_Next(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, send_uemail "
        vSql += "from request rq "
        vSql += "join subject on  subject.subject_id = rq.subject_id "
        vSql += "join project on  project.project_id = subject.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM ( "
        vSql += "            select top 1 * from request_flow_sub "
        vSql += "            where request_id = '" + vRequest_id + "' "
        vSql += "            and flow_complete = 0 "
        vSql += "            and disable = 0 "
        vSql += "            order by flow_step desc, flow_sub_step desc "
        vSql += "        ) ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Next_Flow(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select top 1 * from ( "
        vSql += "    select request_id, flow_id "
        vSql += "    , flow_step, 0 flow_sub_step, next_step, flow_complete "
        vSql += "    from request_flow "
        vSql += "    where request_id = '" + vRequest_id + "' "
        vSql += "    and flow_complete = 0 "
        vSql += "    and next_step <> '-' "
        vSql += "    and next_step <> 'end' "
        vSql += "    and disable = 0 "
        vSql += "    union "
        vSql += "    select request_id, flow_id "
        vSql += "    , flow_step, flow_sub_step, next_step, flow_complete "
        vSql += "    from request_flow_sub "
        vSql += "    where request_id = '" + vRequest_id + "' "
        vSql += "    and flow_complete = 0 "
        vSql += "    and next_step <> '-' "
        vSql += "    and next_step <> 'end' "
        vSql += "    and disable = 0 "
        vSql += ") rf_union "
        vSql += "order by flow_step, flow_sub_step "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Try
            If vDT.Rows(0).Item("flow_sub_step") = 0 Then
                vSql = loadDT_Next_Flow_Main(vRequest_id, vDT.Rows(0).Item("flow_step"))
            Else
                vSql = loadDT_Next_Flow_Sub(vRequest_id)
            End If

            Return DB105.GetDataTable(vSql)
        Catch ex As Exception
            CP.InteruptRefresh()
        End Try
    End Function

    Public Function loadDT_Next_Flow_Main(ByVal vRequest_id As String, ByVal vNext_step As String) As String
        Dim vSql As String = ""
        'vSql = "select project_name ,subject_name, subject_url, request_title, send_uemail, next_depart_id "
        'vSql += "from request rq "
        'vSql += "join subject on  subject.subject_id = rq.subject_id "
        'vSql += "join project on  project.project_id = subject.project_id "
        'vSql += "join ( "
        vSql += "    SELECT rm.Document_No 'request_id', send_uemail = STUFF(( " + vbCr
        vSql += "        SELECT ';' + ru.send_uemail " + vbCr
        vSql += "        FROM request_flow ru " + vbCr
        vSql += "        WHERE rm.Document_No = ru.request_id " + vbCr
        vSql += "        and ru.flow_step = '" + vNext_step + "' "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM FeasibilityDocument rm "
        vSql += "    where rm.Document_No = '" + vRequest_id + "' "
        'vSql += ") rf "
        'vSql += "on rf.request_id = rq.request_id "

        '***** เอา department ถัดไปเพื่อไป stamp ที่ request (next_depart) 
        'vSql += "left join ( "
        'vSql += "    select request_id, depart_id next_depart_id from request_flow "
        'vSql += "    where request_id = '" + vRequest_id + "' and flow_step = '" + vNext_step + "' "
        'vSql += "    and disable = 0 "
        'vSql += ") nx "
        'vSql += "on nx.request_id = rq.request_id "
        '***** เอา department ถัดไปเพื่อไป stamp ที่ request (next_depart)

        'vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return vSql
    End Function

    Public Function loadDT_Next_Flow_Sub(ByVal vRequest_id As String) As String
        Dim vSql As String
        vSql = "declare @temp table (request_id varchar(20), send_uemail varchar(255), next_depart_id int) "
        vSql += "insert into @temp select top 1 request_id, send_uemail, depart_id next_depart_id "
        vSql += "from request_flow_sub  "
        vSql += "where request_id = '" + vRequest_id + "' and flow_complete = 0 "
        vSql += "and disable = 0 "
        vSql += "order by flow_step, flow_sub_step "

        vSql += "select project_name, subject_name, subject_url, request_title, rf.send_uemail, next_depart_id "
        vSql += "from request rq "
        vSql += "join subject on  subject.subject_id = rq.subject_id "
        vSql += "join project on  project.project_id = subject.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM ( "
        vSql += "            select * from @temp "
        vSql += "        ) ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "

        '***** เอา department ถัดไปเพื่อไป stamp ที่ request (next_depart)
        vSql += "left join @temp next_sub "
        vSql += "on next_sub.request_id = rq.request_id "
        '***** เอา department ถัดไปเพื่อไป stamp ที่ request (next_depart)

        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return vSql
    End Function

    Public Function declareDX01(ByVal dx01 As String)
        If dx01.Trim() = "" Then
            Return "DECLARE @dx01 DATETIME = NULL "
        Else
            Return "DECLARE @dx01 DATETIME = CONVERT(DATE,'" + dx01 + "',103) "
        End If
    End Function

    Public Function declareDX02(ByVal dx02 As String)
        If dx02.Trim() = "" Then
            Return "DECLARE @dx02 DATETIME = NULL "
        Else
            Return "DECLARE @dx02 DATETIME = CONVERT(DATE,'" + dx02 + "',103) "
        End If
    End Function

    Public Function declareDX03(ByVal dx03 As String)
        If dx03.Trim() = "" Then
            Return "DECLARE @dx03 DATETIME = NULL "
        Else
            Return "DECLARE @dx03 DATETIME = CONVERT(DATE,'" + dx03 + "',103) "
        End If
    End Function

#End Region

End Class

