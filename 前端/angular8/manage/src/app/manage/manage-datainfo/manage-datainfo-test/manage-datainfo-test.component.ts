import { TestdataService } from './../../../services/testdata.service';
import { CategoryService } from './../../../services/category.service';
import { CommonService } from './../../../services/common.service';
import { CustomformService } from 'src/app/services/customform.service';
import { PassportService } from './../../../services/passport.service';
import { DataInfoService } from 'src/app/services/datainfo.service';
import { Component, OnInit } from '@angular/core';
import { ManageBaseDataInfo } from '../manage-basedatainfo';
import { NgbModal } from 'src/app/common/modal/modal';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { ReturnPagingModel } from 'src/app/models/ReturnInfoModel';
import { DisplayDataInfoModel } from 'src/app/models/DataInfoModels';
import { PaginationModel } from 'src/app/common/pagination/PaginationModels';
import { SweetError } from 'src/app/common/common';

@Component({
  selector: 'app-manage-datainfo-test',
  templateUrl: './manage-datainfo-test.component.html',
  styleUrls: ['./manage-datainfo-test.component.less']
})
export class ManageDatainfoTestComponent extends ManageBaseDataInfo implements OnInit {

  constructor(
    public TestdataService:TestdataService,
    public DataInfoService: DataInfoService,
    public CategoryService: CategoryService,
    public modalService: NgbModal,
    public routeInfo: ActivatedRoute,
    public PassportService: PassportService,
    public CustomformService: CustomformService,
    public fb: FormBuilder,
    public CommonService: CommonService) {
    super(DataInfoService, CategoryService, modalService, routeInfo, PassportService, CustomformService, fb);

    this.customFormKeycode = "test";
    this.DataInfoName = "test数据";
  }

  loadDataInfoPaging(PageNum: number, PageSize: number) {
    this.TestdataService.pagingDataInfo(
      this.formModel.value,
      PageNum,
      PageSize,
      (data: ReturnPagingModel<DisplayDataInfoModel>) => {
        if (data.State) {
          this.DataInfoPaging = data.DataObject;
          this.DataInfoPaginationModel = new PaginationModel(
            data.DataObject.Module_Page.AllCount,
            data.DataObject.Module_Page.PageNum,
            data.DataObject.Module_Page.PageCount);

          this.DataInfoPaging.PageListInfos.forEach(t => {
            var arr = [];
            t.CustomFormSetting.CustomFormItemSettings.forEach(item => {
              arr.push(item.Name + ":" + item.Value);
            });
            t.CustomFormSettingDescription = arr.join("\n");
          });
        }
        else
          SweetError("", data.Message);
      });
  }
}