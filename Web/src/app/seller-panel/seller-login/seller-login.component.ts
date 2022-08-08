import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ILogin } from 'src/app/models/ILogin';
import { ISellerRegister } from 'src/app/models/ISellerRegister';
import { AuthService } from 'src/app/services/auth.service';
import { ToastService } from 'src/app/services/toast.service';

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
    private router: Router
  ) {}

  tabId: string = 'login';
  tabChange(id: string) {
    this.tabId = id;
  }

  // prettier-ignore
  cities = ['Adana', 'Adıyaman', 'Afyon', 'Ağrı', 'Amasya', 'Ankara', 'Antalya', 'Artvin',
  'Aydın', 'Balıkesir', 'Bilecik', 'Bingöl', 'Bitlis', 'Bolu', 'Burdur', 'Bursa', 'Çanakkale',
  'Çankırı', 'Çorum', 'Denizli', 'Diyarbakır', 'Edirne', 'Elazığ', 'Erzincan', 'Erzurum', 'Eskişehir',
  'Gaziantep', 'Giresun', 'Gümüşhane', 'Hakkari', 'Hatay', 'Isparta', 'Mersin', 'İstanbul', 'İzmir', 
  'Kars', 'Kastamonu', 'Kayseri', 'Kırklareli', 'Kırşehir', 'Kocaeli', 'Konya', 'Kütahya', 'Malatya', 
  'Manisa', 'Kahramanmaraş', 'Mardin', 'Muğla', 'Muş', 'Nevşehir', 'Niğde', 'Ordu', 'Rize', 'Sakarya',
  'Samsun', 'Siirt', 'Sinop', 'Sivas', 'Tekirdağ', 'Tokat', 'Trabzon', 'Tunceli', 'Şanlıurfa', 'Uşak',
  'Van', 'Yozgat', 'Zonguldak', 'Aksaray', 'Bayburt', 'Karaman', 'Kırıkkale', 'Batman', 'Şırnak',
  'Bartın', 'Ardahan', 'Iğdır', 'Yalova', 'Karabük', 'Kilis', 'Osmaniye', 'Düzce'];

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
      confirmPassword: new FormControl('', [Validators.required]),
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      phoneNumber: new FormControl('', [
        Validators.required,
        Validators.minLength(10),
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
    const registerData: ISellerRegister = {
      email: this.registerForm.value.email,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword,
      firstName: this.registerForm.value.firstName,
      lastName: this.registerForm.value.lastName,
      phoneNumber: this.registerForm.value.phoneNumber,
      shopName: this.registerForm.value.storeName,
      address: {
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

  login() {
    const loginData: ILogin = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password,
    };

    this.authService.login(loginData).subscribe({
      next: (data) => {
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
