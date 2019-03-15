﻿using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataRepair
{
    public class AirForCast
    {
        public string con = "";
        public DateTime startTime, endTime, maxTime;

        /// <summary>
        /// 将文本文件数据存入数据库
        /// </summary>
        public void InsertAirForeCastData()
        {
            string SavePath = ConfigurationManager.AppSettings["AirForeCastData"];
            string[] fileinfo = GetFTPFileList();
            string[] filename = new string[fileinfo.Length];
            string[] fileCreateTime = new string[fileinfo.Length];
            for (int i = 0; i < fileinfo.Length; i++)
            {
                string fi = fileinfo[i];
                string[] arr = fi.Split(',');
                filename[i] = arr[0];
                fileCreateTime[i] = arr[1];
                DownloadFile(SavePath, filename[i]);
            }

            for (int i = 0; i < filename.Length; i++)
            {
                //string MoniDate = Convert.ToDateTime(fileCreateTime[i]).ToString("yyyy-MM-dd HH");
                //string ForecastTime = DateTime.Now.ToString("yyyy-MM-dd HH");
                string ForecastTime = Convert.ToDateTime(fileCreateTime[i]).ToString("yyyy-MM-dd HH");
                string MoniDate = filename[i].Substring(11, 4) + "-" + filename[i].Substring(15, 2) + "-" + filename[i].Substring(17, 2) + " 00";

                string[,] filedarr = ParseTxt(SavePath + filename[i]);
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
                    if (IsExistForeCast(stationname, MoniDate + ":00:00", ForecastTime + ":00:00",1))
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
                    writelog(strSql);
                }
            }
        }

        public void InsertAirForeCastWRFData()
        {
            string SavePath = ConfigurationManager.AppSettings["AirForeCastData"];
            string FTPPath = ConfigurationManager.AppSettings["FTPWRFPath"];
            string ExistFolder = GetExistFolder(2);
            string[] folders = GetFTPWRFFolder(FTPPath);
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
                    string PublicTime = folders[i].Substring(9, 4) + "-" + folders[i].Substring(13, 2) + "-" + folders[i].Substring(15, 2)+" 08:00:00";
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
                        if (filedarr[k, 2]=="1")
                            strSql += "'',";
                        else
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

        /// <summary>
        /// 将文本文件SSCI数据存入数据库
        /// </summary>
        public void InsertAirForeCastSSCIData()
        {
            string SavePath = ConfigurationManager.AppSettings["AirForeCastData"];
            string FTPPath = ConfigurationManager.AppSettings["FTPSSCIPath"];
            string[] fileinfo = GetFTPSSCIFileListAll();
            if (fileinfo.Length == 0) return;
            string[] filename = new string[fileinfo.Length];
            string[] fileCreateTime = new string[fileinfo.Length];
            for (int i = 0; i < fileinfo.Length; i++)
            {
                string fi = fileinfo[i];
                string[] arr = fi.Split(',');
                filename[i] = arr[0];
                fileCreateTime[i] = arr[1];
                if (!File.Exists(SavePath + "SSCI/" + filename[i]))
                {
                    DownloadFile(SavePath + "SSCI/", filename[i], FTPPath);
                }
            }

            for (int i = 0; i < filename.Length; i++)
            {
                //string MoniDate = Convert.ToDateTime(fileCreateTime[i]).ToString("yyyy-MM-dd HH");
                //string ForecastTime = DateTime.Now.ToString("yyyy-MM-dd HH");
                if (Int32.Parse(filename[i].Substring(11,8)) < 20150518) continue;
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
                            else if (Int32.Parse(stationcode) < 59)
                            {
                                string strselect = "select stationname from T_Bas_AirStation where StationCode = '" + stationcode + "'";
                                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    stationname = ds.Tables[0].Rows[0]["stationname"].ToString();
                                }
                            }
                            else if (stationcode == "99")
                            {
                                string strselect = "select StationCode,StationName from T_Bas_AirStation where StationType = 2 order by StationCode";
                                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    stationname = ds.Tables[0].Rows[k-7]["StationName"].ToString();
                                    stationcode = ds.Tables[0].Rows[k-7]["StationCode"].ToString();
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
                    if (IsExistForeCast1(stationname, MoniDate + ":00:00", ForecastTime + ":00:00",0))
                    {
                        writelog("获取统计结果跳过--" + filename[i] + "已存在");
                        continue;
                    }
                    if (filedarr[k, 1] == "NaN")
                    {
                        continue;
                    }
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
                    writelog(strSql);
                }
                writelog("获取统计结果成功--" + filename[i]);
            }
        }

        public void InsertAirForeCastTJData()
        {
            string folder = ConfigurationManager.AppSettings["AirForeCastData"];
            string date = DateTime.Now.ToShortDateString();
            string file = folder + "ybSHUOZHOU_" + date + ".xlsx";
            if (File.Exists(file))
            {
                string forecastTime = System.Text.RegularExpressions.Regex.Match(file, @"\d{4}-\d{1,2}-\d{1,2}").Value;
                ImportData(file, forecastTime);
            }
        }

        public void InsertAirForeCastTJData1()
        {
            string folder = ConfigurationManager.AppSettings["AirForeCastData"] + "SSCI\\";
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string file = "ybSHUOZHOU_" + date + ".xlsx";
            DownloadExcel(folder , file);
            file = folder + file;
            if (File.Exists(file))
            {
                string forecastTime = System.Text.RegularExpressions.Regex.Match(file, @"\d{4}-\d{1,2}-\d{1,2}").Value;
                ImportData(file, forecastTime);
            }
        }

        public void ImportData(string path, string time)
        {
            DateTime forecastTime = Convert.ToDateTime(time);
            DataTable dt = ExcelToDataTable(path);
            try
            {
                for (int i = 0; i <= 5; i++)
                {
                    for (int j = 0; j <= 8; j++)
                    {

                        string stationName = dt.Rows[i + 4][0].ToString().Substring(2) == "市平均" ? "朔州市" : dt.Rows[i + 4][0].ToString().Substring(2);//站点名称
                        string stationCode = GetStationCode(dt.Rows[i + 4][1].ToString());//站点编号
                        object SO2AQI = IsNull(dt.Rows[4 + i][6 + 2 * j]);//SO2AQI
                        object SO2 = IsNull(dt.Rows[4 + i][5 + 2 * j]);//SO2
                        object COAQI = IsNull(dt.Rows[10 + i][6 + 2 * j]);//COAQI
                        object CO = IsNull(dt.Rows[10 + i][5 + 2 * j]);//CO
                        object NO2AQI = IsNull(dt.Rows[16 + i][6 + 2 * j]);//NO2AQI
                        object NO2 = IsNull(dt.Rows[16 + i][5 + 2 * j]);//NO2
                        object PM10AQI = IsNull(dt.Rows[22 + i][6 + 2 * j]);//PM10AQI
                        object PM10 = IsNull(dt.Rows[22 + i][5 + 2 * j]);//PM10
                        object PM25AQI = IsNull(dt.Rows[28 + i][6 + 2 * j]);//PM25AQI
                        object PM25 = IsNull(dt.Rows[28 + i][5 + 2 * j]);//PM25                           
                        //CONVERT(varchar(100),MonitorTime,23)
                        string sql = "select count(*) from T_Mid_AirForeCast where StationName=" + "'" + stationName + "'" + " and MonitorTime=" + "'" + forecastTime.AddDays(j) + "'" + " and ForecastTime=" + "'" + forecastTime + "'";
                        int count = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sql));
                        if (count > 0)
                        {
                            //if (MessageBox.Show("站点：" + stationName + ";预测日期：" + forecastTime.AddDays(j).ToShortDateString() + ";预报日期：" + forecastTime.ToShortDateString() + "数据已存在！") == DialogResult.OK)
                            //{
                            //    continue;
                            //}
                             
                        }
                        string sqlstr = string.Format("insert into T_Mid_AirForeCast (StationName, StationCode," + "MonitorTime, ForecastTime, SO2AQI, SO2, COAQI, CO,NO2AQI, NO2, PM10AQI, PM10,PM25AQI," + "PM25,  AQI,AirLevel,FirstP,ForecastModel) values ('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8},{9}," +
                             "{10},{11},{12},{13},{14},{15},{16},{17})",
                             stationName, stationCode, forecastTime.AddDays(j), forecastTime, SO2AQI, SO2, COAQI, CO, NO2AQI, NO2, PM10AQI, PM10, PM25AQI, PM25, "[dbo].[GetMaxAQI](" + SO2AQI + "," + COAQI + "," + NO2AQI + "," + PM10AQI + "," + PM25AQI + ",''" + ")", "[dbo].[fun_AirAQIdegree](" + "[dbo].[GetMaxAQI](" + SO2AQI + "," + COAQI + "," + NO2AQI + "," + PM10AQI + "," + PM25AQI + ",''" + ")" + ")", "[dbo].[fun_AQI_FristPollutant](" + SO2 + "," + CO + "," + NO2 + "," + PM10 + "," + PM25 + ",0" + ",0" + ")", 0);
                        SqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr);

                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("错误\n" + ex.ToString(), "!");
                writelog("错误\n" + ex.ToString());
                return;
            }
        }
        /// <summary>
        /// 站点变更
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetStationCode(string code)
        {
            string codeUp = "";
            switch (code)
            {
                case "2":
                    codeUp = "57";
                    break;
                case "3":
                    codeUp = "58";
                    break;
                case "4":
                    codeUp = "53";
                    break;
                case "5":
                    codeUp = "55";
                    break;
                case "6":
                    codeUp = "56";
                    break;
                default:
                    codeUp = "00";
                    break;
            }
            return codeUp;
        }
        /// <summary>
        /// 判断表的值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public object IsNull(object val)
        {
            return Regex.IsMatch(val.ToString(), @"[0-9]{1,4}\.{0,1}[0-9]{0,2}") == false ? "null" : val;
        }
        public object IsNum(object val)
        {
            return Regex.IsMatch(val.ToString(), @"[0-9]{1,4}\.{0,1}[0-9]{0,2}") == false ? "0" : val;
        }
        /// <summary>
        /// 将excel数据存到DataTable
        /// </summary>
        public DataTable ExcelToDataTable(string file)
        {
            DataTable dt = new System.Data.DataTable();
            Aspose.Cells.Workbook workBook = new Aspose.Cells.Workbook();

            workBook.Open(file, FileFormatType.Excel2007Xlsx);
            Aspose.Cells.Worksheet sheet = workBook.Worksheets[1];
            Aspose.Cells.Cells cells = sheet.Cells;
            dt = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
            return dt;
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
                    string stationcode =Int32.Parse(arr[2]).ToString();
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

        public void InsertAirForeCastWRFDataNew()
        {
            string SavePath = ConfigurationManager.AppSettings["AirForeCastData"];
            string FTPPath = ConfigurationManager.AppSettings["FTPWRFPath"];
            maxTime = DateTime.Parse(GetNewForCastTime(2));
            //string ExistFolder = GetExistFolder(2);
            string[] folders = GetFTPWRFFolder(FTPPath);
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
                    //string stationcode = arr[3].Substring(arr[3].Length - 2, 2);
                    string stationcode = int.Parse(arr[3]).ToString();
                    if (stationcode == "0") stationcode += "0";
                    string stationname = string.Empty;
                    string strselect = "select stationname,StationType from T_Bas_AirStation where StationCode = '" + stationcode + "'";
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strselect);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        stationname = ds.Tables[0].Rows[0]["stationname"].ToString();
                        if (ds.Tables[0].Rows[0]["StationType"].ToString() == "-1")
                            continue;
                    }
                    else
                    {
                        continue;
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
                        if (filedarr[k, 2] == "1")
                            strSql += "'',";
                        else
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
                    if (ExistCMAQFolder.IndexOf(line.Substring(0, 10)) < 0)
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
                    if ((line.IndexOf("Z_SXSZ_ENVAQFC") >= 0 || line.IndexOf("SZ_ENVAQFC") >= 0 )&& line.IndexOf("daily") < 0)
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
        /// 解析SSCI文本文件
        /// </summary>
        /// <param name="filepath">文本文件路径加文件名</param>
        /// <returns>二维数组</returns>
        private string[,] ParseSSCITxt(string filepath)
        {
            string str = File.ReadAllText(filepath);
            //str = str.Substring(0, str.IndexOf("099") - 1);
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
        /// 获取最新的预测结果时间点
        /// </summary>
        /// <returns>返回最新时间</returns>
        private string GetNewForCastTime(int datatype)
        {
            string sql = "SELECT top 1 ForecastTime FROM T_Mid_AirForeCast WHERE ForecastModel = " + datatype + " ORDER BY ForecastTime DESC";
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            string time = ds.Tables[0].Rows[0][0].ToString();
            return time;
        }

        private string[] GetFTPWRFFolder(string FTPPath)
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
                    //if (ExistFolder.IndexOf(arr[2]) < 0)
                    if (WRF_IsResult(arr[2]))
                    {
                        result.Append(line);
                        result.Append("\n");
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
        /// FTP从服务器上下载文件
        /// </summary>
        private void DownloadFile(string filepath, string fileName)
        {
            FtpWebRequest reqFTP;
            string ftpServerIP = ConfigurationManager.AppSettings["FTPIP"];
            string ftpUserID = ConfigurationManager.AppSettings["FTPAdmin"];
            string ftpPassword = ConfigurationManager.AppSettings["FTPPwd"];
            string FTPPath = ConfigurationManager.AppSettings["FTPPath"];
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

        private void DownloadExcel(string filepath, string fileName)
        {
            FtpWebRequest reqFTP;
            string ftpServerIP = ConfigurationManager.AppSettings["tjFTPIP"];
            string ftpUserID = ConfigurationManager.AppSettings["tjFTPAdmin"];
            string ftpPassword = ConfigurationManager.AppSettings["tjFTPPwd"];
            string FTPPath = ConfigurationManager.AppSettings["tjFilePath"];
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

        /// <summary>
        /// 解析文本文件
        /// </summary>
        /// <param name="filepath">文本文件路径加文件名</param>
        /// <returns>二维数组</returns>
        private string[,] ParseTxt(string filepath)
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


        private Boolean IsExistForeCast(string name, string monitortime, string forecasttime, int forecastmodel)
        {
            string table = "T_Mid_AirForeCast";
            if (forecastmodel > 0) table = "T_Mid_AirForeCastHour";
            string sql = "select * from " + table + " where StationName='" + name + "' and MonitorTime='" + monitortime + "' and ForecastTime='" + forecasttime + "' and ForecastModel = " + forecastmodel;
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

        private Boolean IsExistForeCast1(string name, string monitortime, string forecasttime, int forecastmodel)
        {
            string sql = "select * from T_Mid_AirForeCast where StationName='" + name + "' and MonitorTime='" + monitortime + "' and ForecastModel = " + forecastmodel;
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
        /// 获取FTP下载目录文件列表
        /// </summary>
        /// <returns>符合条件的文件列表</returns>
        private string[] GetFTPFileList()
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            string ftpServerIP = ConfigurationManager.AppSettings["FTPIP"];
            string ftpUserID = ConfigurationManager.AppSettings["FTPAdmin"];
            string ftpPassword = ConfigurationManager.AppSettings["FTPPwd"];
            string FTPPath = ConfigurationManager.AppSettings["FTPPath"];
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
                        if (datetime >= dt)
                        {
                            //if (datetime > dt) result.Clear();
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
        /// 获取FTP中SSCI下载目录文件列表(获取所有文件)
        /// </summary>
        /// <returns>符合条件的文件列表</returns>
        private string[] GetFTPSSCIFileListAll()
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
                maxTime = Convert.ToDateTime(GetNewForCastTime(0));
                while (line != null)
                {
                    if (line.IndexOf("JC_SSCD_JC_") > 0)
                    {
                        string[] arr = line.Split(' ');
                        string iurl = "ftp://" + ftpServerIP + FTPPath +"//"+ arr[arr.Length-1];
                        FtpWebRequest reqFTP1;
                        reqFTP1 = (FtpWebRequest)FtpWebRequest.Create(new Uri(iurl));
                        reqFTP1.UseBinary = true;
                        reqFTP1.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                        reqFTP1.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                        reqFTP1.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                        FtpWebResponse ftpresponse = (FtpWebResponse)reqFTP1.GetResponse();
                        DateTime datetime = ftpresponse.LastModified;
                        //CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                        //string format = "yyyy-MM-dd HH:mm:ss";
                        //string format = "MMM d HH:mm";
                        //string stringValue = DateTime.Now.ToString(format, cultureInfo); // 得到日期字符串
                        //DateTime datetime = Convert.ToDateTime(stringValue);
                        //DateTime datetime = DateTime.ParseExact(arr[23] + " " + arr[24] + " " + arr[25], format, cultureInfo);
                        //if (datetime.Month > 10)
                        //    datetime = DateTime.Parse("2014-" + datetime.Month + "-" + datetime.Day + " " + datetime.Hour + ":" + datetime.Minute + ":" + datetime.Second);
                        if (startTime > Convert.ToDateTime("2015-1-1"))
                        {
                            if (datetime >= startTime && datetime <= endTime)
                            {
                                filename = arr[arr.Length - 1];
                                result.Append(filename + "," + datetime.ToString());
                                result.Append("\n");
                            }
                        }
                        else
                        {
                            if (datetime > maxTime)
                            {
                                filename = arr[arr.Length - 1];
                                result.Append(filename + "," + datetime.ToString());
                                result.Append("\n");
                            }
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

        private bool WRF_IsResult(string folderdate)
        {
            bool isResult = false;
            if (startTime>Convert.ToDateTime("2015-1-1"))
            {
                if (int.Parse(folderdate) >= int.Parse(startTime.ToString("yyyyMMdd")) && int.Parse(folderdate) <= int.Parse(endTime.ToString("yyyyMMdd")))
                    isResult = true;
            }
            else
            {
                if (int.Parse(folderdate) > int.Parse(maxTime.ToString("yyyyMMdd")))
                {
                    isResult = true;
                }
            }
            return isResult;
        }

    }
}