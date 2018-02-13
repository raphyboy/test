
$('input[name="rad_search"]').click(function(){
	$('#search0').hide();

	if($('#rad_search1').prop("checked")){
		$('#txt_search').attr("placeholder", "670148072");
		$('#btn_account_search').show();
		$('#btn_doc_num_search').hide();
		$('#btn_bcs_num_search').hide();
	}
	else if($('#rad_search2').prop("checked")){
		$('#txt_search').attr("placeholder", "DOTCV05PYO51/1703/0285");
		$('#btn_account_search').hide();
		$('#btn_doc_num_search').show();
		$('#btn_bcs_num_search').hide();
	}
	else if($('#rad_search3').prop("checked")){
		$('#txt_search').attr("placeholder", "4-BS-PYOM1-201703-0000020");
		$('#btn_doc_num_search').hide();
		$('#btn_account_search').hide();
		$('#btn_bcs_num_search').show();
	}
});

/********************* auto search input *********************/

$('.auto-sch, .auto-sch_2').focusout(function() {
	$('.auto-sch').removeClass("txt-blue-highlight");
	$('.auto-sch_2').removeClass("txt-blue-highlight");
});

$('#btn_account_search').click(function() {
    	var url = "json_redebt.aspx?qrs=searchAccount&account_number=" + $('#txt_search').val();
    	console.log(url);

    	$('.auto-sch').removeClass("txt-blue-highlight");
    	$('#search0').hide();

    	$.ajax({
			url: url,
			cache: false,
			dataType: "json",
			timeout: 120000,
			success: function( data ) { 
				if(data.length > 0) {
					var acc_name = data[0].prefix_name + " " + data[0].first_name + " " + data[0].last_name
					$('input[xd="txt_account_number"]').val($('#txt_search').val());
					$('input[xd="txt_account_name"]').val(acc_name);

					$('input[xd="txt_account_number"]').addClass("txt-blue-highlight");
					$('input[xd="txt_account_name"]').addClass("txt-blue-highlight");

					$('input[xd="txt_account_number"]').removeClass("error");
					$('input[xd="txt_account_name"]').removeClass("error");

					count_acc_RQclose($('input[xd="txt_account_number"]').val());
					count_acc_RQprocess($('input[xd="txt_account_number"]').val());
				}
				else {
					$('#dis_search').html($('#txt_search').val());
					$('#search0').fadeIn();
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
});

$('#btn_account_to_search').click(function() {
    	var url = "json_redebt.aspx?qrs=searchAccount&account_number=" + $('input[xd="txt_account_number_to"]').val();
    	console.log(url);

    	$('.auto-sch_2').removeClass("txt-blue-highlight");
    	$('#search0_2').hide();

    	$.ajax({
			url: url,
			cache: false,
			dataType: "json",
			timeout: 120000,
			success: function( data ) { 
				if(data.length > 0) {
					var acc_name = data[0].prefix_name + " " + data[0].first_name + " " + data[0].last_name
					$('input[xd="txt_account_name_to"]').val(acc_name);

					$('input[xd="txt_account_number_to"]').addClass("txt-blue-highlight");
					$('input[xd="txt_account_name_to"]').addClass("txt-blue-highlight");

					$('input[xd="txt_account_number_to"]').removeClass("error");
					$('input[xd="txt_account_name_to"]').removeClass("error");
				}
				else {
					$('#dis_search_2').html($('input[xd="txt_account_number_to"]').val());
					$('#search0_2').fadeIn();
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
});

// $('#btn_doc_num_search').click(function() {
//     	var url = "json_redebt.aspx?qrs=searchBill&doc_number=" + $('#txt_search').val();
//     	console.log(url);

//     	$('.auto-sch').removeClass("txt-blue-highlight");
//     	$('#search0').hide();

//     	$.ajax({
// 			url: url,
// 			cache: false,
// 			dataType: "json",
// 			timeout: 120000,
// 			success: function( data ) { 
// 				if(data.length > 0) {
// 					$('input[xd="txt_doc_number"]').val(data[0].doc_number);
// 					$('input[xd="txt_account_number"]').val(data[0].acc_number);
// 					$('input[xd="txt_account_name"]').val(data[0].acc_name);
// 					$('#span_amount').html("ยอดตามใบเสร็จ <span class='txt-blue-highlight'>" + data[0].amount + "</span> บาท");
// 					$('input[xd="txt_amount"]').val("");
// 					$('input[xd="txt_dx01"]').val(data[0].bill_date);
// 					$('select[xd="sel_area_ro"]').val(data[0].ro);

// 					$('input[xd="txt_doc_number"]').addClass("txt-blue-highlight");
// 					$('input[xd="txt_account_number"]').addClass("txt-blue-highlight");
// 					$('input[xd="txt_account_name"]').addClass("txt-blue-highlight");
// 					$('input[xd="txt_dx01"]').addClass("txt-blue-highlight");
// 					$('select[xd="sel_area_ro"]').addClass("txt-blue-highlight");

// 					$('input[xd="txt_doc_number"]').removeClass("error");
// 					$('input[xd="txt_account_number"]').removeClass("error");
// 					$('input[xd="txt_account_name"]').removeClass("error");
// 					$('input[xd="txt_amount"]').removeClass("error");
// 					$('input[xd="txt_dx01"]').removeClass("error");
// 					$('select[xd="sel_area_ro"]').removeClass("error");

// 					count_acc_RQclose($('input[xd="txt_account_number"]').val());
// 					count_acc_RQprocess($('input[xd="txt_account_number"]').val());
// 				}
// 				else {
// 					$('#dis_search').html($('#txt_search').val());
// 					$('#search0').fadeIn();
// 				}
// 			},
// 			error: function(x, t, m) {
// 				console.log('ajax error /n x>' + x + ' t>' + t + ' m>' + m);

// 				modalAlert("ไม่สำเร็จ กรุณาลองอีกครั้ง หรือติดต่อ support_pos@jasmine.com");
// 				$('#modal_alert').on('hidden.bs.modal', function (e) {
// 					location.reload();
// 				})
// 			}
// 		});
// });

$('#btn_doc_num_search').click(function() {
    	var url = "json_redebt.aspx?qrs=getRedebt&pos_receipt=" + $('#txt_search').val();
    	console.log(url);

    	$('.auto-sch').removeClass("txt-blue-highlight");
    	$('#search0').hide();

    	$.ajax({
			url: url,
			cache: false,
			dataType: "json",
			timeout: 120000,
			success: function( data ) { 
				if(data.length > 0) {
					$('input[xd="txt_doc_number"]').val(data[0].pos_receipt_id);
					$('input[xd="txt_bcs_number"]').val(data[0].bcs_receipt_id);
					$('input[xd="txt_account_number"]').val(data[0].account_num);
					$('input[xd="txt_account_name"]').val(data[0].account_name);
					$('#span_amount').html("ยอดตามใบเสร็จ <span class='txt-blue-highlight'>" + data[0].receipt_amount + "</span> บาท");
					$('input[xd="txt_amount"]').val("");
					$('input[xd="txt_dx01"]').val(data[0].receipt_date);
					$('select[xd="sel_area_ro"]').val(data[0].ro);

					$('input[xd="txt_doc_number"]').addClass("txt-blue-highlight");
					$('input[xd="txt_bcs_number"]').addClass("txt-blue-highlight");
					$('input[xd="txt_account_number"]').addClass("txt-blue-highlight");
					$('input[xd="txt_account_name"]').addClass("txt-blue-highlight");
					$('input[xd="txt_dx01"]').addClass("txt-blue-highlight");
					$('select[xd="sel_area_ro"]').addClass("txt-blue-highlight");

					$('input[xd="txt_doc_number"]').removeClass("error");
					$('input[xd="txt_bcs_number"]').removeClass("error");
					$('input[xd="txt_account_number"]').removeClass("error");
					$('input[xd="txt_account_name"]').removeClass("error");
					$('input[xd="txt_amount"]').removeClass("error");
					$('input[xd="txt_dx01"]').removeClass("error");
					$('select[xd="sel_area_ro"]').removeClass("error");

					count_acc_RQclose($('input[xd="txt_account_number"]').val());
					count_acc_RQprocess($('input[xd="txt_account_number"]').val());
				}
				else {
					$('#dis_search').html($('#txt_search').val());
					$('#search0').fadeIn();
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
});

$('#btn_bcs_num_search').click(function() {
    	var url = "json_redebt.aspx?qrs=getRedebt&bcs_receipt=" + $('#txt_search').val();
    	console.log(url);

    	$('.auto-sch').removeClass("txt-blue-highlight");
    	$('#search0').hide();

    	$.ajax({
			url: url,
			cache: false,
			dataType: "json",
			timeout: 120000,
			success: function( data ) { 
				if(data.length > 0) {
					$('input[xd="txt_doc_number"]').val(data[0].pos_receipt_id);
					$('input[xd="txt_bcs_number"]').val(data[0].bcs_receipt_id);
					$('input[xd="txt_account_number"]').val(data[0].account_num);
					$('input[xd="txt_account_name"]').val(data[0].account_name);
					$('#span_amount').html("ยอดตามใบเสร็จ <span class='txt-blue-highlight'>" + data[0].receipt_amount + "</span> บาท");
					$('input[xd="txt_amount"]').val("");
					$('input[xd="txt_dx01"]').val(data[0].receipt_date);
					$('select[xd="sel_area_ro"]').val(data[0].ro);

					$('input[xd="txt_doc_number"]').addClass("txt-blue-highlight");
					$('input[xd="txt_bcs_number"]').addClass("txt-blue-highlight");
					$('input[xd="txt_account_number"]').addClass("txt-blue-highlight");
					$('input[xd="txt_account_name"]').addClass("txt-blue-highlight");
					$('input[xd="txt_dx01"]').addClass("txt-blue-highlight");
					$('select[xd="sel_area_ro"]').addClass("txt-blue-highlight");

					$('input[xd="txt_doc_number"]').removeClass("error");
					$('input[xd="txt_bcs_number"]').removeClass("error");
					$('input[xd="txt_account_number"]').removeClass("error");
					$('input[xd="txt_account_name"]').removeClass("error");
					$('input[xd="txt_amount"]').removeClass("error");
					$('input[xd="txt_dx01"]').removeClass("error");
					$('select[xd="sel_area_ro"]').removeClass("error");

					count_acc_RQclose($('input[xd="txt_account_number"]').val());
					count_acc_RQprocess($('input[xd="txt_account_number"]').val());
				}
				else {
					$('#dis_search').html($('#txt_search').val());
					$('#search0').fadeIn();
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
});

/********************* auto search input *********************/


/********************* auto count acc *********************/

function count_acc_RQclose(account_number){
	var url = "json_redebt.aspx?qrs=count_acc_RQclose&account_number=" + account_number;
	console.log(url);

	$.ajax({
		url: url,
		cache: false,
		timeout: 120000,
		success: function( data ) { 
			$('.count-acc-close').html(data);
			$('.count-acc-close').show();
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

function count_acc_RQprocess(account_number){
	var url = "json_redebt.aspx?qrs=count_acc_RQprocess&account_number=" + account_number + "&request_id=" + _GET('request_id');
	console.log(url);

	$.ajax({
		url: url,
		cache: false,
		timeout: 120000,
		success: function( data ) { 
			$('.count-acc-process').html(data);
			$('.count-acc-process').show();
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

$('input[xd="txt_account_number"]').focusout(function() {
	count_acc_RQclose($('input[xd="txt_account_number"]').val());
	count_acc_RQprocess($('input[xd="txt_account_number"]').val());
});

/********************* auto count acc *********************/



/********************* load Cause สาเหตุ *********************/
jQuery.fn.exists = function(){ return this.length > 0; }

$('select[xd="sel_title"]').change(function() {
	if ($('select[xd="sel_cause"]').exists() && $(this).val().trim().length > 0){
		loadCause($(this).val());
	}
});

function loadCause(request_title_id, redebt_cause_id = ""){
	if (request_title_id == null)
		request_title_id = "";
	
	var $el = $('select[xd="sel_cause"]');
	var $el2 = $('span[xd="inn_redebt_cause"]');
	$el.empty();
	$el.append($("<option></option>")
		.attr("value", "").text("กำลังโหลด"));

	var url = "json_redebt.aspx?qrs=loadCause&request_title_id=" + request_title_id;
	console.log(url);

	$.ajax({
		url: url,
		cache: false,
		dataType: "json",
		timeout: 120000,
		success: function( data ) { 
			$el.empty();
			$el.append($("<option></option>")
				.attr("value", "").text("เลือกสาเหตุ"));

			$.each(data,function( i,item ) {
				$el.append($("<option></option>")
					.attr("value", item.redebt_cause_id).text(item.redebt_cause_title));

				if(item.redebt_cause_id == redebt_cause_id)
					$el2.text(item.redebt_cause_title);

			});

			$el.val(redebt_cause_id);
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
/********************* load Cause สาเหตุ *********************/
