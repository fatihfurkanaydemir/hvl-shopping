import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/IApiResponse';
import { IApiResponseSingle } from '../models/IApiResponseSingle';
import { IOrder } from '../models/IOrder';

@Injectable({ providedIn: 'root' })
export class OrdersService {
  constructor(private httpClient: HttpClient) {}

  getAllOrders(pageNumber: number, pageSize: number): Observable<IApiResponse> {
    return this.httpClient.get<IApiResponse>(
      `${environment.orderServiceUrl}/Order?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
  }

  getAllOrdersByCustomerIdentityId(
    identityId: string,
    pageNumber: number,
    pageSize: number
  ): Observable<IApiResponse> {
    return this.httpClient.get<IApiResponse>(
      `${environment.orderServiceUrl}/Order/CustomerOrders/${identityId}?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
  }

  getAllOrdersBySellerIdentityId(
    identityId: string,
    pageNumber: number,
    pageSize: number
  ): Observable<IApiResponse> {
    return this.httpClient.get<IApiResponse>(
      `${environment.orderServiceUrl}/Order/SellerOrders/${identityId}?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
  }

  getStatusTranslation(status: string) {
    switch (status) {
      case 'Created':
        return 'Oluşturuldu';
      case 'Shipped':
        return 'Kargolandı';
      case 'Returned':
        return 'İade Edildi';
      case 'Cancelled':
        return 'İptal Edildi';
      default:
        return '';
    }
  }

  getOrderProductCount(order: IOrder) {
    return order.products.reduce((acc, product) => acc + product.count, 0);
  }
}
