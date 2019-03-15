<%@ WebHandler Language="C#" Class="GetTreeData" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.SingleTable;
using System.SingleTable.PublicMethod;

public class GetTreeData : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        string query = "";
        if (context.Session["Query"] != null && context.Session["Query"].ToString() != "")
        {
            string queryParam = context.Session["Query"].ToString();

            if (queryParam == "Example")
            {
                query = "[NodeID] like 'Example%'";
            }
            else if (queryParam == "Reference")
            {
                query = "[NodeID] like 'Reference%'";
            }
            else if (queryParam == "Guide")
            {
                query = "[NodeID] like 'Guide%'";
            }
            else if (queryParam == "question")
            {
                query = "[NodeID] like 'Question%'";
            }
        }
        string strSql = "";
        //配置树节点
        if (context.Request["param"] != null && context.Request["param"] == "T_Sys_CommonTree")
        {
            query = " and VisibleType = '1' ";
            if (context.Request["pId"] != null)
            {
                query +=" and NodeParentID like '" + context.Request["pId"].ToString() + "%' ";
            }
            if (context.Request["query"] != null)
            {
                query += " and ((VisibleQuery = '1' and NodeName like '%" +
                    context.Request["query"].ToString() + "%') or (VisibleQuery !='1')) ";
            }
            strSql = @" select NodeID as id,NodeName as name,NodeParentID as pId,'false' as [nocheck] ,VisibleOpen as [open],SYSBack1 as [code],SYSBack2 as [direction] 
                        from T_Sys_CommonTree 
                        where 1=1 " + query + "  order by OrderID ";
        }
      

        SQLHelper sqlH = new SQLHelper();

        System.Data.DataSet ds = sqlH.ExecuteSQLDataSet(strSql);
        System.Data.DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            if (ds.Tables.Count == 2 && ds.Tables[1].Rows.Count > 0)
            {
                string modelId = ds.Tables[1].Rows[0][0].ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string rId = dt.Rows[i]["code"].ToString() + "|";
                    if (modelId.IndexOf(rId) != -1)
                    {
                        dt.Rows[i]["checked"] = "true";
                    }
                }
            }
            context.Response.ContentType = "text/plain";
            string json = DataToJson.DataTable2Json(dt);
            context.Response.Write(json);
        }
        else
        {
            context.Response.Write("");
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}