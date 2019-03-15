<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirForeCastContrast.aspx.cs" Inherits="Devoops_ForeCast_AirDataContrast" %>

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
            margin-left: -8px;
        }

        .dtable {
            margin-top: 5px;
            border: 1px solid #ddd;
            border-collapse: separate;
            border-radius: 4px;
        }

            .dtable th {
                border-left: 1px solid #ddd;
                font-weight: bold;
                height: 35px;
                text-align: center;
            }

            .dtable td {
                border-left: 1px solid #ddd;
                border-top: 1px solid #ddd;
                height: 30px;
                text-align: center;
            }
    </style>


</head>
<body style="background: #EBEBEB; overflow: auto;">
    <form id="Form1" runat="server">
        <div class="box ui-draggable ui-droppable" style="margin: 10px;">
            <div class="box-header">
                <div class="box-name">
                    <i class="fa fa-table"></i>
                    <span>预报评估</span>
                </div>
                <div class="box-icons">
                    <a class="collapse-link">
                        <i class="fa fa-chevron-up"></i>
                    </a>
                    <a class="expand-link">
                        <i class="fa fa-expand"></i>
                    </a>
                    <a class="close-link">
                        <i class="fa fa-times"></i>
                    </a>
                </div>
                <div class="no-move"></div>
            </div>
            <div class="box-content" style="height: 100px;">
                
                <div id="stationPanel" style="width: 100%; float: left">
                        <label class="control-label" style="font-size: 15px;">监测站点：</label>
                        <div class="radio-inline">
                            <label class='radiolabel'>
                                <input type="radio" name="station" />朔州市
                                <i class="fa fa-circle-o small"></i>
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class='radiolabel'>
                                <input type="radio" name="station" />平朔
                                <i class="fa fa-circle-o small"></i>
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class='radiolabel'>
                                <input type="radio" name="station" />朔唯
                                <i class="fa fa-circle-o small"></i>
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class='radiolabel'>
                                <input type="radio" name="station" />区政府
                                <i class="fa fa-circle-o small"></i>
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class='radiolabel'>
                                <input type="radio" name="station" />市环保局
                                <i class="fa fa-circle-o small"></i>
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label class='radiolabel'>
                                <input type="radio" name="station" />朔州一中
                                <i class="fa fa-circle-o small"></i>
                            </label>
                        </div>
                    </div>

                <div style="width: 567px; float: left;">
                    <label class="control-label" style="font-size: 15px;">预测项目：</label>
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

                <div style="width: 100%; float: left;">
                    <label class="control-label" style="font-size: 15px;">预报模式：</label>
                    <select id="DayhourID" name="DayhourID" style="font-size: 14px" onchange="changeModel()">
                        <option id="dayID" value="0" selected="selected">统计预报</option>
                        <option id="hourID" value="1">数值预报</option>
                    </select>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <label class="control-label">开始时间：</label>
                    <asp:TextBox ID="txt_begin" runat="server" CssClass="Wdate" onchange="changeData()" Width="110px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <label class="control-label">结束时间：</label>
                    <asp:TextBox ID="txt_end" runat="server" CssClass="Wdate" onchange="changeData()" Width="110px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="button" class="btn btn-primary" style="top: -5px;"
                            value="分析" onclick="changeData()" />
                </div>


            </div>

        </div>
        <div class="box ui-draggable ui-droppable" style="margin: 10px;">
            <div class="box-header">
                <div class="box-name">
                    <i class="fa fa-table"></i>
                    <span>预报准确率</span>
                </div>
                <div class="box-icons">
                    <a class="collapse-link">
                        <i class="fa fa-chevron-up"></i>
                    </a>
                    <a class="expand-link">
                        <i class="fa fa-expand"></i>
                    </a>
                    <a class="close-link">
                        <i class="fa fa-times"></i>
                    </a>
                </div>
                <div class="no-move"></div>
            </div>
            <div class="box-content">
                <div style="width: 100%; height: 150px;">
                    <table class="dtable" style="width: 100%;">
                        <tr>
                            <th style="width: 130px;">检验参数</th>
                            <th style="width: 130px;">站点</th>
                            <th>H0</th>
                            <th>H24</th>
                            <th>H48</th>
                        </tr>
                        <tr>
                            <td>绝对误差</td>
                            <td>朔州市</td>
                            <td>42</td>
                            <td>44</td>
                            <td>46</td>
                        </tr>
                        <tr>
                            <td>相对误差</td>
                            <td>朔州市</td>
                            <td>0.03</td>
                            <td>0.05</td>
                            <td>0.08</td>
                        </tr>
                        <tr>
                            <td>相关系数</td>
                            <td>朔州市</td>
                            <td>0.72</td>
                            <td>0.71</td>
                            <td>0.70</td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="container" style="width: 99%;background-color: white;">
            </div>

        </div>
    </form>
    <script type="text/javascript">

        $(function () {
            $('#container').highcharts({
                chart: {
                    type: 'line'
                },
                title: {
                    text: '预报评估'
                },
                subtitle: {
                    text: '2015/4/1 - 2015/4/30'
                },
                xAxis: {
                    categories: ['4-1', '4-2', '4-3', '4-4', '4-5', '4-6', '4-7', '4-8', '4-9', '4-10', '4-11', '4-12']
                },
                yAxis: {
                    title: {
                        text: 'AQI'
                    }
                },
                tooltip: {
                    enabled: false,
                    formatter: function () {
                        return '<b>' + this.series.name + '</b><br>' + this.x + ': ' + this.y + '°C';
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: false
                    }
                },
                series: [{
                    name: '实际监测',
                    data: [7.0, 6.9, 9.5, 14.5, 18.4, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
                }, {
                    name: '统计预报',
                    data: [3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8]
                }]
            });
        });

    </script>
</body>
</html>
