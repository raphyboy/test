
$('#btn_submit').click(function() {
	if (!checkSubmit('required')) { // ถ้าช่อง required มีค่าว่าง
		modalAlert("กรุณากรอกข้อมูลให้ครบถ้วน");
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$('.error:first').focus();
		})
	}
	else{ // บันทึก
		$('input[xd="hide_redebt_cause"]').val($('select[xd="sel_cause"]').val());
		$('input[xd="btn_submit_hidden"]').click();
	}
});