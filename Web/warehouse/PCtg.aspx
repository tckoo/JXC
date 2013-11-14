<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PCtg.aspx.cs" Inherits="warehouse_PCtg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品分类管理</title>
    <link href="../style/css/admin/admin.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <fieldset style="padding: 10px 5px;">
        <legend style="font-size: 14px;">快速添加产品分类</legend>
        <table cellpadding="0" cellspacing="0" border="0" class="searchbar">
            <tr>
                <th colspan="2" style="text-align: left;">
                    请选择类型:
                </th>
                <td colspan="7">
                    <asp:RadioButtonList ID="rbtType" runat="server" RepeatDirection="Horizontal" Font-Size="Smaller">
                        <asp:ListItem Value="0" Selected="True">大类:</asp:ListItem>
                        <asp:ListItem Value="1">小类</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>
                    所属父级:
                </th>
                <td>
                    <asp:DropDownList ID="ddlCtg" runat="server" Enabled="false">
                    </asp:DropDownList>
                </td>
                <th>
                    分类名称:
                </th>
                <td>
                    <input type="text" id="txtName" runat="server" class="txt" />
                </td>
                <th>
                    编码:
                </th>
                <td>
                    <input type="text" id="txtCode" runat="server" class="txt" />
                </td>
                <th>
                    状态:
                </th>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Value="0">启用</asp:ListItem>
                        <asp:ListItem Value="1">停用</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="保存" class="btn" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
    </fieldset>
    <div class="hr">
    </div>
    <fieldset style="padding: 10px 5px; display: none;" id="fEdit" runat="server">
        <legend style="font-size: 14px;">编辑产品分类</legend>
        <table cellpadding="0" cellspacing="0" border="0" class="searchbar">
            <tr>
                <th colspan="2" style="text-align: left;">
                    请选择类型:
                </th>
                <td colspan="7">
                    <asp:RadioButtonList ID="rbtnNType" runat="server" RepeatDirection="Horizontal" Font-Size="Smaller">
                        <asp:ListItem Value="0" Selected="True">大类:</asp:ListItem>
                        <asp:ListItem Value="1">小类</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>
                    所属父级:
                </th>
                <td>
                    <asp:DropDownList ID="ddlNCtg" runat="server" Enabled="false">
                    </asp:DropDownList>
                </td>
                <th>
                    分类名称:
                </th>
                <td>
                    <input type="text" id="txtNName" runat="server" class="txt" />
                </td>
                <th>
                    编码:
                </th>
                <td>
                    <input type="text" id="txtNCode" runat="server" class="txt" />
                </td>
                <th>
                    状态:
                </th>
                <td>
                    <asp:DropDownList ID="ddlNStatus" runat="server">
                        <asp:ListItem Value="0">启用</asp:ListItem>
                        <asp:ListItem Value="1">停用</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnNSave" runat="server" Text="保存" class="btn" OnClick="btnNSave_Click" />
                    <input type="button" value="取消" class="btn" onclick="$('#fEdit').hide();" />
                </td>
            </tr>
        </table>
    </fieldset>
    <table cellpadding="0" cellspacing="0" border="0" class="tbl_data">
        <asp:Repeater ID="rptCtg" runat="server" OnItemCommand="rptCtg_ItemCommand">
            <HeaderTemplate>
                <thead>
                    <tr>
                        <th>
                            分类名称
                        </th>
                        <th>
                            所属父级
                        </th>
                        <th>
                            分类编码
                        </th>
                        <th>
                            状态
                        </th>
                        <th>
                            添加日期
                        </th>
                        <th>
                            操作
                        </th>
                    </tr>
                </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tbody>
                    <tr>
                        <td>
                            <%#Eval("F_DicName")%>
                        </td>
                        <td>
                            <%#Eval("parName")%>
                        </td>
                        <td>
                            <%#Eval("F_DicCode") %>
                        </td>
                        <td>
                            <%#(Eval("F_Status")+"")=="0"?"启用":"停用" %>
                        </td>
                        <td>
                            <%#Convert.ToDateTime(Eval("F_AddDate").ToString()).ToString("yyyy-MM-dd") %>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="edit" CommandArgument='<%#Eval("F_DictionaryID") %>'>编辑</asp:LinkButton><asp:LinkButton
                                ID="lbtnUse" runat="server" CommandName="use" CommandArgument='<%#Eval("F_DictionaryID") %>'>启用</asp:LinkButton><asp:LinkButton
                                    ID="lbtnUnUse" runat="server" CommandName="unuse" CommandArgument='<%#Eval("F_DictionaryID") %>'>停用</asp:LinkButton>
                        </th>
                    </tr>
                </tbody>
            </ItemTemplate>
        </asp:Repeater>
        <tfoot>
            <tr style="height: 60px; line-height: 60px;">
                <td colspan="6">
                    <webdiyer:AspNetPager CssClass="pages" CurrentPageButtonClass="cpb" ID="aspCtg" runat="server"
                        PageSize="8" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页"
                        PrevPageText="上一页" AlwaysShow="true" OnPageChanged="aspCtg_PageChanged">
                    </webdiyer:AspNetPager>
                </td>
            </tr>
        </tfoot>
    </table>
    <script src="../js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(":radio[name='rbtType']").click(function () {
                var val = $(this).val();
                $("#ddlCtg").attr("disabled", val == "0");
            });

            $(":radio[name='rbtnNType']").click(function () {
                var val = $(this).val();
                $("#ddlNCtg").attr("disabled", val == "0");
            });
        });
    </script>
    </form>
</body>
</html>
