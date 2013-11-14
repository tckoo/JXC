<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Person.aspx.cs" Inherits="admin_Person" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>员工管理</title>
    <link href="../style/css/admin/admin.css" rel="stylesheet" type="text/css" />
    <link href="../js/jMetro/css/jquery-ui.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" class="searchbar">
        <tr>
            <th>
                所属对象:
            </th>
            <td>
                <select id="sltObj">
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
                是否管理:
            </th>
            <td>
                <select id="sltAdmin">
                    <option value="">不限</option>
                    <option value="0">是</option>
                    <option value="1">否</option>
                </select>
            </td>
            <th>
                入职日期:
            </th>
            <td>
                <input type="text" id="txtEnterDate" class="txt" style="width: 160px;" />
            </td>
            <th>
                姓名:
            </th>
            <td>
                <input type="text" id="txtName" class="txt" style="width: 160px;" />
            </td>
            <td>
                <input type="button" id="btnSearch" class="btn" value="搜索" />
            </td>
        </tr>
    </table>
    <div class="hr">
    </div>
    <div class="functionbabr">
        <input type="button" class="btn" value="添加员工" id="btnAdd" />
        <input type="button" class="btn" value="删除员工" id="btnDelete" />
    </div>
    <div id="tbl">
    </div>
    <!-- 弹出层相关 -->
    <div id="divPerson" style="display: none;" class="pop">
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <th>
                    姓名:
                </th>
                <td>
                    <input type="text" id="txtPName" class="txt" />
                </td>
                <th>
                    所属对象:
                </th>
                <td>
                    <select id="sltObject">
                    </select>
                </td>
            </tr>
            <tr>
                <th>
                    性别:
                </th>
                <td>
                    <select id="sltSex">
                        <option value="0">男</option>
                        <option value="1">女</option>
                    </select>
                </td>
                <th>
                    联系电话:
                </th>
                <td>
                    <input type="text" id="txtTel" class="txt" />
                </td>
            </tr>
            <tr>
                <th>
                    基本工资:
                </th>
                <td>
                    <input type="text" id="txtMoney" class="txt" />
                </td>
                <th>
                    证件号码:
                </th>
                <td>
                    <input type="text" id="txtCard" class="txt" />
                </td>
            </tr>
            <tr>
                <th>
                    家庭住址:
                </th>
                <td colspan="3">
                    <input type="text" id="txtAddress" class="ltxt" />
                </td>
            </tr>
            <tr>
                <th>
                    入职日期:
                </th>
                <td>
                    <input type="text" id="txtEDate" class="txt" />
                </td>
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
            <tr>
                <th>
                    是否管理员:
                </th>
                <td>
                    <input type="radio" name="admin" id="rdYes" value="0" /><label for="rdYes">是</label>
                    <input type="radio" name="admin" id="rdNo" value="1" checked="checked" /><label for="rdNo">否</label>
                </td>
                <th style="display: none;">
                    登录名:
                </th>
                <td style="display: none;">
                    <input type="text" id="txtAccount" class="txt" />
                </td>
            </tr>
        </table>
    </div>
    <!-- 查看详情 -->
    <div id="divDetail" style="display: none;" class="pop">
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <th>
                    姓名:
                </th>
                <td>
                    <label id="lName">
                    </label>
                </td>
                <th>
                    所属对象:
                </th>
                <td>
                    <label id="lObj">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    性别:
                </th>
                <td>
                    <label id="lSex">
                    </label>
                </td>
                <th>
                    联系电话:
                </th>
                <td>
                    <label id="lTel">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    基本工资:
                </th>
                <td>
                    <label id="lWage">
                    </label>
                </td>
                <th>
                    证件号码:
                </th>
                <td>
                    <label id="lCard">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    家庭住址:
                </th>
                <td colspan="3">
                    <label id="lAddress">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    入职日期:
                </th>
                <td>
                    <label id="lEnterDate">
                    </label>
                </td>
                <th>
                    状态:
                </th>
                <td>
                    <label id="lStatus">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    是否管理员:
                </th>
                <td colspan="3">
                    <label id="lIsAdmin">
                    </label>
                </td>
            </tr>
            <tr>
                <th>
                    登录名:
                </th>
                <td>
                    <b id="lAccount"></b>
                </td>
                <th>
                    密码:
                </th>
                <th>
                    <b id="lPwd"></b>
                </th>
            </tr>
        </table>
    </div>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../js/jMetro/js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../js/Utils.js" type="text/javascript"></script>
    <script src="../js/jTable.js" type="text/javascript"></script>
    <script src="../js/admin/person.js" type="text/javascript"></script>
    </form>
</body>
</html>
