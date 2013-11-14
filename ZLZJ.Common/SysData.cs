using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLZJ.Common
{
    public class SysData
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        public enum UserType : byte
        {
            /// <summary>
            /// 管理员
            /// </summary>
            U_ADMIN = 0,

            /// <summary>
            /// 分店
            /// </summary>
            U_Store = 1,

            /// <summary>
            /// 仓库
            /// </summary>
            U_Warehouse = 2
        }
    }
}
