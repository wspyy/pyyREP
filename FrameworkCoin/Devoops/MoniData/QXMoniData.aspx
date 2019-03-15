<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QXMoniData.aspx.cs" Inherits="Devoops_MoniData_QXMoniData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>地图</title>
    <link href="../plugins/jquery-ui/jquery-ui.min.css" rel="stylesheet" />
    <link href="../plugins/jquery-ui/jquery-ui.theme.css" rel="stylesheet" />
    <link href="../css/font-awesome.css" rel="stylesheet" />
    <link href="../../Demo/plugins/jquery-ui-timepicker-addon/jquery-ui-timepicker-addon.css" rel="stylesheet" />
    <link href="../css/font-awesome.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../plugins/data-tables/DT_bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../../Demo/plugins/bootstrap/bootstrap.css" rel="stylesheet" />

    <script src="../js/jquery-1.7.2.js"></script>
    <script src="../js/jquery-2.1.0.min.js"></script>
    <script src="../plugins/jquery-ui/jquery-ui.js"></script>
    <script src="../../API-2.0/MapAPI.js"></script>
    <script src="../js/MapOperation.js"></script>
    <script src="../plugins/data-tables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="../js/devoops.js"></script>
    <script src="../js/method.js"></script>
    <script src="../../Controls/Highcharts-4.0.1/highcharts.js"></script>
    <script src="../plugins/jquery-ui-timepicker-addon/jquery-ui-timepicker-addon.js"></script>
    <script src="../../Demo/plugins/jquery-ui/i18n/jquery.ui.datepicker-zh-CN.min.js"></script>
   
    <style>
        html, body {
            margin: 0px 0px 0px 0px;
            height: 100%;
            overflow: hidden;
            font-size: 12px;
        }

        .tab {
            word-break: keep-all;
            border: 1px solid #EBEBEB;
            border-bottom: none;
            border-left: none;
            border-top-left-radius: 6px;
            width: 100%;
            margin-top: 10px;
        }

            .tab td {
                border-bottom: 1px solid #EBEBEB;
                border-left: 1px solid #EBEBEB;
                padding: 5px;
                line-height: 25px;
                color: #41A7E2;
                cursor: pointer;
            }

            .tab th {
                background: #6494BB;
                border-left: 1px solid #EBEBEB;
                padding: 5px;
                line-height: 25px;
                border-bottom: 1px solid #EBEBEB;
                color: #fff;
            }
    </style>
    <script>
        var obj;
        function LoadData(wtitle) {
            var url = "QXMoniHistData.aspx";
            $("#dialog").load(url, function () {

                $("#dialog").dialog({
                    autoOpen: false,
                    width: "100%",
                    title: wtitle,
                    closeText: "关闭",
                    height: document.body.clientHeight,
                    position: {
                        my: "right",
                        at: "top"
                    },
                    show: { effect: "blind", duration: 500 },
                    hide: { effect: "blind", duration: 500 }
                });
                $('#txt_begin').datepicker({ showOn: "click" });
                $('#txt_end').datepicker({ showOn: "click" });

                $("#dialog").dialog("open");
            });
        }
        $(function () {

            $("#opener").click(function () { LoadData("气象数据监测") });
            $("#tabs").tooltip();
            //设置Tab高度
            $("#tabs").tabs({ height: "auto" }).height($(window).height() - 50);
            $("div[id^=tabs-]").height($(window).height() - 100);
            $(window).resize(function () {
                $(".box").css({ left: $(window).width() - $(".box").width() - 8 + "px" });
                $(".box-content").css({ height: $(window).height() - 50 });
                $("#tabs").tabs({ height: "auto" }).height($(window).height() - 50);
                $("div[id^=tabs-]").height($(window).height() - 100);
            });
            $(".box").css({ left: $(window).width() - $(".box").width() - 8 + "px" });
            $(".box-content").css({ height: $(window).height() - 50 });

            $("body").on('click', '.collapse-link', function (e) {
                e.preventDefault();
                var box = $(this).closest('div.box');
                var button = $(this).find('i');
                var content = box.find('div.box-content');
                content.slideToggle('fast');
                box.css({ "height": "auto" });
                button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
                setTimeout(function () {
                    box.resize();
                    box.find('[id^=map-]').resize();
                }, 50);
            })
        .on('click', '.close-link', function (e) {
            e.preventDefault();
            var content = $(this).closest('div.box');
            content.remove();
        });
            InitData();
        });

        function InitData() {
            $("#tabDay").html("");
            $("#tabDay").append("<tr><th>测点名称</th><th>风向</th><th>风速<br/>m/s</th><th>风力</th><th>温度<br/>℃</th><th>湿度<br/>%</th></tr>");//<th>气压<br/>hPa</th><th>降水量<br/>mm</th>
            $.ajax({
                url: "../../Data/GetQxRealTimeData.ashx",
                dataType: "json",
                success: function (result) {
                    if (result && result.DataTable && result.DataTable.length > 0) {
                        obj = result.DataTable;
                        
                        var time = result.DataTable[0]["time"].replace("T"," ").substr(0,16);
                        //var formattime = observtime.substr(0, 4) + "-" + observtime.substr(4, 2) + "-" + observtime.substr(6, 2) + " " + observtime.substr(8, 2) + ":" + observtime.substr(10, 2);
                        $("#tdtime").html(time);
                        for (var i = 0; i < result.DataTable.length; i++) {
                            var contents = $("<tr >"
                              + "<td>" + result.DataTable[i]["cityname"] + "</td>"
                              + "<td>" + result.DataTable[i]["windDir"] + "</td><td>" + windSpeed(result.DataTable[i]["windPower"])
                              + "</td><td>" + result.DataTable[i]["windPower"] + "</td><td>" + result.DataTable[i]["temNow"] + "</td><td>" + result.DataTable[i]["humidity"] + "</td>"
                              + "</tr>");
                            $("#tabDay").append(contents);
                        }
                        addPicGraphics(result.DataTable, 2);
                        addMeteorGraphics(result.DataTable, 1);
                        $("#tabDay" + " tr:gt(0)").each(function (index) {
                            $(this).click(function () {
                                $("#tabDay" + " tr:gt(0)").css({ background: "#fff" });
                                $(this).css({ background: "#D2E27D" });
                                centerPoint(obj[index].lon, obj[index].lat);

                            });
                        });
                    } else {
                        $("#tabDay").append($("<tr><td colspan='6' style='text-align:center; height:40px;'>暂无数据</td></tr>"));
                    }
                }
            });
        }
    </script>
</head>
<body>


    <div class="box ui-draggable ui-droppable" style="border: 2px solid #6494BB; width: 336px; position: absolute; z-index: 0; left: 100px; top: 5px;">
        <div class="box-header" style="background: #6494BB;">
            <div class="box-name" style="color: #fff; font-size: 12px; font-weight: bold;">
                <i class="fa fa-bar-chart-o"></i>&nbsp;&nbsp;气象数据监测
            </div>

            <div class="box-icons">
                <a class="collapse-link" style="border: none;">
                    <i class="fa fa-chevron-up"></i>
                </a>

                <a class="close-link" style="border: none;">
                    <i class="fa fa-times"></i>
                </a>
            </div>
            <div class="no-move"></div>
        </div>
        <div class="box-content" style="overflow:hidden; padding: 2px; background: #fff;">
             <div style="padding-left: 16px;">
                        <table cellpadding="0" cellspacing="0" style="line-height: 30px; color: #456782; width: 100%;">
                            <tr>
                                <td style="width:60px;">监测时间：</td>
                                <td id="tdtime" style="text-align: left;"></td>
                                <td style="text-align: right; padding-right: 5px; text-decoration-style: none;">
                                    <img src="../img/17759.gif" id="opener" style="cursor: pointer;" title="查看历史数据" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table class="tab" id="tabDay" cellspacing="0" cellpadding="0">
                    </table>
           
        </div>
    </div>
    <div id="flashContent" style="width: 100%; height: 100%;"></div>
    <div id="dialog"></div>
</body>
</html>
