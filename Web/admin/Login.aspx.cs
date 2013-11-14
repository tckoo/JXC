using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.BLL.Users;
using ZLZJ.Common;

public partial class admin_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["tag"]))
        {
            string account = Request.QueryString["account"];
            string pwd = Utils.EncryptByDESbase64(Request.QueryString["pwd"]);
            string code = Request.QueryString["code"];
            string res = string.Empty;
            if (code != Session["code"].ToString())
            {
                res = "c_err";
            }
            else
            {
                UsersBLL bll = new UsersBLL();
                bll.UserLogin(0, null, account, pwd, out res);
            }
            Response.ClearContent();
            Response.ContentType = "text/plain";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(res);
            Response.End();
        }
    }
}