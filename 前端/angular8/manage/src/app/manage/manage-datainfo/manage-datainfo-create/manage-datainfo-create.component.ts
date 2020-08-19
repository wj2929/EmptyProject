import { DataInfoService } from './../../../services/datainfo.service';
import { CategoryService } from './../../../services/category.service';
import { CommonService } from './../../../services/common.service';
import { CustomformService } from './../../../services/customform.service';
import { Component, OnInit } from '@angular/core';
import { BaseCreateCustomForm } from 'src/app/common/util/BaseCreateCustomForm';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { FormBuilder, Validators } from '@angular/forms';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
@Component({
  selector: 'app-manage-datainfo-create',
  templateUrl: './manage-datainfo-create.component.html',
  styleUrls: ['./manage-datainfo-create.component.css']
})
export class ManageDatainfoCreateComponent extends BaseCreateCustomForm implements OnInit {
  customFormKeycode = "";
  customFormId = "";
  parentDataInfo_Id = "";
  customFormList = [];
  customFormItemList = [];
  DataInfoName="数据";

  constructor(public activeModal: NgbActiveModal,
    public fb: FormBuilder,
    public CustomformService: CustomformService,
    public CommonService: CommonService,
    public CategoryService: CategoryService,
    public DataInfoService :DataInfoService) {
    super(CustomformService, CommonService, CategoryService, fb);

  }

  Close() {
    this.activeModal.close();
  }

  SetForm(DataInfoName,customFormKeycode, customFormId, parentDataInfo_Id) {
    this.DataInfoName = DataInfoName;
    this.customFormKeycode = customFormKeycode;
    this.customFormId = customFormId;
    this.parentDataInfo_Id = parentDataInfo_Id;

    this.formModel = this.fb.group({
      CustomFormKeycode: [this.customFormKeycode],
      Name: ['', Validators.required],
      ParentDataInfo_Id: [this.parentDataInfo_Id],
      // OrderBy: ['0'],
      CustomFormSetting: this.fb.group({
        CustomFormItemSettings: this.fb.array([])
      })
    });

    this.listCustomFormItemById(this.customFormId);
  }

  OnSubmit() {
    if (this.formModel.valid) {
      this.saveFormModelCustomFormSetting();

      this.DataInfoService.createDataInfo(this.formModel.value, (data: ReturnInfoModel) => {
        this.activeModal.close(data);
      });
      // this.CategoryService.createCategory(this.formModel.value, (data: ReturnInfoModel) => {
      //   this.activeModal.close(data);
      // });
    }
    else {
      this.validateForm();
    }
  }

  changeCustomForm($event) {
    this.listCustomFormItemById($event);
  }

  ngOnInit() {
  }
}
