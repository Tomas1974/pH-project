import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { InboxPageRoutingModule } from './Graf-routing.module';

import { GrafPage } from './Graf.page';
import {NgxChartsModule} from "@swimlane/ngx-charts";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    InboxPageRoutingModule,
    NgxChartsModule
  ],
  declarations: [GrafPage]
})
export class InboxPageModule {}
