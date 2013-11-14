using System;
using System.Collections.Generic;
using System.Text;

namespace ZLZJ.Common
{
    /// <summary>
    /// 单据编码操作类
    /// 完成单据编码的下一位计算，例如：输入“DJ0000002”，将返回“DJ0000003”。
    /// </summary>
    /// <remark>
    /// Copyright (c) 2013 ZLZJ. All Rights Reserved.<br/>
    /// 作者：<br/>
    /// 创建时间：<br/>
    /// 功能说明：单据编码操作类<br/>
    /// 修订时间：<br/>
    /// 修订人：<br/>
    /// 修订说明：
    /// </remark>
    public class SheetNumber
    {
        /// <summary>
        /// 生成零字符串
        /// </summary>
        /// <param name="iLen">长度</param>
        /// <returns>零字符串</returns>
        private static string Zero(int iLen)
        {
            string cZero = "0";
            string cResult = "";
            for (int i = 0; i < iLen; i++)
            {
                cResult += cZero;
            }
            return cResult;
        }

        /// <summary>
        /// 下一单据编码
        /// </summary>
        /// <param name="SheetType">单据类型，主要用于分别新的单据类型</param>
        /// <param name="BaseNumber">单据编码（原值）</param>
        /// <returns>下一单据编码</returns>
        public static string NextNumber(string SheetType, string BaseNumber)
        {
            string NewNumber = "";//新值
            int InNumber = 1;//进位
            int PlaceValue;//位值
            char[] No = BaseNumber.ToCharArray();
            for (int i = BaseNumber.Length - 1; i >= 0; i--)
            {
                if (No[i] == '9' && InNumber == 1)
                {
                    InNumber = 1;
                    NewNumber = "0" + NewNumber;
                }
                else
                    if (InNumber == 1 && No[i] >= '0' && No[i] < '9')
                    {
                        PlaceValue = Int32.Parse(No[i].ToString());
                        PlaceValue = (InNumber + PlaceValue);
                        InNumber = 0;
                        NewNumber = PlaceValue.ToString() + NewNumber;
                    }
                    else
                    {
                        InNumber = 0;
                        NewNumber = No[i] + NewNumber;
                    }
            }
            if (BaseNumber == NewNumber)
                NewNumber = SheetType + Zero(9) + "1";
            return NewNumber;
        }

        /// <summary>
        /// 下一单据编码
        /// </summary>
        /// <param name="SheetType">单据类型，主要用于分别新的单据类型</param>
        /// <param name="Import">插入值</param>
        /// <param name="BaseNumber">单据编码（原值）</param>
        /// <returns>下一单据编码</returns>
        public static string NextNumber(string SheetType, string Import, string BaseNumber)
        {
            string NewNumber = "";//新值
            int InNumber = 1;//进位
            int PlaceValue;//位值
            char[] No = BaseNumber.ToCharArray();
            for (int i = BaseNumber.Length - 1; i >= 0; i--)
            {
                if (No[i] == '9' && InNumber == 1)
                {
                    InNumber = 1;
                    NewNumber = "0" + NewNumber;
                }
                else
                    if (InNumber == 1 && No[i] >= '0' && No[i] < '9')
                    {
                        PlaceValue = Int32.Parse(No[i].ToString());
                        PlaceValue = (InNumber + PlaceValue);
                        InNumber = 0;
                        NewNumber = PlaceValue.ToString() + NewNumber;
                    }
                    else
                    {
                        InNumber = 0;
                        NewNumber = No[i] + NewNumber;
                    }
            }
            if (BaseNumber == NewNumber)
                NewNumber = SheetType + Import + Zero(9) + "1";
            return NewNumber;
        }

        /// <summary>
        /// 下一单据编码
        /// </summary>
        /// <param name="SheetType">单据类别</param>
        /// <param name="NumberCount">进位个数</param>
        /// <param name="BaseNumber">单据编码（原值）</param>
        /// <returns>下一单据编码</returns>
        public static string NextNumber(string SheetType, int NumberCount, string BaseNumber)
        {
            string NewNumber = "";//新值
            int InNumber = 1;//进位
            int PlaceValue;//位值
            char[] No = BaseNumber.ToCharArray();
            for (int i = BaseNumber.Length - 1; i >= 0; i--)
            {
                if (No[i] == '9' && InNumber == 1)
                {
                    InNumber = 1;
                    NewNumber = "0" + NewNumber;
                }
                else
                    if (InNumber == 1 && No[i] >= '0' && No[i] < '9')
                    {
                        PlaceValue = Int32.Parse(No[i].ToString());
                        PlaceValue = (InNumber + PlaceValue);
                        InNumber = 0;
                        NewNumber = PlaceValue.ToString() + NewNumber;
                    }
                    else
                    {
                        InNumber = 0;
                        NewNumber = No[i] + NewNumber;
                    }
            }
            if (BaseNumber == NewNumber)
            {
                NewNumber = SheetType + Zero(NumberCount - 1) + "1";
            }
            return NewNumber;
        }

        /// <summary>
        /// 下一单据编码
        /// </summary>
        /// <param name="SheetType">单据类别</param>
        /// <param name="Import">插入值</param>
        /// <param name="NumberCount">进位个数</param>
        /// <param name="BaseNumber">单据编码（原值）</param>
        /// <returns>下一单据编码</returns>
        public static string NextNumber(string SheetType, string Import, int NumberCount, string BaseNumber)
        {
            string NewNumber = "";//新值
            int InNumber = 1;//进位
            int PlaceValue;//位值
            char[] No = BaseNumber.ToCharArray();
            for (int i = BaseNumber.Length - 1; i >= 0; i--)
            {
                if (No[i] == '9' && InNumber == 1)
                {
                    InNumber = 1;
                    NewNumber = "0" + NewNumber;
                }
                else
                    if (InNumber == 1 && No[i] >= '0' && No[i] < '9')
                    {
                        PlaceValue = Int32.Parse(No[i].ToString());
                        PlaceValue = (InNumber + PlaceValue);
                        InNumber = 0;
                        NewNumber = PlaceValue.ToString() + NewNumber;
                    }
                    else
                    {
                        InNumber = 0;
                        NewNumber = No[i] + NewNumber;
                    }
            }
            if (BaseNumber == NewNumber)
            {
                NewNumber = SheetType + Import + Zero(NumberCount - 1) + "1";
            }
            return NewNumber;
        }
    }
}
