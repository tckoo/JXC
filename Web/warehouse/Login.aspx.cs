using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.BLL.Users;
using ZLZJ.Common;
using ZLZJ.BLL.Objects;
using ZLZJ.Entitys;

public partial class admin_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Utils.GetParams("tag", null)))
        {
            string res = string.Empty;
            if (Utils.GetParams("tag", null) == "USER_LOGIN")
            {
                string account = Request.Params["account"];
                string pwd = Utils.EncryptByDESbase64(Request.Params["pwd"]);
                string code = Request.Params["code"];
                if (code != Session["code"].ToString())
                {
                    res = "c_err";
                }
                else
                {
                    UsersBLL bll = new UsersBLL();
                    bll.UserLogin(2, Guid.Parse(Utils.GetParams("objID", Guid.Empty.ToString())), account, pwd, out res);
                }
            }
            else if (Utils.GetParams("tag", null) == "GET_WAREHOUSE")
            {
                ObjectsBLL bll = new ObjectsBLL();
                res = Utils.ToJson<ObjectModel>(bll.GetObjects(2, 0, null));
            }
            Response.ClearContent();
            Response.ContentType = "text/plain";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(res);
            Response.End();
        }
    }
}