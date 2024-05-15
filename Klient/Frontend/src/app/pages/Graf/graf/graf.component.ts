import { Component } from '@angular/core';
import {DataService} from "../../../Services/Data.service";
import {LegendPosition} from "@swimlane/ngx-charts";


@Component({
  selector: 'app-graf',
  template: `






    <ion-grid>
      <ion-row>
        <ion-col></ion-col>

        <ion-col size="8">
          <ion-row>
            <ion-card>


              <ion-row>

                <ion-list>

                  <ion-item>

                    <ion-select placeholder="Clients" [(ngModel)]="this.dataService.selectedClient"
                                (ionChange)="getClientList()">
                      <ion-text>Name</ion-text>
                      <ion-select-option *ngFor="let text of this.dataService.clientsNames"
                                         [value]="text">{{ text }}
                      </ion-select-option>
                    </ion-select>


                  </ion-item>

                </ion-list>

              </ion-row>


              <ngx-charts-line-chart
                [view]=[900,400]
                [scheme]="colorScheme"
                [legend]="showLegend"
                [showXAxisLabel]="showXAxisLabel"
                [showYAxisLabel]="showYAxisLabel"
                [animations]="true"
                [xAxis]="xAxis"
                [yAxis]="yAxis"
                [xAxisLabel]="xAxisLabel"
                [yAxisLabel]="yAxisLabel"
                [timeline]="timeline"
                [results]="this.dataService.pHData"
                [autoScale]="true"
                [yScaleMin]="this.dataService.minGrafValue"
                [yScaleMax]="this.dataService.maxGrafValue"
                (select)="onSelect($event)"
                (activate)="onActivate($event)"
                (deactivate)="onDeactivate($event)"

              >
              </ngx-charts-line-chart>


            </ion-card>
          </ion-row>
          <ion-row>
            <ion-col>
              <ion-row>
                <p>Maximal value :{{ dataService.maxValue }}</p>
              </ion-row>
              <ion-row>
                <p>Minimal value :{{ dataService.minValue }}</p>
              </ion-row>
            </ion-col>

            <ion-col>
              <ion-row>
                <p>Maximal limit :{{ dataService.client?.max_value }}</p>
              </ion-row>
              <ion-row>
                <p>Minimal limit :{{ dataService.client?.min_value }}</p>
              </ion-row>
            </ion-col>

          </ion-row>


        </ion-col>
        <ion-col></ion-col>

      </ion-row>
    </ion-grid>



  `,
  styleUrls: ['./graf.component.scss'],
})
export class GrafComponent  {



  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'Sek';
  yAxisLabel = 'pH';
  showYAxisLabel = true;
  xAxis: boolean = true;
  yAxis: boolean = true;
  timeline: boolean = true;



  colorScheme: any  = {
    domain: ['#A10A28', '#C7B42C', '#5AA454','#AAAAAA']
  };

  legendPosition=LegendPosition.Below; //placerer landene under grafen




  constructor(public dataService: DataService,

  )  {

  }


  onSelect(event:any) {
    console.log(event);
  }


  onActivate(event:any): void {
    console.log(event)
  }

  onDeactivate(event:any): void {
    console.log(event)
  }







  async getClientList() {

    this.dataService.pHData=[]; //Her nulstilles gamle grafer
    this.dataService.getGraf(); //Her bestilles en ny graf




  }

}
