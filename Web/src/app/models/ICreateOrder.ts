export interface ICreateOrder {
  customerIdentityId: string;
  addressTitle: string;
  addressDescription: string;
  addressCity: string;
  shipmentPrice: number;
  couponCode?: string;
}
