using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace qxDataGather
{
    public partial class QxDataGatherService : ServiceBase
    {
        private static System.Timers.Timer timer = null;
        private static string scon = "", tcon = "";
        public QxDataGatherService()
        {
            InitializeComponent();
            scon = ConfigurationManager.ConnectionStrings["sourceDB"].ToString();
            tcon = ConfigurationManager.ConnectionStrings["targetDB"].ToString();
            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.Enabled = true;
                timer.Interval = Int32.Parse(ConfigurationManager.AppSettings["timeInterval"]) * 60 * 1000; // 10秒
                timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
                this.ServiceName = "QxDataGatherService";
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
        // 定时执行

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) 
        {
            try
            {
                getQxPacket();
            }
            catch (Exception ex)
            {
                writelog("文件同步出错：" + ex.Message.ToString());
            }
            try
            {
                getQxMonitorData();
            }
            catch (Exception ex)
            {
                writelog("数据同步出错：" + ex.Message.ToString());
            }
            try
            {
                if (DateTime.Now.Hour == 1 | DateTime.Now.Hour == 13)
                    getShareFile();
            }
            catch (Exception ex)
            {
                writelog("文件同步出错：" + ex.Message.ToString());
            }
        }

        //把文件从一个服务器拷到另一个服务器
        private void getShareFile()
        {
            string sharefileSource = ConfigurationManager.AppSettings["sharefileSource"];
            string sharefileTarget = ConfigurationManager.AppSettings["sharefileTarget"];
            string ftpTarget = ConfigurationManager.AppSettings["FTPTarget"];
            string strAdmin = ConfigurationManager.AppSettings["shareAdmin"];
            string strPwd = ConfigurationManager.AppSettings["sharePwd"];
            string strIP = ConfigurationManager.AppSettings["shareIP"];

            string[] sAry = sharefileSource.Split('|');
            string[] tAry = sharefileTarget.Split('|');
            string[] fAry = ftpTarget.Split('|');

            using (IdentityScope iss = new IdentityScope(strAdmin, strPwd, strIP))
            {
                for (int i = 0; i < sAry.Length; i++)
                {
                    DirectoryInfo TheFolder = new DirectoryInfo(sAry[i]);
                    foreach (FileInfo FileItem in TheFolder.GetFiles())
                    {
                        string str1 = DateTime.Now.ToString("yyyMMdd");
                        string str2 = DateTime.Now.AddDays(-1).ToString("yyyMMdd");
                        if (FileItem.Name.IndexOf(str1) > -1 || FileItem.Name.IndexOf(str2) > -1)
                        {
                            File.Copy(sAry[i] + FileItem.Name, tAry[i] + FileItem.Name, true);
                            UploadFile(fAry[i], tAry[i] + FileItem.Name);
                            //File.Delete(tempfilepath + FileItem.Name);
                            writelog("成功同步文件：" + sAry[i] + FileItem.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// FTP上传文件到服务器
        /// </summary>
        private void UploadFile(string filepath,string filename)
        {
            FtpWebRequest reqFTP = null;  
            string serverIP;  
            string userName;  
            string password;  
            string url;
  
            try  
            {
                FileInfo fileInf = new FileInfo(filename);
                serverIP = ConfigurationManager.AppSettings["FTPIP"];
                userName = ConfigurationManager.AppSettings["FTPAdmin"];
                password = ConfigurationManager.AppSettings["FTPPwd"];
                url = "ftp://" + serverIP + filepath + fileInf.Name;  
  
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
                writelog("文件同步出错：" + filename + ex.InnerException.Message);  
            }  

        }

        /// <summary>
        /// 获取气象预报报文
        /// </summary>
        private void getQxPacket() 
        {
            string SourcePath = ConfigurationManager.AppSettings["PacketSource"];
            string targetPath = ConfigurationManager.AppSettings["PacketTarget"];
            string strAdmin = ConfigurationManager.AppSettings["shareAdmin"];
            string strPwd = ConfigurationManager.AppSettings["sharePwd"];
            string strIP = ConfigurationManager.AppSettings["shareIP"];

            using (IdentityScope iss = new IdentityScope(strAdmin, strPwd, strIP))
            {
                DirectoryInfo TheFolder = new DirectoryInfo(SourcePath);
                writelog("遍历文件夹：" + SourcePath);
                foreach (FileInfo FileItem in TheFolder.GetFiles())
                {
                    string str1 = DateTime.Now.ToString("yyyMMdd") + "224519";
                    string str2 = DateTime.Now.ToString("yyyMMdd") + "083019";
                    if (FileItem.Name.IndexOf(str1) > -1 || FileItem.Name.IndexOf(str2) > -1)
                    {
                        File.Copy(SourcePath + FileItem.Name, targetPath + FileItem.Name, true);
                        ReadTXT(targetPath + FileItem.Name);
                        writelog("成功同步文件：" + FileItem.Name);
                    }
                }
            }
        }

        /// <summary>cmd
        /// 
        /// 获取气象监测数据
        /// </summary>
        private void getQxMonitorData()
        {
            string lastTime = "";
            string strSql = "select max(InsertTime) from [dbo].[T_Mid_QXRealTimeData]";
            DataSet ds = SqlHelper.ExecuteDataset(tcon, CommandType.Text, strSql);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count>0)
                {
                    lastTime = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            if (lastTime.Length > 0)
            {
                strSql = "select * from [dbo].[tabtimedata] where InsertTime > '" + lastTime + "'";
                ds = SqlHelper.ExecuteDataset(scon, CommandType.Text, strSql);
                if (ds != null)
                {
                    string strInsertCol = "insert T_Mid_QXRealTimeData(";
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        strInsertCol += ds.Tables[0].Columns[i].ToString() + ",";
                    }
                    strInsertCol = strInsertCol.Substring(0, strInsertCol.Length - 1);
                    strInsertCol += " values(";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string strInsertRow = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (IsExist(ds.Tables[0].Rows[i]["StationNum"].ToString(), ds.Tables[0].Rows[i]["InsertTime"].ToString()))
                                continue;
                            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
			                {
                                strInsertRow += ds.Tables[0].Rows[i][j] + "," ;
                            }
                            strInsertRow = strInsertRow.Substring(0, strInsertRow.Length - 1);
                            strInsertCol += strInsertRow;
                            try
                            {
                                SqlHelper.ExecuteNonQuery(tcon, CommandType.Text, strInsertCol);
                            }
                            catch (Exception ex)
                            {
                                writelog("数据同步出错：" + ex.Message.ToString());
                                //throw;
                            }
                        }
                    }
                }
            }
        }

        private bool IsExist(string stationNum,string InsertTime)
        {
            string strSql = "select * from [dbo].[T_Mid_QXRealTimeData] where StationNum = " + stationNum + " InsertTime = '" + InsertTime + "'";
            DataSet ds = SqlHelper.ExecuteDataset(tcon, CommandType.Text, strSql);
            if (ds != null && ds.Tables.Count>0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void ReadTXT(string path)
        {
            StringBuilder sb = new StringBuilder();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        int stationCode = 0;
                        decimal lon = 0;
                        decimal lat = 0;
                        int ForeCastHours = 0;
                        double maxTemp = 0;
                        double minTemp = 0;
                        double weather = 0;
                        double winDirection = 0;
                        double winSeep = 0;
                        string createTime = DateTime.Now.ToString();
                        string sLine = sr.ReadLine();
                        if (sLine.Length < 1)
                        {
                            continue;
                        }
                        string[] strAry = sLine.Split(' ');
                        if (sLine.IndexOf("14 21") > -1)//判断站点
                        {
                            stationCode = Int32.Parse(strAry[0]);
                        }
                        if (strAry.Length > 20)
                        {
                            if (Int32.Parse(strAry[1]) % 24 == 0)
                            {
                                ForeCastHours = Int32.Parse(strAry[1]);
                                maxTemp = Double.Parse(strAry[13]);
                                minTemp = Double.Parse(strAry[15]);
                                weather = Double.Parse(strAry[24]);
                                winDirection = Double.Parse(strAry[27]);
                                winSeep = Double.Parse(strAry[30]);

                                string strSql = "select * from T_Mid_QXForeCastData where StationCode=" + stationCode + " and ForeCastHours=" + ForeCastHours;
                                DataSet ds = SqlHelper.ExecuteDataset(tcon, CommandType.Text, strSql);
                                if (ds == null || ds.Tables.Count == 0)
                                {
                                    string QXstr = "select lon,lat from T_Bas_QxStation where StationCode=" + stationCode;
                                    DataSet Qxds = SqlHelper.ExecuteDataset(tcon, CommandType.Text, QXstr);
                                    if (Qxds.Tables[0].Rows.Count > 0)
                                    {
                                        lon = Convert.ToDecimal(Qxds.Tables[0].Rows[0]["lon"].ToString());
                                        lat = Convert.ToDecimal(Qxds.Tables[0].Rows[0]["lat"].ToString());
                                    }
                                    string InsertStr = "insert into T_Mid_QXForeCastData(StationCode,CreateTime,ForCastTime,ForeCastHours,lon,lat,MaxTemp,MinTemp,WeatherCode,WindSpeed,WindDirection) values('" + stationCode + "','" + createTime + "','" + createTime + "'," + ForeCastHours + "," + lon + "," + lat + "," + maxTemp + "," + minTemp + "," + weather + "," + winSeep + "," + winDirection + ")";
                                    SqlHelper.ExecuteNonQuery(tcon, CommandType.Text, InsertStr);
                                    writelog(InsertStr);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将信息输出到文本文件
        /// </summary>
        /// <param name="readme"></param>
        public void writelog(string txt)
        {
            string logPath = ConfigurationManager.AppSettings["logPath"]  +"QxDataGather\\" + DateTime.Now.ToString("yyyy-MM-dd").ToString() + ".txt";
            StreamWriter dout = new StreamWriter(logPath, true);
            dout.Write(System.DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + " => " + txt + "\r\n");
            dout.Close();
        }   
    }
}
