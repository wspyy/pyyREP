<%@ WebHandler Language="C#" Class="ReNameModule" %>

using System;
using System.Web;
using System.SingleTable;
using System.SingleTable.PublicMethod;

public class ReNameModule : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string strSql = "";
        string strLogDesc = "";
        if (context.Request["param"] == "ReName")
        {
            string nodeId = context.Request["nodeId"].ToString();
            string nodeName = context.Request["nodeName"].ToString();
            strSql = "update T_Sys_Model set ModuleName = '" + nodeName + "' where ModuleID='" + nodeId + "'";
            strLogDesc = "重命名 ID 为" + nodeId + "的模块为" + nodeName+"。";
        }


        SQLHelper sqlH = new SQLHelper();

        string strMessage = "";
        int flag = sqlH.ExecuteSQLNonQuery(strSql);
        if (flag == 1)
        {
            strMessage = "  修改成功！  ";
        }
        else
        {
            strMessage = "修改失败，请联系管理员！";
        }

        #region 操作记录
        string IP = GetSystemProperties.GetIP();
        string Mac = GetSystemProperties.GetMacBySendARP(IP);
        string userID = "";
        string userName = "";
        if (context.Session["userID"] != null)
        {
            userID = context.Session["userID"].ToString();
            userName = context.Session["userName"].ToString();
        }
        LogInformation.OperationLog(userID, userName, "Empowerment", context.Request["config_Id"] + "01", strLogDesc, IP, Mac);
        #endregion

        context.Response.ContentType = "text/plain";
        context.Response.Write(strMessage);

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}