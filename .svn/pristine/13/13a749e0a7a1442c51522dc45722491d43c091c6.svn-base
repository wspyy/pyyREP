using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;

/// <summary>
/// 模板页面
/// </summary>
public class AudiPage : System.Web.UI.Page
{
    /// <summary>
    /// 页面初始加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreInit(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 获得Session值
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public string getSession(string sessionId)
    {
        if (Session[sessionId] != null)
        {
            return Session[sessionId].ToString();
        }
        else
        {
            //session过期后页面提示
            //Response.Write("<script type='text/javascript'>sessionTimeOut();</script>");
            return "";
        }
    }

    /// <summary>
    /// 当前城市名称
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public string getCurCityName()
    {
        if (ConfigurationManager.AppSettings["CityName"] != null)
        {
            return ConfigurationManager.AppSettings["CityName"].ToString();
        }
        else
        {
            return "";
        }
    }


    /// <summary>
    /// 判断是否具有该模块权限
    /// </summary>
    /// <param name="ModuleID">模块编号</param>
    /// <returns>是、否</returns>
    public bool IsUserRight(string ModuleID)
    {
        string userRight = "";
        //从session中获取该用户的模块权限
        if (Session["userRight"] != null)
        {
            userRight = Session["userRight"].ToString();
        }
        string modelidStr = ModuleID + "|";
        if (userRight.IndexOf(modelidStr) != -1)
        {
            //有权限
            return true;
        }
        else
        {
            //无权限
            return false;
        }
    }
}

