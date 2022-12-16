import { environment } from './../../environments/environment';
import { AppResponse } from './../models/app-response';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FriendService {

  constructor(private httpClient : HttpClient) { }

  baseUrl:string = environment.apiUrl;

  public getFriendSuggestionList(){
    return this.httpClient.get<any>(this.baseUrl+"/Friend/GetFriendSuggestionList");
  }

  public sendFriendRequest(model:number){
    return this.httpClient.post<AppResponse>(this.baseUrl+"/Friend/SendFriendRequest", model);
  }

  public revokeFriendshipRequestByFriendId(model:any){
    return this.httpClient.post<AppResponse>(this.baseUrl+"/Friend/RevokeFriendshipRequestByFriendId", model);
  }

  // public checkForHaveFriendshipRequest(model:number){
  //   return this.httpClient.post<any>(this.baseUrl+"/Friend/CheckForHaveFriendshipRequest", model);
  // }

  public checkForFriendship(model:number){
    return this.httpClient.post(this.baseUrl+"/Friend/CheckForFriendship",
      model, {responseType: 'text'});
  }

  public getFriendshipRequestForUser(){
    return this.httpClient.get<any>(this.baseUrl+"/Friend/GetFriendshipRequestListForUser");
  }

  public answerFriendshipRequest(model:any){
    return this.httpClient.post<any>(this.baseUrl+"/Friend/AnswerFriendshipRequest", model);
  }

  public getFriendships(userId:any){
      return this.httpClient.get<any>(this.baseUrl+'/Friend/GetFriendships/'+userId);
  }

  public removeFriendship(model:any){
    return this.httpClient.post<any>(this.baseUrl+'/Friend/RemoveFriendship', model);
  }


}
