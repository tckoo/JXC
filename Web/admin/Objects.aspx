<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Objects.aspx.cs" Inherits="admin_Objects" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>分店仓库管理</title>
    <link href="../style/css/admin/admin.css" rel="stylesheet" type="text/css" />
    <link href="../js/jMetro/css/jquery-ui.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" class="searchbar">
        <tr>
            <th>
                类型:
            </th>
            <td>
                <select id="sltType">
                    <option value="">不限</option>
                    <option value="1">分店</option>
                    <option value="2">仓库</option>
                </select>
            </td>
            <th>
                状态
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
                <input type="text" id="txtKey" class="txt" />
            </td>
            <td>
                <input type="button" id="btnSearch" value="搜索" class="btn" />
            </td>
        </tr>
    </table>
    <div class="hr">
    </div>
    <div class="functionbabr">
        <input type="button" class="btn" value="添加对象" id="btnAdd" />
        <input type="button" class="btn" value="删除对象" id="btnDelete" />
    </div>
    <div id="tbl">
    </div>
    <!-- 弹出层相关 -->
    <div id="divObjects" style="display: none;" class="pop">
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <th>
                    对象类型:
                </th>
                <td>
                    <select id="sltCtg">
                        <option value="1">分店</option>
                        <option value="2">仓库</option>
                    </select>
                </td>
                <th>
                    所属仓库:
                </th>
                <td>
                    <select id="sltQY">
                    </select>
                </td>
            </tr>
            <tr>
                <th>
                    对象名称:
                </th>
                <td>
                    <input type="text" id="txtName" class="txt" />
                </td>
            </tr>
            <tr>
                <th>
                    联系人:
                </th>
                <td>
                    <input type="text" id="txtContact" class="txt" />
                </td>
                <th>
                    联系电话:
                </th>
                <td>
                    <input type="text" id="txtPhone" class="txt" />
                </td>
            </tr>
            <tr>
                <th>
                    地址:
                </th>
                <td colspan="3">
                    <input type="text" id="txtAddress" class="ltxt" />
                </td>
            </tr>
            <tr>
                <th>
                    状态:
                </th>
                <td>
                    <select id="sltZT">
                        <option value="0">启用</option>
                        <option value="1">禁用</option>
                    </select>
                </td>
            </tr>
        </table>
    </div>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../js/jMetro/js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../js/Utils.js" type="text/javascript"></script>
    <script src="../js/jTable.js" type="text/javascript"></script>
    <script src="../js/admin/objects.js" type="text/javascript"></script>
    </form>
</body>
</html>
