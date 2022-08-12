import { Component, OnInit } from '@angular/core';
import { exhaustMap, take } from 'rxjs';
import { IOrder, OrderStatus } from 'src/app/models/IOrder';
import { AuthService } from 'src/app/services/auth.service';
import { OrdersService } from 'src/app/services/orders.service';

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.css'],
})
export class MyOrdersComponent implements OnInit {
  constructor(
    private ordersService: OrdersService,
    private authService: AuthService
  ) {}

  pageNumber: number = 1;
  pageSize: number = 5;
  dataCount: number = 0;

  OrderStatus = OrderStatus;

  ngOnInit(): void {
    this.getOrders();
  }

  orders: IOrder[] = [];

  getOrders() {
    this.authService.userSubject
      .pipe(
        take(1),
        exhaustMap((user) => {
          return this.ordersService.getAllOrdersByCustomerIdentityId(
            user.identityId,
            this.pageNumber,
            this.pageSize
          );
        })
      )
      .subscribe({
        next: (response) => {
          this.orders = response.data;
          this.dataCount = response.dataCount;
        },
      });
  }
}
