import { CommonService } from './common.service';
import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SelhttpService {

  constructor(private http: HttpClient,private commom:CommonService) { }

  public get<T>(url, params?: Object, cb?: Function) {
    let httpParams = new HttpParams();
    this.log('get开始请求');
    const vm = this;
    if (params) {
      for (const key in params) {
        if (params[key] === false || params[key]) {
          httpParams = httpParams.set(key, params[key]);
        }
      }
    }
    vm.http.get<T>(this.commom.buildUrl(url), {params: this.commom.attachTokenPostParam(httpParams)})
      .subscribe(data => {
        this.log('get请求结束', data);
        cb(data);
      });
  }

  public post<T>(url, data?: any, cb?: Function, options?: Object) {
    // debugger;
    this.log('post开始请求');
    const vm = this;
    if (data) {
      if(this.commom.getToken()!="" && this.commom.getToken()!=null)
        data.Token=this.commom.getToken();
    }
    vm.http.post<T>(this.commom.buildUrl(url), data,  options )
      .subscribe(data => {
        this.log('post请求结束', data);
        cb(data);
      });
  }

  public put<T>(url, data?: any, cb?: Function, options?: Object) {
    this.log('put开始请求');
    const vm = this;
    if (data) {
      if(this.commom.getToken()!="" && this.commom.getToken()!=null)
        data.Token=this.commom.getToken();
    }
    vm.http.put<T>(this.commom.buildUrl(url), data, options)
      .subscribe(data => {
        this.log('put请求结束', data);
        cb(data);
      });
  }

  public delete<T>(url, params?: Object, cb?: Function) {
    let httpParams = new HttpParams();
    this.log('delete开始请求');
    const vm = this;
    if (params) {
      for (const key in params) {
        if (params[key]) {
          httpParams = httpParams.set(key, params[key]);
        }
      }
    }
    vm.http.delete<T>(this.commom.buildUrl(url), {params:  this.commom.attachTokenPostParam(httpParams)})
      .subscribe(data => {
        this.log('delete请求结束', data);
        cb(data);
      });
  }

  private log(message?: any, ...optionalParams: any[]): void{
    if(this.commom.isDebug())
      this.commom.log(message,optionalParams);
  }


}
