using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.BLL.Product;
using ZLZJ.Common;

public partial class inc_in_library_sheet : System.Web.UI.Page
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
    /// 入库单ID
    /// </summary>
    private Guid LibID
    {
        get
        {
            return Guid.Parse(Utils.GetParams("sheetId", Guid.Empty.ToString()));
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
                res = "{\"sheet\":";
                res += Utils.ToJson(bll.GetSheetInfo(LibID));
                res += ",\"detail\":";
                res += Utils.ToJson(bll.GetSheetDetail(LibID));
                res += "}";
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