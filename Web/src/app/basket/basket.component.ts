import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem, IBasketTotals } from '../models/IBasket';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';
import { BasketService } from './basket.service';
import { take, exhaustMap } from 'rxjs/operators';
import { IAddress } from '../models/IAddress';
import { OrdersService } from '../services/orders.service';
import { ICreateOrder } from '../models/ICreateOrder';
import { ICustomer } from '../models/ICustomer';
import { ToastService } from '../services/toast.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css'],
})
export class BasketComponent implements OnInit {
  basket$!: Observable<IBasket>;
  basketTotal$!: Observable<IBasketTotals>;
  basketItemsLength!: Observable<IBasket>;

  customer!: ICustomer;
  addresses: IAddress[] = [];
  selectedAddress: IAddress = this.addresses[0];

  constructor(
    private basketService: BasketService,
    private userService: UserService,
    private authService: AuthService,
    private orderService: OrdersService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    this.basketTotal$ = this.basketService.basketTotal$;
    this.getCustomer();
  }

  OnAddressClicked(address: IAddress) {
    this.selectedAddress = address;
  }

  getCustomer() {
    this.authService.userSubject
      .pipe(
        take(1),
        exhaustMap((user) => {
          return this.userService.getCustomerInfo(user.identityId);
        })
      )
      .subscribe({
        next: (response) => {
          this.customer = response.data;
          this.addresses = this.customer.addresses;
          this.selectedAddress = this.addresses[0];
        },
      });
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
    const order: ICreateOrder = {
      customerIdentityId: this.customer.identityId,
      addressCity: this.selectedAddress.city,
      addressDescription: this.selectedAddress.addressDescription,
      addressTitle: this.selectedAddress.title,
      shipmentPrice: 13,
    };

    this.orderService.createOrder(order).subscribe({
      next: (response) => {
        this.basketService.clearItems();
        document.location = response.data;
      },
      error: (error) => {
        this.toastService.showToast({
          icon: 'error',
          title: 'Sipariş oluşturulamadı.',
        });
      },
    });
  }
}
