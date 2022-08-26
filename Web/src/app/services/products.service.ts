import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/IApiResponse';
import { IApiResponseSingle } from '../models/IApiResponseSingle';
import { IProductCreate } from '../models/IProductCreate';
import { IProductUpdate } from '../models/IProductUpdate';

@Injectable({ providedIn: 'root' })
export class ProductsService {
  constructor(private httpClient: HttpClient) {}

  getAllProducts(
    pageNumber: number,
    pageSize: number
  ): Observable<IApiResponse> {
    return this.httpClient.get<IApiResponse>(
      `${environment.apiUrl}/Product?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
  }

  createProduct(product: IProductCreate): Observable<IApiResponse> {
    return this.httpClient.post<IApiResponse>(
      environment.apiUrl + '/Product',
      product
    );
  }

  getProduct(id: number): Observable<IApiResponseSingle> {
    return this.httpClient.get<IApiResponseSingle>(
      environment.apiUrl + '/Product/' + id
    );
  }

  getAllProductsByIdentityId(
    identityId: string,
    pageNumber: number,
    pageSize: number
  ): Observable<IApiResponse> {
    return this.httpClient.get<IApiResponse>(
      `${environment.apiUrl}/Seller/${identityId}/Products?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
  }

  getProductsBySearchFilter(
    filterString: string,
    pageNumber: number,
    pageSize: number
  ): Observable<IApiResponse> {
    return this.httpClient.get<IApiResponse>(
      `${environment.apiUrl}/Product/Search?FilterString=${filterString}&PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
  }

  updateProduct(product: IProductUpdate): Observable<IApiResponse> {
    return this.httpClient.patch<IApiResponse>(
      environment.apiUrl + '/Product/',
      product
    );
  }

  deactivateProduct(id: number): Observable<IApiResponseSingle> {
    return this.httpClient.post<IApiResponseSingle>(
      environment.apiUrl + '/Product/deactivate',
      { id }
    );
  }

  activateProduct(id: number): Observable<IApiResponseSingle> {
    return this.httpClient.post<IApiResponseSingle>(
      environment.apiUrl + '/Product/activate',
      { id }
    );
  }
}
