
$('#btn_flow_submit').click(function() {
	$('input[xd="hide_flow_no"]').val($('#flow_no').val());
	$('input[xd="hide_flow_sub"]').val($('#flow_sub').val());
	$('input[xd="hide_next_step"]').val($('#next_step').val());
	$('input[xd="hide_back_step"]').val($('#back_step').val());
	$('input[xd="hide_department"]').val($('#department').val());
	$('input[xd="hide_flow_status"]').val($('#sel_flow_status').val());
	$('input[xd="hide_flow_remark"]').val($('#txt_flow_remark').val());

	var $el1 = $("#sel_flow_status");

	if ($el1.val() == 30 && $.trim($('#txt_flow_remark').val()).length == 0) {
		var txt = "กรุณากรอกหมายเหตุที่ต้องการข้อมูลเพิ่ม"
		modalAlert(txt);
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$('#txt_flow_remark').focus();
		})
	}
	else if ( ($el1.val() == 55 || $el1.val() == 65) && $.trim($('#txt_flow_remark').val()).length == 0) {
		var txt = "กรุณากรอกหมายเหตุที่ไม่อนุมัติ/ไม่ดำเนินการ"
		modalAlert(txt);
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$('#txt_flow_remark').focus();
		})
	}
	else if ($.trim($el1.val()) != "" && $el1.val() != 50 && $el1.val() != 60) {
		// ถ้าเลือกสถานะอื่นที่ไม่ใช่ อนุมัติ และดำเนินการเรียบร้อย สามารถ submit ได้เลย
		$('input[xd="btn_flow_hidden"]').click();
	}
	else if (!checkSubmit('required-f')) {
		var txt = "หากต้องการอนุมัติ หรือดำเนินการเรียบร้อย <br>กรุณากรอกข้อมูลช่องที่มีเครื่องหมาย <span class='txt-red'>*</span> <br><b>ในฟอร์มอนุมัติให้ครบ เพื่อบันทึก</b>"
		modalAlert(txt);
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$('.error:first').focus();
		})
	}
	else{
		$('input[xd="btn_flow_hidden"]').click();
	}
});

$('#btn_add_next_submit').click(function() {
	$('input[xd="hide_flow_no"]').val($('#flow_no').val());
	$('input[xd="hide_flow_sub"]').val($('#flow_sub').val());
	$('input[xd="hide_depart_id"]').val($('#sel_depart_id').val());

	if (!checkSubmit('required-n')) {
		modalAlert("กรุณาเลือกส่วนงาน เพื่อแทรกลำดับถัดไป");
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$('.error:first').focus();
		})
	}
	else{
		$('input[xd="btn_add_next_hidden"]').click();
	}
});
