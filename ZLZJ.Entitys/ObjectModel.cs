using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLZJ.Entitys
{
    /// <summary>
    /// Object实体类(显示信息)
    /// </summary>
    public class ObjectModel
    {
        public Guid ObjID { get; set; }
        public byte? ObjType { get; set; }
        public string ObjName { get; set; }
        public Guid? WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string Contact { get; set; }
        public byte? Status { get; set; }
        public DateTime? AddDate { get; set; }
    }
}
