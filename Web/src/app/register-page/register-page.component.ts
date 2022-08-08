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

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css'],
})
export class RegisterPageComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  registerForm!: FormGroup;

  submitted = false;
  validPattern =
    '^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*/.?&])[A-Za-zd$@$!%*?&].{6,}$';
  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      lastName: [null, [Validators.required, Validators.min(1)]],
      firstName: [null, [Validators.required, Validators.min(1)]],
      phoneNumber: [
        null,
        [
          Validators.required,
          Validators.pattern('^[0-9].{1,}$'),
          Validators.min(1),
        ],
      ],
      email: ['', [Validators.required, Validators.email, Validators.min(1)]],
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(this.validPattern),
          Validators.min(1),
        ],
      ],
      confirmPassword: ['', [Validators.required, Validators.min(1)]],
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.registerForm.controls;
  }

  register() {
    this.submitted = true;
    if (this.registerForm.invalid) {
      return;
    } else {
    }
  }
}
