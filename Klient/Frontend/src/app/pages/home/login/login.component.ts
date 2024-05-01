import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {DataService} from "../../../Services/Data.service";

@Component({
  selector: 'app-login2',
  template: `



    <ion-row>
      <ion-col size="3.3">
        <ion-item>

          <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.brugerNavn">
            <div slot="label">Brugernavn
              <ion-text *ngIf="!ValidateData.controls.brugerNavn.valid" color="danger">(min. 6 karakter)</ion-text>
            </div>

          </ion-input>
        </ion-item>
      </ion-col>
    </ion-row>


    <ion-row>
      <ion-col size="3.3">
        <ion-item>
          <ion-input labelPlacement="stacked" [type]="showPassword ? 'text' : 'password'"
                     [formControl]="ValidateData.controls.kodeord">
            <div slot="label">Kodeord
              <ion-text *ngIf="!ValidateData.controls.kodeord.valid" color="danger">(min. 8 kar. min 1 special)</ion-text>
            </div>
          </ion-input>
          <ion-icon [name]="showPassword ? 'eye-off' : 'eye'" slot="end"
                    (click)="togglePasswordVisibility()"></ion-icon>
        </ion-item>
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
                (keydown.enter)="selectUser()">Save
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

    brugerNavn: ["",[Validators.required, Validators.minLength(6)]],
    kodeord: ["", [Validators.required, Validators.minLength(8), Validators.pattern(".*\\W.*")]],
  })


  constructor(public dataservice: DataService,
              public formbuilder: FormBuilder) { }




  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }


  selectUser() {

  }

  UnselectUser() {
    this.ValidateData.controls.brugerNavn.setValue("");
    this.ValidateData.controls.kodeord.setValue("");
  }
}
