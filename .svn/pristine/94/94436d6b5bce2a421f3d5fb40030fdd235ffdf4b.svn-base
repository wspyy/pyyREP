<%@ WebHandler Language="C#" Class="GetAirMonitorData" %>

using System;
using System.Web;

public class GetAirMonitorData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {       
        context.Response.ContentType = "text/plain";
        string flag = context.Request["flag"];
        string Date = context.Request["date"];
        string hour = context.Request["hour"];
        string sqlWhere = context.Request["sqlWhere"];
        Bll.BusinessFun.Pollution bll = new Bll.BusinessFun.Pollution();
        string strContent = "";
              
        if (flag == "Entlist")
        {
            System.Data.DataSet dt = bll.GetEntList();         
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(dt).Replace("null", "\"--\"");

        }
        if (flag == "PSource")
        {
            System.Data.DataSet dt = bll.GetPollutionSourceList(sqlWhere);
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(dt).Replace("null","0");
        
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