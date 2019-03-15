<%@ WebHandler Language="C#" Class="GetChartData" %>

using System;
using System.Web;
using System.Text;
using System.SingleTable;
using System.SingleTable.PublicMethod;

public class GetChartData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string strSql = "";
        string strChart = "";
        if (context.Request["sql"] != null)
        {
            strSql = context.Request["sql"].ToString();
        }

        SQLHelper sqlH = new SQLHelper();
        System.Data.DataSet ds = sqlH.ExecuteSQLDataSet(strSql);

        strChart = HighCharts(ds);

        context.Response.ContentType = "text/html";
        context.Response.Write(strChart);
    }

    /// <summary>
    /// HighCharts，DataSet To JSON
    /// </summary>
    /// <param name="ds">查询数据集</param>
    /// <returns></returns>
    private string HighCharts(System.Data.DataSet ds)
    {
        StringBuilder json = new StringBuilder();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            json.Append("{\"ChartTime\":[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                json.Append("\"" + ds.Tables[0].Rows[i]["X"].ToString() + "\",");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("],\"ChartValue\":[");
            for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
            {
                json.Append("{\"name\":\"" + ds.Tables[0].Columns[j].ColumnName + "\",");
                json.Append("\"data\":[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string yValue = "0";
                    if (ds.Tables[0].Rows[i][j].ToString() != "")
                    {
                        yValue = ds.Tables[0].Rows[i][j].ToString();
                    }
                    json.Append(yValue + ",");
                }
                json.Remove(json.Length - 1, 1);
                json.Append("]},");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]}");

        }
        return json.ToString();
    }

    

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}