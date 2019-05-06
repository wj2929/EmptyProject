//弹出层代码开始//////////////////////////////////////////////////
//网页
function webLay(layTitle, layLink, layMode) {
    showOverlay("overlayWeb", layMode, null, null, layTitle, layLink);
};
//网页(无关闭功能)
function webLayX(layTitle, layLink, layMode) {
    showOverlay("overlayWeb", layMode, 'noClose', null, layTitle, layLink);
};
//代码
function codeLay(layTitle, layContent, layMode) {
    showOverlay("overlayCode", layMode, null, null, layTitle, layContent);
};
//代码(无关闭功能)
function codeLayX(layTitle, layContent, layMode) {
    showOverlay("overlayCode", layMode, 'noClose', null, layTitle, layContent);
};
//消息
function msgLay(layTitle, layContent, layMode, layCallBack) {
    showOverlay("overlayMsg", layMode, null, null, layTitle, layContent, null, null, layCallBack);
};
//消息(无关闭功能)
function msgLayX(layTitle, layContent, layMode) {
    showOverlay("overlayMsg", layMode, 'noClose', null, layTitle, layContent);
};
//警示消息(无关闭功能)
function msgLayV(layContent) {
    showOverlay("overlayMsgV", "warning", null, null, null, layContent);
};
//确认
function yonLay(layTitle, layContent, layCallBack, layRefValue) {
    showOverlay("overlayYoN", null, null, null, layTitle, layContent, null, null, layCallBack, layRefValue);
};
//确认
function yonLayX(layTitle, layContent, layCallBack, layRefValue) {
    showOverlay("overlayYoN", null, 'noClose', null, layTitle, layContent, null, null, layCallBack, layRefValue);
};
//showOverlay
function showOverlay(layType, layMode, layEffect, layTheme, layTitle, layContent, layWidth, layHeight, layCallBack, layRefValue) {
    var widthMin = window.top.$(window).width() * 0.3; //小弹出层宽度
    var widthMid = window.top.$(window).width() * 0.5; //中弹出层宽度
    var widthMax = window.top.$(window).width() * 0.8; //大弹出层宽度
    var widthDefault = widthMin;
    //设置HTML代码
    var layBox = "<div id='LayBox'><div class='LayCover'></div></div>"; //遮罩层
    var layMain = "<div class='LayMain'></div>"; //弹出层
    var layMainC = "<div class='LayMain-t'><h2 class='title'>" + layTitle + "</h2></div><div class='LayMain-m'></div>"; //弹出层内容
    var layMainClose = "<a href='javascript:closeLay()' class='close' title='关闭'>×</a>"//弹出层关闭连接
    var layMainWeb = "<div class='m_c web_c'><iframe class='LayMainFrame' scrolling='no' allowtransparency='true' frameborder='0'></iframe></div>"; //网页HTML
    var layMainCodeC = "<div class='m_c code_c'></div>"; //消息、确认内容HTML
    var layMainMsgC = "<div class='m_c msg_c'></div>"; //消息、确认内容HTML
    var layMainMsgF = layMainCodeF = "<div class='m_f msg_f'></div>"; //消息、确认脚步HTML
    var msgButton = codeButton = "<input type='button' value='确定' class='form_button button_close' />"; //消息取消按钮
    var yonButton = "<input type='button' onclick='javascript:layCallBackResult(true)' value='是' class='form_button button_yes' /><input type='button' onclick='javascript:layCallBackResult(false)' value='否' class='form_button button_no' />"; //确认是否按钮
    //弹出框代码
    if (window.top.$("#LayBox").size()) {
        window.top.$("#LayBox").append(layMain);
        if (layType == "overlayMsgV") {
            window.top.$("#LayBox>.LayMain:last").html(layContent);
        } else {
            window.top.$("#LayBox>.LayMain:last").append(layMainC);
        };
        if (window.top.$(".LayMain:last").prev().is(".LayMain")) {
            window.top.$(".LayMain").not(":last").css("z-index", "9900");
        };
    } else {
        window.top.$("body").append(layBox);
        window.top.$("#LayBox").append(layMain);
        if (layType == "overlayMsgV") {
            window.top.$("#LayBox>.LayMain:last").html(layContent);
        } else {
            window.top.$("#LayBox>.LayMain:last").append(layMainC);
        };
    };
    //判断是否加入关闭功能
    if (layEffect != "noClose") {
        window.top.$(".LayMain:last>.LayMain-t").append(layMainClose);
    };
    //判断总宽度，左右居中定位
    if (!layMode) {
        window.top.$(".LayMain:last").css("margin-left", -widthDefault / 2);
        window.top.$(".LayMain:last").width(widthDefault);
    } else if (layMode == "min") {
        window.top.$(".LayMain:last").css("margin-left", -widthMin / 2);

        window.top.$(".LayMain:last").width(widthMin);
    } else if (layMode == "mid") {
        window.top.$(".LayMain:last").css("margin-left", -widthMid / 2);
        window.top.$(".LayMain:last").width(widthMid);
    } else if (layMode == "max") {
        window.top.$(".LayMain:last").css("margin-left", -widthMax / 2);
        window.top.$(".LayMain:last").width(widthMax);
    } else if (layMode == "warning") {
        window.top.$(".LayMain:last").css({ width: "80%", "margin-left": "-40%", color: "#FFFFFF", top: "50%", "text-align": "center" });
    } else {
        window.top.$(".LayMain:last").css("margin-left", -layMode / 2);
        window.top.$(".LayMain:last").width(layMode);
    };

    if (layType == "overlayWeb") {
        //弹出网页
        window.top.$(".LayMain:last>.LayMain-m").append(layMainWeb);
        window.top.$(".LayMain:last>.LayMain-m>.m_c").find("iframe.LayMainFrame").attr("src", layContent);
        //window.top.$(".LayMain:last").width
    } else if (layType == "overlayCode") {
        //弹出消息
        window.top.$(".LayMain:last>.LayMain-m").append(layMainCodeC);
        window.top.$(".LayMain:last>.LayMain-m>.m_c").html(layContent);
        if (layEffect != "noClose") {
            window.top.$(".LayMain:last>.LayMain-m").append(layMainCodeF);
            window.top.$(".LayMain:last>.LayMain-m>.m_f").append(codeButton);
        }; //判断是否有关闭功能
        alert("ddd");
    } else if (layType == "overlayMsg") {
        //弹出消息
        window.top.$(".LayMain:last>.LayMain-m").append(layMainMsgC);
        window.top.$(".LayMain:last>.LayMain-m>.m_c").html(layContent);
        if (layEffect != "noClose") {
            window.top.$(".LayMain:last>.LayMain-m").append(layMainMsgF);
            window.top.$(".LayMain:last>.LayMain-m>.m_f").append(msgButton);
        }; //判断是否有关闭功能
    } else if (layType == "overlayYoN") {
        //弹出确认
        window.top._ref = layCallBack;
        window.top._refValue = layRefValue;
        window.top.$(".LayMain:last>.LayMain-m").append(layMainMsgC + layMainMsgF);
        if (!layContent) {
            window.top.$(".LayMain:last>.LayMain-m>.m_c").html("确认要" + layTitle + "吗？");
        } else {
            window.top.$(".LayMain:last>.LayMain-m>.m_c").html(layContent);
        };
        window.top.$(".LayMain:last>.LayMain-m>.m_f").append(yonButton);
    };
    window.top.$("input.button_close,input.button_yes,input.button_no").unbind().click(function () { closeLay(); });
};
//处理判断窗口返回结果
function layCallBackResult($e) {
    if (window.top._ref) {
        window.top._ref($e, window.top._refValue);
    };
    closeLay();
};
//层之间传值
function GetIframe(Num) {
    if (Num < 0) {
        var n = window.top.$("#LayBox>.LayMain").size() + Num - 1;
        return window.top.frames[n];
    } else if (Num > 0) {
        //var c = "iframe_main_" + Num;
        var n = Num - 1;
        return window.top.frames[n];
    } else {
        return this;
    }
}
//绑定ESC键盘事件
$(document).keydown(function (evt) {
    evt = (evt) ? evt : window.event;
    if (evt.keyCode == 27 && window.top.$(".LayMain:last>.LayMain-t a.close").size() > 0) {
        //if (evt.keyCode == 27) {
        closeLay();
    }
});
//关闭弹出层
function closeLay(Num) {
    if (Num == 0) {
        window.top.$("#LayBox").remove();
    } else if (Num > 0) {
        var layNum = window.top.$(".LayMain").size();
        if (layNum < Num) {
            msgLay("提示", "你要关闭的层不存在！");
        } else {
            if (layNum == Num) {
                window.top.$(".LayMain").slice(-2).css("z-index", "9910");
            };
            window.top.$(".LayMain").eq(Num - 1).remove();
        };
    } else if (!Num) {
        if (window.top.$(".LayMain:last").prev().is(".LayMain")) {
            window.top.$(".LayMain").slice(-2).css("z-index", "9910");
            window.top.$(".LayMain:last").remove();
        } else {
            window.top.$("#LayBox").remove();
        };
    };
};
//弹出层代码结束//////////////////////////////////////////////////

//自动更新高度开始//////////////////////////////////////////////////
window.setInterval("reinitIframe()", 50); //定时不间断更新高度
function reinitIframe() {
    try {
        if (window.top.$(".LayMain:last").size() > 0) {
            //处理弹出层的高度适应问题
            var cFrameH = window.top.$(".LayMain:last").find(".LayMainFrame").contents().find("body").height();
            var cMcH = window.top.$(".LayMain:last").find(".m_c").contents().height();
            var cH = cFrameH > cMcH ? cFrameH : cMcH; //判断内容高度
            $(".LayMain:last .LayMainFrame").height(cFrameH);
            if (window.top.$(".LayMain:last").find(".m_f").size() > 0) {
                var wMaxH = $(window).height() - 212; //弹出层相对于浏览器上下空间总高度带按钮+标题栏高度+按钮高度
            } else {
                var wMaxH = $(window).height() - 152; //弹出层相对于浏览器上下空间总高度+标题栏高度+内容部分内间距
            };
            if (cH > wMaxH) {
                $(".LayMain:last .m_c").height(wMaxH);
            } else {
                $(".LayMain:last .m_c").height(cH + 10);
            };
        };
    } catch (e) { };
};
//自动更新高度结束//////////////////////////////////////////////////