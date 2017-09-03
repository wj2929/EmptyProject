$(function () {
    //附加测试代码
    var navs = $(".navbar-nav");
    //var detached_sidebar = $(".sidebar-detached");
    //var detached_sidebar_navigation = $(".sidebar-detached").find(".navigation");
    $(".container section").each(function (i, item) {
        var sectionId = $(item).attr("id");
        var navItem = navs.find("a[href='#" + sectionId + "']");
        if (navItem.length > 0) {
            navItem = navItem.parent();
            navItem.addClass("dropdown");
            navItem.find(">a").addClass("dropdown-toggle").attr("data-toggle", "dropdown").append("<span class=\"caret\"></span>").on("click", function () {
                $("#HeaderNav li.dropdown").removeClass("open");
                var _thisparent = $(this).parent();
                _thisparent.hasClass("open") ? _thisparent.removeClass("open") : _thisparent.addClass("open");
            });
            navItem.append("<ul class=\"dropdown-menu\" role=\"menu\"></ul>");

            //detached_sidebar_navigation.append($.format("<li><a href=\"#{0}\">{1}</a><ul></ul></li>", sectionId, navItem[0].innerText));
        }
        if ($(item).find("h2").length > 0) {
            //var lastSidebarNavigationItem = detached_sidebar_navigation.find("li").last().find("ul");

            $(jQuery.makeArray($(item).find("h2:gt(0)")).reverse()).each(function (h2i, h2item) {
                var h2s = $(h2item).parent().find("h2");
                var prevh2item = $(h2item).parent().find("h2").eq(h2s.length - h2i - 2);
                var url = prevh2item.html().replace(/\s*<.*/ig, "").replace(/\?.*/igm, "");
                prevh2item.attr("id", url.replace("/", "_") + "_Nav").attr("title", url + "(" + prevh2item.next().html() + ")");
                $(h2item).before(getTestCode(url.split('/')[0] + "_" + url.split('/')[1], prevh2item.find("small").html()));

                //lastSidebarNavigationItem.append($.format("<li><a href=\"#{0}\">{1}</a></li>", url.replace("/", "_") + "_Nav", prevh2item.next().html()));
            });
            //lastSidebarNavigationItem.html(jQuery.makeArray(lastSidebarNavigationItem.find("li")).reverse());
            var lasth2item = $(item).find("h2:last");
            var url = lasth2item.html().replace(/\s*<.*/ig, "").replace(/\?.*/igm, "");
            lasth2item.attr("id", url.replace("/", "_") + "_Nav").attr("title", url + "(" + lasth2item.next().html() + ")");
            $(item).append(getTestCode(url.split('/')[0] + "_" + url.split('/')[1], lasth2item.find("small").html()));

            $(jQuery.makeArray($(item).find("h2"))).each(function (h2i, h2item) {
                navItem.find("ul").append("<li><a href=\"#" + $(h2item).attr("id") + "\">" + $(h2item).attr("title") + "</a></li>")
            });
        }
    });

    $(document).delegate(".form-control.param", "keyup", function () {
        var parentFormDiv = $(this).parent().parent().parent();
        var parentBody = parentFormDiv.parent().parent();
        var p = {};
        parentFormDiv.find("input.form-control.param").each(function (i, item) {
            p[$(this).attr("id").split("_")[2]] = $(this).val();
        });
        var httpmethod = parentBody.attr("httpmethod");
        httpmethod = (httpmethod == "" || httpmethod == null) ? "GET" : httpmethod;
        var highlightstr = ""; //"$.get(\"/" + $.format("{0}/{1}", $(this).attr("id").split("_")[0], $(this).attr("id").split("_")[1]);
        if (httpmethod.toLowerCase() == "get") {
            highlightstr = "$.get(\"/" + $.format("{0}/{1}", $(this).attr("id").split("_")[0], $(this).attr("id").split("_")[1]);
            highlightstr += "?" + jQuery.param(p) + "\");"
        } else if (httpmethod.toLowerCase() == "post") {
            highlightstr = "$.post(\"/" + $.format("{0}/{1}", $(this).attr("id").split("_")[0], $(this).attr("id").split("_")[1]);
            highlightstr += "\"," + JsonUti.convertToString(p) + ");"
        } else if (httpmethod.toLowerCase() == "post(array)") {
            //highlightstr = "$.post(\"/" + $.format("{0}/{1}", $(this).attr("id").split("_")[0], $(this).attr("id").split("_")[1]);
            //highlightstr += "\",[" + JsonUti.convertToString(p) + "]);"

            var str = $.format("var arr = new Array();\nvar model={0};\n", JsonUti.convertToString(p));
            str += "arr.push(model);\n\n";
            str += "$.ajax({\n";
            str += $.format("    url: '{0}/{1}',\n", $(this).attr("id").split("_")[0], $(this).attr("id").split("_")[1]);
            str += '    type: "POST",\n';
            str += '    dataType: "json",\n';
            str += '    contentType: "application/json",\n';
            str += '    data:JSON.stringify(arr)\n',
            str += '});\n';
            highlightstr = str;
        }
        parentBody.find(".highlight").html('<code class="language-javascript">' + hljs.highlight("javascript", highlightstr, true).value + '</code>')
        //parentBody.find(".highlight").html('<code class="language-javascript">' + highlightstr + '</code>');
    });
});

function getTestCode(testInputId, httpmethod) {
    var testCode = "<h3>测试</h3>";
    testCode += "<div class=\"panel panel-default\">";
    testCode += "    <div class=\"panel-heading\">测试代码</div>";
    testCode += "    <div class=\"panel-body\" httpmethod=\"" + httpmethod + "\">";
    testCode += "        <pre class=\"highlight debugcode\">";
    testCode += "        </pre>";
    testCode += "        <div>";
    testCode += "            <div class=\"form-horizontal\"></div><button class=\"btn btn-default\" type=\"submit\" id=\"" + testInputId + "\">测试</button>&nbsp;<button class=\"btn btn-default\" type=\"submit\" id=\"test" + testInputId + "\">清空</button>"; //>测试</button> <input value=\"测试\" type=\"button\" id=\"" + testInputId + "\" />";
    testCode += "        </div>";
    testCode += "    </div>";
    testCode += "    <div class=\"panel-heading\">测试结果</div>";
    testCode += "    <div class=\"panel-body\">";
    testCode += "        <pre class=\"highlight\">";
    testCode += "        </pre>";
    testCode += "    </div>";
    testCode += "</div>";
    return testCode;
}

//空的Guid
function GuidEmpty() {
    return "00000000-0000-0000-0000-000000000000";
}

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

function buildTestContent(testInputId, param) {
    var emptyParam = jQuery.param(param) == "";
    jQuery.each(param, function (name, value) {
        $("#" + testInputId).prev().append($.format('<div class="form-group"><label for="{0}_{1}" class="col-sm-2 control-label">{1}</label><div class="col-sm-10"><input type="text" class="form-control param" id="{0}_{1}" value="{2}"></div></div>', testInputId, name, value));
    });
    var parentBody = $("#" + testInputId).parent().parent();
    var httpmethod = parentBody.attr("httpmethod");
    httpmethod = (httpmethod == "" || httpmethod == null) ? "GET" : httpmethod;
    if (httpmethod.toLowerCase() == "get") {
        $("#" + testInputId).parent().parent().find(".highlight").html('$.get("/' + $.format("{0}/{1}", testInputId.split("_")[0], testInputId.split("_")[1]) + (emptyParam ? '' : '?' + jQuery.param(param)) + '");');
        $("#" + testInputId).on("click", function () {
            var data = {};
            jQuery.each(param, function (name, value) {
                data[name] = parentBody.find($.format("#{0}_{1}", testInputId, name)).val();
            });
            var loadurl = $.format("/{0}/{1}", testInputId.split("_")[0], testInputId.split("_")[1]) + (emptyParam ? '' : '?' + jQuery.param(data));
            $(this).parent().parent().parent().find(".panel-body .highlight").eq(1).load(loadurl, function () { $(this).html("<code class='language-javascript'>" + hljs.highlight("json", $.jsonformat($(this).html().replace(/\&amp;/ig, '&').replace(/\\u0026/ig, '&'))).value + "</code>"); });

        });
    } else if (httpmethod.toLowerCase() == "post") {
        $("#" + testInputId).parent().parent().find(".highlight").html('$.post("/' + $.format("{0}/{1}", testInputId.split("_")[0], testInputId.split("_")[1]) + (emptyParam ? '' : '",' + JsonUti.convertToString(param)) + ');');
        $("#" + testInputId).on("click", function () {
            var data = {};
            jQuery.each(param, function (name, value) {
                data[name] = parentBody.find($.format("#{0}_{1}", testInputId, name)).val();
                if (data[name] == null) {
                    var obj = document.getElementById($.format("{0}_{1}", testInputId, name));
                    data[name] = obj.value;
                }
            });
            var loadurl = $.format("/{0}/{1}", testInputId.split("_")[0], testInputId.split("_")[1]);
            $(this).parent().parent().parent().find(".panel-body .highlight").eq(1).load(loadurl, data, function () {
                $(this).html("<code class='language-javascript'>" + hljs.highlight("json", $.jsonformat($(this).html().replace(/\&amp;/ig, '&').replace(/\\u0026/ig, '&'))).value + "</code>");
            });
        });
    }
    else if (httpmethod.toLowerCase() == "post(array)") {
        //$("#" + testInputId).parent().parent().find(".highlight").html('$.post("/' + $.format("{0}/{1}", testInputId.split("_")[0], testInputId.split("_")[1]) + (emptyParam ? '' : '",[' + JsonUti.convertToString(param)) + ']);');
        var str = $.format("var arr = new Array();\nvar model={0};\n", JsonUti.convertToString(param));
        str += "arr.push(model);\n\n";
        str += "$.ajax({\n";
        str += $.format("    url: '{0}/{1}',\n", testInputId.split("_")[0], testInputId.split("_")[1]);
        str += '    type: "POST",\n';
        str += '    dataType: "json",\n';
        str += '    contentType: "application/json",\n';
        str += '    data:JSON.stringify(arr)\n',
        str += '});\n';
        $("#" + testInputId).parent().parent().find(".highlight").html(str);
        $("#" + testInputId).on("click", function () {
            var data = {};
            jQuery.each(param, function (name, value) {
                data[name] = parentBody.find($.format("#{0}_{1}", testInputId, name)).val();
                if (data[name] == null) {
                    var obj = document.getElementById($.format("{0}_{1}", testInputId, name));
                    data[name] = obj.value;
                }
            });
            var loadurl = $.format("/{0}/{1}", testInputId.split("_")[0], testInputId.split("_")[1]);
            var arr = new Array();
            arr.push(data);
            //return;
            //$(this).parent().parent().parent().find(".panel-body .highlight").eq(1).load(loadurl, xx, function () { $(this).html("<code class='language-javascript'>" + hljs.highlight("json", $.jsonformat($(this).html().replace(/\&amp;/ig, '&').replace(/\\u0026/ig, '&'))).value + "</code>"); });
            var This = $(this);
            $.ajax({
                url: loadurl,
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                //processData: false,
                data: JSON.stringify(arr),
                success: function (data) {
                    This.parent().parent().parent().find(".panel-body .highlight").eq(1).html("<code class='language-javascript'>" + hljs.highlight("json", $.jsonformat(JSON.stringify(data).replace(/\&amp;/ig, '&').replace(/\\u0026/ig, '&'))).value + "</code>");
                    //$(this).parent().parent().parent().find(".panel-body .highlight").eq(1).html("<code class='language-javascript'>" + hljs.highlight("json", $.jsonformat($(this).html().replace(/\&amp;/ig, '&').replace(/\\u0026/ig, '&'))).value + "</code>");
                },
                error: function (data) {
                    alert(JSON.stringify(data));
                }
            });
        });
    }
    $("#test" + testInputId).on("click", function () {
        $(this).parent().parent().parent().find(".panel-body .highlight").eq(1).html("");
    });
}

var JsonUti = {
    //定义换行符  
    n: "",
    //定义制表符  
    t: "",
    //转换String  
    convertToString: function (obj) {
        return JsonUti.__writeObj(obj, 1);
    },
    //写对象  
    __writeObj: function (obj //对象  
        , level //层次（基数为1）  
        , isInArray) { //此对象是否在一个集合内  
        //如果为空，直接输出null  
        if (obj == null) {
            return "null";
        }
        //为普通类型，直接输出值  
        if (obj.constructor == Number || obj.constructor == Date || obj.constructor == String || obj.constructor == Boolean) {
            var v = obj.toString();
            var tab = isInArray ? JsonUti.__repeatStr(JsonUti.t, level - 1) : "";
            if (obj.constructor == String || obj.constructor == Date) {
                //时间格式化只是单纯输出字符串，而不是Date对象  
                return tab + ("\"" + v + "\"");
            }
            else if (obj.constructor == Boolean) {
                return tab + v.toLowerCase();
            }
            else {
                return tab + (v);
            }
        }
        //写Json对象，缓存字符串  
        var currentObjStrings = [];
        //遍历属性  
        for (var name in obj) {
            var temp = [];
            //格式化Tab  
            var paddingTab = JsonUti.__repeatStr(JsonUti.t, level);
            //if (!/\d+/.test(name)) {
            //过滤属性名称中包含[\d+]的字符串
            if (!/\d+/.test(name.replace(/\[\d+\]/, ""))) {
                temp.push(paddingTab);
                //写出属性名  
                //temp.push("\"" + name + "\" : ");
                temp.push(name + ":");
            }
            var val = obj[name];
            if (val == null) {
                temp.push("null");
            }
            else {
                var c = val.constructor;
                if (c == Array) { //如果为集合，循环内部对象  
                    temp.push(JsonUti.n + paddingTab + "[" + JsonUti.n);
                    var levelUp = level + 2; //层级+2  
                    var tempArrValue = []; //集合元素相关字符串缓存片段  
                    for (var i = 0; i < val.length; i++) {
                        //递归写对象  
                        tempArrValue.push(JsonUti.__writeObj(val[i], levelUp, true));
                    }
                    temp.push(tempArrValue.join("," + JsonUti.n));
                    temp.push(JsonUti.n + paddingTab + "]");
                }
                else if (c == Function) {
                    temp.push("[Function]");
                }
                else {
                    //递归写对象  
                    temp.push(JsonUti.__writeObj(val, level + 1));
                }
            }
            //加入当前对象“属性”字符串  
            currentObjStrings.push(temp.join(""));
        }
        return (level > 1 && !isInArray ? JsonUti.n : "") //如果Json对象是内部，就要换行格式化  
            + JsonUti.__repeatStr(JsonUti.t, level - 1) + "{" + JsonUti.n //加层次Tab格式化  
            + currentObjStrings.join("," + JsonUti.n) //串联所有属性值  
            + JsonUti.n + JsonUti.__repeatStr(JsonUti.t, level - 1) + "}"; //封闭对象  
    },
    __isArray: function (obj) {
        if (obj) {
            return obj.constructor == Array;
        }
        return false;
    },
    __repeatStr: function (str, times) {
        var newStr = [];
        if (times > 0) {
            for (var i = 0; i < times; i++) {
                newStr.push(str);
            }
        }
        return newStr.join("");
    }
};

$.jsonformat = function (jsonstr) {
    var text = jsonstr.replace(/\n/g, ' ').replace(/\r/g, ' ');
    var t = [];
    var tab = 0;
    var inString = false;
    for (var i = 0, len = text.length; i < len; i++) {
        var c = text.charAt(i);
        if (inString && c === inString) {
            // TODO: \\"
            if (text.charAt(i - 1) !== '\\') {
                inString = false;
            }
        } else if (!inString && (c === '"' || c === "'")) {
            inString = c;
        } else if (!inString && (c === ' ' || c === "\t")) {
            c = '';
        } else if (!inString && c === ':') {
            c += ' ';
        } else if (!inString && c === ',') {
            c += "\n" + String.space(tab * 2);
        } else if (!inString && (c === '[' || c === '{')) {
            tab++;
            c += "\n" + String.space(tab * 2);
        } else if (!inString && (c === ']' || c === '}')) {
            tab--;
            c = "\n" + String.space(tab * 2) + c;
        }
        t.push(c);
    }
    return t.join('');
}

//function concatpostparam(param) { 
//    
//}


String.space = function (len) {
    var t = [], i;
    for (i = 0; i < len; i++) {
        t.push(' ');
    }
    return t.join('');
};

function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
            + " " + date.getHours() + seperator2 + date.getMinutes()
            + seperator2 + date.getSeconds();
    return currentdate;
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}