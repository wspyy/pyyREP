using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OleDb;
using System.SingleTable.PublicMethod;
using System.SingleTable;
using System.IO;

public partial class SingleTable_SingleTableImportExcel : AudiPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
        else if (Request.Form["flag"] == "Import")
        {
            ImportSave();
        }
        else if (Request.Form["flag"] == "ImportAgain")
        {
            ImportSaveAgain();
        }
    }
    /// <summary>
    /// 导入保存数据
    /// </summary>
    private void ImportSave()
    {
        string message = "";

        DataTable dt = SaveToServer();

        dt = RemoveNullRow(dt);

        if (ConfigurationManager.AppSettings["ImportRemoveDuplicate"].ToString() == "true")
        {
            GetFormInfo getF = new GetFormInfo();
            string[] strCK = getF.ImportCheckDuplicate(Request.Form["config_Id"].ToString(), dt);

            #region 导入有重复，弹出对话框
            if (strCK[0] != null)
            {
                SQLHelper sqlh = new SQLHelper();
                DataSet dsCK = sqlh.ExecuteSQLDataSet(strCK[0]);
                //若果有重复数据
                if (dsCK.Tables[0].Rows.Count > 0)
                {
                    string rowNum = "";
                    for (int i = 0; i < dsCK.Tables[0].Rows.Count; i++)
                    {
                        rowNum += (i + 1).ToString();
                        if (i + 1 < dsCK.Tables[0].Rows.Count)
                        {
                            rowNum += "、";
                        }
                    }
                    message = " 导入Excel中，有 " + dsCK.Tables[0].Rows.Count + " 行重复数据，<br/>行号：" + rowNum + "! ";

                    Session["ExcelData"] = dt;
                    //将重复的行号、删除重复记录的sql保存到session中
                    strCK[0] = rowNum;
                    Session["ExcelSql"] = strCK;
                    Response.Clear();
                    Response.Write(message);
                    Response.End();
                    return;
                }
            }
            #endregion
        }
        message = ImportData(dt);

        Response.Clear();
        Response.Write(message);
        Response.End();

    }
    /// <summary>
    /// 覆盖导入保存数据
    /// </summary>
    private void ImportSaveAgain()
    {
        if (Request.Form["key"] != null)
        {
            DataTable dt = (DataTable)Session["ExcelData"];

            string[] strSql = (string[])Session["ExcelSql"];
            //覆盖导入
            if (Request.Form["key"].ToString() == "cover")
            {
                SQLHelper sqlh = new SQLHelper();
                sqlh.ExecuteSQLNonQuery(strSql[1]);
            }
            //仅导入未重复
            else if (Request.Form["key"].ToString() == "NotRepeated")
            {
                string[] rowNum = strSql[0].Split('、');
                for (int i = rowNum.Length; i > 0; i--)
                {
                    int rowN = int.Parse(rowNum[i - 1]) - 1;
                    dt.Rows.RemoveAt(rowN);
                }
            }
            string message = ImportData(dt);

            Session["ExcelData"] = null;
            Session["ExcelSql"] = null;

            Response.Clear();
            Response.Write(message);
            Response.End();

        }
    }

    #region 导入操作相关附属私有方法

    /// <summary>
    /// 保存到服务器，读出Excel中原始数据数据
    /// </summary>
    /// <returns></returns>
    private DataTable SaveToServer()
    {
        if (Request.Files.Count > 0 && Request.Files[0].FileName != "")
        {

            HttpPostedFile postedFile = Request.Files[0];
            byte[] buffer = new byte[postedFile.ContentLength];
            postedFile.InputStream.Read(buffer, 0, postedFile.ContentLength);
            postedFile.InputStream.Close();

            string folderPath = Server.MapPath("~/Framework/SingleTable/upfiles/");
            string savePath = folderPath + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".xls";

            FileStream pFileStream = null;
            try
            {
                pFileStream = new FileStream(savePath, FileMode.OpenOrCreate);
                pFileStream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
            }
            finally
            {
                if (pFileStream != null)
                {
                    pFileStream.Close();
                }
            }

            MSExcel msE = new MSExcel();
            DataTable dt = msE.ExcelToDataTable(savePath);
            return dt;
        }
        else
        {
            return null;
        }

    }

    /// <summary>
    /// 移除表中空行
    /// </summary>
    /// <param name="dt">原始表</param>
    /// <returns>移除后的表</returns>
    private DataTable RemoveNullRow(DataTable dtOld)
    {
        DataTable dt = dtOld.Clone();//克隆出表结构
        foreach (DataRow dr in dtOld.Rows)
        {
            bool isNullRow = false;

            #region 判断当前行是否为空，若行中有一列不为空，即认为当前行有数据
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dr[i].ToString().Trim() != "")
                {
                    isNullRow = true;
                    break;
                }
            }
            #endregion

            if (isNullRow)
            {
                //去空格
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dr[i].ToString() != "")
                    {
                        dr[i] = dr[i].ToString().Trim();
                    }
                }

                dt.Rows.Add(dr.ItemArray);
            }
        }
        return dt;
    }

    /// <summary>
    /// 导入数据
    /// </summary>
    /// <param name="dt">需要导入数据表</param>
    /// <returns>提示信息</returns>
    private string ImportData(DataTable dt)
    {
        GetFormInfo getF = new GetFormInfo();
        int executeRowNum = getF.ImportExcelData(Request.Form["config_Id"].ToString(), dt);

        string message = " 成功导入数据 " + executeRowNum.ToString() + " 行! ";

        #region 操作记录
        string IP = GetSystemProperties.GetIP();
        string Mac = GetSystemProperties.GetMacBySendARP(IP);
        string userID = getSession("userID");
        string userName = getSession("userName");

        LogInformation.OperationLog(userID, userName, "Import", Request.QueryString["Config_ID"], message, IP, Mac);
        #endregion

        return message;
    }

    #endregion
}
