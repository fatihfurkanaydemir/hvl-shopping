import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IReview } from '../models/IReview';
import { ReviewsService } from '../services/reviews.service';

@Component({
  selector: 'app-comments-list',
  templateUrl: './comments-list.component.html',
  styleUrls: ['./comments-list.component.css']
})
export class CommentsListComponent implements OnInit {

  // Some initilized values are for testing the service.
  @Input() productId: number = 0;
  reviewsOfProduct: IReview[] = [];
  everyReview: IReview[] = [];
  review!: IReview;
  decimalFlag: boolean = true;

  reviewCount: number = 0;
  ratingColors: string[] = [];

  tempTestingReview: IReview = {
    productId: 2,
    name: "Harry",
    lastName: "Smiths",
    comment: "Lorem ipsmilique rerum soluta nulla voluptate eaque odio animi, saepe aperiam, cumque officia nihil consectetur? Non, ipsam ipsa?",
    rate: 4
  };

  averageRating: number = 0;
  averageRatingCount: number = 0;

  constructor(private reviewsService: ReviewsService) { }

  ngOnInit(): void {
    this.getAllReviewsOfProduct();
  }

  getEveryReview(){ // For testing purposes.
    this.reviewsService.getAllReviews().subscribe((response) => {
      this.everyReview = response;
      console.log(this.everyReview);
    })
  }

  getAllReviewsOfProduct(){
    this.reviewsService.getAllReviews().subscribe((response) => {
      this.reviewsOfProduct = response.filter((s: { productId: number; }) => s.productId == this.productId);
      this.reviewCount = this.reviewsOfProduct.length;
    })
  }

  getReview(){ // For testing purposes.
    this.reviewsService.getReview('6304ce4ce3214306296638c3').subscribe(
      (response) => {
        this.review = response;
      }
    );
  }

  addReviewToProduct(){ // For testing purposes.
    this.reviewsService.addReviewToProduct(this.tempTestingReview).subscribe();
  }

  deleteReviewFromProduct(id: string){ // For testing purposes.
    this.reviewsService.deleteReviewFromProduct(id).subscribe();
  }

  updateReviewById(id:string){ // For testing purposes.
    this.reviewsService.updateReviewFromProduct(id, this.tempTestingReview).subscribe();
  }

  adjustRatingColors(itemRate: number, ratingStar:number): string{
    const active = "color:#fbc634 !important;"
    if (ratingStar <= itemRate){
      return active;
    } else{
      return '';
    };
  }

  getAverageRatingOfProduct(): string{
    var averageRating = 0;
    this.reviewsOfProduct.forEach(item => {
      averageRating += item.rate;
    });
    averageRating = averageRating / this.reviewCount;
    this.averageRating = averageRating;
    this.averageRatingCount = this.averageRating;
    return this.averageRating.toFixed(1);
  }

  shapeAverageRatingStar(): string{
    if(this.averageRatingCount == 0){
      return "fa fa-star";
    }
    if(this.averageRatingCount < 1){
      if(this.averageRatingCount <= 0.5){
        var decimal = this.averageRatingCount - Math.floor(this.averageRatingCount);
        this.averageRatingCount = this.averageRatingCount - decimal;
        return "fa fa-star-half-full";
      } else {
        return "fa fa-star";
      }
    } else {
      this.averageRatingCount = this.averageRatingCount - 1;
      return "fa fa-star";
    }
  }
}
