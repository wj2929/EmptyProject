import { DataanalysisSignupComponent } from './dataanalysis/dataanalysis-signup/dataanalysis-signup.component';
import { DataanalysisComponent } from './dataanalysis/dataanalysis.component';
import { ManageDashboardComponent } from './manage/manage-dashboard/manage-dashboard.component';

import { ManageLogComponent } from './manage/manage-log/manage-log.component';
import { ManageBasicdataCategoryComponent } from './manage/manage-basicdata/manage-basicdata-category/manage-basicdata-category.component';
import { ManageBasicdataCustomformComponent } from './manage/manage-basicdata/manage-basicdata-customform/manage-basicdata-customform.component';
import { ManageBasicdataComponent } from './manage/manage-basicdata/manage-basicdata.component';
import { ManageComponent } from './manage/manage.component';
import { AccountLoginComponent } from './account/account-login/account-login.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PassportService } from './services/passport.service';
import { ManageUserComponent } from './manage/manage-user/manage-user.component';


const routes: Routes = [
  { path:"",redirectTo:"login",pathMatch:"full"},
  { path:"login",component:AccountLoginComponent},
  {
    path: "dataanalysis", component: DataanalysisComponent, children: [
      { path: "", redirectTo: "signup", pathMatch: "full" },
      { path:"signup",component:DataanalysisSignupComponent},
    ]},
  { path:"manage",component:ManageComponent, canActivate: [PassportService],children:[
    { path: "",redirectTo:"dashboard",pathMatch:"full"},
    { path:"basicdata",component:ManageBasicdataComponent,canActivate: [PassportService],children:[
      { path:"",redirectTo:"customform",pathMatch:"full"},
      { path:"customform/:id",component:ManageBasicdataCustomformComponent},
      { path:"category/:id/:customFormId",component:ManageBasicdataCategoryComponent},
      { path:"childcategory/:id/:parentCategoryId",component:ManageBasicdataCategoryComponent}
    ]},
    {path:"user",component:ManageUserComponent,canActivate: [PassportService],children:[

    ]},
    {path:"log",component:ManageLogComponent,canActivate: [PassportService]},
    {path:"dashboard",component:ManageDashboardComponent,canActivate: [PassportService]},
    // {path:"weixin",component:ManageWeixinComponent,canActivate: [PassportService],children:[
    //   { path:"",redirectTo:"autoreply",pathMatch:"full"},
    //   { path:"autoreply",component:ManageWeixinAutoreplyComponent,children:[
    //     { path:"",redirectTo:"subscribe",pathMatch:"full"},
    //     { path:"subscribe",component:ManageWeixinAutoreplySubscribeComponent},
    //     { path:"msg",component:ManageWeixinAutoreplyMsgComponent}
    //   ]},
    //   { path:"custommenu",component:ManageWeixinCustommenuComponent},
    //   { path:"childcustommenu/:id",component:ManageWeixinCustommenuComponent}
    // ]},    
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{ useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
