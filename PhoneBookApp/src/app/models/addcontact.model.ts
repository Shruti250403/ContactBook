import { Country } from "./country.model";
import { State } from "./state.model";

export interface AddContact{
    firstName: string,
  lastName: string,
  email: string,
  phone: string,
  company: string,
  image: string,
  gender: string,
  favourites: true,
  countryId: 0,
  stateId: 0,
  country: Country,
    state: State,
  imageByte:string,
  birthDate:Date|null,
}