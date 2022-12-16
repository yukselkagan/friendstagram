import { environment } from './../../environments/environment.prod';
import { AppResponse } from './../models/app-response';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { DataTransferService } from './data-transfer.service';
import { CommonInformation } from '../models/common-information';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private httpClient: HttpClient, private dataTransferService: DataTransferService,
    private router: Router) { }

  baseUrl:string = environment.apiUrl;


  subscription: Subscription = new Subscription();
  commonInformationSubscription: Subscription = new Subscription();
  commonInformation:CommonInformation = new CommonInformation();


  public getToken(): any {
    return localStorage.getItem('token');
  }

  public postRegisterForm(model:any){
    return this.httpClient.post<any>(this.baseUrl+"/User/Register", model);
  }

  public postLoginForm(model:any){
    return this.httpClient.post<any>(this.baseUrl+"/User/Login", model);
  }



  public logout(){
    this.removeOldToken();
    this.changeAuthenticationStatus(false);
    this.clearCommonInformation();
    this.router.navigateByUrl("/");
  }

  public removeOldToken(){
    localStorage.removeItem('token');
  }

  public changeAuthenticationStatus(inputBoolean: boolean){
    let newInformation = this.commonInformation;
    newInformation.isAuthenticated = inputBoolean;
    this.dataTransferService.changeCommonInformation(newInformation);
  }

  public clearCommonInformation(){
    let newInformation = new CommonInformation();
    this.dataTransferService.changeCommonInformation(newInformation);
  }

  public changeCommonInformation(input: CommonInformation){
    let newInformation = this.commonInformation;
    newInformation.isAuthenticated = true;
    newInformation.userId = input.userId;
    newInformation.email = input.email;
    newInformation.userName = input.userName;
    newInformation.displayName = input.displayName;
    newInformation.profilePicture = input.profilePicture;

    this.dataTransferService.changeCommonInformation(newInformation);
  }


  private getUserFromApi(){
    return this.httpClient.get<any>(this.baseUrl+"/User/GetUserSelf");
  }

  public getUserInformation() {
    this.getUserFromApi().subscribe({
      next: (response) => {
        //console.log(response);
        let newCommonInformation = new CommonInformation();

        newCommonInformation.userId = response['userId'];
          newCommonInformation.email = response['email'];
          newCommonInformation.userName = response['userName'];
          newCommonInformation.displayName = response['displayName'] ? response['displayName'] : response['userName'];
          newCommonInformation.profilePicture = response['profilePicture'];
          this.changeCommonInformation(newCommonInformation);
      },
      error: (msg) => {
        console.log(msg);
        this.logout();
        if (msg['status'] == 401) {
          localStorage.removeItem('token');
          console.log('401 error');
        }
      },
    });
  }



}
