import {Component} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {DataService} from "../../../Services/Data.service";
import {ClientModel} from "../../../Models/clientModel";


@Component({
  selector: 'client-overview',
  template: `

    <ion-row>
      <ion-col size="1.3">
        <ion-list *ngFor="let client of dataservice.clients">
          <ion-label>{{client.client_name}}</ion-label>
        </ion-list>
      </ion-col>
    </ion-row>

    <ion-row>
      <ion-col size="1.3" offset="1.6">
        <ion-item>
          <ion-input labelPlacement="stacked" [formControl]="ValidateClient.controls.client_id">
            <div slot="label">Product id from manufacture
              <ion-text *ngIf="!ValidateClient.controls.client_id.valid"></ion-text>
            </div>
          </ion-input>
        </ion-item>
      </ion-col>
    </ion-row>


    <ion-row>
      <ion-col size="1.3" offset="1.6">
        <ion-item>
          <ion-input labelPlacement="stacked" [formControl]="ValidateClient.controls.client_name">
            <div slot="label">Device name
              <ion-text *ngIf="!ValidateClient.controls.client_name.valid"></ion-text>
            </div>
          </ion-input>
        </ion-item>
      </ion-col>
    </ion-row>

    <ion-row>
      <ion-col size="1.3" offset="1.6">
        <ion-item>
          <ion-input labelPlacement="stacked" [formControl]="ValidateClient.controls.max_value">
            <div slot="label">Maximum value for alarm
              <ion-text *ngIf="!ValidateClient.controls.max_value.valid"></ion-text>
            </div>
          </ion-input>
        </ion-item>
      </ion-col>
    </ion-row>

    <ion-row>
      <ion-col size="1.3" offset="1.6">
        <ion-item>
          <ion-input labelPlacement="stacked" [formControl]="ValidateClient.controls.min_value">
            <div slot="label">Minimum value for alarm
              <ion-text *ngIf="!ValidateClient.controls.min_value.valid"></ion-text>
            </div>
          </ion-input>
        </ion-item>
        <ion-row>
          <ion-col>
            <ion-button style=".grey {
                      --ion-color-base: grey !important;
                        --ion-color-base-rgb: 128,128,128 !important;
        }" [disabled]="!ValidateClient.valid"
                        [class.grey]="!ValidateClient.valid"
                        size="small"
                        (click)="SaveClient()"
                        (keydown.enter)="SaveClient()">Register device
            </ion-button>
          </ion-col>
          <ion-col>
            <ion-button size="small" (click)="ResetClient()" (keydown.enter)="ResetClient()">Clear</ion-button>
          </ion-col>
          <ion-col>
            <ion-button size="small" (click)="deleteClient()">Delete</ion-button>
          </ion-col>
        </ion-row>
      </ion-col>
    </ion-row>
  `,
  styleUrls: ['./client.componen.scss'],
})
export class ClientComponent {

  ValidateClient = this.formbuilder.group({

    client_id: ["client/", [Validators.required, Validators.minLength(16), Validators.maxLength(16)]],
    client_name: ["", [Validators.required, Validators.minLength(1)]],
    max_value: [0, [Validators.required, Validators.min(1), Validators.max(15)]],
    min_value: [0, [Validators.required, Validators.min(1), Validators.max(15)]],
  })

  constructor(public dataservice: DataService,
              public formbuilder: FormBuilder) {
  }

  async SaveClient() {
    let clientModel: ClientModel = {

      client_id: this.ValidateClient.controls.client_id.value + "",
      client_name: this.ValidateClient.controls.client_name.value + "",
      max_value: Number(this.ValidateClient.controls.max_value.value),
      min_value: Number(this.ValidateClient.controls.min_value.value),
    };
    let email = this.dataservice.loginUser!

    this.dataservice.saveClient(clientModel, email)
    await this.dataservice.timePromise();
    if (!this.dataservice.duplicatedClient) {
      this.dataservice.clients.push(clientModel);
      this.ResetClient();
    }
  }

  deleteClient(){
    let clientModel: ClientModel = {
      client_id: "",
      client_name: undefined,
      max_value: undefined,
      min_value: undefined
    };
    let email = this.dataservice.loginUser!
    
    this.dataservice.saveClient(clientModel, email)
  }

  ResetClient() {
    this.ValidateClient.controls.client_id.setValue("");
    this.ValidateClient.controls.client_name.setValue("");
    this.ValidateClient.controls.max_value.setValue(0);
    this.ValidateClient.controls.min_value.setValue(0);


  }
}
