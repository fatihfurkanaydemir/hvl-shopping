import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/IApiResponse';
import { IApiResponseSingle } from '../models/IApiResponseSingle';
import { ICategory } from '../models/ICategory';
import { ICategoryCreate } from '../models/ICategoryCreate';
import { ICategoryUpdate } from '../models/ICategoryUpdate';

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

  createCategory(category: ICategoryCreate): Observable<IApiResponse> {
    return this.httpClient.post<IApiResponse>(
      this.apiUrl + '/Category',
      category
    );
  }

  updateCategory(category: ICategoryUpdate): Observable<IApiResponse> {
    return this.httpClient.patch<IApiResponse>(
      this.apiUrl + '/Category/',
      category
    );
  }

  getCategory(id: number): Observable<IApiResponseSingle> {
    return this.httpClient.get<IApiResponseSingle>(
      this.apiUrl + '/Category/' + id
    );
  }

  deleteCategory(id: number): Observable<IApiResponse> {
    return this.httpClient.delete<IApiResponse>(
      this.apiUrl + '/Category/' + id
    );
  }
}
