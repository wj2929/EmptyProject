import { ManageUserEditPasswordComponent } from './manage-user-edit-password/manage-user-edit-password.component';
import { ManageUserCreateComponent } from './manage-user-create/manage-user-create.component';

import { CategoryService } from './../../services/category.service';
import { remove, assign } from 'lodash';
import { Component, OnInit } from '@angular/core';
import { ReturnInfoModel, ReturnPagingModel, PagingModel, ModulePageModel } from 'src/app/models/ReturnInfoModel';
import { PassportService } from 'src/app/services/passport.service';
import { NgbModalRef } from 'src/app/common/modal/modal-ref';
import { NgbModal } from 'src/app/common/modal/modal';
import { ActivatedRoute } from '@angular/router';
import { PaginationModel } from 'src/app/common/pagination/PaginationModels';
import { DisplayCategoryModel } from 'src/app/models/CategoryModels';
import { SweetError, SweetConfirm, SweetSuccess } from 'src/app/common/common';
import * as moment from 'moment';
declare var $: any;
import { DisplayUserExtendModel } from 'src/app/models/PassportLoginModel';
import { ManageUserEditComponent } from './manage-user-edit/manage-user-edit.component';
import { DisplayOrganModel } from 'src/app/models/OrganModels';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { FormControl, FormBuilder } from '@angular/forms';
import { debug } from 'util';

@Component({
  selector: 'app-manage-user',
  templateUrl: './manage-user.component.html',
  styleUrls: ['./manage-user.component.css']
})
export class ManageUserComponent extends BaseForm implements OnInit {

  public UserPaging: PagingModel<DisplayUserExtendModel> = new PagingModel<DisplayUserExtendModel>(new ModulePageModel(), []);
  public UserPaginationModel: PaginationModel = new PaginationModel();
  public userName: string = "";
  public organType: string = "";
  public isLock: string = "";
  public categoryIds = [];
  public selectUserList: Array<DisplayUserExtendModel> = [];
  public certificateCategories: Array<DisplayCategoryModel> = [];
  public isSelectAll: boolean = false;
  public organList: Array<DisplayOrganModel> = [];

  constructor(private PassportService: PassportService,
    private CategoryService: CategoryService,
    private modalService: NgbModal,
    private routeInfo: ActivatedRoute,
    private fb: FormBuilder) {
    super();

    this.formModel = this.fb.group({
      UserName: [''],
      IsLock: ['']
    });
  }

  ngOnInit() {
    this.loadUserPaging();
    let _this = this;
    $('.select-search').select2().on("change", function (e) {
      console.log(e);
      let formControl = _this.formModel.get(e.target.name) as FormControl;
      formControl.setValue(e.target.value);
    });
  }

  updateAllSelect(ev) {
    this.selectAll(ev.target.checked);
  }

  selectAll(isSelectAll) {
    this.isSelectAll = isSelectAll;
    if (isSelectAll)
      this.selectUserList = this.UserPaging.PageListInfos;
    else
      this.selectUserList = [];
  }

  updateOrganTypeSelect(organType, ev) {
    this.organType = organType;
    this.formModel.get("OrganType").setValue(organType);
  }

  updateIsLockSelect(isLock, ev) {
    this.isLock = isLock;
    this.formModel.get("IsLock").setValue(isLock);
  }

  updateSelect(item, ev) {
    if (ev.target.checked) {
      console.log(item);
      if (this.selectUserList.indexOf(item) === -1) {
        this.selectUserList.push(item);
      }
    } else {
      if (this.selectUserList.indexOf(item) !== -1) {
        this.selectUserList.splice(this.selectUserList.indexOf(item), 1);
      }
    }
  }

  onSearch() {
    this.loadUserPaging();
  }

  loadUserPaging(PageNum: number = 1, PageSize: number = 10) {
    this.PassportService.pagingUser(this.formModel.value, PageNum, PageSize, (data: ReturnPagingModel<DisplayUserExtendModel>) => {
      if (data.State) {
        this.UserPaging = data.DataObject;
        this.UserPaginationModel = new PaginationModel(
          data.DataObject.Module_Page.AllCount,
          data.DataObject.Module_Page.PageNum,
          data.DataObject.Module_Page.PageCount);
      }
      else
        SweetError("", data.Message);
    });
  }

  onUserPageChanged($event) {
    this.loadUserPaging($event.page);
  }

  removeUser(userName: string) {
    SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.PassportService.deleteUser(userName, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else
            this.loadUserPaging();
        });
      }
    });
  }
  createUser() {
    let modal: NgbModalRef = this.modalService.open(ManageUserCreateComponent, { backdrop: 'static', keyboard: true });
    modal.componentInstance.SetForm();
    modal.result.then((result: ReturnInfoModel) => {
      if (!result) {
        return;
      }

      if (!result.State) {
        SweetError("", result.Message);
      } else {
        this.loadUserPaging();
      }
    });
  }

  refreshToken(modelInfo) {
    SweetConfirm("", "确定刷新该用户Token吗？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.PassportService.refreshToken(
          {
            UserName: modelInfo.UserName,
            UserToken: modelInfo.AccessToken
          }, (returnInfo: ReturnInfoModel) => {
            if (!returnInfo.State) {
              setTimeout(() => {
                SweetError('', returnInfo.Message);
              }, 200);
            }
            else
              this.loadUserPaging();
          });
      }
    });
  }

  editUser(modelInfo) {
    let modal: NgbModalRef = this.modalService.open(ManageUserEditComponent, { backdrop: 'static', keyboard: true });
    modal.componentInstance.SetForm(modelInfo);
    modal.result.then((result: ReturnInfoModel) => {
      if (!result) return;

      if (!result.State) {
        SweetError("", result.Message);
      } else {
        this.loadUserPaging();
      }
    });
  }

  editPassword(modelInfo) {
    let modal: NgbModalRef = this.modalService.open(ManageUserEditPasswordComponent, { backdrop: 'static', keyboard: true });
    modal.componentInstance.SetForm(modelInfo);
    modal.result.then((result: ReturnInfoModel) => {
      if (!result) return;

      if (!result.State) {
        SweetError("", result.Message);
      } else {
        this.loadUserPaging();
      }
    });
  }

  removeUsers() {
    if (this.selectUserList.length > 0) {
      SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
        if (isConfirm) {
          this.PassportService.deleteUsers(this.selectUserList.map(t => t.UserName).join(","), (returnInfo: ReturnInfoModel) => {
            if (!returnInfo.State) {
              setTimeout(() => {
                SweetError('', returnInfo.Message);
              }, 200);
            }
            else
              this.loadUserPaging();
          });
        }
      });
    }
    else
      SweetError("", "请选择用户！");
  }

  unlockUser(username) {
    SweetConfirm("", "确定解锁该用户？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.PassportService.unLockUser(username, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else
            this.loadUserPaging();
        });
      }
    });
  }

  lockUser(username) {
    SweetConfirm("", "确定锁定该用户？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.PassportService.lockUser(username, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else
            this.loadUserPaging();
        });
      }
    });
  }

}
