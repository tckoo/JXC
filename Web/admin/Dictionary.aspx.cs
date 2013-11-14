using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.BLL.Dictionary;
using ZLZJ.Common;
using ZLZJ.Entitys;

public partial class admin_Dictionary : System.Web.UI.Page
{
    #region 异步请求参数
    /// <summary>
    /// 功能参数
    /// </summary>
    private string Tag
    {
        get
        {
            return Utils.GetParams("tag", null);
        }
    }

    /// <summary>
    /// 字典ID
    /// </summary>
    private Guid DicID
    {
        get
        {
            string o = Utils.GetParams("dicID", null);
            if (string.IsNullOrEmpty(o)) return Guid.Empty;
            return Guid.Parse(o);
        }
    }

    /// <summary>
    /// 字典名称
    /// </summary>
    private string DicName
    {
        get
        {
            return HttpUtility.UrlDecode(Utils.GetParams("dicName", null));
        }
    }

    /// <summary>
    /// 父级ID
    /// </summary>
    private Guid? ParID
    {
        get
        {
            string o = Utils.GetParams("parID", null);
            if (string.IsNullOrEmpty(o)) return null;
            return Guid.Parse(o);
        }
    }

    /// <summary>
    /// 字典编码
    /// </summary>
    private string DicCode
    {
        get
        {
            return HttpUtility.UrlDecode(Utils.GetParams("dicCode", null));
        }
    }

    /// <summary>
    /// 状态
    /// </summary>
    private byte Status
    {
        get
        {
            return byte.Parse(Utils.GetParams("status", "0"));
        }
    }

    private readonly DictionaryBLL bll = new DictionaryBLL();
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
        switch (Tag)
        {
            // 获取字典列表
            case "GET_DICTIONARY_LIST":
                res = bll.GetDictionarysForTree(0, null);
                break;
            // 添加字典
            case "ADD_DICTIONARY":
                res = bll.AddDictionary(GetObject()) ? "success" : "failed";
                break;
            // 编辑字典
            case "UPDATE_DICTIONARY":
                res = bll.EditDictionary(GetObject()) ? "success" : "failed";
                break;
            // 删除字典
            case "DELETE_DICTIONARY":
                res = bll.SetDictionaryStatus(DicID, 1) ? "success" : "failed";
                break;
        }
        Response.ClearContent();
        Response.ContentType = "text/plain";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(res);
        Response.End();
    }

    /// <summary>
    /// 获取对象实体
    /// </summary>
    /// <returns></returns>
    private T_Dictionary GetObject()
    {
        T_Dictionary o = new T_Dictionary();
        if (Tag == "UPDATE_DICTIONARY")
        {
            o.F_DictionaryID = DicID;
        }
        else
        {
            o.F_DictionaryID = Guid.NewGuid();
            o.F_AddDate = DateTime.Now;
        }
        o.F_DicName = DicName;
        o.F_DicCode = DicCode;
        o.F_IsPublic = 0;
        o.F_ParID = ParID;
        o.F_Status = Status;

        return o;
    }
    #endregion
}