import { SelhttpService } from './selhttp.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';
import { ReturnInfoModel } from '../models/ReturnInfoModel';
import { Observable, of } from 'rxjs';
import { ValidationErrors } from '@angular/forms';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CustomformService {

  constructor(private common: CommonService,
    private http: HttpClient,
    private SelhttpService: SelhttpService) { }


  keyExist(key): Promise<ValidationErrors> | Observable<ValidationErrors> {
    return this.http.get(this.common.attachTokenGetParam(this.common.buildUrl('/CustomForm/ExistKey?Key=' + key))).pipe(map((data: ReturnInfoModel) => {
      return data.State ? { "CustomFormKeyExists": true } : null;
    }));
  }

  /**
   * 通过表单Key获取表单信息
   * @param keycode 
   * @param cb 
   */
  singleByKeycode(customformKey, cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/CustomForm/SingleByKeycode", { CustomformKeycode: customformKey }, cb);
  }

  /**
   * 创建表单
   * @param data
   * @param cb 
   */
  create(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/CustomFormCreate", data, cb);
  }


  /**
   * 编辑表单
   * @param data
   * @param cb 
   */
  edit(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/CustomFormEdit", data, cb);
  }

  /**
   * 表单列表
   * @param data
   * @param cb 
   */
  list(cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/CustomForm/CustomFormList", null, cb);
  }

  /**
   * 删除表单
   * @param data
   * @param cb 
   */
  delete(Id, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/CustomFormDelete", { Id: Id }, cb);
  }

  /**
 * 创建表单项
 * @param data
 * @param cb 
 */
  createItem(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/CustomFormItemCreate", data, cb);
  }


  /**
   * 编辑表单项
   * @param data
   * @param cb 
   */
  editItem(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/CustomFormItemEdit", data, cb);
  }

  /**
   * 表单项列表
   * @param data
   * @param cb 
   */
  itemList(customformId, onlyenabled, cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/CustomForm/CustomFormItemList", { customformId: customformId, onlyenabled: onlyenabled }, cb);
  }

  /**
 * 表单项列表
 * @param data
 * @param cb 
 */
  itemListByKey(customformKey, onlyenabled, cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/CustomForm/CustomFormItemListByKey", { customformKey: customformKey, onlyenabled: onlyenabled }, cb);
  }

  /**
   * 删除表单项
   * @param data
   * @param cb 
   */
  deleteItem(Id, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/CustomFormItemDelete", { Id: Id }, cb);
  }

  /**
   * 锁定表单项
   * @param data
   * @param cb 
   */
  lockItem(Id, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/CustomFormItemLock", { Id: Id }, cb);
  }

  /**
   * 解锁表单项
   * @param data
   * @param cb 
   */
  unlockItem(Id, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/CustomFormItemUnlock", { Id: Id }, cb);
  }

  /**
   * 删除表单项
   * @param data
   * @param cb 
   */
  deleteItems(Ids, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/CustomFormItemDeletes", { Ids: Ids }, cb);
  }

  // /**
  //  * 保存排序
  //  * @param customFormId 
  //  * @param sortId 
  //  * @param previousId 
  //  * @param cb 
  //  */
  // saveOrderItem(customFormId,sortId,previousId,cb?: Function) {
  //   this.SelhttpService.post<ReturnInfoModel>("/CustomForm/SaveOrder", 
  //     { CustomFormId:customFormId,SortId:sortId,PreviousId:previousId}, cb);
  // }

  /**
   * 保存排序
   * @param customFormId 
   * @param sortId 
   * @param previousId 
   * @param cb 
   */
  saveOrder(sortIds, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/SaveOrder",
      { sortIds: sortIds }, cb);
  }

  /**
   * 保存排序
   * @param customFormId 
   * @param sortId 
   * @param previousId 
   * @param cb 
   */
  saveItemOrder(customFormId, sortIds, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/CustomForm/SaveItemOrder",
      { CustomFormId: customFormId, sortIds: sortIds }, cb);
  }

  
  /**
   * 检查表单项Key是否存在
   * @param customFormId 
   * @param key 
   */
  itemKeyExist(customFormId,key): Promise<ValidationErrors> | Observable<ValidationErrors> {
    return this.http.get(this.common.attachTokenGetParam(this.common.buildUrl(`/CustomForm/CustomFormItemExistKey?Key=${key}&CustomFormId=${customFormId}`))).pipe(map((data: ReturnInfoModel) => {
      return data.State ? { "CustomFormItemKeyExists": true } : null;
    }));
  }

  /**
   *  检查表单项Name是否存在
   * @param customFormId 
   * @param name 
   */
  itemNameExist(customFormId,name): Promise<ValidationErrors> | Observable<ValidationErrors> {
    return this.http.get(this.common.attachTokenGetParam(this.common.buildUrl(`/CustomForm/CustomFormItemExistName?Name=${name}&CustomFormId=${customFormId}`))).pipe(map((data: ReturnInfoModel) => {
      return data.State ? { "CustomFormItemNameExists": true } : null;
    }));
  }

    /**
   * 获取微信公众号图文素材分页列表
   * @param data
   * @param cb 
   */
  newsMediaPaging(pageNum: number = 1, pageSize: number = 10, cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/WeiXin/NewsMediaPaging", {
      PageNum:pageNum,
      PageSize:pageSize
    }, cb);
  }    
}
