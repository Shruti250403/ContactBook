import { Injectable } from '@angular/core';
import { Country } from '../models/country.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  private apiUrl='http://localhost:5220/api/Country/';

  constructor(private http:HttpClient) { }
  getAllCountries():Observable<ApiResponse<Country[]>>{
    return this.http.get<ApiResponse<Country[]>>(this.apiUrl+"GetAll")
  }
}
