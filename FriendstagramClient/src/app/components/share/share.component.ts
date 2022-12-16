import { environment } from './../../../environments/environment.prod';
import { Router } from '@angular/router';
import { CommonService } from './../../services/common.service';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-share',
  templateUrl: './share.component.html',
  styleUrls: ['./share.component.scss']
})
export class ShareComponent implements OnInit {

  constructor(private httpClient: HttpClient, private commonService : CommonService,
    private router : Router) { }

  ngOnInit(): void {
  }



  public progress: number = 0;
  public message: string = "";
  @Output() public onUploadFinished = new EventEmitter();

  sharingText : string = "";
  imagePreviewSource : string = "assets/images/image-preview.png";


  public uploadFile = (files:any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    formData.append('sharingText', this.sharingText);

    this.httpClient.post<any>(environment.apiUrl+'/Sharing/UploadImage', formData, {reportProgress: true, observe: 'events'})
      .subscribe({
        next : (event) => {
          console.log(event);
          if (event.type === HttpEventType.UploadProgress)
            this.progress = Math.round(100 * event.loaded / (event.total ? event.total : 100) );
          else if (event.type === HttpEventType.Response) {
            this.message = 'Upload success';
            this.onUploadFinished.emit(event.body);
            this.router.navigateByUrl("/");
          }
        },
        error : (error) => {
          console.log("error");
          console.log(error);
          this.commonService.showToast(error.error);
        }
      });
  }




  public uploadFileOld = (files:any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    formData.append('sharingText', this.sharingText);
    this.httpClient.post('https://localhost:44330/api/Sharing/UploadImage', formData, {reportProgress: true, observe: 'events'})
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / (event.total ? event.total : 100) );
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success';
          this.onUploadFinished.emit(event.body);
        }
      });
  }






  onUploadImageChange(event: any){
    const reader = new FileReader();

    const [file] = event.target.files;
    reader.readAsDataURL(file);

    reader.onload = () => {
      this.imagePreviewSource = reader.result as string;
      this.progress = 0;
    };

  }



}
