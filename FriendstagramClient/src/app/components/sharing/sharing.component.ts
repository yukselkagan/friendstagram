import { ActivatedRoute } from '@angular/router';
import { SharingService } from './../../services/sharing.service';
import { Sharing } from './../../models/sharing';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-sharing',
  templateUrl: './sharing.component.html',
  styleUrls: ['./sharing.component.scss']
})
export class SharingComponent implements OnInit {

  constructor(private sharingService: SharingService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    //this.targetProfileUsername = this.activatedRoute.snapshot.paramMap.get('username') as string;
    let targetSharingId = this.activatedRoute.snapshot.paramMap.get('sharingId');
    console.log(targetSharingId);

    //this.GetAllSharings();
    this.getSharing(targetSharingId);
  }

  sharingReady:boolean = false;
  sharing:Sharing = new Sharing();

  commentForm : FormGroup = new FormGroup({
    comment : new FormControl(null, Validators.required)
  });

  getSharing(sharingId:any){
    this.sharingService.getSharing(sharingId).subscribe({
      next: (response) => {
        console.log(response);
        this.sharing = response;
        this.sharingReady = true;
      },
      error: (error) => {
        console.log(error);
      }
    })
  }

  public async GetAllSharings(){

    //all
    // let responseSharingList = await this.sharingService.getAllSharingsAsync();
    // this.sharingList = responseSharingList as Sharing[];

    //for main page
    let responseSharingList = await this.sharingService.getSharingsForMainPageAsync();
    let temp1 = responseSharingList as Sharing[];
    this.sharing = temp1[0];
    this.sharingReady = true;

  }

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



}
