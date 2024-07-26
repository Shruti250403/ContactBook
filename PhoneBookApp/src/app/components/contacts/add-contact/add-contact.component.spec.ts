import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddContactComponent } from './add-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ContactService } from 'src/app/services/contact.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

describe('AddContactComponent', () => {
  let component: AddContactComponent;
  let fixture: ComponentFixture<AddContactComponent>;

  beforeEach(async () => {
    TestBed.configureTestingModule({
      declarations: [AddContactComponent],
      imports: [ HttpClientTestingModule,ReactiveFormsModule,RouterTestingModule], // Import HttpClientModule
      providers: [ ContactService ]
    });
    fixture = TestBed.createComponent(AddContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(AddContactComponent);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });
});
