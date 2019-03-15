using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepair
{
    class QxMonitor
    {
        public string scon = "", tcon = "";

        /// <summary>cmd
        /// 
        /// 获取气象监测数据
        /// </summary>
        public void getQxMonitorData()
        {
            string lastTime = "";
            string strSql = "select max(InsertTime) from [dbo].[T_Mid_QXRealTimeData]";
            DataSet ds = SqlHelper.ExecuteDataset(tcon, CommandType.Text, strSql);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lastTime = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            strSql = "select * from [dbo].[T_Bas_QxStation] where StationType = 1";
            DataSet dts = SqlHelper.ExecuteDataset(tcon, CommandType.Text, strSql);
            string station = "";
            for (int i = 0; i < dts.Tables[0].Rows.Count; i++)
            {
                station += "'" + dts.Tables[0].Rows[i]["StationCode"].ToString() + "',";
            }
            if (station.Length > 0) station = station.Substring(0, station.Length - 1);
            if (lastTime.Length > 0)
            {
                strSql = "select * from [dbo].[tabtimedata] where SUBSTRING(ObservTimes,11,LEN(ObservTimes)) = '00' and InsertTime > '" + lastTime + "' and stationnum in (" + station + ")";
                ds = SqlHelper.ExecuteDataset(scon, CommandType.Text, strSql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string strInsertCol = "insert T_Mid_QXRealTimeData(";
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        strInsertCol += ds.Tables[0].Columns[i].ToString() + ",";
                    }
                    strInsertCol = strInsertCol.Substring(0, strInsertCol.Length - 1);
                    strInsertCol += ") values(";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string inSql = "";
                            string strInsertRow = "";
                            if (IsExist(ds.Tables[0].Rows[i]["StationNum"].ToString(), ds.Tables[0].Rows[i]["ObservTimes"].ToString()))
                                continue;
                            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                            {
                                strInsertRow += "'" + ds.Tables[0].Rows[i][j] + "',";
                            }
                            strInsertRow = strInsertRow.Substring(0, strInsertRow.Length - 1);
                            inSql = strInsertCol + strInsertRow + ")";
                            try
                            {
                                SqlHelper.ExecuteNonQuery(tcon, CommandType.Text, inSql);
                                writelog(strInsertCol);
                            }
                            catch (Exception ex)
                            {
                                writelog("数据同步出错：" + ex.Message.ToString());
                                //throw;
                            }
                        }
                    }
                }
            }
        }

        private bool IsExist(string stationNum, string ObservTimes)
        {
            string strSql = "select * from [dbo].[T_Mid_QXRealTimeData] where StationNum = '" + stationNum + "' and ObservTimes = '" + ObservTimes + "'";
            DataSet ds = SqlHelper.ExecuteDataset(tcon, CommandType.Text, strSql);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 将信息输出到文本文件
        /// </summary>
        /// <param name="readme"></param>
        public void writelog(string txt)
        {
            string logPath = ConfigurationManager.AppSettings["logPath"] + "QxDataGather\\" + DateTime.Now.ToString("yyyy-MM-dd").ToString() + ".txt";
            StreamWriter dout = new StreamWriter(logPath, true);
            dout.Write(System.DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + " => " + txt + "\r\n");
            dout.Close();
        }   
    }
}
