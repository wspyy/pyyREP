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
using System.SingleTable;
using System.SingleTable.PublicMethod;
using System.IO;

public partial class SingleTable_SingleTableForm : AudiPage
{
    #region Manage相关变量
    public string strManageTitle = "";
    public string strManageButton = "";
    public string strManageBody = "";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataLoad();
        }
        else if (Request.Form["flag"] == "AddOrUpdate")
        {
            DataSave();
        }
    }

    /// <summary>
    /// 第一次加载数据
    /// </summary>
    private void DataLoad()
    {

        string config_id = Request.QueryString["config_Id"].ToString();

        GetFormInfo getfrom = new GetFormInfo();

        //得到操作类型 Detail、Update、Add
        string operate_Type = Request.QueryString["operate_Type"].ToString();

        if (operate_Type == "Add")
        {
            strManageTitle = getfrom.GetFormTitle("添加信息", "edit");
            strManageButton = getfrom.GetFormButton(true, true);
            strManageBody = getfrom.GetFormAdd(config_id, 2);
        }
        else
        {
            if (operate_Type == "Detail")
            {
                strManageTitle = getfrom.GetFormTitle("详细信息", "book");
                strManageButton = getfrom.GetFormButton(true, false);
            }
            else if(operate_Type == "DetailNoReturn")
            {
                strManageTitle = getfrom.GetFormTitle("详细信息", "book");
                strManageButton = getfrom.GetFormButton(false, false);
            }
            else
            {
                strManageTitle = getfrom.GetFormTitle("编辑信息", "edit");
                strManageButton = getfrom.GetFormButton(true, true);
            }


            string pk_Field = Request.QueryString["pk_Field"].ToString();
            string pk_Value = Request.QueryString["pk_Value"].ToString();
            strManageBody = getfrom.GetDetailUpdate(config_id, pk_Field, pk_Value, 2, operate_Type);
        }
    }



    /// <summary>
    /// 保存数据
    /// </summary>
    private void DataSave()
    {
        string config_id = Request.Form["config_Id"].ToString();

        GetFormInfo getfrom = new GetFormInfo();

        #region 得到 post 提交的数据
        int dataLength = Request.Form.Count + 3;
        string[] filedName = new string[dataLength];
        object[] filedValue = new object[dataLength];

        for (int i = 0; i < Request.Form.Count; i++)
        {
            string controlName = Request.Form.Keys[i];
            filedName[i] = controlName;
            filedValue[i] = Request.Form[controlName].Trim();//添加去除空格
        }
        if (Request.Files.Count > 0 && Request.Files[0].FileName != "")
        {
            //保存二进制数据
            filedName[dataLength - 3] = Request.Files.Keys[0];

            HttpPostedFile postedFile = Request.Files[0];

            string type = getfrom.CheckSaveFileAddress(config_id,Request.Files.Keys[0]);
            if (type == "文件上传控件_Disk")
            {
                string uploadPath =
                    HttpContext.Current.Server.MapPath("upfiles") + "\\" + config_id + "\\";

                if (postedFile != null)
                {
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + "-" + postedFile.FileName;
                    postedFile.SaveAs(uploadPath + fileName);
                    filedValue[dataLength - 3] = fileName;
                    //保存文件格式
                    filedName[dataLength - 2] = "imgFormat";
                    filedValue[dataLength - 2] = postedFile.FileName.Remove(0, postedFile.FileName.LastIndexOf('.'));
                }
            }
            else
            {
                byte[] buffer = new byte[postedFile.ContentLength];
                postedFile.InputStream.Read(buffer, 0, postedFile.ContentLength);
                postedFile.InputStream.Close();

                filedValue[dataLength - 3] = buffer;

                //保存文件格式
                filedName[dataLength - 2] = "imgFormat";
                filedValue[dataLength - 2] = postedFile.FileName.Remove(0, postedFile.FileName.LastIndexOf('.'));
            }
        }
        #endregion

        //联合主键
        if (Request.Form["Composite_FieldName"] != null && Request.Form["Composite_FieldName"] != "undefined")
        {
            filedName[dataLength - 1] = "." + Request.Form["Composite_FieldName"].ToString();
            filedValue[dataLength - 1] = Request.Form["Composite_FieldValue"].ToString();
        }



        string message = "";
        //得到操作类型 Detail、Update、Add
        string operate_Type = Request.Form["operate_Type"].ToString();
        string desc = "";
        if (operate_Type == "Update")
        {
            string pk_Field = Request.Form["pk_Field"].ToString();
            string pk_Value = Request.Form["pk_Value"].ToString();
            string sqlWhere = " where " + pk_Field + " = '" + pk_Value + "'";
            message = getfrom.DataSave(config_id, filedName, filedValue, sqlWhere);

            desc = Request.Form["pk_Field"].ToString() + " 为 " + Request.Form["pk_Value"].ToString();
        }
        else
        {
            message = getfrom.DataSave(config_id, filedName, filedValue);
            desc = message;
        }

        #region 操作记录
        string IP = GetSystemProperties.GetIP();
        string Mac = GetSystemProperties.GetMacBySendARP(IP);
        string userID = getSession("userID");
        string userName = getSession("userName");
        LogInformation.OperationLog(userID, userName, operate_Type, config_id, desc, IP, Mac);
        #endregion

        Response.Write(message);
        Response.End();

    }


}
