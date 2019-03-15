<%@ WebHandler Language="C#" Class="GetStandardData" %>

using System;
using System.Web;

public class GetStandardData : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        Bll.BusinessFun.AirMonitor bll = new Bll.BusinessFun.AirMonitor();
        string date = context.Request["date"];
        string content = "";
        //string now=DateTime.Now.ToString("yyyy-MM-dd");
        if (!string.IsNullOrEmpty(date))
        {
            String model = bll.getPublishModel();
            DateTime time = Convert.ToDateTime(date);
            DateTime now = DateTime.Now;
            if (model == "0")
            {
                if (bll.GetStationDayStandard(now.ToString("yyyy-MM-dd"), time) != null)
                {
                    content = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetStationDayStandard(now.ToString("yyyy-MM-dd"), time));
                }
            }
            else if (model == "1")
            {
                if (bll.GetStationDayStandard1(now.ToString("yyyy-MM-dd"), time) != null)
                {
                    content = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetStationDayStandard1(now.ToString("yyyy-MM-dd"), time));
                }
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