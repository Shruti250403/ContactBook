import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SigninComponent } from './signin.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthService } from 'src/app/services/auth.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { LocalstorageService } from 'src/app/services/helpers/localstorage.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('SigninComponent', () => {
  let component: SigninComponent;
  let fixture: ComponentFixture<SigninComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  let routerSpy: Router;
  let localStorageService : jasmine.SpyObj<LocalstorageService>;
  beforeEach(() => {
    authService = jasmine.createSpyObj('AuthService',['signIn'])
    localStorageService = jasmine.createSpyObj('LocalstorageService',['setItem'])
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [SigninComponent],
      providers: [{provide: AuthService, useValue: authService},
                  {provide: LocalstorageService, useValue: localStorageService},]
    });
    fixture = TestBed.createComponent(SigninComponent);
    component = fixture.componentInstance;
    routerSpy = TestBed.inject(Router);
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(SigninComponent);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });
  it('should login successfully',()=>{
    spyOn(routerSpy,'navigate');
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    authService.signIn.and.returnValue(of(mockResponse));
    component.Username = 'username';
    component.password = 'password';
    component.login();
    expect(authService.signIn).toHaveBeenCalledWith('username', 'password');
    expect(localStorageService.setItem).toHaveBeenCalledWith('jwtToken', '');
    expect(localStorageService.setItem).toHaveBeenCalledWith('loginId', 'username');
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/home']);
    expect(component.loading).toBe(false);
  });
  it('should handle login failure', () => {
    authService.signIn.and.returnValue(of({ success: false,data: '', message: "Login failed" }));
    spyOn(window, 'alert'); // Spy on window.alert to check if it is called
    component.Username = 'username';
    component.password = 'password';
    component.login();
    expect(authService.signIn).toHaveBeenCalledWith('username', 'password');
    expect(window.alert).toHaveBeenCalledWith("Login failed");
    expect(component.loading).toBe(false);
  });
  
  
  it('should handle http error', () => {
    const mockError = { error: { message: 'HTTP error' } };
    authService.signIn.and.returnValue(throwError(mockError));
    spyOn(window, 'alert'); // Spy on window.alert to check if it is called
    component.Username = 'username';
    component.password = 'password';
    component.login();
    expect(authService.signIn).toHaveBeenCalledWith('username', 'password');
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message);
    expect(component.loading).toBe(false);
  });
});