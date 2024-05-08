import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {DataService} from "../../../Services/Data.service";
import {LoginModel} from "../../../Models/userModel";
import {HomeService} from "../../../Services/home.service";

@Component({
  selector: 'app-login2',
  template: `


    <H1>Login</H1>
    <ion-row>
      <ion-col size="3.3">
        <ion-item>

          <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.email">
            <div slot="label">Email
              <ion-text *ngIf="!ValidateData.controls.email.valid"></ion-text>
            </div>

          </ion-input>
        </ion-item>
      </ion-col>
    </ion-row>


    <ion-row>
      <ion-col size="3.3">
        <ion-item>
          <ion-input labelPlacement="stacked" [type]="showPassword ? 'text' : 'password'"
                     [formControl]="ValidateData.controls.password">
            <div slot="label">Password
              <ion-text *ngIf="!ValidateData.controls.password.valid"></ion-text>
            </div>
          </ion-input>
          <ion-icon [name]="showPassword ? 'eye-off' : 'eye'" slot="end"
                    (click)="togglePasswordVisibility()"></ion-icon>
        </ion-item>
      </ion-col>
    </ion-row>
<ion-row>
  <ion-col>
    <p style="height: 7px;color: red">{{dataservice.loginResponse}}</p>
  </ion-col>
</ion-row>
    <ion-button style=".grey {
                  --ion-color-base: grey !important;
                    --ion-color-base-rgb: 128,128,128 !important;
}

" [disabled]="!ValidateData.valid"
                [class.grey]="!ValidateData.valid"

                size="small"
                (click)="selectUser()"
                (keydown.enter)="selectUser()">Login
    </ion-button>


    <ion-button

      size="small" (click)="UnselectUser()"
      (keydown.enter)="UnselectUser()"
    >Clear
    </ion-button>



  `,
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent   {

  showPassword: boolean = false;


  ValidateData=this.formbuilder.group({

    email: ["", [Validators.required, Validators.email, Validators.minLength(6)]],
    password: ["", [Validators.required, Validators.minLength(8), Validators.pattern(".*\\W.*")]],
  })


  constructor(public dataservice: DataService,
              public formbuilder: FormBuilder) { }




  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }


  selectUser() {


    let loginuser:LoginModel={

      email: this.ValidateData.controls.email.value+"",
      password: this.ValidateData.controls.password.value+"",

    };

    this.dataservice.LoginUser(loginuser)


  }

  UnselectUser() {
    this.ValidateData.controls.email.setValue("");
    this.ValidateData.controls.password.setValue("");

  }
}
