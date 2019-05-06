function showYesOrNo(returnFun,refParams1,refParams2,winWidth,winHeight)
{
	window.parent.showol(returnFun,refParams1,refParams2,winWidth,winHeight);
}

function showYesOrNo1(returnFun,refParams1,refParams2,winWidth,winHeight)
{
	window.top.showol(returnFun,refParams1,refParams2,winWidth,winHeight);
}

function showYesOrNo2(returnFun,refParams1,refParams2,winWidth,winHeight,title,content)
{
    window.parent.showol2(returnFun,refParams1,refParams2,winWidth,winHeight,title,content);
}

function ShowConfirm(msg, reurl) {
    window.parent.showConfirm(msg, reurl);
}

function showalert(_body)
{
    window.parent.showal(_body);
}
function showalert1(_body)
{
    window.top.showal(_body);
}

function showpage(pageUrl,pageWidth,pageHeight)
{
    window.parent.showpage(pageUrl,pageWidth,pageHeight);
}

function showiframe(_pageUrl,_pageTitle,_pageWidth,_pageHeight)
{
    window.parent.showiframe(_pageUrl + (_pageUrl.indexOf("?") == -1 ? "?" : "&") + "UseCustomIFrameFlag=true", _pageTitle, _pageWidth, _pageHeight);
}

function closeiframe()
{
    parent.closeiframe();
}

function showiframe2(_pageUrl,_pageTitle,_pageWidth,_pageHeight)
{
    window.parent.showiframe2(_pageUrl + (_pageUrl.indexOf("?") == -1 ? "?" : "&") + "UseCustomIFrameFlag=true",_pageTitle,_pageWidth,_pageHeight);
}

function closeiframe2()
{
    parent.closeiframe2();
}


function GetUrlParams()
{
    var args = {};
    if (location.search.length > 1)
    {
        var query=location.search.substring(1);//获取查询串
        var pairs=query.split("&");
        for(var i=0; i < pairs.length; i++ )
        {
            var pos=pairs[i].indexOf('=');//查找name=value
                if(pos==-1)   continue;//如果没有找到就跳过
                var argname=pairs[i].substring(0,pos);//提取name
                var value=pairs[i].substring(pos+1);//提取value
                args[argname]=unescape(value);//存为属性
        }
    }
    return args;
}

//得到选中的值集合，以“，”分开的
//idmatch：过滤全选checkbox
function GetAllCheckedValues(idmatch)
{
    var checkedvalues = [];
    var jqselector = ":checkbox:checked";
    if(idmatch)
        jqselector += "[id*=\'" + idmatch + "\']";
    $(jqselector).each(function(){checkedvalues.push(this.value)});
    return checkedvalues.join(",");
}
window.onload = function() 
{
    if (document.getElementById("asyncZone")) 
    {
        if ($("#asyncZone").length > 0) 
        {
            HideLoadIngPlan();
            $("body").bind("ajaxSend", function() { ShowLoadIngPlan("Loading.."); }).bind("ajaxComplete", function() { HideLoadIngPlan(); });
        }
    }
}


function ShowLoadIngPlan(shwomsg)
{
    $("#LoadingPlan").scrollLeft(10);
    $("#LoadingPlan").scrollLeft(10);
    $("#LoadingPlan").replaceWith("<span id='LoadingPlan'>"+ shwomsg +"</span>"); 
    $("#LoadingPlan").show();
}
function HideLoadIngPlan()
{
    $("#LoadingPlan").hide();
}

function ShowMask()
{
    window.parent.ShowAllMaskLayer();
}

function HideMask()
{
    window.parent.HideAllMaskLayer();
}