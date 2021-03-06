﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using DataRepair;

namespace AirDataService
{
    public partial class Service1 : ServiceBase
    {
        private static System.Timers.Timer timer = null;
        private static string con = "";
        public Service1()
        {
            InitializeComponent();
            con = ConfigurationManager.ConnectionStrings["DB"].ToString();
            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.Enabled = true;
                timer.Interval = Int32.Parse(ConfigurationManager.AppSettings["timeInterval"]) * 60 * 1000;
                //timer.Interval = 2 * 60 * 1000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
                this.ServiceName = "AirDataGather";
            }
        }

        protected override void OnStart(string[] args)
        {
            writelog("数据同步服务启动");
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            writelog("数据同步服务停止");
            System.Diagnostics.Process[] myproc = System.Diagnostics.Process.GetProcessesByName("ServerForm");

            if (myproc != null && myproc.Length > 0)
            {
                int n = myproc.Length;
                for (int i = 0; i < n; i++)
                {
                    myproc[i].Kill();
                }
            }
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            doSth();
        }

        private void doSth()
        {
            try
            {
                //if (DateTime.Now.Minute > 30)
                    //ReadXml();
            }
            catch (Exception ex)
            {
                writelog("获取气象数据出错：" + ex.Message.ToString());
            }
            try
            {
                if (DateTime.Now.Minute > 10)
                    getAirHourData();
                if (DateTime.Now.Hour == 2)
                {
                    //getAirDayData();
                    //getCountyAirData();
                    statAirDayData();
                }
            }
            catch (Exception ex)
            {
                writelog("数据同步出错：" + ex.Message.ToString());
            }
            try
            {
                //if (DateTime.Now.Hour == 12 || DateTime.Now.Hour == 16 || DateTime.Now.Hour == 21)
                    //InsertAirForeCastSSCIData();
                    //InsertAirForeCastTJData();
            }
            catch (Exception ex)
            {
                writelog("获取统计出错：" + ex.Message.ToString());
            }
            try
            {
                //if (DateTime.Now.Hour == 9 || DateTime.Now.Hour == 13 || DateTime.Now.Hour == 21)
                    //InsertAirForeCastCMAQData();
            }
            catch (Exception ex)
            {
                writelog("数值获取出错：" + ex.Message.ToString());
            }
            try
            {
                //if (DateTime.Now.Hour == 17 || DateTime.Now.Hour == 18)
                    //InsertAirForeCastWRFData();
            }
            catch (Exception ex)
            {
                writelog("数值获取出错：" + ex.Message.ToString());
            }
            try
            {
                if (DateTime.Now.Hour == 8 || DateTime.Now.Hour == 16)
                //getCityAirHtml( DateTime.Now.ToShortDateString());
                {
                    //string scon = ConfigurationManager.ConnectionStrings["sourceDB"].ToString();
                    //string tcon = ConfigurationManager.ConnectionStrings["targetDB"].ToString();
                    //QxForeCast qf = new QxForeCast();
                    //qf.scon = scon;
                    //qf.tcon = tcon;
                    //qf.getQxPacket();
                }
            }
            catch (Exception ex)
            {
                writelog("气象报文解析出错：" + ex.Message.ToString());
            }
            writelog("数据请求一次完成！");
        }

        private string getCityAirHtml(string date)
        {
            string strSql = "select * from [dbo].[T_Mid_CityDayData] where MonitorTime = '" + date + "'";
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strSql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

            }
            else
            {
                int page = 13;
                for (int i = 0; i < page; i++)
                {
                    string targetUrl = "http://datacenter.mep.gov.cn/report/air_daily/air_dairy.jsp?city=&";
                    targetUrl += "startdate=" + date + "&enddate=" + date + "&page=" + (i + 1);
                    string html = GetPage(targetUrl);
                    //ReadHtml(html);
                }
            }
            return "";
        }

        public string GetPage(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            Encoding encoding = Encoding.UTF8;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "www.csddt.com";
                request.Timeout = 20000;
                request.AllowAutoRedirect = false;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    reader = new StreamReader(response.GetResponseStream(), encoding);
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                    reader.Close();
                if (request != null)
                    request = null;
            }
            return string.Empty;
        }

        public void getAirDayData()
        {
            RemoteService.ServiceSoap ser = new RemoteService.ServiceSoapClient();
            RemoteService.GetDayValueRequest request = new RemoteService.GetDayValueRequest();
            string strJson = ser.GetDayValue(request).Body.GetDayValueResult;

            //string strJson = "[{\"position_name\":\"白马寺\",\"so2\":52.000,\"no2\":27.000,\"pm10\":161.000,\"co\":1.300,\"o3_1\":64.000,\"o3_8\":55.000,\"pm25\":77.000,\"AQI\":106,\"sywrw\":颗粒物（粒径<=10μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"白马寺\",\"so2\":52.000,\"no2\":25.000,\"pm10\":104.000,\"co\":1.800,\"o3_1\":56.000,\"o3_8\":47.000,\"pm25\":90.000,\"AQI\":119,\"sywrw\":颗粒物（粒径<=2.5μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"自来水公司\",\"so2\":143.000,\"no2\":41.000,\"pm10\":180.000,\"co\":1.100,\"o3_1\":65.000,\"o3_8\":55.000,\"pm25\":78.000,\"AQI\":115,\"sywrw\":颗粒物（粒径<=10μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"白云商贸\",\"so2\":21.000,\"no2\":29.000,\"pm10\":162.000,\"co\":1.100,\"o3_1\":67.000,\"o3_8\":58.000,\"pm25\":58.000,\"AQI\":106,\"sywrw\":颗粒物（粒径<=10μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"市环保局\",\"so2\":41.000,\"no2\":19.000,\"pm10\":128.000,\"co\":1.600,\"o3_1\":25.000,\"o3_8\":22.000,\"pm25\":73.000,\"AQI\":98,\"sywrw\":颗粒物（粒径<=2.5μm）,\"wrjb\":Ⅱ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"技术学院\",\"so2\":20.000,\"no2\":24.000,\"pm10\":190.000,\"co\":1.500,\"o3_1\":102.000,\"o3_8\":87.000,\"pm25\":93.000,\"AQI\":123,\"sywrw\":颗粒物（粒径<=2.5μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"泽州一中\",\"so2\":35.000,\"no2\":22.000,\"pm10\":147.000,\"co\":1.200,\"o3_1\":61.000,\"o3_8\":52.000,\"pm25\":82.000,\"AQI\":109,\"sywrw\":颗粒物（粒径<=2.5μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"}]";

            if (strJson.Length == 0)
            {
                writelog("无返回数据！");
                return;
            }

            DataTable dt = CreateJsonToTable.JsonToDataTable(strJson);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strselect = "select * from T_Mid_AirDayData where StationCode = " + dt.Rows[i]["\"position_code\""] + " and MoniDate = '" + dt.Rows[i]["\"time\""] + ":00:00'";
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    double SO2 = double.Parse(dt.Rows[i]["\"so2\""].ToString()) / 1000;
                    double NO2 = double.Parse(dt.Rows[i]["\"no2\""].ToString()) / 1000;
                    double CO = double.Parse(dt.Rows[i]["\"co\""].ToString());
                    double O3 = double.Parse(dt.Rows[i]["\"o3_1\""].ToString()) / 1000;
                    double PM25 = double.Parse(dt.Rows[i]["\"pm25\""].ToString()) / 1000;
                    double PM10 = double.Parse(dt.Rows[i]["\"pm10\""].ToString()) / 1000;
                    string strSql = "insert T_Mid_AirDayData(StationName,StationCode,MoniDate,SO2,NO2,CO,O3,PM25,PM10,SO2AQI,NO2AQI,COAQI,O3AQI,PM25AQI,PM10AQI,AQI,FirstP,AirLevel) values(";
                    strSql += "'" + (dt.Rows[i]["\"position_name\""].ToString() == "晋城" ? "晋城市" : dt.Rows[i]["\"position_name\""]) + "',";
                    strSql += "'" + (dt.Rows[i]["\"position_code\""].ToString() == "0" ? "00" : dt.Rows[i]["\"position_code\""]) + "',";
                    strSql += "'" + dt.Rows[i]["\"time\""] + ":00:00',";
                    strSql += dt.Rows[i]["\"so2\""] + ",";
                    strSql += dt.Rows[i]["\"no2\""] + ",";
                    strSql += dt.Rows[i]["\"co\""] + ",";
                    strSql += dt.Rows[i]["\"o3_1\""] + ",";
                    strSql += dt.Rows[i]["\"pm25\""] + ",";
                    strSql += dt.Rows[i]["\"pm10\""] + ",";
                    strSql += "dbo.so22iaqi(" + SO2 + "),";
                    strSql += "dbo.no22iaqi(" + NO2 + "),";
                    strSql += "dbo.co2iaqi(" + CO + "),";
                    strSql += "dbo.o32iaqi(" + O3 + ",8),";
                    strSql += "dbo.pm252iaqi(" + PM25 + "),";
                    strSql += "dbo.pm102iaqi(" + PM10 + "),";
                    strSql += dt.Rows[i]["\"AQI\""] + ",";
                    strSql += "'" + dt.Rows[i]["\"sywrw\""] + "',";
                    strSql += "'" + dt.Rows[i]["\"wrjb\""] + "'";
                    strSql += ")";
                    writelog("城区=>" + strSql);
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strSql);
                }

            }
        }

        public void getAirHourData()
        {
            RemoteService.ServiceSoap ser = new RemoteService.ServiceSoapClient();
            RemoteService.GetHourValueRequest request = new RemoteService.GetHourValueRequest();
            string strJson = ser.GetHourValue(request).Body.GetHourValueResult;

            //string strJson = "[{\"position_name\":\"泽州一中\",\"so2\":0.02490,\"no2\":0.01209,\"pm10\":0.05100,\"co\":1.82525,\"o3\":0.05138,\"pm25\":0.02500,\"time\":\"2014/10/23 15:00:00\"},{\"position_name\":\"白云商贸\",\"so2\":-1.00000,\"no2\":0.00865,\"pm10\":0.09400,\"co\":-1.00000,\"o3\":0.06009,\"pm25\":0.06500,\"time\":\"2014/10/23 15:00:00\"},{\"position_name\":\"自来水公司\",\"so2\":0.00143,\"no2\":0.02251,\"pm10\":0.04900,\"co\":0.43775,\"o3\":0.05977,\"pm25\":0.02600,\"time\":\"2014/10/23 15:00:00\"},{\"position_name\":\"市环保局\",\"so2\":0.00824,\"no2\":0.00866,\"pm10\":0.14100,\"co\":3.00300,\"o3\":0.03348,\"pm25\":0.08000,\"time\":\"2014/10/23 15:00:00\"},{\"position_name\":\"技术学院\",\"so2\":0.01092,\"no2\":0.02122,\"pm10\":0.05600,\"co\":4.79075,\"o3\":0.09136,\"pm25\":0.03000,\"time\":\"2014/10/23 15:00:00\"},{\"position_name\":\"白马寺\",\"so2\":-1.00000,\"no2\":0.00548,\"pm10\":0.04600,\"co\":0.92200,\"o3\":0.06335,\"pm25\":0.02300,\"time\":\"2014/10/23 15:00:00\"}]";

            if (strJson.Length == 0) return;
            DataTable dt = CreateJsonToTable.JsonToDataTable(strJson);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strselect = "select * from T_Mid_AirHourData where StationName = '" + dt.Rows[i]["\"position_name\""] + "' and MoniDate = '" + dt.Rows[i]["\"time\""] + ":00:00'";
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    double SO2 = double.Parse(dt.Rows[i]["\"so2\""].ToString());
                    double NO2 = double.Parse(dt.Rows[i]["\"no2\""].ToString());
                    double CO = double.Parse(dt.Rows[i]["\"co\""].ToString());
                    double O3 = double.Parse(dt.Rows[i]["\"o3\""].ToString());
                    double PM25 = double.Parse(dt.Rows[i]["\"pm25\""].ToString());
                    double PM10 = double.Parse(dt.Rows[i]["\"pm10\""].ToString());
                    string strSql = "insert T_Mid_AirHourData(StationName,StationCode,MoniDate,SO2,NO2,CO,O3,PM25,PM10,SO2AQI,NO2AQI,COAQI,O3AQI,PM25AQI,PM10AQI,AQI,FirstP,AirLevel) values(";
                    strSql += "'" + dt.Rows[i]["\"position_name\""] + "',";
                    strSql += "'" + dt.Rows[i]["\"position_code\""] + "',";
                    strSql += "'" + dt.Rows[i]["\"time\""] + ":00:00',";
                    strSql += (SO2 * 1000 > 0 ? (SO2 * 1000).ToString() : "null") + ",";
                    strSql += (NO2 * 1000 > 0 ? (NO2 * 1000).ToString() : "null") + ",";
                    strSql += (CO > 0 ? (CO).ToString() : "null" )+ ",";
                    strSql += (O3 * 1000 > 0 ? (O3 * 1000).ToString() : "null" )+ ",";
                    strSql += (PM25 * 1000 > 0 ? (PM25 * 1000).ToString() : "null" )+ ",";
                    strSql += (PM10 * 1000 > 0 ? (PM10 * 1000).ToString() : "null") + ",";
                    strSql += "dbo.so22iaqi(" + SO2 + "),";
                    strSql += "dbo.no22iaqi(" + NO2 + "),";
                    strSql += "dbo.co2iaqi(" + CO + "),";
                    strSql += "dbo.o32iaqi(" + O3 + ",1),";
                    strSql += "dbo.pm252iaqi(" + PM25 + "),";
                    strSql += "dbo.pm102iaqi(" + PM10 + "),";
                    strSql += "dbo.fun_AirQualityIndex(dbo.co2iaqi(" + CO + "),dbo.so22iaqi(" + SO2 + "),dbo.no22iaqi(" + NO2 + "),dbo.o32iaqi(" + O3 + ",1),dbo.o32iaqi(" + O3 + ",8),dbo.pm252iaqi(" + PM25 + "),dbo.pm102iaqi(" + PM10 + ")),";//AQI
                    strSql += "dbo.fun_AQI_FristPollutant(dbo.co2iaqi(" + CO + "),dbo.so22iaqi(" + SO2 + "),dbo.no22iaqi(" + NO2 + "),dbo.o32iaqi(" + O3 + ",1),dbo.o32iaqi(" + O3 + ",8),dbo.pm252iaqi(" + PM25 + "),dbo.pm102iaqi(" + PM10 + ")),";//首要污染物
                    strSql += "dbo.fun_AirAQILevel(dbo.fun_AirQualityIndex(dbo.co2iaqi(" + CO + "),dbo.so22iaqi(" + SO2 + "),dbo.no22iaqi(" + NO2 + "),dbo.o32iaqi(" + O3 + ",1),dbo.o32iaqi(" + O3 + ",8),dbo.pm252iaqi(" + PM25 + "),dbo.pm102iaqi(" + PM10 + ")))";//空气等级
                    strSql += ")";
                    writelog("小时=>" + strSql);
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strSql);
                    

                    string strdate = dt.Rows[i]["\"time\""].ToString().Substring(0, dt.Rows[i]["\"time\""].ToString().Length - 2);
                    string strfile = new DateTime(Int32.Parse(strdate.Split('/')[0]), Int32.Parse(strdate.Split('/')[1]), Int32.Parse(strdate.Split('/')[2])).ToString("yyyy-MM-dd");
                    //txt
                    //string col = "time SO2 NO2 CO O3 PM25 PM10";
                    //writeAirData(dt.Rows[i]["\"position_name\""].ToString(), col, false, strfile);//保存txt文件到linux服务器上
                    //string data = dt.Rows[i]["\"time\""] + ":00:00 " + dt.Rows[i]["\"so2\""].ToString() + " " + dt.Rows[i]["\"no2\""].ToString() + " " + dt.Rows[i]["\"co\""].ToString() + " " + dt.Rows[i]["\"o3\""].ToString() + " " + dt.Rows[i]["\"pm25\""].ToString() + " " + dt.Rows[i]["\"pm10\""].ToString();
                    //writeAirData(dt.Rows[i]["\"position_name\""].ToString(), data, true, strfile);

                }

            }
            string strselect1 = "select * from T_Mid_AirHourData where StationCode = '00' and MoniDate = '" + dt.Rows[0]["\"time\""] + ":00:00'";
            DataSet ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect1);
            if (ds1.Tables[0].Rows.Count == 0)
            {
                string Sql = @"insert T_Mid_AirHourData(StationName,StationCode,MoniDate,SO2AQI,SO2,NO2AQI,NO2,COAQI,CO,O3AQI,O3,PM25AQI,PM25,PM10AQI,PM10,AQI,FirstP,AirLevel)
select '朔州市','00',MoniDate,
avg(SO2AQI),avg(SO2),avg(NO2AQI),avg(NO2),avg(COAQI),avg(CO),avg(O3AQI),avg(O3),avg(PM25AQI),avg(PM25),avg(PM10AQI),avg(PM10) ,
[dbo].[fun_AirQualityIndex](avg(COAQI),avg(SO2AQI),avg(NO2AQI),avg(O3AQI),0,avg(PM25AQI),avg(PM10AQI)),
[dbo].[fun_AQI_FristPollutant](avg(COAQI),avg(SO2AQI),avg(NO2AQI),avg(O3AQI),0,avg(PM25AQI),avg(PM10AQI)),
[dbo].[fun_AirAQILevel]([dbo].[fun_AirQualityIndex](avg(COAQI),avg(SO2AQI),avg(NO2AQI),avg(O3AQI),0,avg(PM25AQI),avg(PM10AQI)))
from  [dbo].[T_Mid_AirHourData]
where MoniDate = '" + dt.Rows[0]["\"time\""].ToString() + ":00:00' group by MoniDate ";
                writelog("小时=>" + Sql);
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, Sql);
            }

        }


        private void statAirDayData()
        {
             
        }

        public void getCountyAirData()
        {
            RemoteService.ServiceSoap ser = new RemoteService.ServiceSoapClient();
            RemoteService.GetXianValueRequest request = new RemoteService.GetXianValueRequest(new RemoteService.GetXianValueRequestBody(DateTime.Now.AddDays(-1).ToShortDateString().ToString()));
            string strJson = ser.GetXianValue(request).Body.GetXianValueResult;

            //string strJson = "[{\"position_name\":\"白马寺\",\"so2\":52.000,\"no2\":27.000,\"pm10\":161.000,\"co\":1.300,\"o3_1\":64.000,\"o3_8\":55.000,\"pm25\":77.000,\"AQI\":106,\"sywrw\":颗粒物（粒径<=10μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"白马寺\",\"so2\":52.000,\"no2\":25.000,\"pm10\":104.000,\"co\":1.800,\"o3_1\":56.000,\"o3_8\":47.000,\"pm25\":90.000,\"AQI\":119,\"sywrw\":颗粒物（粒径<=2.5μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"自来水公司\",\"so2\":143.000,\"no2\":41.000,\"pm10\":180.000,\"co\":1.100,\"o3_1\":65.000,\"o3_8\":55.000,\"pm25\":78.000,\"AQI\":115,\"sywrw\":颗粒物（粒径<=10μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"白云商贸\",\"so2\":21.000,\"no2\":29.000,\"pm10\":162.000,\"co\":1.100,\"o3_1\":67.000,\"o3_8\":58.000,\"pm25\":58.000,\"AQI\":106,\"sywrw\":颗粒物（粒径<=10μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"市环保局\",\"so2\":41.000,\"no2\":19.000,\"pm10\":128.000,\"co\":1.600,\"o3_1\":25.000,\"o3_8\":22.000,\"pm25\":73.000,\"AQI\":98,\"sywrw\":颗粒物（粒径<=2.5μm）,\"wrjb\":Ⅱ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"技术学院\",\"so2\":20.000,\"no2\":24.000,\"pm10\":190.000,\"co\":1.500,\"o3_1\":102.000,\"o3_8\":87.000,\"pm25\":93.000,\"AQI\":123,\"sywrw\":颗粒物（粒径<=2.5μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"},{\"position_name\":\"泽州一中\",\"so2\":35.000,\"no2\":22.000,\"pm10\":147.000,\"co\":1.200,\"o3_1\":61.000,\"o3_8\":52.000,\"pm25\":82.000,\"AQI\":109,\"sywrw\":颗粒物（粒径<=2.5μm）,\"wrjb\":Ⅲ,\"time\":\"2014/10/22 0:00:00\"}]";

            if (strJson.Length == 0)
            {
                writelog("无返回数据！");
                return;
            }

            DataTable dt = CreateJsonToTable.JsonToDataTable(strJson);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strselect = "select * from T_Mid_AirDayData where StationCode = '" + dt.Rows[i]["\"position_code\""] + "' and MoniDate = '" + dt.Rows[i]["\"time\""] + ":00:00'";
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    double SO2 = double.Parse(dt.Rows[i]["\"so2\""].ToString()) / 1000;
                    double NO2 = double.Parse(dt.Rows[i]["\"no2\""].ToString()) / 1000;
                    double CO = double.Parse(dt.Rows[i]["\"co\""].ToString());
                    double O3 = double.Parse(dt.Rows[i]["\"o3_1\""].ToString()) / 1000;
                    double PM25 = double.Parse(dt.Rows[i]["\"pm25\""].ToString()) / 1000;
                    double PM10 = double.Parse(dt.Rows[i]["\"pm10\""].ToString()) / 1000;
                    string strSql = "insert T_Mid_AirDayData(StationName,StationCode,MoniDate,SO2,NO2,CO,O3,PM25,PM10,SO2AQI,NO2AQI,COAQI,O3AQI,PM25AQI,PM10AQI,AQI,FirstP,AirLevel) values(";
                    strSql += "'" + dt.Rows[i]["\"position_name\""].ToString().Replace(" ", "") + "',";
                    strSql += "'" + dt.Rows[i]["\"position_code\""].ToString() + "',";
                    strSql += "'" + dt.Rows[i]["\"time\""] + ":00:00',";
                    strSql += dt.Rows[i]["\"so2\""] + ",";
                    strSql += dt.Rows[i]["\"no2\""] + ",";
                    strSql += dt.Rows[i]["\"co\""] + ",";
                    strSql += dt.Rows[i]["\"o3_1\""] + ",";
                    strSql += dt.Rows[i]["\"pm25\""] + ",";
                    strSql += dt.Rows[i]["\"pm10\""] + ",";
                    strSql += "dbo.so22iaqi(" + SO2 + "),";
                    strSql += "dbo.no22iaqi(" + NO2 + "),";
                    strSql += "dbo.co2iaqi(" + CO + "),";
                    strSql += "dbo.o32iaqi(" + O3 + ",8),";
                    strSql += "dbo.pm252iaqi(" + PM25 + "),";
                    strSql += "dbo.pm102iaqi(" + PM10 + "),";
                    strSql += dt.Rows[i]["\"AQI\""] + ",";
                    strSql += "'" + dt.Rows[i]["\"sywrw\""] + "',";
                    strSql += "'" + dt.Rows[i]["\"wrjb\""] + "'";
                    strSql += ")";
                    writelog("县城=>" + strSql);
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strSql);

                    string strdate = dt.Rows[i]["\"time\""].ToString().Substring(0, dt.Rows[i]["\"time\""].ToString().Length - 2);
                    string strfile = new DateTime(Int32.Parse(strdate.Split('/')[0]), Int32.Parse(strdate.Split('/')[1]), Int32.Parse(strdate.Split('/')[2])).ToString("yyyy-MM-dd");
                    //txt
                    string col = "time SO2 NO2 CO O3 PM25 PM10";
                    writeAirData(dt.Rows[i]["\"position_name\""].ToString().Substring(0, 2), col, false, strfile);//保存txt文件到linux服务器上
                    string data = dt.Rows[i]["\"time\""] + ":00:00 " + dt.Rows[i]["\"so2\""].ToString() + " " + dt.Rows[i]["\"no2\""].ToString() + " " + dt.Rows[i]["\"co\""].ToString() + " " + dt.Rows[i]["\"o3_1\""].ToString() + " " + dt.Rows[i]["\"pm25\""].ToString() + " " + dt.Rows[i]["\"pm10\""].ToString();
                    writeAirData(dt.Rows[i]["\"position_name\""].ToString().Substring(0, 2), data, true, strfile);
                }

            }
        }


        /// <summary>
        /// 将信息输出到文本文件
        /// </summary>
        /// <param name="readme"></param>
        public void writelog(string txt)
        {
            string logPath = ConfigurationManager.AppSettings["logPath"] + "AirDataGather\\" + DateTime.Now.ToString("yyyy-MM-dd").ToString() + ".txt";
            StreamWriter dout = new StreamWriter(logPath, true);
            dout.Write(System.DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + " => " + txt + "\r\n");
            dout.Close();
        }

        public void writeAirData(string StationName, string txt, bool up, string date)
        {
            string logPath = ConfigurationManager.AppSettings["AairDataPath"] + "\\" + StationName + "\\" + date + ".txt";
            StreamWriter dout = new StreamWriter(logPath, true);
            dout.Write(txt + "\r\n");
            dout.Close();
            if (!up) return;
            string stationid = string.Empty;
            string strselect = "select stationcode from T_Bas_AirStation where StationName = '" + StationName + "'";
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect);
            if (ds.Tables[0].Rows.Count > 0)
            {
                stationid = ds.Tables[0].Rows[0]["stationcode"].ToString();
            }
            UploadFile(logPath, stationid);
        }

        /// <summary>
        /// FTP上传文件到服务器
        /// </summary>
        private void UploadFile(string logPath, string stationid)
        {
            FtpWebRequest reqFTP = null;
            string serverIP;
            string userName;
            string password;
            string url;

            try
            {
                FileInfo fileInf = new FileInfo(logPath);
                serverIP = ConfigurationManager.AppSettings["FTPIP"];
                userName = ConfigurationManager.AppSettings["FTPAdmin"];
                password = ConfigurationManager.AppSettings["FTPPwd"];
                url = "ftp://" + serverIP + "/AIR/" + stationid + "/" + fileInf.Name;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.Credentials = new NetworkCredential(userName, password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;

                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                FileStream fs = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                Stream strm = reqFTP.GetRequestStream();

                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {

                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                if (reqFTP != null)
                {
                    reqFTP.Abort();
                }
                writelog("文件同步出错：" + logPath + ex.InnerException.Message);
            }
        }

        /// <summary>
        /// FTP从服务器上下载文件
        /// </summary>
        private void DownloadFile(string filepath, string fileName, string FTPPath)
        {
            FtpWebRequest reqFTP;
            string ftpServerIP = ConfigurationManager.AppSettings["FTPIP"];
            string ftpUserID = ConfigurationManager.AppSettings["FTPAdmin"];
            string ftpPassword = ConfigurationManager.AppSettings["FTPPwd"];
            string url = "ftp://" + ftpServerIP + FTPPath + fileName;
            try
            {
                FileStream outputStream = new FileStream(filepath + fileName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                writelog("文件下载出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 将文本文件SSCI数据存入数据库
        /// </summary>
        private void InsertAirForeCastSSCIData()
        {
            string SavePath = ConfigurationManager.AppSettings["AirForeCastData"];
            string FTPPath = ConfigurationManager.AppSettings["FTPSSCIPath"];
            string[] fileinfo = GetFTPSSCIFileList();
            string[] filename = new string[3];
            string[] fileCreateTime = new string[3];
            for (int i = 0; i < fileinfo.Length; i++)
            {
                string fi = fileinfo[i];
                string[] arr = fi.Split(',');
                filename[i] = arr[0];
                fileCreateTime[i] = arr[1];
                DownloadFile(SavePath + "SSCI/", filename[i], FTPPath);
            }

            for (int i = 0; i < filename.Length; i++)
            {
                //string MoniDate = Convert.ToDateTime(fileCreateTime[i]).ToString("yyyy-MM-dd HH");
                //string ForecastTime = DateTime.Now.ToString("yyyy-MM-dd HH");
                if (filename[i] == null) continue;
                string ForecastTime = Convert.ToDateTime(fileCreateTime[i]).ToString("yyyy-MM-dd HH");
                string MoniDate = filename[i].Substring(11, 4) + "-" + filename[i].Substring(15, 2) + "-" + filename[i].Substring(17, 2) + " 00";

                string[,] filedarr = ParseSSCITxt(SavePath + "SSCI/" + filename[i]);
                for (int k = 0; k < filedarr.GetLength(0) - 1; k++)
                {
                    string stationcode = string.Empty;
                    string stationname = string.Empty;
                    string strSql = "insert into T_Mid_AirForeCast(StationName,StationCode,SO2,NO2,PM10,CO,O3,PM25,SO2AQI,NO2AQI,PM10AQI,COAQI,O3AQI,PM25AQI,AQI,FirstP,AirLevel,MonitorTime,ForecastTime,ForecastModel) values(";
                    for (int j = 0; j < filedarr.GetLength(1); j++)
                    {
                        if (j == 0)
                        {
                            stationcode = filedarr[k, j].Substring(1);
                            if (stationcode == "00")
                            {
                                stationname = "晋城市";
                            }
                            else
                            {
                                string strselect = "select stationname from T_Bas_AirStation where StationCode = '" + stationcode + "'";
                                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    stationname = ds.Tables[0].Rows[0]["stationname"].ToString();
                                }
                            }
                            strSql += "'" + stationname + "',";
                            strSql += "'" + stationcode + "',";
                        }
                        else
                        {
                            strSql += filedarr[k, j] + ",";
                        }
                    }
                    if (IsExistForeCast(stationname, MoniDate + ":00:00", ForecastTime + ":00:00", 0))
                        continue;
                    double so2value = double.Parse(filedarr[k, 1]) / 1000;
                    double no2value = double.Parse(filedarr[k, 2]) / 1000;
                    double pm10value = double.Parse(filedarr[k, 3]) / 1000;
                    double o3value = double.Parse(filedarr[k, 5]) / 1000;
                    double pm25value = double.Parse(filedarr[k, 6]) / 1000;
                    strSql += "dbo.so22iaqi(" + so2value + "),";
                    strSql += "dbo.no22iaqi(" + no2value + "),";
                    strSql += "dbo.pm102iaqi(" + pm10value + "),";
                    strSql += "dbo.co2iaqi(" + filedarr[k, 4] + "),";
                    strSql += "dbo.o32iaqi(" + o3value + ",1),";
                    strSql += "dbo.pm252iaqi(" + pm25value + "),";
                    strSql += "dbo.fun_AirQualityIndex(dbo.co2iaqi(" + filedarr[k, 4] + "),dbo.so22iaqi(" + so2value + "),dbo.no22iaqi(" + no2value + "),dbo.o32iaqi(" + o3value + ",1),dbo.o32iaqi(" + o3value + ",8),dbo.pm252iaqi(" + pm25value + "),dbo.pm102iaqi(" + pm10value + ")),";//AQI
                    strSql += "dbo.fun_AQI_FristPollutant(dbo.co2iaqi(" + filedarr[k, 4] + "),dbo.so22iaqi(" + so2value + "),dbo.no22iaqi(" + no2value + "),dbo.o32iaqi(" + o3value + ",1),dbo.o32iaqi(" + o3value + ",8),dbo.pm252iaqi(" + pm25value + "),dbo.pm102iaqi(" + pm10value + ")),";//首要污染物
                    strSql += "dbo.fun_AirAQIdegree(dbo.fun_AirQualityIndex(dbo.co2iaqi(" + filedarr[k, 4] + "),dbo.so22iaqi(" + so2value + "),dbo.no22iaqi(" + no2value + "),dbo.o32iaqi(" + o3value + ",1),dbo.o32iaqi(" + o3value + ",8),dbo.pm252iaqi(" + pm25value + "),dbo.pm102iaqi(" + pm10value + "))),";//空气等级
                    strSql += "'" + MoniDate + ":00:00',";
                    strSql += "'" + ForecastTime + ":00:00',";
                    strSql += "0)";
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strSql);
                    
                }
                writelog("获取统计结果--"+filename[i]);
            }
        }

        private Boolean IsExistForeCast(string name,string monitortime,string forecasttime,int forecastmodel) 
        {
            string sql = "select * from T_Mid_AirForeCast where StationName='" + name + "' and MonitorTime='" + monitortime + "' and ForecastTime='" + forecasttime + "' and ForecastModel = " + forecastmodel;
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取FTP中SSCI下载目录文件列表
        /// </summary>
        /// <returns>符合条件的文件列表</returns>
        private string[] GetFTPSSCIFileList()
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            string ftpServerIP = ConfigurationManager.AppSettings["FTPIP"];
            string ftpUserID = ConfigurationManager.AppSettings["FTPAdmin"];
            string ftpPassword = ConfigurationManager.AppSettings["FTPPwd"];
            string FTPPath = ConfigurationManager.AppSettings["FTPSSCIPath"];
            string url = "ftp://" + ftpServerIP + FTPPath;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                string line = reader.ReadLine();
                string filename = string.Empty;
                DateTime dt = Convert.ToDateTime("1970-01-01 00:00:00");
                while (line != null)
                {
                    if (line.IndexOf("JC_SSCD_JC_") > 0)
                    {
                        string[] arr = line.Split(' ');
                        CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                        string format = "MMM d HH:mm";
                        string stringValue = DateTime.Now.ToString(format, cultureInfo); // 得到日期字符串
                        DateTime datetime = DateTime.ParseExact(arr[23] + " " + arr[24] + " " + arr[25], format, cultureInfo);
                        if (datetime.Month > 10)
                            datetime = DateTime.Parse("2014-" + datetime.Month + "-" + datetime.Day + " " + datetime.Hour + ":" + datetime.Minute + ":" + datetime.Second);
                        if (datetime >= dt)
                        {
                            if (datetime > dt) result.Clear();
                            dt = datetime;
                            filename = arr[26];
                            result.Append(filename + "," + datetime.ToString());
                            result.Append("\n");
                        }
                    }
                    line = reader.ReadLine();
                }

                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                writelog("获取下载文件列表出错：" + ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }
        }

        /// <summary>
        /// 解析SSCI文本文件
        /// </summary>
        /// <param name="filepath">文本文件路径加文件名</param>
        /// <returns>二维数组</returns>
        private string[,] ParseSSCITxt(string filepath)
        {
            string str = File.ReadAllText(filepath);
            str = str.Substring(0, str.IndexOf("099") - 1);
            string[] rowarr = str.Split('\n');
            string[,] recordarrs = new string[rowarr.Length - 1, 7];
            for (int i = 1; i < rowarr.Length; i++)
            {
                string[] recordarr = rowarr[i].Split(' ');
                List<string> list = new List<string>();
                for (int j = 0; j < recordarr.Length; j++)
                {
                    if (recordarr[j] == "") continue;
                    list.Add(recordarr[j]);
                }
                recordarr = list.ToArray();
                if (recordarr.Length > 0)
                {
                    for (int j = 0; j < recordarr.Length; j++)
                    {
                        recordarrs[i - 1, j] = recordarr[j];
                    }
                }
            }
            return recordarrs;
        }

        /// <summary>
        /// 将文本文件CMAQ数据存入数据库
        /// </summary>
        public void InsertAirForeCastCMAQData()
        {
            string SavePath = ConfigurationManager.AppSettings["AirForeCastData"];
            string FTPPath = ConfigurationManager.AppSettings["FTPCMAQPath"];
            string ExistFolder = GetExistFolder(1);
            string[] folders = GetFTPCMAQFolder(ExistFolder);
            if (folders == null) return;
            for (int i = 0; i < folders.Length; i++)
            {
                if (!Directory.Exists(SavePath + "CMAQ/" + folders[i])) Directory.CreateDirectory(SavePath + "CMAQ/" + folders[i]);
                List<string> files = GetFTPCMAQFilelist(folders[i]);
                for (int j = files.Count - 1; j >= 0; j--)
                {
                    DownloadFile(SavePath + "CMAQ/" + folders[i] + "/", files[j], FTPPath + folders[i] + "/");
                    if (files[j].IndexOf("Z_SXSZ") >= 0) files.Remove(files[j]);
                }
                for (int j = 0; j < files.Count; j++)
                {
                    string[,] filedarr = ParseCMAQTxt(SavePath + "CMAQ/" + folders[i] + "/" + files[j]);
                    string[] arr = files[j].Split('_');
                    //string stationcode = arr[2].Substring(arr[2].Length - 2, 2);
                    if (Int32.Parse(arr[2]) > 0 && Int32.Parse(arr[2]) <= 20) continue;
                    string stationcode = Int32.Parse(arr[2]).ToString();
                    if (stationcode == "0") stationcode = "00";
                    string stationname = string.Empty;
                    int QualityLevel = 0;
                    string QualityRome = string.Empty;
                    string AirColor = string.Empty;
                    string FirstP = string.Empty;
                    string strselect = "select stationname from T_Bas_AirStation where StationCode = '" + stationcode + "'";
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        stationname = ds.Tables[0].Rows[0]["stationname"].ToString();
                    }

                    for (int k = 0; k < filedarr.GetLength(0); k++)
                    {
                        string strsql = "select QualityLevel,QualitySymbol,ColorName from T_Bas_QualityStandard where MinValue <= " + filedarr[k, 1] + " and MaxValue >= " + filedarr[k, 1];
                        DataSet ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            QualityLevel = int.Parse(ds1.Tables[0].Rows[0]["QualityLevel"].ToString());
                            QualityRome = ds1.Tables[0].Rows[0]["QualitySymbol"].ToString();
                            AirColor = ds1.Tables[0].Rows[0]["ColorName"].ToString();
                            if (QualityLevel == 1)
                            {
                                FirstP = "";
                            }
                            else
                            {
                                FirstP = filedarr[k, 2];
                            }
                        }

                        string[] arrtime = filedarr[k, 0].Split('+');
                        string ForecastTime = arrtime[0].Substring(0, 4) + "-" + arrtime[0].Substring(4, 2) + "-" + arrtime[0].Substring(6, 2) + " " + arrtime[0].Substring(8, 2) + ":00:00";
                        DateTime dt = Convert.ToDateTime(ForecastTime).AddHours(int.Parse(arrtime[1]));
                        if (IsExistForeCast(stationname, dt.ToString(), ForecastTime, 1))
                            continue;
                        string strSql = "insert into T_Mid_AirForeCastHour(StationName,StationCode,SO2,NO2,PM10,CO,O3,PM25,SO2AQI,NO2AQI,PM10AQI,COAQI,O3AQI,PM25AQI,AQI,FirstP,AirLevel,MonitorTime,ForecastTime,ForecastModel) values(";
                        strSql += "'" + stationname + "',";
                        strSql += "'" + stationcode + "',";
                        strSql += filedarr[k, 5] + ",";
                        strSql += filedarr[k, 7] + ",";
                        strSql += filedarr[k, 13] + ",";
                        strSql += filedarr[k, 9] + ",";
                        strSql += filedarr[k, 11] + ",";
                        strSql += filedarr[k, 15] + ",";
                        strSql += filedarr[k, 6] + ",";
                        strSql += filedarr[k, 8] + ",";
                        strSql += filedarr[k, 14] + ",";
                        strSql += filedarr[k, 10] + ",";
                        strSql += filedarr[k, 12] + ",";
                        strSql += filedarr[k, 16] + ",";
                        strSql += filedarr[k, 1] + ",";
                        strSql += "'" + FirstP + "',";
                        strSql += "'" + QualityLevel + "',";
                        strSql += "'" + dt.ToString() + "',";
                        strSql += "'" + ForecastTime + "',";
                        strSql += "1)";
                        SqlHelper.ExecuteNonQuery(con, CommandType.Text, strSql);
                    }
                }
                writelog("获取CMAQ结果成功：" + folders[i]);
            }
        }

        /// <summary>
        /// 获取数据库中最新的10个文件夹
        /// </summary>
        /// <returns>10个文件夹名称相连的字符串</returns>
        private string GetExistFolder(int datatype)
        {
            string foldername = string.Empty;
            string sql = "SELECT ForecastTime FROM T_Mid_AirForeCastHour WHERE ForecastModel = " + datatype + " GROUP BY ForecastTime ORDER BY ForecastTime DESC";
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DateTime dt = Convert.ToDateTime(ds.Tables[0].Rows[i][0].ToString());
                foldername += dt.ToString("yyyyMMddHH") + ",";
            }

            return foldername;
        }
        
        /// <summary>
        /// 获取FTP中CMAQ未上传的文件夹列表
        /// </summary>
        /// <returns>文件夹数组</returns>
        private string[] GetFTPCMAQFolder(string ExistCMAQFolder)
        {
            string[] downloadFolders;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            string ftpServerIP = ConfigurationManager.AppSettings["FTPIP"];
            string ftpUserID = ConfigurationManager.AppSettings["FTPAdmin"];
            string ftpPassword = ConfigurationManager.AppSettings["FTPPwd"];
            string FTPPath = ConfigurationManager.AppSettings["FTPCMAQPath"];
            string url = "ftp://" + ftpServerIP + FTPPath;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                string line = reader.ReadLine();
                string foldername = string.Empty;
                while (line != null && line.Length >= 10)
                {                
                    if (ExistCMAQFolder.IndexOf(line.Substring(0,10)) < 0)
                    {
                        result.Append(line);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                if (result.ToString() != "")
                {
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    reader.Close();
                    response.Close();
                    return result.ToString().Split('\n');
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                writelog("获取下载文件夹列表出错：" + ex.Message);
                downloadFolders = null;
                return downloadFolders;
            }
        }

        /// <summary>
        /// 获取FTP中某个文件夹下CMAQ的文件列表
        /// </summary>
        /// <param name="foldername">文件夹名称</param>
        /// <returns>文件名列表</returns>
        private List<string> GetFTPCMAQFilelist(string foldername)
        {
            List<string> list = new List<string>();
            FtpWebRequest reqFTP;
            string ftpServerIP = ConfigurationManager.AppSettings["FTPIP"];
            string ftpUserID = ConfigurationManager.AppSettings["FTPAdmin"];
            string ftpPassword = ConfigurationManager.AppSettings["FTPPwd"];
            string FTPPath = ConfigurationManager.AppSettings["FTPCMAQPath"];
            string url = "ftp://" + ftpServerIP + FTPPath + foldername + "/";
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                string line = reader.ReadLine();
                while (line != null)
                {
                    if ((line.IndexOf("Z_SXSZ_ENVAQFC") >= 0 || line.IndexOf("SZ_ENVAQFC") >= 0) && line.IndexOf("daily") < 0)
                    {
                        list.Add(line);
                    }
                    line = reader.ReadLine();
                }

                reader.Close();
                response.Close();
                return list;
            }
            catch (Exception ex)
            {
                writelog("获取下载文件列表出错：" + ex.Message);
                list = null;
                return list;
            }
        }

        /// <summary>
        /// 解析CMAQ文本文件
        /// </summary>
        /// <param name="filepath">文本文件路径加文件名</param>
        /// <returns>二维数组</returns>
        private string[,] ParseCMAQTxt(string filepath)
        {
            string str = File.ReadAllText(filepath);
            //str = str.Substring(0, str.IndexOf("099") - 1);
            string[] rowarr = str.Split('\n');
            string[,] recordarrs = new string[rowarr.Length - 2, 19];
            for (int i = 1; i < rowarr.Length-1; i++)
            {
                string[] recordarr = rowarr[i].Split(' ');
                List<string> list = new List<string>();
                for (int j = 0; j < recordarr.Length; j++)
                {
                    if (recordarr[j] == "") continue;
                    list.Add(recordarr[j]);
                }
                recordarr = list.ToArray();
                if (recordarr.Length > 0)
                {
                    for (int j = 0; j < recordarr.Length; j++)
                    {
                        recordarrs[i - 1, j] = recordarr[j];
                    }
                }
            }
            return recordarrs;
        }

        public void InsertAirForeCastWRFData()
        {
            string SavePath = ConfigurationManager.AppSettings["AirForeCastData"];
            string FTPPath = ConfigurationManager.AppSettings["FTPWRFPath"];
            string ExistFolder = GetExistFolder(2);
            string[] folders = GetFTPWRFFolder(FTPPath, ExistFolder);
            for (int i = 0; i < folders.Length; i++)
            {
                if (!Directory.Exists(SavePath + "WRF/" + folders[i])) Directory.CreateDirectory(SavePath + "WRF/" + folders[i]);
                List<string> files = GetFTPWRFFilelist(folders[i]);
                for (int j = files.Count - 1; j >= 0; j--)
                {
                    DownloadFile(SavePath + "WRF/" + folders[i] + "/", files[j], FTPPath + folders[i] + "/");
                    if (files[j].IndexOf("Z_SEVP_C") >= 0) files.Remove(files[j]);
                }

                for (int j = 0; j < files.Count; j++)
                {
                    string[,] filedarr = ParseWRFTxt(SavePath + "WRF/" + folders[i] + "/" + files[j]);
                    string[] arr = files[j].Split('_');
                    string stationcode = arr[3].Substring(arr[3].Length - 2, 2);
                    string stationname = string.Empty;
                    string strselect = "select stationname from T_Bas_AirStation where StationCode = '" + stationcode + "'";
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        stationname = ds.Tables[0].Rows[0]["stationname"].ToString();
                    }
                    string PublicTime = folders[i].Substring(9, 4) + "-" + folders[i].Substring(13, 2) + "-" + folders[i].Substring(15, 2) + " 08:00:00";
                    for (int k = 0; k < filedarr.GetLength(0); k++)
                    {
                        string[] arrtime = filedarr[k, 0].Split('+');
                        string ForecastTime = arrtime[0].Substring(0, 4) + "-" + arrtime[0].Substring(4, 2) + "-" + arrtime[0].Substring(6, 2) + " " + arrtime[0].Substring(8, 2) + ":00:00";
                        DateTime dt = Convert.ToDateTime(ForecastTime).AddHours(int.Parse(arrtime[1]));
                        string strSql = "insert into T_Mid_AirForeCast(StationName,StationCode,SO2,NO2,PM10,CO,O3,PM25,SO2AQI,NO2AQI,PM10AQI,COAQI,O3AQI,PM25AQI,AQI,FirstP,AirLevel,MonitorTime,ForecastTime,ForecastModel) values(";
                        strSql += "'" + stationname + "',";
                        strSql += "'" + stationcode + "',";
                        strSql += filedarr[k, 4] + ",";
                        strSql += filedarr[k, 6] + ",";
                        strSql += filedarr[k, 12] + ",";
                        strSql += filedarr[k, 8] + ",";
                        strSql += filedarr[k, 10] + ",";
                        strSql += filedarr[k, 14] + ",";
                        strSql += filedarr[k, 5] + ",";
                        strSql += filedarr[k, 7] + ",";
                        strSql += filedarr[k, 13] + ",";
                        strSql += filedarr[k, 9] + ",";
                        strSql += filedarr[k, 11] + ",";
                        strSql += filedarr[k, 15] + ",";
                        strSql += filedarr[k, 1] + ",";
                        strSql += "'" + filedarr[k, 2] + "',";
                        strSql += "'" + getRomeLevel(Int32.Parse(filedarr[k, 3])) + "',";
                        strSql += "'" + dt.ToString() + "',";
                        strSql += "'" + PublicTime + "',";
                        strSql += "2)";
                        SqlHelper.ExecuteNonQuery(con, CommandType.Text, strSql);
                        //writelog(strSql);
                    }
                }
            }
        }

        private string[] GetFTPWRFFolder(string FTPPath, string ExistFolder)
        {
            string[] downloadFolders;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            string ftpServerIP = ConfigurationManager.AppSettings["FTPIP"];
            string ftpUserID = ConfigurationManager.AppSettings["FTPAdmin"];
            string ftpPassword = ConfigurationManager.AppSettings["FTPPwd"];
            string url = "ftp://" + ftpServerIP + FTPPath;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                string line = reader.ReadLine();
                string foldername = string.Empty;
                while (line != null)
                {
                    string[] arr = line.Split('_');
                    if (arr.Length < 3)
                    {
                        line = reader.ReadLine();
                        continue;
                    }
                    if (ExistFolder.IndexOf(arr[2]) < 0)
                    {
                        result.Append(line);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                if (result.ToString() != "")
                {
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    reader.Close();
                    response.Close();
                    return result.ToString().Split('\n');
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //writelog("获取下载文件夹列表出错：" + ex.Message);
                downloadFolders = null;
                return downloadFolders;
            }
        }

        private List<string> GetFTPWRFFilelist(string foldername)
        {
            List<string> list = new List<string>();
            FtpWebRequest reqFTP;
            string ftpServerIP = ConfigurationManager.AppSettings["FTPIP"];
            string ftpUserID = ConfigurationManager.AppSettings["FTPAdmin"];
            string ftpPassword = ConfigurationManager.AppSettings["FTPPwd"];
            string FTPPath = ConfigurationManager.AppSettings["FTPWRFPath"];
            string url = "ftp://" + ftpServerIP + FTPPath + foldername + "/";
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                string line = reader.ReadLine();
                while (line != null)
                {
                    if ((line.IndexOf("JC_WRF_CHEM") >= 0 && line.IndexOf("SX_Jincheng") < 0) || line.IndexOf("Z_SEVP_C") >= 0)
                    {
                        list.Add(line);
                    }
                    line = reader.ReadLine();
                }

                reader.Close();
                response.Close();
                return list;
            }
            catch (Exception ex)
            {
                //writelog("获取下载文件列表出错：" + ex.Message);
                list = null;
                return list;
            }
        }

        private string[,] ParseWRFTxt(string filepath)
        {
            string str = File.ReadAllText(filepath);
            string[] rowarr = str.Split('\n');
            string[,] recordarrs = new string[rowarr.Length - 2, 16];
            for (int i = 1; i < rowarr.Length - 1; i++)
            {
                string[] recordarr = rowarr[i].Split(' ');
                List<string> list = new List<string>();
                for (int j = 0; j < recordarr.Length; j++)
                {
                    if (recordarr[j] == "") continue;
                    list.Add(recordarr[j]);
                }
                recordarr = list.ToArray();
                if (recordarr.Length > 0)
                {
                    for (int j = 0; j < recordarr.Length; j++)
                    {
                        recordarrs[i - 1, j] = recordarr[j];
                    }
                }
            }
            return recordarrs;
        }


        /// <summary>
        /// 读取XML
        /// </summary>
        public void ReadXml()
        {
            XmlDocument doc = new XmlDocument();
            string path = ConfigurationManager.AppSettings["weatherData"];
            doc.Load(path);
            // 得到根节点<shuozhou>
            XmlNode xn = doc.SelectSingleNode("shuozhou");
            // 得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xn1 in xnl)
            {
                // 将节点转换为元素，便于得到节点的属性值s
                XmlElement xe = (XmlElement)xn1;
                // 得到<city>节点的属性值
                string cityName = xe.GetAttribute("cityname");
                string stateDetailed = xe.GetAttribute("stateDetailed");
                short tem1 = Convert.ToInt16(xe.GetAttribute("tem1"));
                short tem2 = Convert.ToInt16(xe.GetAttribute("tem2"));
                short temNow = Convert.ToInt16(xe.GetAttribute("temNow"));
                string windState = xe.GetAttribute("windState");
                string windDir = xe.GetAttribute("windDir");
                string windPower = xe.GetAttribute("windPower");
                string humidity = xe.GetAttribute("humidity");
                DateTime time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + xe.GetAttribute("time"));
                string stationNum = xe.GetAttribute("url");
                #region 判断数据库是否存在数据
                string sqlStr = "select count(*) from T_Mid_WeatherData where cityname=" + "'" + cityName + "'" + " and time=" + "'" + time + "'";
                //int count = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sqlStr));
                //if (count > 0)
                //{
                //    if (MessageBox.Show("站点：" + cityName + ";日期：" + time + "数据已存在！") == DialogResult.OK)
                //    {
                //        continue;
                //    }
                //}
                #endregion
                string sql = string.Format("insert into T_Mid_WeatherData(cityname,stateDetailed,tem1,tem2,temNow,windState,windDir,windPower,humidity,time,stationNum) values('{0}','{1}',{2},{3},{4},'{5}','{6}','{7}','{8}','{9}','{10}')", cityName, stateDetailed, tem1, tem2, temNow, windState, windDir, windPower, humidity, time, stationNum);
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
                writelog("下载气象数据成功：" + time.ToString("yyyy-MM-dd hh:mm:ss"));
            }
        }

        private string getRomeLevel(int num)
        {
            switch (num)
            {
                case 1:
                    return "I";
                case 2:
                    return "I";
                case 3:
                    return "III";
                case 4:
                    return "IV";
                case 5:
                    return "V";
                case 6:
                    return "VI";
                default:
                    return "I";
            }
        }


    }
}
