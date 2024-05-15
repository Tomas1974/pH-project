import { Component, OnInit } from '@angular/core';
import {DataService} from "../../Services/Data.service";
import { LegendPosition } from '@swimlane/ngx-charts';


@Component({
  selector: 'app-Graf',
  templateUrl: './Graf.page.html',
  styleUrls: ['./Graf.scss'],
})
export class GrafPage  {


  selectedClient: string="";


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




  async startGraf() {





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




  }
}
