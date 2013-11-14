using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Collections;
using System.Web.Security;
using System.Runtime.Serialization.Json;

namespace ZLZJ.Common
{
    /// <summary>
    /// 工具类
    /// </summary>
    public sealed class Utils
    {
        public Utils() { }

        /// <summary>
        /// 8位密钥
        /// </summary>
        public static readonly string Key = "zhuhong1";

        /// <summary>
        /// 获得字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirName">文件夹名称</param>
        /// <returns></returns>
        public static bool CreateDirectory(string dirName)
        {
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得指定的文件扩展名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetFileExtension(string fileName)
        {
            string exten = fileName.Substring(fileName.LastIndexOf(".") + 1);
            return exten;
        }

        /// <summary>
        /// 获得指定长度的字符串随机数
        /// </summary>
        /// <param name="length">指定长度</param>
        /// <returns>返回随机数</returns>
        public static string GetRandom(int length)
        {
            string[] source = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string code = "";
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                code += source[rd.Next(0, source.Length - 1)];
            }
            return code;
        }

        /// <summary>
        /// 判断传入的字符串是否为数字
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            Regex reg = new Regex(@"^[-]?\d+[.]?\d*$");
            return reg.IsMatch(str);
        }

        /// <summary>
        /// 判断是否符合Email格式
        /// </summary>
        /// <param name="strEmail">要验证的Email字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        /// <summary>
        /// 判断是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url地址</param>
        /// <returns></returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }

        /// <summary>
        /// 判断IP地址是否有效
        /// </summary>
        /// <param name="ipString">要判断的IP地址字符串</param>
        /// <returns></returns>
        public static bool IsIP(string ipString)
        {
            return Regex.IsMatch(ipString, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool HasDangerSqlString(string sqlString)
        {
            return Regex.IsMatch(sqlString, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="stayString">要检查的字符串</param>
        /// <param name="outLength">超出指定的长度</param>
        /// <param name="disLength">显示的长度</param>
        /// <param name="replaceString">用来代替超出部分的字符串</param>
        /// <returns></returns>
        public static string GetSubString(string stayString, int outLength, int disLength, string replaceString)
        {
            if (stayString.Length > outLength)
            {
                return stayString.Substring(0, disLength) + replaceString;
            }
            else
            {
                return stayString;
            }
        }

        /// <summary>
        /// 格式化从数据中提取的HTML代码在页面上准确显示
        /// </summary>
        /// <param name="htmlcode">要转化的HTML代码</param>
        /// <returns></returns>
        public static string FormatHtml(string htmlcode)
        {
            string str = "";
            str = htmlcode.Replace("\n", "<br />");//回车
            str = htmlcode.Replace("\r", "<br />");//换行
            str = htmlcode.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");//制表符
            str = htmlcode.Replace("\v", "&nbsp;&nbsp;&nbsp;&nbsp;");//垂直制表符
            str = htmlcode.Replace(" ", "&nbsp;");//空格
            str = htmlcode.Replace("<", "&lt;");
            str = htmlcode.Replace(">", "&gt;");
            str = htmlcode.Replace("'", "‘");//单引号
            str = htmlcode.Replace("\"", "“");//双引号
            return str;
        }

        /// <summary>
        /// 去掉字符串中的所有空格（包括全角和半角空格）
        /// </summary>
        /// <param name="str">输入的字符串</param>
        /// <returns></returns>
        public static string TrimAll(string str)
        {
            string str1 = str.Replace("　", " ");
            string str2 = str1.Replace(" ", "");
            return str2;
        }

        /// <summary>
        /// 使用MD5算法加密，由于该算法不可逆因此没有解密方法
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的结果</returns>
        public static string EncryptByMD5(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        /// <summary>
        /// 使用SHA1算法加密，由于该算法不可逆因此没有解密方法
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的结果</returns>
        public static string EncryptBySHA1(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
        }

        /// <summary>
        /// DES加密算法（Base64加密，根据字符串的长度而增长，加密后的字符串长度不固定）
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串</param>
        /// <returns>加密后的结果</returns>
        public static string EncryptByDESbase64(string pToEncrypt)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// DES解密算法（Base64）
        /// </summary>
        /// <param name="pToDecrypt">要解密的字符串</param>
        /// <returns>解密后的结果</returns>
        public static string DecryptByDESbase64(string pToDecrypt)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// DES加密算法（根据字符串的长度而增长，加密后的字符串长度不固定。不能用于中文及其他文字否则不能正常解密）
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串</param>
        /// <returns>加密后的结果</returns>
        public static string EncryptByDES(string pToEncrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// DES解密算法
        /// </summary>
        /// <param name="pToDecrypt">要解密的字符串</param>
        /// <returns>解密后的结果</returns>
        public static string DecryptByDES(string pToDecrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.ASCII.GetString(ms.ToArray());
        }

        /// <summary>
        /// RSA加密算法（生成256位加密字符，每次加密后的结果都不相同。不能用于中文及其他文字否则不能正常解密）
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <returns>加密后的结果</returns>
        public static string EncryptByRSA(string encryptString)
        {
            CspParameters csp = new CspParameters();
            csp.KeyContainerName = Key;
            RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider(csp);
            byte[] encryptBytes = RSAProvider.Encrypt(ASCIIEncoding.ASCII.GetBytes(encryptString), true);
            string str = "";
            foreach (byte b in encryptBytes)
            {
                str = str + string.Format("{0:x2}", b);
            }
            return str;
        }

        /// <summary>
        /// RSA解密算法
        /// </summary>
        /// <param name="decryptString">要解密的字符串</param>
        /// <returns>解密后的结果</returns>
        public static string DecryptByRSA(string decryptString)
        {
            CspParameters csp = new CspParameters();
            csp.KeyContainerName = Key;
            RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider(csp);
            int length = (decryptString.Length / 2);
            byte[] decryptBytes = new byte[length];
            for (int index = 0; index < length; index++)
            {
                string substring = decryptString.Substring(index * 2, 2);
                decryptBytes[index] = Convert.ToByte(substring, 16);
            }
            decryptBytes = RSAProvider.Decrypt(decryptBytes, true);
            return ASCIIEncoding.ASCII.GetString(decryptBytes);
        }

        /// <summary>
        /// 获取URL参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetParams(string paramName, string defaultValue)
        {
            string param = HttpContext.Current.Request.Params[paramName];
            if (!string.IsNullOrEmpty(param))
            {
                string str = param[param.Length - 1].ToString();
                if (str.Equals("#"))
                    param = param.Substring(0, param.Length - 1);
            }
            return string.IsNullOrEmpty(param) || param == "undefined" ? defaultValue : param;
        }

        /// <summary>
        /// 验证邮箱地址是否有效
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <returns>返回true或者false</returns>
        public static bool CheckEmail(string email)
        {
            string EMail = @"^([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$";
            if (string.IsNullOrEmpty(email.Trim()))
                return false;
            else if (Regex.IsMatch(email.Trim(), EMail))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证手机号码是否有效
        /// </summary>
        /// <param name="phone">需要验证的手机号码</param>
        /// <param name="msg">错误信息</param>
        /// <returns>返回true或者false，以及输出参数的值</returns>
        public static bool CheckPhone(string phone, out string msg)
        {
            string Phone = @"^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$";
            if (!string.IsNullOrEmpty(phone.Trim()) && !Regex.IsMatch(phone.Trim(), Phone))
            {
                msg = "手机号码格式错误!";
                return false;
            }
            else
            {
                msg = "";
                return true;
            }
        }

        /// <summary>
        /// 验证身份证号码是否有效
        /// </summary>
        /// <param name="idCard">需要验证的身份证号码</param>
        /// <param name="msg">错误信息</param>
        /// <returns>返回true或者false，以及输出参数的值</returns>
        public static bool CheckIDCard(string idCard, out string msg)
        {
            if (string.IsNullOrEmpty(idCard.Trim()))
            {
                msg = "";
                return true;
            }
            else if (idCard.Trim().Length == 15)
                idCard = ToEighteent(idCard);
            string[] aCity = new string[] { null, null, null, null, null, null, null, null, null, null, null, "北京", "天津", "河北", "山西", 
                "内蒙古", null, null, null, null, null, "辽宁", "吉林", "黑龙江", null, null, null, null, null, null, null, "上海", "江苏", "浙江", 
                "安微", "福建", "江西", "山东", null, null, null, "河南", "湖北", "湖南", "广东", "广西", "海南", null, null, null, "重庆", "四川", 
                "贵州", "云南", "西藏", null, null, null, null, null, null, "陕西", "甘肃", "青海", "宁夏", "新疆", null, null, null, null, null, "台湾", 
                null, null, null, null, null, null, null, null, null, "香港", "澳门", null, null, null, null, null, null, null, null, "国外" };
            double iSum = 0;
            Regex rg = new Regex(@"^\d{17}(\d|x)$");
            Match mc = rg.Match(idCard);
            if (!mc.Success)
            {
                msg = "身份证号码格式错误!";
                return false;
            }

            idCard = idCard.ToLower().Trim().Replace('x', 'a');
            if (aCity[int.Parse(idCard.Substring(0, 2))] == null)
            {
                msg = "非法地区!";
                return false;
            }

            try
            {
                DateTime.Parse(idCard.Substring(6, 4) + "-" + idCard.Substring(10, 2) + "-" + idCard.Substring(12, 2));
            }
            catch
            {
                msg = "非法生日!";
                return false;
            }

            for (int i = 17; i >= 0; i--)
                iSum += (Math.Pow(2, i) % 11) * int.Parse(idCard[17 - i].ToString(), NumberStyles.HexNumber);
            if (iSum % 11 != 1)
            {
                msg = "非法证号!";
                return false;
            }
            msg = "";
            return true;
        }

        /// <summary>
        /// 将15位身份证号码转换为18位
        /// </summary>
        /// <param name="idCard">15位身份证号码</param>
        /// <returns>返回18位的身份证号码</returns>
        private static string ToEighteent(string idCard)
        {
            int iS = 0;
            //加权因子常数 
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码常数 
            string LastCode = "10X98765432";
            //新身份证号 
            string perIDNew;
            perIDNew = idCard.Substring(0, 6);
            //填在第6位及第7位上填上‘1’，‘9’两个数字 
            perIDNew += "19";
            perIDNew += idCard.Substring(6, 9);
            //进行加权求和 
            for (int i = 0; i < 17; i++)
            {
                iS += int.Parse(perIDNew.Substring(i, 1)) * iW[i];
            }
            //取模运算，得到模值 
            int iY = iS % 11;
            //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。 
            perIDNew += LastCode.Substring(iY, 1);
            return perIDNew;
        }


        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
                str = str.ToLower();
            else
                str = "\"" + str + "\"";
            return str;
        }

        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        public T Factore<T>() where T : new()
        {
            return new T();
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static bool IsFileExist(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return false;
            return true;
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(string fileName)
        {
            if (!IsFileExist(fileName)) return null;
            FileInfo info = new FileInfo(fileName);
            return info;
        }

        /// <summary>
        /// 判断是否是图片格式
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool IsImageMode(string filePath)
        {
            if (!IsFileExist(filePath)) return false;
            FileInfo info = GetFileInfo(filePath);
            ArrayList list = new ArrayList { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };
            return list.Contains(info.Extension.ToLower());
        }

        /// <summary>
        /// 获取指定日期为星期几
        /// </summary>
        /// <param name="dt">日期(若未指定, 则返回当前日期)</param>
        /// <returns></returns>
        public static string GetNowWeek()
        {
            switch (Convert.ToInt32(DateTime.Now.DayOfWeek))
            {
                case 0: return "星期日";
                case 1: return "星期一";
                case 2: return "星期二";
                case 3: return "星期三";
                case 4: return "星期四";
                case 5: return "星期五";
                case 6: return "星期六";
                default: return string.Empty;
            }
        }

        /// <summary>   
        /// List转成json    
        /// </summary>   
        /// <typeparam name="T"></typeparam>   
        /// <param name="list"></param>   
        /// <returns></returns>   
        public static string ToJson<T>(IList<T> list)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        object val = pi[j].GetValue(list[i], null);
                        Type type = val == null ? Type.GetType("string") : val.GetType();
                        Json.Append("\"" + pi[j].Name + "\":" + StringFormat(pi[j].GetValue(list[i], null) + "", type));

                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }

        /// <summary>    
        /// 对象转换为Json字符串    
        /// </summary>    
        /// <param name="jsonObject">对象</param>    
        /// <returns>Json字符串</returns>    
        public static string ToJson<T>(T t)
        {
            string jsonString = "{";
            if (t != null)
            {
                PropertyInfo[] pi = t.GetType().GetProperties();
                for (int i = 0; i < pi.Length; i++)
                {
                    object val = pi[i].GetValue(t, null);
                    Type type = val == null ? Type.GetType("string") : val.GetType();
                    jsonString += "\"" + pi[i].Name + "\":" + StringFormat(pi[i].GetValue(t, null) + "", type);
                    if (i < pi.Length - 1)
                    {
                        jsonString += ",";
                    }
                }
            }
            return jsonString + "}";
        }

        /// <summary>   
        /// DataTable转成Json    
        /// </summary>   
        /// <param name="dt"></param>   
        /// <returns></returns>   
        public static string ToJson(DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }

        /// <summary>   
        /// DataRow转成Json    
        /// </summary>   
        /// <param name="dr">DataRow</param>   
        /// <param name="dc">DataColumnCollection</param>
        /// <returns></returns>   
        public static string ToJson(DataRow dr, DataColumnCollection dc)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{");
            if (dr != null && dc != null)
            {
                for (int j = 0; j < dc.Count; j++)
                {
                    Type type = dr[j].GetType();
                    Json.Append("\"" + dc[j].ColumnName.ToString() + "\":" + StringFormat(dr[j].ToString(), type));
                    if (j < dc.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("}");
            return Json.ToString();
        }
    }
}
