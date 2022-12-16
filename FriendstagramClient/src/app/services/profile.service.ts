import { environment } from './../../environments/environment.prod';
import { AppResponse } from './../models/app-response';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private httpClient : HttpClient) { }

  baseUrl:string = environment.apiUrl;

  postChangeProfileInformation(model:any){
    console.log("model " + model);
    return this.httpClient.post<AppResponse>(this.baseUrl+"/User/ChangeProfileInformation", model);
  }

  getUserbyUsername(username:any){
    return this.httpClient.get<any>(this.baseUrl+"/User/GetUserByUsername/"+username);
  }

  getUserById(userId:number){
    return this.httpClient.get<any>(this.baseUrl + '/User/GetUserById/'+userId);
  }

}
