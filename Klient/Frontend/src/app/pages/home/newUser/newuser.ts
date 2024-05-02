import {Component, ElementRef, NgZone, ViewChild} from '@angular/core';
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {Address, AddressAPIJsonResponseModel } from "../../../Models/LookupModels";
import {DataService} from "../../../Services/Data.service";
import {UserModel} from "../../../Models/userModel";



@Component({
  selector: 'app-login',
  template: `



    <ion-grid>

      <ion-row>
        <ion-col size="3.3">
          <ion-item>

            <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.name">
              <div slot="label">Full name
                <ion-text *ngIf="!ValidateData.controls.name.valid" color="danger">Min 6 cha.</ion-text>
              </div>
            </ion-input>
          </ion-item>
        </ion-col>
      </ion-row>


      <ion-row>
        <ion-col size="2.3">

          <ion-item>

            <ion-input
              [formControl]="ValidateData.controls.street"
              (ionInput)="updateSuggestions()"
              (ionBlur)="hideSuggestions()"
              (keydown)="handleKeyDown($event)"
              labelPlacement="stacked"
            >
              <div slot="label">Street
                <ion-text *ngIf="!ValidateData.controls.street.valid" color="danger">Min 3 cha.</ion-text>
              </div>
            </ion-input>
          </ion-item>

          <ion-list *ngIf="showSuggestions">
            <ion-item-option *ngFor="let address of addressSuggestions; let i=index"
                             class="white-background-black-text"
                             [class.highlighted-selection]="i === selectedIndex"
                             (click)="splitAddress(i)"
                             (mouseenter)="selectedIndex = i"

            >
              {{ address.formatted }}
            </ion-item-option>
          </ion-list>
        </ion-col>
        <ion-col size="1">
          <ion-item>

            <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.street_number">
              <div slot="label">No.:
                <ion-text *ngIf="!ValidateData.controls.street_number.valid" color="danger">Missing</ion-text>
              </div>
            </ion-input>
          </ion-item>
        </ion-col>


      </ion-row>


      <ion-row>
        <ion-col size="1.2">
          <ion-item>

            <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.zip_code">
              <div slot="label">zip.:
                <ion-text *ngIf="!ValidateData.controls.zip_code.valid" color="danger">Missing</ion-text>
              </div>
            </ion-input>
          </ion-item>
        </ion-col>

        <ion-col size="2.1">
          <ion-item>

            <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.city">
              <div slot="label">City:
                <ion-text *ngIf="!ValidateData.controls.city.valid" color="danger">Missing</ion-text>
              </div>
            </ion-input>
          </ion-item>
        </ion-col>


      </ion-row>


      <ion-row>
        <ion-col size="3.3">
          <ion-item>

            <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.email">
              <div slot="label">Email
                <ion-text *ngIf="!ValidateData.controls.email.valid" color="danger">(Valid email address)</ion-text>
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
                <ion-text *ngIf="!ValidateData.controls.password.valid" color="danger">(min. 8 cha. min 1 special)
                </ion-text>
              </div>
            </ion-input>
            <ion-icon [name]="showPassword ? 'eye-off' : 'eye'" slot="end"
                      (click)="togglePasswordVisibility()"></ion-icon>
          </ion-item>
        </ion-col>
      </ion-row>

    </ion-grid>

    <ion-button style=".grey {
                  --ion-color-base: grey !important;
                    --ion-color-base-rgb: 128,128,128 !important;
}

" [disabled]="!ValidateData.valid"
                [class.grey]="!ValidateData.valid"

                size="small"
                (click)="selectAddress()"
                (keydown.enter)="selectAddress()">Save
    </ion-button>


    <ion-button

      size="small" (click)="UnselectAddress()"
      (keydown.enter)="UnselectAddress()"
    >Clear
    </ion-button>



  `,
  styleUrls: ['./newUser.scss'],
})
export class NewUserComponent {


  constructor(public dataservice: DataService,
              public formbuilder: FormBuilder
  ) {
  }

  addressSuggestions: Address[] = [];
  addressParts: string[]=[];
  showPassword: boolean = false;
  showSuggestions = false;
  selectedIndex = -1;


ValidateData=this.formbuilder.group({

  name:  ["",[Validators.required, Validators.minLength(6)]],
  street: ["", [Validators.required, Validators.minLength(3)]],
  street_number: ["", [Validators.required, Validators.minLength(1)]],
  zip_code: ["",[Validators.required, Validators.min(1000),Validators.max(9999)]],
  city: ["",[Validators.required, Validators.minLength(3)]],
  email: ["", [Validators.required, Validators.email, Validators.minLength(6)]],
  password: ["", [Validators.required, Validators.minLength(8), Validators.pattern(".*\\W.*")]],
})




  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }


  async updateSuggestions()  {

    // @ts-ignore
    if (this.ValidateData.controls.street.value.length > 3)
    {
      this.showSuggestions = true;

      // @ts-ignore
      this.dataservice.sendAddressLine(this.ValidateData.controls.street.value);
      this.addressSuggestions = this.dataservice.addressSuggestions;

    }
    else
    {
      this.addressSuggestions=[];
    }

  }


  splitAddress(i:number)
  {



    this.ValidateData.controls.street.setValue(this.addressSuggestions[i].street);

    this.ValidateData.controls.street_number.setValue(this.addressSuggestions[i].housenumber);


    this.ValidateData.controls.zip_code.setValue(this.addressSuggestions[i].postcode);
    this.ValidateData.controls.city.setValue(this.addressSuggestions[i].city);


  }


  UnselectAddress() {
    this.ValidateData.controls.street.setValue("");
    this.ValidateData.controls.street_number.setValue("");
    this.ValidateData.controls.zip_code.setValue("");
    this.ValidateData.controls.city.setValue("");
    this.ValidateData.controls.name.setValue("");
    this.ValidateData.controls.email.setValue("");
    this.ValidateData.controls.password.setValue("");

    this.addressSuggestions=[];

  }

  hideSuggestions() {
     setTimeout(() => this.showSuggestions = false, 200);
  }

  handleKeyDown(event: KeyboardEvent) {
    if (event.key === 'ArrowDown') {

      event.preventDefault();
      if (this.selectedIndex < this.addressSuggestions.length - 1) {
        this.selectedIndex++;
      }
    } else if (event.key === 'ArrowUp') {

      event.preventDefault();
      if (this.selectedIndex > 0) {
        this.selectedIndex--;
      }
    } else if (event.key === 'Enter') {
      event.preventDefault();
      if (this.selectedIndex >= 0) {
        this.splitAddress(this.selectedIndex);
        this.addressSuggestions=[];


      }
    }
  }

  selectAddress() {

  let userModel:UserModel = {

    name:this.ValidateData.controls.name.value+"",
    email: this.ValidateData.controls.email.value+"",
    password: this.ValidateData.controls.password.value+"",
    address: this.ValidateData.controls.street.value+ " "+this.ValidateData.controls.street_number.value,
    zip_code: Number(this.ValidateData.controls.zip_code.value),
    cvr: 0

    }

    this.dataservice.saveUser(userModel);

  }

}



