<%@ WebHandler Language="C#" Class="GetData" %>

using System;
using System.Web;
using Bll.BusinessFun;//0730
using System.IO;//0730
using System.Data;
public class GetData : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        Bll.BusinessFun.AirMonitor bll = new Bll.BusinessFun.AirMonitor();
        string date = context.Request["date"];
        string now = DateTime.Now.ToString("yyyy-MM-dd");
         string content = "";
        if (!string.IsNullOrEmpty(date))
        {
           
            if (bll.GetStationDayData(date,now) != null)
            {
                content = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetStationDayData(date,now));
            }
        }
        context.Response.Write(content);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}