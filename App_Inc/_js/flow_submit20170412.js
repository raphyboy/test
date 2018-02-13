
$('#btn_flow_submit').click(function() {
	$('#hide_flow_no').val($('#flow_no').val());
	$('#hide_flow_sub').val($('#flow_sub').val());
	$('#hide_next_step').val($('#next_step').val());
	$('#hide_department').val($('#department').val());
	$('#hide_flow_status').val($('#sel_flow_status').val());
	$('#hide_flow_remark').val($('#txt_flow_remark').val());

	// if(checkFlowSubmit() != 0){
	// 	$('#btn_flow_hidden').click();
	// }

	var $el1 = $("#sel_flow_status");

	if ($el1.val() == 30 && $.trim($('#txt_flow_remark').val()).length == 0) {
		var txt = "กรุณากรอกหมายเหตุที่ต้องการข้อมูลเพิ่ม"
		modalAlert(txt);
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$('#txt_flow_remark').focus();
		})
	}
	else if ($.trim($el1.val()) != "" && $el1.val() != 50 && $el1.val() != 60) {
		// ถ้าเลือกสถานะอื่นที่ไม่ใช่ อนุมัติ และดำเนินการเรียบร้อย สามารถ submit ได้เลย
		$('#btn_flow_hidden').click();
	}
	else if (!checkSubmit('required-f')) {
		var txt = "หากต้องการอนุมัติ หรือดำเนินการเรียบร้อย <br>กรุณากรอกข้อมูลช่องที่มีเครื่องหมาย <span class='txt-red'>*</span> <br><b>ในฟอร์มอนุมัติให้ครบ เพื่อบันทึก</b>"
		modalAlert(txt);
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$('.error:first').focus();
		})
	}
	else{
		$('#btn_flow_hidden').click();
	}
});

$('#btn_add_next_submit').click(function() {
	$('#hide_flow_no').val($('#flow_no').val());
	$('#hide_flow_sub').val($('#flow_sub').val());
	$('#hide_depart_id').val($('#sel_depart_id').val());

	// if(checkAddNext() != 0){
	// 	$('#btn_add_next_hidden').click();
	// }
	if (!checkSubmit('required-n')) {
		modalAlert("กรุณาเลือกส่วนงาน เพื่อแทรกลำดับถัดไป");
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$('.error:first').focus();
		})
	}
	else{
		$('#btn_add_next_hidden').click();
	}
});

function checkFlowSubmit() {
	var $el1 = $("#sel_flow_status");

	if( $.trim($el1.val()) == ""){
		modalAlert("กรุณาเลือกสถานะ");
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$el1.focus();
		})
		return false;
	}
	else {
		return true;
	}
}

function checkAddNext() {
	var $el1 = $("#sel_depart_id");

	if( $.trim($el1.val()) == ""){
		modalAlert("กรุณาเลือกส่วนงาน");
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$el1.focus();
		})
		return 0;
	}
	else {
		return 1;
	}
}