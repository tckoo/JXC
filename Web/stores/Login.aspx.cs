using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.Common;
using ZLZJ.BLL.Users;
using ZLZJ.Entitys;
using ZLZJ.BLL.Dictionary;
using ZLZJ.BLL.Objects;

public partial class stores_Login : System.Web.UI.Page
{

    private string Tag
    {
        get { return Request.Params["tag"] + ""; }
    }
    /// <summary>
    /// 门店ID
    /// </summary>
    private string ObjId
    {
        get { return Request.Params["objId"] + ""; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Tag))
        {
            ProcessRequest();
            return;
        }
    }


    private readonly ObjectsBLL bll = new ObjectsBLL();
    private readonly DictionaryBLL dBll = new DictionaryBLL();


    /// <summary>
    /// 处理异步请求
    /// </summary>
    private void ProcessRequest()
    {
        string res = string.Empty;
        switch (Tag)
        {
            // 获取仓库列表
            case "GET_WAREHOUSE":
                res = Utils.ToJson<ObjectModel>(bll.GetObjects(1, 0, null));
                break;
            case "login":
                string account = Request.QueryString["account"];
                string pwd = Utils.EncryptByDESbase64(Request.QueryString["pwd"]);
                string code = Request.QueryString["code"];
                res = string.Empty;
                if (code != Session["code"].ToString())
                {
                    res = "c_err";
                }
                else
                {
                    UsersBLL userBll = new UsersBLL();
                    Guid stroeId = Guid.Parse(ObjId);
                    userBll.UserLogin(1, stroeId, account, pwd, out res);
                }
                break;

        }
        Response.ClearContent();
        Response.ContentType = "text/plain";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(res);
        Response.End();
    }


}