import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponseSingle } from '../models/IApiResponseSingle';
import { IAuthData } from '../models/IAuthData';
import { ILogin } from '../models/ILogin';
import { ISellerRegister } from '../models/ISellerRegister';
import { User } from '../models/User';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(
    private httpClient: HttpClient,
    private router: Router,
    private route: ActivatedRoute
  ) {}
  userSubject = new BehaviorSubject<User>(null!);
  private tokenExpirationTimer: any;

  authUrl: string = environment.authUrl;
  apiUrl: string = environment.apiUrl;

  registerCustomer() {}

  registerSeller(
    registerData: ISellerRegister
  ): Observable<IApiResponseSingle> {
    return this.httpClient.post<IApiResponseSingle>(
      `${this.apiUrl}/Seller`,
      registerData
    );
  }

  login(loginData: ILogin) {
    return this.httpClient
      .post<IApiResponseSingle>(
        `${this.authUrl}/Account/authenticate`,
        loginData
      )
      .pipe(
        map((reponse) => reponse.data),
        tap((authData: IAuthData) => this.handleAuthentication(authData))
      );
  }

  autoLogin() {
    const userData: {
      identityId: string;
      email: string;
      roles: string[];
      _expires: number;
      _token: string;
    } = JSON.parse(localStorage.getItem('userData')!);

    if (!userData) {
      return;
    }

    const user = new User(
      userData.identityId,
      userData.email,
      userData.roles,
      userData._expires,
      userData._token
    );

    if (!user.token) return;

    this.userSubject.next(user);
    const expirationTime =
      new Date(userData._expires * 1000).getTime() - new Date().getTime();
    this.autoLogout(expirationTime);
  }

  logout() {
    this.userSubject.next(null!);
    this.router.navigate(['/']);
    localStorage.removeItem('userData');
    if (this.tokenExpirationTimer) clearTimeout(this.tokenExpirationTimer);
    this.tokenExpirationTimer = null;
  }

  autoLogout(expirationTime: number) {
    console.log(expirationTime);

    this.tokenExpirationTimer = setTimeout(() => {
      this.logout();
    }, expirationTime);
  }

  handleAuthentication(authData: IAuthData) {
    const user = new User(
      authData.id,
      authData.email,
      authData.roles,
      authData.expires,
      authData.jwToken
    );

    this.userSubject.next(user);
    const expirationTime =
      new Date(authData.expires * 1000).getTime() - new Date().getTime();
    this.autoLogout(expirationTime);
    localStorage.setItem('userData', JSON.stringify(user));
  }
}
