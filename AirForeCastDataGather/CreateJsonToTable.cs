using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
///CreateJsonToTable 的摘要说明
/// </summary>
public class CreateJsonToTable
{
	public CreateJsonToTable()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public string JsonToTable(string tableName, string jsonString)
    {
        List<string> columnList = new List<string>();
        string[] rows = jsonString.Split('}');
        if (rows.Length > 0)
        {          
            string[] columns=rows[0].Substring(2,rows[0].Length-2).Split(',');
            foreach (string column in columns)
            {
                string[] keyValues = column.Split(':');
                columnList.Add(keyValues[0].Substring(1,keyValues[0].Length-2));//去掉引号
            }
        }
        //string checkAndCreatTable = "IF EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'["+tableName+"]') AND OBJECTPROPERTY(ID, 'IsTable') = 1) PRINT '存在' ELSE ";
        string TableSQL ="IF EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'["+tableName+"]') AND OBJECTPROPERTY(ID, 'IsTable') = 1) PRINT '存在' ELSE "
            + " create table " + tableName + "(";//判读是否有表
        for (int i = 0; i < columnList.Count; i++)
        {
            TableSQL += columnList[i] + " nvarchar(100) null ";
            if (i != columnList.Count - 1)
            {
                TableSQL += ",";
            }
        }
        TableSQL += ",Createdate datetime DEFAULT getdate()) ";
       
        return TableSQL;
    }

    public void InsertDataToTable(string url,string tableName, string jsonString)
    {
       
        string[] rows = jsonString.Split('}');
        if (rows.Length > 0)
        {
            for (int i = 0; i < rows.Length-1; i++)
            {
                List<string> fieldList = new List<string>();
                List<string> valueList = new List<string>();
                string row = rows[i];
                if (i == 0)
                {
                    string[] columns = rows[i].Substring(2, rows[i].Length - 2).Split(',');
                    foreach (string column in columns)
                    {
                        string[] keyValues = column.Split(':');
                        fieldList.Add(keyValues[0].Substring(1, keyValues[0].Length - 2));//去掉引号
                        valueList.Add(keyValues[1].Substring(1, keyValues[1].Length - 2));//获得value值
                    }
                }
                else
                {
                    string[] columns = rows[i].Substring(2, rows[i].Length - 2).Split(',');
                    foreach (string column in columns)
                    {
                        string[] keyValues = column.Split(':');
                        fieldList.Add(keyValues[0].Substring(1, keyValues[0].Length - 2));//去掉引号
                        valueList.Add(keyValues[1].Substring(1, keyValues[1].Length - 2));//获得value值
                    }
                }
                //写插入语句
                string insertDataSQL = " insert into " + tableName + " (";
                for (int j = 0; j < fieldList.Count; j++)
                {
                    insertDataSQL += fieldList[j] + ",";
                }
                insertDataSQL = insertDataSQL.Substring(0, insertDataSQL.Length - 1) + ") values ('";//去掉最后一个“，”号
                for (int k = 0; k < valueList.Count; k++)
                {
                    insertDataSQL += valueList[k] + "','";
                }
                insertDataSQL = insertDataSQL.Substring(0, insertDataSQL.Length - 2) + ")";
                SqlHelper.ExecuteNonQuery(url, System.Data.CommandType.Text, insertDataSQL);
            }
        }
        
        //return insertDataSQL;
    }

    private void CheckExistTable(string tableName)
    {
        //Boolean
    }

    /// <summary>   
    /// 根据Json返回DateTable,JSON数据格式如:   
    /// {table:[{column1:1,column2:2,column3:3},{column1:1,column2:2,column3:3}]}   
    /// </summary>   
    /// <param name="strJson">Json字符串</param>   
    /// <returns></returns>   
    public static DataTable JsonToDataTable(string strJson)
    {
        //取出表名   
        var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
        string strName = rg.Match(strJson).Value;
        DataTable tb = null;
        //去除表名   
        strJson = strJson.Substring(strJson.IndexOf("[") + 1);
        strJson = strJson.Substring(0, strJson.IndexOf("]"));

        //获取数据   
        rg = new Regex(@"(?<={)[^}]+(?=})");
        MatchCollection mc = rg.Matches(strJson);
        for (int i = 0; i < mc.Count; i++)
        {
            string strRow = mc[i].Value;
            string[] strRows = strRow.Split(',');

            //创建表   
            if (tb == null)
            {
                tb = new DataTable();
                tb.TableName = strName;
                foreach (string str in strRows)
                {
                    var dc = new DataColumn();
                    string[] strCell = str.Split(':');
                    dc.ColumnName = strCell[0];
                    tb.Columns.Add(dc);
                }
                tb.AcceptChanges();
            }

            //增加内容   
            DataRow dr = tb.NewRow();
            for (int r = 0; r < strRows.Length; r++)
            {
                dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
            }
            tb.Rows.Add(dr);
            tb.AcceptChanges();
        }

        return tb;
    }
}