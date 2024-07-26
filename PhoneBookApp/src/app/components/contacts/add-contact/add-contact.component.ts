import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html',
  styleUrls: ['./add-contact.component.css']
})
export class AddContactComponent implements OnInit {
  loading: boolean = false;
  country: Country[] = [];
  state: State[] = [];
  contactForm!: FormGroup;
  imageUrl: string | ArrayBuffer | null = null;
  @ViewChild('imageInput') imageInput!: ElementRef;
  fileSizeExceeded =  false;
fileFormatInvalid =  false;
  constructor(
    private contactService: ContactService,
    private countryService: CountryService, 
    private stateService: StateService, 
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.contactForm = this.fb.group({

      firstName: ['',[Validators.required, Validators.minLength(2)]],
      lastName: ['',[Validators.required, Validators.minLength(2)]],
      company: ['',[Validators.required, Validators.minLength(2)]],
      phone: ['',[Validators.required, Validators.minLength(10), Validators.maxLength(10)]],
      countryId : [0, [Validators.required, this.contactValidator]],
      stateId : [0, [Validators.required, this.contactValidator]],
      email: ['',[Validators.required, Validators.email]],
      gender: [, Validators.required],
      favourites: [false],
      image: ['girl.jpg'],
      imageByte:[''],
      birthDate:['']
    })
    this.loadCountries();
    this.fetchStateByCountry();

     }

     get formControl(){
      return this.contactForm.controls;
     }
  
     contactValidator(control: any){
      return control.value ==''? {invalidContact:true}:null;
     }
     onFileChange(event: any): void {
      const file = event.target.files[0];
      if (file) {
        const fileType = file.type; // Get the MIME type of the file
        if (fileType === 'image/jpeg' || fileType === 'image/png' || fileType === 'image/jpg')
           {
            if(file.size > 50 * 1024)
              {
                this.fileSizeExceeded = true; // Set flag to true if file size exceeds the limit
                const inputElement = document.getElementById('imageByte') as HTMLInputElement;
                inputElement.value = '';
                return;
              }
              this.fileFormatInvalid = false;
              this.fileSizeExceeded = false; 
        const reader = new FileReader();
        reader.onload = () => {
          this.contactForm.patchValue({
            imageByte: (reader.result as string).split(',')[1],
            image: file.name // Patch the value of the form control
          });
          // You might want to remove this line if you're not using it in the template
          this.imageUrl = reader.result;
          console.log(file.name);
        };
        reader.readAsDataURL(file);
      }
      else {
        this.fileFormatInvalid = true;
        const inputElement = document.getElementById('imageByte') as HTMLInputElement;
        inputElement.value = '';
         
      }
      }
    }

     loadCountries():void{
      this.loading = true;
      this.countryService.getAllCountries().subscribe({

        
        next:(response: ApiResponse<Country[]>) =>{
          if(response.success){
            this.country = response.data;
          }
          else{
            console.error('Failed to fetch countries', response.message);
          }
          this.loading = false;
        },
        error:(error => {
          console.error('Failed to fetch countries', error);
          this.loading = false;
        })
      }
    )
    }

    fetchStateByCountry(): void {
      this.contactForm.get('countryId')?.valueChanges.subscribe((countryId: number) => {
        if (countryId !== 0) {
          this.loading = true;
          this.stateService.getStateByCountryId(countryId).subscribe({
            next: (response: ApiResponse<State[]>) => {
              if (response.success) {
                this.state = response.data;
              } else {
                console.error('Failed to fetch states', response.message);
              }
              this.loading = false;
            },
            error: (error) => {
              console.error('Failed to fetch states', error);
              this.loading = false;
            }
          });
        }
      });
    }
    removeFile() {
      this.contactForm.patchValue({
        imageUrl: null,
        image: null
      });
      this.imageInput.nativeElement.value = '';
      this.imageUrl = null; // Also reset imageUrl to remove the displayed image
      const inputElement = document.getElementById('imageByte') as HTMLInputElement;
      inputElement.value = '';
 
    }

    OnSubmit(){
      this.loading = true;
  
      if(this.contactForm.valid){
        if (!this.contactForm.get('birthDate')?.value) {
          this.contactForm.get('birthDate')?.setValue(null);
        }
        if(this.imageUrl === null)
          {
            this.contactForm.patchValue({
                imageByte : '',
                fileName: null
            })
          }
        console.log(this.contactForm.value);
        this.contactService.addContact(this.contactForm.value).subscribe({
          next: (response) => {
            if (response.success) {
              this.router.navigate(['/pagination']);
            }
            else{
              this.imageUrl=null;
              alert(response.message)
            }
          },
          error:(err)=>{
            alert(err.error.message);
            console.log(err);
            this.loading = false;
  
          },
          complete:()=>{
            console.log("Completed");
            this.loading = false;
  
          }
        })
      }
    }
  }