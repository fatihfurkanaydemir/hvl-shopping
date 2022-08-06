import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-seller-login',
  templateUrl: './seller-login.component.html',
  styleUrls: ['./seller-login.component.css']
})
export class SellerLoginComponent implements OnInit {

  registerForm!: FormGroup;
  loginForm!: FormGroup;
  
  constructor(private formBuilder : FormBuilder, private authService: AuthService) { }

  tabId: string = 'login';
  tabChange(id: string) {
    this.tabId = id;
    //console.log(id);
  }

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
    this.registerForm = this.formBuilder.group({
      email: new FormControl(""),
      password: new FormControl(""),
      confirmPassword: new FormControl(""),
      firstName: new FormControl(""),
      lastName: new FormControl(""),
      phoneNumber: new FormControl(""),
      address: new FormControl(""),
      storeName: new FormControl(""),
      city: new FormControl(""),
    }),
    this.loginForm = this.formBuilder.group({
      email: new FormControl(""),
      password: new FormControl(""),
    });
  }
  register(){
    this.authService
      .createUser(this.registerForm.getRawValue())
      .subscribe((response) => {
        this.registerForm = response.data;
        console.log(response)
      });
  }
  login(){
    this.authService
      .loginUser(this.loginForm.getRawValue())
      .subscribe((response) => {
        this.loginForm = response.data;
        console.log(response)
      });
  }
}
