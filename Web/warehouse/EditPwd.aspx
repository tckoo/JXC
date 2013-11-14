<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditPwd.aspx.cs" Inherits="warehouse_EditPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改密码</title>
    <link href="../style/css/admin/admin.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" class="tbl" style="margin: 0 auto;
        margin-top: 20px;">
        <tbody>
            <tr>
                <th>
                    帐号:
                </th>
                <th>
                    <label>
                        <%=Session["account"]%>
                    </label>
                </th>
            </tr>
            <tr>
                <th>
                    旧密码:
                </th>
                <th>
                    <input type="password" id="txtOldPwd" runat="server" class="txt" />
                </th>
            </tr>
            <tr>
                <th>
                    新密码:
                </th>
                <th>
                    <input type="password" id="txtNewPwd" runat="server" class="txt" />
                </th>
            </tr>
            <tr>
                <th>
                    确认新密码:
                </th>
                <th>
                    <input type="password" id="txtConfirmPwd" runat="server" class="txt" />
                </th>
            </tr>
            <tr style="height: 60px; line-height: 60px;">
                <td colspan="2" align="right">
                    <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return Validate();"
                        OnClick="btnSave_Click" CssClass="btn" />
                </td>
            </tr>
        </tbody>
    </table>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Validate() {
            var isTrue = true;
            $(":password").each(function () {
                var id = $(this).attr("id");
                if (!$(this).val()) {
                    if (id === "txtOldPwd") {
                        alert("请输入旧密码.");
                    }
                    if (id === "txtNewPwd") {
                        alert("请输入新密码.");
                    }
                    if (id === "txtConfirmPwd") {
                        alert("请输入确认密码.");
                    }
                    isTrue = false;
                    return false;
                }
            });
            if (!isTrue) return false;
            if (isTrue && $("#txtNewPwd").val() !== $("#txtConfirmPwd").val()) {
                alert("新密码和确认密码不一致.");
                return false;
            }
            return true;
        }
    </script>
    </form>
</body>
</html>
