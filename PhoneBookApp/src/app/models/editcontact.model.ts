import { Country } from "./country.model";
import { State } from "./state.model";

export interface EditContact {
    contactId : number,
    firstName: string,
    lastName: string,
    phone: string,
    email: string,
    company:string,
    gender: string,
    favourites: boolean,
    countryId: number,
    stateId: number,
    image: string,
    country: Country,
    state: State,
    imageByte:string;
    birthDate:Date|string| null;
}