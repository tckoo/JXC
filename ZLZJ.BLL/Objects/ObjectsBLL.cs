using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLZJ.Entitys;
using ZLZJ.DAL.Objects;

namespace ZLZJ.BLL.Objects
{
    /// <summary>
    /// 对象表操作类
    /// </summary>
    public class ObjectsBLL
    {
        private readonly ObjectsDAL dal;

        public ObjectsBLL()
        {
            dal = new ObjectsDAL();
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="objType">对象类型(1-分店, 2-仓库)</param>
        /// <param name="status">状态(0-启用; 1-禁用)</param>
        /// <param name="objName">对象名称</param>
        /// <returns></returns>
        public List<ObjectModel> GetObjects(byte? objType, byte? status, string objName)
        {
            return dal.GetObjects(objType, status, objName);
        }

        /// <summary>
        /// 获取对象详情
        /// </summary>
        /// <param name="objID">对象ID</param>
        /// <returns></returns>
        public T_Object GetObject(Guid objID)
        {
            return dal.GetObject(objID);
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <returns></returns>
        public bool AddObject(T_Object obj)
        {
            return dal.AddObject(obj);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <returns></returns>
        public bool UpdateObject(T_Object obj)
        {
            return dal.UpdateObject(obj);
        }

        /// <summary>
        /// 删除对象(假删除)
        /// </summary>
        /// <param name="list">对象ID列表</param>
        /// <returns></returns>
        public bool DeleteObject(List<Guid> list)
        {
            return dal.DeleteObject(list);
        }
    }
}
