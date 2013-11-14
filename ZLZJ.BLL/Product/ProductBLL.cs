using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZLZJ.DAL.Product;
using ZLZJ.Entitys;

namespace ZLZJ.BLL.Product
{
    /// <summary>
    /// 产品操作类
    /// </summary>
    public class ProductBLL
    {
        private readonly ProductDAL dal;

        public ProductBLL()
        {
            dal = new ProductDAL();
        }

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
            return dal.GetProductList(objectID, subCtg, status, key);
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="objID">仓库ID</param>
        /// <param name="subCtg">所属子类</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public DataTable GetProductList(Guid objID, Guid? subCtg, string key)
        {
            return dal.GetProductList(objID, subCtg, key);
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="objID">仓库ID</param>
        /// <param name="list">产品ID列表</param>
        /// <returns></returns>
        public DataTable GetProductList(Guid objID, List<Guid> list)
        {
            return dal.GetProductList(objID, list);
        }

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="productID">产品ID</param>
        /// <returns></returns>
        public DataTable GetProductDetail(Guid productID)
        {
            return dal.GetProductDetail(productID);
        }

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="pro">产品实体</param>
        /// <param name="info">输出参数, 添加信息</param>
        /// <returns></returns>
        public bool AddProduct(T_Product pro, out string info)
        {
            return dal.AddProduct(pro, out info);
        }

        /// <summary>
        /// 编辑产品
        /// </summary>
        /// <param name="pro">产品实体</param>
        /// <param name="info">输出参数, 错误信息</param>
        /// <returns></returns>
        public bool EditProduct(T_Product pro, out string info)
        {
            return dal.EditProduct(pro, out info);
        }

        /// <summary>
        /// 设置产品状态
        /// </summary>
        /// <param name="productID">产品ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public bool SetProductStatus(Guid productID, byte status)
        {
            return dal.SetProductStatus(productID, status);
        }

        #region 产品出入库相关
        /// <summary>
        /// 产品出入库
        /// </summary>
        /// <param name="lib">出入库表实体</param>
        /// <param name="list">出入库详情实体列表</param>
        /// <returns></returns>
        public bool ProductInOutWarehouse(T_InOutLibrary lib, List<T_InOutLibraryDetail> list)
        {
            return dal.ProductInOutWarehouse(lib, list);
        }

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
            return dal.GetInWarehouseList(type, startDate, endDate, code);
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
            return dal.SetSheetStatus(libID, status, out info);
        }

        /// <summary>
        /// 获取出入库单详情
        /// </summary>
        /// <param name="libID">出入库单ID</param>
        /// <returns></returns>
        public DataTable GetSheetDetail(Guid libID)
        {
            return dal.GetSheetDetail(libID);
        }

        /// <summary>
        /// 获取出入库单信息(制单用)
        /// </summary>
        /// <param name="libID">出入库单ID</param>
        /// <returns></returns>
        public DataTable GetSheetInfo(Guid libID)
        {
            return dal.GetSheetInfo(libID);
        }

        /// <summary>
        /// 获取前一个编码
        /// </summary>
        /// <param name="sheetType">单据类型(RK-入库; CK-出库)</param>
        /// <returns></returns>
        public string GetPrevSheetNumber(string sheetType)
        {
            return dal.GetPrevSheetNumber(sheetType);
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
            return dal.GetInOutWarehouseDetail(productId, warehouseId, type);
        }
        #endregion

        #region 库存相关
        /// <summary>
        /// 获取需要预警的产品列表
        /// </summary>
        /// <param name="objId">仓库ID</param>
        /// <param name="subCtg">所属子类</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public DataTable GetWarnProduct(Guid objId, Guid? subCtg, string key)
        {
            return dal.GetWarnProduct(objId, subCtg, key);
        }
        #endregion
    }
}
