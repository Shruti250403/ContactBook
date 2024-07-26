import { Injectable } from '@angular/core';
import { State } from '../models/state.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private apiUrl='http://localhost:5220/api/State/';
  constructor(private http:HttpClient) { }
  getAllStates():Observable<ApiResponse<State[]>>{
    return this.http.get<ApiResponse<State[]>>(this.apiUrl+"GetStates")
  }
  getStateByCountryId(countryId:number):Observable<ApiResponse<State[]>>{
    return this.http.get<ApiResponse<State[]>>(this.apiUrl+"GetAllStateByCountryId/"+countryId)
  }
}
