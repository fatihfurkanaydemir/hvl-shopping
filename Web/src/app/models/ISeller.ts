import { IAddress } from './IAddress';

export interface ISeller {
  id: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  shopName: string;
  address: IAddress;
}
