<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Devoops_Publish_Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/bootstrap/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="../css/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <%--<script src="../js/jquery-2.1.0.min.js"></script>--%>
    <script src="../js/jquery-1.10.2.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script src="../plugins/jquery-ui/jquery-ui.js"></script>
    <script src="../../Controls/Highcharts-4.0.1/highcharts.js"></script>
    <style>
        body {
            margin: -20px;
            background: url(back.jpg);
            background-attachment: fixed;
        }

        #main {
            width: 1000px;
            margin: auto;
            background: rgba(256,256,256,0.8);
           filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=#ffffff,endColorstr=#3363370b);
        }

        #header {
            height: 120px;
            margin: auto;
        }

        #logos {
            width: 100%;
            float: left;
            padding-left: 50px;
        }

        #group {
            width: 100%;
            float: right;
        }

        #forecast {
            height: 370px;
        }      
        .day {
            font-weight: bold;
            font-size: 16px;
            font-family: '微软雅黑';
            float: left;
            text-align: center;
            width: 100%;
        }

        .date {
            font-family: '微软雅黑';
            text-align: center;
            float: left;
            width: 100%;
        }


        .today {
            height: 360px;
            width: 310px;
            border-left: 1px solid gray;
            border-right: 1px solid gray;
            /*border-bottom:1px solid gray;
            border-top:1px solid gray;*/
            float: left;
            padding-left: 10px;
        }        
            .today:hover {
                FILTER: progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr=#E6EEF4,endColorStr=#E6EEF4); /*IE 6 7 8*/
                background: -ms-linear-gradient(top, #fff, #0000ff); /* IE 10 */
                background: -moz-linear-gradient(top,#b8c4cb,#f6f6f8); /*火狐*/                           
                background: -webkit-gradient(linear, 0% 0%, 0% 100%,from(#b8c4cb), to(#f6f6f8)); /*谷歌*/
                background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#fff), to(#0000ff)); /* Safari 4-5, Chrome 1-9*/
                background: -webkit-linear-gradient(top, rgba(256,256,256,0),rgba(62,150,158,0.5),rgba(256,256,256,0)); /*Safari5.1 Chrome 10+*/
                background: -o-linear-gradient(top, #fff, #0000ff); /*Opera 11.10+*/

            }
           
        .future {
            height: 365px;
            width: 225px;
            border-right: 1px solid gray;
            float: left;
            margin-left: 5px;
            padding-right: 5px;
        }

            .future:hover {
                FILTER: progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr=#E6EEF4,endColorStr=#E6EEF4); /*IE 6 7 8*/
                background: -webkit-linear-gradient(top, rgba(256,256,256,0),rgba(62,150,158,0.5),rgba(256,256,256,0)); /*Safari5.1 Chrome 10+*/
            }

        .AQI {
            font-family: impact;
            font-weight: normal;
            font-size: 55px;
            line-height: 50px;
            margin-left: 45px;           
        }

        .category {
            font-size: 20px;
            font-family: '微软雅黑';
            font-weight: bold;
            padding-left: 5px;           
        }

        .tipItem {
            font-weight: bolder;
            height: 30px;
            font-size: 13px;
            font-family: '微软雅黑';
        }

        .tipVaule {
            height: 30px;
            font-size: 13px;
            font-family: '微软雅黑';
        }

        table {
            width: 100%;
            
        }

        .big {
            font-size: 85px;
        }

        .divShadow {
            filter: progid:DXImageTransform.Microsoft.Shadow(color=#909090,direction=120,strength=3); /*ie*/
            -moz-box-shadow: 2px 2px 10px #909090; /*firefox*/
            -webkit-box-shadow: 2px 2px 10px #909090; /*safari或chrome*/
            box-shadow: 2px 2px 10px #909090; /*opera或ie9*/
        }
    </style>
    <script>
       
        $(function () {
            //测试时间，改
            
            //获得当前时间
            var myDate = new Date();            
            var date = myDate.getFullYear() + "-" + ((myDate.getMonth()+1) < 10 ? ("0" + (myDate.getMonth() + 1)) : (myDate.getMonth() + 1)) + "-" + myDate.getDate();
            loadStandardData(date);
            $(".today").click(function () {
                var date = $(this).find(".date").html();//'2015-04-02'
                var dateStr = myDate.getFullYear() + "-" + date.replace('月','-').split('日')[0];              
                $("#span1").css("display", "block").parent().siblings().find("table").siblings().css("display", "none");
                loadChar(dateStr);
            });
            $(".future").click(function () {
                if ($("#noData").html() == "暂无数据！") {
                   
                } else {
                    var date = $(this).find(".date").html();//'2015-04-02'               
                    var dateStr = myDate.getFullYear() + "-" + date.replace('月', '-').split('日')[0];
                    $(this).find("table").siblings().css("display", "block").parent().siblings().find("table").siblings().css("display", "none");
                    loadChar(dateStr);
                }               
            });
            //测试时间，改 
            loadChar(date);
            $("#span1").css("display", "block");
        });
        //加载空气质量标准数据
        function loadStandardData(date) {
            $.post("../../Data/GetStandardData.ashx", { date: date }, function (result) {
                if (result && result.DataTable && result.DataTable.length==4) {
                    //今天                  
                    $("<tr><td style='height: 95px;'><span class='big AQI' style='color:" + columsColor(result.DataTable[0].Aqi) + "'>" + result.DataTable[0].Aqi + "</span><span class='category'  style='color:" + columsColor(result.DataTable[0].Aqi) + "'>" + result.DataTable[0].Category +
                              "</span></td></tr><tr class='tipItem'><td>首要污染物：</td></tr><tr class='tipVaule'>" +
                          "<td>" + result.DataTable[0].FirstP + "</td></tr> <tr class='tipItem'><td>对健康的影响：</td></tr><tr class='tipVaule'>" +
                          "<td>" + result.DataTable[0].Affect + "</td></tr><tr class='tipItem'><td>建议措施：</td></tr>" +
                      "<tr class='tipVaule'><td>" + result.DataTable[0].Proposal + "</td></tr>").appendTo("#jin");                   
                  
                    //第二天                   
                    $("<tr><td style='height: 95px;'><span class='AQI' style='color:" + columsColor(result.DataTable[1].Aqi) + "'>" + result.DataTable[1].Aqi + "</span><span class='category' style='color:" + columsColor(result.DataTable[1].Aqi) + "'>" + result.DataTable[1].Category +
                               "</span></td></tr><tr class='tipItem'><td>首要污染物：</td></tr><tr class='tipVaule'>" +
                           "<td>" + result.DataTable[1].FirstP + "</td></tr> <tr class='tipItem'><td>对健康的影响：</td></tr><tr class='tipVaule'>" +
                           "<td>" + result.DataTable[1].Affect + "</td></tr><tr class='tipItem'><td>建议措施：</td></tr>" +
                       "<tr class='tipVaule'><td>" + result.DataTable[1].Proposal + "</td></tr>").appendTo("#ming");
                   
                   
                    //第三天                   
                    $("<tr><td style='height: 95px;'><span class='AQI' style='color:" + columsColor(result.DataTable[2].Aqi) + "'>" + result.DataTable[2].Aqi + "</span><span class='category' style='color:" + columsColor(result.DataTable[2].Aqi) + "'>" + result.DataTable[2].Category +
                                "</span></td></tr><tr class='tipItem'><td>首要污染物：</td></tr><tr class='tipVaule'>" +
                            "<td>" + result.DataTable[2].FirstP + "</td></tr> <tr class='tipItem'><td>对健康的影响：</td></tr><tr class='tipVaule'>" +
                            "<td>" + result.DataTable[2].Affect + "</td></tr><tr class='tipItem'><td>建议措施：</td></tr>" +
                        "<tr class='tipVaule'><td>" + result.DataTable[2].Proposal + "</td></tr>").appendTo("#hou");                                       
                    //第四天                   
                    $("<tr><td style='height: 95px;'><span class='AQI' style='color:" + columsColor(result.DataTable[3].Aqi) + "'>" + result.DataTable[3].Aqi + "</span><span class='category' style='color:" + columsColor(result.DataTable[3].Aqi) + "'>" + result.DataTable[3].Category +
                               "</span></td></tr><tr class='tipItem'><td>首要污染物：</td></tr><tr class='tipVaule'>" +
                           "<td>" + result.DataTable[3].FirstP + "</td></tr> <tr class='tipItem'><td>对健康的影响：</td></tr><tr class='tipVaule'>" +
                           "<td>" + result.DataTable[3].Affect + "</td></tr><tr class='tipItem'><td>建议措施：</td></tr>" +
                       "<tr class='tipVaule'><td>" + result.DataTable[3].Proposal + "</td></tr>").appendTo("#wl");                    
                    
                } else if (result.DataTable.length == 3) {
                    //今天                  
                    $("<tr><td style='height: 95px;'><span class='big AQI' style='color:" + columsColor(result.DataTable[0].Aqi) + "'>" + result.DataTable[0].Aqi + "</span><span class='category' style='color:" + columsColor(result.DataTable[0].Aqi) + "'>" + result.DataTable[0].Category +
                          "</span></td></tr><tr class='tipItem'><td>首要污染物：</td></tr><tr class='tipVaule'>" +
                      "<td>" + result.DataTable[0].FirstP + "</td></tr> <tr class='tipItem'><td>对健康的影响：</td></tr><tr class='tipVaule'>" +
                      "<td>" + result.DataTable[0].Affect + "</td></tr><tr class='tipItem'><td>建议措施：</td></tr>" +
                  "<tr class='tipVaule'><td>" + result.DataTable[0].Proposal + "</td></tr>").appendTo("#jin");

                    //第二天                   
                    $("<tr><td style='height: 95px;'><span class='AQI' style='color:" + columsColor(result.DataTable[1].Aqi) + "'>" + result.DataTable[1].Aqi + "</span><span class='category' style='color:" + columsColor(result.DataTable[1].Aqi) + "'>" + result.DataTable[1].Category +
                           "</span></td></tr><tr class='tipItem'><td>首要污染物：</td></tr><tr class='tipVaule'>" +
                       "<td>" + result.DataTable[1].FirstP + "</td></tr> <tr class='tipItem'><td>对健康的影响：</td></tr><tr class='tipVaule'>" +
                       "<td>" + result.DataTable[1].Affect + "</td></tr><tr class='tipItem'><td>建议措施：</td></tr>" +
                   "<tr class='tipVaule'><td>" + result.DataTable[1].Proposal + "</td></tr>").appendTo("#ming");


                    //第三天                   
                    $("<tr><td style='height: 95px;'><span class='AQI' style='color:" + columsColor(result.DataTable[2].Aqi) + "'>" + result.DataTable[2].Aqi + "</span><span class='category' style='color:" + columsColor(result.DataTable[2].Aqi) + "'>" + result.DataTable[2].Category +
                            "</span></td></tr><tr class='tipItem'><td>首要污染物：</td></tr><tr class='tipVaule'>" +
                        "<td>" + result.DataTable[2].FirstP + "</td></tr> <tr class='tipItem'><td>对健康的影响：</td></tr><tr class='tipVaule'>" +
                        "<td>" + result.DataTable[2].Affect + "</td></tr><tr class='tipItem'><td>建议措施：</td></tr>" +
                    "<tr class='tipVaule'><td>" + result.DataTable[2].Proposal + "</td></tr>").appendTo("#hou");
                    $("<span class='noData' style='font-weight:bolder;font-size:x-large'>暂无数据！</span>").appendTo("#wl");
                } else if (result.DataTable.length == 2) {
                    //今天                  
                    $("<tr><td style='height: 95px;'><span class='big AQI' style='color:" + columsColor(result.DataTable[0].Aqi) + "'>" + result.DataTable[0].Aqi + "</span><span class='category' style='color:" + columsColor(result.DataTable[0].Aqi) + "'>" + result.DataTable[0].Category +
                          "</span></td></tr><tr class='tipItem'><td>首要污染物：</td></tr><tr class='tipVaule'>" +
                      "<td>" + result.DataTable[0].FirstP + "</td></tr> <tr class='tipItem'><td>对健康的影响：</td></tr><tr class='tipVaule'>" +
                      "<td>" + result.DataTable[0].Affect + "</td></tr><tr class='tipItem'><td>建议措施：</td></tr>" +
                  "<tr class='tipVaule'><td>" + result.DataTable[0].Proposal + "</td></tr>").appendTo("#jin");

                    //第二天                   
                    $("<tr><td style='height: 95px;'><span class='AQI' style='color:" + columsColor(result.DataTable[1].Aqi) + "'>" + result.DataTable[1].Aqi + "</span><span class='category' style='color:" + columsColor(result.DataTable[1].Aqi) + "'>" + result.DataTable[1].Category +
                           "</span></td></tr><tr class='tipItem'><td>首要污染物：</td></tr><tr class='tipVaule'>" +
                       "<td>" + result.DataTable[1].FirstP + "</td></tr> <tr class='tipItem'><td>对健康的影响：</td></tr><tr class='tipVaule'>" +
                       "<td>" + result.DataTable[1].Affect + "</td></tr><tr class='tipItem'><td>建议措施：</td></tr>" +
                   "<tr class='tipVaule'><td>" + result.DataTable[1].Proposal + "</td></tr>").appendTo("#ming");
                    $("<span class='noData' style='font-weight:bolder;font-size:x-large'>暂无数据！</span>").appendTo("#hou");
                    $("<span class='noData' style='font-weight:bolder;font-size:x-large'>暂无数据！</span>").appendTo("#wl");
                } else if (result.DataTable.length == 1) {
                    //今天                  
                    $("<tr><td style='height: 95px;'><span class='big AQI' style='color:" + columsColor(result.DataTable[0].Aqi) + "'>" + result.DataTable[0].Aqi + "</span><span class='category' style='color:" + columsColor(result.DataTable[0].Aqi) + "'>" + result.DataTable[0].Category +
                          "</span></td></tr><tr class='tipItem'><td>首要污染物：</td></tr><tr class='tipVaule'>" +
                      "<td>" + result.DataTable[0].FirstP + "</td></tr> <tr class='tipItem'><td>对健康的影响：</td></tr><tr class='tipVaule'>" +
                      "<td>" + result.DataTable[0].Affect + "</td></tr><tr class='tipItem'><td>建议措施：</td></tr>" +
                  "<tr class='tipVaule'><td>" + result.DataTable[0].Proposal + "</td></tr>").appendTo("#jin");
                    $("<span class='noData' style='font-weight:bolder;font-size:x-large'>暂无数据！</span>").appendTo("#ming");
                    $("<span class='noData' style='font-weight:bolder;font-size:x-large'>暂无数据！</span>").appendTo("#hou");
                    $("<span class='noData' style='font-weight:bolder;font-size:x-large'>暂无数据！</span>").appendTo("#wl");
                } else {
                    $("#span1").hide();
                    $("<span class='noData' style='font-weight:bolder;font-size:x-large'>暂无数据！</span>").appendTo("#jin");
                    $("<span class='noData' style='font-weight:bolder;font-size:x-large'>暂无数据！</span>").appendTo("#ming");
                    $("<span class='noData' style='font-weight:bolder;font-size:x-large'>暂无数据！</span>").appendTo("#hou");
                    $("<span class='noData' style='font-weight:bolder;font-size:x-large'>暂无数据！</span>").appendTo("#wl");
                }
            }, "json");
        };
        //加载折线图和柱状图
        function loadChar(date) {
            var xData = new Array();
            var yData = new Array();                      
            var color = new Array();
            var yData2 = new Array();
            var xData2 = new Array();
            var color = new Array();
            //柱状图后改
            var xxData = new Array();
            var yyData = new Array();
            $.post("../../Data/GetHourData.ashx", { date: date }, function (result) {
                if (result && result.DataTable && result.DataTable.length > 0) {
                    //var xData = new Array();
                    //var yData = new Array();
                    $.each(result.DataTable, function (index, item) {
                        xData.push(item.MonitorTime.substr(11, 2) + "时");
                        yData.push(item.Aqi);
                    })
                    setLineChar(xData, yData, date);
                }
            }, "json");
            $.post("../../Data/GetData.ashx", { date: date }, function (result) {
               
                if (result && result.DataTable && result.DataTable.length > 0) {                    
                    //var dataObj = new Array();       
                    for (var i = 0; i < result.DataTable.length; i++) {
                       // var station = {name:[],data:[]};
                        $.each(result.DataTable[i], function (index, item) {
                            //var datas = { y: '', color: '', drilldown: { categories: [], data: [], color: '' } };              
                            if (!isNaN(item)) {
                                //station.data.push(item);
                                //color.push(columsColor(item));
                                yyData.push({ y: item ,color:columsColor(item)})
                            }
                            else {
                                //station.name.push(item);
                                xxData.push(item);
                            }                           
                        })
                       
                        //yData2.push(station);
                    }
                   // xData2.push(date);
                    //setColumsChar(yData2, xData2,date,color);//"red", date
                    setColumsChar(xxData, yyData, date)
                }
            }, "json");
        }
        //柱子颜色
        function columsColor(value) {
            var color = "";
            if (value > 0 && value <= 50) {
                color = '#66CC00';
            } else if (value > 50 && value <= 100) {
                color = '#FBD12A';
            } else if (value > 100 && value <= 150) {
                color = '#FFA641';
            } else if (value > 150 && value <= 200) {
                color = '#EB5B13';
            } else if (value > 200 && value <= 300) {
                color = '#960453';
            }
            else {
                color = '#580422';
            }
            return color;
        };
        //站点空气质量柱状图   yData, xData, date, color
        function setColumsChar(xData,yData,date) {//, color,date   

            //var colors = Highcharts.getOptions().colors,
              categories = xData,             
              data = yData;

            function setChart(name, categories, data, color) {
                chart.xAxis[0].setCategories(categories, false);
                chart.series[0].remove(false);
                chart.addSeries({
                    name: name,
                    data: data,
                    color: color || 'white'
                }, false);
                chart.redraw();
            }

            var chart= $('#statoin').highcharts({
                chart: {
                    type: 'column',
                    backgroundColor: 'rgba(256,256,256,0.7)'
                },
                title: {
                    text: '站点空气质量状况'
                },
                subtitle: {
                    text: date
                },
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: 'AQI'
                    }
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    column: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    var drilldown = this.drilldown;
                                    if (drilldown) { // drill down
                                        setChart(drilldown.name, drilldown.categories, drilldown.data, drilldown.color);
                                    } else { // restore
                                        setChart(name, categories, data);
                                    }
                                }
                            }
                        },
                        dataLabels: {
                            enabled: true,
                            //color: colors[0],
                            style: {
                                fontWeight: 'bold'
                            },
                            formatter: function () {
                                return this.y + '';
                            }
                        }
                    }
                },
                tooltip: {
                    formatter: function () {
                        var point = this.point,
                            s = this.x + ':<b>' + this.y + '</b><br/>';
                        //if (point.drilldown) {
                        //    s += 'Click to view ' + point.category + ' versions';
                        //} else {
                        //    s += 'Click to return to browser brands';
                        //}
                        return s;
                    }
                },
                series: [{
                    name: name,
                    data: data,
                    color: 'white'
                }],
                exporting: {
                    enabled: false
                }
            })
            .highcharts();




        //    $('#statoin').highcharts({
        //chart: {
        //    type: 'column'
        //},
        //title: {
        //    text: '朔州市('+date+'AQI趋势图)'
        //},       
        //xAxis: {
        //    categories: xData
        //},
        //yAxis: {
        //    min: 0,
        //    title: {
        //        text: 'AQI'
        //    }
        //},
        //colors: color,
        //tooltip: {
        //    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
        //    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
        //        '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
        //    footerFormat: '</table>',
        //    shared: true,
        //    useHTML: true
        //},
        //plotOptions: {
        //    column: {
        //        pointPadding: 0.2,
        //        borderWidth: 0
        //    }
        //},
        //series: yData
    //});

        };
        //空气质量小时折线图
        function setLineChar(x, y, date) {
            $('#trend').highcharts({
                chart: {
                    type: 'line'
                },
                title: {
                    text: '空气质量趋势'
                },
                subtitle: {
                    text: date + ' AQI走向'
                },
                xAxis: {
                    categories: x
                },
                yAxis: {
                    title: {
                        text: 'AQI'
                    }
                },
                legend: {
                    enabled: false
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
                    //name: 'Tokyo',
                    data: y
                }]
            });
        };
    </script>
</head>

<body>
    <form id="form1" runat="server">        
        <div id="main">
            <div id="header">
                <div id="logos">
                    <h1 style="margin-left: 200px;margin-top:0px;padding-top:20px;">朔州市空气质量预测预报发布系统</h1>
                </div>
                <div id="group">
                    <p style="margin-left: 320px;">朔州市环境保护局（<%=DateTime.Now.Month.ToString() %>月<%=DateTime.Now.Day.ToString() %>日<%=DateTime.Now.Hour.ToString() %>时发布）(...调试中...)</p>
                </div>
                <hr />
            </div>
            <div id="forecast">
                
                <div class="today">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tbody id="jin">
                            <tr>
                                <td>
                                    <div class="day">
                                        今天&nbsp;
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="date"><%=DateTime.Now.Month.ToString() %>月 <%=DateTime.Now.Day.ToString() %>日</div>
                                </td>
                            </tr>
                            <tr class="tipItem">
                                <td>空气质量指数：
                                </td>
                            </tr>
                        </tbody>
                    </table>
                  <span id="span1" class="glyphicon glyphicon-hand-down" aria-hidden="true" style="float:left;margin-top:526px;margin-left:130px ;position:absolute;display:none"></span>
                </div>                              
                <div class="future">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tbody id="ming">
                            <tr>
                                <td>
                                    <div class="day">
                                        <%=System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.AddDays(1).DayOfWeek) %>&nbsp;
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="date"><%=DateTime.Now.Month.ToString() %>月 <%=DateTime.Now.AddDays(1).Day.ToString() %>日</div>
                                    <%-- <%=DateTime.Now.Month.ToString() %>月 <%=DateTime.Now.AddDays(1).Day.ToString() %>日--%>
                                </td>
                            </tr>
                            <tr class="tipItem">
                                <td>空气质量指数：
                                </td>
                            </tr>
                        </tbody>
                    </table>
                     <span class="glyphicon glyphicon-hand-down" aria-hidden="true" style="margin-top:535px;margin-left:95px ;position:absolute;display:none;"></span>
                </div>
            
                <div class="future">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tbody id="hou">
                            <tr>
                                <td>
                                    <div class="day">
                                        <%=System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.AddDays(2).DayOfWeek)%>&nbsp;
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="date"><%=DateTime.Now.AddDays(2).ToString("MM-dd").Replace("-","月") %>日</div>


                                    <%-- <%=DateTime.Now.AddDays(2).ToString("MM-dd").Replace("-","月") %>日--%>
                                </td>
                            </tr>
                            <tr class="tipItem">
                                <td>空气质量指数：
                                </td>
                            </tr>
                        </tbody>
                    </table>
                     <span class="glyphicon glyphicon-hand-down" aria-hidden="true" style="float:left;margin-top:535px;position:absolute;margin-left:95px;display:none"></span>
                </div>              
                <div class="future">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tbody id="wl">
                            <tr>
                                <td>
                                    <div class="day">
                                        <%=System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.AddDays(3).DayOfWeek)%>&nbsp;
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <div class="date"><%=DateTime.Now.AddDays(3).ToString("MM-dd").Replace("-","月") %>日</div>
                                </td>
                            </tr>

                            <tr class="tipItem">
                                <td>空气质量指数：
                                </td>
                            </tr>                          
                        </tbody>
                    </table>
                     <span class="glyphicon glyphicon-hand-down" aria-hidden="true" style="float:left;margin-top:535px;position:absolute;margin-left:95px;display:none"></span>
                </div>
            </div>
            <div id="trend" style="margin-top: 10px;height:400px;" class="divShadow"></div>
            <div id="statoin" style="margin-top: 30px;height:400px;" class="divShadow"></div>

        </div>
    </form>
</body>
</html>
