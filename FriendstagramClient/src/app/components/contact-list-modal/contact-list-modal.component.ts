import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AppUser } from 'src/app/models/app-user';
import { CommonService } from 'src/app/services/common.service';
import { FriendService } from 'src/app/services/friend.service';

@Component({
  selector: 'app-contact-list-modal',
  templateUrl: './contact-list-modal.component.html',
  styleUrls: ['./contact-list-modal.component.scss']
})
export class ContactListModalComponent implements OnInit {
  @ViewChild('friendsModal') friendsModalElementRef!: ElementRef;
  @ViewChild('overlay') overlayElementRef!: ElementRef;

  @Output() startChatEvent = new EventEmitter<any>();


  constructor(private friendService: FriendService,
    private commonService: CommonService, private router: Router) { }

  ngOnInit(): void {
  }

  friendList: AppUser[] = [];

  public processProfileImage(imagePath:any){
    return this.commonService.processProfileImage(imagePath);
  }

  public processProfileDisplayName(userName:any, displayName:any){
    return this.commonService.processProfileDisplayName(userName, displayName);
  }

  public startChat(userId:number){
    this.closeModal();
    this.startChatEvent.emit(userId);
  }

  openModal(){
    this.loadFriends();

    this.friendsModalElementRef.nativeElement.classList.add('active');
    this.overlayElementRef.nativeElement.classList.add('active');
  }

  closeModal(){
    this.friendsModalElementRef.nativeElement.classList.remove('active');
    this.overlayElementRef.nativeElement.classList.remove('active');
  }

  loadFriends(){
    this.friendService.getFriendships(0).subscribe({
      next : (response) => {
        //console.log(response);
        this.friendList = response.map((x:any) =>{
          return x['friend'];
        });
        //console.log(this.friendList);
      },
      error : (error) => {
        console.log(error);
      }
    });
  }


}
