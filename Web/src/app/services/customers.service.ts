import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/IApiResponse';

@Injectable({ providedIn: 'root' })
export class CustomersService {
  constructor(private httpClient: HttpClient) {}

  getAllCustomers(
    pageNumber: number,
    pageSize: number
  ): Observable<IApiResponse> {
    return this.httpClient.get<IApiResponse>(
      `${environment.apiUrl}/Customer?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
  }
}
