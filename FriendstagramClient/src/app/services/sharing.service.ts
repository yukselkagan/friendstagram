import { environment } from './../../environments/environment';
import { AppResponse } from './../models/app-response';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class SharingService {

  constructor(private httpClient : HttpClient) { }

  baseUrl:string = environment.apiUrl;


  // getAllSharings(){
  //   this.getAllSharingsFromApi().subscribe({
  //     next : (response) => {
  //       console.log(response);
  //       return response;
  //     },
  //     error : (message) => {
  //       console.log(message);
  //     }
  //   });
  // }

  getAllSharingsAsync(){

      return new Promise((resolve, reject) => {
        this.getAllSharingsFromApi().subscribe({
          next : (response) => {
            resolve(response);
          },
          error : (error) => {
            console.log(error);
            reject(error);
          }
        })
     });

  }

  getSharingsForMainPageAsync(){
    return new Promise( (resolve, reject) => {
      this.getSharingsForMainPageFromApi().subscribe({
        next : (response) => {
          resolve(response);
        },
        error : (error) => {
          console.log(error);
          reject(error);
        }
      })
    });
  }

  private getSharingsForMainPageFromApi(){
    return this.httpClient.get<any>(this.baseUrl+'/Sharing/GetSharingsForMainPage');
  }

  private getAllSharingsFromApi(){
    return this.httpClient.get<AppResponse>(this.baseUrl+"/Sharing/GetAllSharings");
  }

  public postCommentToSharing(model:any){
    return this.httpClient.post<AppResponse>(this.baseUrl+"/Comment/CommentToSharing", model)
  }

  public getCommentsForSharing(sharingId:any){
    let params = new HttpParams().set('sharingId', sharingId);
    return this.httpClient.get<any>(this.baseUrl+"/Comment/GetCommentsForSharing", { params: params });
  }

  public getSharingsByUserId(userId:any){
    let params = new HttpParams().set('userId', userId);
    return this.httpClient.get<any>(this.baseUrl+"/Sharing/GetSharingsByUserId", { params: params } );
  }

  public getSharing(sharingId:number){
    return this.httpClient.get<any>(this.baseUrl + '/Sharing/GetSharing/'+sharingId);
  }


}
