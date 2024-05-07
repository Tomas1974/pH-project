import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SettingsPage } from './settings.page';
import {ClientComponent} from "./client-oversigt/client.component";

const routes: Routes = [
  {
    path: '',
    component: SettingsPage
  },
  { path: 'client-overview', component: ClientComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OutboxPageRoutingModule {}
