using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.Common;
using ZLZJ.BLL.Dictionary;
using ZLZJ.BLL.Product;
using ZLZJ.Entitys;

public partial class warehouse_StockWarn : System.Web.UI.Page
{
    #region URL参数
    /// <summary>
    /// 异步请求功能参数
    /// </summary>
    private string Tag
    {
        get
        {
            return Utils.GetParams("tag", null);
        }
    }

    /// <summary>
    /// 产品ID
    /// </summary>
    private Guid ProductID
    {
        get
        {
            return Guid.Parse(Utils.GetParams("productID", Guid.Empty.ToString()));
        }
    }

    /// <summary>
    /// 产品小类
    /// </summary>
    private Guid? SubCtg
    {
        get
        {
            string o = Utils.GetParams("subCtg", null);
            if (string.IsNullOrEmpty(o)) return null;
            return Guid.Parse(o);
        }
    }

    /// <summary>
    /// 父级ID
    /// </summary>
    private Guid ParID
    {
        get
        {
            return Guid.Parse(Utils.GetParams("parID", Guid.Empty.ToString()));
        }
    }

    /// <summary>
    /// 产品名称
    /// </summary>
    private string PName
    {
        get
        {
            return HttpUtility.UrlDecode(Utils.GetParams("pName", null));
        }
    }

    /// <summary>
    /// 关键字
    /// </summary>
    private string Key
    {
        get
        {
            return HttpUtility.UrlDecode(Utils.GetParams("key", null));
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Tag))
        {
            ProcessRequest();
            return;
        }
    }

    #region 异步方法
    /// <summary>
    /// 处理异步请求
    /// </summary>
    private void ProcessRequest()
    {
        string res = string.Empty;
        DictionaryBLL pbll = new DictionaryBLL();
        ProductBLL bll = new ProductBLL();
        switch (Tag)
        {
            // 获取产品大类
            case "GET_PRO_MAIN_CTG":
                res = Utils.ToJson<T_Dictionary>(pbll.GetDictionarys("CP", Guid.Parse(Session["obj_id"] + "")));
                break;
            // 获取产品小类
            case "GET_PRO_SUB_CTG":
                res = Utils.ToJson<T_Dictionary>(pbll.GetDictionarys(ParID, Guid.Parse(Session["obj_id"] + "")));
                break;
            // 获取产品预警列表
            case "GET_WARN_PRODUCT_LIST":
                res = Utils.ToJson(bll.GetWarnProduct(Guid.Parse(Session["obj_id"] + ""), SubCtg, Key));
                break;
        }
        Response.ClearContent();
        Response.ContentType = "text/plain";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(res);
        Response.End();
    }
    #endregion
}