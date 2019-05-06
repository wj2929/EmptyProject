//页面AjaxError错误统一处理
$(document).ajaxError(function (e, xhr, settings, exception) {
    if (xhr.responseText.indexOf("抱歉，页面错误，缺少权限") != -1) {
        //webLay('缺少权限', '/error.aspx', 'max')
    }
    else {
        //webLay("服务器异常：" + settings.url, settings.url, "max");
        //alert('error in: ' + settings.url + ' \n' + 'error:\n' + xhr.responseText);
    }
}); 

//空的Guid
function GuidEmpty() {
    return "00000000-0000-0000-0000-000000000000";
}
	
//从子窗口刷新内容
function ParentReLoad(LoadType) {
    window.frames["centerFrame"].ReLoad(LoadType);
    closeLay();
}

//关闭子窗口，调用父页面提示消息
function ParentShowMsg(Message) {
    closeLay();
    window.frames["centerFrame"].msgLay("提示", Message);
}

//获取选择的复选框value
function GetAllChecked(TagName) {
    var checkedvalues = [];
    $("#" + TagName + " input:checked").each(function () {
        checkedvalues.push(this.value);
    });
    return checkedvalues.join(",");
}

//设置复选框（全选/反选）
function SelectAllCheckBox(chAllId, ClassName) {
    var allValue = $("#" + chAllId).attr("checked");
    $("." + ClassName + " input:checkbox").attr("checked", allValue == "checked");
}

//startWith和endWith的扩展
String.prototype.endWith = function (str) {
    if (str == null || str == "" || this.length == 0 || str.length > this.length)
        return false;
    if (this.substring(this.length - str.length) == str)
        return true;
    else
        return false;
    return true;
}

String.prototype.startWith = function (str) {
    if (str == null || str == "" || this.length == 0 || str.length > this.length)
        return false;
    if (this.substr(0, str.length) == str)
        return true;
    else
        return false;
    return true;
}

//验证字符串是否为Guid
//测试："C0869370-70BF-4408-A8CF-72A77BB1D788".IsGuid()
String.prototype.IsGuid = function () {
    return /^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$/.test(this);
}
//从Url获取参数
//测试链接：<a href="?name=abc&sex=男&age=12">test getQueryString</a>
//var args = queryStrings();
//alert(args.name + " | " + args.sex + " | " + args.age);
var queryStrings = function () {//get url querystring
    var params = document.location.search, reg = /(?:^\?|&)(.*?)=(.*?)(?=&|$)/g, temp, args = {};
    while ((temp = reg.exec(params)) != null) args[temp[1]] = decodeURIComponent(temp[2]);
    return args;
};

//添加“正在操作”标签
function addDo(message, autoRemove) {
    if (autoRemove == null) autoRemove = true;
    if (message == null || message == "")
        message = "正在保存...";
    $("body").append("<em class='doing'>" + message + "</em>");
    if(autoRemove)
        setTimeout("removeDo(300)", 500);
};
//移除“正在操作”标签
function removeDo(outTime) {
    $("body").children("em.doing").animate({ opacity: 0 }, outTime, function () {
        $(this).remove();
    });
};

(function ($) {
    $.fn.showLoading = function () {
        this.html('加载中...<img src="/images/large-loading.gif" border=0 />')
    };
    $.fn.showSmallLoading = function () {
        this.html('<img src="/images/large-loading.gif" border=0 />')
    }
})(jQuery)