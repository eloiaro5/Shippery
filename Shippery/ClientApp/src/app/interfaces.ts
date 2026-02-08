import { CityType } from './enumerators';

export interface IUser {
  username: string;
  password: string;
  mail: string;
  currency: ICurrency;

  validate(): boolean;
}

export interface ICurrency {
  name: string;
  code: string;
  symbol: string;
}

export interface ICity {
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
}

/* --- Responses --- */
export interface IBaseResponse {
  success: boolean;
  message: string;
  data: object;
}

export interface IUserResponse extends IBaseResponse { }
export interface ICityResponse extends IBaseResponse { }
