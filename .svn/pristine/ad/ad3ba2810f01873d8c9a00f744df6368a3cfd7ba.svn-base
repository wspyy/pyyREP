using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Devoops_MoniData_AirHourHistData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txt_begin.Value = DateTime.Now.ToString("yyyy-MM-dd");
            //InitTableContent();
            for(int i=0;i<=23;i++){
                ddl_hour.Items.Add(new ListItem(i + "时", i.ToString()));
            }
            ddl_hour.SelectedValue = (DateTime.Now.Hour - DateTime.Now.Hour).ToString();
        }
    }
}