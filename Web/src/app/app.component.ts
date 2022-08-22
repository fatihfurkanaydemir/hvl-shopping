import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription, take, tap } from 'rxjs';
import { BasketService } from './basket/basket.service';
import { User } from './models/User';
import { AuthService } from './services/auth.service';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { IDiscountCouponCreatedNotification } from './models/IDiscountCouponCreatedNotification';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'Havelsan Shopping';
  userSub?: Subscription;
  user?: User;

  private signalRClient: HubConnection;

  constructor(
    private authService: AuthService,
    private router: Router,
    private basketService: BasketService
  ) {
    this.signalRClient = new HubConnectionBuilder()
      .withUrl(`${environment.notificationServiceUrl}/discounthub`)
      .build();

    this.signalRClient.on(
      'Notify_DiscountCouponCreated',
      (notification: IDiscountCouponCreatedNotification) => {
        console.log(notification);
      }
    );
  }

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
    this.signalRClient.start();
  }

  goToPage(pageName: string) {
    this.router.navigate([`${pageName}`]);
  }

  logout() {
    this.authService.logout();
    this.basketService.logout();
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
