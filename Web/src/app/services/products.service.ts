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

  apiUrl: string = environment.apiUrl;

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
      this.apiUrl + '/Product',
      product
    );
  }

  getProduct(id: number): Observable<IApiResponseSingle> {
    return this.httpClient.get<IApiResponseSingle>(
      this.apiUrl + '/Product/' + id
    );
  }

  updateProduct(product: IProductUpdate): Observable<IApiResponse> {
    return this.httpClient.patch<IApiResponse>(
      this.apiUrl + '/Product/',
      product
    );
  }

  deactivateProduct(id: number): Observable<IApiResponseSingle> {
    return this.httpClient.post<IApiResponseSingle>(
      this.apiUrl + '/Product/deactivate',
      { id }
    );
  }

  activateProduct(id: number): Observable<IApiResponseSingle> {
    return this.httpClient.post<IApiResponseSingle>(
      this.apiUrl + '/Product/activate',
      { id }
    );
  }
}
