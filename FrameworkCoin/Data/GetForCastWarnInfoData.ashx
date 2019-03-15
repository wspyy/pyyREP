<%@ WebHandler Language="C#" Class="GetForCastWarnInfoData" %>

using System;
using System.Web;
using System.Data;
using Bll.BusinessFun;
using System.Text;
using Model;
using System.Collections.Generic;
public class GetForCastWarnInfoData : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {


        if (context.Request["mode"] != null)
        {
            string mode = context.Request["mode"].ToString();
            switch (mode)
            {
                case "QryWarnInfo":
                    QueryData(context);
                    break;
                case "region":
                    GetRegionData(context);
                    break;
                case "QryWarnDetail":
                    GetWarnDetailData(context);
                    break;
                case "QryWarnDetailSec":
                    GetWarnDetailDataSec(context);
                    break;
                case "adddetail":
                    InsertData(context);
                    break;
                case "DEL":
                    DeleteData(context);
                    break;
                case "UP":
                    UpdataData(context);
                    break;
               
            }
        }
    }
 
    ForCastWarnInfoDal bill = new ForCastWarnInfoDal();
   
    public void QueryData(HttpContext context)
    {
        int countPage=0;
        int rowcount = 0;
        string begindate =Convert.ToDateTime(context.Request["begin"].ToString()).ToString("yyyy-MM-dd");
        string enddate =Convert.ToDateTime(context.Request["end"].ToString()).ToString("yyyy-MM-dd");
        int page = Convert.ToInt32(context.Request["page"].ToString());
        int rows = Convert.ToInt32(context.Request["rows"].ToString());
        DataTable ds = bill.GetWarnInfoData(begindate, enddate, page, rows, ref rowcount, ref countPage);
        int RowCount = 0;
        if (!(ds == null || ds.Rows.Count <= 0))
        {
            RowCount = ds.Rows.Count;
        }
        StringBuilder sb = new StringBuilder();
        if (ds!=null)
        {
            sb.Append("{HasData:\"yes\",CountPage:\""+countPage+"\",");
            sb.Append("Data:[");
            int i = 0;

            foreach (DataRow dr in ds.Rows)
            {
                i++;
                sb.Append("{FutureDate:\"" + Convert.ToDateTime(dr["FutureDate"]).ToString("yyyy-MM-dd"));
                sb.Append("\",AQI:\"" + dr["AQI"]);
                sb.Append("\",Category:\"" + dr["Category"]);
                sb.Append("\",QualityIndex:\"" + dr["Grade"]);
                sb.Append("\",ColorCode:\"" + dr["ColorCode"]);
                sb.Append("\",ReportCode:\"" + dr["ReportCode"]);
                sb.Append("\",ColorName:\"" + dr["ColorName"]);
                sb.Append("\",RadiusColor:\""+dr["RadiusColor"]);
                sb.Append("\"}");
                if (i < ds.Rows.Count)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]}");
        }
        else
        {
            sb.Append(" {HasData:\"no\"}");
        }
        context.Response.Write(sb.ToString());


    }
    
    public void GetRegionData(HttpContext context)
    {
        string strlidata = "";
        DataTable data = bill.GetregionData().Tables[0] ;
        if (data.Rows.Count > 0)
        {
            for (int i = 0; i < data.Rows.Count; i++)
            {
                strlidata += "<li value=\"" + data.Rows[i]["StationCode"].ToString() + "\" onclick=\"onclicli(this)\">" + data.Rows[i]["StationName"].ToString() + "</li>";
                 
            }
            
        }
        context.Response.Write(strlidata.ToString());
     
    }
    public void GetWarnDetailData(HttpContext context)
    {
        string regionName = "";
        if (context.Request["regionName"] != null)
        {
            regionName = context.Request["regionName"].ToString();
        }
        string datesec = context.Request["datesec"].ToString();
        string date=context.Request["date"].ToString();
        string strF = "";
        string strS = "";
        int hourF =Convert.ToInt32(context.Request["hourF"].ToString());
        int hourS =Convert.ToInt32(context.Request["hourS"].ToString());
        if (hourF < 10)
        {
            strF = "0" + hourF + ":00:00";
        }
            
        else
        {
            strF = Convert.ToString(hourF) + ":00:00";
        }
        if (hourS < 10)
        {
            strS = "0" + hourS + ":00:00";
        }
      
        else
        {
            strS = Convert.ToString(hourS) + ":00:00";
        }
        
        string strdate1 = date + " " + strF;
        string datesecstr = datesec + " " + strS;
        string datetime1 = Convert.ToDateTime(strdate1).ToString("yyyy-MM-dd HH:mm:ss");
        string datetime2 = Convert.ToDateTime(datesecstr).ToString("yyyy-MM-dd HH:mm:ss");
        int countPage = 0;
        int rowcount = 0;
      
        int page = Convert.ToInt32(context.Request["page"].ToString());
        int rows = Convert.ToInt32(context.Request["rows"].ToString());
       
            DataTable ds = bill.GetWarnDetailData(regionName, datetime1, datetime2, page, rows, ref rowcount, ref countPage);
      
        int RowCount = 0;
        if (!(ds == null || ds.Rows.Count <= 0))
        {
            RowCount = ds.Rows.Count;
        }
        StringBuilder sb = new StringBuilder();
        if (ds!=null)
        {
            sb.Append("{HasData:\"yes\",CountPageSec:\"" + countPage + "\",");
            sb.Append("Data:[");
            int i = 0;

            foreach (DataRow dr in ds.Rows)
            {
                i++;
                sb.Append("{MonitorTime:\"" + Convert.ToDateTime(dr["MonitorTime"]).ToString("yyyy-MM-dd hh"));
                sb.Append("\",ForecastTime:\"" + Convert.ToDateTime(dr["ForecastTime"]).ToString("yyyy-MM-dd hh"));
                sb.Append("\",StationName:\"" + dr["StationName"]);
                sb.Append("\",AirLevelDes:\"" + dr["AirLevelDes"]);
                sb.Append("\",AirColor:\"" + dr["AirColor"]);
                sb.Append("\",FirstP:\"" + dr["FirstP"]);
                sb.Append("\",AQI:\"" + dr["AQI"]);
                sb.Append("\",SO2:\"" + dr["SO2"]);
                sb.Append("\",NO2:\"" + dr["NO2"]);
                sb.Append("\",CO:\"" + dr["CO"]);
                sb.Append("\",O3:\"" + dr["O3"]);
                sb.Append("\",PM25:\"" + dr["PM25"]);
                sb.Append("\",PM10:\"" + dr["PM10"]);
         
                sb.Append("\"}");
                if (i < ds.Rows.Count)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]}");
        }
        else
        {
            sb.Append(" {HasData:\"no\"}");
        }
       
        context.Response.Write(sb.ToString());
         

    }
    public void GetWarnDetailDataSec(HttpContext context)
    {
        string textid = context.Request["id"];
        string textname = context.Request["name"];

        int page = Convert.ToInt32(context.Request["page"]);
        int rows = Convert.ToInt32(context.Request["rows"]);
        DataSet ds = bill.SelectWDetaile();
        int RowCount = 0;
        if (!(ds == null || ds.Tables[0].Rows.Count <= 0))
        {
            RowCount = ds.Tables[0].Rows.Count;
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        sb.Append(" \"total\":\"" + RowCount + "\",");
        sb.Append(" \"rows\":[");
        int i = 0;
        if (!(ds == null || ds.Tables[0].Rows.Count <= 0))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                i++;
                sb.Append("{\"id\":\"" + dr["ID"] + "\",\"StationCode\":\"" + dr["StationCode"] + "\",\"AQI\":\"");
                sb.Append("" + dr["AQI"] + "\",\"QualityLevelID\":\"" + dr["QualityLevelID"] + "\",\"FirstP\":\"" + dr["FirstP"] + "\",");
                sb.Append("\"Visibility\":\"" + dr["Visibility"] + "\",\"SustainTime\":\"" + dr["SustainTime"] +"\"}");
                if (i < ds.Tables[0].Rows.Count)
                {
                    sb.Append(",");
                }
            }
        }
        sb.Append("]}");
        context.Response.Write(sb.ToString());
    }
   
    //public void GetWarnDetailData(HttpContext context)
    //{
    //    //string textid = context.Request["id"];
    //    //string textname = context.Request["name"];

    //    //int page = Convert.ToInt32(context.Request["page"]);
    //    //int rows = Convert.ToInt32(context.Request["rows"]);

    //    //DataSet ds = bill.GetWarnDetailData();
    //    //int RowCount = 0;
    //    //if (!(ds == null || ds.Tables[0].Rows.Count <= 0))
    //    //{
    //    //    RowCount = ds.Tables[0].Rows.Count;
    //    //}
    //    //StringBuilder sb = new StringBuilder();
    //    //sb.Append("{");
    //    //sb.Append(" \"total\":\"" + RowCount + "\",");
    //    //sb.Append(" \"rows\":[");
    //    //int i = 0;
    //    //if (!(ds == null || ds.Tables[0].Rows.Count <= 0))
    //    //{
    //    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    //    {
    //    //        i++;
    //    //        sb.Append("{\"id\":\"" + dr["ID"] + "\",\"StationName\":\"" + dr["StationName"] + "\",\"SO2\":\"");
    //    //        sb.Append("" + dr["SO2"] + "\",\"NO2\":\"" + dr["NO2"] + "\",\"CO\":\"" + dr["CO"] + "\",");
    //    //        sb.Append("\"AirLevelDes\":\"" + dr["AirLevelDes"] + "\",\"AirColor\":\"" + dr["AirColor"] + "\",\"AirLevel\":\"" + dr["AirLevel"] + "\",");
    //    //        sb.Append("\"O3\":\"" + dr["O3"] + "\",\"PM25\":\"" + dr["PM25"] + "\",\"PM10\":\"" + dr["PM10"] + "\",\"AQI\":\"" + dr["AQI"] + "\",\"FirstP\":\"" + dr["FirstP"] + "\"}");
    //    //        if (i < ds.Tables[0].Rows.Count)
    //    //        {
    //    //            sb.Append(",");
    //    //        }
    //    //    }
    //    //}
    //    //sb.Append("]}");
    //    //context.Response.Write(sb.ToString());
        
    //}
     /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="context"></param>
        public void InsertData(HttpContext context)
        {
            //id为GUID
           // string id = Guid.NewGuid().ToString();

            string StationCode = context.Request["StationCode"].ToString();
            string AQI = context.Request["AQI"].ToString();
            int QualityLevelID =Convert.ToInt32(context.Request["QualityLevelID"]);
            string FirstP = context.Request["FirstP"].ToString();
           
            try
            {
             WarnInfoDetailModel model = new WarnInfoDetailModel();          
                model.QualityLevelID=QualityLevelID;
                    model.StationCode=StationCode;
                    model.AQI = AQI;
                 
                    model.FirstP = FirstP;
                int count = bill.InSertWDetaile(model);
                if (count > 0)
                {
                    context.Response.Write(count);
                }
                else
                {
                    context.Response.Write("{\"Error\":\"true\"}");
                }
            }catch(Exception e)
            {
                context.Response.Write("{\"Error\":\"true\"}");
            }
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="context"></param>
        public void DeleteData(HttpContext context)
        {
            int id =Convert.ToInt32(context.Request["id"]);
            int del = bill.DeleteWDtaile(id);
            if (del > 0)
            {
                context.Response.Write(del);
            }
            else
            {
                context.Response.Write("{\"Error\":\"true\"}");
            }
        }

        public void UpdataData(HttpContext context)
        {
            string StationCode = context.Request["StationCode"].ToString();
            string AQI = context.Request["AQI"].ToString();
            int QualityLevelID = Convert.ToInt32(context.Request["QualityLevelID"]);
            string FirstP = context.Request["FirstP"].ToString();
           int id =Convert.ToInt32(context.Request["id"].ToString());
            int count = 0;
          
            try
            {
                WarnInfoDetailModel model = new WarnInfoDetailModel();
                model.id = id;
                model.QualityLevelID = QualityLevelID;
                model.StationCode = StationCode;
                model.AQI = AQI;

                model.FirstP = FirstP;
                count = bill.UpdateWDetaile(model);
                if (count > 0)
                {
                    context.Response.Write(count);
                }
                
            }catch(Exception e)
            {
                context.Response.Write("{\"Error\":\"true\"}");
            }
        }
    

    public bool IsReusable {
        get {
            return false;
        }
    }

}