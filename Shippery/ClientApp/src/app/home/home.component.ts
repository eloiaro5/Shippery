import { Component, Inject } from '@angular/core';
import { ICity, ICityResponse } from '../interfaces';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': 'my-auth-token'
  })
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  protected http: HttpClient;
  private baseUrl: string;
  public cities: ICity[];

  constructor(protected h: HttpClient, @Inject("BASE_URL") bURL: string) {
    this.http = h;
    this.baseUrl = bURL;

    this.http.post<ICityResponse>(this.baseUrl + "CityController/GetMatchings",
      {
        'match': 'barc',
      }, httpOptions).subscribe(result => { if (!result.success) console.error(result.message); else this.cities = result.data as ICity[]; }, error => console.warn(error));


      //this.http.post<ICityResponse>(this.baseUrl + "CityController/GetMatchings",
      //  {
      //    'username': 'eloiaro5',
      //    'password': 'hammer',
      //  }, httpOptions).subscribe(result => { if (!result.success) console.error(result.message); else this.cities = result.data as ICity[]; }, error => console.warn(error));

    window.onload = (event) => {
      this.LoadCompleters();
    };
  }

  Autocomplete(inp): void {
    inp.addEventListener('input', (event) => {
      this.SetCities(event.target.value);
    });
    inp.addEventListener('focus', (event) => {
      this.SetCities(event.target.value)
    });

  }

  SetCities(match: string): void {
    if (match.length >= 3) {

      //this.http.post<ICityResponse>(this.baseUrl + "CityController/GetMatchings",
      //  {
      //    'match': match
      //  }, httpOptions).subscribe(result => { if (!result.success) console.error(result.message); else this.cities = result.data as ICity[]; }, error => console.warn(error));

      //var options = "";
      //if (match == "") {
      //  for (var i = 0; i < 10; i++) {
      //    options += '<option value="' + this.cities[i] + '" />';
      //  }
      //}
      //else {
      //  var c = 10;
      //  for (var i = 0; c > 0 && i < this.cities.length; i++) {
      //    if (this.cities[i].ascii.substr(0, match.length).toUpperCase() == match.toUpperCase()) {
      //      options += '<option value="' + this.cities[i] + '" />';
      //      c--;
      //    }
      //  }
      //}
      //document.getElementById("cities").innerHTML = options;
    }
  }

  LoadCompleters(): void {
    var completers = document.getElementsByClassName("cityAutocompleter");
    for (var i = 0; i < completers.length; i++) {
      this.Autocomplete(completers.item(i));
    }
  }
}
