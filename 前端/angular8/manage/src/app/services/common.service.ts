import { SelhttpService } from './selhttp.service';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpParams } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';
import { ReturnInfoModel } from '../models/ReturnInfoModel';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(private LocalStorageService:LocalStorageService) { }

     /**
   * 
   * @param url 构建Url
   */
  buildUrl(url) : string{
    return environment.ServiceUrl + `${url}`;
  }

  /**
   * 是否调试/测试模式
   */
  isDebug():Boolean{
    return !environment.production;
  }

  /**
   * 附加 Token Post参数
   * @param httpParams 
   * @param token 
   */
  attachTokenPostParam(httpParams:HttpParams) : HttpParams{
    if(httpParams==null)
      httpParams = new HttpParams();
    
    const token = this.getToken();
    return token!="" && token!=null ? httpParams.set("Token",token) : httpParams;
  }

  /**
   * 附加 Token Get参数
   * @param url 
   * @param token 
   */
  attachTokenGetParam(url:string) : string{
    const token = this.getToken();
    return token!=null && token!="" ? url.concat(url.indexOf("?")!=-1 ? "&" : "?",`token=${token}`) : url;
  }

  /**
   * 设置身份令牌
   * @param token 
   */
  setToken(token:string){
    // debugger;
    // this.LocalStorageService.set("token",token);
    sessionStorage.token = token;
    // localStorage.token = token;
  }

  /**
   * 获取身份令牌
   */
  getToken():string{

    return sessionStorage.token;
    
    // debugger;
    // let token:string = this.LocalStorageService.get("token",60*20*1000);  //20分钟有效期
    // if(token!=null)
    //   this.setToken(token); //续期

    // return token;
    // return localStorage.token;
  } 

  /**
   * 输出日志
   * @param message 
   * @param optionalParams 
   */
  log(message?: any, ...optionalParams: any[]): void{
    // if(this.isDebug())
    console.log(message,optionalParams);
  }


  /**
   * 上传
   * @param option 
   */
  upload(option:any) {
    var file,
        fd = new FormData(),
        xhr = new XMLHttpRequest(),
        loaded, tot, per, uploadUrl, input;

    input = document.createElement("input");
    input.setAttribute('id', "myUpload-input");
    input.setAttribute('type', "file");
    input.setAttribute('name', "files");

    input.click();

    var uploadUrl = option.uploadUrl;
    var callback = option.callback;
    var uploading = option.uploading;
    var beforeSend = option.beforeSend;
    var appendFormData = option.appendFormData;

    input.onchange = function () {
        file = input.files[0];
        if (beforeSend instanceof Function) {
            if (beforeSend(file) === false) {
                return false;
            }
        }

        if (appendFormData instanceof Function)
            appendFormData(fd);

        fd.append("files", file);

        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                if (callback instanceof Function) {
                    callback(xhr.responseText);
                }
            }
        }

        //侦查当前附件上传情况
        xhr.upload.onprogress = function (evt) {
            loaded = evt.loaded;
            tot = evt.total;
            per = Math.floor(100 * loaded / tot); //已经上传的百分比
            if (uploading instanceof Function) {
                uploading(per);
            }
        };

        xhr.open("post", uploadUrl);
        xhr.send(fd);
    }
  }


}
