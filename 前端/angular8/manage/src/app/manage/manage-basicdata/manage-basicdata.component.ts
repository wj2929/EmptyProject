import { ManageBasicdataCustomformCreateComponent } from './manage-basicdata-customform/manage-basicdata-customform-create/manage-basicdata-customform-create.component';
import { CategoryService } from './../../services/category.service';
import { CustomformService } from './../../services/customform.service';
import { Component, OnInit } from '@angular/core';
import { NgbModal } from 'src/app/common/modal/modal';
import { Router } from '@angular/router';
import { ManageBasicdataCustomformEditComponent } from './manage-basicdata-customform/manage-basicdata-customform-edit/manage-basicdata-customform-edit.component';
import { NgbModalRef } from 'src/app/common/modal/modal-ref';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { remove, assign } from 'lodash';
import { DisplayCustomFormModel } from 'src/app/models/CustomFormModels';
import { SweetConfirm, SweetError } from 'src/app/common/common';
import { DisplayCategoryTypeModel } from 'src/app/models/CategoryModels';
import { ManageBasicdataCategorytypeCreateComponent } from './manage-basicdata-category/manage-basicdata-categorytype-create/manage-basicdata-categorytype-create.component';
import { ManageBasicdataCategorytypeEditComponent } from './manage-basicdata-category/manage-basicdata-categorytype-edit/manage-basicdata-categorytype-edit.component';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-manage-basicdata',
  templateUrl: './manage-basicdata.component.html',
  styleUrls: ['./manage-basicdata.component.css']
})
export class ManageBasicdataComponent implements OnInit {

  customFormList: Array<DisplayCustomFormModel> = [];
  categoryTypeList: Array<DisplayCategoryTypeModel> = [];
  constructor(private modalService: NgbModal,
    private router: Router,
    private CustomformService: CustomformService,
    private CategoryService: CategoryService) { }

  ngOnInit() {
    this.listCustomForm();
    this.listCategoryType();
  }

  dropCustomForm(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.customFormList, event.previousIndex, event.currentIndex);
    this.CustomformService.saveOrder(
      this.customFormList.map(t => t.Id).join(","),
      (data: ReturnInfoModel) => {
        if (!data.State)
          SweetError('', data.Message);
      });
  }

  dropCategoryType(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.categoryTypeList, event.previousIndex, event.currentIndex);
    this.CategoryService.saveOrder(
      this.categoryTypeList.map(t => t.Id).join(","),
      (data: ReturnInfoModel) => {
        if (!data.State)
          SweetError('', data.Message);
      });
  }

  listCustomForm() {
    this.CustomformService.list(data => {
      if (data.State) {
        this.customFormList = data.DataObject;
      }
      else
        SweetError('',data.Message);
    });
  }

  createCustomForm() {
    this.modalService.open(ManageBasicdataCustomformCreateComponent, { backdrop: 'static', keyboard: false }).result.then((result: ReturnInfoModel) => {
      if (!result) {
        return;
      }

      if (!result.State) {
        SweetError("", result.Message);
      } else {
        this.customFormList.push(result.DataObject);
      }
    });
  }

  editCustomForm(modelInfo) {
    let modal: NgbModalRef = this.modalService.open(ManageBasicdataCustomformEditComponent, { backdrop: 'static', keyboard: false });
    modal.componentInstance.SetForm(modelInfo);
    modal.result.then((result: ReturnInfoModel) => {
      if (!result) return;

      if (!result.State) {
        SweetError("", result.Message);
      } else {
        for (let info of this.customFormList) {
          if (info.Id === result.DataObject.Id) {
            assign(info, result.DataObject);
            return;
          }
        }
      }
    });
  }



  removeCustomForm(Id: string) {
    SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.CustomformService.delete(Id, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else {
            remove(this.customFormList, function (item) {
              return item.Id === Id;
            });
          }
        });
      }
    });
  }


  listCategoryType() {
    this.CategoryService.listCategoryType((data: ReturnInfoModel) => {
      if (data.State) {
        this.categoryTypeList = data.DataObject;
      }
      else  
        SweetError('',data.Message);
    });
  }

  createCategoryType() {
    this.modalService.open(ManageBasicdataCategorytypeCreateComponent, { backdrop: 'static', keyboard: false }).result.then((result: ReturnInfoModel) => {
      if (!result) {
        return;
      }

      if (!result.State) {
        SweetError("", result.Message);
      } else {
        this.categoryTypeList.push(result.DataObject);
      }
    });
  }

  editCategoryType(modelInfo) {
    let modal: NgbModalRef = this.modalService.open(ManageBasicdataCategorytypeEditComponent, { backdrop: 'static', keyboard: false });
    modal.componentInstance.SetForm(modelInfo);
    modal.result.then((result: ReturnInfoModel) => {
      if (!result) return;

      if (!result.State) {
        SweetError("", result.Message);
      } else {
        for (let info of this.categoryTypeList) {
          if (info.Id === result.DataObject.Id) {
            assign(info, result.DataObject);
            return;
          }
        }
      }
    });
  }



  removeCategoryType(Id: string) {
    SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.CategoryService.deleteCategoryType(Id, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else {
            remove(this.categoryTypeList, function (item) {
              return item.Id === Id;
            });
          }
        });
      }
    });
  }
}
