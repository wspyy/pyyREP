<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirForeCastNum.aspx.cs" Inherits="Devoops_ForeCast_AirForeCast" %>

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


    <style>
        html, body
        {
            margin: 0px 0px 0px 0px;
            height: 100%;
            overflow: hidden;
            font-size: 12px;
        }

        .tab
        {
            word-break: keep-all;
            border: 1px solid #EBEBEB;
            border-bottom: none;
            border-left: none;
            border-top-left-radius: 6px;
            width: 100%;
            margin-top: 10px;
        }

            .tab td
            {
                border-bottom: 1px solid #EBEBEB;
                border-left: 1px solid #EBEBEB;
                padding: 5px;
                line-height: 25px;
                color: #41A7E2;
                cursor: pointer;
            }

            .tab th
            {
                background: #6494BB;
                border-left: 1px solid #EBEBEB;
                padding: 5px;
                line-height: 25px;
                border-bottom: 1px solid #EBEBEB;
                color: #fff;
            }

        .table img
        {
            width: 30px;
            height: 30px;
        }

        .indie
        {
            cursor:pointer;
            background-color: #456782;
            float: left;
            width: 100px;
            -moz-border-radius-topright: 30px;
            -moz-border-radius-topleft: 5px;
            -webkit-border-top-right-radius:30px;
            -webkit-border-top-left-radius: 5px;
        }

        .indie1
        {
            cursor:pointer;
            background-color: #929090;
            float: left;
            width: 100px;
            -moz-border-radius-topright: 30px;
            -moz-border-radius-topleft: 5px;
            -webkit-border-top-right-radius: 30px;
            -webkit-border-top-left-radius: 5px;
        }
        
        .dialog {
            padding:0px;
        }
    </style>
    <script type="text/javascript">
        var dayNum = 0;
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

            $("#cmax").addClass('indie');
            $("#wrf-charm").addClass('indie1');
            $("#jt").css({ "background-color": "#3276b1" });
        });

        function getdata(d,modeltype,hour) {
            //debugger;
            $("#tdtime").html("");
            $(".tab tr:not(:first)").empty();
            $.ajax({
                url: "../../Data/GetAirForeCastData.ashx",
                dataType: "json",
                data: { flag: "newnum", num: d, datatype: modeltype, hourtime: hour },
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
                        $("#tdtime").append("（暂无预测数据）");
                        $(".tab").append($("<tr><td colspan='5' style='text-align:center;'>暂无数据</td></tr>"));
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

        getdata(0,1,0);

        function seldatatype(datatype) {
            if (datatype == 1) {
                $("#cmax").removeClass("indie1");
                $("#cmax").addClass("indie");
                $("#wrf-charm").removeClass("indie");
                $("#wrf-charm").addClass("indie1");
                $("#datatype").val(1);
            }
            else {
                $("#cmax").removeClass("indie");
                $("#cmax").addClass("indie1");
                $("#wrf-charm").removeClass("indie1");
                $("#wrf-charm").addClass("indie");
                $("#datatype").val(2);
            }
            getdata($("#seldate").val(), $("#datatype").val(), $("#hourtime").val());
        }

        function selday(d) {
            if (d == 0) {
                $("#seldate").val(0);
                $("#jt").css({ "background-color": "#3276b1" });
                $("#mt").css({ "background-color": "#428bca" });
                $("#ht").css({ "background-color": "#428bca" });
            }
            else if (d == 1) {
                $("#seldate").val(1);
                $("#mt").css({ "background-color": "#3276b1" });
                $("#jt").css({ "background-color": "#428bca" });
                $("#ht").css({ "background-color": "#428bca" });
            }
            else {
                $("#seldate").val(2);
                $("#ht").css({ "background-color": "#3276b1" });
                $("#mt").css({ "background-color": "#428bca" });
                $("#jt").css({ "background-color": "#428bca" });
            }
            getdata($("#seldate").val(), $("#datatype").val(), $("#hourtime").val());
            dayNum = d;
        }

        function selhour() {
            $("#hourtime").get(0).selectedIndex = $("#hourtime").val();
            getdata($("#seldate").val(), $("#datatype").val(), $("#hourtime").val());
        }

        function callbackedFun(e) {
            var code = e.data.code;
            LoadData();
        }
        function LoadData() {
            var url = "AirForeCastNumDetail.aspx?forecastmodel="+$("#datatype").val();

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

        function ImgPlay()
        {
            var box = $('div.box');
            var button = $('i.fa-chevron-up');
            var content = box.find('div.box-content');
            content.slideToggle('fast');
            box.css({ "height": "auto" });
            button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
            addWidget();
        }

        function addWidget() {
            var date = new Date().addDays(dayNum).format("yyyyMMdd");
            //var pubdate = new Date().addDays(-1).format("yyyyMMdd");
            var pubdate = '20141213';
            var host = "localhost";//window.location.hostname;
            var poll = $("#pollItem").val();
            //alert(poll);
            var imgUrl = "http://" + host + "/JcData/AirForeCastData/CMAQ/" + pubdate + "20jc/Z_SEVP_C_SXJC_P_MSP3_JC_ENVAQFC_" + poll + "_AJC_L88_PB_" + pubdate + "20072";

            var pictureExtent = new HMap.Extent(111.8491648, 35.1608641, 113.7031752, 36.1074119);
            var picArr = new Array();
            for (var i = 0; i < 72; i++) {
                var num = i < 10 ? "0" + i : i;
                var picture = new HMap.PlayPic({
                    picId: "pic" + i,
                    picUrl: imgUrl + num + ".png",
                    //picInfo: i + "小时预报图片",
                    fadeInTime: 1000,
                    fadeOutTime: 1000,
                    pauseTime: 1000,
                    alphaMax: 0.8,
                    alphaMin: 0,
                    picExtent: pictureExtent
                });
                picArr.push(picture);
            }


            var widgetParam = new HMap.WidgetParam({ bottom: 10, right: 40 });
            playPicWidget = new HMap.PlayPicWidget(picArr, { timeUnit: "h", isReplay: true, widgetParam: widgetParam, widgetWisible: true });
            widget = hmap.addControl(playPicWidget);
            playPicWidget.play();
        }

    </script>
</head>
<body>
    <div class="box ui-draggable ui-droppable" style="border: 2px solid #6494BB; width: 336px; position: absolute; z-index: 0; left: 100px; top: 5px;">
        <div class="box-header" style="background: #6494BB;">
            <div class="box-name" style="color: #fff; font-size: 12px; font-weight: bold;">
                <i class="fa fa-bar-chart-o"></i>&nbsp;&nbsp;数值预报
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
            <div>
                <table cellpadding="0" cellspacing="0" style="line-height: 25px; color: #456782; width: 100%; padding-top: 10px;">
                    
                    <tr style="border-bottom: solid; border-width: 2px;">
                        <td >
                            <div style="float: left; color: white; font-weight: bold;text-align:center;">
                                <div id="cmax" onclick="seldatatype(1)">CMAQ</div>
                                <div id="wrf-charm" onclick="seldatatype(2)">WRF-Chem</div>
                                <input type="hidden" id="datatype" value="1" />
                            </div>
                        </td>
                         <td style="text-align:right;"> 
                           
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tabContainer" style="overflow: auto;">
                <div style="margin-top:5px; padding-left:10px; line-height:40px; height:40px;">
                    预测项：
                    <select id="pollItem">
                        <option value="AQI">AQI</option>
                        <option value="SO2">SO2</option>
                        <option value="NOX">NO2</option>
                        <option value="CO">CO</option>
                        <option value="O3">O3</option>
                        <option value="PM2.5">PM2.5</option>
                        <option value="PM10">PM10</option>
                    </select>
                    &nbsp;&nbsp;
                    时间：
                     <select id="hourtime" onchange="selhour()">
                                <option value="0" selected="selected">0</option>
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="4">4</option>
                                <option value="5">5</option>
                                <option value="6">6</option>
                                <option value="7">7</option>
                                <option value="8">8</option>
                                <option value="9">9</option>
                                <option value="10">10</option>
                                <option value="11">11</option>
                                <option value="12">12</option>
                                <option value="13">13</option>
                                <option value="14">14</option>
                                <option value="15">15</option>
                                <option value="16">16</option>
                                <option value="17">17</option>
                                <option value="18">18</option>
                                <option value="19">19</option>
                                <option value="20">20</option>
                                <option value="21">21</option>
                                <option value="22">22</option>
                                <option value="23">23</option>
                            </select>
                            时 <img src="../img/17759.gif" id="opener" style="cursor: pointer;  margin-left:20px;" title="查看预测数据" onclick="LoadData(1)" />
                </div>
                <table class="tab" cellspacing="0" cellpadding="0">
                    <tr>
                        <th>监测站点</th>
                        <th>AQI</th>
                        <th>质量状况</th>
                        <th>首要污染物</th>
                    </tr>

                </table>
            </div>
            <div style="float: left;display:none;">
                <input type="button" class="btn btn-success btn-label-left" value="区域预报" onclick="ImgPlay()" />
            </div>
            <div class="btn-group" data-toggle="buttons" style="float: right;">
                <label id="jt" class="btn btn-primary" onclick="selday(0)">
                    今天
                </label>
                <label id="mt" class="btn btn-primary" onclick="selday(1)">
                    明天
                </label>
                <label id="ht" class="btn btn-primary" onclick="selday(2)">
                    后天
                </label>
                <input type="hidden" id="seldate" value="0" />
            </div>
        </div>
    </div>
    <div id="flashContent" style="width: 100%; height: 100%;"></div>
    <div id="dialog" class="dialog"></div>

</body>
</html>
