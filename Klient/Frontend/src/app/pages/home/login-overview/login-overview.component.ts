import { Component, OnInit } from '@angular/core';
import {HomeService} from "../../../Services/home.service";
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
  <a routerLink="/Graf">Go to graf</a>
</ion-row>
  <ion-row>
    <a routerLink="/Settings">Go to settings</a>
  </ion-row>
  <ion-row style="height: 100px">
    <a routerLink="/Log">Go to log</a>
  </ion-row>

  <ion-row>
  <ion-button size="small" (click)="LogOut()">LogOut</ion-button>
  <ion-button size="small" (click)="DeleteUser()">Delete User</ion-button>
  <ion-button size="small">Edit User</ion-button>
  </ion-row>



</ion-col>








  `,
  styleUrls: ['./login-overview.component.scss'],
})
export class LoginOverviewComponent  implements OnInit {

  constructor(private homeService: DataService,
              private utilitiesservice: UtilitiesService,
              private router: Router)
{ }

  ngOnInit() {}

  LogOut() {
    this.homeService.loginUser="";
    this.homeService.loginResponse="";

    this.homeService.UserActions("logOff");
  }

  async DeleteUser() {


    const response=await this.utilitiesservice.confirmDelete();

    if (response)
    {
      this.homeService.UserActions("delete");
      this.homeService.loginUser="";
      this.homeService.loginResponse="";

    }

  }
}
