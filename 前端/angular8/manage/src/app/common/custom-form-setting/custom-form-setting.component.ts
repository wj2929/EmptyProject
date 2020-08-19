
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DisplayCustomFormItemModel, CustomFormItemSetting } from 'src/app/models/CustomFormModels';

@Component({
  selector: 'app-custom-form-setting',
  templateUrl: './custom-form-setting.component.html',
  styleUrls: ['./custom-form-setting.component.css']
})
export class CustomFormSettingComponent implements OnInit {
  @Input()
  CustomFormItemSettings:Array<CustomFormItemSetting>=[]

  // @Output()
  // GradeSchoolYearChanged: EventEmitter<GradeSchoolYearChangedEventArguments> = new EventEmitter<GradeSchoolYearChangedEventArguments>();

  // private SchoolYears: Array<number> = [];


  constructor() { }

  ngOnInit() {
    debugger;
    // let now = new Date();
    // let nowYear = now.getFullYear();
    // for (let i = nowYear - 5; i < nowYear + 5; i++) {
    //   this.SchoolYears.push(i);
    // }

  }
  // onChange($event) {
  //   this.GradeSchoolYearChanged.emit(new GradeSchoolYearChangedEventArguments(this.GradeModel.Id, $event == "" ? 0 : parseInt($event)));
  // }
}
