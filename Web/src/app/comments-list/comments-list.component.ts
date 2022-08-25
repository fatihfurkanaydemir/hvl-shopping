import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ICustomer } from '../models/ICustomer';
import { IProduct } from '../models/IProduct';
import { IReview } from '../models/IReview';
import { AuthService } from '../services/auth.service';
import { ReviewsService } from '../services/reviews.service';
import { take, exhaustMap } from 'rxjs/operators';
import { UserService } from '../services/user.service';
import { Subject } from 'rxjs';
import { OrdersService } from '../services/orders.service';

@Component({
  selector: 'app-comments-list',
  templateUrl: './comments-list.component.html',
  styleUrls: ['./comments-list.component.css'],
})
export class CommentsListComponent implements OnInit {
  // Initilized values are for testing the service.
  @Input() product!: IProduct;
  reviewsOfProduct: IReview[] = [];
  everyReview: IReview[] = [];
  review!: IReview;
  decimal: number = 0;

  canAddComment: Subject<boolean> = new Subject<boolean>();

  reviewCount: number = 0;
  ratingColors: string[] = [];

  tempTestingReview: IReview = {
    date: new Date(),
    productId: 13,
    customerIdentityId: 'asasdas-d1243124-asfas124-asfasf-213',
    name: 'Harry',
    lastName: 'Smiths',
    comment:
      'Lorem ipsmilique reiam, cumque officia nihil consectetur? Non, ipsam ipsa?',
    rate: 4,
  };

  averageRating: number = 0;
  averageRatingCount: number = 0;

  comment!: string;

  commentForm: FormGroup = new FormGroup({
    comment: new FormControl(this.comment),
  });

  visible = false;
  setStar: number = 0;

  constructor(
    private reviewsService: ReviewsService,
    private authService: AuthService,
    private userService: UserService,
    private ordersService: OrdersService
  ) {}

  ngOnInit(): void {
    this.getUser();
    this.getAllReviewsOfProduct();
  }

  customer: ICustomer = {
    id: 0,
    identityId: '',
    addresses: [],
    email: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
  };

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
          let result: boolean = false;

          this.reviewsService
            .getCanAddComment(this.customer.identityId, this.product.id)
            .pipe(
              exhaustMap((response: boolean) => {
                result = response;
                return this.ordersService.getDidCustomerBuyProduct(
                  this.customer.identityId,
                  this.product.id
                );
              })
            )
            .subscribe({
              next: (response) => {
                this.canAddComment.next(result && response.data);
              },
              error: (error) => {
                this.canAddComment.next(false);
              },
            });
        },
      });
  }

  // getEveryReview() {
  //   // For testing purposes.
  //   this.reviewsService.getAllReviews().subscribe((response) => {
  //     this.everyReview = response;
  //   });
  // }

  getAllReviewsOfProduct() {
    this.reviewsService.getAllReviews().subscribe((response) => {
      this.reviewsOfProduct = response.filter(
        (s: { productId: number }) => s.productId == this.product.id
      );
      this.reviewCount = this.reviewsOfProduct.length;
    });
  }

  // getReview() {
  //   // The value inside is for testing purposes.
  //   this.reviewsService
  //     .getReview('6304ce4ce3214306296638c3')
  //     .subscribe((response) => {
  //       this.review = response;
  //     });
  // }

  // deleteReviewFromProduct(id: string) {
  //   // For testing purposes.
  //   this.reviewsService.deleteReviewFromProduct(id).subscribe();
  // }

  // updateReviewById(id: string) {
  //   // For testing purposes.
  //   this.reviewsService
  //     .updateReviewFromProduct(id, this.tempTestingReview)
  //     .subscribe();
  // }

  adjustRatingColors(itemRate: number, ratingStar: number): string {
    const active = 'color:#fbc634 !important;';
    if (ratingStar <= itemRate) {
      return active;
    } else {
      return '';
    }
  }

  getAverageRatingOfProduct(): string {
    var averageRating = 0;
    this.reviewsOfProduct.forEach((item) => {
      averageRating += item.rate;
    });
    averageRating = averageRating / this.reviewCount;
    this.averageRating = averageRating;
    this.averageRatingCount = this.averageRating;
    return this.averageRating.toFixed(1);
  }

  shapeAverageRatingStar(): string {
    if (this.averageRatingCount == 0) {
      return 'fa fa-star';
    }
    if (this.averageRatingCount < 1) {
      var decimal =
        this.averageRatingCount - Math.floor(this.averageRatingCount);
      this.averageRatingCount = this.averageRatingCount - decimal;
      return 'fa fa-star-half-full';
    } else {
      this.averageRatingCount = this.averageRatingCount - 1;
      return 'fa fa-star';
    }
  }

  toggleCollapse(): void {
    this.visible = !this.visible;
  }

  addReviewToProduct(review: IReview) {
    this.reviewsService.addReviewToProduct(review).subscribe(() => {
      this.getAllReviewsOfProduct();
      this.getUser();
    });
  }

  onCommentSubmit(product: IProduct) {
    this.comment = this.commentForm.value.comment;
    const review = this.mapReviewObject(this.customer, this.comment, product);

    this.addReviewToProduct(review);
  }

  changeSetStar(rate: string) {
    this.setStar = Number(rate);
  }

  mapReviewObject(
    customer: ICustomer,
    comment: string,
    product: IProduct
  ): IReview {
    return {
      productId: product.id,
      name: customer.firstName,
      date: new Date(),
      customerIdentityId: customer.identityId,
      lastName: customer.lastName,
      comment: comment,
      rate: this.setStar,
    };
  }
}
