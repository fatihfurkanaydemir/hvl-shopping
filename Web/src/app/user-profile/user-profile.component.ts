import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupName, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  userDisplayForm: FormGroup;

  constructor(fb: FormBuilder) { 
    this.userDisplayForm = new FormGroup({
      isim: new FormControl('', Validators.required),
      soyIsim: new FormControl('', Validators.required),
      sifre: new FormControl('', Validators.required),
      telNo: new FormControl('', Validators.required)
    });
  }

  ngOnInit(): void {
    this.createUserDisplayForm();
  }

  createUserDisplayForm(){
    this.userDisplayForm = new FormGroup({
      isim: new FormControl('', Validators.required),
      soyIsim: new FormControl('', Validators.required),
      sifre: new FormControl('', Validators.required),
      telNo: new FormControl('', Validators.required)
    })
  }
  
  onUserDisplayFormSubmit(){
    console.log(this.userDisplayForm.value);
  }

}
