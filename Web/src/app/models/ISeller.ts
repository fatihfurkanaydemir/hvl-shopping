import { IAddress } from './IAddress';

export interface ISeller {
  id: number;
  identityId: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string;
  shopName: string;
  address: IAddress;
}
