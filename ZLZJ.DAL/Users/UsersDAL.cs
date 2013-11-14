using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZLZJ.Entitys;
using ZLZJ.Common;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ZLZJ.DAL.Users
{
    public class UsersDAL : IRequiresSessionState
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户表实体</param>
        /// <param name="msg">输出参数，提示信息</param>
        /// <returns></returns>
        public bool AddUsers(T_User user, out string msg)
        {
            msg = string.Empty;
            if (user == null) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    IQueryable<T_User> iQuery = from t in edm.T_User
                                                where (t.F_Account == user.F_Account && t.F_ObjID == user.F_ObjID)
                                                select t;
                    if (iQuery.Count() > 0)
                    {
                        msg = "exist";
                        return false;
                    }
                    edm.T_User.AddObject(user);
                    msg = "success";
                    return true;
                }
            }
            catch
            {
                msg = "fail";
                return false;
            }
        }

        /// <summary>
        /// 用户登录<br/>
        /// Session["user_id"]-用户ID; Session["account"]-用户名; 
        /// Session["user_type"]-用户类型; Session["obj_id"]-对象ID
        /// Session["person_name"]-员工姓名; Session["user_type_name"]-用户类型名称
        /// </summary>
        /// <param name="userType">用户类型,0-管理员; 1-分店; 2-仓管</param>
        /// <param name="objID">对象ID</param>
        /// <param name="account">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="info">输出参数, 登录信息</param>
        /// <returns></returns>
        public bool UserLogin(byte userType, Guid? objID, string account, string password, out string info)
        {
            info = "failed";
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password)) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var iQuery = from t in edm.T_User
                                 where (t.F_UserType == userType && t.F_Account == account && t.F_UserPwd == password)
                                 select t;
                    if (userType > 0)
                    {
                        iQuery = iQuery.Where(t => t.F_ObjID == objID);
                    }
                    if (iQuery.Count() > 0)
                    {
                        T_User u = iQuery.First<T_User>();
                        if (u.F_Status == 1)
                        {
                            info = "disabled";
                            return false;
                        }
                        HttpContext.Current.Session["user_id"] = u.F_UserID;
                        HttpContext.Current.Session["account"] = u.F_Account;
                        HttpContext.Current.Session["user_type"] = u.F_UserType;
                        HttpContext.Current.Session["obj_id"] = u.F_ObjID;
                        if (u.F_PersonID == null || u.F_PersonID == Guid.Empty)
                        {
                            HttpContext.Current.Session["person_name"] = u.F_Account;
                        }
                        else
                        {
                            var query = from t in edm.T_Person
                                        where t.F_PersonID == u.F_PersonID
                                        select t;
                            T_Person p = query.Single<T_Person>();
                            HttpContext.Current.Session["person_name"] = p.F_Name;
                        }
                        if (u.F_ObjID == null || u.F_ObjID == Guid.Empty)
                        {
                            HttpContext.Current.Session["user_type_name"] = string.Empty;
                        }
                        else
                        {
                            var query = from t in edm.T_Object
                                        where t.F_ObjectID == u.F_ObjID
                                        select t;
                            T_Object o = query.Single<T_Object>();
                            HttpContext.Current.Session["user_type_name"] = o.F_ObjectName;
                        }
                        DateTime dt = DateTime.Now;
                        if (u.F_FirstDate == null) u.F_FirstDate = dt;
                        u.F_LastDate = dt;

                        edm.SaveChanges();
                        info = "success";
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="info">输出参数, 错误信息</param>
        /// <returns></returns>
        public bool EditPwd(Guid userID, string oldPwd, string newPwd, out string info)
        {
            info = "failed";
            if (userID == Guid.Empty || string.IsNullOrEmpty(oldPwd) || string.IsNullOrEmpty(newPwd)) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_User
                                where t.F_UserID == userID
                                select t;
                    if (query.Count() == 0)
                    {
                        info = "unexist";
                        return false;
                    }
                    T_User u = query.FirstOrDefault();
                    if (u.F_UserPwd != oldPwd)
                    {
                        info = "errorpwd";
                        return false;
                    }
                    u.F_UserPwd = newPwd;
                    edm.SaveChanges();
                    info = "success";
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取员工列表
        /// </summary>
        /// <param name="objID">所属对象</param>
        /// <param name="enterDate">入职日期</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="status">状态</param>
        /// <param name="name">关键字</param>
        /// <returns></returns>
        public DataTable GetPersonList(Guid? objID, DateTime? enterDate, byte? isAdmin, byte? status, string name)
        {
            try
            {
                string strSql = @"select p.*,o.F_ObjectName,u.F_Account,u.F_FirstDate,u.F_LastDate,u.F_RegDate,u.F_UserPwd from T_Person as p
                                left join T_User as u
                                on p.F_PersonID = u.F_PersonID 
                                inner join T_Object as o 
                                on p.F_ObjectID=o.F_ObjectID where 1=1 ";
                if (objID != null && objID != Guid.Empty)
                {
                    strSql += "and p.F_ObjectID=@objID ";
                }
                if (enterDate != null)
                {
                    strSql += "and datediff(day,p.F_EnterDate,@enterDate)=0 ";
                }
                if (isAdmin != null)
                {
                    strSql += "and p.F_IsAdmin=@isAdmin ";
                }
                if (status != null)
                {
                    strSql += "and p.F_Status=@status ";
                }
                if (!string.IsNullOrEmpty(name))
                {
                    strSql += "and (p.F_Name like @name or u.F_Account like @name)";
                }

                SqlParameter[] parmList = new SqlParameter[]
                {
                    new SqlParameter(){ParameterName="@objID",SqlDbType=SqlDbType.UniqueIdentifier,Value=objID},
                    new SqlParameter(){ParameterName="@enterDate",SqlDbType=SqlDbType.DateTime,Value=enterDate},
                    new SqlParameter(){ParameterName="@isAdmin",SqlDbType=SqlDbType.TinyInt,Value=isAdmin},
                    new SqlParameter(){ParameterName="@status",SqlDbType=SqlDbType.TinyInt,Value=status},
                    new SqlParameter(){ParameterName="@name",SqlDbType=SqlDbType.NVarChar,Value="%"+name+"%"}
                };

                return SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql, parmList).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="model">员工实体模型</param>
        /// <returns></returns>
        public bool AddPerson(PersonModel model)
        {
            if (model == null) return false;
            using (JXCEntities edm = new JXCEntities())
            {
                edm.Connection.Open();
                using (var tran = edm.Connection.BeginTransaction())
                {
                    try
                    {
                        T_Person p = new T_Person();
                        p.F_PersonID = model.PersonID;
                        p.F_UserType = model.ObjectType;
                        p.F_Name = model.PName;
                        p.F_Sex = model.PSex;
                        p.F_Phone = model.Phone;
                        p.F_Address = model.Address;
                        p.F_IDCard = model.IdCard;
                        p.F_Wage = model.Wage;
                        p.F_EnterDate = model.EnterDate;
                        p.F_ObjectID = model.ObjectID;
                        p.F_IsAdmin = model.IsAdmin;
                        p.F_Status = model.Status;
                        p.F_AddDate = model.AddDate;
                        edm.T_Person.AddObject(p);

                        if (p.F_IsAdmin == 0)
                        {
                            T_User u = new T_User();
                            u.F_UserID = Guid.NewGuid();
                            u.F_UserType = model.ObjectType;
                            u.F_PersonID = model.PersonID;
                            u.F_Account = model.Account;
                            u.F_UserPwd = model.Password;
                            u.F_ObjID = model.ObjectID;
                            u.F_Status = model.Status;
                            edm.T_User.AddObject(u);
                        }

                        edm.SaveChanges();
                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns></returns>
        public bool EditPerson(PersonModel model)
        {
            if (model == null) return false;
            using (JXCEntities edm = new JXCEntities())
            {
                edm.Connection.Open();
                using (var tran = edm.Connection.BeginTransaction())
                {
                    try
                    {
                        var p = (from t in edm.T_Person
                                 where t.F_PersonID == model.PersonID
                                 select t).FirstOrDefault<T_Person>();

                        if (p == null) return false;

                        p.F_UserType = model.ObjectType;
                        p.F_Name = model.PName;
                        p.F_Sex = model.PSex;
                        p.F_Phone = model.Phone;
                        p.F_Address = model.Address;
                        p.F_IDCard = model.IdCard;
                        p.F_Wage = model.Wage;
                        p.F_EnterDate = model.EnterDate;
                        p.F_ObjectID = model.ObjectID;
                        p.F_IsAdmin = model.IsAdmin;
                        p.F_Status = model.Status;

                        if (model.IsAdmin == 0)
                        {
                            var u = (from t in edm.T_User
                                     where t.F_PersonID == model.PersonID
                                     select t).FirstOrDefault<T_User>();
                            if (u != null)
                            {
                                u.F_PersonID = p.F_PersonID;
                                u.F_UserType = model.ObjectType;
                                u.F_Account = model.Account;
                                if (model.IsAdmin == 1)
                                {
                                    u.F_UserPwd = null;
                                }
                                u.F_ObjID = model.ObjectID;
                                u.F_Status = model.Status;
                            }
                            else
                            {
                                u = new T_User();
                                u.F_UserID = Guid.NewGuid();
                                u.F_PersonID = model.PersonID;
                                u.F_UserType = model.ObjectType;
                                u.F_Account = model.Account;
                                u.F_UserPwd = model.Password;
                                u.F_ObjID = model.ObjectID;
                                u.F_Status = model.Status;
                                edm.T_User.AddObject(u);
                            }
                        }

                        edm.SaveChanges();
                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 删除员工(假删除)
        /// </summary>
        /// <param name="list">员工ID列表</param>
        /// <returns></returns>
        public bool DeletePerson(List<Guid> list)
        {
            if (list == null || list.Count == 0) return false;
            using (JXCEntities edm = new JXCEntities())
            {
                edm.Connection.Open();
                using (var tran = edm.Connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var l in list)
                        {
                            var p = (from t in edm.T_Person
                                     where t.F_PersonID == l
                                     select t).FirstOrDefault<T_Person>();

                            if (p == null) return false;
                            p.F_Status = 1;

                            var u = (from t in edm.T_User
                                     where t.F_PersonID == l
                                     select t).FirstOrDefault<T_User>();
                            if (u != null)
                            {
                                u.F_Status = 1;
                            }
                        }
                        edm.SaveChanges();
                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 获取员工详情
        /// </summary>
        /// <param name="personID">员工ID</param>
        /// <returns></returns>
        public DataTable GetPersonDetail(Guid personID)
        {
            if (personID == Guid.Empty) return null;
            try
            {
                string strSql = @"select p.*,o.F_ObjectName,u.F_Account,u.F_FirstDate,u.F_LastDate,u.F_RegDate,u.F_UserPwd from T_Person as p
                                left join T_User as u
                                on p.F_PersonID = u.F_PersonID 
                                inner join T_Object as o 
                                on p.F_ObjectID=o.F_ObjectID where p.F_PersonID = @personID ";
                SqlParameter param = new SqlParameter()
                {
                    ParameterName = "@personID",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Value = personID
                };
                return SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql, param).Tables[0];
            }
            catch
            {
                return null;
            }
        }
    }
}
