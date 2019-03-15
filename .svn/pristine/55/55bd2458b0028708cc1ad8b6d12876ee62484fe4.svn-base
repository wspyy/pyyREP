using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Devoops_ForeCast_QXForeCastHist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txt_begin.Value = DateTime.Now.ToString("yyyy-MM-dd");
            ddl_day.Items.Add( "上午");
            ddl_day.Items.Add( "下午");
            ddl_hour.SelectedValue = ("上午").ToString();

            ddl_hour.Items.Add(24 + "小时");
            ddl_hour.Items.Add(48 + "小时");
            ddl_hour.Items.Add(72+ "小时");

            ddl_hour.SelectedValue = (24 + "小时").ToString();
        }
    }
}