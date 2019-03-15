<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirDataContrast.aspx.cs" Inherits="Devoops_ForeCast_AirDataContrast" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>数据对比</title>
    <link href="../plugins/jquery-ui/jquery-ui.min.css" rel="stylesheet" />
    <link href="../plugins/jquery-ui/jquery-ui.theme.css" rel="stylesheet" />
    <link href="../../Demo/plugins/bootstrap/bootstrap.css" rel="stylesheet" />
    <link href="../../Controls/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" />
    <link href="../css/font-awesome.css" rel="stylesheet" />
    <link href="../../Demo/plugins/jquery-ui-timepicker-addon/jquery-ui-timepicker-addon.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../plugins/data-tables/DT_bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.2.js"></script>
    <script src="../js/jquery-2.1.0.min.js"></script>
    <script src="../plugins/jquery-ui/jquery-ui.js"></script>
    <script src="../plugins/data-tables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="../js/devoops.js"></script>
    <script src="../js/method.js"></script>
    <script src="../../Controls/Highcharts-4.0.1/highcharts.js"></script>
    <script src="../plugins/jquery-ui-timepicker-addon/jquery-ui-timepicker-addon.js"></script>
    <script src="../../Demo/plugins/jquery-ui/i18n/jquery.ui.datepicker-zh-CN.min.js"></script>
    <script src="../../Controls/My97DatePicker/WdatePicker.js"></script>
    <style>
        html, body {
            margin: 0px 0px 0px 0px;
            height: 100%;
            overflow: hidden;
            font-size: 13px;
        }
        .radiolabel {
            margin-left:-8px;
        }
    </style>
    <script type="text/javascript">

        $(function change() {

            $("#txt_begin").unbind();
            $("#txt_end").unbind();
            var myDate = new Date();
            var month = 0;
            var day = 0;
            if ((parseInt(myDate.getMonth()) + 1) > 9) {
                month = parseInt(myDate.getMonth()) + 1;

            }
            else {
                month = "0" + (parseInt(myDate.getMonth()) + 1);
            }
            if (myDate.getDate() > 9) {
                day = myDate.getDate();
            }
            else {
                day = "0" + myDate.getDate();
            }
            $("#txt_begin").val(myDate.getFullYear() + '-' + month + '-' + 1 + ' 00');
            $("#txt_end").val(myDate.getFullYear() + '-' + month + '-' + day + ' 00');
            $("#txt_begin").bind("focus", function () { WdatePicker({ skin: 'whyGreen', dateFmt: 'yyyy-MM-dd HH' }); });
            $("#txt_end").bind("focus", function () { WdatePicker({ skin: 'whyGreen', dateFmt: 'yyyy-MM-dd HH' }); });
            $("#txt_begin").change(changeData());
            $("#txt_end").change(changeData());
        })

    
        var selected;

        function getData(stationcode, poll, strtime, endtime, datatype) {
            var ylable;
            debugger;
            if (poll == "AQI") ylable = "AQI";
            else ylable = poll+"IAQI";
            $.ajax({
                url: "../../Data/GetAirForeCastData.ashx",
                data: { flag: "four", StationCode: stationcode, PollItem: poll, Strtime: strtime, Endtime: endtime, datatype: datatype },
                dataType: "json",
                success: function (result) {
                    
                    if (result && result.Table && result.Table.length > 0) {
                        obj = result.Table;
                        $('#container').show();
                        var StationNames = new Array();
                        var ReportData = new Array();
                        var Sdata = { name: "实际监测", data: [] };
                        var Tdata = { name: "统计预报", data: [] };
                        var Cdata = { name: "数值预报", data: [] };//CMAQ
                        //var Wdata = { name: "WRF-Chem", data: [] }
                        for (var i = 0; i < result.Table.length; i++) {
                            //StationNames.push(result.DataTable[i]["MonitorTime"].replace(":00:00", "").replace("T", " "));
                            StationNames.push(result.Table[i]["time"]);
                            Sdata.data.push(result.Table[i]["SAQI"]);
                            Tdata.data.push(result.Table[i]["TAQI"]);
                            Cdata.data.push(result.Table[i]["CAQI"]);
                            //Wdata.data.push(result.Table[i]["WAQI"]);

                        }
                        ReportData.push(Sdata);
                        if (document.getElementById("DayhourID").selectedIndex == 0)
                        {

                            ReportData.push(Tdata);
                            ReportData.push(Cdata);
                            //ReportData.push(Wdata);
                        }
                        else if (document.getElementById("DayhourID").selectedIndex == 1) {

                            ReportData.push(Cdata);
                            //ReportData.push(Wdata);
                        }
                        
                        $('#container').highcharts({
                            chart: {
                                type: 'spline'
                            },
                            colors: ['#6394BB', '#FFAA25', '#ED561B', '#058DC7', '#50B432', '#64E572', '#FF9655', '#FFF263', '#6AF9C4'],
                            title: {
                                text: '预测与实测数据(' + poll + ')对比分析图'
                            },
                            xAxis: {
                                type: 'string',
                                categories: StationNames,
                                labels: {
                                    rotation: -15
                                }
                            },
                            yAxis: {
                                title: {
                                    text: ylable
                                },
                                plotLines: [{
                                    color: 'red',
                                    width: 1,
                                    value: 150,
                                    id: 'plotline'
                                }]
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size: 10px;">{point.key}</span><table>',
                                pointFormat: '<tbody><tr><td style="color:{series.color};padding: 0px;">{series.name}: </td>' +
                                    '<td style="padding: 0px;"><b>{point.y:.0f}</b></td></tr>',
                                footerFormat: '</tbody></table>',
                                shared: true,
                                useHTML: true
                            },
                            plotOptions: {
                                column: {
                                    pointPadding: 0,
                                    borderWidth: 0
                                }
                            },
                            series: ReportData
                        });
                    }
                    else {

                        $('#container').html("<div style='text-align:center;font-weight:700;font-size:large;'>暂无数据...</div>");
                    }
                }
            });
        }

        function getSatationList() {
            $.ajax({
                url: "../../Data/GetBaseData.ashx",
                data: { flag: "AirStation" },
                dataType: "json",
                success: function (result) {
                    var content = "";
                    if (result && result.DataTable && result.DataTable.length > 0) {
                        for (var i = 0; i < result.DataTable.length; i++) {
                            content += "<div class=\"radio-inline\"><label class='radiolabel'><input type=\"radio\" name=\"station\" "
                            if (result.DataTable[i]["StationCode"] == 0) content += " checked='checked' ";
                            content += "value=\""
                            + result.DataTable[i]["StationCode"] + "\">"
                            + result.DataTable[i]["StationName"]
                            + "<i class=\"fa fa-circle-o small\"></i></label></div>"
                        }
                        $("#stationPanel").append(content);
                        $("[name='station']").change(changeData);
                        $("[name='poll']").change(changeData);

                        changeData();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                    //alert(XMLHttpRequest.status);
                    //alert(XMLHttpRequest.readyState);
                    //alert(textStatus);
                }
            });
        }

        $(function () {
            getSatationList();
        });

        function changeData() {
            var code = $("[name='station']:checked").val();
            var poll = $("[name='poll']:checked").val();
            var strtime = $("#txt_begin").val();
            var endtime = $("#txt_end").val();
            var selectIndexs = document.getElementById("DayhourID").selectedIndex;//获得是第几个被选中了

            var datatype = document.getElementById("DayhourID").options[selectIndexs].value;//获得被选中的项目的文本

            getData(code, poll, strtime, endtime, datatype);


        }

        function changeModel() {
            debugger;
            var myDate = new Date();
            var month = 0;
            var day = 0;
            if ((parseInt(myDate.getMonth()) + 1) > 9) {
                month = parseInt(myDate.getMonth()) + 1;

            }
            else {
                month = "0" + (parseInt(myDate.getMonth()) + 1);
            }
            if (myDate.getDate() > 9) {
                day = myDate.getDate();
            }
            else {
                day = "0" + myDate.getDate();
            }
            if (document.getElementById("DayhourID").selectedIndex == 0) {
                $("#txt_begin").val(myDate.getFullYear() + '-' + month + '-0' + 1 + ' 00');
                $("#txt_end").val(myDate.getFullYear() + '-' + month + '-' + day + ' 00');
            }
            else {
                $("#txt_begin").val(myDate.getFullYear() + '-' + month + '-' + day + ' 00');
                $("#txt_end").val(myDate.getFullYear() + '-' + month + '-' + day + ' ' + myDate.getHours());
            }

            $("#txt_begin").bind("focus", function () { WdatePicker({ skin: 'whyGreen', dateFmt: 'yyyy-MM-dd HH' }); });
            $("#txt_end").bind("focus", function () { WdatePicker({ skin: 'whyGreen', dateFmt: 'yyyy-MM-dd HH' }); });
            changeData();
        }
    </script>
</head>
<body style="background: #EBEBEB;overflow:auto;">
    <form id="Form1" runat="server">
        <div class="box" style="">
            <div class="box-header">
                <div class="box-name">
                    <i class="fa fa-table"></i>
                    <span>多模式预测与实测对比分析</span>
                </div>
                <%--<div class="box-icons">
                    <a class="collapse-link">
                        <i class="fa fa-chevron-up"></i>
                    </a>
                    <a class="expand-link">
                        <i class="fa fa-expand"></i>
                    </a>
                    <a class="close-link">
                        <i class="fa fa-times"></i>
                    </a>
                </div>--%>
                <div class="no-move"></div>
            </div>
            <div class="box-content" style="height: 100px;">
                <div>
                    <div id="stationPanel" style="width: 100%; float: left">
                        <label class="control-label">监测站点：</label>
                    </div>

                </div>
                <div>
                    <div style="width: 567px; float: left;">
                        <label class="control-label">预测项目：</label>
                        <div class="radio-inline">
                            <label class="radiolabel">
                                <input type="radio" name="poll" value="AQI" onselect="true" checked="checked"><i class="fa fa-circle-o small"></i>AQI
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class="radiolabel">
                                <input type="radio" name="poll" value="SO2"><i class="fa fa-circle-o small"></i>SO<sub>2</sub>
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class="radiolabel">
                                <input type="radio" name="poll" value="NO2"><i class="fa fa-circle-o small"></i>NO<sub>2</sub>
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class="radiolabel">
                                <input type="radio" name="poll" value="PM10"><i class="fa fa-circle-o small"></i>PM10
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class="radiolabel">
                                <input type="radio" name="poll" value="PM2.5"><i class="fa fa-circle-o small"></i>PM2.5
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class="radiolabel">
                                <input type="radio" name="poll" value="CO"><i class="fa fa-circle-o small"></i>CO
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class="radiolabel">
                                <input type="radio" name="poll" value="O3"><i class="fa fa-circle-o small"></i>O<sub>3</sub>
                            </label>
                        </div>
                    </div>

                </div>

                <div>
                    <div style="width: 100%;float:left;">
                        <label class="control-label">预报模式：</label>
                        <select id="DayhourID" name="DayhourID" style="font-size: 14px" onchange="changeModel()">
                            <option id="dayID" value="0" selected="selected">日数据</option>
                            <option id="hourID" value="1">小时数据</option>
                        </select>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <label class="control-label">开始时间：</label>
                        <asp:TextBox ID="txt_begin" runat="server" CssClass="Wdate" onchange="changeData()" Width="110px"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <label class="control-label">结束时间：</label>
                        <asp:TextBox ID="txt_end" runat="server" CssClass="Wdate" onchange="changeData()" Width="110px"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="button" class="btn btn-primary"  style="top:-5px;"
                            value="分析" onclick="changeData()" />
                    </div>

                </div>
            </div>
        </div>
        <div id="container" style="margin-bottom: 10px; border: 1px solid #cfc2c2; width: 100%; height: 78%;background-color:white;"></div>
    </form>
    <script type="text/javascript">

        $("#DayhourID").change(function () {
            changeData();
        })
        $(document).ready(function () {

            changeData();
        });

    </script>
</body>
</html>
