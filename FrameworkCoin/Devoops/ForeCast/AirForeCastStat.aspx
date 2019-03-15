<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" CodeFile="AirForeCastStat.aspx.cs" Inherits="Devoops_ForeCast_AirForeCastStat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../plugins/jquery-ui/jquery-ui.min.css" rel="stylesheet" />
    <link href="../plugins/jquery-ui/jquery-ui.theme.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <script src="../js/jquery-1.7.2.js"></script>
    <script src="../js/jquery-2.1.0.min.js"></script>
    <script src="../plugins/jquery-ui/jquery-ui.js"></script>
    <script src="../js/devoops.js"></script>
    <script src="../js/method.js"></script>
    <link href="../../Demo/plugins/bootstrap/bootstrap.css" rel="stylesheet" />
    <script src="../../Demo/plugins/jquery-ui/i18n/jquery.ui.datepicker-zh-CN.min.js"></script>
    <style>
        html, body {
            margin: 0px 0px 0px 0px;
            height: 100%;
            overflow: hidden;
            font-size: 12px;
            background: #fff;
        }

        .tab {
            word-break: keep-all;
            border: 1px solid #EBEBEB;
            border-bottom: none;
            border-left: none;
            border-top-left-radius: 6px;
            margin-top: 10px;
            width: 100%;
        }

            .tab td {
                border-bottom: 1px solid #EBEBEB;
                border-left: 1px solid #EBEBEB;
                padding: 5px;
                line-height: 25px;
                color: #41A7E2;
                width: 200px;
            }

            .tab th {
                background: #6494BB;
                border-left: 1px solid #EBEBEB;
                padding: 5px;
                line-height: 25px;
                border-bottom: 1px solid #EBEBEB;
                color: #fff;
                text-align: center;
            }

    </style>
    <script>
        var hasData = false;
        $(function () {
            $('#txt_Begin').val(new Date().format("yyyy-MM-dd"));
            $('#txt_Begin').datepicker({ showOn: "click" });
            $('#txt_End').val(new Date().addDays(2).format("yyyy-MM-dd"));
            $('#txt_End').datepicker({ showOn: "click" });
            getData();
        });

        function getData() {
            debugger;
            $("#table").html("");
            var htmlHead = "<tr><th rowspan=\"2\">预报模式</th>";
            var htmlHead1 = "<tr>";
            var intvel = new Date($('#txt_End').val()).addDays(1).getTime() - new Date($('#txt_Begin').val()).getTime();
            var timeAry = new Array();
            var days = Math.floor(intvel / (24 * 3600 * 1000));
            for (var i = 0; i < days ; i++) {
                var date = new Date($('#txt_Begin').val()).addDays(i);
                htmlHead += "<th colspan=\"2\">" + date.getDate() + "日</th>";
                htmlHead1 += "<th>AQI</th><th>首要污染物</th>";
                timeAry.push(date.getDate());
            }
            htmlHead += "</tr>";
            htmlHead1 += "</tr>";
            $("#table").append(htmlHead);
            $("#table").append(htmlHead1);

            $.ajax({
                url: "../../Data/GetAirForeCastData.ashx",
                data: { flag: "AirForeCastStat", Strtime: $('#txt_Begin').val(), Endtime: $('#txt_End').val() },
                dataType: "json",
                beforeSend: function () { $("#waiting").show() },
                success: function (result) {
                    debugger;
                    var content = "";
                    if (result && result.Table && result.Table.length > 0) {
                        for (var i = 0; i < result.Table.length; i++) {
                            var htmlTd = "<tr><td>" + result.Table[i]["ModelTypeName"] + "</td>";
                            debugger;
                            var j = 0;
                            for (var data in result.Table[i]) {
                                if (data != "ModelTypeName" && data != "ForecastModel") {
                                    htmlTd += "<td>" + result.Table[i][timeAry[j]] + "</td>";
                                    htmlTd += "<td>" + result.Table1[i][timeAry[j]] + "</td>";
                                }
                                j++;
                            }
                            htmlTd += "</tr>";
                            $("#table").append(htmlTd);
                        }
                        hasData = true;
                    }
                    else {
                        var htmlTd = "<tr ><td id='nodatatd' colspan='" + (days * 2 + 1) + "' style='text-align:center; height:40px;'>暂无数据</td></tr>";
                        $("#table").append(htmlTd);
                        hasData = false;
                    }
                    $("#waiting").hide()
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $("#waiting").hide()
                }
            });
        }

        function GetHtml() {
            debugger;
            if (!hasData)
                return false;
            $("#tbHtml").val($("#tableDiv").html());
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="height: 40px; border: 1px solid #EBEBEB; border-radius: 7px; padding-left: 15px; padding-top: 6px;">
                日期：
       <input type="text" style="color: #1571a0; vertical-align: central; width: 100px;" runat="server" id="txt_Begin" placeholder="开始日期" />
                至
        <input type="text" style="color: #1571a0; vertical-align: central; width: 100px;" runat="server" id="txt_End" placeholder="结束日期" />
                <input type="button" style="margin-left: 50px;" value="统计" onclick="getData()" />
                <asp:Button ID="Button1" runat="server" Text="导出" OnClick="Button1_Click" OnClientClick="return GetHtml();" />
            </div>
            <div id="tableDiv" style="overflow-x:auto;">
                <table id="table" class="tab">
                    <tr>
                        <th rowspan="2">预报模式</th>
                        <th colspan="2">31日</th>
                        <th colspan="2">1日</th>
                        <th colspan="2">2日</th>
                    </tr>
                    <tr>
                        <th>AQI</th>
                        <th>首要污染物</th>
                        <th>AQI</th>
                        <th>首要污染物</th>
                        <th>AQI</th>
                        <th>首要污染物</th>
                    </tr>
                    <tr>
                        <td>统计模式</td>
                        <td>100</td>
                        <td>PM2.5</td>
                        <td>100</td>
                        <td>100</td>
                        <td>PM2.5</td>
                        <td>PM2.5</td>
                    </tr>
                    <tr>
                        <td>CMAQ</td>
                        <td>100</td>
                        <td>PM2.5</td>
                        <td>100</td>
                        <td>100</td>
                        <td>PM2.5</td>
                        <td>PM2.5</td>
                    </tr>
                    <tr>
                        <td>WRF-Chem</td>
                        <td>100</td>
                        <td>PM2.5</td>
                        <td>100</td>
                        <td>100</td>
                        <td>PM2.5</td>
                        <td>PM2.5</td>
                    </tr>
                    <tr>
                        <td>加权均值</td>
                        <td>100</td>
                        <td>PM2.5</td>
                        <td>100</td>
                        <td>100</td>
                        <td>PM2.5</td>
                        <td>PM2.5</td>
                    </tr>
                    <tr>
                        <td>实测</td>
                        <td>100</td>
                        <td>PM2.5</td>
                        <td>100</td>
                        <td>100</td>
                        <td>PM2.5</td>
                        <td>PM2.5</td>
                    </tr>
                </table>
            </div>
            <img id="waiting" src="../img/loading.gif" style="display:none;" width="100%" height="2px;" />
        </div>
        <input type="hidden" id="tbHtml" runat="server" />
    </form>
</body>
</html>
