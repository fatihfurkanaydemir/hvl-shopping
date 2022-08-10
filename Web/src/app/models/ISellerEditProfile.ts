import { IEditSellerAddress } from './IEditSellerAddress';

export interface ISellerEditProfile {
  identityId: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  shopName: string;
  address: IEditSellerAddress;
}
