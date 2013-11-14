<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Product.aspx.cs" Inherits="warehouse_Product" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品管理</title>
    <link href="../style/css/admin/admin.css" rel="stylesheet" type="text/css" />
    <link href="../js/jMetro/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../js/jquery.uploadify/uploadify.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
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
                状态:
            </th>
            <td>
                <select id="sltStatus">
                    <option value="">不限</option>
                    <option value="0">启用</option>
                    <option value="1">禁用</option>
                </select>
            </td>
            <th>
                关键字:
            </th>
            <td>
                <input type="text" id="txtKey" class="txt" style="width: 160px;" />
            </td>
            <td>
                <input type="button" id="btnSearch" class="btn" value="搜索" />
            </td>
        </tr>
    </table>
    <div class="hr">
    </div>
    <div class="functionbabr">
        <input type="button" class="btn" value="添加产品" id="btnAdd" />
    </div>
    <div id="tbl">
    </div>
    <!-- 添加产品 -->
    <div id="divProduct" class="pop" style="display: none;">
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <th>
                    所属分类:
                </th>
                <td>
                    <select id="sltBig">
                    </select>
                    <select id="sltSmall">
                    </select>
                </td>
                <th>
                    产品名称:
                </th>
                <td>
                    <input type="text" id="txtPName" class="txt" />
                </td>
            </tr>
            <tr>
                <th>
                    货号:
                </th>
                <td>
                    <input type="text" id="txtCode" class="txt" />
                </td>
                <th>
                    单位:
                </th>
                <td>
                    <select id="sltUnit">
                    </select>
                </td>
            </tr>
            <tr>
                <th>
                    规格尺寸:
                </th>
                <td>
                    <input type="text" id="txtPSize" class="txt" />
                </td>
                <th>
                    产品净重:
                </th>
                <td>
                    <input type="text" id="txtPWeight" class="txt" />
                </td>
            </tr>
            <tr>
                <th>
                    装箱数量:
                </th>
                <td>
                    <input type="text" id="txtPNum" class="txt" />
                </td>
                <th>
                    报警数量:
                </th>
                <td>
                    <input type="text" id="txtPAlarm" class="txt" />
                </td>
            </tr>
            <tr>
                <th>
                    提成类型:
                </th>
                <td>
                    <select id="sltBonusType">
                        <option value="0">百分比</option>
                        <option value="1">固定金额</option>
                    </select>
                </td>
                <th>
                    提成额度:
                </th>
                <td>
                    <input type="text" id="txtPBonus" class="txt" />
                </td>
            </tr>
            <tr>
                <th>
                    产品图片:
                </th>
                <td colspan="3">
                    <div id="fileQueue">
                    </div>
                    <div style="position: relative; height: 40px; line-height: 40px;">
                        <input type="file" name="uploadify" id="uploadify" />
                        <input type="button" id="btnUpload" value="上传" class="btn" style="margin-left: 20px;
                            border-color: #aaa; position: absolute; top: 3px;" />
                    </div>
                </td>
            </tr>
            <tr>
                <th>
                    产品描述:
                </th>
                <td colspan="3">
                    <textarea rows="5" cols="83" id="txtRemark"></textarea>
                </td>
            </tr>
            <tr>
                <th>
                    状态:
                </th>
                <td>
                    <select id="sltPStatus">
                        <option value="0">启用</option>
                        <option value="1">禁用</option>
                    </select>
                </td>
            </tr>
        </table>
    </div>
    <!-- 获取产品详情 -->
    <div id="divDetail" style="display: none;" class="pop">
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <th>
                    产品图片:
                </th>
                <td colspan="3">
                    <img id="iImage" src="" width="100" alt="" />
                </td>
            </tr>
            <tr>
                <th>
                    产品名称:
                </th>
                <td>
                    <label id="lName">
                    </label>
                </td>
                <th>
                    产品类别:
                </th>
                <td>
                    <label id="lCtg">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    货号:
                </th>
                <td>
                    <label id="lCode">
                    </label>
                </td>
                <th>
                    单位:
                </th>
                <td>
                    <label id="lUnit">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    规格尺寸:
                </th>
                <td>
                    <label id="lSize">
                    </label>
                </td>
                <th>
                    产品净重:
                </th>
                <td>
                    <label id="lWeight">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    装箱数量:
                </th>
                <td>
                    <label id="lNum">
                    </label>
                </td>
                <th>
                    报警数量:
                </th>
                <td>
                    <label id="lAlarm">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    提成类型:
                </th>
                <td>
                    <label id="lBonusType">
                    </label>
                </td>
                <th>
                    提成额度:
                </th>
                <td>
                    <label id="lBonus">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    产品描述:
                </th>
                <td colspan="3">
                    <div id="lRemark">
                    </div>
                </td>
            </tr>
            <tr>
                <th>
                    状态:
                </th>
                <td>
                    <label id="lStatus">
                    </label>
                </td>
                <th>
                    添加时间:
                </th>
                <td>
                    <label id="lDate">
                    </label>
                </td>
            </tr>
        </table>
    </div>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../js/Utils.js" type="text/javascript"></script>
    <script src="../js/jMetro/js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../js/jTable.js" type="text/javascript"></script>
    <script src="../js/jquery.uploadify/swfobject.js" type="text/javascript"></script>
    <script src="../js/jquery.uploadify/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../js/warehouse/product.js" type="text/javascript"></script>
    </form>
</body>
</html>
