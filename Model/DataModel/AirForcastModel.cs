using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AirForcastModel
    {
        private string str_StationName;
        public string StationName
        {
            get { return str_StationName; }
            set { str_StationName = value; }
        }
        private string str_StationCode;
        public string StationCode
        {
            get { return str_StationCode; }
            set { str_StationCode = value; }
        }
        private DateTime datetime_MonitorTime;
        public DateTime MonitorTime
        {
            get { return datetime_MonitorTime; }
            set { datetime_MonitorTime = value; }
        }
        private decimal dec_SO2;
        public decimal SO2
        {
            get { return dec_SO2; }
            set { dec_SO2 = value; }
        }
        private decimal dec_NO2;
        public decimal NO2
        {
            get { return dec_NO2; }
            set { dec_NO2 = value; }
        }
        private decimal dec_CO;
        public decimal CO
        {
            get { return dec_CO; }
            set { dec_CO = value; }
        }
        private decimal dec_O3;
        public decimal O3
        {
            get { return dec_O3; }
            set { dec_O3 = value; }
        }
        private decimal dec_PM25;
        public decimal PM25
        {
            get { return dec_PM25; }
            set { dec_PM25 = value; }
        }
        private decimal dec_PM10;
        public decimal PM10
        {
            get { return dec_PM10; }
            set { dec_PM10 = value; }
        }
        private int int_AQI;
        public int AQI
        {
            get { return int_AQI; }
            set { int_AQI = value; }
        }
        private string str_FirstP;
        public string FirstP
        {
            get { return str_FirstP; }
            set { str_FirstP = value; }
        }
        private string str_AirLevel;
        public string AirLevel
        {
            get { return str_AirLevel; }
            set { str_AirLevel = value; }
        }
        private string str_AirLevelDes;
        public string AirLevelDes
        {
            get { return str_AirLevelDes; }
            set { str_AirLevelDes = value; }
        }
        private string str_AirColor;
        public string AirColor
        {
            get { return str_AirColor; }
            set { str_AirColor = value; }
        }

        //来自T_Bas_QualityStandard表
        private int int_QualityLevel;
        public int QualityLevel
        {
            get { return int_QualityLevel; }
            set { int_QualityLevel = value; }
        }
        private string str_QualityIndex;
        public string QualityIndex
        {
            get { return str_QualityIndex; }
            set { str_QualityIndex = value; }
        }
        private string str_Category;
        public string Category
        {
            get { return str_Category; }
            set { str_Category = value; }
        }
        private decimal dec_MinValue;
        public decimal MinValue
        {
            get { return dec_MinValue; }
            set { dec_MinValue = value; }
        }
        private decimal dec_MaxValue;
        public decimal MaxValue
        {
            get { return dec_MaxValue; }
            set { dec_MaxValue = value; }
        }
        private string str_ColorCode;
        public string ColorCode
        {
            get { return str_ColorCode; }
            set { str_ColorCode = value; }
        }
        private string str_Affect;
        public string Affect
        {
            get { return str_Affect; }
            set { str_Affect = value; }
        }
        private string str_Proposal;
        public string Proposal
        {
            get { return str_Proposal; }
            set { str_Proposal = value; }
        }
    }
}