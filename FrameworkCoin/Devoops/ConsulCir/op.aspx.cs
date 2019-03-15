﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;


public partial class Devoops_ConsulCir_op : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindReport();

        }
    }


    private void bindReport()
    {
        string ReportCode = Request["ReportCode"];
        DataTable dt = new Bll.BusinessFun.ConsultReportDal().getpollPublish(ReportCode);
        
        //var date = Request["date"];
        // = date.replace("-", "/");
        DataTable dt2 = new DataTable();
        this.ReportViewer2.ProcessingMode = ProcessingMode.Local;
        this.ReportViewer2.PageCountMode = PageCountMode.Actual;
        //报表参数设置
        this.ReportViewer2.LocalReport.ReportPath = "Devoops/ConsulCir/AirPublishReport.rdlc";
        string level = dt == null ? "" : dt.Rows[0]["YJColor"].ToString();
        string forecastlevel = GetLevelColor(level) + "【" + level + "】";
        ReportParameter plevel = new ReportParameter("level", forecastlevel.ToString());//dt.Rows[0]["forecastlevel"]
        ReportParameter rpYearDate = new ReportParameter("date", dt == null ? "" : Convert.ToDateTime(dt.Rows[0]["ReportTime"]).ToString("yyyy 年 MM 月 dd 日"));
        //dt == null ? "" : Convert.ToDateTime(dt.Rows[0]["ReportTime"]).ToString("yyyy 年 MM 月 dd 日")
        ReportParameter poll = new ReportParameter("poll", dt == null ? "" : dt.Rows[0]["Basiscontent"].ToString());
        ReportParameter op = new ReportParameter("op", dt == null ? "" : dt.Rows[0]["maincontent"].ToString());                       
        
        this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rpYearDate, poll, op, plevel });//,plevel
        ReportViewer2.LocalReport.DisplayName = "朔州市重污染天气预警信息发布（解除）审批表" + DateTime.Now.ToString("yyyy年MM月dd日");
        //报表数据设置
        this.ReportViewer2.LocalReport.DataSources.Clear();
        this.ReportViewer2.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt2));
        this.ReportViewer2.DataBind();
    }
    private string GetLevelColor(string level)
    {
        if (string.IsNullOrEmpty(level)) return null;
        string level_color = null;
        switch (level)
        {           
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