import { SharingService } from './../../services/sharing.service';
import { Component, OnInit } from '@angular/core';
import { Sharing } from 'src/app/models/sharing';

@Component({
  selector: 'app-explore',
  templateUrl: './explore.component.html',
  styleUrls: ['./explore.component.scss']
})
export class ExploreComponent implements OnInit {

  constructor(private sharingService: SharingService) { }

  ngOnInit(): void {
    this.GetAllSharings();
  }


  sharingList: Sharing[] = [];

  public async GetAllSharings(){
    let responseSharingList = await this.sharingService.getAllSharingsAsync();
    this.sharingList = responseSharingList as Sharing[];
  }


}
