import { Injectable } from '@angular/core';
import { SelhttpService } from './selhttp.service';
import { ReturnInfoModel } from '../models/ReturnInfoModel';

@Injectable({
  providedIn: 'root'
})
export class LogService {

  constructor(
    private SelhttpService: SelhttpService) { }

  /**
   * 日志列表分页
   * @param data
   * @param cb 
   */
  pagingLog(userName, logType, ip, content, begin_date, end_date, pageNum: number = 1, pageSize: number = 10, cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Log/LogPaging",
      { UserName: userName, LogType: logType, Ip: ip, Content: content, Begin_date: begin_date, End_date: end_date, PageNum: pageNum, PageSize: pageSize },
      cb);
  }

  /**
   * 导出日志
   * @param userName 
   * @param logType 
   * @param ip 
   * @param content 
   * @param begin_date 
   * @param end_date 
   * @param cb 
   */
  exportLogs(userName, logType, ip, content, begin_date, end_date, cb?: Function){
    this.SelhttpService.post<ReturnInfoModel>("/Log/LogExports",
      { UserName: userName, LogType: logType, Ip: ip, Content: content, Begin_date: begin_date, End_date: end_date},
      cb);
  }

  /**
   * 删除日志
   * @param data
   * @param cb 
   */
  deleteLog(Id, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Log/LogDelete", { Id: Id }, cb);
  }  


  /**
   * 删除日志
   * @param data
   * @param cb 
   */
  deleteLogs(Ids, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Log/LogDeletes", { Ids: Ids }, cb);
  }  
  
  /**
   * 获取最近登录记录
   */
  recentLoginRecord(cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Log/RecentLoginRecord", null, cb);
  }  
}
