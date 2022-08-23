import { CurrencyPipe } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription, take, tap } from 'rxjs';
import { BasketService } from './basket/basket.service';
import { User } from './models/User';
import { AuthService } from './services/auth.service';
import { DiscountHubService } from './services/discounthub.service';
import { ToastService } from './services/toast.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'Havelsan Shopping';
  userSub?: Subscription;
  user?: User;

  constructor(
    private authService: AuthService,
    private router: Router,
    private basketService: BasketService,
    private discountHubService: DiscountHubService,
    private toastService: ToastService,
    private currencyPipe: CurrencyPipe
  ) {}

  ngOnInit(): void {
    this.authService.autoLogin();

    this.userSub = this.authService.userSubject.subscribe((user) => {
      this.user = user;
    });

    const basketId = localStorage.getItem('basket_id');
    if (basketId) {
      this.basketService.getBasket(basketId).subscribe(() => {
        console.log('initialised basket');
      });
    }
  }

  ngAfterContentInit() {
    this.discountHubService.start();

    this.discountHubService.OnDiscountCouponCreated.subscribe((notification) =>
      this.toastService.showNotification(
        'Yeni İndirim Kuponunuz Var!',
        `<strong class="fs-4">${this.currencyPipe.transform(
          notification.amount,
          'TRY',
          'symbol'
        )}</strong> değerindeki kupon kodunuz <br /> <strong class="text-primary fs-3">${
          notification.couponCode
        }</strong>`,
        'info'
      )
    );
  }

  goToPage(pageName: string) {
    this.router.navigate([`${pageName}`]);
  }

  logout() {
    this.authService.logout();
    this.basketService.logout();
    this.discountHubService.start();
  }

  ngOnDestroy(): void {
    this.userSub?.unsubscribe();
  }

  getProfileLink() {
    if (this.user?.isOnlyCustomer) return '/user-profile';
    if (this.user?.isOnlySeller) return '/seller-profile';
    return '';
  }
}
