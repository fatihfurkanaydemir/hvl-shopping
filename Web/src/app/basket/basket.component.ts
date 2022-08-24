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
import { CouponsService } from '../services/coupons.service';
import { ICoupon } from '../models/ICoupon';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';

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

  coupons: ICoupon[] = [];
  selectedCoupon?: ICoupon;

  addresses: IAddress[] = [];
  selectedAddress: IAddress = this.addresses[0];

  closeResult: string = '';

  constructor(
    private basketService: BasketService,
    private userService: UserService,
    private authService: AuthService,
    private orderService: OrdersService,
    private toastService: ToastService,
    private couponService: CouponsService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    this.basketTotal$ = this.basketService.basketTotal$;
    this.getCustomer();
  }

  OnAddressClicked(address: IAddress) {
    this.selectedAddress = address;
  }

  OnCouponClicked(coupon: ICoupon) {
    if (this.selectedCoupon === coupon) {
      this.selectedCoupon = undefined;
    } else {
      this.selectedCoupon = coupon;
    }
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

  getCoupons() {
    this.basketTotal$.subscribe((basketTotal) => {
      this.authService.userSubject
        .pipe(
          take(1),
          exhaustMap((user) => {
            return this.couponService.getCustomerUsableCoupons(user.identityId);
          })
        )
        .subscribe({
          next: (response) => {
            this.coupons = response.data.filter((coupon: ICoupon) => {
              return coupon.amount < basketTotal.total;
            });
          },
        });
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
    if (!this.selectedAddress) {
      this.toastService.showToast({
        icon: 'error',
        title: 'Lütfen bir adres seçin veya profil sayfasından ekleyin.',
      });
      return;
    }

    const order: ICreateOrder = {
      customerIdentityId: this.customer.identityId,
      addressCity: this.selectedAddress.city,
      addressDescription: this.selectedAddress.addressDescription,
      addressTitle: this.selectedAddress.title,
      shipmentPrice: 13,
      couponCode: this.selectedCoupon?.code,
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
        console.log(error);
      },
    });
  }

  openModal(content: any) {
    this.getCoupons();

    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
        },
        (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        }
      );
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}
