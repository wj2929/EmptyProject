$(function () {

    AjaxLoad(); //页面加载、异步加载均触发
    openMenu(1); //默认打开菜单
    //表单鼠标交互颜色
    $("ul.module_form>li:odd").addClass("bg");

    //站点切换
    $("#site_select>ol").hide();
    $("#site_select").hover(function () {
        $(this).addClass("activation");
        $(this).children("ol").show();
    }, function () {
        $(this).removeClass("activation");
        $(this).children("ol").hide();
    });

    //侧边栏菜单伸展
    $("#side_menu>.menu>.menu-t").live("click", function () {
        if ($(this).next().is(":hidden")) {
            $(this).find("em").addClass("open");
            $(this).next().show();
        } else {
            $(this).find("em").removeClass("open");
            $(this).next().hide();
        };
        SideHorCenterH(); //Main与Side高度自适应
        return false;
    });
    MenuInitialization();
    //标识当前点击并打开的菜单
    $("#side_menu>.menu>.menu-c li>a").live("click", function () {
        $("#side_menu>.menu>.menu-c li>a").removeClass("curr");
        $(this).addClass("curr");
    });
    $("#head a,#nav a").live("click", function () {
        $("#side_menu>.menu>.menu-c li>a").removeClass("curr");
    });

})//自加载结束

function MenuInitialization() {
    //无限极菜单交互
    $("#side_menu>.menu>.menu-c li ul").hide();
    $("#side_menu>.menu>.menu-c li>a").prepend("<span class='icon'></span>");
    $("#side_menu>.menu>.menu-c li:has(ul)>a").addClass("close");
    $("#side_menu>.menu>.menu-c li:has(ul)>a>.icon").toggle(function () {
        $(this).parent().removeClass("close");
        $(this).parent().next().show();
        SideHorCenterH(); //Main与Side高度自适应
    }, function () {
        $(this).parent().addClass("close");
        $(this).parent().next().hide();
        SideHorCenterH(); //Main与Side高度自适应
    });
}

//高级搜索、简单搜索切换
function searchControl() {
    if ($(".search_advanced").is(":hidden")) {
        $(".search_advanced").show();
        $(".search_simple").hide();
    } else {
        $(".search_advanced").hide();
        $(".search_simple").show();
    };
    //return false;
};

//改变窗体大小加载部分
$(window).resize(function () {
    WinChange(); //页面改变触发
});
	
//页面加载、异步加载完成后触发
function AjaxLoad() {
    WinChange(); //页面改变触发
    tabsMenu(); //TAB
};

window.setInterval("reinitLoad1()", 50);//定时不间断更新高度
function reinitLoad1() {
    try {
		var cFrameH = window.top.$("#centerFrame").contents().find("body").height();
		window.top.$("#centerFrame").height(cFrameH);
		CenterOrMainH(); //Main与centerFrame高度自适应
    } catch (e) { };
};

//表格鼠标交互颜色
$("table.module_table>tbody>tr").live('mouseover',function () {
        $(this).addClass("curr");
    });
$("table.module_table>tbody>tr").live('mouseout',function () {
        $(this).removeClass("curr");
    });
$("ul.module_form").live('load',function(){
		$(this).$("li:odd").addClass("bg");
	});

//页面加载、浏览器缩放后触发
function WinChange() {
    MinBroW(1000); //限制浏览器最小宽度为1000
    SideHorCenterH(); //Main与Side高度自适应
    //侧边栏菜单显示、隐藏(原在页面自加载部分，因Chrome在窗口缩放的时候没有及时获取高度，现加入resize调用)
    $("#nav>.side_col>a").click(function () {
        var SideW = $("#side").width();
        if ($("#side").is(":hidden")) {
            $(this).removeClass("close");
            $("#center").animate({"margin-left":"+=" + SideW},0);
            $("#side").show();
        } else {
            $(this).addClass("close");
            $("#center").animate({"margin-left":"-=" + SideW},0);
            $("#side").hide();
        };
        return false;
    });
};

//限制浏览器最小宽度
function MinBroW(MinW) {
    var currBroW = $(window).width();
    if (currBroW < MinW) {
        $("#head,#nav,#main").width(MinW);
    } else {
        $("#head,#nav,#main").width("100%");
    };
};
	
//Main与Side高度自适应
function SideHorCenterH() {
    var TopH = $("#head").height() + $("#nav").height();
    var MainH = $(window).height() - TopH;
    var SideMenuH = $("#side_menu").height() + 48; //加css中定义的side_menu上下共20px的间距，为消除高版本IE及FF、Chrome浏览器显示URL时出现的28px高度遮挡（高版本IE为28px，FF为22px，Chrome为23px）
    var CurrMainH = MainH > SideMenuH ? MainH : SideMenuH;
    $("#main").height(CurrMainH);
    CenterOrMainH(); //Main与centerFrame高度自适应
};
	
//Main与centerFrame高度自适应
function CenterOrMainH() {
    var MainH = $("#main").height();
    var cFrameH = $("#centerFrame").height() + 30; //加css中定义的center上下个15px的间距
    var CurrMainH = MainH > cFrameH ? MainH : cFrameH;
    $("#main").height(CurrMainH);
};

//TABS
function tabsMenu(openNum) {
	if (!openNum) {
		openNum = 0;
	} else {
		openNum = openNum - 1;	
	};
	//隐藏所有选项卡内容
	$(".tabsMenu ul.tabs_con>li").hide();
	//打开默认选项卡
	$(".tabsMenu").each(function(){
		$(this).find("ul.tabs_nav>li").eq(openNum).addClass("curr");
		var openID = $(this).find("ul.tabs_nav>li").eq(openNum).find("a").attr("href");
		$(this).find("ul.tabs_con").find("li" + openID).addClass("curr").show();
	});
	//选项卡点击触发
	$(".tabsMenu ul.tabs_nav>li>a").click(function(){
		var ShowId = $(this).attr("href");
		var noClick = $(this).parent().hasClass("no_click");
		if (!noClick) {
			$(this).parent().siblings().removeClass("curr");
			$(this).parent().addClass("curr");
			$(this).parent().parent().next(".tabs_con").children().hide();
			$(this).parent().parent().next(".tabs_con").find(ShowId).show().addClass("curr");
		}
		return false;
	});	
};
	
//打开指定的左侧菜单
function openMenu(num) {
    $("#side_menu>.menu").eq(num - 1).children(".menu-t").find("em").addClass("open");
    $("#side_menu>.menu").eq(num - 1).children(".menu-c").show();
};
