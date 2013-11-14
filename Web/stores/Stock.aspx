<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Stock.aspx.cs" Inherits="stores_Stock" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>库存查询</title>
    <link href="../style/css/admin/admin.css" rel="stylesheet" type="text/css" />
    <link href="../js/jMetro/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../js/miniTips/css/minitip.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" class="searchbar">
        <tr>
            <th>
                所属分类:
            </th>
            <td>
                <select id="sltMainCtg" style="width:80px;">
                </select>
                <select id="sltSubCtg" style="width:80px;">
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
        <input type="button" class="btn" value="打印全部" id="btnPrintAll" />
        <input type="button" class="btn" value="打印所选" id="btnPrintSelect" />
    </div>
    <div id="tbl">
    </div>
    <div style="display:none" id="printContent">
        
    </div>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../js/Utils.js" type="text/javascript"></script>
    <script src="../js/jMetro/js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../js/jTable.js" type="text/javascript"></script>
    <script src="../js/miniTips/js/miniTip.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/PrintArea.js"></script>
    <script src="../js/store/stock.js" type="text/javascript"></script>
    </form>
