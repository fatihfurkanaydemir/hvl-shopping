import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponseSingle } from '../models/IApiResponseSingle';
import { ICustomerEditProfile } from '../models/ICustomerEditProfile';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private httpClient: HttpClient) {}

  apiUrl = environment.apiUrl;

  getSellerInfo(identityId: string): Observable<IApiResponseSingle> {
    return this.httpClient.get<IApiResponseSingle>(
      `${this.apiUrl}/Seller/${identityId}`
    );
  }

  getCustomerInfo(identityId: string): Observable<IApiResponseSingle> {
    return this.httpClient.get<IApiResponseSingle>(
      `${this.apiUrl}/Customer/${identityId}`
    );
  }

  editCustomerProfile(
    customer: ICustomerEditProfile
  ): Observable<IApiResponseSingle> {
    return this.httpClient.patch<IApiResponseSingle>(
      `${this.apiUrl}/Customer`,
      customer
    );
  }
}
