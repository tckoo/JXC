using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLZJ.Entitys
{
    /// <summary>
    /// 员工实体模型
    /// </summary>
    public class PersonModel
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid PersonID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// 所属对象ID
        /// </summary>
        public Guid ObjectID { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        public byte ObjectType { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string PName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public byte? PSex { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCard { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public decimal Wage { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime EnterDate { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public byte IsAdmin { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 添加日期
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
