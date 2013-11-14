using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLZJ.Entitys;
using System.Data;
using System.Data.SqlClient;
using ZLZJ.DAL.Dictionary;
using System.Data.Common;

namespace ZLZJ.DAL.Product
{
    /// <summary>
    /// 产品相关操作类
    /// </summary>
    public class ProductDAL
    {
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="objectID">所属仓库ID</param>
        /// <param name="subCtg">产品所属小类</param>
        /// <param name="status">状态</param>
        /// <param name="key">关键字(名称、货号、描述)</param>
        /// <returns></returns>
        public DataTable GetProductList(Guid objectID, Guid? subCtg, byte? status, string key)
        {
            try
            {
                string strSql = @"select product.*,
                                    (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_MainCtg)as MainCtg,
                                    (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_SubCtg)as SubCtg,
                                    (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_Unit)as Unit from T_Product as product
                                where product.F_ObjectID = @objID ";
                if (subCtg != null && subCtg != Guid.Empty)
                {
                    strSql += "and product.F_SubCtg = @subCtg ";
                }
                if (status != null)
                {
                    strSql += "and product.F_Status = @status ";
                }
                if (!string.IsNullOrEmpty(key))
                {
                    strSql += "and (product.F_ProductName like @key or product.F_Code like @key or product.F_Remark like @key)";
                }
                SqlParameter[] paramList = new SqlParameter[]
                {
                    new SqlParameter(){ParameterName="@objID",SqlDbType=SqlDbType.UniqueIdentifier,Value=objectID},
                    new SqlParameter(){ParameterName="@subCtg",SqlDbType=SqlDbType.UniqueIdentifier,Value=subCtg},
                    new SqlParameter(){ParameterName="@status",SqlDbType=SqlDbType.TinyInt,Value=status},
                    new SqlParameter(){ParameterName="@key",Value="%"+key+"%"}
                };
                return SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql, paramList).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取产品列表(库存)
        /// </summary>
        /// <param name="objID">仓库ID</param>
        /// <param name="subCtg">所属子类</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public DataTable GetProductList(Guid objID, Guid? subCtg, string key)
        {
            try
            {
                string strSql = @"select product.*,
                                    (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_MainCtg)as MainCtg,
                                    (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_SubCtg)as SubCtg,
                                    (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_Unit)as Unit,
                                    (select F_StockNum from T_Stock where F_ObjectID = product.F_ObjectID and F_ProductID = product.F_ProductID)as WarehouseNum,
                                    (select sum(F_Num) from T_SamplesStore where F_ProductID = product.F_ProductID and F_ObjectID IN(
                                            select F_ObjectID from T_Object where F_ObjectType = 1 and F_WarehouseID = product.F_ObjectID)) as StoreNum,
                                    (select top 1 F_Price from (
			                                select detail.F_Price,detail.F_AddDate from T_InOutLibraryDetail as detail
			                                inner join T_InOutLibrary as lib on detail.F_InOutLibraryID = lib.F_InOutLibraryID 
			                                where detail.F_ProductID = product.F_ProductID and lib.F_ObjectID = product.F_ObjectID
			                                )as b order by F_AddDate desc)as LastInPrice
                                 from T_Product as product
                                 where product.F_ObjectID = @objID and product.F_Status = 0 ";
                if (subCtg != null && subCtg != Guid.Empty)
                {
                    strSql += "and product.F_SubCtg = @subCtg ";
                }
                if (!string.IsNullOrEmpty(key))
                {
                    strSql += "and (product.F_ProductName like @key or product.F_Code like @key or product.F_Remark like @key)";
                }
                SqlParameter[] paramList = new SqlParameter[]
                {
                    new SqlParameter(){ParameterName="@objID",SqlDbType=SqlDbType.UniqueIdentifier,Value=objID},
                    new SqlParameter(){ParameterName="@subCtg",SqlDbType=SqlDbType.UniqueIdentifier,Value=subCtg},
                    new SqlParameter(){ParameterName="@key",Value="%"+key+"%"}
                };
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql, paramList).Tables[0];
                dt.Columns.Add("StoreDetail"); // 店仓库存详情
                foreach (DataRow dr in dt.Rows)
                {
                    dr["StoreDetail"] = GetStoreStockDetail(Guid.Parse(dr["F_ProductID"] + ""), Guid.Parse(dr["F_ObjectID"] + ""));
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="objID">仓库ID</param>
        /// <param name="list">产品ID列表</param>
        /// <returns></returns>
        public DataTable GetProductList(Guid objID, List<Guid> list)
        {
            if (objID == Guid.Empty || list == null || list.Count == 0) return null;
            try
            {
                string where = "";
                for (int i = 0; i < list.Count; i++)
                {
                    where += "'" + list[i] + "'";
                    if (i < list.Count - 1)
                    {
                        where += ",";
                    }
                }
                string strSql = string.Format(@"select product.*,
                                                (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_MainCtg)as MainCtg,
                                                (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_SubCtg)as SubCtg,
                                                (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_Unit)as Unit,
                                                (select F_StockNum from T_Stock where F_ObjectID = product.F_ObjectID and F_ProductID = product.F_ProductID)as WarehouseNum,
                                                (select sum(F_Num) from T_SamplesStore where F_ProductID = product.F_ProductID and F_ObjectID IN(
                                                        select F_ObjectID from T_Object where F_ObjectType = 1 and F_WarehouseID = product.F_ObjectID)) as StoreNum,
                                                (select top 1 F_Price from (
			                                            select detail.F_Price,detail.F_AddDate from T_InOutLibraryDetail as detail
			                                            inner join T_InOutLibrary as lib on detail.F_InOutLibraryID = lib.F_InOutLibraryID 
			                                            where detail.F_ProductID = product.F_ProductID and lib.F_ObjectID = product.F_ObjectID
			                                            )as b order by F_AddDate desc)as LastInPrice
                                             from T_Product as product
                                             where product.F_ObjectID = '{0}' and product.F_Status = 0 and product.F_ProductID in({1})",
                                                                                                                                                                    objID,
                                                                                                                                                                    where);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql).Tables[0];
                dt.Columns.Add("StoreDetail"); // 店仓库存详情
                foreach (DataRow dr in dt.Rows)
                {
                    dr["StoreDetail"] = GetStoreStockDetail(Guid.Parse(dr["F_ProductID"] + ""), Guid.Parse(dr["F_ObjectID"] + ""));
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取门店样品库存详情
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="objId">仓库ID</param>
        /// <returns></returns>
        private string GetStoreStockDetail(Guid productId, Guid objId)
        {
            if (productId == Guid.Empty || objId == Guid.Empty) return null;
            try
            {
                string strSql = string.Format(@"select obj.F_ObjectName,sample.F_Num from T_SamplesStore as sample
                                                inner join T_Object as obj on sample.F_ObjectID = obj.F_ObjectID 
                                                where sample.F_ProductID = '{0}' and sample.F_ObjectID IN(
                                                select F_ObjectID from T_Object where F_ObjectType = 1 and F_WarehouseID = '{1}')",
                                                                                                                                  productId,
                                                                                                                                  objId);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql).Tables[0];
                string str = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str += dt.Rows[i]["F_ObjectName"] + ":<em>" + dt.Rows[i]["F_Num"] + "</em>";
                    if (i < dt.Rows.Count - 1)
                    {
                        str += "</br>";
                    }
                }
                return str;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取最近一次入库价格
        /// </summary>
        /// <param name="objId">仓库ID</param>
        /// <param name="productId">产品ID</param>
        /// <returns></returns>
        private decimal GetLastInPrice(Guid productId, Guid objId)
        {
            if (objId == Guid.Empty || productId == Guid.Empty) return 0;
            try
            {
                string strSql = string.Format(@"select top 1 F_Price from (
                                                select detail.F_Price,detail.F_AddDate from T_InOutLibraryDetail as detail
                                                inner join T_InOutLibrary as lib
                                                on detail.F_InOutLibraryID = lib.F_InOutLibraryID 
                                                where detail.F_ProductID = '{0}' and lib.F_ObjectID = '{1}'
                                                )as b order by F_AddDate desc",
                                                                  productId,
                                                                  objId);
                return decimal.Parse(SqlHelper.ExecuteScalar(SqlHelper.ConnStr, CommandType.Text, strSql) + "");
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取库存数
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="objId">仓库ID</param>
        /// <returns></returns>
        private int GetStock(Guid productId, Guid objId)
        {
            if (productId == Guid.Empty || objId == Guid.Empty) return 0;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    T_Stock stock = (from t in edm.T_Stock
                                     where (t.F_ObjectID == objId && t.F_ProductID == productId)
                                     select t).FirstOrDefault();
                    if (stock == null) return 0;
                    return stock.F_StockNum.Value;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取仓库下所有门店样品数量
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="objId">仓库ID</param>
        /// <returns></returns>
        private int GetStoreStock(Guid productId, Guid objId)
        {
            if (productId == Guid.Empty || objId == Guid.Empty) return 0;
            try
            {
                string strSql = string.Format(@"select sum(F_Num) from T_SamplesStore where F_ProductID = '{0}' and F_ObjectID IN(
                                select F_ObjectID from T_Object where F_ObjectType = 1 and F_WarehouseID = '{1}')",
                                                                                                                  productId,
                                                                                                                  objId);
                return int.Parse(SqlHelper.ExecuteScalar(SqlHelper.ConnStr, CommandType.Text, strSql) + "");
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="productID">产品ID</param>
        /// <returns></returns>
        public DataTable GetProductDetail(Guid productID)
        {
            if (productID == Guid.Empty) return null;
            try
            {
                string strSql = string.Format(@"select product.*,
                                    (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_MainCtg)as MainCtg,
                                    (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_SubCtg)as SubCtg,
                                    (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_Unit)as Unit from T_Product as product 
                                where product.F_ProductID = '{0}'", productID);
                return SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="pro">产品实体</param>
        /// <param name="info">输出参数, 添加信息</param>
        /// <returns></returns>
        public bool AddProduct(T_Product pro, out string info)
        {
            info = "failed";
            if (pro == null) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Product
                                where (t.F_ProductName == pro.F_ProductName || t.F_Code == pro.F_Code)
                                select t;
                    if (query.Count() > 0)
                    {
                        info = "exist";
                        return false;
                    }
                    edm.T_Product.AddObject(pro);
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
        /// 编辑产品
        /// </summary>
        /// <param name="pro">产品实体</param>
        /// <param name="info">输出参数, 错误信息</param>
        /// <returns></returns>
        public bool EditProduct(T_Product pro, out string info)
        {
            info = "failed";
            if (pro == null) return false;
            try
            {
                using (JXCEntities edm = new JXCEntities())
                {
                    var query = from t in edm.T_Product
                                where (t.F_ProductID != pro.F_ProductID && (t.F_ProductName == pro.F_ProductName || t.F_Code == pro.F_Code))
                                select t;
                    if (query.Count() > 0)
                    {
                        info = "exist";
                        return false;
                    }
                    T_Product p = (from t in edm.T_Product
                                   where t.F_ProductID == pro.F_ProductID
                                   select t).FirstOrDefault();
                    p.F_ProductName = pro.F_ProductName;
                    p.F_MainCtg = pro.F_MainCtg;
                    p.F_SubCtg = pro.F_SubCtg;
                    p.F_Code = pro.F_Code;
                    p.F_Unit = pro.F_Unit;
                    p.F_PurchasePrice = pro.F_PurchasePrice;
                    p.F_SellPrice = pro.F_SellPrice;
                    p.F_Size = pro.F_Size;
                    p.F_Weight = pro.F_Weight;
                    p.F_Amount = pro.F_Amount;
                    p.F_Alarm = pro.F_Alarm;
                    p.F_BonusType = pro.F_BonusType;
                    p.F_Bonus = pro.F_Bonus;
                    if (!string.IsNullOrEmpty(pro.F_Image))
                    {
                        p.F_Image = pro.F_Image;
                    }
                    p.F_Remark = pro.F_Remark;
                    p.F_Status = pro.F_Status;

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
        /// 设置产品状态
        /// </summary>
        /// <param name="productID">产品ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public bool SetProductStatus(Guid productID, byte status)
        {
            if (productID == Guid.Empty) return false;
            using (JXCEntities edm = new JXCEntities())
            {
                try
                {
                    T_Product p = (from t in edm.T_Product
                                   where t.F_ProductID == productID
                                   select t).FirstOrDefault();
                    p.F_Status = status;
                    edm.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #region 产品出入库相关
        /// <summary>
        /// 获取产品出入库单列表
        /// </summary>
        /// <param name="type">单据类型(0-入库;1-出库)</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="code">单据编码</param>
        /// <returns></returns>
        public DataTable GetInWarehouseList(byte type, string startDate, string endDate, string code)
        {
            try
            {
                string strSql = string.Format(@"select lib.*,p.F_Name from T_InOutLibrary as lib
                                            inner join T_User as u
                                            on lib.F_AddUser = u.F_UserID
                                            inner join T_Person as p
                                            on u.F_PersonID = p.F_PersonID where 1=1 and lib.F_ChangeType = {0} ",
                                                                                                                 type);
                if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
                {
                    strSql = string.Format(strSql += "and datediff(day,lib.F_AddDate,'{0}')=0 ", startDate);
                }
                if (string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    strSql = string.Format(strSql += "and datediff(day,lib.F_AddDate,'{0}')=0 ", endDate);
                }
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    strSql = string.Format(strSql += "and lib.F_AddDate between '{0}' and '{1}' ", startDate, endDate);
                }
                if (!string.IsNullOrEmpty(code))
                {
                    strSql = string.Format(strSql += "and lib.F_Code like '{0}'", "%" + code + "%");
                }
                return SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 产品出入库
        /// </summary>
        /// <param name="lib">出入库表实体</param>
        /// <param name="list">出入库详情实体列表</param>
        /// <returns></returns>
        public bool ProductInOutWarehouse(T_InOutLibrary lib, List<T_InOutLibraryDetail> list)
        {
            if (lib == null || list == null || list.Count == 0) return false;
            using (JXCEntities edm = new JXCEntities())
            {
                edm.Connection.Open();
                using (var tran = edm.Connection.BeginTransaction())
                {
                    try
                    {
                        decimal sum = 0;
                        lib.F_InOutLibraryID = Guid.NewGuid();
                        foreach (T_InOutLibraryDetail d in list)
                        {
                            d.F_InOutLibraryID = lib.F_InOutLibraryID;
                            sum += d.F_Total.Value;
                            edm.T_InOutLibraryDetail.AddObject(d);

                            // 修改库存
                            UpdateStock(lib.F_InOutLibraryID, d.F_ProductID, lib.F_ObjectID, lib.F_ChangeType, d.F_Amount, lib.F_AddUser);
                        }
                        lib.F_Total = sum;
                        edm.T_InOutLibrary.AddObject(lib);
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
        /// 设置单据状态
        /// </summary>
        /// <param name="libID">出入库单ID</param>
        /// <param name="status">状态</param>
        /// <param name="info">输出参数, 错误信息</param>
        /// <returns></returns>
        public bool SetSheetStatus(Guid libID, byte status, out string info)
        {
            info = "failed";
            if (libID == Guid.Empty) return false;
            using (JXCEntities edm = new JXCEntities())
            {
                edm.Connection.Open();
                using (var tran = edm.Connection.BeginTransaction())
                {
                    try
                    {
                        var query = from t in edm.T_InOutLibrary
                                    where t.F_InOutLibraryID == libID
                                    select t;
                        if (query.Count() == 0)
                        {
                            info = "noexist";
                            return false;
                        }
                        T_InOutLibrary lib = query.FirstOrDefault();
                        // 如果现有状态与将要修改的状态一致, 则返回
                        if (status == lib.F_Status)
                        {
                            info = "unupdate";
                            return false;
                        }

                        lib.F_Status = status;

                        // 修改单据状态(修改库存详情表状态，并且update库存表库存量)
                        var q = from t in edm.T_StockDetail
                                where t.F_LibID == libID
                                select t;
                        if (q.Count() == 0) return false;

                        List<T_StockDetail> list = q.ToList();
                        foreach (var l in list)
                        {
                            l.F_Status = status;

                            var iq = from d in edm.T_Stock
                                     where d.F_StockID == l.F_StockID
                                     select d;
                            if (iq.Count() == 0) return false;
                            T_Stock ts = iq.FirstOrDefault();
                            if (status == 0)
                            {
                                ts.F_StockNum = l.F_EndNum;
                            }
                            else
                            {
                                ts.F_StockNum = l.F_StartNum;
                            }
                        }

                        edm.SaveChanges();
                        tran.Commit();
                        info = "success";
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
        /// 获取出入库单详情
        /// </summary>
        /// <param name="libID">出入库单ID</param>
        /// <returns></returns>
        public DataTable GetSheetDetail(Guid libID)
        {
            if (libID == null) return null;
            try
            {
                string strSql = string.Format(@"select detail.*,pro.F_ProductName,pro.F_Code,pro.F_Size,pro.F_Image from T_InOutLibrary as lib
                                                inner join T_InOutLibraryDetail as detail
                                                on lib.F_InOutLibraryID = detail.F_InOutLibraryID
                                                inner join T_Product as pro
                                                on detail.F_ProductID = pro.F_ProductID
                                                where lib.F_InOutLibraryID = '{0}'", libID);
                return SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取出入库单信息(制单用)
        /// </summary>
        /// <param name="libID">出入库单ID</param>
        /// <returns></returns>
        public DataTable GetSheetInfo(Guid libID)
        {
            if (libID == Guid.Empty) return null;
            try
            {
                string strSql = string.Format(@"select lib.F_Code,lib.F_AddDate,lib.F_Total,person.F_Name from T_InOutLibrary as lib
                                        inner join T_User as u
                                        on lib.F_AddUser = u.F_UserID
                                        inner join T_Person as person
                                        on u.F_PersonID = person.F_PersonID
                                        where lib.F_InOutLibraryID = '{0}'", libID);
                return SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取前一个编码
        /// </summary>
        /// <param name="sheetType">单据类型(RK-入库; CK-出库)</param>
        /// <returns></returns>
        public string GetPrevSheetNumber(string sheetType)
        {
            if (string.IsNullOrEmpty(sheetType)) return string.Empty;
            try
            {
                byte type = 0;
                if (sheetType == "RK")
                {
                    type = 0;
                }
                else if (sheetType == "CK")
                {
                    type = 1;
                }
                string strSql = string.Format("select top 1 F_Code from T_InOutLibrary where F_ChangeType = {0} order by F_AddDate desc", type);
                return SqlHelper.ExecuteScalar(SqlHelper.ConnStr, CommandType.Text, strSql).ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取产品出入库详情
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="type">变更类型(0-入库，1-出库)</param>
        /// <returns></returns>
        public DataTable GetInOutWarehouseDetail(Guid productId, Guid warehouseId, byte type)
        {
            if (productId == Guid.Empty || warehouseId == Guid.Empty) return null;
            try
            {
                string strSql = string.Format(@"select detail.* from T_InOutLibrary as lib
                                                inner join T_InOutLibraryDetail as detail
                                                on lib.F_InOutLibraryID = detail.F_InOutLibraryID
                                                where lib.F_ObjectID = '{0}' and lib.F_Status = 0
                                                and lib.F_ChangeType = '{1}' and detail.F_ProductID = '{2}'",
                                                                                warehouseId,
                                                                                type,
                                                                                productId);
                return SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql).Tables[0];
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 库存相关
        /// <summary>
        /// 更新库存
        /// </summary>
        /// <param name="libId">出入库ID</param>
        /// <param name="productId">产品ID</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="type">所属类型(0-入库, 1-出库)</param>
        /// <param name="num">变更数量</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        private bool UpdateStock(Guid libId, Guid? productId, Guid? warehouseId, byte? type, int? num, Guid? user)
        {
            if (productId == Guid.Empty || warehouseId == Guid.Empty) return false;
            try
            {
                int? startNum = 0, // 期初数
                    endNum; // 期末数
                using (JXCEntities edm = new JXCEntities())
                {
                    var s = (from t in edm.T_Stock
                             where (t.F_ProductID == productId && t.F_ObjectID == warehouseId)
                             select t).FirstOrDefault();
                    if (s != null) startNum = s.F_StockNum;
                }
                string remark = "",
                    strSql = @"declare @num int;
	                            declare @startNum int;
                                declare @stockId uniqueidentifier;
                                select top 1 @num = COUNT(0) from T_Stock where F_ProductID = @proId and F_ObjectID = @objId;
                                if(@num=0)
                                begin
	                                set @stockId = NEWID();
	                                set @startNum = 0;
	                                insert into T_Stock(F_StockID,F_ProductID,F_ObjectID,F_StockNum) values(@stockId,@proId,@objId,@endNum);
                                end
                                else
                                begin
                                    set @stockId = (select F_StockID from T_Stock where F_ProductID = @proId and F_ObjectID = @objId);
	                                select @startNum = F_StockNum from T_Stock where F_ProductID = @proId and F_ObjectID = @objId;
	                                update T_Stock set F_StockNum = @endNum where F_ProductID = @proId and F_ObjectID = @objId
                                end
	                            insert into T_StockDetail(F_StockID,F_ChangType,F_LibID,F_StartNum,F_EndNum,F_ChangeNum,F_Remark,F_Status,F_AddUser,F_AddDate) values(@stockId,@type,@libId,@startNum,@endNum,@changeNum,@remark,0,@author,GETDATE());";

                // 入库
                if (type == 0)
                {
                    endNum = startNum + num;
                }
                // 出库
                else if (type == 1)
                {
                    endNum = startNum - num;
                }
                else endNum = 0;

                // 备注信息
                remark = "productId:" + productId + ",warehouseId:" + warehouseId + ",type:" + type + ",change_num:" + num + ",user:" + user + "start_num:" + startNum + ",end_num:" + endNum;

                SqlParameter[] paramList = new SqlParameter[]
                {
                    new SqlParameter(){ParameterName="@proId",SqlDbType=SqlDbType.UniqueIdentifier,Value=productId},
                    new SqlParameter(){ParameterName="@objId",SqlDbType=SqlDbType.UniqueIdentifier,Value=warehouseId},
                    new SqlParameter(){ParameterName="@type",SqlDbType=SqlDbType.TinyInt,Value=type},
                    new SqlParameter(){ParameterName="@libId",SqlDbType=SqlDbType.UniqueIdentifier,Value=libId},
                    new SqlParameter(){ParameterName="@changeNum",SqlDbType=SqlDbType.Int,Value=num},
                    new SqlParameter(){ParameterName="@endNum",SqlDbType=SqlDbType.Int,Value=endNum},
                    new SqlParameter(){ParameterName="@remark",SqlDbType=SqlDbType.NVarChar,Value=remark},
                    new SqlParameter(){ParameterName="@author",SqlDbType=SqlDbType.UniqueIdentifier,Value=user}
                };
                return SqlHelper.ExecuteNonQuery(SqlHelper.ConnStr, CommandType.Text, strSql, paramList) > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取需要预警的产品列表
        /// </summary>
        /// <param name="objId">仓库ID</param>
        /// <param name="subCtg">所属子类</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public DataTable GetWarnProduct(Guid objId, Guid? subCtg, string key)
        {
            if (objId == Guid.Empty) return null;
            try
            {
                string strSql = string.Format(@"select * from (select product.*,
                                                (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_MainCtg)as MainCtg,
                                                (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_SubCtg)as SubCtg,
                                                (select F_DicName from T_Dictionary as dic where dic.F_DictionaryID = product.F_Unit)as Unit,
                                                (select F_StockNum from T_Stock where F_ObjectID = product.F_ObjectID and F_ProductID = product.F_ProductID)as WarehouseNum,
                                                (select sum(F_Num) from T_SamplesStore where F_ProductID = product.F_ProductID and F_ObjectID IN(
                                                        select F_ObjectID from T_Object where F_ObjectType = 1 and F_WarehouseID = product.F_ObjectID)) as StoreNum,
                                                (select top 1 F_Price from (
			                                            select detail.F_Price,detail.F_AddDate from T_InOutLibraryDetail as detail
			                                            inner join T_InOutLibrary as lib on detail.F_InOutLibraryID = lib.F_InOutLibraryID 
			                                            where detail.F_ProductID = product.F_ProductID and lib.F_ObjectID = product.F_ObjectID
			                                            )as b order by F_AddDate desc)as LastInPrice
                                             from T_Product as product
                                             where product.F_ObjectID = '{0}' and product.F_Status = 0)as b
                                             where ISNULL(ISNULL(WarehouseNum,0)+ISNULL(StoreNum,0),0) < F_Alarm ",
                                                                                                                 objId);
                if (subCtg != null && subCtg != Guid.Empty)
                {
                    strSql = string.Format(strSql += "and F_SubCtg = '{0}' ", subCtg);
                }
                if (!string.IsNullOrEmpty(key))
                {
                    strSql = string.Format(strSql += "and (F_ProductName like '{0}' or F_Code like '{0}' or F_Remark like '{0}'", "%" + key + "%");
                }

                return SqlHelper.ExecuteDataset(SqlHelper.ConnStr, CommandType.Text, strSql).Tables[0];
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
