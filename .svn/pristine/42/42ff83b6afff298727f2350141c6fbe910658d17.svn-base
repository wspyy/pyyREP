<%@ WebHandler Language="C#" Class="ReadImg" %>

using System;
using System.Web;
using System.IO;
using System.Text;

public class ReadImg : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
       // string pathParam=context.Request["path"];
        string date = context.Request["t"];
        if (string.IsNullOrEmpty(date))
        {
            date = DateTime.Now.ToString("yyyyMMdd");
        }
        else {
            date = Convert.ToDateTime(context.Request["t"]).ToString("yyyyMMdd");
        }
        //string filePath = context.Server.MapPath("../PlayImgs/" + date + "00/weatherchart/WRF/" + pathParam);
        string filePath = context.Server.MapPath("../ForeCast/PlayImgs/" + date + "/500hpa");
        if (!Directory.Exists(filePath))
        {
           context.Response.Write("0");
            return;
        }
        else
        {
            DirectoryInfo foldinfo = new DirectoryInfo(filePath);
            FileInfo[] dirs = null;
            dirs = foldinfo.GetFiles("*.gif");
           // dirs = foldinfo.GetFiles("*.png");
            StringBuilder strb = new StringBuilder();
            for (int i = 0, j = dirs.Length; i < j; i++) 
            {
                strb.Append("PlayImgs/" + date + "/500hpa/");
                strb.Append(dirs[i].Name);
                strb.Append(",");
            }
            strb.Remove(strb.Length-1, 1);
            context.Response.Write(strb.ToString());
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}