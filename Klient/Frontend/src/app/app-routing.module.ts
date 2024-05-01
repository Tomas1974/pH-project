import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },

  {
    path: 'home',
    loadChildren: () => import('./pages/home/home.module').then(m => m.HomePageModule)
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'Graf',
    loadChildren: () => import('./pages/Graf/Graf').then(m => m.InboxPageModule)
  },
  {
    path: 'Settings',
    loadChildren: () => import('./pages/Settings/settings.module').then(m => m.OutboxPageModule)
  },
  {
    path: 'Log',
    loadChildren: () => import('./pages/Log/log.module').then(m => m.SpamPageModule)
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
