import { CategoryService } from './../../../../services/category.service';
import { CustomformService } from './../../../../services/customform.service';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { Component, OnInit } from '@angular/core';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { DisplayCustomFormModel } from 'src/app/models/CustomFormModels';
import { Validators, FormBuilder, FormControl } from '@angular/forms';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { DisplayCategoryTypeModel } from 'src/app/models/CategoryModels';
declare var $: any;
@Component({
  selector: 'app-manage-basicdata-categorytype-edit',
  templateUrl: './manage-basicdata-categorytype-edit.component.html',
  styleUrls: ['./manage-basicdata-categorytype-edit.component.css']
})
export class ManageBasicdataCategorytypeEditComponent extends BaseForm implements OnInit {

  customFormList:Array<DisplayCustomFormModel>=[];
  model: DisplayCategoryTypeModel;
  constructor(
    public activeModal: NgbActiveModal, 
    private fb: FormBuilder,
    private CustomformService:CustomformService,
    private CategoryService:CategoryService) {
    super();

    // this.formModel = this.fb.group({
    //   CustomFormId:['',Validators.required],
    //   Name: ['', Validators.required],
    //   Keycode: ['', Validators.required]
    // });
  }

  SetForm(model: DisplayCategoryTypeModel) {
    this.model = model;
    this.formModel = this.fb.group({
      Name: [this.model.Name, Validators.required],
      Keycode: [this.model.Keycode],
      CustomFormId:[this.model.CustomFormId],
      EditId: [this.model.Id, Validators.required]
    });

    let _this = this;
    $('.select-search').select2().on("change", function (e) {
      console.log(e);
      let formControl = _this.formModel.get(e.target.name) as FormControl;
      formControl.setValue(e.target.value);
    });
  }


  ngOnInit() {
    this.listCustomForm();
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
      this.CategoryService.editCategoryType(this.formModel.value, (data: ReturnInfoModel) => {
        this.activeModal.close(data);
      });
    }
    else {
      this.validateForm();
    }
  }

}
