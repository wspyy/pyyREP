<%@ WebHandler Language="C#" Class="GetWeatherImg" %>

using System;
using System.Web;

public class GetWeatherImg : IHttpHandler {
    Bll.BusinessFun.AirMonitor air = new Bll.BusinessFun.AirMonitor();
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string type = context.Request["type"];
        string height = context.Request["height"];
        string begin = context.Request["begin"];
        string end = context.Request["end"];
        string flag=context.Request["flag"];
        string strContent = "";
        if (flag == "loadData")
        {
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(air.GetWeatherImg(type, height, begin, end));
        }
        else if (flag == "loadHeight")
        {
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(air.GetHeight(type));
        }           

       context.Response.Write(strContent);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}