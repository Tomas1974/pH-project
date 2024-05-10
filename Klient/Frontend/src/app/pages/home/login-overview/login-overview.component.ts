import { Component, OnInit } from '@angular/core';
import {Route, Router} from "@angular/router";
import {DataService} from "../../../Services/Data.service";
import {UtilitiesService} from "../../../Services/utilities.service";

@Component({
  selector: 'app-login-overview',
  template: `

<ion-col>

  <ion-row>
    <h1 style="height: 50px">Successfully login</h1>

  </ion-row>
<ion-row>
  <a style="color: blue; text-decoration: underline; cursor: pointer;" routerLink="/Graf">Go to graf</a>
</ion-row>
  <ion-row>
    <a style="color: blue; text-decoration: underline; cursor: pointer;" routerLink="/Settings">Go to settings</a>
  </ion-row>
  <ion-row style="height: 100px">
    <a style="color: blue; text-decoration: underline; cursor: pointer;" routerLink="/Log">Go to log</a>
  </ion-row>

  <ion-row>
  <ion-button size="small" (click)="LogOut()">LogOut</ion-button>
  <ion-button size="small" (click)="DeleteUser()">Delete User</ion-button>
  <ion-button size="small" (click)="EditUser()">Edit User</ion-button>
  </ion-row>



</ion-col>



  `,
  styleUrls: ['./login-overview.component.scss'],
})
export class LoginOverviewComponent   {

  constructor(private dataservice: DataService,
              private utilitiesservice: UtilitiesService,
              )
{ }



  LogOut() {
    this.dataservice.loginUser="";
    this.dataservice.loginResponse="";
    this.dataservice.UserActions("logOff");
    this.dataservice.chooseComponent=1; //Her vælges login
  }

  async DeleteUser() {


    const response=await this.utilitiesservice.confirmDelete();

    if (response)
    {

      this.dataservice.UserActions("delete");
      this.dataservice.loginUser="";
      this.dataservice.chooseComponent=0; //Her vælges new User
      this.dataservice.loginResponse="";
    }

  }

  async EditUser() {


    this.dataservice.UserActions("logInInfo");

    await this.dataservice.timePromise(); //Den her venter på responsen fra websocket.

    this.dataservice.loginResponse="";
     this.dataservice.getUserInfo();


  }
}
