/*
	TXT by HTML5 UP
	html5up.net | @ajlkn
	Free for personal and commercial use under the CCA 3.0 license (html5up.net/license)
*/
class CookieHelper {
	static setCookie(name, value) {
		document.cookie = name + '=' + value + '; Path=/;';
	}
	static getCookie(name) {
		const value = `; ${document.cookie}`;
		const parts = value.split(`; ${name}=`);
		if (parts.length === 2) return parts.pop().split(';').shift();
	}
	static deleteCookie(name) {
		document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
	}

}

class User {
	constructor(username, image) {
		this.username = username;
		this.image = image;
	}
}

class City {
	constructor(id, name) {
		this.id = id;
		this.name = name;
	}
}

class Trip {
	constructor(user, source, destination, price, date, time, arrival, description, needsCard, personsAllowed) {
		this.user = user;
		this.source = source;
		this.destination = destination;
		this.price = price;
		this.date = date;
		this.time = time;
		this.arrival = arrival;
		this.description = description;
		this.needsCard = needsCard;
		this.personsAllowed = personsAllowed;
	}
}

class Chat {
	constructor(from, to, messages) {
		this.from = from;
		this.to = to;
		this.messages = messages;
	}
}

class Message {
	constructor(user, message, date) {
		this.user = user;
		this.message = message;
		this.date = date;
	}
}

var controllers = {
	Page: "",
	SearcherForm: false,
	UserChatSending: false,
	SearchFloatingForm: 0,
	UserChatExpandableForm: -1,
	UserChatActual: 0,
	UserChatCheckerAjax: null,
};

var timers = {
	BufferTime: 10,
	SearchFloatingFormOpen: 1000,
	SearchFloatingFormClose: 1000,
	UserChatExpandableFormOpen: 750,
	UserChatExpandableFormClose: 750,
};
var trips = [];
var chats = [];

(function ($) {

	var $window = $(window),
		$body = $('body'),
		$nav = $('#nav');

	// Breakpoints.
	breakpoints({
		xlarge: ['1281px', '1680px'],
		large: ['981px', '1280px'],
		medium: ['737px', '980px'],
		small: ['361px', '736px'],
		xsmall: [null, '360px']
	});

	// Play initial animations on page load.
	$window.on('load', function () {
		window.setTimeout(function () {
			$body.removeClass('is-preload');
		}, 100);
	});

	// Dropdowns.
	$('#nav > ul').dropotron({
		mode: 'fade',
		noOpenerFade: true,
		speed: 300,
		alignment: 'left'
	});

	// Scrolly
	$('.scrolly').scrolly({
		speed: 1000,
		offset: function () { return $nav.height() - 5; }
	});

	// Nav.

	// Title Bar.
	$(
		'<div id="titleBar">' +
		'<a href="#navPanel" class="toggle"></a>' +
		'<span class="title">TransMail</span>' +
		'</div>'
	)
		.appendTo($body);

	// Panel.
	$(
		'<div id="navPanel">' +
		'<nav>' +
		$('#nav').navList() +
		'</nav>' +
		'</div>'
	)
		.appendTo($body)
		.panel({
			delay: 500,
			hideOnClick: true,
			hideOnSwipe: true,
			resetScroll: true,
			resetForms: true,
			side: 'left',
			target: $body,
			visibleClass: 'navPanel-visible'
		});

})(jQuery);

function notify() {
	var message = CookieHelper.getCookie("ALERT_MESSAGE");
	if (message != null) {
		CookieHelper.deleteCookie("ALERT_MESSAGE");
		while (message.includes("~N")) message = message.replace("~N", "\n");
		while (message.includes("~W")) message = message.replace("~W", " ");
		while (message.includes("~E")) message = message.replace("~E", "!");
		alert(message);
    }	
}

function configure(init) {
	page = window.location.pathname.replace('/', '');
	//Always
	notify()

	document.addEventListener('DOMContentLoaded', function () {
		function stylizing() {

		}

		//General modifications
		if (init) {
			stylizing();
		} else {

		}

		//Specific modifications
		var tmp01, tmp02;
		switch (page) {
			case "":
				if (init) {
					document.getElementById('home').className = 'current';
				} else {

				}
				break;
			case "Searcher":
				if (init) {
					document.getElementById('home').className = 'current';
				} else {

				}
				break;
			case "login":
				if (init) {
					document.getElementById('user').className = 'current';
					changeLoginMenu('logger');
				} else {

				}
				break;
			case "TransUser":
				if (init) {
					document.getElementById('user').className = 'current';
					changeUserMenu();

					//Photo selection
					tmp01 = document.getElementsByName('icon');
					for (var i = 0; i < tmp01.length; i++) {
						if (tmp01[i].checked) {
							tmp02 = tmp01[i].id;
							break;
						}
					}
					if (document.getElementById('male').checked) changeUserGender('male');
					else changeUserGender('female');
					document.getElementById(tmp02).checked = true;
				} else {

				}
				break;
			default:
				break;
		}
	}, false);
}

/**
 * Opens the floating window using the appearing animation.
 * 
 * Opens a floating window with the 'name' passed with parameter and completes all
 * the fields with the 'trip index' passed as paramater. All while using an animation
 * to show the window.
 * 
 * @param {string} name
 * @param {number} index
 */
function openFloatingMenu(name, index) {
	//Appearing animation
	function appear(target) {
		target.animate([
			{ opacity: '0' },
			{ opacity: '1' }
		], {
			duration: timers.SearchFloatingFormOpen,
			fill: "forwards"
		});
	}

	//Check if the floating window is closed
	if (controllers.SearchFloatingForm == 0) {
		//Update controller value
		controllers.SearchFloatingForm = 1;

		//Initialize elements
		var elem = document.getElementById(name);
		var closer = document.getElementById(name + "Closer");

		//Display elements
		elem.style.display = "block";
		closer.style.display = "block";

		//Do animation
		appear(elem);
		appear(closer);

		//Complete the floating window with the trip in the specified index
		document.getElementById("floatingusername").innerHTML = trips[index].user.username;
		document.getElementById("floatingimg").src = trips[index].user.image;
		document.getElementById("floatingfrom").innerHTML = trips[index].source;
		document.getElementById("floatingto").innerHTML = trips[index].destination;
		document.getElementById("floatingdescription").innerHTML = trips[index].description;
		document.getElementById("floatingdeparture").innerHTML = trips[index].date + " - " + trips[index].time + '"';
		document.getElementById("floatingarrival").innerHTML = trips[index].arrival;
		document.getElementById("floatingprice").innerHTML = trips[index].price;
		trips[index].needsCard ? document.getElementById("floatingcard").className = "with fas fa-credit-card" : document.getElementById("floatingcard").className = "without fas fa-credit-card";
		trips[index].personsAllowed ? document.getElementById("floatingpersons").className = "with fas fa-male" : document.getElementById("floatingpersons").className = "without fas fa-male";

		//Wait for animation to end
		setTimeout(function () {
			controllers.SearchFloatingForm = 2;
		}, timers.SearchFloatingFormOpen + timers.BufferTime);
	}
}

/**
 * Closes the floating window using the dissapearing animation.
 *
 * Closes a floating window with the 'name' passed with parameter with a
 * dissapearing animation.
 *
 * @param {string} name
 * @param {number} index
 */
function closeFloatingMenu(name) {
	//Dissapearing animation
	function dissapear(target) {
		target.animate([
			{ opacity: '1' },
			{ opacity: '0' }
		], {
			duration: timers.SearchFloatingFormClose,
			fill: "forwards"
		});
	}

	//Check if the floating window is open
	if (controllers.SearchFloatingForm == 2) {
		//Update controller value
		controllers.SearchFloatingForm = 3;

		//Initialize elements
		var elem = document.getElementById(name);
		var closer = document.getElementById(name + "Closer");

		//Do animation
		dissapear(elem);
		dissapear(closer);

		//Undisplay elements
		setTimeout(function () {
			elem.style.display = "none";
			closer.style.display = "none";
			controllers.SearchFloatingForm = 0;
		}, timers.SearchFloatingFormClose + timers.BufferTime);
    }
}

/**
 * Opens the expandable window using the height animation.
 *
 * Opens a expandable window with the 'name' passed with parameter with a
 * height animation and change the slot named 'notch' passed with parameter with
 * the minimize icon.
 * 
 * @param {string} father
 * @param {string} name
 * @param {string} notch
 */
function openExpandableMenu(father, name, notch) {
	//Shrink animation
	function expand(target) {
		target.animate([
			{ height: '0em' },
			{ height: '25em' }
		], {
			duration: timers.UserChatExpandableFormOpen,
			fill: "forwards"
		});
	}
	//Descend animation
	function ascend(target) {
		target.animate([
			{ bottom: '-0.15em' },
			{ bottom: '24.85em' }
		], {
			duration: timers.UserChatExpandableFormOpen,
			fill: "forwards"
		});
	}
	//Dissapear animation
	function dissapear(target) {
		target.animate([
			{ opacity: '1' },
			{ opacity: '0' }
		], {
			duration: timers.UserChatExpandableFormOpen / 2.1,
			fill: "forwards"
		});
	}
	//Appear animation
	function appear(target) {
		target.animate([
			{ opacity: '0' },
			{ opacity: '1' }
		], {
			duration: timers.UserChatExpandableFormOpen / 2.1,
			fill: "forwards"
		});
	}

	//Check if the floating window is open
	if (controllers.UserChatExpandableForm == -1) {
		document.getElementById(father).style.display = "block";
		controllers.UserChatExpandableForm = 0
    }
	if (controllers.UserChatExpandableForm == 0) {
		//Update controller value
		controllers.UserChatExpandableForm = 1;

		//Initialize elements
		var elem = document.getElementById(name);
		var displayer = document.getElementById(name + "Display");
		var closer = document.getElementById(name + "Closer");
		var notchOpener = document.getElementById(notch + "Opener");
		var notchCloser = document.getElementById(notch + "Minimizer");

		//Set displayer
		displayer.style.height = '25em';
		notchCloser.style.display = "block";

		//Do animation
		dissapear(notchOpener);
		ascend(closer);
		expand(elem);

		//Setup notch timer
		setTimeout(function () {
			appear(notchCloser);
		}, timers.UserChatExpandableFormClose / 2.1);

		//Undisplay elements
		setTimeout(function () {
			notchCloser.style.zIndex = "2";
			notchOpener.style.zIndex = "1";
			controllers.UserChatExpandableForm = 2;
		}, timers.UserChatExpandableFormOpen + timers.BufferTime);
	}
}

/**
 * Minimizes the expandable window using the height animation.
 *
 * Minimizes a expandable window with the 'name' passed with parameter with a
 * height animation and change the slot named 'notch' passed with parameter with
 * the maximize icon.
 *
 * @param {string} name
 * @param {string} notch
 */
function minimizeExpandableMenu(name, notch) {
	//Shrink animation
	function shrink(target) {
		target.animate([
			{ height: '25em' },
			{ height: '0em' }
		], {
			duration: timers.UserChatExpandableFormClose,
			fill: "forwards"
		});
	}
	//Descend animation
	function descend(target) {
		target.animate([
			{ bottom: '24.85em' },
			{ bottom: '-0.15em' }
		], {
			duration: timers.UserChatExpandableFormClose,
			fill: "forwards"
		});
	}
	//Dissapear animation
	function dissapear(target) {
		target.animate([
			{ opacity: '1' },
			{ opacity: '0' }
		], {
			duration: timers.UserChatExpandableFormClose / 2.1,
			fill: "forwards"
		});
	}
	//Appear animation
	function appear(target) {
		target.animate([
			{ opacity: '0' },
			{ opacity: '1' }
		], {
			duration: timers.UserChatExpandableFormClose / 2.1,
			fill: "forwards"
		});
	}

	//Check if the floating window is open
	if (controllers.UserChatExpandableForm == 2) {
		//Update controller value
		controllers.UserChatExpandableForm = 3;

		//Initialize elements
		var elem = document.getElementById(name);
		var closer = document.getElementById(name + "Closer");
		var notchOpener = document.getElementById(notch + "Opener");
		var notchCloser = document.getElementById(notch + "Minimizer");

		//Do animation
		dissapear(notchCloser);
		descend(closer);
		shrink(elem);

		//Setup notch timer
		setTimeout(function () {
			appear(notchOpener);
		}, timers.UserChatExpandableFormClose / 2.1);

		//Finish function
		setTimeout(function () {
			notchCloser.style.zIndex = "1";
			notchOpener.style.zIndex = "2";
			controllers.UserChatExpandableForm = 0;
		}, timers.UserChatExpandableFormClose + timers.BufferTime);
	}
}

/**
 * Closes the expandable and hides it's layout.
 *
 * Closes and hides a expandable window with the 'name' passed with parameter.
 *
 * @param {string} father
 */
function closeExpandableMenu(father) {
	if (controllers.UserChatExpandableForm == 0 || controllers.UserChatExpandableForm == 2) {
		document.getElementById(father).style.display = "none";
		controllers.UserChatExpandableForm = -1;
    }
}

function changeLoginMenu(section) {
	var elems = document.getElementsByClassName("sign");
	var btns = document.getElementsByClassName("signBTN");
	for (var i = 0; i < elems.length; i++) elems[i].style.display = "none";
	for (var i = 0; i < btns.length; i++) btns[i].className = "clickable signBTN signBTNhide";

	switch (section) {
		case "logger":
			document.getElementById("registerTitle").innerHTML = "Log in";
			document.getElementById("logger").style.display = "block";
			document.getElementById("register").className = "clickable signBTN signBTNshow";
			document.getElementById("forgot").className = "clickable signBTN signBTNshow";
			break;
		case "registrer":
			document.getElementById("registerTitle").innerHTML = "Register";
			document.getElementById("registrer").style.display = "block";
			document.getElementById("login").className = "clickable signBTN signBTNshow";
			document.getElementById("forgot").className = "clickable signBTN signBTNshow";
			break;
		case "forggoter":
			document.getElementById("registerTitle").innerHTML = "Password forgotten";
			document.getElementById("forggoter").style.display = "block";
			document.getElementById("login").className = "clickable signBTN signBTNshow";
			document.getElementById("register").className = "clickable signBTN signBTNshow";
			break;
		default:
			break;
    }
}

function showPassword() {
	var x = document.getElementsByClassName("PasswordI");
	for (var i = 0; i < x.length; i++) x[i].type = "text";
}

function hidePassword() {
	var x = document.getElementsByClassName("PasswordI");
	for (var i = 0; i < x.length; i++) x[i].type = "password";
}

function checkUserForm() {
	//Check than a password had been introduced
	var pass = document.getElementById("userViewPassword").value;
	if (pass == null || pass == "") return false;

	//Image helper
	var icons = document.getElementsByName('icon');
	var selected;
	for (var i = 0; i < icons.length; i++) {
		console.log(i);
		if (icons[i].checked) {
			selected = icons[i].value;
			break;
		}
	}
	document.getElementById('ImageSRC').value = selected;

	return true;
}

function changeUserMenu(section) {
	if (section == undefined && CookieHelper.getCookie("USER_MENU") != null) {
		var tmp = CookieHelper.getCookie("USER_MENU");
		CookieHelper.deleteCookie("USER_MENU")
		changeUserMenu(tmp);
	} else if (section == undefined) {
		changeUserMenu('GeneralSettings')
	} else if (location.href.endsWith('TransUser')) {
		var elems = document.getElementsByClassName("user-menu");
		var btns = document.getElementsByClassName("user-menu-btn active");

		for (var i = 0; i < elems.length; i++) elems[i].style.display = "none";
		for (var i = 0; i < btns.length; i++) btns[i].className = "user-menu-btn";

		document.getElementById(section).style.display = "block";
		document.getElementById(section + "BTN").className = "user-menu-btn active";
	} else {
		CookieHelper.setCookie('USER_MENU', section);
		location.href = 'TransUser';
    }
}

function changeUserGender(value) {
	var males = document.getElementsByClassName('male');
	var females = document.getElementsByClassName('female');

	if (value == "male") {
		for (var i = 0; i < males.length; i++) males[i].style.display = 'inline-block';
		for (var i = 0; i < females.length; i++) females[i].style.display = 'none';
		males[0].children[0].children[1].children[0].checked = true;
	} else if (value == "female") {
		for (var i = 0; i < males.length; i++) males[i].style.display = 'none';
		for (var i = 0; i < females.length; i++) females[i].style.display = 'inline-block';
		females[0].children[0].children[1].children[0].checked = true;
    }
}

function openChat(index) {
	var chat = chats[index];

	document.getElementById("userChatUsername").innerHTML = chat.to;
	var msgs = document.getElementById("userMessages");
	msgs.textContent = "";

	for (var i = 0; i < chat.messages.length; i++) {
		var par = document.createElement("P");
		var mes = document.createElement("B");
		var dec = document.createElement("I");

		dec.className = "fas fa-caret-down";

		mes.innerHTML = chat.messages[i].message;
		par.appendChild(mes);

		if (chat.messages[i].user == chat.from) {
			par.className = "their";
			msgs.appendChild(par);

			dec.className += " their";

			msgs.appendChild(dec);
		}
		else {
			par.className = "your";
			msgs.appendChild(par);

			dec.className += " your";

			msgs.appendChild(dec);
        }

		
    }

	if (controllers.UserChatExpandableForm == -1 || controllers.UserChatExpandableForm == 0 || controllers.UserChatExpandableForm == 2) openExpandableMenu('userChat', 'userBackgroundChat','userChatNotch')
}

function interactSearcherNav() {
	if (controllers.SearchFloatingForm) {
		controllers.SearchFloatingForm = false;
		document.getElementById("searcherNavContent").className = "";
	}
	else {
		controllers.SearchFloatingForm = true;
		document.getElementById("searcherNavContent").className = "active";
	}
}

//Var declarations
var toolboxer;
var slideIndex;
var slideNumber;

//Basic operations
function redirect(l) {
	location.href = l;
}

//Slider
function initSlider() {
	slideIndex = 1;
	showSlider(slideIndex);
}

function nextSlider(n) {
	showSlider(slideIndex += n);
}

function currentSlider(n) {
	showSlider(slideIndex = n);
}

function showSlider(n) {
	var slides = document.getElementsByClassName("divSlider");
	var dots = document.getElementsByClassName("dotSlider");
	var btns = document.getElementsByClassName("btnSlider");
	if (n > slideNumber) { slideIndex = 1 }
	if (n < 1) { slideIndex = slideNumber }
	for (var i = 0; i < slideNumber; i++) {
		slides[i].style.display = "none";
	}
	for (var i = 0; i < dots.length; i++) {
		dots[i].className = dots[i].className.replace(" active", "");
	}
	for (var i = 0; i < btns.length; i++) {
		btns[i].style.marginTop = 6 * slides[slideIndex - 1].childElementCount + "%";
	}
	slides[slideIndex - 1].style.display = "block";
	dots[slideIndex - 1].className += " active";
}

//Toolbox
function formatToolboxer() {
	toolboxer = document.getElementById("toolboxer");
	toolboxer.style.marginLeft = "20px";
	toolboxer.style.textAlign = "center";
	toolboxer.style.textDecoration = "underline";
}
function accept(e) {
	if (toolboxer.style.display == "none") show(e, "Accept costumer's offer.");
}
function deny(e) {
	if (toolboxer.style.display == "none") show(e, "Deny costumer's offer.");
}
function denied(e) {
	if (toolboxer.style.display == "none") show(e, "The transporter denied your reserve.");
}
function paid(e) {
	if (toolboxer.style.display == "none") show(e, "The costumer has alredy paid the reserve.");
}
function notPaid(e) {
	if (toolboxer.style.display == "none") show(e, "The costumer hasn't paid the reserve yet.");
}
function insured(e) {
	if (toolboxer.style.display == "none") show(e, "The transportet has alredy paid the bond as insurance.");
}
function notInsured(e) {
	if (toolboxer.style.display == "none") show(e, "The costumer hasn't paid the bond insurance yet.");
}
function cantEnsure(e) {
	if (toolboxer.style.display == "none") show(e, "The costumer has decided to not demand an insurance to the transporter.");
}
function erase(e) {
	if (toolboxer.style.display == "none") show(e, "Erase the selected object.");
}
function create(e) {
	if (toolboxer.style.display == "none") show(e, "Create a new object.");
}
function show(e, t) {
	toolboxer.innerHTML = "<img src='" + e.getAttribute("src") + "' style='width: 30px;'/> <p>" + t + "</p>";
	toolboxer.style.display = "block";
}
function hide() {
	toolboxer.style.display = "none";
}

function doSendChatMessage(from) {
    try {
		if (!controllers.UserChatSending) {
			controllers.UserChatSending = true;

			var message = document.getElementById('userChatMessage').value;
			var to = document.getElementById('userChatUsername').innerHTML;
			$.ajax({
				type: "POST",
				url: 'UpdateData',
				data: {
					From: from,
					To: to,
					Message: message
				},
				dataType: 'json',
				success: function (result) {
					if (!result['success']) {
						alert("There was an error sending the message!");
					} else {
						document.getElementById('userChatMessage').value = "";
						doRefreshChat(from);
					}
					controllers.UserChatSending = false;
				}
			})
		}
    } catch (e) {
		controllers.UserChatSending = false;
    }

}

function doRefreshChat(from) {
	var to = document.getElementById('userChatUsername').innerHTML;
	$.ajax({
		type: "POST",
		url: 'RefreshChatData',
		data: {
			From: from,
			To: to
		},
		dataType: 'json',
		success: function (result) {
			if (!result['success']) {
				console.log("There was an error reloading the messages!")
			} else {
				var messages = result['messages'];
				var msgs = [];
				var section = messages.split(';');
				var f = messages.split(';')[0].split(',')[0];
				var t = messages.split(';')[0].split(',')[1];
				var dest;
				var pos = 0;

				if (from == f) dest = t;
				else dest = f;
				while (pos < chats.length && chats[pos].to != dest) pos++;
				
				for (var i = 0; i < section.length - 1; i++) {
					var subsection = section[i].split(',');
					msgs.push(new Message(subsection[1], subsection[2], subsection[3]))
					console.log(msgs);
				}

				chats[pos] = new Chat(from, dest, msgs);
				openChat(pos);
			}
		}
	})
}

function doUpdateChat(from, to, top) {
	$.ajax({
		type: "POST",
		url: 'RefreshChatData',
		data: {
			From: from,
			To: to,
			Limit: top
		},
		dataType: 'json',
		success: function (result) {
			if (!result['success']) {
				console.log("There was an error reloading the messages!")
			} else {
				var messages = result['messages'];
				var msgs = [];
				var section = messages.split(';');
				var f = messages.split(';')[0].split(',')[0];
				var t = messages.split(';')[0].split(',')[1];
				var dest;
				var pos = 0;

				if (from == f) dest = t;
				else dest = f;
				while (pos < chats.length && chats[pos].to != dest) pos++;

				for (var i = 0; i < section.length - 1; i++) {
					var subsection = section[i].split(',');
					msgs.push(new Message(subsection[1], subsection[2], subsection[3]))
					console.log(msgs);
				}

				chats[pos] = new Chat(from, dest, msgs);
				openChat(pos);
			}
		}
	})
}

function doCheckChats(from) {
	var checker = window.setInterval(function () {
		if (controllers.UserChatCheckerAjax == null || controllers.UserChatCheckerAjax.state() != "pending") {
			controllers.UserChatCheckerAjax = $.ajax({
				type: "POST",
				url: 'CheckChats',
				data: {
					From: from,
				},
				dataType: 'json',
				success: function (result) {
					if (!result['success']) {
						if (result['error'] == "EXTEND") {
							console.log("Extending session");
						} else {
							console.log("There was an error while checking for new messages!");
						}
					} else {
						var numbers = result['numbers'];
						var n = [];

						for (var i = 0; i < chats.length; i++) {
							n.push({
								key: chats[i].to,
								value: chats[i].messages.length
							});
						}
						if (Object.keys(numbers).length != n.length) {

                        }
						for (var i = 0; i < n.length; i++) {
							if (numbers[n[i].key] != n[i].value) {
								doUpdateChat(from, numbers[n[i].key], n[i].value - numbers[n[i].key]);
							}
						}
					}
				}
			});
		} else {
			console.log("Last checking not completed!");
        }
	}, 10000);
}