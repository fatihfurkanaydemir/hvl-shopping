import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { OrdersService } from 'src/app/services/orders.service';
import { take, exhaustMap } from 'rxjs/operators';
import { IOrder } from 'src/app/models/IOrder';

@Component({
  selector: 'app-seller-manageorders-tab',
  templateUrl: './seller-manageorders-tab.component.html',
  styleUrls: ['./seller-manageorders-tab.component.css'],
})
export class SellerManageordersTabComponent implements OnInit {
  orders: IOrder[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  dataCount: number = 0;

  constructor(
    public ordersService: OrdersService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders() {
    this.authService.userSubject
      .pipe(
        take(1),
        exhaustMap((user) => {
          return this.ordersService.getAllOrdersBySellerIdentityId(
            user.identityId,
            this.pageNumber,
            this.pageSize
          );
        })
      )
      .subscribe((response) => {
        this.orders = response.data;
        this.dataCount = +response.dataCount;
      });
  }

  onPageChange(newPageNumber: number) {
    this.pageNumber = newPageNumber;
    this.getOrders();
  }
}
