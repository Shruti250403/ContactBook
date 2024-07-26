import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Contact } from '../models/contact.model';
import { AddContact } from '../models/addcontact.model';
import { EditContact } from '../models/editcontact.model';
import { ContactSP } from '../models/contactSP.model';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private apiUrl="http://localhost:5220/api/Contact/";
  constructor(private http:HttpClient){}

  getAllContacts():Observable<ApiResponse<Contact[]>>{
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl+"GetAllContacts")
  }
  addContact(product: AddContact): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl + 'Create', product);
}
getAllFavouriteContacts():Observable<ApiResponse<Contact[]>>{
  return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + 'GetAllFavouriteContacts')
}
deleteContact(contactId: number|undefined):Observable<ApiResponse<Contact>>{
  return this.http.delete<ApiResponse<Contact>>(`${this.apiUrl}Remove/${contactId}`)

}
getAllContactsCounts() : Observable<ApiResponse<number>>{
  return this.http.get<ApiResponse<number>>(this.apiUrl+'GetContactsCount');
}
getAllContactsCount(letter:string) : Observable<ApiResponse<number>>{
  return this.http.get<ApiResponse<number>>(this.apiUrl+'GetContactsCount/?letter='+letter);
}
getAllPaginatedContacts(page: number, pageSize: number,sortOrder: string,letter?: string,search?: string):Observable<ApiResponse<Contact[]>>{
  if (letter != null && search !=null) {
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + `GetAllContactsByPagination?letter=${letter}&search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
  }
  else if(letter==null && search != null){
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + `GetAllContactsByPagination?search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
  }
  else if(letter!=null && search == null){
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + `GetAllContactsByPagination?letter=${letter}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
  }
  else {
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + `GetAllContactsByPagination?page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
  }
  }
  fetchContactCount(letter?: string,search?:string): Observable<ApiResponse<number>> {
    if (letter != null && search!=null) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + `GetContactsCount?letter=${letter}&search=${search}`);
    }
    else if (letter == null && search!=null) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + `GetContactsCount?search=${search}`);
    }
    if (letter != null && search==null) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + `GetContactsCount?letter=${letter}`);
    }
    else {
      return this.http.get<ApiResponse<number>>(this.apiUrl + 'GetContactsCount');
    }
  }
modifyContact(editContact : EditContact): Observable<ApiResponse<EditContact>> {
  return this.http.put<ApiResponse<EditContact>>(`${this.apiUrl}ModifyContact`,editContact );
}
getContactById(contactId: number|undefined):Observable<ApiResponse<EditContact>>{
  return this.http.get<ApiResponse<EditContact>>(`${this.apiUrl}GetContactById/${contactId}`)
}
getAllFavouriteContactsCount(letter:string) : Observable<ApiResponse<number>>{
  return this.http.get<ApiResponse<number>>(this.apiUrl+'TotalContactFavourite/?letter='+letter);
}
getAllFavouriteContactsWithPagination(pageNumber: number,pageSize:number,letter:string) : Observable<ApiResponse<Contact[]>>{
  return this.http.get<ApiResponse<Contact[]>>(this.apiUrl+'GetPaginatedFavouriteContacts?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize);
}
getContactsBasedOnBirthdayMonth(month: number):Observable<ApiResponse<ContactSP[]>>{
  return this.http.get<ApiResponse<ContactSP[]>>(`${this.apiUrl}GetContactsByBirthMonthSP/?month=${month}`)
}

getContactByState(stateId: number):Observable<ApiResponse<ContactSP[]>>{
 return this.http.get<ApiResponse<ContactSP[]>>(`${this.apiUrl}GetContactsByStateSP/?stateId=${stateId}`)
}

getContactsCountBasedOnCountry(countryId: number):Observable<ApiResponse<number>>{
 return this.http.get<ApiResponse<number>>(`${this.apiUrl}GetContactsCountByCountrySP/?countryId=${countryId}`)
}

getContactsCountBasedOnGender(gender: string):Observable<ApiResponse<number>>{
 return this.http.get<ApiResponse<number>>(`${this.apiUrl}GetContactCountByGenderSP/?gender=${gender}`)
}
}
