using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.BLL.Dictionary;
using ZLZJ.BLL.Product;
using ZLZJ.Common;
using ZLZJ.Entitys;
using ZLZJ.BLL.Objects;

public partial class stores_Stock : System.Web.UI.Page
{
    private string Tag
    {
        get { return Request.Params["tag"] + ""; }
    }

    private string ParID
    {
        get { return Request.Params["parID"] + ""; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Tag))
        {
            ProcessRequest();
            return;
        }
    }

    private Guid? SubCtg
    {
        get
        {
            string o = Utils.GetParams("subCtg", null);
            if (string.IsNullOrEmpty(o)) return null;
            return Guid.Parse(o);
        }
    }

    private string Key
    {
        get { return Request.Params["key"] + ""; }
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
                T_Object wareHouse = (new ObjectsBLL()).GetObject(Guid.Parse(Session["obj_id"] + ""));
                if (wareHouse != null)
                {
                    HttpContext.Current.Session["wareHouseId"] = wareHouse.F_WarehouseID;
                    res = Utils.ToJson<T_Dictionary>(pbll.GetDictionarys("CP", wareHouse.F_WarehouseID));
                }

                break;
            // 获取产品小类
            case "GET_PRO_SUB_CTG":
                res = Utils.ToJson<T_Dictionary>(pbll.GetDictionarys(Guid.Parse(ParID)));
                break;
            // 获取产品列表
            case "GET_PRODUCT_LIST":
                res = Utils.ToJson(bll.GetProductList(Guid.Parse(Session["wareHouseId"] + ""), SubCtg, Key));
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