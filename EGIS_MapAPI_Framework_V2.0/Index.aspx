<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="description" content="动态配置、单表采集、Coin">
    <meta name="author" content="Coin">
    <title>中科宇图.环境GIS平台-小马地图API</title>
    <link href="Controls/bootstrap-3.0.1-dist/css/bootstrap.min.css" rel="stylesheet"
        type="text/css" />
    <link href="CSS/sticky-footer-navbar_css.css" rel="stylesheet"
        type="text/css" />
</head>
<body>
    <!-- 头-->
    <div class="navbar navbar-inverse navbar-fixed-top" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="sr-only">Toggle navigation</span> <span class="icon-bar"></span><span
                        class="icon-bar"></span><span class="icon-bar"></span>
                </button>
                <%-- <a class="navbar-brand" href="../Default.aspx">中科宇图.环境GIS平台-小马地图API<span class="label"> BETA </span></a>--%>
                <a class="navbar-brand" href="#">中科宇图.环境GIS平台-小马地图API&nbsp;&nbsp;<span class="label"
                    style="background-color: #999999;"> BETA </span></a>
            </div>
            <div class="navbar-collapse collapse">
                <div id="divSuccess" class="navbar-right" style="display: none">
                    <!-- BEGIN USER LOGIN DROPDOWN -->
                    <li class="dropdown"><a href="#" class="navbar-brand" data-toggle="dropdown"><span
                        class="glyphicon glyphicon-user">Admin</span></a>
                        <ul class="dropdown-menu">
                            <%-- <li><a href="extra_profile.html"><i class="icon-user"></i>My Profile</a></li>
                            <li><a href="page_calendar.html"><i class="icon-calendar"></i>My Calendar</a></li>
                            <li><a href="inbox.html"><i class="icon-envelope"></i>My Inbox(3)</a></li>
                            <li><a href="#"><i class="icon-tasks"></i>My Tasks</a></li>
                            <li class="divider"></li>
                            <li><a href="extra_lock.html"><i class="icon-lock"></i>Lock Screen</a></li>--%>
                            <li class="divider"></li>
                            <li><a href="Index.aspx" onclick="logout()"><i class="icon-key"></i>Log Out</a></li>
                        </ul>
                    </li>
                    <!-- END USER LOGIN DROPDOWN -->
                </div>
                <form class="navbar-form navbar-right" runat="server">
                <div id="divSignIn" style="display: none">
                    <div class="form-group">
                        <input type="text" placeholder="用户名" class="form-control" id="login-name" />
                    </div>
                    <div class="form-group">
                        <input type="password" placeholder="密码" class="form-control" id="login-pass" />
                    </div>
                    <button type="button" class="btn btn-success" onclick="loginClick();">
                        登 录</button>
                </div>
                <asp:HiddenField ID="hfUserName" runat="server" />
                </form>
            </div>
        </div>
    </div>
    <!-- 整个系统介绍 -->
    <div class="jumbotron">
        <div class="container">
            <br />
            <h1>
                小马地图 API</h1>
            <p>
              &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
              &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; 
              &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; 
              &nbsp; &nbsp;&nbsp;&nbsp;
              —— 更高、更远、更好的为大家服务</p>
            <p />
            <p>
                <a class="btn btn-primary btn-lg" role="button" target="_blank" href="Framework/Index.aspx?Example">
                    调用示例 &raquo;</a></p>
        </div>
    </div>
    <!-- /头-->
    <div class="container">
        <!-- 分系统 -->
        <table style="width: 100%">
            <tr>
                <td>
                    <div class="col-md-4" style="width: 100%;">
                        <h2>
                            开发指南</h2>
                        <p>
                            使用小马地图API开发详解。</p>
                        <p>
                            <a class="btn btn-default" role="button" target="_blank" href="Framework/Index.aspx?Guide">
                                查看 &raquo;</a></p>
                    </div>
                </td>
                <td>
                    <div class="col-md-4" style="width: 100%;">
                        <h2>
                            类参考</h2>
                        <p>
                            地图参数详细说明。</p>
                        <p>
                            <a class="btn btn-default" role="button" target="_blank" href="Framework/Index.aspx?Reference">
                                查看 &raquo;</a></p>
                    </div>
                </td>
                <td>
                    <div class="col-md-4" style="width: 100%;">
                        <h2>
                            更新日志</h2>
                        <p>
                            记录点滴，成长看得见。</p>
                        <p>
                            <a class="btn btn-default" role="button" target="_blank" href="HorseMap/UpdateLog/UpdateLog.htm">
                                查看 &raquo;</a></p>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="col-md-4" style="width: 100%;" >
                        <h2>
                            常见问题</h2>
                        <p>
                            解答小马地图使用中常见的问题。</p>
                        <p>
                            <a class="btn btn-default" role="button" target="_blank" href="HorseMap/Question/Question.htm">
                                查看 &raquo;</a></p>
                    </div>
                </td>
                <td>
                    <div class="col-md-4" style="width: 100%;" >
                        <h2>
                            相关下载</h2>
                        <p>
                            提供开发指南和类参考的文档下载。</p>
                        <p>
                            <a class="btn btn-default" role="button" target="_blank" href="HorseMap/DownLoad/DownClassPackage.aspx">
                                查看 &raquo;</a></p>
                    </div>
                </td>
                <td>
                    <div class="col-md-4" style="display: none; width: 100%;" id="licenseId">
                        <h2>
                            许可申请</h2>
                        <p>
                            提供许可文件。</p>
                        <p>
                            <a class="btn btn-default" role="button" target="_blank" href="HorseMap/Lisence/LisenceApply.aspx">
                                申请 &raquo;</a></p>
                    </div>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <hr />
    <footer class="footer">
        <p class="pull-right"><a href="#">返回顶部</a></p>
        <p>&copy; 2013 China Sciences MapUniverse Technology Co.,Ltd &middot; </p>
        <p>（本框架仅适用IE浏览器，推荐使用IE9版本)</p>
      </footer>
    </div>
    <!-- 需要加载的脚本 -->
    <script src="Scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="Controls/bootstrap-3.0.1-dist/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="Controls/lhgdialog/lhgdialog/lhgdialog.min.js?self=true&skin=iblue"
        type="text/javascript"></script>
    <script src="Scripts/JScript.js" type="text/javascript"></script>
    <script type='text/javascript'>

        function loginClick() {

            var userID = $("#login-name").val();
            var loginPwd = $("#login-pass").val();
            var data = { name: userID, password: loginPwd };

            $.post("Data/UserLogin.ashx", data,
                function (msg) {
                    if (msg == "未连接上数据库！" || msg == "该用户名不存在！" || msg == "用户名或密码不正确！") {
                        ShowLittleBoxMessage('提示', '<br/>' + msg + '<br/><br/>');
                    }
                    else {
                        ShowLittleBoxMessage('提示', '<br/>登录成功！<br/><br/>');
                        $("#aUser").empty().append(msg);
                        $("#divSignIn").hide();
                        $("#divSuccess").show();
                        $("#licenseId").show();
                    }
                });
        }
        function logout() {
            $.post("Data/UserLogin.ashx", null,
                function (msg) {
                    $("#hfUserName").val("");
                    $("#divSignIn").show();
                    $("#divSuccess").hide();
                    $("#licenseId").hide();
                });
        }
        function ShowUserInfo() {
            ShowDataFrame('Framework/RightsManagement/UserInformation.aspx', '600px', '300px', '用户信息维护', true);
        }
        $(document).ready(function () {
            if ($("#hfUserName").val() != '') {
                $("#aUser").append($("#hfUserName").val());
                $("#divSuccess").show();
                $("#licenseId").show();
            }
            else {
                $("#divSignIn").show();
            }
        });
    </script>
</body>
</html>
