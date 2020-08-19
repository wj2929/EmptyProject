import { CustomFormItemSetting } from './../../models/CustomFormModels';
import { DisplayLogModel } from 'src/app/models/LogModels';
import { CategoryService } from './../../services/category.service';
import { remove, assign } from 'lodash';
import { Component, OnInit } from '@angular/core';
import { ReturnInfoModel, ReturnPagingModel, PagingModel, ModulePageModel } from 'src/app/models/ReturnInfoModel';
import { LogService } from 'src/app/services/log.service';
import { NgbModalRef } from 'src/app/common/modal/modal-ref';
import { NgbModal } from 'src/app/common/modal/modal';
import { ActivatedRoute } from '@angular/router';
import { PaginationModel } from 'src/app/common/pagination/PaginationModels';
import { DisplayCategoryModel } from 'src/app/models/CategoryModels';
import { SweetError, SweetConfirm, SweetSuccess, ShowBlockUI, HideBlockUI } from 'src/app/common/common';
import * as moment from 'moment';
import * as $ from "jquery";
@Component({
  selector: 'app-manage-log',
  templateUrl: './manage-log.component.html',
  styleUrls: ['./manage-log.component.css']
})
export class ManageLogComponent implements OnInit {

  public logPaging: PagingModel<DisplayLogModel> = new PagingModel<DisplayLogModel>(new ModulePageModel(), []);
  public logPaginationModel: PaginationModel = new PaginationModel();
  public userName: string = "";
  public content: string = "";
  public ip: string = "";
  public logType = "0";
  // private begin_date="";
  // private end_date="";
  public categoryIds = [];
  public selectLogList: Array<DisplayLogModel> = [];
  public certificateCategories: Array<DisplayCategoryModel> = [];
  public isSelectAll: boolean = false;

  begin_date: any = moment().subtract(30, 'days');
  end_date: any = moment();
  selectdate: any = { startDate: moment().subtract(30, 'days'), endDate: moment };
  ranges: any = {
    '今天': [moment(), moment()],
    '最近7天': [moment().subtract(7, 'days'), moment()],
    '最近30天': [moment().subtract(30, 'days'), moment()],
    '这个月': [moment().startOf('month'), moment().endOf('month')],
    '上个月': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
  }
  locale: any = {
    format: "YYYY-MM-DD",
    separator: " - ",
    daysOfWeek: ["日","一","二","三","四","五","六"],
    monthNames: ["一月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","十二月"],
    applyLabel: '确定',
    cancelLabel: '取消'
  };
  constructor(private LogService: LogService,
    private CategoryService: CategoryService,
    private modalService: NgbModal,
    private routeInfo: ActivatedRoute) { }

  ngOnInit() {
    this.loadLogPaging();
  }

  changeDaterangepickerWidth() {
    setTimeout(() => {
      $(".md-drppicker").width("600px");
    }, 50);
  }

  onDaterangepickerChange($event) {
    this.begin_date = $event.startDate || this.begin_date;
    this.end_date = $event.endDate || this.end_date;
  }

  updateAllSelect(ev) {
    this.selectAll(ev.target.checked);
  }

  selectAll(isSelectAll) {
    this.isSelectAll = isSelectAll;
    if (isSelectAll)
      this.selectLogList = this.logPaging.PageListInfos;
    else
      this.selectLogList = [];
  }

  updateLogTypeSelect(logType, ev) {
    this.logType = logType;
  }

  updateSelect(item, ev) {
    if (ev.target.checked) {
      console.log(item);
      if (this.selectLogList.indexOf(item) === -1) {
        this.selectLogList.push(item);
      }
    } else {
      if (this.selectLogList.indexOf(item) !== -1) {
        this.selectLogList.splice(this.selectLogList.indexOf(item), 1);
      }
    }
  }

  onSearch() {
    this.loadLogPaging();
  }

  loadLogPaging(PageNum: number = 1, PageSize: number = 10) {
    this.LogService.pagingLog(this.userName, this.logType, this.ip, this.content, this.begin_date.format("YYYY-MM-DD"), this.end_date.format("YYYY-MM-DD"), PageNum, PageSize, (data: ReturnPagingModel<DisplayLogModel>) => {
      if (data.State) {
        this.logPaging = data.DataObject;
        this.logPaging.PageListInfos.forEach(t =>{
          t.JsonObj = {};
          t.CustomFormSetting.CustomFormItemSettings.forEach((CustomFormItemSetting:CustomFormItemSetting) =>{
            t.JsonObj[CustomFormItemSetting.Key] = CustomFormItemSetting.Value;
          });
        });
        this.logPaginationModel = new PaginationModel(
          data.DataObject.Module_Page.AllCount,
          data.DataObject.Module_Page.PageNum,
          data.DataObject.Module_Page.PageCount);
      }
      else
        SweetError("", data.Message);
    });
  }

  onLogPageChanged($event) {
    this.loadLogPaging($event.page);
  }

  removeLog(Id: string) {
    SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
      if (isConfirm) {
        this.LogService.deleteLog(Id, (returnInfo: ReturnInfoModel) => {
          if (!returnInfo.State) {
            setTimeout(() => {
              SweetError('', returnInfo.Message);
            }, 200);
            return;
          }
          this.loadLogPaging();
        });
      }
    });
  }

  removeLogs() {
    if (this.selectLogList.length > 0) {
      SweetConfirm("删除", "此操作不可逆，是否继续？", (isConfirm: boolean) => {
        if (isConfirm) {
          this.LogService.deleteLogs(this.selectLogList.map(t => t.Id).join(","), (returnInfo: ReturnInfoModel) => {
            if (!returnInfo.State) {
              setTimeout(() => {
                SweetError('', returnInfo.Message);
              }, 200);
              return;
            }
            this.loadLogPaging();
          });
        }
      });
    }
    else
      SweetError("", "请选择日志！");
  }

  exportLogs() {
    ShowBlockUI("LogManage");
    this.LogService.exportLogs(this.userName, this.logType, this.ip, this.content, this.begin_date.format("YYYY-MM-DD"), this.end_date.format("YYYY-MM-DD"), (returnInfo: ReturnInfoModel) => {
      HideBlockUI("LogManage");
      if (!returnInfo.State) {
        SweetError('',returnInfo.Message);
        return;
      }
      SweetConfirm("导出成功！","点击确定按钮下载日志",(callback:Boolean) =>{
        if(callback)
          window.open(returnInfo.Message);
      });
    });
  }

}
