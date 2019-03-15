﻿<%@ WebHandler Language="C#" Class="DbfUp" %>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;//DataTable
using System.Text.RegularExpressions;//正则表达式

public class DbfUp : IHttpHandler {
    int f = 0;
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        List<string> list = new List<string>() { ".dbf" };
       
        if (context.Request.Files.Count <= 0)
        {
            context.Response.Write("请选择要上传的bdf文件");
            context.Response.Redirect("../Devoops/MoniData/AirMonitorData.aspx?flag=0");
            return;
        }

        HttpPostedFile file = context.Request.Files["dbf"];
        if (!string .IsNullOrEmpty( file.FileName))
        {
            string fileName = Path.GetFileName(file.FileName);//获得文件名包括扩展名
            string fileExt = Path.GetExtension(fileName);//获得扩展名用来限定上传的文件
            if (list.Contains(fileExt))
            {
                
                string path = "../Devoops/Files/" ;
                if (!Directory.Exists(context.Request.MapPath(path)))
                {
                    Directory.CreateDirectory(context.Request.MapPath(path));
                }
                string newPath = path + fileName ;
                file.SaveAs(context.Request.MapPath(newPath));
                context.Response.Write( fileName);
                addToDBbase(fileName, context.Request.MapPath(newPath));
                if (f > 0)
                { 
                    context.Response.Write("<html><body><script>alert(导入成功！)</script><body><html>");
                    context.Response.Redirect("../Devoops/MoniData/AirMonitorData.aspx?flag=1"); 
                }
                else { 
                    context.Response.Write("导入失败！");
                    context.Response.Redirect("../Devoops/MoniData/AirMonitorData.aspx?flag=2"); 
                }
                
            }
            else
            {
                context.Response.Write("只能上传dbf文件");
                context.Response.Redirect("../Devoops/MoniData/AirMonitorData.aspx?flag=-1");
            }
        }
        else
        {
            context.Response.Write("请选择要上传的dbf文件");
            context.Response.Redirect("../Devoops/MoniData/AirMonitorData.aspx?flag=0");
        }
    }

    public DataTable GetDbfDataByODBC(string tableName, string defaultDir)
    {
        
        DbfReader.TDbfTable dbf = new DbfReader.TDbfTable(defaultDir);
        return dbf.Table;
    }
    public void addToDBbase(string tableName, string defaultDir)
    {
        DataTable dt = GetDbfDataByODBC(tableName, defaultDir);
        Bll.SQLHelper sqlH = new Bll.SQLHelper();
        DateTime time = DateTime.Now;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string cityname = dt.Rows[i][1].ToString();//CityName
            string stationname = dt.Rows[i][6].ToString();//stationname
            string stationcode = dt.Rows[i][5].ToString();//stationcode

            string year = dt.Rows[i][2].ToString();
            string month = dt.Rows[i][3].ToString();
            string day = dt.Rows[i][4].ToString();
            time = Convert.ToDateTime(year + "-" + month + "-" + day);

            object s1 = RegexVal(dt.Rows[i][7]);//SO2
            decimal SO2con = (s1 == "null") ? 0 : Convert.ToDecimal(s1) / 1000;//SO2:mg/cm3

            object n1 = RegexVal(dt.Rows[i][8]);//NO2
            decimal NO2con = (n1 == "null") ? 0 : Convert.ToDecimal(n1) / 1000;//NO2:mg/cm3

            object c1 = RegexVal(dt.Rows[i][10]);//CO
            decimal COcon = (c1 == "null") ? 0 : Convert.ToDecimal(c1);//CO:mg/cm3

            /*decimal o1=Convert.ToDecimal(dt.Rows[i][11]);//O3*/
            object o1 = RegexVal(dt.Rows[i][11]);//O3
            decimal O3con = (o1 == "null") ? 0 : Convert.ToDecimal(o1) / 1000;//O3:mg/cm3

            object o8_1 = RegexVal(dt.Rows[i][12]);//O3_8H
            decimal O3_8con = (o8_1 == "null") ? 0 : Convert.ToDecimal(o8_1) / 1000;//O3_8H:mg/cm3

            object p25 = RegexVal(dt.Rows[i][13]);//PM25
            decimal PM25con = (p25 == "null") ? 0 : Convert.ToDecimal(p25) / 1000;//PM25:mg/cm3

            object p10 = RegexVal(dt.Rows[i][9]);//PM10
            decimal PM10con = (p10 == "null") ? 0 : Convert.ToDecimal(p10) / 1000;//PM10:mg/cm3

            string s2 = (s1 == "null") ? "null" : "[dbo].[so22iaqi](" + Convert.ToDecimal(s1) / 1000 + ")";
            //string s2="[dbo].[so22iaqi]("+s1/1000+")";//SO2AQI

            string n2 = (n1 == "null") ? "null" : "[dbo].[no22iaqi](" + Convert.ToDecimal(n1) / 1000 + ")";//NO2AQI
            string c2 = (c1 == "null") ? "null" : "[dbo].[co2iaqi](" + c1 + ")";//COAQI
            string o2 = (o1 == "null") ? "null" : "[dbo].[o32iaqi](" + Convert.ToDecimal(o1) / 1000 + ",1)";//O3AQI,臭氧1小时
            string o8_2 = (o8_1 == "null") ? "null" : "[dbo].[o32iaqi](" + Convert.ToDecimal(o8_1) / 1000 + ",8)";//O3AQI,臭氧8小时
            string p25A = (p25 == "null") ? "null" : "[dbo].[pm252iaqi](" + Convert.ToDecimal(p25) / 1000 + ")";//PM25AQI
            string p10A = (p10 == "null") ? "null" : "[dbo].[pm102iaqi](" + Convert.ToDecimal(p10) / 1000 + ")";//PM10AQI

            string aqi = "[dbo].[GetMaxAQI]([dbo].[so22iaqi](" + SO2con + "),[dbo].[no22iaqi](" + NO2con + "),[dbo].[co2iaqi](" + COcon + "),"
                + "[dbo].[o32iaqi](" + O3con + ",1),[dbo].[pm252iaqi](" + PM25con + "),[dbo].[pm102iaqi](" + PM10con + "))";//AQI
            //string FirstP = "[dbo].[fun_AQI_FristPollutant](" + COcon + "," + SO2con + "," + NO2con + "," + O3con + "," + O3_8con + "," + PM25con + "," + PM10con + ")";//FirstP
            string FirstP = "[dbo].[fun_AQI_FristPollutant](" + c2 + "," + s2 + "," + n2 + "," + o2 + "," + o8_2 + "," + p25A + "," + p10A + ")";//FirstP
            string AirLevel = "[dbo].[fun_AirAQILevel](" + aqi + ")";//AirLevel


            /*//直接插入要导入的数据，没有验证是否会重复插入
            string sql = string.Format("insert into [dbo].[a] (CityName, StationName, StationCode," +
            "MoniDate, SO2AQI, SO2, NO2AQI, NO2, COAQI, CO, O3AQI, O3,O38AQI, O38,PM25AQI,PM25, PM10AQI, PM10,AQI,FirstP,AirLevel)" +
            "values ('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20})", dt.Rows[i][1].ToString(),
            dt.Rows[i][6].ToString(), dt.Rows[i][5].ToString(),
            time, s2, s1, n2, n1, c2, c1, o2, o1, o8_2,o8_1, p25A, p25, p10A, p10, aqi,
            FirstP, AirLevel);*/
            //数据存在，不重复插入
            string SQL = string.Format("insert into [dbo].[T_Mid_AirDayData] select '" + cityname + "','" + stationname + "','" + stationcode + "','" + time + "'," +
                s2 + "," + s1 + "," + n2 + "," + n1 + "," + c2 + "," + c1 + "," + o2 + "," + o1 + "," + o8_2 + "," + o8_1 + "," + p25A + "," + p25 + "," + p10A + "," + p10 + "," + aqi + "," + FirstP + "," + AirLevel
                + " where not exists(select *  from [dbo].[T_Mid_AirDayData] where StationName='" + stationname
                + "' and StationCode='" + stationcode + "' and MoniDate='" + time + "')");
            /*string SQL1 = string.Format("insert into [dbo].[T_Mid_AirDayData] select '" + cityname + "','" + stationname + "','" + stationcode + "','" + time + "'," +
                s2 + "," + s1 + "," + n2 + "," + n1 + "," + c2 + "," + c1 + "," + o2 + "," + o1 + ","  + p25A + "," + p25 + "," + p10A + "," + p10 + "," + aqi + "," + FirstP + "," + AirLevel
                + " where not exists(select CityName, StationName, StationCode,MoniDate, SO2AQI, SO2, NO2AQI, NO2, COAQI, CO, O3AQI, O3,PM25AQI,PM25, PM10AQI, PM10,AQI,FirstP,AirLevel  from [dbo].[T_Mid_AirDayData] where StationName='" 
                + stationname+ "' and StationCode='" + stationcode + "' and MoniDate='" + time + "')");*/
            
            sqlH.ExecuteSQLNonQuery(SQL);
           
            f++;
        }
        string strSQL = @"insert into [dbo].[T_Mid_AirDayData] 
select '朔州','朔州市','00',MoniDate,
[dbo].[so22iaqi](AVG(SO2)/1000),AVG(SO2),
[dbo].[no22iaqi](AVG(NO2)/1000),AVG(NO2),
[dbo].[co2iaqi](AVG(CO)),AVG(CO),
[dbo].[o32iaqi](AVG(O3)/1000,1),AVG(O3),
[dbo].[o32iaqi](AVG(O38)/1000,8),AVG(O38),
[dbo].[pm252iaqi](AVG(PM25)/1000),AVG(PM25),
[dbo].[pm102iaqi](AVG(PM10)/1000),AVG(PM10),
[dbo].[fun_AirQualityIndex]([dbo].[co2iaqi](AVG(CO)),[dbo].[so22iaqi](AVG(SO2)/1000),[dbo].[no22iaqi](AVG(NO2)/1000),[dbo].[o32iaqi](AVG(O3)/1000,1),[dbo].[o32iaqi](AVG(O38)/1000,8),[dbo].[pm252iaqi](AVG(PM25)/1000),[dbo].[pm102iaqi](AVG(PM10)/1000)),
[dbo].[fun_AQI_FristPollutant]([dbo].[co2iaqi](AVG(CO)),[dbo].[so22iaqi](AVG(SO2)/1000),[dbo].[no22iaqi](AVG(NO2)/1000),[dbo].[o32iaqi](AVG(O3)/1000,1),[dbo].[o32iaqi](AVG(O38)/1000,8),[dbo].[pm252iaqi](AVG(PM25)/1000),[dbo].[pm102iaqi](AVG(PM10)/1000)),
[dbo].[fun_AirAQILevel]([dbo].[fun_AirQualityIndex]([dbo].[co2iaqi](AVG(CO)),[dbo].[so22iaqi](AVG(SO2)/1000),[dbo].[no22iaqi](AVG(NO2)/1000),[dbo].[o32iaqi](AVG(O3)/1000,1),[dbo].[o32iaqi](AVG(O38)/1000,8),[dbo].[pm252iaqi](AVG(PM25)/1000),[dbo].[pm102iaqi](AVG(PM10)/1000)))
from [dbo].[T_Mid_AirDayData] 
where  not exists(
select *  from [dbo].[T_Mid_AirDayData] 
where StationCode='00' and MoniDate='" + time + "') and MoniDate='" + time + "'group by MoniDate";
        sqlH.ExecuteSQLNonQuery(strSQL);

    }

    public object RegexVal(object val)
    {
        return Regex.IsMatch(val.ToString(), @"^\d+(\.\d+)?$") == false ? "null" : val;
        //([0-9]{1,4}\.{0,1}[0-9]{0,2})
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}