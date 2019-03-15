<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebPager.ascx.cs" Inherits="Controls_WebPager" %>
<script type="text/javascript">
    function checkNum() {
        //输入页码时只能输入正整数
        if ((event.keyCode > 47 && event.keyCode < 58) || (event.keyCode > 95 && event.keyCode < 106) || (event.keyCode == 46) || (event.keyCode == 8)) {
            event.returnValue = true;
        }
        else {
            event.returnValue = false;
        }
    }
</script>
<!--table 添加 style="bottom:0px;position:fixed;" 使分页控件一直在页面最底部-->

        <td align="center" style="">
            <div class="pagination" style="text-align:center">
                <ul>
                    <li>
                        <asp:ImageButton ID="LinkButtonFirst" runat="server" OnClick="LinkButton_Click" CommandName="First"
                            ToolTip="首页" Width="16px" Height="16px" Enabled="False" ImageUrl="~/Images/Buttons/skip_backward.gif">
                        </asp:ImageButton></li>
                    <li>
                        <asp:ImageButton ID="LinkButtonPrevious" runat="server" OnClick="LinkButton_Click"
                            ToolTip="上一页" Width="16px" Height="16px" CommandName="Previous" Enabled="False"
                            ImageUrl="~/Images/Buttons/rewind.gif"></asp:ImageButton>
                    </li>
                    第
                    <asp:TextBox ID="TextBoxPage" runat="server" Width="20px"  SkinID="PagerField" Text="1"></asp:TextBox>
                    页&nbsp;共<asp:Label ID="LabelMessage" runat="server" Text="1"></asp:Label>页
                    <asp:ImageButton ID="LinkButtonGoto" runat="server" OnClick="LinkButton_Click" CommandName="Goto"
                        ImageUrl="~/Images/Buttons/go2.png" ToolTip="跳页"></asp:ImageButton>
                    <li>
                        <asp:ImageButton ID="LinkButtonNext" runat="server" OnClick="LinkButton_Click" Width="16px"
                            ToolTip="下一页" Height="16px" CommandName="Next" Enabled="False" ImageUrl="~/Images/Buttons/fast_forward.gif">
                        </asp:ImageButton>
                    </li>
                    <li>
                        <asp:ImageButton ID="LinkButtonLast" runat="server" OnClick="LinkButton_Click" Width="16px"
                            ToolTip="尾页" Height="16px" CommandName="Last" Enabled="False" ImageUrl="~/Images/Buttons/skip_forward.gif">
                        </asp:ImageButton>
                    </li>
                    共<asp:Label ID="LabelRecord" runat="server" Text="1"></asp:Label>条
                </ul>
            </div>

