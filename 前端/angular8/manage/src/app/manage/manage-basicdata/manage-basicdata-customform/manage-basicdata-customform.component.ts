import { remove, assign } from 'lodash';
import { CustomformService } from './../../../services/customform.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { DisplayCustomFormModel, DisplayCustomFormItemModel } from 'src/app/models/CustomFormModels';
import { SweetError, SweetConfirm } from 'src/app/common/common';
import { NgbModal } from 'src/app/common/modal/modal';
import { ManageBasicdataCustomformCreateComponent } from './manage-basicdata-customform-create/manage-basicdata-customform-create.component';
import { ManageBasicdataCustomformitemCreateComponent } from './manage-basicdata-customformitem-create/manage-basicdata-customformitem-create.component';
import { NgbModalRef } from 'src/app/common/modal/modal-ref';
import { ManageBasicdataCustomformitemEditComponent } from './manage-basicdata-customformitem-edit/manage-basicdata-customformitem-edit.component';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-manage-basicdata-customform',
  templateUrl: './manage-basicdata-customform.component.html',
  styleUrls: ['./manage-basicdata-customform.component.css']
})
export class ManageBasicdataCustomformComponent implements OnInit {

  customFormId: string = "";
  customFormItemList: Array<DisplayCustomFormItemModel> = [];
  selectCustomFormItemList: Array<DisplayCustomFormItemModel> = [];
  isSelectAll: boolean = false;

  constructor(private routeInfo: ActivatedRoute,
    private CustomformService: CustomformService,
    private modalService: NgbModal) {
    this.routeInfo.params.subscribe((p: Params) => {
      this.customFormId = p['id'];
      this.loadCustomformItemList();
    });
  }

  drop(event: CdkDragDrop<string[]>) {
    // debugger;
    // console.log("pre:"+this.customFormItemList[event.previousIndex].Id);
    // console.log("cur:"+this.customFormItemList[event.currentIndex].Id);
    // console.log(event);
    moveItemInArray(this.customFormItemList, event.previousIndex, event.currentIndex);
    this.CustomformService.saveItemOrder(this.customFormId,
      this.customFormItemList.map(t => t.Id).join(","),
      (data: ReturnInfoModel) => {
        if (!data.State)
          SweetError('', data.Message);
      });
  }

  ngOnInit() {
  }

  updateAllSelect(ev) {
    this.selectAll(ev.target.checked);
  }

  selectAll(isSelectAll) {
    this.isSelectAll = isSelectAll;
    if (isSelectAll)
      this.selectCustomFormItemList = this.customFormItemList.filter(t => !t.IsLock);
    else
      this.selectCustomFormItemList = [];
  }

  updateSelect(item: DisplayCustomFormItemModel, ev) {
    if (item.IsLock) {
      return;
    }
    if (ev.target.checked) {
      console.log(item);
      if (this.selectCustomFormItemList.indexOf(item) === -1) {
        this.selectCustomFormItemList.push(item);
      }
    } else {
      if (this.selectCustomFormItemList.indexOf(item) !== -1) {
        this.selectCustomFormItemList.splice(this.selectCustomFormItemList.indexOf(item), 1);
      }
    }
  }

  loadCustomformItemList() {
    if (this.customFormId != "") {
      this.CustomformService.itemList(this.customFormId, false, (data: ReturnInfoModel) => {
        if (data.State) {
          this.customFormItemList = data.DataObject;
        }
        else
          SweetError("", data.Message);
      });
    }
  }

  createCustomFormItem() {
    if (this.customFormId != null) {
      let modal: NgbModalRef = this.modalService.open(ManageBasicdataCustomformitemCreateComponent, { backdrop: 'static', keyboard: true });
      modal.componentInstance.SetForm(this.customFormId);
      modal.result.then((result: ReturnInfoModel) => {
        if (!result) {
          return;
        }

        if (!result.State) {
          SweetError("", result.Message);
        } else {
          this.customFormItemList.push(result.DataObject);
        }
      });
    }
    else
      SweetError("", "请选择表单！");
  }

  removeCustomFormItem(Id: string) {
    SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.CustomformService.deleteItem(Id, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else {
            remove(this.customFormItemList, function (item) {
              return item.Id === Id;
            });
          }
        });
      }
    });
  }

  lockCustomFormItem(Id: string) {
    SweetConfirm("", "确定锁定该表单项吗？如锁定，该表单项将无法修改及删除。", (isConfirm: boolean) => {
      if (isConfirm) {
        this.CustomformService.lockItem(Id, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else {
            for (let info of this.customFormItemList) {
              if (info.Id === Id) {
                info.IsLock = true;
                return;
              }
            }
          }
        });
      }
    });
  }

  unlockCustomFormItem(Id: string) {
    SweetConfirm("", "确定解锁该表单项吗？如解锁，该表单项将能够修改及删除。", (isConfirm: boolean) => {
      if (isConfirm) {
        this.CustomformService.unlockItem(Id, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else {
            for (let info of this.customFormItemList) {
              if (info.Id === Id) {
                info.IsLock = false;
                return;
              }
            }
          }
        });
      }
    });
  }

  removeCustomFormItems() {
    if (this.selectCustomFormItemList.length > 0) {
      SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
        if (isConfirm) {
          this.CustomformService.deleteItems(this.selectCustomFormItemList.map(t => t.Id).join(","), (returnInfo: ReturnInfoModel) => {
            if (!returnInfo.State) {
              setTimeout(() => {
                SweetError('', returnInfo.Message);
              }, 200);
            }
            else {
              var _this = this;
              remove(this.customFormItemList, function (item) {
                return _this.selectCustomFormItemList.map(t => t.Id).indexOf(item.Id) !== -1;
              });
            }
          });
        }
      });
    }
    else
      SweetError("", "请选择表单项！");
  }


  editCustomFormItem(modelInfo) {
    let modal: NgbModalRef = this.modalService.open(ManageBasicdataCustomformitemEditComponent, { backdrop: 'static', keyboard: true });
    modal.componentInstance.SetForm(modelInfo);
    modal.result.then((result: ReturnInfoModel) => {
      if (!result) return;

      if (!result.State) {
        SweetError("", result.Message);
      } else {
        for (let info of this.customFormItemList) {
          if (info.Id === result.DataObject.Id) {
            assign(info, result.DataObject);
            return;
          }
        }
      }
    });
  }
}
