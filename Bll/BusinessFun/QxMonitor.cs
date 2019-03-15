using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.BusinessFun
{
   public class QxMonitor
    {
       /// <summary>
       /// 返回最新气象监测数据
       /// </summary>
       /// <returns></returns>
       public DataSet GetQxRealTimeData() 
       {
           SQLHelper sqlh = new SQLHelper();
//           string sql = @"select * from V_Mid_QxRealTimeData
//                          where ObservTimes = ( 
//                          select max(ObservTimes) from [T_Mid_QXRealTimeData] " + sqlwhere + ")";
           string sql = "select q.StationName,q.StationCode,q.lon,q.lat,w.cityname, w.temNow,w.windPower,w.windDir,substring(w.humidity,1,len(w.humidity)-1)humidity, w.time, w.stationNum,d.[WindDirectionCenter] from [dbo].[T_Mid_WeatherData]w inner join [dbo].[T_Bas_QxStation]q on w.stationNum=q.StationCode left join (select convert(char(3),WindDirectionCenter)WindDirectionCenter,[WindDirectionName] from [dbo].[T_Bas_WindDirection] )d on SUBSTRING(w.windDir,1,(len(w.windDir)-1))=d.[WindDirectionName] where time=(select max(time) from [dbo].[T_Mid_WeatherData] )";
          return  sqlh.ExecuteSQLDataSet(sql);
       }

       /// <summary>
       /// 返回时间段内气监测数据
       /// </summary>
       /// <returns></returns>
       public DataSet GetQxRealTimeDataByDate(string StationName,string begin,string end) 
       {
           return null;
       }

       /// <summary>
       /// 查询气象数据历史数据
       /// </summary>
       /// <param name="sqlwhere"></param>
       /// <returns></returns>
       public object GetQxHistData(string sqlwhere)
       {
           SQLHelper sqlh = new SQLHelper();
           //string sql = @"select * from V_Mid_QxRealTimeData " + sqlwhere;
           string sql = "select windPower,temNow,windDir,substring(humidity,1,len(humidity)-1)humidity, time, stationNum from [dbo].[T_Mid_WeatherData]" + sqlwhere;
           return sqlh.ExecuteSQLDataSet(sql);
       }
    }
}
