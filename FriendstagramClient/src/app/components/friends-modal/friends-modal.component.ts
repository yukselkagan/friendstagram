import { Router } from '@angular/router';
import { CommonService } from './../../services/common.service';
import { map } from 'rxjs/operators';
import { FriendService } from './../../services/friend.service';
import { Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { AppUser } from 'src/app/models/app-user';

@Component({
  selector: 'app-friends-modal',
  templateUrl: './friends-modal.component.html',
  styleUrls: ['./friends-modal.component.scss']
})
export class FriendsModalComponent implements OnInit {
  @ViewChild('friendsModal') friendsModalElementRef!: ElementRef;
  @ViewChild('overlay') overlayElementRef!: ElementRef;
  @Output() openModalEvent = new EventEmitter<string>();
  @Input() profileIdInput = 0;

  constructor(private friendService: FriendService,
    private commonService: CommonService, private router: Router) { }

  ngOnInit(): void {

    //this.loadFriends();
    //console.log("Profile id"+ this.profileIdInput);

  }



  openModal(){
    // console.log(this.friendsModalElementRef.nativeElement.innerHTML);
    // console.log(this.overlayElementRef.nativeElement.innerHTML);
    this.loadFriends(this.profileIdInput);

    this.friendsModalElementRef.nativeElement.classList.add('active');
    this.overlayElementRef.nativeElement.classList.add('active');

    //console.log("Profile id"+ this.profileIdInput);
  }

  closeModal(){
    this.friendsModalElementRef.nativeElement.classList.remove('active');
    this.overlayElementRef.nativeElement.classList.remove('active');
  }

  friendList: AppUser[] = [];

  loadFriends(userId:any){

    this.friendService.getFriendships(userId).subscribe({
      next : (response) => {
        console.log(response);
        this.friendList = response.map((x:any) =>{
          return x['friend'];
        });
        console.log(this.friendList);
      },
      error : (error) => {
        console.log(error);
      }
    });
  }

  public processProfileImage(imagePath:any){
    return this.commonService.processProfileImage(imagePath);
  }

  public processProfileDisplayName(userName:any, displayName:any){
    return this.commonService.processProfileDisplayName(userName, displayName);
  }

  public goToProfile(userName:any){
    this.closeModal();
    this.router.navigateByUrl('/'+userName);
  }



}
