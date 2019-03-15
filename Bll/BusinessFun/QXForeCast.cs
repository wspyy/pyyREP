﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.BusinessFun
{
    public class QXForeCast
    {
        public DataSet GetQXForeCastNewData()
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = @"SELECT * FROM V_Mid_QXForeCast 
where ForeCastTime = (select max(ForeCastTime) from V_Mid_QXForeCast) and ForeCastHours=24
and TimeType = (SELECT max(TimeType) FROM V_Mid_QXForeCast 
where ForeCastTime = (select max(ForeCastTime) from V_Mid_QXForeCast) and ForeCastHours=24)";
            return sqlh.ExecuteSQLDataSet(sql);
        }

        public DataSet GetQXForeCastHisData(string datetime,string timeType,string foreCastHours)
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = @"SELECT * FROM V_Mid_QXForeCast where ForeCastTime = '" + datetime + "' and timetype = " + timeType + " and  ForeCastHours=" + foreCastHours;
            return sqlh.ExecuteSQLDataSet(sql);
        }
    }
}
