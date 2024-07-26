import { ContactCountry } from "./contact.country.model";
import { ContactState } from "./contact.state.model";

export interface Contact{
    contactId:number;
    countryId:number;
    stateId:number;
    firstName:string;
    lastName:string;
    email:string;
    phone:string;
    image:string;
    country:ContactCountry;
    state:ContactState;
    imageByte:string;
    birthDate:Date|null;
    company:string;
    gender:string;
    favourites:boolean;

}