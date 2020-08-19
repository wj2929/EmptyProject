import { CommonService } from './../../../../services/common.service';
import { CustomformService } from './../../../../services/customform.service';
import { CategoryService } from './../../../../services/category.service';
import { Component, OnInit } from '@angular/core';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { CustomFormSettingModel } from 'src/app/common/custom-form-setting/CustomFormSettingModels';
import * as $ from "jquery";
import { BaseCreateCustomForm } from 'src/app/common/util/BaseCreateCustomForm';
@Component({
  selector: 'app-manage-basicdata-category-create',
  templateUrl: './manage-basicdata-category-create.component.html',
  styleUrls: ['./manage-basicdata-category-create.component.css']
})
export class ManageBasicdataCategoryCreateComponent extends BaseCreateCustomForm implements OnInit {
  categoryTypeId = "";
  customFormId = "";
  parentCategory_Id = "";
  customFormList = [];
  customFormItemList = [];
  categoryTypeName ="";
  constructor(public activeModal: NgbActiveModal,
    public fb: FormBuilder,
    public CustomformService: CustomformService,
    public CommonService:CommonService,
    public CategoryService:CategoryService) {
    super(CustomformService,CommonService,CategoryService,fb);

  }

  Close() {
    this.activeModal.close();
  }

  SetForm(categoryTypeId, customFormId, parentCategory_Id,categoryTypeName) {
    this.categoryTypeId = categoryTypeId;
    this.customFormId = customFormId;
    this.parentCategory_Id = parentCategory_Id;
    this.categoryTypeName = categoryTypeName;
    this.formModel = this.fb.group({
      CategoryTypeId: [this.categoryTypeId],
      CustomFormId: [this.customFormId],
      Name: ['', Validators.required],
      ParentCategory_Id: [this.parentCategory_Id],
      // OrderBy: ['0'],
      CustomFormSetting: this.fb.group({
        CustomFormItemSettings: this.fb.array([])
      })
    });

    // this.formModel.get("CategoryTypeId").setValue(this.categoryTypeId);
    // this.formModel.get("CustomFormId").setValue(this.customFormId);
    // this.formModel.get("ParentCategory_Id").setValue(this.parentCategory_Id);


    this.listCustomFormItemById(this.customFormId);
  }

  OnSubmit() {
    if (this.formModel.valid) {
      this.saveFormModelCustomFormSetting();

      this.CategoryService.createCategory(this.formModel.value, (data: ReturnInfoModel) => {
        this.activeModal.close(data);
      });
    }
    else {
      this.validateForm();
    }
  }

  changeCustomForm($event) {
    this.listCustomFormItemById($event);
  }

  ngOnInit() {
  //   this.formModel = this.fb.group({
  //     CategoryTypeId: [this.categoryTypeId],
  //     CustomFormId: [this.customFormId],
  //     Name: ['', Validators.required],
  //     ParentCategory_Id: [this.parentCategory_Id],
  //     // OrderBy: ['0'],
  //     CustomFormSetting: this.fb.group({
  //       CustomFormItemSettings: this.fb.array([])
  //     })
  //   });
  }
}
