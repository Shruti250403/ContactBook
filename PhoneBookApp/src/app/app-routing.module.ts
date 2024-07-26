import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { ContactsListComponent } from './components/contacts/contacts-list/contacts-list.component';
import { SignupComponent } from './components/Auth/signup/signup.component';
import { SigninComponent } from './components/Auth/signin/signin.component';
import { AddContactComponent } from './components/contacts/add-contact/add-contact.component';
import { EditContactComponent } from './components/contacts/edit-contact/edit-contact.component';
import { DetailContactComponent } from './components/contacts/detail-contact/detail-contact.component';
import { PaginationContactComponent } from './components/contacts/pagination-contact/pagination-contact.component';
import { SignupsuccessComponent } from './components/Auth/signupsuccess/signupsuccess.component';
import { authGuard } from './guard/auth.guard';
import { FavouriteContactComponent } from './components/contacts/favourite-contact/favourite-contact.component';
import { ForgotPasswordComponent } from './components/Auth/forgot-password/forgot-password.component';
import { ChangePasswordComponent } from './components/Auth/change-password/change-password.component';
import { EditUserComponent } from './components/Auth/edit-user/edit-user.component';
import { ProfileComponent } from './components/Auth/profile/profile.component';
import { AddContactTfComponent } from './components/contacts/add-contact-tf/add-contact-tf.component';
import { EditContactTfComponent } from './components/contacts/edit-contact-tf/edit-contact-tf.component';
import { BasedOnBirthdayComponent } from './components/report/based-on-birthday/based-on-birthday.component';
import { BasedOnStateComponent } from './components/report/based-on-state/based-on-state.component';
import { BasedOnCountryComponent } from './components/report/based-on-country/based-on-country.component';
import { BasedOnGenderComponent } from './components/report/based-on-gender/based-on-gender.component';

const routes: Routes = [
  {path:'',redirectTo:'home',pathMatch:'full'},
  {path:'home',component:HomeComponent},
  {path:'privacy',component:PrivacyComponent},
  {path:'contacts',component:ContactsListComponent},
  {path:'signup',component:SignupComponent},
  {path:'signin',component:SigninComponent},
  {path:'addcontact',component:AddContactComponent,canActivate:[authGuard]},
  {path:'editcontact/:id',component:EditContactComponent,canActivate:[authGuard]},
  {path:'detailcontact/:id',component:DetailContactComponent},
  {path:'pagination',component:PaginationContactComponent},
  {path:'signupsuccess',component:SignupsuccessComponent},
  {path:'favourite',component:FavouriteContactComponent},
  {path:'forgotPassword',component:ForgotPasswordComponent},
  {path:'changePassword',component:ChangePasswordComponent},
  {path:'editUser/:loginId',component:EditUserComponent},
  {path:'profile/:loginId',component:ProfileComponent},
  {path:'addtf',component:AddContactTfComponent},
  {path:'edittf/:contactId',component:EditContactTfComponent},
  {path: 'basedOnBirthday', component: BasedOnBirthdayComponent},
  {path: 'basedOnState', component: BasedOnStateComponent},
  {path: 'basedOnCountry', component: BasedOnCountryComponent},
  {path: 'basedOnGender', component: BasedOnGenderComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
