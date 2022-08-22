import { CouponStatus } from './CouponStatus';

export interface IUpdateCoupon {
  code: string;
  amount: number;
  expireDate: Date;
  status: CouponStatus;
}
