function autocomplete(inp) {
    inp.addEventListener("input", function (e) {
        setCities(this.value);
    });
    inp.addEventListener('focus', (event) => {
        setCities(event.target.value)
    });
}

function setCities(match) {
    var options = "";
    if (match == "") {
        for (var i = 0; i < 10; i++) {
            options += '<option value="' + cities[i] + '" />';
        }
    }
    else {
        var c = 10;
        for (var i = 0; c > 0 && i < cities.length; i++) {
            if (cities[i].substr(0, match.length).toUpperCase() == match.toUpperCase()) {
                options += '<option value="' + cities[i] + '" />';
                c--;
            }
        }
    }
    document.getElementById("cities").innerHTML = options;
}

/*initiate the autocomplete function on the "myInput" element, and pass along the countries array as possible autocomplete values:*/
function LoadCompleters() {
    var completers = document.getElementsByClassName("cityAutocompleter");
    setCities("");
    for (var i = 0; i < completers.length; i++) {
        autocomplete(completers.item(i));
    }
}