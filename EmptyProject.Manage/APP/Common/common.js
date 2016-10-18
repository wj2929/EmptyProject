define(["jquery"], function ($) {
    'use strict';
    return {
        templateVersion: "3.1",
        TableMinHeight:300,

        //判空
        isEmpty: function (object) {
            if (!object) {
                return true;
            }
            if (object instanceof Array && object.length === 0) {
                return true;
            }
            return false;
        },
        //格式化数据
        format: function (source, params) {
            if (arguments.length == 1)
                return function () {
                    var args = $.makeArray(arguments);
                    args.unshift(source);
                    return $.format.apply(this, args);
                };
            if (arguments.length > 2 && params.constructor != Array) {
                params = $.makeArray(arguments).slice(1);
            }
            if (params.constructor != Array) {
                params = [params];
            }
            $.each(params, function (i, n) {
                source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
            });
            return source;
        },
        upload: function (option) {
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
        },
        addDo: function (message, autoRemove) {
            if (autoRemove == null) autoRemove = true;
            if (message == null || message == "")
                message = "正在保存...";
            $("body").append("<em class='doing'>" + message + "</em>");
            if (autoRemove)
                setTimeout("removeDo(300)", 500);
        },
        removeDo: function (outTime) {
            $("body").children("em.doing").animate({ opacity: 0 }, outTime, function () {
                $(this).remove();
            });
        },
        makeTemplateUrl: function (templateUrl) {
            return templateUrl + (templateUrl.indexOf("?") > -1 ? "&" : "?") + "v=" + this.templateVersion;
        }
    }
});
