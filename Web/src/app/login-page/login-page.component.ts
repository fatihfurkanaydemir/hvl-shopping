import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { BasketService } from '../basket/basket.service';
import { ILogin } from '../models/ILogin';
import { DiscountHubService } from '../services/discounthub.service';
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
    private basketService: BasketService,
    private router: Router,
    private discountHubService: DiscountHubService
  ) {}

  loginForm!: FormGroup;

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
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
        this.basketService.getBasket(data.id).subscribe();
        this.discountHubService.start();
        this.router.navigate(['/']);
      },
      error: (error) => {
        console.log(error);

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
