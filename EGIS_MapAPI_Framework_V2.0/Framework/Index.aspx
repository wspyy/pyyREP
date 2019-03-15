<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="SingleTableManage_Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>中科宇图.环境GIS平台-小马地图API</title>
    <link href="../Controls/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Controls/zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../Controls/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../Controls/lhgdialog/lhgdialog/lhgdialog.min.js?self=true&skin=mac"
        type="text/javascript"></script>
    <script src="../Scripts/JScript.js" type="text/javascript"></script>
    <script src="javascript/Index.js" type="text/javascript"></script>
    <style>
        html, body
        {
            overflow: hidden;
        }
        .leftdiv
        {
            float: left;
            position: absolute;
            left: 0;
            top: 45px;
            width: 240px;
            border-right: 2px solid #999;
        }
        .rightdiv
        {
            float: left;
            position: absolute;
            left: 242px;
            top: 45px;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            Resize();
            setIframeUrl();
            $(window).resize(function () {
                Resize();
                setIframeUrl();
            });
        });


        //窗体修改后动态修改页面高度
        function Resize() {
            var autoHeight = $(window).height() - 50;
            var autoWidth = $(window).width() - 242;
            $(".leftdiv").height(autoHeight);
            $(".rightdiv").width(autoWidth);
            $(".rightdiv").height(autoHeight);
            $("#mainFrame").height(autoHeight);
            $("#indexFrame").height(autoHeight);

        }

        //设定内容窗口
        function SetMainFrame(url) {
            if (parent != null) {
                $('#mainFrame').attr('src', url);
                $('#hfMainFrameUrl').val(url);
            }
            else {
                parent.$('#mainFrame').attr('src', url);
                parent.$('#hfMainFrameUrl').val(url);
            }
        }

        //为Iframe赋值
        function setIframeUrl() {
            var mainUrl = window.location.href;

            var leftUrl = mainUrl.replace(/Index/, "SingleTableTree");
            var rightUrl = mainUrl.replace(/Index/, "MainPage");
//            debugger;
            $("#indexFrame").attr("src", leftUrl);
            $("#mainFrame").attr("src", rightUrl);
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container" style="width: 100%;">
                <a class="brand" style="margin-left: 5px;" href="../Index.aspx">中科宇图.环境GIS平台-小马地图API
                    <span class="label">BETA </span></a>
                <ul class="nav pull-right" style="marign-right: 40px;">
                    <li><a href="#">版权所有：中科宇图</a></li>
                </ul>
                <!--/.nav-collapse -->
            </div>
        </div>
    </div>
    <div class="leftdiv">
        <iframe frameborder="0" id="indexFrame" width="100%">
        </iframe>
    </div>
    <div class="rightdiv">
        <iframe id="mainFrame" width="100%" frameborder="0" scrolling="no">
        </iframe>
    </div>
    <asp:HiddenField ID="hfMessage" runat="server" />
    <asp:HiddenField ID="hfMainFrameUrl" runat="server" />
    </form>
</body>
</html>
