import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { exhaustMap, take } from 'rxjs';
import { ICustomer } from 'src/app/models/ICustomer';
import { IOrder, OrderStatus } from 'src/app/models/IOrder';
import { IOrderProduct } from 'src/app/models/IOrderProduct';
import { IProduct } from 'src/app/models/IProduct';
import { IReview } from 'src/app/models/IReview';
import { AuthService } from 'src/app/services/auth.service';
import { OrdersService } from 'src/app/services/orders.service';
import { ReviewsService } from 'src/app/services/reviews.service';
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
    private userService: UserService,
    private reviewsService: ReviewsService
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

  comment!: string;

  commentForm: FormGroup = new FormGroup({
    comment: new FormControl(this.comment)
  })

  pageNumber: number = 1;
  pageSize: number = 5;
  dataCount: number = 0;
  
  visible = false;
  setStar: number = 0;

  myNewReview!: IReview;

  OrderStatus = OrderStatus;
  orders: IOrder[] = [];
  
  ngOnInit(): void {
    this.getOrders();
    this.getUser();
  }

  private getUser(){
    this.authService.userSubject.pipe(
      take(1),
      exhaustMap((user) => {
        return this.userService.getCustomerInfo(user.identityId);
      })
    ).subscribe({
      next: (response) => {
        this.customer = response.data;
        console.log(this.customer);
      }
    })
  }

  addReviewToProduct(review: IReview){
    this.reviewsService.addReviewToProduct(review).subscribe();
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

  toggleCollapse(): void {
    this.visible = !this.visible;
  }

    onCommentSubmit(order: IOrder){
      this.comment = this.commentForm.value.comment;
      this.mapReviewObject(this.customer, this.comment, order);
      this.addReviewToProduct(this.myNewReview);
    }

    changeSetStar(rate: string){
      this.setStar = Number(rate);
      console.log("returned", rate);
      console.log(this.setStar);
    }

    mapReviewObject(customer: ICustomer, comment: string, order: IOrder)
    {
      this.myNewReview = {
        productId : order.products[0].productId,
        name : customer.firstName,
        lastName : customer.lastName,
        comment: comment,
        rate: this.setStar
      }
    }
}

