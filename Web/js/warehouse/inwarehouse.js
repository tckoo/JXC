var g = { u: new Utils(), url: "InWarehouse.aspx", tbl: null, tbl_product: null, tbl_detail: null, filePath: "", el: null };

$(function () {
    Init();
    RegEvent();
});

// 初始化加载
function Init() {
    $("#txtStartDate,#txtEndDate").datepicker();

    LoadInWarehouseList();

    LoadMainCtg();
    LoadSubCtg($("#sltMainCtg").val());
}

// 表单事件注册
function RegEvent() {
    // 产品入库事件
    $("#btnAdd").click(function () {
        $("#inwarehouse").slideDown();
    });

    $("#btnCancel").click(function () {
        $('#inwarehouse').slideUp();
        $("#tblIn tbody").html("");
    });

    // 选择产品
    $(".searchbar a[tag='slt']").die("click").live("click", function () {
        LoadProducts();
        g.el = this;
        g.u.mBox("divProducts", "选择产品", null);
    });

    // 添加行
    $("#tblIn a[tag='add']").die("click").live("click", function () {
        var html = [];
        html.push("<tr><th>选择产品:</th><td><a href=\"javascript:;\" tag=\"slt\" style=\"margin-right: 5px;\">点击选择</a><strong tag=\"product\"></strong>");
        html.push("</td><th>数量:</th><td><input type=\"text\" tag=\"num\" class=\"txt\" style=\"width: 30px;\" /></td>");
        html.push("<th>单价:</th><td><input type=\"text\" tag=\"price\" class=\"txt\" style=\"text-align: right; width: 30px;\" />");
        html.push("</td><th>合计:</th><td><strong tag=\"total\"></strong></td><td>");
        html.push("<a href=\"javascript:;\" tag=\"add\">继续添加</a><a href=\"javascript:;\" tag=\"remove\" style=\"margin-left: 10px;\">移除</a></td></tr>");
        $("#tblIn tbody").append(html.join(""));
    });

    // 查询入库单
    $("#btnSearch").click(function () {
        var startDate = $("#txtStartDate").val(),
            endDate = $("#txtEndDate").val(),
            code = $("#txtCode").val();
        LoadInWarehouseList(startDate, endDate, code);
    });

    $("#sltMainCtg").change(function () {
        LoadSubCtg($(this).val());
    });

    // 查询产品
    $("#btnFind").click(function () {
        LoadProducts($("#sltSubCtg").val(), $("#txtKey").val());
    });

    // 计算总价
    $("#tblIn :text[tag='num']").die("blur").live("blur", function () {
        var count = parseInt($(this).val()),
            price = parseFloat($(this).parent().parent().find(":text[tag='price']").val());
        $(this).parent().parent().find("strong[tag='total']").text((isNaN(count) ? 0 : count) * isNaN(price) ? 0 : price);
    });

    $("#tblIn :text[tag='price']").die("blur").live("blur", function () {
        var price = parseFloat($(this).val()),
            count = parseInt($(this).parent().parent().find(":text[tag='num']").val());
        $(this).parent().parent().find("strong[tag='total']").text(count * price);
    });

    // 移除行
    $("#tblIn a[tag='remove']").die("click").live("click", function () {
        $(this).parent().parent().remove();
    });

    // 产品入库
    $("#btnSave").click(function () {
        ProductInWarehouse();
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
            $("#sltMainCtg").html(html.join(""));
        }
    });
}

// 加载产品小类
function LoadSubCtg(ctgID) {
    if (!ctgID) {
        $("#sltSubCtg").html("<option value=\"\">--请选择--</option>");
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
            if (json && json.length) {
                for (var i = 0, len = json.length; i < len; i++) {
                    html.push("<option value=\"" + json[i]["F_DictionaryID"] + "\">" + json[i]["F_DicName"] + "</option>");
                }
            }
            $("#sltSubCtg").html(html.join(""));
        }
    });
}

// 加载入库单列表
function LoadInWarehouseList(startDate, endDate, code) {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_IN_WAREHOUSE_LIST", start: $("#txtStartDate").val(), end: $("#txtEndDate").val(), code: $("#txtCode").val() },
        dataType: "json",
        success: function (json) {
            g.tbl = new Table(
                        {
                            el: "tbl",
                            datas: json,
                            PageSize: 15,
                            tblHeader:
                            {
                                F_Code: { text: "单据编码" },
                                F_AddDate: { text: "入库日期", format: function (val) {
                                    var d = val.match("(.+) (.+)");
                                    return d[1];
                                }
                                },
                                F_Name: { text: "经办人" },
                                F_Total: { text: "总金额" },
                                F_IsPay: { text: "结账", format: function (val) {
                                    switch (val) {
                                        case "0": return "已结账";
                                        case "1": return "待结账";
                                        default: return "";
                                    }
                                }
                                },
                                F_Status: { text: "单据状态", format: function (val) {
                                    switch (val) {
                                        case "0": return "使用中";
                                        case "1": return "已作废";
                                        default: return "";
                                    }
                                }
                                }
                            },
                            IsOperate: true,
                            IsSelect: true,
                            operateData:
                            {
                                filed: "F_InOutLibraryID", obj: { valid: "启用单据", invalid: "单据作废", scan: "查看入库单明细", print: "打印入库单" }
                            },
                            width: "100%",
                            onbindend: function () {
                                g.tbl.el.find("tbody a[tag='invalid'],tbody a[tag='valid']").unbind("click").bind("click", function () {
                                    var libID = $(this).attr("F_InOutLibraryID"),
                                        tag = $(this).attr("tag");
                                    if (!libID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    SetSheetStatus(libID, tag === "invalid" ? 1 : 0);
                                });

                                g.tbl.el.find("tbody a[tag='scan']").unbind("click").bind("click", function () {
                                    var libID = $(this).attr("F_InOutLibraryID");
                                    if (!libID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    BindSheetDetail(libID);
                                    g.u.mBox("divDetail", "单据详情", null);
                                });

                                g.tbl.el.find("tbody a[tag='print']").unbind("click").bind("click", function () {
                                    var libID = $(this).attr("F_InOutLibraryID");
                                    if (!libID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    window.open('../inc/_in_library_sheet.aspx?sheetId=' + libID, '', 'directorys=no,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,width=700,top=150,left=740');
                                });
                            }
                        });
            g.tbl.Init();
        }
    });
}

// 加载产品列表
function LoadProducts(subCtg, key) {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_PRODUCT_LIST", subCtg: subCtg, status: status, key: escape(key) },
        dataType: "json",
        success: function (json) {
            g.tbl_product = new Table(
                        {
                            el: "tbl_product",
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
                                F_Alarm: { text: "报警数量" }
                            },
                            IsOperate: true,
                            operateData:
                            {
                                filed: "F_ProductID", obj: { select: "选择" }
                            },
                            width: "100%",
                            onbindend: function () {
                                g.tbl_product.el.find("tbody a[tag='select']").unbind("click").bind("click", function () {
                                    var productId = $(this).attr("F_ProductID"),
                                    productName = $(this).parent().parent().children().eq(1).text();
                                    if (!productId) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    g.u.mdlg.dialog("destroy");
                                    $("#tblIn a[tag='slt']").each(function () {
                                        if ($(this).attr("productId") == productId) {
                                            alert("您已经选择过此产品, 请重新选择.");
                                            g.el = null;
                                            return false;
                                        }
                                    });
                                    $(g.el).attr("productId", productId).nextAll().filter("strong[tag='product']").text(productName);
                                    g.el = null;
                                });
                            }
                        });
            g.tbl_product.Init();
        }
    });
}

// 产品入库
function ProductInWarehouse() {
    var proList = [],
        numList = [],
        priceList = [],
        isPay,
        remark;
    $("#tblIn a[tag='slt'][productId]").each(function () {
        var productId = $(this).attr("productId"),
            num = $(this).parent().parent().find(":text[tag='num']").val(),
            price = $(this).parent().parent().find(":text[tag='price']").val();
        if (productId) {
            if (!num) {
                alert("请输入入库数量.");
                return false;
            }
            if (!price) {
                alert("请输入入库价格.");
                return false;
            }
            proList.push(productId);
            numList.push(num);
            priceList.push(price);
        }
    });
    if (!proList.length) {
        alert("请选择入库的产品.");
        return false;
    }
    isPay = $("#tblIn :radio[name='pay']:checked").val();
    remark = escape($("#txtRemark").val());
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "PRODUCT_INWAREHOUSE", proList: proList.join(";"), numList: numList.join(";"), priceList: priceList.join(";"), isPay: isPay, remark: remark },
        success: function (res) {
            if (res && res === "success") {
                alert("产品入库成功.");
                LoadInWarehouseList();
                $("#btnCancel").click();
                $("#tblIn a[tag='slt']").attr("productId", null);
                $("#tblIn strong[tag='product']").text("");
                $("#tblIn :text,#tblIn textarea").val("");
            }
            else {
                alert("产品入库失败, 请稍后重试.");
            }
        }
    });
}

// 设置单据状态
function SetSheetStatus(libID, status) {
    if (!libID) return;
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "SET_SHEET_STATUS", libID: libID, status: status },
        success: function (res) {
            var msg = status === 0 ? "启用" : "作废";
            if (res && res === "success") {
                LoadInWarehouseList();
                alert("单据已成功" + msg + ".");
            }
            else if (res === "unupdate") {
                alert("此单据已是" + msg + "状态, 请勿重复操作.");
            }
            else if (res === "noexist") {
                alert("此单据不存在.");
            }
            else {
                alert("单据" + msg + "失败, 请稍后重试.");
            }
        }
    });
}

// 绑定入库单据详情
function BindSheetDetail(libID) {
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_SHEET_DETAIL", libID: libID },
        dataType: "json",
        success: function (json) {
            g.tbl_detail = new Table(
                        {
                            el: "tbl_detail",
                            datas: json,
                            PageSize: 15,
                            tblHeader:
                            {
                                F_Image: { text: "产品图片", format: function (path) {
                                    return "<img src=\"" + path + "\" width=\"60\" alt=\"\" style=\"margin:5px 0;\"/>";
                                }
                                },
                                F_ProductName: { text: "产品名称", sortType: "string" },
                                F_Code: { text: "产品货号" },
                                F_Amount: { text: "数量" },
                                F_Price: { text: "单价" },
                                F_Total: { text: "总价" }
                            },
                            width: "100%"
                        });
            g.tbl_detail.Init();
        }
    });
}