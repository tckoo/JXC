using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// 配置信息类
/// </summary>
public class Config
{
    public Config()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 账务分类
    /// </summary>
    public static readonly string KEY_ZWLB = ConfigurationManager.AppSettings["ZW"].ToString();

    /// <summary>
    /// 产品类别
    /// </summary>
    public static readonly string KEY_CPLB = ConfigurationManager.AppSettings["CP"].ToString();

    /// <summary>
    /// 产品单位
    /// </summary>
    public static readonly string KEY_DW = ConfigurationManager.AppSettings["DW"].ToString();

    /// <summary>
    /// 入库编码
    /// </summary>
    public static readonly string KEY_RK = ConfigurationManager.AppSettings["RK"].ToString();

    /// <summary>
    /// 出库编码
    /// </summary>
    public static readonly string KEY_CK = ConfigurationManager.AppSettings["CK"].ToString();

    /// <summary>
    /// 文件上传路径
    /// </summary>
    public static readonly string UPLOAD_PATH = ConfigurationManager.AppSettings["UPLOAD_PATH"].ToString();
}