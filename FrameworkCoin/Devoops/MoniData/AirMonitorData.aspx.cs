using Bll.BusinessFun;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//连接odbc
using System.Text;
using System.Data.Odbc;
using System.IO;
//OleDb
using System.Data.OleDb;
//正则表达式的命名空间
using System.Text.RegularExpressions;

public partial class Devoops_MoniData_AirMonitorData : System.Web.UI.Page
{
    public int Flag;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Flag = Convert.ToInt32(Request["flag"]);
        }
    }
    Bll.BusinessFun.AirMonitor bll = new Bll.BusinessFun.AirMonitor();
    protected void Button1_Click(object sender, EventArgs e)
    {
        string beign = BeginTime.Value;
        string end = EndTime.Value;
        string station = Station.Value;
        string item = Items.Value;
        string type = Type.Value;
        if (station.Length > 0) station = station.Substring(0, station.Length - 1);
        if (item.Length > 0)
        {
            if (type == "yoy")
                item = item.Split(',')[0];
            else
                item = item.Substring(0, item.Length - 1);
        }
        else
        {
            if (type == "yoy")
                item = "AQI";
        }
        DataSet ds = bll.GetMonitorData(type, beign, end, station, item) as DataSet;
        DataTable dt = ds.Tables[0];
        if (dt != null)
        {
            ExportToExcel(dt, "空气质量监测数据", item,type);

        }
    }

    public void ExportToExcel(DataTable dtSource, string fileName, string item,string type)
    {
        HttpResponse resp = System.Web.HttpContext.Current.Response;
        resp.Charset = "utf-8";
        resp.Clear();
        string filename = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
        resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
        resp.ContentEncoding = System.Text.Encoding.UTF8;

        resp.ContentType = "application/ms-excel";
        string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" + "<style> .table{ font: 9pt Tahoma, Verdana; color: #000000; text-align:center;  background-color:#8ECBE8;  }.table td{text-align:center;height:21px;background-color:#EFF6FF;}.table th{ font: 9pt Tahoma, Verdana; color: #000000; font-weight: bold; background-color: #8ECBEA; height:25px;  text-align:center; padding-left:10px;}</style>";
        resp.Write(style);
        string[] items = item.Split(',');
        if (type == "yoy") {
            resp.Write("<table class='table'><thead><tr class=\"thcss\"><th >监测站点</th>");
            foreach (DataColumn column in dtSource.Columns)
            {
                if (column.ColumnName != "sc")
                {
                    resp.Write("<th>" + column.ColumnName + "</th>");
                }
            }
            resp.Write("</tr></thead>");
            foreach (DataRow tmpRow in dtSource.Rows)
            {
                resp.Write("<tr>");
                resp.Write("<td>" + tmpRow["sc"] + "</td>");
                foreach (DataColumn column in dtSource.Columns)
                {
                    if (column.ColumnName != "sc")
                    {
                        resp.Write("<td>" + tmpRow[column.ColumnName].ToString() + "</td>");
                    }
                }
                resp.Write("<td></tr>");
            }        
        }
        else if (type == "quarter")
        {
            resp.Write("<table class='table'><thead><tr class=\"thcss\"><th >监测站点</th><th>监测时间</th>");
            foreach (string im in items)
            {
                resp.Write("<th>" + im + "</th>");
            }
            resp.Write("</tr></thead>");
            foreach (DataRow tmpRow in dtSource.Rows)
            {
                resp.Write("<tr>");
                resp.Write("<td>" + tmpRow["StationName"] + "</td><td>" + tmpRow["JD"].ToString() + "</td>");
                foreach (string it in items)
                {
                    resp.Write("<td>" + tmpRow[it] + "</td>");
                }
                resp.Write("<td></tr>");
            }        
        }
        else if (type == "year")
        {
            resp.Write("<table class='table'><thead><tr class=\"thcss\"><th >监测站点</th><th>监测时间</th>");
            foreach (string im in items)
            {
                resp.Write("<th>" + im + "</th>");
            }
            resp.Write("</tr></thead>");
            foreach (DataRow tmpRow in dtSource.Rows)
            {
                resp.Write("<tr>");
                resp.Write("<td>" + tmpRow["StationName"] + "</td><td>" + tmpRow["YY"].ToString() + "</td>");
                foreach (string it in items)
                {
                    resp.Write("<td>" + tmpRow[it] + "</td>");
                }
                resp.Write("<td></tr>");
            }
        }
        else
        {
            resp.Write("<table class='table'><thead><tr class=\"thcss\"><th >监测站点</th><th>监测时间</th>");
            foreach (string im in items)
            {
                resp.Write("<th>" + im + "</th>");
            }
            resp.Write("</tr></thead>");
            foreach (DataRow tmpRow in dtSource.Rows)
            {
                resp.Write("<tr>");
                resp.Write("<td>" + tmpRow["StationName"] + "</td><td>" + tmpRow["MoniDate"].ToString() + "</td>");
                foreach (string it in items)
                {
                    resp.Write("<td>" + tmpRow[it] + "</td>");
                }
                resp.Write("<td></tr>");
            }
        }
                              
              
        resp.Flush();
        resp.End();
    }


}
