import { remove } from 'lodash';
import { CommonService } from 'src/app/services/common.service';
import { BaseForm } from './BaseForm';
import { FormGroup, FormArray, FormBuilder } from "@angular/forms";
import { CustomformService } from "src/app/services/customform.service";
import { ReturnInfoModel } from 'src/app/models/ReturnInfoModel';
import { CustomFormSettingModel } from '../custom-form-setting/CustomFormSettingModels';
import { CustomFormSetting, DisplayCustomFormModel, CustomFormItemSetting } from 'src/app/models/CustomFormModels';
import { CustomformitemValidator } from 'src/app/validators/customformitem-validator';
import * as moment from 'moment';
declare var $: any;
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import '@ckeditor/ckeditor5-build-classic/build/translations/zh-cn.js';


export abstract class BaseCustomForm extends BaseForm {
    public customFormKey: string = "";
    public customFormId: string = "";
    public customFormItemList = [];
    public CustomFormSetting: CustomFormSetting;
    customFormList: Array<DisplayCustomFormModel> = [];
    public childCustomFormItemList=[];
    public Editor = ClassicEditor;
    public newsMediaList = [];
    // 配置语言
    public EditorConfig = {
        language: 'zh-cn',
        height : 300,
        resize_minHeight : 250
    };

    SummernoteConfig = {
        placeholder: '',
        tabsize: 2,
        height: '300px',
        // uploadImagePath: '/api/upload',
        toolbar: [
            ['misc', ['codeview', 'undo', 'redo']],
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['font', ['bold', 'italic', 'underline', 'strikethrough', 'superscript', 'subscript', 'clear']],
            ['fontsize', ['fontname', 'fontsize', 'color']],
            ['para', ['style', 'ul', 'ol', 'paragraph', 'height']],
            ['insert', ['table', 'picture', 'link', 'video', 'hr']]
        ],
        fontNames: ['Helvetica', 'Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Roboto', 'Times']
      }
    daterangepickerLocale: any = {
        format: "YYYY-MM-DD",
        separator: " - ",
        daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
        monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
        applyLabel: '确定',
        cancelLabel: '取消'
    };

    daterangepickerRanges: any = {
        '今天': [moment(), moment()],
        '最近7天': [moment().subtract(6, 'days'), moment()],
        '这个月': [moment().startOf('month'), moment().endOf('month')],
        '今年': [moment().startOf('year'), moment().endOf('year')],
        // '去年': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')],
        '全部': [moment().subtract(19, 'year').startOf('year'), moment()]
    }
    constructor(
        public CustomformService: CustomformService,
        public CommonService: CommonService,
        public fb: FormBuilder) {
        super();

        this.listCustomForm();
    }

    listCustomForm() {
        this.CustomformService.newsMediaPaging(1,12,(data:ReturnInfoModel)=>{
            if(data.State){
                this.newsMediaList = data.DataObject.PageListInfos;
            }
        });

        this.CustomformService.list((data: ReturnInfoModel) => {
            if (data.State) {
                this.customFormList = data.DataObject;
                // debugger;

                this.initSelectSearchItem();
                setTimeout(() => {
                    this.initSelectSearchItem();
                }, 500);
            }
        });
    }

    initSelectSearchItem() {
        setTimeout(() => {
            console.log($('.select-search.customformitem').length);
            let _this = this;
            $('.select-search.customformitem').select2().off("change").on("change", function (e) {
                // debugger;
                console.log(e);
                let customFormItemKey = $(e.target).parent().siblings()[1].value;
                let formpath = $($(e.target).parents("div.form-group")[0]).find("input[name='formpath']").val();
                if(formpath){
                   _this.formModel.get(formpath).setValue(e.target.value);
                   $(e.target).next().find(".select2-selection__rendered").html(` ${$(e.target).find("option:selected").text()}`);
                   $(e.target).next().find(".select2-selection__rendered").attr("title",` ${$(e.target).val()}`);
                }
                else{
                    _this.formModel.get("CustomFormSetting.CustomFormItemSettings." + _this.formModel.value.CustomFormSetting.CustomFormItemSettings.findIndex(t => t.Key == customFormItemKey) + ".Value").setValue(e.target.value);
                    _this.formModel.value.CustomFormSetting.CustomFormItemSettings.filter(t => t.Key == customFormItemKey).forEach(element => {
                        element.Value = e.target.value;
                    });
                }
                // _this.formModel.get("CustomFormSetting.CustomFormItemSettings." + _this.formModel.value.CustomFormSetting.CustomFormItemSettings.findIndex(t => t.Key == customFormItemKey) + ".Value").setValue(e.target.value);
                // _this.formModel.value.CustomFormSetting.CustomFormItemSettings.filter(t => t.Key == customFormItemKey).forEach(element => {
                //     element.Value = e.target.value;
                // });
            });
            $('.select-search.categoryitem').off("change").on("change", function (e) {
                console.log(e);
                let customFormItemKey = $(e.target).parent().siblings()[1].value;
                let formpath = $($(e.target).parents("div.form-group")[0]).find("input[name='formpath']").val();
                if(formpath){
                   _this.formModel.get(formpath).setValue(e.target.value);
                   $(e.target).next().find(".select2-selection__rendered").html(` ${$(e.target).val()}`);
                   $(e.target).next().find(".select2-selection__rendered").attr("title",` ${$(e.target).val()}`);
                }
                else{
                    _this.formModel.get("CustomFormSetting.CustomFormItemSettings." + _this.formModel.value.CustomFormSetting.CustomFormItemSettings.findIndex(t => t.Key == customFormItemKey) + ".Value").setValue(e.target.value);
                    _this.formModel.value.CustomFormSetting.CustomFormItemSettings.filter(t => t.Key == customFormItemKey).forEach(element => {
                        element.Value = e.target.value;
                    });
                }
            }).select2tree();
        }, 200);
    }

    /**
     * 创建子表单
     * @param item 
     * @param index 
     */
    createChildCustomForm(item,index){
        // debugger;

        let CustomFormItemSettings = this.fb.array([]);
        this.childCustomFormItemList.forEach((tt,index1) =>{
            tt.index = index1;
            CustomFormItemSettings.push(this.doCustomFormItem(tt,new CustomFormItemSetting(tt.Key,tt.Name,""),this.childCustomFormItemList));
        });

        let ChildCustomForms = this.formModel.get(`CustomFormSetting.CustomFormItemSettings.${index}.Value.ChildCustomForms`) as FormArray;
        ChildCustomForms.push(this.fb.group({ChildCustomFormItemSettings: CustomFormItemSettings}));

        item.ChildCustomForms.push({});

        this.initSelectSearchItem();
    }

    /**
     * 移除子表单
     * @param item 
     * @param index 
     */
    removeChildCustomForm(item,index,childIndex){
        let ChildCustomForms = this.formModel.get(`CustomFormSetting.CustomFormItemSettings.${index}.Value.ChildCustomForms`) as FormArray;
        ChildCustomForms.removeAt(childIndex);

        item.ChildCustomForms.splice(childIndex, 1);
    }


    CustomFormItemError(FieldName: string) {
        let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;
        let formGroup = CustomFormItemSettings.controls.filter(t => t.value.Key == FieldName)[0] as FormGroup;
        let formControl = formGroup.controls.Value;
        return formControl.invalid && formControl.touched;
    }

    listCustomFormItemById(customFormId) {
        this.customFormId = customFormId;
        this.CustomformService.itemList(customFormId, true, (data: ReturnInfoModel) => {
            this.listCustomFormItemByReturnInfo(data);

            this.initSelectSearchItem();
        });
    }

    listCustomFormItem(customFormKey: string) {
        this.customFormKey = customFormKey;
        if (this.customFormKey !== "") {
            this.CustomformService.itemListByKey(this.customFormKey, true, (data: ReturnInfoModel) => {
                this.listCustomFormItemByReturnInfo(data);

                this.initSelectSearchItem();
            });
        }
    }

    abstract listCustomFormItemByReturnInfo(data: ReturnInfoModel);
    abstract doCustomFormItem(t:any,customFormItemSettingValue:CustomFormItemSetting,childCustomFormItemList:any);

    onFormItemDaterangepickerChange($event: any, item, single: boolean = true) {
        this.formModel.value.CustomFormSetting.CustomFormItemSettings.filter(t => t.Key == item.Key).forEach(element => {
            if ($event.startDate != null) {
                if (single)
                    element.Value = $event.startDate.format("YYYY-MM-DD");
                else {
                    element.Value = $event.startDate.format("YYYY-MM-DD") + " - " + $event.endDate.format("YYYY-MM-DD");
                }

                if (element.Value != null)
                    this.formModel.get("CustomFormSetting.CustomFormItemSettings." + this.formModel.value.CustomFormSetting.CustomFormItemSettings.findIndex(t => t.Key == element.Key) + ".Value").setValue(element.Value);
            }
            else
                element.Value = "";
        });
    }

    changeRangeDaterangepickerWidth() {
        setTimeout(() => {
            $(".md-drppicker.show-ranges").width("600px");
        }, 50);
    }

    saveFormModelCustomFormSetting() {
        let index = 0;
        for (let element of this.formModel.value.CustomFormSetting.CustomFormItemSettings) {

            var customFormItemSetting = this.customFormItemList.find(t => t.Key == element.Key);

            if (element.Value != null && element.Value.constructor == Array)
                element.Value = element.Value.join(",");

            if (customFormItemSetting.FormType == 4) {
                let date = element.Value;
                if (date != null && date.constructor == Object) {
                    if (date.startDate.constructor == String)
                        element.Value = date.startDate;
                    else if (date.startDate.constructor.name == "Moment")
                        element.Value = date.startDate.format("YYYY-MM-DD");
                }
            }
            else if (customFormItemSetting.FormType == 5) {
                let date = element.Value;
                if (date != null && date.constructor == Object) {
                    if (date.startDate.constructor == String)
                        element.Value = date.startDate + " - " + date.endDate;
                    else if (date.startDate.constructor.name == "Moment")
                        element.Value = date.startDate.format("YYYY-MM-DD") + " - " + date.endDate.format("YYYY-MM-DD");
                }
            }
            else if(customFormItemSetting.FormType == 9) {
                element.value = JSON.stringify(this.formModel.get(`CustomFormSetting.CustomFormItemSettings.${index}.Value`).value);
            }
            index++;
        }
    }

    onFormItemWexXinTextChange($event: any, item){
        var textContent = $event.target.value;
        this.formModel.value.CustomFormSetting.CustomFormItemSettings.filter(t => t.Key == item.Key).forEach(element => {
            if(element.Value==""){
                element.Value = JSON.stringify({current:"text",text:textContent,image:"",newsmedia:""});
            }
            else {
                var o = JSON.parse(element.Value);
                o.current="text";
                o.text = textContent;
                element.Value = JSON.stringify(o);
            }
            if (element.Value != null)
                this.formModel.get("CustomFormSetting.CustomFormItemSettings." + this.formModel.value.CustomFormSetting.CustomFormItemSettings.findIndex(t => t.Key == element.Key) + ".Value").setValue(element.Value);
            else
                element.Value ="";

            // if ($event.startDate != null) {
            //     if (single)
            //         element.Value = $event.startDate.format("YYYY-MM-DD");
            //     else {
            //         element.Value = $event.startDate.format("YYYY-MM-DD") + " - " + $event.endDate.format("YYYY-MM-DD");
            //     }

            //     if (element.Value != null)
            //         this.formModel.get("CustomFormSetting.CustomFormItemSettings." + this.formModel.value.CustomFormSetting.CustomFormItemSettings.findIndex(t => t.Key == element.Key) + ".Value").setValue(element.Value);
            // }
            // else
            //     element.Value = "";
        });
    }

    selectNewsMedia($event: any, item: any,childItem:any){
        item.selectNewsMedia =childItem;
        var textContent = $event.target.value;
        this.formModel.value.CustomFormSetting.CustomFormItemSettings.filter(t => t.Key == item.Key).forEach(element => {
            if(element.Value==""){
                element.Value = JSON.stringify({current:"newsmedia",text:textContent,image:"",newsmedia:JSON.stringify(childItem)});
            }
            else {
                var o = JSON.parse(element.Value);
                o.current="newsmedia";
                o.newsmedia = JSON.stringify(childItem);
                element.Value = JSON.stringify(o);
            }
            if (element.Value != null)
                this.formModel.get("CustomFormSetting.CustomFormItemSettings." + this.formModel.value.CustomFormSetting.CustomFormItemSettings.findIndex(t => t.Key == element.Key) + ".Value").setValue(element.Value);
            else
                element.Value ="";
        });
    }

    fileInputOptsWithWexXinForeverMedia: any = {
        uploadUrl: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl("/Attachment/UploadWexXinForeverMedia")),
        maxFilesNum: 1,
        maxFileCount: 1,
        maxImageWidth: 1000,
        maxImageHeight: 1000,
        previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
        initialPreviewFileType: 'image',
        preferIconicPreview: true,
        initialPreviewDownloadUrl: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl("/Attachment/DownloadFile") + '?key={key}'),
        previewFileIconSettings: { // configure your icon file extensions
            'doc': '<i class="fa fa-file-word-o text-primary"></i>',
            'xls': '<i class="fa fa-file-excel-o text-success"></i>',
            'ppt': '<i class="fa fa-file-powerpoint-o text-danger"></i>',
            'pdf': '<i class="fa fa-file-pdf-o text-danger"></i>',
            'txt': '<i class="fa fa-file-text-o text-info"></i>',
            'zip': '<i class="fa fa-file-archive-o text-muted"></i>',
            'htm': '<i class="fa fa-file-code-o text-info"></i>',
            'mov': '<i class="fa fa-file-movie-o text-warning"></i>',
            'mp3': '<i class="fa fa-file-audio-o text-warning"></i>'
        },
        previewFileExtSettings: { // configure the logic for determining icon file extensions
            'doc': function (ext) {
                return ext.match(/(doc|docx)$/i);
            },
            'xls': function (ext) {
                return ext.match(/(xls|xlsx)$/i);
            },
            'ppt': function (ext) {
                return ext.match(/(ppt|pptx)$/i);
            },
            'zip': function (ext) {
                return ext.match(/(zip|rar|tar|gzip|gz|7z)$/i);
            },
            'htm': function (ext) {
                return ext.match(/(htm|html)$/i);
            },
            'mov': function (ext) {
                return ext.match(/(avi|mpg|mkv|mov|mp4|3gp|webm|wmv)$/i);
            },
            'mp3': function (ext) {
                return ext.match(/(mp3|wav)$/i);
            },
            'txt': function (ext) {
                return ext.match(/(txt|ini|csv|java|php|js|css)$/i);
            }
        },
        uploadExtraData: function (previewId, index) {           //传参
            var data = {
                "test": "test"      //此处自定义传参
            };
            return data;
        }
    };

    fileInputOptsWithIdCardFront: any = {
        uploadUrl: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl("/Attachment/UploadIdCardFrontFile")),
        maxFilesNum: 1,
        maxFileCount: 1,
        maxImageWidth: 1000,
        maxImageHeight: 1000,
        previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
        initialPreviewFileType: 'image',
        preferIconicPreview: true,
        initialPreviewDownloadUrl: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl("/Attachment/DownloadFile") + '?key={key}'),
        previewFileIconSettings: { // configure your icon file extensions
            'doc': '<i class="fa fa-file-word-o text-primary"></i>',
            'xls': '<i class="fa fa-file-excel-o text-success"></i>',
            'ppt': '<i class="fa fa-file-powerpoint-o text-danger"></i>',
            'pdf': '<i class="fa fa-file-pdf-o text-danger"></i>',
            'txt': '<i class="fa fa-file-text-o text-info"></i>',
            'zip': '<i class="fa fa-file-archive-o text-muted"></i>',
            'htm': '<i class="fa fa-file-code-o text-info"></i>',
            'mov': '<i class="fa fa-file-movie-o text-warning"></i>',
            'mp3': '<i class="fa fa-file-audio-o text-warning"></i>'
        },
        previewFileExtSettings: { // configure the logic for determining icon file extensions
            'doc': function (ext) {
                return ext.match(/(doc|docx)$/i);
            },
            'xls': function (ext) {
                return ext.match(/(xls|xlsx)$/i);
            },
            'ppt': function (ext) {
                return ext.match(/(ppt|pptx)$/i);
            },
            'zip': function (ext) {
                return ext.match(/(zip|rar|tar|gzip|gz|7z)$/i);
            },
            'htm': function (ext) {
                return ext.match(/(htm|html)$/i);
            },
            'mov': function (ext) {
                return ext.match(/(avi|mpg|mkv|mov|mp4|3gp|webm|wmv)$/i);
            },
            'mp3': function (ext) {
                return ext.match(/(mp3|wav)$/i);
            },
            'txt': function (ext) {
                return ext.match(/(txt|ini|csv|java|php|js|css)$/i);
            }
        },
        uploadExtraData: function (previewId, index) {           //传参
            var data = {
                "test": "test"      //此处自定义传参
            };
            return data;
        }
    };

    fileInputOptsWithIdCardBack: any = {
        uploadUrl: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl("/Attachment/UploadIdCardBackFile")),
        maxFilesNum: 1,
        maxFileCount: 1,
        maxImageWidth: 1000,
        maxImageHeight: 1000,
        previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
        initialPreviewFileType: 'image',
        preferIconicPreview: true,
        initialPreviewDownloadUrl: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl("/Attachment/DownloadFile") + '?key={key}'),
        previewFileIconSettings: { // configure your icon file extensions
            'doc': '<i class="fa fa-file-word-o text-primary"></i>',
            'xls': '<i class="fa fa-file-excel-o text-success"></i>',
            'ppt': '<i class="fa fa-file-powerpoint-o text-danger"></i>',
            'pdf': '<i class="fa fa-file-pdf-o text-danger"></i>',
            'txt': '<i class="fa fa-file-text-o text-info"></i>',
            'zip': '<i class="fa fa-file-archive-o text-muted"></i>',
            'htm': '<i class="fa fa-file-code-o text-info"></i>',
            'mov': '<i class="fa fa-file-movie-o text-warning"></i>',
            'mp3': '<i class="fa fa-file-audio-o text-warning"></i>'
        },
        previewFileExtSettings: { // configure the logic for determining icon file extensions
            'doc': function (ext) {
                return ext.match(/(doc|docx)$/i);
            },
            'xls': function (ext) {
                return ext.match(/(xls|xlsx)$/i);
            },
            'ppt': function (ext) {
                return ext.match(/(ppt|pptx)$/i);
            },
            'zip': function (ext) {
                return ext.match(/(zip|rar|tar|gzip|gz|7z)$/i);
            },
            'htm': function (ext) {
                return ext.match(/(htm|html)$/i);
            },
            'mov': function (ext) {
                return ext.match(/(avi|mpg|mkv|mov|mp4|3gp|webm|wmv)$/i);
            },
            'mp3': function (ext) {
                return ext.match(/(mp3|wav)$/i);
            },
            'txt': function (ext) {
                return ext.match(/(txt|ini|csv|java|php|js|css)$/i);
            }
        },
        uploadExtraData: function (previewId, index) {           //传参
            var data = {
                "test": "test"      //此处自定义传参
            };
            return data;
        }
    };

    fileInputOpts: any = {
        uploadUrl: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl("/Attachment/UploadFile")),
        maxFilesNum: 1,
        maxFileCount: 1,
        maxImageWidth: 1000,
        maxImageHeight: 1000,
        previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
        initialPreviewFileType: 'image',
        preferIconicPreview: true,
        initialPreviewDownloadUrl: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl("/Attachment/DownloadFile") + '?key={key}'),
        previewFileIconSettings: { // configure your icon file extensions
            'doc': '<i class="fa fa-file-word-o text-primary"></i>',
            'xls': '<i class="fa fa-file-excel-o text-success"></i>',
            'ppt': '<i class="fa fa-file-powerpoint-o text-danger"></i>',
            'pdf': '<i class="fa fa-file-pdf-o text-danger"></i>',
            'txt': '<i class="fa fa-file-text-o text-info"></i>',
            'zip': '<i class="fa fa-file-archive-o text-muted"></i>',
            'htm': '<i class="fa fa-file-code-o text-info"></i>',
            'mov': '<i class="fa fa-file-movie-o text-warning"></i>',
            'mp3': '<i class="fa fa-file-audio-o text-warning"></i>'
        },
        previewFileExtSettings: { // configure the logic for determining icon file extensions
            'doc': function (ext) {
                return ext.match(/(doc|docx)$/i);
            },
            'xls': function (ext) {
                return ext.match(/(xls|xlsx)$/i);
            },
            'ppt': function (ext) {
                return ext.match(/(ppt|pptx)$/i);
            },
            'zip': function (ext) {
                return ext.match(/(zip|rar|tar|gzip|gz|7z)$/i);
            },
            'htm': function (ext) {
                return ext.match(/(htm|html)$/i);
            },
            'mov': function (ext) {
                return ext.match(/(avi|mpg|mkv|mov|mp4|3gp|webm|wmv)$/i);
            },
            'mp3': function (ext) {
                return ext.match(/(mp3|wav)$/i);
            },
            'txt': function (ext) {
                return ext.match(/(txt|ini|csv|java|php|js|css)$/i);
            }
        },
        uploadExtraData: function (previewId, index) {           //传参
            var data = {
                "test": "test"      //此处自定义传参
            };
            return data;
        }
    };


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
                $event.fileInputOpts.initialPreview = ['<img src="' + itemObject.Url + '" class="file-preview-image" style="width:auto;height:160px;">'];
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

    readyWithWexXinForeverMedia($event: any, item) {
        console.log($event);

        if (!item) return;

        let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;

        var _this = this;
        var itemValue = this.formModel.value.CustomFormSetting.CustomFormItemSettings.filter(t => t.Key == item.Key).map(t => t.Value);
        if (itemValue.length > 0 && itemValue[0]) {

            var itemObject = JSON.parse(itemValue[0]);
            if(itemObject.image!=""){
                var imageObject = JSON.parse(itemObject.image);
                // debugger;
                if (/(jpg|jpeg|bmp|png|gif|)$/.test(imageObject.Ext.replace(".", "")))
                    $event.fileInputOpts.initialPreview = ['<img src="' + imageObject.Url + '" class="file-preview-image" style="width:auto;height:160px;">'];
                else
                    $event.fileInputOpts.initialPreview = [imageObject.Url];
                $event.fileInputOpts.initialPreviewConfig = [
                    { caption: itemObject.FileName, type: imageObject.Ext.replace(".", ""), size: imageObject.Size, key: imageObject.Id, url: this.CommonService.attachTokenGetParam(this.CommonService.buildUrl("/Attachment/RemoveFile")), showDrag: false }
                ];
                $event.fileInputOpts.preferIconicPreview = true;
            }
            
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

                if (element.Value != null) {
                    try {
                        this.formModel.get("CustomFormSetting.CustomFormItemSettings." + this.formModel.value.CustomFormSetting.CustomFormItemSettings.findIndex(t => t.Key == element.Key) + ".Value").setValue(element.Value);
                    }
                    catch (e) { }
                }
            }
        });

    }

    
    fileuploadedWithWexXinForeverMedia($event: any, item) {
        console.log($event);

        let CustomFormItemSettings = this.formModel.get("CustomFormSetting").get("CustomFormItemSettings") as FormArray;
        this.formModel.value.CustomFormSetting.CustomFormItemSettings.forEach(element => {
            if (element.Key === item.Key) {
                if(element.Value==""){
                    element.Value = JSON.stringify({current:"image",text:"",image:JSON.stringify($event.data.response.DataObject),newsmedia:""});
                }
                else{
                    var o = JSON.parse(element.Value);
                    o.current = "image";
                    o.image = JSON.stringify($event.data.response.DataObject);
                    element.Value = JSON.stringify(o);
                }
                // var dataObject = $event.data.response.DataObject;
                
                // element.Value = JSON.stringify($event.data.response.DataObject);

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

    filedeletedWithWexXinForeverMedia($event: any, item) {
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

    fileclear($event: any, item) {
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

    fileclearWithWexXinForeverMedia($event: any, item) {
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
}
