﻿//SystemEvaluate1.html
//表头
var SO2 = "SO2";
var NO2 = "NO2";
var CO = "CO";
var O3 = "O3";
var PM10 = "PM10";
var PM25 = "PM25";
var AQI = "AQI";
function table_head() {
    debugger;
    var table_head = "";

    $("#showDiagram").hide();
    $("#showcontrast").hide();

    if (item == "浓度") {
        $("#comment").show();
        //polltip();
        table_head += "<tr class='thcss' id='tbHeadCon'>";
        table_head += "<th rowspan='2' style='width:10%;'>时间</th>"
                            + "<th colspan='2' id='CSO2' style='width:15%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('SO2');>二氧化硫<br />(SO<sub>2</sub>)<br />CC:["
                            + relativeResult(SO2) + "]</th>"
                            + "<th colspan='2' id='NO2' style='width:15%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('NO2');>二氧化氮<br />(NO<sub>2</sub>)<br />CC:["
                            + relativeResult(NO2) + "]</th>"
                            + "<th colspan='2' id='CO' style='width:15%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('CO');>一氧化碳<br />(CO)<br />CC:["
                            + relativeResult(CO) + "]</th>"
                            + "<th colspan='2' id='O3' style='width:15%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('O3');>臭氧<br />(O<sub>3</sub>)<br />CC:["
                            + relativeResult(O3) + "]</th>"
                            + "<th colspan='2' id='PM10' style='width:15%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('PM10');>可吸入颗粒物<br />(PM<sub>10</sub>)<br />CC:["
                            + relativeResult(PM10) + "]</th>"
                            + "<th colspan='2' id='PM25' style='width:15%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('PM25');>微颗粒物<br />(PM<sub>2.5</sub>)<br />CC:["
                            + relativeResult(PM25) + "]</th>";
        
        table_head += "</tr><tr>";
        for (var j = 0; j < 6; j++) {
            table_head += "<th>绝对误差</th><th>相对误差(%)</th>";
        }
        
    }
    else if (item == "指数") {
        $("#comment").hide();
        //polltip();
        table_head += "<tr class='thcss' id='tbHeadindex'>";
        table_head += "<th rowspan='2' style='width:9%;'>时间</th><th colspan='2' id='AQI' style='width:13%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('AQI');>空气质量指数<br />(AQI)<br />CC:["
                            + relativeResult(AQI) + "]</th>"
                            + "<th colspan='2' id='SO2' style='width:13%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('SO2');>二氧化硫<br />(SO<sub>2</sub>)<br />CC:["
                            + relativeResult(SO2) + "]</th>"
                            + "<th colspan='2' id='NO2' style='width:13%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('NO2');>二氧化氮<br />(NO<sub>2</sub>)<br />CC:["
                            + relativeResult(NO2) + "]</th>"
                            + "<th colspan='2' id='CO' style='width:13%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('CO');>一氧化碳<br />(CO)<br />CC:["
                            + relativeResult(CO) + "]</th>"
                            + "<th colspan='2' id='O3' style='width:13%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('O3');>臭氧<br />(O<sub>3</sub>)<br />CC:["
                            + relativeResult(O3) + "]</th>"
                            + "<th colspan='2' id='PM10' style='width:13%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('PM10');>可吸入颗粒物<br />(PM<sub>10</sub>)<br />CC:["
                            + relativeResult(PM10) + "]</th>"
                            + "<th colspan='2' id='PM25' style='width:13%;' title='相关系数(Correlation Coefficient)' onclick=SetChartData('PM25');>微颗粒物<br />(PM<sub>2.5</sub>)<br />CC:["
                            + relativeResult(PM25) + "]</th>";
        table_head += "</tr><tr>";
        for (var j = 0; j < 7; j++) {
            table_head += "<th>绝对误差</th><th>相对误差(%)</th>";
        }
        //SetChartData(AQI);

    }
    else if (item == "级别") {
        $("#comment").hide();
        table_head += "<tr class='thcss' id='tbHeadlevel'>";
        table_head += "<th rowspan='2' >时间</th><th >实测等级</th>"
                            + "<th >预报等级</th>";
        //$("#showcontrast").show();
        setChart3();

    }
    table_head += "</tr>";
    return table_head;
}

//计算相关系数（用到实测值和监测值）
function relativeResult(poll) {
    var dataObj = result_data;
    var ydata_d = new Array();//监测值    
    var ydata_f = new Array();//实测值   

    if (dataObj && dataObj.length > 0) {
        for (var i = 0; i < dataObj.length; i++) {
            var poll_d = poll + "_d";
            var stationdata1 = (dataObj[i][poll_d] == "--" || dataObj[i][poll_d]<0) ? 0 : dataObj[i][poll_d];
            /*if (stationdata1 == '--') {
                stationdata1 = 0;
            }*/
            ydata_d.push(stationdata1);

            var poll_f = poll + "_f";
            var stationdata2 = (dataObj[i][poll_f] == "--" || dataObj[i][poll_f] < 0) ? 0 : dataObj[i][poll_f];
            /*if (!isNaN(stationdata2))
            {
                stationdata2 = 0;
            }*/
            ydata_f.push(stationdata2);
        }
    }
    var f = ydata_f;
    var m = ydata_d;
    //fpow为f的每项平方后结果，mpow为m的每项平方后结果，fm为f和m每项相乘    var fpow = [], mpow = [], fm = [];    for (var i = 0; i < f.length; i++) {
        
        fpow.push(Math.pow(f[i], 2));//Math.pow(底数,几次方)        mpow.push(Math.pow(m[i], 2));        fm.push(f[i] * m[i]);
        debugger;
    }    //计算相关系数公式    var result = (eval(fm.join('+')) - eval(f.join('+')) * eval(m.join('+')) / f.length) / Math.sqrt((eval(fpow.join('+')) -        Math.pow(eval(f.join('+')), 2) / f.length) * (eval(mpow.join('+')) - Math.pow(eval(m.join('+')), 2) / f.length));
    if (isNaN(result)) {
        return 0;
    } else {
        return result.toFixed(4);
    }
}

//表内容
function table_body() {
    var table_body = "";
   
    for (var i = 0  ; i < result_data.length; i++) {
        
        table_body += "<tr>";
        table_body += "<td>" + result_data[i]["MonitorTime"] + "</td>";
        debugger;
        if (item == "级别") {
            table_body += "<td>" + result_data[i]["level_d"] + "</td>"
                + "<td>" + result_data[i]["level_f"] + "</td>";
            
        }
        else if (item == "指数") {
            table_body += "<td>" + result_data[i]["AQI"] + "</td>"
                + "<td>" + result_data[i]["AQI_XD"] + "</td>"
            + "<td>" + result_data[i]["SO2"] + "</td>"
            + "<td>" + result_data[i]["SO2_XD"] + "</td>"
            + "<td>" + result_data[i]["NO2"] + "</td>"
            + "<td>" + result_data[i]["NO2_XD"] + "</td>"
            + "<td>" + result_data[i]["CO"] + "</td>"
            + "<td>" + result_data[i]["CO_XD"] + "</td>"
            + "<td>" + result_data[i]["O3"] + "</td>"
            + "<td>" + result_data[i]["O3_XD"] + "</td>"
            + "<td>" + result_data[i]["PM10"] + "</td>"
            + "<td>" + result_data[i]["PM10_XD"] + "</td>"
            + "<td>" + result_data[i]["PM25"] + "</td>"
            + "<td>" + result_data[i]["PM25_XD"] + "</td>";
            
        }
        else if (item == "浓度") {
            table_body += "<td>" + result_data[i]["SO2"] + "</td>"
            + "<td>" + result_data[i]["SO2_XD"] + "</td>"
            + "<td>" + result_data[i]["NO2"] + "</td>"
            + "<td>" + result_data[i]["NO2_XD"] + "</td>"
            + "<td>" + result_data[i]["CO"] + "</td>"
            + "<td>" + result_data[i]["CO_XD"] + "</td>"
            + "<td>" + result_data[i]["O3"] + "</td>"
            + "<td>" + result_data[i]["O3_XD"] + "</td>"
            + "<td>" + result_data[i]["PM10"] + "</td>"
            + "<td>" + result_data[i]["PM10_XD"] + "</td>"
            + "<td>" + result_data[i]["PM25"] + "</td>"
            + "<td>" + result_data[i]["PM25_XD"] + "</td>";
        }
        table_body += "</tr>";
    } 
    if (item == "浓度") {        
        SetChartData('SO2');
    }
    else if(item == "指数")
    {       
        SetChartData('AQI');
    }
    return table_body;
}

//单元格点击事件
function SetChartData(poll) {//item:浓度、指数、级别;, result_data
    debugger;

    $("#showDiagram").show();
    $("#showcontrast").show();
    var dataObj = result_data;
    var xdata = [];
    var ydata1 = new Array();
    var ydata2 = new Array();
    var ydata3 = new Array();
    var ydata4 = new Array();
    if (dataObj && dataObj.length > 0) {
        for (var i = 0; i < dataObj.length; i++) {
            xdata.push(dataObj[i]["MonitorTime"]);
            //绝对误差为空时，赋值为0            
            var stationdata1 = (dataObj[i][poll] == "--") ? 0 : dataObj[i][poll];
            ydata1.push(stationdata1);

            var poll_xd = poll + "_XD";
            //相对误差为空时，赋值为0
            var stationdata2 = (dataObj[i][poll_xd] == "--") ? 0 : dataObj[i][poll_xd];
            ydata2.push(stationdata2);

            var poll_d = poll + "_d";
            //监测值为空时，赋值为0
            var stationdata3 = (dataObj[i][poll_d]== "--") ? 0 :dataObj[i][poll_d] ;
            ydata3.push(stationdata3);

            var poll_f = poll + "_f";
            //预测值为空时，赋值为0
            var stationdata4 = (dataObj[i][poll_f]== "--") ? 0 :dataObj[i][poll_f];
            ydata4.push(stationdata4);
        }
    }
    var ylabel;
    if (item == "浓度") {
        if ("CO" == poll) {
            ylabel = poll + "浓度（mg/m³）";
        }
        else {
            ylabel = poll + "浓度（ug/m³）";
        }
    }
    else if (item == "指数") {
        if ("AQI" == poll) {
            ylabel = "AQI值";
        }
        else {
            ylabel = poll + "AQI值";
        }
    }

    getChart1(xdata, ydata1, ydata2, poll, ylabel);
    getChart2(xdata, ydata3, ydata4, poll, ylabel);

}

//绝对误差、相对误差
function getChart1(xdata, ydata1, ydata2, poll, ylabel) {
    
    $('#showDiagram').highcharts({
        chart: {
            type: 'line'
        },
        title: {
            text: stationName + "  " + modelName + "  " + poll + "  " + item + "准确率统计"
        },
        subtitle: {
            text: beginTime + '至' + endTime
        },
        xAxis: {
            categories: xdata
        },
        yAxis: [{
            title: { text: ylabel }
        }, {
            title: { text: '(%)' },
            opposite: true

        }],
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}:</td>' +
                '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            },
            //line: {
            //dataLabels: {
            //enabled: true
            //},
            //enableMouseTracking: false
            //},
            spline: {
                states: {
                    hover: {
                        lineWidth: 3
                    }
                },
                marker: {
                    enabled: false
                },
            }
        },
        /*legend: {//图标显示位置  
            layout: 'vertical',
            //align: 'right',
            //verticalAlign: 'top',
            x: -10,
            y: 100,
            borderWidth: 0
        },*/

        series: [{
            name: '绝对误差', data: ydata1
        }, {
            name: '相对误差(%)', yAxis: 1, data: ydata2
        }]
    });
}

    //监测值、预测值
    function getChart2(xdata, ydata3, ydata4, poll, ylabel) {        
        $('#showcontrast').highcharts({
            chart: {
                type: 'line'
            },
            title: {
                text: stationName + "  " + modelName + "  " + poll + "  " + item + "预测值和实测值对比"
            },
            subtitle: {
                text: beginTime + '至' + endTime
            },
            xAxis: {
                categories: xdata
            },
            yAxis: {
                min: 0,
                title: {
                    text: ylabel
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name};</td>' +
                    '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                },
                //line: {
                // dataLabels: {
                // enabled: true
                //},
                //enableMouseTracking: false
                //},
                spline: {
                    states: {
                        hover: {
                            lineWidth: 3
                        }
                    },
                    marker: {
                        enabled: false
                    },
                }
            },
            /*legend: {//图标显示位置  
                layout: 'vertical',
                align: 'bottom',
                verticalAlign: 'bottom',
                x: -10,
                y: 100,
                borderWidth: 0
            },*/

            series: [{
                name: '实测', data: ydata3
            }, {
                name: '预报', data: ydata4
            }]
        });
    }

    function setChart3(){
        $("#showcontrast").show();
        var dataObj = result_data;
        var xdata = [];
        var ydata_d = new Array();
        var ydata_f = new Array();
        if (dataObj && dataObj.length > 0) {
            for (var i = 0; i < dataObj.length; i++) {
                xdata.push(dataObj[i]["MonitorTime"]);
                //实测，AQI等级为空时，赋值为0
                var stationdata_d = getAirLevel(dataObj[i]["level_d"] );
                ydata_d.push(stationdata_d);
                //预测，AQI等级为空时，赋值为0
                var stationdata_f = getAirLevel(dataObj[i]["level_f"]) ;
                ydata_f.push(stationdata_f);

            }
        }
        getChart3(xdata,ydata_d,ydata_f);
    }
    function getAirLevel(level) {
        var airLevel;
        if (level == "I" || level == 1) {
            airLevel = 1;
        }
        else if (level == "II" || level == 2) {
            airLevel = 2;
        }
        else if (level == "III" || level == 3) {
            airLevel = 3;
        }
        else if (level == "IV" || level == 4)
        {
            airLevel = 4;
        }
        else if (level == "Ⅴ" || level == 5) {
            airLevel = 5;
        }
        else if (level == "VI" || level == 6) {
            airLevel = 6;
        }
        else {
            airLevel = 0;
        }
        return airLevel;
    }

    //等级监测、预测值(ydata5\ydata6)
    function getChart3(xdata, ydata_d, ydata_f) {
        
        $('#showcontrast').highcharts({
            chart: {
                type: 'line'
            },
            title: {
                text: stationName + "  " + modelName + "预测和实测空气质量等级对比"
            },
            subtitle: {
                text: beginTime + '至' + endTime
            },
            xAxis: {
                categories: xdata
            },
            yAxis: {
                min: 0,
                title: {
                    text: ''
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name};</td>' +
                    '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                },
                //line: {
                // dataLabels: {
                // enabled: true
                //},
                //enableMouseTracking: false
                //},
                spline: {
                    states: {
                        hover: {
                            lineWidth: 3
                        }
                    },
                    marker: {
                        enabled: false
                    },
                }
            },
            /*legend: {//图标显示位置  
                layout: 'vertical',
                //align: 'right',
                //verticalAlign: 'top',
                x: -10,
                y: 100,
                borderWidth: 0
            },*/

            series: [{
                name: '实测', data: ydata_d
            }, {
                name: '预报', data: ydata_f
            }]
        });
    }


