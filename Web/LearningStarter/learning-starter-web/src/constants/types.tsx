//This type uses a generic (<T>).  For more information on generics see: https://www.typescriptlang.org/docs/handbook/2/generics.html
//You probably wont need this for the scope of this class :)
export type ApiResponse<T> = {
  data: T;
  errors: Error[];
  hasErrors: boolean;
};

export type Error = {
  property: string;
  message: string;
};

export type AnyObject = {
  [index: string]: any;
};

export type User = {
  firstName: string;
  lastName: string;
  userName: string;
};

export type ClassDto = {
  id: number;
  capacity: number;
  subject: string;
  userId: number;
  user: User;
};

export type DomicileLocationCreateDto = {
  name: string; 
  phoneNum: string;
  streetaddress1: string;
  streetAddress2: string;
  city: string;
  state: string;
  zipCode: number;
};
export type DomicileLocationGetDto = {
  id: number;
  name: string; 
  phoneNum: string;
  streetAddress1: string;
  streetAddress2: string;
  city: string;
  state: string;
  zipCode: number;
};
