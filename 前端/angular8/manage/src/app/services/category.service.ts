import { Injectable } from '@angular/core';
import { ReturnInfoModel } from '../models/ReturnInfoModel';
import { CommonService } from './common.service';
import { HttpClient } from '@angular/common/http';
import { SelhttpService } from './selhttp.service';
import { ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private common: CommonService, 
    private http: HttpClient, 
    private SelhttpService: SelhttpService) { }

    keyExist(key): Promise<ValidationErrors> | Observable<ValidationErrors> {
      return this.http.get(this.common.attachTokenGetParam(this.common.buildUrl('/Category/CheckCategoryTypeKeyExists?Keycode=' + key))).pipe(map((data: ReturnInfoModel) => {
        return data.State ? { "CategoryTypeKeyExists": true } : null;
      }));
    }

  /**
   * 创建分类类型
   * @param data
   * @param cb 
   */
  createCategoryType(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Category/CategoryTypeCreate", data, cb);
  }


  /**
   * 编辑分类类型
   * @param data
   * @param cb 
   */
  editCategoryType(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Category/CategoryTypeEdit", data, cb);
  }

  /**
   * 分类类型列表
   * @param data
   * @param cb 
   */
  listCategoryType(cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Category/CategoryTypeList", null, cb);
  }

  /**
   * 删除分类类型
   * @param data
   * @param cb 
   */
  deleteCategoryType(Id, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Category/CategoryTypeDelete", { Id: Id }, cb);
  }

  
  /**
   * 分类类型信息
   * @param data
   * @param cb 
   */
  singleCategoryType(id,cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Category/CategoryTypeSingle", {Id:id}, cb);
  }

  /**
   * 获取分类分支导航
   * @param data
   * @param cb 
   */
  ramusCategorys(parentid,cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Category/CategoryRamus", {ParentCategory_Id:parentid}, cb);
  }

  /**
   * 创建分类
   * @param data
   * @param cb 
   */
  createCategory(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Category/CategoryCreate", data, cb);
  }


  /**
   * 编辑分类
   * @param data
   * @param cb 
   */
  editCategory(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Category/CategoryEdit", data, cb);
  }

  /**
   * 分类列表
   * @param data
   * @param cb 
   */
  listCategory(categoryTypeId,parentid,cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Category/CategoryList", {CategoryTypeId:categoryTypeId,ParentCategory_Id:parentid}, cb);
  }

  /**
   * 获取父分类（迭代）关联自定义表单Id
   * @param categoryTypeId 
   * @param parentid 
   * @param cb 
   */
  getCategoryParentCustomFormId(categoryTypeId,parentid,cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Category/CategoryParentCustomFormId", {CategoryTypeId:categoryTypeId,ParentCategory_Id:parentid}, cb);
  }

    /**
   * 分类列表
   * @param data
   * @param cb 
   */
  listCategoryWithChildCategory(categoryTypeId,parentid,cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Category/CategoryListWithChildCategory", {CategoryTypeId:categoryTypeId,ParentCategory_Id:parentid}, cb);
  }
  /**
   * 删除分类
   * @param data
   * @param cb 
   */
  deleteCategory(Id, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Category/CategoryDelete", { Id: Id }, cb);
  }  


  /**
   * 删除分类
   * @param data
   * @param cb 
   */
  deleteCategorys(Ids, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Category/CategoryDeletes", { Ids: Ids }, cb);
  }    

  /**
   * 获取指定类型根分类列表
   * @param categoryTypeKeycode 
   * @param cb 
   */
  rootCategoryListByType(categoryTypeKeycode, cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Category/RootCategoryListByType", { CategoryTypeKeycode: categoryTypeKeycode }, cb);
  }  

  /**
   * 获取指定类型根分类列表
   * @param categoryTypeKeycode 
   * @param cb 
   */
  rootCategoryListWithChildCategoryByType(categoryTypeKeycode, cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Category/RootCategoryListWithChildCategoryByType", { CategoryTypeKeycode: categoryTypeKeycode }, cb);
  }  

  /**
   * 保存排序
   * @param categoryTypeId 
   * @param parentid 
   * @param sortIds 
   * @param cb 
   */
  SaveCategoryOrder(categoryTypeId,parentid,sortIds,cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Category/SaveCategoryOrder", 
    {CategoryTypeId:categoryTypeId,ParentCategory_Id:parentid,SortIds:sortIds}, cb);
  }    

  /**
   * 导出数据
   * @param categoryTypeId 
   * @param parentid 
   * @param sortIds 
   * @param cb 
   */
  exportCategorys(categoryTypeId,parentid,cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Category/ExportCategorys", {CategoryTypeId:categoryTypeId,ParentCategory_Id:parentid}, cb);
  }    


  /**
   * 保存排序
   * @param customFormId 
   * @param sortId 
   * @param previousId 
   * @param cb 
   */
  saveOrder(sortIds, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Category/SaveOrder",
      { sortIds: sortIds }, cb);
  }
}
