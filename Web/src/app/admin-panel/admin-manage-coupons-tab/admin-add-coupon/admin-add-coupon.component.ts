import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CouponStatus } from 'src/app/models/CouponStatus';
import { ICreateCoupon } from 'src/app/models/ICreateCoupon';
import { CouponsService } from 'src/app/services/coupons.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-admin-add-coupon',
  templateUrl: './admin-add-coupon.component.html',
  styleUrls: ['./admin-add-coupon.component.css'],
})
export class AdminAddCouponComponent implements OnInit {
  @Output('OnCouponAdded') couponAddedEvent = new EventEmitter<boolean>();

  addCouponForm!: FormGroup;
  closeResult: string = '';
  pickedDate: Date = new Date();

  constructor(
    private modalService: NgbModal,
    private toastService: ToastService,
    private couponService: CouponsService
  ) {}

  ngOnInit(): void {
    this.addCouponForm = new FormGroup({
      code: new FormControl(null, [Validators.required]),
      amount: new FormControl(null, [Validators.required, Validators.min(1)]),
      status: new FormControl('Active', [Validators.required]),
    });

    this.pickedDate.setUTCHours(this.pickedDate.getUTCHours() + 1);
  }

  addCouponSubmit(modal: NgbActiveModal) {
    if (this.addCouponForm.invalid || !this.isDateValid()) return;

    const couponData: ICreateCoupon = {
      amount: +this.addCouponForm.value.amount,
      code: this.addCouponForm.value.code,
      status: +CouponStatus[this.addCouponForm.value.status],
      expireDate: this.pickedDate,
    };

    this.couponService.createCoupon(couponData).subscribe({
      next: (response) => {
        this.toastService.showToast({
          icon: 'success',
          title: 'Kupon Eklendi',
        });

        this.couponAddedEvent.emit(true);
        this.addCouponForm.reset({
          status: 'Active',
        });
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
    this.pickedDate = date;
  }

  isFormControlInvalid(name: string) {
    return (
      this.addCouponForm.get(name)?.invalid &&
      (this.addCouponForm.get(name)?.dirty ||
        this.addCouponForm.get(name)?.touched)
    );
  }

  isFormControlValid(name: string) {
    return (
      this.addCouponForm.get(name)?.valid &&
      (this.addCouponForm.get(name)?.dirty ||
        this.addCouponForm.get(name)?.touched)
    );
  }

  isDateValid() {
    return new Date() <= this.pickedDate;
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
