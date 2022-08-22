import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { CouponStatus } from 'src/app/models/CouponStatus';
import { ICoupon } from 'src/app/models/ICoupon';
import { IDeleteCoupon } from 'src/app/models/IDeleteCoupon';
import { IUpdateCoupon } from 'src/app/models/IUpdateCoupon';
import { CouponsService } from 'src/app/services/coupons.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-admin-coupon-actions',
  templateUrl: './admin-coupon-actions.component.html',
  styleUrls: ['./admin-coupon-actions.component.css'],
})
export class AdminCouponActionsComponent implements OnInit {
  @Output('OnCouponEdited') couponEditedEvent = new EventEmitter<boolean>();

  @Input('coupon') coupon!: ICoupon;

  editCouponForm!: FormGroup;
  closeResult: string = '';
  pickedDate: BehaviorSubject<Date> = new BehaviorSubject<Date>(null!);

  constructor(
    private toastService: ToastService,
    private couponService: CouponsService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.editCouponForm = new FormGroup({
      amount: new FormControl(this.coupon.amount, [
        Validators.required,
        Validators.min(1),
      ]),
      status: new FormControl(this.coupon.status, [Validators.required]),
    });

    this.pickedDate.next(this.coupon.expireDate);
  }

  deleteCoupon(modal: NgbActiveModal) {
    const couponData: IDeleteCoupon = {
      code: this.coupon.code,
    };

    this.couponService.deleteCoupon(couponData).subscribe({
      next: (response) => {
        console.log(response);

        this.toastService.showToast({
          icon: 'success',
          title: 'Kupon silindi',
        });

        modal.close();
        this.couponEditedEvent.emit(true);
      },
      error: (error) => {
        this.toastService.showToast({
          icon: 'error',
          title: error.error?.message ?? 'Bir hata meydana geldi',
        });
        if (error.error?.errors) {
          error.error.errors.forEach((err: any) => {
            this.toastService.showToast({
              icon: 'error',
              title: err,
            });
          });
        }
      },
    });
  }

  editCouponSubmit(modal: NgbActiveModal) {
    if (this.editCouponForm.invalid || !this.isDateValid()) return;

    const couponData: IUpdateCoupon = {
      amount: +this.editCouponForm.value.amount,
      code: this.coupon.code,
      status: +CouponStatus[this.editCouponForm.value.status],
      expireDate: this.pickedDate.value,
    };

    this.couponService.updateCoupon(couponData).subscribe({
      next: (response) => {
        this.toastService.showToast({
          icon: 'success',
          title: 'Kupon Düzenlendi',
        });

        this.couponEditedEvent.emit(true);
        this.editCouponForm.reset();
        modal.close();
      },
      error: (error) => {
        this.toastService.showToast({
          icon: 'error',
          title: error.error?.message ?? 'Bir hata meydana geldi',
        });
        if (error.error.errors) {
          error.error.errors.forEach((err: any) => {
            this.toastService.showToast({
              icon: 'error',
              title: err,
            });
          });
        }
      },
    });
  }

  onDatePicked(date: Date) {
    this.pickedDate.next(date);
  }

  isFormControlInvalid(name: string) {
    return (
      this.editCouponForm.get(name)?.invalid &&
      (this.editCouponForm.get(name)?.dirty ||
        this.editCouponForm.get(name)?.touched)
    );
  }

  isFormControlValid(name: string) {
    return (
      this.editCouponForm.get(name)?.valid &&
      (this.editCouponForm.get(name)?.dirty ||
        this.editCouponForm.get(name)?.touched)
    );
  }

  isDateValid() {
    return new Date() <= new Date(this.pickedDate.value);
  }

  openModal(content: any) {
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
        },
        (reason) => {
          this.closeResult = `Dismissed ${reason}`;
        }
      );
  }
}
