(function($) {

    $.fn.pager = function(options) {

        var opts = $.extend({}, $.fn.pager.defaults, options);

        return this.each(function() {
            $(this).empty().append(renderpager(parseInt(options.pagenumber), parseInt(options.pagecount), options.buttonClickCallback,options.allcount));
            $('.pages span').mouseover(function() { document.body.style.cursor = "pointer"; }).mouseout(function() { document.body.style.cursor = "auto"; });
        });
    };

    function renderpager(pagenumber, pagecount, buttonClickCallback,allcount) {
        var $pager = $('<div class="pages">共有：'+allcount+'条数据 | 分页：'+pagenumber+'/'+pagecount+' | 页码：</div>');
        $pager.append(renderButton('首页', pagenumber, pagecount, buttonClickCallback)).append(renderButton('上一页', pagenumber, pagecount, buttonClickCallback));
        $pager.append(renderButton('下一页', pagenumber, pagecount, buttonClickCallback)).append(renderButton('末页', pagenumber, pagecount, buttonClickCallback));
        $pager.append("<a href='javascript:GoPage($(\"#PageCount\").val());' id='PageGo'>跳转</a>：<input type='text' name='PageCount' id='PageCount' size='3' value='"+pagenumber+"' title='设定要跳转的页码' />");
        return $pager;
    }
    function renderButton(buttonLabel, pagenumber, pagecount, buttonClickCallback) {

        var $Button = $('<span class="pgNext">' + buttonLabel + '</span>');

        var destPage = 1;
        switch (buttonLabel) {
            case "首页":
                destPage = 1;
                break;
            case "上一页":
                destPage = pagenumber - 1;
                break;
            case "下一页":
                destPage = pagenumber + 1;
                break;
            case "末页":
                destPage = pagecount;
                break;
        }
        if (buttonLabel == "首页" || buttonLabel == "上一页") {
            pagenumber <= 1 ? $Button.addClass('pgEmpty') : $Button.click(function() { buttonClickCallback(destPage); });
        }
        else {
            pagenumber >= pagecount ? $Button.addClass('pgEmpty') : $Button.click(function() { buttonClickCallback(destPage); });
        }

        return $Button;
    }
    $.fn.pager.defaults = {
        pagenumber: 1,
        pagecount: 1,
        allcount :0
    };

})(jQuery);





