﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Aspose.Cells;
using System.Xml;


namespace DataRepair
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer timer = null;
        private static string con = "";
        public Form1()
        {
            InitializeComponent();
            con = ConfigurationManager.ConnectionStrings["DB"].ToString();
        }
        /// <summary>
        /// 补数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            DateTime StartTime = timeStart.Value;
            DateTime EndTime = timeEnd.Value;
            listBox1.Items.Add("..........");

            //空气质量监测数据 
            if (rbAirMon.Checked)
            {
                AirMonitor am = new AirMonitor();
                am.con = con;
                am.getAirHourData();
                //am.getAirDayData();
                //am.getCountyAirData();
                //TimeSpan ts = timeEnd.Value - timeStart.Value;
                //int days = int.Parse(ts.TotalDays.ToString().Split('.')[0]);
                //for (int i = 0; i < days; i++)
                //{
                //    am.getCountyAirData(0 - i);
                //}
                listBox1.Items.Add("空气质量检测数据更新成功！");
            }
            //空气质量预报数据
            if (rbAirForCast.Checked)
            {
                AirForCast afc = new AirForCast();
                afc.con = con;
                if (rbNew.Checked)
                {
                    //afc.InsertAirForeCastWRFDataNew();
                    //afc.InsertAirForeCastSSCIData();
                    afc.InsertAirForeCastCMAQData();
                }
                if (rbOld.Checked)
                {

                    DateTime start = timeStart.Value.AddDays(-1);
                    DateTime end = timeEnd.Value;                   
                    afc.startTime = start;
                    afc.endTime = end;
                    //afc.InsertAirForeCastWRFDataNew();
                    afc.InsertAirForeCastSSCIData();

                  
                    afc.con = con;
                    if (rbNew.Checked)
                    {
                        //afc.InsertAirForeCastWRFDataNew();
                        afc.InsertAirForeCastCMAQData();
                        //afc.InsertAirForeCastSSCIData();
                    }
                    if (rbOld.Checked)
                    {
                        DateTime startS = timeStart.Value.AddDays(-1);
                        DateTime endE = timeEnd.Value;
                        afc.startTime = startS;
                        afc.endTime = endE;
                        //afc.InsertAirForeCastWRFDataNew();
                        afc.InsertAirForeCastSSCIData();
                    }
                    //afc.InsertAirForeCastSSCIData();
                    //afc.InsertAirForeCastCMAQData();
                    //afc.InsertAirForeCastWRFData();
                    listBox1.Items.Add("空气质量预报数据更新成功！");

                }
                //afc.InsertAirForeCastSSCIData();
                //afc.InsertAirForeCastCMAQData();
                //afc.InsertAirForeCastWRFData();
                listBox1.Items.Add("空气质量预报数据更新成功！");
            }
            //气象监测数据  
            if (rbQxMon.Checked)
            {
                //string scon = ConfigurationManager.ConnectionStrings["sourceDB"].ToString();
                //string tcon = ConfigurationManager.ConnectionStrings["targetDB"].ToString();
                //QxMonitor qm = new QxMonitor();
                //qm.scon = scon;
                //qm.tcon = tcon;
                //qm.getQxMonitorData();
                //listBox1.Items.Add("气象监测数据更新成功！");
                ReadXml();
                listBox1.Items.Add("气象监测数据更新成功！");

            }
            //气象预报报文 
            if (rbQxForCast.Checked)
            {
                string scon = ConfigurationManager.ConnectionStrings["sourceDB"].ToString();
                string tcon = ConfigurationManager.ConnectionStrings["targetDB"].ToString();
                QxForeCast qf = new QxForeCast();
                qf.scon = scon;
                qf.tcon = tcon;
                qf.getQxPacket();
                listBox1.Items.Add("气象预报报文更新成功！");
            }

            //气象监测资料 
            if (rbQxFile.Checked)
            {
                QxFiles qf = new QxFiles();
                //qf.getShareFile();
                qf.ClearFile();
                listBox1.Items.Add("气象监测资料更新成功！");
            }
                    //QxImg qximg = new QxImg();
                    //qximg.DownLoadSoundingImg();
            //气象监测图片
            if (rbQxImg.Checked)
            {

            }
            if (rbCityAir.Checked)
            {
                getCityAirHtml(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                listBox1.Items.Add("全国空气质量获取成功！");
            }
            //统计预报数据
            if (rbQxTj.Checked)
            {
                if (!string.IsNullOrEmpty(txtPath.Text.Trim()))
                {

                    string forecastTime = System.Text.RegularExpressions.Regex.Match(txtPath.Text, @"\d{4}-\d{1,2}-\d{1,2}").Value;
                    ImportData(txtPath.Text, forecastTime);
                    listBox1.Items.Add("统计预报数据更新成功！");

                    //getCityAirHtml("2015-10-01");

                }
                else
                {

                    MessageBox.Show("请选择Excel文件！");
          AirForCast afc = new AirForCast();
                    afc.con = con;
                    //afc.InsertAirForeCastTJData();
                    listBox1.Items.Add("统计预测数据获取成功！");
                    return;
                    if (!string.IsNullOrEmpty(txtPath.Text.Trim()))
                    {
                        string forecastTime = System.Text.RegularExpressions.Regex.Match(txtPath.Text, @"\d{4}-\d{1,2}-\d{1,2}").Value;
                        ImportData(txtPath.Text, forecastTime);
                        listBox1.Items.Add("统计预报数据更新成功！");
                    }
                    else
                    {
                        MessageBox.Show("请选择Excel文件！"); 
                    }
                    

                }

            }

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
                DateTime time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ")+xe.GetAttribute("time"));
                string stationNum = xe.GetAttribute("url");

                #region 判断数据库是否存在数据
                string sqlStr = "select count(*) from T_Mid_WeatherData where cityname=" + "'" + cityName + "'" + " and time=" + "'" + time + "'";
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sqlStr));
                if (count > 0)
                {
                    if (MessageBox.Show("站点：" + cityName + ";日期：" + time + "数据已存在！") == DialogResult.OK)
                    {
                        continue;
                    }
                } 
                #endregion
                string sql = string.Format("insert into T_Mid_WeatherData(cityname,stateDetailed,tem1,tem2,temNow,windState,windDir,windPower,humidity,time,stationNum) values('{0}','{1}',{2},{3},{4},'{5}','{6}','{7}','{8}','{9}','{10}')", cityName, stateDetailed, tem1, tem2, temNow, windState, windDir, windPower, humidity, time, stationNum);
                 SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
            }
        }
      
        public static bool IsUrlExist(string Url)
        {
            bool IsExist = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(Url));
            ServicePointManager.Expect100Continue = false;
            try
            {
                ((HttpWebResponse)request.GetResponse()).Close();
                IsExist = true;
            }
            catch (WebException exception)
            {
                //if (exception.Status != WebExceptionStatus.ProtocolError)
                //{
                //    return num;
                //}
                //if (exception.Message.IndexOf("500 ") > 0)
                //{
                //    return 500;
                //}
                //if (exception.Message.IndexOf("401 ") > 0)
                //{
                //    return 401;
                //}
                //if (exception.Message.IndexOf("404") > 0)
                //{
                //    num = 404;
                //}
                IsExist = false;
            }
            return IsExist;
        }

        private static bool UrlIsExist(String url)
        {
            System.Uri u = null;
            try
            {
                u = new Uri(url);
            }
            catch { return false; }
            bool isExist = false;
            System.Net.HttpWebRequest r = System.Net.HttpWebRequest.Create(u) as System.Net.HttpWebRequest;
            r.Method = "HEAD";
            try
            {
                System.Net.HttpWebResponse s = r.GetResponse() as System.Net.HttpWebResponse;
                if (s.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    isExist = true;
                }
            }
            catch (System.Net.WebException x)
            {
                try
                {
                    isExist = ((x.Response as System.Net.HttpWebResponse).StatusCode != System.Net.HttpStatusCode.NotFound);
                }
                catch { isExist = (x.Status == System.Net.WebExceptionStatus.Success); }
            }
            return isExist;
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
                    if (html.Length > 0)
                        ReadHtml(html);
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

        public string ReadHtml(string strHtml)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(strHtml);

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                if (table.Id == "report1")
                {
                    foreach (HtmlNode row in table.SelectNodes("tr"))
                    {
                        if (row.Attributes[0].Value == "30")
                        {
                            string strInsert = "insert into [dbo].[T_Mid_CityDayData](CityName,MonitorTime,AQI,FirstP,AirLevel) values(";
                            string strValue = "";
                            foreach (HtmlNode cell in row.SelectNodes("th|td"))
                            {
                                if (cell.Attributes[0].Value == "report1_5")
                                    strValue += "'" + cell.InnerText + "',";
                                else if (cell.Attributes[0].Name == "onmouseover")
                                {
                                    if (cell.Attributes[1].Value == "report1_5")
                                        strValue += "'" + cell.InnerText + "',";
                                    else if (cell.Attributes[1].Value == "report1_3")
                                        strValue += "'" + cell.InnerText + "',";
                                    else if (cell.Attributes[1].Value == "report1_4")
                                        strValue += "'" + cell.InnerText + "',";
                                    else if (cell.Attributes[1].Value == "report1_2")
                                        strValue += "'" + cell.InnerText + "',";
                                }
                                else if (cell.Attributes[0].Value == "report1_3")
                                    strValue += "'" + cell.InnerText + "',";
                                else if (cell.Attributes[0].Value == "report1_4")
                                    strValue += "'" + cell.InnerText + "',";
                                else if (cell.Attributes[0].Value == "report1_2")
                                    strValue += "'" + cell.InnerText + "',";
                            }
                            if (strValue.Length > 0 && strValue.IndexOf("序号") == -1)
                            {
                                strInsert += strValue.Substring(0, strValue.Length - 1) + ")";
                                try
                                {
                                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strInsert);
                                }
                                catch (Exception)
                                {
                                    
                                }

                            }
                        }
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "选择要解析的Excel文件";
            file.InitialDirectory = @"C:\Users\Administrator\Desktop";
            file.Multiselect = false;
            file.Filter = "Excel文件|*.xlsx";
            file.ShowDialog();
            txtPath.Text = file.FileName;

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
        //<summary>
        //向数据库导入数据
        //</summary>
        public void ImportData(string path, string time)
        {
            DateTime forecastTime = Convert.ToDateTime(time);
            DataTable dt = ExcelToDataTable(path);
            int countColumns = dt.Columns.Count;
            int countRows = dt.Rows.Count;
            int end = 0;
            if (countColumns == 24)
            {
                end = 6;
            }
            else if (countColumns == 28)
            {
                end = 8;
            }
            else if (countColumns == 26)
            {
                end = 7;
            }
            else if (countColumns == 22)
            {
                end = 5;
            }
            try
            {
                if (countRows == 34)
                {
                    for (int i = 0; i <= 5; i++)
                    {
                        for (int j = 0; j <= end; j++)
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
                                if (MessageBox.Show("站点：" + stationName + ";预测日期：" + forecastTime.AddDays(j).ToShortDateString() + ";预报日期：" + forecastTime.ToShortDateString() + "数据已存在！") == DialogResult.OK)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                string sqlstr = string.Format("insert into T_Mid_AirForeCast (StationName, StationCode," + "MonitorTime, ForecastTime, SO2AQI, SO2, COAQI, CO,NO2AQI, NO2, PM10AQI, PM10,PM25AQI," + "PM25,  AQI,AirLevel,FirstP,ForecastModel) values ('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8},{9}," +
                                     "{10},{11},{12},{13},{14},{15},{16},{17})",
                                     stationName, stationCode, forecastTime.AddDays(j), forecastTime, SO2AQI, SO2, COAQI, CO, NO2AQI, NO2, PM10AQI, PM10, PM25AQI, PM25, "[dbo].[GetMaxAQI](" + SO2AQI + "," + NO2AQI + "," + COAQI + ",''," + PM25AQI + "," + PM10AQI + ")", "[dbo].[fun_AirAQILevel](" + "[dbo].[GetMaxAQI](" + SO2AQI + "," + NO2AQI + "," + COAQI + ",''," + PM25AQI + "," + PM10AQI + ")" + ")", "[dbo].[fun_AQI_FristPollutant](" + COAQI + "," + SO2AQI + "," + NO2AQI + ",0,0," + PM25AQI + "," + PM10AQI + ")", 0);
                                int o = 0;
                                SqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr);
                            }
                        }
                    }
                }
                if (countRows == 29)
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        for (int j = 0; j <= end; j++)
                        {
                            string stationName = dt.Rows[i + 4][0].ToString().Substring(2) == "市平均" ? "朔州市" : dt.Rows[i + 4][0].ToString().Substring(2);//站点名称
                            string stationCode = GetStationCode(dt.Rows[i + 4][1].ToString());//站点编号
                            object SO2AQI = IsNull(dt.Rows[4 + i][6 + 2 * j]);//SO2AQI
                            object SO2 = IsNull(dt.Rows[4 + i][5 + 2 * j]);//SO2
                            object COAQI = IsNull(dt.Rows[9 + i][6 + 2 * j]);//COAQI
                            object CO = IsNull(dt.Rows[9 + i][5 + 2 * j]);//CO
                            object NO2AQI = IsNull(dt.Rows[14 + i][6 + 2 * j]);//NO2AQI
                            object NO2 = IsNull(dt.Rows[14 + i][5 + 2 * j]);//NO2
                            object PM10AQI = IsNull(dt.Rows[19 + i][6 + 2 * j]);//PM10AQI
                            object PM10 = IsNull(dt.Rows[19 + i][5 + 2 * j]);//PM10
                            object PM25AQI = IsNull(dt.Rows[24 + i][6 + 2 * j]);//PM25AQI
                            object PM25 = IsNull(dt.Rows[24 + i][5 + 2 * j]);//PM25                           
                            //CONVERT(varchar(100),MonitorTime,23)
                            string sql = "select count(*) from T_Mid_AirForeCast where StationName=" + "'" + stationName + "'" + " and MonitorTime=" + "'" + forecastTime.AddDays(j) + "'" + " and ForecastTime=" + "'" + forecastTime + "'";
                            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sql));
                            if (count > 0)
                            {
                                if (MessageBox.Show("站点：" + stationName + ";预测日期：" + forecastTime.AddDays(j).ToShortDateString() + ";预报日期：" + forecastTime.ToShortDateString() + "数据已存在！") == DialogResult.OK)
                                {
                                    continue;
                                }
                            }
                            string sqlstr = string.Format("insert into T_Mid_AirForeCast (StationName, StationCode," + "MonitorTime, ForecastTime, SO2AQI, SO2, COAQI, CO,NO2AQI, NO2, PM10AQI, PM10,PM25AQI," + "PM25,  AQI,AirLevel,FirstP,ForecastModel) values ('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8},{9}," +
                                 "{10},{11},{12},{13},{14},{15},{16},{17})",
                                 stationName, stationCode, forecastTime.AddDays(j), forecastTime, SO2AQI, SO2, COAQI, CO, NO2AQI, NO2, PM10AQI, PM10, PM25AQI, PM25, "[dbo].[GetMaxAQI](" + SO2AQI + "," + COAQI + "," + NO2AQI + "," + PM10AQI + "," + PM25AQI + ",''" + ")", "[dbo].[fun_AirAQILevel](" + "[dbo].[GetMaxAQI](" + SO2AQI + "," + COAQI + "," + NO2AQI + "," + PM10AQI + "," + PM25AQI + ",''" + ")" + ")", "[dbo].[fun_AQI_FristPollutant](" + SO2 + "," + CO + "," + NO2 + "," + PM10 + "," + PM25 + ",0" + ",0" + ")", 0);
                            int o = 0;
                            SqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr);

                        }
                    }
                } if (countRows == 30)
                {
                    for (int i = 0; i <= 5; i++)
                    {
                        for (int j = 0; j <= end; j++)
                        {
                            if (i == 0)
                            {
                                string stationName = "平朔";//站点名称
                                string stationCode = GetStationCode("2");//站点编号
                                object SO2AQI = "null";//SO2AQI
                                object SO2 = "null";//SO2
                                object COAQI = "null";//COAQI
                                object CO = "null";//CO
                                object NO2AQI = "null";//NO2AQI
                                object NO2 = "null";//NO2
                                object PM10AQI = "null";//PM10AQI
                                object PM10 = "null";//PM10
                                object PM25AQI = IsNull(dt.Rows[24 + i][6 + 2 * j]);//PM25AQI
                                object PM25 = IsNull(dt.Rows[24 + i][5 + 2 * j]);//PM25 
                                string sql = "select count(*) from T_Mid_AirForeCast where StationName=" + "'" + stationName + "'" + " and MonitorTime=" + "'" + forecastTime.AddDays(j) + "'" + " and ForecastTime=" + "'" + forecastTime + "'";
                                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sql));
                                if (count > 0)
                                {
                                    if (MessageBox.Show("站点：" + stationName + ";预测日期：" + forecastTime.AddDays(j).ToShortDateString() + ";预报日期：" + forecastTime.ToShortDateString() + "数据已存在！") == DialogResult.OK)
                                    {
                                        continue;
                                    }
                                }
                                string sqlstr = string.Format("insert into T_Mid_AirForeCast (StationName, StationCode," + "MonitorTime, ForecastTime, SO2AQI, SO2, COAQI, CO,NO2AQI, NO2, PM10AQI, PM10,PM25AQI," + "PM25,  AQI,AirLevel,FirstP,ForecastModel) values ('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8},{9}," +
                                     "{10},{11},{12},{13},{14},{15},{16},{17})",
                                     stationName, stationCode, forecastTime.AddDays(j), forecastTime, SO2AQI, SO2, COAQI, CO, NO2AQI, NO2, PM10AQI, PM10, PM25AQI, PM25, "[dbo].[GetMaxAQI](" + SO2AQI + "," + COAQI + "," + NO2AQI + "," + PM10AQI + "," + PM25AQI + ",''" + ")", "[dbo].[fun_AirAQILevel](" + "[dbo].[GetMaxAQI](0,0,0,0" + "," + PM25AQI + ",''" + ")" + ")", "[dbo].[fun_AQI_FristPollutant](0,0,0,0" + "," + PM25 + ",0" + ",0" + ")", 0);
                                int o = 0;
                                SqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr);
                            }
                            else
                            {
                                string stationName = dt.Rows[i + 4][0].ToString().Substring(2) == "市平均" ? "朔州市" : dt.Rows[i + 4][0].ToString().Substring(2);//站点名称
                                string stationCode = GetStationCode(dt.Rows[i + 4][1].ToString());//站点编号
                                object SO2AQI = IsNull(dt.Rows[4 + i][6 + 2 * j]);//SO2AQI
                                object SO2 = IsNull(dt.Rows[4 + i][5 + 2 * j]);//SO2
                                object COAQI = IsNull(dt.Rows[9 + i][6 + 2 * j]);//COAQI
                                object CO = IsNull(dt.Rows[9 + i][5 + 2 * j]);//CO
                                object NO2AQI = IsNull(dt.Rows[14 + i][6 + 2 * j]);//NO2AQI
                                object NO2 = IsNull(dt.Rows[14 + i][5 + 2 * j]);//NO2
                                object PM10AQI = IsNull(dt.Rows[19 + i][6 + 2 * j]);//PM10AQI
                                object PM10 = IsNull(dt.Rows[19 + i][5 + 2 * j]);//PM10
                                object PM25AQI = IsNull(dt.Rows[24 + i][6 + 2 * j]);//PM25AQI
                                object PM25 = IsNull(dt.Rows[24 + i][5 + 2 * j]);//PM25 
                                string sql = "select count(*) from T_Mid_AirForeCast where StationName=" + "'" + stationName + "'" + " and MonitorTime=" + "'" + forecastTime.AddDays(j) + "'" + " and ForecastTime=" + "'" + forecastTime + "'";
                                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sql));
                                if (count > 0)
                                {
                                    if (MessageBox.Show("站点：" + stationName + ";预测日期：" + forecastTime.AddDays(j).ToShortDateString() + ";预报日期：" + forecastTime.ToShortDateString() + "数据已存在！") == DialogResult.OK)
                                    {
                                        continue;
                                    }
                                }
                                string sqlstr = string.Format("insert into T_Mid_AirForeCast (StationName, StationCode," + "MonitorTime, ForecastTime, SO2AQI, SO2, COAQI, CO,NO2AQI, NO2, PM10AQI, PM10,PM25AQI," + "PM25,  AQI,AirLevel,FirstP,ForecastModel) values ('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8},{9}," +
                                     "{10},{11},{12},{13},{14},{15},{16},{17})",
                                     stationName, stationCode, forecastTime.AddDays(j), forecastTime, SO2AQI, SO2, COAQI, CO, NO2AQI, NO2, PM10AQI, PM10, PM25AQI, PM25, "[dbo].[GetMaxAQI](" + SO2AQI + "," + COAQI + "," + NO2AQI + "," + PM10AQI + "," + PM25AQI + ",''" + ")", "[dbo].[fun_AirAQILevel](" + "[dbo].[GetMaxAQI](" + SO2AQI + "," + COAQI + "," + NO2AQI + "," + PM10AQI + "," + PM25AQI + ",''" + ")" + ")", "[dbo].[fun_AQI_FristPollutant](" + SO2 + "," + CO + "," + NO2 + "," + PM10 + "," + PM25 + ",0" + ",0" + ")", 0);
                                int o = 0;
                                SqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误\n" + ex.ToString(), "!");
                return;
            }
        }
        public void ImportDB(int countColumns, int countRows, DataTable dt, DateTime forecastTime)
        {
            int end = 0;
            int row = 0;
            if (countColumns == 24)
            {
                end = 6;
            }
            else if (countColumns == 28)
            {
                end = 8;
            }
            else if (countColumns == 26)
            {
                end = 7;
            }
            else if (countColumns == 22)
            {
                end = 5;
            }
            if (countRows == 34)
            {
                row = 5;
            }
            else if (countRows == 29)
            {
                row = 4;
            }
            for (int i = 0; i <= row; i++)
            {
                for (int j = 0; j <= end; j++)
                {
                    string stationName = dt.Rows[i + 4][0].ToString().Substring(2) == "市平均" ? "朔州市" : dt.Rows[i + 4][0].ToString().Substring(2);//站点名称
                    string stationCode = GetStationCode(dt.Rows[i + 4][1].ToString());//站点编号
                    object SO2AQI = IsNull(dt.Rows[4 + i][6 + 2 * j]);//SO2AQI
                    object SO2 = IsNull(dt.Rows[4 + i][5 + 2 * j]);//SO2
                    object COAQI = IsNull(dt.Rows[4 + (row + 1) + i][6 + 2 * j]);//COAQI
                    object CO = IsNull(dt.Rows[4 + (row + 1) + i][5 + 2 * j]);//CO
                    object NO2AQI = IsNull(dt.Rows[14 + i][6 + 2 * j]);//NO2AQI
                    object NO2 = IsNull(dt.Rows[14 + i][5 + 2 * j]);//NO2
                    object PM10AQI = IsNull(dt.Rows[19 + i][6 + 2 * j]);//PM10AQI
                    object PM10 = IsNull(dt.Rows[19 + i][5 + 2 * j]);//PM10
                    object PM25AQI = IsNull(dt.Rows[24 + i][6 + 2 * j]);//PM25AQI
                    object PM25 = IsNull(dt.Rows[24 + i][5 + 2 * j]);//PM25                           
                    //CONVERT(varchar(100),MonitorTime,23)
                    string sql = "select count(*) from T_Mid_AirForeCast where StationName=" + "'" + stationName + "'" + " and MonitorTime=" + "'" + forecastTime.AddDays(j) + "'" + " and ForecastTime=" + "'" + forecastTime + "'";
                    int count = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, sql));
                    if (count > 0)
                    {
                        if (MessageBox.Show("站点：" + stationName + ";预测日期：" + forecastTime.AddDays(j).ToShortDateString() + ";预报日期：" + forecastTime.ToShortDateString() + "数据已存在！") == DialogResult.OK)
                        {
                            continue;
                        }
                    }
                    string sqlstr = string.Format("insert into T_Mid_AirForeCast (StationName, StationCode," + "MonitorTime, ForecastTime, SO2AQI, SO2, COAQI, CO,NO2AQI, NO2, PM10AQI, PM10,PM25AQI," + "PM25,  AQI,AirLevel,FirstP,ForecastModel) values ('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8},{9}," +
                         "{10},{11},{12},{13},{14},{15},{16},{17})",
                         stationName, stationCode, forecastTime.AddDays(j), forecastTime, SO2AQI, SO2, COAQI, CO, NO2AQI, NO2, PM10AQI, PM10, PM25AQI, PM25, "[dbo].[GetMaxAQI](" + SO2AQI + "," + COAQI + "," + NO2AQI + "," + PM10AQI + "," + PM25AQI + ",''" + ")", "[dbo].[fun_AirAQILevel](" + "[dbo].[GetMaxAQI](" + SO2AQI + "," + COAQI + "," + NO2AQI + "," + PM10AQI + "," + PM25AQI + ",''" + ")" + ")", "[dbo].[fun_AQI_FristPollutant](" + SO2 + "," + CO + "," + NO2 + "," + PM10 + "," + PM25 + ",0" + ",0" + ")", 0);
                    int o = 0;
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr);

                }
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

        private void statForeCastData_CheckedChanged(object sender, EventArgs e)
        {
            string file = "E:\\Fanlu\\朔州\\项目数据\\统计预测数据\\ybSHUOZHOU_2015-10-06.xlsx";

        }



        private void btnBegin_Click(object sender, EventArgs e)
        {
            AirForCast afc = new AirForCast();
            afc.con = con;
           // afc.InsertAirForeCastTJData1(); 
        }
             



    }
}
