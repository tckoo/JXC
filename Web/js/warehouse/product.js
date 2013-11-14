var g = { u: new Utils(), url: "Product.aspx", tbl: null, filePath: "", productID: "" };

$(function () {
    Init();
    RegEvent();
});

// 初始化加载
function Init() {
    LoadMainCtg();
    LoadSubCtg($("#sltMainCtg").val(), "sltSubCtg");

    LoadProductList();

    LoadUnit();
    LoadSubCtg($("#sltBig").val(), "sltSmall");
}

// 表单事件注册
function RegEvent() {
    // 选择分类事件
    $("#sltMainCtg").change(function () {
        LoadSubCtg($(this).val(), "sltSubCtg");
    });

    // 选择分类事件
    $("#sltBig").change(function () {
        LoadSubCtg($(this).val(), "sltSmall");
    });

    // 添加产品事件
    $("#btnAdd").click(function () {
        g.u.MessageBox("divProduct", "添加产品", null, null, SaveProduct);
    });

    // 查询对象
    $("#btnSearch").click(function () {
        var subCtg = $("#sltSubCtg").val(),
            status = $("#sltStatus").val(),
            key = $("#txtKey").val();
        LoadProductList(subCtg, status, key);
    });

    // 初始化上传插件
    $("#uploadify").uploadify({
        'uploader': '../js/jquery.uploadify/uploadify.swf',
        'script': '../inc/upload.ashx',
        'cancelImg': '../js/jquery.uploadify/cancel.png',
        'folder': 'upload',
        'queueID': 'fileQueue',
        'auto': false,
        'multi': false,
        'fileExt': '*.bmp;*.jpg;*.jpeg;*.gif;*.png',
        'fileDesc': '图像文件(*.bmp;*.jpg;*.jpeg;*.gif;*.png)',
        'onComplete': function (event, queueId, fileObj, response, data) {
            var obj = eval('(' + response + ')');
            g.filePath = obj.url;
            $("#fileQueue").html("文件上传成功!");
        }
    });

    // 上传文件
    $("#btnUpload").click(function () {
        $("#uploadify").uploadifyUpload();
    });
}

/********************************* 内部方法 **********************************/
// 加载产品大类
function LoadMainCtg() {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_PRO_MAIN_CTG" },
        dataType: "json",
        success: function (json) {
            var html = [];
            if (json && json.length) {
                for (var i = 0, len = json.length; i < len; i++) {
                    html.push("<option value=\"" + json[i]["F_DictionaryID"] + "\">" + json[i]["F_DicName"] + "</option>");
                }
            }
            $("#sltMainCtg,#sltBig").html(html.join(""));
        }
    });
}

// 加载产品小类
function LoadSubCtg(ctgID, el) {
    if (!ctgID) {
        $("#" + el).html("<option value=\"\">--请选择--</option>");
        return;
    }
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_PRO_SUB_CTG", parID: ctgID },
        dataType: "json",
        success: function (json) {
            var html = [];
            if (el === "sltSmall") {
                html.push("<option value=\"\">--请选择--</option>");
            }
            else {
                html.push("<option value=\"\">不限</option>");
            }
            if (json && json.length) {
                for (var i = 0, len = json.length; i < len; i++) {
                    html.push("<option value=\"" + json[i]["F_DictionaryID"] + "\">" + json[i]["F_DicName"] + "</option>");
                }
            }
            $("#" + el).html(html.join(""));
        }
    });
}

// 加载计量单位
function LoadUnit() {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_PRO_UNIT" },
        dataType: "json",
        success: function (json) {
            var html = [];
            if (json && json.length) {
                for (var i = 0, len = json.length; i < len; i++) {
                    html.push("<option value=\"" + json[i]["F_DictionaryID"] + "\">" + json[i]["F_DicName"] + "</option>");
                }
            }
            $("#sltUnit").html(html.join(""));
        }
    });
}

// 加载产品列表
function LoadProductList(subCtg, status, key) {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_PRODUCT_LIST", subCtg: subCtg, status: status, key: escape(key) },
        dataType: "json",
        success: function (json) {
            g.tbl = new Table(
                        {
                            el: "tbl",
                            datas: json,
                            PageSize: 15,
                            tblHeader:
                            {
                                F_Image: { text: "产品图片", format: function (path) {
                                    return "<img src=\"" + path + "\" width=\"100\" alt=\"\" style=\"margin:5px 0;\"/>";
                                }
                                },
                                F_ProductName: { text: "产品名称", sortType: "string" },
                                SubCtg: { text: "所属分类", sortType: "string" },
                                F_Code: { text: "产品货号" },
                                F_Alarm: { text: "报警数量" },
                                F_AddDate: { text: "添加日期", format: function (val) {
                                    var d = val.match("(.+) (.+)");
                                    return d[1];
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
                                filed: "F_ProductID", obj: { scan: "查看", edit: "编辑", unable: "禁用", able: "启用" }
                            },
                            width: "100%",
                            onbindend: function () {
                                g.tbl.el.find("tbody a[tag='unable'],tbody a[tag='able']").unbind("click").bind("click", function () {
                                    g.productID = $(this).attr("f_productid");
                                    if (!g.productID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    SetProductStatus(g.productID, $(this).attr("tag") === "able" ? 0 : 1);
                                });

                                g.tbl.el.find("tbody a[tag='edit']").unbind("click").bind("click", function () {
                                    g.productID = $(this).attr("f_productid");
                                    if (!g.productID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    GetProductDetail(true);
                                    g.u.MessageBox("divProduct", "编辑产品", null, null, SaveProduct);
                                });

                                g.tbl.el.find("tbody a[tag='scan']").unbind("click").bind("click", function () {
                                    g.productID = $(this).attr("f_productid");
                                    if (!g.productID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    GetProductDetail(false);
                                    g.u.MessageBox("divDetail", "查看详情", null, null, function () { g.u.dlg.dialog("destroy"); });
                                });
                            }
                        });
            g.tbl.Init();
        }
    });
}

// 保存员工
function SaveProduct() {
    var obj = {};
    obj.productID = g.productID;
    obj.tag = g.productID ? "UPDATE_PRODUCT" : "ADD_PRODUCT";
    obj.mainCtg = $("#sltBig").val();
    if (!obj.mainCtg) {
        alert("请选择产品所属大类.");
        return false;
    }
    obj.subCtg = $("#sltSmall").val();
    if (!obj.subCtg) {
        alert("请选择产品所属小类.");
        return false;
    }
    obj.pName = escape($("#txtPName").val());
    if (!obj.pName) {
        alert("请输入产品名称.");
        return false;
    }
    obj.pCode = $("#txtCode").val();
    obj.unit = $("#sltUnit").val();
    if (!obj.unit) {
        alert("请选择产品计量单位.");
        return false;
    }
    obj.size = $("#txtPSize").val();
    obj.weight = $("#txtPWeight").val();
    obj.pNum = $("#txtPNum").val();
    obj.alarm = $("#txtPAlarm").val();
    if (!obj.alarm) {
        alert("请输入库存报警数量.");
        return false;
    }
    obj.bonusType = $("#sltBonusType").val();
    obj.bonus = $("#txtPBonus").val();
    if (!obj.bonus) {
        alert("请输入提成额度.");
        return false;
    }
    obj.filePath = g.filePath;
    if (obj.tag === "ADD_PRODUCT" && !g.filePath) {
        alert("请上传产品图片.");
        return false;
    }
    obj.remark = $("#txtRemark").val();
    obj.status = $("#sltPStatus").val();

    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: obj,
        success: function (res) {
            if (res && res == "success") {
                $("#divProduct :text").val("");
                g.productID = "";
                g.filePath = "";
                LoadProductList();
                g.u.dlg.dialog("destroy");
            }
            else {
                alert("产品保存失败, 请稍后重试.");
            }
        }
    });
}

// 设置产品状态
function SetProductStatus(productID, status) {
    if (!productID) {
        alert("未知错误, 请稍后重试.");
        return false;
    }

    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "SET_PRODUCT_STATUS", productID: productID, status: status },
        success: function (res) {
            if (res && res == "success") {
                alert("产品状态设置成功.");
                g.productID = "";
                g.filePath = "";
                LoadProductList();
            }
            else {
                alert("产品状态设置删除失败, 请稍后重试.");
            }
        }
    });
}

// 获取员工详情
function GetProductDetail(isEdit) {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_PRODUCT_DETAIL", productID: g.productID },
        dataType: "json",
        success: function (json) {
            if (!json || !json.length) {
                alert("未知错误, 请稍后重试.");
                return false;
            }
            if (isEdit) {
                $("#txtPName").val(json[0]["F_ProductName"]);
                $("#sltBig").val(json[0]["F_MainCtg"]);
                LoadSubCtg(json[0]["F_MainCtg"], "sltSmall");
                $("#sltSmall").val(json[0]["F_SubCtg"]);
                $("#txtCode").val(json[0]["F_Code"]);
                $("#sltUnit").val(json[0]["F_Unit"]);
                $("#txtPSize").val(json[0]["F_Size"]);
                $("#txtPWeight").val(json[0]["F_Weight"]);
                $("#txtPNum").val(json[0]["F_Amount"]);
                $("#txtPAlarm").val(json[0]["F_Alarm"]);
                $("#sltZT").val(json[0]["F_Status"]);
                $("#sltBonusType").val(json[0]["F_BonusType"]);
                $("#txtPBonus").val(json[0]["F_Bonus"]);
                $("#txtRemark").val(json[0]["F_Remark"]);
                $("#sltPStatus").val(json[0]["F_Status"]);
            }
            else {
                $("#iImage").attr("src", json[0]["F_Image"]);
                $("#lName").text(json[0]["F_ProductName"]);
                $("#lCtg").text(json[0]["MainCtg"] + "-" + json[0]["SubCtg"]);
                $("#lCode").text(json[0]["F_Code"]);
                $("#lUnit").text(json[0]["Unit"]);
                $("#lSize").text(json[0]["F_Size"]);
                $("#lWeight").text(json[0]["F_Weight"]);
                $("#lNum").text(json[0]["F_Amount"]);
                $("#lAlarm").text(json[0]["F_Alarm"]);
                $("#lStatus").text(function () {
                    return json[0]["F_Status"] == "0" ? "启用" : "停用";
                });
                $("#lBonusType").text(function () {
                    return json[0]["F_BonusType"] == "0" ? "按百分比提成" : "固定提成";
                });
                $("#lBonus").text(json[0]["F_Bonus"]);
                $("#lRemark").html(json[0]["F_Remark"]);
                $("#lDate").text(json[0]["F_AddDate"]);
            }
        }
    });
}