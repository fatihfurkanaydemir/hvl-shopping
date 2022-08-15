import { v4 as uuidv4 } from 'uuid';

export interface IBasket {
  id: string;
  items: IBasketItem[];
}

export interface IBasketItem {
  id: number;
  productName: string;
  sellerIdentityId: string;
  price: number;
  quantity: number;
  pictureUrl: string;
  categoryName: string;
}

export interface IBasketTotals {
  shipping: number;
  subtotal: number;
  total: number;
}
export class Basket implements IBasket {
  items: IBasketItem[] = [];
  id: string;

  constructor(id: string) {
    this.id = id;
  }
}
