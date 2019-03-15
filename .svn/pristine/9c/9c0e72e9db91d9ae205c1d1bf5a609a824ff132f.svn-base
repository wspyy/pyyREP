<%@ WebHandler Language="C#" Class="GetTreeData" %>

using System;
using System.Web;
using System.SingleTable;
using System.SingleTable.PublicMethod;
using System.Web.SessionState;

public class GetTreeData : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        string query = "";
        if (context.Session["Query"] != null && context.Session["Query"].ToString() != "")
        {
            query = context.Session["Query"].ToString();
        }

        string strSql = "";
        if (context.Request["param"] != null && context.Request["param"] == "CommonTree")
        {
            strSql = " select NodeID as id,NodeName as name,NodeParentID as pId,'true' as [nocheck] ,'true' as [open] from T_Sys_CommonTree ";

            if (context.Request["treeCode"] != null)
            {
                strSql += " where  VisibleType = 1 and  NodeParentID like '" + context.Request["treeCode"] + "%'";
            }
            strSql += " order by OrderID";
        }
        else if (context.Request["param"] != null && context.Request["param"] == "SingleTableList")
        {
            //不显示T_Sys开头的表
            query = " and t.[name] not like 'T_Sys%' ";
            if (context.Request["query"] != null)
            {
                query += " and t.[name] like '%" + context.Request["query"].ToString() + "%'";
            }

            strSql = "";
            //架构清单
            strSql += " select s.[name] as id,'0' as pId,s.[name] as name ,'true' as [nocheck] ,'true' as [open] from sys.tables as t,sys.schemas as s where t.schema_id = s.schema_id  group by s.[name] ";
            strSql += " union ";
            //表清单
            strSql += " select convert(varchar,t.[name]) as id, s.[name] as pId,t.[name] as name ,'true' as [nocheck] , 'false' as [open] from sys.tables as t,sys.schemas as s where t.schema_id = s.schema_id   " + query;
            strSql += " union ";
            //列清单
            strSql += " select convert(varchar,t.[name])+'.'+c.[name] as id,convert(varchar,t.[name]) as pId,c.[name],'true' as [nocheck],'false' as [open]  from sys.columns as c inner join sys.tables as t on c.[object_id] = t.[object_id] " + query;

            strSql += " order by name ";
        }
        else if (context.Request["param"] != null && context.Request["param"] == "tableList")
        {
            strSql = " select CONFIG_ID as id,'T' as pId,DT_NAME+'-'+DT_NAME_CN as name,'true' as [nocheck],'true' as [open]   from T_Sys_ConfigTable where 1=1 and IS_QUERY=1 " + query;
            strSql += " union select 'T' as id,'0' as pId,'数据表' as name,'true' as [nocheck] ,'true' as [open] from T_Sys_ConfigTable ";
            strSql += " order by DT_NAME+'-'+DT_NAME_CN ";
        }
        else if (context.Request["param"] != null && context.Request["param"] == "1007")
        {
            strSql = " select RoleID as id,'R' as pId,RoleName as name,'true' as [nocheck],'true' as [open]   from T_PC_Role where 1=1 " + query;
            strSql += " union select 'R' as id,'0' as pId,'角色列表' as name,'true' as [nocheck] ,'true' as [open] from T_PC_Role ";
            strSql += " order by RoleID ";
        }
        else if (context.Request["param"] != null && context.Request["param"] == "2004")
        {
            strSql = " select DepartmentID as id,'D' as pId,DepartmentName as name,'true' as [nocheck],'true' as [open]   from T_PC_Department where 1=1 " + query;
            strSql += " union select 'D' as id,'0' as pId,'部门列表' as name,'true' as [nocheck] ,'true' as [open] from T_PC_Department ";
            strSql += " order by DepartmentID ";
        }
        else if (context.Request["param"] != null && context.Request["param"] == "1005")
        {
            strSql = " select UserID as id,'U' as pId,U_RealName+'-'+U_LoginName as name,'true' as [nocheck],'true' as [open]   from T_PC_User where 1=1 " + query;
            strSql += " union select 'U' as id,'0' as pId,'用户列表' as name,'true' as [nocheck] ,'true' as [open] from T_PC_User ";
            strSql += " order by UserID ";
        }
        else if (context.Request["param"] != null && context.Request["param"] == "module")
        {
            strSql = "select ModuleID as id,ModuleParentID as pId,ModuleName as name,'false' as checked,'true' as [open]  from T_Sys_Model";
            strSql += " order by orderid ";

            if (context.Request["ID"] != null)
            {
                if (context.Request["config_Id"] == "1007")
                {
                    strSql += " select ModelID from T_PC_Role_Model where RoleID='" + context.Request["ID"].ToString() + "' ";
                }
                else if (context.Request["config_Id"] == "2004")
                {
                    strSql += " select ModelID from T_PC_Department_Model where DepartmentID='" + context.Request["ID"].ToString() + "' ";
                }
                else if (context.Request["config_Id"] == "1005")
                {
                    strSql += " select ModelID from T_PC_User_Model where UserID='" + context.Request["ID"].ToString() + "' ";
                }
            }
        }

        SQLHelper sqlH = new SQLHelper();

        System.Data.DataSet ds = sqlH.ExecuteSQLDataSet(strSql);
        System.Data.DataTable dt = ds.Tables[0];
        if (ds.Tables.Count == 2 && ds.Tables[1].Rows.Count > 0)
        {
            string modelId = ds.Tables[1].Rows[0][0].ToString();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string rId = dt.Rows[i]["id"].ToString() + "|";
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}