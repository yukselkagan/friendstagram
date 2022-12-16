import { map } from 'rxjs/operators';
import { CommonService } from './../../services/common.service';
import { FriendService } from './../../services/friend.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SharingService } from './../../services/sharing.service';
import { DataTransferService } from 'src/app/services/data-transfer.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonInformation } from 'src/app/models/common-information';
import { Subscription } from 'rxjs';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Sharing } from 'src/app/models/sharing';
import { AppUser } from 'src/app/models/app-user';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {

  constructor(private authenticationService : AuthenticationService,
    private dataTransferService : DataTransferService, private http: HttpClient,
    private sharingService : SharingService, private friendService : FriendService,
    private commonService : CommonService) { }

  ngOnInit(): void {

    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => this.commonInformation = commonData );


      this.GetAllSharings();

    this.friendService.getFriendSuggestionList().pipe(
      map(x => {
        x.map((y:any) => {
          y['displayName'] = y['displayName'] ? y['displayName'] : y['userName'];
          return y;
        });
        return x;
      })
    ).subscribe({
      next : (response) => {
        //console.log(response);
        this.friendSuggestionList = response as any;
      },
      error : (error) => {
        this.commonService.showToast(error);
      }
    });



  }

  isOpenComment: boolean = false;

  friendSuggestionList: AppUser[] = [];

  public async GetAllSharings(){

    //all
    // let responseSharingList = await this.sharingService.getAllSharingsAsync();
    // this.sharingList = responseSharingList as Sharing[];

    //for main page
    let responseSharingList = await this.sharingService.getSharingsForMainPageAsync();
    this.sharingList = responseSharingList as Sharing[];

  }

  public commentToSharing(sharing: Sharing, sharingIdInput:number = 0){
    let sharingId = sharing.sharingId;

    let comment = this.commentForm.controls['comment'].value;
    let commentToSharingObject = { "SharingId": sharingId, "CommentText" : comment }

    this.sharingService.postCommentToSharing(commentToSharingObject).subscribe({
      next : (response) => {
        //console.log(response);
        this.getCommentsForSharing(sharing);
      },
      error : (error) => {
        console.log(error);

      }
    });

    this.commentForm.reset();

    console.log("commented"+ sharingId);
    console.log(comment);
  }

  commentForm : FormGroup = new FormGroup({
    comment : new FormControl(null, Validators.required)
  })



  subscription: Subscription = new Subscription();
  commonInformationSubscription: Subscription = new Subscription();
  commonInformation:CommonInformation = new CommonInformation();


  testString = "Hello";


  sharingList: Sharing[] = [

    /*
    test
    {sharingId : 1, sharingText : "Hello how are you", path : 'c7mSEbjZEaf3BCSgq8XA.jpg',
    userId : 1, showComment : false},
    {sharingId : 2, sharingText : "Wazap", path : 'GBbdWVKSjUCeVaDx1QqstA.jpg',
    userId : 1, showComment : false},
    {sharingId : 3, sharingText : "WWW", path : 'wSs2CZe2Emsm57rnxxgA.jpg',
    userId : 1, showComment : false}
    */
  ];


  //              src="assets/images/legolas1.jpg"

  public processProfileImage(imagePath:any){
    if(imagePath == null){
      return 'https://localhost:44356/Main/default-profile-image.jpg';
    }else{
      return 'https://localhost:44356/ProfileImages/'+ imagePath;
    }
  }

  public processProfileDisplayName(userName:any, displayName:any){
    if(displayName == null){
      return '@'+userName;
    }else{
      return displayName;
    }
  }

  public toggleComments(sharing:Sharing){
    sharing.showComment = !sharing.showComment;
    this.getCommentsForSharing(sharing);
  }


  public getCommentsForSharing(sharing:Sharing){
    this.sharingService.getCommentsForSharing(sharing.sharingId).subscribe({
      next : (response) => {
        console.log(response);
        sharing.comments = response;
      },
      error : (error) => {
        console.log(error);
      }
    });
  }








}
