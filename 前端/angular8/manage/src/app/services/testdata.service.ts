import { SelhttpService } from './selhttp.service';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';
import { ReturnInfoModel } from '../models/ReturnInfoModel';
import { DataInfoService } from './datainfo.service';

@Injectable({
  providedIn: 'root'
})
export class TestdataService {

  constructor(public common: CommonService,public  SelhttpService: SelhttpService) { 

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
    this.SelhttpService.get<ReturnInfoModel>("/TestDataInfo/TestDataInfoPaging", data, cb);
  }
}
