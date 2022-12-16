import { CommonService } from 'src/app/services/common.service';
import { FriendshipRequest } from './../../models/friendship-request';
import { FriendService } from './../../services/friend.service';
import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonInformation } from 'src/app/models/common-information';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { DataTransferService } from 'src/app/services/data-transfer.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(private dataTransferService: DataTransferService,
    private authenticationService: AuthenticationService, private friendService: FriendService,
    private commonService: CommonService) { }

  ngOnInit(): void {

    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => {
        this.commonInformation = commonData;
        if(commonData.isAuthenticated){
          if(commonData.profilePicture != null && commonData.profilePicture != "" ){
            this.profileImageSrc = 'https://localhost:44356/ProfileImages/' + commonData['profilePicture'];
          }else{
            this.profileImageSrc = 'https://localhost:44356/Main/default-profile-image.jpg';
          }
        }
      });

  }

  commonInformationSubscription: Subscription = new Subscription();
  commonInformation:CommonInformation = new CommonInformation();

  changeAuthenticationStatus(inputBoolean: boolean){
    let newInformation = this.commonInformation;
    newInformation.isAuthenticated = inputBoolean;
    this.dataTransferService.changeCommonInformation(newInformation);
  }

  logout(){
    this.authenticationService.logout();
    console.log("logout");
  }

  profileImageSrc = 'https://localhost:44330/Main/default-profile-image.jpg';


  friendSuggestionList = [1,2,3];

  friendshipRequestList:FriendshipRequest[] = [];

  haveFriendShipRequestList:boolean = false;
  getFriendshipRequestList(){
    if(!this.haveFriendShipRequestList){
      this.friendService.getFriendshipRequestForUser().subscribe({
        next : (response) => {
          this.friendshipRequestList = response;
          this.haveFriendShipRequestList = true;
        },
        error : (error) => {
          console.log(error);
        }
      });
    }
  }

  public processDisplayName(userName:any, displayName:any){
    let processedName = this.commonService.processProfileDisplayName(userName, displayName);
    return processedName;
  }

  public processDisplayImage(imagePath:any){
    let processedImagePath = this.commonService.processProfileImage(imagePath);
    return processedImagePath;
  }

  public answerFriendshipRequest(requestId:any, accept:boolean){

    let postModel = {'friendshipRequestId': requestId, 'accepted': accept};

    this.friendService.answerFriendshipRequest(postModel).subscribe({
      next : (response) => {
        //console.log(response);
        //clean from menu
        var targetRequestIndex = this.friendshipRequestList.findIndex(x => x.friendshipRequestId == requestId);
        this.friendshipRequestList.splice(targetRequestIndex, 1);
      },
      error : (error) => {
        this.commonService.showToast(error);
      }
    })

  }


}
