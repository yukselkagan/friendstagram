import { Router } from '@angular/router';
import { CommonService } from './../../services/common.service';
import { AuthenticationService } from './../../services/authentication.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegisterForm } from 'src/app/models/register-form';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  constructor(
    private authenticationService: AuthenticationService,
    private commonService: CommonService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  registerForm: FormGroup = new FormGroup({
    email: new FormControl(null, [Validators.required, Validators.email]),
    username: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required),
  });

  submitRegisterForm() {
    let newRegisterForm = new RegisterForm();
    newRegisterForm.username = this.registerForm.controls['username'].value;
    newRegisterForm.email = this.registerForm.controls['email'].value;
    newRegisterForm.password = this.registerForm.controls['password'].value;

    if (this.registerForm.valid) {
      this.authenticationService
        .postRegisterForm(newRegisterForm)
        .subscribe((response) => {
          let token = response['accessToken'];
          localStorage.setItem('token', token);
          this.authenticationService.changeAuthenticationStatus(true);
          this.authenticationService.getUserInformation();
          this.router.navigateByUrl('/');
        });
    } else {
      this.registerForm.markAllAsTouched();
      this.commonService.showToast('Need complete form');
    }
  }
}
