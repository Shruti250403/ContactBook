import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaginationContactComponent } from './pagination-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ChangeDetectorRef } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { ContactService } from 'src/app/services/contact.service';
import { Contact } from 'src/app/models/contact.model';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('PaginationContactComponent', () => {
  let component: PaginationContactComponent;
  let fixture: ComponentFixture<PaginationContactComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let cdrSpy: jasmine.SpyObj<ChangeDetectorRef>;
  let router: Router;

  const mockContacts: Contact[] = [
    {
      contactId: 1, firstName: 'Test', lastName: 'Test', email: 'Test@gmail.com', phone: '1234567890', gender: 'M', favourites: true, countryId: 1, stateId: 1, image: "", birthDate: new Date(), imageByte: '', country: { countryId: 1, countryName: 'India' }, state: { stateId: 1, stateName: 'Gujrat', countryId: 1 },
      company: 'Civica'
    },
    {
      contactId: 1, firstName: 'Test2', lastName: 'Test2', email: 'Test2@gmail.com', phone: '1244567890', gender: 'F', favourites: true, countryId: 1, stateId: 1, image: "", birthDate: new Date(), imageByte: '', country: { countryId: 1, countryName: 'India' }, state: { stateId: 1, stateName: 'Gujrat', countryId: 1 },
      company: 'Civica'
    },
    
  ];
  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['fetchContactCount','getAllPaginatedContacts','getAllContacts','modifyContact','deleteContact','calculateTotalPages']);
    authServiceSpy = jasmine.createSpyObj('AuthService', ['isAuthenticated']);
    cdrSpy = jasmine.createSpyObj('ChangeDetectorRef', ['detectChanges']);
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [PaginationContactComponent],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
      ],
    });
    fixture = TestBed.createComponent(PaginationContactComponent);
    component = fixture.componentInstance;
    //fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should calaulate total contact count without letter without search successfully',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.fetchContactCount.and.returnValue(of(mockResponse));

    //Act
    component.totalContactsCount();

    //Assert
    expect(contactServiceSpy.fetchContactCount).toHaveBeenCalled();

  })
  it('should calaulate total contact count with out letter successfully with search',()=>{
    //Arrange
    const search='e'
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.fetchContactCount.and.returnValue(of(mockResponse));

    //Act
    component.totalContactsCount(undefined,search);

    //Assert
    expect(contactServiceSpy.fetchContactCount).toHaveBeenCalled();

  })

  it('should fail to calculate total count without letter without search',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts'};
    contactServiceSpy.fetchContactCount.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.totalContactsCount();
    //Assert
    expect(contactServiceSpy.fetchContactCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })
  it('should fail to calculate total count without letter with search',()=>{
    //Arrange
    const search='e'
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts'};
    contactServiceSpy.fetchContactCount.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.totalContactsCount(undefined,search);
    //Assert
    expect(contactServiceSpy.fetchContactCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should handle Http error response',()=>{
    //Arrange
    const mockError = {message:'Network Error'};
    contactServiceSpy.fetchContactCount.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.totalContactsCount();

    //Assert
    expect(contactServiceSpy.fetchContactCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts',mockError);


  })

  it('should calaulate total contact count with letter successfully without letter',()=>{
    //Arrange
    const letter = 'A';
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.fetchContactCount.and.returnValue(of(mockResponse));

    //Act
    component.totalContactsCount(letter);

    //Assert
    expect(contactServiceSpy.fetchContactCount).toHaveBeenCalled();
  })
  it('should calaulate total contact count with letter successfully with letter with serach',()=>{
    //Arrange
    const letter = 'A';
    const search='e'
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.fetchContactCount.and.returnValue(of(mockResponse));

    //Act
    component.totalContactsCount(letter,search);

    //Assert
    expect(contactServiceSpy.fetchContactCount).toHaveBeenCalled();
  })

  it('should fail to calculate total count with letter without search ',()=>{
    //Arrange
    const letter = 'A';
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts'};
    contactServiceSpy.fetchContactCount.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.totalContactsCount(letter);
    //Assert
    expect(contactServiceSpy.fetchContactCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })
  it('should fail to calculate total count with letter with search ',()=>{
    //Arrange
    const letter = 'A';
    const search='e'

    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts'};
    contactServiceSpy.fetchContactCount.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.totalContactsCount(letter, search);
    //Assert
    expect(contactServiceSpy.fetchContactCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should handle Http error response',()=>{
    //Arrange
    const letter = 'A';
    const mockError = {message:'Network Error'};
    contactServiceSpy.fetchContactCount.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.totalContactsCount(letter);

    //Assert
    expect(contactServiceSpy.fetchContactCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts',mockError);
    expect(component.loading).toBe(false);


  })

  it('should load contacts without letter without search successfully',()=>{
    //Arrange
   
    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllPaginatedContacts.and.returnValue(of(mockResponse));

    //Act
    component.loadContacts();

    //Assert
    expect(contactServiceSpy.getAllPaginatedContacts).toHaveBeenCalled();
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })
  it('should load contacts without letter with search successfully',()=>{
    //Arrange
    const search='e'

    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllPaginatedContacts.and.returnValue(of(mockResponse));

    //Act
    component.loadContacts(undefined,search);

    //Assert
    expect(contactServiceSpy.getAllPaginatedContacts).toHaveBeenCalled();
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })


  it('should fail to load contacts  without letter without serach',()=>{
    //Arrange
    
    const mockResponse :ApiResponse<Contact[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.getAllPaginatedContacts.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.loadContacts();
    //Assert
    expect(contactServiceSpy.getAllPaginatedContacts).toHaveBeenCalled();
    expect(component.contacts).toEqual(undefined);
    expect(component.loading).toBe(false);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should fail to load contacts  without letter with serach',()=>{
    //Arrange
    const search='e'

    const mockResponse :ApiResponse<Contact[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.getAllPaginatedContacts.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.loadContacts(undefined,search);
    //Assert
    expect(contactServiceSpy.getAllPaginatedContacts).toHaveBeenCalled();
    expect(component.contacts).toEqual(undefined);
    expect(component.loading).toBe(false);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should handle Http error response',()=>{
    // Arrange
    
    const mockError = {message:'Network Error'};
    contactServiceSpy.getAllPaginatedContacts.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.loadContacts();

    //Assert
    expect(contactServiceSpy.getAllPaginatedContacts).toHaveBeenCalled();
    expect(component.contacts).toEqual(undefined);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts',mockError);
    expect(component.loading).toBe(false);


  })

  it('should load contacts with letter successfully without search',()=>{
    //Arrange
   const letter = 'R';
    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllPaginatedContacts.and.returnValue(of(mockResponse));

    //Act
    component.loadContacts(letter);

    //Assert
    expect(contactServiceSpy.getAllPaginatedContacts).toHaveBeenCalled();
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })
  it('should load contacts with letter successfully with search',()=>{
    //Arrange
    const search='e'

   const letter = 'R';
    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllPaginatedContacts.and.returnValue(of(mockResponse));

    //Act
    component.loadContacts(letter,search);

    //Assert
    expect(contactServiceSpy.getAllPaginatedContacts).toHaveBeenCalled();
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })

  it('should fail to load contacts  with letter without search',()=>{
    //Arrange
    const letter = 'R';
    const mockResponse :ApiResponse<Contact[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.getAllPaginatedContacts.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.loadContacts(letter);
    //Assert
    expect(contactServiceSpy.getAllPaginatedContacts).toHaveBeenCalled();
    expect(component.contacts).toEqual(undefined);
    expect(component.loading).toBe(false);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should fail to load contacts  with letter with search',()=>{
    //Arrange
    const letter = 'R';
    const search='e'

    const mockResponse :ApiResponse<Contact[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.getAllPaginatedContacts.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.loadContacts(letter,search);
    //Assert
    expect(contactServiceSpy.getAllPaginatedContacts).toHaveBeenCalled();
    expect(component.contacts).toEqual(undefined);
    expect(component.loading).toBe(false);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should handle Http error response',()=>{
    //Arrange
    const letter = 'R';
    const mockError = {message:'Network Error'};
    contactServiceSpy.getAllPaginatedContacts.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.loadContacts(letter);

    //Assert
    expect(contactServiceSpy.getAllPaginatedContacts).toHaveBeenCalled();
    expect(component.contacts).toEqual(undefined);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts',mockError);
    expect(component.loading).toBe(false);
  })


  it('should load all contacts successfully',()=>{
    //Arrange
   
    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContacts.and.returnValue(of(mockResponse));

    //Act
    component.loadAllContacts();

    //Assert
    expect(contactServiceSpy.getAllContacts).toHaveBeenCalled();
    expect(component.contactsForInitial).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })

  it('should fail to load all contacts',()=>{
    //Arrange
    
    const mockResponse :ApiResponse<Contact[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.getAllContacts.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.loadAllContacts();
    //Assert
    expect(contactServiceSpy.getAllContacts).toHaveBeenCalled();
    expect(component.contactsForInitial).toEqual([]);
    expect(component.loading).toBe(false);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should handle Http error response',()=>{
    //Arrange
    
    const mockError = {message:'Network Error'};
    contactServiceSpy.getAllContacts.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.loadAllContacts();

    //Assert
    expect(contactServiceSpy.getAllContacts).toHaveBeenCalled();
    expect(component.contactsForInitial).toEqual([]);
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts.',mockError);
    expect(component.loading).toBe(false);
  })

  it('should call loadContactsCount, loadAllContacts, and loadContacts on initialization', () => {
    // Mocking isAuthenticated to return true
    authServiceSpy.isAuthenticated.and.returnValue(of(true));

    // Spy on component methods
    spyOn(component, 'totalContactsCount');
    spyOn(component, 'loadAllContacts');
    spyOn(component, 'loadContacts');
    // Call ngOnInit
    component.ngOnInit();   
    // Expectations
    expect(component.totalContactsCount).toHaveBeenCalled();
    expect(component.loadAllContacts).toHaveBeenCalled();
    expect(component.loadContacts).toHaveBeenCalled();
   
  });

  it('should toggle favourite status of contact and handle success response', () => {
    // Arrange
    const mockContact: Contact = {
      contactId: 1,
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      phone: '1234567890',
      gender: 'm',
      favourites: false,
      countryId: 1,
      stateId: 1,
      image: '',
      birthDate: new Date(),
      imageByte: '',
      country: { countryId: 1, countryName: 'Country' },
      state: { stateId: 1, stateName: 'State', countryId: 1 },
      company: ''
    };
  
    // Mock editContact to return success
    const successResponse = { success: true, data:mockContact,message: 'Contact updated successfully' };
    contactServiceSpy.modifyContact.and.returnValue(of(successResponse));
    spyOn(console,'log')
    // Act
    component.toggleFavourite(mockContact);
  
    // Assert
    expect(mockContact.favourites).toBe(true); // Ensure isFavourite is toggled
    expect(contactServiceSpy.modifyContact).toHaveBeenCalledWith(mockContact);
    expect(console.log).toHaveBeenCalledWith(successResponse.message)
  });

  it('should toggle favourite status of contact and handle false response', () => {
    // Arrange
    const mockContact: Contact = {
      contactId: 1,
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      phone: '1234567890',
      gender: 'm',
      favourites: false,
      countryId: 1,
      stateId: 1,
      image: '',
      birthDate: new Date(),
      imageByte: '',
      country: { countryId: 1, countryName: 'Country' },
      state: { stateId: 1, stateName: 'State', countryId: 1 },
      company: ''
    };
  
    // Mock editContact to return success
    const successResponse = { success: false, data:mockContact,message: 'Contact updated successfully' };
    contactServiceSpy.modifyContact.and.returnValue(of(successResponse));
    spyOn(window,'alert')
    // Act
    component.toggleFavourite(mockContact);
  
    // Assert
    expect(mockContact.favourites).toBe(true); // Ensure isFavourite is toggled
    expect(contactServiceSpy.modifyContact).toHaveBeenCalledWith(mockContact);
    expect(window.alert).toHaveBeenCalledWith(successResponse.message)
  });

  it('should revert favourite status of contact and handle error response', () => {
    // Arrange
    const mockContact: Contact = {
      contactId: 1,
      firstName: 'Jane',
      lastName: 'Smith',
      email: 'jane.smith@example.com',
      phone: '0987654321',
      gender: 'f',
      favourites: true,
      countryId: 1,
      stateId: 1,
      image: '',
      birthDate: new Date(),
      imageByte: '',
      country: { countryId: 1, countryName: 'Country' },
      state: { stateId: 1, stateName: 'State', countryId: 1 },
      company: ''
    };
  
    // Mock editContact to return error
    const errorResponse = {message: 'Failed to update contact' };
    contactServiceSpy.modifyContact.and.returnValue(throwError(errorResponse));
    spyOn(console,'error');
  
    // Act
    component.toggleFavourite(mockContact);
  
    // Assert
    expect(mockContact.favourites).toBe(true); // Ensure isFavourite is reverted
    expect(contactServiceSpy.modifyContact).toHaveBeenCalledWith(mockContact);
    expect(console.error).toHaveBeenCalledWith(errorResponse);
  });
  it('should not delete category if user cancels', () => {
    const mockContacts: Contact = {
      contactId: 1,
      firstName: 'Jane',
      lastName: 'Smith',
      email: 'jane.smith@example.com',
      phone: '0987654321',
      gender: 'f',
      favourites: true,
      countryId: 1,
      stateId: 1,
      image: '',
      birthDate: new Date(),
      imageByte: '',
      country: { countryId: 1, countryName: 'Country' },
      state: { stateId: 1, stateName: 'State', countryId: 1 },
      company: ''
    };
    // Arrange
    const mockDeleteResponse: ApiResponse<Contact> = { success: false, data: mockContacts, message: 'Failed to delete category' };
    const contactId = 1;
    spyOn(window, 'confirm').and.returnValue(false);
    // spyOn(router, 'navigate').and.stub();
    contactServiceSpy.deleteContact.and.returnValue(of(mockDeleteResponse));

    // Act
    component.deleteContact(contactId);

    // Assert
    expect(window.confirm).toHaveBeenCalledWith("Are you sure you want to delete this contact?")
    expect(contactServiceSpy.deleteContact).not.toHaveBeenCalled();
    // expect(router.navigate).not.toHaveBeenCalled();
  });
  it('should delete contact on confirmation', () => {
    // Arrange
    const contactId = 1;
    const mockContacts: Contact = {
      contactId: 1,
      firstName: 'Jane',
      lastName: 'Smith',
      email: 'jane.smith@example.com',
      phone: '0987654321',
      gender: 'f',
      favourites: true,
      countryId: 1,
      stateId: 1,
      image: '',
      birthDate: new Date(),
      imageByte: '',
      country: { countryId: 1, countryName: 'Country' },
      state: { stateId: 1, stateName: 'State', countryId: 1 },
      company: ''
    };
    const mockDeleteResponse: ApiResponse<Contact> = { success: true, data: mockContacts, message: 'Contact deleted successfully' };
    spyOn(window, 'confirm').and.returnValue(true); // Simulate user confirmation
    spyOn(component, 'loadContacts');
    spyOn(component, 'totalContactsCount');
    spyOn(component, 'loadAllContacts');
    spyOn(component, 'calculateTotalPages');
    spyOn(component, 'onPageChange');
  component.calculateTotalPages();
    contactServiceSpy.deleteContact.and.returnValue(of(mockDeleteResponse));
  
    // Act
    component.deleteContact(contactId);
  
    // Assert
    expect(window.confirm).toHaveBeenCalledWith('Are you sure you want to delete this contact?');
    expect(contactServiceSpy.deleteContact).toHaveBeenCalledWith(contactId);
    expect(component.loadContacts).toHaveBeenCalledWith(component.selectedLetter);
    expect(component.totalContactsCount).toHaveBeenCalledWith(component.selectedLetter);
    expect(component.loadAllContacts).toHaveBeenCalled();
    expect(component.calculateTotalPages).toHaveBeenCalled();
    expect(component.onPageChange).not.toHaveBeenCalled(); // Check that onPageChange was not called for success case
  });
});
