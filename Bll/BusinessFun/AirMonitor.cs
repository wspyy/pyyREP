﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;//0730

namespace Bll.BusinessFun
{
   public class AirMonitor
    {
        /// <summary>
        /// 全国空气质量0730
        /// </summary>
        /// <param name="sqlwhere"></param>
        /// <returns></returns>
        public DataSet GetAirChinaQuality()
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = @"select * from V_Mid_CityDayData 
where MonitorTime=
(select max(MonitorTime) from V_Mid_CityDayData)
order by CityCode";
         DataSet data = sqlh.ExecuteSQLDataSet(sql);
            return data;
        }
       /// <summary>
       /// 获取空气质量日报数据
       /// </summary>
       /// <param name="sqlwhere"></param>
       /// <returns></returns>
       public DataSet GetAirDayMonitDataByWhere(string sqlwhere) 
       {
           SQLHelper sqlh = new SQLHelper();
           string sql = @"select a.StationName,a.StationCode,b.MoniDate,lon,lat,[SO2],[SO2AQI],[NO2AQI],[NO2],[COAQI],[CO],[O3AQI],[O3],[PM25AQI],[PM25],[PM10AQI],[PM10],[AQI],[FirstP],[AirLevel],c.* 
                        from dbo.T_Bas_AirStation a 
                        left join dbo.T_Mid_AirDayData b on a.StationName=b.StationName 
                        left join [dbo].[T_Bas_QualityStandard] c on b.AirLevel = c.QualityLevel
                        " + sqlwhere + " order by OrderID";
           return sqlh.ExecuteSQLDataSet(sql);
       }

       /// <summary>
       /// 获取空气质量小时数据
       /// </summary>
       /// <param name="sqlwhere"></param>
       /// <returns></returns>
       public DataSet GetHourAirMonitDataByWhere(string sqlwhere) 
       {
           SQLHelper sqlh = new SQLHelper();
           string sql = @"select a.StationName,a.StationCode,b.MoniDate,lon,lat,[SO2],[SO2AQI],[NO2AQI],[NO2],[COAQI],[CO],[O3AQI],[O3],[PM25AQI],[PM25],[PM10AQI],[PM10],[AQI],[FirstP],[AirLevel],c.* 
                        from dbo.T_Bas_AirStation a 
                        left join dbo.T_Mid_AirHourData b on a.StationName=b.StationName 
                        left join [dbo].[T_Bas_QualityStandard] c on b.AirLevel = c.QualityLevel
                        " + sqlwhere + " order by OrderID";
           return sqlh.ExecuteSQLDataSet(sql);
       }
       /// <summary>
       /// 获取最新日数据
       /// </summary>
       /// <param name="sqlwhere"></param>
       /// <returns></returns>
       public object GetAirDayMonitData(string sqlwhere)
       {
           SQLHelper sqlh = new SQLHelper();
           string sql = @"select * from  V_Mid_AirDayDataNew order by OrderID";
           return sqlh.ExecuteSQLDataSet(sql);
       }
       /// <summary>
       /// 获取最新小时数据
       /// </summary>
       /// <param name="sqlwhere"></param>
       /// <returns></returns>
       public object GetHourAirMonitData(string sqlwhere)
       {
           SQLHelper sqlh = new SQLHelper();
           string sql = @"select * from  V_Mid_AirHourDataNew order by OrderID";
           return sqlh.ExecuteSQLDataSet(sql);
       }
       /// <summary>
       /// 日数据展示
       /// </summary>
       /// <param name="sqlwhere"></param>
       /// <returns></returns>
       public object GetAirDayMonitDataDay(string sqlwhere)
       {
           SQLHelper sqlh = new SQLHelper();
           string sql = @"SELECT top 1  a.QualitySymbol [AirLevel] ,
        a.Category [AirLevelDes] ,
        b.AQI [AQI] ,
        b.FirstP [FirstP] ,
        CONVERT(VARCHAR(10), b.MoniDate, 120) [MoniDate]
FROM    dbo.T_Bas_QualityStandard a
        JOIN ( SELECT   *
               FROM     ( SELECT    * ,
                                    ROW_NUMBER() OVER ( ORDER BY MoniDate DESC ) AS rowno
                          FROM      T_Mid_AirDayData
                        ) t
               WHERE    rowno = 1
                        AND t.StationCode = 0
             ) b ON b.AQI >= a.MinValue
                    AND b.AQI <= a.MaxValue " + sqlwhere;
           return sqlh.ExecuteSQLDataSet(sql);
       }

       /// <summary>
       /// 获取空气质量监测数据
       /// </summary>
       /// <param name="sqlwhere"></param>
       /// <returns></returns>
       public object GetMonitorData(string datatype, string begin, string end, string station, string item)
       {
            string beginYear="";
            string endYear = "";
           if (datatype == "quarter")
           {
               DataSet ds = null;
               if (begin != "" && end != "")
               {
                   beginYear = begin.Substring(0, 4);
                   endYear = end.Substring(0, 4);
                  begin=begin.Substring(5, 1);
                   end=end.Substring(5, 1);                   
                   SqlParameter[] para = { 
                                          new SqlParameter("@datatype",datatype),
                                          new SqlParameter("@begin",begin),
                                          new SqlParameter("@end",end),
                                          new SqlParameter("@station",station),
                                          new SqlParameter("@item",item),                                          new SqlParameter("@beginYear",beginYear),
                                            new SqlParameter("@endYear",endYear),
                              };
                    ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Proc_AirMonitorData", para);
                  
               }
               return ds;
           }
           else
           {
               SqlParameter[] para = { 
                                          new SqlParameter("@datatype",datatype),
                                          new SqlParameter("@begin",begin),
                                          new SqlParameter("@end",end),
                                          new SqlParameter("@station",station),
                                          new SqlParameter("@item",item),                                     new SqlParameter("@beginYear",beginYear),
                                          new SqlParameter("@endYear",endYear),
                              };
               DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.GetConnString(), CommandType.StoredProcedure, "Proc_AirMonitorData", para);
               return ds;
           } 
          
       }
       /// <summary>
       /// 根据日期查询预测日报数据
       /// </summary>
       /// <param name="date"></param>
       /// <returns></returns>
       public DataSet GetStationDayData(string date,string now)
       {
           DataSet ds = null;
           SQLHelper sqlh = new SQLHelper();
           string sqlStr = "select  s.StationName,h.Aqi from T_Bas_AirStation s  left join [dbo].[V_Mid_AirForeCastNew]  h  on s.StationCode=h.StationCode where  h.MonitorTime='" + date + "' and h.ForecastModel=0 order by h.StationCode";
           ds=  sqlh.ExecuteSQLDataSet(sqlStr);
           return ds;

       }
       /// <summary>
       /// 根据日期查询预测小时数据
       /// </summary>
       /// <param name="date"></param>
       /// <returns></returns>
       public DataSet GetStationHourData(DateTime time,string now)
       {
           DataSet ds = null;
           SQLHelper sqlh = new SQLHelper();
           string sqlStr = "select distinct s.StationName,h.Aqi,h.MonitorTime,h.ForecastTime,h.ForecastModel,s.StationCode from T_Bas_AirStation s  left join [dbo].[V_Mid_AirForeCastNew]  h  on s.StationCode=h.StationCode where s.StationCode='00' and h.MonitorTime>='" + time.ToString("yyyy-MM-dd") + "' and h.MonitorTime<'" + time.AddDays(1).ToString("yyyy-MM-dd") + "' and h.ForecastModel=1 order by h.MonitorTime";
           ds = sqlh.ExecuteSQLDataSet(sqlStr);
           return ds;
       }
       /// <summary>
       /// 获取站点空气质量标准数据
       /// </summary>
       /// <param name="date"></param>
       /// <returns></returns>
       public DataSet GetStationDayStandard(string now,DateTime  time)
       {
           DataSet ds = null;
           SQLHelper sqlh = new SQLHelper();
           string sqlStr = "select   q.Category,  q.Affect, q.Proposal,a.FirstP,a.MonitorTime,a.Aqi from [dbo].[T_Bas_QualityStandard] q  join [dbo].[V_Mid_AirForeCastNew] a on q.QualityLevel=a.AirLevel1 where a.MonitorTime between'" + time.ToString("yyyy-MM-dd") + "' and '" + time.AddDays(3).ToString("yyyy-MM-dd") + "' and a.StationCode='00' and a.ForecastModel=0 order by a.MonitorTime";
           ds = sqlh.ExecuteSQLDataSet(sqlStr);
           return ds;
       }

       /// <summary>
       /// 获取站点空气质量标准数据
       /// </summary>
       /// <param name="date"></param>
       /// <returns></returns>
       public DataSet GetStationDayStandard1(string now, DateTime time)
       {
           DataSet ds = null;
           SQLHelper sqlh = new SQLHelper();
           string sqlStr = @"select   q.Category,  q.Affect, q.Proposal,a.FirstP,a.date MonitorTime,a.Aqi from [dbo].[T_Bas_QualityStandard] q  join [dbo].[V_Mid_AirForeCastDay] a on q.QualityLevel=a.AirLevel1 
where a.date between '"+ time.ToString("yyyy-MM-dd") +"' and '"+ time.AddDays(3).ToString("yyyy-MM-dd") +"' and a.StationCode='00' and a.ForecastModel=1 order by a.date ";
           ds = sqlh.ExecuteSQLDataSet(sqlStr);
           return ds;
       }
       /// <summary>
       /// 获得天气图图片信息
       /// </summary>
       /// <returns></returns>
       public DataSet GetWeatherImg(string type,string height,string begin,string end)
       {
           DataSet ds = null;
           SQLHelper sqlh = new SQLHelper();
           string sql = " select  MenuLeave4Type, MenuLeave5Type, PredictiveTime, PredictiveRealTime, Duration, Picposition, PicName from  [dbo].[T_DATA_FORECASTIMAGE] where [MenuLeave3Type]='" + type + "' and [MenuLeave4Type]='" + height + "' and (left(convert(varchar(100),[PredictiveTime],21),10)='" + begin + "' or left(convert(varchar(100),[PredictiveTime],21),10)='" + end + "' )order by [PredictiveTime]";
           ds = sqlh.ExecuteSQLDataSet(sql);
           return ds;
       }
       /// <summary>
       /// 根据类型获得高度数据
       /// </summary>
       /// <param name="type"></param>
       /// <returns></returns>
       public DataSet GetHeight(string type)
       {
           DataSet ds = null;
           SQLHelper sqlh = new SQLHelper();
           string sql = " select  distinct MenuLeave4Type  from  [dbo].[T_DATA_FORECASTIMAGE] where [MenuLeave3Type]='"+type+"'";
           ds = sqlh.ExecuteSQLDataSet(sql);
           return ds;
       }

        /// <summary>
       /// 获取发布的模式
       /// </summary>
       /// <param name="type"></param>
       /// <returns></returns>
       public string getPublishModel()
       {
           DataSet ds = null;
           SQLHelper sqlh = new SQLHelper();
           string sql = " select * from  T_Cod_ModelType where IsPublish = 1";
           ds = sqlh.ExecuteSQLDataSet(sql);
           if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
           {
               return ds.Tables[0].Rows[0]["ModelTypeCode"].ToString();
           }
           return "0";
       }
    }
}
