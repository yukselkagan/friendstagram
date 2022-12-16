import { FriendsModalComponent } from './../friends-modal/friends-modal.component';
import { environment } from './../../../environments/environment';
import { Sharing } from './../../models/sharing';
import { SharingService } from './../../services/sharing.service';
import { FriendService } from './../../services/friend.service';
import { ProfileService } from './../../services/profile.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { CommonInformation } from 'src/app/models/common-information';
import { DataTransferService } from 'src/app/services/data-transfer.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  @ViewChild('friendsModal') friendsModal!: FriendsModalComponent;

  constructor(private dataTransferService: DataTransferService, private activatedRoute: ActivatedRoute,
    private profileService : ProfileService, private commonService: CommonService,
    private friendService : FriendService, private sharingService : SharingService) { }

  ngOnInit(): void {

      //once
      this.targetProfileUsername = this.activatedRoute.snapshot.paramMap.get('username') as string;
      if(this.targetProfileUsername != "self" && this.targetProfileUsername != null){
        this.selfProfile = false;
        this.getUserSelfInformation();
        this.getUserInformationForProfile(this.targetProfileUsername);
      }else{
        this.getUserSelfInformation(true);
      }

      //dynamic
      this.activatedRoute.params.subscribe(routeParams => {
        //console.log(routeParams['username']);
        this.targetProfileUsername = routeParams['username'];
        if(this.targetProfileUsername != "self" && this.targetProfileUsername != null){
          this.selfProfile = false;
          this.getUserSelfInformation();
          this.getUserInformationForProfile(this.targetProfileUsername);
        }else{
          this.getUserSelfInformation(true);
        }
      });





      //this.profileImageSrc = 'https://localhost:44330/ProfileImages/'+this.commonInformation.profilePicture;
  }

  selfProfile:boolean = true;
  targetProfileUsername:string = "self";

  commonInformationSubscription: Subscription = new Subscription();
  commonInformation:CommonInformation = new CommonInformation();

  profileId = 0;
  profileUsername = "Unknown";
  profileDisplayName = "Unknown";
  profileImageSrc = environment.baseUrl+'/Main/default-profile-image.jpg';

  profileFriendCount = 0;
  profileSharingCount = 0;

  baseProfileImageUrl = environment.baseUrl+'/ProfileImages/';
  defaultProfileImageSrc = environment.baseUrl+'/Main/default-profile-image.jpg';

  haveFriendship = false;
  haveFriendRequest = false;

  notExistUser = false;

  profileSharingList:Sharing[] = [];


  getUserSelfInformation(withSharings:boolean = false){
    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => {
        this.commonInformation = commonData;
        if(commonData.isAuthenticated){
          this.profileId = commonData['userId'];
          this.profileUsername = commonData['userName'];
          this.profileDisplayName = commonData['displayName'];
          if(commonData.profilePicture != null && commonData.profilePicture != "" ){
            this.profileImageSrc = environment.baseUrl+'/ProfileImages/'+commonData['profilePicture'];
          }
          if(withSharings){
            this.getSharingsForProfile(this.profileId);
          }
          this.countFriends(this.profileId);
        }
      });
  }


  getUserInformationForProfile(username:string){
    this.profileService.getUserbyUsername(username).subscribe({
      next : (response) => {
        //console.log(response);
        this.profileDisplayName = response['displayName'] ? response['displayName'] : response['userName'];
        this.profileUsername = response['userName'];
        this.profileId = response['userId'];
        this.profileImageSrc = response['profilePicture'] ? this.baseProfileImageUrl+response['profilePicture'] : this.defaultProfileImageSrc;
        this.checkForHaveFriendRequest(this.profileId);
        this.getSharingsForProfile(this.profileId);
        this.countFriends(this.profileId);

      },
      error : (error) => {
        console.log(error);
        if(error.error == "Can not find user" ){
          this.notExistUser = true;
        }else{
          this.commonService.showToast(error);
        }
      }
    })
  }

  addFriend(userId:any){
    this.friendService.sendFriendRequest(userId).subscribe({
      next : (response) => {
        this.haveFriendRequest = true;
        this.checkForHaveFriendRequest(userId);
        console.log(response);
      },
      error : (error) => {
        console.log(error);
        this.commonService.showToast(error.error);
      }
    })
  }

  revokeFriendshipRequest(userId:any){
    this.friendService.revokeFriendshipRequestByFriendId(userId).subscribe({
      next : (response) => {
        this.haveFriendRequest = false;
        console.log(response);
      },
      error : (error) => {
        this.commonService.showToast(error.error);
      }
    })
  }

  removeFriendship(friendId:any){
    this.friendService.removeFriendship(friendId).subscribe({
      next : (response) => {
        console.log(response);
        this.haveFriendship = false;
        this.haveFriendRequest = false;
      },
      error : (error) => {
        console.log(error);
      }
    })

  }


  checkForHaveFriendRequest(profileId:any){
    this.friendService.checkForFriendship(profileId).subscribe({
      next : (response) => {
        //console.log(response);
        if(response == "Friend"){
          this.haveFriendship = true;
        }else if(response == "Request"){
          this.haveFriendRequest = true;
        }else{

        }
      },
      error : (error) => {
        console.log(error);
        this.commonService.showToast(error);
      }
    })
  }

  getSharingsForProfile(inputId:number){
    this.sharingService.getSharingsByUserId(this.profileId).subscribe({
      next : (response) => {
        //console.log(response);
        this.profileSharingList = response;
        this.prepareGrid(this.profileSharingList.length);
        this.profileSharingCount = this.profileSharingList.length;
      },
      error : (error) => {
        console.log(error.error);
      }
    });
  }

  public prepareGrid(input:any){
    let rowNumber = Math.ceil(input / 3);
    for(let i=0; i < rowNumber; i++){
      this.gridRowList.push(i);
    }

  }

  gridRowList:any = [];
  gridColumnList = [1,2,3];


  openFriendsModal(){
    this.friendsModal.openModal();
  }

  countProfileParameters(){

  }

  private countFriends(userId:any){
    return this.friendService.getFriendships(userId).subscribe({
      next : (response) => {
        let friendCount = response.length;
        this.profileFriendCount = friendCount;
        console.log(this.profileFriendCount);
      },
      error : (error) => {
        console.log(error)
      }
    })
  }




}
