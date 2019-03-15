<%@ WebHandler Language="C#" Class="GetQxRealTimeData" %>

using System;
using System.Web;

public class GetQxRealTimeData : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string flag = context.Request["flag"];
        string str = context.Request["Strtime"];
        string end = context.Request["Endtime"];
        string code = context.Request["StationCode"];
        //string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm").Replace("-", "").Replace("-", "").Replace(" ", "").Replace(":", "");
        //string date = DateTime.Now.ToString("yyyy-MM-dd")+" "+(DateTime.Now.Hour-1)+":00";
        Bll.BusinessFun.QxMonitor bll = new Bll.BusinessFun.QxMonitor();
        if(flag==null||flag==""){
            //string sqlwhere = " WHERE  time ='" + date + "'";
        string qx = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetQxRealTimeData()).Replace("null", "\"--\"");
        context.Response.Write(qx);
        }
        else if (flag == "HistData") {
            //string Str = Convert.ToDateTime(str).ToString("yyyy-MM-dd HH:mm").Replace("-", "").Replace("-", "").Replace(" ", "").Replace(":", "");
            //string End = Convert.ToDateTime(end).ToString("yyyy-MM-dd HH:mm").Replace("-", "").Replace("-", "").Replace(" ", "").Replace(":", "");
            string sqlwhere = "  WHERE  time <='" + end + "' and time>='" + str + "' and cityname='" + code + "'";
            string qx = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetQxHistData(sqlwhere)).Replace("null", "\"--\"");
            context.Response.Write(qx);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}