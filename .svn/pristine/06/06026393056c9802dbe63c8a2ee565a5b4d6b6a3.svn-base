<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>朔州市空气质量预报预警系统</title>
    <link href="css/Loginstyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/method.js"></script>
    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/cloud.js" ></script>
    <script type="text/javascript" src="js/pdremenber.js" ></script>
    <script type="text/javascript" src="js/devoops.js"></script>
    <script type="text/javascript" src="js/Sign-in.js"></script>

    <script type="text/javascript">
        $(function () {
            $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
            $(window).resize(function () {
                $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
            })
          //  VerifyUser();
        });
      

        onkeydown = function (event) {
            if (event.keyCode == 13) {
                VerifyUser();
            }
        }
    </script>

</head>

<body style="background-color: #1c77ac; background-image: url(images/light.png); background-repeat: no-repeat; background-position: center top; overflow: hidden;">
    <div id="mainBody">
        <div id="cloud1" class="cloud"></div>
        <div id="cloud2" class="cloud"></div>
    </div>
    <div class="logintop">
        <span>欢迎登录&nbsp;朔州市空气质量预报预警系统</span>
    </div>
    <div class="loginbody">
        <span class="systemlogo"></span>
        <div class="loginbox">
            <span></span>
            <ul>
                <li>
                    <input class="loginuser" type="text" autocomplete="off"
                        name="username" id="username" required /></li>
                <li>
                    <input class="loginpwd" type="password" autocomplete="off"
                        name="password" id="password" required /></li>
                <li>
                    <button type="button" onclick="VerifyUser();" class="loginbtn">
                        登 录
                    </button>
                    <label>
                        <input name="" id="pdcheck" type="checkbox" value="" checked="checked" />记住密码</label><%--<label><a href="#">忘记密码？</a></label>--%>
                </li>
                <li>
                    <p id="tip" style="color:red;display:none;"></p>
                </li>
            </ul>
        </div>
    </div>
    <div class="loginbm">技术支持：中科宇图天下科技有限公司&nbsp;&nbsp;&nbsp; 推荐浏览器：IE9+,Chrome,FireFox</div>
</body>

</html>

