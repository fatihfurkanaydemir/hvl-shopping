import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ICustomerRegister } from '../models/ICustomerRegister';
import { ToastService } from '../services/toast.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css'],
})
export class RegisterPageComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private router: Router,
    private toastService: ToastService
  ) {}

  registerForm!: FormGroup;

  passwordValidPattern =
    '^(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[/@/#/$/%/^/./&/+/=/*/-]).*$';

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      lastName: new FormControl(null, [Validators.required]),
      firstName: new FormControl(null, [Validators.required]),
      phoneNumber: new FormControl(null, [
        Validators.required,
        Validators.pattern('^[+]?\\d{10,12}$'),
      ]),
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [
        Validators.required,
        Validators.pattern(this.passwordValidPattern),
      ]),
      confirmPassword: new FormControl(null, [
        Validators.required,
        Validators.pattern(this.passwordValidPattern),
      ]),
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.registerForm.controls;
  }

  register() {
    if (this.registerForm.invalid) return;

    const registerData: ICustomerRegister = {
      email: this.registerForm.value.email,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword,
      firstName: this.registerForm.value.firstName,
      lastName: this.registerForm.value.lastName,
      phoneNumber: this.registerForm.value.phoneNumber,
    };

    this.authService.registerCustoemr(registerData).subscribe({
      next: (response) => {
        this.toastService.showToast({
          icon: 'success',
          title: 'Kayıt oluşturuldu.',
        });

        this.registerForm.reset();
        this.router.navigate(['/login']);
      },
      error: (errorResp) => {
        this.toastService.showToast({
          icon: 'error',
          title: 'Kayıt oluşturulamadı.',
        });

        errorResp.error.errors.forEach((err: string) => {
          this.toastService.showToast({
            icon: 'error',
            title: err,
          });
        });
      },
    });
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
