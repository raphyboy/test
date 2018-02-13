function checkSubmit(rq) {
	var isValid = true;

	$("."+rq+" input, ."+rq+" select").each(function(){
		if(!checkRequired($(this))) {
			isValid = false;
		}
	});

	return isValid;
}

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
	if($el.val() == null){
		console.log("error required -> " + $el.attr('id'));
		$el.addClass("error");
		return false;
	}
	else if($el.val().trim().length > 0) {
		$el.removeClass("error");
		return true;
	}
	else {
		console.log("error required -> " + $el.attr('id'));
		$el.addClass("error");
		return false;
	}
}