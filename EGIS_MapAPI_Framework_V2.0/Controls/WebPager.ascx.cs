using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_WebPager : System.Web.UI.UserControl
{

    private int currentPage = 1;
    public int CurrentPage
    {
        get
        {
            if (ViewState["CurrentPage"] != null)
            {
                return Convert.ToInt32(ViewState["CurrentPage"]);
            }
            else
            {
                return 1;
            }
        }
        set
        {
            if (currentPage != value)
            {
                currentPage = value;
                ViewState["CurrentPage"] = currentPage;
                ChangePage(currentPage);
            }
        }
    }

    private int recorderCount = 1;
    public int RecorderCount
    {
        get
        {
            if (ViewState["RecorderCount"] != null)
            {
                return Convert.ToInt32(ViewState["RecorderCount"]);
            }
            else
            {
                return 1;
            }
        }
        set
        {
            recorderCount = value;
            pageCount = (recorderCount + pageSize - 1) / pageSize;
            ViewState.Add("PageCount", pageCount);
            ViewState.Add("RecorderCount", recorderCount);
            this.LabelRecord.Text = recorderCount.ToString();
        }
    }

    private int pageSize = 1;
    public int PageSize
    {
        get
        {
            if (ViewState["PageSize"] != null)
            {
                return Convert.ToInt32(ViewState["PageSize"]);
            }
            else
            {
                return 1;
            }
        }
        set
        {
            pageSize = value;
            ViewState.Add("PageSize", pageSize);
            pageCount = (recorderCount + pageSize - 1) / pageSize;
            this.LabelMessage.Text = pageCount.ToString();
            ViewState.Add("PageCount", pageCount);
        }
    }

    private int pageCount = 1;
    public int PageCount
    {
        get
        {
            if (ViewState["PageCount"] != null)
            {
                return Convert.ToInt32(ViewState["PageCount"]);
            }
            else
            {
                return 1;
            }
        }
        set
        {
            if (pageCount != value)
            {
                pageCount = value;
                ViewState["PageCount"] = pageCount;
                ChangePage(currentPage);
            }
        }
    }


    /// <summary>
    /// 添加图层之前
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    public delegate void PageChangedEventHandler(object sender, int e);
    public event PageChangedEventHandler PageChanged;
    protected virtual void OnPageChanged(int e)
    {
        if (PageChanged != null)
        {
            PageChanged(this, e);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        TextBoxPage.Attributes.Add("onkeydown", "checkNum()");

        if (!IsPostBack)
        {

        }
        else
        {
            try
            {
                currentPage = Convert.ToInt32(TextBoxPage.Text);
            }
            catch (Exception ex)
            {
                TextBoxPage.Text = "1";
                currentPage = 1;
            }
            pageCount = Convert.ToInt32(ViewState["PageCount"]);
            pageSize = Convert.ToInt32(ViewState["PageSize"]);
            recorderCount = Convert.ToInt32(ViewState["RecorderCount"]);
        }
        ChangePage(currentPage);
    }

    protected void LinkButton_Click(object sender, EventArgs e)
    {
        int iTmpCurrent = 1;
        ImageButton myLinkButton = (ImageButton)sender;

        if (myLinkButton.CommandName == "First")
        {
            iTmpCurrent = 1;
        }
        else if (myLinkButton.CommandName == "Previous")
        {
            iTmpCurrent = currentPage - 1;
        }
        else if (myLinkButton.CommandName == "Next")
        {
            iTmpCurrent = currentPage + 1;
        }
        else if (myLinkButton.CommandName == "Last")
        {
            iTmpCurrent = pageCount;
        }
        else if (myLinkButton.CommandName == "Goto")
        {
            int iGoto = 1;
            if (int.TryParse(this.TextBoxPage.Text, out iGoto))
            {
                if (iGoto <= 1)
                {
                    iGoto = 1;
                }
                if (iGoto > pageCount)
                {
                    iGoto = pageCount;
                }
                TextBoxPage.Text = iGoto.ToString();
                iTmpCurrent = iGoto;
            }
            else
            {
                TextBoxPage.Text = string.Empty;
                iTmpCurrent = currentPage;
                return;
            }

        }

        if (iTmpCurrent < 1)
        {
            iTmpCurrent = 1;
        }

        ChangePage(iTmpCurrent);
        OnPageChanged(currentPage);
    }
    private void ChangePage(int page)
    {
        currentPage = page;
        //加载热点商品推荐
        if (page <= 1)
        {
            this.LinkButtonFirst.Enabled = false; this.LinkButtonFirst.ImageUrl = "~/Images/Buttons/skip_backward_grey.gif";
            this.LinkButtonPrevious.Enabled = false; this.LinkButtonPrevious.ImageUrl = "~/Images/Buttons/rewind_grey.gif";
            this.LinkButtonNext.Enabled = true; this.LinkButtonNext.ImageUrl = "~/Images/Buttons/fast_forward.gif";
            this.LinkButtonLast.Enabled = true; this.LinkButtonLast.ImageUrl = "~/Images/Buttons/skip_forward.gif";
            currentPage = page;
        }
        else if (page >= pageCount)
        {
            this.LinkButtonFirst.Enabled = true; this.LinkButtonFirst.ImageUrl = "~/Images/Buttons/skip_backward.gif";
            this.LinkButtonPrevious.Enabled = true; this.LinkButtonPrevious.ImageUrl = "~/Images/Buttons/rewind.gif";
            this.LinkButtonNext.Enabled = false; this.LinkButtonNext.ImageUrl = "~/Images/Buttons/fast_forward_grey.gif";
            this.LinkButtonLast.Enabled = false; this.LinkButtonLast.ImageUrl = "~/Images/Buttons/skip_forward_grey.gif";
            currentPage = pageCount;
        }
        else
        {
            this.LinkButtonFirst.Enabled = true; this.LinkButtonFirst.ImageUrl = "~/Images/Buttons/skip_backward.gif";
            this.LinkButtonPrevious.Enabled = true; this.LinkButtonPrevious.ImageUrl = "~/Images/Buttons/rewind.gif";
            this.LinkButtonNext.Enabled = true; this.LinkButtonNext.ImageUrl = "~/Images/Buttons/fast_forward.gif";
            this.LinkButtonLast.Enabled = true; this.LinkButtonLast.ImageUrl = "~/Images/Buttons/skip_forward.gif";
        }
        if (pageCount == 0 || pageCount == 1)
        {
            this.LinkButtonFirst.Enabled = false; this.LinkButtonFirst.ImageUrl = "~/Images/Buttons/skip_backward_grey.gif";
            this.LinkButtonPrevious.Enabled = false; this.LinkButtonPrevious.ImageUrl = "~/Images/Buttons/rewind_grey.gif";
            this.LinkButtonNext.Enabled = false; this.LinkButtonNext.ImageUrl = "~/Images/Buttons/fast_forward_grey.gif";
            this.LinkButtonLast.Enabled = false; this.LinkButtonLast.ImageUrl = "~/Images/Buttons/skip_forward_grey.gif";
            currentPage = 1;
        }

        this.LabelRecord.Text = recorderCount.ToString();
        ViewState["CurrentPage"] = currentPage;
        this.TextBoxPage.Text = currentPage.ToString();
        this.LabelMessage.Text = pageCount.ToString();

    }
}