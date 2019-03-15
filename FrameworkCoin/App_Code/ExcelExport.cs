using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;


namespace Utils
{
    public class Export
    {
        private string appType;
        private HttpResponse response;

        public Export()
        {
            this.appType = "Web";
            this.response = HttpContext.Current.Response;
        }

        public Export(string ApplicationType)
        {
            this.appType = ApplicationType;
            if ((this.appType != "Web") && (this.appType != "Win"))
            {
                throw new Exception("Provide valid application format (Web/Win)");
            }
            if (this.appType == "Web")
            {
                this.response = HttpContext.Current.Response;
            }
        }

        private void CreateStylesheet(XmlTextWriter writer, string[] sHeaders, string[] sFileds, ExportFormat FormatType)
        {
            try
            {
                string ns = "http://www.w3.org/1999/XSL/Transform";
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("xsl", "stylesheet", ns);
                writer.WriteAttributeString("version", "1.0");
                writer.WriteStartElement("xsl:output");
                writer.WriteAttributeString("method", "text");
                writer.WriteAttributeString("version", "4.0");
                writer.WriteEndElement();
                writer.WriteStartElement("xsl:template");
                writer.WriteAttributeString("match", "/");
                for (int i = 0; i < sHeaders.Length; i++)
                {
                    writer.WriteString("\"");
                    writer.WriteStartElement("xsl:value-of");
                    writer.WriteAttributeString("select", "'" + sHeaders[i] + "'");
                    writer.WriteEndElement();
                    writer.WriteString("\"");
                    if (i != (sFileds.Length - 1))
                    {
                        writer.WriteString((FormatType == ExportFormat.CSV) ? "," : "\t");
                    }
                }
                writer.WriteStartElement("xsl:for-each");
                writer.WriteAttributeString("select", "Export/Values");
                writer.WriteString("\r\n");
                for (int j = 0; j < sFileds.Length; j++)
                {
                    writer.WriteString("\"");
                    writer.WriteStartElement("xsl:value-of");
                    writer.WriteAttributeString("select", sFileds[j]);
                    writer.WriteEndElement();
                    writer.WriteString("\"");
                    if (j != (sFileds.Length - 1))
                    {
                        writer.WriteString((FormatType == ExportFormat.CSV) ? "," : "\t");
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void Export_with_XSLT_Web(DataSet dsExport, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string FileName)
        {
            try
            {
                //读取要存进Excel报表中的内容
                MemoryStream w = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(w, Encoding.Default);
                this.CreateStylesheet(writer, sHeaders, sFileds, FormatType);
                writer.Flush();
                w.Seek(0L, SeekOrigin.Begin);

                XmlDataDocument document = new XmlDataDocument(dsExport);
                XslTransform transform = new XslTransform();
                transform.Load(new XmlTextReader(w), null, null);
                StringWriter writer2 = new StringWriter();
                transform.Transform((IXPathNavigable)document, null, (TextWriter)writer2, null);

                /*
                 * 保存临时Excel供下载
                */
                string _TempFilePath = "~/business/report/down/" + FileName; // +".xls";
                string _FilePath = HttpContext.Current.Server.MapPath(_TempFilePath);
                FileStream oFileStream;
                if (!File.Exists(_FilePath))
                {
                    oFileStream = new FileStream(_FilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                }
                else
                {
                    //休眠一秒 避免文件未删除完就开始创建产生冲突
                    Thread.Sleep(1000);
                    oFileStream = new FileStream(_FilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    //oFileStream = new FileStream(_FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                }
                StreamWriter oStreamWriter = new StreamWriter(oFileStream, Encoding.Default);
                oStreamWriter.Write(writer2.ToString());
                oStreamWriter.Close();
                oFileStream.Close();
                writer2.Close();
                writer.Close();
                w.Close();
                
            }
            catch (ThreadAbortException exception)
            {
                string message = exception.Message;
            }
            catch (Exception exception2)
            {
                throw exception2;
            }
        }

        private void Export_with_XSLT_Windows(DataSet dsExport, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string FileName)
        {
            try
            {
                MemoryStream w = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(w, Encoding.UTF8);
                this.CreateStylesheet(writer, sHeaders, sFileds, FormatType);
                writer.Flush();
                w.Seek(0L, SeekOrigin.Begin);
                XmlDataDocument document = new XmlDataDocument(dsExport);
                XslTransform transform = new XslTransform();
                transform.Load(new XmlTextReader(w), null, null);
                StringWriter writer2 = new StringWriter();
                transform.Transform((IXPathNavigable)document, null, (TextWriter)writer2, null);
                StreamWriter writer3 = new StreamWriter(FileName);
                writer3.WriteLine(writer2.ToString());
                writer3.Close();
                writer2.Close();
                writer.Close();
                w.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void ExportDetails(DataTable DetailsTable, ExportFormat FormatType, string FileName)
        {
            try
            {
                if (DetailsTable.Rows.Count == 0)
                {
                    throw new Exception("There are no details to export.");
                }
                DataSet dsExport = new DataSet("Export");
                DataTable table = DetailsTable.Copy();
                table.TableName = "Values";
                dsExport.Tables.Add(table);
                string[] sHeaders = new string[table.Columns.Count];
                string[] sFileds = new string[table.Columns.Count];
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sHeaders[i] = table.Columns[i].ColumnName;
                    sFileds[i] = this.ReplaceSpclChars(table.Columns[i].ColumnName);
                }
                if (this.appType == "Web")
                {
                    this.Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
                }
                else if (this.appType == "Win")
                {
                    this.Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void ExportDetails(DataTable DetailsTable, int[] ColumnList, ExportFormat FormatType, string FileName)
        {
            try
            {
                if (DetailsTable.Rows.Count == 0)
                {
                    throw new Exception("There are no details to export");
                }
                DataSet dsExport = new DataSet("Export");
                DataTable table = DetailsTable.Copy();
                table.TableName = "Values";
                dsExport.Tables.Add(table);
                if (ColumnList.Length > table.Columns.Count)
                {
                    throw new Exception("ExportColumn List should not exceed Total Columns");
                }
                string[] sHeaders = new string[ColumnList.Length];
                string[] sFileds = new string[ColumnList.Length];
                for (int i = 0; i < ColumnList.Length; i++)
                {
                    if ((ColumnList[i] < 0) || (ColumnList[i] >= table.Columns.Count))
                    {
                        throw new Exception("ExportColumn Number should not exceed Total Columns Range");
                    }
                    sHeaders[i] = table.Columns[ColumnList[i]].ColumnName;
                    sFileds[i] = this.ReplaceSpclChars(table.Columns[ColumnList[i]].ColumnName);
                }
                if (this.appType == "Web")
                {
                    this.Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
                }
                else if (this.appType == "Win")
                {
                    this.Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void ExportDetails(DataTable DetailsTable, int[] ColumnList, string[] Headers, ExportFormat FormatType, string FileName)
        {
            try
            {
                if (DetailsTable.Rows.Count == 0)
                {
                    throw new Exception("There are no details to export");
                }
                DataSet dsExport = new DataSet("Export");
                DataTable table = DetailsTable.Copy();
                table.TableName = "Values";
                dsExport.Tables.Add(table);
                if (ColumnList.Length != Headers.Length)
                {
                    throw new Exception("ExportColumn List and Headers List should be of same length");
                }
                if ((ColumnList.Length > table.Columns.Count) || (Headers.Length > table.Columns.Count))
                {
                    throw new Exception("ExportColumn List should not exceed Total Columns");
                }
                string[] sFileds = new string[ColumnList.Length];
                for (int i = 0; i < ColumnList.Length; i++)
                {
                    if ((ColumnList[i] < 0) || (ColumnList[i] >= table.Columns.Count))
                    {
                        throw new Exception("ExportColumn Number should not exceed Total Columns Range");
                    }
                    sFileds[i] = this.ReplaceSpclChars(table.Columns[ColumnList[i]].ColumnName);
                }
                if (this.appType == "Web")
                {
                    this.Export_with_XSLT_Web(dsExport, Headers, sFileds, FormatType, FileName);
                }
                else if (this.appType == "Win")
                {
                    this.Export_with_XSLT_Windows(dsExport, Headers, sFileds, FormatType, FileName);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void DownLoadExportExcel(string _FileName,string _FilePath, ExportFormat FormatType)
        {
            /*
            * 下载之前保存的临时Excel
            */
            _FilePath = HttpContext.Current.Server.MapPath(_FilePath);
            FileInfo fi = new FileInfo(_FilePath);
            this.response.Clear();
            this.response.Buffer = true;
            if (FormatType == ExportFormat.CSV)
            {
                this.response.ContentType = "text/csv";
                this.response.AppendHeader("content-disposition", "attachment; filename=" + _FileName);
            }
            else
            {
                this.response.Clear();
                this.response.ClearContent();
                this.response.ClearHeaders();
                this.response.AddHeader("Content-Disposition", "attachment;filename=" + _FileName);
                this.response.AddHeader("Content-Length", fi.Length.ToString());
                this.response.AddHeader("Content-Transfer-Encoding", "binary");
                this.response.ContentType = "application/octet-stream";
                this.response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                this.response.WriteFile(fi.FullName);
                /*
                if (fi.Length > 0)
                {
                    FileStream sr = new FileStream(fi.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                    int size = 1024;//设置每次读取长度。
                    for (int i = 0; i < fi.Length / size + 1; i++)
                    {
                        byte[] buffer = new byte[size];
                        int length = sr.Read(buffer, 0, size);
                        this.response.OutputStream.Write(buffer, 0, length);
                    }
                    sr.Close();
                }
                else
                {
                    this.response.WriteFile(fi.FullName);
                }
                this.response.OutputStream.Flush();
                */
                this.response.Flush();
                this.response.End();

                /*
                this.response.ClearContent();
                this.response.ClearHeaders();
                this.response.ContentType = "application/vnd.ms-excel";
                this.response.Charset = "GB2312";
                this.response.ContentEncoding = Encoding.GetEncoding("GB2312");
                this.response.AppendHeader("content-disposition", String.Format("attachment;filename={0}", _FileName));
                this.response.AppendHeader("Content-Length", fi.Length.ToString());
                */
            }
            this.response.Write(fi.FullName);
            this.response.Flush();
            this.response.End();
        }

        // 输出硬盘文件，提供下载 
        // 输入参数 _Request: Page.Request对象， _Response: Page.Response对象， _fileName: 下载文件名， _fullPath: 带文件名下载路径， _speed 每秒允许下载的字节数 
        // 返回是否成功 
        public bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    int pack = 10240; //10K bytes 
                    //int sleep = 200; //每秒5次 即5*10K bytes每秒 
                    int sleep = (int)Math.Floor(Convert.ToDouble(1000 * pack / _speed)) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor(Convert.ToDouble((fileLength - startBytes) / pack)) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(pack));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private string ReplaceSpclChars(string fieldName)
        {
            fieldName = fieldName.Replace(" ", "_x0020_");
            fieldName = fieldName.Replace("%", "_x0025_");
            fieldName = fieldName.Replace("#", "_x0023_");
            fieldName = fieldName.Replace("&", "_x0026_");
            fieldName = fieldName.Replace("/", "_x002F_");
            return fieldName;
        }

        public enum ExportFormat
        {
            CSV = 1,
            Excel = 2
        }
    }
}


