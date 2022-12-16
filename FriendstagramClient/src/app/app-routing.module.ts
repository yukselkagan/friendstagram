import { SharingComponent } from './components/sharing/sharing.component';
import { MessageComponent } from './components/message/message.component';
import { ExploreComponent } from './components/explore/explore.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { ProfileEditComponent } from './components/profile-edit/profile-edit.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ShareComponent } from './components/share/share.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {path : 'home', component : HomeComponent},
  {path : 'share', component : ShareComponent},
  {path : 'profile', component : ProfileComponent},
  {path : 'profile/edit', component : ProfileEditComponent, canActivate : [AuthenticationGuard]},
  {path : 'settings', component : ProfileEditComponent, canActivate : [AuthenticationGuard]},
  {path : 'explore', component : ExploreComponent},
  {path : 'message', component : MessageComponent},
  {path : 'login', component : LoginComponent},
  {path : 'register', component : RegisterComponent },
  {path : '', component : HomeComponent},
  {path:  's/:sharingId', component : SharingComponent},
  {path:  ':username', component : ProfileComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
