<%@ WebHandler Language="C#" Class="GetReportData" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Web.Script.Serialization;
public class GetReportData : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        Bll.BusinessFun.ReportInfo report = new Bll.BusinessFun.ReportInfo();
        int pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
        int pageSize = 10;
        string sqlWhere = "";
        string begin = context.Request["beginTime"];
        string end = context.Request["endTime"];
        string value = context.Request["val"];
        if (!string.IsNullOrEmpty(begin) && !string.IsNullOrEmpty(end))
        {
            sqlWhere += " and r.ReportTime between '" + begin + "'" + " and '" + end + "'";
          
        }
        if (value == "yes")
        {
            sqlWhere += " and YJDJ IS NOT NULL  and YJDJ <> ''";
        }
        else if(value=="no")
        {
            sqlWhere += " and (YJDJ IS NULL or YJDJ = '')";
        }
        //else if (value == "all")
        //{
        //    sqlWhere = "";
        //}
        int pageCount = report.GetCount(pageSize, sqlWhere);
        pageIndex = pageIndex < 1 ? 1 : pageIndex;
        pageIndex = pageIndex > pageCount ? pageCount : pageIndex;
        string pageBar = Bll.BusinessFun.ReportInfo.GetPageBar (pageIndex, pageCount);       
        List<Model.ReportModel> list = report.GetReportData(pageSize, pageIndex, sqlWhere);
        JavaScriptSerializer json = new JavaScriptSerializer();           
        string strContent = "";
        strContent = json.Serialize(new { Report = list, PageBar = pageBar });
        context.Response.Write(strContent);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}