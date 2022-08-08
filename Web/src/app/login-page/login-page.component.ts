import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ILogin } from '../models/ILogin';
import { ToastService } from '../services/toast.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css'],
})
export class LoginPageComponent implements OnInit {
  constructor(
    private toastService: ToastService,
    private authService: AuthService,
    private router: Router
  ) {}

  loginForm!: FormGroup;

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
    });
  }

  login() {
    const loginData: ILogin = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password,
    };

    this.authService.login(loginData).subscribe({
      next: (data) => {
        this.router.navigate(['/']);
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
}
