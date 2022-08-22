import { CouponStatus } from './CouponStatus';

export interface ICreateCoupon {
  code: string;
  amount: number;
  expireDate: Date;
  status: CouponStatus;
}
