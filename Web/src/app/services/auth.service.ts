import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/IApiResponse';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private httpClient: HttpClient) {}

  authUrl: string = environment.authUrl;

  createUser(form: any): Observable<IApiResponse> {
    return this.httpClient.post<IApiResponse>(
      this.authUrl + '/Account/register',
      form
    );
  }
  loginUser(form: any): Observable<IApiResponse> {
    return this.httpClient.post<IApiResponse>(
      this.authUrl + '/Account/authenticate',
      form
    );
  }
}
