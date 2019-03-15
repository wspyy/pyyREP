<%@ WebHandler Language="C#" Class="MoniRadarImg" %>

using System;
using System.Web;
using System.SingleTable;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
public class MoniRadarImg : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string flag = context.Request["flag"].ToString();
        DateTime begin = Convert.ToDateTime(context.Request["begin"]);
        DateTime end = Convert.ToDateTime(context.Request["end"]).AddHours(23).AddMinutes(59);
        string sql = "select convert(varchar(16),[FileDate],20) FileDate,FullFileName from T_Mid_ImgFiles where cate=" + flag + "and  FileDate between '" + begin + "' and " + " '" + end + "' order by FileDate ";
        SQLHelper sqlh = new SQLHelper("MetaDB");
        DataSet ds = sqlh.ExecuteSQLDataSet(sql);
        string text = string.Empty;
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        {
            //Newtonsoft.Json.Converters.IsoDateTimeConverter convert = new IsoDateTimeConverter();
            //convert.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm'";
           text= JsonConvert.SerializeObject(ds.Tables[0]);
        }
        context.Response.Write(text);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}