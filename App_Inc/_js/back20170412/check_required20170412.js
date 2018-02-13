function checkSubmit(rq) {
	var isValid = true;

	// $(".required input, .required select").each(function(){
	$("."+rq+" input, ."+rq+" select").each(function(){
		if(!checkRequired($(this))) {
			isValid = false;
		}
	});

	return isValid;
}

$('#sel_title').change(function() {
	var $el = $('#txt_request_title');

	$el.val($('#sel_title option:selected').text());

	if($(this).val().trim().length > 0)
		$el.removeClass("error");
	else
		$el.addClass("error");
});

$('input[name="rad_search"]').click(function(){
	$('#search0').hide();

	if($('#rad_search1').prop("checked")){
		$('#txt_search').attr("placeholder", "109235252");
		$('#btn_doc_num_search').hide();
		$('#btn_account_search').show();
	}
	else if($('#rad_search2').prop("checked")){
		$('#txt_search').attr("placeholder", "DOTCV05SPN51/1608/0017");
		$('#btn_account_search').hide();
		$('#btn_doc_num_search').show();
	}
});

$('.required input, .required select').focusout(function() {
	checkRequired($(this))
});

$('.required-f input, .required-f select').focusout(function() {
	checkRequired($(this))
});

$('.required-n input, .required-n select').focusout(function() {
	checkRequired($(this))
});

function checkRequired($el) {
	if($el.val().trim().length > 0) {
		$el.removeClass("error");
		return true;
	}
	else {
		console.log("error required -> " + $el.attr('id'));
		$el.addClass("error");
		return false;
	}
}