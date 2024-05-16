import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {AuthGuard} from "./AuthGuard";

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },

  {
    path: 'home',
    loadChildren: () => import('./pages/home/home.module').then(m => m.HomePageModule),
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'Graf',
    loadChildren: () => import('./pages/Graf/Graf').then(m => m.InboxPageModule), canActivate: [AuthGuard]
  },
  {
    path: 'Settings',
    loadChildren: () => import('./pages/Settings/settings.module').then(m => m.OutboxPageModule), canActivate: [AuthGuard]
  },
  {
    path: 'Log',
    loadChildren: () => import('./pages/Log/log').then(m => m.SpamPageModule), canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  providers: [AuthGuard],
  exports: [RouterModule]
})
export class AppRoutingModule { }
