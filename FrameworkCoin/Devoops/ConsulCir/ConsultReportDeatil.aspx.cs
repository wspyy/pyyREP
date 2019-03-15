using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.SingleTable;
using Bll.BusinessFun;
using Model;
using Microsoft.Reporting.WebForms;
using System.Text;
using System.Configuration;

public partial class Devoops_ConsulCir_ConsultReportDeatil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (null != Request.QueryString["date"] && "" != Request.QueryString["date"])
            {
                string selecDate = Request.QueryString["date"].ToString();
                string code=  InitReportTitle(selecDate);
                LoadReport(Convert.ToInt32(code),selecDate);
                LoadPlloReport(code);
                bindReportPublish(code);
            }
            else
            {
                LoadReport(0,"");
            }
        }
    }
  
    private void LoadReport(int code,string selecDate)
    {
           ConsultReportDal bill = new ConsultReportDal();
            DataTable dt = bill.GetConsultReportData(code);
            if (dt == null)
            {
                return;
            }
            this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
            this.ReportViewer1.PageCountMode = PageCountMode.Actual;
            //报表参数设置
            this.ReportViewer1.LocalReport.ReportPath = "Devoops/ConsulCir/ConsulCirReport.rdlc";
            ReportParameter rpYearDate = new ReportParameter("rpYearDate", Convert.ToDateTime(selecDate).ToString("yyyy年MM月dd日"));
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rpYearDate });
            ReportViewer1.LocalReport.DisplayName =ConfigurationManager.AppSettings["CityName"].ToString() + "环境空气质量预测预报会商报告" + Convert.ToDateTime(selecDate).ToString("yyyy年MM月dd日");
            //报表数据设置
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            this.ReportViewer1.DataBind();
        }

    /// <summary>
    /// 初始化日报标题
    /// </summary>
    private string InitReportTitle(string date) 
    {
        string code = "";
        ConsultReportDal bill = new ConsultReportDal();
        DataTable dt = bill.GetReportTitle(date);
         StringBuilder strb = new StringBuilder();
        if (dt != null) {
           
            int rowcount = 0;
            foreach (DataRow row in dt.Rows) 
            {
                if (rowcount == 0)
                {
                    strb.Append("<li class='mli' name='" + row[1].ToString() + "'>" + row[0].ToString() + "</li>");
                    hid_code.Value=code = row[1].ToString();
                }
                else {
                    strb.Append("<li name='" + row[1].ToString() + "'>" + row[0].ToString() + "</li>");
                }
                rowcount++;
            }
        }
        olID.InnerHtml = strb.ToString();
        return code;
    }

    private void LoadPlloReport(string code) {

        DataTable dt = new Bll.BusinessFun.ConsultReportDal().GetPollByCode(code);
        if (dt != null)
        {
            this.ReportViewer2.ProcessingMode = ProcessingMode.Local;
            this.ReportViewer2.PageCountMode = PageCountMode.Actual;
            //报表参数设置
            this.ReportViewer2.LocalReport.ReportPath = "Devoops/ConsulCir/pollutionDayliy.rdlc";
            ReportParameter rpYearDate = new ReportParameter("rpYearDate", Convert.ToDateTime(DateTime.Now).ToString("yyyy 年 MM 月 dd 日"));
            ReportParameter poll = new ReportParameter("poll", dt == null ? "" : dt.Rows[0]["Conclusions"].ToString());
            ReportParameter op = new ReportParameter("op", dt == null ? "" : dt.Rows[0]["Opinion"].ToString());
            this.ReportViewer2.LocalReport.EnableExternalImages = true;//设定EnableExternalImages属性为TRUE
            this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rpYearDate, poll, op });            
            ReportViewer2.LocalReport.DisplayName =ConfigurationManager.AppSettings["CityName"].ToString() + "重污染天气预报会商意见" + DateTime.Now.ToString("yyyy年MM月dd日");
            //报表数据设置
            this.ReportViewer2.LocalReport.DataSources.Clear();
            this.ReportViewer2.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", new DataTable()));
            this.ReportViewer2.DataBind();
        }
    }

    private void bindReportPublish(string code)
    {
        DataTable dt = new Bll.BusinessFun.ConsultReportDal().getpollPublishBycode(code);
        if (dt != null)
        {
            DataTable dt2 = new DataTable();
            this.ReportViewer3.ProcessingMode = ProcessingMode.Local;
            this.ReportViewer3.PageCountMode = PageCountMode.Actual;
            //报表参数设置
            this.ReportViewer3.LocalReport.ReportPath = "Devoops/ConsulCir/AirPublishReport.rdlc";
            ReportParameter rpYearDate = new ReportParameter("date", Convert.ToDateTime(DateTime.Now).ToString("yyyy 年 MM 月 dd 日"));
            ReportParameter poll = new ReportParameter("poll", dt.Rows[0]["Basiscontent"].ToString());
            ReportParameter op = new ReportParameter("op", dt.Rows[0]["maincontent"].ToString());
            ReportParameter plevel = new ReportParameter("level", dt.Rows[0]["forecastlevel"].ToString());
            this.ReportViewer3.LocalReport.SetParameters(new ReportParameter[] { rpYearDate, poll, op, plevel });
            ReportViewer3.LocalReport.DisplayName = ConfigurationManager.AppSettings["CityName"].ToString()+"重污染天气预警信息发布（解除）审批表" + DateTime.Now.ToString("yyyy年MM月dd日");
            //报表数据设置
            this.ReportViewer3.LocalReport.DataSources.Clear();
            this.ReportViewer3.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt2));
            this.ReportViewer3.DataBind();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        LoadReport(Convert.ToInt32(hid_code.Value),Request.QueryString["date"].ToString());
        LoadPlloReport(hid_code.Value);
        bindReportPublish(hid_code.Value);
    }
}