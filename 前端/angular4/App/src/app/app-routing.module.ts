import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LaddaModule } from 'angular2-ladda';

const routes: Routes = [
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{ useHash: true }),
    LaddaModule.forRoot({
      style: "contract",
      spinnerSize: 40,
      spinnerColor: "blue",
      spinnerLines: 12
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
