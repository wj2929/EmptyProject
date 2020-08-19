import { LogService } from 'src/app/services/log.service';
import { Component, OnInit } from '@angular/core';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { SweetError } from 'src/app/common/common';

@Component({
  selector: 'app-manage-dashboard-login-log',
  templateUrl: './manage-dashboard-login-log.component.html',
  styleUrls: ['./manage-dashboard-login-log.component.css']
})
export class ManageDashboardLoginLogComponent implements OnInit {

  recentLoginRecordList: any={};

  constructor(private LogService: LogService) { 
    this.recentLoginRecordList.Today = [];
    this.recentLoginRecordList.PastDay = [];
  }

  ngOnInit() {
    this.LogService.recentLoginRecord((data: ReturnInfoModel) => {
      if (data.State) {
        this.recentLoginRecordList = data.DataObject;
      }
      else
        SweetError('', data.Message);
    });
  }

}
