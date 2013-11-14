<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="admin_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Web进销存-仓管-登录</title>
    <link href="../style/css/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="login_box">
        <div class="t">
            <h4>
                WEB进销存-仓管版</h4>
        </div>
        <div class="c">
            <table width="297" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr style="height: 20px;">
                    <th>
                        仓库：
                    </th>
                    <td>
                        <select id="sltWarehouse">
                        </select>
                    </td>
                </tr>
                <tr>
                    <th height="35">
                        登录名：
                    </th>
                    <td height="35">
                        <input type="text" class="txt" id="txtAccount" />
                    </td>
                </tr>
                <tr>
                    <th height="35">
                        密 码：
                    </th>
                    <td height="35">
                        <input type="password" class="txt" id="txtPwd" />
                    </td>
                </tr>
                <tr>
                    <th height="35">
                        验证码：
                    </th>
                    <td height="35">
                        <input type="text" maxlength="6" size="6" style="padding-left: 5px;" id="txtCode" />
                        <img style="cursor: pointer;" src='../inc/code.ashx' onclick="this.src=this.src+'?r='+Math.random();"
                            align="absmiddle" alt="" title="换一个" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <th height="35">
                        &nbsp;
                    </th>
                    <td height="35">
                        <input type="button" class="login_btn" id="btnLogin" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="b">
        </div>
    </div>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.ajax({
                url: "Login.aspx",
                async: false,
                type: "POST",
                dataType: "json",
                data: { tag: "GET_WAREHOUSE" },
                success: function (json) {
                    var html = [];
                    if (json && json.length) {
                        for (var i = 0, len = json.length; i < len; i++) {
                            html.push("<option value=\"" + json[i]["ObjID"] + "\">" + json[i]["ObjName"] + "</option>");
                        }
                    }
                    $("#sltWarehouse").html(html.join(""));
                }
            });

            $("#btnLogin").click(function () {
                var flag = true,
                    e_id = "",
                    e_info = "",
                    that = this;
                if (!$("#sltWarehouse").val()) {
                    alert("请选择仓库.");
                    return false;
                }
                $(":text,:password").each(function () {
                    if (!$(this).val()) {
                        e_id = $(this).attr("id");
                        flag = false;
                        return false;
                    }
                });
                if (!flag) {
                    if (e_id === "txtAccount") {
                        e_info = "请输入用户名";
                    }
                    else if (e_id === "txtPwd") {
                        e_info = "请输入登录密码";
                    }
                    else {
                        e_info = "请输入验证码";
                    }
                    alert(e_info);
                    return false;
                }
                $.ajax({
                    url: "Login.aspx",
                    data: { tag: "USER_LOGIN", objID: $("#sltWarehouse").val(), account: $("#txtAccount").val(), pwd: $("#txtPwd").val(), code: $("#txtCode").val() },
                    beforeSend: function () {
                        $(that).attr("disabled", true);
                    },
                    success: function (res) {
                        if (res && res === "success") {
                            window.location = "Index.aspx";
                        }
                        else if (res === "disabled") {
                            alert("对不起, 此帐号已禁用, 请联系管理员.");
                        }
                        else if (res === "c_err") {
                            alert("验证码错误.");
                        }
                        else {
                            alert("登录失败, 用户名或密码错误.");
                        }
                    },
                    complete: function () {
                        $(that).attr("disabled", false);
                    }
                });
            });
        });
    </script>
    </form>
</body>
</html>
