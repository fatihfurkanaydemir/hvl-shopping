import { IAddress } from './IAddress';

export interface ICustomer {
  id: number;
  identityId: string;
  email: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  addresses: IAddress[];
}
