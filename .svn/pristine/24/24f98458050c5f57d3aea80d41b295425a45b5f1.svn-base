<%@ WebHandler Language="C#" Class="DataUpload" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class DataUpload : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        DateTime reportTime = Convert.ToDateTime(context.Request["reportTime"]);
        string contentTxt = context.Request["contentTxt"];
        int f = 0;
        string path = "../Devoops/Files/";
       if (!Directory.Exists(context.Request.MapPath(path)))
        {
            Directory.CreateDirectory(context.Request.MapPath(path));
        }
       string  upName ="sz" + reportTime.ToString("yyyyMMdd") + "huish.txt";
       string newPath = path + upName;
       if (File.Exists(context.Request.MapPath(newPath)))
       //@"E:/pyy/shuozhou/朔州/FrameworkCoin/Devoops/Files/sz20151015huish.txt"
       {
           f = -1;
       }
       else
       {
           FileStream fs = new FileStream(context.Request.MapPath(newPath), FileMode.Append);
           StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GB2312"));
           //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " +
           sw.WriteLine(contentTxt);
           sw.Close();
           fs.Close();
           f = 1;
       }
       if (f == -1)
       {
           context.Response.Write(upName+"已存在，请勿重复上报！");
       }
       else if (f == 1)
       {
           context.Response.Write(upName + "上报成功！");
       }
       else {
           context.Response.Write(upName + "上报失败，请重新上报！");
       }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}