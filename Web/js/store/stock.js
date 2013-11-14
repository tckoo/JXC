var g = { u: new Utils(), url: "Stock.aspx", tbl: null, filePath: "", productID: "" };

$(document).ready(function () {
    LoadMainCtg();
    $("#sltSubCtg").html("<option value=\"\">--请选择--</option>");
    $("#sltMainCtg").change(function () {
        var ctgID = $(this).val();
        LoadSubCtg(ctgID);
    });
    LoadProductList("", "");

    $("#btnSearch").click(function () {

        LoadProductList($("#sltSubCtg").val(), $("#txtKey").val());
    });

    $("#btnPrintSelect").click(function () {
        PrintSelect();
    });

    $("#btnPrintAll").click(function () {
        PrintAll();
    });
});

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
            html.push("<option value=''>--请选择--</option>")
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
                                    var wNum = parseInt(val),
                                        sNum = parseInt(row["StoreNum"]);
                                    return "共<b><i>" + (wNum + sNum) + "</i></b>, 仓库<b>"
                                             + wNum + "</b>, 门店<span class=\"tip\" title=\"" + row["StoreDetail"] + "\">" + sNum + "</span>";
                                }
                                },
                                F_Amount: { text: "装箱数量" },
                                LastInPrice: { text: "入库价格" }
                            },
                            IsOperate: true,
                            IsSelect: true,
                            operateData:
                            {
                                filed: "F_ProductID", obj: { indetail: "入库明细", outdetail: "出库明细" }
                            },
                            width: "100%",
                            onbindend: function () {
                                // 查看入库明细
                                g.tbl.el.find("tbody a[tag='indetail']").unbind("click").bind("click", function () {
                                    g.productID = $(this).attr("f_productid");
                                    alert(g.productID);
                                    if (!g.productID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }

                                });

                                // 查看出库明细
                                g.tbl.el.find("tbody a[tag='outdetail']").unbind("click").bind("click", function () {
                                    g.productID = $(this).attr("f_productid");
                                    if (!g.productID) {
                                        alert("未知错误, 请稍后重试.");
                                        return false;
                                    }

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


function PrintSelect() {
    var choiceSize = $("#tbl table tbody input[type='checkbox'][checked='true']").size();
    if (choiceSize) {
        var tdCount = $("#tbl table thead td").size();
        var titleHtml = "<TR sizeache='6' sizset='0' align='center'>";
        var i = 1;
        $("#tbl table thead td").each(function () {
            if (i !== 1 && i !== tdCount) {
                titleHtml += "<td style='" + $(this).attr("style") + "'>" + $(this).html() + "</td>";
            }
            i += 1;
        });
        titleHtml += "</TR>";
        var contentHtml = "";

        $("#tbl table tbody input[type='checkbox'][checked='true']").each(function () {
            var parentTr = $($($(this).parent()).parent()).html();
            var starTdIndex = parentTr.indexOf("</TD>");
            var endTdIndex = parentTr.lastIndexOf("<TD");
            contentHtml += "<TR align='center'>" + parentTr.substring(starTdIndex + 5, endTdIndex) + "</TR>";
        });

        var printContent = "<!--startprint1--><table>" + titleHtml + contentHtml + "</table><!--endprint1-->  ";
        // alert(printContent)
        $("#printContent").html(printContent).printArea();
    } else {
        alert("请选择需要打印的项。");
    }
}


function PrintAll() {
    var tdCount = $("#tbl table thead td").size();
    var titleHtml = "<TR sizeache='6' sizset='0' align='center'>";
    var i = 1;
    $("#tbl table thead td").each(function () {
        if (i !== 1 && i !== tdCount) {
            titleHtml += "<td style='" + $(this).attr("style") + "'>" + $(this).html() + "</td>";
        }
        i += 1;
    });
    titleHtml += "</TR>";
    var contentHtml = "";

    $("#tbl table tbody input[type='checkbox']").each(function () {
        var parentTr = $($($(this).parent()).parent()).html();
        var starTdIndex = parentTr.indexOf("</TD>");
        var endTdIndex = parentTr.lastIndexOf("<TD");
        contentHtml += "<TR align='center'>" + parentTr.substring(starTdIndex + 5, endTdIndex) + "</TR>";
    });

    var printContent = "<!--startprint1--><table>" + titleHtml + contentHtml + "</table><!--endprint1-->  ";
    // alert(printContent)
    $("#printContent").html(printContent).printArea();
}


