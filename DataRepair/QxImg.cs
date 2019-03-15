using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataRepair
{
    public class QxImg
    {

        public void DownLoadSoundingImg()
        {
            DateTime dt = DateTime.Now.Hour - 12 > 0 ? DateTime.Now : DateTime.Now.AddDays(-1); ;
            string year = dt.Year.ToString();
            string month = dt.Month.ToString();
            string date = dt.Day.ToString();
            string state = dt.Hour - 12 > 0 ? "00" : "12";
            string targetUrl = "http://weather.uwyo.edu/cgi-bin/sounding?region=seasia&TYPE=GIF%3ASKEWT&YEAR=" + year
                + "&MONTH=" + month + "&FROM=" + date + state + "&TO=" + date + state + "&STNM=53772";
            string html = GetPage(targetUrl);
            string strdate = dt.ToString("yyyyMMdd") + state;
            //网页路径
            string webpath = "http://weather.uwyo.edu/upperair/bimages/" + strdate + ".53772.skewt.parc.gif";
            string fileName = dt.ToString("yyyyMMddHHmm") + ".GIF";
            string locationUrl = ConfigurationManager.AppSettings["fullpath"] + "SoundingImg\\" + fileName;//保存到本地路径
            DateTime CurrTime = Convert.ToDateTime(dt.ToString("yyyy-MM-dd") +" "+ state+":00");//文件时间
            string relativePath = "SoundingImg\\" + fileName;//文件相对路径
            T_Moni_ImgFiles img = new T_Moni_ImgFiles();
            img.FileName = fileName;
            img.FullFileName = relativePath;
            img.Cate = 7;
            img.CreatedTime = DateTime.Now;
            img.FileDate = CurrTime;
            ImgAcp(webpath, locationUrl, img);
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
         
        /// <summary>
        /// 图片采集
        /// </summary>
        private void ImgAcp(string imgUrl, string localpath, T_Moni_ImgFiles imgfile)
        {
            if (GetUrlError(imgUrl) != 200)
            {
                writelog("图片未找到：" + imgUrl);
                return;
            }

            WebRequest request = WebRequest.Create(imgUrl);
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
                request.Timeout = 5 * 60 * 1000;
                ImageFormat f = ImageFormat.Gif;
                string fileext = imgUrl.Substring(imgUrl.LastIndexOf('.'));
                if (fileext.ToUpper() == ".JPG")
                {
                    f = ImageFormat.Jpeg;
                }
                else if (fileext.ToUpper() == ".PNG")
                {
                    f = f = ImageFormat.Png;
                }
                else if (fileext.ToUpper() == ".BMP")
                {
                    f = ImageFormat.Bmp;
                }
                Stream resStream = response.GetResponseStream();
                FileInfo fi = new FileInfo(localpath);
                //如果存在文件则先干掉
                if (!fi.Exists)
                {
                    System.Drawing.Image img;
                    img = System.Drawing.Image.FromStream(resStream);
                    img.Save(localpath, f);
                    resStream.Close();
                    string ConnectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
                    SqlConnection conn = new SqlConnection(ConnectionString);
                    conn.Open();
                    string sql = "insert into T_Mid_ImgFiles(FileName,Cate,FileDate,FullFileName,CreatedTime) values('" + imgfile.FileName + "'," + imgfile.Cate + ",'" + imgfile.FileDate + "','" + imgfile.FullFileName + "','" + imgfile.CreatedTime + "')";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    //EntityHelper.Add<T_Moni_ImgFiles>(imgfile);
                }
                writelog("图片同步成功：" + localpath);
            }
            catch (Exception ex)
            {
                writelog("图片同步出错：" + ex.Message.ToString() + ">>" + imgUrl);
            }
            finally
            {
                response.Close();
                response.Dispose();
            }
        }


        /// <summary>
        /// 将信息输出到文本文件
        /// </summary>
        /// <param name="readme"></param>
        public void writelog(string txt)
        {
            string logPath = ConfigurationManager.AppSettings["logPath"] + "ImgDataGather\\" + DateTime.Now.ToString("yyyy-MM-dd").ToString() + ".txt";
            StreamWriter dout = new StreamWriter(logPath, true);
            dout.Write(System.DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + " => " + txt + "\r\n");
            dout.Close();
        }

        
        private int GetUrlError(string curl)
        {
            int num = 200;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(curl));
            ServicePointManager.Expect100Continue = false;
            try
            {
                ((HttpWebResponse)request.GetResponse()).Close();
            }
            catch (WebException exception)
            {
                if (exception.Status != WebExceptionStatus.ProtocolError)
                {
                    return num;
                }
                if (exception.Message.IndexOf("500 ") > 0)
                {
                    return 500;
                }
                if (exception.Message.IndexOf("401 ") > 0)
                {
                    return 401;
                }
                if (exception.Message.IndexOf("404") > 0)
                {
                    num = 404;
                }
            }
            return num;
        }   
    }
}
