<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="stores_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Web进销存-管理员-登录</title>
    <link href="../style/css/login.css" rel="stylesheet" type="text/css" />
    <style>
        .txt_right
        {
            text-align: right;
            padding-right: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="login_box">
        <div class="t">
            <h4>
                WEB进销存-门店版</h4>
        </div>
        <div class="c">
            <table width="297" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr style="height: 20px;">
                </tr>
                <tr>
                    <th height="35" class="txt_right">
                        门 店：
                    </th>
                    <td height="35">
                        <select id="sltStroe">
                        </select>
                    </td>
                </tr>
                <tr>
                    <th height="35" class="txt_right">
                        登录名：
                    </th>
                    <td height="35">
                        <input type="text" class="txt" id="txtAccount" />
                    </td>
                </tr>
                <tr>
                    <th height="35" class="txt_right">
                        密 码：
                    </th>
                    <td height="35">
                        <input type="password" class="txt" id="txtPwd" />
                    </td>
                </tr>
                <tr>
                    <th height="35" class="txt_right">
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

        $(document).ready(function () {
            $("body").keydown(function (e) {
                var curKey = e.which;
                if (curKey == 13) {
                    $("#btnLogin").click();
                    return false;
                }
            });

            /*
            * 绑定界面上门店下拉框
            */
            $.getJSON("Login.aspx", { tag: "GET_WAREHOUSE" }, function (result) {
                if (result && result.length) {
                    var html = "<option value=''>---请选择分店---</option>";
                    for (var i = 0; i < result.length; i++) {
                        var stroe = result[i];
                        html += "<option value='" + stroe.ObjID + "'>" + stroe.ObjName + "</option>";
                    }
                    $("#sltStroe").html(html);
                }
            });
        });

        $(function () {
            $("#btnLogin").click(function () {
                var flag = true,
                    e_stroeId = $("#sltStroe").val(),
                    e_id = "",
                    e_info = "",
                    that = this;
                $(":text,:password").each(function () {
                    if (!$(this).val()) {
                        e_id = $(this).attr("id");
                        flag = false;
                        return false;
                    }
                });
                if (!flag) {
                    if (e_stroeId === "") {
                        e_info = "请选择您要登录的门店";
                    }else if (e_id === "txtAccount") {
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
                    data: { tag: "login", objId: e_stroeId, account: $("#txtAccount").val(), pwd: $("#txtPwd").val(), code: $("#txtCode").val() },
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
