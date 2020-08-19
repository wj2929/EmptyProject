import { CommonService } from './../../../../services/common.service';
import { CategoryService } from './../../../../services/category.service';
import { Component, OnInit } from '@angular/core';
import { BaseForm } from 'src/app/common/util/BaseForm';
import { DisplayCategoryModel } from 'src/app/models/CategoryModels';
import { DisplayUserExtendModel } from 'src/app/models/PassportLoginModel';
import { NgbActiveModal } from 'src/app/common/modal/modal-ref';
import { FormBuilder, FormArray } from '@angular/forms';
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { SweetError, ShowBlockUI, HideBlockUI, SweetConfirm } from 'src/app/common/common';

@Component({
  selector: 'app-manage-basicdata-category-import',
  templateUrl: './manage-basicdata-category-import.component.html',
  styleUrls: ['./manage-basicdata-category-import.component.css']
})
export class ManageBasicdataCategoryImportComponent extends BaseForm implements OnInit {

  certificateCategories: Array<DisplayCategoryModel> = [];
  certificateCategoryTypeKey: string = "CertificateType";
  associationCustomFormKeyName: string = "AssociationCustomFormKey";
  selectCertificateCategoryName: string = "";
  selectCertificateCategoryId: string = ""
  downloadTemplateUrl = "";

  cagegoryTypeId: any = "";
  parentCategoryId: any = "";
  categoryTypeName:string="";

  userExtendModel: DisplayUserExtendModel;
  constructor(public activeModal: NgbActiveModal,
    private fb: FormBuilder,
    private CategoryService: CategoryService,
    private CommonService: CommonService) {
    super();
  }

  Close() {
    this.activeModal.close();
  }

  SetForm(cagegoryTypeId: any, parentCategoryId: any,categoryTypeName) {
    this.cagegoryTypeId = cagegoryTypeId;
    this.parentCategoryId = parentCategoryId;
    this.categoryTypeName = categoryTypeName;
    this.downloadTemplateUrl =
      this.CommonService.attachTokenGetParam(this.CommonService.buildUrl(`/Category/DownloadCategoryTemplate?CategoryTypeId=${this.cagegoryTypeId}&ParentCategory_Id=${this.parentCategoryId}`));
  }



  OnSubmit() {
    // debugger;
    // debugger;
    // if (this.formModel.valid) {
    //   let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;
    //   this.formModel.value.CustomFormSetting.CustomFormItemSettings.forEach(element => {
    //     if (element.Value.constructor == Array)
    //       element.Value = element.Value.join(",");
    //   });

    //   this.CertificateService.createCertificate(this.formModel.value, (data: ReturnInfoModel) => {
    //     this.activeModal.close(data);
    //   });
    // }
  }
  ngOnInit() {


  }

  uploadFile() {
    var _this = this;
    this.CommonService.upload({
      //上传文件接收地址
      uploadUrl: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl(`/Category/ImportCategoryTemplate?CategoryTypeId=${this.cagegoryTypeId}&ParentCategory_Id=${this.parentCategoryId}`)),
      //选择文件后，发送文件前自定义事件
      //file为上传的文件信息，可在此处做文件检测、初始化进度条等动作
      beforeSend: function (file) {
        if (file.name.indexOf(".xlsx") == -1) {
          SweetError("", "要求文件扩展名:.xlsx！");
          // $.messager.alert(getLang("要求文件扩展名:.xls,.xlsx！", langz_code));
          return false;
        }
        else {
          ShowBlockUI("ImportData");
          return true;
        }
      },
      //文件上传完成后回调函数
      //res为文件上传信息
      callback: function (res) {
        HideBlockUI("ImportData");
        var data = JSON.parse(res);
        if (data.State) {
          SweetConfirm("导入完成\n" + `待导入数：${data.DataObject.WaitImportCount}，实际导入数：${data.DataObject.ImportCount}`, "点击确定按钮下载导入详情", (callback: Boolean) => {
            if (callback)
              window.open(data.DataObject.ImportResult);
          });
          _this.activeModal.close(data);
          // _this.batchImportData(data.BatchCount,1,_this.BatchSize);
        }
        else {
          SweetError("", data.Message);
        }
        // alert(data.Message);
        // ReLoad("DataInfo");
      },
      //返回上传过程中包括上传进度的相关信息
      //详细请看res,可在此加入进度条相关代码
      uploading: function (res) {

      }
    });
  }

}
