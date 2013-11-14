using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 登录验证
/// </summary>
public class BaseCls : System.Web.UI.Page
{
    public BaseCls()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    protected override void OnInit(EventArgs e)
    {
        if (Session["user_id"] == null || Session["account"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        base.OnInit(e);
    }
}