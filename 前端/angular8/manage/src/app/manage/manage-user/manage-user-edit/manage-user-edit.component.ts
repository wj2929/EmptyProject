import { CategoryService } from './../../../services/category.service';
import { CommonService } from './../../../services/common.service';
import { DisplayUserExtendModel } from './../../../models/PassportLoginModel';
import { SweetError } from 'src/app/common/common';
import { PassportService } from './../../../services/passport.service';
import { Component, OnInit } from '@angular/core';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { FormBuilder, FormArray, Validators } from '@angular/forms';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { DisplayOrganModel } from 'src/app/models/OrganModels';
import { CustomformService } from 'src/app/services/customform.service';
import { CustomFormSettingModel } from 'src/app/common/custom-form-setting/CustomFormSettingModels';
import { UniqueUserUsernameValidator } from 'src/app/validators/unique-user-username-validator';
import { BaseEditCustomForm } from 'src/app/common/util/BaseEditCustomForm';

@Component({
  selector: 'app-manage-user-edit',
  templateUrl: './manage-user-edit.component.html',
  styleUrls: ['./manage-user-edit.component.css']
})
export class ManageUserEditComponent extends BaseEditCustomForm implements OnInit {

  public organs:Array<DisplayOrganModel>=[];
  customFormKey :string = "UserExtend";   
  customFormItemList=[];
  model: DisplayUserExtendModel;

  constructor(public activeModal: NgbActiveModal, 
    public fb: FormBuilder,
    public CustomformService:CustomformService,
    private PassportService:PassportService,
    public CommonService:CommonService,
    public CategoryService:CategoryService){
    super(CustomformService,CommonService,CategoryService,fb);
  }

  Close() {
    this.activeModal.close();
  }

  SetForm(model: DisplayUserExtendModel) {
    this.model = model;

    this.CustomFormSetting = model.CustomFormSetting;

    this.formModel = this.fb.group({
      EditId:[model.Id],
      CustomFormKeycode: [this.customFormKey],
      UserName: [{value:model.UserName,disabled: true}, Validators.required],
      CustomFormSetting: this.fb.group({
        CustomFormItemSettings: this.fb.array([])
      })
    });

    this.listCustomFormItem(this.customFormKey);
  }

  OnSubmit() {
    // debugger;
    if (this.formModel.valid) {
      this.saveFormModelCustomFormSetting();
      
      this.PassportService.editUser(this.formModel.value,(data:ReturnInfoModel)=>{
        if(!data.State)
          SweetError("",data.Message);
        else
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
