<%@ WebHandler Language="C#" Class="SingleTable" %>

using System;
using System.Web;
using System.SingleTable;
using System.SingleTable.PublicMethod;
using System.Web.SessionState;

public class SingleTable : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        if (context.Request["param"] != null && context.Request["param"] == "Delete")
        {
            string message = "";
            string[] value = context.Request["pk_Value"].Split(',');
            int success = 0;
            int fail = 0;
            foreach (string pk_value in value)
            {
                if (DeleteData(context.Request["config_Id"], context.Request["pk_Field"], pk_value, context))
                {
                    success++;
                }
                else
                {
                    fail++;
                }
            }
            message = "删除操作: 成功 " + success.ToString() + " 行,失败 " + fail.ToString() + " 行!";
            context.Response.Write(message);
        }
        else if (context.Request["param"] != null && context.Request["param"] == "Export")
        {
            context.Response.Clear();
            GetGridInfo getg = new GetGridInfo();
            string query = "";
            if (context.Session["Query"] != null)
            {
                query = context.Session["Query"].ToString();
            }

            System.Data.DataTable dt = getg.GetQueryTableSqlExport(context.Request["config_Id"], query);
            context.Response.Write("<meta http-equiv=Content-Type Content=text/html;CharSet=UTF-8\">");
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            ////attachment --- 作为附件下载
            ////inline --- 在线打开
            context.Response.AppendHeader("Content-Disposition", "attachment;filename=Export.xls");

            MSExcel msE = new MSExcel();

            context.Response.Write(msE.AddExcelHead());//显示excel的网格线
            context.Response.Write(msE.ExportTable(dt));//导出
            context.Response.Write(msE.AddExcelbottom());//显示excel的网格线

            context.Response.Flush();
            context.Response.Close();

        }
        else if (context.Request["param"] != null && context.Request["param"] == "ExportDot")
        {
            string excelUrl = System.Web.HttpContext.Current.Server.MapPath("../SingleTable/upfiles/ImportDot.xls");

            GetGridInfo getg = new GetGridInfo();
            bool flag = getg.GetQueryTableSqlExportDot(context.Request["config_Id"], excelUrl);
            if (flag == true)
            {
                //以字符流的形式下载文件
                System.IO.FileStream fs = new System.IO.FileStream(excelUrl, System.IO.FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                context.Response.ContentType = "application/vnd.ms-excel";
                //attachment --- 作为附件下载
                //inline --- 在线打开
                context.Response.AppendHeader("Content-Disposition", "attachment;filename=ImportDot.xls");
                context.Response.BinaryWrite(bytes);
                context.Response.Flush();
                context.Response.End();

            }
            else
            {
                context.Response.Write("");
            }
        }
        else if (context.Request["param"] != null && (context.Request["param"] == "Open" || context.Request["param"] == "Preview"))
        {

            GetGridInfo getg = new GetGridInfo();
            System.Data.DataTable dt = getg.GetCfgTableByID(context.Request["config_Id"]);
            string tableName = dt.Rows[0]["DT_NAME"].ToString();//表名
            string sqlWhere = " where " + context.Request["pk_Field"] + " ='" + context.Request["pk_Value"] + "'";//where条件

            string fileNameColumn = context.Request["fileNameColumn"];//文件名列
            string imgColumn = context.Request["imgColumn"];//二进制数据列
            string imgFormat = context.Request["imgFormat"];//二进制数据格式列

            string strSql = "select " + fileNameColumn + "," + imgColumn + "," + imgFormat + " from " + tableName + sqlWhere;
            SQLHelper sqlH = new SQLHelper();
            System.Data.DataSet ds = sqlH.ExecuteSQLDataSet(strSql);
            if (ds.Tables.Count > 0)
            {
                imgFormat = ds.Tables[0].Rows[0][imgFormat].ToString().Trim();

                if (context.Request["param"] == "Preview" && (imgFormat == ".xls" || imgFormat == ".xlsx" || imgFormat == ".doc" || imgFormat == ".docx" || imgFormat == ".txt"))
                {
                    string htmlUrl = "../SingleTable/upfiles/temp.htm";
                    MSOffice office = new MSOffice();
                    office.OfficeToHTML(imgFormat, (byte[])ds.Tables[0].Rows[0][imgColumn], System.Web.HttpContext.Current.Server.MapPath(htmlUrl));
                    context.Response.Redirect(htmlUrl);
                }
                else
                {
                    if (imgFormat == ".doc" || imgFormat == ".docx")
                    {
                        context.Response.ContentType = "application/msword";
                    }
                    else if (imgFormat == ".xls" || imgFormat == ".xlsx")
                    {
                        context.Response.ContentType = "application/vnd.ms-excel";
                    }
                    else if (imgFormat == ".pdf")
                    {
                        context.Response.ContentType = "application/pdf";
                    }
                    else
                    {
                        context.Response.ContentType = "application/octet-stream";
                    }
                    ////attachment --- 作为附件下载
                    ////inline --- 在线打开
                    context.Response.AppendHeader("Content-Disposition", "inline;filename=" + HttpUtility.UrlEncode(ds.Tables[0].Rows[0][fileNameColumn].ToString(), System.Text.Encoding.UTF8).ToString() + imgFormat);
                    if (ds.Tables[0].Rows[0][imgColumn] != null)
                    {
                        context.Response.BinaryWrite((byte[])ds.Tables[0].Rows[0][imgColumn]);
                    }
                    else
                    {
                        context.Response.Write("无附件");
                    }
                }
            }
        }
        else if (context.Request["param"] != null && (context.Request["param"] == "Open_Disk" || context.Request["param"] == "Preview_Disk"))
        {
            GetGridInfo getg = new GetGridInfo();
            System.Data.DataTable dt = getg.GetCfgTableByID(context.Request["config_Id"]);
            string tableName = dt.Rows[0]["DT_NAME"].ToString();//表名
            string sqlWhere = " where " + context.Request["pk_Field"] + " ='" + context.Request["pk_Value"] + "'";//where条件

            string fileNameColumn = context.Request["imgColumn"];//文件名列
            string imgFormat = context.Request["imgFormat"];//二进制数据格式列

            string strSql = "select " + imgFormat + "," + fileNameColumn + " from " + tableName + sqlWhere;
            SQLHelper sqlH = new SQLHelper();
            System.Data.DataSet ds = sqlH.ExecuteSQLDataSet(strSql);
            if (ds.Tables.Count > 0)
            {
                string fileUrl = "../SingleTable/upfiles/" + context.Request["config_Id"] + "/" + ds.Tables[0].Rows[0][fileNameColumn].ToString().Trim();

                imgFormat = ds.Tables[0].Rows[0][imgFormat].ToString().Trim();

                if (context.Request["param"] == "Preview_Disk" && (imgFormat == ".xls" || imgFormat == ".xlsx" || imgFormat == ".doc" || imgFormat == ".docx" || imgFormat == ".txt"))
                {
                    string htmlUrl = "../SingleTable/upfiles/temp.htm";
                    MSOffice office = new MSOffice();
                    office.OfficeToHTML(imgFormat, System.Web.HttpContext.Current.Server.MapPath(fileUrl), System.Web.HttpContext.Current.Server.MapPath(htmlUrl));
                    fileUrl = htmlUrl;
                }

                context.Response.Redirect(fileUrl);

            }
        }
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="config_ID">配置编号</param>
    /// <param name="pk_Field">主键代码</param>
    /// <param name="pk_Value">主键值</param>
    /// <param name="context"></param>
    /// <returns></returns>
    private bool DeleteData(string config_ID, string pk_Field, string pk_Value, HttpContext context)
    {
        bool flag = false;
        //删除表
        GetGridInfo getg = new GetGridInfo();
        System.Data.DataTable dt = getg.GetCfgTableByID(config_ID);
        string strSql = " delete from " + dt.Rows[0]["DT_NAME"].ToString() + " where " + pk_Field + " ='" + pk_Value + "'";
        SQLHelper sqlH = new SQLHelper();
        int k = sqlH.ExecuteSQLNonQuery(strSql);
        if (k > 0)
        {
            flag = true;
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

        LogInformation.OperationLog(userID, userName, "Delete", config_ID, pk_Field + " 为 " + pk_Value, IP, Mac);
        #endregion
        return flag;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}