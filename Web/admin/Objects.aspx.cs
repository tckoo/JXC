using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.BLL.Objects;
using ZLZJ.Entitys;
using ZLZJ.BLL.Dictionary;
using ZLZJ.Common;

public partial class admin_Objects : System.Web.UI.Page
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
    /// 对象类型
    /// </summary>
    private byte? ObjType
    {
        get
        {
            string o = Utils.GetParams("objType", null);
            if (string.IsNullOrEmpty(o)) return null;
            return byte.Parse(o);
        }
    }

    /// <summary>
    /// 所属仓库ID
    /// </summary>
    private Guid? WarehouseID
    {
        get
        {
            string o = Utils.GetParams("warehouseID", null);
            if (string.IsNullOrEmpty(o)) return null;
            return Guid.Parse(o);
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
    /// 对象名称
    /// </summary>
    private string ObjName
    {
        get
        {
            return HttpUtility.UrlDecode(Utils.GetParams("objName", null));
        }
    }

    /// <summary>
    /// 联系人
    /// </summary>
    private string Contact
    {
        get
        {
            return HttpUtility.UrlDecode(Utils.GetParams("contact", null));
        }
    }

    /// <summary>
    /// 联系电话
    /// </summary>
    private string Phone
    {
        get
        {
            return Utils.GetParams("phone", null);
        }
    }

    /// <summary>
    /// 地址
    /// </summary>
    private string Address
    {
        get
        {
            return HttpUtility.UrlDecode(Utils.GetParams("address", null));
        }
    }

    /// <summary>
    /// 对象ID
    /// </summary>
    private Guid ObjID
    {
        get
        {
            return Guid.Parse(Utils.GetParams("objID", Guid.Empty.ToString()));
        }
    }

    /// <summary>
    /// 对象ID列表
    /// </summary>
    private List<Guid> ObjList
    {
        get
        {
            var l = Utils.GetParams("objList", "").Split(',');
            List<Guid> list = new List<Guid>();
            foreach (var s in l)
            {
                list.Add(Guid.Parse(s));
            }
            return list;
        }
    }

    private readonly ObjectsBLL bll = new ObjectsBLL();
    private readonly DictionaryBLL dBll = new DictionaryBLL();
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
            // 获取对象列表
            case "GET_OBJ_LIST":
                res = Utils.ToJson<ObjectModel>(bll.GetObjects(ObjType, null, null));
                break;
            // 获取对象列表(搜索)
            case "SEARCH_OBJECT_LIST":
                res = Utils.ToJson<ObjectModel>(bll.GetObjects(ObjType, Status, ObjName));
                break;
            // 获取仓库列表
            case "GET_WAREHOUSE":
                res = Utils.ToJson<ObjectModel>(bll.GetObjects(2, 0, null));
                break;
            // 添加对象
            case "ADD_OBJECT":
                res = bll.AddObject(GetObject()) ? "success" : "failed";
                break;
            // 更新对象
            case "UPDATE_OBJECT":
                res = bll.UpdateObject(GetObject()) ? "success" : "failed";
                break;
            // 删除对象
            case "DELETE_OBJECT":
                res = bll.DeleteObject(ObjList) ? "success" : "failed";
                break;
            // 获取对象详情
            case "GET_OBJECT_DETAIL":
                res = Utils.ToJson(bll.GetObject(ObjID));
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
    private T_Object GetObject()
    {
        T_Object o = new T_Object();
        if (Tag == "UPDATE_OBJECT")
        {
            o.F_ObjectID = ObjID;
        }
        else
        {
            o.F_ObjectID = Guid.NewGuid();
            o.F_AddDate = DateTime.Now;
        }
        o.F_ObjectType = ObjType;
        o.F_ObjectName = ObjName;
        if (ObjType == 1)
        {
            o.F_WarehouseID = WarehouseID;
        }
        o.F_Address = Address;
        o.F_Conatct = Contact;
        o.F_Tel = Phone;
        o.F_Status = Status;
        return o;
    }
    #endregion
}