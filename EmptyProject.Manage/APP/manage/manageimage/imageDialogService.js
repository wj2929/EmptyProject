define(["app", "common", "bootstrapdialog"], function (app, common) {

    app.directive('manageImageModelDetail', function () {
        return {
            restrict: 'EA',
            link: function (scope, element, attrs) {
                scope.getContentUrl = function () {
                    return common.makeTemplateUrl(common.format('manage/{0}/_image_model_detail.html', attrs["type"]));
                }
            },
            template: '<div ng-include="getContentUrl()"></div>'
        };
    });

    angular.module('manage-image-dialog', []).service('manage-image-dialog',
	["simple-angular-dialog", "$http",
	function (SADialog, $http) {
	    this.open = function (Item, Type) {
	        var model = {
	            entity: Item,
	            SelectImage: {},
	            ImageList: {},
	            Type: Type
	        };

	        //切换图片
	        model.SwitchImage = function (ImageItem) {
	            angular.forEach(model.ImageList, function (value, key) {
	                value.Selected = false;
	            });
	            ImageItem.Selected = true;
	            model.SelectImage = ImageItem;
	        }

	        model.ViewImage = function (ImageSrc) {
	            window.open(ImageSrc);
	        }

	        //删除图片
	        model.DeleteImage = function (ImageItem) {
	            $.messager.confirm("询问", "确定删除吗？", function () {
	                $http.post("/Attachment/Delete", { Id: ImageItem.Id }).success(function (data) {
	                    if (data.State) {
	                        loadImageList(Item, model, Type);
	                    }
	                    else
	                        $.messager.alert(data.Message);
	                });
	            });
	        }

	        //上传图片
	        model.UploadImage = function () {
	            common.upload({
	                //上传文件接收地址
	                uploadUrl: "/Attachment/UploadImage",
	                //选择文件后，发送文件前自定义事件
	                //file为上传的文件信息，可在此处做文件检测、初始化进度条等动作
	                beforeSend: function (file) {
	                    if (file.name.toLowerCase().indexOf(".jpg") == -1 && file.name.toLowerCase().indexOf(".png") == -1 && file.name.toLowerCase().indexOf(".bmp") == -1) {
	                        $.messager.alert("要求文件扩展名:.jpg,.png,.bmp！");
	                        return false;
	                    }
	                    else
	                        return true;
	                },
	                appendFormData: function (fd) {
	                    fd.append("RelationId", Item.Id);
	                    fd.append("Type", Type);
	                },
	                //文件上传完成后回调函数
	                //res为文件上传信息
	                callback: function (res) {
	                    var data = JSON.parse(res);
	                    if (data.State) {
	                        loadImageList(Item, model, Type);
	                    }
	                    else
	                        $.messager.alert(data.Message);

	                },
	                //返回上传过程中包括上传进度的相关信息
	                //详细请看res,可在此加入进度条相关代码
	                uploading: function (res) {

	                }
	            });
	        }

	        loadImageList(Item, model, Type);

	        SADialog.open("manageimage",
                            common.makeTemplateUrl("manage/manageimage/_index.html"),
                            model,
                            {
                                Title: "管理图片",
                                dialogClass: "modal-lg",
                                Buttons: [
                                {
                                    value: "关闭",
                                    callback: function (result) {
                                        result.$parent.close();
                                    }
                                }]
                            });
	    }

	    //获取附件（图片）列表
	    function loadImageList(Item, model, Type) {
	        $http.post("/Attachment/GetList", { RelationId: Item.Id, Type: Type }).success(function (data) {
	            if (data.State) {
	                model.ImageList = data.DataObject;
	                angular.forEach(model.ImageList, function (value, key) {
	                    value.Selected = false;
	                });
	                if (model.ImageList.length > 0) {
	                    model.ImageList[0].Selected = true;
	                    model.SelectImage = model.ImageList[0];
	                }
	            }
	            else
	                $.messager.alert(data.Message);
	        });

	    }
	} ]);
});