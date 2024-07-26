import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Contact } from 'src/app/models/contact.model';
import { EditContact } from 'src/app/models/editcontact.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-detail-contact',
  templateUrl: './detail-contact.component.html',
  styleUrls: ['./detail-contact.component.css']
})
export class DetailContactComponent {
  contactId:number|undefined;
  contact:EditContact={
    contactId: 0,
    phone: '',
    countryId: 0,
    country: {
      countryId: 0,
      countryName: ''
    },
    stateId: 0,
    state: {
      countryId: 0,
      stateId: 0,
      stateName: ''
    },
    firstName: '',
    lastName: '',
    email: '',
    gender: '',
    company: '',
    favourites: false,
    image: '',
    imageByte: '',
    birthDate: ''
  };

  constructor(private contactService:ContactService,private route:ActivatedRoute, private router:Router){}
  ngOnInit(): void {
    const contactId = Number(this.route.snapshot.paramMap.get('id'));
    this.contactService.getContactById(contactId).subscribe({
      next: (response) => {
        if (response.success) {
          this.contact = response.data;
        } else {
          console.error('Failed to fetch contact', response.message);
        }
      },
      error: (error) => {
        console.error('Failed to fetch contact', error);
      },
    });
  }
  
  deleteContact(contactId: number) {
    if (confirm('Are you sure you want to delete this contact?')) {
      this.contactService.deleteContact(contactId).subscribe(() => {
        this.router.navigate(['/contacts']);

      });
    }
  }
}
