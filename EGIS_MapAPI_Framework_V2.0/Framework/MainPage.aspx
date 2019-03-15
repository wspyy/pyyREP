<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="Framework_MainPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>中科宇图.环境GIS平台-小马地图API</title>
    <link href="../Controls/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../Controls/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            var url = "";
            var type = "";
            var flag = "";
            var mainUrl = window.location.href;
            var param = mainUrl.split("?")[1];


            if (param != undefined) {
                type = param.split("&")[0];

            }

            switch (type) {

                case "Example":     //调用示例
                    {
                        if (param.split("&")[1] != undefined) {
                            flag = param.split("&")[1];
                        }
                        else {
                            flag = "HelloWorld";    //默认值 
                        }
                        url = "../HorseMap/Example/TabControl.htm?treeId=" + flag;
                    }
                    break;

                case "Reference":   //类参考
                    {
                        if (param.split("&")[1] != undefined) {
                            flag = param.split("&")[1];
                        }
                        else {
                            flag = "Map";   //默认值
                        }
                        url = "../HorseMap/Reference/R_" + flag + ".mht";
                    }
                    break;
                case "Guide":       //开发指南
                    {
                        if (param.split("&")[1] != undefined) {
                            flag = param.split("&")[1];
                        }
                        else {
                            flag = "HelloWorld";    //默认值 
                        }
                        url = "../HorseMap/Guide/G_" + flag + ".mht";
                    }
                    break;
                case "Question":    //常见问题
                    {

                    }
                    break;
            }

            $("#iframePage").attr("src", url);
            var autoHeight = $(window).height() - 10;
            $("#iframePage").height(autoHeight);
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <iframe id="iframePage" width="100%" height="100%" frameborder="0" />
    </form>
</body>
</html>
