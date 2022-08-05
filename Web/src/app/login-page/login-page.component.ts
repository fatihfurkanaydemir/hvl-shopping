import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { Emitters } from '../emitters/emitters';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {

  constructor(private formBuilder : FormBuilder, private authService : AuthService, private router : Router) { }

  loginForm! : FormGroup

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: new FormControl(''),
      password: new FormControl('')
    })
  }

  login(){
    this.authService
      .loginUser(this.loginForm.getRawValue())
      .subscribe((response) => {
        next: {
          Emitters.authEmitter.emit(true)
        }
        error: (err : any) => {
          Emitters.authEmitter.emit(false)
        }
        if(response.succeeded){
          this.router.navigate(['']);
        }
        this.loginForm = response.data;
        console.log(response)
      });
  }

}
