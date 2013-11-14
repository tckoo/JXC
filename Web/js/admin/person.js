var g = { u: new Utils(), url: "Person.aspx", tbl: null, personID: null };

$(function () {
    Init();
    RegEvent();
});

// 初始化加载
function Init() {
    LoadObjects();

    LoadPersonList("GET_PERSON_LIST");
}

// 表单事件注册
function RegEvent() {
    // 添加对象事件
    $("#btnAdd").click(function () {
        g.u.MessageBox("divPerson", "添加员工", null, null, SavePerson);
    });

    // 删除对象事件
    $("#btnDelete").click(function () {
        var array = g.tbl.getSelected();
        if (!array || !array.length) {
            alert("请选择需要删除的员工.");
            return false;
        }
        DeletePerson(array);
    });

    // 查询对象
    $("#btnSearch").click(function () {
        var objID = $("#sltObj").val(),
            status = $("#sltStatus").val(),
            isAdmin = $("#sltAdmin").val(),
            enterDate = $("#txtEnterDate").val(),
            pName = $("#txtName").val();
        LoadPersonList("GET_PERSON_LIST", objID, status, isAdmin, enterDate, pName);
    });

    $(":radio[name='admin']").click(function () {
        if ($(this).attr("id") === "rdYes") {
            $(this).parent().nextAll().show();
        }
        else {
            $(this).parent().nextAll().hide();
        }
    });

    $("#txtEDate,#txtEnterDate").datepicker();
}

/********************************* 内部方法 **********************************/
// 加载对象列表
function LoadObjects() {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_OBJ_LIST" },
        dataType: "json",
        success: function (json) {
            var html = [];
            html.push("<option value=\"\">不限</option>");
            if (json && json.length) {
                for (var i = 0, len = json.length; i < len; i++) {
                    html.push("<option value=\"" + json[i]["ObjID"] + "\" otype=\"" + json[i]["ObjType"] + "\">" + json[i]["ObjName"] + "</option>");
                }
            }
            $("#sltObj").html(html.join(""));
            html.shift();
            $("#sltObject").html(html.join(""));
        }
    });
}

// 加载员工列表
function LoadPersonList(tag, objID, status, isAdmin, enterDate, key) {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: tag, objID: objID, status: status, isAdmin: isAdmin, enterDate: enterDate, pName: escape(key) },
        dataType: "json",
        success: function (json) {
            g.tbl = new Table(
                        {
                            el: "tbl",
                            datas: json,
                            PageSize: 15,
                            tblHeader:
                            {
                                F_Name: { text: "姓名", sortType: "string" },
                                F_ObjectName: { text: "所属对象", sortType: "string" },
                                F_Phone: { text: "联系电话" },
                                F_Wage: { text: "基本工资" },
                                F_EnterDate: { text: "入职日期", format: function (val) {
                                    var d = val.match("(.+) (.+)");
                                    return d[1];
                                }
                                },
                                F_IsAdmin: { text: "是否管理员", format: function (val) {
                                    if (val == "0") {
                                        return "是";
                                    }
                                    else {
                                        return "否";
                                    }
                                }
                                },
                                F_Status: { text: "状态", format: function (val) {
                                    switch (val) {
                                        case "0": return "启用";
                                        case "1": return "禁用";
                                        default: return "";
                                    }
                                }
                                }
                            },
                            IsOperate: true,
                            IsSelect: true,
                            operateData:
                            {
                                filed: "F_PersonID", obj: { scan: "查看", edit: "编辑", del: "删除" }
                            },
                            width: "100%",
                            onbindend: function () {
                                g.tbl.el.find("tbody a[tag='del']").unbind("click").bind("click", function () {
                                    var personID = $(this).attr("F_PersonID");
                                    if (!personID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    DeletePerson(new Array(personID));
                                });

                                g.tbl.el.find("tbody a[tag='edit']").unbind("click").bind("click", function () {
                                    g.personID = $(this).attr("F_PersonID");
                                    if (!g.personID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    GetPersonDetail(true);
                                    g.u.MessageBox("divPerson", "更新员工", null, null, SavePerson);
                                });

                                g.tbl.el.find("tbody a[tag='scan']").unbind("click").bind("click", function () {
                                    g.personID = $(this).attr("F_PersonID");
                                    if (!g.personID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    GetPersonDetail(false);
                                    g.u.MessageBox("divDetail", "查看详情", null, null, function () { g.u.dlg.dialog("destroy"); });
                                });
                            }
                        });
            g.tbl.Init();
        }
    });
}

// 保存员工
function SavePerson() {
    var obj = {};
    obj.personID = g.personID;
    obj.tag = g.personID ? "UPDATE_PERSON" : "ADD_PERSON";
    obj.pName = escape($("#txtPName").val());
    if (!obj.pName) {
        alert("请输入员工姓名.");
        return false;
    }
    obj.objType = $("#sltObject option:selected").attr("otype");
    obj.objID = $("#sltObject").val();
    if (!obj.objID) {
        alert("请选择所属对象.");
        return false;
    }
    obj.sex = $("#sltSex").val();
    obj.phone = $("#txtTel").val();
    if (!obj.phone) {
        alert("请输入联系电话.");
        return false;
    }
    obj.wage = $("#txtMoney").val();
    if (!obj.wage) {
        alert("请输入基本工资.");
        return false;
    }
    obj.idCard = $("#txtCard").val();
    if (!obj.idCard) {
        alert("请输入证件号码.");
        return false;
    }
    obj.address = escape($("#txtAddress").val());
    if (!obj.address) {
        alert("请输入家庭住址.");
        return false;
    }
    obj.enterDate = $("#txtEDate").val();
    if (!obj.enterDate) {
        alert("请输入入职日期.");
        return false;
    }
    obj.status = $("#sltZT").val();
    obj.isAdmin = $(":radio[name='admin']:checked").val();
    obj.account = $("#txtAccount").val();

    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: obj,
        success: function (res) {
            if (res && res == "success") {
                $("#divPerson :text").val("");
                g.personID = null;
                LoadPersonList("GET_PERSON_LIST");
                g.u.dlg.dialog("destroy");
            }
            else {
                alert("员工保存失败, 请稍后重试.");
            }
        }
    });
}

// 删除员工(假删除)
function DeletePerson(array) {
    if (!array || !array.length) {
        alert("请选择需要删除的员工.");
        return false;
    }

    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "DELETE_PERSON", personList: array.join(",") },
        success: function (res) {
            if (res && res == "success") {
                alert("员工删除成功.");
                LoadPersonList("GET_PERSON_LIST");
            }
            else {
                alert("员工删除失败, 请稍后重试.");
            }
        }
    });
}

// 获取员工详情
function GetPersonDetail(isEdit) {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_PERSON_DETAIL", personID: g.personID },
        dataType: "json",
        success: function (json) {
            if (!json || !json.length) {
                alert("未知错误, 请稍后重试.");
                return false;
            }
            if (isEdit) {
                $("#txtPName").val(json[0]["F_Name"]);
                $("#sltObject").val(json[0]["F_ObjectID"]);
                $("#sltSex").val(json[0]["F_Sex"]);
                $("#txtTel").val(json[0]["F_Phone"]);
                $("#txtMoney").val(json[0]["F_Wage"]);
                $("#txtCard").val(json[0]["F_IDCard"]);
                $("#txtAddress").val(json[0]["F_Address"]);
                $("#txtEDate").val(json[0]["F_EnterDate"]);
                $("#sltZT").val(json[0]["F_Status"]);
                $(":radio[name='admin'][value='" + json[0]["F_IsAdmin"] + "']").attr("checked", true);
                if (json[0]["F_IsAdmin"] == "0") {
                    $("#txtAccount").parent().show().prev().show();
                }
                else {
                    $("#txtAccount").parent().hide().prev().hide();
                }
                $("#txtAccount").val(json[0]["F_Account"]);
            }
            else {
                $("#lName").text(json[0]["F_Name"]);
                $("#lObj").text(json[0]["F_ObjectName"]);
                $("#lSex").text(function () {
                    return json[0]["F_Sex"] == "0" ? "男" : "女";
                });
                $("#lTel").text(json[0]["F_Phone"]);
                $("#lWage").text(json[0]["F_Wage"]);
                $("#lCard").text(json[0]["F_IDCard"]);
                $("#lAddress").text(json[0]["F_Address"]);
                $("#lEnterDate").text(function () {
                    var d = json[0]["F_EnterDate"].match("(.+) (.+)");
                    return d[1];
                });
                $("#lStatus").text(function () {
                    return json[0]["F_Status"] == "0" ? "启用" : "停用";
                });
                $("#lIsAdmin").text(function () {
                    return json[0]["F_IsAdmin"] == "0" ? "是" : "否";
                });

                $("#lAccount").text(json[0]["F_Account"]);
                $("#lPwd").text(json[0]["pwd"]);
            }
        }
    });
}