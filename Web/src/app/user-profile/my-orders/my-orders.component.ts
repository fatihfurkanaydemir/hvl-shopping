import { Component, OnInit } from '@angular/core';
import { exhaustMap, take } from 'rxjs';
import { ICustomer } from 'src/app/models/ICustomer';
import { IOrder, OrderStatus } from 'src/app/models/IOrder';
import { AuthService } from 'src/app/services/auth.service';
import { OrdersService } from 'src/app/services/orders.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.css'],
})
export class MyOrdersComponent implements OnInit {
  constructor(
    private ordersService: OrdersService,
    private authService: AuthService,
    private userService: UserService
  ) {}

  customer: ICustomer = {
    id: 0,
    identityId: '',
    addresses: [],
    email: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
  };

  pageNumber: number = 1;
  pageSize: number = 5;
  dataCount: number = 0;

  OrderStatus = OrderStatus;
  orders: IOrder[] = [];

  ngOnInit(): void {
    this.getOrders();
    this.getUser();
  }

  private getUser() {
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
          console.log(this.customer);
        },
      });
  }

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
          this.dataCount = +response.dataCount;
          console.log(this.orders);
        },
      });
  }

  onPageChange(newPageNumber: number) {
    this.pageNumber = newPageNumber;
    this.getOrders();
  }
}
