import { SweetError } from 'src/app/common/common';
import { CommonService } from './../../services/common.service';
import { Component, OnInit } from '@angular/core';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { FormBuilder, Validators } from '@angular/forms';
import { PassportService } from 'src/app/services/passport.service';
import { Router } from '@angular/router';
import { ReturnInfoModel, ReturnTokenInfoModel } from 'src/app/models/ReturnInfoModel';

@Component({
  selector: 'app-account-login',
  templateUrl: './account-login.component.html',
  styleUrls: ['./account-login.component.css']
})
export class AccountLoginComponent extends BaseForm implements OnInit {
  ngOnInit(): void {
    //throw new Error("Method not implemented.");
  }

  constructor( private fb: FormBuilder, 
    private PassportService:PassportService,
    private CommonService:CommonService, 
    private router: Router) { 

    super();
    this.formModel = fb.group({ 
      UserName: ['', Validators.required],
      Password: ['', Validators.required]
    });

    let body = document.getElementsByTagName('body')[0];
    body.classList.add("login-container");  
    body.classList.add("login-cover");  
    body.classList.add("pace-done");  
  }

  OnSubmit() {
    if (this.formModel.valid) {
      this.PassportService.login(this.formModel.value,(data:ReturnTokenInfoModel) =>{
        if(data.State){
          this.CommonService.setToken(data.AccessToken);
          this.router.navigate(["/manage"]);
        }
        else
          SweetError('无法登陆',data.Message);
      });
    }
  }

}

