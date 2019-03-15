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

public partial class Framework_RightsManagement_UserInformation : AudiPage
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
        //用户信息 config_id  直接写死
        string config_id = "1005";

        GetFormInfo getfrom = new GetFormInfo();

        //得到操作类型 Detail、Update、Add、DetailNoReturn
        string operate_Type = "Update";

        strManageTitle = getfrom.GetFormTitle("编辑信息", "edit");
        strManageButton = getfrom.GetFormButton(false, true);

        string pk_Field = "ID";
        string pk_Value = GetID();

        strManageBody = getfrom.GetDetailUpdate(config_id, pk_Field, pk_Value, 2, operate_Type);

    }

    private string GetID()
    {

        string strSql = "select ID  from T_PC_User where UserID = '" + getSession("userID")
                   + "' and U_RealName='" + getSession("userName") + "'";

        SQLHelper sqlh = new SQLHelper();
        DataSet ds = sqlh.ExecuteSQLDataSet(strSql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["ID"].ToString();
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    private void DataSave()
    {
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
            byte[] buffer = new byte[postedFile.ContentLength];
            postedFile.InputStream.Read(buffer, 0, postedFile.ContentLength);
            postedFile.InputStream.Close();

            filedValue[dataLength - 3] = buffer;

            //保存文件格式
            filedName[dataLength - 2] = "imgFormat";
            filedValue[dataLength - 2] = postedFile.FileName.Remove(0, postedFile.FileName.LastIndexOf('.'));
        }
        #endregion

        string config_id = "1005";

        GetFormInfo getfrom = new GetFormInfo();

        string message = "";
        //得到操作类型 Detail、Update、Add
        string operate_Type = "Update";
        string desc = "";

        string pk_Field = "ID";
        string pk_Value = GetID();
        string sqlWhere = " where " + pk_Field + " = '" + pk_Value + "'";
        message = getfrom.DataSave(config_id, filedName, filedValue, sqlWhere);

        desc = " ID 为 " + pk_Value;

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
