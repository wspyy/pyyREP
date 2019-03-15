<%@ WebHandler Language="C#" Class="GetTreeClick" %>

using System;
using System.Web;
using System.SingleTable;
using System.SingleTable.PublicMethod;

public class GetTreeClick : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string strSql = "";
        if (context.Request["param"] != null && context.Request["param"] == "CommonTree")
        {
            strSql = " select NodeID as id,OnClick from T_Sys_CommonTree ";

            if (context.Request["sqlWhere"] != null && context.Request["sqlWhere"] != "")
            {
                strSql += " where  1 = 1 and  " + context.Request["sqlWhere"];
            }
            strSql += " order by OrderID";
        }
        string strUrl = "";
        SQLHelper sqlH = new SQLHelper();
        System.Data.DataSet ds = sqlH.ExecuteSQLDataSet(strSql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUrl = ds.Tables[0].Rows[0]["OnClick"].ToString();
        }
        context.Response.Write(strUrl);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}