$(function(){

$("#sidebarMenu>div").addClass("column_no");
$("#sidebarMenu>div>h3").addClass("column_head");
$("#sidebarMenu>div>ul").addClass("columnBody");
$("#sidebarMenu>div>h3").toggle(function(){
    $(this).parent().removeClass("column_no").addClass("column");
  },function(){
    $(this).parent().removeClass("column").addClass("column_no");
  });

$("#sidebarMenu>div>ul>li").css("padding-left","0").find("ul").prev().toggle(function(){
    $(this).next().show();
    $(this).css("background-position","0 6px");
  },function(){
    $(this).next().hide();
    $(this).css("background-position","-2000px 6px");
});
$("#sidebarMenu>div>ul>li").find("ul").hide().prev().css("background-position","-2000px 6px");
$("#sidebarMenu>div>ul>li a").click(function(){
	$("#sidebarMenu>div>ul>li").find("a.curr").removeClass("curr");
	if ($(this).next().is("ul")) {} else {$(this).addClass("curr");};
  });

$("#sidebarDisplay").toggle(function(){
    $("#sidebarFrame").css("display","none");
	$("#sidebar").width(14);
	$("#sidebarDisplay").css("background-position","-50px 0").attr("title","展开侧栏");
	if ($(window).width() < 1000) {$("#content").width(965)} else {$("#content").width($(window).width()-35)}
  },function(){
    $("#sidebarFrame").css("display","block");
	$("#sidebar").width(214);
	$("#sidebarDisplay").css("background-position","0 0").attr("title","收起侧栏");
	if ($(window).width() < 1000) {$("#content").width(765)} else {$("#content").width($(window).width()-235)}
  });

$("#checkbox_all").toggle(function(){
	$(this).text("全不选")
    $("table[class=list_table]>tbody input[type=checkbox]").attr("checked","checked");
  },function(){
	$(this).text("全选")
    $("table[class=list_table]>tbody input[type=checkbox]").removeAttr("checked");
  });

$("#data_tree>.body li:has(ul)").addClass("son")
$("#data_tree>.body li:has(ul)>table").css("cursor","pointer").toggle(function(){
  $(this).parent("li").removeClass("son");
  $(this).next("ul").css("display","block")
  },function(){
  $(this).parent("li").addClass("son");
  $(this).next("ul").css("display","none")
  });
  
  $("div.search_foot>em.zkss").toggle(function(){
	  $(this).parent().prev().find("li.search_sub").show();
	  $(this).text("简单搜索");
    },function(){
	  $(this).parent().prev().find("li.search_sub").hide();
	  $(this).text("高级搜索");
    }
  );

});

function showMenuNum(Num) {
  var menuNum = Num - 1;
  $("#sidebarFrame").contents().find("#sidebarMenu>div").eq(menuNum).removeClass("column_no").addClass("column");
}

function showMenu(parentMenuName,childMenuName) {
    $("#sidebarFrame").contents().find("#sidebarMenu>div:contains('" + parentMenuName + "')").contents().find("a.curr").removeClass("curr");
    $("#sidebarFrame").contents().find("#sidebarMenu>div:contains('" + parentMenuName + "')").removeClass("column_no").addClass("column").contents().find("li:contains('" + childMenuName + "')").find("a").addClass("curr");
}
