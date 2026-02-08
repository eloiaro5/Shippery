import { Component, Inject } from '@angular/core';
import { IUser, ICurrency, IUserResponse } from '../interfaces';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../classes';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization':'my-auth-token'
  })
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  protected http: HttpClient;
  private baseUrl: string;
  public currencies: ICurrency[];
  public default: string;

  constructor(protected h: HttpClient, @Inject("BASE_URL") bURL: string) {
    this.http = h;
    this.baseUrl = bURL;

    this.http.get<ICurrency[]>(this.baseUrl + "CurrencyController/GetCurrencies").subscribe(result => {
      this.currencies = result;
      this.default = this.currencies[0].code;
    }, error => console.error(error));  
  }

  Login(username, password) {
    //Change password and notify user
    this.http.post<IUserResponse>(this.baseUrl + "UserController/CheckUser",
      {
        'username': username,
        'password': password,
      }, httpOptions).subscribe(result => { if (!result.success) console.error(result.message); else location.href = '/user'; }, error => console.error(error));
  }

  AddUser(username, password, mail, currencyCode) {
    //Select currency from the list of currencies
    var cur: ICurrency;
    for (var i = 0; i < this.currencies.length; i++) {
      if (this.currencies[i].code == currencyCode) cur = this.currencies[i];
    }

    //Create user for interact with him
    var u: User = new User(username, password, mail, cur);
    if (u.validate()) {
      //Add user to the database
      this.http.post<IUserResponse>(this.baseUrl + "UserController/AddUser",
        {
          'username': username,
          'password': password,
          'mail': mail,
          'currency': cur
        }, httpOptions).subscribe(result => { if (!result.success) console.error(result.message); else window.location.reload(); }, error => console.error(error));
    } else {
      console.error("User not valid!");
    }
  }

  ForgotPassword(username: string, mail: string) {
    //Change password and notify user
    this.http.post<IUserResponse>(this.baseUrl + "UserController/ResetUser",
      {
        'username': username,
        'mail': mail,
      }, httpOptions).subscribe(result => { if (!result.success) console.error(result.message); else window.location.reload(); }, error => console.error(error));
  }

  public Log(data) {
    this.Login(data["Username"], data["Password"]);
  }

  public Add(data) {
    this.AddUser(data["Username"], data["Password"], data["Mail"], data["Currency"]);
  }

  public Forgot(data) {
    this.ForgotPassword(data["Username"], data["Mail"]);
  }
}
