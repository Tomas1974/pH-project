import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {LegendPosition, LineChartModule} from "@swimlane/ngx-charts";

import { GrafPage } from './Graf.page';
import {GrafComponent} from "./graf/graf.component";
import {IonicModule} from "@ionic/angular";
import {FormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";


const routes: Routes = [
  {
    path: '',
    component: GrafPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes), LineChartModule, IonicModule, FormsModule,CommonModule],
    exports: [RouterModule, GrafComponent ],
    declarations: [
        GrafComponent
    ]
})
export class InboxPageRoutingModule {}
