import { Component, OnInit } from '@angular/core';
import {HomeService} from "../Service/home.service";
import {Route, Router} from "@angular/router";

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
  <ion-button size="small">Delete User</ion-button>
  <ion-button size="small">Edit User</ion-button>
  </ion-row>



</ion-col>








  `,
  styleUrls: ['./login-overview.component.scss'],
})
export class LoginOverviewComponent  implements OnInit {

  constructor(private homeService: HomeService,
              private router: Router)
{ }

  ngOnInit() {}

  LogOut() {
       this.homeService.checkIfAnyoneHasLoggedIn();
  }
}