import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/IApiResponse';

@Injectable({ providedIn: 'root' })
export class SellersService {
  constructor(private httpClient: HttpClient) {}

  getAllSellers(
    pageNumber: number,
    pageSize: number
  ): Observable<IApiResponse> {
    return this.httpClient.get<IApiResponse>(
      `${environment.apiUrl}/Seller?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
  }
}
