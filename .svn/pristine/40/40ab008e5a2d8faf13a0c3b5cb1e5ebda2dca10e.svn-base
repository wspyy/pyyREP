<%@ WebHandler Language="C#" Class="GetAirMonitorData" %>

using System;
using System.Web;
using Bll.BusinessFun;//0730
using System.IO;//0730
using System.Data;//0730

public class GetAirMonitorData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string flag = context.Request["flag"];
        string Date = context.Request["date"];
        string hour = context.Request["hour"];

        Bll.BusinessFun.AirMonitor bll = new Bll.BusinessFun.AirMonitor();
        string strContent = "";
        string sqlwhere;
        if (flag == "tabDay")
        {
            if (Date == "" || Date == null)
            {
                DateTime DateNow = DateTime.Now;
                sqlwhere = " where c.AQI IS NOT NULL and b.MoniDate <='" + DateNow.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetAirDayMonitData(sqlwhere)).Replace("null", "\"--\"");

            }
            else
            {
                DateTime date = Convert.ToDateTime(Date);
                //sqlwhere = " where b.MoniDate >='" + date.ToString("yyyy-MM-dd 00:00:00") + "'and b.MoniDate<='" + date.ToString("yyyy-MM-dd 23:59:59") + "'";
                sqlwhere = "where convert(varchar,b.MoniDate,23) = '" + date.ToString("yyyy-MM-dd") + "'";
                strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetAirDayMonitDataByWhere(sqlwhere)).Replace("null", "\"--\"");
            }

        }
        if (flag == "daydata") {
            if (Date == "" || Date == null)
            {
                DateTime DateNow = DateTime.Now;
                sqlwhere = " where b.AQI IS NOT NULL AND  b.MoniDate <='" + DateNow.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetAirDayMonitDataDay(sqlwhere)).Replace("null", "\"--\"");

            }
        }
        else if (flag == "tabHour")
        {
            string str = Date + " " + hour + ":00:00";
            if (str == " :00:00")
            {
                DateTime DateTimeNow = DateTime.Now;
                sqlwhere = "where c.AQI IS NOT NULL and  b.MoniDate<='" + DateTimeNow.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetHourAirMonitData(sqlwhere)).Replace("null", "\"--\"");

            }
            else 
            {
                DateTime strdate = Convert.ToDateTime(str);
                sqlwhere = "where convert(varchar,b.MoniDate,120) like'" + strdate.ToString("yyyy-MM-dd HH:mm:ss") + "%'";
                strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetHourAirMonitDataByWhere(sqlwhere)).Replace("null", "\"--\"");

            }
        }
        else if (flag == "query")
        {
            string type = context.Request["type"];
            string beign = context.Request["begin"];
            string end = context.Request["end"];
            string station = context.Request["station"];
            string item = context.Request["item"];
            if (station.Length > 0) station = station.Substring(0, station.Length - 1);
            if (item.Length > 0)
            {
                if (type == "yoy")
                    item = item.Split(',')[0];
                else
                    item = item.Substring(0, item.Length - 1);
            }
            else {
                if (type == "yoy")
                    item = "AQI";
            }
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetMonitorData(type,beign,end,station,item)).Replace("null", "\"--\"");
        }
        ///ChinaAirQuality.html中的result.DataTable
        else if (flag == "china_city") {
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetAirChinaQuality()).Replace("null", "\"--\"");
            
        }
        context.Response.Write(strContent);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}