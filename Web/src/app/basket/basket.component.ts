import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem, IBasketTotals } from '../models/IBasket';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css'],
})
export class BasketComponent implements OnInit {
  basket$!: Observable<IBasket>;
  basketTotal$!: Observable<IBasketTotals>;
  basketItemsLength!: Observable<IBasket>;

  constructor(private basketService: BasketService) {}

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    this.basketTotal$ = this.basketService.basketTotal$;
    //console.log(this.basket$);
  }

  removeBasketItem(item: IBasketItem) {
    this.basketService.removeItem(item);
  }

  incrementQuantity(item: IBasketItem) {
    this.basketService.incrementItemQuantity(item);
  }

  decrementQuantity(item: IBasketItem) {
    this.basketService.decrementItemQuantity(item);
  }

  getBasketItemsLength() {
    return this.basketService.getItemsLength();
  }

  OnCheckout() {
    console.log(this.basketService.getCurrentBasketValue());
  }
}
