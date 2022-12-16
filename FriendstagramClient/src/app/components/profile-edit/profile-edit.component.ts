import { environment } from './../../../environments/environment.prod';
import { CommonService } from './../../services/common.service';
import { AppResponse } from './../../models/app-response';
import { AuthenticationService } from './../../services/authentication.service';
import { ProfileService } from './../../services/profile.service';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonInformation } from 'src/app/models/common-information';
import { DataTransferService } from 'src/app/services/data-transfer.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.scss'],
})
export class ProfileEditComponent implements OnInit {
  constructor(
    private httpClient: HttpClient,
    private dataTransferService: DataTransferService,
    private profileService: ProfileService,
    private authenticationService: AuthenticationService,
    private router: Router,
    private commonService: CommonService
  ) {}

  ngOnInit(): void {
    this.commonInformationSubscription =
      this.dataTransferService.currentCommonInformation.subscribe(
        (commonData) => {
          this.commonInformation = commonData;

          if (this.commonInformation.isAuthenticated) {
            if (this.commonInformation.profilePicture != null) {
              this.imagePreviewSource =
                this.baseProfilePictureUrl + commonData.profilePicture;
            } else {
              this.imagePreviewSource =
                environment.baseUrl + '/Main/default-profile-image.jpg';
            }
          }
        }
      );
  }

  subscription: Subscription = new Subscription();
  commonInformationSubscription: Subscription = new Subscription();
  commonInformation: CommonInformation = new CommonInformation();

  baseProfilePictureUrl: string = environment.baseUrl + '/ProfileImages/';
  imagePreviewSource: string =
    environment.baseUrl + '/Main/default-profile-image.jpg';

  public progress: number = 0;
  public message: string = '';
  @Output() public onUploadFinished = new EventEmitter();

  previewProfileImage(event: any) {
    const reader = new FileReader();

    //const [file] = event.target.files;
    const file = event.target.files[0];
    reader.readAsDataURL(file);

    reader.onload = () => {
      this.imagePreviewSource = reader.result as string;
    };
  }

  public uploadProfileImage = (files: any) => {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.httpClient
      .post(environment.apiUrl + '/User/ChangeProfilePicture', formData, {
        reportProgress: true,
        observe: 'events',
      })
      .subscribe((event) => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(
            (100 * event.loaded) / (event.total ? event.total : 100)
          );
        else if (event.type === HttpEventType.Response) {
          //console.log(event.body);
          let responseBody = event.body as AppResponse;
          this.message = 'Upload success';
          this.onUploadFinished.emit(event.body);
          this.authenticationService.getUserInformation();
          this.router.navigateByUrl('/profile');
        }
      });
  };

  changeProfileInformation = (input: any) => {
    let postObjectValue = { displayName: input };

    this.profileService
      .postChangeProfileInformation(postObjectValue)
      .subscribe((response) => {
        this.authenticationService.getUserInformation();
        this.router.navigateByUrl('/profile');
      });
  };
}
