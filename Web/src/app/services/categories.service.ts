import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/IApiResponse';
import { ICategory } from '../models/ICategory';

@Injectable({ providedIn: 'root' })
export class CategoriesService {
  constructor(private httpClient: HttpClient) {}

  apiUrl: string = environment.apiUrl;

  getAllCategories(
    pageNumber: number,
    pageSize: number
  ): Observable<IApiResponse> {
    return this.httpClient.get<IApiResponse>(
      `${environment.apiUrl}/Category?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
  }

  getCategory(id: number): Observable<ICategory> {
    return this.httpClient.get<ICategory>(this.apiUrl + '/Category/' + id);
  }
}
