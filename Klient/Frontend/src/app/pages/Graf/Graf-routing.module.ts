import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {LegendPosition} from "@swimlane/ngx-charts";

import { GrafPage } from './Graf.page';

const routes: Routes = [
  {
    path: '',
    component: GrafPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InboxPageRoutingModule {}
