import { CustomformService } from './../../../../services/customform.service';
import { Component, OnInit } from '@angular/core';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { UniqueCustomFormKeyValidator } from 'src/app/validators/unique-custom-form-key-validator';

@Component({
  selector: 'app-manage-basicdata-customform-create',
  templateUrl: './manage-basicdata-customform-create.component.html',
  styleUrls: ['./manage-basicdata-customform-create.component.css']
})
export class ManageBasicdataCustomformCreateComponent extends BaseForm implements OnInit {


  constructor(public activeModal: NgbActiveModal,
    private fb: FormBuilder,
    private CustomformService: CustomformService) {
    super();
    this.formModel = fb.group({
      Name: ['', Validators.required],
      Key: ['', {
        validators: Validators.required,
        asyncValidators: [UniqueCustomFormKeyValidator(this.CustomformService)],
        updateOn: 'blur'
      }]
    });
  }

  Close() {
    this.activeModal.close();
  }

  OnSubmit() {
    if (this.formModel.valid) {
      this.CustomformService.create(this.formModel.value, (data: ReturnInfoModel) => {
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
