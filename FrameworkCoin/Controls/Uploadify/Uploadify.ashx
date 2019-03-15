<%@ WebHandler Language="C#" Class="Uploadify" %>

using System;
using System.Web;
using System.SingleTable;
using System.SingleTable.PublicMethod;
using System.Web.SessionState;

public class Uploadify : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Charset = "utf-8";

        HttpPostedFile file = context.Request.Files["Filedata"];
        string uploadPath =
            HttpContext.Current.Server.MapPath("~") + "\\Framework\\SingleTable\\upfiles\\3018\\";

        if (file != null)
        {
            if (!System.IO.Directory.Exists(uploadPath))
            {
                System.IO.Directory.CreateDirectory(uploadPath);
            }

            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + "-" + file.FileName;
            string imgFormat = file.FileName.Remove(0, file.FileName.LastIndexOf('.'));
            
            string Folder = context.Request.Form["FolderName"] == null ? "" : context.Request.Form["FolderName"].ToString();
            string Tag = context.Request.Form["Tag"] == null ? "" : context.Request.Form["Tag"].ToString();
            
            file.SaveAs(uploadPath + fileName);

            SQLHelper sqlh = new SQLHelper();
            string strSql = @"INSERT INTO [T_Oth_Image]([FileName],[FileUrl],[Folder],[imgFormat],[Tag]) VALUES
           ('" + file.FileName + "','" + fileName + "','" + Folder + "','" + imgFormat + "','" + Tag + "')";

            sqlh.ExecuteSQLNonQuery(strSql);

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
            LogInformation.OperationLog(userID, userName, "Add", context.Request["config_Id"], "上传图片", IP, Mac);
            #endregion

            //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
            context.Response.Write("上传成功!");
        }
        else
        {
            context.Response.Write("0");
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