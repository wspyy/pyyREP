<%@ WebHandler Language="C#" Class="UserLogin" %>

using System;
using System.Web;
using System.SingleTable;
using System.SingleTable.PublicMethod;
using System.Web.SessionState;

public class UserLogin : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        string strMessage = "";
        if (context.Request.Params["name"] != null)
        {
            string loginName = context.Request.Params["name"].ToString();
            string loginPwd = context.Request.Params["password"].ToString();

            string strSql = "select U_Password,U_RealName,UserID,U_Role,DepartmentID from T_PC_User where U_LoginName ='" + loginName + "' ";

            SQLHelper sqlH = new SQLHelper();
            System.Data.DataSet ds = sqlH.ExecuteSQLDataSet(strSql);
            if (ds.Tables.Count == 0)
            {
                strMessage = "未连接上数据库！";
            }
            else if (ds.Tables[0].Rows.Count == 0)
            {
                strMessage = "该用户名不存在！";
            }
            else
            {
                string pwdInDB = ds.Tables[0].Rows[0]["U_Password"].ToString();
                GetMD5 md = new GetMD5();
                if (pwdInDB != md.StringToMD5(loginPwd))
                {
                    strMessage = "用户名或密码不正确！";
                }
                else
                {
                    string userName = ds.Tables[0].Rows[0]["U_RealName"].ToString();
                    string userID = ds.Tables[0].Rows[0]["UserID"].ToString();

                    strMessage = userName;

                    #region 操作记录
                    string IP = GetSystemProperties.GetIP();
                    string Mac = GetSystemProperties.GetMacBySendARP(IP);
                    LogInformation.OperationLog(userID, userName, "Login", "9999", " 登录成功！", IP, Mac);
                    #endregion

                    strSql = " select ModelID from T_PC_Role_Model where RoleID='" + ds.Tables[0].Rows[0]["U_Role"].ToString() + "' ";
                    strSql += "union select ModelID from T_PC_Department_Model where DepartmentID='" + ds.Tables[0].Rows[0]["DepartmentID"].ToString() + "' ";
                    strSql += "union select ModelID from T_PC_User_Model where UserID='" + ds.Tables[0].Rows[0]["UserID"].ToString() + "' ";
                    ds = sqlH.ExecuteSQLDataSet(strSql);

                    string userRight = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        userRight += ds.Tables[0].Rows[i]["ModelID"].ToString();
                    }

                    context.Session["userRight"] = userRight;
                    context.Session["userID"] = userID;
                    context.Session["userName"] = userName;
                }
            }
        }
        else if (context.Request.Params["menuId"] != null) //保存点击的模块
        {
            context.Session["menuId"] = context.Request.Params["menuId"].ToString();
        }
        else//注销
        {
            context.Session.Remove("userRight");
            context.Session.Remove("userID");
            context.Session.Remove("userName");
        }
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