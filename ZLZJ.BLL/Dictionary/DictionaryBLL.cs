using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLZJ.DAL.Dictionary;
using ZLZJ.Entitys;
using System.Data;

namespace ZLZJ.BLL.Dictionary
{
    /// <summary>
    /// 字典操作类
    /// </summary>
    public class DictionaryBLL
    {
        private readonly DictionaryDAL dal;

        public DictionaryBLL()
        {
            dal = new DictionaryDAL();
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="isPublic">是否公共字典</param>
        /// <param name="objID">所属对象ID</param>
        /// <returns></returns>
        public List<T_Dictionary> GetDictionarys(byte isPublic, Guid? objID)
        {
            return dal.GetDictionarys(isPublic, objID);
        }

        /// <summary>
        /// 获取字典列表(绑定树形插件)
        /// </summary>
        /// <param name="isPublic">是否公共字典</param>
        /// <param name="objID">所属对象ID</param>
        /// <returns></returns>
        public string GetDictionarysForTree(byte isPublic, Guid? objID)
        {
            return dal.GetDictionarysForTree(isPublic, objID);
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="code">字典编码</param>
        /// <param name="objID">所属对象ID</param>
        /// <returns></returns>
        public List<T_Dictionary> GetDictionarys(string code, Guid? objID)
        {
            return dal.GetDictionarys(code, objID);
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="parID">父ID</param>
        /// <param name="objID">所属对象ID</param>
        /// <returns></returns>
        public List<T_Dictionary> GetDictionarys(Guid parID, Guid? objID)
        {
            return dal.GetDictionarys(parID, objID);
        }
		
		/// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="parId">字典编码</param>
        /// <returns></returns>
        public List<T_Dictionary> GetDictionarys(Guid parId)
        {
            return dal.GetDictionarys(parId);
        }

        /// <summary>
        /// 获取字典列表(门店或仓管读取分类使用)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetDictionarysForUser(string code)
        {
            return dal.GetDictionarysForUser(code);
        }

        /// <summary>
        /// 获取字典详情
        /// </summary>
        /// <param name="dicID">字典ID</param>
        /// <returns></returns>
        public T_Dictionary GetDictionary(Guid dicID)
        {
            return dal.GetDictionary(dicID);
        }

        /// <summary>
        /// 根据ID获取字典名称
        /// </summary>
        /// <param name="dicID">字典ID</param>
        /// <returns></returns>
        public string GetDicNameByID(Guid dicID)
        {
            return dal.GetDicNameByID(dicID);
        }

        /// <summary>
        /// 根据编码获取ID
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public Guid GetDicIDByCode(string code)
        {
            return dal.GetDicIDByCode(code);
        }

        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <returns></returns>
        public bool AddDictionary(T_Dictionary obj)
        {
            return dal.AddDictionary(obj);
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
            return dal.AddDictionary(parCode, parName, obj);
        }

        /// <summary>
        /// 编辑字典
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <returns></returns>
        public bool EditDictionary(T_Dictionary obj)
        {
            return dal.EditDictionary(obj);
        }

        /// <summary>
        /// 设置字典状态
        /// </summary>
        /// <param name="dicID">字典ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public bool SetDictionaryStatus(Guid dicID, byte status)
        {
            return dal.SetDictionaryStatus(dicID, status);
        }
    }
}
