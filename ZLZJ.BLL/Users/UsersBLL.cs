using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLZJ.DAL.Users;
using ZLZJ.Entitys;
using ZLZJ.Common;
using System.Data;

namespace ZLZJ.BLL.Users
{
    public class UsersBLL
    {
        private readonly UsersDAL dal;

        public UsersBLL()
        {
            dal = new UsersDAL();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户表实体</param>
        /// <param name="msg">输出参数，提示信息</param>
        /// <returns></returns>
        public bool AddUsers(T_User user, out string msg)
        {
            return dal.AddUsers(user, out msg);
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
            return dal.UserLogin(userType, objID, account, password, out info);
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
            return dal.EditPwd(userID, oldPwd, newPwd, out info);
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
            return dal.GetPersonList(objID, enterDate, isAdmin, status, name);
        }

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="model">员工实体模型</param>
        /// <returns></returns>
        public bool AddPerson(PersonModel model)
        {
            return dal.AddPerson(model);
        }

        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns></returns>
        public bool EditPerson(PersonModel model)
        {
            return dal.EditPerson(model);
        }

        /// <summary>
        /// 删除员工(假删除)
        /// </summary>
        /// <param name="list">员工ID列表</param>
        /// <returns></returns>
        public bool DeletePerson(List<Guid> list)
        {
            return dal.DeletePerson(list);
        }

        /// <summary>
        /// 获取员工详情
        /// </summary>
        /// <param name="personID">员工ID</param>
        /// <returns></returns>
        public DataTable GetPersonDetail(Guid personID)
        {
            return dal.GetPersonDetail(personID);
        }
    }
}
