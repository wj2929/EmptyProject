import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() { }

  set(key, v) {
    //添加一个缓存这个关键字的时候的时间戳
    var currentTime = new Date().getTime();
    localStorage.setItem(key, JSON.stringify({ data: v, time: currentTime }))
  }

  get(key, exp) {
    try {
      var data = localStorage.getItem(key);

      var dataObj = JSON.parse(data);
      if (exp != null) {
        var currentTime = new Date().getTime();
        if (currentTime - dataObj.time > exp) {
          console.log("超时了");
          localStorage.removeItem(key);
          return null;
          //业务处理
        } else {
          //业务处理
          console.log(dataObj.data);
          return dataObj.data;
        }
      }
      else
        return dataObj.data;
    }
    catch (e) {
      return null;
    }

  }

  getObject(key,exp){
    return JSON.parse(this.get(key,exp) || '{}');
  }
}
