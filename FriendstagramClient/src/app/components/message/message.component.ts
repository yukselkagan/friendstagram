import { CommonService } from 'src/app/services/common.service';
import { ContactListModalComponent } from './../contact-list-modal/contact-list-modal.component';
import { ProfileService } from './../../services/profile.service';
import { FriendService } from './../../services/friend.service';
import { AppUser } from 'src/app/models/app-user';
import { MessageService } from './../../services/message.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ChatModel } from 'src/app/models/chat-model';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {
  @ViewChild('contactListModal') contactListModal!: ContactListModalComponent;

  constructor(private messageService: MessageService, private friendService: FriendService,
    private profileService: ProfileService, private commonService: CommonService) { }

  ngOnInit(): void {

    //test
    //this.getContactFriendInformation(3);

    this.getChatList();

  }

  contactFriendId:number = 0;
  contactFriendInformation: AppUser = new AppUser();

  chatMessages = [];
  chatList: ChatModel[] = [];

  messageForm: FormGroup = new FormGroup({
    message : new FormControl(null, Validators.required)
  });


  sendMessage(){
    let receiverId = this.contactFriendId;
    let messageText = this.messageForm.controls['message'].value;

    let postModel = {'receiverId': receiverId, messageText: messageText};

    if(messageText == "" || !messageText){
      return;
    }

    this.messageService.sendMessage(postModel).subscribe({
      next : (response) => {
        console.log(response);
        this.getMessagesForChat(this.contactFriendId);
      },
      error : (error) => {
        console.log(error);
      }
    })

    this.messageForm.reset();
  }

  getContactFriendInformation(targetId:any){
    //targetId = 2;

    this.profileService.getUserById(targetId).subscribe({
      next : (response) => {
        //console.log(response);
        this.contactFriendInformation = response;
        this.contactFriendId = response['userId'];
        this.getMessagesForChat(this.contactFriendId);
      },
      error : (error) => {
        console.log(error);
      }
    })
  }

  getMessagesForChat(friendId:number){
    console.log("get messages" + friendId);
    this.messageService.getMessagesForChat(friendId).subscribe({
      next : (response) => {
        console.log(response);
        this.chatMessages = response;
      },
      error : (error) => {
        console.log(error);
      }
    })
  }


  openNewChatPanel(){
    this.contactListModal.openModal();
  }

  startChat(event:any){
    let targetId = event;
    this.getContactFriendInformation(targetId);
  }

  getChatList(){
    this.messageService.getChatList().subscribe({
      next : (response) => {
        console.log(response);
        this.chatList = response;
      },
      error : (error) => {
        console.log(error);
      }
    })
  }

  public processProfileDisplayName(userName:any, displayName:any){
    return this.commonService.processProfileDisplayName(userName, displayName);
  }

  public processProfileImage(imagePath:any){
    return this.commonService.processProfileImage(imagePath);
  }







}
