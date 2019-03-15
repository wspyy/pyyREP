using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.SingleTable;

public partial class Devoops_Index : AudiPage
{
    public string defultEdition = string.Empty;

    public string strModule = "";
    public string strUser = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        defultEdition = "配置系统框架";

        if (!IsPostBack)
        {
            if (Session["userName"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            strUser = Session["userName"].ToString();

            DataTable dt = GetAllModul();
            CreateModul(dt);

        }
    }



    /// <summary>
    /// 返回全部模块
    /// </summary>
    /// <returns>数据集</returns>
    private DataTable GetAllModul()
    {
        SQLHelper sqlH = new SQLHelper();
        string strSql = " select ModuleID,ModuleName,ModuleParentID,IconUrl,ModuleURL,OnClick,OrderID from T_Sys_Model ";
        strSql += " where SystemName='" + defultEdition + "' and VisibleType = 1 order by orderid";

        DataSet ds = sqlH.ExecuteSQLDataSet(strSql);

        return ds.Tables[0];
    }

    /// <summary>
    /// 初始化一级模块
    /// </summary>
    private void CreateModul(DataTable dt)
    {
        //一级模块
        DataRow[] drAll = dt.Select(" ModuleParentID = '0' ", "  OrderID ");

        foreach (DataRow dr in drAll)
        {
            #region 判断权限

            if (!IsUserRight(dr["ModuleID"].ToString()))
            {
                continue;
            }

            #endregion 判断权限

            string imageUrl = dr["IconUrl"].ToString();
            string moduleID = dr["ModuleID"].ToString();
            string moduleName = dr["ModuleName"].ToString();

            //模块下二级菜单
            DataRow[] drAllM = dt.Select(" ModuleParentID = '" + moduleID + "' ", "  OrderID ");
            string submenu = CreateMenu(drAllM);
            string moduleURL = dr["ModuleURL"].ToString();
            string onClick = dr["OnClick"].ToString();
            //没有子菜单
            if (submenu == "")
            {
                strModule += "<li>";
                if (moduleURL == "")
                {
                    strModule += "<a class=\"active\" href=\"#\" id='" + moduleID + "' onclick=\"" + onClick + "\">";
                }
                else
                {
                    strModule += "<a class=\"active\" href=\"#\" id='" + moduleID + "' onclick=\"clickMenu('" + moduleID + "')\"  hreflang=\"" + moduleURL + "\">";
                }
                strModule += "<i class='" + imageUrl + "'></i>";
                strModule += "<span class='hidden-xs'>" + moduleName + "</span>";
                strModule += "</a>";
                strModule += "</li>";
            }
            else
            {
                strModule += "<li class=\"dropdown\">";
                strModule += "<a class=\"dropdown-toggle\" href=\"#\" id='" + moduleID + "'>";
                strModule += "<i class='" + imageUrl + "'></i>";
                strModule += "<span class='hidden-xs'>" + moduleName + "</span>";
                strModule += "</a>";
                strModule += submenu;
                strModule += "</li>";

            }

        }

    }

    /// <summary>
    /// 创建二级菜单
    /// </summary>
    private string CreateMenu(DataRow[] drAll)
    {
        string strMenu = "";

        if (drAll.Length > 0)
        {
            strMenu += "<ul class=\"dropdown-menu\">";
            foreach (DataRow dr in drAll)
            {
                #region 判断权限

                if (!IsUserRight(dr["ModuleID"].ToString()))
                {
                    continue;
                }

                #endregion 判断权限

                string moduleID = dr["ModuleID"].ToString();
                string moduleName = dr["ModuleName"].ToString();
                string moduleURL = dr["ModuleURL"].ToString();
                string onClick = dr["OnClick"].ToString();

                if (moduleURL == "")
                {
                    strMenu += "<li><a  class=\"ajax-link\" style=\"padding-left:60px;\" href=\"#\" id='" + moduleID + "' onclick=\"" + onClick + "\">" + moduleName + "</a></li>";
                }
                else
                {
                    strMenu += "<li><a  class=\"ajax-link\" style=\"padding-left:60px;\"  href=\"#\" id='" + moduleID + "' onclick=\"clickMenu('" + moduleID + "')\"  hreflang=\"" + moduleURL + "\">" + moduleName + "</a></li>";
                }
            }
            strMenu += "</ul>";
        }
        return strMenu;
    }
}