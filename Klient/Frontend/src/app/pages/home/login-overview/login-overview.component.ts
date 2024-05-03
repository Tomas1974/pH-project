import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login-overview',
  template: `

<ion-col>

  <ion-row>
    <h1 style="height: 50px">Successfully login</h1>

  </ion-row>
<ion-row>
  <a>Go to graf</a>
</ion-row>
  <ion-row>
    <a>Go to settings</a>
  </ion-row>
  <ion-row style="height: 100px">
    <a>Go to log</a>
  </ion-row>

  <ion-row>
  <ion-button size="small">LogOut</ion-button>
  <ion-button size="small">Delete User</ion-button>
  <ion-button size="small">Edit User</ion-button>
  </ion-row>



</ion-col>








  `,
  styleUrls: ['./login-overview.component.scss'],
})
export class LoginOverviewComponent  implements OnInit {

  constructor() { }

  ngOnInit() {}

}
