import { CategoryService } from './../../../services/category.service';
import { CommonService } from 'src/app/services/common.service';
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
import { BaseCreateCustomForm } from 'src/app/common/util/BaseCreateCustomForm';

@Component({
  selector: 'app-manage-user-create',
  templateUrl: './manage-user-create.component.html',
  styleUrls: ['./manage-user-create.component.css']
})
export class ManageUserCreateComponent extends BaseCreateCustomForm implements OnInit {

  public organs:Array<DisplayOrganModel>=[];
  customFormKey :string = "UserExtend";   
  // customFormItemList=[];
  
  constructor(public activeModal: NgbActiveModal, 
    public fb: FormBuilder,
    public CustomformService:CustomformService,
    private PassportService:PassportService,
    public CategoryService: CategoryService,
    public CommonService:CommonService) {
    super(CustomformService,CommonService,CategoryService,fb);
  }

  Close() {
    this.activeModal.close();
  }

  SetForm() {
    this.listCustomFormItem(this.customFormKey);
  }

  // listCustomFormItem(){
  //   this.CustomformService.itemListByKey(this.customFormKey,true,(data:ReturnInfoModel) =>{
  //     if(data.State){
  //       this.customFormItemList = data.DataObject;
  //       let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;
  //       this.customFormItemList.forEach(t => {
  //         CustomFormItemSettings.push(this.fb.group(new CustomFormSettingModel(t.Key,t.Name,"")));
  //       });
  //     }
  //   });
  // }

  OnSubmit() {
    // debugger;
    if (this.formModel.valid) {
      // let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;
      // this.formModel.value.CustomFormSetting.CustomFormItemSettings.forEach(element => {
      //   if(element.Value.constructor == Array)
      //     element.Value =  element.Value.join(",");
      // });
      
      this.saveFormModelCustomFormSetting();
      this.PassportService.createUser(this.formModel.value,(data:ReturnInfoModel)=>{
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
    this.formModel = this.fb.group({
      UserName:['',{
        validators: Validators.required,
        asyncValidators: [UniqueUserUsernameValidator(this.PassportService)],
        updateOn: 'blur'
      }],
      Password:['', Validators.required],
      ConfirmPassword:['',Validators.required],
      CustomFormKeycode:[this.customFormKey],
      IsLock:[false],
      CustomFormSetting:this.fb.group({
        CustomFormItemSettings: this.fb.array([])
      })
    });


  }
}
