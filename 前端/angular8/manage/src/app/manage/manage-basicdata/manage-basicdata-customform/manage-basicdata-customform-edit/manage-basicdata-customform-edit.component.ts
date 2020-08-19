import { CustomformService } from './../../../../services/customform.service';
import { Component, OnInit } from '@angular/core';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { DisplayCustomFormModel } from 'src/app/models/CustomFormModels';
import { Validators, FormBuilder } from '@angular/forms';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';

@Component({
  selector: 'app-manage-basicdata-customform-edit',
  templateUrl: './manage-basicdata-customform-edit.component.html',
  styleUrls: ['./manage-basicdata-customform-edit.component.css']
})
export class ManageBasicdataCustomformEditComponent extends BaseForm implements OnInit {
  CPSId: string;
  model: DisplayCustomFormModel;
  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private CustomformService:CustomformService) {
    super();
  }

  SetForm(model: DisplayCustomFormModel) {
    this.model = model;
    this.formModel = this.fb.group({
      Name: [this.model.Name, Validators.required],
      Key: [this.model.Key],
      EditId: [this.model.Id, Validators.required]
    });
  }

  ngOnInit() {
    
  }

  Close() {
    this.activeModal.close();
  }

  OnSubmit() {
    if (this.formModel.valid) {
      this.CustomformService.edit(this.formModel.value, (data: ReturnInfoModel) => {
        this.activeModal.close(data);
      });
    }
    else {
      this.validateForm();
    }
  }


}