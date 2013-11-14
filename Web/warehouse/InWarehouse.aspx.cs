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

public partial class warehouse_InWarehouse : System.Web.UI.Page
{
    #region 参数
    /// <summary>
    /// 月初日期
    /// </summary>
    public string MonthFirstDay
    {
        get
        {
            DateTime dt = DateTime.Now;
            return new DateTime(dt.Year, dt.Month, 1).ToString("yyyy-MM-dd");
        }
    }

    /// <summary>
    /// 月末日期
    /// </summary>
    public string MonthLastDay
    {
        get
        {
            DateTime dt = DateTime.Now;
            DateTime start = new DateTime(dt.Year, dt.Month, 1);
            return start.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        }
    }

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
    /// 关键字
    /// </summary>
    private string Key
    {
        get
        {
            return HttpUtility.UrlDecode(Utils.GetParams("key", null));
        }
    }

    /// <summary>
    /// 开始日期
    /// </summary>
    private string StartDate
    {
        get
        {
            return Utils.GetParams("start", null);
        }
    }

    /// <summary>
    /// 结束日期
    /// </summary>
    private string EndDate
    {
        get
        {
            return Utils.GetParams("end", null);
        }
    }

    /// <summary>
    /// 单据编码
    /// </summary>
    private string Code
    {
        get
        {
            return Utils.GetParams("code", null);
        }
    }

    /// <summary>
    /// 单据ID
    /// </summary>
    private Guid InLibraryID
    {
        get
        {
            return Guid.Parse(Utils.GetParams("libID", Guid.Empty.ToString()));
        }
    }

    /// <summary>
    /// 单据状态
    /// </summary>
    private byte Status
    {
        get
        {
            return byte.Parse(Utils.GetParams("status", "0"));
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
        DictionaryBLL bll = new DictionaryBLL();
        ProductBLL pBll = new ProductBLL();
        switch (Tag)
        {
            // 获取产品大类
            case "GET_PRO_MAIN_CTG":
                res = Utils.ToJson<T_Dictionary>(bll.GetDictionarys("CP", Guid.Parse(Session["obj_id"] + "")));
                break;
            // 获取产品小类
            case "GET_PRO_SUB_CTG":
                res = Utils.ToJson<T_Dictionary>(bll.GetDictionarys(ParID, Guid.Parse(Session["obj_id"] + "")));
                break;
            // 获取产品列表
            case "GET_PRODUCT_LIST":
                res = Utils.ToJson(pBll.GetProductList(Guid.Parse(Session["obj_id"] + ""), SubCtg, 0, Key));
                break;
            // 获取入库单列表
            case "GET_IN_WAREHOUSE_LIST":
                res = Utils.ToJson(pBll.GetInWarehouseList(0, StartDate, EndDate, Code));
                break;
            // 产品入库
            case "PRODUCT_INWAREHOUSE":
                res = pBll.ProductInOutWarehouse(GetInLibrary(), GetInLibraryDetailList()) ? "success" : "failed";
                break;
            // 设置单据状态
            case "SET_SHEET_STATUS":
                pBll.SetSheetStatus(InLibraryID, Status, out res);
                break;
            // 获取入库单据详情
            case "GET_SHEET_DETAIL":
                res = Utils.ToJson(pBll.GetSheetDetail(InLibraryID));
                break;
        }
        Response.ClearContent();
        Response.ContentType = "text/plain";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(res);
        Response.End();
    }

    /// <summary>
    /// 获取入库表实体
    /// </summary>
    /// <returns></returns>
    private T_InOutLibrary GetInLibrary()
    {
        ProductBLL bll = new ProductBLL();
        T_InOutLibrary lib = new T_InOutLibrary();
        lib.F_ChangeType = 0;
        lib.F_ObjectID = Guid.Parse(Session["obj_id"] + "");
        lib.F_Code = SheetNumber.NextNumber(Config.KEY_RK, bll.GetPrevSheetNumber("RK"));
        lib.F_IsPay = byte.Parse(Utils.GetParams("isPay", "0"));
        lib.F_Remark = HttpUtility.UrlDecode(Utils.GetParams("remark", null));
        lib.F_Status = 0; // 单据状态默认为使用中
        lib.F_AddUser = Guid.Parse(Session["user_id"] + "");
        lib.F_AddDate = DateTime.Now;
        return lib;
    }

    /// <summary>
    /// 获取入库详情实体列表
    /// </summary>
    /// <returns></returns>
    private List<T_InOutLibraryDetail> GetInLibraryDetailList()
    {
        List<T_InOutLibraryDetail> list = new List<T_InOutLibraryDetail>();
        string[] proList = Utils.GetParams("proList", null).Split(';');
        string[] numList = Utils.GetParams("numList", null).Split(';');
        string[] priceList = Utils.GetParams("priceList", null).Split(';');
        for (int i = 0; i < proList.Length; i++)
        {
            T_InOutLibraryDetail d = new T_InOutLibraryDetail();
            d.F_ID = Guid.NewGuid();
            d.F_ProductID = Guid.Parse(proList[i]);
            d.F_Amount = int.Parse(numList[i]);
            d.F_Price = decimal.Parse(priceList[i]);
            d.F_Total = d.F_Amount * d.F_Price;
            d.F_AddDate = DateTime.Now;
            list.Add(d);
        }
        return list;
    }
    #endregion
}