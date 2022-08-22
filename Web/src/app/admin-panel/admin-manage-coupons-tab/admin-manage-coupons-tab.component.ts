import { Component, OnInit } from '@angular/core';
import { CouponStatus } from 'src/app/models/CouponStatus';
import { ICoupon } from 'src/app/models/ICoupon';
import { CouponsService } from 'src/app/services/coupons.service';

@Component({
  selector: 'app-admin-manage-coupons-tab',
  templateUrl: './admin-manage-coupons-tab.component.html',
  styleUrls: ['./admin-manage-coupons-tab.component.css'],
})
export class AdminManageCouponsTabComponent implements OnInit {
  coupons: ICoupon[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  dataCount: number = 0;

  CouponStatus = CouponStatus;

  constructor(private couponService: CouponsService) {}

  ngOnInit(): void {
    this.getCoupons();
  }

  getCoupons() {
    this.couponService.getAllCoupons(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.coupons = response.data;
        this.dataCount = response.dataCount;
      },
    });
  }

  onPageChange(newPageNumber: number) {
    this.pageNumber = newPageNumber;
    this.getCoupons();
  }
}
