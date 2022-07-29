import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/IApiResponse';
import { IProduct } from '../models/IProduct';

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
  };
  sendProduct(
    product: IProduct) : Observable<IProduct> { 
    return this.httpClient.post<IProduct>(this.apiUrl + '/product', product);
  };
  getProduct( id: number) : Observable<IProduct> { 
    return this.httpClient.get<IProduct>(this.apiUrl + '/product/' + id);
  };
  updateProduct(id : number,
    product: IProduct) : Observable<IProduct> { 
    return this.httpClient.put<IProduct>(this.apiUrl + '/product/' + id, product);
  }
}
