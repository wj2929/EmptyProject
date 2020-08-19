import { CategoryService } from './../../../../services/category.service';
import { CustomformService } from './../../../../services/customform.service';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { Component, OnInit } from '@angular/core';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { DisplayCustomFormModel } from 'src/app/models/CustomFormModels';
import { Validators, FormBuilder, FormControl } from '@angular/forms';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { UniqueCategoryTypeKeyValidator } from 'src/app/validators/unique-category-type-key-validator';
declare var $: any;
@Component({
  selector: 'app-manage-basicdata-categorytype-create',
  templateUrl: './manage-basicdata-categorytype-create.component.html',
  styleUrls: ['./manage-basicdata-categorytype-create.component.css']
})
export class ManageBasicdataCategorytypeCreateComponent extends BaseForm implements OnInit {

  customFormList:Array<DisplayCustomFormModel>=[];
  constructor(
    public activeModal: NgbActiveModal, 
    private fb: FormBuilder,
    private CustomformService:CustomformService,
    private CategoryService:CategoryService) {
    super();

    this.formModel = this.fb.group({
      CustomFormId:['',Validators.required],
      Name: ['', Validators.required],
      Keycode: ['', {
        validators: Validators.required,
        asyncValidators: [UniqueCategoryTypeKeyValidator(this.CategoryService)],
        updateOn: 'blur'
      }]
    });
  }

  ngOnInit() {
    this.listCustomForm();

    let _this = this;
    $('.select-search').select2().on("change", function (e) {
      console.log(e);
      let formControl = _this.formModel.get(e.target.name) as FormControl;
      formControl.setValue(e.target.value);
    });
  }

  listCustomForm(){
    this.CustomformService.list((data :ReturnInfoModel) =>{
      if(data.State){
        this.customFormList = data.DataObject;
      }
    });
  }

  Close() {
    this.activeModal.close();
  }


  OnSubmit() {
    if (this.formModel.valid) {
      this.CategoryService.createCategoryType(this.formModel.value, (data: ReturnInfoModel) => {
        this.activeModal.close(data);
      });
    }
    else {
      this.validateForm();
    }
  }

}
