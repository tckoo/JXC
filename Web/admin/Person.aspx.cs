using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLZJ.Common;
using ZLZJ.BLL.Objects;
using ZLZJ.Entitys;
using ZLZJ.BLL.Users;
using System.Data;

public partial class admin_Person : System.Web.UI.Page
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
    /// 入职日期
    /// </summary>
    private DateTime? EnterDate
    {
        get
        {
            string o = Utils.GetParams("enterDate", null);
            if (string.IsNullOrEmpty(o)) return null;
            return DateTime.Parse(o);
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
    /// 员工姓名
    /// </summary>
    private string PName
    {
        get
        {
            return HttpUtility.UrlDecode(Utils.GetParams("pName", null));
        }
    }

    /// <summary>
    /// 联系人
    /// </summary>
    private byte? IsAdmin
    {
        get
        {
            string o = Utils.GetParams("isAdmin", null);
            if (string.IsNullOrEmpty(o)) return null;
            return byte.Parse(o);
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
    /// 员工ID
    /// </summary>
    private Guid PersonID
    {
        get
        {
            return Guid.Parse(Utils.GetParams("personID", Guid.Empty.ToString()));
        }
    }

    /// <summary>
    /// 帐号
    /// </summary>
    private string Account
    {
        get
        {
            return Utils.GetParams("account", null);
        }
    }



    /// <summary>
    /// 员工ID列表
    /// </summary>
    private List<Guid> PersonList
    {
        get
        {
            var l = Utils.GetParams("personList", "").Split(',');
            List<Guid> list = new List<Guid>();
            foreach (var s in l)
            {
                list.Add(Guid.Parse(s));
            }
            return list;
        }
    }

    /// <summary>
    /// 证件号码
    /// </summary>
    private string IdCard
    {
        get
        {
            return Utils.GetParams("idCard", null);
        }
    }

    /// <summary>
    /// 性别
    /// </summary>
    private byte Sex
    {
        get
        {
            return byte.Parse(Utils.GetParams("sex", "0"));
        }
    }

    /// <summary>
    /// 基本工资
    /// </summary>
    private decimal Wage
    {
        get
        {
            return decimal.Parse(Utils.GetParams("wage", "0"));
        }
    }

    /// <summary>
    /// 对象类型
    /// </summary>
    private byte ObjectType
    {
        get
        {
            return byte.Parse(Utils.GetParams("objType", "0"));
        }
    }

    private readonly ObjectsBLL bll = new ObjectsBLL();
    private readonly UsersBLL uBll = new UsersBLL();
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
                res = Utils.ToJson<ObjectModel>(bll.GetObjects(ObjType, 0, null));
                break;
            // 获取员工列表
            case "GET_PERSON_LIST":
                res = Utils.ToJson(uBll.GetPersonList(ObjID, EnterDate, IsAdmin, Status, PName));
                break;
            // 获取员工列表(查询)
            case "SEARCH_PERSON_LIST":
                break;
            // 添加员工
            case "ADD_PERSON":
                res = uBll.AddPerson(GetPerson()) ? "success" : "failed";
                break;
            // 删除员工
            case "DELETE_PERSON":
                res = uBll.DeletePerson(PersonList) ? "success" : "failed";
                break;
            // 获取员工详情
            case "GET_PERSON_DETAIL":
                DataTable dt = uBll.GetPersonDetail(PersonID);
                dt.Columns.Add("pwd");
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["F_UserPwd"] != DBNull.Value)
                        {
                            dr["pwd"] = Utils.DecryptByDESbase64(dr["F_UserPwd"] + "");
                        }
                    }
                }
                res = Utils.ToJson(dt);
                break;
            // 更新员工信息
            case "UPDATE_PERSON":
                res = uBll.EditPerson(GetPerson()) ? "success" : "failed";
                break;
        }
        Response.ClearContent();
        Response.ContentType = "text/plain";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(res);
        Response.End();
    }

    /// <summary>
    /// 获取员工实体
    /// </summary>
    /// <returns></returns>
    private PersonModel GetPerson()
    {
        PersonModel o = new PersonModel();
        if (Tag == "UPDATE_PERSON")
        {
            o.PersonID = PersonID;
        }
        else
        {
            o.PersonID = Guid.NewGuid();
            o.AddDate = DateTime.Now;
        }
        o.ObjectType = ObjectType;
        o.Address = Address;
        o.EnterDate = EnterDate.Value;
        o.IdCard = IdCard;
        o.IsAdmin = IsAdmin.Value;
        if (o.IsAdmin == 0)
        {
            o.Account = Account;
            o.Password = Utils.EncryptByDESbase64(Utils.GetRandom(6));
        }
        else
        {
            o.Account = null;
        }
        o.ObjectID = ObjID;
        o.Phone = Phone;
        o.PName = PName;
        o.PSex = Sex;
        o.Wage = Wage;
        o.Status = Status.Value;
        return o;
    }
    #endregion
}