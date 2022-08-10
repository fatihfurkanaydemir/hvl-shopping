import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { take, exhaustMap } from 'rxjs';
import { IChangePassword } from 'src/app/models/IChangePassword';
import { ISeller } from 'src/app/models/ISeller';
import { ISellerEditProfile } from 'src/app/models/ISellerEditProfile';
import { AuthService } from 'src/app/services/auth.service';
import { ToastService } from 'src/app/services/toast.service';
import { UserService } from 'src/app/services/user.service';
import { SharedValues } from 'src/app/shared/SharedValues';
import { ValidationPatterns } from 'src/app/shared/ValidationPatterns';

@Component({
  selector: 'app-seller-profile',
  templateUrl: './seller-profile.component.html',
  styleUrls: ['./seller-profile.component.css'],
})
export class SellerProfileComponent implements OnInit {
  seller: ISeller = {
    identityId: '',
    id: 0,
    email: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
    shopName: '',
    address: {
      id: 0,
      addressDescription: '',
      city: '',
      title: '',
    },
  };

  editProfileForm: FormGroup = new FormGroup({
    firstName: new FormControl(this.seller.firstName, [Validators.required]),
    lastName: new FormControl(this.seller.lastName, [Validators.required]),
    phoneNumber: new FormControl(this.seller.phoneNumber, [
      Validators.required,
    ]),
    shopName: new FormControl(this.seller.shopName, [Validators.required]),
    address: new FormGroup({
      addressDescription: new FormControl(
        this.seller.address.addressDescription,
        [Validators.required]
      ),
      city: new FormControl(this.seller.address.city, [Validators.required]),
    }),
  });

  changePasswordForm: FormGroup = new FormGroup({
    oldPassword: new FormControl(this.seller.firstName, [Validators.required]),
    newPassword: new FormControl(this.seller.lastName, [
      Validators.required,
      Validators.pattern(ValidationPatterns.password),
    ]),
    confirmNewPassword: new FormControl(this.seller.phoneNumber, [
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
          return this.userService.getSellerInfo(user.identityId);
        })
      )
      .subscribe({
        next: (response) => {
          this.seller = response.data;
          this.editProfileForm.patchValue({ ...this.seller });
        },
      });
  }

  onEditProfileSubmit() {
    if (this.editProfileForm.invalid) return;

    const sellerData: ISellerEditProfile = {
      identityId: this.seller.identityId,
      firstName: this.editProfileForm.value.firstName,
      lastName: this.editProfileForm.value.lastName,
      phoneNumber: this.editProfileForm.value.phoneNumber,
      shopName: this.editProfileForm.value.shopName,
      address: {
        ...this.seller.address,
        ...this.editProfileForm.value.address,
      },
    };

    this.userService.editSellerProfile(sellerData).subscribe({
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
          title: error.error?.message ?? 'Profil Güncellenemedi',
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

  onChangePasswordSubmit() {
    if (this.changePasswordForm.invalid) return;

    const changePasswordData: IChangePassword = {
      email: this.seller.email,
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
