using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.SingleTable;
using System.Data;
using System.Text;

public partial class SingleTable_SingleTableChart : AudiPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["config_Id"] != null)
            {
                string strSql = GetDataSql();
                if (strSql != "")
                {
                    SQLHelper sqlH = new SQLHelper();
                    DataSet ds = sqlH.ExecuteSQLDataSet(strSql);

                    hfData.Value = HighCharts(ds);
                }
            }

        }
    }

    private string GetDataSql()
    {
        string config_id = Request.QueryString["config_Id"].ToString();
        SQLHelper sqlh = new SQLHelper();
        string strSql = @"select ID,CONFIG_ID,Chart_Style,Chart_Type,Chart_Title,Chart_Table,
                            X_Axis,Y_Axis,Y_Axis_Sql,Y_Value,Y_Dimension,
                            SYSBack1,SYSBack2,SYSBack3 from T_Sys_ConfigChart";
        strSql += " where config_id = '" + config_id + "'";
        DataSet ds = sqlh.ExecuteSQLDataSet(strSql);

        if (ds.Tables[0].Rows.Count == 0)
        {
            return "";
        }
        hfStyle.Value = ds.Tables[0].Rows[0]["Chart_Style"].ToString();
        hfType.Value = ds.Tables[0].Rows[0]["Chart_Type"].ToString();
        hfTitle.Value = ds.Tables[0].Rows[0]["Chart_Title"].ToString();
        hfDimension.Value = ds.Tables[0].Rows[0]["Y_Dimension"].ToString();

        string strWhere = "";
        if (Session["Query"] != null)
        {
            strWhere += Session["Query"].ToString();
        }

        string maxSql = "select X,";
        string mainSql = "";
        //单线图
        if (ds.Tables[0].Rows[0]["Y_Axis_Sql"].ToString() == "")
        {
            maxSql = "select " + ds.Tables[0].Rows[0]["X_Axis"].ToString() + " as X,"
                    + ds.Tables[0].Rows[0]["Y_Value"].ToString() + " as "
                    + ds.Tables[0].Rows[0]["Y_Axis"].ToString() + " from "
                    + ds.Tables[0].Rows[0]["Chart_Table"].ToString()
                    + " where 1=1 " + strWhere + " group by "
                    + ds.Tables[0].Rows[0]["X_Axis"].ToString();
        }
        //多线图
        else
        {
            mainSql = "select " + ds.Tables[0].Rows[0]["X_Axis"].ToString() + " as X,"
                    + ds.Tables[0].Rows[0]["Y_Axis"].ToString() + " as YType,"
                    + ds.Tables[0].Rows[0]["Y_Value"].ToString() + " as Y from "
                    + ds.Tables[0].Rows[0]["Chart_Table"].ToString()
                    + " where 1=1 " + strWhere + " group by "
                    + ds.Tables[0].Rows[0]["X_Axis"].ToString() + ","
                    + ds.Tables[0].Rows[0]["Y_Axis"].ToString();

            if (ds.Tables[0].Rows[0]["Y_Value"].ToString().IndexOf("count") == -1)
            {
                mainSql += "," + ds.Tables[0].Rows[0]["Y_Value"].ToString();
            }
            DataSet dsYAxis = sqlh.ExecuteSQLDataSet(ds.Tables[0].Rows[0]["Y_Axis_Sql"].ToString());
            string code = "";
            string name = "";
            for (int i = 0; i < dsYAxis.Tables[0].Rows.Count; i++)
            {
                code = dsYAxis.Tables[0].Rows[i]["code"].ToString();
                name = dsYAxis.Tables[0].Rows[i]["name"].ToString();
                maxSql += " max(case YType when '" + code + "' then Y end) as '" + name + "'";
                if (i + 1 < dsYAxis.Tables[0].Rows.Count)
                {
                    maxSql += ",";
                }
                else
                {
                    maxSql += " from (" + mainSql + ")a group by X";
                }
            }
        }
        return maxSql;
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


}