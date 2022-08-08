import { IAddress } from './IAddress';

export interface ISellerRegister {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  shopName: string;
  address: IAddress;
}
