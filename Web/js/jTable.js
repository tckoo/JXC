/********************************************
* name: Tbale-Plugin
* Author: zh
* Date: 2012-03-31
* des: 表格插件
* ps: var tbl = new Table(
{
el: "tbl",
datas: json.datas,
PageSize: 15,
tblHeader:
{
F_LoginID: "登录ID",
F_Name: "昵称",
F_Email: "EMail",
F_RegTime: "注册日期"
},
IsOperate: true,
operateData:
{
filed: "F_ID", obj: { edit: "编辑", del: "删除" }
},
width: 1000
});
tbl.Init();
********************************************/
function Table(option) {
    this.el = null; // 容器ID, 也可为object。
    this.datas = []; // 数据源
    this.ds = []; // 每页显示的数据源(不用传入)
    this.tblHeader = {}; // 表头, tblHeader:{F_LoginID: {text:"登录ID",sortType:"string",format:function(val){}},F_Name: {text:"昵称",sortType:"string",F_RegTime: {text:"注册日期",sortType:"date"}}
    // 对象的key表示数据源中的字段, 对象的value为一个object(text:表头文本, sortType:排序的类型, 包括string,int,float,date,bool五种, 若该列不需要排序, 可不需要sortType)
    this.html = []; // 保存表格html代码
    this.sortField = null;
    this.operateData = {}; // 操作列数据, operateData:{filed: "F_ID", obj: { edit: "编辑", del: "删除" }},field为主键ID,obj中的数据作为a元素的tag属性

    this.isshow = false; // 是否显示合计列
    this.sum = null; // 需要显示合计列的时候，合计列的字段

    this.PageSize = 5; // 每页显示的行数
    this.RecordCount = 0; // 总记录数
    this.CurrentPageIndex = 0; // 当前页索引
    this.PageCount = 0; // 总页数

    this.width = 800; // 表格的宽度

    this.IsOperate = false; // 是否添加操作列

    this.IsSelect = false; // 是否添加选择列

    this.onbindend = null; // 数据绑定后触发

    $.extend(this, option);
    this.el = $("#" + this.el) || $(this.el);
}

Table.prototype = {
    /*
    * 插件加载
    */
    Init: function () {
        this.GetPageData();
        this.CreateTableData();
    },
    /*
    * 获取分页数据
    */
    GetPageData: function () {
        this.RecordCount = this.datas.length;
        if (this.datas.length <= this.PageSize) {
            this.PageCount = 1;
            this.ds = this.datas;
            return;
        }
        this.PageCount = this.datas.length % this.PageSize != 0 ? (parseInt(this.datas.length / this.PageSize) + 1) : (this.datas.length / this.PageSize);
        this.ds = this.datas.slice(this.CurrentPageIndex * this.PageSize, this.PageSize * (1 + this.CurrentPageIndex));
    },
    /*
    * 构建表格数据
    */
    CreateTableData: function () {
        this.html = [];
        this.html.push("<table cellspacing='0' cellspadding='0' width='" + this.width + "'>");
        // 生成表头
        var count = 0; // 保存表头的数量
        if (this.tblHeader) {
            this.html.push("<thead style='line-height:35px; background-color:#ebebeb; font-weight:bold;'><tr align='center'>");
            if (this.IsSelect) {
                this.html.push("<td width=\"60\"><input type=\"checkbox\" id=\"tbl_all_select\"><label for=\"tbl_all_select\">全选</label></td>");
                count++;
            }
            for (var key in this.tblHeader) {
                if (this.tblHeader[key]["sortType"])
                    this.html.push("<td sortType='" + this.tblHeader[key]["sortType"] + "' field='" + key + "'>" + this.tblHeader[key]["text"] + "</td>");
                else
                    this.html.push("<td>" + this.tblHeader[key]["text"] + "</td>");
                count++;
            }
            if (this.IsOperate) {
                this.html.push("<td>操作</td>");
                count++;
            }
            this.html.push("</tr></thead>");
        }

        var self = this;
        // 生成表格数据
        this.html.push("<tbody>");
        if (!this.ds || !this.ds.length)
            this.html.push("<tr align='left' style='line-height:30px;'><td colspan='" + count + "' align='center'>暂无数据</td></tr></tbody>");
        else {
            for (var i = 0, len = this.ds.length; i < len; i++) {
                var obj = this.ds[i];
                this.html.push("<tr align='center' style='line-height:30px;'>");
                if (this.IsSelect) {
                    this.html.push("<td><input type=\"checkbox\" value=\"" + this.ds[i][self.operateData.filed] + "\" /></td>");
                }
                for (var key in this.tblHeader) {
                    if (this.tblHeader[key]["format"])
                        this.html.push("<td>" + this.tblHeader[key]["format"](obj[key], obj) + "</td>");
                    else
                        this.html.push("<td>" + obj[key] + "</td>");
                }

                if (this.IsOperate) {
                    this.html.push("<td>");
                    for (var txt in this.operateData.obj)
                        this.html.push("<a href='javascript:;' tag='" + txt + "' " + self.operateData.filed + "='" + obj[self.operateData.filed] + "' style='margin-left:5px; color:#000;'>" + self.operateData.obj[txt] + "</a>");
                    this.html.push("</td>");
                }

                this.html.push("</tr>");
            }
            // 合计列
            if (this.isshow) {
                var sum = 0;
                this.html.push("<tr>");
                this.html.push("<td>合计:</td>");
                this.html.push("<td cols=\"" + (count - 1) + "\">");
                for (var j = 0, l = this.ds.length; j < l; j++) {
                    sum += parseFloat(this.ds[j][this.sum]);
                }
                this.html.push("</td>");
                this.html.push("</tr>");
            }
            this.html.push("</tbody>");

            // 生成分页条
            this.html.push("<tfoot><tr align='center' valign='middle' style='line-height:23px;height:60px;'>");
            this.html.push("<td colspan='" + count + "'>");
            this.html.push("<lable>当前" + (this.CurrentPageIndex + 1) + "/" + this.PageCount + "页</label>");
            this.html.push("<a href='javascript:;' tag='lHome'>首页</a>");
            this.html.push("<a href='javascript:;' tag='lPrev'>上一页</a>");
            this.html.push("<a href='javascript:;' tag='lNext'>下一页</a>");
            this.html.push("<a href='javascript:;' tag='lLast'>末页</a></td>");
            this.html.push("</tr></tfoot>");
        }
        this.html.push("</table>");
        this.el.html(this.html.join("")).css({ font: "normal 13px Helvetica Neue,Lucida Grande,Segoe UI,Arial,Helvetica,Verdana,sans-serif" })
        .find("tfoot a").css({ display: "inline-block", width: 50, height: 20, textDecoration: "none", border: "solid 1px #ccc", marginLeft: 5, color: "#333" });
        this.el.find("tbody tr:odd").css("background-color", "#f1f1f1"); // 设置奇数行颜色

        if (this.onbindend) this.onbindend();

        // 设置鼠标移上行效果
        var bgColor = null;
        this.el.find("tbody tr").hover(function () {
            bgColor = $(this).css("background-color");
            $(this).css({ backgroundColor: "#d5e7f3" });
        }, function () {
            $(this).css({ backgroundColor: bgColor });
        });

        // 设置鼠标移上分页按钮的效果
        this.el.find("tfoot a").hover(function () {
            $(this).css("background-color", "#ccccff");
        }, function () {
            $(this).css("background-color", "#fff");
        });

        // 添加分页条事件
        this.el.find("a[tag='lHome']").unbind("click").bind("click", function () {
            self.GetHomePage();
        });
        this.el.find("a[tag='lPrev']").unbind("click").bind("click", function () {
            self.GetPrevPage();
        });
        this.el.find("a[tag='lNext']").unbind("click").bind("click", function () {
            self.GetNextPage();
        });
        this.el.find("a[tag='lLast']").unbind("click").bind("click", function () {
            self.GetLastPage();
        });

        // 表头排序事件
        this.el.find("thead td[sortType]").click(function () {
            self.sort($(this).attr("field"), $(this).attr("sortType"));
        }).css("cursor", "pointer");

        // 添加全选事件
        this.el.find("#tbl_all_select").unbind("click").bind("click", function () {
            self.el.find("tbody :checkbox").attr("checked", this.checked);
        });
    },
    /*
    * 首页
    */
    GetHomePage: function () {
        this.CurrentPageIndex = 0;
        this.Init();
    },
    /*
    * 下一页
    */
    GetNextPage: function () {
        if (this.CurrentPageIndex == this.PageCount - 1) return;
        ++this.CurrentPageIndex;
        this.Init();
    },
    /*
    * 上一页
    */
    GetPrevPage: function () {
        if (this.CurrentPageIndex == 0) return;
        --this.CurrentPageIndex;
        this.Init();
    },
    /*
    * 末页
    */
    GetLastPage: function () {
        this.CurrentPageIndex = this.PageCount - 1;
        this.Init();
    },
    /*
    * 表格数据排序(firld: 需要排序的字段, sortType: 排序的类型(int, float, date, bool))
    */
    sort: function (field, sortType) {
        this.datas = this.sortField == field ? this.datas.reverse() : this.datas.sort(this._compare(field, sortType));
        this.Init();
        this.sortField = field;
    },
    // 以下为表格排序方法
    /*
    * 按排序字段的类型生成值
    */
    _getValue: function (sValue, sortType) {
        switch (sortType) {
            case "int":
                return parseInt(sValue, 10) || 0;
            case "float":
                return parseFloat(sValue) || 0;
            case "date":
                return this._stringToDate(sValue);
            case "bool":
                return sValue === true || String(sValue).toLowerCase() == "true" ? 1 : 0;
            default:
                return sValue.toString() || "";
        }
    },
    /*
    * 排序方法(_field为排序的字段, sortType为排序字段的类型)
    */
    _compare: function (_field, sortType) {
        var self = this;
        return function (_obj1, _obj2) {
            var v1 = self._getValue(_obj1[_field], sortType);
            var v2 = self._getValue(_obj2[_field], sortType);
            if (v1 < v2) return -1;
            else if (v1 > v2) return 1;
            else return 0;
        }
    },
    /*
    * 将日期型字符串转换为日期类型
    * 参数: str,日期型字符串
    */
    _stringToDate: function (str) {
        if (typeof str == 'string') {
            var results = str.match(/^ *(\d{4})-(\d{1,2})-(\d{1,2}) *$/);
            if (results && results.length > 3)
                return new Date(parseInt(results[1]), parseInt(results[2]) - 1, parseInt(results[3]));
            results = str.match(/^ *(\d{4})-(\d{1,2})-(\d{1,2}) +(\d{1,2}):(\d{1,2}):(\d{1,2}) *$/);
            if (results && results.length > 6)
                return new Date(parseInt(results[1]), parseInt(results[2]) - 1, parseInt(results[3]), parseInt(results[4]), parseInt(results[5]), parseInt(results[6]));
            results = str.match(/^ *(\d{4})-(\d{1,2})-(\d{1,2}) +(\d{1,2}):(\d{1,2}):(\d{1,2})\.(\d{1,9}) *$/);
            if (results && results.length > 7)
                return new Date(parseInt(results[1]), parseInt(results[2]) - 1, parseInt(results[3]), parseInt(results[4]), parseInt(results[5]), parseInt(results[6]), parseInt(results[7]));
        }
        return new Date().getDate();
    },
    /*
    * 获取选择的行
    */
    getSelected: function () {
        var array = [];
        this.el.find("tbody :checkbox:checked").each(function () {
            array.push($(this).val());
        });
        return array;
    }
};