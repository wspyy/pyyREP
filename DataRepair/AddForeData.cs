using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepair
{
    public class AddForeData
    {
        public string tcon = "";

        public void AddCMAQ()
        {
            string strSql = "select max(MonitorTime) from [dbo].[T_Mid_AirForeCast] where MonitorTime > '2015-1-1' and ForecastModel = 1 group by MonitorTime order by MonitorTime";
            DataSet ds = SqlHelper.ExecuteDataset(tcon, CommandType.Text, strSql);
            DateTime time = Convert.ToDateTime(ds.Tables[0].Rows[0][0].ToString());
            int i = 1;
            while (true)
            {
                DateTime time1 = Convert.ToDateTime(ds.Tables[0].Rows[i][0].ToString());
                if (time.AddHours(1) == time1)
                {
                    i++;
                    time = time1;
                    continue;
                }
                else 
                { 
                    //string sql = "select 
                }
            }
            

            DateTime startTime = new DateTime(2015, 1, 1);
            DateTime endTime = DateTime.Now;
            DateTime tmpTime = startTime;
            while (tmpTime < endTime)
            {
                //Add
            }
        }
    }
}
