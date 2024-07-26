import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AddContact } from 'src/app/models/addcontact.model';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-add-contact-tf',
  templateUrl: './add-contact-tf.component.html',
  styleUrls: ['./add-contact-tf.component.css']
})
export class AddContactTfComponent {
  contact : AddContact={
    firstName: '',
    lastName:'',
    phone: '',
    company: '',
    email:'',
    gender:'' ,
    favourites:true,
    countryId: 0,
    stateId : 0,
    image : 'girl.jpg',
    imageByte:'',
    birthDate:new Date(),
    country : {
      countryId : 0,
      countryName : ''
    },
    state : {
      stateId : 0,
      stateName : '',
      countryId : 0
    }

  };
  loading:boolean=false;
  country : Country[] = []
  state : State[] = []
  fileSizeError = false; 
  imageUrl: string | ArrayBuffer | null = null;
  @ViewChild('imageInput') imageInput!: ElementRef;
  constructor(
    private contactService : ContactService,
    private countryService : CountryService,
    private stateService: StateService,
    private router :Router) {}

    ngOnInit(): void {
      this.loadCountries();
    } 

    loadCountries() {
      
      this.countryService.getAllCountries().subscribe({
        next:(response : ApiResponse<Country[]>) => {
          if(response.success){
            
            this.country = response.data;
          } else {
            console.error('Failed to fetch countries' , response.message);
          }
        },
        error:(error=>{
          console.error('Error fetching countries :' ,error);
        })
      });
    }

    onSelectCountry(countryId: number) {
      this.contact.stateId = 0; // Reset the state selection
      this.state = []; // Clear the states list
      // Fetch states based on selected country
      this.stateService.getStateByCountryId(countryId).subscribe({
        next:(response : ApiResponse<State[]>) => {
          if(response.success){
            this.state = response.data;
          } else {
            console.error('Failed to fetch states' , response.message);
          }
        },
        error:(error=>{
          console.error('Error fetching states :' ,error);
        })
      });
    }
    checkFutureDate(event: any) {
      const selectedDate = new Date(event.target.value);
      const currentDate = new Date();

      if (selectedDate > currentDate) {

          this.contact.birthDate = new Date();
      }
    }
    onSubmit(myform:NgForm){
      if (myform.valid) {
        this.loading=true;
        if (this.imageUrl === null) {
          // If file has been removed, clear the imageByte and fileName in the contact object
          this.contact.imageByte = '';
          this.contact.image = 'girl.jpg';
        }
        console.log(myform.value);
        this.contactService.addContact(this.contact)
          .subscribe({
            next:(response)=> {
              if (response.success) {
              console.log("Contact created successfully:", response);
              this.router.navigate(['/pagination']);
              }
              else {
                this.imageUrl = null;
                alert(response.message);
              } 
              this.loading = false;
            },
            error: (err) => {
              console.log(err);
              this.loading = false;
              alert(err.error.message);
            },
            complete: () => {
              this.loading = false;
              console.log('completed');
            }
      });
      }     
  
    }
    
    onFileChange(event: any): void {
      const file = event.target.files[0];
      if (file) {
        const fileType = file.type;
        if (fileType === 'image/jpeg' || fileType === 'image/png' || fileType === 'image/jpg') {
          this.fileSizeError = true;
          const reader = new FileReader();
          reader.onload = () => {
            this.contact.imageByte = (reader.result as string).split(',')[1];
            this.contact.image = file.name;
            this.imageUrl = reader.result;
          };
          
          reader.readAsDataURL(file);
        } else {
          this.imageUrl=null;
        this.contact.imageByte='';
        this.contact.image='girl.jpg';
        this.imageInput.nativeElement.value = '';
          // Alert user about invalid file format
          alert('Invalid file format! Please upload an image in JPG, JPEG, or PNG format.');
           
        }
      }}
      removeFile() {
        this.imageUrl = null; // Clear the imageUrl variable to remove the image
        // You may want to also clear any associated form data here if needed
        this.contact.image = 'girl.jpg';
        this.imageInput.nativeElement.value = '';
    }
}
