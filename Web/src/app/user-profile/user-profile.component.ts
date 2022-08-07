import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupName, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  userDisplayForm: FormGroup;

  myCustomerUser: any = ({
    IdentityId: "temp-Identity-Id",
    FirstName: "temp-first-name",
    LastName: "temp-last-name",
    PhoneNumber: "temp-phone-number",
    Addresses: "temp-adressess",
    Password: "temp-password"
  });

  myChangedCustomer: Object;

  constructor(fb: FormBuilder) { 
    this.userDisplayForm = new FormGroup({
      isim: new FormControl('', Validators.required),
      soyIsim: new FormControl('', Validators.required),
      sifre: new FormControl('', Validators.required),
      telNo: new FormControl('', Validators.required),
      adresler: new FormControl(['temp-adres-1', 'temp-adres-2'], Validators.required)
    });

    this.myCustomerUser = new Object({
      IdentityId: "temp-Identity-Id",
      FirstName: "temp-first-name",
      LastName: "temp-last-name",
      PhoneNumber: "temp-phone-number",
      Addresses: "temp-adresses",
      Password: "temp-password"
    });

    this.myChangedCustomer = new Object({

    });
  }

  ngOnInit(): void {
    
  }

  getChangedCustomer(){
    this.myCustomerUser = {
      IdentityId: "temp-Identity-Id",
      FirstName: this.userDisplayForm.controls['isim'].value,
      LastName: this.userDisplayForm.controls['soyIsim'].value,
      PhoneNumber: this.userDisplayForm.controls['telNo'].value,
      Addresses: this.userDisplayForm.controls['adresler'].value,
      Password: this.userDisplayForm.controls['sifre'].value
    };
  }
  
  onUserDisplayFormSubmit(){
    this.getChangedCustomer();
    console.log(this.userDisplayForm.value);
    console.log(this.myCustomerUser)
  }

}
