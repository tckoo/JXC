<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dictionary.aspx.cs" Inherits="admin_Dictionary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>字典维护</title>
    <link href="../js/jMetro/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../style/css/admin/admin.css" rel="stylesheet" type="text/css" />
    <link href="../js/JQuery_zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="leftbar">
        <div class="functionbabr">
            <input type="button" class="btn" value="添加分类" id="btnAdd" />
            <input type="button" class="btn" value="编辑分类" id="btnEdit" />
            <input type="button" class="btn" value="删除分类" id="btnDelete" />
        </div>
        <!-- 实际开发采用zTree加载 -->
        <div id="divTree" class="ztree">
        </div>
    </div>
    <div class="rightbar">
        <table cellpadding="0" cellspacing="0" border="0" class="tbl">
            <tr>
                <th>
                    字典名称:
                </th>
                <td>
                    <input type="text" id="txtName" class="txt" />
                </td>
            </tr>
            <tr>
                <th>
                    字典编码:
                </th>
                <td>
                    <input type="text" id="txtCode" class="txt" />
                </td>
            </tr>
            <tr>
                <th>
                    状态:
                </th>
                <td>
                    <select id="sltStatus">
                        <option value="0">启用</option>
                        <option value="1">禁用</option>
                    </select>
                </td>
            </tr>
            <tr style="height: 100px; line-height: 100px;">
                <td colspan="2" align="center">
                    <input type="button" class="btn" value="添加同级" id="btnSame" disabled="disabled" />
                    <input type="button" class="btn" value="添加子级" id="btnChild" disabled="disabled" />
                    <input type="button" class="btn" value="保存" id="btnSave" disabled="disabled" />
                </td>
            </tr>
        </table>
        <!-- 实际开发时使用Table插件 -->
    </div>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../js/JQuery_zTree/js/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../js/admin/dictionary.js" type="text/javascript"></script>
    </form>
</body>
</html>
