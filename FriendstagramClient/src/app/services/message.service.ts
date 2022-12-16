import { environment } from './../../environments/environment.prod';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private httpClient: HttpClient) { }

  baseUrl:string = environment.apiUrl;

  public sendMessage(model:any){
    return this.httpClient.post<any>(this.baseUrl + '/Message/SendMessage', model);
  }

  public getMessagesForChat(friendId:number){
    let paramsForRequest = new HttpParams().set('friendId', friendId);
    return this.httpClient.get<any>(this.baseUrl + '/Message/GetMessagesForChat', { params: paramsForRequest });
  }

  public getChatList(){
    return this.httpClient.get<any>(this.baseUrl + '/Message/GetChats');
  }


}
