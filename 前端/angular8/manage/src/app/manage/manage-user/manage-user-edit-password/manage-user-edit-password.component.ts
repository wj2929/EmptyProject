import { DisplayUserExtendModel } from './../../../models/PassportLoginModel';
import { PassportService } from 'src/app/services/passport.service';
import { Component, OnInit } from '@angular/core';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { FormArray, Validators, FormBuilder } from '@angular/forms';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { SweetSuccess, SweetError } from 'src/app/common/common';


@Component({
  selector: 'app-manage-user-edit-password',
  templateUrl: './manage-user-edit-password.component.html',
  styleUrls: ['./manage-user-edit-password.component.css']
})
export class ManageUserEditPasswordComponent extends BaseForm implements OnInit {

  constructor(public activeModal: NgbActiveModal,
    private fb: FormBuilder,
    private PassportService:PassportService) {
    super();
  }

  Close() {
    this.activeModal.close();
  }

  SetForm(model:DisplayUserExtendModel) {
    this.formModel = this.fb.group({
      UserName: [model.UserName, Validators.required],
      Password: ['', Validators.required],
      ConfirmPassword: ['', Validators.required]
    });
  }

  OnSubmit() {
    if (this.formModel.valid) {
      this.PassportService.managePassword(this.formModel.value, (data: ReturnInfoModel) => {
        if(data.State){
          this.activeModal.close(data);
          SweetSuccess("", "修改密码完成！");
        }
        else
          SweetError("",data.Message);
      });
    }
    else {
      this.validateForm();
    }
  }

  ngOnInit() {

  }

}
