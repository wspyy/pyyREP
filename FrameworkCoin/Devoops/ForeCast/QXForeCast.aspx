<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QXForeCast.aspx.cs" Inherits="Devoops_ForeCast_QXForeCast" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../plugins/jquery-ui/jquery-ui.min.css" rel="stylesheet" />
    <link href="../plugins/jquery-ui/jquery-ui.theme.css" rel="stylesheet" />
    <link href="../css/font-awesome.css" rel="stylesheet" />
    <link href="../css/font-awesome.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
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
    <link href="../../Demo/plugins/bootstrap/bootstrap.css" rel="stylesheet" />
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

        .table img {
            width: 30px;
            height: 30px;
        }

        .table th {
            text-align: center;
            height:40px;
            background-color:#edf0fe;
        }

        .table td {
            background-color:#edf0fe;
        }
    </style>
    <script type="text/javascript">
        $(function () {

            $(window).resize(function () {
                $(".box").css({ left: $(window).width() - $(".box").width() - 8 + "px" });
                $(".box-content").css({ height: $(window).height() - 50 });
                $(".tabContainer").css({ height: $(".box").height() - 120 });

            });
            $(".box").css({ left: $(window).width() - $(".box").width() - 8 + "px" });
            $(".box-content").css({ height: $(window).height() - 50 });
            $(".tabContainer").css({ height: $(".box").height() - 120 });

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

        });
        function LoadData(wtitle) {
            var url = "QXForeCastHist.aspx";
            var wtitle = "气象预报数据";
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
                    hide: { effect: "blind", duration: 500 }//explode
                });
                $('#txt_begin').datepicker({ showOn: "click" });

                $("#dialog").dialog("open");
            });
        }
        function getdata() {

            $.ajax({
                url: "../../Data/GetQxForeCastData.ashx",
                dataType: "json",
                success: function (result) {

                    if (result && result.DataTable && result.DataTable.length > 0) {
                        obj = result.DataTable;
                        //$("#tdtime").html(result.DataTable[0]["ForeCastTime"].replace("T", " "));
                        $("#tdtime").html(result.DataTable[0]["ForeCastTime"].toString().split('T')[0]);
                        for (var i = 0; i < result.DataTable.length; i++) {
                            var contents = $("<tr >"
                            + "<td style='text-align: center;'>" + result.DataTable[i]["StationName"] + "</td><td style='padding-left:50px;vertical-align:middle;'>"
                            + "<img src='../img/Weather/day/" + result.DataTable[i]["WeatherCode"] + ".png' alt='天气'/><img src='../img/Weather/night/" + result.DataTable[i]["WeatherCode"] + ".png'  alt='天气'/>&nbsp;&nbsp;&nbsp;" + result.DataTable[i]["WeatherName"] + "</td><td style='text-align: center;'>"
                            + result.DataTable[i]["MinTemp"] + " - " + result.DataTable[i]["MaxTemp"] + " ℃" + "</td>"
                            + "</tr>");
                            $(".table").append(contents);

                        }
                        addWeatherGraphics(result.DataTable, 1);
                        //addMeteorGraphics(result.DataTable, 2);
                        $(".table tr:gt(0)").each(function (index) {
                            $(this).click(function () {
                                $(".table tr:gt(0)").css({ background: "#fff" });
                                $(this).css({ background: "#D2E27D" });
                                centerPoint(obj[index].lon, obj[index].lat);
                            });
                        });

                    } else {
                        $(".tab").append($("<tr><td colspan='3' style='text-align:center; height:40px;'>暂无数据</td></tr>"));
                    }
                }
            });
        }

        getdata();
    </script>
</head>
<body>
    <div class="box ui-draggable ui-droppable" style="border: 2px solid #6494BB; width: 336px; position: absolute; z-index: 0; left: 100px; top: 5px;">
        <div class="box-header" style="background: #6494BB;">
            <div class="box-name" style="color: #fff; font-size: 12px; font-weight: bold;">
                <i class="fa fa-bar-chart-o"></i>&nbsp;&nbsp;气象预报
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
        <div class="box-content" style="overflow: auto; padding: 2px; background: #fff;">
            <div style="padding-left: 16px;">
                <table cellpadding="0" cellspacing="0" style="line-height: 25px; color: #456782; width: 100%; padding-top: 10px;">
                  
                    <tr style="font-size:16px; line-height:40px;">
                        <td style="width:90px;">发布日期：</td>
                        <td id="tdtime" style="text-align: left;"></td>
                        <td style="text-align: right; padding-right: 5px; text-decoration-style: none;">
                            <img src="../img/17759.gif" id="opener" style="cursor: pointer;" title="查看历史数据" onclick="LoadData(1)" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tabContainer" style="padding-top: 10px;">
                <table class="table" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th>区  域</th>
                            <th>天  气</th>
                            <th>温  度</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
    <div id="flashContent" style="width: 100%; height: 100%;"></div>
    <div id="dialog"></div>
</body>
</html>
