using qxDataGather;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataRepair
{
    public class QxFiles
    {

        //把文件从一个服务器拷到另一个服务器
        public void getShareFile()
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

            int thour = 1;
            string tday = "";
            if (DateTime.Now.Hour == 1)
            {
                tday = DateTime.Now.AddDays(-1).ToString("yyyMMdd");
                thour = 12;
            }
            if (DateTime.Now.Hour == 13)
            {
                tday = DateTime.Now.ToString("yyyMMdd");
                thour = 0;
            }

            tday = DateTime.Now.ToString("yyyMMdd");
            thour = 0;

            using (IdentityScope iss = new IdentityScope(strAdmin, strPwd, strIP))
            {
                for (int i = 0; i < sAry.Length; i++)
                {
                    DirectoryInfo TheFolder = new DirectoryInfo(sAry[i]);
                    foreach (FileInfo FileItem in TheFolder.GetFiles())
                    {
                        if (FileItem.Name.IndexOf(tday) > -1)
                        {
                            int hour = Int32.Parse(FileItem.Name.Split('_')[4].Substring(8, 2));
                            if (true)//hour == thour
                            {
                                int num = Int32.Parse(FileItem.Name.Split('-')[4].Split('.')[0]);
                                if (num % 600 == 0)
                                {
                                    if (File.Exists(tAry[i] + FileItem.Name)) continue;
                                    File.Copy(sAry[i] + FileItem.Name, tAry[i] + FileItem.Name, false);
                                    UploadFile(fAry[i], tAry[i] + FileItem.Name);
                                    //File.Delete(tempfilepath + FileItem.Name);
                                    writelog("成功同步文件：" + sAry[i] + FileItem.Name);
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// FTP上传文件到服务器
        /// </summary>
        private void UploadFile(string filepath, string filename)
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
        /// 将信息输出到文本文件
        /// </summary>
        /// <param name="readme"></param>
        public void writelog(string txt)
        {
            string logPath = ConfigurationManager.AppSettings["logPath"] + "QxDataGather\\" + DateTime.Now.ToString("yyyy-MM-dd").ToString() + ".txt";
            StreamWriter dout = new StreamWriter(logPath, true);
            dout.Write(System.DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + " => " + txt + "\r\n");
            dout.Close();
        }


        public void ClearFile()
        {
            string sharefileTarget = ConfigurationManager.AppSettings["sharefileTarget"];

            string[] tAry = sharefileTarget.Split('|');

            for (int i = 0; i < tAry.Length; i++)
            {
                DirectoryInfo TheFolder = new DirectoryInfo(tAry[i]);
                foreach (FileInfo FileItem in TheFolder.GetFiles())
                {
                    int  d = FileItem.CreationTime.Subtract(DateTime.Now).Days;
                    if (Math.Abs(d)>2)
                    {
                        //File.Delete(FileItem.Name);
                        string ddd = FileItem.Name;
                    }
                }
            }
        }
    }
}
