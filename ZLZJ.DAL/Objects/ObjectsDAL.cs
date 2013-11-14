using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLZJ.Entitys;
using System.Data;

namespace ZLZJ.DAL.Objects
{
    /// <summary>
    /// 对象表操作类
    /// </summary>
    public class ObjectsDAL
    {
        /// <summary>
        /// 获取对象名称
        /// </summary>
        /// <param name="objID">对象ID</param>
        /// <returns></returns>
        private string GetNameByObjID(Guid? objID)
        {
            if (objID == null || objID == Guid.Empty) return null;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    return (from t in edm.T_Object
                            where t.F_ObjectID == objID
                            select t).FirstOrDefault<T_Object>().F_ObjectName;
                }
            }
            catch
            {
                return null;
            }
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
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Object
                                let name = (from d in edm.T_Object where d.F_ObjectID == t.F_WarehouseID select d.F_ObjectName).FirstOrDefault()
                                select new ObjectModel()
                                {
                                    ObjID = t.F_ObjectID,
                                    ObjName = t.F_ObjectName,
                                    ObjType = t.F_ObjectType,
                                    WarehouseID = t.F_WarehouseID,
                                    WarehouseName = name,
                                    Contact = t.F_Conatct,
                                    Status = t.F_Status,
                                    AddDate = t.F_AddDate
                                };

                    if (objType != null)
                    {
                        query = query.Where(t => t.ObjType == objType);
                    }
                    if (status != null)
                    {
                        query = query.Where(t => t.Status == status);
                    }
                    if (!string.IsNullOrEmpty(objName))
                    {
                        query = query.Where(t => t.ObjName.Contains(objName));
                    }
                    return query.ToList<ObjectModel>();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取对象详情
        /// </summary>
        /// <param name="objID">对象ID</param>
        /// <returns></returns>
        public T_Object GetObject(Guid objID)
        {
            if (objID == Guid.Empty) return null;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Object
                                where t.F_ObjectID == objID
                                select t;
                    return query.FirstOrDefault<T_Object>();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <returns></returns>
        public bool AddObject(T_Object obj)
        {
            if (obj == null) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    edm.T_Object.AddObject(obj);
                    edm.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <returns></returns>
        public bool UpdateObject(T_Object obj)
        {
            if (obj == null) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var o = (from t in edm.T_Object
                             where t.F_ObjectID == obj.F_ObjectID
                             select t).FirstOrDefault<T_Object>();
                    o.F_ObjectName = obj.F_ObjectName;
                    // o.F_ObjectType = obj.F_ObjectType;
                    o.F_Address = obj.F_Address;
                    if (o.F_ObjectType == 1)
                    {
                        o.F_WarehouseID = obj.F_WarehouseID;
                    }
                    else
                    {
                        o.F_WarehouseID = null;
                    }
                    o.F_Conatct = obj.F_Conatct;
                    o.F_Status = obj.F_Status;
                    o.F_Tel = obj.F_Tel;

                    edm.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除对象(假删除)
        /// </summary>
        /// <param name="list">对象ID列表</param>
        /// <returns></returns>
        public bool DeleteObject(List<Guid> list)
        {
            if (list == null || list.Count == 0) return false;
            using (JXCEntities edm = new JXCEntities())
            {
                edm.Connection.Open();
                using (var trans = edm.Connection.BeginTransaction())
                {
                    foreach (Guid objID in list)
                    {
                        T_Object obj = edm.T_Object.FirstOrDefault(t => t.F_ObjectID == objID);
                        obj.F_Status = 1;
                    }
                    try
                    {
                        edm.SaveChanges();
                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}
