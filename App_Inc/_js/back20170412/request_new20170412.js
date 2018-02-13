
$('#sel_subject').change(function() {
	var page = "new" + $('#sel_subject').val() + ".aspx";
	window.location.href = page;
});

$('#btn_submit').click(function() {
	// if($('#hide_uemail').val() == $('#txt_uemail_approve').val()){ // ถ้ากรอกชื่อผู้อนุมัติเป็นตัวเอง ไม่ได้
	// 	modalAlert("ตรวจสอบอีเมล์ ผู้ดูแลที่มีสิทธิ์อนุมัติ");
	// 	$('#modal_alert').on('hidden.bs.modal', function (e) {
	// 		$('#txt_uemail_approve').focus();
	// 	})
	// }
	// else 
	if (!checkSubmit('required')) { // ถ้าช่อง required มีค่าว่าง
		modalAlert("กรุณากรอกข้อมูลให้ครบถ้วน");
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$('.error:first').focus();
		})
	}
	else{ // บันทึก
		$('#hide_redebt_cause').val($('#sel_cause').val());
		$('#btn_submit_hidden').click();
	}
});