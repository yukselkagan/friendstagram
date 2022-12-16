import { AuthenticationGuard } from './guards/authentication.guard';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { FooterComponent } from './components/footer/footer.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MainComponent } from './components/main/main.component';
import { ReceptionComponent } from './components/reception/reception.component';
import { HeaderComponent } from './components/header/header.component';
import { BrainComponent } from './components/brain/brain.component';
import { ShareComponent } from './components/share/share.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ProfileEditComponent } from './components/profile-edit/profile-edit.component';
import { ToastComponent } from './components/toast/toast.component';
import { PopUpComponent } from './components/pop-up/pop-up.component';
import { FriendsModalComponent } from './components/friends-modal/friends-modal.component';
import { ExploreComponent } from './components/explore/explore.component';
import { MessageComponent } from './components/message/message.component';
import { ContactListModalComponent } from './components/contact-list-modal/contact-list-modal.component';
import { SharingComponent } from './components/sharing/sharing.component'

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    MainComponent,
    ReceptionComponent,
    HeaderComponent,
    BrainComponent,
    ShareComponent,
    ProfileComponent,
    ProfileEditComponent,
    ToastComponent,
    PopUpComponent,
    FriendsModalComponent,
    ExploreComponent,
    MessageComponent,
    ContactListModalComponent,
    SharingComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    {provide : HTTP_INTERCEPTORS, useClass : TokenInterceptor, multi : true},
    {provide : HTTP_INTERCEPTORS, useClass : ErrorInterceptor, multi : true},
    AuthenticationGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
