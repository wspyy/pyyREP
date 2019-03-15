<%@ WebHandler Language="C#" Class="SaveModule" %>

using System;
using System.Web;
using System.SingleTable;
using System.SingleTable.PublicMethod;
using System.Web.SessionState;

public class SaveModule : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        string strSql = "";
        string strLogDesc = "";
        if (context.Request["param"] == "List")
        {
            string list = context.Request["list"].ToString();
            string module = context.Request["module"].ToString();
            if (context.Request["config_Id"] == "1007")
            {
                strSql = "delete from T_PC_Role_Model where RoleID='" + list + "' ";
                strSql += " insert into T_PC_Role_Model (RoleID,ModelID) values ('" + list + "','" + module + "') ";
                strLogDesc = "角色编号 为 " + list;
            }
            else if (context.Request["config_Id"] == "2004")
            {
                strSql = "delete from T_PC_Department_Model where DepartmentID='" + list + "' ";
                strSql += " insert into T_PC_Department_Model (DepartmentID,ModelID) values ('" + list + "','" + module + "') ";
                strLogDesc = "部门编号 为 " + list;
            }
            else if (context.Request["config_Id"] == "1005")
            {
                strSql = "delete from T_PC_User_Model where UserID='" + list + "' ";
                strSql += " insert into T_PC_User_Model (UserID,ModelID) values ('" + list + "','" + module + "') ";
                strLogDesc = "用户编号 为 " + list;
            }
        }


        SQLHelper sqlH = new SQLHelper();

        string strMessage = "";
        int flag = sqlH.ExecuteSQLNonQuery(strSql);
        if (flag == 1 || flag == 2)
        {
            strMessage = "  赋权成功！  ";
        }
        else
        {
            strMessage = "赋权失败，请联系管理员！";
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