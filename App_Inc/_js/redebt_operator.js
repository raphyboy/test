
$('#add_cc').click(function(){
	if($('.form-cc1').css('display') == 'none'){
		$('.form-cc1').show();
	}
	else if($('.form-cc2').css('display') == 'none'){
		$('.form-cc2').show();
	}
	else {
		modalAlert('ผู้รับผิดชอบร่วม ได้สูงสุด 2 คน');
	}
});

$('#remove_cc1').click(function(){
	$('.form-cc1').hide();
	$('input[xd="txt_uemail_cc1"]').val("");
});

$('#remove_cc2').click(function(){
	$('.form-cc2').hide();
	$('input[xd="txt_uemail_cc2"]').val("");
});

$('#copy_same').click(function(){
	if($('#txt_uemail_verify').val().trim().length > 0){
		$('#txt_uemail_approve').val($('#txt_uemail_verify').val());
		$('input[xd="hide_uemail_approve"]').val($('input[xd="hide_uemail_verify"]').val());
		$('#txt_desc_approve1').html($('#txt_desc_approve2').text());

		$('#copy_same').hide();
		$('#uncopy_same').show();
	}
	else {
		modalAlert('กรุณาระบุ"ผู้ตรวจสอบ"ก่อน');
		$('#modal_alert').on('hidden.bs.modal', function (e) {
			$('#auto_emp2').focus();
		})
	}
});

$('#uncopy_same').click(function(){
	$('#txt_uemail_approve').val("");
	$('input[xd="hide_uemail_approve"]').val("");
	$('#txt_desc_approve1').html("");

	$('#copy_same').show();
	$('#uncopy_same').hide();
});



function source_autocomplete(request, response) {
	var url = "json_redebt.aspx?qrs=autoEmp&kw=" + request.term + "&token=" + $('input[xd="hide_token"]').val();
	console.log(url)
	$.ajax({
		url: url,
		cache: false,
		dataType: "json",
		success: function( data ) {
			response( $.map( data, function( item ) {
				if(item.accountStatus == true && item.dateExpired == null && (
					item.position == "ผู้จัดการเขต" || item.position == "หัวหน้าหน่วยงาน" || 
					item.position == "ผู้อำนวยการภาค" || item.position == "ผู้อำนวยการ" || 
					(item.position == "ผู้จัดการส่วน" && item.department == "ภาคตะวันตก (RO6)") ||
					(item.position == "Manager" && item.section == "Operation Support") ||
					item.position == "group email" || item.department == "departadmin"
					)){
					return {
						desc: item.thaiFullname + " / " + item.position + " / " + item.department,
						email: item.email,
						label: item.thaiFullname + " / " + item.position + " / " + item.department + " / " + item.email ,
						value: item.email.replace("@jasmine.com", "")
					}
				}

				// if(request.term == "panu123"){
				// 	return {
				// 		desc: "ภาณุพงศ์ พันธ์เวช / position / department",
				// 		email: "panupong.pa@jasmine.com",
				// 		label: "ภาณุพงศ์ พันธ์เวช / position / department / panupong.pa@jasmine.com" ,
				// 		value: "panupong.pa"
				// 	}
				// }
			}));
		},
		error: function() {
			console.log("autocomplete fail!!");
			$('#page_loading').fadeOut();
		}
	});
}

$('#auto_emp1').autocomplete({
	minLength: 2,
	focus: function(event, ui) {
		event.preventDefault();
		$("#auto_emp1-search").val(ui.item.label);
	},
	source: function( request, response ) {
		source_autocomplete(request, response);
	}
});

$('#auto_emp2').autocomplete({
	minLength: 2,
	focus: function(event, ui) {
		event.preventDefault();
		$("#auto_emp2-search").val(ui.item.label);
	},
	source: function( request, response ) {
		source_autocomplete(request, response);
	}
});

$('#auto_emp1').on('autocompleteselect', function (e, ui) {
	$('#auto_emp1').val("");
	$('input[xd="hide_uemail_approve"]').val(ui.item.value);
	$('#txt_uemail_approve').val(ui.item.email);
	$('#txt_desc_approve1').html(ui.item.desc);
});

$('#auto_emp2').on('autocompleteselect', function (e, ui) {
	$('#auto_emp2').val("");
	$('input[xd="hide_uemail_verify"]').val(ui.item.value);
	$('#txt_uemail_verify').val(ui.item.email);
	$('#txt_desc_approve2').html(ui.item.desc);
});

$('#auto_emp1').focusout(function() {
	$('#auto_emp1').val("");
});

$('#auto_emp2').focusout(function() {
	$('#auto_emp2').val("");
});

function loadDescApprove(){
	var url = "json_redebt.aspx?qrs=autoEmp&kw=" + $('input[xd="txt_uemail_approve"]').val() + "@jasmine.com&token=" + $('input[xd="hide_token"]').val();
	console.log(url);

	$.ajax({
		url: url,
		cache: false,
		dataType: "json",
		timeout: 120000,
		success: function( data ) { 
			if(data[0].nodata == null){
				var desc_approve = data[0].thaiFullname + " / " + data[0].position + " / " + data[0].department;
				$('.txt-desc-approve').html(desc_approve);
			}
		},
		error: function(x, t, m) {
			console.log('ajax error /n x>' + x + ' t>' + t + ' m>' + m);

			modalAlert("ไม่สำเร็จ กรุณาลองอีกครั้ง หรือติดต่อ support_pos@jasmine.com");
			$('#modal_alert').on('hidden.bs.modal', function (e) {
				location.reload();
			})
		}
	});
}

function loadDescVerify(){
	var url = "json_redebt.aspx?qrs=autoEmp&kw=" + $('input[xd="txt_uemail_verify"]').val() + "@jasmine.com&token=" + $('input[xd="hide_token"]').val();
	console.log(url);

	$.ajax({
		url: url,
		cache: false,
		dataType: "json",
		timeout: 120000,
		success: function( data ) { 
			if(data[0].nodata == null){
				var desc_verify = data[0].thaiFullname + " / " + data[0].position + " / " + data[0].department;
				$('.txt-desc-verify').html(desc_verify);
			}
		},
		error: function(x, t, m) {
			console.log('ajax error /n x>' + x + ' t>' + t + ' m>' + m);

			modalAlert("ไม่สำเร็จ กรุณาลองอีกครั้ง หรือติดต่อ support_pos@jasmine.com");
			$('#modal_alert').on('hidden.bs.modal', function (e) {
				location.reload();
			})
		}
	});
}