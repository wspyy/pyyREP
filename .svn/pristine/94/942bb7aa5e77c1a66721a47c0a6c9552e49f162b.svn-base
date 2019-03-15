<%@ WebHandler Language="C#" Class="GetAirForeCastData" %>

using System;
using System.Web;
using Bll.BusinessFun;
using System.Data;
using System.Text;
using System.IO;
/*绘图*/

using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Imaging;


public class GetAirForeCastData : System.Web.UI.Page,IHttpHandler
{

    BaseInfo bll = new BaseInfo();
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string flag = context.Request["flag"];
        string Date = context.Request["date"];
        string StationCode = context.Request["StationCode"];

     
        string strContent = "";
        if (flag == "AirStation")
        { 
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetAirStation()).Replace("null", "--");
        }
        else if (flag == "city") {
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetCityList()).Replace("null", "--");
        }
        else if (flag == "CheckCity") {
            strContent = QueryCityData(context);
        }
        else if (flag == "CheckCityExport")
        {
            CityExportData(context);
        }
        else if (flag == "Pollute")
        {
            string type = context.Request["type"];
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetPolluteList()).Replace("null", "--");
        }
        else if (flag == "QXStation")
        {
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetQXStation()).Replace("null", "--");
        }
        else if (flag == "CheckForeCast")
        {
            strContent = QueryData(context);
        }
        else if (flag == "CheckForeCastExport")
        {
            ExportData(context);
        }
        else if (flag == "SMSUserList")
        {
            Bll.BaseFun.UserBll bll = new Bll.BaseFun.UserBll();
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetUserList_SMS()).Replace("null", "\"--\"");
        }
        else if (flag == "VerifyUser")
        {
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(VerifyUser(context)).Replace("null", "--");
        }
        else if (flag == "ACCAnaCode")
        {
            strContent = Newtonsoft.Json.JsonConvert.SerializeObject(bll.GetACCAnaCode()).Replace("null", "--");
        }

        context.Response.Write(strContent);
    }
    
    public bool VerifyUser(HttpContext context)
    {
        string code = context.Request["code"];
        string pwd = context.Request["pwd"];
        System.SingleTable.PublicMethod.GetMD5 md = new System.SingleTable.PublicMethod.GetMD5();
        string strSql = "select U_Password,U_RealName,UserID,U_Role,DepartmentID from T_PC_User where U_LoginName ='" + code + "' ";
        System.SingleTable.SQLHelper sqlH = new System.SingleTable.SQLHelper();
        System.Data.DataSet ds = sqlH.ExecuteSQLDataSet(strSql);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string pwdInDB = ds.Tables[0].Rows[0]["U_Password"].ToString();
            if (pwdInDB == md.StringToMD5(pwd))
            {
                string imgpath = "../Devoops/img/sign/"+ code + ".png";
                string filepath = HttpContext.Current.Server.MapPath(imgpath);
                string code_name = ds.Tables[0].Rows[0]["U_RealName"].ToString();
                if (File.Exists(filepath))
                {

                }
                else
                {
                    Drawpng(code_name,code); 
                }
                return true;
            }
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    protected void Drawpng(string content,string code)
    {
     //判断字符串不等于空和null

        if (content == null || content.Trim() == String.Empty)
            return;
        //创建一个位图对象
        Bitmap image = new Bitmap(140, 60);
        //创建Graphics
        Graphics g = Graphics.FromImage(image);
        try
        {
            //清空图片背景颜色
            g.Clear(Color.White);
            Font font = new Font("Arial", 25f, (FontStyle.Bold));//15.5f
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Black, Color.Black, 1.2f, true);//image.Width, image.Height;;Color.DarkRed
            g.DrawString(content, font, brush, 10, 10);
            //画图片的边框线
            //g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            string dirpath = HttpContext.Current.Server.MapPath("../Devoops/img/sign/" + code + ".png");
            image.Save(dirpath);
        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }
    }

    public string QueryCityData(HttpContext context)
    {
        int countPage = 0;
        int rowcount = 0;
        string strCheckID = context.Request["strCheckID"].ToString();
        //string begindate = Convert.ToDateTime(context.Request["begin"].ToString()).ToString("yyyy-MM-dd HH");
        //string enddate = Convert.ToDateTime(context.Request["end"].ToString()).ToString("yyyy-MM-dd HH");
        string begindate = context.Request["begin"].ToString();
        string enddate = context.Request["end"].ToString();
        if (begindate.Length > 11) begindate += ":00:00";
        if (enddate.Length > 11) enddate += ":00:00";        
        int page = Convert.ToInt32(context.Request["page"].ToString());
        int rows = Convert.ToInt32(context.Request["rows"].ToString());
        DataTable ds = null;
        if (strCheckID != "" && begindate != "" && enddate != "")
        {
           
            ds = bll.GetCityCheckData(begindate, enddate, strCheckID,  page, rows, ref rowcount, ref countPage);
        }

        int RowCount = 0;
        if (!(ds == null || ds.Rows.Count <= 0))
        {
            RowCount = ds.Rows.Count;
        }
        StringBuilder sb = new StringBuilder();
        if (ds != null)
        {
            sb.Append("{HasData:\"yes\",CountPage:\"" + countPage + "\",");
            sb.Append("Data:[");
            int i = 0;

            foreach (DataRow dr in ds.Rows)
            {
                i++;
                sb.Append("{CityName:\"" + dr["CityName"]);
                sb.Append("\",MonitorTime:\"" + dr["MonitorTime"]);
                sb.Append("\",SO2:\"" + dr["SO2"]);
                sb.Append("\",SO2AQ:\"" + dr["SO2AQI"]);
                sb.Append("\",NO2:\"" + dr["NO2"]);
                sb.Append("\",NO2AQI:\"" + dr["NO2AQI"]);
                sb.Append("\",CO:\"" + dr["CO"]);
                sb.Append("\",COAQI:\"" + dr["COAQI"]);
                sb.Append("\",O3:\"" + dr["O3"]);
                sb.Append("\",O3AQI:\"" + dr["O3AQI"]);
                sb.Append("\",PM10:\"" + dr["PM10"]);
                sb.Append("\",PM10AQI:\"" + dr["PM10AQI"]);
                sb.Append("\",PM25:\"" + dr["PM25"]);
                sb.Append("\",PM25AQI:\"" + dr["PM25AQI"]);
                sb.Append("\",AQI:\"" + dr["AQI"]);
                sb.Append("\",FirstP:\"" + dr["FirstP"]);
                sb.Append("\",AirLevel:\"" + dr["AirLevel"]);
                
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
        return sb.ToString();

    }
   
    public void CityExportData(HttpContext context)
    {
        int countPage = 0;
        int rowcount = 0;
        string strCheckID = context.Request["strCheckID"].ToString();
        //string begindate = Convert.ToDateTime(context.Request["begin"].ToString()).ToString("yyyy-MM-dd HH");
        //string enddate = Convert.ToDateTime(context.Request["end"].ToString()).ToString("yyyy-MM-dd HH");
        string begindate = context.Request["begin"].ToString();
        string enddate = context.Request["end"].ToString();        
        int page = Convert.ToInt32(context.Request["page"].ToString());
        int rows = Convert.ToInt32(context.Request["rows"].ToString());
        string html = context.Request["html"].ToString();
        DataTable ds = null;
        if (strCheckID != "" && begindate != "" && enddate != "")
        {
            ds = bll.GetCityCheckData(begindate, enddate, strCheckID,  page, rows, ref rowcount, ref countPage);

        }
        if (ds != null)
        {
            //ExportToExcel(ds, "shuju");

            test1(context);

            //StringBuilder sb = new StringBuilder();
            //sb.Append(html);
            //ExportDocument(sb, "企业排口设备汇总表", "excel");
        }
    }   
    public string QueryData(HttpContext context)
    {
        int countPage = 0;
        int rowcount = 0;
        string strCheckID = context.Request["strCheckID"].ToString();
        //string begindate = Convert.ToDateTime(context.Request["begin"].ToString()).ToString("yyyy-MM-dd HH");
        //string enddate = Convert.ToDateTime(context.Request["end"].ToString()).ToString("yyyy-MM-dd HH");
        string begindate = context.Request["begin"].ToString();
        string enddate = context.Request["end"].ToString();
        if (begindate.Length > 11) begindate += ":00:00";
        if (enddate.Length > 11) enddate += ":00:00";
        int model = Convert.ToInt32(context.Request["model"].ToString());
        int page = Convert.ToInt32(context.Request["page"].ToString());
        int rows = Convert.ToInt32(context.Request["rows"].ToString());
        DataTable ds=null;
        if (strCheckID != "" && begindate != "" && enddate != "")
        {
            ds = bll.GetForeCastCheckData(begindate, enddate, strCheckID, model, page, rows, ref rowcount, ref countPage);
        }
          
        int RowCount = 0;
        if (!(ds == null || ds.Rows.Count <= 0))
        {
            RowCount = ds.Rows.Count;
        }
        StringBuilder sb = new StringBuilder();
        if (ds != null)
        {
            sb.Append("{HasData:\"yes\",CountPage:\"" + countPage + "\",");
            sb.Append("Data:[");
            int i = 0;

            foreach (DataRow dr in ds.Rows)
            {
                i++;
                sb.Append("{StationName:\"" + dr["StationName"]);
                sb.Append("\",MonitorTime:\"" + dr["MonitorTime"]);
                sb.Append("\",ForecastTime:\"" + dr["ForecastTime"]);
                sb.Append("\",SO2:\"" + dr["SO2"]);
                sb.Append("\",SO2AQ:\"" + dr["SO2AQI"]);
                sb.Append("\",NO2:\"" + dr["NO2"]);
                sb.Append("\",NO2AQI:\"" + dr["NO2AQI"]);
                sb.Append("\",CO:\"" + dr["CO"]);
                sb.Append("\",COAQI:\"" + dr["COAQI"]);
                sb.Append("\",O3:\"" + dr["O3"]);
                sb.Append("\",O3AQI:\"" + dr["O3AQI"]);
                sb.Append("\",PM10:\"" + dr["PM10"]);
                sb.Append("\",PM10AQI:\"" + dr["PM10AQI"]);
                sb.Append("\",PM25:\"" + dr["PM25"]);
                sb.Append("\",PM25AQI:\"" + dr["PM25AQI"]);
                sb.Append("\",AQI:\"" + dr["AQI"]);
                sb.Append("\",FirstP:\"" + dr["FirstP"]);
                sb.Append("\",AirLevel:\"" + dr["AirLevel"]);
                sb.Append("\",AirLevelDes:\"" + dr["AirLevelDes"]);
                sb.Append("\",AirColor:\"" + dr["AirColor"]);
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
        return sb.ToString();

    }

    public void ExportData(HttpContext context)
    {
        int countPage = 0;
        int rowcount = 0;
        string strCheckID = context.Request["strCheckID"].ToString();
        //string begindate = Convert.ToDateTime(context.Request["begin"].ToString()).ToString("yyyy-MM-dd HH");
        //string enddate = Convert.ToDateTime(context.Request["end"].ToString()).ToString("yyyy-MM-dd HH");
        string begindate = context.Request["begin"].ToString();
        string enddate = context.Request["end"].ToString();
        int model = Convert.ToInt32(context.Request["model"].ToString());
        int page = Convert.ToInt32(context.Request["page"].ToString());
        int rows = Convert.ToInt32(context.Request["rows"].ToString());
        string html = context.Request["html"].ToString();
        DataTable ds = null;
        if (strCheckID != "" && begindate != "" && enddate != "")
        {
            ds = bll.GetForeCastCheckData(begindate, enddate, strCheckID, model, page, rows, ref rowcount, ref countPage);
        }
        if (ds != null)
        {
            //ExportToExcel(ds, "shuju");

            test1(context);

            //StringBuilder sb = new StringBuilder();
            //sb.Append(html);
            //ExportDocument(sb, "企业排口设备汇总表", "excel");
        }
    }
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private void test1(HttpContext context)
    {
        HttpResponse resp = System.Web.HttpContext.Current.Response;
        resp.Charset = "utf-8";
        resp.Clear();
        string filename = "统计贴标报表_" + DateTime.Now.ToString("yyyyMMddHHmmss");
        resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
        resp.ContentEncoding = System.Text.Encoding.UTF8;

        resp.ContentType = "application/ms-excel";
        string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" + "<style> .table{ font: 9pt Tahoma, Verdana; color: #000000; text-align:center;  background-color:#8ECBE8;  }.table td{text-align:center;height:21px;background-color:#EFF6FF;}.table th{ font: 9pt Tahoma, Verdana; color: #000000; font-weight: bold; background-color: #8ECBEA; height:25px;  text-align:center; padding-left:10px;}</style>";
        resp.Write(style);

        resp.Write("<table class='table'><tr><th>姓名</th><th>出生年月</th><th>籍贯</th><th>毕业时间</th></tr>");

        System.Data.DataTable dtSource = new System.Data.DataTable();
        dtSource.TableName = "statistic";
        dtSource.Columns.Add("第一列");
        dtSource.Columns.Add("第二列");
        dtSource.Columns.Add("第三列");
        dtSource.Columns.Add("第四列");

        System.Data.DataRow row = null;
        row = dtSource.NewRow();
        row[0] = "张三";
        row[1] = "1987-09-09";
        row[2] = "河北保定";
        row[3] = "2008年毕业";
        dtSource.Rows.Add(row);

        row = dtSource.NewRow();
        row[0] = "李四";
        row[1] = "1987-09-02";
        row[2] = "湖北武汉";
        row[3] = "2009年毕业";
        dtSource.Rows.Add(row);

        row = dtSource.NewRow();
        row[0] = "王五";
        row[1] = "1987-09-01";
        row[2] = "湖南湘潭";
        row[3] = "2013年毕业";
        dtSource.Rows.Add(row);

        foreach (DataRow tmpRow in dtSource.Rows)
        {
            resp.Write("<tr><td>" + tmpRow[0] + "</td>");
            resp.Write("<td>" + tmpRow[1] + "</td>");
            resp.Write("<td>" + tmpRow[2] + "</td>");
            resp.Write("<td>" + tmpRow[3] + "</td>");
            resp.Write("</tr>");
        }
        resp.Write("<table>");

        resp.Flush();
        resp.End();
    }
  

    #region 导出Excel
    public bool ExportDocument(StringBuilder DataSoure, string infoName, string format)
    {
        try
        {
            #region  根据文件类型  选择文件后缀名
            string postfix = null; //文件后缀名
            if (format == "excel")
            {
                postfix = ".xls";
            }
            else if (format == "word")
            {
                postfix = ".doc";
            }
            else if (format == "text")
            {
                postfix = ".txt";
            }
            #endregion

            #region 导出操作

            HttpResponse context = System.Web.HttpContext.Current.Response;
            context.Clear();
            HttpContext.Current.Response.Charset = "gb2312";
            context.Write("<meta http-equiv=Content-Type Content=text/html;CharSet=UTF-8\">");
            context.ContentType = "application/ms-" + format;
            context.HeaderEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            context.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(infoName, System.Text.Encoding.UTF8) + postfix);
            context.ContentEncoding = System.Text.Encoding.UTF8;
            System.IO.StringWriter ow = new System.IO.StringWriter(DataSoure);
            System.Web.UI.HtmlTextWriter ht = new System.Web.UI.HtmlTextWriter(ow);
            context.Write(ow);
            context.Flush();
            context.Close();
            #endregion
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    #endregion
    
    /// <summary>
        /// 执行导出
        /// </summary>
        /// <param name="ds">要导出的DataSet</param>
        /// <param name="strExcelFileName">要导出的文件名</param>
        public void ExportToExcel(DataTable source, string fileName)
        {
            System.IO.StreamWriter excelDoc;
            excelDoc = new System.IO.StreamWriter(fileName);
            const string startExcelXML = "<xml version>/r/n<Workbook " +
                  "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n" +
                  " xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n " +
                  "xmlns:x=\"urn:schemas-    microsoft-com:office:" +
                  "excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:" +
                  "office:spreadsheet\">\r\n <Styles>\r\n " +
                  "<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n " +
                  "<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>" +
                  "\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>" +
                  "\r\n <Protection/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"BoldColumn\">\r\n <Font " +
                  "x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n " +
                  "<Style     ss:ID=\"StringLiteral\">\r\n <NumberFormat" +
                  " ss:Format=\"@\"/>\r\n </Style>\r\n <Style " +
                  "ss:ID=\"Decimal\">\r\n <NumberFormat " +
                  "ss:Format=\"0.0000\"/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"Integer\">\r\n <NumberFormat " +
                  "ss:Format=\"0\"/>\r\n </Style>\r\n <Style " +
                  "ss:ID=\"DateLiteral\">\r\n <NumberFormat " +
                  "ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n " +
                  "</Styles>\r\n ";
            const string endExcelXML = "</Workbook>";
            int rowCount = 0;
            int sheetCount = 1;
            /**//*
           <xml version>
           <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
           xmlns:o="urn:schemas-microsoft-com:office:office"
           xmlns:x="urn:schemas-microsoft-com:office:excel"
           xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
           <Styles>
           <Style ss:ID="Default" ss:Name="Normal">
             <Alignment ss:Vertical="Bottom"/>
             <Borders/>
             <Font/>
             <Interior/>
             <NumberFormat/>
             <Protection/>
           </Style>
           <Style ss:ID="BoldColumn">
             <Font x:Family="Swiss" ss:Bold="1"/>
           </Style>
           <Style ss:ID="StringLiteral">
             <NumberFormat ss:Format="@"/>
           </Style>
           <Style ss:ID="Decimal">
             <NumberFormat ss:Format="0.0000"/>
           </Style>
           <Style ss:ID="Integer">
             <NumberFormat ss:Format="0"/>
           </Style>
           <Style ss:ID="DateLiteral">
             <NumberFormat ss:Format="mm/dd/yyyy;@"/>
           </Style>
           </Styles>
           <Worksheet ss:Name="Sheet1">
           </Worksheet>
           </Workbook>
           */
            excelDoc.Write(startExcelXML);
            excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
            excelDoc.Write("<Table>");
            excelDoc.Write("<Row>");
            for (int x = 0; x < source.Columns.Count; x++)
            {
                excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
                excelDoc.Write(source.Columns[x].ColumnName);
                excelDoc.Write("</Data></Cell>");
            }
            excelDoc.Write("</Row>");
            foreach (DataRow x in source.Rows)
            {
                rowCount++;
                //if the number of rows is > 64000 create a new page to continue output
                if (rowCount == 64000)
                {
                    rowCount = 0;
                    sheetCount++;
                    excelDoc.Write("</Table>");
                    excelDoc.Write(" </Worksheet>");
                    excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                    excelDoc.Write("<Table>");
                }
                excelDoc.Write("<Row>"); //ID=" + rowCount + "
                for (int y = 0; y < source.Columns.Count; y++)
                {
                    System.Type rowType;
                    rowType = x[y].GetType();
                    switch (rowType.ToString())
                    {
                        case "System.String":
                            string XMLstring = x[y].ToString();
                            XMLstring = XMLstring.Trim();
                            XMLstring = XMLstring.Replace("&", "&");
                            XMLstring = XMLstring.Replace(">", ">");
                            XMLstring = XMLstring.Replace("<", "<");
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                           "<Data ss:Type=\"String\">");
                            excelDoc.Write(XMLstring);
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.DateTime":
                            //Excel has a specific Date Format of YYYY-MM-DD followed by  
                            //the letter 'T' then hh:mm:sss.lll Example 2005-01-31T24:01:21.000
                            //The Following Code puts the date stored in XMLDate 
                            //to the format above
                            DateTime XMLDate = (DateTime)x[y];
                            string XMLDatetoString = ""; //Excel Converted Date
                            XMLDatetoString = XMLDate.Year.ToString() +
                                 "-" +
                                 (XMLDate.Month < 10 ? "0" +
                                 XMLDate.Month.ToString() : XMLDate.Month.ToString()) +
                                 "-" +
                                 (XMLDate.Day < 10 ? "0" +
                                 XMLDate.Day.ToString() : XMLDate.Day.ToString()) +
                                 "T" +
                                 (XMLDate.Hour < 10 ? "0" +
                                 XMLDate.Hour.ToString() : XMLDate.Hour.ToString()) +
                                 ":" +
                                 (XMLDate.Minute < 10 ? "0" +
                                 XMLDate.Minute.ToString() : XMLDate.Minute.ToString()) +
                                 ":" +
                                 (XMLDate.Second < 10 ? "0" +
                                 XMLDate.Second.ToString() : XMLDate.Second.ToString()) +
                                 ".000";
                            excelDoc.Write("<Cell ss:StyleID=\"DateLiteral\">" +
                                         "<Data ss:Type=\"DateTime\">");
                            excelDoc.Write(XMLDatetoString);
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Boolean":
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                        "<Data ss:Type=\"String\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            excelDoc.Write("<Cell ss:StyleID=\"Integer\">" +
                                    "<Data ss:Type=\"Number\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Decimal":
                        case "System.Double":
                            excelDoc.Write("<Cell ss:StyleID=\"Decimal\">" +
                                  "<Data ss:Type=\"Number\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.DBNull":
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                  "<Data ss:Type=\"String\">");
                            excelDoc.Write("");
                            excelDoc.Write("</Data></Cell>");
                            break;
                        default:
                            throw (new Exception(rowType.ToString() + " not handled."));
                    }
                }
                excelDoc.Write("</Row>");
            }
            excelDoc.Write("</Table>");
            excelDoc.Write(" </Worksheet>");
            excelDoc.Write(endExcelXML);
            excelDoc.Close();
        }

}