<%@ WebHandler Language="C#" Class="DBOperation" %>

using System;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
public class DBOperation : IHttpHandler {
    private readonly string sqlServer = ConfigurationManager.AppSettings.Get("DbServer");//连接数据库地址
    private readonly string uid = ConfigurationManager.AppSettings.Get("Uid");//连接数据库用户名
    private readonly string pwd = ConfigurationManager.AppSettings.Get("Pwd");//连接数据库密码
    string path = ConfigurationManager.AppSettings.Get("DbPath");
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        if (context.Request["action"] == "load")
        {
            try
            {
                string SqlStr1 = "Data Source=" + sqlServer + ";Initial Catalog=master;Persist Security Info=True;User ID=" + uid + ";Password=" + pwd + ";";
                //string SqlStr1 = "Server=" + sqlServer + ";DataBase=master;Uid=" + uid + ";Pwd=" + pwd;
                string SqlStr2 = "Exec sp_helpdb";
                SqlConnection con = new SqlConnection(SqlStr1);
                con.Open();
                SqlCommand com = new SqlCommand(SqlStr2, con);
                SqlDataReader dr = com.ExecuteReader(CommandBehavior.CloseConnection);//reder连接关闭时conn连接自动关闭
                DataTable dt = new DataTable();
                dt.Load(dr);
                dr.Close();                
               context. Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dt));
            }
            catch (Exception error)
            {

               context. Response.Write("<span style='font-size:14px;color:red'>" + error.Message + "</span>");
            }
        }
        else if (context.Request["flag"] == "bf")
        {
            string name = context.Request["name"].Trim();
            string selectValue = context.Request["value"];
            string SqlStr1 = "Server=" + sqlServer + ";DataBase='" + selectValue + "';Uid=" + uid + ";Pwd=" + pwd;
            string SqlStr2 = "backup database " + selectValue + " to disk='" +path+name + ".bak'";
            SqlConnection con = new SqlConnection(SqlStr1);
            con.Open();
            try
            {
                if (System.IO.File.Exists(path+name))
                {
                   context. Response.Write("no");
                    return;
                }
                SqlCommand com = new SqlCommand(SqlStr2, con);
                com.ExecuteNonQuery();
                GetBak();
                context.Response.Write("ok");
            }
            catch (Exception error)
            {
                context.Response.Write(error.Message);
            }
            finally
            {
                con.Close();
            }
        }
        else if (context.Request["flag"] == "hy")
        {
            string name = context.Request["name"].Trim();
            string selectValue = context.Request["value"];            
            string path = ConfigurationManager.AppSettings.Get("DbPath");
            string SqlStr1 = "Server=" + sqlServer + ";DataBase=" + "master" + ";Uid=" + uid + ";Pwd=" + pwd;
            //SqlConnection conn = new SqlConnection(SqlStr1);
            //conn.Open();
            //Kill(conn);
            string SqlStr2 = "use master restore database " + selectValue + " from disk='" + path + name + "' WITH NOUNLOAD,REPLACE";
            SqlConnection con = new SqlConnection(SqlStr1);
            con.Open();
            try
            {
                SqlCommand com = new SqlCommand(SqlStr2, con);
                com.ExecuteNonQuery();
                context. Response.Write("ok");
            }
            catch (Exception error)
            {
                context.Response.Write(error.Message);                
            }
            finally
            {
                con.Close();
            }
        }        
    }
    public void GetBak() 
    {
        string SourcePath = ConfigurationManager.AppSettings["PacketSource"];
        string targetPath = ConfigurationManager.AppSettings["PacketTarget"];
        string strAdmin = ConfigurationManager.AppSettings["shareAdmin"];
        string strPwd = ConfigurationManager.AppSettings["sharePwd"];
        string strIP = ConfigurationManager.AppSettings["shareIP"];
        using (Bll.BusinessFun.IdentityScope iss = new Bll.BusinessFun.IdentityScope(strAdmin, strPwd, strIP))
        {
            System.IO.DirectoryInfo TheFolder = new System.IO.DirectoryInfo(SourcePath);
            foreach (System.IO.FileInfo FileItem in TheFolder.GetFiles())
            {
                if (FileItem.Name.IndexOf(".bak") > -1)//有备份文件
                {
                    System.IO.File.Copy(SourcePath + FileItem.Name, targetPath + FileItem.Name, true);

                }               
            }
        }
        
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}