import { IOrderProduct } from './IOrderProduct';

export const OrderStatus = {
  Created: 'Created',
  Canceled: 'Canceled',
  Shipped: 'Shipped',
  Returned: 'Returned',
};

export interface IOrder {
  customerIdentityId: string;
  customerFirstName: string;
  customerLastName: string;
  customerPhoneNumber: string;
  sellerIdentityId: string;
  created: Date;
  status: string;
  addressTitle: string;
  addressDescription: string;
  addressCity: string;
  totalPrice: number;
  products: IOrderProduct[];
}
