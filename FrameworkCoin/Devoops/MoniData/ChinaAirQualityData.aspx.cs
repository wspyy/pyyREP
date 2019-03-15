using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.BusinessFun;
using System.Data;


public partial class Devoops_MoniData_ChinaAirQualityData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPoll(ddl_day, "");
        }
    }
    BaseInfo bll = new BaseInfo();
    private void BindPoll(DropDownList ddl, string selected)
    {
        //dt = bll.GetCityList();
        DataSet ds = bll.GetCityList();//new DataTable();
        DataTable dt = ds.Tables[0];
        int length = dt.Rows.Count;
        string[] array = new string[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = dt.Rows[i]["CityName"].ToString();
        }
        //string[] array = { "CO", "SO2", "NO2", "O3", "PM25", "PM10" };
        ddl.Items.Clear();
        foreach (var r in array)
        {
            ListItem item = new ListItem();
            item.Text = r;
            item.Value = r;
            if ((r).Equals(selected))
                item.Selected = true;
            else
                item.Selected = false;
            ddl.Items.Add(item);
        }
    }  
    protected void Button1_Click(object sender, EventArgs e)
    {
        string begindate = StartTime.Value;
        string enddate = EndTime.Value;
        string strCheckID = CheckGroup.Value;
        int rowcount = 0;
        int countPage = 0;
        DataTable dt = bll.GetCityCheckData(begindate, enddate, strCheckID, 1, 10000, ref rowcount, ref countPage);
        if (dt != null)
        {
            ExportToExcel(dt, "全国空气质量");
        }
    }
    /// <summary>
    /// 执行导出
    /// </summary>
    /// <param name="ds">要导出的DataSet</param>
    /// <param name="strExcelFileName">要导出的文件名</param>
    public void ExportToExcel(DataTable dtSource, string fileName)
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

        resp.Write("<table class='table'><thead><tr class=\"thcss\"><th rowspan=\"2\">城市名称</th><th rowspan=\"2\">监测时间</th><th colspan=\"2\">二氧化硫(SO<sub>2</sub>)</th><th colspan=\"2\">二氧化氮(NO<sub>2</sub>)</th><th colspan=\"2\">一氧化碳(CO)</th><th colspan=\"2\">臭氧(O<sub>3</sub>)</th><th colspan=\"2\">PM<sub>10</sub></th><th colspan=\"2\">PM<sub>2.5</sub></th><th rowspan=\"2\">空气质量指数<br>(AQI)</th><th rowspan=\"2\">首要污染物</th><th rowspan=\"2\">空气质量<br>指数级别</th></tr><tr class=\"thcss\"><th>浓度</th><th>分指数</th><th>浓度</th><th>分指数</th><th>浓度</th><th>浓度</th><th>浓度</th><th>分指数</th><th>浓度</th><th>分指数</th><th>浓度</th><th>分指数</th></tr></thead>");

        foreach (DataRow tmpRow in dtSource.Rows)
        {
            resp.Write("<tr>");
            resp.Write("<td>" + tmpRow["CityName"] + "</td><td>" + tmpRow["MonitorTime"].ToString().Substring(0, 10) + "</td>"
                + "<td>" + tmpRow["SO2"] + "</td><td>" + tmpRow["SO2AQI"] + "</td>"
                + "<td>" + tmpRow["NO2"] + "</td><td>" + tmpRow["NO2AQI"] + "</td><td>" + tmpRow["CO"] + "</td>"
                + "<td>" + tmpRow["COAQI"] + "</td><td>" + tmpRow["O3"] + "</td><td>" + tmpRow["O3AQI"] + "</td>"
                + "<td>" + tmpRow["PM10"] + "</td><td>" + tmpRow["PM10AQI"] + "</td><td>" + tmpRow["PM25"] + "</td>"
                + "<td>" + tmpRow["PM25AQI"] + "</td><td>" + tmpRow["AQI"] + "</td><td>" + tmpRow["FirstP"] + "</td>"
                + "<td>" + tmpRow["AirLevel"] + "</td>");
            resp.Write("</tr>");
        }
        resp.Write("<table>");

        resp.Flush();
        resp.End();
    }
}