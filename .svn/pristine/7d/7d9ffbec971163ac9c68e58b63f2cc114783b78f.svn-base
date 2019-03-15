<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SingleTableChart.aspx.cs"
    Inherits="SingleTable_SingleTableChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HightChart</title>
</head>
<body>
    <div class="row">
        <div class="col-xs-12">
            <div class="box">
                <div class="box-header">
                    <div class="box-name">
                        <i class="fa fa-bar-chart-o"></i>&nbsp;&nbsp;图表展示
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
                    <!-- BEGIN FORM-->
                    <form id="formChart" runat="server">
                        <div id="container">
                        </div>
                        <asp:HiddenField runat="server" ID="hfStyle" />
                        <asp:HiddenField runat="server" ID="hfType" />
                        <asp:HiddenField runat="server" ID="hfTitle" />
                        <asp:HiddenField runat="server" ID="hfDimension" />
                        <asp:HiddenField runat="server" ID="hfData" />
                    </form>
                    <!-- END FORM-->
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function () {

            var data = $("#hfData").val();
            if (data == "") {
                $("#container").html("")
            }
            else {
                var chartTitle = $("#hfTitle").val();
                var chartHeight = 400;
                var chartType = $("#hfType").val();; //line column
                var yTitle = $("#hfDimension").val();;
                var yStep = 1;
                DataToChart("container", data, chartTitle, chartHeight, chartType, yTitle, yStep);
            }

        });

        function DataToChart(div, data, title, height, type, yTitle, yStep) {
            //string To json 序列化
            var json = JSON.parse(data);
            var chart = new Highcharts.Chart({
                chart: {
                    zoomType: 'x',
                    spacingRight: 20,
                    renderTo: div,
                    height: height,
                    type: type
                },
                title: {
                    text: title
                },
                xAxis: {
                    categories: json.ChartTime,
                    labels: {
                        step: yStep
                    }
                },
                yAxis: {
                    title: {
                        text: yTitle
                    },
                    labels: {
                        formatter: function () {
                            return this.value;
                        }
                    }
                },
                attr: {
                    'stroke-width': 1,
                    stroke: '#cccccc'
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                tooltip: {
                    crosshairs: true,
                    formatter: function () {
                        return '' + this.series.name + ':' + this.y + yTitle + '<br/>' + this.x;
                    }
                },
                exporting: {
                    url: 'Controls/Highcharts-4.0.1/HighChartExport.aspx',
                    width: 1200
                },
                plotOptions: {
                    column: { borderWidth: 0 }
                },

                series: json.ChartValue
            })
        }

    </script>
</body>
</html>
