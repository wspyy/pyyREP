using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Model;


namespace Bll.BusinessFun
{
   public class ForCastWarnInfoDal
    {
       SQLHelper bill = new SQLHelper();
       public DataTable GetWarnInfoData(string begin, string end,int page,int rows,ref int rowcount,ref int countPage)
       {

           string sql = "select f.ReportTime as FutureDate,f.ReportCode as ReportCode,f.AQI as AQI,g.Category as Category,g.ColorName as ColorName,g.ColorCode,g.Grade,g.RadiusColor from (select distinct e.ReportTime,d.AQI,d.ReportCode,d.QualityLevel from (select AQI,ReportCode,QualityLevel from T_Mid_ReportData) d inner join (select b.ReportCode,MIN(AQI) as AQI,b.ReportTime FROM T_Mid_ReportData a inner join T_Mid_Report b on a.ReportCode=b.ReportCode where a.AQI>=200 group by b.ReportTime,b.ReportCode) e on d.ReportCode=e.ReportCode and d.AQI=e.AQI) f inner join T_Bas_WarningLevel g on (f.QualityLevel-4)=g.id";
           sql += " where convert(varchar(15),f.ReportTime ,23)>=convert(varchar(15),'" + begin + "',23) and convert(varchar(15),f.ReportTime ,23)<= convert(varchar(15),'" + end + "',23) order by f.ReportTime desc";
          DataSet infodataset= ConsultReportDal.Paging(sql, page, rows, ref rowcount, ref countPage);
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
       
       public List<WarnInfoModel> getList(string begin, string end, int pagesize, int pageIndex, string orderby)
       {
          List<WarnInfoModel> WarnInfoList=new List<WarnInfoModel>();
           if (begin != "" && end != "")
           {
               DateTime begindate = Convert.ToDateTime(begin);
               DateTime enddate = Convert.ToDateTime(end);
           }
           //try
           //{
               string sql = "select Row_Number() over(order by " + orderby + ") as RowNum,ReportCode,FutureDate,Category,ColorName,ColorCode,AQI,QualityIndex  from T_Mid_ReportData a inner join T_Bas_QualityStandard b on a.AQI=b.QualityLevel ";
               //sql += "where a.ReportCode not in(select ReportCode from T_Mid_ReportData where AQI>3) and a.FutureDate >=" + begindate + " and a.FutureDate <=" + enddate + "";
               sql = "select RowNum,ReportCode,FutureDate,Category,ColorName,ColorCode,AQI,QualityIndex from (" + sql + ") as Tab where Tab.RowNum>" + (pageIndex - 1) * pagesize + " and Tab.RowNum<=" + pageIndex * pagesize;

               DataTable Wartable = bill.ExecuteSQLDataSet(sql).Tables[0];
               WarnInfoModel model= null;
               if (Wartable.Rows.Count <= 0)
               {
                   return WarnInfoList;
               }
               for (int i = 0; i < Wartable.Rows.Count; i++)
               {
                   model = new WarnInfoModel();
                   if(Wartable.Rows[i]["AQI"].ToString()!="")
                   {
                   model.AQI=Convert.ToInt32(Wartable.Rows[i]["AQI"].ToString());
                   }
                    model.Category=Wartable.Rows[i]["Category"].ToString();
                    model.ColorCode=Wartable.Rows[i]["ColorCode"].ToString();
                    model.ColorName=Wartable.Rows[i]["ColorName"].ToString();
                    if (Wartable.Rows[i]["FutureDate"].ToString() != "")
                    {
                        model.FutureDate = Convert.ToDateTime(Wartable.Rows[i]["FutureDate"].ToString());
                    }

                    model.QualityIndex=Wartable.Rows[i]["ColorName"].ToString();
                    if (Wartable.Rows[i]["ReportCode"].ToString() != "")
                    {
                        model.ReportCode = Convert.ToInt32(Wartable.Rows[i]["ReportCode"].ToString());
                    }
                    if (Wartable.Rows[i]["RowNum"].ToString() != "")
                    {
                        model.RowNum = Convert.ToInt32(Wartable.Rows[i]["RowNum"].ToString());
                    }
                    WarnInfoList.Add(model);

               }
           //}
           //catch (Exception e)
           //{
           //    WarnInfoList = null;
           //}


           return WarnInfoList;

       }
       public DataTable GetWarnDetailData(string regionName, string hour1, string hour2, int page, int rows, ref int rowcount, ref int countPage)
       {

           string sql = "select * from T_Mid_AirForeCast where StationName='" + regionName + "' and CONVERT(varchar(30),MonitorTime,120)>CONVERT(varchar(30),'" + hour1 + "',120) and CONVERT(varchar(30),MonitorTime,120)<CONVERT(varchar(30),'" + hour2 + "',120) Order by MonitorTime desc";
         DataSet infoSet = ConsultReportDal.Paging(sql, page, rows, ref rowcount, ref countPage);
         if (infoSet != null && infoSet.Tables.Count == 3)
         {
             DataTable infoTable = infoSet.Tables[2];
             return infoTable;
         }
         else
         {
             return null;
         }
           
       }
       public DataSet SelectWDetaile()
       {
           string sql = "select a.ID as id,a.QualityLevelID as QualityLevelID, a.StationCode as StationCode,b.StationName as StationName,c.Grade+' '+c.Category as QualityCet,a.AQI as AQI,a.FirstP as FirstP from T_Mid_WarnInfoDetail a inner join T_Bas_AirStation b on a.StationCode = b.StationCode inner join T_Bas_WarningLevel c on a.QualityLevelID = c.id order by id asc";
           DataSet data= bill.ExecuteSQLDataSet(sql);
           return data;
       }
       public int InSertWDetaile(WarnInfoDetailModel model)
       {
           string sql = "insert into T_Mid_WarnInfoDetail(StationCode,AQI,QualityLevelID,FirstP) values('" + model.StationCode + "',";
           sql += "'" + model.AQI + "','" + model.QualityLevelID + "','" + model.FirstP + "')";
           int rows = bill.ExecuteSQLNonQuery(sql);
           return rows;

       }
       public int DeleteWDtaile(int id)
       {
           string sql = "delete from T_Mid_WarnInfoDetail where id='" + id + "'";
           int rows = bill.ExecuteSQLNonQuery(sql);
           return rows;
       }

       public int UpdateWDetaile(WarnInfoDetailModel model)
       {
           string sql = "update T_Mid_WarnInfoDetail set StationCode='" + model.StationCode + "',AQI='" + model.AQI + "',QualityLevelID='" + model.QualityLevelID + "',";
           sql += "FirstP='" + model.FirstP + "' where ID='" + model.id + "'";
           int rows = bill.ExecuteSQLNonQuery(sql);
           return rows;
       }
       public DataSet GetregionData()
       {
           string sql = "select StationName,StationCode from T_Bas_AirStation where StationType = 1";
           DataSet data = bill.ExecuteSQLDataSet(sql);
           return data;
       }
       public DataSet GetQxStationData()
       {
           string sql = "select StationCode,StationName from T_Bas_AirStation";
           DataSet data = bill.ExecuteSQLDataSet(sql);
           return data;
       }
    }
}
