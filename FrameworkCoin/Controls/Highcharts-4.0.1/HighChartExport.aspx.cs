using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using Svg;
using System.Drawing.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Xml;
using System.Collections.Generic;


public partial class Controls_Highcharts_HighChartExport : System.Web.UI.Page
{
    /// <summary>
    /// 修复图表隐藏数据后，导出还是全部数据bugger
    /// </summary>
    /// <param name="svg"></param>
    /// <returns></returns>
    private string SvgRemoveHidden(string svg)
    {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(svg);
        XmlNodeList nodeListAllg = xml.GetElementsByTagName("g");
        Dictionary<int, XmlNode[,]> dic = new Dictionary<int, XmlNode[,]>();
        int i = 0;
        foreach (XmlNode xNod in nodeListAllg)
        {
            i++;
            XmlNode xmlvisibility = xNod.Attributes.GetNamedItem("class");
            if (xmlvisibility != null && xmlvisibility.Value == "highcharts-series-group")
            {
                foreach (XmlNode xNod2 in xNod.ChildNodes)
                {
                    i++;
                    XmlNode xmlvisibility1 = xNod2.Attributes.GetNamedItem("visibility");
                    if (xmlvisibility1 != null && xmlvisibility1.Value == "hidden")
                    {
                        XmlNode[,] xmln = new XmlNode[1, 2];
                        xmln[0, 0] = xNod;
                        xmln[0, 1] = xNod2;
                        dic.Add(i, xmln);
                    }
                }
            }
            else if (xmlvisibility != null && xmlvisibility.Value == "highcharts-tooltip")
            {
                XmlNode[,] xmln = new XmlNode[1, 2];
                xmln[0, 0] = xml.FirstChild;
                xmln[0, 1] = xNod;
                dic.Add(i, xmln);
            }
        }
        foreach (KeyValuePair<int, XmlNode[,]> a in dic)
        {
            a.Value[0, 0].RemoveChild(a.Value[0, 1]);
        }
        return xml.OuterXml;
    }

    /// <summary>
    /// 修复折线图导出线没有颜色且很粗bugger
    /// </summary>
    /// <param name="svg"></param>
    /// <returns></returns>
    private string SvgLine(string svg)
    {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(svg);
        XmlNodeList nodeListAllg = xml.GetElementsByTagName("g");

        foreach (XmlNode xNod in nodeListAllg)
        {
            XmlNode xmlvisibility = xNod.Attributes.GetNamedItem("class");
            if (xmlvisibility != null && xmlvisibility.Value == "highcharts-series-group")
            {
                foreach (XmlNode xNod2 in xNod.ChildNodes)
                {
                    XmlNode xmlvisibility1 = xNod2.Attributes.GetNamedItem("class");
                    if (xmlvisibility1 != null && xmlvisibility1.Value == "highcharts-series")
                    {
                        for (int i = 0; i < xNod2.ChildNodes.Count; i++)
                        {
                            if (i + 1 < xNod2.ChildNodes.Count)
                            {
                                xNod2.ChildNodes[i + 1].Attributes["stroke"].Value = xNod2.ChildNodes[i].Attributes["stroke"].Value;
                                xNod2.ChildNodes[i + 1].Attributes["stroke-width"].Value = xNod2.ChildNodes[i].Attributes["stroke-width"].Value;
                            }
                        }
                    }
                }
            }
        }

        return xml.OuterXml;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.Form["type"] != null && Request.Form["svg"] != null && Request.Form["filename"] != null)
            {
                string tType = Request.Form["type"].ToString();
                string tSvg = Request.Form["svg"].ToString();

                tSvg = SvgRemoveHidden(tSvg);
                tSvg = SvgLine(tSvg);

                string tFileName = Request.Form["filename"].ToString();
                if (tFileName == "")
                {
                    tFileName = "chart";
                }
                MemoryStream tData = new MemoryStream(Encoding.UTF8.GetBytes(tSvg));
                MemoryStream tStream = new MemoryStream();
                string tTmp = new Random().Next().ToString();

                string tExt = "";
                string tTypeString = "";

                switch (tType)
                {
                    case "image/png":
                        tTypeString = "-m image/png";
                        tExt = "png";
                        break;
                    case "image/jpeg":
                        tTypeString = "-m image/jpeg";
                        tExt = "jpg";
                        break;
                    case "application/pdf":
                        tTypeString = "-m application/pdf";
                        tExt = "pdf";
                        break;
                    case "image/svg+xml":
                        tTypeString = "-m image/svg+xml";
                        tExt = "svg";
                        break;
                }

                if (tTypeString != "")
                {
                    string tWidth = Request.Form["width"].ToString();
                    Svg.SvgDocument tSvgObj = SvgDocument.Open(tData);

                    switch (tExt)
                    {
                        case "jpg":
                            tSvgObj.Draw().Save(tStream, ImageFormat.Jpeg);
                            break;
                        case "png":
                            tSvgObj.Draw().Save(tStream, ImageFormat.Png);
                            break;
                        case "pdf":
                            PdfWriter tWriter = null;
                            Document tDocumentPdf = null;
                            try
                            {
                                tSvgObj.Draw().Save(tStream, ImageFormat.Png);
                                tDocumentPdf = new Document(new Rectangle((float)tSvgObj.Width, (float)tSvgObj.Height));
                                tDocumentPdf.SetMargins(0.0f, 0.0f, 0.0f, 0.0f);
                                iTextSharp.text.Image tGraph = iTextSharp.text.Image.GetInstance(tStream.ToArray());
                                tGraph.ScaleToFit((float)tSvgObj.Width, (float)tSvgObj.Height);

                                tStream = new MemoryStream();
                                tWriter = PdfWriter.GetInstance(tDocumentPdf, tStream);
                                tDocumentPdf.Open();
                                tDocumentPdf.NewPage();
                                tDocumentPdf.Add(tGraph);
                                tDocumentPdf.CloseDocument();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                tDocumentPdf.Close();
                                tDocumentPdf.Dispose();
                                tWriter.Close();
                                tWriter.Dispose();
                                tData.Dispose();
                                tData.Close();
                            }
                            break;

                        case "svg":
                            tStream = tData;
                            break;
                    }

                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = tType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + tFileName + "." + tExt + "");
                    Response.BinaryWrite(tStream.ToArray());
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }

}