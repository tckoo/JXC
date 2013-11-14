<%@ WebHandler Language="C#" Class="code" %>

using System;
using System.Web;
using System.Drawing;
using System.IO;
using System.Web.SessionState;

public class code : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ClearContent();
        context.Response.ContentType = "text/plain";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        string validateCode = CreateValidateCode(context);
        // 生成BITMAP图像 
        Bitmap bitmap = new Bitmap(imgWidth, imgHeight);
        // 给图像设置干扰 
        DisturbBitmap(bitmap);
        // 绘制验证码图像 
        DrawValidateCode(bitmap, validateCode);
        // 保存验证码图像，等待输出 
        bitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    #region 设置验证码
    // 验证码长度 
    private int codeLen = 4;
    // 图片清晰度 
    private int fineness = 90;
    // 图片宽度 
    private int imgWidth = 50;
    // 图片高度 
    private int imgHeight = 18;
    // 字体家族名称 
    private string fontFamily = "Arial";
    // 字体大小 
    private int fontSize = 11;
    // 字体样式 
    private int fontStyle = 3;
    // 绘制起始坐标 X 
    private int posX = 5;
    // 绘制起始坐标 Y 
    private int posY = 2;

    private static char[] allCharArray = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    #endregion

    /// <summary> 
    /// 随机生成验证码,并把验证码保存到Session中. 
    /// </summary> 
    /// <returns></returns> 
    private string CreateValidateCode(HttpContext context)
    {
        #region 0-9数字的验证码

        string randomCode = "";
        int t = 0;

        Random rand = new Random();
        for (int i = 0; i < codeLen; i++)
        {
            t = rand.Next(9);

            randomCode += allCharArray[t];
        }

        context.Session["code"] = randomCode;
        return randomCode;

        #endregion
    }

    /// <summary> 
    /// 为图片设置干扰点 
    /// </summary> 
    /// <param name="bitmap"></param> 
    private void DisturbBitmap(Bitmap bitmap)
    {
        // 通过随机数生成 
        Random random = new Random();

        for (int i = 0; i < bitmap.Width; i++)
        {
            for (int j = 0; j < bitmap.Height; j++)
            {
                if (random.Next(100) <= this.fineness)
                    bitmap.SetPixel(i, j, Color.White);
            }
        }
    }

    /// <summary> 
    /// 绘制验证码图片 
    /// </summary> 
    /// <param name="bitmap"></param> 
    /// <param name="validateCode"></param> 
    private void DrawValidateCode(Bitmap bitmap, string validateCode)
    {
        // 获取绘制器对象 
        Graphics g = Graphics.FromImage(bitmap);

        // 设置绘制字体 
        Font font = new Font(fontFamily, fontSize, GetFontStyle());


        // 绘制验证码图像 
        g.DrawString(validateCode, font, Brushes.Black, posX, posY);

        font.Dispose();
        g.Dispose();
    }

    /// <summary> 
    /// 换算验证码字体样式：1 粗体 2 斜体 3 粗斜体，默认为普通字体 
    /// </summary> 
    /// <returns></returns> 
    private FontStyle GetFontStyle()
    {
        if (fontStyle == 1)
            return FontStyle.Bold;
        else if (fontStyle == 2)
            return FontStyle.Italic;
        else if (fontStyle == 3)
            return FontStyle.Bold | FontStyle.Italic;
        else
            return FontStyle.Regular;
    }

}