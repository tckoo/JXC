var g = { u: new Utils(), url: "Stock.aspx", tbl: null, filePath: "", productID: "" };

$(function () {
    Init();
    RegEvent();
});

// 初始化加载
function Init() {
    LoadMainCtg();
    LoadSubCtg($("#sltMainCtg").val());

    LoadProductList();
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

    // 查询对象
    $("#btnSearch").click(function () {
        var subCtg = $("#sltSubCtg").val(),
            key = $("#txtKey").val();
        LoadProductList(subCtg, key);
    });

    $("#btnPrintAll,#btnPrintSelect").click(function () {
        var id = $(this).attr("id"),
            product_array = [];
        if (id === "btnPrintAll") {
            if (g.tbl.datas && g.tbl.datas.length) {
                for (var i = 0, len = g.tbl.datas.length; i < len; i++) {
                    product_array.push(g.tbl.datas[i]["F_ProductID"]);
                }
            }
        }
        if (id === "btnPrintSelect") {
            product_array = g.tbl.getSelected();
        }
        if (!product_array || !product_array.length) {
            alert("对不起, 没有可打印的数据.");
            return;
        }
        window.open('../inc/_stock_sheet.aspx?r=' + Math.random() + '&product_array=' + product_array.join(";"), '', 'directorys=no,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,width=800,top=150,left=740');
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
            html.push("<option value=\"\">不限</option>");
            if (json && json.length) {
                for (var i = 0, len = json.length; i < len; i++) {
                    html.push("<option value=\"" + json[i]["F_DictionaryID"] + "\">" + json[i]["F_DicName"] + "</option>");
                }
            }
            $("#sltSubCtg").html(html.join(""));
        }
    });
}

// 加载产品列表
function LoadProductList(subCtg, key) {
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
                                Unit: { text: "单位" },
                                F_Size: { text: "规格尺寸" },
                                WarehouseNum: { text: "库存数量", format: function (val, row) {
                                    var wNum = parseInt(val) || 0,
                                        sNum = parseInt(row["StoreNum"]) || 0;
                                    return "共<b><i>" + (wNum + sNum) + "</i></b>, 仓库<b>"
                                             + wNum + "</b>, 门店<span class=\"tip\" title=\"" + row["StoreDetail"] + "\">" + sNum + "</span>";
                                }
                                },
                                F_Amount: { text: "装箱数量" },
                                LastInPrice: { text: "入库价格", format: function (val) {
                                    return val || 0;
                                }
                                }
                            },
                            IsOperate: true,
                            IsSelect: true,
                            operateData:
                            {
                                filed: "F_ProductID", obj: { indetail: "入库明细", outdetail: "出库明细" }
                            },
                            width: "100%",
                            onbindend: function () {
                                g.tbl.el.find("tbody a[tag='indetail'],tbody a[tag='outdetail']").unbind("click").bind("click", function () {
                                    g.productID = $(this).attr("f_productid");
                                    if (!g.productID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }
                                    var tag = $(this).attr("tag");
                                    g.u.mBox("divDetail", tag === "indetail" ? "入库明细" : "出库明细", null);
                                    GetInOutWarehoueDetail(g.productID, tag === "indetail" ? 0 : 1);
                                });

                                g.tbl.el.find("tbody .tip").each(function () {
                                    if ($(this).attr("title")) {
                                        $(this).miniTip({ "className": "blue" });
                                    }
                                });
                            }
                        });
            g.tbl.Init();
        }
    });
}

// 查看产品出入库详情
function GetInOutWarehoueDetail(proID, type) {
    if (!proID) return;
    $.ajax({
        url: g.url,
        type: "POST",
        async: false,
        data: { tag: "GET_IN_OUT_WAREHOUSE_DETAIL", productID: proID, type: type },
        dataType: "json",
        success: function (json) {
            var t = new Table(
                        {
                            el: "divDetail",
                            datas: json,
                            PageSize: 15,
                            tblHeader:
                            {
                                F_Amount: { text: "数量" },
                                F_Price: { text: "价格" },
                                F_Total: { text: "总价" },
                                F_AddDate: { text: "日期", format: function (val) {
                                    var d = val.match("(.+) (.+)");
                                    return d[1];
                                }
                                }
                            },
                            width: "100%"
                        });
            t.Init();
        }
    });
}