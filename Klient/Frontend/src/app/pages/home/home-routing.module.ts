import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePage } from './home.page';
import {LoginComponent} from "./login/login.component";
import {NewUserComponent} from "./newUser/newuser";
import {LoginOverviewComponent} from "./login-overview/login-overview.component";

const routes: Routes = [
  {
    path: '',
    component: HomePage,
  },
  { path: 'home/app-login', component: NewUserComponent },
  { path: 'home/app-login2', component: LoginComponent },
  { path: 'app-login-overview', component: LoginOverviewComponent},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomePageRoutingModule {}
