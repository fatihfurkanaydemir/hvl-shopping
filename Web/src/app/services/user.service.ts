import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponseSingle } from '../models/IApiResponseSingle';
import { IChangePassword } from '../models/IChangePassword';
import { ICustomerEditProfile } from '../models/ICustomerEditProfile';
import { IAddCustomerAddress } from '../models/IAddCustomerAddress';
import { IDeleteCustomerAddress } from '../models/IDeleteCustomerAddress';
import { IEditCustomerAddress } from '../models/IEditCustomerAddress';
import { ISellerEditProfile } from '../models/ISellerEditProfile';

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

  editSellerProfile(
    seller: ISellerEditProfile
  ): Observable<IApiResponseSingle> {
    return this.httpClient.patch<IApiResponseSingle>(
      `${this.apiUrl}/Seller`,
      seller
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

  addCustomerAddress(
    addAddressdata: IAddCustomerAddress
  ): Observable<IApiResponseSingle> {
    return this.httpClient.post<IApiResponseSingle>(
      `${this.apiUrl}/Customer/AddAddress`,
      addAddressdata
    );
  }

  editCustomerAddress(
    editAddressdata: IEditCustomerAddress
  ): Observable<IApiResponseSingle> {
    return this.httpClient.patch<IApiResponseSingle>(
      `${this.apiUrl}/Customer/UpdateAddress`,
      editAddressdata
    );
  }

  deleteCustomerAddress(
    deleteAddressdata: IDeleteCustomerAddress
  ): Observable<IApiResponseSingle> {
    return this.httpClient.request<IApiResponseSingle>(
      'delete',
      `${this.apiUrl}/Customer/DeleteAddress`,
      { body: deleteAddressdata }
    );
  }
}
