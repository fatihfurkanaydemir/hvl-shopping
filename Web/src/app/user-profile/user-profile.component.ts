import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupName, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  userSettingsForm: FormGroup;

  myCustomerUser: any = ({
    IdentityId: "temp-Identity-Id",
    FirstName: "temp-first-name",
    LastName: "temp-last-name",
    EMail: "temp-Email@gmail.com",
    PhoneNumber: "051231241241215",
    Addresses: "temp-adressess",
    Password: "temp-password"
  });

  myChangedCustomer: Object;

  constructor(fb: FormBuilder) { 
    this.userSettingsForm = new FormGroup({
      isim: new FormControl('', Validators.required),
      soyIsim: new FormControl('', Validators.required),
      telNo: new FormControl('', Validators.required),
      adres1: new FormControl('', Validators.required),
      adres2: new FormControl('', Validators.required),
      email: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
      city: new FormControl('', Validators.required)
    });

    this.myCustomerUser = new Object({
      IdentityId: "temp-Identity-Id",
      FirstName: "temp-first-name",
      LastName: "temp-last-name",
      EMail: "temp-Email@gmail.com",
      PhoneNumber: "051231241241215",
      Addresses: "temp-adresses",
      Password: "temp-password"
    });

    this.myChangedCustomer = new Object({
      
    });
  }

  ngOnInit(): void {
    
  }

  setChangedCustomer(){
      this.myChangedCustomer = {
        IdentityId: this.myCustomerUser.IdentityId,
        FirstName: this.userSettingsForm.controls['isim'].value,
        LastName: this.userSettingsForm.controls['soyIsim'].value,
        PhoneNumber: this.userSettingsForm.controls['telNo'].value,
        Adres1: this.userSettingsForm.controls['adres1'].value,
        Adres2: this.userSettingsForm.controls['adres2'].value,
        Email: this.userSettingsForm.controls['email'].value,
        Country: this.userSettingsForm.controls['country'].value,
        City: this.userSettingsForm.controls['city'].value
      };
  }
  
  onUserDisplayFormSubmit(){
    this.setChangedCustomer();
    console.log(this.myChangedCustomer);
  }

  onUserSettingsFormSubmit(){
    this.setChangedCustomer();
    console.log(this.userSettingsForm.value);
    console.log(this.myChangedCustomer);
  }
}
