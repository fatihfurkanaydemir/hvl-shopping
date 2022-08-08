import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription, take, tap } from 'rxjs';
import { User } from './models/User';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'Havelsan Shopping';
  userSub?: Subscription;
  user?: User;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.authService.autoLogin();

    this.userSub = this.authService.userSubject.subscribe((user) => {
      this.user = user;
    });
  }

  goToPage(pageName: string) {
    this.router.navigate([`${pageName}`]);
  }

  logout() {
    this.authService.logout();
  }

  ngOnDestroy(): void {
    this.userSub?.unsubscribe();
  }
}
