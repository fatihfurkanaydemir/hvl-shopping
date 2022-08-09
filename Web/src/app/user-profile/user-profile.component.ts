import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { exhaustMap, map, take, tap } from 'rxjs';
import { IChangePassword } from '../models/IChangePassword';
import { ICustomer } from '../models/ICustomer';
import { ICustomerEditProfile } from '../models/ICustomerEditProfile';
import { AuthService } from '../services/auth.service';
import { ToastService } from '../services/toast.service';
import { UserService } from '../services/user.service';
import { SharedValues } from '../shared/SharedValues';
import { ValidationPatterns } from '../shared/ValidationPatterns';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'],
})
export class UserProfileComponent implements OnInit {
  customer: ICustomer = {
    id: 0,
    identityId: '',
    addresses: [],
    email: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
  };

  editProfileForm: FormGroup = new FormGroup({
    firstName: new FormControl(this.customer.firstName, [Validators.required]),
    lastName: new FormControl(this.customer.lastName, [Validators.required]),
    phoneNumber: new FormControl(this.customer.phoneNumber, [
      Validators.required,
    ]),
  });

  changePasswordForm: FormGroup = new FormGroup({
    oldPassword: new FormControl(this.customer.firstName, [
      Validators.required,
    ]),
    newPassword: new FormControl(this.customer.lastName, [
      Validators.required,
      Validators.pattern(ValidationPatterns.password),
    ]),
    confirmNewPassword: new FormControl(this.customer.phoneNumber, [
      Validators.required,
      Validators.pattern(ValidationPatterns.password),
    ]),
  });

  cities = SharedValues.cities;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.getUser();
  }

  onAddressChanged() {
    this.getUser();
  }

  getUser() {
    this.authService.userSubject
      .pipe(
        take(1),
        exhaustMap((user) => {
          return this.userService.getCustomerInfo(user.identityId);
        })
      )
      .subscribe({
        next: (response) => {
          this.customer = response.data;
          this.editProfileForm.patchValue({ ...this.customer });
        },
      });
  }

  onEditProfileSubmit() {
    if (this.editProfileForm.invalid) return;

    const customerData: ICustomerEditProfile = {
      identityId: this.customer.identityId,
      firstName: this.editProfileForm.value.firstName,
      lastName: this.editProfileForm.value.lastName,
      phoneNumber: this.editProfileForm.value.phoneNumber,
    };

    this.userService.editCustomerProfile(customerData).subscribe({
      next: (response) => {
        this.toastService.showToast({
          icon: 'success',
          title: 'Profil Güncellendi',
        });

        this.getUser();
      },
      error: (error) => {
        this.toastService.showToast({
          icon: 'error',
          title: 'Profil Güncellenemedi',
        });
      },
    });
  }

  onChangePasswordSubmit() {
    if (this.changePasswordForm.invalid) return;

    const changePasswordData: IChangePassword = {
      email: this.customer.email,
      oldPassword: this.changePasswordForm.value.oldPassword,
      newPassword: this.changePasswordForm.value.newPassword,
      confirmNewPassword: this.changePasswordForm.value.confirmNewPassword,
    };

    this.userService.changePassword(changePasswordData).subscribe({
      next: (response) => {
        this.toastService.showToast({
          icon: 'success',
          title: 'Şifre Güncellendi, Lütfen Tekrar Giriş Yapınız.',
        });

        this.changePasswordForm.reset();
        this.authService.logout();
      },
      error: (error) => {
        this.toastService.showToast({
          icon: 'error',
          title: error.error.message,
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

  isEditProfileFormControlInvalid(name: string) {
    return (
      this.editProfileForm.get(name)?.invalid &&
      (this.editProfileForm.get(name)?.dirty ||
        this.editProfileForm.get(name)?.touched)
    );
  }

  isEditProfileFormControlValid(name: string) {
    return (
      this.editProfileForm.get(name)?.valid &&
      (this.editProfileForm.get(name)?.dirty ||
        this.editProfileForm.get(name)?.touched)
    );
  }

  isChangePasswordFormControlInvalid(name: string) {
    return (
      this.changePasswordForm.get(name)?.invalid &&
      (this.changePasswordForm.get(name)?.dirty ||
        this.changePasswordForm.get(name)?.touched)
    );
  }

  isChangePasswordFormControlValid(name: string) {
    return (
      this.changePasswordForm.get(name)?.valid &&
      (this.changePasswordForm.get(name)?.dirty ||
        this.changePasswordForm.get(name)?.touched)
    );
  }
}
