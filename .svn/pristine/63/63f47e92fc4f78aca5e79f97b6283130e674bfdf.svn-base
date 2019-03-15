<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<!DOCTYPE html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>欢迎登录大气重污染天气预测预警系统平台</title>
    <script src="js/jquery-2.1.0.min.js"></script>
    <link href="css/font-awesome.css" rel="stylesheet" />
    <link href="css/Loginstyle.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <script src="js/cloud.js" type="text/javascript"></script>
    <script src="js/devoops.js"></script>
    <script language="javascript">
        $(function () {
            $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
            $(window).resize(function () {
                $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
            })
            $(".loginbtn").click(function () {
                var userName = $(".loginuser");
                var loginpwd = $(".loginpwd");
                if ($.trim(userName.val()) == "") {
                    $.ConfirmMess("提示", "请您输入密码", function () { alert(111);});
                   // OpenModalBox("提示", "请您输入用户名", $('<input  type="button" style="height:30px;" onclick="CloseModalBox()" class="btn btn-primary" value="确定"/>'));
                    return false;
                }
                if ($.trim(loginpwd.val()) == "") {
                    //OpenModalBox("提示", "请您输入密码", $('<input  type="button" style="height:30px;" onclick="CloseModalBox()" class="btn btn-primary" value="确定"/>'));
                    $.AlertMess("提示", "请您输入密码");
                    return false;
                }
                return true;
            });
        });
    </script>
</head>

<body style="background-color: #1c77ac; background-image: url(img/light.png); background-repeat: no-repeat; background-position: center top; overflow: hidden;">
    <form runat="server">
   
        <div id="mainBody">
            <div id="cloud1" class="cloud"></div>
            <div id="cloud2" class="cloud"></div>
        </div>
        <div class="loginbody">
            <span class="systemlogo"></span>
            <div class="loginbox">
                <span></span>
                <ul>
                    <li>
                    <asp:TextBox ID="txt_username" CssClass="loginuser"  Text="system" runat="server"></asp:TextBox>
                    <li>
                        <asp:TextBox ID="txt_pass" TextMode="Password" Text="system" CssClass="loginpwd" runat="server"></asp:TextBox>
                    <li>
                        <asp:Button ID="btn_login" CssClass="loginbtn" runat="server" Text="登录" OnClick="btn_login_Click" />
                      <label><input name="" type="checkbox" value="" checked="checked" />记住密码</label><label><a href="#">忘记密码？</a></label></li>
                </ul>
            </div>
        </div>
        <div class="loginbm"><a>技术支持：中科宇图天下科技有限公司</a></div>
    </form>
</body>
</html>

