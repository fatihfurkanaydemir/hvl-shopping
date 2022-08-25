import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { Subject, take, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IDiscountCouponCreatedNotification } from '../models/IDiscountCouponCreatedNotification';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class DiscountHubService {
  private discountCouponCreatedNotification: Subject<IDiscountCouponCreatedNotification> =
    new Subject<IDiscountCouponCreatedNotification>();

  private client: HubConnection;

  constructor(private authService: AuthService) {
    this.client = new HubConnectionBuilder()
      .withUrl(`${environment.notificationServiceUrl}/discounthub`)
      .build();

    this.client.on(
      'Notify_DiscountCouponCreated',
      (notification: IDiscountCouponCreatedNotification) => {
        this.discountCouponCreatedNotification.next(notification);
      }
    );
  }

  get OnDiscountCouponCreated() {
    return this.discountCouponCreatedNotification.asObservable();
  }

  async start() {
    await this.stop();

    this.authService.userSubject
      .pipe(
        take(1),
        tap(async (user) => {
          if (user.isOnlyCustomer) {
            await this.client.start();

            await this.client.send('JoinGroup', 'Customer');
          }
        })
      )
      .subscribe();
  }

  async stop() {
    if (
      this.client.state === HubConnectionState.Disconnected ||
      this.client.state === HubConnectionState.Disconnecting
    )
      return;

    await this.client.send('LeaveGroup', 'Customer');
    await this.client.send('LeaveGroup', 'Admin');
    await this.client.send('LeaveGroup', 'Seller');

    await this.client.stop();
  }
}
