import { String } from './util';
import { CategoryService } from './../../services/category.service';
import { DisplayCategoryModel } from './../../models/CategoryModels';
import { debug } from 'util';
import { CommonService } from 'src/app/services/common.service';
import { BaseForm } from './BaseForm';
import { FormGroup, FormArray, FormBuilder, Validators } from "@angular/forms";
import { CustomformService } from "src/app/services/customform.service";
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { CustomFormSettingModel } from '../custom-form-setting/CustomFormSettingModels';
import { CustomFormSetting, DisplayCustomFormItemModel, CustomFormItemSetting } from 'src/app/models/CustomFormModels';
import { CustomformitemValidator } from 'src/app/validators/customformitem-validator';
import * as moment from 'moment';
import { BaseCustomForm } from './BaseCustomForm';
declare var $: any;

export class BaseCreateCustomForm extends BaseCustomForm {

   

    constructor(
        public CustomformService: CustomformService,
        public CommonService: CommonService,
        public CategoryService: CategoryService,
        public fb: FormBuilder) {
        super(CustomformService, CommonService, fb);
    }


    listCustomFormItemByReturnInfo(data: ReturnInfoModel) {
        if (data.State) {
            let customFormItemList = data.DataObject;
            // this.customFormItemList = data.DataObject;
            let childCustomFormItemList = [];
            let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;
            CustomFormItemSettings.clear();
            // debugger;
            let customFormItemList1 = customFormItemList.filter(t => t.FormType == 9);
            if(customFormItemList1.length>0){
                let customFormItem = customFormItemList1[0];
                if (customFormItem.OptionText != null && customFormItem.OptionText != "") {
                    this.CustomformService.itemListByKey(customFormItem.OptionText,true,(data1: ReturnInfoModel) => {
                        this.customFormItemList = customFormItemList;
                        let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;
                        CustomFormItemSettings.clear();
                        childCustomFormItemList = data1.DataObject;
                        this.customFormItemList.forEach((t,index) => {        
                            t.index = index;
                            CustomFormItemSettings.push(this.doCustomFormItem(t,new CustomFormItemSetting(t.Key,t.Name,""),childCustomFormItemList));
                        });
                        this.childCustomFormItemList = childCustomFormItemList;
                    });
                }
            }
            else{
                this.customFormItemList = customFormItemList;
                this.customFormItemList.forEach((t,index) => {        
                    t.index = index;
                    CustomFormItemSettings.push(this.doCustomFormItem(t,new CustomFormItemSetting(t.Key,t.Name,""),childCustomFormItemList));
                });
            }
        }
    }

    doCustomFormItem(customFormItem:any,customFormItemSettingValue:CustomFormItemSetting,childCustomFormItemList:any){
        let formGroup = this.fb.group({
            Key: [customFormItem.Key],
            Name: [customFormItem.Name],
            Value: ['', {
                validators: [CustomformitemValidator(customFormItem)],
                asyncValidators: null,
                updateOn: 'blur'
            }]
        });
        // formGroup.get("Value").setValue()
        if (customFormItem.FormType == 2) {
            if (customFormItem.OptionText != null && customFormItem.OptionText != "") {
                customFormItem.OptionList = [];
                const regex = /(\{.*?\}|.*?),/gm;
                let m;
                while ((m = regex.exec(customFormItem.OptionText + ",")) !== null) {
                    // This is necessary to avoid infinite loops with zero-width matches
                    if (m.index === regex.lastIndex) {
                        regex.lastIndex++;
                    }

                    try {
                        var jsonObject = eval('(' + m[1] + ')');
                        if (jsonObject.constructor == Object) {
                            customFormItem.OptionList.push({
                                text: jsonObject.text,
                                value: jsonObject.value
                            });
                        }
                        else {
                            customFormItem.OptionList.push({
                                text: jsonObject,
                                value: jsonObject
                            });
                        }
                    } catch (e) {
                        customFormItem.OptionList.push({
                            text: m[1],
                            value: m[1]
                        });
                    }
                }
            }
        }
        else if (customFormItem.FormType == 8) {
            customFormItem.CategoryList = Array<DisplayCategoryModel>();
            if (customFormItem.OptionText != null && customFormItem.OptionText != "") {
                this.CategoryService.rootCategoryListWithChildCategoryByType(customFormItem.OptionText, (data1: ReturnInfoModel) => {
                    // t.CategoryListWithChildCategory = data1.DataObject;
                    this.iterateCategory(customFormItem.CategoryList, data1.DataObject);
                });
            }
        }
        else if(customFormItem.FormType == 9){
            customFormItem.CustomFormItemList = [];
            customFormItem.ChildCustomForms = [];
            var c :any= {};
            c.index = customFormItem.ChildCustomForms.length;
            if (customFormItem.OptionText != null && customFormItem.OptionText != "") {
                formGroup.controls.Value = this.fb.group({ChildCustomForms: this.fb.array([])});
                let ChildCustomForms = (formGroup.controls.Value as FormGroup).controls.ChildCustomForms as FormArray;            
                ChildCustomForms.clear();    
                ChildCustomForms.push(this.fb.group({ChildCustomFormItemSettings: this.fb.array([])}));
                let CustomFormItemSettings = ChildCustomForms.get("0.ChildCustomFormItemSettings") as FormArray;//this.formModel.get(`CustomFormSetting.CustomFormItemSettings.${t.index}.Value.CustomForms.0.CustomFormItemSettings`)  as FormArray;     
                // debugger;      
                CustomFormItemSettings.clear();
                // CustomFormItemSettings.push(this.fb.group({
                //     Key: ["test"],
                //     Name: ["test"],
                //     Value: ['']
                // }));
                        
                // this.CustomformService.itemListByKey(t.OptionText,true,(data1: ReturnInfoModel) => {
                //     debugger;
                    // CustomFormItemSettings = this.formModel.get(`CustomFormSetting.CustomFormItemSettings.${t.index}.Value.CustomForms.0.CustomFormItemSettings`)  as FormArray;     
                    // CustomFormItemSettings.clear();
                    customFormItem.CustomFormItemList = childCustomFormItemList;
                    customFormItem.CustomFormItemList.forEach((tt,index1) => {
                        tt.index = index1;
                        CustomFormItemSettings.push(this.doCustomFormItem(tt,new CustomFormItemSetting(tt.Key,tt.Name,""),childCustomFormItemList));
                        // this.doCustomFormItem(tt);
                        c.Key = tt.Key;
                        c.Value = "",
                        c.Name = tt.Name;
                    });
                    customFormItem.ChildCustomForms.push({});
                    
                // });
            }
        }

        return formGroup;
    }



    iterateCategory(RetCategoryList: Array<DisplayCategoryModel>, CategoryList: Array<DisplayCategoryModel>) {
        CategoryList = CategoryList.sort(function(a,b){return a.OrderBy-b.OrderBy});
        CategoryList.forEach(t => {
            t.Name =t.Name ;// new Array(t.Level * 2 + 1).join("-") + t.Name;
            RetCategoryList.push(t);
            // t.ChildCategorys = t.ChildCategorys.sort(function(a,b){return a.OrderBy-b.OrderBy});
            this.iterateCategory(RetCategoryList, t.ChildCategorys);
        });
    }

    ready($event: any) {
        console.log($event);
    }


    filebatchuploadcomplete($event: any) {
        console.log($event);
    }

    filebatchuploaderror($event: any) {
        console.log($event);
    }

    fileuploaded($event: any, item) {
        // debugger;
        console.log($event);

        this.formModel.value.CustomFormSetting.CustomFormItemSettings.forEach(element => {
            if (element.Key === item.Key) {
                element.Value = JSON.stringify($event.data.response.DataObject);

                if (element.Value != null) {
                    try {
                        this.formModel.get("CustomFormSetting.CustomFormItemSettings." + this.formModel.value.CustomFormSetting.CustomFormItemSettings.findIndex(t => t.Key == element.Key) + ".Value").setValue(element.Value);
                    }
                    catch (e) { }
                }
            }
        });

    }

    filedeleted($event: any, item) {
        console.log($event);

        this.formModel.value.CustomFormSetting.CustomFormItemSettings.forEach(element => {
            if (element.Key === item.Key) {
                element.Value = "";

                if (element.Value != null) {
                    try {
                        this.formModel.get("CustomFormSetting.CustomFormItemSettings." + this.formModel.value.CustomFormSetting.CustomFormItemSettings.findIndex(t => t.Key == element.Key) + ".Value").setValue(element.Value);
                    }
                    catch (e) { }
                }
            }
        });
    }


    fileuploaderror($event: any) {
        console.log($event);
    }

}
