/* 公共方法类
*  作者：zh
*  日期：2011-06-15
*  参数obj为对象，形如:var obj={site:"http://www.baidu.com/",name:"张三"}
*  调用方法为:
*  var obj={site:"http://www.baidu.com/",name:"张三"}
*  var utils = new Utils(obj);
*  var tmp = utils.GetParams();
*  注:其中，参数obj对象中的对象名必须与公共方法中提到的参数名一致
*     --如调用GetParams方法，则应为: var obj = {site:""}
**/

function Utils(obj) {
    this.dlg = null;
    this.mdlg = null;
    $.extend(this, obj);  //将传入的参数扩展到this对象
}

//Utils类原型操作,添加公共方法
Utils.prototype = {

    /* 根据url返回参数列表
    *   参数名为site,实例化Utils类的时候扩展到this中
    *   调用方法为:
    *   var obj = {site:"http://www.baidu.com/"}
    *   var utils = new Utils(obj);
    *   var p = utils.GetParams();
    */

    GetParams: function () {
        if (!this.site) return null;
        var obj = {};
        var site = this.site;
        //过滤掉url末尾的#号
        var index = site.indexOf("#");
        if (index != -1)
            site = site.substr(0, index);

        if (site.indexOf("?") != -1) {
            var param = site.split("?")[1];
            var temp = param.split("&");
            for (var i = 0, len = temp.length; i < len; i++) {
                var tmp = temp[i].split("=");
                obj[tmp[0]] = tmp[1];
            }
        }
        return obj;
    },

    /* 正则表达式验证
    *  参数名为正则表达式express和需要验证的字符串value
    *  调用方法为:
    *  var obj = {express:"^(?:https?|ftp)\:\/\/(?:\w+\.?)*\.\w+(?:\/?\w*)*$",value:"http://www.baidu.com/"},
    *     -- 其中，正则表达式既可为字符串形式，也可为正则对象.
    *  返回结果为true或者false
    */
    CheckRegex: function () {
        if (!this.value || !this.express) return false;
        var reg;
        if (typeof (this.express) == "string")
            reg = new RegExp(this.express);
        else {
            reg = this.express;
        }
        if (reg.test(this.value))
            return true;
        return false;
    },

    /* 处理特殊字符
    *  参数html:需要处理的字符串
    *  调用方法:
    *  var html="<script></script>";
    *  var utils = new Utils();    utils.FilterChar(html);
    *  返回处理后的字符串
    */
    EncodeStr: function (html) {
        encodedHtml = escape(html);
        encodedHtml = encodedHtml.replace(/\//g, "%2F");
        encodedHtml = encodedHtml.replace(/\?/g, "%3F");
        encodedHtml = encodedHtml.replace(/=/g, "%3D");
        encodedHtml = encodedHtml.replace(/&/g, "%26");
        encodedHtml = encodedHtml.replace(/@/g, "%40");
        return encodedHtml;
    },

    /* 还原解析后的字符串
    *   参数 html:需要解析的字符串
    *   调用方法
    */
    UnEncodeStr: function (html) {
        return unescape(html);
    },

    /* 阻止事件冒泡
    *  参数:event:触发事件的event对象
    */
    StopEventPropagation: function (e) {
        if (e && e.stopPropagation)
            e.stopPropagation();
        else
            window.event.cancelBubble = true;
    },

    /*
    * 将日期型字符串转换为日期类型
    * 参数: str,日期型字符串
    */
    StringToDate: function (str) {
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
        return null;
    },

    /*
    * 格式化日期
    * 参数: date,需要格式化的日期; fmt,格式字符串
    */
    FormatDate: function (date, fmt) {
        var d = this.StringToDate(date);
        var o = {
            "M+": d.getMonth() + 1, //月份        
            "d+": d.getDate(), //日        
            "h+": d.getHours() % 12 == 0 ? 12 : d.getHours() % 12, //小时        
            "H+": d.getHours(), //小时        
            "m+": d.getMinutes(), //分        
            "s+": d.getSeconds(), //秒        
            "q+": Math.floor((d.getMonth() + 3) / 3), //季度        
            "S": d.getMilliseconds() //毫秒        
        };
        var week = {
            "0": "\u65e5",
            "1": "\u4e00",
            "2": "\u4e8c",
            "3": "\u4e09",
            "4": "\u56db",
            "5": "\u4e94",
            "6": "\u516d"
        };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        if (/(E+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "\u661f\u671f" : "\u5468") : "") + week[d.getDay() + ""]);
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    },

    /*
    * 与当前日期间隔的天数
    * 参数: date,日期
    */
    DaysElapsed: function (date) {
        var d1;
        if ((typeof date).toLowerCase() == "string")
            d1 = this.StringToDate(date);
        else if ((typeof date).toLowerCase() == "object")
            d1 == date;
        var d = new Date();
        var difference = Date.UTC(d1.getYear(), d1.getMonth(), d1.getDate(), 0, 0, 0) - Date.UTC(d.getYear(), d.getMonth(), d.getDate(), 0, 0, 0);
        return difference / 1000 / 60 / 60 / 24;
    },

    /*
    * 生成Jquery弹出层
    */
    MessageBox: function (id, title, html, isConfirm, callback) {
        this.dlg && this.dlg.dialog("destroy");

        var opts = {
            autoOpen: true,
            modal: true,
            title: title || "提示",
            resizable: false,
            width: "auto",
            buttons: {
                "确定": function () {
                    callback && callback();
                }
            }
        },
        o = $("#" + id);
        if (isConfirm) {
            opts.buttons["取消"] = function () {
                this.dlg.dialog("destroy");
            }
        }
        html && o.html(html);
        this.dlg = o.dialog(opts);
        delete o;
    },
    mBox: function (id, title, callback) {
        this.mdlg && this.mdlg.dialog("destroy");

        var opts = {
            autoOpen: true,
            modal: true,
            title: title || "提示",
            resizable: false,
            width: "auto"
        },
        o = $("#" + id);
        this.mdlg = o.dialog(opts);
        delete o;
    }
}



