import { SelhttpService } from './selhttp.service';
import { Injectable } from '@angular/core';
import { PassportLoginModel } from '../models/PassportLoginModel';
import { Observable } from 'rxjs';
import { ReturnInfoModel, ReturnTokenInfoModel } from '../models/ReturnInfoModel';
import { CommonService } from './common.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class PassportService {

  constructor(private router: Router,
    private common:CommonService,
    private http: HttpClient,
    private SelhttpService:SelhttpService) { }

    canActivate(route: ActivatedRouteSnapshot,
      state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
  
      return this.activate();
    }
  
    /**
     * 如果用户未登录转到登录地址
     */
    activate(): Observable<boolean> {
      return this.http.get(this.common.attachTokenGetParam(this.common.buildUrl('/Passport/IsLoginAndHasPermission?PermissionUrl='+encodeURIComponent(window.location.hash)))).pipe(map((data:ReturnInfoModel) => {
        if(!data.State)
          this.router.navigate(['/login']); 
        return data.State;
      }));
      // return this.http.get(this.common.attachTokenGetParam(this.common.buildUrl('/Passport/IsLogin'))).pipe(map((data:ReturnInfoModel) => {
      //   if(!data.State)
      //     this.router.navigate(['/login']); 
      //   return data.State;
      // }));
    }    

  // /**
  //  * 账号登录
  //  * @param modelInfo 
  //  */
  // login(modelInfo : PassportLoginModel ) :Observable<ReturnInfoModel>{
  //   const options = new HttpParams().set('UserName', modelInfo.UserName).set('Password', modelInfo.Password);
  //   return this.http.post<ReturnInfoModel>(this.common.buildUrl('/Passport/Login'), options);
  // }

  /**
   * 获取用户扩展信息
   * @param cb 
   */
  userExtendInfo(cb?: Function){
    return this.SelhttpService.get<ReturnInfoModel>("/Passport/UserExtendInfo",null,cb);
  }
  
  userNameExists(username): Promise<ValidationErrors> | Observable<ValidationErrors> {
    return this.http.get(this.common.attachTokenGetParam(this.common.buildUrl('/Passport/UserNameExists?username=' + username))).pipe(map((data: ReturnInfoModel) => {
      return data.State ? { "UserNameExists": true } : null;
    }));
  }  

    /**
   * 账号登录
   * @param modelInfo 
   */
  login(modelInfo : PassportLoginModel, cb?: Function){
    // const options = new HttpParams().set('UserName', modelInfo.UserName).set('Password', modelInfo.Password);
    // return this.http.post<ReturnInfoModel>(this.common.buildUrl('/Passport/Login'), options);
    return this.SelhttpService.post<ReturnTokenInfoModel>("/Passport/Login",modelInfo,cb);
  }
  /**
   * 验证是否登录
   * @param cb 
   */
  isLogin(cb?: Function){
    return this.SelhttpService.get<ReturnInfoModel>("/Passport/IsLogin",{},cb);
  }

  /**
   * 锁定用户
   * @param username 
   * @param cb 
   */
  lockUser(username,cb?: Function){
    return this.SelhttpService.post<ReturnInfoModel>("/Passport/LockUser",{Username:username},cb);
  }

  /**
   * 解锁用户
   * @param username 
   * @param cb 
   */
  unLockUser(username,cb?: Function){
    return this.SelhttpService.post<ReturnInfoModel>("/Passport/UnLockUser",{Username:username},cb);
  }

  /**
   * 创建用户
   * @param data
   * @param cb 
   */
  createUser(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Passport/UserCreate", data, cb);
  }


  /**
   * 编辑用户
   * @param data
   * @param cb 
   */
  editUser(data?: Object, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Passport/UserEdit", data, cb);
  }

  // /**
  //  * 用户列表分页
  //  * @param data
  //  * @param cb 
  //  */
  // pagingUser(username,organtype?,islock?,pageNum: number = 1, pageSize: number = 10, cb?: Function) {
  //   this.SelhttpService.get<ReturnInfoModel>("/Passport/UserPaging",
  //       { username:username, organtype: organtype, islock:islock,PageNum:pageNum,PageSize:pageSize }, 
  //     cb);
  // }

    /**
   * 用户列表分页
   * @param data
   * @param cb 
   */
  pagingUser(data?:any,pageNum: number = 1, pageSize: number = 10, cb?: Function) {
    if(data){
      data.PageNum = pageNum;
      data.PageSize = pageSize;
    }
    this.SelhttpService.get<ReturnInfoModel>("/Passport/UserPaging",
      data, 
      cb);
  }
  /**
   * 删除用户
   * @param data
   * @param cb 
   */
  deleteUser(username, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Passport/UserDelete", { UserName: username }, cb);
  }

  /**
   * 删除用户
   * @param data
   * @param cb 
   */
  deleteUsers(usernames, cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Passport/UserDeletes", { UserNames: usernames }, cb);
  }

  /**
   * 修改密码，要求填写旧密码
   * @param data 
   * @param cb 
   */
  changePassword(data?:object,cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Passport/ChangePassword", data, cb);
  }

  /**
   * 管理密码，不要求填写旧密码
   * @param data 
   * @param cb 
   */
  managePassword(data?:object,cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Passport/ManagePassword", data, cb);
  }  

  /**
   * 获取管理权限菜单
   * @param cb 
   */
  getMenus(cb?: Function) {
    this.SelhttpService.get<ReturnInfoModel>("/Passport/GetMenus", null, cb);
  }  

  /**
   * 刷选Token
   * @param data 
   * @param cb 
   */
  refreshToken(data?:object,cb?: Function) {
    this.SelhttpService.post<ReturnInfoModel>("/Passport/RefreshToken", data, cb);
  }  
}
