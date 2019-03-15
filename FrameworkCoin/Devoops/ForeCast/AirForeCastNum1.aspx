﻿<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>数值预报</title>
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
    <script src="../../Controls/My97DatePicker/WdatePicker.js"></script>
    <!---->

    <script src="imgPng.js"></script>
    <style>
        html, body, legend {
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

        .indie {
            cursor: pointer;
            background-color: #456782;
            float: left;
            width: 100px;
            -moz-border-radius-topright: 30px;
            -moz-border-radius-topleft: 5px;
            -webkit-border-top-right-radius: 30px;
            -webkit-border-top-left-radius: 5px;
        }

        .indie1 {
            cursor: pointer;
            background-color: #929090;
            float: left;
            width: 100px;
            -moz-border-radius-topright: 30px;
            -moz-border-radius-topleft: 5px;
            -webkit-border-top-right-radius: 30px;
            -webkit-border-top-left-radius: 5px;
        }

        .dialog {
            padding: 0px;
        }

        #selFactor td {
            border: none;
            color: #000;
        }
        .tab-link:hover {
            background-color:#accff5;
        }
        #picture {
            color:red;
        }
    </style>
    <script type="text/javascript">
        var dayNum = 0;
        var playPicWidget;
        var pollItem;
        $(function () {
            $('#txt_Time').val(new Date().addDays(-1).format("yyyy-MM-dd"));
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
            //$("#jt").css({ "background-color": "#3276b1" });            
            $("#buttonDate div:gt(0)").each(function (index) {
                $(this).click(function () {
                    $("#buttonDate div:gt(0)").css({ background: "#428bca" });
                    $(this).css({ background: "#3276b1" });
                });
            });

            //选择因子
            $('#selFactor').on('click', '.tab-link', function (e) {
                pollItem = this.id;
                addWidget(pollItem);
            });
            //改变因子颜色            
            $("#selFactor .tab-link:gt(0)").each(function (index) {
                $(this).click(function () {
                    $("#selFactor .tab-link:gt(0)").css({ background: "#fff" });
                    $(this).css({ background: "#1D79CE" });
                });
            });
            //点击播放按钮
            $("#imgPlay").on('click', function () {
                if (playPicWidget != null) {
                    playPicWidget.play();
                }
            });


            var date = new Date();//获取当前时间
            date.setHours(date.getHours() + 1);//当前时间加1小时
            var h = date.getHours();//得到小时值

            //$("#hourtime").val(h);//selected="selected"
            $("#hourtime option[value=" + h + "]").attr("selected", "selected");

            // $("#hourtime option[value='3']").attr("selected", true);
            if (h > 0) { getdata(0, 1, h); }
            if (h == 0) { getdata(1, 1, h); }
        });

        function getdata(d, modeltype, hour) {
            //debugger;
            $("#tdtime").html("");
            $("#Stationtab tr:not(:first)").empty();
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
                            $("#Stationtab").append(contents);

                        }
                        addPicGraphics(result.DataTable, 1);
                        addAirGraphics(result.DataTable, 2, callbackedFun);
                        $("#Stationtab tr:gt(0)").each(function (index) {
                            $(this).click(function () {
                                $("#Stationtab tr:gt(0)").css({ background: "#fff" });
                                $(this).css({ background: "#D2E27D" });
                                centerPoint(obj[index].lon, obj[index].lat);
                            });
                        });

                    } else {
                        $("#tdtime").append("（暂无预测数据）");
                        $("#Stationtab").append($("<tr><td colspan='5' style='text-align:center;'>暂无数据</td></tr>"));
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
            $("#seldate").val(d);
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
            var url = "AirForeCastNumDetail.aspx?forecastmodel=" + $("#datatype").val();

            $("#dialog").load(url, function (res) {
                console.log(res);
                $("#dialog").dialog({
                    autoOpen: false,
                    width: "100%",
                    title: "历史数据查询",
                    closeText: "关闭",
                    //close: function () {
                    //location.reload();
                    //},
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
            <!--站点预报-->
            <div id="Station">
                <div>
                    <table cellpadding="0" cellspacing="0" style="display: none; line-height: 25px; color: #456782; width: 100%; padding-top: 10px;">

                        <tr style="border-bottom: solid; border-width: 2px;">
                            <td>
                                <div style="float: left; color: white; font-weight: bold; text-align: center;">
                                    <div id="cmax" onclick="seldatatype(1)">CMAQ</div>
                                    <div id="wrf-charm" onclick="seldatatype(2)">WRF-Chem</div>
                                    <input type="hidden" id="datatype" value="1" />
                                </div>
                            </td>
                            <td style="text-align: right;"></td>
                        </tr>
                    </table>
                </div>
                <div class="tabContainer" style="overflow: auto; display: block;">
                    <h4 class="page-header" style="margin: 10px;">站点预报</h4>
                    <div style="margin-top: 5px; padding-left: 10px; line-height: 40px; height: 40px;">
                        <p style="float: left; font-size: medium;">选择小时：</p>
                        <select id="hourtime" onchange="selhour()"  class="form-control" style="float:left;width:80px;height:30px;margin-top:5px;">
                            <option value="0">0</option>
                            <option value="1" >1</option>
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
                        时
                    <img src="../img/17759.gif" id="opener"
                        style="cursor: pointer; float: right; margin-top: 10px; margin-right: 10px;" title="查看预测数据" onclick="LoadData(1)" />
                    </div>
                    <table class="tab" id="Stationtab" cellspacing="0" cellpadding="0">
                        <tr>
                            <th>监测站点</th>
                            <th>AQI</th>
                            <th>质量状况</th>
                            <th>首要污染物</th>
                        </tr>

                    </table>
                </div>
                <div class="btn-group" data-toggle="buttons" id="buttonDate" style="float: left;">
                    <div id="jt" class="btn btn-primary" onclick="selday(0)">
                        今天
                    </div>
                    <div id="mt" class="btn btn-primary" onclick="selday(1)">
                        明天
                    </div>
                    <div id="ht" class="btn btn-primary" onclick="selday(2)">
                        后天
                    </div>
                    <input type="hidden" id="seldate" value="0" />
                </div>
                <div style="float: right;">
                    <input type="button" class="btn btn-success btn-label-left" value="区域预报>>" onclick="ShowArea()" />
                </div>
            </div>
            <!--区域预报-->
            <div id="imgselect" style="background-color: #fff; display: none; width: 100%; position: relative;">
                <div>
                    <h4 class="page-header" style="margin: 10px;">区域预报</h4>
                    <div style="margin-top: 5px; padding-left: 10px; line-height: 40px; height: 40px;">
                        <p style="float: left; font-size: medium;">选择日期：</p>
                        <input type="text" class="form-control" id="txt_Time" onclick="WdatePicker({
    onpicked: function () {
        var time = $('#txt_Time').val();
        if (pollItem != null) {
            addWidget(pollItem);
        }
    }
})"
                            style="float: left; width: 120px;height:30px;margin-top:5px;" />
                         <img src="../img/17759.gif" id="opener1" style="cursor: pointer; float: right; margin-top: 10px; margin-right: 10px" title="查看预测数据" onclick="LoadData()" />
                    </div>
                    <table class="tab" cellspacing="0px" cellpadding="0" style="width: 100%; padding-top: 0px;">
                         
                        <tr>
                            <td colspan="2" style="width: 100%">
                                <table style="width: 100%; border: none;" id="selFactor">
                                    <tr style="width: 100%; border: none">
                                        <td style="width: 33%"><b>环境质量要素：</b>
                                        </td>
                                        <td style="width: 33%" class="tab-link"></td>
                                        <td style="width: 33%" class="tab-link" id="AQI">AQI</td>
                                    </tr>
                                    <tr style="width: 100%">
                                        <td style="width: 33%" class="tab-link" id="PM2">PM2.5</td>
                                        <td style="width: 33%" class="tab-link" id="PM10">PM10</td>
                                        <td style="width: 33%" class="tab-link" id="CO">CO</td>
                                    </tr>
                                    <tr style="width: 100%">
                                        <td style="width: 33%" class="tab-link" id="NOX">NO<sub>2</sub></td>
                                        <td style="width: 33%" class="tab-link" id="O3">O<sub>3</sub></td>
                                        <td style="width: 33%" class="tab-link" id="SO2">SO<sub>2</sub></td>
                                    </tr>
                                    <tr style="width: 100%">
                                        <td style="width: 33%"><b>气象要素：</b></td>
                                        <td style="width: 33%"></td>
                                        <td style="width: 33%"></td>
                                    </tr>
                                    <tr style="width: 100%">
                                        <td style="width: 33%" class="tab-link" id="VIS">VIS<br/>(能见度)</td>
                                        <td style="width: 33%" class="tab-link" id="FOG">FOG<br/>(雾等级)</td>
                                        <td style="width: 33%" class="tab-link" id="HAZE">HAZE<br/>(霾等级)</td>
                                    </tr>
                                    <tr style="width: 100%">
                                        <td style="width: 33%" class="tab-link" id="Rain6">Rain6<br/>(6小时降水)</td>
                                        <td style="width: 33%" class="tab-link" id="Rain24">Rain24<br/>(24小时降水)</td>
                                        <td style="width: 33%" class="tab-link" id="RH">RH<br/>(相对湿度)</td>
                                    </tr>
                                    <tr style="width: 100%">
                                        <td style="width: 33%" class="tab-link" id="2m">T2m<br/>(2米气温)</td>
                                        <td style="width: 33%" class="tab-link" id="WIND10">WIND10<br/>(10米风速)</td>
                                        <td style="width: 33%" class="tab-link" id="PBL">PBL<br/>(边界层高度)</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <button class="btn btn-primary btn-label-left" id="PreImg" type="button" onclick="PreImg()">
                                    上一张
                                </button>
                                <button class="btn btn-primary btn-label-left" id="imgPlay" type="button">
                                    <!--onclick="PlayImg()"-->
                                    播放
                                </button>
                                <button class="btn btn-primary btn-label-left" id="PauseImg" type="button" onclick="PauseImg()">
                                    暂停
                                </button>
                                <button class="btn btn-primary btn-label-left" id="btn_Stop" type="button" onclick="StopImg()">
                                    停止
                                </button>
                                <button class="btn btn-primary btn-label-left" id="NextImg" type="button" onclick="NextImg()">
                                    下一张
                                </button>
                            </td>
                        </tr>
                    </table>
                    <div style="float: right; position: fixed; bottom: 20px;width:320px;">
                        <input type="button" class="btn btn-success btn-label-left" value="<<站点预报" onclick="ShowSta()" />
                        <div id="picture" style="float:right;"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="flashContent" style="width: 100%; height: 100%;"></div>
    <div id="dialog" class="dialog"></div>
</body>
</html>
