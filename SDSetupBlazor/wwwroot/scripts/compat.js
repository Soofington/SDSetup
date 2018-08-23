﻿
function checkIE() {
	if (getBrowserCompatInfo() == 2) {
		$('body').replaceWith(
			'<div class="ui active dimmer" style="height:100%;width:100%">' +
			'<img class="ui small image" src="/img/fail.png">' +
			'<h3 style="color:#ffffff;margin-top:1.5em">Unfortunately, Internet Explorer is not compatible with this site.</h3>' +
			'<h6 style="color:#ffffff">(You should seriously consider using a better browser like Firefox or Chrome)</h6>' +
			'</div>'
		);
		return;
	}
}

function getBrowserCompatInfo() {
	if (window.navigator.userAgent.indexOf("Edge") > -1) {
		return 1
	}
	if (window.navigator.userAgent.indexOf('Trident/') > -1) {
		return 2
	}
	if (window.navigator.userAgent.indexOf('MSIE ') > -1) {
		return 2
	}
	if (window.navigator.userAgent.indexOf('Firefox') > -1) {
		return 3;
	}
	return 0;
}