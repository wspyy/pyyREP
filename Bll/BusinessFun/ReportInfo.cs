﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using Model;
namespace Bll.BusinessFun
{
   public class ReportInfo
    {
       /// <summary>
       /// 获取预报数据
       /// </summary>   
       /// <returns></returns>
       public List<ReportModel> GetReportData(int pageSize, int pageIndex,string sqlWhere)
       {          
           SQLHelper sqlh = new SQLHelper();
           string sql = @"select * from (
select  r.*, rd.AQI,rd.QualityLevel,rd. Firstp,rd.FutureDate,
 (row_number() over(order by r.ReportTime desc)) /@pageSize as pagenum  
 from (
 select  r.ReportTime,r.JCSubmit,r.ReportCode,r.YJColor
FROM [dbo].[T_Mid_Report] r" +
" where  JCSubmit =1"+sqlWhere+
")  as r "+
 "join [dbo].[T_Mid_ReportData] as rd on r.[ReportCode]=rd.[ReportCode])t where pagenum =@pageIndex - 1";
           SqlParameter[] ps = { 
           new SqlParameter("@pageSize",pageSize),
           new SqlParameter("@pageIndex",pageIndex)};
           List<ReportModel> list = new List<ReportModel>();
           DataTable dt= sqlh.GetDataTable(sql, CommandType.Text, ps);
           if (dt.Rows.Count > 0)
           {
               foreach (DataRow row in dt.Rows)
               {
                   list.Add(LoadReport(row));
               }
           }
           return list;
       }
       /// <summary>
       /// 返回总的页数
       /// </summary>
       /// <param name="pageSize"></param>
       /// <returns></returns>
       public int GetCount(int pageSize,string sqlWhere)
       {
           int count = 0;
           SQLHelper sqlh = new SQLHelper();
           string sql = @"select count(*) from (select  r.*, rd.AQI,rd.QualityLevel,rd. Firstp,rd.FutureDate
 from (
 select  r.ReportTime,r.JCSubmit,r.ReportCode,r.YJColor
FROM [dbo].[T_Mid_Report] r
 where  JCSubmit =1 "+sqlWhere+
")  as r  join [dbo].[T_Mid_ReportData] as rd on r.[ReportCode]=rd.[ReportCode])t";
           if (sqlh.ExecuteSQLScalar(sql) != null)
           {
               count = Convert.ToInt32( sqlh.ExecuteSQLScalar(sql));
           }
           return Convert.ToInt32(Math.Ceiling((double)count/ pageSize));            
       }
       /// <summary>
       /// 页码条
       /// </summary>
       /// <param name="pageIndex"></param>
       /// <param name="pageCount"></param>
       /// <returns></returns>
       public static string GetPageBar(int pageIndex, int pageCount)
       {
           if (pageCount == 1)
           {
               return string.Empty;
           }
           StringBuilder sb = new StringBuilder();
           if (pageCount > 10)
           {
               int start = pageIndex - 5;//计算起始位置.要求页面上显示10个数字页码.
               if (start < 1)
               {
                   start = 1;
               }
               int end = start + 9;//计算终止位置.
               if (end > pageCount)
               {
                   end = pageCount;
                   //重新计算一下Start值.
                   start = end - 9 < 1 ? 1 : end - 9;
               }

               if (pageIndex > 1)
               {
                   sb.AppendFormat("<a href='?pageIndex={0}' class='myPageBar'>首页</a><a href='?pageIndex={1}' class='myPageBar'>上一页</a>",1, pageIndex - 1);
               }
               for (int i = start; i <= end; i++)
               {
                   if (i == pageIndex)
                   {
                       sb.Append(i);
                   }
                   else
                   {
                       sb.AppendFormat("<a href='?pageIndex={0}' class='myPageBar'>{0}</a>", i);
                   }
               }
               if (pageIndex < pageCount)
               {
                   sb.AppendFormat("<a href='?pageIndex={0}' class='myPageBar'>下一页</a><a href='?pageIndex={1}' class='myPageBar'>尾页</a>", pageIndex + 1,pageCount);
               }

               return sb.ToString();
           }
           else
           {
               for (int i = 1; i <= pageCount; i++)
               {
                   if (i == pageIndex)
                   {
                       sb.Append(i);
                   }
                   else
                   {
                       sb.AppendFormat("<a href='?pageIndex={0}' class='myPageBar'>{0}</a>", i);
                   }
               }
               return sb.ToString();
           }

       }
       private ReportModel LoadReport(DataRow row)
       {
           DateTime d = new DateTime();
          
           ReportModel r = new ReportModel()
           {
               ReportTime = row["ReportTime"]!= DBNull.Value ?Convert.ToDateTime(row["ReportTime"]).ToShortDateString():"--",
               FutureDate = row["FutureDate"] != DBNull.Value ? Convert.ToDateTime(row["FutureDate"]).ToShortDateString() : "--",
               AQI = row["AQI"]!=DBNull.Value?Convert.ToInt32(row["AQI"]):-99,
               QualityLevel = row["QualityLevel"] != DBNull.Value ? Convert.ToInt32(row["QualityLevel"]) : -99,
               Firstp = row["Firstp"] != DBNull.Value ? row["Firstp"].ToString():"--",
               YJColor = row["YJColor"] != DBNull.Value ? row["YJColor"].ToString() : "--",
               ReportCode = row["ReportCode"] != DBNull.Value ? row["ReportCode"].ToString() : "--",
           };
           return r;
       }
    }
   
}