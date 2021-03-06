﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.BusinessFun
{
    public class BaseInfo
    {
        ///获取城市名称
        public DataSet GetCityList()
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = @"select distinct(d.CityName),c.CityCode  from  V_Mid_CityDayData d
inner join T_Bas_City c on charindex(c.cityname,d.cityname)>0 order by c.CityCode";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        ///获取V_Mid_AirForeCastNew、T_Mid_AirDayDataOld中的监测站点编号
        public DataSet GetACCAnaCode()
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = @"select distinct(f.StationCode),f.StationName from [AirDB_shuozh].[dbo].[V_Mid_AirForeCastNew] f inner join [AirDB_shuozh].[dbo].[T_Mid_AirDayDataOld] d on f.StationCode=d.StationCode and f.MonitorTime=d.MoniDate where ForeCastModel=0";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        /// <summary>
        /// 获取空气监测站列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetAirStation()
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = @"select StationCode,StationName,StationType from [dbo].[T_Bas_AirStation] where StationType > -1 order by OrderID";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        /// <summary>
        /// 获取监测项
        /// </summary>
        /// <returns></returns>
        public DataSet GetPolluteList()
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = @"select * from T_Cod_PolluteCode where PolluteType = 'air' order by OrderCode";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        /// <summary>
        /// 获取气象监测站点列表
        /// </summary>
        /// <returns></returns>
        public object GetQXStation()
        {
            SQLHelper sqlh = new SQLHelper();
//            string sql = @"SELECT  RegionName ,StationName ,StationCode ,lon ,lat ,StationHeiht ,StationType 
//                              FROM    T_Bas_QxStation  WHERE   ( StationType = 1 )";
            string sql = "select distinct cityname from [dbo].[T_Mid_WeatherData] ";
            return sqlh.ExecuteSQLDataSet(sql);
        }
        public DataTable GetForeCastCheckData(string beginDate, string endDate, string strCheckID, int forecastmodel, int page, int rows, ref int rowcount, ref int countPage)
        {
            SQLHelper sqlh = new SQLHelper();

            //string sql = @"select * from V_Mid_AirForeCastNew where Convert(varchar(30),MonitorTime,23)>=Convert(varchar(30),'" + beginDate + "',23) and Convert(varchar(30),MonitorTime,23)<=Convert(varchar(30),'" + endDate + "',23) and StationCode in(" + strCheckID + ") and forecastmodel=" + forecastmodel + " order by MonitorTime desc";
            string sql = @"select * from V_Mid_AirForeCastNew where MonitorTime>='" + beginDate + "' and MonitorTime<='" + endDate + "' and StationCode in(" + strCheckID + ") and forecastmodel=" + forecastmodel + " order by MonitorTime desc";
            DataSet infodataset = ConsultReportDal.Paging(sql, page, rows, ref rowcount, ref countPage);
            if (infodataset != null && infodataset.Tables.Count == 3)
            {
                DataTable infoTable = infodataset.Tables[2];
                return infoTable;
            }
            else
            {
                return null;
            }
          
        }

        public DataTable GetCityCheckData(string beginDate, string endDate, string strCheckID,  int page, int rows, ref int rowcount, ref int countPage)
        {
            SQLHelper sqlh = new SQLHelper();
            string idwhere = "";
            if (strCheckID != "")
            {
                idwhere = "and CityName in(" + strCheckID + ")";
            }
            //string sql = @"select * from V_Mid_AirForeCastNew where Convert(varchar(30),MonitorTime,23)>=Convert(varchar(30),'" + beginDate + "',23) and Convert(varchar(30),MonitorTime,23)<=Convert(varchar(30),'" + endDate + "',23) and StationCode in(" + strCheckID + ") and forecastmodel=" + forecastmodel + " order by MonitorTime desc";
            string sql = @"select * from V_Mid_CityDayData where MonitorTime>='" + beginDate + "' and MonitorTime<='" + endDate + "' " + idwhere + " order by MonitorTime desc";
            DataSet infodataset = ConsultReportDal.Paging(sql, page, rows, ref rowcount, ref countPage);
            if (infodataset != null && infodataset.Tables.Count == 3)
            {
                DataTable infoTable = infodataset.Tables[2];
                return infoTable;
            }
            else
            {
                return null;
            }
          
        }
    }
}
