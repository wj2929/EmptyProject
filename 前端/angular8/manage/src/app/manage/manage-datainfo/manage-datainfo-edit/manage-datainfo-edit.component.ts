import { DataInfoService } from './../../../services/datainfo.service';
import { CommonService } from './../../../services/common.service';
import { CustomformService } from 'src/app/services/customform.service';
import { CategoryService } from './../../../services/category.service';
import { Component, OnInit } from '@angular/core';
import { BaseEditCustomForm } from 'src/app/common/util/BaseEditCustomForm';
import { DisplayCategoryModel } from 'src/app/models/CategoryModels';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { FormBuilder, Validators } from '@angular/forms';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { DisplayDataInfoModel } from 'src/app/models/DataInfoModels';

@Component({
  selector: 'app-manage-datainfo-edit',
  templateUrl: './manage-datainfo-edit.component.html',
  styleUrls: ['./manage-datainfo-edit.component.css']
})
export class ManageDatainfoEditComponent extends BaseEditCustomForm implements OnInit {
  categoryTypeId = "";
  customFormId = "";
  parentCategory_Id = "";
  customFormList = [];
  customFormItemList = [];
  model: DisplayDataInfoModel;
  DataInfoName="数据";
  constructor(public activeModal: NgbActiveModal,
    public fb: FormBuilder,
    public CategoryService: CategoryService,
    public CustomformService: CustomformService,
    public CommonService: CommonService,
    public DataInfoService:DataInfoService) {
    super(CustomformService,CommonService,CategoryService,fb);
  }

  Close() {
    this.activeModal.close();
  }

  SetForm(DataInfoName,model: DisplayDataInfoModel, customFormId: string) {
    this.DataInfoName = DataInfoName;
    this.model = model;
    this.formModel = this.fb.group({
      EditId: [model.Id],
      CustomFormId: [customFormId],
      Name: [model.Name, Validators.required],
      CustomFormKeycode:[model.CustomFormKeycode],
      ParentDataInfo_Id: [model.ParentDataInfo_Id],
      CustomFormSetting: this.fb.group({
        CustomFormItemSettings: this.fb.array([])
      })
    });

    this.CustomFormSetting = model.CustomFormSetting;
    this.listCustomFormItemById(customFormId);
  }

  OnSubmit() {
    if (this.formModel.valid) {
      
      this.saveFormModelCustomFormSetting();

      this.DataInfoService.editDataInfo(this.formModel.value, (data: ReturnInfoModel) => {
        this.activeModal.close(data);
      });
    }
    else {
      this.validateForm();
    }
  }

  ngOnInit() {

  }
}
