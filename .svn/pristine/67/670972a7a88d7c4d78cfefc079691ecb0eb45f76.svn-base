using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataRepair
{
    public class AirMonitor
    {
        public string con = "";

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
                string strselect = "select * from T_Mid_AirDayData where StationName = '" + dt.Rows[i]["\"position_name\""] + "' and MoniDate = '" + dt.Rows[i]["\"time\""] + ":00:00'";
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
                    writelog(strSql);
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
                    strSql += SO2 * 1000 + ",";
                    strSql += NO2 * 1000 + ",";
                    strSql += dt.Rows[i]["\"co\""] + ",";
                    strSql += O3 * 1000 + ",";
                    strSql += PM25 * 1000 + ",";
                    strSql += PM10 * 1000 + ",";
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
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strSql);
                    writelog(strSql);

                    string strdate = dt.Rows[i]["\"time\""].ToString().Substring(0, dt.Rows[i]["\"time\""].ToString().Length - 2);
                    string strfile = new DateTime(Int32.Parse(strdate.Split('/')[0]), Int32.Parse(strdate.Split('/')[1]), Int32.Parse(strdate.Split('/')[2])).ToString("yyyy-MM-dd");
                    //txt
                    //string col = "time SO2 NO2 CO O3 PM25 PM10";
                    //writeAirData(dt.Rows[i]["\"position_name\""].ToString(), col, false, strfile);//保存txt文件到linux服务器上
                    //string data = dt.Rows[i]["\"time\""] + ":00:00 " + dt.Rows[i]["\"so2\""].ToString() + " " + dt.Rows[i]["\"no2\""].ToString() + " " + dt.Rows[i]["\"co\""].ToString() + " " + dt.Rows[i]["\"o3\""].ToString() + " " + dt.Rows[i]["\"pm25\""].ToString() + " " + dt.Rows[i]["\"pm10\""].ToString();
                    //writeAirData(dt.Rows[i]["\"position_name\""].ToString(), data, true, strfile);
                }

            }

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
                    //string col = "time SO2 NO2 CO O3 PM25 PM10";
                    //writeAirData(dt.Rows[i]["\"position_name\""].ToString().Substring(0, 2), col, false, strfile);//保存txt文件到linux服务器上
                    //string data = dt.Rows[i]["\"time\""] + ":00:00 " + dt.Rows[i]["\"so2\""].ToString() + " " + dt.Rows[i]["\"no2\""].ToString() + " " + dt.Rows[i]["\"co\""].ToString() + " " + dt.Rows[i]["\"o3_1\""].ToString() + " " + dt.Rows[i]["\"pm25\""].ToString() + " " + dt.Rows[i]["\"pm10\""].ToString();
                    //writeAirData(dt.Rows[i]["\"position_name\""].ToString().Substring(0, 2), data, true, strfile);
                }

            }
        }


        public void getCountyAirData(double days)
        {
            RemoteService.ServiceSoap ser = new RemoteService.ServiceSoapClient();
            RemoteService.GetXianValueRequest request = new RemoteService.GetXianValueRequest(new RemoteService.GetXianValueRequestBody(DateTime.Now.AddDays(days).ToShortDateString().ToString()));
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
                    strSql += "'" + dt.Rows[i]["\"position_name\""].ToString().Replace(" ","") + "',";
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
                    writelog(strSql);
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strSql);

                    string strdate = dt.Rows[i]["\"time\""].ToString().Substring(0, dt.Rows[i]["\"time\""].ToString().Length-2);
                    string strfile = new DateTime(Int32.Parse(strdate.Split('/')[0]), Int32.Parse(strdate.Split('/')[1]), Int32.Parse(strdate.Split('/')[2])).ToString("yyyy-MM-dd");
                    //txt
                    //string col = "time SO2 NO2 CO O3 PM25 PM10";
                    //writeAirData(dt.Rows[i]["\"position_name\""].ToString().Substring(0, 2), col, false, strfile);//保存txt文件到linux服务器上
                    //string data = dt.Rows[i]["\"time\""] + ":00:00 " + dt.Rows[i]["\"so2\""].ToString() + " " + dt.Rows[i]["\"no2\""].ToString() + " " + dt.Rows[i]["\"co\""].ToString() + " " + dt.Rows[i]["\"o3_1\""].ToString() + " " + dt.Rows[i]["\"pm25\""].ToString() + " " + dt.Rows[i]["\"pm10\""].ToString();
                    //(dt.Rows[i]["\"position_name\""].ToString().Substring(0, 2), data, true, strfile);
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


        public void writeAirData(string StationName, string txt,bool up,string date)
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

    }
}
