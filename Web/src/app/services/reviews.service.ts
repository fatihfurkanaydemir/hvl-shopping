import { getLocaleEraNames } from '@angular/common';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/IApiResponse';
import { IApiResponseSingle } from '../models/IApiResponseSingle';
import { IReview } from '../models/IReview';

@Injectable({
  providedIn: 'root',
})
export class ReviewsService {
  //Error handling is not added at the moment.

  returnedReviews: IReview[] = [];

  constructor(private http: HttpClient) {}

  getAllReviews(): Observable<any> {
    return this.http.get<any>(`${environment.reviewServiceUrl}/Review`);
  }

  getReview(id: string): Observable<any> {
    return this.http.get<any>(environment.reviewServiceUrl + '/Review/' + id);
  }

  getCanAddComment(
    customerIdentityId: string,
    productId: number
  ): Observable<any> {
    return this.http.get<any>(
      environment.reviewServiceUrl +
        '/Review/CanAddComment/' +
        customerIdentityId +
        '/' +
        productId
    );
  }

  addReviewToProduct(review: IReview): Observable<IReview> {
    return this.http.post<IReview>(
      `${environment.reviewServiceUrl}/Review/`,
      review
    );
  }

  deleteReviewFromProduct(id: string) {
    const url = `${environment.reviewServiceUrl}/Review/${id}`;
    return this.http.delete(url);
  }

  updateReviewFromProduct(id: string, review: IReview) {
    return this.http.put(
      `${environment.reviewServiceUrl}/Review/${id}`,
      review
    );
  }
}
