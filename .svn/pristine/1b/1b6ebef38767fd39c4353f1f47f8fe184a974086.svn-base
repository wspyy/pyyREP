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

public partial class SingleTable_SingleTableGrid : AudiPage
{

    #region Grid相关变量
    public string strGridTitle = "";
    public string strGridButton = "";
    public string strGridBody = "";
    public string strPageInfo = "";
    public string strAllPageNum = "";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataLoad();
        }
    }

    /// <summary>
    /// 重新绑定数据
    /// </summary>
    private void DataLoad()
    {
        string config_id = Request.QueryString["config_Id"].ToString();
        int pageNum = 1;
        //当前页码
        if (Request.QueryString["pageNum"] != null)
        {
            pageNum = int.Parse(Request.QueryString["pageNum"].ToString());
        }
        //每页显示几行
        int pageShowNum = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SingleGridRowCount"].ToString());

        GetGridInfo getGrid = new GetGridInfo();

        strGridTitle = getGrid.GetGridTitle(" 数据列表", "th");

        #region 查询过滤条件
        string strWhere = "";
        strWhere = GetUserDataFiltering();

        //联合主键时，不使用session中保存的查询条件
        if (Request.QueryString["Composite_FieldName"] != null && Request.QueryString["Composite_FieldName"] != "")
        {
            strWhere += " and " + Request.QueryString["Composite_FieldName"].ToString()
                + "='" + Request.QueryString["Composite_FieldValue"].ToString() + "' ";
        }
        else if (Session["Query"] != null)
        {
            strWhere += Session["Query"].ToString();
        }
        #endregion

        string sortField = "";
        string sortRole = "";
        if (Request.QueryString["sortField"] != null)
        {
            sortField = Request.QueryString["sortField"].ToString();
        }
        if (Request.QueryString["sortRole"] != null)
        {
            sortRole = Request.QueryString["sortRole"].ToString();
        }

        string moduleId = getSession("menuId");
        //判断用户权限
        bool userEdit = true;
        bool isExport = true;
        bool isImport = true;

        string title = "";
        if (Request.QueryString["title"] != null)
        {
            title = Request.QueryString["title"].ToString();
        }

        //按钮组中添加事件
        if (config_id == "3018")
        {
            string CustomButton = "<button type=\"button\" class=\"btn blue\" onclick=\"jumpAdd_More();\">批量添加</button>&nbsp;";
            strGridButton = getGrid.GetGridButton(config_id, userEdit, isExport, isImport, title, CustomButton);
        }
        else
        {
            strGridButton = getGrid.GetGridButton(config_id, userEdit, isExport, isImport, title);
        }

        //Grid中添加自定义事件
        if (config_id == "4001")
        {
            string[] CustomButtonName = { "附表信息" };
            string[] CustomButtonValue = { "Composite" };
            strGridBody = getGrid.GetGridHtml(config_id, strWhere, sortField, sortRole, pageNum, pageShowNum, userEdit, CustomButtonName, CustomButtonValue);
        }
        else
        {
            strGridBody = getGrid.GetGridHtml(config_id, strWhere, sortField, sortRole, pageNum, pageShowNum, userEdit);
        }

        //如果总页数小于当前页，重新查询
        if (getGrid.allNum / pageShowNum + 1 < pageNum)
        {
            pageNum = 1;
            strGridBody = getGrid.GetGridHtml(config_id, strWhere, sortField, sortRole, pageNum, pageShowNum, userEdit);
        }
        #region 页码信息
        int startRow = (pageNum - 1) * pageShowNum + 1;
        int endRow = pageNum * pageShowNum > getGrid.allNum ? getGrid.allNum : pageNum * pageShowNum;
        int flag = 0;
        if (getGrid.allNum % pageShowNum != 0)
        {
            flag = 1;
        }
        strAllPageNum = (getGrid.allNum / pageShowNum + flag).ToString();
        strPageInfo = "第 " + startRow.ToString() + " - " + endRow.ToString() + " 条记录 / 共 " + getGrid.allNum.ToString() + " 条";
        #endregion
    }

    /// <summary>
    /// 得到用户数据过滤条件
    /// </summary>
    /// <returns></returns>
    private string GetUserDataFiltering()
    {
        string userID = getSession("userID");
        string config_id = Request.QueryString["config_Id"].ToString();
        string strSql = "select SqlWhere from T_PC_User_DataFiltering where UserID='" + userID + "' and ConfigID='" + config_id + "'";
        SQLHelper sqlh = new SQLHelper();
        DataSet ds = sqlh.ExecuteSQLDataSet(strSql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string sqlw = ds.Tables[0].Rows[0]["SqlWhere"].ToString();

            return sqlw.Replace("’", "'");
        }
        else
        {
            return "";
        }
    }

}
