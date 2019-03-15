using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Devoops_MoniData_AirHistData : System.Web.UI.Page
{

    public string TableContent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            txt_begin.Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            //InitTableContent();
        }
       
    }

    public void InitTableContent() 
    {
         Bll.BusinessFun.AirMonitor bll = new Bll.BusinessFun.AirMonitor();
         DataSet ds= bll.GetAirDayMonitDataByWhere(" and MoniDate=" + Convert.ToDateTime(txt_begin.Value));
         StringBuilder strb = new StringBuilder();
         if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
         {
             DataRowCollection rows=ds.Tables[0].Rows;
             foreach(DataRow row in rows){
                 strb.Append("<tr>");
                 strb.Append("<td>" + row["stationName"].ToString() + "</td>");
                 strb.Append("<td>" + row["SO2"].ToString() + "</td>");
                 strb.Append("<td>" + row["SO2AQI"].ToString() + "</td>");
                 strb.Append("<td>" + row["NO2"].ToString() + "</td>");
                 strb.Append("<td>" + row["NO2AQI"].ToString() + "</td>");
                 strb.Append("<td>" + row["CO"].ToString() + "</td>");
                 strb.Append("<td>" + row["COAQI"].ToString() + "</td>");
                 strb.Append("<td>" + row["O3"].ToString() + "</td>");
                 strb.Append("<td>" + row["O3AQI"].ToString() + "</td>");
                 strb.Append("<td>" + row["PM10"].ToString() + "</td>");
                 strb.Append("<td>" + row["PM10AQI"].ToString() + "</td>");
                 strb.Append("<td>" + row["PM25"].ToString() + "</td>");
                 strb.Append("<td>" + row["PM25AQI"].ToString() + "</td>");
                 strb.Append("<td>" + row["AQI"].ToString() + "</td>");
                 strb.Append("<td>" + row["FirstP"].ToString() + "</td>");
                 strb.Append("<td>" + row["AirLevel"].ToString() + "</td>");
                 strb.Append("<td>" + row["AirLevelDes"].ToString() + "</td>");
                 strb.Append("<td>" + row["AirColor"].ToString() + "</td>");
                 strb.Append("</tr>");
             }
         }
         else {
             strb.Append("<tr><td colspan='18' style='text-align:center;'>暂无相关数据。</td></tr>");
         }
         TableContent = strb.ToString();
    }
}