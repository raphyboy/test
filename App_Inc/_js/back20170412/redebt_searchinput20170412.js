
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
					$('#txt_account_number').val($('#txt_search').val());
					$('#txt_account_name').val(acc_name);

					$('#txt_account_number').addClass("txt-blue-highlight");
					$('#txt_account_name').addClass("txt-blue-highlight");

					$('#txt_account_number').removeClass("error");
					$('#txt_account_name').removeClass("error");

					count_acc_RQclose($('#txt_account_number').val());
					count_acc_RQprocess($('#txt_account_number').val());
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

$('#btn_doc_num_search').click(function() {
    	var url = "json_redebt.aspx?qrs=searchBill&doc_number=" + $('#txt_search').val();
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
					$('#txt_doc_number').val(data[0].doc_number);
					$('#txt_account_number').val(data[0].acc_number);
					$('#txt_account_name').val(data[0].acc_name);
					// $('#txt_amount').val(data[0].amount);
					$('#span_amount').html("ยอดตามใบเสร็จ <span class='txt-blue-highlight'>" + data[0].amount + "</span> บาท");
					$('#txt_amount').val("");
					$('#txt_dx01').val(data[0].bill_date);
					$('#sel_area_ro').val(data[0].ro);

					$('#txt_doc_number').addClass("txt-blue-highlight");
					$('#txt_account_number').addClass("txt-blue-highlight");
					$('#txt_account_name').addClass("txt-blue-highlight");
					// $('#txt_amount').addClass("txt-blue-highlight");
					$('#txt_dx01').addClass("txt-blue-highlight");
					$('#sel_area_ro').addClass("txt-blue-highlight");

					$('#txt_doc_number').removeClass("error");
					$('#txt_account_number').removeClass("error");
					$('#txt_account_name').removeClass("error");
					$('#txt_amount').removeClass("error");
					$('#txt_dx01').removeClass("error");
					$('#sel_area_ro').removeClass("error");

					count_acc_RQclose($('#txt_account_number').val());
					count_acc_RQprocess($('#txt_account_number').val());
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
    	var url = "json_redebt.aspx?qrs=searchAccount&account_number=" + $('#txt_account_number_to').val();
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
					$('#txt_account_name_to').val(acc_name);

					$('#txt_account_number_to').addClass("txt-blue-highlight");
					$('#txt_account_name_to').addClass("txt-blue-highlight");

					$('#txt_account_number_to').removeClass("error");
					$('#txt_account_name_to').removeClass("error");
				}
				else {
					$('#dis_search_2').html($('#txt_account_number_to').val());
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

$('#txt_account_number').focusout(function() {
	count_acc_RQclose($('#txt_account_number').val());
	count_acc_RQprocess($('#txt_account_number').val());
});

/********************* auto count acc *********************/



/********************* load Cause สาเหตุ *********************/
function loadCause(request_title_id, redebt_cause_id = ""){
	if (request_title_id == null)
		request_title_id = "";
	
	var $el = $('#sel_cause');
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

$('#sel_title').change(function() {
	var $el = $('#sel_title');

	if($(this).val().trim().length > 0){
		loadCause($el.val());
	}
});
/********************* load Cause สาเหตุ *********************/
