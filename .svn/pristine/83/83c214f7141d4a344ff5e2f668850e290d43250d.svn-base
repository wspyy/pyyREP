using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.SingleTable;
using Bll.BusinessFun;
using Model;



public partial class Devoops_ForeCast_WarnInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //bindData();
        }
    } /// <summary>
    /// 执行页面绑定数据信息
    /// </summary>
    ForCastWarnInfoDal bill = new ForCastWarnInfoDal();
    //public void bindData()
    //{
    //    ////string name = txtEnterName.Text.Trim();    //搜索关键字
    //    //if (name.Contains("@") || name.Contains("%") || name.Contains("_"))
    //    //{
    //    //    BindEmpty();
    //    //}
    //    //else
    //    //{
    //    //DataTable ds = bill.getList("", "",6,);
    //       // this.AspNetPager1.RecordCount = ds.Rows.Count;   //全部的数据条数
    //        int Pagesize = this.AspNetPager1.PageSize;  //页面显示的条数
    //        int CurentIndex = this.AspNetPager1.CurrentPageIndex;   //页面的索引,放置到页面总数的后面，不然删除后刷新，可能显示没有数据。
    //        List<WarnInfoModel> list = bill.getList("", "", this.AspNetPager1.PageSize, CurentIndex, "ReportCode");  //获取符合条件的对象泛型
    //        if (list.Count > 0)
    //        {
    //            gv_Enterprise.DataSource = list; //数据源赋值
    //            gv_Enterprise.DataBind();
    //        }
    //        else
    //        {
    //            BindEmpty();
    //        }
    //    //}
    //}
    /// <summary>
    /// 当数据源为空时显示的数据信息
    ///// </summary>
    //public void BindEmpty()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("EntCode");
    //    dt.Columns.Add("EntName");
    //    dt.Columns.Add("Longitude");
    //    dt.Columns.Add("Latitude");
    //    dt.Rows.Add(dt.NewRow());
    //    gv_Enterprise.DataSource = dt;
    //    gv_Enterprise.DataBind();
    //    int columCount = dt.Columns.Count + 4;
    //    gv_Enterprise.Rows[0].Cells.Clear();
    //    gv_Enterprise.Rows[0].Cells.Add(new TableCell());
    //    gv_Enterprise.Rows[0].Cells[0].ColumnSpan = columCount;
    //    gv_Enterprise.Rows[0].Cells[0].Text = "没有数据......";
    //    gv_Enterprise.Rows[0].Cells[0].Style.Add("text-align", "center");
    //}
    //protected void AspNetPager1_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
    //{
    //    this.AspNetPager1.CurrentPageIndex = e.NewPageIndex;
    //    bindData();
    //}
    //protected void txtEnterName_TextChanged(object sender, EventArgs e)
    //{
    //    bindData();
    //}
    //protected void btn_query_Click(object sender, EventArgs e)
    //{
    //    bindData();
    //}
}
