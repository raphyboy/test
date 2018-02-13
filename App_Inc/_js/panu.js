// ############################# auto Session expired #############################
var sessionout = 0;
var sstimeout = null;
var ssStart,ssEnd;

$(document).ready(function() {
	checkSession();
});

function checkSession() {
	if($('#hide_uemail').val() != null) {
		if($('#hide_uemail').val().length > 0){
			ssStart = new Date().getTime();
			console.log("checkSession start");

			sstimeout = setTimeout(function() {

				getSession(function(uemail){
					checkSession();

					ssEnd = (new Date().getTime() - ssStart)/1000;
					console.log(ssEnd + " > " + uemail);

					if(uemail.length == 0)
						sessionExpired();
				});
			}, 1210000);
		}
    }
}

function getSession(handleData){
	var path_url = window.location.pathname.split( '/' );
	var url = window.location.protocol + "//" + window.location.host + "/" + path_url[1] + "/json_default.aspx?qrs=getSession";
	console.log(url);

	$.ajax({
		url: url,
		success: function( res ) {
			handleData(res); 
		}
	});
}

function sessionExpired() {
	sessionout = 1;
	modalAlert("Session expired!!");

	$('#modal_alert').on('hidden.bs.modal', function (e) {
		window.location.replace("default.aspx");
	})
}
// ############################# auto Session expired #############################

function _GET(v) {
	var $_GET = {};

	document.location.search.replace(/\??(?:([^=]+)=([^&]*)&?)/g, function () {
		function decode(s) {
			return decodeURIComponent(s.split("+").join(" "));
		}

		$_GET[decode(arguments[1])] = decode(arguments[2]);
	});

	return $_GET[v];
}

function getHash() {
	if(window.location.hash) {
		return window.location.hash.substring(1);
	} else {
		return ""
	}
}

function getFullDate_EN() {
	var d = new Date();

	var month = d.getMonth()+1;
	var day = d.getDate();

	return (day<10 ? '0' : '') + day + '/' +
	(month<10 ? '0' : '') + month + '/' +
	(d.getFullYear());
}

function getFullDate_TH() {
	var d = new Date();

	var month = d.getMonth()+1;
	var day = d.getDate();

	return (day<10 ? '0' : '') + day + '/' +
	(month<10 ? '0' : '') + month + '/' +
	(d.getFullYear() + 543);
}

function validateDate(dtValue) {
	var dtRegex = new RegExp(/\b\d{1,2}[\/-]\d{1,2}[\/-]\d{4}\b/);
	return dtRegex.test(dtValue);
}

function popupCenter(url, title, w, h) {
	var left = (screen.width/2)-(w/2);
	var top = (screen.height/2)-(h/2);
	top = top/2;
	// var top = 75;
	return window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width='+w+', height='+h+', top='+top+', left='+left);
}

function itemNull(v) {
	if (v == null)
		return "-";
	else if(v.toLowerCase() == "null")
		return "-";
	else if(v.length == 0)
		return "-";
	else if(v.trim() == "")
		return "-";
	else
		return v;
}

function itemBlank(v) {
	if (v == null)
		return "";
	else if(v.toLowerCase() == "null")
		return "";
	else if(v.length == 0)
		return "";
	else if(v.trim() == "")
		return "";
	else
		return v;
}

function checkIsEmpty(v) {
	if (v == null)
		return true;
	else if(v.toLowerCase() == "null")
		return true;
	else if(v.length == 0)
		return true;
	else if(v.trim() == "")
		return true;
	else
		return false;
}

function checkNotEmpty(v) {
	if (v == null)
		return false;
	else if(v.toLowerCase() == "null")
		return false;
	else if(v.length == 0)
		return false;
	else if(v.trim() == "")
		return false;
	else
		return true;
}

function limitStr(str, length) {
	var limit_str = str;

	if(str.length > length){
		limit_str = str.substring(0, length) + "...";
	}

	limit_str = "<span title='" + str + "'>" +  itemNull(limit_str)  + "</span>";

	return limit_str;
}

function limitLink(url, length) {
	var str = itemNull(url);

	if(str != "-") {
		if(str.length > length){
			str = str.substring(0, length) + "...";
		}

		str = "<a href='" + url + "' title='" + url + "' target='_blank'>" + str + "</a>";
	}
	
	return str;
}

function openNewTab(url) {
	var win = window.open(url, '_blank');
	win.focus();
}

function gotoUrl(url) {
	window.location = url;
}

function refineUrl() {
    var reUrl = window.location.protocol + "//" + window.location.host + window.location.pathname;
    window.history.pushState({path:reUrl},'',reUrl);
}

function hashTab(hash){
	$('.navbar-nav a[href="#tab_' + hash + '"]').tab('show');
	window.location.hash=hash;
}

function serverTime(){
	var xmlHttp;
	
    try {
        //FF, Opera, Safari, Chrome
        xmlHttp = new XMLHttpRequest();
    }
    catch (err1) {
        //IE
        try {
            xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
        }
        catch (err2) {
            try {
                xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
            }
            catch (eerr3) {
                //AJAX not supported, use CPU time.
                alert("AJAX not supported");
            }
        }
    }
    // var url = window.location.href.toString();
	var url = window.location.protocol + "//" + window.location.host + window.location.pathname;
	url = url.replace("report.aspx", "blank.html");

    xmlHttp.open('HEAD',url,false);
    xmlHttp.setRequestHeader("Content-Type", "text/html");
    xmlHttp.send('');
    return xmlHttp.getResponseHeader("Date");
}

function randomNum(min, max){
	return Math.floor(Math.random() * max) + min;
}

function setDatePicker() {
	$('.datepicker').datepicker({
		dateFormat: 'dd/mm/yy',  
		dayNamesMin: ['อา', 'จ', 'อ', 'พ', 'พฤ', 'ศ', 'ส'],   
		monthNames: ['มกราคม','กุมภาพันธ์','มีนาคม','เมษายน','พฤษภาคม','มิถุนายน','กรกฎาคม','สิงหาคม','กันยายน','ตุลาคม','พฤศจิกายน','ธันวาคม'],
		beforeShow: function() {
			setTimeout(function(){
				$('.ui-datepicker').css('z-index', 999);
			}, 0);
		}
	});
}