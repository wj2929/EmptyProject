$(function () {

    autoHeight();
    $(window).bind("resize", function () {
        autoHeight();
    });

    getList();

    //全选或反选
    $(document).undelegate("#CBSelectAll", "click").delegate("#CBSelectAll", "click", function () {
        if ($(this).attr("checked") == "checked")
            $("#NDBTable tbody input[type='checkbox']").attr("checked", "checked");
        else
            $("#NDBTable tbody input[type='checkbox']").removeAttr("checked");
    });


    //编辑
    $(document).undelegate("#NDBTable button.edit", "click").delegate("#NDBTable button.edit", "click", function () {
        var Id = $(this).parent().parent().attr("lineid");
        var Name = $(this).parent().parent().attr("linename");
        var templatehtml1 = _.template($('#EditNDBTemplate').html(), { Id: Id, Name: Name });
        $(".modalpanel").html(templatehtml1);
        $(".modalpanel").dialog({
            title: "编辑年度",
            dialogClass: "modal-dialog",
            buttons: [
                {
                    text: "确定",
                    class: "btn btn-primary",
                    click: function (a, b) {
                        Name = $("#EditNDB_Name").val();
                        editNDB(Id, Name);
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
    });

    //导入
    $(document).delegate("#NDBTable button.import", "click", function () { 
            
    });

    //批量删除
    $("#DeleteNDB").click(function () {
        var IdArr = getSelectIds();
        if (IdArr.length > 0)
            deleteNDBs(IdArr.join(","));
        else
            $.messager.alert("未选择！");
    });

    //删除
    $(document).undelegate("#NDBTable button.remove", "click").delegate("#NDBTable button.remove", "click", function () {
        var Id = $(this).parent().parent().attr("lineid");
        deleteNDB(Id);
    });

    $("#AddNDB").click(function () {
        var templatehtml1 = _.template($('#AddNDBTemplate').html(), {});
        $(".modalpanel").html(templatehtml1);
        $(".modalpanel").dialog({
            title: "新建年度",
            dialogClass: "modal-dialog",
            buttons: [
                {
                    text: "确定",
                    class: "btn btn-primary",
                    click: function (a, b) {
                        var Name = $("#AddNDB_Name").val();
                        var SelectCopyId = $("#AddNDB_SelectCopyId").val();
                        addNDB(Name, SelectCopyId);
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

        $.post("/NDB/List", {}, function (data) {
            $("#AddNDB_SelectCopyId").empty();
            if (data.State) {
                $("#AddNDB_SelectCopyId").append("<option value=''>空</option>");
                if (data.DataObject.length > 0) {
                    $(data.DataObject).each(function (index, item) {
                        $("#AddNDB_SelectCopyId").append($.format("<option value='{0}'>{1}</option>", item.Id, item.Name));
                    });
                }
            } else {
                $.messager.alert(data.Message);
            }
            removeDo(300);
        });
    });
});

function getList() {
    addDo("加载中，请稍后……", false);
    $.post("/NDB/List", {}, function (data) {
        $("table tbody").empty();
        if (data.State) {
            if (data.DataObject.length > 0) {
                $("table tbody").append(_.template($('#NDBListTemplate').html(), { items: data.DataObject }));
                autoHeight();
            }
        } else {
            $.messager.alert(data.Message);
        }
        removeDo(300);
    });
}

function addNDB(name, selectCopyId) {
    $.post("/NDB/Add",{
        Name: name,
        SelectCopyId: selectCopyId
    }, function (data) {
        if (data.State) {
            getList();
        }
        else
            $.messager.alert(data.Message);
    });
}

function editNDB(Id, name) {
    $.post("/NDB/Edit", {
        EditId: Id,
        Name: name
    }, function (data) {
        if (data.State) {
            getList();
        }
        else
            $.messager.alert(data.Message);
    });
}

function deleteNDB(Id) {
    $.messager.confirm("询问", "确定删除吗，本操作将级联删除该年度所有数据，请确定？", function () {
        $.post("/NDB/Delete", {
            Id: Id
        }, function (data) {
            if (data.State) {
                getList();
            }
            else
                $.messager.alert(data.Message);
        });
    });
}

function deleteNDBs(Ids) {
    $.messager.confirm("询问", "确定删除吗，本操作将级联删除该年度所有数据，请确定？", function () {
        $.post("/NDB/Deletes", {
            Ids: Ids
        }, function (data) {
            if (data.State) {
                getList();
            }
            else
                $.messager.alert(data.Message);
        });
    });
}

//自动设置高度
function autoHeight() {
    if ($("#NDBTable>tbody>tr").length > 8) {
        var height = $(window).height() - $('#NDBTable').offset().top - 20;
        height = height > TableMinHeight ? height : TableMinHeight;
        $('#NDBTable').fixedHeaderTable({ height: height + "px" });
    }
}

function getSelectIds() {
    var IdArr = new Array();
    $("#NDBTable>tbody>tr").each(function () {
        var _this = $(this);
        if (_this.find("td:eq(0) input").attr("checked") == "checked") {
            IdArr.push(_this.attr("lineid"));
        }
    });
    return IdArr;
}