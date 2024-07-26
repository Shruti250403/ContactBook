import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { AuthService } from 'src/app/services/auth.service';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-favourite-contact',
  templateUrl: './favourite-contact.component.html',
  styleUrls: ['./favourite-contact.component.css']
})
export class FavouriteContactComponent implements OnInit {
  contacts: any[] | undefined | null;
  contactsForInitial: Contact[] = [];
  contactId: number | undefined;
  loading: boolean = false;
  colors: string[] = ['red', 'blue', 'green', 'orange', 'purple', 'teal', 'pink', 'brown','red', 'blue', 'green', 'orange', 'purple', 'teal', 'pink', 'brown','red', 'blue', 'green', 'orange', 'purple', 'teal', 'pink', 'brown','pink','blue'];
  pageNumber: number = 1;
  
  pageSize: number = 2;
  totalItems: number = 0;
  totalPages: number = 0;
  isAuthenticated: boolean = false;
  letter: string | null = null;
  username: string | null | undefined;
  uniqueFirstLetters: string[] = [];
  contactForm: FormGroup; // Define your form group

  constructor(
    private formBuilder: FormBuilder,
    private contactService: ContactService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private authService: AuthService
  ) {
    this.contactForm = this.formBuilder.group({
      contactId: [0],
      firstName: ['',[Validators.required, Validators.minLength(2)]],
      lastName: ['',[Validators.required, Validators.minLength(2)]],
      company: ['',[Validators.required, Validators.minLength(2)]],
      phone: ['',Validators.required],
      countryId : [0, [Validators.required, this.contactValidator]],
      stateId : [0, [Validators.required, this.contactValidator]],
      email: ['',[Validators.required, Validators.email]],
      gender: [, Validators.required],
      favourites: [false],
      imageByte:[''],
      birthDate:[''],
      image:[null]
    });
  }

  ngOnInit(): void {
    this.loadContactsCount();
    this.loadAllContacts();
    this.authService.isAuthenticated().subscribe((authState: boolean) => {
      this.isAuthenticated = authState;
      this.cdr.detectChanges();
    });
    this.authService.getUsername().subscribe((username: string | null | undefined) => {
      this.username = username;
      this.cdr.detectChanges();
    });
  }
  contactValidator(control: any){
    return control.value ==''? {invalidContact:true}:null;
   }
  loadContactsCount(): void {
    if (this.letter) {
      this.getAllContactsCountWithLetter(this.letter);
    
    } else {
      this.getAllContactsCountWithoutLetter();     
    }
  }
  getAllContactsCountWithLetter(letter:string):void{
    this.contactService.getAllFavouriteContactsCount(letter).subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          console.log(response.data);
          this.totalItems = response.data;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          this.getAllContactsWithLetter(letter);
        } else {
          console.error('Failed to fetch contacts count', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts count.', error);
        this.loading = false;
      }
    });

  }
  getAllContactsCountWithoutLetter():void{
    this.contactService.getAllFavouriteContactsCount('').subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          console.log(response.data);
          this.totalItems = response.data;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          this.getAllContactsWithPagination();
        } else {
          console.error('Failed to fetch contacts count', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts count.', error);
        this.loading = false;
      }
    });

  }
  loadContacts(): void {
    this.loading = true;
    if (this.letter) {
      this.getAllContactsWithLetter(this.letter);
    } else {
      this.getAllContactsWithPagination();
    }
  }
  getAllContactsWithLetter(letter: string): void {
    this.loading = true;
    this.contactService.getAllFavouriteContactsWithPagination(this.pageNumber, this.pageSize, letter).subscribe({
      next: (response: ApiResponse<Contact[]>) => {
        if (response.success) {
          console.log(response.data);
          this.contacts = response.data;
        } else {
          console.error('Failed to fetch contacts', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts.', error);
        this.loading = false;
      }
    });
  }
  getAllContactsWithPagination(): void {
    if (this.pageNumber > this.totalPages) {
      console.log('Requested page does not exist.');
      return;
    }
  
    this.contactService.getAllFavouriteContactsWithPagination(this.pageNumber, this.pageSize,'').subscribe({
      next: (response: ApiResponse<Contact[]>) => {
        if (response.success) {
          console.log(response.data);
          this.contacts = response.data;
        } else {
          console.error('Failed to fetch contacts', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts.', error);
        this.loading = false;
      }
    });
  }
  
  filterByLetter(letter: string|null): void {
    this.letter = letter;
    this.pageNumber = 1;
    this.loadContactsCount();
  }
  changePageSize(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageNumber = 1; 
    this.totalPages = Math.ceil(this.totalItems / this.pageSize);
    this.loadContacts();
  }

  changePage(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.loadContacts();
  }

  getContactImage(contact: Contact): string {
    if (contact.imageByte) {
        return 'data:image/jpeg;base64,' + contact.imageByte;
    } else {
        return 'assets/Defaultimage.png'; // Path to your default image
    }
}

  confirmDelete(id:number):void{
    if(confirm('Are you sure?')){
      this.contactId = id;
      this.deleteContact(this.contactId);
    }
  }

  loadAllContacts(): void {
    this.loading = true;
    this.contactService.getAllFavouriteContacts().subscribe({
      next: (response: ApiResponse<Contact[]>) => {
        if (response.success) {
          console.log(response.data);
          this.contactsForInitial = response.data;
          this.updateUniqueFirstLetters();
        } else {
          console.error('Failed to fetch contacts', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts.', error);
        this.loading = false;
      }
    });
  }
  getUniqueFirstLetters(): string[] {
    // Extract first letters from contact names and filter unique letters
    const firstLetters = Array.from(new Set(this.contactsForInitial.map(contact => contact.firstName.charAt(0).toUpperCase())));
    return firstLetters.sort(); // Sort alphabetically
}
  updateUniqueFirstLetters(): void {
    this.uniqueFirstLetters = this.getUniqueFirstLetters();
}

  
  deleteContact(contactId:number):void{
    this.contactService.deleteContact(this.contactId).subscribe({
      next:(response)=>{
        if(response.success){
          this.loadContacts();
        }else{
          alert(response.message);
        }
      },
      error:(err)=>{
        alert(err.error.message);
      },
      complete:()=>{
        console.log('completed');
      }
    })
    this.router.navigate(['/pagination']);
  }
}
