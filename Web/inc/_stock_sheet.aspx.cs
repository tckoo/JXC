using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.BLL.Product;
using ZLZJ.Common;

public partial class inc_stock_sheet : System.Web.UI.Page
{
    #region 参数
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
    /// 产品列表
    /// </summary>
    private string ProductArray
    {
        get
        {
            return Utils.GetParams("product_array", null);
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
        ProductBLL bll = new ProductBLL();
        switch (Tag)
        {
            // 获取单据信息
            case "GET_SHEET":
                string[] array = ProductArray.Split(';');
                List<Guid> list = new List<Guid>();
                foreach (var s in array)
                {
                    list.Add(Guid.Parse(s));
                }
                res = Utils.ToJson(bll.GetProductList(Guid.Parse(Session["obj_id"] + ""), list));
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