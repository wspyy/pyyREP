using Bll;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_login_Click(object sender, EventArgs e)
    {
        Base_UserInfo info = new UserBll().UserLogin(this.txt_username.Text,txt_pass.Text);
        if (info == null)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "", " $.AlertMess('提示', '请确认用户名密码输入正确。');", true);
        }
        else {
            Session["User"] = info;
            Response.Redirect("index.html");
        }
    }
}