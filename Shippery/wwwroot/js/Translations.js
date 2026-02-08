function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

var translations = {};

if (getCookie("Language") == null) {
    document.getElementById("LanguageSetter").Value = "en-EN"
    setLanguageCookie(document.getElementById("LanguageSetter").Value)
}

$.ajax({
    type: "GET",
    url: "/lib/text/translations.json",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (data) {
        try {
            translations = data;
            setLanguage();
        } catch (ReferenceError) {
            console.log("Translations not loaded, proceeding to wait...")
            waitLanguageLoad()
        }
    },
    error: function (jqXHR, textStatus, errorThrown) {
        alert("Error: " + textStatus + " errorThrown: " + errorThrown);
    }
}); 

function waitLanguageLoad() {
    setTimeout(function () {
        setLanguage();
    }, 100);
}

function setLanguageCookie(elem) {
    document.cookie = "Language=" + elem + "; expires=Thu, 31 Dec 2099 22:59:59 UTC;path=/;SameSite=Strict";
    location.reload();
}
function setLanguage() {
    document.getElementById("LanguageSetter").value = getCookie("Language"); 
    changeLanguage(getCookie("Language"), 'Layout');
    changeLanguage(getCookie("Language"), name);
}

function normalize(inner, replace) {
    let er = /<[^/].*?>/;
    let erreplace = /<[^/]{1,5}?>/
    while (er.test(inner)) {
        let elem = er.exec(inner)[0];
        inner = inner.replace(elem, "");
        replace = replace.replace(erreplace, elem);
    }
    er = /\[\[.*?\]\]/;
    while (er.test(inner)) {
        let elem = er.exec(inner)[0];
        inner = inner.replace(elem, "");
        replace = replace.replace(er, elem.replace("[[", "").replace("]]", ""));
    }
    return replace;
}

function changeLanguage(language, section) {
    var notFound = "";
    var nFCounter = 0;
    for (var translation in translations[language][section]) {
        if (document.getElementById(translation) != null) {
            switch (document.getElementById(translation).tagName) {
                case "INPUT":
                    document.getElementById(translation).value = translations[language][section][translation];
                    if (document.getElementById(translation).getAttribute("placeholder") != null) {
                        document.getElementById(translation).placeholder = translations[language][section][translation + "P"];
                    }
                    break;
                case "TEXTAREA":
                    console.log("Textarea");
                    document.getElementById(translation).value = translations[language][section][translation];
                    if (document.getElementById(translation).getAttribute("placeholder") != null) {
                        document.getElementById(translation).placeholder = translations[language][section][translation + "P"];
                    }
                default:
                    document.getElementById(translation).innerHTML = normalize(document.getElementById(translation).innerHTML, translations[language][section][translation]);
                    break;
            }
        }
        else {
            notFound += translation + ", ";
            nFCounter++;
        }
    }
    if (nFCounter > 0) {
        notFound = notFound.substr(0, notFound.length - 2);
        console.warn("The elements with id '[" + notFound + "]' don't exist!");
    }   
}