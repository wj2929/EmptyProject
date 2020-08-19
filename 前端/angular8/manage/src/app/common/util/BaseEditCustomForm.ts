import { CategoryService } from './../../services/category.service';
import { CommonService } from 'src/app/services/common.service';
import { BaseForm } from './BaseForm';
import { FormGroup, FormArray, FormBuilder } from "@angular/forms";
import { CustomformService } from "src/app/services/customform.service";
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { CustomFormSettingModel } from '../custom-form-setting/CustomFormSettingModels';
import { CustomFormSetting, CustomFormItemSetting } from 'src/app/models/CustomFormModels';
import { CustomformitemValidator } from 'src/app/validators/customformitem-validator';
import * as moment from 'moment';
import { BaseCustomForm } from './BaseCustomForm';
import { DisplayCategoryModel } from 'src/app/models/CategoryModels';

export class BaseEditCustomForm extends BaseCustomForm {

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
            if (customFormItemList1.length > 0) {
                let customFormItem = customFormItemList1[0];
                if (customFormItem.OptionText != null && customFormItem.OptionText != "") {
                    this.CustomformService.itemListByKey(customFormItem.OptionText, true, (data1: ReturnInfoModel) => {
                        this.customFormItemList = customFormItemList;
                        let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;
                        CustomFormItemSettings.clear();
                        childCustomFormItemList = data1.DataObject;
                        this.customFormItemList.forEach((t, index) => {
                            t.index = index;
                            let customFormItemSettingValue = this.CustomFormSetting.CustomFormItemSettings.find(tt => tt.Key == t.Key);
                            CustomFormItemSettings.push(this.doCustomFormItem(t, customFormItemSettingValue,childCustomFormItemList));
                        });
                        this.childCustomFormItemList = childCustomFormItemList;
                    });
                }
            }
            else {
                this.customFormItemList = customFormItemList;
                this.customFormItemList.forEach((t, index) => {
                    t.index = index;
                    let customFormItemSettingValue = this.CustomFormSetting.CustomFormItemSettings.find(tt => tt.Key == t.Key);
                    CustomFormItemSettings.push(this.doCustomFormItem(t, customFormItemSettingValue,childCustomFormItemList));
                });
            }
        }
    }

    doCustomFormItem(customFormItem: any, customFormItemSettingValue:CustomFormItemSetting,childCustomFormItemList: any) {
        let formGroup = this.fb.group({
            Key: [customFormItem.Key],
            Name: [customFormItem.Name],
            Value: ['', {
                validators: [CustomformitemValidator(customFormItem)],
                asyncValidators: null,
                updateOn: 'blur'
            }]
        });

        customFormItem.current = "text";
        customFormItem.text = "";
        customFormItem.selectNewsMedia = null;

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

        // var customFormItemSetting = this.CustomFormSetting.CustomFormItemSettings.find(tt => tt.Key == customFormItem.Key);
        if (customFormItemSettingValue) {
            if (customFormItem.FormType == 2 && customFormItem.MoreSelect) {
                formGroup = this.fb.group({
                    Key: [customFormItemSettingValue.Key],
                    Name: [customFormItemSettingValue.Name],
                    // Value: [customFormItemSetting.Value.split(',')]
                    Value: [customFormItemSettingValue.Value.split(','), {
                        validators: [CustomformitemValidator(customFormItem)],
                        asyncValidators: null,
                        updateOn: 'blur'
                    }]
                });

                this.initSelectSearchItem();
            }
            else if (customFormItem.FormType == 3) {
                // CustomFormItemSettings.push(this.fb.group(new CustomFormSettingModel(customFormItemSetting.Key, customFormItemSetting.Name, "")));
                formGroup = this.fb.group({
                    Key: [customFormItemSettingValue.Key],
                    Name: [customFormItemSettingValue.Name],
                    Value: ['', {
                        validators: null,
                        asyncValidators: null,
                        updateOn: 'blur'
                    }]
                });
            }
            else if (customFormItem.FormType == 4) {
                if (customFormItemSettingValue.Value != "" && customFormItemSettingValue.Value != null) {
                    formGroup = this.fb.group({
                        Key: [customFormItemSettingValue.Key],
                        Name: [customFormItemSettingValue.Name],
                        Value: [{ startDate: customFormItemSettingValue.Value, endDate: customFormItemSettingValue.Value }, {
                            validators: [CustomformitemValidator(customFormItem)],
                            asyncValidators: null,
                            updateOn: 'blur'
                        }]
                    });
                }
                else {
                    formGroup = this.fb.group({
                        Key: [customFormItemSettingValue.Key],
                        Name: [customFormItemSettingValue.Name],
                        Value: ['', {
                            validators: [CustomformitemValidator(customFormItem)],
                            asyncValidators: null,
                            updateOn: 'blur'
                        }]
                    });
                }
            }
            else if (customFormItem.FormType == 5) {
                if (customFormItemSettingValue.Value != "" && customFormItemSettingValue.Value != null) {
                    formGroup = this.fb.group({
                        Key: [customFormItemSettingValue.Key],
                        Name: [customFormItemSettingValue.Name],
                        Value: [{ startDate: customFormItemSettingValue.Value.split(' - ')[0], endDate: customFormItemSettingValue.Value.split(' - ')[1] }, {
                            validators: [CustomformitemValidator(customFormItem)],
                            asyncValidators: null,
                            updateOn: 'blur'
                        }]
                    });
                }
                else {
                    formGroup = this.fb.group({
                        Key: [customFormItemSettingValue.Key],
                        Name: [customFormItemSettingValue.Name],
                        Value: [, {
                            validators: [CustomformitemValidator(customFormItem)],
                            asyncValidators: null,
                            updateOn: 'blur'
                        }]
                    });
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
                // if(t.MoreSelect){
                formGroup = this.fb.group({
                    Key: [customFormItemSettingValue.Key],
                    Name: [customFormItemSettingValue.Name],
                    // Value: [customFormItemSetting.Value.split(',')]
                    Value: [customFormItemSettingValue.Value.split(','), {
                        validators: [CustomformitemValidator(customFormItem)],
                        asyncValidators: null,
                        updateOn: 'blur'
                    }]
                });
                // }
                // if (t.OptionText != null && t.OptionText != "") {
                //     this.CategoryService.rootCategoryListWithChildCategoryByType(t.OptionText, (data1: ReturnInfoModel) => {
                //         // t.CategoryListWithChildCategory = data1.DataObject;
                //         this.iterateCategory(t.CategoryList, data1.DataObject);
                //     });
                // }
                // if(t.MoreSelect){

                // }
            }
            else if (customFormItem.FormType == 9) {
                // debugger;
                let customFormItemSettingValueObject = JSON.parse(customFormItemSettingValue.Value);
                formGroup.controls.Value = this.fb.group({ChildCustomForms: this.fb.array([])});
                let ChildCustomForms = (formGroup.controls.Value as FormGroup).controls.ChildCustomForms as FormArray;            
                ChildCustomForms.clear();    
                for(let ChildCustomForm of customFormItemSettingValueObject.ChildCustomForms){
                    let ChildCustomFormItemSettings = this.fb.array([]) as FormArray; //this.fb.group({ChildCustomFormItemSettings: this.fb.array([])});
                    childCustomFormItemList.forEach((tt,index1) => {
                        tt.index = index1;
                        let ChildCustomFormItemSettingValue = ChildCustomForm.ChildCustomFormItemSettings.find(t => t.Key == tt.Key);
                        // debugger;
                        ChildCustomFormItemSettings.push(this.doCustomFormItem(tt,ChildCustomFormItemSettingValue == null ? new CustomFormItemSetting(tt.Key,tt.Name,"") : ChildCustomFormItemSettingValue,childCustomFormItemList));
                        // this.doCustomFormItem(tt);
                        // c.Key = tt.Key;
                        // c.Value = "",
                        // c.Name = tt.Name;
                    });
                    ChildCustomForms.push(this.fb.group({ChildCustomFormItemSettings:ChildCustomFormItemSettings}));
                }
                // debugger;      
                // CustomFormItemSettings.clear();
                // childCustomFormItemList.forEach((tt,index1) => {
                //     tt.index = index1;
                //     CustomFormItemSettings.push(this.doCustomFormItem(tt,childCustomFormItemList));
                //     // this.doCustomFormItem(tt);
                //     // c.Key = tt.Key;
                //     // c.Value = "",
                //     // c.Name = tt.Name;
                // });
                customFormItem.ChildCustomForms = customFormItemSettingValueObject.ChildCustomForms;
            }
            else if(customFormItem.FormType == 13) {
                if(customFormItemSettingValue.Value!=""){
                    let customFormItemSettingValueObject = JSON.parse(customFormItemSettingValue.Value);
                    customFormItem.current = customFormItemSettingValueObject.current;
                    customFormItem.text = customFormItemSettingValueObject.text;
                    if(customFormItemSettingValueObject.newsmedia!=""){
                        customFormItem.selectNewsMedia = JSON.parse(customFormItemSettingValueObject.newsmedia);
                    }
                }
                formGroup = this.fb.group({
                    Key: [customFormItemSettingValue.Key],
                    Name: [customFormItemSettingValue.Name],
                    Value: [customFormItemSettingValue.Value, {
                        validators: [CustomformitemValidator(customFormItem)],
                        asyncValidators: null,
                        updateOn: 'blur'
                    }]
                });
            }
            else {
                // CustomFormItemSettings.push(this.fb.group(new CustomFormSettingModel(customFormItemSetting.Key, customFormItemSetting.Name, customFormItemSetting.Value)));
                formGroup = this.fb.group({
                    Key: [customFormItemSettingValue.Key],
                    Name: [customFormItemSettingValue.Name],
                    Value: [customFormItemSettingValue.Value, {
                        validators: [CustomformitemValidator(customFormItem)],
                        asyncValidators: null,
                        updateOn: 'blur'
                    }]
                });
            }
        }
        else {
            // CustomFormItemSettings.push(this.fb.group(new CustomFormSettingModel(t.Key, t.Name, "")));
            formGroup = this.fb.group({
                Key: [customFormItem.Key],
                Name: [customFormItem.Name],
                Value: ['', {
                    validators: [CustomformitemValidator(customFormItem)],
                    asyncValidators: null,
                    updateOn: 'blur'
                }]
            });
        }

        return formGroup;
    }

    // listCustomFormItemByReturnInfo(data: ReturnInfoModel) {
    //     if (data.State) {
    //         this.customFormItemList = data.DataObject;

    //         let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;
    //         CustomFormItemSettings.clear();
    //         this.customFormItemList.forEach(t => {
    //             if (t.FormType == 2) {
    //                 if (t.OptionText != null && t.OptionText != "") {
    //                     t.OptionList = [];
    //                     const regex = /(\{.*?\}|.*?),/gm;
    //                     let m;
    //                     while ((m = regex.exec(t.OptionText + ",")) !== null) {
    //                         // This is necessary to avoid infinite loops with zero-width matches
    //                         if (m.index === regex.lastIndex) {
    //                             regex.lastIndex++;
    //                         }

    //                         try {
    //                             var jsonObject = eval('(' + m[1] + ')');
    //                             if (jsonObject.constructor == Object) {
    //                                 t.OptionList.push({
    //                                     text: jsonObject.text,
    //                                     value: jsonObject.value
    //                                 });
    //                             }
    //                             else {
    //                                 t.OptionList.push({
    //                                     text: jsonObject,
    //                                     value: jsonObject
    //                                 });
    //                             }
    //                         } catch (e) {
    //                             t.OptionList.push({
    //                                 text: m[1],
    //                                 value: m[1]
    //                             });
    //                         }
    //                     }
    //                 }
    //             }

    //             var customFormItemSetting = this.CustomFormSetting.CustomFormItemSettings.find(tt => tt.Key == t.Key);
    //             if (customFormItemSetting) {
    //                 if (t.FormType == 2 && t.MoreSelect) {
    //                     CustomFormItemSettings.push(this.fb.group({
    //                         Key: [customFormItemSetting.Key],
    //                         Name: [customFormItemSetting.Name],
    //                         // Value: [customFormItemSetting.Value.split(',')]
    //                         Value: [customFormItemSetting.Value.split(','), {
    //                             validators: [CustomformitemValidator(t)],
    //                             asyncValidators: null,
    //                             updateOn: 'blur'
    //                         }]
    //                     }));
    //                 }
    //                 else if (t.FormType == 3) {
    //                     // CustomFormItemSettings.push(this.fb.group(new CustomFormSettingModel(customFormItemSetting.Key, customFormItemSetting.Name, "")));
    //                     CustomFormItemSettings.push(this.fb.group({
    //                         Key: [customFormItemSetting.Key],
    //                         Name: [customFormItemSetting.Name],
    //                         Value: ['', {
    //                             validators: null,
    //                             asyncValidators: null,
    //                             updateOn: 'blur'
    //                         }]
    //                     }));
    //                 }
    //                 else if (t.FormType == 4) {
    //                     if (customFormItemSetting.Value != "" && customFormItemSetting.Value != null) {
    //                         CustomFormItemSettings.push(this.fb.group({
    //                             Key: [customFormItemSetting.Key],
    //                             Name: [customFormItemSetting.Name],
    //                             Value: [{ startDate: customFormItemSetting.Value, endDate: customFormItemSetting.Value }, {
    //                                 validators: [CustomformitemValidator(t)],
    //                                 asyncValidators: null,
    //                                 updateOn: 'blur'
    //                             }]
    //                         }));
    //                     }
    //                     else {
    //                         CustomFormItemSettings.push(this.fb.group({
    //                             Key: [customFormItemSetting.Key],
    //                             Name: [customFormItemSetting.Name],
    //                             Value: ['', {
    //                                 validators: [CustomformitemValidator(t)],
    //                                 asyncValidators: null,
    //                                 updateOn: 'blur'
    //                             }]
    //                         }));
    //                     }
    //                 }
    //                 else if (t.FormType == 5) {
    //                     if (customFormItemSetting.Value != "" && customFormItemSetting.Value != null) {
    //                         CustomFormItemSettings.push(this.fb.group({
    //                             Key: [customFormItemSetting.Key],
    //                             Name: [customFormItemSetting.Name],
    //                             Value: [{ startDate: customFormItemSetting.Value.split(' - ')[0], endDate: customFormItemSetting.Value.split(' - ')[1] }, {
    //                                 validators: [CustomformitemValidator(t)],
    //                                 asyncValidators: null,
    //                                 updateOn: 'blur'
    //                             }]
    //                         }));
    //                     }
    //                     else {
    //                         CustomFormItemSettings.push(this.fb.group({
    //                             Key: [customFormItemSetting.Key],
    //                             Name: [customFormItemSetting.Name],
    //                             Value: [, {
    //                                 validators: [CustomformitemValidator(t)],
    //                                 asyncValidators: null,
    //                                 updateOn: 'blur'
    //                             }]
    //                         }));
    //                     }
    //                 }
    //                 else if (t.FormType == 8) {
    //                     t.CategoryList = Array<DisplayCategoryModel>();
    //                     if (t.OptionText != null && t.OptionText != "") {
    //                         this.CategoryService.rootCategoryListWithChildCategoryByType(t.OptionText, (data1: ReturnInfoModel) => {
    //                             // t.CategoryListWithChildCategory = data1.DataObject;
    //                             this.iterateCategory(t.CategoryList, data1.DataObject);
    //                         });
    //                     }
    //                     // if(t.MoreSelect){
    //                         CustomFormItemSettings.push(this.fb.group({
    //                             Key: [customFormItemSetting.Key],
    //                             Name: [customFormItemSetting.Name],
    //                             // Value: [customFormItemSetting.Value.split(',')]
    //                             Value: [customFormItemSetting.Value.split(','), {
    //                                 validators: [CustomformitemValidator(t)],
    //                                 asyncValidators: null,
    //                                 updateOn: 'blur'
    //                             }]
    //                         }));
    //                     // }
    //                     // if (t.OptionText != null && t.OptionText != "") {
    //                     //     this.CategoryService.rootCategoryListWithChildCategoryByType(t.OptionText, (data1: ReturnInfoModel) => {
    //                     //         // t.CategoryListWithChildCategory = data1.DataObject;
    //                     //         this.iterateCategory(t.CategoryList, data1.DataObject);
    //                     //     });
    //                     // }
    //                     // if(t.MoreSelect){

    //                     // }
    //                 }
    //                 else if(t.FormType == 9){

    //                 }
    //                 else {
    //                     // CustomFormItemSettings.push(this.fb.group(new CustomFormSettingModel(customFormItemSetting.Key, customFormItemSetting.Name, customFormItemSetting.Value)));
    //                     CustomFormItemSettings.push(this.fb.group({
    //                         Key: [customFormItemSetting.Key],
    //                         Name: [customFormItemSetting.Name],
    //                         Value: [customFormItemSetting.Value, {
    //                             validators: [CustomformitemValidator(t)],
    //                             asyncValidators: null,
    //                             updateOn: 'blur'
    //                         }]
    //                     }));
    //                 }
    //             }
    //             else {
    //                 // CustomFormItemSettings.push(this.fb.group(new CustomFormSettingModel(t.Key, t.Name, "")));
    //                 CustomFormItemSettings.push(this.fb.group({
    //                     Key: [t.Key],
    //                     Name: [t.Name],
    //                     Value: ['', {
    //                         validators: [CustomformitemValidator(t)],
    //                         asyncValidators: null,
    //                         updateOn: 'blur'
    //                     }]
    //                 }));
    //             }

    //         });
    //     }
    // }

    iterateCategory(RetCategoryList: Array<DisplayCategoryModel>, CategoryList: Array<DisplayCategoryModel>) {
        CategoryList = CategoryList.sort(function (a, b) { return a.OrderBy - b.OrderBy });
        CategoryList.forEach(t => {
            t.Name = t.Name;// new Array(t.Level * 2 + 1).join("-") + t.Name;
            RetCategoryList.push(t);
            // t.ChildCategorys = t.ChildCategorys.sort(function(a,b){return a.OrderBy-b.OrderBy});
            this.iterateCategory(RetCategoryList, t.ChildCategorys);
        });
    }

    ready($event: any, item) {
        console.log($event);

        if (!item) return;

        let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;

        var _this = this;
        var itemValue = this.CustomFormSetting.CustomFormItemSettings.filter(t => t.Key == item.Key).map(t => t.Value);
        if (itemValue.length > 0 && itemValue[0]) {

            var itemObject = JSON.parse(itemValue[0]);
            // debugger;
            if (/(jpg|jpeg|bmp|png|gif|)$/.test(itemObject.Ext.replace(".", "")))
                $event.fileInputOpts.initialPreview = ['<img src="' + itemObject.Url + '" class="file-preview-image" style="width: auto; height: auto; max-width: 100%; max-height: 100%;">'];
            else
                $event.fileInputOpts.initialPreview = [itemObject.Url];
            $event.fileInputOpts.initialPreviewConfig = [
                { caption: itemObject.FileName, type: itemObject.Ext.replace(".", ""), size: itemObject.Size, key: itemObject.Id, url: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl("/Attachment/RemoveFile")), showDrag: false }
            ];
            $event.fileInputOpts.preferIconicPreview = true;

            this.formModel.value.CustomFormSetting.CustomFormItemSettings.filter(t => t.Key == item.Key).forEach(element => {
                element.Value = itemValue[0];
            });
        }
        else {
            $event.fileInputOpts.initialPreview = [];
            $event.fileInputOpts.initialPreviewConfig = [];
            $event.fileInputOpts.preferIconicPreview = true;
            this.formModel.value.CustomFormSetting.CustomFormItemSettings.filter(t => t.Key == item.Key).forEach(element => {
                element.Value = "";
            });
        }

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

        let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;
        this.formModel.value.CustomFormSetting.CustomFormItemSettings.forEach(element => {
            if (element.Key === item.Key) {
                element.Value = JSON.stringify($event.data.response.DataObject);
            }
        });

    }

    filedeleted($event: any, item) {
        this.formModel.value.CustomFormSetting.CustomFormItemSettings.forEach(element => {
            if (element.Key === item.Key) {
                element.Value = "";
            }
        });
    }

    fileuploaderror($event: any) {
        console.log($event);
    }

}
