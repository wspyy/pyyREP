<%@ WebHandler Language="C#" Class="GetQxForeCastData" %>

using System;
using System.Web;

public class GetQxForeCastData : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string date = context.Request["date"];
        string hour = context.Request["hour"];
        string day = context.Request["day"];
        string sqlwhere;
        Bll.BusinessFun.QXForeCast bll = new Bll.BusinessFun.QXForeCast();
        if (date == null || hour == null)
        {
            string qx = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetQXForeCastNewData()).Replace("null", "\"--\"");
            context.Response.Write(qx);
        }
        else if (date != null || hour != null)
        {
            DateTime strdate = Convert.ToDateTime(date);
            if (day == "上午")
            {
                string qx = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetQXForeCastHisData(strdate.ToString("yyyy-MM-dd"), "0", hour.Replace("小时", ""))).Replace("null", "\"--\"");
                context.Response.Write(qx);
            }
            else if (day == "下午")
            {
                string qx = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetQXForeCastHisData(strdate.ToString("yyyy-MM-dd"), "1", hour.Replace("小时", ""))).Replace("null", "\"--\"");
                context.Response.Write(qx);
            }
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}