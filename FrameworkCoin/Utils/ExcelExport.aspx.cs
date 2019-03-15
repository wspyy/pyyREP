using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Utils_ExcelExport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        test1();
    }


    private void test1()
    {
        HttpResponse resp = System.Web.HttpContext.Current.Response;
        resp.Charset = "utf-8";
        resp.Clear();
        string filename = "统计贴标报表_" + DateTime.Now.ToString("yyyyMMddHHmmss");
        resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
        resp.ContentEncoding = System.Text.Encoding.UTF8;

        resp.ContentType = "application/ms-excel";
        string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" + "<style> .table{ font: 9pt Tahoma, Verdana; color: #000000; text-align:center;  background-color:#8ECBE8;  }.table td{text-align:center;height:21px;background-color:#EFF6FF;}.table th{ font: 9pt Tahoma, Verdana; color: #000000; font-weight: bold; background-color: #8ECBEA; height:25px;  text-align:center; padding-left:10px;}</style>";
        resp.Write(style);

        resp.Write("<table class='table'><tr><th>姓名</th><th>出生年月</th><th>籍贯</th><th>毕业时间</th></tr>");

        System.Data.DataTable dtSource = new System.Data.DataTable();
        dtSource.TableName = "statistic";
        dtSource.Columns.Add("第一列");
        dtSource.Columns.Add("第二列");
        dtSource.Columns.Add("第三列");
        dtSource.Columns.Add("第四列");

        System.Data.DataRow row = null;
        row = dtSource.NewRow();
        row[0] = "张三";
        row[1] = "1987-09-09";
        row[2] = "河北保定";
        row[3] = "2008年毕业";
        dtSource.Rows.Add(row);

        row = dtSource.NewRow();
        row[0] = "李四";
        row[1] = "1987-09-02";
        row[2] = "湖北武汉";
        row[3] = "2009年毕业";
        dtSource.Rows.Add(row);

        row = dtSource.NewRow();
        row[0] = "王五";
        row[1] = "1987-09-01";
        row[2] = "湖南湘潭";
        row[3] = "2013年毕业";
        dtSource.Rows.Add(row);

        foreach (DataRow tmpRow in dtSource.Rows)
        {
            resp.Write("<tr><td>" + tmpRow[0] + "</td>");
            resp.Write("<td>" + tmpRow[1] + "</td>");
            resp.Write("<td>" + tmpRow[2] + "</td>");
            resp.Write("<td>" + tmpRow[3] + "</td>");
            resp.Write("</tr>");
        }
        resp.Write("<table>");

        resp.Flush();
        resp.End();
    }
}