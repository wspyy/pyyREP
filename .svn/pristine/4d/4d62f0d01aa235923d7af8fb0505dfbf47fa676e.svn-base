using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.SingleTable;

public partial class SingleTable_SingleTableQuery : AudiPage
{
    #region Query相关变量
    public string strQueryTitle = "";
    public string strQueryButton = "";
    public string strQueryBody = "";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Query"] = "";
            QueryLoad();
        }
        else if (Request.Form["flag"] == "Query")
        {
            QuerySave();
        }

    }

    /// <summary>
    /// 加载查询数据
    /// </summary>
    private void QueryLoad()
    {
        string config_id = Request.QueryString["config_Id"].ToString();

        GetQueryInfo getquery = new GetQueryInfo();

        string title = "";
        if (Request.QueryString["title"] != null)
        {
            title = Request.QueryString["title"].ToString();
        }

        strQueryTitle = getquery.GetQueryTitle("快捷查询", "search");
        strQueryButton = getquery.GetQueryButton(true, true);
        strQueryBody = getquery.GetQueryBody(config_id, 2);

    }

    /// <summary>
    /// 查询保存数据
    /// </summary>
    private void QuerySave()
    {
        #region 得到 post 提交的数据
        int dataLength = Request.Form.Count + 2;
        string[] filedName = new string[dataLength];
        object[] filedValue = new object[dataLength];

        for (int i = 0; i < Request.Form.Count; i++)
        {
            string controlName = Request.Form.Keys[i];
            filedName[i] = controlName;
            filedValue[i] = Request.Form[controlName];
        }
        #endregion

        string config_id = Request.Form["config_Id"].ToString();

        GetQueryInfo getquery = new GetQueryInfo();

        string message = getquery.DataSave(config_id, filedName, filedValue);

        Session["Query"] = message;

        Response.End();
    }
}