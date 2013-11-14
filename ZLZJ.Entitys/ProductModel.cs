using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLZJ.Entitys
{
    /// <summary>
    /// 产品实体模型
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public Guid ProductID { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 所属大类
        /// </summary>
        public string MainCtg { get; set; }

        /// <summary>
        /// 所属小类
        /// </summary>
        public string SubCtg { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 进价
        /// </summary>
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// 卖价
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 净重
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 装箱数量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 报警数量
        /// </summary>
        public int WarnNum { get; set; }

        /// <summary>
        /// 提成类型
        /// </summary>
        public string BonusType { get; set; }

        /// <summary>
        /// 提成金额
        /// </summary>
        public string Bonus { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public byte Status { get; set; }
    }
}
