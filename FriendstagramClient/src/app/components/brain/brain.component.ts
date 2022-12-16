import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonInformation } from 'src/app/models/common-information';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { DataTransferService } from 'src/app/services/data-transfer.service';

@Component({
  selector: 'app-brain',
  templateUrl: './brain.component.html',
  styleUrls: ['./brain.component.scss']
})
export class BrainComponent implements OnInit {

  constructor(private dataTransferService: DataTransferService,
    private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    if(localStorage.getItem('token') != null){
      this.authenticationService.getUserInformation();
    }

    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => this.commonInformation = commonData );
  }

  subscription: Subscription = new Subscription();
  commonInformationSubscription: Subscription = new Subscription();
  commonInformation:CommonInformation = new CommonInformation();

}
