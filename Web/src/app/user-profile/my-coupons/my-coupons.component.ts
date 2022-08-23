import { Component, OnInit } from '@angular/core';
import { ICoupon } from 'src/app/models/ICoupon';
import { AuthService } from 'src/app/services/auth.service';
import { CouponsService } from 'src/app/services/coupons.service';
import { take, exhaustMap } from 'rxjs/operators';

@Component({
  selector: 'app-my-coupons',
  templateUrl: './my-coupons.component.html',
  styleUrls: ['./my-coupons.component.css'],
})
export class MyCouponsComponent implements OnInit {
  coupons: ICoupon[] = [];

  constructor(
    private authService: AuthService,
    private couponsService: CouponsService
  ) {}

  ngOnInit(): void {
    this.getCoupons();
  }

  getCoupons() {
    this.authService.userSubject
      .pipe(
        take(1),
        exhaustMap((user) => {
          return this.couponsService.getCustomerUsableCoupons(user.identityId);
        })
      )
      .subscribe({
        next: (response) => {
          this.coupons = response.data;
        },
      });
  }
}
