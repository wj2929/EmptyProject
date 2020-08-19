import { Injectable } from '@angular/core';
import { CommonService } from './common.service';
import { SelhttpService } from './selhttp.service';
import { ReturnInfoModel, BatchProgressInfo } from '../models/ReturnInfoModel';

@Injectable({
  providedIn: 'root'
})
export class DataInfoService {

  constructor(public common: CommonService,
    public SelhttpService: SelhttpService) { }

  /**
   * 创建数据
   * @param data
   * @param cb 
   */
  createDataInfo(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoCreate", data, cb);
  }


  /**
   * 编辑数据
   * @param data
   * @param cb 
   */
  editDataInfo(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoEdit", data, cb);
  }

  /**
   * 数据列表分页
   * @param data
   * @param cb 
   */
  pagingDataInfo(data?: any, pageNum: number = 1, pageSize: number = 10, cb?: Function) {
    if (data) {
      data.PageNum = pageNum;
      data.PageSize = pageSize;
    }
    this.SelhttpService.get<ReturnInfoModel>("/DataInfo/DataInfoPaging", data, cb);
  }

  /**
   * 删除数据
   * @param data
   * @param cb 
   */
  deleteDataInfo(Id, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoDelete", { Id: Id }, cb);
  }

  /**
   * 删除数据
   * @param data
   * @param cb 
   */
  deleteDataInfos(Ids, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoDeletes", { Ids: Ids }, cb);
  }

  /**
   * 按搜索条件删除数据
   * @param data 
   * @param cb 
   */
  deleteDataInfosByCondition(data?: any, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DeleteDataInfosByCondition", data, cb);
  }


  // /**
  //  * 审核数据
  //  * @param data
  //  * @param cb 
  //  */
  // auditDataInfos(Ids, cb?: Function) {
  //   this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoAuidts", { Ids: Ids }, cb);
  // }


  /**
   * 导出数据
   * @param data
   * @param cb 
   */
  exportDataInfos(data?: any, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/DataInfo/ExportDataInfos", data, cb);
  }

  /**
   * 获取数据分支导航
   * @param data
   * @param cb 
   */
  ramusDataInfos(parentid,cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/DataInfo/DataInfoRamus", {ParentCategory_Id:parentid}, cb);
  }


  

  // /**
  //  * 统一生成数据编号
  //  * @param categoryId 
  //  * @param cb 
  //  */
  // generateZSBH(categoryId: string = "", cb?: Function) {
  //   this.SelhttpService.post<ReturnInfoModel>("/DataInfo/GenerateZSBH",
  //     { CategoryId: categoryId }, cb);
  // }

  // /**
  //  * 取消数据
  //  * @param data
  //  * @param cb 
  //  */
  // cancelAuditDataInfos(Ids, cb?: Function) {
  //   this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoCancelAuidts", { Ids: Ids }, cb);
  // }

  // /**
  //  * 
  //  * @param data 
  //  * @param cb 
  //  */
  // prepareSetStatus(data?: Object, cb?: Function) {
  //   this.SelhttpService.post<BatchProgressInfo>("/DataInfo/DataInfoPrepareSetStatus", data, cb);
  // }

  // /**
  //  * 分批批量设置状态
  //  * @param data 
  //  * @param cb 
  //  */
  // batchSetStatus(DataInfoStatus, memo, currentBatch: number = 1, batchSize: number = 10, cb?: Function) {
  //   this.SelhttpService.post<BatchProgressInfo>("/DataInfo/DataInfoBatchSetStatus", { DataInfoStatus: DataInfoStatus, Memo: memo, CurrentBatch: currentBatch, BatchSize: batchSize }, cb);
  // }

  // /**
  //  * 批量设置状态
  //  * @param Ids 
  //  * @param DataInfoStatus 
  //  * @param cb 
  //  */
  // setStatus(Ids: Array<string> = [], DataInfoStatus, memo, cb?: Function) {
  //   this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoSetStatus", { Ids: Ids.join(","), DataInfoStatus: DataInfoStatus, Memo: memo }, cb);
  // }

  // /**
  //  * 创建数据流转记录
  //  * @param data
  //  * @param cb 
  //  */
  // createDataInfoWorkFlowLog(data?: Object, cb?: Function) {
  //   this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoWorkFlowLogCreate", data, cb);
  // }


  // /**
  //  * 编辑数据流转记录
  //  * @param data
  //  * @param cb 
  //  */
  // editDataInfoWorkFlowLog(data?: Object, cb?: Function) {
  //   this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoWorkFlowLogEdit", data, cb);
  // }

  // /**
  //  * 数据流转记录列表分页
  //  * @param data
  //  * @param cb 
  //  */
  // pagingDataInfoWorkFlowLog(DataInfoId, pageNum: number = 1, pageSize: number = 10, cb?: Function) {
  //   this.SelhttpService.get<ReturnInfoModel>("/DataInfo/DataInfoWorkFlowLogPaging",
  //     { DataInfoId: DataInfoId, PageNum: pageNum, PageSize: pageSize },
  //     cb);
  // }

  // /**
  //  * 删除数据流转记录
  //  * @param data
  //  * @param cb 
  //  */
  // deleteDataInfoWorkFlowLog(Id, cb?: Function) {
  //   this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoWorkFlowLogDelete", { Id: Id }, cb);
  // }

  // /**
  //  * 删除数据流转记录
  //  * @param data
  //  * @param cb 
  //  */
  // deleteDataInfoWorkFlowLogs(Ids, cb?: Function) {
  //   this.SelhttpService.post<ReturnInfoModel>("/DataInfo/DataInfoWorkFlowLogDeletes", { Ids: Ids }, cb);
  // }

  /**
   * 
   * @param data 
   * @param cb 
   */
  overAllStat(data?: any, cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/DataInfo/OverAllStat", data, cb);
  }

/**
 * 
 * @param data 
 * @param cb 
 */
  dailyStat(data?: any, cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/DataInfo/DailyStat", data, cb);
  }


/**
 * 
 * @param data 
 * @param cb 
 */
  createStat(cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/DataInfo/CreateStat", null, cb);
  }
}
