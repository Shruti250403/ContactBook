import { Injectable } from '@angular/core';
import { LocalstorageKeys } from './helpers/localstoragekeys';
import { BehaviorSubject, Observable, Subject, tap } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { LocalstorageService } from './helpers/localstorage.service';
import { HttpClient } from '@angular/common/http';
import { ForgotPassword } from '../models/forgot-password.model';
import { UserDetail } from '../models/user-details.model';
import { UpdateUser } from '../models/update-user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5220/api/Auth';
  private authState = new BehaviorSubject<boolean>(this.localStorageHelper.hasItem(LocalstorageKeys.TokenName));
  private UsernameSubject = new BehaviorSubject<string | null | undefined>(this.localStorageHelper.getItem(LocalstorageKeys.UserId));
  constructor(private localStorageHelper: LocalstorageService, private http: HttpClient) { }
  
  signUp(user: any): Observable<ApiResponse<string>> {
    const body = user;
    return this.http.post<ApiResponse<string>>(this.apiUrl + '/RegisterUser', body)
  }

  signIn(Username: string, password: string): Observable<ApiResponse<string>> {
    const body = { Username, password };
    return this.http.post<ApiResponse<string>>(this.apiUrl + '/LoginUser', body).pipe(
      tap(response => {
        if (response.success) {
          this.localStorageHelper.setItem(LocalstorageKeys.TokenName, response.data);
          this.localStorageHelper.setItem(LocalstorageKeys.UserId, Username);
          this.authState.next(this.localStorageHelper.hasItem(LocalstorageKeys.TokenName));
          this.UsernameSubject.next(Username);
        }
      })
    )
  }
  isAuthenticated(){
    return this.authState.asObservable();
  }
  forgotPassword(forgotPassword: ForgotPassword){
    const body =forgotPassword;
    return this.http.post<ApiResponse<string>>(this.apiUrl+"/ForgetPassword",body);
   }
  SignOut() {
    this.localStorageHelper.removeItem(LocalstorageKeys.TokenName);
    this.localStorageHelper.removeItem(LocalstorageKeys.UserId);
    this.authState.next(false);
    this.UsernameSubject.next(null);
  }
  getUsername(): Observable<string | null | undefined> {
    return this.UsernameSubject.asObservable();
  }
  getUserByLoginId(loginId: string|null|undefined): Observable<ApiResponse<UserDetail>> {
    return this.http.get<ApiResponse<UserDetail>>(`${this.apiUrl}/GetUserById/${loginId}`)
  }

  modifyUser(editContact: UpdateUser): Observable<ApiResponse<UpdateUser>> {
    return this.http.put<ApiResponse<UpdateUser>>(`${this.apiUrl}/ModifyUser`, editContact);
  }
  private profileUpdatedSource = new Subject<void>();
 
   // Method to emit profile update event
   emitProfileUpdated(): void {
    this.profileUpdatedSource.next();
  }
 
  // Method to subscribe to profile update event
  onProfileUpdated(): Observable<void> {
    return this.profileUpdatedSource.asObservable();
  }
}
