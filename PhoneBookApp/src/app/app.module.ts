import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { ContactsListComponent } from './components/contacts/contacts-list/contacts-list.component';
import { SignupComponent } from './components/Auth/signup/signup.component';
import { SigninComponent } from './components/Auth/signin/signin.component';
import { AddContactComponent } from './components/contacts/add-contact/add-contact.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditContactComponent } from './components/contacts/edit-contact/edit-contact.component';
import { DetailContactComponent } from './components/contacts/detail-contact/detail-contact.component';
import { PaginationContactComponent } from './components/contacts/pagination-contact/pagination-contact.component';
import { SignupsuccessComponent } from './components/Auth/signupsuccess/signupsuccess.component';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './Interceptors/auth.interceptor';
import { FavouriteContactComponent } from './components/contacts/favourite-contact/favourite-contact.component';
import { DatePipe } from '@angular/common';
import { ForgotPasswordComponent } from './components/Auth/forgot-password/forgot-password.component';
import { ChangePasswordComponent } from './components/Auth/change-password/change-password.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ProfileComponent } from './components/Auth/profile/profile.component';
import { EditUserComponent } from './components/Auth/edit-user/edit-user.component';
import { AddContactTfComponent } from './components/contacts/add-contact-tf/add-contact-tf.component';
import { EditContactTfComponent } from './components/contacts/edit-contact-tf/edit-contact-tf.component';
import { BasedOnBirthdayComponent } from './components/report/based-on-birthday/based-on-birthday.component';
import { BasedOnCountryComponent } from './components/report/based-on-country/based-on-country.component';
import { BasedOnGenderComponent } from './components/report/based-on-gender/based-on-gender.component';
import { BasedOnStateComponent } from './components/report/based-on-state/based-on-state.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PrivacyComponent,
    ContactsListComponent,
    SignupComponent,
    SigninComponent,
    AddContactComponent,
    EditContactComponent,
    DetailContactComponent,
    PaginationContactComponent,
    SignupsuccessComponent,
    FavouriteContactComponent,
    ForgotPasswordComponent,
    ChangePasswordComponent,
    ProfileComponent,
    EditUserComponent,
    AddContactTfComponent,
    EditContactTfComponent,
    BasedOnBirthdayComponent,
    BasedOnCountryComponent,
    BasedOnGenderComponent,
    BasedOnStateComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    BrowserAnimationsModule,
    NgbModule
  ],
  providers: [AuthService,{provide:HTTP_INTERCEPTORS,useClass:AuthInterceptor,multi:true},DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
