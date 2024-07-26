import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ContactService } from '../../../services/contact.service';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { AuthService } from 'src/app/services/auth.service';
import { EditContact } from 'src/app/models/editcontact.model';

@Component({
  selector: 'app-contacts-list',
  templateUrl: './contacts-list.component.html',
  styleUrls: ['./contacts-list.component.css']
})
export class ContactsListComponent implements OnInit {
  contacts:Contact[]|undefined;
  username:string |null|undefined;
  loading:boolean=false;
  isAuthenticated: boolean = false;
  constructor(private contactService:ContactService,private authService:AuthService,private cdr: ChangeDetectorRef){}
  ngOnInit(): void {
    this.loadCategories();
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated=authState;
      this.cdr.detectChanges();
     });
     this.authService.getUsername().subscribe((username:string |null|undefined)=>{
      this.username=username;
      this.cdr.detectChanges();
     });
  }
  deleteContact(contactId: number) {
    if (confirm('Are you sure you want to delete this contact?')) {
      this.contactService.deleteContact(contactId).subscribe(() => {
        // Refresh categories after successful deletion
        this.loadCategories();
      });
    }
  }
  toggleFavourite(contact: EditContact): void {
    contact.favourites = !contact.favourites; // Toggle isFavourite property
    this.contactService.modifyContact(contact).subscribe({
        next: (response) => {
          if(response.success){
            console.log(response.message)
          }
          else{
            alert(response.message)
          }
        },
        error:(error) => {
            console.error( error);
            contact.favourites = !contact.favourites;
        }
  });
}
  loadCategories():void{
    this.loading=true;
this.contactService.getAllContacts().subscribe({next:(response:ApiResponse<Contact[]>)=>{
  if(response.success){
    this.contacts=response.data;
  }
  else{
    console.error('Failed to fetch Category',response.message)
  }
  this.loading=false;
},error:(error)=>{
  console.error('Error fetching categories',error);
  this.loading=false;
}
});
}

}
