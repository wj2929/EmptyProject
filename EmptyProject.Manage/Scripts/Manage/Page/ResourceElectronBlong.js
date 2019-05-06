var OldParentId = "";
$(function () {
    $("#QuestionTypeTree").tree({
        data: {
            type: "xml_flat",
            async: true,
            opts: {
                async: true,
                url: "/ResourceElectron/WebService/ResourceBlong.asmx/LoadTree"
            }
        },
        types: {
            "root": {
                creatable: true,
                max_children: -1,
                draggable: false,
                ax_depth: -1,
                icon: {
                    image: "/Scripts/jstree/icon/root.png"
                }
            },
            "items": {
                valid_children: "items",
                draggable: false,
                creatable: true,
                icon: {
                    image: "/Scripts/jstree/icon/block.png"
                }
            }
        },
        callback: {
            beforedata: function (NODE, t) {
                return { 'ParentId': $(NODE).attr("id") || '' }
            },
            check_move: function (NODE, REF_NODE, TYPE, TREE_OBJ) {
                OldParentId = $.tree.focused().parent(NODE).attr("id");
            },
            onmove: function (NODE, REF_NODE, TYPE, TREE_OBJ, RB) {
            },
            oncreate: function (NODE, REF_NODE, TYPE, TREE_OBJ, RB) {
                $(NODE).attr("rel", "items");
                $(NODE).attr("id", "tempid");
            },
            onrename: function (NODE, TREE_OBJ, RB) {
                if (TREE_OBJ.get_text(NODE) != "New folder") {
                    $.ajax({
                        type: "POST",
                        url: "/ResourceElectronBlong/SaveResourceBlong",
                        data: '{ParentId:"' + $.tree.focused().parent(NODE).attr("id") + '",qtName:"' + TREE_OBJ.get_text(NODE) + '",qtId:"' + $(NODE).attr("id") + '"}',
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (result) {
                            if (result.d == "err") {
                                if ($(NODE).attr("id") == "tempid") {
                                    RemoveNode("tempid");
                                }
                                else {
                                    $.tree.rollback(RB);
                                }
                            }
                            else if ($(NODE).attr("id") == "tempid") {
                                $(NODE).attr("id", result.d);
                            }
                        }
                    });
                }
                else {
                    $.tree.rollback(RB);
                    RemoveNode("tempid");
                }
            }
        },
        plugins: {
            contextmenu: {
                items: {
                    create: {
                        label: "新建子分类",
                        icon: "",
                        visible: function (NODE, TREE_OBJ) {
                            if (NODE.length != 1) return 0;
                            return TREE_OBJ.check("creatable", NODE);
                        },
                        action: function (NODE, TREE_OBJ) {
                            TREE_OBJ.create(false, TREE_OBJ.get_node(NODE[0]));
                        }
                    },
                    rename: {
                        label: "编辑分类",
                        icon: "",
                        visible: function (NODE, TREE_OBJ) {
                            if ($(NODE).attr("rel") == "root") {
                                return -1;
                            }
                            else {
                                return 1;
                            }
                        },
                        action: function (NODE, TREE_OBJ) {
                            TREE_OBJ.rename(NODE);
                        }
                    },
                    remove: {
                        label: "删除分类",
                        icon: "",
                        visible: function (NODE, TREE_OBJ) {
                            if ($(NODE).attr("rel") != "root") {
                                return 1;
                            }
                            else {
                                return -1;
                            }
                        },
                        action: function (NODE, TREE_OBJ) {
                            this.selected;
                            if ($(NODE).attr("rel") != "root") {
                                DelInfo($(NODE).attr("id"), "删除","确认删除吗？");
                            }
                        }
                    }
                }
            }
        }
    });
});

function RemoveNode(inputID) {
    $.tree.focused().remove("#" + inputID + "");
}

function DelInfo(retVal, _refP1, _refP2) {
    yonLay(_refP1, _refP2, function (flag) {
        if (flag) {
            $.ajax({
                type: "POST",
                url: "/ResourceElectronBlong/DelInfo",
                data: '{Id:"' + retVal + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d == "err") {
                        alert(msg.d);
                    }
                    else {
                        RemoveNode(retVal);
                    }
                }
            });
        }
    });
}