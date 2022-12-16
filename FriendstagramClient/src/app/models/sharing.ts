import { AppUser } from './app-user';
export class Sharing{
  public sharingId:number = 0;
  public userId:number = 0;

  public path:string = "";
  public sharingText:string = "";

  public showComment:boolean = false;

  public user?:AppUser = new AppUser();

  public comments?:any = "";

}
