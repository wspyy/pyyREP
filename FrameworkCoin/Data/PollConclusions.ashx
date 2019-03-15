<%@ WebHandler Language="C#" Class="PollConclusions" %>

using System;
using System.Web;

public class PollConclusions : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string flag = context.Request["flag"];
        if (flag == "1")
        {
            SetPollReport(context);
            context.Response.Write("True");
        }
        else if(flag=="2") {
            string ReportCode = context.Request["ReportCode"].Replace("'", "\"");
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(new Bll.BusinessFun.ConsultReportDal().getpoll(ReportCode));
            context.Response.Write(result);
        }
        else if (flag == "3") {

            SetPollPublishReport(context);
            //context.Response.Write("Hello World");
            context.Response.Write("True");
        }
        
       // context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private void SetPollReport(HttpContext context)
    {
        string poll = context.Request["poll"].Replace("'","\"");
        string opinion = context.Request["opinion"].Replace("'", "\"");
        string ReportCode = context.Request["ReportCode"].Replace("'", "\"");
        string ExpertSign = context.Request["ExpertSign"].Replace("'", "\"");
        string CheckSign = context.Request["CheckSign"].Replace("'", "\"");
        Bll.BusinessFun.ConsultReportDal bll = new Bll.BusinessFun.ConsultReportDal();
        bll.SetPoll(poll, opinion, ReportCode, ExpertSign, CheckSign);
    }

    private void SetPollPublishReport(HttpContext context)
    {
        string poll = context.Request["poll"].Replace("'", "\"");
        string opinion = context.Request["opinion"].Replace("'", "\"");
        string ReportCode = context.Request["ReportCode"].Replace("'", "\"");
        Bll.BusinessFun.ConsultReportDal bll = new Bll.BusinessFun.ConsultReportDal();
        bll.SetPollPublish(poll, opinion, context.Request["level"], ReportCode);
    }

}