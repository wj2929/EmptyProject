import { CategoryService } from './services/category.service';
import { CustomformService } from './services/customform.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AccountLoginComponent } from './account/account-login/account-login.component';
import { HttpClientModule } from '@angular/common/http';

import { NgHChartsModule } from 'ng-hcharts';
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';
import { NgbModalModule } from "./common/modal/modal.module";

import { PaginationComponent } from './common/pagination/pagination.component';
import { CommonService } from './services/common.service';
import { PassportService } from './services/passport.service';
import { ManageComponent } from './manage/manage.component';
import { ManageBasicdataComponent } from './manage/manage-basicdata/manage-basicdata.component';
import { ManageBasicdataCustomformComponent } from './manage/manage-basicdata/manage-basicdata-customform/manage-basicdata-customform.component';
import { ManageBasicdataCategoryComponent } from './manage/manage-basicdata/manage-basicdata-category/manage-basicdata-category.component';
import { SelhttpService } from './services/selhttp.service';

import { ManageBasicdataCustomformCreateComponent } from './manage/manage-basicdata/manage-basicdata-customform/manage-basicdata-customform-create/manage-basicdata-customform-create.component';
import { ManageBasicdataCustomformEditComponent } from './manage/manage-basicdata/manage-basicdata-customform/manage-basicdata-customform-edit/manage-basicdata-customform-edit.component';
import { ManageBasicdataCustomformitemCreateComponent } from './manage/manage-basicdata/manage-basicdata-customform/manage-basicdata-customformitem-create/manage-basicdata-customformitem-create.component';
import { ManageBasicdataCustomformitemEditComponent } from './manage/manage-basicdata/manage-basicdata-customform/manage-basicdata-customformitem-edit/manage-basicdata-customformitem-edit.component';
import { ManageBasicdataCategorytypeCreateComponent } from './manage/manage-basicdata/manage-basicdata-category/manage-basicdata-categorytype-create/manage-basicdata-categorytype-create.component';
import { ManageBasicdataCategorytypeEditComponent } from './manage/manage-basicdata/manage-basicdata-category/manage-basicdata-categorytype-edit/manage-basicdata-categorytype-edit.component';
import { ManageBasicdataCategoryCreateComponent } from './manage/manage-basicdata/manage-basicdata-category/manage-basicdata-category-create/manage-basicdata-category-create.component';
import { ManageBasicdataCategoryEditComponent } from './manage/manage-basicdata/manage-basicdata-category/manage-basicdata-category-edit/manage-basicdata-category-edit.component';
import { CustomFormSettingComponent } from './common/custom-form-setting/custom-form-setting.component';

import { ManageLogComponent } from './manage/manage-log/manage-log.component';

import { ENgxFileUploadModule } from "e-ngx-fileupload";
import { SortnessDirective } from './directives/sortness.directive';

import { DragDropModule } from '@angular/cdk/drag-drop';

import { AccountEditPasswordComponent } from './account/account-edit-password/account-edit-password.component';
// import { SortableModule } from "angular-sortable/angular-sortable"
import { ManageUserComponent } from './manage/manage-user/manage-user.component';
import { ManageUserCreateComponent } from './manage/manage-user/manage-user-create/manage-user-create.component';
import { ManageUserEditComponent } from './manage/manage-user/manage-user-edit/manage-user-edit.component';
import { ManageUserEditPasswordComponent } from './manage/manage-user/manage-user-edit-password/manage-user-edit-password.component';
import { ManageDashboardComponent } from './manage/manage-dashboard/manage-dashboard.component';
import { ManageDashboardLoginLogComponent } from './manage/manage-dashboard/manage-dashboard-login-log/manage-dashboard-login-log.component';

// import {UniqueCustomFormKeyDirective} from './validators/unique-custom-form-key-validator';
// import {UniqueCustomFormItemNameDirective} from './validators/unique-custom-form-item-name-validator'
// import {UniqueCustomFormItemKeyDirective} from './validators/unique-custom-form-item-key-validator'

import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { ManageBasicdataCategoryImportComponent } from './manage/manage-basicdata/manage-basicdata-category/manage-basicdata-category-import/manage-basicdata-category-import.component';
import { ManageDatainfoCreateComponent } from './manage/manage-datainfo/manage-datainfo-create/manage-datainfo-create.component';
import { ManageDatainfoEditComponent } from './manage/manage-datainfo/manage-datainfo-edit/manage-datainfo-edit.component';
import { ManageDatainfoTestComponent } from './manage/manage-datainfo/manage-datainfo-test/manage-datainfo-test.component';
import { ManageDatainfoComponent } from './manage/manage-datainfo/manage-datainfo.component';

import { ShareModule } from './share.module';
import { OrderByPipe } from './pipes/order-by.pipe';
import { DataanalysisComponent } from './dataanalysis/dataanalysis.component';
import { DataanalysisSignupComponent } from './dataanalysis/dataanalysis-signup/dataanalysis-signup.component';

// import { ManageWeixinCustommenuComponent } from './manage/manage-weixin/manage-weixin-custommenu/manage-weixin-custommenu.component';
// import { ManageWeixinAutoreplyComponent } from './manage/manage-weixin/manage-weixin-autoreply/manage-weixin-autoreply.component';
// import { ManageWeixinAutoreplySubscribeComponent } from './manage/manage-weixin/manage-weixin-autoreply/manage-weixin-autoreply-subscribe/manage-weixin-autoreply-subscribe.component';
// import { ManageWeixinAutoreplyMsgComponent } from './manage/manage-weixin/manage-weixin-autoreply/manage-weixin-autoreply-msg/manage-weixin-autoreply-msg.component';

import { NgxSummernoteModule } from 'ngx-summernote';
@NgModule({
  declarations: [
    AppComponent,
    AccountLoginComponent,
    // PaginationComponent,
    ManageComponent,
    CustomFormSettingComponent,
    ManageBasicdataComponent,
    ManageBasicdataCustomformComponent,
    ManageBasicdataCategoryComponent,
    ManageBasicdataCustomformCreateComponent,
    ManageBasicdataCustomformEditComponent,
    ManageBasicdataCustomformitemCreateComponent,
    ManageBasicdataCustomformitemEditComponent,
    ManageBasicdataCategorytypeCreateComponent,
    ManageBasicdataCategorytypeEditComponent,
    ManageBasicdataCategoryCreateComponent,
    ManageBasicdataCategoryEditComponent,
    ManageLogComponent,
    SortnessDirective,
    AccountEditPasswordComponent,
    ManageUserComponent,
    ManageUserCreateComponent,
    ManageUserEditComponent,
    ManageUserEditPasswordComponent,
    ManageDashboardComponent,
    ManageDashboardLoginLogComponent,

    ManageBasicdataCategoryImportComponent,
    
    ManageDatainfoCreateComponent,
    ManageDatainfoEditComponent,
    ManageDatainfoTestComponent,
    ManageDatainfoComponent,

    OrderByPipe,
    DataanalysisSignupComponent,
    DataanalysisComponent
    // ManageWeixinCustommenuComponent,
    // ManageWeixinAutoreplyComponent,
    // ManageWeixinAutoreplySubscribeComponent,
    // ManageWeixinAutoreplyMsgComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgHChartsModule,
    ENgxFileUploadModule,
    DragDropModule,
    ShareModule,
    NgxDaterangepickerMd.forRoot({
      format: "YYYY-MM-DD",
        separator: " - ",
        daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
        monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
        applyLabel: '确定',
        cancelLabel: '取消'
    }),
    NgbModalModule.forRoot(),
    CKEditorModule,
    NgxSummernoteModule
  ],
  entryComponents: [
    ManageBasicdataCustomformCreateComponent,
    ManageBasicdataCustomformEditComponent,
    ManageBasicdataCustomformitemCreateComponent,
    ManageBasicdataCustomformitemEditComponent,
    ManageBasicdataCategorytypeCreateComponent,
    ManageBasicdataCategorytypeEditComponent,
    ManageBasicdataCategoryCreateComponent,
    ManageBasicdataCategoryEditComponent,
    AccountEditPasswordComponent,
    ManageUserCreateComponent,
    ManageUserEditComponent,
    ManageUserEditPasswordComponent,
    ManageBasicdataCategoryImportComponent,
  
    ManageDatainfoCreateComponent,
    ManageDatainfoEditComponent],
  providers: [
    CommonService,
    PassportService,
    CustomformService,
    CategoryService,
    SelhttpService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
