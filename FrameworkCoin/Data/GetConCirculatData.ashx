<%@ WebHandler Language="C#" Class="GetConCirculatData" %>

using System;
using System.Web;
using System.Data;
using Bll.BusinessFun;
using System.Configuration;
using System.Web.SessionState;
using System.Text;


public class GetConCirculatData : IHttpHandler, IRequiresSessionState
{
    string jcDeparID = ConfigurationManager.AppSettings["JCDepartID"].ToString();//获取部门ID
    string seDeparid = "";
    string strUserID = "";
    string strreport = "";
    string year = "";
    string month = "";
    int date = 0;
    string reportDate = "";
    
    public void ProcessRequest (HttpContext context) {
        reportDate = context.Session["clientDate"].ToString();
        int selectdata = 0;
        string QXTexts = "";
        string PollutName = "";
        string strStyple = context.Request["stype"];
        if (context.Request["Name"] != null && context.Request["region"] != null)
        {
            PollutName = context.Request["Name"].ToString();
            QXTexts = context.Request["region"].ToString();
        }
        if (context.Request["stryear"] != null && context.Request["stryear"].ToString() != "")
        {
            year =context.Request["stryear"].ToString();
        }
        if (context.Request["strmonth"] != null && context.Request["strmonth"].ToString() != "")
        {
            month =context.Request["strmonth"].ToString();
        }
        if (context.Request["strdate"] != null && context.Request["strdate"].ToString() != "")
        {
            date =Convert.ToInt32(context.Request["strdate"].ToString());
        }
        if (context.Session["departID"] != null && context.Session["departID"].ToString() != "")
        {
            seDeparid = context.Session["departID"].ToString();
            
        }
        if (context.Session["UserID"] != null && context.Session["UserID"].ToString() != "")
        {
            strUserID = context.Session["UserID"].ToString();
        }
        if (context.Request["selectdata"] != null && context.Request["selectdata"] != "")
        {
            selectdata = Convert.ToInt32(context.Request["selectdata"]);
        }
        if (context.Request["QXText"] != null && context.Request["QXText"] != "")
        {
            QXTexts = context.Request["QXText"];                
        }
        if (context.Request["txtdata"] != null && context.Request["txtdata"] != "")
        {
            QXTexts = context.Request["txtdata"];
        }
        string strContent = MakeTableAndData(strStyple, selectdata, QXTexts, PollutName);
        context.Response.ContentType = "text/plain";
        context.Response.Write(strContent);
    }

    private string MakeTableAndData(string stype, int data, string strQXT, string PollutName)
    {
        string strnull="";
        AirForeCast bill = new AirForeCast();

        if (stype == "yprocast")
        {
            DataSet ypTable = bill.Get_YesDay_ForecasData();
            DataSet preTable = bill.Get_YesDay_PracData();
            DataTable Datatables = ypTable.Tables[0];
            DataTable Datatablepre=preTable.Tables[0];
            string stableData = "";


            stableData += "<table class=\"tab\" cellspacing=\"0\" cellpadding=\"0\" rules=\"all\" border=\"1\" style=\"width:100%;border-collapse:collapse;\">";
            stableData += "<th>数据类型</th><th>日期</th><th>空气质量指数</th><th>空气质量等级</th><th>空气质量类别</th><th>首要污染物</th>";

            if (Datatables.Rows.Count > 0)
            {
                stableData += "<tr class=\"\">";
                stableData += "<td scope=\"col\" width=\"20%\" style=\"text-align:center\">预测结果</td><td>"+DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")+"</td><td  style=\"text-align:center\">" + Datatables.Rows[0]["AQI"].ToString() + "</td><td scope=\"col\" width=\"20%\" style=\"text-align:center\">" + Datatables.Rows[0]["AirLevel"].ToString() + "</td><td scope=\"col\" width=\"20%\" style=\"text-align:center\">" + Datatables.Rows[0]["Category"].ToString() + "</td>";
                stableData += "<td scope=\"col\" width=\"20%\" style=\"text-align:center\">" + Datatables.Rows[0]["FirstP"].ToString() + "</td></tr>";
            }
            else
            {
                stableData += "<tr class=\"\">";
                stableData += "<td scope=\"col\" width=\"20%\" style=\"text-align:center\">预测结果</td><td></td><td></td><td scope=\"col\" width=\"20%\" style=\"text-align:center\"></td><td scope=\"col\" width=\"20%\" style=\"text-align:center\"></td>";
                stableData += "<td scope=\"col\" width=\"20%\" style=\"text-align:center\"></td></tr>";
            }
            if (Datatablepre.Rows.Count > 0)
            {
                stableData += "<tr class=\"\">";
                stableData += "<td scope=\"col\" width=\"20%\" style=\"text-align:center\">实测结果</td><td>" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "</td><td  style=\"text-align:center\">" + Datatablepre.Rows[0]["AQI"].ToString() + "</td><td scope=\"col\" width=\"20%\" style=\"text-align:center\">" + Datatablepre.Rows[0]["AirLevel"].ToString() + "</td><td scope=\"col\" width=\"20%\" style=\"text-align:center\">" + Datatablepre.Rows[0]["AirLevelDes"].ToString() + "</td>";
                stableData += "<td scope=\"col\" width=\"20%\" style=\"text-align:center\">" + Datatablepre.Rows[0]["FirstP"].ToString() + "</td></tr></table>";
            }
            else
            {
                stableData += "<tr class=\"\">";
                stableData += "<td scope=\"col\" width=\"20%\" style=\"text-align:center\">实测结果</td><td></td><td  style=\"text-align:center\"></td><td scope=\"col\" width=\"20%\" style=\"text-align:center\"></td><td scope=\"col\" width=\"20%\" style=\"text-align:center\"></td>";
                stableData += "<td scope=\"col\" width=\"20%\" style=\"text-align:center\"></td></tr></table>";
            }
            
            
                return stableData;
        }
       
        else if (stype == "hourprac")
        {
            DataSet houSet = bill.Get_AvgHourData();
            DataTable hoTable = houSet.Tables[0];

            string strData = "";

            if (hoTable.Rows.Count > 0)
            {
              
               
                strData += "<table class=\"\" cellspacing=\"0\" cellpadding=\"0\" rules=\"all\" border=\"1\" style=\"width:100%;border-collapse:collapse;\">";
                strData += "<tr class=\"\">";
                strData += "<th scope=\"col\"  width=\"16%\" style=\"text-align:center\">SO₂：</th><td scope=\"col\" width=\"16%\" style=\"text-align:center\">" + hoTable.Rows[0]["SO2"].ToString() + "μg/m³" + "</td>";
                strData += "<th scope=\"col\" width=\"16%\" style=\"text-align:center\">NO₂：</th><td scope=\"col\" width=\"16%\" style=\"text-align:center\">" + hoTable.Rows[0]["NO2"].ToString() + "μg/m³" + "</td>";
                strData += "<th scope=\"col\" width=\"16%\" style=\"text-align:center\">PM<sub>10</sub>：</th><td scope=\"col\" width=\"16%\" style=\"text-align:center\">" + hoTable.Rows[0]["PM10"].ToString() + "μg/m³" + "</td></tr>";
                strData += "<tr class=\"\">";
                strData += "<th scope=\"col\" width=\"16%\" style=\"text-align:center\">CO：</th><td scope=\"col\" width=\"16%\" style=\"text-align:center\">" + hoTable.Rows[0]["CO"].ToString() + "mg/m³" + "</td>";
                strData += "<th scope=\"col\" width=\"16%\" style=\"text-align:center\">O₃：</th><td scope=\"col\" width=\"16%\" style=\"text-align:center\">" + hoTable.Rows[0]["O3"].ToString() + "μg/m³" + "</td>";
                strData += "<th scope=\"col\" width=\"16%\" style=\"text-align:center\">PM<sub>2.5</sub>：</th><td scope=\"col\" width=\"16%\" style=\"text-align:center\">" + hoTable.Rows[0]["PM25"].ToString() + "μg/m³" + "</td></tr>";
                strData += "</table>";
            }
            return strData;
        }
        else if (stype == "droplist")
        {
          
            StringBuilder oPJson = new StringBuilder();
            DataSet dataset = bill.Get_Bas_LableBind(data);
            DataTable seletable = dataset.Tables[0];
            if (seletable.Rows.Count > 0)
            {
                oPJson.Append("{HasData:\"yes\",MinValue:\"" + seletable.Rows[0]["MinValue"].ToString() + "\",Proposal:\"" + seletable.Rows[0]["Proposal"].ToString() + "\"");


                oPJson.Append("}");
            }
            else
            {
                oPJson.Append(" {HasData:\"no\"}");
            }
            return oPJson.ToString();
           
        }
        else if (stype == "QXtext")
        {
            string stRtext = "";
            bool strtrue=false;
            DataTable retable = bill.SelectReport(reportDate);
            int rows = retable.Rows.Count;
          
            if (rows > 0)
            {
                strtrue = bill.UpdataQXTextData(strQXT, strUserID,reportDate);
              
            }
            else
            {
                strtrue = bill.InsertQXtEXT(strQXT, strUserID, reportDate);
            }
           if (strtrue)
           {
               DataTable stable = bill.SelectReport(reportDate);
               if (stable.Rows.Count > 0)
               {
                   stRtext = stable.Rows[0]["QXText"].ToString();
               }
               return stRtext;
           }
                        
        }
        else if (stype == "selectQXText")
        {

            DataTable QXTable = bill.SelectReport(reportDate);
            return Newtonsoft.Json.JsonConvert.SerializeObject( QXTable);
        }
        else if (stype == "selecdate")
        {
            string strlidata = "";
          
           // strlidata += "<h4 class=\"page-header\">" + year + "-" + month + "</h4>";
            strlidata += "<ol style=\"margin-left:-50px;\">";
            for (int i = 1; i <= date; i++)
            {
                 
                string stri = "";
                if (i < 10)
                {
                    stri = "0" + i;
                }
                else
                {
                    stri = i.ToString();
                }
                 string strdate=year+"-"+month+"-"+stri;
                 string resintdata = bill.strCode(strdate,"1");
                 if (resintdata != null && resintdata != "")
                 {
                     strlidata += "<li><img name=\"" + strdate + "\" src=\"../img/report3.png\" onclick=\"showdialog('" + strdate + "');\"/><p>" + strdate + "</p></li>";
                 }
                 else
                 {
                      strlidata += "<li ><img name=\"" + strdate + "\" src=\"../img/report2.png\" /> <p>" + strdate + "</p></li>";
                 }
                
            }
            strlidata += "</ol>";

            return strlidata;
        }
        else if (stype == "submit")
        {
            string strsubmit = "";
            int sub = bill.SubmitData(reportDate,1);
            if (sub > 0)
            {
                strsubmit = "提交成功";

            }
            return strsubmit;

        }
        else if (stype == "selectsub")
        {
            string strsubmit = "";
            int sub = 0;
            DataTable subtable = bill.SelectReport(reportDate);
            if (subtable.Rows.Count > 0)
            {
                strsubmit = subtable.Rows[0]["JCSubmit"].ToString();
                if (strsubmit != "" && strsubmit != null)
                {
                    sub = Convert.ToInt32(strsubmit);
                }
                if (sub == 1)
                {
                    strsubmit = "已提交";
                    return strsubmit;
                }
                else
                {
                    if (seDeparid == jcDeparID)
                    {
                        strsubmit = "监测站登录";
                        return strsubmit;
                    }
                    else
                    {
                        strsubmit = "气象台登录";
                        return strsubmit;
                    }
                }


            }
            else
            {
                if (seDeparid == jcDeparID)
                {
                    strsubmit = "监测站登录没保存";
                    return strsubmit;
                }
                else
                {
                    strsubmit = "气象台登录没保存";
                    return strsubmit;
                    
                }
            }



        }
        else if (stype == "regionpollut")
        {
           StringBuilder oPJson=new StringBuilder();
            DataTable regiontable = bill.Get_Bas_region();
            DataTable pollution = bill.Get_Bas_pollution(" where TimeType=1");

            if (regiontable.Rows.Count > 0&&pollution.Rows.Count>0)
            {
                oPJson.Append("{HasData:\"yes\",region:[");
                for (int i = 0; i < regiontable.Rows.Count; i++)
                {
                    oPJson.Append("{Code:\"" + regiontable.Rows[i]["StationCode"].ToString() + "\",Name:\"" + regiontable.Rows[i]["StationName"].ToString() + "\"},");

                }
                oPJson.Remove(oPJson.ToString().Length - 1, 1);
                oPJson.Append("],");
                oPJson.Append("pollution:[");
                for (int i = 0; i < pollution.Rows.Count; i++)
                {
                    oPJson.Append("{Code:\"" + pollution.Rows[i]["ItemCode"].ToString() + "\",Name:\"" + pollution.Rows[i]["ItemName"] + "\"},");
                }
                oPJson.Remove(oPJson.ToString().Length - 1, 1);
                oPJson.Append("]");
                    oPJson.Append("}");

            }
            else
            {
                oPJson.Append("{HasData:\"no\"}");
            }
            return oPJson.ToString();
        }
        else if (stype == "selectchart")
        {
            StringBuilder dataPJson = new StringBuilder();
            DataTable pollution = bill.Get_Bas_pollution(" where TimeType=1 and ItemName='" + PollutName + "'");
            DataTable chartdata = bill.Get_Pollut_Data(PollutName, strQXT);
            if (pollution.Rows.Count > 0 && chartdata.Rows.Count>0)
            {
                dataPJson.Append("{HasData:\"yes\",WarnUp:\"" + pollution.Rows[0]["Two"].ToString() + "\",Data:[");

               
                    for (int i = 0; i < chartdata.Rows.Count; i++)
                    {
                        dataPJson.Append("{MoniDate:\"" + Convert.ToDateTime(chartdata.Rows[i]["MoniDate"]).Hour.ToString()+"时" + "\",MonitorValue:\"" + chartdata.Rows[i]["" + PollutName + ""].ToString() + "\"},");
                    }
                    dataPJson.Remove(dataPJson.ToString().Length - 1, 1);
                    
                
                dataPJson.Append("]");
                dataPJson.Append("}");
            }
            else
            {
                dataPJson.Append(" {HasData:\"no\"}");
            }
            return dataPJson.ToString();
        }
        else if (stype == "txtddl")
        {
            StringBuilder dataPJson = new StringBuilder();
            
        
            DataTable txtSetData = ConsultReportDal.GettxtddlData(strQXT);

            if (txtSetData.Rows.Count>0)
            {
                dataPJson.Append("{HasData:\"yes\",selelevel:\"" + txtSetData.Rows[0]["pollutlevel"].ToString() + "\",procal:\"" + txtSetData.Rows[0]["procal"].ToString() + "\"");


                dataPJson.Append("}");
            }
            else
            {
                dataPJson.Append(" {HasData:\"no\"}");
            }
            return dataPJson.ToString();
        }
            
        
 
        return strnull;
    }
   
    public bool IsReusable {
        get {
            return false;
        }
    }

}