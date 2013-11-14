var g = { url: "Dictionary.aspx", tree: null };

$(function () {
    Init();
    RegEvent();
});

function Init() {
    LoadTree();
}

function RegEvent() {
    // 添加分类按钮事件
    $("#btnAdd").click(function () {
        if (!g.tree.getSelectedNodes().length) {
            alert("请选择需要添加的节点.");
            return false;
        }
        $(".tbl :input").attr("disabled", false).filter(":text").val("");
        $("#btnSame,#btnChild").attr("disabled", false);
        $("#btnSave").attr("disabled", true);
    });

    // 编辑按钮事件
    $("#btnEdit").click(function () {
        if (!g.tree.getSelectedNodes().length) {
            alert("请选择需要编辑的节点.");
            return false;
        }
        $(".tbl :input").attr("disabled", false);
        $("#btnSame,#btnChild").attr("disabled", true);
        $("#btnSave").attr("disabled", false);
    });

    // 添加字典事件
    $("#btnSame,#btnChild").click(function () {
        var dicName = $("#txtName").val();
        if (!dicName) {
            alert("请输入字典名称.");
            return false;
        }
        var id = $(this).attr("id"),
            pId;
        if (id === "btnSame") {
            pId = g.tree.getSelectedNodes()[0]["pId"];
        }
        else {
            pId = g.tree.getSelectedNodes()[0]["id"];
        }
        AddDictionary(dicName, pId, $("#txtCode").val(), $("#sltStatus").val());
    });

    // 保存字典
    $("#btnSave").click(function () {
        var dicName = $("#txtName").val();
        if (!dicName) {
            alert("请输入字典名称.");
            return false;
        }
        EditDictionary(g.tree.getSelectedNodes()[0]["id"], dicName, $("#txtCode").val(), $("#sltStatus").val());
    });

    // 删除字典事件
    $("#btnDelete").click(function () {
        if (!g.tree.getSelectedNodes().length) {
            alert("请选择需要删除的节点.");
            return false;
        }
        $.post(g.url, { tag: "DELETE_DICTIONARY", dicID: g.tree.getSelectedNodes()[0]["id"] },
            function (res) {
                if (res && res === "success") {
                    LoadTree();
                    $(".tbl :text").val("");
                }
            });
    });
}

/************************************* 私有方法 *************************************/
// 加载树形插件
function LoadTree() {
    $.ajax({
        url: g.url,
        type: "POST",
        dataType: "json",
        data: { tag: "GET_DICTIONARY_LIST" },
        success: function (json) {
            g.tree = $.fn.zTree.init($("#divTree"), {
                data: { simpleData: { enable: true} },
                callback: {
                    onClick: function (event, treeId, treeNode) {
                        $(".tbl :input").attr("disabled", true);
                        $("#txtName").val(treeNode["name"]);
                        $("#txtCode").val(treeNode["code"]);
                        $("#sltStatus").val(treeNode["status"]);
                    }
                }
            }, json);
        }
    });
}

// 添加字典
function AddDictionary(dicName, parId, dicCode, status) {
    $.post(g.url, { tag: "ADD_DICTIONARY", dicName: escape(dicName), parId: parId, dicCode: dicCode },
        function (res) {
            if (res && res === "success") {
                LoadTree();
                $(".tbl input").attr("disabled", true).filter(":text").val("");
            }
            else {
                alert("添加失败, 请稍后重试.");
            }
        });
}

// 保存字典
function EditDictionary(dicID, dicName, dicCode, status) {
    $.post(g.url, { tag: "UPDATE_DICTIONARY", dicName: escape(dicName), dicID: dicID, dicCode: dicCode },
        function (res) {
            if (res && res === "success") {
                LoadTree();
                $(".tbl input").attr("disabled", true).filter(":text").val("");
            }
            else {
                alert("保存失败, 请稍后重试.");
            }
        });
}