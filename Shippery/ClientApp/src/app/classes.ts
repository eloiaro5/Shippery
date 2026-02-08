import { Inject } from '@angular/core';
import { IUser, ICity, ICurrency, ICityResponse } from './interfaces';
import { CityType } from './enumerators';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: {
    'Content-Type': 'application/json'
  }
}

export class CityAutocompleter {
  private http: HttpClient;
  private baseUrl: string;
  public cities: ICity[];

  constructor(protected h: HttpClient, @Inject("BASE_URL") bURL: string) {
    this.http = h;
    this.baseUrl = bURL;

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
      console.log(match);
      console.log(httpOptions);
      this.http.post<ICityResponse>(this.baseUrl + "CityController/GetMatchings",
        {
          'match': match
        }, httpOptions).subscribe(result => { if (!result.success) console.error(result.message); else this.cities = result.data as ICity[]; }, error => console.warn(error));

      var options = "";
      if (match == "") {
        for (var i = 0; i < 10; i++) {
          options += '<option value="' + this.cities[i] + '" />';
        }
      }
      else {
        var c = 10;
        for (var i = 0; c > 0 && i < this.cities.length; i++) {
          if (this.cities[i].ascii.substr(0, match.length).toUpperCase() == match.toUpperCase()) {
            options += '<option value="' + this.cities[i] + '" />';
            c--;
          }
        }
      }
      document.getElementById("cities").innerHTML = options;
    }
  }

  LoadCompleters(): void {
    var completers = document.getElementsByClassName("cityAutocompleter");
    for (var i = 0; i < completers.length; i++) {
      this.Autocomplete(completers.item(i));
    }
  }
}

export class User implements IUser {
  username: string;
  password: string;
  mail: string;
  currency: ICurrency;

  constructor(username: string, password: string, mail: string, currency: ICurrency) {
    this.username = username;
    this.password = password;
    this.mail = mail;
    this.currency = currency;
  }

  validate(): boolean {
    var rg = new RegExp(/[\w-\.]+@([\w-]+\.)+[\w-]{2,4}/);
    if (this.username.length < 3 || this.username.length > 15) return false;
    else if (this.password.length < 3 || this.password.length > 31) return false;
    else if (this.mail.length < 3 || this.mail.length > 63 || !rg.test(this.mail)) return false;
    return true;
  }
}

export class City implements ICity {
  id: number;
  name: string;
  ascii: string;
  latitude: number;
  longitude: number;
  country: string;
  iso2: string;
  iso3: string;
  adminName: string;
  type: CityType;
  population: number;

  constructor(id: number, name: string, ascii: string, country: string, adminName: string) {
    this.id = id;
    this.name = name;
    this.ascii = ascii;
    this.country = country;
    this.adminName = adminName;
  }

  toString(): string {
    if (this.id == -1) return name;
    else return this.ascii + ", " + this.adminName + ", " + this.country;
  }
}
