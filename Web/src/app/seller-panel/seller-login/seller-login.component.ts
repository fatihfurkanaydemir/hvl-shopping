import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ILogin } from 'src/app/models/ILogin';
import { ISellerRegister } from 'src/app/models/ISellerRegister';
import { AuthService } from 'src/app/services/auth.service';
import { DiscountHubService } from 'src/app/services/discounthub.service';
import { ToastService } from 'src/app/services/toast.service';
import { SharedValues } from 'src/app/shared/SharedValues';
import { ValidationPatterns } from 'src/app/shared/ValidationPatterns';

@Component({
  selector: 'app-seller-login',
  templateUrl: './seller-login.component.html',
  styleUrls: ['./seller-login.component.css'],
})
export class SellerLoginComponent implements OnInit {
  registerForm!: FormGroup;
  loginForm!: FormGroup;

  constructor(
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router,
    private discountHubService: DiscountHubService
  ) {}

  tabId: string = 'login';
  tabChange(id: string) {
    this.tabId = id;
  }

  // prettier-ignore
  cities = SharedValues.cities;

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [
        Validators.required,
        Validators.pattern(ValidationPatterns.password),
      ]),
      confirmPassword: new FormControl('', [
        Validators.required,
        Validators.pattern(ValidationPatterns.password),
      ]),
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      phoneNumber: new FormControl('', [
        Validators.required,
        Validators.pattern(ValidationPatterns.phoneNumber),
      ]),
      address: new FormControl('', [Validators.required]),
      storeName: new FormControl('', [Validators.required]),
      city: new FormControl('Ankara', [Validators.required]),
    });

    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
    });
  }

  register() {
    if (this.registerForm.invalid) return;

    const registerData: ISellerRegister = {
      email: this.registerForm.value.email,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword,
      firstName: this.registerForm.value.firstName,
      lastName: this.registerForm.value.lastName,
      phoneNumber: this.registerForm.value.phoneNumber,
      shopName: this.registerForm.value.storeName,
      address: {
        id: 0,
        title: '',
        city: this.registerForm.value.city,
        addressDescription: this.registerForm.value.address,
      },
    };

    this.authService.registerSeller(registerData).subscribe({
      next: (response) => {
        this.toastService.showToast({
          icon: 'success',
          title: 'Kayıt oluşturuldu.',
        });
        this.registerForm.reset({
          city: 'Ankara',
        });
        this.loginForm.reset();
        this.tabChange('login');
      },
      error: (errorResp) => {
        console.log(errorResp);

        if (errorResp.error) {
          this.toastService.showToast({
            icon: 'error',
            title: errorResp.error?.message ?? 'Kayıt oluşturulamadı.',
          });
        }

        if (errorResp.errors?.errors) {
          errorResp.error.errors.forEach((err: string) => {
            this.toastService.showToast({
              icon: 'error',
              title: err,
            });
          });
        }
      },
    });
  }

  login() {
    if (this.loginForm.invalid) return;

    const loginData: ILogin = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password,
    };

    this.authService.login(loginData).subscribe({
      next: (data) => {
        this.discountHubService.start();
        this.router.navigate(['/seller-panel']);
      },
      error: (error) => {
        this.toastService.showToast({
          icon: 'error',
          title: error.error.message,
        });
      },
    });
  }

  isLoginFormControlInvalid(name: string) {
    return (
      this.loginForm.get(name)?.invalid &&
      (this.loginForm.get(name)?.dirty || this.loginForm.get(name)?.touched)
    );
  }

  isLoginFormControlValid(name: string) {
    return (
      this.loginForm.get(name)?.valid &&
      (this.loginForm.get(name)?.dirty || this.loginForm.get(name)?.touched)
    );
  }

  isRegisterFormControlInvalid(name: string) {
    return (
      this.registerForm.get(name)?.invalid &&
      (this.registerForm.get(name)?.dirty ||
        this.registerForm.get(name)?.touched)
    );
  }

  isRegisterFormControlValid(name: string) {
    return (
      this.registerForm.get(name)?.valid &&
      (this.registerForm.get(name)?.dirty ||
        this.registerForm.get(name)?.touched)
    );
  }
}
