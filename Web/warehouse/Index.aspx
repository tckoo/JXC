﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="admin_Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Web进销存-管理员</title>
    <link href="../style/css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="top" id="top">
        <table width="100%" height="112" cellpadding="0" cellspacing="0" border="0">
            <tr height="84">
                <td width="343" colspan="2">
                    <h4 class="logo">
                        WEB进销存-仓管版
                    </h4>
                </td>
                <td colspan="2">
                    <table width="82%" height="62" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="center">
                                <a href="desk.asp" target="right">
                                    <img src="../style/images/top_nav1.png" width="73" height="63" border="0"></a>
                            </td>
                            <td align="center">
                                <a href="produit/produit_sell.asp" target="right">
                                    <img src="../style/images/top_nav2.png" width="73" height="63" border="0"></a>
                            </td>
                            <td align="center">
                                <a href="produit/produit_add.asp" target="right">
                                    <img src="../style/images/top_nav4.png" width="73" height="63" border="0"></a>
                            </td>
                            <td align="center">
                                <a href="produit/produit.asp" target="right">
                                    <img src="../style/images/top_nav3.png" width="73" height="63" border="0"></a>
                            </td>
                            <td align="center">
                                <a href="produit/sell.asp" target="right">
                                    <img src="../style/images/top_nav6.png" width="73" height="63" border="0"></a>
                            </td>
                            <td align="center">
                                <a href="count/count_sell.asp" target="right">
                                    <img src="../style/images/top_nav9.png" width="73" height="63" border="0"></a>
                            </td>
                            <td align="center">
                                <a href="count/count_buy.asp" target="right">
                                    <img src="../style/images/top_nav5.png" width="73" height="63" border="0"></a>
                            </td>
                            <td align="center">
                                <a href="system/backup.asp" target="right">
                                    <img src="../style/images/top_nav7.png" width="73" height="63" border="0"></a>
                            </td>
                            <td align="center">
                                <a class="blue" href="javascript:parent.location.href='exit.asp'" onclick="return confirm('确定要退出吗？');">
                                    <img src="../style/images/top_nav10.png" width="73" height="63" border="0"></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th width="43" align="right" valign="middle">
                    <img src="../style/images/user.png" width="16" height="16">
                </th>
                <td width="300" style="text-indent: 20px;">
                    <marquee onmouseover="this.stop()" onmouseout="this.start()" scrollamount="1" scrolldelay="4"
                        width="200" align="left">请各位做好自己的本职工作。
	</marquee>
                </td>
                <th width="50%" align="right">
                    <img src="../style/images/time.png" width="16" height="16">
                </th>
                <td style="text-indent: 20px;">
                    <span class="top_h1">您好,
                        <%=Session["person_name"] == null ? "" : Session["person_name"]%>. 今天是：<%=DateTime.Now.ToString("yyyy年MM月dd日") %>,
                        <%=ZLZJ.Common.Utils.GetNowWeek() %></span>
                </td>
            </tr>
        </table>
    </div>
    <div class="left">
        <h4 class="menu_top">
            <input type="button" class="btn" value="展开菜单" onclick="setMenu($('div.left dl').length);" />
            <input type="button" class="btn" value="收缩菜单" onclick="setMenu(-1);" />
        </h4>
        <dl>
            <dt><i class="title3"></i><em>库存管理</em></dt>
            <dd>
                <a href="StockWarn.aspx" target="f" class="cur">库存预警</a></dd>
            <dd>
                <a href="InWarehouse.aspx" target="f">产品入库</a></dd>
            <dd>
                <a href="javascript:;">产品调配</a></dd>
            <dd>
                <a href="Stock.aspx" target="f">库存查询</a></dd>
        </dl>
        <dl>
            <dt><i class="title4"></i><em>出库管理</em></dt>
            <dd>
                <a href="javascript:;">订单查询</a></dd>
            <dd>
                <a href="javascript:;">产品盘点</a></dd>
        </dl>
        <dl>
            <dt><i class="title8"></i><em>财务管理</em></dt>
            <dd>
                <a href="FCtg.aspx" target="f">账务类别</a></dd>
            <dd>
                <a href="javascript:;">添加账务</a></dd>
            <dd>
                <a href="javascript:;">财务统计</a></dd>
        </dl>
        <dl>
            <dt><i class="title7"></i><em>系统管理</em></dt>
            <dd>
                <a href="Product.aspx" target="f">产品管理</a></dd>
            <dd>
                <a href="PCtg.aspx" target="f">产品分类管理</a></dd>
            <dd>
                <a href="Unit.aspx" target="f">计量单位管理</a></dd>
            <dd>
                <a href="EditPwd.aspx" target="f">修改密码</a></dd>
        </dl>
    </div>
    <div class="right">
        <iframe name="f" src="Product.aspx" scrolling="no" frameborder="0" width="100%" onload="this.height=window.innerHeight-document.getElementById('top').offsetHeight-10;">
        </iframe>
    </div>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            setMenu(0);
            $("div.left dt").click(function () {
                setMenu($($("div.left dt")).index(this));
            });

            $("dl a").click(function () {
                $("dl a").removeClass();
                $(this).addClass("cur");
            });
        });

        function setMenu(index) {
            if (index === $("div.left dl").length) {
                $("div.left dd").show();
            }
            else if (index === -1) {
                $("div.left dd").hide();
            }
            else {
                $("div.left dd").hide();
                $("div.left dt:eq(" + index + ")").nextAll().show();
            }
        }
    </script>
    </form>
</body>
</html>
