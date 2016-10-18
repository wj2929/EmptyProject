var PageSize = 1000;
var CurrId = null;
$(function () {
    autoHeight();
    $(window).bind("resize", function () {
        autoHeight();
    });
    getSearchNDList();

    $('.icon-reorder').click(function () {
        if ($('#SideBar').is(":visible") === true) {
            $('#MainContent').removeClass("col-md-9").addClass("col-md-12");
            $('#SideBar').hide();
            $("#container").addClass("sidebar-closed");
        } else {
            $('#MainContent').removeClass("col-md-12").addClass("col-md-9");
            $('#SideBar').show();
            $("#container").removeClass("sidebar-closed");
        }
    });

    //全选或反选
    $(document).undelegate("#CBSelectAll", "click").delegate("#CBSelectAll", "click", function () {
        if ($(this).attr("checked") == "checked")
            $("#gyqytdjbqkbTable tbody input[type='checkbox']").attr("checked", "checked");
        else
            $("#gyqytdjbqkbTable tbody input[type='checkbox']").removeAttr("checked");
    });


    $("#Search").click(function () {
        loadPage(1);
    });

    $("#Export").click(function () {
        Export()
    });


    //批量删除
    $("#Delete").click(function () {
        var IdArr = getSelectIds();
        if (IdArr.length > 0)
            deleteItems(IdArr.join(","));
        else
            $.messager.alert("未选择！");
    });


    //删除
    $(document).undelegate("#gyqytdjbqkbTable button.remove", "click").delegate("#gyqytdjbqkbTable button.remove", "click", function () {
        var Id = $(this).parent().parent().attr("lineid");
        deleteItem(Id);
    });

    //删除
    $(document).delegate("#btnSearchMap_ZDWZ", "click", function () {
        var ZDWZ = $("#ZDWZ").val();
        if (ZDWZ) {
            var templatehtml1 = _.template($('#ZDWZBaiduMapTemplate').html(), { ZDMC: ZDWZ });
            $(".modalpanel2").html(templatehtml1);
            $(".modalpanel2").dialog({
                title: "宗地位置地图",
                dialogClass: "modal-max2",
                buttons: [
                        {
                            text: "关闭",
                            class: "btn btn-default",
                            click: function () {
                                $(this).dialog("close");
                            }
                        }]
            });

        }
        else
            $.messager.alert("请输入宗地位置！");
    });


    //管理图片
    $(document).delegate("#gyqytdjbqkbTable button.manageimage", "click", function () {
        var Id = $(this).parent().parent().attr("lineid");
        CurrId = Id;
        $.post("/gyqytdjbqkb/Single", { Id: Id }, function (data) {
            if (data.State) {
                $("div.modal-dialog").removeClass().addClass("modal-dialog");
                var templatehtml1 = _.template($('#ImageManageModelTemplate').html(), { item: data.DataObject });
                $(".modalpanel").html(templatehtml1);
                $(".modalpanel").dialog({
                    title: "管理图片",
                    dialogClass: "modal-lg",
                    buttons: [
                        {
                            text: "关闭",
                            class: "btn btn-default",
                            click: function () {
                                //                                loadAttachments(Id, "gyqytdjbqkb");
                                $(this).dialog("close");
                            }
                        }]
                });

                loadAttachments(CurrId, "gyqytdjbqkb");
            } else
                $.messager.alert(data.Message);
        });
    });

    //切换浏览图片
    $(document).undelegate(".pro-img-list>dd", "click").delegate(".pro-img-list>dd", "click", function (e) {
        var _this = $(this);
        _this.parent().find("dd").removeClass("curr");
        _this.addClass("curr");
        $(".pro-img-details>img").attr("src", _this.attr("part_image"));
    });

    //删除图片
    $(document).undelegate(".pro-img-list em", "click").delegate(".pro-img-list em", "click", function (e) {
        var _this = $(this).parent("dd");
        deleteAttachment(_this.attr("Id"));
    });

    //上传图片
    $(document).undelegate("#UploadPartImage", "click").delegate("#UploadPartImage", "click", function (e) {
        myUpload({
            //上传文件接收地址
            uploadUrl: "/Attachment/UploadImage",
            //选择文件后，发送文件前自定义事件
            //file为上传的文件信息，可在此处做文件检测、初始化进度条等动作
            beforeSend: function (file) {
                if (file.name.toLowerCase().indexOf(".jpg") == -1 && file.name.toLowerCase().indexOf(".png") == -1 && file.name.toLowerCase().indexOf(".bmp") == -1) {
                    $.messager.alert(getLang("要求文件扩展名:.jpg,.png,.bmp！", langz_code));
                    return false;
                }
                else
                    return true;
            },
            appendFormData: function (fd) {
                fd.append("RelationId", CurrId);
                fd.append("Type", "gyqytdjbqkb");
            },
            //文件上传完成后回调函数
            //res为文件上传信息
            callback: function (res) {
                data = JSON.parse(res);
                if (data.State) {
                    var list = new Array();
                    list.push(data.DataObject);
                    $(".pro-img-list").append(_.template($('#ImageListTemplate').html(), { items: list }));
                    $(".pro-img-details").empty().append($.format("<img src='{0}'>", data.DataObject.Url));
                    $(".pro-img-list>dd").removeClass("curr");
                    $(".pro-img-list>dd:last").addClass("curr");
                }
                else
                    $.messager.alert(data.Message);

            },
            //返回上传过程中包括上传进度的相关信息
            //详细请看res,可在此加入进度条相关代码
            uploading: function (res) {

            }
        });
    });

    //编辑
    $(document).undelegate("#gyqytdjbqkbTable button.edit", "click").delegate("#gyqytdjbqkbTable button.edit", "click", function () {
        var Id = $(this).parent().parent().attr("lineid");
        editItem(Id);
    });

    $("#New").click(function () {
        $("div.modal-dialog").removeClass().addClass("modal-dialog");
        var templatehtml1 = _.template($('#AddOrUpdateTemplate').html(), { item: {} });
        $(".modalpanel").html(templatehtml1);
        $(".modalpanel").dialog({
            title: "新建",
            dialogClass: "modal-max2",
            buttons: [
                {
                    text: "确定",
                    class: "btn btn-primary",
                    click: function (a, b) {
                        var ND = $("#ND").val();                    //年度
                        var ZDBH = $("#ZDBH").val();                //宗地编号
                        var QYMC = $("#QYMC").val();                //企业名称
                        var TDMJ = $("#TDMJ").val();                //土地面积
                        var TDQYFW = $("#TDQYFW").val();            //土地区域范围
                        var YTDZSL = $("#YTDZSL").val();            //有土地证数量
                        var TDZJBLQK = $("#TDZJBLQK").val();        //土地证件办理情况
                        var XZTDYT = $("#XZTDYT").val();            //现状土地用途
                        var XGHXZ = $("#XGHXZ").val();              //现规划性质
                        var TDCRYX = $("#TDCRYX").val();            //土地出让意向
                        var DSQYSL = $("#DSQYSL").val();            //地上企业数量
                        var RCZGZGSL = $("#RCZGZGSL").val();        //日常在岗职工数量
                        var TZLDLH = $("#TZLDLH").val();            //调整列的列号
                        var TZQDNR = $("#TZQDNR").val();            //调整前的内容
                        var YJQY = $("#YJQY").val();                //一级企业
                        var ZZTDSYQR = $("#ZZTDSYQR").val();        //证载土地使用权人
                        var ZDWZ = $("#ZDWZ").val();                //宗地位置
                        var TDQDFS = $("#TDQDFS").val();            //土地取得方式
                        var TDZZZMJ = $("#TDZZZMJ").val();          //土地证证载面积
                        var WBLTDZQY = $("#WBLTDZQY").val();        //未办理土地证原因
                        var TDKFLX = $("#TDKFLX").val();            //土地开发类型
                        var NTZGHXZ = $("#NTZGHXZ").val();          //拟调整规划性质
                        var TDLYXZ = $("#TDLYXZ").val();            //土地利用现状
                        var ZCQYSL = $("#ZCQYSL").val();            //注册企业数量
                        var BHLX = $("#BHLX").val();                //变化类型
                        var BZ = $("#BZ").val();                    //备注
                        $.post("/gyqytdjbqkb/Add", {
                            ndbId: ND,
                            ZDBH: ZDBH,
                            QYMC: QYMC,
                            TDMJ: TDMJ,
                            TDQYFW: TDQYFW,
                            YTDZSL: YTDZSL,
                            TDZJBLQK: TDZJBLQK,
                            XZTDYT: XZTDYT,
                            XGHXZ: XGHXZ,
                            TDCRYX: TDCRYX,
                            DSQYSL: DSQYSL,
                            RCZGZGSL: RCZGZGSL,
                            TZLDLH: TZLDLH,
                            TZQDNR: TZQDNR,
                            YJQY: YJQY,
                            ZZTDSYQR: ZZTDSYQR,
                            ZDWZ: ZDWZ,
                            TDQDFS: TDQDFS,
                            TDZZZMJ: TDZZZMJ,
                            WBLTDZQY: WBLTDZQY,
                            TDKFLX: TDKFLX,
                            NTZGHXZ: NTZGHXZ,
                            TDLYXZ: TDLYXZ,
                            ZCQYSL: ZCQYSL,
                            BHLX: BHLX,
                            BZ: BZ
                        }, function (data) {
                            if (data.State) {
                                loadPage(1);
                            }
                            else
                                $.messager.alert(data.Message);
                        });
                        $(this).dialog("close");
                    }
                },
                {
                    text: "取消",
                    class: "btn btn-default",
                    click: function () {
                        $(this).dialog("close");
                    }
                }]
        });
        getNDList();
        loadTypeConfig();
    });
});


function loadAttachments(relationId, type) {
    addDo("加载中，请稍后……", false);
    $.post("/Attachment/GetList", { RelationId: relationId, Type: type }, function (data) {
        if (data.State) {
            $(".pro-img-list").empty();
            $(".pro-img-details").empty();
            $(".pro-img-list").html(_.template($('#ImageListTemplate').html(), { items: data.DataObject }));
            if (data.DataObject.length > 0) {
                $(".pro-img-details").append($.format("<img src='{0}'>", data.DataObject[0].Url));
                $(".pro-img-list>dd:eq(0)").addClass("curr");
            } 
        }
        else
            $.messager.alert(data.Message);

        removeDo(300);
    });
}

function deleteAttachment(Id) {
    $.messager.confirm("询问", "确定删除吗？", function () {
        $.post("/Attachment/Delete", {
            Id: Id
        }, function (data) {
            if (data.State) {
                loadAttachments(CurrId, "gyqytdjbqkb");
            }
            else
                $.messager.alert(data.Message);
        });
    });
}

function deleteItem(Id) {
    $.messager.confirm("询问", "确定删除吗？", function () {
        $.post("/gyqytdjbqkb/Delete", {
            Id: Id
        }, function (data) {
            if (data.State) {
                loadPage(1);
            }
            else
                $.messager.alert(data.Message);
        });
    });
}

function deleteItems(Ids) {
    $.messager.confirm("询问", "确定删除吗？", function () {
        $.post("/gyqytdjbqkb/Deletes", {
            Ids: Ids
        }, function (data) {
            if (data.State) {
                loadPage(1);
            }
            else
                $.messager.alert(data.Message);
        });
    });
}


function editItem(Id) {
    $.post("/gyqytdjbqkb/Single", { Id: Id }, function (data) {
        if (data.State) {
            $("div.modal-dialog").removeClass().addClass("modal-dialog");
            var templatehtml1 = _.template($('#AddOrUpdateTemplate').html(), { item: data.DataObject });
            $(".modalpanel").html(templatehtml1);
            $(".modalpanel").dialog({
                title: "编辑",
                dialogClass: "modal-max2",
                buttons: [
                {
                    text: "确定",
                    class: "btn btn-primary",
                    click: function (a, b) {
                        var ND = $("#ND").val();                    //年度
                        var ZDBH = $("#ZDBH").val();                //宗地编号
                        var QYMC = $("#QYMC").val();                //企业名称
                        var TDMJ = $("#TDMJ").val();                //土地面积
                        var TDQYFW = $("#TDQYFW").val();            //土地区域范围
                        var YTDZSL = $("#YTDZSL").val();            //有土地证数量
                        var TDZJBLQK = $("#TDZJBLQK").val();        //土地证件办理情况
                        var XZTDYT = $("#XZTDYT").val();            //现状土地用途
                        var XGHXZ = $("#XGHXZ").val();              //现规划性质
                        var TDCRYX = $("#TDCRYX").val();            //土地出让意向
                        var DSQYSL = $("#DSQYSL").val();            //地上企业数量
                        var RCZGZGSL = $("#RCZGZGSL").val();        //日常在岗职工数量
                        var TZLDLH = $("#TZLDLH").val();            //调整列的列号
                        var TZQDNR = $("#TZQDNR").val();            //调整前的内容
                        var YJQY = $("#YJQY").val();                //一级企业
                        var ZZTDSYQR = $("#ZZTDSYQR").val();        //证载土地使用权人
                        var ZDWZ = $("#ZDWZ").val();                //宗地位置
                        var TDQDFS = $("#TDQDFS").val();            //土地取得方式
                        var TDZZZMJ = $("#TDZZZMJ").val();          //土地证证载面积
                        var WBLTDZQY = $("#WBLTDZQY").val();        //未办理土地证原因
                        var TDKFLX = $("#TDKFLX").val();            //土地开发类型
                        var NTZGHXZ = $("#NTZGHXZ").val();          //拟调整规划性质
                        var TDLYXZ = $("#TDLYXZ").val();            //土地利用现状
                        var ZCQYSL = $("#ZCQYSL").val();            //注册企业数量
                        var BHLX = $("#BHLX").val();                //变化类型
                        var BZ = $("#BZ").val();                    //备注
                        $.post("/gyqytdjbqkb/Edit", {
                            EditId:Id,
                            ndbId: ND,
                            ZDBH: ZDBH,
                            QYMC: QYMC,
                            TDMJ: TDMJ,
                            TDQYFW: TDQYFW,
                            YTDZSL: YTDZSL,
                            TDZJBLQK: TDZJBLQK,
                            XZTDYT: XZTDYT,
                            XGHXZ: XGHXZ,
                            TDCRYX: TDCRYX,
                            DSQYSL: DSQYSL,
                            RCZGZGSL: RCZGZGSL,
                            TZLDLH: TZLDLH,
                            TZQDNR: TZQDNR,
                            YJQY: YJQY,
                            ZZTDSYQR: ZZTDSYQR,
                            ZDWZ: ZDWZ,
                            TDQDFS: TDQDFS,
                            TDZZZMJ: TDZZZMJ,
                            WBLTDZQY: WBLTDZQY,
                            TDKFLX: TDKFLX,
                            NTZGHXZ: NTZGHXZ,
                            TDLYXZ: TDLYXZ,
                            ZCQYSL: ZCQYSL,
                            BHLX: BHLX,
                            BZ: BZ
                        }, function (data) {
                            if (data.State) {
                                loadPage(1);
                            }
                            else
                                $.messager.alert(data.Message);
                        });
                        $(this).dialog("close");
                    }
                },
                {
                    text: "取消",
                    class: "btn btn-default",
                    click: function () {
                        $(this).dialog("close");
                    }
                }]
            });
            getNDList(data.DataObject);
            loadTypeConfig(data.DataObject);

        }
        else
            $.messager.alert(data.Message);
    });
}

//导出
function Export() {
    addDo("查询中，请稍后……", false);
    $.post("/gyqytdjbqkb/Export", getSearchParams(100000,1), function (data) {
        if (data.State) {
            $.messager.alert("导出文件", $.format("点击<a href='{0}'>这里</a>下载", data.Message));
        }
        else
            $.messager.alert(data.Message);

        removeDo(300);
    });
}

function getSearchParams(pageSize,page){
    var ND = $("#Search_ND").val();
    var ZDBH = $("#Search_ZDBH").val();
    var QYMC = $("#Search_QYMC").val();
    var ZDWZ = $("#Search_ZDWZ").val();
    return {
        ndbId: ND,
        ZDBH: ZDBH,
        QYMC: QYMC,
        ZDWZ:ZDWZ,
        PageNum: page,
        PageSize:pageSize
    };
}

function loadPage(page) {
    addDo("查询中，请稍后……", false);
    $.post("/gyqytdjbqkb/Search", getSearchParams(PageSize,page), function (data) {
        $("table tbody").empty();
        if (data.State) {
            if (data.DataObject.PageListInfos.length > 0) {
                for (var i = 0; i < data.DataObject.PageListInfos.length; i++) {
                    data.DataObject.PageListInfos[i].rownum = i + 1;
                }
                $("#gyqytdjbqkbTable tbody").append(_.template($('#gyqytdjbqkbListTemplate').html(), { items: data.DataObject.PageListInfos }));
                autoHeight();
            }
        } else {
            $.messager.alert(data.Message);
        }
        removeDo(300);
    });
}

function getNDList(datItem) {
    datItem = datItem || {};
    addDo("加载中，请稍后……", false);
    $.post("/NDB/List", {}, function (data) {
        $("#ND").empty();
        if (data.State) {
            $(data.DataObject).each(function (index, item) {
                $("#ND").append($.format("<option value='{0}' {2}>{1}</option>", item.Id, item.Name, datItem.ndbId == item.Id ? "selected" : ""));
            });
        } else {
            $.messager.alert(data.Message);
        }
        removeDo(300);
    });
}

function getSearchNDList() {
    addDo("加载中，请稍后……", false);
    $.post("/NDB/List", {}, function (data) {
        $("#Search_ND").empty();
        if (data.State) {
            $(data.DataObject).each(function (index, item) {
                $("#Search_ND").append($.format("<option value='{0}'>{1}</option>", item.Id, item.Name));
            });
            if (data.DataObject.length > 0) {
                loadPage(1);
            }
        } else {
            $.messager.alert(data.Message);
        }
        removeDo(300);
    });
}

function loadTypeConfig(datItem) {
    datItem = datItem || {};
    $.post("/gyqytdjbqkb/getTypeConfig", {}, function (data) {
        if (data.State) {
            $(data.DataObject[0].TDQYFWTypeConfig.TypeItemConfigs).each(function () {
                $("#TDQYFW").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.TDQYFW == $(this).attr("Value") ? "selected" : ""));
            });
            $(data.DataObject[0].TDQDFSTypeConfig.TypeItemConfigs).each(function () {
                $("#TDQDFS").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.TDQDFS == $(this).attr("Value") ? "selected" : ""));
            });
            $(data.DataObject[0].TDZJBLQKTypeConfig.TypeItemConfigs).each(function () {
                $("#TDZJBLQK").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.TDZJBLQK == $(this).attr("Value") ? "selected" : ""));
            });
            $(data.DataObject[0].WBLTDZQYTypeConfig.TypeItemConfigs).each(function () {
                $("#WBLTDZQY").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.WBLTDZQY == $(this).attr("Value") ? "selected" : ""));
            });
            $(data.DataObject[0].XZTDYTTypeConfig.TypeItemConfigs).each(function () {
                $("#XZTDYT").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.XZTDYT == $(this).attr("Value") ? "selected" : ""));
            });
            $(data.DataObject[0].TDKFLXTypeConfig.TypeItemConfigs).each(function () {
                $("#TDKFLX").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.TDKFLX == $(this).attr("Value") ? "selected" : ""));
            });
            $(data.DataObject[0].XGHXZTypeConfig.TypeItemConfigs).each(function () {
                $("#XGHXZ").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.XGHXZ == $(this).attr("Value") ? "selected" : ""));
            });
            $(data.DataObject[0].NTZGHXZTypeConfig.TypeItemConfigs).each(function () {
                $("#NTZGHXZ").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.NTZGHXZ == $(this).attr("Value") ? "selected" : ""));
            });
            $(data.DataObject[0].TDCRYXTypeConfig.TypeItemConfigs).each(function () {
                $("#TDCRYX").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.TDCRYX == $(this).attr("Value") ? "selected" : ""));
            });
            $(data.DataObject[0].TDLYXZTypeConfig.TypeItemConfigs).each(function () {
                $("#TDLYXZ").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.TDLYXZ == $(this).attr("Value") ? "selected" : ""));
            });
            $(data.DataObject[2].BHLXTypeConfig.TypeItemConfigs).each(function () {
                $("#BHLX").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("Value"), $(this).attr("Text"), datItem.BHLX == $(this).attr("Value") ? "selected" : ""));
            });
            datItem.YJQY = datItem.YJQY == null ? "017" : datItem.YJQY;
            $(data.DataObject[1].YJQYItems).each(function () {
                $("#YJQY").append($.format("<option value='{0}' {2}>{0} {1}</option>", $(this).attr("BM"), $(this).attr("Name"), datItem.YJQY == $(this).attr("BM") ? "selected" : ""));
            });

        }
        else
            $.messager.alert(data.Message);
    });
}

function getSelectIds() {
    var IdArr = new Array();
    $("#gyqytdjbqkbTable>tbody>tr").each(function () {
        var _this = $(this);
        if (_this.find("td:eq(0) input").attr("checked") == "checked") {
            IdArr.push(_this.attr("lineid"));
        }
    });
    return IdArr;
}

//自动设置高度
function autoHeight() {
    if ($("#gyqytdjbqkbTable>tbody>tr").length > 3) {
        var height = $(window).height() - $('#gyqytdjbqkbTable').offset().top - 20;
        height = height > TableMinHeight ? height : TableMinHeight;
        $('#gyqytdjbqkbTable').fixedHeaderTable({ height: height + "px" });
    }
}

