<%@ WebHandler Language="C#" Class="LisenceApply" %>

using System;
using System.Web;
using System.SingleTable;
using System.SingleTable.PublicMethod;
using System.Web.SessionState;
public class LisenceApply : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
      
        string strMessage = "";
        
            string proNum = context.Request.Params["projectNum"].ToString();
            string proName = context.Request.Params["projectName"].ToString();
            string application = context.Request.Params["application"].ToString();
            string domain = context.Request.Params["domain"].ToString();
            string hostname = context.Request.Params["hostName"].ToString();
            string ip = context.Request.Params["ip"].ToString();
            string starttime = context.Request.Params["startTime"].ToString();
            string endtime = context.Request.Params["endTime"].ToString();
            string lisencoding = context.Request.Params["lisenceCoding"].ToString();

            string sql1 = "select * from T_PC_LisenceApply";
            SQLHelper sqlH = new SQLHelper();
            System.Data.DataSet ds = sqlH.ExecuteSQLDataSet(sql1);
            if (ds.Tables.Count == 0)
            {
                strMessage = "未连接上数据库！";
            }
        
            string sql = "INSERT INTO [T_PC_LisenceApply]([projectNum],[projectName],[application],[domain],[hostName],[ip],[startTime],[endTime],[lisenceCoding])VALUES('" + proNum + "','" + proName + "','" + application + "','" + domain + "','" + hostname + "','" + ip + "','" + starttime + "','" + endtime + "','" + lisencoding + "')";
            //string strSql = "select U_Password,U_RealName,UserID,U_Role,DepartmentID from T_PC_User where U_LoginName ='" + loginName + "' ";
            
            sqlH.ExecuteSQLDataSet(sql);
            
      
        context.Response.ContentType = "text/plain";
        context.Response.Write(strMessage);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}