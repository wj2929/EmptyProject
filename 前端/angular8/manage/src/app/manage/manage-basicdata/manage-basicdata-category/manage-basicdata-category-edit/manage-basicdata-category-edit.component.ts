import { CommonService } from './../../../../services/common.service';
import { DisplayCategoryModel } from './../../../../models/CategoryModels';
import { CustomformService } from './../../../../services/customform.service';
import { CategoryService } from './../../../../services/category.service';
import { Component, OnInit } from '@angular/core';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { CustomFormSettingModel } from 'src/app/common/custom-form-setting/CustomFormSettingModels';
import { BaseEditCustomForm } from 'src/app/common/util/BaseEditCustomForm';

@Component({
  selector: 'app-manage-basicdata-category-edit',
  templateUrl: './manage-basicdata-category-edit.component.html',
  styleUrls: ['./manage-basicdata-category-edit.component.css']
})
export class ManageBasicdataCategoryEditComponent extends BaseEditCustomForm implements OnInit {
  categoryTypeId = "";
  customFormId = "";
  parentCategory_Id = "";
  customFormList = [];
  customFormItemList = [];
  model: DisplayCategoryModel;
  categoryTypeName="";
  constructor(public activeModal: NgbActiveModal,
    public fb: FormBuilder,
    public CategoryService: CategoryService,
    public CustomformService: CustomformService,
    public CommonService: CommonService) {
    super(CustomformService,CommonService,CategoryService,fb);
  }

  Close() {
    this.activeModal.close();
  }

  SetForm(model: DisplayCategoryModel, customFormId: string,categoryTypeName) {
    this.model = model;
    this.categoryTypeName = categoryTypeName;
    this.formModel = this.fb.group({
      EditId: [model.Id],
      CategoryTypeId: [model.CategoryTypeId],
      CustomFormId: [customFormId],
      Name: [model.Name, Validators.required],
      ParentCategory_Id: [model.ParentCategory_Id],
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

      this.CategoryService.editCategory(this.formModel.value, (data: ReturnInfoModel) => {
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
