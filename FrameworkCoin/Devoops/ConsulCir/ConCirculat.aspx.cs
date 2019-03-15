﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.SingleTable;
using Bll.BusinessFun;
using Model;
using System.Text;
using System.Configuration;


public partial class Devoops_ConsulCir_ConCirculat : AudiPage
{
    public string ShowRpt, strReportDate, strReportCode,strCode = "";
    public DateTime ReportDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            strReportDate = Session["clientDate"].ToString();
        }
        catch (Exception)
        {
            //Response.Redirect("../Login.aspx",true);
            Response.Write("<script>top.window.location.href = '../Login.aspx';</script>");
            return;
        }
        
        ReportDate = new DateTime(int.Parse(strReportDate.Split('-')[0]), int.Parse(strReportDate.Split('-')[1]), int.Parse(strReportDate.Split('-')[2]));
        string rptUser = ConfigurationManager.AppSettings["ReportUser"].ToString();
        string curUser = Session["UserID"].ToString();
        if (rptUser == curUser) ShowRpt = "true";
        if (!IsPostBack)
        {
            lbl_time.Text = DateTime.Now.ToString("yyyy年MM月dd日");
            InitHuiShang();
            BindDataSelect();
            DataTable indata = bill.Get_AvgHourData().Tables[0];
            StringBuilder strb = new StringBuilder();
            strb.Append("<table class=\"tab\" id=\"tb_report\" cellspacing=\"0\" cellpadding=\"0\" style=\"width: 100%; margin-top: -10px;\">");
            strb.Append("<tr><th>二氧化硫</th>");
            strb.Append("<th>二氧化氮</th>");
            strb.Append("<th>一氧化碳</th>");
            strb.Append("<th>臭氧</th>");
            strb.Append("<th>PM<sub>10</sub></th>");
            strb.Append("<th>PM<sub>2.5</sub></th>");
            strb.Append("<th>空气质量指数(AQI)</th>");
            strb.Append("<th>首要污染物</th>");
            strb.Append("<th>空气质量指数级别</th>");
            strb.Append("<th>类别</th></tr>");
            strb.Append("<tr>");
            if (indata != null && indata.Rows.Count > 0)
            {
                strb.Append("<td>" + indata.Rows[0]["so2"] + "</td>");
                strb.Append("<td>" + indata.Rows[0]["no2"] + "</td>");
                strb.Append("<td>" + indata.Rows[0]["co"] + "</td>");
                strb.Append("<td>" + indata.Rows[0]["o3"] + "</td>");
                strb.Append("<td>" + indata.Rows[0]["pm10"] + "</td>");
                strb.Append("<td>" + indata.Rows[0]["pm25"] + "</td>");
                strb.Append("<td>" + indata.Rows[0]["aqi"] + "</td>");
                strb.Append("<td>" + indata.Rows[0]["fp"] + "</td>");
                strb.Append("<td>" + indata.Rows[0]["cate"] + "</td>");
                strb.Append("<td>" + indata.Rows[0]["des"] + "</td>");
            }
            else {
                strb.Append("<td colspan='10' style='text-align:center; height:40px;'>暂无数据</td>");
            }
            strb.Append("</tr></table>");
            tbreport.InnerHtml = strb.ToString();
        }
    }
    AirForeCast bill = new AirForeCast();
    ConsultReportDal fistbill = new ConsultReportDal();

    #region 初始化会商值
    private void InitHuiShang()
    {
        DateTime startTime = ReportDate;
        DateTime endTime = startTime.AddDays(2);
        DataSet dataset = bill.GetAirContrastStatData(startTime,endTime);
        if (dataset != null && dataset.Tables.Count > 0)
        {
            int aqicolumns = dataset.Tables[0].Columns.Count;
            int fpcolumns = dataset.Tables[0].Columns.Count;
            int qzIndex = 0;// dataset.Tables[0].Rows.Count - 1;
            //绑定AQI
            if (aqicolumns > 2)
                txt_td.Value = dataset.Tables[0].Rows[qzIndex][2].ToString();
            if (aqicolumns > 3)
                txt_su.Value = dataset.Tables[0].Rows[qzIndex][3].ToString();
            if (aqicolumns > 4)
                txt_tfu.Value = dataset.Tables[0].Rows[qzIndex][4].ToString();
            //绑定首要污染物
            if (fpcolumns > 2)
                hd_day.Value = dataset.Tables[1].Rows[qzIndex][2].ToString();
            if (fpcolumns > 3)
                hd_day1.Value = dataset.Tables[1].Rows[qzIndex][3].ToString();
            if (fpcolumns > 4)
                hd_day2.Value = dataset.Tables[1].Rows[qzIndex][4].ToString();
        }
        //绑定空气质量级别
        BindAirQuaLevel(ddlQuauLeveltd, GetAirLevel(txt_td.Value));
        BindAirQuaLevel(ddlQuauLevelsu, GetAirLevel(txt_su.Value));
        BindAirQuaLevel(ddlQuauLeveltfu, GetAirLevel(txt_tfu.Value));

        BindPoll(ddl_day,"");
        BindPoll(ddl_day1,"");
        BindPoll(ddl_day2, "");
    }
    #endregion
    int[] fn = new int[3];
    double[] AQI = new double[3];
    string[] fp = new string[3];
    private void InArray(string code, ref int rows)
    {

        fn[0] = Convert.ToInt32(ddlQuauLeveltd.SelectedValue);
        fp[0] = hd_day.Value;
        fn[1] = Convert.ToInt32(ddlQuauLevelsu.SelectedValue);
        fp[1] = hd_day1.Value;

        fn[2] = Convert.ToInt32(ddlQuauLeveltfu.SelectedValue);
        fp[2] = hd_day2.Value;

        AQI[0] = Convert.ToDouble(txt_td.Value.Trim());

        AQI[1] = Convert.ToDouble(txt_su.Value.Trim());

        AQI[2] = Convert.ToDouble(txt_tfu.Value.Trim());

        rows = bill.InserMoreData(fn, AQI, code, fp, ReportDate);
    }
    private void BindDataSelect()
    {
        int int_data = bill.SelectReport(ReportDate.ToShortDateString()).Rows.Count;
        DataTable indatas = bill.Get_Bas_DateData(strReportDate);
        DataTable typeData = null;
        if (int_data > 0)
        {
            typeData = indatas;
            this.btn_save.Text = "修改";
        }
        else
        {
            typeData = bill.Get_Bas_SelectStardData().Tables[0];
        }
        if (typeData != null)
        {
            DataRow[] arrayDR = typeData.Select();
            foreach (DataRow dr in arrayDR)
            {
                if (dr["MonitorDay"].ToString() == ReportDate.Day.ToString())
                {
                    hd_day.Value = dr["firstp"].ToString();
                    ddlQuauLeveltd.SelectedValue = dr["QualityLevel"].ToString();
                    laQuauLeveltd.Text = dr["Proposal"].ToString();
                    txt_td.Value = dr["AQI"].ToString();
                }
                else if (dr["MonitorDay"].ToString() == ReportDate.AddDays(1).Day.ToString())
                {
                    ddlQuauLevelsu.SelectedValue = dr["QualityLevel"].ToString();
                    laQuauLevelsu.Text = dr["Proposal"].ToString();
                    txt_su.Value = dr["AQI"].ToString();
                    hd_day1.Value = dr["firstp"].ToString();
                }
                else if (dr["MonitorDay"].ToString() == ReportDate.AddDays(2).Day.ToString())
                {
                    ddlQuauLeveltfu.SelectedValue = dr["QualityLevel"].ToString();
                    laQuauLeveltfu.Text = dr["Proposal"].ToString();
                    txt_tfu.Value = dr["AQI"].ToString();
                    hd_day2.Value = dr["firstp"].ToString();
                }
            }
        }
        else
        {

        }
    }
    //市环境监测站人员保存操作，其中包括数据插入，数据修改等
    protected void btn_save_Click(object sender, EventArgs e)
    {
        TmidReportModel SecModel = new TmidReportModel();
        TmidReportDataModel FmoDel = new TmidReportDataModel();
        bool result = false;
        int rows = 0;
        DataTable dt = bill.SelectReport(strReportDate);
        int int_data = dt.Rows.Count;
        string YJ = "";
        string YJColor = "";
        string firstP = "";//首要污染物
        Int32 PolLast = 0;//污染物持续时间
        bool firstDay = false,secondDay=false;//某天是否超标
        try
        {
           int td=Convert.ToInt32(txt_td.Value);
           int su=Convert.ToInt32(txt_su.Value);
           int fu=Convert.ToInt32(txt_tfu.Value);
       
           if (!chk_jch.Checked && !chk_tsh.Checked)
           {
               if (td >= 500 || su >= 500 || fu >= 500)
               {
                   YJ = "一级";
                   YJColor = "红色";
               }
               else if (td >= 300 || su >= 300 || fu >= 300)
               {
                   YJ = "二级";
                   YJColor = "橙色";
               }
               else if (td >= 200 || su >= 200 || fu >= 200)
               {
                   YJ = "三级";
                   YJColor = "黄色";
               }
               else if (td >= 200 || su >= 200 || fu >= 200)
               {
                   YJ = "四级";
                   YJColor = "蓝色";
               }

               #region 判断是否超标
               if (td >= 500 || td >= 300 || td >= 200)
               {
                   SecModel.PolStart = ReportDate.Date;
                   firstP = hd_day.Value;
                   PolLast = 1;
                   firstDay = true;
               }
               if ((su >= 500 || su >= 300 || su >= 200) && firstDay)
               {
                   firstP =hd_day.Value+ ";" + hd_day1.Value;
                   PolLast += 1;
                   secondDay = true;
               }else if((su >= 500 || su >= 300 || su >= 200)&&firstDay==false)
               {
                   SecModel.PolStart = ReportDate;
                   firstP = hd_day1.Value;
                   PolLast = 1;
                   secondDay = true;
               }

               if ((fu >= 500 || fu >= 300 || fu >= 200) && firstDay && secondDay)
               {
                   firstP = hd_day.Value+";"+hd_day1.Value+";"+hd_day2.Value;
                   PolLast += 1;
               }
               else if ((fu >= 500 || fu >= 300 || fu >= 200) && firstDay == false && secondDay)
               {
                   SecModel.PolStart = ReportDate.Date.AddDays(1);
                   firstP =hd_day1.Value+ ";" + hd_day2.Value;
                   PolLast =2;
               }
               else if ((fu >= 500 || fu >= 300 || fu >= 200) && firstDay&& secondDay==false)
               {
                   SecModel.PolStart = ReportDate;
                   firstP = hd_day.Value;
                   PolLast = 1;
               }
               else if ((fu >= 500 || fu >= 300 || fu >= 200) && firstDay==false && secondDay == false)
               {
                   SecModel.PolStart = ReportDate.AddDays(2);
                   firstP = hd_day2.Value;
                   PolLast = 1;
               }
               SecModel.PolLast = PolLast;
               if (PolLast > 0)
                   SecModel.Firstp =RemoveRepeatFirstP( firstP);
               #endregion

           }

           SecModel.PolRange = getCurCityName();
            SecModel.YJDJ = YJ;
            SecModel.YJColor = YJColor;
            SecModel.JCSubmit = 0;
            SecModel.JCUser = Session["UserID"].ToString();
            SecModel.QXSubmit = 0;
            SecModel.IsJCH = chk_jch.Checked.ToString();
            SecModel.IsTSH = chk_tsh.Checked.ToString();
            SecModel.ReprtTime = ReportDate.Date;
            SecModel.QXText = taID.Value;

            //if (int_data > 0 && dt.Rows[0]["JCSubmit"].ToString()!="1")
            //{
            //    result = bill.UpdataConMessage(SecModel, ref strCode);
            //}
            //else
            //{
            //    result = bill.AddConMessage(SecModel, ref strCode);
            //}
            if (int_data == 0 || dt.Rows[0]["JCSubmit"].ToString() == "1")
            {
                result = bill.AddConMessage(SecModel, ref strCode);
                dt = bill.SelectReport(strReportDate);
            }
            result = bill.UpdataConMessage(SecModel, ref strCode);
            InArray(strCode, ref rows);
            
            if (waitSubmit.Value == "true")
            {
                int sub = bill.SubmitData(strReportDate, 1);
                result = bill.AddConMessage(SecModel, ref strCode);
                InArray(strCode, ref rows);
            }
            if (YJ.Length>0)
            {
                strCode = bill.strCode(strReportDate,"1");
            }
        }
        catch
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WasSave", "WasSave('fail')", true);
        }

        if (result && rows == 3)
        {
            this.btn_save.Text = "修改";
            BindDataSelect();
            if (int_data > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "WasSave", "WasSave('modify','" + YJ + "','" + chk_jch.Checked + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "WasSave", "WasSave('OK','" + YJ + "','" + chk_jch.Checked + "');", true);
            }

        }
        else
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WasSave", "WasSave('fail')", true);
        }
    }
    /// <summary>
    /// 消除重数据
    /// </summary>
    /// <param name="firstp"></param>
    /// <returns></returns>
    private string RemoveRepeatFirstP(string firstp)
    {
        if (string.IsNullOrEmpty(firstp)) return null;
        string[] arry = firstp.Replace(',',';').Split(';');
        var s = (from a in arry where !a.Equals("") select a).Distinct().ToArray();
        string newFirstp = String.Join(";", s);
        return newFirstp;
    }

    private void BindPoll(DropDownList ddl,string selected)
    {
        string[] array = { "CO", "SO2", "NO2", "O3", "PM25", "PM10" };
        ddl.Items.Clear();
        foreach (var  r in array)
        {
            ListItem item = new ListItem();
            item.Text = r;
            item.Value = r;
            if ((r).Equals(selected))
                item.Selected = true;
            else
                item.Selected = false;
            ddl.Items.Add(item);
        }
    }

    private void BindAirQuaLevel(DropDownList ddl,string selected)
    {
        DataTable typeData = bill.Get_Bas_StardData().Tables[0];
        if (typeData == null) return;
        ddl.Items.Clear();
        foreach (DataRow r in typeData.Rows)
        {
            ListItem item = new ListItem();
            item.Text = r["QualityIndex"].ToString();
            item.Value = r["QualityLevel"].ToString();
            if (item.Value.Equals(selected))
                item.Selected = true;
            else
                item.Selected = false;
            ddl.Items.Add(item);
        }
    }
    private string GetAirLevel(string sValue)
    {
        int temp = 0;
        if (!int.TryParse(sValue, out temp))
        {
            return "1";
        }
        Int32 airValue = Convert.ToInt32(sValue);
        if (airValue > 0 && airValue <= 50)
            return "1";
        else if (airValue > 50 && airValue <= 100)
            return "2";
        else if (airValue > 100 && airValue <= 150)
            return "3";
        else if (airValue > 150 && airValue <= 200)
            return "4";
        else if (airValue > 200 && airValue <= 300)
            return "5";
        else if (airValue > 300 && airValue <=500)
            return "6";
        return "0";
    }
    protected void hd_day_ValueChanged(object sender, EventArgs e)
    {

    }
    protected void ddl_day_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlQuauLeveltd_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
