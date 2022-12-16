import { CommonService } from './../../services/common.service';
import { DataTransferService } from './../../services/data-transfer.service';
import { AuthenticationService } from './../../services/authentication.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoginForm } from 'src/app/models/login-form';
import { CommonInformation } from 'src/app/models/common-information';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  constructor(
    private authenticationService: AuthenticationService,
    private dataTransferService: DataTransferService,
    private router: Router,
    private commonService: CommonService
  ) {}

  ngOnInit(): void {}

  commonInformationSubscription: Subscription = new Subscription();
  commonInformation: CommonInformation = new CommonInformation();

  loginForm: FormGroup = new FormGroup({
    email: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required),
  });

  submitLoginForm() {
    let newLoginForm = new LoginForm();
    newLoginForm.email = this.loginForm.controls['email'].value;
    newLoginForm.password = this.loginForm.controls['password'].value;

    if (this.loginForm.valid) {
      this.authenticationService.postLoginForm(newLoginForm).subscribe({
        next: (response) => {
          console.log(response);
          let token = response['accessToken'];
          localStorage.setItem('token', token);
          this.authenticationService.changeAuthenticationStatus(true);
          this.authenticationService.getUserInformation();
          this.router.navigateByUrl('/');
        },
        error: (error) => {
          console.log(error);
          if(error.status != 0){
            this.commonService.showToast(error.error);
          }
        },
      });
    } else {
      this.loginForm.markAllAsTouched();
      this.commonService.showToast('Need complete form');
    }
  }

  changeCommonInformation(input: CommonInformation) {
    let newInformation = this.commonInformation;
    newInformation.isAuthenticated = true;
    newInformation.email = input.email;
    this.dataTransferService.changeCommonInformation(newInformation);
  }
}
