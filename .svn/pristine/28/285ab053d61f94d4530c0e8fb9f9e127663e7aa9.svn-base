using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using Bll.BusinessFun;
using System.Text;
using System.Data;

public partial class Devoops_ForeCast_WarnInfoDetail : System.Web.UI.Page
{
    protected string _DetailHtml = "";
    ForCastWarnInfoDal bill = new ForCastWarnInfoDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitData();
        }
    }
    private void InitData()
    {
        StringBuilder _StrB = new StringBuilder();
        DataTable dt = bill.SelectWDetaile().Tables[0];
        List<string> _List = new List<string>();
        _DetailHtml += GetHtml(dt, ref _List);

        if (_DetailHtml == "")
        {
            _DetailHtml = "<tr><td colspan='7' style='padding:5px 0px; text-align:center; '>暂无数据</td></tr>";

        }
    }
    AirForeCast sqlh = new AirForeCast();
   
    private string GetHtml(DataTable dt, ref List<string> _ListPollutantCode)
    {
        StringBuilder _StrB = new StringBuilder();

        DataTable standard = sqlh.Get_Bas_PollutStard().Tables[0];
        DataTable region=bill.GetQxStationData().Tables[0];
        int addrow = dt.Rows.Count-1;
        int addid = 0;
        if (dt.Rows.Count > 0)
        {
          addid = Convert.ToInt32(dt.Rows[addrow]["id"].ToString()) + 1;
        }
        int second = 0;
        if (dt.Rows.Count > 0)
        {

            foreach (DataRow dr in dt.Rows)
            {

                _StrB.Append("<tr id=\"tr" + dr["id"] + "\" class=\"StatusSave\">");
                _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\">");
                _StrB.Append("<select id=\"regionid" + dr["id"] + "\">");
                if (region.Rows.Count > 0)
                {
                    for (int i = 0; i < region.Rows.Count; i++)
                    {
                        _StrB.Append("<option value=\"" + region.Rows[i]["StationCode"].ToString() + "\">" + region.Rows[i]["StationName"].ToString() + "</option>");
                    }
                }
                _StrB.Append("</select><span id=\"spanAQI" + dr["id"] + "\">" + dr["StationName"].ToString() + "</span></td>");
                _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\" align=\"center\">");
                _StrB.Append("<select id=\"standardid" + dr["id"] + "\">");
                if (standard.Rows.Count > 0)
                {
                    for (int i = 0; i < standard.Rows.Count; i++)
                    {
                        _StrB.Append("<option value=\"" + standard.Rows[i]["QualityLevel"].ToString() + "\">" + standard.Rows[i]["QualityIndex"].ToString() + "</option>");
                    }
                }
                _StrB.Append("</select><span id=\"spanAQI" + dr["id"] + "\">" + dr["QualityCet"].ToString() + "</span></td>");
                _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\" align=\"center\" valign='middle'>");
                _StrB.Append("<input type=\"text\" id=\"txtAQI" + dr["id"] + "\" value=\"" + dr["AQI"].ToString() + "\" />");
                _StrB.Append("<span id=\"spanAQI" + dr["id"] + "\">" + dr["AQI"].ToString() + "</span>");
                _StrB.Append("</td>");
                _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\" align=\"center\">");
                _StrB.Append("<input type=\"text\" id=\"txtFirstP" + dr["id"] + "\" value=\"" + dr["FirstP"].ToString() + "\" />");
                _StrB.Append("<span id=\"spanFirstP" + dr["id"] + "\">" + dr["FirstP"].ToString() + "</span>");
                _StrB.Append("</td>");

                _StrB.Append("<td align=\"center\" valign='middle'>");
                _StrB.Append("<input type=\"button\" id=\"btnSave" + dr["id"] + "\" class=\"Button_Save\" value=\"保存\" name=\"" + dr["id"] + "\" />");
                _StrB.Append("<input type=\"button\" id=\"btnCancel" + dr["id"] + "\" class=\"Button_Closed\" value=\"取消\" name=\"" + dr["id"] + "\" />");
                _StrB.Append("<input type=\"button\" id=\"btnEdit" + dr["id"] + "\" class=\"Button_Edit\" value=\"编辑\" name=\"" + dr["id"] + "\" />");
                _StrB.Append("<input type=\"button\" id=\"btnDelete" + dr["id"] + "\" class=\"Button_Del\" value=\"删除\" name=\"" + dr["id"] + "\" />");
                _StrB.Append("</td>");
                _StrB.Append("</tr>");


            }
            _StrB.Append("<tr id=\"tr" + addid + "\" class=\"StatusSave\">");
            _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\">");
            _StrB.Append("<select id=\"regionid" + addid + "\">");
            if (region.Rows.Count > 0)
            {
                for (int i = 0; i < region.Rows.Count; i++)
                {
                    _StrB.Append("<option value=\"" + region.Rows[i]["StationCode"].ToString() + "\">" + region.Rows[i]["StationName"].ToString() + "</option>");
                }
            }
            _StrB.Append("</select></td>");
            _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\" align=\"center\">");
            _StrB.Append("<select id=\"standardid" + addid + "\">");
            if (standard.Rows.Count > 0)
            {
                for (int i = 0; i < standard.Rows.Count; i++)
                {
                    _StrB.Append("<option value=\"" + standard.Rows[i]["QualityLevel"].ToString() + "\">" + standard.Rows[i]["QualityIndex"].ToString() + "</option>");
                }
            }
            _StrB.Append("</select></td>");
            _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\" align=\"center\" valign='middle'>");
            _StrB.Append("<input type=\"text\" id=\"txtAQI" + addid + "\" value=\"\" />");
            _StrB.Append("<span id=\"spanAQI" + addid + "\"></span>");
            _StrB.Append("</td>");
            _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\" align=\"center\">");
            _StrB.Append("<input type=\"text\" id=\"txtFirstP" + addid + "\" value=\"\" />");
            _StrB.Append("<span id=\"spanFirstP" + addid + "\"></span>");
            _StrB.Append("</td>");

            _StrB.Append("<td align=\"center\" valign='middle'>");
            _StrB.Append("<input type=\"button\" id=\"Button_Add" + addid + "\" class=\"Button_Add\" value=\"添加\" name=\"" + addid + "\" />");
            _StrB.Append("<input type=\"button\" id=\"btnCancel" + addid + "\" class=\"Button_Closed\" value=\"取消\" name=\"" + addid + "\" />");
            _StrB.Append("<input type=\"button\" id=\"btnEdit" + addid + "\" class=\"Button_Edit\" value=\"编辑\" name=\"" + addid + "\" />");
            _StrB.Append("<input type=\"button\" id=\"btnDelete" + addid + "\" class=\"Button_Del\" value=\"删除\" name=\"" + addid + "\" />");

            _StrB.Append("</td>");
            _StrB.Append("</tr>");
        }
        else
        {
            _StrB.Append("<tr id=\"tr" + second + "\" class=\"StatusSave\">");
            _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\">");
            _StrB.Append("<select id=\"regionid" + second + "\">");
            if (region.Rows.Count > 0)
            {
                for (int i = 0; i < region.Rows.Count; i++)
                {
                    _StrB.Append("<option value=\"" + region.Rows[i]["StationCode"].ToString() + "\">" + region.Rows[i]["StationName"].ToString() + "</option>");
                }
            }
            _StrB.Append("</select></td>");
            _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\" align=\"center\">");
            _StrB.Append("<select id=\"standardid" + second + "\">");
            if (standard.Rows.Count > 0)
            {
                for (int i = 0; i < standard.Rows.Count; i++)
                {
                    _StrB.Append("<option value=\"" + standard.Rows[i]["QualityLevel"].ToString() + "\">" + standard.Rows[i]["QualityIndex"].ToString() + "</option>");
                }
            }
            _StrB.Append("</select></td>");
            _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\" align=\"center\" valign='middle'>");
            _StrB.Append("<input type=\"text\" id=\"txtAQI" + second + "\" value=\"\" />");
            _StrB.Append("<span id=\"spanAQI" + second + "\"></span>");
            _StrB.Append("</td>");
            _StrB.Append("<td class=\"Main_Tab_Style_Bgcolor_white\" align=\"center\">");
            _StrB.Append("<input type=\"text\" id=\"txtFirstP" + second + "\" value=\"\" />");
            _StrB.Append("<span id=\"spanFirstP" + second + "\"></span>");
            _StrB.Append("</td>");

            _StrB.Append("<td align=\"center\" valign='middle'>");
            _StrB.Append("<input type=\"button\" id=\"Button_Add" + second + "\" class=\"Button_Add\" value=\"保存\" name=\"" + second + "\" />");
            _StrB.Append("<input type=\"button\" id=\"btnCancel" + second + "\" class=\"Button_Closed\" value=\"取消\" name=\"" + second + "\" />");
            _StrB.Append("<input type=\"button\" id=\"btnEdit" + second + "\" class=\"Button_Edit\" value=\"添加\" name=\"" + second + "\" />");

            _StrB.Append("</td>");
            _StrB.Append("</tr>");
        }

        return _StrB.ToString();
    }
}