import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ContactService } from './contact.service';
import { Contact } from '../models/contact.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { AddContact } from '../models/addcontact.model';
import { EditContact } from '../models/editcontact.model';

describe('ContactService', () => {
  let service: ContactService;
  let httpMock:HttpTestingController;
  const mockApiResponse:ApiResponse<Contact[]>={
    success:true,
    data:[
  {
    contactId: 1,
    countryId: 2,
    stateId: 2,
    firstName: "Aneri",
    lastName: "Gokani",
    email: "aneri@gmail.com",
    phone: "1234567891",
    company: 'CIVICA',
    image: "string",
    gender: "F",
    favourites: true,
    country: {
      countryId: 1,
      countryName: "country 1"
    },
    state: {
      countryId: 1,
      stateId: 2,
      stateName: "state 1"
    },
    imageByte: '',
    birthDate: new Date()
  }
  
      
    ],
    message:''
  }
  beforeEach(() => {
    TestBed.configureTestingModule({});
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[ContactService]
    });
    service=TestBed.inject(ContactService);
    httpMock=TestBed.inject(HttpTestingController)
  });
  afterEach(()=>{
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  it('should fetch all Categories successfully',()=>{
    //Arrange
    const apiUrl="http://localhost:5220/api/Contact/GetAllContacts";
    //Act
    service.getAllContacts().subscribe((response)=>{
      //Assert
      expect(response.data.length).toBe(1);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req=httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });
  it('should handle an empty categories list',()=>{
    //Arrange
    const apiUrl="http://localhost:5220/api/Contact/GetAllContacts";
    const emptyResponse:ApiResponse<Contact[]>={
      success:true,
      data:[
        
      ],
      message:''
    }
    //Act
    service.getAllContacts().subscribe((response)=>{
      //Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);
    });
    const req=httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });
  it('should handle HTTP error gracefully',()=>{
    //Arrange
    const apiUrl="http://localhost:5220/api/Contact/GetAllContacts";
    const errorMessage="Failed to load categories";
    //Act
    service.getAllContacts().subscribe(()=>fail('expected an error,not categories '),
  (error)=>{
    //Assert
    expect(error.status).toBe(500);
    expect(error.statusText).toBe('Internal server error');
  });
  const req=httpMock.expectOne(apiUrl);
  expect(req.request.method).toBe('GET');
  //Respond the error
  req.flush(errorMessage,{status:500,statusText:'Internal server error'});
  });


  it('shuould add contact',()=>{
    //arrange
   const addContact : AddContact ={
     firstName : "Test",
         lastName :"Test",
         phone : "1234567890",
         email: "Test@gmail.com",
         gender : "m",
         favourites : true,
         countryId : 0,
         stateId : 0,
         company:"Civica",
         country :{
           countryId : 1,
           countryName : "india"
         },
         state : {
           stateId : 1,
           stateName : "gujarat",
           countryId : 1
         },
         image: '',
         birthDate : new Date(),
         imageByte : "0x"
   }

   //Act
   const mockSuccessResponse :ApiResponse<string> ={
     success : true,
     message : "Contact saved successfully",
     data : ''
   }
    //act
    service.addContact(addContact).subscribe(response=>{
     expect(response).toBe(mockSuccessResponse);
   });
   const req = httpMock.expectOne('http://localhost:5220/api/Contact/Create');
   expect(req.request.method).toBe('POST');
   req.flush(mockSuccessResponse);
 });

 it('should handle an error while addition of contact',()=>{
   //Arrange
   const addContact : AddContact ={
     firstName : "Test",
         lastName :"Test",
         phone : "1234567890",
         company:"Civica",
         email: "Test@gmail.com",
         gender : "m",
         favourites : true,
         countryId : 0,
         stateId : 0,
         country :{
           countryId : 1,
           countryName : "india"
         },
         state : {
           stateId : 1,
           stateName : "gujarat",
           countryId : 1
         },
         image: '',
         birthDate : new Date(),
         imageByte : "0x"
   }
   const mockErrorResponse : ApiResponse<string> ={
     success :false,
     message : 'Contact already exist',
     data : " "
   };

   //Act
   service.addContact(addContact).subscribe(response=>{
     expect(response).toBe(mockErrorResponse);
   });
   const req = httpMock.expectOne('http://localhost:5220/api/Contact/Create');
   expect(req.request.method).toBe('POST');
   req.flush(mockErrorResponse);

 })

 it('should handle an http error for addition',()=>{
   //Arrange
   const addContact : AddContact ={
     firstName : "Test",
         lastName :"Test",
         phone : "1234567890",
         company:"Civica",
         email: "Test@gmail.com",
         gender : "m",
         favourites : true,
         countryId : 0,
         stateId : 0,
         country :{
           countryId : 1,
           countryName : "india"
         },
         state : {
           stateId : 1,
           stateName : "gujarat",
           countryId : 1
         },
         image: '',
         birthDate : new Date(),
         imageByte : "0x"
   }
   const mockHttpError ={
     statusText: "Internal Server Error",
     status: 500
     };
   //Act
    //Act
    service.addContact(addContact).subscribe({
     next:()=> fail('should have failed with the 500 error'),
     error: (error=> {
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual("Internal Server Error");
     })
  });
  const req = httpMock.expectOne('http://localhost:5220/api/Contact/Create');
   expect(req.request.method).toBe('POST');
   req.flush({},mockHttpError);
   

 });

 it('should fetch contact by id',()=>{
   const contactId = 1;
   const mockSuccessResponse :ApiResponse<EditContact>={
     success :true,
     data :{
       contactId: 1,
       firstName: "Test",
       lastName: "Test",
       phone: "1234567890",
       email: "Test@gmail.com",
       gender: "m",
       favourites: true,
       countryId: 1,
       stateId: 1,
       country: {
         countryId: 1,
         countryName: "india"
       },
       state: {
         stateId: 1,
         stateName: "gujarat",
         countryId: 1
       },
       image: '',
       birthDate: new Date(),
       imageByte: "0x",
       company: ''
     },
     message:''
   };
   //act
   service.getContactById(contactId).subscribe(response =>{
     //assert
     expect(response).toBe(mockSuccessResponse);
     expect(response.data.contactId).toEqual(contactId);

   });
   const req =httpMock.expectOne('http://localhost:5220/api/Contact/GetContactById/' +contactId);
   expect(req.request.method).toBe('GET');
   req.flush(mockSuccessResponse);
 });

 it('should handle failed contact retrival',()=>{
   //arrange
   const contactId =1;
   const mockErrorResponse : ApiResponse<EditContact>={
     success : false,
     data: {} as EditContact,
     message : "No record found"
   };
   //act
   service.getContactById(contactId).subscribe(response => {
     //assert
     expect(response).toEqual(mockErrorResponse);
     expect(response.message).toEqual("No record found");
     expect(response.success).toBeFalse();

   });
   const req =httpMock.expectOne('http://localhost:5220/api/Contact/GetContactById/' +contactId);
   expect(req.request.method).toBe('GET');
   req.flush(mockErrorResponse);

});

it('should handle http error for get by contact by id',()=>{
 const contactId =1;
 const mockHttpError ={
   status: 500,
   statusText :'Internal server error'
 };
 //act
 service.getContactById(contactId).subscribe({
   next : ()=> fail('should have faild with 500 error'),
   error:(error)=>{
     //assert
     expect(error.status).toBe(500);
     expect(error.statusText).toBe('Internal server error');
   }
 });
 const req =httpMock.expectOne('http://localhost:5220/api/Contact/GetContactById/' +contactId);
 expect(req.request.method).toBe('GET');
 req.flush({},mockHttpError);


});

it('should delete the contact by id successfully',()=>{
 //arrange
  const contactId =1;
   const mockSuccessResponse :ApiResponse<Contact>={
     success : false,
     data: {} as Contact,
     message : "deleted successfully"

   };
   service.deleteContact(contactId).subscribe(response=>{
     //Assert
     expect(response).toEqual(mockSuccessResponse);
     expect(response.message).toBe("deleted successfully");
     expect(response.data).toEqual;(mockSuccessResponse.data);

   });
   const req = httpMock.expectOne("http://localhost:5220/api/Contact/Remove/"+ contactId);
   expect(req.request.method).toBe('DELETE');
   req.flush(mockSuccessResponse);
})

it('should not delete the contact by id successfully',()=>{
 //arrange
  const contactId =1;
   const mockErrorResponse :ApiResponse<Contact>={
     success : false,
     data: {} as Contact,
     message : " not deleted successfully"

   };
   service.deleteContact(contactId).subscribe(response=>{
     //Assert
     expect(response).toEqual(mockErrorResponse);
     expect(response.message).toBe(" not deleted successfully");
     expect(response.data).toEqual;(mockErrorResponse.data);

   });
   const req = httpMock.expectOne("http://localhost:5220/api/Contact/Remove/"+ contactId);
   expect(req.request.method).toBe('DELETE');
   req.flush(mockErrorResponse);
});


it('should handle http error while deleting',()=>{
 const contactId =1;
 const mockHttpError ={
   status: 500,
   statusText :'Internal server error'
 };
 //act
 service.deleteContact(contactId).subscribe({
   next : ()=> fail('should have faild with 500 error'),
   error:(error)=>{
     //assert
     expect(error.status).toBe(500);
     expect(error.statusText).toBe('Internal server error');
   }
 });
 const req = httpMock.expectOne("http://localhost:5220/api/Contact/Remove/"+ contactId);
   expect(req.request.method).toBe('DELETE');
   req.flush({},mockHttpError);


});

it('should update a contact successfully', ()=>{
 const editContact : EditContact={
   contactId: 1,
   firstName: "Test",
   lastName: "Test",
   phone: "1234567890",
   email: "Test@gmail.com",
   gender: "m",
   favourites: true,
   countryId: 1,
   stateId: 1,
   country: {
     countryId: 1,
     countryName: "india"
   },
   state: {
     stateId: 1,
     stateName: "gujarat",
     countryId: 1
   },
   image: '',
   birthDate: new Date(),
   imageByte: "0x",
   company: ''
 }

 const mockSuccessResponse : ApiResponse<EditContact> ={
   success: true,
   message: "Contact updated successfully.",
   data: {} as EditContact
       }

 //Act
 service.modifyContact(editContact).subscribe(response => {
   expect(response).toEqual(mockSuccessResponse);
 });

 const req = httpMock.expectOne( 'http://localhost:5220/api/Contact/ModifyContact');
 expect(req.request.method).toBe('PUT');
 req.flush(mockSuccessResponse);

 });


 it('should handle failed update of contact', ()=>{
   const editContact : EditContact={
     contactId: 1,
     firstName: "Test",
     lastName: "Test",
     phone: "1234567890",
     email: "Test@gmail.com",
     gender: "m",
     favourites: true,
     countryId: 1,
     stateId: 1,
     country: {
       countryId: 1,
       countryName: "india"
     },
     state: {
       stateId: 1,
       stateName: "gujarat",
       countryId: 1
     },
     image: '',
     birthDate: new Date(),
     imageByte: "0x",
     company: ''
   }
  
   const mockErrorResponse : ApiResponse<EditContact> ={
     success: false,
     message: "Contact already exists.",
     data: {} as EditContact
         }
  
   //Act
   service.modifyContact(editContact).subscribe(response => {
     expect(response).toEqual(mockErrorResponse);
   });
  
   const req = httpMock.expectOne('http://localhost:5220/api/Contact/ModifyContact');
   expect(req.request.method).toBe('PUT');
   req.flush(mockErrorResponse);
  
   });

   it('should handle http error for update', ()=>{
     const editContact : EditContact={
       contactId: 1,
       firstName: "Test",
       lastName: "Test",
       phone: "1234567890",
       email: "Test@gmail.com",
       gender: "m",
       favourites: true,
       countryId: 1,
       stateId: 1,
       country: {
         countryId: 1,
         countryName: "india"
       },
       state: {
         stateId: 1,
         stateName: "Gujarat",
         countryId: 1
       },
       image: '',
       birthDate: new Date(),
       imageByte: "0x",
       company: ''
     }
    
     const mockHttpError ={
       statusText: "Internal Server Error",
       status: 500
       };
    
     //Act
     service.modifyContact(editContact).subscribe({
      next:()=> fail('should have failed with the 500 error'),
      error: (error=> {
       expect(error.status).toEqual(500);
       expect(error.statusText).toEqual("Internal Server Error");
      })
   });
    
     const req = httpMock.expectOne('http://localhost:5220/api/Contact/ModifyContact');
     expect(req.request.method).toBe('PUT');
     req.flush({},mockHttpError);
    
     });

     it('should get all paginated contact with letter and search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      const letter  = "d";
      const search = "yes"
     
  
       //act
       service.getAllPaginatedContacts(page,pageSize,sortOrder,letter,search).subscribe(response =>{
        //assert
        expect(response.data.length).toBe(1);
        expect(response.data).toEqual(mockApiResponse.data);
  
      });
      const req =httpMock.expectOne(`http://localhost:5220/api/Contact/GetAllContactsByPagination?letter=${letter}&search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockApiResponse);
  
    });
  
    it('should handle empty list of paginated contact with letter and search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      const letter  = "r";
      const search = "yes"
  
      const emptyResponse: ApiResponse<Contact[]> = {
        success: true,
        data: [],
        message: ''
      }
     
  
       //act
       service.getAllPaginatedContacts(page,pageSize,sortOrder,letter,search).subscribe(response =>{
        //assert
        expect(response.data.length).toBe(0);
        expect(response.data).toEqual([]);
  
      });
      const req =httpMock.expectOne(`http://localhost:5220/api/Contact/GetAllContactsByPagination?letter=${letter}&search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
      expect(req.request.method).toBe('GET');
      req.flush(emptyResponse);
  
    });
  
    it('should handle HTTP error while fetching paginated contact with letter and search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      const letter  = "w";
      const search = "yes"
      const mockHttpError ={
        statusText: "Internal Server Error",
        status: 500
        };
      const ApiUrl = `http://localhost:5220/api/Contact/GetAllContactsByPagination?letter=${letter}&search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`
      
      //act
      service.getAllPaginatedContacts(page,pageSize,sortOrder,letter,search).subscribe({
        next:()=> fail('should have failed with the 500 error'),
        error: (error=> {
         expect(error.status).toEqual(500);
         expect(error.statusText).toEqual("Internal Server Error");
         
        })
     });
      const req =httpMock.expectOne(ApiUrl);
      expect(req.request.method).toBe('GET');
      req.flush({},mockHttpError);
  
    });
  
     it('should get all paginated contact without letter and with search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      const search = "yes"
     
  
       //act
       service.getAllPaginatedContacts(page,pageSize,sortOrder,undefined,search).subscribe(response =>{
        //assert
        expect(response.data.length).toBe(1);
        expect(response.data).toEqual(mockApiResponse.data);
  
      });
      const req =httpMock.expectOne(`http://localhost:5220/api/Contact/GetAllContactsByPagination?search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockApiResponse);
  
    });
  
    it('should handle empty list of paginated contact without letter and with search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      const search = "yes"
  
      const emptyResponse: ApiResponse<Contact[]> = {
        success: true,
        data: [],
        message: ''
      }
     
  
       //act
       service.getAllPaginatedContacts(page,pageSize,sortOrder,undefined,search).subscribe(response =>{
        //assert
        expect(response.data.length).toBe(0);
        expect(response.data).toEqual([]);
  
      });
      const req =httpMock.expectOne(`http://localhost:5220/api/Contact/GetAllContactsByPagination?search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
      expect(req.request.method).toBe('GET');
      req.flush(emptyResponse);
  
    });
  
    it('should handle HTTP error while fetching paginated contact without letter and with search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      const search = "yes"
      const mockHttpError ={
        statusText: "Internal Server Error",
        status: 500
        };
      const ApiUrl = `http://localhost:5220/api/Contact/GetAllContactsByPagination?search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`
      
      //act
      service.getAllPaginatedContacts(page,pageSize,sortOrder,undefined,search).subscribe({
        next:()=> fail('should have failed with the 500 error'),
        error: (error=> {
         expect(error.status).toEqual(500);
         expect(error.statusText).toEqual("Internal Server Error");
         
        })
     });
      const req =httpMock.expectOne(ApiUrl);
      expect(req.request.method).toBe('GET');
      req.flush({},mockHttpError);
  
    });
  

     it('should get all paginated contact with letter and without search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      const letter  = "d";
     
  
       //act
       service.getAllPaginatedContacts(page,pageSize,sortOrder,letter).subscribe(response =>{
        //assert
        expect(response.data.length).toBe(1);
        expect(response.data).toEqual(mockApiResponse.data);
  
      });
      const req =httpMock.expectOne(`http://localhost:5220/api/Contact/GetAllContactsByPagination?letter=${letter}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockApiResponse);
  
    });
  
    it('should handle empty list of paginated contact with letter and without search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      const letter  = "r";
  
      const emptyResponse: ApiResponse<Contact[]> = {
        success: true,
        data: [],
        message: ''
      }
     
  
       //act
       service.getAllPaginatedContacts(page,pageSize,sortOrder,letter).subscribe(response =>{
        //assert
        expect(response.data.length).toBe(0);
        expect(response.data).toEqual([]);
  
      });
      const req =httpMock.expectOne(`http://localhost:5220/api/Contact/GetAllContactsByPagination?letter=${letter}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
      expect(req.request.method).toBe('GET');
      req.flush(emptyResponse);
  
    });
  
    it('should handle HTTP error while fetching paginated contact with letter and without search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      const letter  = "w";

      const mockHttpError ={
        statusText: "Internal Server Error",
        status: 500
        };
      const ApiUrl = `http://localhost:5220/api/Contact/GetAllContactsByPagination?letter=${letter}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`
      
      //act
      service.getAllPaginatedContacts(page,pageSize,sortOrder,letter).subscribe({
        next:()=> fail('should have failed with the 500 error'),
        error: (error=> {
         expect(error.status).toEqual(500);
         expect(error.statusText).toEqual("Internal Server Error");
         
        })
     });
      const req =httpMock.expectOne(ApiUrl);
      expect(req.request.method).toBe('GET');
      req.flush({},mockHttpError);
  
    });
  
    it('should get all paginated contact without letter and search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
     
     
  
       //act
       service.getAllPaginatedContacts(page,pageSize,sortOrder).subscribe(response =>{
        //assert
        expect(response.data.length).toBe(1);
        expect(response.data).toEqual(mockApiResponse.data);
  
      });
      const req =httpMock.expectOne(`http://localhost:5220/api/Contact/GetAllContactsByPagination?page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockApiResponse);
  
    });

    it('should handle empty list of paginated contact without letter and search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      
  
      const emptyResponse: ApiResponse<Contact[]> = {
        success: true,
        data: [],
        message: ''
      }
     
  
       //act
       service.getAllPaginatedContacts(page,pageSize,sortOrder).subscribe(response =>{
        //assert
        expect(response.data.length).toBe(0);
        expect(response.data).toEqual([]);
  
      });
      const req =httpMock.expectOne(`http://localhost:5220/api/Contact/GetAllContactsByPagination?page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
      expect(req.request.method).toBe('GET');
      req.flush(emptyResponse);
  
    });
  
    it('should handle HTTP error while fetching paginated contact without letter and search',()=>{
      //Arrange
      const page = 1;
      const pageSize = 2;
      const sortOrder = "asc";
      
      const mockHttpError ={
        statusText: "Internal Server Error",
        status: 500
        };
      const ApiUrl = `http://localhost:5220/api/Contact/GetAllContactsByPagination?page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`
      
      //act
      service.getAllPaginatedContacts(page,pageSize,sortOrder).subscribe({
        next:()=> fail('should have failed with the 500 error'),
        error: (error=> {
         expect(error.status).toEqual(500);
         expect(error.statusText).toEqual("Internal Server Error");
         
        })
     });
      const req =httpMock.expectOne(ApiUrl);
      expect(req.request.method).toBe('GET');
      req.flush({},mockHttpError);
  
    });
  
    it('should fetch all contact count successfully with letter and search', () => {
      //Arrange
      const letter="a";
      const search="search";
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount?letter=${letter}&search=${search}`;
      const mockApiResponse = { data: 2 }; 
      //Act
      service.fetchContactCount(letter,search).subscribe((response) => {
        //Assert
        expect(response.data).toBe(2);
        expect(response.data).toEqual(mockApiResponse.data);
  
      });
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockApiResponse);
    });
  
    it('should handle zero count with letter and search ', () => {
      //Arrange
      const letter="a";
      const search="search";
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount?letter=${letter}&search=${search}`;
      const mockApiResponse = { data: 0 }; 
      const emptyResponse: ApiResponse<number> = {
        success: true,
        data: 0,
        message: ''
      }
      //Act
      service.fetchContactCount(letter,search).subscribe((response) => {
        //Assert
        expect(response.data).toBe(0);
        expect(response.data).toEqual(mockApiResponse.data);
      });
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(emptyResponse);
    });
  
    it('should handle HTTP error gracefully with letter and search', () => {
      //Arrange
      const letter="a";
      const search="search";
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount?letter=${letter}&search=${search}`;
      const errorMessage = 'Failed to load contacts';
      //Act
      service.fetchContactCount(letter,search).subscribe(
        () => fail('expected an error, not contacts'),
        (error) => {
          //Assert
          expect(error.status).toBe(500);
          expect(error.statusText).toBe('Internal Server Error');
        }
      );
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      //Respond with error
      req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
    });
  
  
    it('should fetch all contact count successfully with letter and without search', () => {
      //Arrange
      const letter="a";
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount?letter=${letter}`;
      const mockApiResponse = { data: 2 }; 
      //Act
      service.fetchContactCount(letter).subscribe((response) => {
        //Assert
        expect(response.data).toBe(2);
        expect(response.data).toEqual(mockApiResponse.data);
  
      });
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockApiResponse);
    });
  
    it('should handle zero count with letter and without search', () => {
      //Arrange
      const letter="a";
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount?letter=${letter}`;
      const mockApiResponse = { data: 0 }; 
      const emptyResponse: ApiResponse<number> = {
        success: true,
        data: 0,
        message: ''
      }
      //Act
      service.fetchContactCount(letter).subscribe((response) => {
        //Assert
        expect(response.data).toBe(0);
        expect(response.data).toEqual(mockApiResponse.data);
      });
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(emptyResponse);
    });
  
    it('should handle HTTP error gracefully with letter and without search', () => {
      //Arrange
      const letter="a";
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount?letter=${letter}`;
      const errorMessage = 'Failed to load contacts';
      //Act
      service.fetchContactCount(letter).subscribe(
        () => fail('expected an error, not contacts'),
        (error) => {
          //Assert
          expect(error.status).toBe(500);
          expect(error.statusText).toBe('Internal Server Error');
        }
      );
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      //Respond with error
      req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
    });
  
    it('should fetch all contact count successfully without letter and with search', () => {
      //Arrange
      
      const search="no";
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount?search=${search}`;
      const mockApiResponse = { data: 2 }; 
      //Act
      service.fetchContactCount(undefined,search).subscribe((response) => {
        //Assert
        expect(response.data).toBe(2);
        expect(response.data).toEqual(mockApiResponse.data);
  
      });
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockApiResponse);
    });
  
    it('should handle zero count without letter and with search', () => {
      //Arrange
      const search="no";
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount?search=${search}`;
      const mockApiResponse = { data: 0 }; 
      const emptyResponse: ApiResponse<number> = {
        success: true,
        data: 0,
        message: ''
      }
      //Act
      service.fetchContactCount(undefined,search).subscribe((response) => {
        //Assert
        expect(response.data).toBe(0);
        expect(response.data).toEqual(mockApiResponse.data);
      });
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(emptyResponse);
    });
  
    it('should handle HTTP error gracefully without letter and with search', () => {
      //Arrange
      const search="no";
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount?search=${search}`;
      const errorMessage = 'Failed to load contacts';
      //Act
      service.fetchContactCount(undefined,search).subscribe(
        () => fail('expected an error, not contacts'),
        (error) => {
          //Assert
          expect(error.status).toBe(500);
          expect(error.statusText).toBe('Internal Server Error');
        }
      );
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      //Respond with error
      req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
    });

    it('should fetch all contact count successfully without letter and search', () => {
      //Arrange
      
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount`;
      const mockApiResponse = { data: 2 }; 
      //Act
      service.fetchContactCount(undefined,undefined).subscribe((response) => {
        //Assert
        expect(response.data).toBe(2);
        expect(response.data).toEqual(mockApiResponse.data);
  
      });
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockApiResponse);
    });
  
    it('should handle zero count without letter and  search', () => {
      //Arrange
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount`;
      const mockApiResponse = { data: 0 }; 
      const emptyResponse: ApiResponse<number> = {
        success: true,
        data: 0,
        message: ''
      }
      //Act
      service.fetchContactCount(undefined,undefined).subscribe((response) => {
        //Assert
        expect(response.data).toBe(0);
        expect(response.data).toEqual(mockApiResponse.data);
      });
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(emptyResponse);
    });
  
    it('should handle HTTP error gracefully without letter and with search', () => {
      //Arrange
      const apiUrl = `http://localhost:5220/api/Contact/GetContactsCount`;
      const errorMessage = 'Failed to load contacts';
      //Act
      service.fetchContactCount(undefined,undefined).subscribe(
        () => fail('expected an error, not contacts'),
        (error) => {
          //Assert
          expect(error.status).toBe(500);
          expect(error.statusText).toBe('Internal Server Error');
        }
      );
      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      //Respond with error
      req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
    });
  
  
  
  
    
});
