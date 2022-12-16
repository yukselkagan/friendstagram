import { AppUser } from './app-user';
export class ChatModel{
  chatId : number = 0;
  user : AppUser = new AppUser();
  friend : AppUser = new AppUser();

  userId : number = 0;
  friendId : number = 0;

  updatedAt : any;

}
