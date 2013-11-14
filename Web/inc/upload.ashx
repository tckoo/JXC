<%@ WebHandler Language="C#" Class="upload" %>

using System;
using System.Web;
using System.IO;

public class upload : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Charset = "utf-8";

        HttpPostedFile file = context.Request.Files["Filedata"];
        string uploadPath =
            HttpContext.Current.Server.MapPath("~/" + Config.UPLOAD_PATH),
            folderName = GetFolderName(),
            fileName = GetFileName(folderName, file.FileName),
            filePath = "../" + Config.UPLOAD_PATH + "/" + folderName + "/" + fileName;

        if (file != null)
        {
            if (!Directory.Exists(uploadPath + "\\" + folderName + "\\"))
            {
                Directory.CreateDirectory(uploadPath + "\\" + folderName + "\\");
            }

            file.SaveAs(uploadPath + "\\" + folderName + "\\" + fileName);
            //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
            context.Response.Write("{'url':'" + filePath + "'}");
        }
        else
        {
            context.Response.Write("{'url':''}");
        }
    }

    /// <summary>
    /// 生成文件夹名称
    /// </summary>
    /// <returns></returns>
    private string GetFolderName()
    {
        return DateTime.Now.ToString("yyyyMMdd");
    }

    /// <summary>
    /// 生成文件名
    /// </summary>
    /// <param name="folderName">文件夹名称</param>
    /// <param name="fileName">原文件名</param>
    /// <returns></returns>
    private string GetFileName(string folderName, string fileName)
    {
        string exName = fileName.Split('.')[1],
                strRd = ZLZJ.Common.Utils.GetRandom(6);
        return folderName + strRd + "." + exName;

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}