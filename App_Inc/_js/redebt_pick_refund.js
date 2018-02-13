
$('input[name="rad_refund"]').click(function(){
	setRefund();
});

function setRefund() {
	if($('#rad_refund1').prop("checked")){
		$('input[xd="txt_mx02"]').val("null");
		$('input[xd="txt_account_number_to"]').val("");
		$('input[xd="txt_account_name_to"]').val("");
		
		$('.refund2').hide();
		$('.refund1').show();
	}
	else if($('#rad_refund2').prop("checked")){
		$('input[xd="txt_account_number_to"]').val("null");
		$('input[xd="txt_account_name_to"]').val("null");
		$('input[xd="txt_mx02"]').val("");

		$('.refund1').hide();
		$('.refund2').show();
	}
}

function checkRefund() {
	if($('input[xd="txt_mx02"]').val().trim().length == 0) {
		$('#rad_refund1').prop("checked", true);
		$('input[xd="txt_mx02"]').val("null");
		
		$('.refund2').hide();
		$('.refund1').show();
	}
	else {
		$('#rad_refund2').prop("checked", true);
		$('input[xd="txt_account_number_to"]').val("null");
		$('input[xd="txt_account_name_to"]').val("null");

		$('.refund1').hide();
		$('.refund2').show();
	}
}