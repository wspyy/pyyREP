﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;


namespace Bll.BusinessFun
{
    public class ConsultReportDal
    {
        public DataTable GetConsultReportData(int code)
        {
            SqlParameter[] parameters ={
                                         new SqlParameter("@ReportCode",code)
                                      };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Proc_ConcirCulation", parameters);
            if (ds != null && ds.Tables.Count == 1)
            {
                return ds.Tables[0];
            }
            return null;
        }

        public DataTable GetFirstPolutionData(string strDate)
        {
            SqlParameter[] parameters ={
                                          new SqlParameter("@MoniDate",strDate)
                                     };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Proc_FirstPolution", parameters);
            if (ds != null && ds.Tables.Count == 1)
            {
                return ds.Tables[0];
            }
            return null;
        }
        public DataTable GetDataUpload(string reportTime, string foremodel)
        {
            SqlParameter[] parameters ={
                                          new SqlParameter("@select_date",reportTime),
                                          new SqlParameter("@model",foremodel)
                                     };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Proc_dataUpload", parameters);
            if (ds != null && ds.Tables.Count == 1)
            {
                return ds.Tables[0];
            }
            return null;
        }
        public static DataSet Paging(string strSql, int page, int rows, ref int recordCount, ref int pageCount)
        {
            SqlParameter outRecordCount = new SqlParameter();
            outRecordCount.ParameterName = "@recordcount";
            outRecordCount.Direction = ParameterDirection.InputOutput;
            outRecordCount.Value = recordCount;
            SqlParameter outPageCount = new SqlParameter();
            outPageCount.ParameterName = "@pagecount";
            outPageCount.Direction = ParameterDirection.InputOutput;
            outPageCount.Value = pageCount;
            SqlParameter[] para = { 
                                  new SqlParameter("@sql",strSql),
                                  new SqlParameter("@currentpage",page),
                                  new SqlParameter("@pagesize",rows),
                                  outRecordCount,
                                  outPageCount
                                  };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "p_splitpage", para);
            if (ds.Tables[2].Rows.Count > 0)
            {
                recordCount = Convert.ToInt32(outRecordCount.Value);
                pageCount = Convert.ToInt32(outPageCount.Value);
                return ds;
            }
            else
            {
                return null;
            }
        }
        public static DataTable GettxtddlData(string txtdata)
        {
            double doudata = Convert.ToDouble(txtdata.Trim());
            SqlParameter[] para = { 
                                  new SqlParameter("@txtdata",txtdata),
                                 
                                
                                  };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Proc_GetConCirddlSelectData", para);

            if (ds != null && ds.Tables.Count == 1)
            {
                return ds.Tables[0];
            }
            return null;
        }

        public void SetPoll(string poll, string op)
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = " select top 1 ReportCode from [dbo].[T_Mid_Report] order by ReportTime desc,ReportCode desc";
            DataSet ds = sqlh.ExecuteSQLDataSet(sql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string reportcode = ds.Tables[0].Rows[0]["ReportCode"].ToString();
                string pollsql = "SELECT *  FROM [dbo].[T_PollConclusions_Report] where [ReportCode]=" + reportcode;
                DataSet ds1 = sqlh.ExecuteSQLDataSet(pollsql);
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {
                    sql = "UPDATE [dbo].[T_PollConclusions_Report] set [Conclusions]='" + poll + "',[Opinion] = '" + op + "' ,[ReportDate] = '" + DateTime.Now + "' where [ReportCode]=" + reportcode;
                }
                else
                {
                    sql = "INSERT INTO [dbo].[T_PollConclusions_Report]([ReportCode],[Conclusions],[Opinion],[Remark],[ReportDate])VALUES(" + reportcode + ",'" + poll + "','" + op + "','','" + DateTime.Now + "')";
                }
                sqlh.ExecuteSQLNonQuery(sql);
            }
        }

        public void SetPoll(string poll, string op, string ReportCode)
        {
            SQLHelper sqlh = new SQLHelper();
                string pollsql = "SELECT *  FROM [dbo].[T_PollConclusions_Report] where [ReportCode]=" + ReportCode;
                DataSet ds1 = sqlh.ExecuteSQLDataSet(pollsql);
            string sql="";
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {
                    sql = "UPDATE [dbo].[T_PollConclusions_Report] set [Conclusions]='" + poll + "',[Opinion] = '" + op + "' ,[ReportDate] = '" + DateTime.Now + "' where [ReportCode]=" + ReportCode;
                }
                else
                {
                    sql = "INSERT INTO [dbo].[T_PollConclusions_Report]([ReportCode],[Conclusions],[Opinion],[Remark],[ReportDate])VALUES(" + ReportCode + ",'" + poll + "','" + op + "','','" + DateTime.Now + "')";
                }
                sqlh.ExecuteSQLNonQuery(sql);
        }

        /// <summary>
        /// 保存签名
        /// </summary>
        /// <param name="poll"></param>
        /// <param name="op"></param>
        /// <param name="ReportCode"></param>
        public void SetPoll(string poll, string op, string ReportCode, string ExpertSign, string CheckSign)
        {
            SQLHelper sqlh = new SQLHelper();
            string pollsql = "SELECT *  FROM [dbo].[T_PollConclusions_Report] where [ReportCode]=" + ReportCode;
            DataSet ds1 = sqlh.ExecuteSQLDataSet(pollsql);
            string sql = "";
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {
                sql = "UPDATE [dbo].[T_PollConclusions_Report] set [Conclusions]='" + poll + "',[Opinion] = '" + op + "',[ExpertSign] = '" + ExpertSign + "',[CheckSign] = '" + CheckSign + "' ,[ReportDate] = '" + DateTime.Now + "' where [ReportCode]=" + ReportCode;
            }
            else
            {
                sql = "INSERT INTO [dbo].[T_PollConclusions_Report]([ReportCode],[Conclusions],[Opinion],[Remark],[ReportDate])VALUES(" + ReportCode + ",'" + poll + "','" + op + "','','" + DateTime.Now + "')";
            }
            sqlh.ExecuteSQLNonQuery(sql);
        }

        public DataTable getpoll()
        {
            SQLHelper sqlh = new SQLHelper();
            StringBuilder strb = new StringBuilder();

            strb.Append(" select top 1 ReportCode,'气象条件：\r\n'+cast(qxtext as varchar(8000))+'\r\n预测结果：\r\n'+(select cast(FutureDate as varchar(11))+'空气质量指数'+cast(AQI as varchar(10))+'空气质量级别为：'+QualityIndex+'出行建议：'+Proposal from(");
            strb.Append("select * from [dbo].[T_Mid_ReportData] where ReportCode=(");
            strb.Append(" select top 1 ReportCode from [dbo].[T_Mid_Report]  order by ReportTime desc,ReportCode desc");
            strb.Append(") ) t1 left join (");
            strb.Append("select QualityLevel,(QualityIndex+'  '+Category) as QualityIndex,Proposal from T_Bas_QualityStandard");
            strb.Append(")t2  on t1.QualityLevel=t2.QualityLevel for xml path('')) as Conclusions,'' as Opinion ");
            strb.Append("from [dbo].[T_Mid_Report] order by ReportTime desc,ReportCode desc");
            DataSet ds = sqlh.ExecuteSQLDataSet(strb.ToString());
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string reportcode = ds.Tables[0].Rows[0]["ReportCode"].ToString();
                string pollsql = "SELECT *  FROM [dbo].[T_PollConclusions_Report] where [ReportCode]=" + reportcode;
                DataSet ds1 = sqlh.ExecuteSQLDataSet(pollsql);
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {
                    return ds1.Tables[0];
                }
                return ds.Tables[0];
            }
            return null;
        }

        public DataTable getpoll(string ReportCode)
        {
            SQLHelper sqlh = new SQLHelper();
            StringBuilder strb = new StringBuilder();
            strb.Append("select d.*,U_RealName,ReportTime from [dbo].[T_PollConclusions_Report] d right join [dbo].[T_Mid_Report] p on d.ReportCode = p.ReportCode left join [dbo].[T_PC_User] u on p.JCUserID = u.UserID where p.reportcode=" + ReportCode);
            DataSet ds = sqlh.ExecuteSQLDataSet(strb.ToString());
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                //strb.Clear();
                //strb.Append(" select '' ExpertSign,'' CheckSign,ReportCode,u.U_RealName,d.ReportTime,'气象条件：\r\n'+cast(qxtext as varchar(8000))+'\r\n预测结果：\r\n'+(select cast(FutureDate as varchar(11))+'空气质量指数'+cast(AQI as varchar(10))+'空气质量级别为：'+QualityIndex+'出行建议：'+Proposal from(");
                ////getdate() ReportDate
                //strb.Append("select * from [dbo].[T_Mid_ReportData] where ReportCode=" + ReportCode + " ) t1 left join (");
                //strb.Append("select QualityLevel,(QualityIndex+'  '+Category) as QualityIndex,Proposal from T_Bas_QualityStandard");
                //strb.Append(")t2  on t1.QualityLevel=t2.QualityLevel for xml path('')) as Conclusions,'' as Opinion ");
                //strb.Append("from [dbo].[T_Mid_Report] d left join [dbo].[T_PC_User] u on d.JCUserID = u.UserID where ReportCode=" + ReportCode);
                //ds = sqlh.ExecuteSQLDataSet(strb.ToString());
                //if (ds != null && ds.Tables[0].Rows.Count > 0)
                //{
                //    return ds.Tables[0];
                //}
                //else
                //{
                //    return null;
                //}
                return null;
            }
            //return null;
        }

        public DataTable GetPollByCode(string code) {
            SQLHelper sqlh = new SQLHelper();
            string pollsql = "SELECT *  FROM [dbo].[T_PollConclusions_Report] where [ReportCode]=" + code;
            DataSet ds1 = sqlh.ExecuteSQLDataSet(pollsql);
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {
                return ds1.Tables[0];
            }
            else {
                return null;
            }
        }


        public void SetPollPublish(string poll, string op, string level)
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = " select top 1 ReportCode from [dbo].[T_Mid_Report] order by ReportTime desc,ReportCode desc";
            DataSet ds = sqlh.ExecuteSQLDataSet(sql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string reportcode = ds.Tables[0].Rows[0]["ReportCode"].ToString();
                string pollsql = "SELECT *  FROM [dbo].[T_pollPublish_Report] where [ReportCode]=" + reportcode;
                DataSet ds1 = sqlh.ExecuteSQLDataSet(pollsql);
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {
                    sql = "UPDATE [dbo].[T_pollPublish_Report] set [Basiscontent]='" + poll + "',forecastlevel='" + level + "',[maincontent] = '" + op + "' ,[ReportDate] = '" + DateTime.Now + "' where [ReportCode]=" + reportcode;
                }
                else
                {
                    sql = "INSERT INTO [dbo].[T_pollPublish_Report]([ReportCode],[Basiscontent],[maincontent],forecastlevel,[Remark],[ReportDate])VALUES(" + reportcode + ",'" + poll + "','" + op + "','" + level + "','','" + DateTime.Now + "')";
                }
                sqlh.ExecuteSQLNonQuery(sql);
            }
        }

        public void SetPollPublish(string poll, string op, string level, string ReportCode)
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = "";
                string pollsql = "SELECT *  FROM [dbo].[T_pollPublish_Report] where [ReportCode]=" + ReportCode;
                DataSet ds1 = sqlh.ExecuteSQLDataSet(pollsql);
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {
                    sql = "UPDATE [dbo].[T_pollPublish_Report] set [Basiscontent]='" + poll + "',forecastlevel='" + level + "',[maincontent] = '" + op + "' ,[ReportDate] = '" + DateTime.Now + "' where [ReportCode]=" + ReportCode;
                }
                else
                {
                    sql = "INSERT INTO [dbo].[T_pollPublish_Report]([ReportCode],[Basiscontent],[maincontent],forecastlevel,[Remark],[ReportDate])VALUES(" + ReportCode + ",'" + poll + "','" + op + "','" + level + "','','" + DateTime.Now + "')";
                }
                sqlh.ExecuteSQLNonQuery(sql);
        }


        public DataTable getpollPublish(string ReportCode)
        {
            SQLHelper sqlh = new SQLHelper();
            StringBuilder strb = new StringBuilder();
            strb.Append("select p.*,r.ReportTime,r.YJColor,r.YJDJ from T_pollPublish_Report p right join T_Mid_Report r on p.ReportCode=r.ReportCode where r.ReportCode=" + ReportCode);
            DataSet ds = sqlh.ExecuteSQLDataSet(strb.ToString());
            if (ds != null && ds.Tables.Count > 0) { return ds.Tables[0]; }
            else {
                return null;
            }
            
            /*else
            {
                strb.Clear();
                strb.Append("select top 1 ReportCode,'气象条件：\r\n'+cast(qxtext as varchar(8000))+'\r\n预测结果：\r\n'+");
                strb.Append("(select cast(FutureDate as varchar(11))+'空气质量指数'+cast(AQI as varchar(10))+'空气质量级别为：'+QualityIndex+'出行建议：'+Proposal from(");
                strb.Append("select * from [dbo].[T_Mid_ReportData] where ReportCode=(");
                strb.Append(" select top 1 ReportCode from [dbo].[T_Mid_Report]  order by ReportTime desc,ReportCode desc");
                strb.Append(") ) t1 left join (");
                strb.Append("select QualityLevel,(QualityIndex+'  '+Category) as QualityIndex,Proposal from T_Bas_QualityStandard");
                strb.Append(")t2  on t1.QualityLevel=t2.QualityLevel for xml path(''))");
                strb.Append("  Basiscontent,'' maincontent,YJDJ,YJColor   from [dbo].[T_Mid_Report] order by ReportTime desc,reportcode desc ");                
                ds = sqlh.ExecuteSQLDataSet(strb.ToString());
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }*/
        }

        public DataTable getNewpollPublish(string ReportCode, bool IsNew)
        {
            SQLHelper sqlh = new SQLHelper();
            StringBuilder strb = new StringBuilder();
            strb.Append("select top 1 ReportCode,'气象条件：\r\n'+cast(qxtext as varchar(8000))+'\r\n预测结果：\r\n'+");

            //  string sql = "
            strb.Append("(select cast(FutureDate as varchar(11))+'空气质量指数'+cast(AQI as varchar(10))+'空气质量级别为：'+QualityIndex+'出行建议：'+Proposal from(");
            strb.Append("select * from [dbo].[T_Mid_ReportData] where ReportCode=(");
            strb.Append(" select top 1 ReportCode from [dbo].[T_Mid_Report]  order by ReportTime desc,ReportCode desc");
            strb.Append(") ) t1 left join (");
            strb.Append("select QualityLevel,(QualityIndex+'  '+Category) as QualityIndex,Proposal from T_Bas_QualityStandard");
            strb.Append(")t2  on t1.QualityLevel=t2.QualityLevel for xml path(''))");
            strb.Append("  Basiscontent,'' maincontent,YJDJ   from [dbo].[T_Mid_Report] order by ReportTime desc,reportcode desc");
            DataSet ds = sqlh.ExecuteSQLDataSet(strb.ToString());
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string reportcode = ds.Tables[0].Rows[0]["ReportCode"].ToString();
                string pollsql = "SELECT *  FROM [dbo].[T_pollPublish_Report] where [ReportCode]=" + reportcode;
                DataSet ds1 = sqlh.ExecuteSQLDataSet(pollsql);
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {
                    return ds1.Tables[0];
                }
                return ds.Tables[0];
            }
            return null;
        }

        public DataTable getpollPublishBycode(string code)
        {
            SQLHelper sqlh = new SQLHelper();
            string pollsql = "SELECT *  FROM [dbo].[T_pollPublish_Report] where [ReportCode]=" + code;
            DataSet ds1 = sqlh.ExecuteSQLDataSet(pollsql);
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {
                return ds1.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 返回日报列表
        /// </summary>
        /// <param name="ReportDate"></param>
        /// <returns></returns>
        public DataTable GetReportTitle(string ReportDate) 
        {
            string sql = "SELECT  convert(varchar,[ReportTime],23)+'('+ cast(row_number() over(ORDER BY ReportCode) as varchar(3))+')',ReportCode   FROM [dbo].[T_Mid_Report]  where ReportTime='" + ReportDate + "' and JCSubmit = 1";
            SQLHelper sqlh = new SQLHelper();
            DataSet ds=sqlh.ExecuteSQLDataSet(sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 设置发布模式
        /// </summary>
        /// <param name="ReportDate"></param>
        /// <returns></returns>
        public bool setPublishModel(string foremodel)
        {
            string sql = "update T_Cod_ModelType set IsPublish = 0";
            SQLHelper sqlh = new SQLHelper();
            DataSet ds = sqlh.ExecuteSQLDataSet(sql);
            sql = "update T_Cod_ModelType set IsPublish = 1 where ModelTypeCode = " + foremodel;
            int r = sqlh.ExecuteSQLNonQuery(sql);
            if (r > 0) return true;
            else return false;
        }
    }
}
