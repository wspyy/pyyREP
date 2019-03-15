using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Devoops_ForeCast_AirForeCastStat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string tbhtml = tbHtml.Value;
        ExportToExcel(tbhtml, "多模式预测结果汇总");
    }


    /// <summary>
    /// 执行导出
    /// </summary>
    /// <param name="ds">要导出的DataSet</param>
    /// <param name="strExcelFileName">要导出的文件名</param>
    public void ExportToExcel(string dtSource, string fileName)
    {
        HttpResponse resp = System.Web.HttpContext.Current.Response;
        resp.Charset = "utf-8";
        resp.Clear();
        string filename = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
        resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
        resp.ContentEncoding = System.Text.Encoding.UTF8;
        
        resp.ContentType = "application/ms-excel";
        string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>";// +"<style> .table{ font: 9pt Tahoma, Verdana; color: #000000; text-align:center;  background-color:#8ECBE8;  }.table td{text-align:center;height:21px;background-color:#EFF6FF;}.table th{ font: 9pt Tahoma, Verdana; color: #000000; font-weight: bold; background-color: #8ECBEA; height:25px;  text-align:center; padding-left:10px;}</style>";
        resp.Write(style);
        resp.Write(dtSource);
        resp.Flush();
        resp.End();
    }
}