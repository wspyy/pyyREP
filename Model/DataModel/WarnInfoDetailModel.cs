using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public class WarnInfoDetailModel
    {
        private int int_id;
        public int id
        {
            get
            {
                return int_id;
            }
            set { int_id = value; }
        }
       private string str_StationCode;
       public string StationCode
       {
           get { return str_StationCode; }
           set { str_StationCode = value; }
       }
       private string Str_AQI;
       public string AQI
       {
           get { return Str_AQI; }
           set { Str_AQI = value; }
       }
       private int int_QualityLevelID;
       public int QualityLevelID
       {
           get { return int_QualityLevelID; }
           set { int_QualityLevelID = value; }
       }
       private string str_FirstP;
       public string FirstP
       {
           get { return str_FirstP; }
           set { str_FirstP = value; }
       }
      // private string str_visibility;
      // public string Visibility
      // {
      //     get { return str_visibility; }
      //     set { str_visibility = value; }
      // }
      //private string str_SustainTime;
      //public string SustainTime
      //{
      //    get { return str_SustainTime; }
      //    set { str_SustainTime = value; }
      //}
    }
}
