export interface ICoupon {
  id: number;
  code: string;
  amount: number;
  expireDate: Date;
  status: string;
}
