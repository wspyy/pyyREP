using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public class TmidReportModel
    {
      private int int_reportCode;
      public int ReportCode
      {
          set { int_reportCode = value; }
          get { return int_reportCode; }
      }
      private DateTime date_reportTime;
      public DateTime ReprtTime
      {
          set { date_reportTime = value; }
          get { return date_reportTime; }
      }
      private string str_QXText;
      public string QXText
      {
          set { str_QXText = value; }
          get { return str_QXText; }
      }
      private string str_QXUser;
      public string QXUser
      {
          set { str_QXUser = value; }
          get { return str_QXUser; }
      }
      private int int_QXSubmit;
      public int QXSubmit
      {
          set { int_QXSubmit = value; }
          get { return int_QXSubmit; }
      }
      private string str_JCUser;
      public string JCUser
      {
          set { str_JCUser = value; }
          get { return str_JCUser; }
      }
      private int int_JCSubmit;
      public int JCSubmit
      {
          set { int_JCSubmit = value; }
          get { return int_JCSubmit; }
      }
      private string isTSH;

      public string IsTSH
      {
          get { return isTSH; }
          set { isTSH = value; }
      }
      private string isJCH;

      public string IsJCH
      {
          get { return isJCH; }
          set { isJCH = value; }
      }

      private string yJDJ;

      public string YJDJ
      {
          get { return yJDJ; }
          set { yJDJ = value; }
      }

      private string yJColor;

      public string YJColor
      {
          get { return yJColor; }
          set { yJColor = value; }
      }
       /// <summary>
       /// 首要污染物
       /// </summary>
      public string Firstp { get; set; }
       /// <summary>
       /// 污染范围
       /// </summary>
      public string PolRange { get; set; }
      /// <summary>
      /// 污染物开始时间
      /// </summary>
       public DateTime PolStart { get; set; }
       /// <summary>
       /// 污染物持续时间(单位:X天)
       /// </summary>
       public Int32 PolLast { get; set; }
    }
}
