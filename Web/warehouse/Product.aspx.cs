using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.Common;
using ZLZJ.Entitys;
using ZLZJ.BLL.Dictionary;
using ZLZJ.BLL.Product;

public partial class warehouse_Product : System.Web.UI.Page
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
    /// 产品大类
    /// </summary>
    private Guid MainCtg
    {
        get
        {
            return Guid.Parse(Utils.GetParams("mainCtg", Guid.Empty.ToString()));
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
    /// 编码
    /// </summary>
    private string PCode
    {
        get
        {
            return Utils.GetParams("pCode", null);
        }
    }

    /// <summary>
    /// 单位
    /// </summary>
    private Guid Unit
    {
        get
        {
            return Guid.Parse(Utils.GetParams("unit", Guid.Empty.ToString()));
        }
    }

    /// <summary>
    /// 产品尺寸
    /// </summary>
    private string Size
    {
        get
        {
            return Utils.GetParams("size", null);
        }
    }

    /// <summary>
    /// 产品净重
    /// </summary>
    private float Weigtht
    {
        get
        {
            return float.Parse(Utils.GetParams("weight", "0"));
        }
    }

    /// <summary>
    /// 装箱数量
    /// </summary>
    private int PNum
    {
        get
        {
            return int.Parse(Utils.GetParams("pNum", "0"));
        }
    }

    /// <summary>
    /// 装箱数量
    /// </summary>
    private int Alarm
    {
        get
        {
            return int.Parse(Utils.GetParams("alarm", "0"));
        }
    }

    /// <summary>
    /// 提成类型
    /// </summary>
    private byte BonusType
    {
        get
        {
            return byte.Parse(Utils.GetParams("bonusType", "0"));
        }
    }

    /// <summary>
    /// 提成额度
    /// </summary>
    private string Bonuss
    {
        get
        {
            return Utils.GetParams("bonus", null);
        }
    }

    /// <summary>
    /// 产品图片
    /// </summary>
    private string ProImage
    {
        get
        {
            return Utils.GetParams("filePath", "");
        }
    }

    /// <summary>
    /// 备注
    /// </summary>
    private string Remark
    {
        get
        {
            return Utils.GetParams("remark", null);
        }
    }

    /// <summary>
    /// 状态
    /// </summary>
    private byte? Status
    {
        get
        {
            string o = Utils.GetParams("status", null);
            if (string.IsNullOrEmpty(o)) return null;
            return byte.Parse(o);
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
            // 获取单位列表
            case "GET_PRO_UNIT":
                res = Utils.ToJson<T_Dictionary>(bll.GetDictionarys("DW", Guid.Parse(Session["obj_id"] + "")));
                break;
            // 获取产品列表
            case "GET_PRODUCT_LIST":
                res = Utils.ToJson(pBll.GetProductList(Guid.Parse(Session["obj_id"] + ""), SubCtg, Status, Key));
                break;
            // 添加产品
            case "ADD_PRODUCT":
                pBll.AddProduct(GetProduct(), out res);
                break;
            // 设置产品状态
            case "SET_PRODUCT_STATUS":
                res = pBll.SetProductStatus(ProductID, Status.Value) ? "success" : "failed";
                break;
            // 编辑产品
            case "UPDATE_PRODUCT":
                pBll.EditProduct(GetProduct(), out res);
                break;
            // 获取产品详情
            case "GET_PRODUCT_DETAIL":
                res = Utils.ToJson(pBll.GetProductDetail(ProductID));
                break;
        }
        Response.ClearContent();
        Response.ContentType = "text/plain";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(res);
        Response.End();
    }

    /// <summary>
    /// 获取产品实体
    /// </summary>
    /// <returns></returns>
    private T_Product GetProduct()
    {
        T_Product o = new T_Product();
        if (Tag == "ADD_PRODUCT")
        {
            o.F_ProductID = Guid.NewGuid();
            o.F_ObjectID = Guid.Parse(Session["obj_id"] + "");
            o.F_AddDate = DateTime.Now;
            o.F_AddUser = Guid.Parse(Session["user_id"] + "");
        }
        else
        {
            o.F_ProductID = ProductID;
        }
        o.F_ProductName = PName;
        o.F_MainCtg = MainCtg;
        o.F_SubCtg = SubCtg;
        o.F_Code = PCode;
        o.F_Unit = Unit;
        o.F_Size = Size;
        o.F_Weight = Weigtht;
        o.F_Amount = PNum;
        o.F_Alarm = Alarm;
        o.F_BonusType = BonusType;
        o.F_Bonus = Bonuss;
        o.F_Image = ProImage;
        o.F_Remark = Remark;
        o.F_Status = Status;
        return o;
    }
    #endregion
}