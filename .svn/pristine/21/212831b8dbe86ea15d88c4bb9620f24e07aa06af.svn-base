using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;



public partial class Devoops_ConsulCir_PollutionDayliyReport : System.Web.UI.Page
{
    public string userName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
           // string level= Request["level"] == null ? "" : Request["level"];
            userName = Session["userName"].ToString();
            string ReportCode = Request["ReportCode"] == null ? "" : Request["ReportCode"];
            //string level = Request["level"] == null ? "" : Request["level"];
            string level="";
            DataTable dt = new Bll.BusinessFun.ConsultReportDal().getpollPublish(ReportCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                txt_basis.Text = dt == null ? "" : dt.Rows[0]["Basiscontent"].ToString();
                txt_main.Text = dt == null ? "" : dt.Rows[0]["maincontent"].ToString();                
                lbl_pulish.Text = dt == null ? "" : Convert.ToDateTime(dt.Rows[0]["ReportTime"]).ToString("yyyy 年 MM 月 dd 日");
                level = dt==null ? "" : dt.Rows[0]["YJColor"].ToString();
                lbl_level.Text = GetLevelColor(level) + "【" + level + "】";
            }
            else if (dt == null)
            {
                txt_basis.Text = null;
                txt_main.Text = null;                
                lbl_pulish.Text = null;
                lbl_level.Text = null;
            }
            
           // lbl_level.Text = dt == null ? "" : dt.Rows[0]["YJDJ"].ToString();
            //if (Session["userName"] != null)
            //{
            //    labelUser.Text = Session["userName"].ToString();
            //}
            //else
            //{
            //    Response.Write("<script>top.window.location.href = '../Login.aspx';</script>");
            //}
            //lbl_level.Text = level + "【" + GetLevelColor(level) + "】";
            

        }
    }
    private string GetLevelColor(string level)
    {
        if (string.IsNullOrEmpty(level)) return null;
        string level_color = null;
        switch (level)
        {
            /*case "一级":
                color= "红色";
                break;
            case "二级":
                color = "橙色";
                break;
            case "三级":
                color = "黄色";
                break;
            case "四级":
                color = "蓝色";
                break;*/
            case "红色":
                level_color = "一级";
                break;
            case "橙色":
                level_color = "二级";
                break;
            case "黄色":
                level_color = "三级";
                break;
            case "蓝色":
                level_color = "四级";
                break;
        }
        return level_color;
    }    
}