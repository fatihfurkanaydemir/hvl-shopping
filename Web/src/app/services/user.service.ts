import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IAddAddress } from '../models/IAddAddress';
import { IApiResponseSingle } from '../models/IApiResponseSingle';
import { IChangePassword } from '../models/IChangePassword';
import { ICustomerEditProfile } from '../models/ICustomerEditProfile';
import { IDeleteAddress } from '../models/IDeleteAddress';
import { IEditAddress } from '../models/IEditAddress';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private httpClient: HttpClient) {}

  apiUrl = environment.apiUrl;
  authUrl = environment.authUrl;

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

  changePassword(
    changePasswordData: IChangePassword
  ): Observable<IApiResponseSingle> {
    return this.httpClient.post<IApiResponseSingle>(
      `${this.authUrl}/Account/change-password`,
      changePasswordData
    );
  }

  addAddress(addAddressdata: IAddAddress): Observable<IApiResponseSingle> {
    return this.httpClient.post<IApiResponseSingle>(
      `${this.apiUrl}/Customer/AddAddress`,
      addAddressdata
    );
  }

  editAddress(editAddressdata: IEditAddress): Observable<IApiResponseSingle> {
    return this.httpClient.patch<IApiResponseSingle>(
      `${this.apiUrl}/Customer/UpdateAddress`,
      editAddressdata
    );
  }

  deleteAddress(
    deleteAddressdata: IDeleteAddress
  ): Observable<IApiResponseSingle> {
    return this.httpClient.request<IApiResponseSingle>(
      'delete',
      `${this.apiUrl}/Customer/DeleteAddress`,
      { body: deleteAddressdata }
    );
  }
}
