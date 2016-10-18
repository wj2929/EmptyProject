///*---LEFT BAR ACCORDION----*/
//$(function() {
//    $('#nav-accordion').dcAccordion({
//        eventType: 'click',
//        autoClose: true,
//        saveState: true,
//        disableLink: true,
//        speed: 'slow',
//        showCount: false,
//        autoExpand: true,
////        cookie: 'dcjq-accordion-1',
//        classExpand: 'dcjq-current-parent'
//    });
//});

var TableMinHeight = 300;
var Script = function () {

//    sidebar dropdown menu auto scrolling

    jQuery('#sidebar .sub-menu > a').click(function () {
        var o = ($(this).offset());
        diff = 250 - o.top;
        if(diff>0)
            $("#sidebar").scrollTo("-="+Math.abs(diff),500);
        else
            $("#sidebar").scrollTo("+="+Math.abs(diff),500);
    });

//    sidebar toggle



// custom scrollbar
    //$("#sidebar").niceScroll({styler:"fb",cursorcolor:"#e8403f", cursorwidth: '3', cursorborderradius: '10px', background: '#404040', spacebarenabled:false, cursorborder: ''});

    //$("html").niceScroll({styler:"fb",cursorcolor:"#e8403f", cursorwidth: '6', cursorborderradius: '10px', background: '#404040', spacebarenabled:false,  cursorborder: '', zindex: '1000'});

// widget tools

    jQuery('.panel .tools .icon-chevron-down').click(function () {
        var el = jQuery(this).parents(".panel").children(".panel-body");
        if (jQuery(this).hasClass("icon-chevron-down")) {
            jQuery(this).removeClass("icon-chevron-down").addClass("icon-chevron-up");
            el.slideUp(200);
        } else {
            jQuery(this).removeClass("icon-chevron-up").addClass("icon-chevron-down");
            el.slideDown(200);
        }
    });

    jQuery('.panel .tools .icon-remove').click(function () {
        jQuery(this).parents(".panel").parent().remove();
    });


//    tool tips

    $('.tooltips').tooltip();

//    popovers

    $('.popovers').popover();



// custom bar chart

    if ($(".custom-bar-chart")) {
        $(".bar").each(function () {
            var i = $(this).find(".value").html();
            $(this).find(".value").html("");
            $(this).find(".value").animate({
                height: i
            }, 2000)
        })
    }


} ();

$(function () {
    function responsiveView() {
        var wSize = $(window).width();
        if (wSize <= 768) {
            $('#container').addClass('sidebar-close');
            $('#sidebar > ul').hide();
        }

        if (wSize > 768) {
            $('#container').removeClass('sidebar-close');
            $('#sidebar > ul').show();
        }
    }
    $(window).on('load', responsiveView);
    $(window).on('resize', responsiveView);
});


$(function () {
    $('.tooltips').tooltip();
})

$(function () {
    $("#language>ul>li>a").click(function () {
        var lang = $(this).attr("lang");
        location.href = location.href.replace("langz_code=zh", "langz_code=" + lang).replace("langz_code=en", "langz_code=" + lang);
    });
    var lang = $.url().param()["langz_code"];
    $("#language .username").html(lang == "zh" ? "中文" : (lang == "en" ? "English" : "未知"));
});

; (function (window, document) {
    var myUpload = function (option) {
        var file,
            fd = new FormData(),
            xhr = new XMLHttpRequest(),
            loaded, tot, per, uploadUrl, input;

        input = document.createElement("input");
        input.setAttribute('id', "myUpload-input");
        input.setAttribute('type', "file");
        input.setAttribute('name', "files");

        input.click();

        uploadUrl = option.uploadUrl;
        callback = option.callback;
        uploading = option.uploading;
        beforeSend = option.beforeSend;
        appendFormData = option.appendFormData;

        input.onchange = function () {
            file = input.files[0];
            if (beforeSend instanceof Function) {
                if (beforeSend(file) === false) {
                    return false;
                }
            }

            if(appendFormData instanceof Function)
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
    };

    window.myUpload = myUpload;
})(window, document);



$.format = function (source, params) {
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
};

Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

function SyncPost(Url, Param) {
    var returnData;

    $.ajax({
        url: Url,
        type: "post",
        async: false,
        data: Param,
        success: function (data) {
            returnData = data;
        }
    });

    return returnData;
}


//添加"正在操作"标签
function addDo(message, autoRemove) {
    if (autoRemove == null) autoRemove = true;
    if (message == null || message == "")
        message = "正在保存...";
    $("body").append("<em class='doing'>" + message + "</em>");
    if (autoRemove)
        setTimeout("removeDo(300)", 500);
};
//移除"正在操作"标签
function removeDo(outTime) {
    $("body").children("em.doing").animate({ opacity: 0 }, outTime, function () {
        $(this).remove();
    });
};
