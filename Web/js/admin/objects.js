var g = { u: new Utils(), url: "Objects.aspx", objID: null, tbl: null };

$(function () {
    Init();
    RegEvent();
});

// 初始化加载
function Init() {
    LoadObjects("GET_OBJ_LIST");
}

// 表单事件注册
function RegEvent() {
    // 添加对象事件
    $("#btnAdd").click(function () {
        g.objID = null;
        $("#sltCtg").attr("disabled", false);
        $("#divObjects input").val("");
        g.u.MessageBox("divObjects", "添加对象", null, null, SaveObject);
    });

    // 删除对象事件
    $("#btnDelete").click(function () {
        var array = g.tbl.getSelected();
        if (!array || !array.length) {
            alert("请选择需要删除的对象.");
            return false;
        }
        DeleteObject(array);
    });

    // 查询对象
    $("#btnSearch").click(function () {
        var objType = $("#sltType").val(),
            area = $("#sltArea").val(),
            status = $("#sltStatus").val(),
            objName = $("#txtKey").val();
        LoadObjects("SEARCH_OBJECT_LIST", objType, area, status, objName);
    });

    $("#sltCtg").change(function () {
        var obj = $(this).parent().nextAll();
        if ($(this).val() === "1") {
            obj.show();
        }
        else {
            obj.hide();
        }
        delete obj;
    });
}

/********************************* 内部方法 **********************************/
// 加载对象列表
function LoadObjects(tag, objType, area, status, key) {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: tag, objType: objType, area: area, status: status, objName: escape(key) },
        dataType: "json",
        success: function (json) {
            g.tbl = new Table(
                        {
                            el: "tbl",
                            datas: json,
                            PageSize: 15,
                            tblHeader:
                            {
                                ObjType: { text: "对象类型", sortType: "string", format: function (val) {
                                    switch (val) {
                                        case "1": return "分店";
                                        case "2": return "仓库";
                                        default: return "";
                                    }
                                }
                                },
                                ObjName: { text: "名称", sortType: "string" },
                                Contact: { text: "联系人" },
                                Status: { text: "状态", format: function (val) {
                                    switch (val) {
                                        case "0": return "启用";
                                        case "1": return "禁用";
                                        default: return "";
                                    }
                                }
                                },
                                AddDate: { text: "添加日期", sortType: "date", format: function (val) {
                                    var d = val.match("(.+) (.+)");
                                    return d[1];
                                }
                                }
                            },
                            IsOperate: true,
                            IsSelect: true,
                            operateData:
                            {
                                filed: "ObjID", obj: { edit: "编辑", del: "删除" }
                            },
                            width: "100%",
                            onbindend: function () {
                                g.tbl.el.find("tbody a[tag='del']").unbind("click").bind("click", function () {
                                    var objID = $(this).attr("ObjID");
                                    if (!objID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    DeleteObject(new Array(objID));
                                });

                                g.tbl.el.find("tbody a[tag='edit']").unbind("click").bind("click", function () {
                                    g.objID = $(this).attr("ObjID");
                                    if (!g.objID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    $("#sltCtg").attr("disabled", true);
                                    LoadWarehouse();
                                    GetObjectDetail();
                                    g.u.MessageBox("divObjects", "更新对象", null, null, SaveObject);
                                });
                            }
                        });
            g.tbl.Init();
        }
    });
}

// 加载仓库列表
function LoadWarehouse() {
    $.ajax({
        url: g.url,
        type: "POST",
        dataType: "json",
        data: { tag: "GET_WAREHOUSE" },
        async: false,
        success: function (json) {
            var html = [],
                i = 0;
            html.push("<option value=\"\">--请选择--</option>");
            if (json && json.length) {
                var len = json.length;
                for (; i < len; i++) {
                    html.push("<option value=\"" + json[i]["ObjID"] + "\">" + json[i]["ObjName"] + "</option>");
                }
            }
            $("#sltQY").html(html.join(""));
        }
    });
}

// 保存对象
function SaveObject() {
    var obj = {};
    obj.objID = g.objID;
    obj.tag = g.objID ? "UPDATE_OBJECT" : "ADD_OBJECT";
    obj.objType = $("#sltCtg").val();
    if (obj.objType === "1") {
        obj.warehouseID = $("#sltQY").val();
        if (!obj.warehouseID) {
            alert("请选择该分店所属仓库.");
            return false;
        }
    }
    else {
        obj.warehouseID = "";
    }
    obj.objName = escape($("#txtName").val());
    if (!obj.objName) {
        alert("请输入对象名称.");
        return false;
    }
    obj.contact = escape($("#txtContact").val());
    if (!obj.contact) {
        alert("请输入联系人.");
        return false;
    }
    obj.phone = $("#txtPhone").val();
    if (!obj.phone) {
        alert("请输入联系电话.");
        return false;
    }
    obj.address = escape($("#txtAddress").val());
    if (!obj.address) {
        alert("请输入详细地址.");
        return false;
    }
    obj.status = $("#sltZT").val();

    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: obj,
        success: function (res) {
            if (res && res == "success") {
                $("#divObjects :text").val("");
                g.objID = null;
                LoadObjects("GET_OBJ_LIST");
                g.u.dlg.dialog("destroy");
            }
            else {
                alert("对象保存失败, 请稍后重试.");
            }
        }
    });
}

// 删除对象
function DeleteObject(array) {
    if (!array || !array.length) {
        alert("请选择需要删除的对象.");
        return false;
    }
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "DELETE_OBJECT", objList: array.join(",") },
        success: function (res) {
            if (res && res == "success") {
                alert("对象删除成功.");
                LoadObjects("GET_OBJ_LIST");
            }
            else {
                alert("对象删除失败, 请稍后重试.");
            }
        }
    });
}

// 获取对象详情
function GetObjectDetail() {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_OBJECT_DETAIL", objID: g.objID },
        dataType: "json",
        success: function (json) {
            if (!json || (typeof json) !== "object") {
                alert("未知错误, 请稍后重试.");
                return false;
            }
            $("#sltCtg").val(json["F_ObjectType"]).attr("disabled", true);
            if (json["F_ObjectType"] === "2") {
                $("#sltCtg").parent().nextAll().hide();
            }
            else {
                $("#sltCtg").parent().nextAll().show();
                $("#sltQY").val(json["F_WarehouseID"]);
            }
            $("#txtName").val(json["F_ObjectName"]);
            $("#txtContact").val(json["F_Conatct"]);
            $("#txtPhone").val(json["F_Tel"]);
            $("#txtAddress").val(json["F_Address"]);
            $("#sltZT").val(json["F_Status"]);
        }
    });
}