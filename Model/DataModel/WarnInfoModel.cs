using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public class WarnInfoModel
    {
        private int int_RowNum;
        public int RowNum
        {
            get { return int_RowNum; }
            set { int_RowNum = value; }
        }
        private int int_ReportCode;
        public int ReportCode
        {
            get { return int_ReportCode; }
            set { int_ReportCode = value; }
        }
        private DateTime date_FutureDate;
        public DateTime FutureDate
        {
            get { return date_FutureDate; }
            set { date_FutureDate = value; }
        }
        private string str_Category;
        public string Category
        {
            get { return str_Category; }
            set { str_Category = value; }
        }
        private string str_ColorName;
        public string ColorName
        {
            get { return str_ColorName; }
            set { str_ColorName = value; }
        }
        private string str_ColorCode;
        public string ColorCode
        {
            get { return str_ColorCode; }
            set { str_ColorCode = value; }
        }
      private int int_AQI;
      public int AQI
      {
          get { return int_AQI; }
          set { int_AQI = value; }
      }
      private string str_QualityIndex;
      public string QualityIndex
      {
          get { return str_QualityIndex; }
          set { str_QualityIndex = value; }
      }
    }
}
