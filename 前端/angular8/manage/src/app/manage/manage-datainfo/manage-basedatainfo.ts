import { CustomformService } from '../../services/customform.service';

import { DisplayUserExtendModel } from 'src/app/models/PassportLoginModel';
import { PassportService } from '../../services/passport.service';
import { CategoryService } from '../../services/category.service';
import { remove, assign } from 'lodash';
import { Component, OnInit } from '@angular/core';
import { SweetConfirm, SweetError, SweetSuccess } from 'src/app/common/common';
import { ReturnInfoModel, ReturnPagingModel, PagingModel, ModulePageModel } from 'src/app/models/ReturnInfoModel';
import { DataInfoService } from 'src/app/services/datainfo.service';
import { NgbModalRef } from 'src/app/common/modal/modal-ref';
import { NgbModal } from 'src/app/common/modal/modal';
import { ActivatedRoute, Params } from '@angular/router';
import { DisplayDataInfoModel } from 'src/app/models/DataInfoModels';
import { PaginationModel } from 'src/app/common/pagination/PaginationModels';
import * as moment from 'moment';
declare var $: any;
import { DisplayCategoryModel } from 'src/app/models/CategoryModels';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { FormBuilder, FormControl } from '@angular/forms';
import { DisplayOrganModel } from 'src/app/models/OrganModels';
import { ManageDatainfoCreateComponent } from './manage-datainfo-create/manage-datainfo-create.component';
import { ManageDatainfoEditComponent } from './manage-datainfo-edit/manage-datainfo-edit.component';

// @Component({
//   selector: 'app-manage-datainfo',
//   templateUrl: './manage-datainfo.component.html',
//   styleUrls: ['./manage-datainfo.component.css']
// })
export abstract class ManageBaseDataInfo extends BaseForm {

  public DataInfoPaging: PagingModel<DisplayDataInfoModel> = new PagingModel<DisplayDataInfoModel>(new ModulePageModel(), []);
  public DataInfoPaginationModel: PaginationModel = new PaginationModel();
  public number: string = "";
  public xm: string = "";
  public sfzh: string = ""

  public categoryId = "";
  public selectDataInfoList: Array<DisplayDataInfoModel> = [];
  public DataInfoCategories: Array<DisplayCategoryModel> = [];
  public isSelectAll: boolean = false;
  public DataInfoCategoryTypeKey: string = "DataInfoType";
  // organId = "";
  // organType = "分部";
  public userExtendModel: DisplayUserExtendModel;
  public organList: Array<DisplayOrganModel> = [];
  public createdate_begin: any = moment().subtract(30, 'days');
  public createDate_end: any = moment();
  public fzrq_begin: any = moment().subtract(30, 'days');
  public fzrq_end: any = moment();
  public selectdate: any = { startDate: moment().subtract(30, 'days'), endDate: moment };

  public customFormKeycode: string = "Contract";
  public DataInfoName = "合同";
  public PageSize:number = 10;
  public ParentDataInfo_Id:string="";
  ramusDataInfoList: Array<DisplayDataInfoModel> = [];

  public ranges: any = {
    '今天': [moment(), moment()],
    '最近7天': [moment().subtract(7, 'days'), moment()],
    '最近30天': [moment().subtract(30, 'days'), moment()],
    '这个月': [moment().startOf('month'), moment().endOf('month')],
    '上个月': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
  };

  constructor(public DataInfoService: DataInfoService,
    public CategoryService: CategoryService,
    public modalService: NgbModal,
    public routeInfo: ActivatedRoute,
    public PassportService: PassportService,
    public CustomformService: CustomformService,
    public fb: FormBuilder) {
    super();

    this.routeInfo.params.subscribe((p: Params) => {
      this.ParentDataInfo_Id = p['id'];

      // this.formModel.get("ParentDataInfo_Id").setValue(this.ParentDataInfo_Id);
      // this.loadDataInfoPaging(1,this.PageSize);
    });

  }

  ngOnInit() {
    this.formModel = this.fb.group({
      CustomFormKeycode:[this.customFormKeycode],
      ParentDataInfo_Id:[this.ParentDataInfo_Id],
      Name: [''],
      CreateDate_Begin: [''],
      CreateDate_End: ['']
    });

    this.PassportService.userExtendInfo((data: ReturnInfoModel) => {
      if (data.State) {
        this.userExtendModel = data.DataObject;
        
        setTimeout(() => {
          console.log($('.select-search').length);
          let _this = this;
          $('.select-search').select2().on("change", function (e) {
            console.log(e);
            let formControl = _this.formModel.get(e.target.name) as FormControl;
            formControl.setValue(e.target.value);
          });
        }, 500);

        this.loadDataInfoPaging(1,this.PageSize);
        this.getRamusDataInfos();
      }
      else
        SweetError('', data.Message);
    });

  }

  changeDaterangepickerWidth() {
    setTimeout(() => {
      $(".md-drppicker").width("600px");
    }, 50);
  }

  onLuRuDaterangepickerChange($event) {
    this.createdate_begin = $event.startDate || this.createdate_begin;
    this.createDate_end = $event.endDate || this.createDate_end;

    this.formModel.get("CreateDate_Begin").setValue(this.createdate_begin.format("YYYY-MM-DD"));
    this.formModel.get("CreateDate_End").setValue(this.createDate_end.format("YYYY-MM-DD"));
  }

  updateAllSelect(ev) {
    this.selectAll(ev.target.checked);
  }

  selectAll(isSelectAll) {
    this.isSelectAll = isSelectAll;
    if (isSelectAll)
      this.selectDataInfoList = this.DataInfoPaging.PageListInfos;//.filter(t=> t.DataInfoStatus==0);
    else
      this.selectDataInfoList = [];
  }

  updateCategorySelect(item, ev) {
    this.categoryId = item;

    this.formModel.get("CategoryId").setValue(this.categoryId);
  }

  updateSelect(item, ev) {
    if (ev.target.checked) {
      console.log(item);
      if (this.selectDataInfoList.indexOf(item) === -1) {
        this.selectDataInfoList.push(item);
      }
    } else {
      if (this.selectDataInfoList.indexOf(item) !== -1) {
        this.selectDataInfoList.splice(this.selectDataInfoList.indexOf(item), 1);
      }
    }
  }

  getRamusDataInfos() {
    this.DataInfoService.ramusDataInfos(this.ParentDataInfo_Id, (data: ReturnInfoModel) => {
      if (data.State) {
        this.ramusDataInfoList = data.DataObject;
      }
      else
        SweetError('', data.Message);
    });
  }

  onSearch() {
    this.loadDataInfoPaging(1,10);
  }

  abstract loadDataInfoPaging(PageNum: number , PageSize: number );
  // loadDataInfoPaging(PageNum: number = 1, PageSize: number = 10) {
  //   this.DataInfoService.pagingDataInfo(
  //     this.formModel.value,
  //     PageNum,
  //     PageSize,
  //     (data: ReturnPagingModel<DisplayDataInfoModel>) => {
  //       if (data.State) {
  //         this.DataInfoPaging = data.DataObject;
  //         this.DataInfoPaginationModel = new PaginationModel(
  //           data.DataObject.Module_Page.AllCount,
  //           data.DataObject.Module_Page.PageNum,
  //           data.DataObject.Module_Page.PageCount);

  //         this.DataInfoPaging.PageListInfos.forEach(t => {
  //           var arr = [];
  //           t.CustomFormSetting.CustomFormItemSettings.forEach(item => {
  //             arr.push(item.Name + ":" + item.Value);
  //           });
  //           t.CustomFormSettingDescription = arr.join("\n");
  //         });
  //       }
  //       else
  //         SweetError("", data.Message);
  //     });
  // }

  onDataInfoPageChanged($event) {
    this.loadDataInfoPaging($event.page,this.PageSize);
  }

  exportDataInfos() {
    if (this.categoryId != "") {
      this.DataInfoService.exportDataInfos(
        this.formModel.value,
        (data: ReturnInfoModel) => {
          if (data.State) {
            SweetConfirm("导出成功！", "点击确定按钮下载数据数据", (callback: Boolean) => {
              if (callback)
                window.open(data.Message);
            });
          }
          else
            SweetError('', data.Message);
        }
      )
    }
    else
      SweetError('', "请选择数据分类!");
  }

  createDataInfo() {
    this.CustomformService.singleByKeycode(this.customFormKeycode, (data: ReturnInfoModel) => {
      if (data.State) {
        let modal: NgbModalRef = this.modalService.open(ManageDatainfoCreateComponent, { backdrop: 'static', keyboard: true });
        modal.componentInstance.SetForm(this.DataInfoName, this.customFormKeycode, data.DataObject.Id, this.ParentDataInfo_Id);
        modal.result.then((result: ReturnInfoModel) => {
          if (!result) {
            return;
          }

          if (!result.State) {
            SweetError("", result.Message);
          } else {
            this.loadDataInfoPaging(1,this.PageSize);
          }
        });
      }
      else {
        if (data.Message != null)
          SweetError("", data.Message);
        else
          SweetError("", "指定表单Key不存在！");
      }
    });


  }

  removeDataInfo(Id: string) {
    SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.DataInfoService.deleteDataInfo(Id, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else
            this.loadDataInfoPaging(1,this.PageSize);
        });
      }
    });
  }

  removeDataInfos() {
    if (this.selectDataInfoList.length > 0) {
      SweetConfirm("删除", "确定按条件删除数据吗？", (isConfirm: boolean) => {
        if (isConfirm) {
          this.DataInfoService.deleteDataInfos(this.selectDataInfoList.map(t => t.Id).join(","), (returnInfo: ReturnInfoModel) => {
            if (!returnInfo.State) {
              setTimeout(() => {
                SweetError('', returnInfo.Message);
              }, 200);
            }
            else
              this.loadDataInfoPaging(1,this.PageSize);
          });
        }
      });
    }
    else
      SweetError("", "请选择数据！");
  }

  removeCurrentPageDataInfos() {
    // this.selectDataInfoList = this.DataInfoPaging.PageListInfos.filter(t=> t.DataInfoStatus==0);
    this.removeDataInfos();
  }

  removeAllPageDataInfos() {
    SweetConfirm("删除", "确定按条件删除所有数据吗？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.DataInfoService.deleteDataInfosByCondition(this.formModel.value, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else
            this.loadDataInfoPaging(1,this.PageSize);
        });
      }
    });

  }

  editDataInfo(modelInfo) {
    this.CustomformService.singleByKeycode(this.customFormKeycode, (data: ReturnInfoModel) => {
      if (data.State) {
        
        let modal: NgbModalRef = this.modalService.open(ManageDatainfoEditComponent, { backdrop: 'static', keyboard: true });
        modal.componentInstance.SetForm(this.DataInfoName, modelInfo, data.DataObject.Id);
        modal.result.then((result: ReturnInfoModel) => {
          if (!result) return;

          if (!result.State) {
            SweetError("", result.Message);
          } else {
            this.loadDataInfoPaging(1,this.PageSize);
          }
        });
      }
      else {
        if (data.Message != null)
          SweetError("", data.Message);
        else
          SweetError("", "指定表单Key不存在！");
      }
    });
  }
}
