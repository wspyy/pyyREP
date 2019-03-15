using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Text;
using CityKore.Web.AppCode;
using System.Runtime.Serialization;

public partial class Framework_LisenceApply : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userName"] == null)
        {
            Response.Redirect("../../Index.aspx");
        }
    }
    [WebMethod]
    public static string DESEncrypt(string ip1, string ip2, string ip3, string time1, string time2)
    {
        if (ip1 == "")
        {
            ip1 = "ip1";
        }
        if (ip2 == "")
        {
            ip2 = "ip2";
        }
        if (ip3 == "")
        {
            ip3 = "ip3";
        }
        if (time1 == "")
        {
            time1 = "time1";
        }
        if (time2 == "")
        {
            time2 = "time2";
        }
        string input = ip1 + "," + ip2 + "," + ip3 + "," + time1 + "," + time2;
        byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes("");
        byte[] bytesDESSrc = ASCIIEncoding.ASCII.GetBytes(input);
        DesKey des = new DesKey(bytesDESKey);
        des.encrypt(bytesDESSrc);
        return Convert.ToBase64String(bytesDESSrc);
    }
    [WebMethod]
    public static string DESDecrypt(string src, string k)
    {
        byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(k);
        byte[] bytesDESSrc = ASCIIEncoding.ASCII.GetBytes(src);
        DesKey des = new DesKey(bytesDESKey);
        des.decrypt(bytesDESSrc);

        return Convert.ToBase64String(bytesDESSrc);

    }

    [WebMethod]
    public static string getCode(string ip1,string ip2,string ip3,string time1,string time2)
    {
        string code = "";
        code = DESEncrypt(ip1, ip2, ip3, time1, time2);
        return code;
    }
}