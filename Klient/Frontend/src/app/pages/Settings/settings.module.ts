import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { OutboxPageRoutingModule } from './settings-routing.module';

import { SettingsPage } from './settings.page';
import {ClientComponent} from "./client-oversigt/client.component";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    OutboxPageRoutingModule,
    ReactiveFormsModule
  ],
  declarations: [SettingsPage, ClientComponent]
})
export class OutboxPageModule {}
