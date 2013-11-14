<%@ Page Language="C#" AutoEventWireup="true" CodeFile="_in_library_sheet.aspx.cs"
    Inherits="inc_in_library_sheet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>入库单据-打印</title>
    <style type="text/css">
        table
        {
            font: normal 14px "Arial Narrow" , HELVETICA, "宋体" , "微软雅黑";
        }
        table tr
        {
            height: 25px;
            line-height: 1.5em;
        }
        table tbody
        {
            text-align: center;
        }
        .txt
        {
            width: 400px;
            border: none;
            border-bottom: solid 1px #000;
        }
        .tbl
        {
            width: 100%;
            border: solid 1px #000;
            border-collapse: collapse;
            margin: 10px 0px;
        }
        .tbl td, .tbl th
        {
            border: solid 1px #000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 40px; line-height: 40px; border-bottom: dashed 1px #aaa;">
        <input type="button" value="【打印单据】" onclick="_print();" />
    </div>
    <!--startprint-->
    <table cellpadding="0" cellspacing="0" border="0" width="95%" frame="box" style="margin: 0 auto;">
        <caption style="font-size: 20px; font-weight: bold; height: 50px; line-height: 50px;">
            商品入库单
        </caption>
        <thead>
            <tr>
                <th>
                    单&nbsp;&nbsp;据&nbsp;&nbsp;号:
                </th>
                <td>
                    <label id="lCode">
                    </label>
                </td>
                <th>
                    时间:
                </th>
                <td>
                    <label id="lDate">
                    </label>
                    <th>
                        经办人:
                    </th>
                    <td>
                        <label id="lUser">
                        </label>
                    </td>
            </tr>
            <tr>
                <th>
                    往来单位:
                </th>
                <td colspan="5">
                    <input type="text" class="txt" />
                </td>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td colspan="6">
                    <table cellpadding="0" cellspacing="0" border="0" class="tbl" id="tbl">
                        <thead>
                            <tr style="height: 40px; line-height: 40px;">
                                <th>
                                    商品名称
                                </th>
                                <th>
                                    货号
                                </th>
                                <th>
                                    规格尺寸
                                </th>
                                <th>
                                    数量
                                </th>
                                <th>
                                    单价
                                </th>
                                <th>
                                    小计
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                        <tfoot>
                            <tr style="height: 40px; line-height: 40px;">
                                <th>
                                    合计:
                                </th>
                                <td colspan="5" align="right" id="tTotal">
                                    <b id="bTotal"></b>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <th>
                    制&nbsp;&nbsp;单&nbsp;&nbsp;人:
                </th>
                <td colspan="5">
                    <%=Session["person_name"]%>
                </td>
            </tr>
        </tfoot>
    </table>
    <!--endprint-->
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../js/Utils.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var u = new Utils({ site: document.URL }),
                params = u.GetParams(),
                sheetId = params["sheetId"];
            if (!sheetId) {
                window.close();
            }
            else {
                $.ajax({
                    url: "_in_library_sheet.aspx",
                    type: "POST",
                    data: { tag: "GET_SHEET", sheetId: sheetId },
                    dataType: "json",
                    async: false,
                    success: function (json) {
                        if (!json.sheet.length || !json.detail.length) {
                            window.close();
                        }
                        else {
                            var sheet = json.sheet[0],
                                html = [];
                            $("#lCode").text(sheet["F_Code"]);
                            $("#lDate").text(sheet["F_AddDate"].split(" ")[0]);
                            $("#lUser").text(sheet["F_Name"]);
                            $("#bTotal").text(sheet["F_Total"]);

                            for (var i = 0, len = json.detail.length; i < len; i++) {
                                html.push("<tr>");
                                html.push("<td align='left'>" + json.detail[i]["F_ProductName"] + "</td>");
                                html.push("<td>" + json.detail[i]["F_Code"] + "</td>");
                                html.push("<td>" + json.detail[i]["F_Size"] + "</td>");
                                html.push("<td>" + json.detail[i]["F_Amount"] + "</td>");
                                html.push("<td>" + json.detail[i]["F_Price"] + "</td>");
                                html.push("<td>" + json.detail[i]["F_Total"] + "</td>");
                                html.push("</tr>");
                            }
                            $("#tbl tbody").html(html.join(""));
                        }
                    }
                });
            }
        });
        function _print() {
            var bdhtml = window.document.body.innerHTML,
                sprnstr = "<!--startprint-->",
                eprnstr = "<!--endprint-->",
                prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 17),
                prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            window.document.body.innerHTML = prnhtml;
            window.print();
            window.document.body.innerHTML = bdhtml;
        }
    </script>
    </form>
</body>
</html>
