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
  ratingStarColors: any[] = [];
  setStarsFlag: boolean = true;
  setStar: number = 0;
  leaveArrayInput: number[] = [];

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

  adjustHoverColor(ratingStarIndex: number): void{
    console.log("hovered")
    const activate = "color: #fbc634 !important";
    const disable = "";
    switch(ratingStarIndex){
      case(1): this.changeTheRatingStarArray([0], activate); break;
      case(2): this.changeTheRatingStarArray([0, 1], activate); break;
      case(3): this.changeTheRatingStarArray([0, 1, 2], activate); break;
      case(4): this.changeTheRatingStarArray([0, 1, 2, 3], activate); break;
      case(5): this.changeTheRatingStarArray([0, 1, 2, 3, 4], activate); break;
      
      case(-1): if(this.checkIfSet(1)){this.changeTheRatingStarArray([0], disable); break;} else {break;}
      case(-2): if(this.checkIfSet(2)){this.changeTheRatingStarArray([0, 1], disable); break;} else {break;}
      case(-3): if(this.checkIfSet(3)){this.changeTheRatingStarArray([0, 1, 2], disable); break;} else {break;}
      case(-4): if(this.checkIfSet(4)){this.changeTheRatingStarArray([0, 1, 2, 3], disable); break;} else {break;}
      case(-5): if(this.checkIfSet(5)){this.changeTheRatingStarArray([0, 1, 2, 3, 4], disable); break;} else {break;}
      }
    }

    setStarColor(clickedStar: number){
      const activate = "color: #fbc634 !important";
      const disable = "";
      this.setStarsFlag = false;
      this.setStar = clickedStar;
      console.log(this.setStar)
      for(var i=0; i<5; i++){
        this.ratingStarColors[i] = disable;
      }
      
      for(var i=0; i<clickedStar; i++){
        this.ratingStarColors[i] = activate;
      }
    }

    private checkIfSet(starIndex: number): boolean{
      if(starIndex >= this.setStar){
        return true;
      } else return false;
    }

    onCommentSubmit(order: IOrder){
      this.comment = this.commentForm.value.comment;
      this.mapReviewObject(this.customer, this.comment, order);
      this.addReviewToProduct(this.myNewReview);
      console.log(this.myNewReview);
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

    private changeTheRatingStarArray(ratingStars: number[], change: string){
      //var myRatingStars = ratingStars.splice(0, this.setStar-1);
      ratingStars.forEach(item => {
        this.ratingStarColors[ratingStars[item]] = change, false;
      });
      console.log(this.ratingStarColors);
    }

    
}

