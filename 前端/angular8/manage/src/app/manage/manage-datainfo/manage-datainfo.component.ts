import { CustomformService } from './../../services/customform.service';

import { DisplayUserExtendModel } from 'src/app/models/PassportLoginModel';
import { PassportService } from './../../services/passport.service';
import { CategoryService } from './../../services/category.service';
import { remove, assign } from 'lodash';
import { Component, OnInit } from '@angular/core';
import { SweetConfirm, SweetError, SweetSuccess } from 'src/app/common/common';
import { ReturnInfoModel, ReturnPagingModel, PagingModel, ModulePageModel } from 'src/app/models/ReturnInfoModel';
import { DataInfoService } from 'src/app/services/datainfo.service';
import { NgbModalRef } from 'src/app/common/modal/modal-ref';
import { NgbModal } from 'src/app/common/modal/modal';
import { ActivatedRoute } from '@angular/router';
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

@Component({
  selector: 'app-manage-datainfo',
  templateUrl: './manage-datainfo.component.html',
  styleUrls: ['./manage-datainfo.component.css']
})
export class ManageDatainfoComponent extends BaseForm implements OnInit {

  public DataInfoPaging: PagingModel<DisplayDataInfoModel> = new PagingModel<DisplayDataInfoModel>(new ModulePageModel(), []);
  public DataInfoPaginationModel: PaginationModel = new PaginationModel();
  public number: string = "";
  public xm: string = "";
  public sfzh: string = ""
  public DataInfoStatus = "";
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
  public ranges: any = {
    '今天': [moment(), moment()],
    '最近7天': [moment().subtract(7, 'days'), moment()],
    '最近30天': [moment().subtract(30, 'days'), moment()],
    '这个月': [moment().startOf('month'), moment().endOf('month')],
    '上个月': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
  };
  public fzranges: any = {
    '今天': [moment(), moment()],
    '最近7天': [moment().subtract(6, 'days'), moment()],
    '这个月': [moment().startOf('month'), moment().endOf('month')],
    '今年': [moment().startOf('year'), moment().endOf('year')],
    // '去年': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')],
    '全部': [moment().subtract(19, 'year').startOf('year'), moment()]
  };

  public customFormKeycode: string = "Contract";
  public DataInfoName = "合同";

  constructor(public DataInfoService: DataInfoService,
    public CategoryService: CategoryService,
    public modalService: NgbModal,
    public routeInfo: ActivatedRoute,
    public PassportService: PassportService,
    public CustomformService: CustomformService,
    public fb: FormBuilder) {
    super();

    // this.number,
    //   this.xm,
    //   this.sfzh,
    //   this.DataInfoStatus,
    //   this.categoryId,
    //   this.organId,
    //   this.begin_date.format("YYYY-MM-DD"),
    //   this.end_date.format("YYYY-MM-DD"),

    this.formModel = this.fb.group({
      // Number: [''],
      // Sfzh: [''],
      // DataInfoStatus: [''],
      // CategoryId:[''],
      // OrganId:[''],
      CustomFormKeycode:[this.customFormKeycode],
      Name: [''],
      CreateDate_Begin: [''],
      CreateDate_End: ['']
      // Fzrq_Begin:[''],
      // Fzrq_End:[''],
    });
  }

  ngOnInit() {
    this.PassportService.userExtendInfo((data: ReturnInfoModel) => {
      if (data.State) {
        this.userExtendModel = data.DataObject;
        // this.organId = data.DataObject.Organ_Id;
        // this.organType = data.DataObject.OrganType;

        // if (this.organType != "总部" && this.organId != '') 
        //   this.formModel.get("OrganId").setValue(this.organId);
        // this.loadOrganUsers();

        setTimeout(() => {
          console.log($('.select-search').length);
          let _this = this;
          $('.select-search').select2().on("change", function (e) {
            console.log(e);
            let formControl = _this.formModel.get(e.target.name) as FormControl;
            formControl.setValue(e.target.value);
          });
        }, 500);

        this.queryDataInfoCategories();
        this.loadDataInfoPaging();
      }
      else
        SweetError('', data.Message);
    });


    // this.loadOrganList();
  }

  /**
   * 查询数据类型分类
   */
  // loadOrganList(){
  //   this.OrganService.pagingOrgan('','1',[],1,10000,(data:ReturnInfoModel)=>{
  //     if(data.State){
  //       this.organList = data.DataObject.PageListInfos;
  //     }
  //   });
  // }  

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

  // onFzDaterangepickerChange($event) {
  //   this.fzrq_begin = $event.startDate || this.fzrq_begin;
  //   this.fzrq_end = $event.endDate || this.fzrq_end;

  //   this.formModel.get("Fzrq_Begin").setValue(this.fzrq_begin.format("YYYY-MM-DD"));
  //   this.formModel.get("Fzrq_End").setValue(this.fzrq_end.format("YYYY-MM-DD"));
  // }

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

  /**
   * 查询数据类型分类
   */
  queryDataInfoCategories() {
    // this.CategoryService.rootCategoryListByType(this.DataInfoCategoryTypeKey, (data: ReturnInfoModel) => {
    //   if (data.State) {
    //     this.DataInfoCategories = data.DataObject;
    //     if (this.organType != "总部" && this.organId != '') {
    //       this.DataInfoCategories =
    //         this.DataInfoCategories.filter(t => this.userExtendModel.CategoryIds.indexOf(t.Id) != -1);
    //     }
    //   }
    //   else  
    //     SweetError('',data.Message);
    // });
  }

  updateDataInfoStatusSelect(DataInfoStatus, ev) {
    this.DataInfoStatus = DataInfoStatus;

    this.formModel.get("DataInfoStatus").setValue(this.DataInfoStatus);
  }

  updateCategorySelect(item, ev) {
    this.categoryId = item;

    this.formModel.get("CategoryId").setValue(this.categoryId);
  }

  updateSelect(item, ev) {
    if (item.DataInfoStatus !== 0) {
      return;
    }
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

  onSearch() {
    this.loadDataInfoPaging();
  }

  loadDataInfoPaging(PageNum: number = 1, PageSize: number = 10) {
    this.DataInfoService.pagingDataInfo(
      this.formModel.value,
      PageNum,
      PageSize,
      (data: ReturnPagingModel<DisplayDataInfoModel>) => {
        if (data.State) {
          this.DataInfoPaging = data.DataObject;
          this.DataInfoPaginationModel = new PaginationModel(
            data.DataObject.Module_Page.AllCount,
            data.DataObject.Module_Page.PageNum,
            data.DataObject.Module_Page.PageCount);

          this.DataInfoPaging.PageListInfos.forEach(t => {
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

  onDataInfoPageChanged($event) {
    this.loadDataInfoPaging($event.page);
  }

  // importDataInfos() {
  //   let modal: NgbModalRef = this.modalService.open(ManageDataInfoImportComponent, { backdrop: 'static', keyboard: true });
  //   modal.componentInstance.SetForm(this.userExtendModel);
  //   modal.result.then((result: ReturnInfoModel) => {
  //     if (!result) {
  //       return;
  //     }

  //     if (!result.State) {
  //       SweetError("", result.Message);
  //     } else {
  //       this.loadDataInfoPaging();
  //     }
  //   });
  // }

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

  // generateZSBH() {
  //   if (this.categoryId != "") {
  //     SweetConfirm("确定统一生成数据编号吗？", "只有状态='审核通过'的数据才生成数据编号", (isConfirm: boolean) => {
  //       if (isConfirm) {
  //         this.DataInfoService.generateZSBH(
  //           this.categoryId,
  //           (data: ReturnInfoModel) => {
  //             if (data.State) {
  //               SweetSuccess("", "生成数据编号完成！");
  //               this.loadDataInfoPaging();
  //             }
  //             else
  //               SweetError('', data.Message);
  //           }
  //         )
  //       }
  //     });
  //   }
  //   else
  //     SweetError('', "请选择数据分类!");
  // }

  createDataInfo() {
    this.CustomformService.singleByKeycode(this.customFormKeycode, (data: ReturnInfoModel) => {
      if (data.State) {
        let modal: NgbModalRef = this.modalService.open(ManageDatainfoCreateComponent, { backdrop: 'static', keyboard: true });
        modal.componentInstance.SetForm(this.DataInfoName, this.customFormKeycode, data.DataObject.Id, null);
        modal.result.then((result: ReturnInfoModel) => {
          if (!result) {
            return;
          }

          if (!result.State) {
            SweetError("", result.Message);
          } else {
            this.loadDataInfoPaging();
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
            this.loadDataInfoPaging();
        });
      }
    });
  }

  removeDataInfos() {
    if (this.selectDataInfoList.length > 0) {
      SweetConfirm("删除", "确定按条件删除所有'待审核'数据吗？", (isConfirm: boolean) => {
        if (isConfirm) {
          this.DataInfoService.deleteDataInfos(this.selectDataInfoList.map(t => t.Id).join(","), (returnInfo: ReturnInfoModel) => {
            if (!returnInfo.State) {
              setTimeout(() => {
                SweetError('', returnInfo.Message);
              }, 200);
            }
            else
              this.loadDataInfoPaging();
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
    SweetConfirm("删除", "确定按条件删除所有'待审核'数据吗？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.DataInfoService.deleteDataInfosByCondition(this.formModel.value, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
          }
          else
            this.loadDataInfoPaging();
        });
      }
    });

  }

  // setStatusDataInfos(){
  //   let modal: NgbModalRef = this.modalService.open(ManageDataInfoSetstatusComponent, { backdrop: 'static', keyboard: true });
  //   modal.componentInstance.SetForm(this.selectDataInfoList.map(t => t.Id),this.DataInfoPaging.PageListInfos.map(t => t.Id),this.formModel.value);
  //   modal.result.then((result: ReturnInfoModel) => {
  //     if (!result) {
  //       return;
  //     }

  //     if (!result.State) {
  //       SweetError("", result.Message);
  //     } else {
  //       this.loadDataInfoPaging();
  //     }
  //   });
  // }

  // auditDataInfos() {
  //   if (this.selectDataInfoList.length > 0) {
  //     SweetConfirm("审核数据？", "", (isConfirm: boolean) => {
  //       if (isConfirm) {
  //         this.DataInfoService.auditDataInfos(this.selectDataInfoList.map(t => t.Id).join(","), (returnInfo: ReturnInfoModel) => {
  //           if (!returnInfo.State) {
  //             setTimeout(() => {
  //               SweetError('', returnInfo.Message);
  //             }, 200);
  //           }
  //           else
  //             this.loadDataInfoPaging();
  //         });
  //       }
  //     });
  //   }
  //   else
  //     SweetError("", "请选择数据！");
  // }


  // cancelAuditDataInfos() {
  //   if (this.selectDataInfoList.length > 0) {
  //     SweetConfirm("取消审核数据？", "", (isConfirm: boolean) => {
  //       if (isConfirm) {
  //         this.DataInfoService.cancelAuditDataInfos(this.selectDataInfoList.map(t => t.Id).join(","), (returnInfo: ReturnInfoModel) => {
  //           if (!returnInfo.State) {
  //             setTimeout(() => {
  //               SweetError('', returnInfo.Message);
  //             }, 200);
  //           }
  //           else
  //             this.loadDataInfoPaging();
  //         });
  //       }
  //     });
  //   }
  //   else
  //     SweetError("", "请选择数据！");
  // }


  editDataInfo(modelInfo) {
    // let modal: NgbModalRef = this.modalService.open(ManageDatainfoEditComponent, { backdrop: 'static', keyboard: true });
    // modal.componentInstance.SetForm(this.DataInfoName, modelInfo, this.userExtendModel);
    // modal.result.then((result: ReturnInfoModel) => {
    //   if (!result) return;

    //   if (!result.State) {
    //     SweetError("", result.Message);
    //   } else {
    //     this.loadDataInfoPaging();
    //   }
    // });


    this.CustomformService.singleByKeycode(this.customFormKeycode, (data: ReturnInfoModel) => {
      if (data.State) {
        // let modal: NgbModalRef = this.modalService.open(ManageDatainfoCreateComponent, { backdrop: 'static', keyboard: true });
        // modal.componentInstance.SetForm(this.DataInfoName,this.customFormKeycode,data.DataObject.Id,null);
        // modal.result.then((result: ReturnInfoModel) => {
        //   if (!result) {
        //     return;
        //   }

        //   if (!result.State) {
        //     SweetError("", result.Message);
        //   } else {
        //     this.loadDataInfoPaging();
        //   }
        // });

        let modal: NgbModalRef = this.modalService.open(ManageDatainfoEditComponent, { backdrop: 'static', keyboard: true });
        modal.componentInstance.SetForm(this.DataInfoName, modelInfo, data.DataObject.Id);
        modal.result.then((result: ReturnInfoModel) => {
          if (!result) return;

          if (!result.State) {
            SweetError("", result.Message);
          } else {
            this.loadDataInfoPaging();
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

  // showDataInfoWorkFlow(model: DisplayDataInfoModel) {
  //   let modal: NgbModalRef = this.modalService.open(ManageDataInfoWorkflowlogComponent, { backdrop: 'static', keyboard: true });
  //   modal.componentInstance.SetForm(model);
  //   modal.result.then((result: ReturnInfoModel) => {
  //     if (!result) {
  //       return;
  //     }

  //     // if (!result.State) {
  //     //   SweetError("", result.Message);
  //     // } else {
  //     //   this.loadDataInfoPaging();
  //     // }
  //   });
  // }
}
