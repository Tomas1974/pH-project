import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { SpamPageRoutingModule } from './log-routing.module';
import { LogPage } from './log.page';
import {WebSocketService} from "./LogService";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SpamPageRoutingModule,
  ],
  declarations: [LogPage],

  providers: [WebSocketService]
})
export class SpamPageModule {}
