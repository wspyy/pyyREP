<%@ WebHandler Language="C#" Class="GetHourData" %>

using System;
using System.Web;

public class GetHourData : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        Bll.BusinessFun.AirMonitor bll = new Bll.BusinessFun.AirMonitor();
        string date = context.Request["date"];
        string content = "";
        if (!string.IsNullOrEmpty(date))
        {
            DateTime time = Convert.ToDateTime(date);
            DateTime now = DateTime.Now;
            if (bll.GetStationHourData(time,now.ToString("yyyy-MM-dd"))!= null)
            {
                content = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetStationHourData(time, now.ToString("yyyy-MM-dd")));
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