using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLZJ.Entitys;
using System.Data;

namespace ZLZJ.DAL.Dictionary
{
    /// <summary>
    /// 字典表操作类
    /// </summary>
    public class DictionaryDAL
    {
        /// <summary>
        /// 获取字典列表(如获取所有产品分类、区域等)
        /// </summary>
        /// <param name="isPublic">是否公共字典</param>
        /// <param name="objID">所属对象ID</param>
        /// <returns></returns>
        public List<T_Dictionary> GetDictionarys(byte isPublic, Guid? objID)
        {
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Dictionary
                                where (t.F_IsPublic == isPublic && t.F_Status == 0)
                                select t;
                    if (isPublic == 1)
                    {
                        query = query.Where(t => t.F_ObjectID == objID);
                    }
                    return query.ToList<T_Dictionary>();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取字典列表(绑定树形插件)
        /// </summary>
        /// <param name="isPublic">是否公共字典</param>
        /// <param name="objID">所属对象ID</param>
        /// <returns></returns>
        public string GetDictionarysForTree(byte isPublic, Guid? objID)
        {
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Dictionary
                                where t.F_IsPublic == isPublic
                                select t;
                    if (isPublic == 1)
                    {
                        query = query.Where(t => t.F_ObjectID == objID);
                    }
                    string json = string.Empty;
                    json += "[";
                    List<T_Dictionary> list = query.ToList<T_Dictionary>();
                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            json += "{\"id\":\"" + list[i].F_DictionaryID +
                                    "\",\"pId\":\"" + list[i].F_ParID +
                                    "\",\"name\":\"" + list[i].F_DicName +
                                    "\",\"code\":\"" + list[i].F_DicCode +
                                    "\",\"status\":\"" + list[i].F_Status + "\"";
                            if (list[i].F_ParID == null || list[i].F_ParID == Guid.Empty)
                            {
                                json += ",\"open\":\"true\"";
                            }
                            json += "}";
                            if (i < list.Count - 1)
                            {
                                json += ",";
                            }
                        }
                    }
                    json += "]";
                    return json;
                }
            }
            catch
            {
                return "[]";
            }
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="code">字典编码</param>
        /// <param name="objID">所属对象ID</param>
        /// <returns></returns>
        public List<T_Dictionary> GetDictionarys(string code, Guid? objID)
        {
            if (string.IsNullOrEmpty(code)) return null;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Dictionary
                                where (t.F_ParID == (from d in edm.T_Dictionary where d.F_DicCode == code select d).FirstOrDefault<T_Dictionary>().F_DictionaryID && t.F_Status == 0)
                                select t;
                    if (objID != null && objID != Guid.Empty)
                    {
                        query = query.Where(t => t.F_ObjectID == objID);
                    }
                    return query.ToList<T_Dictionary>();
                }
            }
            catch
            {
                return null;
            }
        }
		
		/// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="parId">父级节点ID</param>
        /// <returns></returns>
        public List<T_Dictionary> GetDictionarys(Guid parId)
        {
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Dictionary
                                where (t.F_ParID == (from d in edm.T_Dictionary where d.F_ParID == parId select d).FirstOrDefault<T_Dictionary>().F_ParID && t.F_Status == 0)
                                select t;
                    return query.ToList<T_Dictionary>();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取字典列表(门店或仓管读取分类使用, 递归获取所有数据)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetDictionarysForUser(string code)
        {
            if (string.IsNullOrEmpty(code)) return new DataTable();
            try
            {
                string strSql = @"declare @parID uniqueidentifier;
                                select top 1 @parID = F_DictionaryID from T_Dictionary where F_DicCode='{0}';
                                with ctg as(
	                                select * from T_Dictionary where F_DictionaryID = @parID
	                                union all select d.* from T_Dictionary as d,ctg as c where d.F_ParID = c.F_DictionaryID
                                )
                                select *,(select F_DicName from T_Dictionary where F_DictionaryID = ctg.F_ParID)as parName from ctg where F_DictionaryID<>@parID";
                return SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, string.Format(strSql, code)).Tables[0];
            }
            catch
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="parID">父ID</param>
        /// <param name="objID">所属对象ID</param>
        /// <returns></returns>
        public List<T_Dictionary> GetDictionarys(Guid parID, Guid? objID)
        {
            if (parID == Guid.Empty) return null;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Dictionary
                                where t.F_ParID == parID
                                select t;
                    if (objID != null && objID != Guid.Empty)
                    {
                        query = query.Where(t => t.F_ObjectID == objID && t.F_Status == 0);
                    }
                    return query.ToList<T_Dictionary>();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取字典详情
        /// </summary>
        /// <param name="dicID">字典ID</param>
        /// <returns></returns>
        public T_Dictionary GetDictionary(Guid dicID)
        {
            if (dicID == Guid.Empty) return null;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Dictionary
                                where t.F_DictionaryID == dicID
                                select t;
                    return query.First<T_Dictionary>();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据ID获取字典名称
        /// </summary>
        /// <param name="dicID">字典ID</param>
        /// <returns></returns>
        public string GetDicNameByID(Guid dicID)
        {
            if (dicID == Guid.Empty) return null;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var d = (from t in edm.T_Dictionary
                             where t.F_DictionaryID == dicID
                             select t).FirstOrDefault<T_Dictionary>();
                    return d == null ? null : d.F_DicName;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据编码获取ID
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public Guid GetDicIDByCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return Guid.Empty;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Dictionary
                                where t.F_DicCode == code
                                select t;
                    return query.Count() == 0 ? Guid.Empty : query.FirstOrDefault().F_DictionaryID;
                }
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <returns></returns>
        public bool AddDictionary(T_Dictionary obj)
        {
            if (obj == null) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    edm.T_Dictionary.AddObject(obj);
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
        /// 添加字典(用于仓管以及门店添加分类, 如账务分类、产品分类等)
        /// </summary>
        /// <param name="parCode">父级分类编码(若添加账务, 则为ZW; 如为产品分类，则为CP)</param>
        /// <param name="parName">父级分类名称(如账务分类、产品分类等)</param>
        /// <param name="obj">字典实体</param>
        /// <returns></returns>
        public bool AddDictionary(string parCode, string parName, T_Dictionary obj)
        {
            if (string.IsNullOrEmpty(parCode) || obj == null) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Dictionary
                                where t.F_DicCode == parCode
                                select t;
                    Guid parID;
                    T_Dictionary d;
                    if (query.Count() == 0)
                    {
                        d = new T_Dictionary();
                        parID = Guid.NewGuid();
                        d.F_DictionaryID = parID;
                        d.F_DicName = parName;
                        d.F_ParID = Guid.Empty;
                        d.F_DicCode = parCode;
                        d.F_IsPublic = 0;
                        d.F_Status = 0;
                        d.F_AddDate = DateTime.Now;
                        edm.T_Dictionary.AddObject(d);
                    }
                    else
                    {
                        d = query.FirstOrDefault<T_Dictionary>();
                        parID = d.F_DictionaryID;
                    }
                    if (obj.F_ParID == Guid.Empty)
                    {
                        obj.F_ParID = parID;
                    }
                    edm.T_Dictionary.AddObject(obj);
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
        /// 编辑字典
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <returns></returns>
        public bool EditDictionary(T_Dictionary obj)
        {
            if (obj == null) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var o = (from t in edm.T_Dictionary
                             where t.F_DictionaryID == obj.F_DictionaryID
                             select t).FirstOrDefault<T_Dictionary>();
                    o.F_ParID = obj.F_ParID;
                    o.F_DicName = obj.F_DicName;
                    o.F_DicCode = obj.F_DicCode;
                    o.F_Status = obj.F_Status;

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
        /// 设置字典状态
        /// </summary>
        /// <param name="dicID">字典ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public bool SetDictionaryStatus(Guid dicID, byte status)
        {
            if (dicID == Guid.Empty) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var o = (from t in edm.T_Dictionary
                             where t.F_DictionaryID == dicID
                             select t).FirstOrDefault<T_Dictionary>();

                    o.F_Status = status;
                    edm.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
