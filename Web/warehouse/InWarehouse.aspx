<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InWarehouse.aspx.cs" Inherits="warehouse_InWarehouse" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品入库</title>
    <link href="../style/css/admin/admin.css" rel="stylesheet" type="text/css" />
    <link href="../js/jMetro/css/jquery-ui.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" class="searchbar">
        <tr>
            <th>
                开始日期:
            </th>
            <td>
                <input type="text" class="txt" id="txtStartDate" value="<%=MonthFirstDay %>" />
            </td>
            <th>
                结束日期:
            </th>
            <td>
                <input type="text" class="txt" id="txtEndDate" value="<%=MonthLastDay %>" />
            </td>
            <th>
                单据编码:
            </th>
            <td>
                <input type="text" class="txt" id="txtCode" />
            </td>
            <td>
                <input type="button" id="btnSearch" class="btn" value="搜索" />
            </td>
        </tr>
    </table>
    <div class="hr">
    </div>
    <div class="functionbabr">
        <input type="button" class="btn" value="产品入库" id="btnAdd" />
    </div>
    <fieldset id="inwarehouse" style="padding: 10px 5px; display: none;">
        <legend style="font-size: 14px;">产品入库</legend>
        <table cellpadding="0" cellspacing="0" border="0" class="searchbar" id="tblIn">
            <thead>
                <tr>
                    <th>
                        选择产品:
                    </th>
                    <td>
                        <a href="javascript:;" tag="slt" style="margin-right: 5px;">点击选择</a><strong tag="product"></strong>
                    </td>
                    <th>
                        数量:
                    </th>
                    <td>
                        <input type="text" tag="num" class="txt" style="width: 30px;" />
                    </td>
                    <th>
                        单价:
                    </th>
                    <td>
                        <input type="text" tag="price" class="txt" style="text-align: right; width: 30px;" />
                    </td>
                    <th>
                        合计:
                    </th>
                    <td>
                        <strong tag="total"></strong>
                    </td>
                    <td>
                        <a href="javascript:;" tag="add">继续添加</a>
                    </td>
                </tr>
            </thead>
            <tbody>
            </tbody>
            <tfoot>
                <tr>
                    <th>
                        是否支付:
                    </th>
                    <td colspan="8">
                        <input type="radio" name="pay" id="rdYes" checked="checked" value="0" /><label for="rdYes">已支付</label>
                        <input type="radio" name="pay" id="rdNo" value="1" /><label for="rdNo">待支付</label>
                    </td>
                </tr>
                <tr>
                    <th valign="top">
                        备注:
                    </th>
                    <td colspan="8">
                        <textarea id="txtRemark" cols="70" rows="5"></textarea>
                    </td>
                </tr>
                <tr style="height: 60px; line-height: 60px;">
                    <td colspan="9" align="right">
                        <input type="button" class="btn" value="保存" id="btnSave" />
                        <input type="button" class="btn" value="取消" id="btnCancel" />
                    </td>
                </tr>
            </tfoot>
        </table>
    </fieldset>
    <div id="tbl">
    </div>
    <!-- 产品列表 -->
    <div id="divProducts" style="display: none;">
        <table cellpadding="0" cellspacing="0" border="0" class="searchbar">
            <tr>
                <th>
                    所属分类:
                </th>
                <td>
                    <select id="sltMainCtg">
                    </select>
                    <select id="sltSubCtg">
                    </select>
                </td>
                <th>
                    关键字:
                </th>
                <td>
                    <input type="text" id="txtKey" class="txt" style="width: 160px;" />
                </td>
                <td>
                    <input type="button" id="btnFind" class="btn" value="搜索" />
                </td>
            </tr>
        </table>
        <div id="tbl_product">
        </div>
    </div>
    <!-- 单据详情 -->
    <div id="divDetail" style="display: none;">
        <div id="tbl_detail">
        </div>
    </div>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../js/Utils.js" type="text/javascript"></script>
    <script src="../js/jMetro/js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../js/jTable.js" type="text/javascript"></script>
    <script src="../js/warehouse/inwarehouse.js" type="text/javascript"></script>
    </form>
</body>
</html>
