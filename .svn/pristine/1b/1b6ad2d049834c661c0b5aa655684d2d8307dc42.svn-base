<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirForeCast.aspx.cs" Inherits="Devoops_ForeCast_AirForeCast" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>空气质量预测</title>
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
    <link href="../../Demo/plugins/bootstrap/bootstrap.css" rel="stylesheet" />
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

        .table img {
            width: 30px;
            height: 30px;
        }
    </style>
    <script type="text/javascript">

        $(function () {

            $(window).resize(function () {
                $(".box").css({ left: $(window).width() - $(".box").width() - 8 + "px" });
                $(".box-content").css({ height: $(window).height() - 40 });
                $(".tabContainer").css({ height: $(".box").height() - 95 });
            });
            $(".box").css({ left: $(window).width() - $(".box").width() - 8 + "px" });
            $(".box-content").css({ height: $(window).height() - 40 });
            $(".tabContainer").css({ height: $(".box").height() - 95 });

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

        function getdata(d) {
            //debugger;
            $("#tdtime").html("");
            $(".tab tr:not(:first)").empty();
            $.ajax({
                url: "../../Data/GetAirForeCastData.ashx",
                dataType: "json",
                data: { flag: "new", num: d },
                success: function (result) {
                    //debugger;
                    if (result && result.DataTable && result.DataTable.length > 0) {
                        obj = result.DataTable;
                        $("#tdtime").append(result.DataTable[0]["PublicTime"].replace(" 00:00:00", ""));
                        for (var i = 0; i < result.DataTable.length; i++) {
                            var contents = $("<tr>" + "<td>" + result.DataTable[i]["StationName"] + "</td><td>"
                            //+ result.DataTable[i]["hour"] + "</td><td>"
                            + result.DataTable[i]["AQI"] + "</td><td>" + getdiv(result.DataTable[i]["AQI"]) + "&nbsp;"
                            + result.DataTable[i]["Category"] + "</td><td>" + result.DataTable[i]["FirstP"] + "</td>"
                            + "</tr>");
                            $(".tab").append(contents);

                        }
                        addPicGraphics(result.DataTable, 1);
                        addAirGraphics(result.DataTable, 2, callbackedFun);
                        $(".tab tr:gt(0)").each(function (index) {
                            $(this).click(function () {
                                $(".tab tr:gt(0)").css({ background: "#fff" });
                                $(this).css({ background: "#D2E27D" });
                                centerPoint(obj[index].lon, obj[index].lat);
                            });
                        });

                    } else {
                        $("#tdtime").append("（暂无数据）");
                        $(".tab").append($("<tr><td colspan='5' style='text-align:center;height:40px;'>暂无数据</td></tr>"));
                    }

                    function getdiv(value) {
                        var strDiv = "<div "
                        if (parseFloat(value) <= 50) strDiv += "class='aqi1'";
                        else if (parseFloat(value) <= 100) strDiv += "class='aqi2'";
                        else if (parseFloat(value) <= 150) strDiv += "class='aqi3'";
                        else if (parseFloat(value) <= 200) strDiv += "class='aqi4'";
                        else if (parseFloat(value) <= 300) strDiv += "class='aqi5'";
                        else if (parseFloat(value) > 300) strDiv += "class='aqi6'";
                        strDiv += "></div>";
                        return strDiv;
                    }
                }
            });
        }

        getdata(0);

        function callbackedFun(e) {
            var code = e.data.code;
            LoadData();
        }
        function LoadData() {
            var url = "AirForeCastData.aspx";

            $("#dialog").load(url, function (res) {
                console.log(res);
                $("#dialog").dialog({
                    autoOpen: false,
                    width: "100%",
                    title: "历史数据查询",
                    closeText: "关闭",
                    close: function () {
                        //location.reload();
                    },
                    height: document.body.clientHeight,
                    position: {
                        my: "right",
                        at: "top"
                    },
                    show: { effect: "blind", duration: 500 },
                    hide: { effect: "blind", duration: 500 }
                });
                $("#dialog").dialog("open");
            });
        }
    </script>
</head>
<body>
    <div class="box ui-draggable ui-droppable" style="border: 2px solid #6494BB; width: 336px; position: absolute; z-index: 0; left: 100px; top: 5px;">
        <div class="box-header" style="background: #6494BB;">
            <div class="box-name" style="color: #fff; font-size: 12px; font-weight: bold;">
                <i class="fa fa-bar-chart-o"></i>&nbsp;&nbsp;统计预报
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
                   
                    <tr>
                        <td style="width:60px;">发布日期：</td>
                        <td id="tdtime" style="text-align: left;"></td>
                        <td style="text-align: right; padding-right: 5px; text-decoration-style: none;">
                            <img src="../img/17759.gif" id="opener" style="cursor: pointer;" title="查看预测数据" onclick="LoadData(1)" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tabContainer" style="overflow: auto;">
                <table class="tab" cellspacing="0" cellpadding="0">
                    <tr>
                        <th>监测站点</th>
                        <th>AQI</th>
                        <th>质量状况</th>
                        <th>首要污染物</th>
                    </tr>

                </table>
            </div>
            <%--<div style="float: left;">
                <label class="btn btn-primary">
                    区域预测
                </label>
            </div>--%>
            <div class="btn-group" data-toggle="buttons" style="float: right;">
                <label class="btn btn-primary">
                    <input type="radio" name="options" id="option1" onclick="getdata(0)" />
                    今天
                </label>
                <label class="btn btn-primary">
                    <input type="radio" name="options" id="option2" onclick="getdata(1)" />
                    明天
                </label>
                <label class="btn btn-primary">
                    <input type="radio" name="options" id="option3" onclick="getdata(2)" />
                    后天
                </label>
            </div>
        </div>
    </div>
    <div id="flashContent" style="width: 100%; height: 100%;"></div>
    <div id="dialog" class="dialog"></div>

</body>
</html>
