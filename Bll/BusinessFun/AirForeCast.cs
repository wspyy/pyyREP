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
    public class AirForeCast
    {
        SQLHelper sqlh = new SQLHelper();

        /// <summary>
        /// 获取空气质量预测数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>返回最新三天预测结果</returns>
        /// 
        public DataSet GetAirForeCastNew(string date)
        {


            //            string sql = @"SELECT * FROM (
            //                SELECT  * , ROW_NUMBER() OVER ( PARTITION BY StationCode ORDER BY MonitorTime DESC ) AS rowno
            //                FROM    V_Mid_AirForeCastNew) t " + sqlwhere;
            string sql = "SELECT *  FROM  V_Mid_AirForeCastNew WHERE StationType>-1 and CONVERT(VARCHAR(100), MonitorTime, 23) = '" + date + "' and ForecastModel = 0 order by OrderID";
            DataSet data = sqlh.ExecuteSQLDataSet(sql);
            return data;

        }

        /// <summary>
        /// 获取空气质量数值预报
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>返回最新三天预测结果</returns>
        /// 
        public DataSet GetAirForeCastNumNew(string date, int datatype)
        {
            string sql = "SELECT *  FROM  V_Mid_AirForeCastNew WHERE StationType>-1 and MonitorTime = '" + date + "' and ForecastModel = " + datatype + " order by OrderID";
            DataSet data = sqlh.ExecuteSQLDataSet(sql);
            return data;
        }

        /// <summary>
        /// 获取空气质量预测数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>返回最新三天预测结果</returns>
        public DataSet GetAirForeCastByStation(string StationCode)
        {

            string sql = @"select * from V_Mid_AirForeCastNew
                           where StationCode = '" + StationCode + "'order by [MonitorTime] asc";
            return sqlh.ExecuteSQLDataSet(sql);
        }

        /// <summary>
        /// 获取空气质量预测数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetAirForeCastAll()
        {

            string sql = @"select * from V_Mid_AirForeCastNew order by [MonitorTime] asc";
            return sqlh.ExecuteSQLDataSet(sql);
        }


        /// <summary>
        /// 获取空气质量预测数据
        /// </summary>
        /// <param name="date">小时数据</param>
        /// <returns>返回预测结果</returns>
        public DataSet GetAirContrastData(string StationCode, string PollItem, DateTime StrTime, DateTime EndTime, string DataType)
        {
            string col = "AQI";
            if (PollItem == "PM2.5") PollItem = "PM25";
            if (PollItem != "AQI") col = PollItem + col;

            //string sql = @"select t.StationCode,t.StationName,t.MonitorTime,s." + col + @" SAQI,t." + col + @" TAQI,C." + col + @" CAQI,W." + col + @" WAQI";
            //     sql+=" from [dbo].[V_Mid_AirForeCast] t";
            //                sql+= " left join [dbo].[T_Mid_AirHourData] s on t.StationCode = s.StationCode and t.MonitorTime=s.MoniDate and t.ForecastModel = 0";
            //                 sql+=" left join [dbo].[T_Mid_AirForeCast] c on c.StationCode = s.StationCode and c.MonitorTime=s.MoniDate and t.ForecastModel = 1";
            //                 sql += " left join [dbo].[T_Mid_AirForeCast] w on w.StationCode = s.StationCode and w.MonitorTime=s.MoniDate and t.ForecastModel = 2";
            //                 sql+=" where t.StationCode = '" + StationCode + "' and t.MonitorTime >= '" + StrTime.ToString("yyyy-MM-dd HH:mm:ss") + "'and t.MonitorTime <='" + EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' order by [MonitorTime] asc";
            //return sqlh.ExecuteSQLDataSet(sql);

            SqlParameter[] para = { 
                                          new SqlParameter("@StationCode",StationCode),
                                          new SqlParameter("@pollItem",PollItem),
                                          new SqlParameter("@StartTime",StrTime),
                                          new SqlParameter("@EndTime",EndTime),
                                          new SqlParameter("@DataType",DataType)
                              };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Proc_AirForCastContrast", para);
            return ds;
        }

        /// <summary>
        /// 获取空气质量预测统计数据
        /// </summary>
        /// <param name="date">小时数据</param>
        /// <returns>返回预测结果</returns>
        public DataSet GetAirContrastStatData(DateTime StrTime, DateTime EndTime)
        {
            SqlParameter[] para = { 
                                          new SqlParameter("@StartTime",StrTime.ToString("yyyy-MM-dd")),
                                          new SqlParameter("@EndTime",EndTime.ToString("yyyy-MM-dd"))
                              };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Proc_AirForeCastStat", para);
            return ds;
        }
        /// <summary>
        /// 获取空气质量预测统计数据
        /// </summary>
        /// <param name="date">小时数据</param>
        /// <returns>返回预测结果</returns>
        public DataSet GetAirForeCastByDate(string StrTime)
        {
            SqlParameter[] para = { 
                                          new SqlParameter("@MonitorTime",StrTime)
                              };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Proc_AirForeCastDay", para);
            return ds;
        }
        /// <summary>
        /// 获取天气预测数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>返回未来7天预测结果</returns>
        /// 
        public DataSet GetWeatherForeCastByDate(string date)
        {
            string sql = "select * from  [dbo].[T_DATA_WEATHERFORECAST] where convert(varchar,PublishDate,23)='" + date + "' order by ForecastDate";
            //string sql = "select * from  [dbo].[T_DATA_WEATHERFORECAST] where convert(varchar,PublishDate,23)='2015-10-26' order by ForecastDate";
            DataSet data = sqlh.ExecuteSQLDataSet(sql);
            return data;

        }        
        /// <summary>
        /// 获取天气预测数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>返回未来7天预测结果</returns>
        /// 
        public DataSet GetWeatherDetailByDate(string date, string foredate)
        {
            string sql = "select * from  [dbo].[T_DATA_WEATHERFINEFORECAST] where convert(varchar,PublishDate,23)='" + date + "'and convert(varchar,ForecastDate,23)='" + foredate + "' order by  OrderBy";
            //string sql = "select * from  [dbo].[T_DATA_WEATHERFINEFORECAST] where convert(varchar,PublishDate,23)='2015-10-26' and convert(varchar,ForecastDate,23)='" + foredate + "' order by OrderBy";
            DataSet data = sqlh.ExecuteSQLDataSet(sql);
            return data;

        }
        /// <summary>
        /// 获取空气质量预测统计数据
        /// </summary>
        /// <param name="date">小时数据</param>
        /// <returns>返回预测结果</returns>
        public DataSet GetAirForeCastDataSSCI(string begin, string end, string station, string item)
        {
            SqlParameter[] para = { 
                                          new SqlParameter("@begin",begin),
                                          new SqlParameter("@end",end),
                                          new SqlParameter("@station",station),
                                          new SqlParameter("@item",item)
                              };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Proc_AirForeCastSSCI", para);
            return ds;
        }

        /// <summary>
        /// 从[V_Mid_AirForeCastNew] \[T_Mid_AirDayDataOld]两个表中读取站点名称、AQI差值、AirLevel（NULL代表相等等，1代表不等，-1代表没有预测）
        /// </summary>
        /// <param name="date">小时数据</param>
        /// <returns>返回预测结果</returns>
        public DataSet GetACCAnalysis(string station, string model, string item, string year, string month)
        {
            SqlParameter[] para = { 
                                          
                                          new SqlParameter("@station",station),
                                          new SqlParameter("@model",model),
                                          new SqlParameter("@item",item),
                                          new SqlParameter("@year",year),
                                          new SqlParameter("@month",month)
                              };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Pro_ACCAnalysis", para);

            return ds;
        }
        /// <summary>
        /// 接GetACCAnalysis，通过Pro_pie读取AT、AF、LT、LF、N
        /// </summary>
        /// <param name="date">小时数据</param>
        /// <returns>返回预测结果</returns>
        public DataSet GetACCAnalysisPie(string station, string item ,string year)
        {
            SqlParameter[] para = { 
                                          
                                          new SqlParameter("@station",station),
                                          new SqlParameter("@item",item),
                                          new SqlParameter("@year",year)
                              };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Pro_pie", para);
            return ds;
        }
        /// <summary>
        /// 接GetSystemEvaluate，通过Pro_GetSystemEvaluate
        /// </summary>
        /// <param name="date">小时数据</param>
        /// <returns>返回预测结果</returns>
        /// GetSystemEvaluate(station, model, item, time, begintime, endtime)
        public DataSet GetSystemEvaluate(string station, string model, string item, string validTime, string begintime, string endtime)
        {
            SqlParameter[] para = { 
                                          
                                          new SqlParameter("@station",station),
                                          new SqlParameter("@model",model),
                                          new SqlParameter("@item",item),
                                          new SqlParameter("@validTime",validTime),
                                          new SqlParameter("@begintime",begintime),
                                          new SqlParameter("@endtime",endtime)
                              };
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Pro_SystemEvaluate", para);
            return ds;
        }

        /// <summary> 
        /// 获取空气质量预测统计数据
        /// </summary>
        /// <param name="date">小时数据</param>
        /// <returns>返回预测结果</returns>
        public bool SetModelWeight(int ssci, int cmaq, int wrf)
        {
            bool suc = false;
            string sql = "";
            try
            {
                if (ssci > 0)
                {
                    sql = "update [dbo].[T_Cod_ModelType] set Weights = " + ssci.ToString() + " where ModelTypeCode = 0";
                    sqlh.ExecuteSQLNonQuery(sql);
                }
                if (ssci > 0)
                {
                    sql = "update [dbo].[T_Cod_ModelType] set Weights = " + cmaq.ToString() + " where ModelTypeCode = 1";
                    sqlh.ExecuteSQLNonQuery(sql);
                }
                if (ssci > 0)
                {
                    sql = "update [dbo].[T_Cod_ModelType] set Weights = " + wrf.ToString() + " where ModelTypeCode = 2";
                    sqlh.ExecuteSQLNonQuery(sql);
                }
                suc = true;
            }
            catch (Exception)
            {
                suc = false;
                throw;
            }
            return suc;
        }
        /// <summary>
        /// 获取空气质量预测数据
        /// </summary>
        /// <param name="date">日数据</param>
        /// <returns>返回预测结果</returns>
        public DataSet GetAirDayContrastData(string StationCode, string PollItem, DateTime StrTime, DateTime EndTime)
        {
            string col = "AQI";
            if (PollItem == "PM2.5") PollItem = "PM25";
            if (PollItem != "AQI") col = PollItem + col;

            string sql = @"select t.StationCode,t.StationName,t.MonitorTime,s." + col + @" SAQI,t." + col + @" TAQI,C." + col + @" CAQI,W." + col + @" WAQI";
            sql += " from [dbo].[V_Mid_AirForeCast] t";
            sql += " left join [dbo].[T_Mid_AirDayData] s on t.StationCode = s.StationCode and t.MonitorTime=s.MoniDate and t.ForecastModel = 0";
            sql += " left join [dbo].[T_Mid_AirForeCast] c on c.StationCode = s.StationCode and c.MonitorTime=s.MoniDate and t.ForecastModel = 1";
            sql += " left join [dbo].[T_Mid_AirForeCast] w on w.StationCode = s.StationCode and w.MonitorTime=s.MoniDate and t.ForecastModel = 2";
            sql += " where t.StationCode = '" + StationCode + "' and t.MonitorTime >= '" + StrTime.ToString("yyyy-MM-dd HH:mm:ss") + "'and t.MonitorTime <='" + EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' order by [MonitorTime] asc";
            return sqlh.ExecuteSQLDataSet(sql);
        }

        /// <summary>
        /// 昨日预报结果回顾
        /// 
        /// </summary>
        /// <returns>返回预报结果预测结果</returns>
        public DataSet Get_YesDay_ForecasData()
        {

            //string sql = @"select top 1 AQI, AirLevel,Category,FirstP from T_Mid_AirForeCast d left join [dbo].[T_Bas_QualityStandard] q on d.AirLevel = q.QualitySymbol where Convert(varchar(12),MonitorTime,23)=Convert(varchar(12),getdate()-1,23) and ForecastModel=0 order by MonitorTime desc";
            string sql = @"select top 1 A.AQI,B.QualityRome AirLevel,B.Category,A.Firstp from [T_Mid_ReportData] A , T_Bas_QualityStandard B where A.QualityLevel=B.QualityLevel    and Convert(varchar(12),FutureDate,23)=Convert(varchar(12),getdate()-1,23)  order by FutureDate desc  ";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        /// <summary>
        ///昨日预报结果回顾
        /// </summary>
        /// <returns>返回实测结果</returns>
        public DataSet Get_YesDay_PracData()
        {

            // string sql = @"select top 1 AQI, AirLevel,Category AirLevelDes,FirstP from T_Mid_AirDayData d left join [dbo].[T_Bas_QualityStandard] s on d.AirLevel = s.QualityRome where Convert(varchar(12),MoniDate,23)=Convert(varchar(12),getdate()-1,23) order by MoniDate desc";
            string sql = @"select AQI,FirstP,l.QualityRome AirLevel,l.category AirLevelDes 
                           from [T_Mid_AirDayData] d
                           left join [dbo].[T_Bas_QualityStandard] l on d.AirLevel=l.QualityLevel 
                           where Convert(varchar(12),MoniDate,23)=Convert(varchar(12),getdate()-1,23)  
                           order by MoniDate desc";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        ///<summary>
        ///1时到9时监测实况
        ///</summary>
        ///<returns>返回监测小时的平均数据</returns>
        public DataSet Get_AvgHourData()
        {

            string sql = @"select so2,no2,pm10,pm25,o3,co,[dbo].[fun_AirQualityIndex](coaqi,so2aqi,no2aqi,0,o3aqi,pm25aqi,pm10aqi) aqi,
[dbo].[fun_AQI_FristPollutant](coaqi,so2aqi,no2aqi,0,o3aqi,pm25aqi,pm10aqi) fp,
[dbo].[fun_AirAQIdes]([dbo].[fun_AirQualityIndex](coaqi,so2aqi,no2aqi,0,o3aqi,pm25aqi,pm10aqi)) des,
[dbo].[fun_AirAQIdegree]([dbo].[fun_AirQualityIndex](coaqi,so2aqi,no2aqi,0,o3aqi,pm25aqi,pm10aqi)) cate
 from (
select *,[dbo].[so22iaqi](so2/1000) so2aqi,
[dbo].[no22iaqi](no2/1000) no2aqi,
[dbo].[pm102iaqi](PM10/1000) pm10aqi,
[dbo].[pm252iaqi](PM25/1000) pm25aqi,
[dbo].[co2iaqi](co) coaqi,
[dbo].[o32iaqi](O3/1000,8) o3aqi
 from(
select Convert(decimal(10,0),avg(SO2)) as SO2,Convert(decimal(10,0),avg(NO2)) as NO2,Convert(decimal(10,0),avg(PM10)) as PM10,Convert(decimal(10,0),avg(CO)) as CO,Convert(decimal(10,0),avg(O3)) as O3,Convert(decimal(10,0),avg(PM25)) as PM25,floor(avg(AQI)) as AQI 
from T_Mid_AirHourData where co>0 and o3>0 and pm25>0 and MoniDate between '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' and '" + DateTime.Now.ToString("yyyy-MM-dd") + " 09:00:00' ) t1) t2";
            return sqlh.ExecuteSQLDataSet(sql);
        }

        ///<summary>
        ///空气质量类别、空气质量级别
        ///</summary>
        ///<returns>返回空气质量类别、空气质量级别</returns>
        public DataSet Get_Bas_QualityData(string indata)
        {

            double indexdata = Convert.ToDouble(indata);
            string sql = @"select QualityIndex,Category from T_Bas_QualityStandard where " + indexdata + ">=MinValue and " + indexdata + "<=MaxValue";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        ///<summary>
        ///绑定空气质量级别
        ///</summary>
        ///<returns>绑定空气质量级别</returns>
        public DataSet Get_Bas_StardData()
        {

            string sql = @"select QualityLevel,(QualityIndex+'  '+Category) as QualityIndex,Proposal from T_Bas_QualityStandard";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        public DataTable Get_Bas_DateData(string ReportDate)
        {

            string sql = @"select a.QualityLevel as QualityLevel,a.firstp,a.AQI AS AQI,b.Proposal as Proposal,DAY(a.FutureDate) as MonitorDay from T_Mid_ReportData a left join T_Bas_QualityStandard b on Convert(nvarchar(5),a.QualityLevel)=b.QualityLevel where  ReportCode in(select top 1 ReportCode from T_Mid_Report where ReportTime ='" + ReportDate + "'  order by ReportCode desc ) order by a.FutureDate desc";
            DataTable data = sqlh.ExecuteSQLDataSet(sql).Tables[0];
            return data;
        }
        ///<summary>
        ///绑定预警污染后空气质量级别
        ///</summary>
        ///<returns>绑定空气质量级别</returns>
        public DataSet Get_Bas_PollutStard()
        {

            string sql = @"select id as QualityLevel,(Grade+'  '+Category) as QualityIndex from T_Bas_WarningLevel";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        ///<summary>
        ///区域绑定<
        ///</summary>
        ///<returns>区域绑定</returns>

        public DataTable Get_Bas_region()
        {
            string sql = @"select StationName,StationCode from T_Bas_AirStation";
            DataTable data = sqlh.ExecuteSQLDataSet(sql).Tables[0];
            return data;
        }
        ///<summary>
        ///污染物绑定
        ///</summary>
        ///<returns>污染物绑定</returns>
        public DataTable Get_Bas_pollution(string strsql)
        {
            string sql = @"select ItemName,ItemCode,Two from T_Bas_IAQIStandard " + strsql;
            DataTable data = sqlh.ExecuteSQLDataSet(sql).Tables[0];
            return data;
        }
        ///<summary>
        ///图表数据绑定
        ///</summary>
        ///<returns>图表数据绑定</returns>
        public DataTable Get_Pollut_Data(string pollut, string region)
        {
            string sql = @"select  Convert(decimal(10,2)," + pollut + ") as " + pollut + @",MoniDate from T_Mid_AirHourData d
                left join [dbo].[T_Bas_AirStation] s on d.stationcode = s.stationcode where s.StationName ='" + region + "' and MoniDate between  Convert(varchar(10),getdate(),23) and getdate() order by MoniDate";
            DataTable data = sqlh.ExecuteSQLDataSet(sql).Tables[0];
            return data;
        }
        ///<summary>
        ///绑定空气质量级别
        ///</summary>
        ///<returns>绑定空气质量级别</returns>
        public DataSet Get_Bas_SelectStardData()
        {

            string sql = @"  select * from(
  select a.AirLevel as AirLevel,a.TimeType as TimeType,firstp,a.AirLevelDes as AirLevelDes,b.Proposal as Proposal,DAY(a.MonitorTime) as MonitorDay,b.QualityLevel as QualityLevel from T_Mid_AirForeCastStat a left join
  T_Bas_QualityStandard b on a.AirLevel=b.QualitySymbol 
 
  where Convert(varchar(10),a.MonitorTime,23)=Convert(varchar(10),getDate(),23) 
  or Convert(varchar(10),a.MonitorTime,23)=Convert(varchar(10),getDate()+1,23) 
  or Convert(varchar(10),a.MonitorTime,23)=Convert(varchar(10),getDate()+2,23) and ForecastModel=0
  ) t1 inner join (
  select Convert(varchar(10),MonitorTime,23) mt ,sum(
cast(AQI*(t3.Weights/100) as int)
  ) aqi from T_Mid_AirForeCastStat t4 left join [dbo].[T_Cod_ModelType] t3 on t3.ModelTypeCode=t4.ForecastModel  where Convert(varchar(10),MonitorTime,23)=Convert(varchar(10),getDate(),23) 
  or Convert(varchar(10),MonitorTime,23)=Convert(varchar(10),getDate()+1,23) 
  or Convert(varchar(10),MonitorTime,23)=Convert(varchar(10),getDate()+2,23) group by Convert(varchar(10),MonitorTime,23)
  ) t2 on t1.MonitorDay=t2.mt";
            return sqlh.ExecuteSQLDataSet(sql);
        }

        ///<summary>
        ///绑定选择Label
        ///</summary>
        ///<returns>绑定选择Label</returns>
        ///<summary>
        public DataSet Get_Bas_LableBind(int data)
        {
            string sql = @"select MinValue,Proposal from T_Bas_QualityStandard where QualityLevel ='" + data + "'";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        ///<summary>
        ///保存前先查询
        ///</summary>
        ///<returns>保存前先查询</returns>
        public DataTable SelectReport(string date)
        {

            string strSql = @"select top 1 * from T_Mid_Report where ReportTime ='" + date + "' order by ReportTime desc,Reportcode desc";
            DataTable ta = sqlh.ExecuteSQLDataSet(strSql).Tables[0];

            return ta;
        }
        ///<summary>
        ///会商提交功能
        ///</summary>
        ///<returns>会商提交功能</returns>
        public int SubmitData(string date, int su)
        {
            int subdata = 0;
            string strSql = @"update T_Mid_Report set JCSubmit='" + su + "' where ReportCode=(select max(ReportCode) from T_Mid_Report where  ReportTime='" + date + "')";
            subdata = sqlh.ExecuteSQLNonQuery(strSql);
            return subdata;
        }

        ///<summary>
        ///
        /// 保存前先查询如果有今天数据就修改
        ///</summary>
        ///<returns>保存前先查询如果有今天数据就修改</returns>
        public bool UpdataQXTextData(string data, string Userid, string date)
        {
            string strSql = @"update T_Mid_Report set QXText='" + data + "',QXUserID='" + Userid + "' where ReportCode=(select max(ReportCode) from T_Mid_Report where  ReportTime='" + date + "' )";
            int rows = sqlh.ExecuteSQLNonQuery(strSql);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdataConMessage(TmidReportModel model, ref string code)
        {
            string strDate = model.ReprtTime.Date.ToShortDateString();
            string strSql = @"update T_Mid_Report set";
            strSql += @" JCUserID='" + model.JCUser + "',JCSubmit='" + model.JCSubmit + "',SFTSH='" + model.IsTSH + "',SFJCH='" + model.IsJCH + "',YJDJ='" + model.YJDJ + "',YJColor='" + model.YJColor + "',Firstp='" + model.Firstp + "',PolRange='" + model.PolRange + "',PolStart='" + model.PolStart + "',PolLast=" + model.PolLast;
            strSql += @" where ReportCode=(select max(ReportCode) from T_Mid_Report where  ReportTime='" + model.ReprtTime + "' )";
            int rows = sqlh.ExecuteSQLNonQuery(strSql);
            if (rows > 0)
            {
                code = strCode(strDate, "0");
                return true;

            }
            else
            {
                return false;
            }
        }
        ///<summary>
        ///会商人员插入表信息
        /// </summary>
        ///<returns>会商人员插入表信息</returns>

        public bool AddConMessage(TmidReportModel model, ref string code)
        {
            string strSql = @"insert into T_Mid_Report(ReportTime,JCUserID,JCSubmit,SFTSH,SFJCH,QXText,YJDJ,YJColor,Firstp,PolRange,PolStart,PolLast) values('" + model.ReprtTime + "','" + model.JCUser + "','" + model.JCSubmit + "','" + model.IsTSH + "','" + model.IsJCH + "','" + model.QXText + "','" + model.YJDJ + "','" + model.YJColor + "','" + model.Firstp + "','" + model.PolRange + "','" + model.PolStart + "'," + model.PolLast + ")";
            int rows = sqlh.ExecuteSQLNonQuery(strSql);
            if (rows > 0)
            {
                code = strCode(model.ReprtTime.ToString(), "0");
                return true;
            }
            else
            {
                return false;
            }
        }
        ///<summary>
        ///会商人员插入表信息QXText
        ///</summary>
        ///<returns>会商人员插入表信息QXText</returns>
        public bool InsertQXtEXT(string data, string UserId, string date)
        {
            string strSql = @"insert into T_Mid_Report(QXText,QXUserID,ReportTime) values('" + data + "','" + UserId + "','" + date + "')";
            int rows = sqlh.ExecuteSQLNonQuery(strSql);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string strCode(string codedate, string submit)
        {
            string strCode = "";
            string matedate = Convert.ToDateTime(codedate).ToString("yyyy-MM-dd");
            string strSql = @"select ReportCode as ReportCode from T_Mid_Report where ReportTime='" + matedate + "' and JCSubmit = " + submit + " order  by ReportCode desc";
            DataTable strtable = sqlh.ExecuteSQLDataSet(strSql).Tables[0];
            if (strtable.Rows.Count > 0)
            {
                strCode = strtable.Rows[0]["ReportCode"].ToString();

            }
            return strCode;
        }
        ///<summary>
        ///会商人员插入表2信息
        ///</summary>
        ///<returns>会商人员插入表2信息</returns>
        public bool AddConSecMessage(TmidReportDataModel model)
        {

            string strSql = "insert into T_Mid_ReportData(ReportCode,AQI,TimeType,FutureDate) values('" + model.ReportCode + "','" + model.AQI + "','" + model.TimeType + "','" + model.FutureDate + "')";
            int rows = sqlh.ExecuteSQLNonQuery(strSql);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        ///<summary>
        ///更新表多条信息
        ///插入表多条信息
        ///</summary>
        ///<returns>更新表多条信息</return 
        ///<returns>插入表多条信息</returns>
        public int InserMoreData(int[] level, double[] array, string strcode, string[] fp, DateTime ReportDate)
        {

            int code = Convert.ToInt32(strcode);
            int rows = 0;
            string delesql = @"delete from T_Mid_ReportData where ReportCode='" + strcode + "'";
            sqlh.ExecuteSQLNonQuery(delesql);



            for (int i = 0; i < 3; i++)
            {
                string strfutur = ReportDate.AddDays(i).ToString();
                int datalevel = level[i];
                double aqi = array[i];

                string strdvalue = "";

                strdvalue = @"insert into T_Mid_ReportData(ReportCode,AQI,QualityLevel,FutureDate,Firstp) values('" + code + "','" + aqi + "','" + datalevel + "','" + strfutur + "','" + fp[i] + "')";


                sqlh.ExecuteSQLNonQuery(strdvalue);
                rows++;
            }



            return rows;
        }

        /// <summary>
        /// 返回会商页面预报数据列表
        /// </summary>
        /// <param name="time">按照时段显示</param>
        /// <param name="cate">城市、区县、测点</param>
        /// <param name="model">预报数据模型统计、CMAX、WRF-CHEM</param>
        /// <returns></returns>
        public DataSet GetAirForecastByCondition(string time, string cate, string model)
        {
            string sql = "";
            if (model == "tj" && cate == "city")
            {
                sql = " select * from ( select top 4 * from V_Mid_AirForeCastNew  where stationcode='00' and  MonitorTime between  CONVERT(varchar, GETDATE(),23) and    DATEADD(dd,3, GETDATE()) and ForecastModel=0 order by MonitorTime desc,OrderID ) t order by MonitorTime";

            }
            else if (model == "tj" && cate == "region")
            {
                sql = "select  * from V_Mid_AirForeCastNew  where stationcode in (select StationCode from [dbo].[T_Bas_AirStation] where StationType='2'  ) and  MonitorTime between  CONVERT(varchar, GETDATE(),23) and    DATEADD(dd,3, GETDATE()) and ForecastModel=0 order by MonitorTime desc,OrderID";

            }
            else if (model == "tj" && cate == "point")
            {

                sql = "select  * from V_Mid_AirForeCastNew  where StationCode  in (select StationCode from [dbo].[T_Bas_AirStation] where StationType='1' and StationCode!='00')  and  MonitorTime between  CONVERT(varchar, GETDATE(),23) and    DATEADD(dd,3, GETDATE()) and ForecastModel=0 order by MonitorTime desc,OrderID";

            }
            else if (cate == "city" && time == "0")
            {
                sql = @"select *,[dbo].[fun_AirAQIdegree](AQI) AirLevel,[dbo].[fun_AQI_FristPollutant](coaqi,so2aqi,no2aqi,o3aqi,0,pm25aqi,pm10aqi) FirstP from(
                         select 
                         StationName,
                         convert(varchar, MonitorTime, 23) MonitorTime,
                         AVG(SO2AQI) SO2AQI,
                         AVG(SO2) so2,
                         AVG(NO2AQI) No2AQI,
                         avg(NO2) no2,
                         avg(COAQI) coaqi,
                         avg(co) co,
                         avg(O3AQI) o3aqi,
                         avg(O3) o3,
                         avg(PM10AQI) pm10AQI,
                         avg(PM10) pm10,
                         avg(PM25AQI) PM25AQI,
                         avg(PM25) pm25,
                         avg(AQI) AQI 
                         from V_Mid_AirForeCastNew  where stationcode='00' and  convert(varchar, MonitorTime, 23) between  CONVERT(varchar, GETDATE(),23) and    DATEADD(dd,3, GETDATE())
                         and ForecastModel=" + model + " group by StationName,convert(varchar, MonitorTime, 23)) t1";
            }
            else if (cate == "city" && time == "1")
            {
                sql = @" select *,[dbo].[fun_AirAQIdegree](AQI) AirLevel,[dbo].[fun_AQI_FristPollutant](coaqi,so2aqi,no2aqi,o3aqi,0,pm25aqi,pm10aqi) FirstP from(
                         select 
                         StationName,
                        MonitorTime1 as MonitorTime,
                        shid,
                         AVG(SO2AQI) SO2AQI,
                         AVG(SO2) so2,
                         AVG(NO2AQI) No2AQI,
                         avg(NO2) no2,
                         avg(COAQI) coaqi,
                         avg(co) co,
                         avg(O3AQI) o3aqi,
                         avg(O3) o3,
                         avg(PM10AQI) pm10AQI,
                         avg(PM10) pm10,
                         avg(PM25AQI) PM25AQI,
                         avg(PM25) pm25,
                         avg(AQI) AQI from(
                         select *, convert(varchar, MonitorTime, 23) MonitorTime1, case when DATEPART(hh,monitortime)>=8 and  DATEPART(hh,monitortime)<=12 then '上午' when 
                         DATEPART(hh,monitortime)>=13 and  DATEPART(hh,monitortime)<20 then '下午' else '夜间' end shid
                         from  V_Mid_AirForeCastNew where stationcode='00' and MonitorTime between  CONVERT(varchar, GETDATE(),23) and    DATEADD(dd,3, GETDATE())
                         and ForecastModel=" + model + " ) c group by  StationName, MonitorTime1,shid) c";
            }
            else if (cate == "region" && time == "0")
            {
                sql = @"select *,[dbo].[fun_AirAQIdegree](AQI) AirLevel,[dbo].[fun_AQI_FristPollutant](coaqi,so2aqi,no2aqi,o3aqi,0,pm25aqi,pm10aqi) FirstP from(
                         select 
                         StationName,
                         convert(varchar, MonitorTime, 23) MonitorTime,
                         AVG(SO2AQI) SO2AQI,
                         AVG(SO2) so2,
                         AVG(NO2AQI) No2AQI,
                         avg(NO2) no2,
                         avg(COAQI) coaqi,
                         avg(co) co,
                         avg(O3AQI) o3aqi,
                         avg(O3) o3,
                         avg(PM10AQI) pm10AQI,
                         avg(PM10) pm10,
                         avg(PM25AQI) PM25AQI,
                         avg(PM25) pm25,
                         avg(AQI) AQI 
                         from V_Mid_AirForeCastNew  where stationcode in(select StationCode from [dbo].[T_Bas_AirStation] where StationType='2') and MonitorTime between  CONVERT(varchar, GETDATE(),23) and    DATEADD(dd,3, GETDATE())

                         and ForecastModel=" + model + " group by StationName,convert(varchar, MonitorTime, 23)) t1";
            }
            else if (cate == "region" && time == "1")
            {
                sql = @" select *,[dbo].[fun_AirAQIdegree](AQI) AirLevel,[dbo].[fun_AQI_FristPollutant](coaqi,so2aqi,no2aqi,o3aqi,0,pm25aqi,pm10aqi) FirstP from(
                         select 
                         StationName,
                        MonitorTime1 as MonitorTime,
                        shid,
                         AVG(SO2AQI) SO2AQI,
                         AVG(SO2) so2,
                         AVG(NO2AQI) No2AQI,
                         avg(NO2) no2,
                         avg(COAQI) coaqi,
                         avg(co) co,
                         avg(O3AQI) o3aqi,
                         avg(O3) o3,
                         avg(PM10AQI) pm10AQI,
                         avg(PM10) pm10,
                         avg(PM25AQI) PM25AQI,
                         avg(PM25) pm25,
                         avg(AQI) AQI from(
                         select *, convert(varchar, MonitorTime, 23) MonitorTime1, case when DATEPART(hh,monitortime)>=8 and  DATEPART(hh,monitortime)<=12 then '上午' when 
                         DATEPART(hh,monitortime)>=13 and  DATEPART(hh,monitortime)<20 then '下午' else '夜间' end shid
                         from  V_Mid_AirForeCastNew where stationcode in (select StationCode from [dbo].[T_Bas_AirStation] where StationType='2') and MonitorTime between  CONVERT(varchar, GETDATE(),23) and    DATEADD(dd,3, GETDATE())

                         and ForecastModel=" + model + " ) c group by  StationName, MonitorTime1,shid) c";
            }
            else if (cate == "point" && time == "0")
            {

                sql = @"select *,[dbo].[fun_AirAQIdegree](AQI) AirLevel,[dbo].[fun_AQI_FristPollutant](coaqi,so2aqi,no2aqi,o3aqi,0,pm25aqi,pm10aqi) FirstP from(
                         select 
                         StationName,
                         convert(varchar, MonitorTime, 23) MonitorTime,
                         AVG(SO2AQI) SO2AQI,
                         AVG(SO2) so2,
                         AVG(NO2AQI) No2AQI,
                         avg(NO2) no2,
                         avg(COAQI) coaqi,
                         avg(co) co,
                         avg(O3AQI) o3aqi,
                         avg(O3) o3,
                         avg(PM10AQI) pm10AQI,
                         avg(PM10) pm10,
                         avg(PM25AQI) PM25AQI,
                         avg(PM25) pm25,
                         avg(AQI) AQI 
                         from V_Mid_AirForeCastNew  where stationcode in (select StationCode from [dbo].[T_Bas_AirStation] where StationType='1'  and StationCode!='00')  and MonitorTime between  CONVERT(varchar, GETDATE(),23) and    DATEADD(dd,3, GETDATE())

                         and ForecastModel=" + model + " group by StationName,convert(varchar, MonitorTime, 23)) t1";
            }
            else if (cate == "point" && time == "1")
            {
                sql = @" select *,[dbo].[fun_AirAQIdegree](AQI) AirLevel,[dbo].[fun_AQI_FristPollutant](coaqi,so2aqi,no2aqi,o3aqi,0,pm25aqi,pm10aqi) FirstP from(
                         select 
                         StationName,
                        MonitorTime1 as MonitorTime,
                        shid,
                         AVG(SO2AQI) SO2AQI,
                         AVG(SO2) so2,
                         AVG(NO2AQI) No2AQI,
                         avg(NO2) no2,
                         avg(COAQI) coaqi,
                         avg(co) co,
                         avg(O3AQI) o3aqi,
                         avg(O3) o3,
                         avg(PM10AQI) pm10AQI,
                         avg(PM10) pm10,
                         avg(PM25AQI) PM25AQI,
                         avg(PM25) pm25,
                         avg(AQI) AQI from(
                         select *, convert(varchar, MonitorTime, 23) MonitorTime1, case when DATEPART(hh,monitortime)>=8 and  DATEPART(hh,monitortime)<=12 then '上午' when 
                         DATEPART(hh,monitortime)>=13 and  DATEPART(hh,monitortime)<20 then '下午' else '夜间' end shid
                         from  V_Mid_AirForeCastNew where stationcode in (select StationCode from [dbo].[T_Bas_AirStation] where StationType='1'  and StationCode!='00')  and MonitorTime between  CONVERT(varchar, GETDATE(),23) and    DATEADD(dd,3, GETDATE())

                         and ForecastModel=" + model + " ) c group by  StationName, MonitorTime1,shid) c";
            }
            if (sql == "") return null;

            return sqlh.ExecuteSQLDataSet(sql);
        }

    }
}
