import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/IApiResponse';
import { IApiResponseSingle } from '../models/IApiResponseSingle';
import { ICreateCoupon } from '../models/ICreateCoupon';
import { IUpdateCoupon } from '../models/IUpdateCoupon';

@Injectable({ providedIn: 'root' })
export class CouponsService {
  constructor(private httpClient: HttpClient) {}

  getAllCoupons(
    pageNumber: number,
    pageSize: number
  ): Observable<IApiResponse> {
    return this.httpClient.get<IApiResponse>(
      `${environment.apiUrl}/Coupon?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
  }

  getCustomerUsableCoupons(identityId: string): Observable<IApiResponseSingle> {
    return this.httpClient.get<IApiResponseSingle>(
      `${environment.apiUrl}/Coupon/CustomerUsableCoupons/${identityId}`
    );
  }

  createCoupon(coupon: ICreateCoupon): Observable<IApiResponseSingle> {
    return this.httpClient.post<IApiResponseSingle>(
      `${environment.apiUrl}/Coupon`,
      coupon
    );
  }

  updateCoupon(coupon: IUpdateCoupon): Observable<IApiResponseSingle> {
    return this.httpClient.patch<IApiResponseSingle>(
      `${environment.apiUrl}/Coupon`,
      coupon
    );
  }
}
