﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirHistData.aspx.cs" Inherits="Devoops_MoniData_AirHistData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <script>


        function getData() {
            $.ajax({
                url: "../../Data/GetAirMonitorData.ashx",
                data: { flag: "tabDay", date: $("#txt_begin").val() },
                dataType: "json",
                success: function (result) {                    
                    if (result && result.DataTable && result.DataTable.length > 0) {
                        obj = result.DataTable;
                        $("#tb_report tr:gt(2)").remove();
                        $('#container').show();

                        var StationNames = new Array();
                        var ReportData = new Array();
                        var SO2 = { name: "SO<sub>2</sub>", data: [] }
                        var NO2 = { name: "NO<sub>2</sub>", data: [] }
                        var CO = { name: "CO", data: [] }
                        var O3 = { name: "O<sub>3</sub>", data: [] }
                        var PM10 = { name: "PM<sub>10</sub>", data: [] }
                        var PM25 = { name: "PM<sub>25</sub>", data: [] }
                        
                        for (var i = 0; i < result.DataTable.length; i++) {
                            StationNames.push(result.DataTable[i]["StationName"] == '/////' ? 'null' : result.DataTable[i]["StationName"]);
                            SO2.data.push(result.DataTable[i]["SO2AQI"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["SO2AQI"]));
                            NO2.data.push(result.DataTable[i]["NO2AQI"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["NO2AQI"]));
                            CO.data.push(result.DataTable[i]["COAQI"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["COAQI"]));
                            O3.data.push(result.DataTable[i]["O3AQI"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["O3AQI"]));
                            PM10.data.push(result.DataTable[i]["PM10AQI"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["PM10AQI"]));
                            PM25.data.push(result.DataTable[i]["PM25AQI"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["PM25AQI"]));
                            var contents = $("<tr >"
                            + "<td>" + result.DataTable[i]["StationName"] + "</td>"
                            + "<td>" + result.DataTable[i]["SO2"] + "</td>"
                            + "<td>" + result.DataTable[i]["SO2AQI"] + "</td>"
                            + "<td>" + result.DataTable[i]["NO2"] + "</td>"
                            + "<td>" + result.DataTable[i]["NO2AQI"] + "</td>"
                            + "<td>" + result.DataTable[i]["CO"] + "</td>"
                            + "<td>" + result.DataTable[i]["COAQI"] + "</td>"
                            + "<td>" + result.DataTable[i]["O3"] + "</td>"
                            + "<td>" + result.DataTable[i]["O3AQI"] + "</td>"
                            + "<td>" + result.DataTable[i]["PM10"] + "</td>"
                            + "<td>" + result.DataTable[i]["PM10AQI"] + "</td>"
                            + "<td>" + result.DataTable[i]["PM25"] + "</td>"
                            + "<td>" + result.DataTable[i]["PM25AQI"] + "</td>"
                            + "<td>" + result.DataTable[i]["AQI"] + "</td>"
                            + "<td>" + result.DataTable[i]["FirstP"] + "</td>"
                            + "<td>" + result.DataTable[i]["QualityRome"] + "</td>"
                            + "<td>" + result.DataTable[i]["Category"] + "</td>"
                            + "<td>" + result.DataTable[i]["ColorName"] + "</td>"
                            + "</tr>");
                            $("#tb_report").append(contents);
                        }
                        ReportData.push(SO2 );
                        ReportData.push(NO2 );
                        ReportData.push(CO );
                        ReportData.push(O3);
                        ReportData.push(PM10);
                        ReportData.push(PM25);
                        $('#container').highcharts({
                            chart: {
                                type: 'column'
                            },
                            title: {
                                text: '空气质量情况统计图'
                            },
                            subtitle:{
                                text:$("#txt_begin").val()
                            },
                            xAxis: {
                                categories: StationNames
                            },
                            yAxis: {
                                min: 0,
                                title: {
                                    text: 'IAQI'
                                },
                                plotLines: [{
                                    color: 'red',
                                    width: 1,
                                    value: 100,
                                    id: 'plotline'
                                }]
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size: 10px;">{point.key}</span><table>',
                                pointFormat: '<tbody><tr><td style="padding: 0px;">{series.name}: </td>' +
                                    '<td style="padding: 0px;"><b>{point.y:.0f}</b></td></tr>',
                                footerFormat: '</tbody></table>',
                                shared: true,
                                useHTML: true
                            } ,
                            plotOptions: {
                                column: {
                                    pointPadding: 0.2,
                                    borderWidth: 0
                                }
                            },
                            series: ReportData
                        });
                    } else {
                        $("#tb_report tr:gt(2)").remove();
                        $("#tb_report").append($("<tr ><td colspan='18' style='text-align:center; height:40px;'>暂无数据</td></tr>"));
                        $('#container').hide();
                    }
                }
            });
        }

        $(function () {
            $("#container").width($(window).width() - 45);
            getData();
            $("#txt_begin").change(function () {
                getData();
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="background: #fff;">
            <table class="tab" id="tb_report" cellspacing="0" cellpadding="0" style="width: 100%;">
                <tr>
                    <th colspan="10" style="text-align: left; padding-left: 15px; border-right:none;">监测时间：  
                        <input type="text" runat="server" id="txt_begin" placeholder="Date" />
                    </th>
                    <th style="text-align:right;padding-right:10px; border-left:none; font-size:12px; font-weight:200; vertical-align:bottom;" colspan="8">
                        注：监测项CO浓度单位为mg/m³，除此均为ug/m³
                    </th>
                </tr>
                <tr>
                    <th rowspan="2">监测站点</th>
                    <th colspan="2">二氧化硫</th>
                    <th colspan="2">二氧化氮</th>
                    <th colspan="2">一氧化碳</th>
                    <th colspan="2">臭氧</th>
                    <th colspan="2">PM<sub>10</sub></th>
                    <th colspan="2">PM<sub>2.5</sub></th>
                    <th rowspan="2">空气质量<br/>指数(AQI)</th>
                    <th rowspan="2">首要污染物</th>
                    <th rowspan="2">空气质量<br/>指数级别</th>
                    <th rowspan="2">空气质量<br/>类别</th>
                    <th rowspan="2">颜色</th>

                </tr>
                <tr>
                    <th>浓度</th>
                    <th>分指数</th>
                    <th>浓度</th>
                    <th>分指数</th>
                    <th>浓度</th>
                    <th>分指数</th>
                    <th>浓度</th>
                    <th>分指数</th>
                    <th>浓度</th>
                    <th>分指数</th>
                    <th>浓度</th>
                    <th>分指数</th>
                </tr>
               
            </table>
            <div id="container" style="margin-bottom: 10px; border: 1px solid #cfc2c2"></div>
        </div>
    </form>
</body>
</html>
