import {Component} from "@angular/core";
import {AbstractControl, FormBuilder, ValidationErrors, ValidatorFn, Validators} from "@angular/forms";
import {DataService} from "../../../Services/Data.service";
import {ClientModel} from "../../../Models/clientModel";
import {UtilitiesService} from "../../../Services/utilities.service";


@Component({
  selector: 'client-overview',
  template: `
      <div style="display: flex; height: 100%">
        <!--  vensrtre -->
          <div style="width: 10%">
              <ion-row>
                  <ion-col>
                      <ion-list>
                          <ion-item *ngFor="let client of dataservice.clients; let i = index"
                                    [ngClass]="{'selected': client === selectedClient}"
                                    (click)="selectClient(client, i)">
                              <ion-label>{{client.client_name}}</ion-label>
                          </ion-item>
                      </ion-list>
                  </ion-col>
              </ion-row>
          </div>


          <!-- højre --->
          <div STYLE="width: 50%">
              <ion-row>
                  <ion-col>
                      <ion-item>
                          <ion-input labelPlacement="stacked" [formControl]="ValidateClient.controls.client_id"
                                     maxlength="9">
                              <div slot="label">Product id from manufacture
                                  <ion-text *ngIf="!ValidateClient.controls.client_id.valid "color="danger">( 9 cha.)</ion-text>
                              </div>
                          </ion-input>
                      </ion-item>
                  </ion-col>
              </ion-row>



              <ion-row>
                  <ion-col>
                      <ion-item>
                          <ion-input labelPlacement="stacked" [formControl]="ValidateClient.controls.client_name">
                              <div slot="label">Device name
                                  <ion-text *ngIf="!ValidateClient.controls.client_name.valid "color="danger">( 1 cha.) </ion-text>
                              </div>
                          </ion-input>
                      </ion-item>
                  </ion-col>
              </ion-row>

              <ion-row>
                  <ion-col>
                      <ion-item>
                          <ion-input labelPlacement="stacked" [formControl]="ValidateClient.controls.max_value">
                              <div slot="label">Max. value for alarm (higher than min)
                                  <ion-text *ngIf="!ValidateClient.controls.max_value.valid "color="danger">(Decimal number)</ion-text>
                              </div>
                          </ion-input>
                      </ion-item>
                  </ion-col>
              </ion-row>

              <ion-row>
                  <ion-col>
                      <ion-item>
                          <ion-input labelPlacement="stacked" [formControl]="ValidateClient.controls.min_value">
                              <div slot="label">Minimum value for alarm (lower than max)
                                  <ion-text *ngIf="!ValidateClient.controls.min_value.valid"color="danger">(Decimal number)</ion-text>
                              </div>
                          </ion-input>
                      </ion-item>
                  </ion-col>
              </ion-row>

            <ion-row>
              <ion-col>
                <p style="height: 7px;color: red">{{ dataservice.textToregardingClient }}</p>
              </ion-col>
            </ion-row>



                      <ion-row>

                              <ion-button style=".grey {
                      --ion-color-base: grey !important;
                        --ion-color-base-rgb: 128,128,128 !important;
        }" [disabled]="!ValidateClient.valid"
                                          [class.grey]="!ValidateClient.valid"
                                          size="small"
                                          (click)="SaveClient()"
                                          (keydown.enter)="SaveClient()">Register device
                              </ion-button>

                              <ion-button size="small" (click)="ResetClient()" (keydown.enter)="ResetClient()">Clear
                              </ion-button>

                              <ion-button size="small" (click)="deleteClient(selectedClient.client_id)">Delete
                              </ion-button>

        </ion-row>

          </div>
      </div>

  `,
  styleUrls: ['./client.componen.scss'],
})
export class ClientComponent {

  ValidateClient = this.formbuilder.group({

    client_id: ["", [Validators.required, Validators.minLength(9), Validators.maxLength(9)]],
    client_name: ["", [Validators.required, Validators.minLength(1)]],
    max_value: [0, [Validators.required, Validators.min(3), Validators.max(11)]],
    min_value: [0, [Validators.required, Validators.min(3), Validators.max(11)]],
  }, { validators: maxGreaterThanMinValidator('min_value', 'max_value') });



  selectedClient: ClientModel = this.dataservice.clients[0];
  selectedIndex: number = 0;
  constructor(public dataservice: DataService,
              public formbuilder: FormBuilder,
              public utilitiesService: UtilitiesService) {
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

      this.ResetClient();
    }
  }

  async deleteClient(client_id: string){

    let confirm=await this.utilitiesService.confirmDelete();
    if (confirm)
    {
      this.dataservice.deleteClient(client_id)
      this.dataservice.clients.splice(this.selectedIndex, 1);
    }

  }

  ResetClient() {
    this.ValidateClient.controls.client_id.setValue("");
    this.ValidateClient.controls.client_name.setValue("");
    this.ValidateClient.controls.max_value.setValue(0);
    this.ValidateClient.controls.min_value.setValue(0);
    this.dataservice.textToregardingClient="";
  }

  selectClient(client: ClientModel, index: number) {
    this.selectedClient = client;
    this.selectedIndex = index;
  }
}




export function maxGreaterThanMinValidator(minControlName: string, maxControlName: string): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const minControl = formGroup.get(minControlName);
    const maxControl = formGroup.get(maxControlName);

    if (!minControl || !maxControl) {
      return null;
    }

    const minValue = minControl.value;
    const maxValue = maxControl.value;

    if (minValue !== null && maxValue !== null && maxValue <= minValue) {
      return { maxGreaterThanMin: true };
    }

    return null;
  };
}
