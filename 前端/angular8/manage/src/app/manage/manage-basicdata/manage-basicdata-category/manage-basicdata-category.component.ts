import { ManageBasicdataCategoryImportComponent } from './manage-basicdata-category-import/manage-basicdata-category-import.component';
import { assign, remove } from 'lodash';
import { ManageBasicdataCategoryEditComponent } from './manage-basicdata-category-edit/manage-basicdata-category-edit.component';
import { ManageBasicdataCategoryCreateComponent } from './manage-basicdata-category-create/manage-basicdata-category-create.component';
import { CategoryService } from './../../../services/category.service';
import { Component, OnInit } from '@angular/core';
import { DisplayCategoryModel } from 'src/app/models/CategoryModels';
import { ActivatedRoute, Params } from '@angular/router';
import { NgbModal } from 'src/app/common/modal/modal';
import { NgbModalRef } from 'src/app/common/modal/modal-ref';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { SweetError, SweetConfirm } from 'src/app/common/common';
import { Location } from '@angular/common';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
@Component({
  selector: 'app-manage-basicdata-category',
  templateUrl: './manage-basicdata-category.component.html',
  styleUrls: ['./manage-basicdata-category.component.css']
})
export class ManageBasicdataCategoryComponent implements OnInit {


  cagegoryTypeId: string = "";
  parentCategoryId: string = "";
  customFormId: string = "";
  categoryList: Array<DisplayCategoryModel> = [];
  ramusCategoryList: Array<DisplayCategoryModel> = [];
  selectCategoryList: Array<DisplayCategoryModel> = [];
  isSelectAll: boolean = false;
  categoryTypeName = "";

  constructor(private routeInfo: ActivatedRoute,
    private CategoryService: CategoryService,
    private modalService: NgbModal,
    private location: Location) {
    this.routeInfo.params.subscribe((p: Params) => {
      this.cagegoryTypeId = p['id'];
      this.parentCategoryId = p['parentCategoryId'];
      this.customFormId = p['customFormId'];
      this.getCustomFormId(this.cagegoryTypeId);
      this.loadCategoryList(this.cagegoryTypeId, this.parentCategoryId);
      this.getRamusCategorys(this.parentCategoryId);
    });
  }

  ngOnInit() {
  }

  drop(event: CdkDragDrop<string[]>) {
    // debugger;
    // console.log("pre:"+this.customFormItemList[event.previousIndex].Id);
    // console.log("cur:"+this.customFormItemList[event.currentIndex].Id);
    // console.log(event);
    moveItemInArray(this.categoryList, event.previousIndex, event.currentIndex);
    this.CategoryService.SaveCategoryOrder(this.cagegoryTypeId,
      this.parentCategoryId,
      this.categoryList.map(t => t.Id).join(","),
      (data: ReturnInfoModel) => {
        if (!data.State)
          SweetError('', data.Message);
      });
  }

  updateAllSelect(ev) {
    this.selectAll(ev.target.checked);
  }

  selectAll(isSelectAll) {
    this.isSelectAll = isSelectAll;
    if (isSelectAll)
      this.selectCategoryList = this.categoryList;
    else
      this.selectCategoryList = [];
  }

  updateSelect(item, ev) {
    debugger;
    if (ev.target.checked) {
      console.log(item);
      if (this.selectCategoryList.indexOf(item) === -1) {
        this.selectCategoryList.push(item);
      }
    } else {
      if (this.selectCategoryList.indexOf(item) !== -1) {
        this.selectCategoryList.splice(this.selectCategoryList.indexOf(item), 1);
      }
    }
  }

  goBack(): void {
    this.location.back();
  }

  getRamusCategorys(parentCategoryId) {
    this.CategoryService.ramusCategorys(parentCategoryId, (data: ReturnInfoModel) => {
      if (data.State) {
        this.ramusCategoryList = data.DataObject;
      }
      else
        SweetError('', data.Message);
    });
  }


  getCustomFormId(categoryTypeId) {
    this.CategoryService.singleCategoryType(categoryTypeId, (data: ReturnInfoModel) => {
      if (data.State) {
        this.customFormId = data.DataObject.CustomFormId;
        this.categoryTypeName = data.DataObject.Name;
      }
      else
        SweetError('', data.Message);
    });
  }

  reload() {
    this.loadCategoryList(this.cagegoryTypeId, this.parentCategoryId);
  }

  loadCategoryList(cagegoryTypeId, parentCategoryId) {
    if (cagegoryTypeId != "") {
      this.CategoryService.listCategory(cagegoryTypeId, parentCategoryId, (data: ReturnInfoModel) => {
        if (data.State) {
          this.categoryList = data.DataObject;

          this.categoryList.forEach(t => {
            var arr = [];
            t.CustomFormSetting.CustomFormItemSettings.forEach(item => {
              arr.push(item.Name + ":" + item.Value);
            });
            t.CustomFormSettingDescription = arr.join("\n");
          });
        }
        else
          SweetError("", data.Message);
      });
    }
  }

  createCategory() {
    if (this.cagegoryTypeId != null) {

      // if(this.parentCategoryId !=null){
      //   this.CategoryService.getCategoryParentCustomFormId(this.cagegoryTypeId,this.parentCategoryId,(data:ReturnInfoModel) =>{
      //     alert(data.DataObject);
      //   });
      // }
      this.CategoryService.getCategoryParentCustomFormId(this.cagegoryTypeId, this.parentCategoryId, (data: ReturnInfoModel) => {
        if (data.State) {
          let modal: NgbModalRef = this.modalService.open(ManageBasicdataCategoryCreateComponent, { backdrop: 'static', keyboard: true });
          modal.componentInstance.SetForm(this.cagegoryTypeId, data.DataObject, this.parentCategoryId, this.categoryTypeName);
          modal.result.then((result: ReturnInfoModel) => {
            if (!result) {
              return;
            }

            if (!result.State) {
              SweetError("", result.Message);
            } else {
              this.categoryList.push(result.DataObject);
            }
          });
        }
        else
          SweetError('', data.Message);
      });

    }
    else
      SweetError("", "请选择分类类型！");
  }

  editCategory(modelInfo) {
    let modal: NgbModalRef = this.modalService.open(ManageBasicdataCategoryEditComponent, { backdrop: 'static', keyboard: true });
    modal.componentInstance.SetForm(modelInfo, this.customFormId, this.categoryTypeName);
    modal.result.then((result: ReturnInfoModel) => {
      if (!result) return;

      if (!result.State) {
        SweetError("", result.Message);
      } else {
        for (let info of this.categoryList) {
          if (info.Id === result.DataObject.Id) {
            assign(info, result.DataObject);
            return;
          }
        }
      }
    });
  }

  removeCategory(Id: string) {
    SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.CategoryService.deleteCategory(Id, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else {
            remove(this.categoryList, function (item) {
              return item.Id === Id;
            });
          }
        });
      }
    });
  }


  removeCategorys() {
    if (this.selectCategoryList.length > 0) {
      SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
        if (isConfirm) {
          this.CategoryService.deleteCategorys(this.selectCategoryList.map(t => t.Id).join(","), (returnInfo: ReturnInfoModel) => {
            if (!returnInfo.State) {
              setTimeout(() => {
                SweetError('', returnInfo.Message);
              }, 200);
            }
            else
              this.reload();
          });
        }
      });
    }
    else
      SweetError("", `请选择{{this.categoryTypeName}}数据！`);
  }

  importCategorys() {
    let modal: NgbModalRef = this.modalService.open(ManageBasicdataCategoryImportComponent, { backdrop: 'static', keyboard: true });
    modal.componentInstance.SetForm(this.cagegoryTypeId, this.parentCategoryId, this.categoryTypeName);
    modal.result.then((result: ReturnInfoModel) => {
      if (!result) {
        return;
      }

      if (!result.State) {
        SweetError("", result.Message);
      } else {
        this.reload();
      }
    });
  }

  exportCategorys() {
    this.CategoryService.exportCategorys(this.cagegoryTypeId, this.parentCategoryId, (data: ReturnInfoModel) => {
      let categoryTypeName:string = this.categoryTypeName;
      if (data.State) {
        SweetConfirm("导出成功！", "点击确定按钮下载" + categoryTypeName + "数据", (callback: Boolean) => {
          if (callback)
            window.open(data.Message);
        });
      }
      else
        SweetError('', data.Message);
    });
  }
}
