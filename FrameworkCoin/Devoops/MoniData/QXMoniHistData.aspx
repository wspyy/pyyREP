﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QXMoniHistData.aspx.cs" Inherits="Devoops_MoniData_QXMoniHistData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>数据对比</title>
    <style>
        html, body {
            font-size: 12px;
        }
    </style>
    <script type="text/javascript">

        function getData() {
            var code = $("[name='station']:checked").val();
            var strtime = $("#txt_begin").val() +" "+ $("#ddl_day").val() +":00";
            var endtime = $("#txt_end").val() + " "+$("#ddl_endday").val() + ":00";
            $.ajax({
                url: "../../Data/GetQxRealTimeData.ashx",
                data: { flag: "HistData", StationCode: code, Strtime: strtime, Endtime: endtime },
                dataType: "json",
                success: function (result) {

                    if (result && result.DataTable && result.DataTable.length > 0) {
                        obj = result.DataTable;
                        $("#tb_report tr:gt(2)").remove();
                        $('#container').show();

                        var StationNames = new Array();
                        var ReportData = new Array();
                        //var MaxWindV = { name: "风速", data: [] }
                        var windPower = { name: "风力", data: [] };
                        var DryBulTemp = { name: "温度", data: [] };
                        var RelHumidity = { name: "湿度", data: [] }
                        //var StationPress = { name: "气压", data: [] }
                        //var PrecipitationAmount = { name: "降水量", data: [] }
                        for (var i = 0; i < result.DataTable.length; i++) {
                            //StationNames.push(result.DataTable[i]["ObservTimes"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["ObservTimes"]));
                            //MaxWindV.data.push(windSpeed(result.DataTable[i]["windPower"]));
                            //DryBulTemp.data.push(result.DataTable[i]["DryBulTemp"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["DryBulTemp"]));
                            //RelHumidity.data.push(result.DataTable[i]["RelHumidity"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["RelHumidity"]));
                            //StationPress.data.push(result.DataTable[i]["StationPress"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["StationPress"])/10);
                            //PrecipitationAmount.data.push(result.DataTable[i]["PrecipitationAmount"] == '/////' ? 'null' : parseFloat(result.DataTable[i]["PrecipitationAmount"]));

                            //StationNames.push(result.DataTable[i]["cityname"]);
                            windPower.data.push(parseInt(result.DataTable[i]["windPower"].substr(0,1)));
                            DryBulTemp.data.push(result.DataTable[i]["temNow"]);
                            var time = result.DataTable[i]["time"].replace("T"," ").substr(0,16);
                            RelHumidity.data.push(parseInt(result.DataTable[i]["humidity"]));
                            //var formattime = observtime.substr(0, 4) + "-" + observtime.substr(4, 2) + "-" + observtime.substr(6, 2) + " " + observtime.substr(8, 2) + ":" + observtime.substr(10, 2);
                           
                            StationNames.push(time);
                            var contents = $("<tr >"
                                + "<td>" + time + "</td>"
                                + "<td>" + result.DataTable[i]["windDir"] + "</td>"
                                + "<td>" + result.DataTable[i]["windPower"] + "</td>"
                                 + "<td>" + windSpeed(result.DataTable[i]["windPower"]) + "</td>"
                                + "<td>" + result.DataTable[i]["temNow"] + "℃" + "</td>"
                                + "<td>" + result.DataTable[i]["humidity"] + " </td></tr>");
                            $("#tb_report").append(contents);
                        }
                        ReportData.push(windPower);
                        ReportData.push(DryBulTemp);
                        ReportData.push(RelHumidity);
                       // ReportData.push(MaxWindV);
                        //ReportData.push(PrecipitationAmount);
                        $('#container').highcharts({
                            chart: {
                                type: 'column'
                            },
                            title: {
                                text: '气象数据监测分析图'
                            },

                            xAxis: {
                               
                                labels: { step: parseInt( result.DataTable.length/10)},
                                categories: StationNames
                            },
                            yAxis: {
                                min: 0,
                               
                                title: {
                                    text: '数值'
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
                                pointFormat: '<tbody><tr><td style="padding: 0px;">{series.name}: </td>' +
                                    '<td style="padding: 0px;"><b>{point.y:.0f}</b></td></tr>',
                                footerFormat: '</tbody></table>',
                                shared: true,
                                useHTML: true
                            },
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
                        $("#tb_report").append($("<tr ><td colspan='7' style='text-align:center;height:40px;'>暂无数据</td></tr>"));
                        $('#container').hide();


                    }
                }
            });
        }
        //获取气象站点
        function getSatationList() {
            $.ajax({
                url: "../../Data/GetBaseData.ashx",
                data: { flag: "QXStation" },
                dataType: "json",
                success: function (result) {
                    var content = "监测站点：";
                    if (result && result.DataTable && result.DataTable.length > 0) {
                        for (var i = 0; i < result.DataTable.length; i++) {
                            content += "<div class=\"radio-inline\"><label><input type=\"radio\" name=\"station\" "
                            //if (result.DataTable[i]["StationCode"] == "B8529") content += " checked='checked' ";
                            //content += "value=\""
                            //+ result.DataTable[i]["StationCode"] + "\">"
                            //+ result.DataTable[i]["StationName"]
                            //+ "<i class=\"fa fa-circle-o small\"></i></label></div>"
                            if (i == 0) content += " checked='checked' ";                               
                                content += "value=\""
                                + result.DataTable[i]["cityname"] + "\">"
                                + result.DataTable[i]["cityname"]
                                + "<i class=\"fa fa-circle-o small\"></i></label></div>"
                            
                        }
                        $("#stationPanel").empty();
                        $("#stationPanel").append(content);
                        //$("[name='station']").change(changeData);

                        getData();
                    }
                }
            });
        }

        $(function () {
            $("#container").width($(window).width() - 45);

            getSatationList();
            
            $("#stationPanel").change(function () {
                getData();
            });
            $("#txt_begin").change(function () {
                getData();
            });
            $("#ddl_day").change(function () {
                getData();
            });
            //$("#ddl_hour").change(function () {
            //    getData();
            //});
            $("#txt_end").change(function () {
                getData();
            });
            $("#ddl_endday").change(function () {
                getData();
            });
            //$("#ddl_endhour").change(function () {
            //    getData();
            //});
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="background: #fff;">
            <table class="tab" id="tb_report" cellspacing="0" cellpadding="0" style="width: 100%;">
                <tr>
                    <td id="stationPanel" colspan="18" style="text-align: left; padding-left: 15px;">监测站点：</td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: left; padding-left: 15px;">监测时间：  
                        <input type="text" style="height: 25px; color:black; line-height:25px; vertical-align:middle;width:100px;" runat="server" id="txt_begin" placeholder="请选择日期" />
                        <asp:DropDownList Style="height: 25px; color: black; vertical-align:middle;" runat="server" ID="ddl_day">
                        </asp:DropDownList>
                   <%--     <asp:DropDownList Style="height: 25px; color: black; vertical-align:middle;" runat="server" ID="ddl_hour">
                        </asp:DropDownList>--%>至：
                        <input type="text" style="height: 25px; color: black; line-height:25px; vertical-align:middle;width:100px;" runat="server" id="txt_end" placeholder="请选择日期" />
                        <asp:DropDownList Style="height: 25px; color: black; vertical-align:middle;" runat="server" ID="ddl_endday">
                        </asp:DropDownList>
                   <%--     <asp:DropDownList Style="height: 25px; color: black; vertical-align:middle;" runat="server" ID="ddl_endhour">
                        </asp:DropDownList>--%>
                    </td>
                </tr>
                <tr>
                    <th>监测时间</th>
                    <th>风向</th>
                    <th>风力</th>
                     <th>风速 m/s</th>
                    <th>温度 ℃</th>
                    <th>湿度 %</th>
                   <%-- <th>气压</th>
                    <th>降水量</th>--%>
                </tr>
            </table>
            <div id="container" style="margin-bottom: 10px; border: 1px solid #cfc2c2"></div>
        </div>
    </form>

</body>
</html>
