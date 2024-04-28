import {Component, ElementRef, NgZone, ViewChild} from '@angular/core';
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {Address, AddressAPIJsonResponseModel } from "./models";
import {DataService} from "../../Services/Data.service";



@Component({
  selector: 'app-login',
  template: `



    <ion-grid>
        <ion-header>Bruger information</ion-header>
      <ion-row>
        <ion-col size="3.3">
          <ion-item>

            <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.navn">
              <div slot="label">Fornavn og efternavn
                <ion-text *ngIf="!ValidateData.controls.navn.valid" color="danger">Min 6 karakter</ion-text>
              </div>
            </ion-input>
          </ion-item>
        </ion-col>
      </ion-row>


      <ion-row>
        <ion-col size="2.3">

          <ion-item>

            <ion-input
              [formControl]="ValidateData.controls.vejField"
              (ionInput)="updateSuggestions()"
              (ionBlur)="hideSuggestions()"
              (keydown)="handleKeyDown($event)"
              labelPlacement="stacked"
            >
              <div slot="label">Vej
                <ion-text *ngIf="!ValidateData.controls.vejField.valid" color="danger">Min 3 karakter</ion-text>
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

              <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.husnrField">
                <div slot="label">Nr.:
                  <ion-text *ngIf="!ValidateData.controls.husnrField.valid" color="danger">Mangler</ion-text>
                </div>
              </ion-input>
            </ion-item>
          </ion-col>






      </ion-row>


      <ion-row>
        <ion-col size="1.2">
          <ion-item>

            <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.postnrField">
              <div slot="label">Postnr.:
                <ion-text *ngIf="!ValidateData.controls.postnrField.valid" color="danger">mangler</ion-text>
              </div>
            </ion-input>
          </ion-item>
        </ion-col>

        <ion-col size="2.1">
          <ion-item>

            <ion-input labelPlacement="stacked" [formControl]="ValidateData.controls.byField">
              <div slot="label">By:
                <ion-text *ngIf="!ValidateData.controls.byField.valid" color="danger">Mangler</ion-text>
              </div>
            </ion-input>
          </ion-item>
        </ion-col>



      </ion-row>




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

    </ion-grid>

    <ion-button style=".grey {
                  --ion-color-base: grey !important;
                    --ion-color-base-rgb: 128,128,128 !important;
}

" [disabled]="!ValidateData.valid"
                [class.grey]="!ValidateData.valid"

                size="small"
                (click)="selectAddress()"
                (keydown.enter)="selectAddress()">Gem
    </ion-button>


    <ion-button

      size="small" (click)="UnselectAddress()"
      (keydown.enter)="UnselectAddress()"
    >Tøm
    </ion-button>



  `,
  styleUrls: ['./app-login.scss'],
})
export class LoginComponent {


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

  navn:  ["",[Validators.required, Validators.minLength(6)]],
  vejField: ["", [Validators.required, Validators.minLength(3)]],
  husnrField: ["", [Validators.required, Validators.minLength(1)]],
  postnrField: ["",[Validators.required, Validators.min(1000),Validators.max(9999)]],
  byField: ["",[Validators.required, Validators.minLength(3)]],
  brugerNavn: ["",[Validators.required, Validators.minLength(6)]],
  kodeord: ["", [Validators.required, Validators.minLength(8), Validators.pattern(".*\\W.*")]],
})




  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }


  async updateSuggestions()  {

    // @ts-ignore
    if (this.ValidateData.controls.vejField.value.length > 3)
    {
      this.showSuggestions = true;

      // @ts-ignore
      this.dataservice.sendAddressLine(this.ValidateData.controls.vejField.value);
      this.addressSuggestions = this.dataservice.addressSuggestions;

    }
    else
    {
      this.addressSuggestions=[];
    }

  }


  splitAddress(i:number)
  {



    this.ValidateData.controls.vejField.setValue(this.addressSuggestions[i].street);

    this.ValidateData.controls.husnrField.setValue(this.addressSuggestions[i].housenumber);


    this.ValidateData.controls.postnrField.setValue(this.addressSuggestions[i].postcode);
    this.ValidateData.controls.byField.setValue(this.addressSuggestions[i].city);


  }


  UnselectAddress() {
    this.ValidateData.controls.vejField.setValue("");
    this.ValidateData.controls.husnrField.setValue("");
    this.ValidateData.controls.postnrField.setValue("");
    this.ValidateData.controls.byField.setValue("");
    this.ValidateData.controls.navn.setValue("");
    this.ValidateData.controls.brugerNavn.setValue("");
    this.ValidateData.controls.kodeord.setValue("");

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


  }

}



