import { CustomformService } from './../../../../services/customform.service';
import { Component, OnInit } from '@angular/core';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { FormBuilder, Validators, FormControl } from '@angular/forms';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { UniqueCustomFormKeyValidator } from 'src/app/validators/unique-custom-form-key-validator';
import { UniqueCustomFormItemNameValidator } from 'src/app/validators/unique-custom-form-item-name-validator';
import { UniqueCustomFormItemKeyValidator } from 'src/app/validators/unique-custom-form-item-key-validator';
declare var $: any;
@Component({
  selector: 'app-manage-basicdata-customformitem-create',
  templateUrl: './manage-basicdata-customformitem-create.component.html',
  styleUrls: ['./manage-basicdata-customformitem-create.component.css']
})
export class ManageBasicdataCustomformitemCreateComponent extends BaseForm implements OnInit {

  public FormTypes = [{index:0,name:"单行文本框"},
    {index:1,name:"多行文本框"},
    {index:2,name:"列表框"},
    {index:3,name:"上传文件框"},
    {index:10,name:"上传身份证正面"},
    {index:11,name:"上传身份证背面"},
    {index:4,name:"日期选择框"},
    {index:5,name:"日期范围选择框"},
    {index:6,name:"表单选择框"},
    {index:7,name:"CKEditor富文本编辑器"},
    {index:12,name:"Summernote富文本编辑器"},
    {index:8,name:"分类选择框"},
    {index:9,name:"可动态维护表单框"},
    {index:13,name:"微信自定义菜单内容"}
  ];

  public ExpressionValidatorLibrarys = [
    {value:"",name:"不使用"},
    {value:"^[A-Za-z0-9](([_\\.\\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\\.\\-]?[a-zA-Z0-9]+)*)\\.([A-Za-z]{2,})$",name:"Email"},
    {value:"^[\\p{N}]*$",name:"无符号整数"},
    {value:"^[0-9-]*$",name:"整数"},
    {value:"^[0-9.-]*$",name:"数字（包括小数和负号）"},
    {value:"^(((http|https)://)|(www\\.))+(([a-zA-Z0-9\\._-]+\\.[a-zA-Z]{2,6})|([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}))(/[a-zA-Z0-9\\&%_\\./-~-]*)?$",name:"Url（http|https）"},
    {value:"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$",name:"IP地址（IPV4）"},
    {value:"^\\d{3,4}-?\\d{6,8}$",name:"带区号电话号码"},
    {value:"^\\d{6}$",name:"邮政编码"},
    {value:"^([1-9]\\d{7}((0\\d)|(1[0-2]))(([0|1|2]\\d)|3[0-1])\\d{3}|[1-9]\\d{5}[1-9]\\d{3}((0\\d)|(1[0-2]))(([0|1|2]\\d)|3[0-1])((\\d{4})|\\d{3}[X]))$",name:"身份证号码（15位或18位）"},
  ];

  customFormId="";
  constructor(public activeModal: NgbActiveModal, 
    private fb: FormBuilder,
    private CustomformService:CustomformService ) {
    super();


  }

  Close() {
    this.activeModal.close();
  }

  SetForm(customFormId) {
    this.customFormId = customFormId;
  }

  OnSubmit() {
    if (this.formModel.valid) {
      this.CustomformService.createItem(this.formModel.value, (data: ReturnInfoModel) => {
        this.activeModal.close(data);
      });
    }
    else {
      this.validateForm();
    }
  }

  onChangeExpressionValidatorLibrary($event){
    this.formModel.get("ValidationConfig_RegularExpressionValidator").setValue($event.srcElement.value);
  }

  ngOnInit() {
    this.formModel = this.fb.group({
      CustomFormId:[this.customFormId],
      Enabled:[false],
      Name: ['', {
        validators: Validators.required,
        asyncValidators: [UniqueCustomFormItemNameValidator(this.CustomformService,this.customFormId)],
        updateOn: 'blur'
      }],
      Key: ['', {
        validators: Validators.required,
        asyncValidators: [UniqueCustomFormItemKeyValidator(this.CustomformService,this.customFormId)],
        updateOn: 'blur'
      }],
      Description: [''],
      FormType: [0],
      OptionText:[''],
      MoreSelect:[false],
      IsLock:[false],
      IsHide:[false],
      ValidationConfig_Required:[false],
      ValidationConfig_AllowExtensionValidation:[false],
      ValidationConfig_RegularExpressionValidator:[''],
      ValidationConfig_ErrorMessage:['']
    });

    let _this = this;
    $('.select-search').select2().on("change", function (e) {
      console.log(e);
      let formControl = _this.formModel.get(e.target.name) as FormControl;
      if(formControl != null)formControl.setValue(e.target.value);
      if(e.currentTarget.id=="ExpressionValidatorLibrary")
        _this.formModel.get("ValidationConfig_RegularExpressionValidator").setValue(e.currentTarget.value);
    });
  }

}
