import { IOrderProduct } from './IOrderProduct';

export const OrderStatus = {
  AwaitingPayment: 'AwaitingPayment',
  AwaitingShipment: 'AwaitingShipment',
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
  orderGroupId: string;
  created: Date;
  status: string;
  addressTitle: string;
  addressDescription: string;
  addressCity: string;
  totalProductPrice: number;
  shipmentPrice: number;
  products: IOrderProduct[];
  couponCode: string;
  couponAmount: number;
}
