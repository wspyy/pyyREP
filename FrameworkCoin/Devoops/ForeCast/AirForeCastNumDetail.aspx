﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirForeCastNumDetail.aspx.cs" Inherits="Devoops_ForeCast_AirForeCastNumDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>空气质量预测结果</title>
    <style>
        #pageID li {
            border: solid;
            border-width: 1px;
            border-color: #C7C7C7;
            padding-left: 5px;
            padding-right: 5px;
            padding-top: 5px;
            margin-left: 0px;
            margin-bottom: 10px;
            float: left;
            list-style: none;
            cursor: pointer;
            width: 30px;
            height: 30px;
            text-align: center;
            vertical-align: central;
            background-color: #d7e7e9;
        }

        .thcss th {
            text-align: center;
        }

        .radio-inline {
            padding-left: 0px;
            margin-left: 0px;
        }
    </style>
    <script type="text/javascript">
        var hasdata = false;
        $(function () {
            $("#forecastmodel").val(<%=Request.QueryString["forecastmodel"].ToString()%>);
            getSatationList();
            document.getElementById("CurrentPagecount").value = 1;
            $('#txt_beginDateSec').val(new Date().addDays(-0).format("yyyy-MM-dd"));
            $('#txt_beginDateSec').datepicker({ showOn: "click" });
            $('#txt_endDateSec').val(new Date().addDays(2).format("yyyy-MM-dd"));
            $('#txt_endDateSec').datepicker({ showOn: "click" });

            //设置Tab高度
            //$("#tabs").tabs({ height: "auto" }).height($(window).height() - 55);
            //$("div[id^=tabs-]").height($(window).height() - 40);
        });

        function getSatationList() {

            $.ajax({
                url: "../../Data/GetBaseData.ashx",
                data: { flag: "AirStation" },
                dataType: "json",
                success: function (result) {
                    var content = "";
                    if (result && result.DataTable && result.DataTable.length > 0) {
                        for (var i = 0; i < result.DataTable.length; i++) {
                            content += "<div class=\"radio-inline\"><label><input type=\"checkbox\" name=\"checkstation\" "
                            if (result.DataTable[i]["StationCode"] == "00") content += " checked='checked' ";
                            content += "value=\""
                            + result.DataTable[i]["StationCode"] + "\" onclick=\" changeCheck(1)\">"
                            + result.DataTable[i]["StationName"]
                            + "</label></div>"
                        }

                        $("#stationPanel").append(content);
                        //$("[name='checkstation']").change(changeCheck(1));

                        changeCheck(1);

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }
        function bindPageCount(count) {

            var countadd;
            var strli = "";
            var allcount = parseInt(document.getElementById("PageCount").value);

            var countpageprevew = (count - 1) * 10 + 1;
            if (allcount <= countpageprevew) {
                countadd = allcount;
            }
            else {

                countadd = count * 10;
                if (allcount <= countadd) {
                    countadd = allcount;

                }
            }

            if (allcount > 0) {
                if (count > 1) {
                    strli += '<li style=\"color:#9d9999\" onclick=\"bindPageLi(-1)\" onmousemove=\"this.style.color=\'blue\'" onmouseout=\"this.style.color=\'#9d9999\'"> << </li>'
                }
                for (; countpageprevew <= countadd; countpageprevew++) {
                    strli += '<li value=\"' + countpageprevew + '\" style=\"color:#9d9999\" onclick=\"clickli(this)\" onmousemove=\"this.style.color=\'blue\'" onmouseout=\"this.style.color=\'#9d9999\'">' + countpageprevew + '</li>'
                }
                if (allcount > 10 && allcount > count * 10) {
                    strli += '<li style=\"color:#9d9999\" onclick=\"bindPageLi(1)\" onmousemove=\"this.style.color=\'blue\'" onmouseout=\"this.style.color=\'#9d9999\'"> >> </li>'
                }
                $("#pageID").html(strli);
            }
            else {
                $("#pageID").empty();
            }
        }
        function bindPageLi(cou) {

            var count = parseInt(document.getElementById("CurrentPagecount").value) + cou;
            document.getElementById("CurrentPagecount").value = count;
            bindPageCount(count);
        }
        function clickli(obj) {
            var page = obj.value;
            changeCheck(page);
        }
        function changeCheck(page) {

            var checkname = document.getElementsByName("checkstation");
            var strCheck = "";
            for (i = 0; i < checkname.length; i++) {
                if (checkname[i].checked == true) {
                    strCheck += checkname[i].value + ",";
                }
            }
            var secondCheck = strCheck.substring(0, strCheck.length - 1);
            changeData(page, secondCheck);
        }

        function changeData(page, CheckName) {
            if ($('#txt_beginDateSec').val() != "") {
                var begintime = $('#txt_beginDateSec').val() + " " + $('#shour').val();
                var endtime = $('#txt_endDateSec').val() + " " + $('#thour').val();
                $.ajax({
                    url: "../../Data/GetBaseData.ashx",
                    method: 'Get',
                    data: { flag: "CheckForeCast", begin: begintime, end: endtime, model: $('#forecastmodel').val(), page: page, rows: 10, strCheckID: CheckName },
                    dataType: 'html',
                    cache: false,
                    beforeSend: function () { $("#tbodyid").html("<div class=\"NoData\" style=\"margin-top:80px;font-size:12px;\"><img src=\"../ForeCast/images/loading.gif\" />加载中...</div>"); },
                    success: function (data) {
                        var strdata = "";

                        var arr_data = eval("(" + data + ")");

                        if (arr_data.CountPage != null) {
                            document.getElementById("PageCount").value = arr_data.CountPage;
                        }
                        else {
                            document.getElementById("PageCount").value = "";
                        }
                        if (arr_data.HasData == 'yes') {
                            for (var i = 0; i < arr_data.Data.length; i++) {
                                strdata += "<tr class=\"thcss\"><td>" + arr_data.Data[i].StationName + "</td><td>" + arr_data.Data[i].MonitorTime.substring(0, 13).replace(':', '') + "</td><td>" + arr_data.Data[i].ForecastTime.substring(0, 13).replace(':', '') + "</td><td>" + arr_data.Data[i].SO2 + "</td><td>" + arr_data.Data[i].SO2AQ + "</td>";
                                strdata += "<td>" + arr_data.Data[i].NO2 + "</td><td>" + arr_data.Data[i].NO2AQI + "</td><td>" + arr_data.Data[i].CO + "</td>";
                                strdata += "<td>" + arr_data.Data[i].COAQI + "</td><td>" + arr_data.Data[i].O3 + "</td><td>" + arr_data.Data[i].O3AQI + "</td>";
                                strdata += "<td>" + arr_data.Data[i].PM10 + "</td><td>" + arr_data.Data[i].PM10AQI + "</td><td>" + arr_data.Data[i].PM25 + "</td>";
                                strdata += "<td>" + arr_data.Data[i].PM25AQI + "</td><td>" + arr_data.Data[i].AQI + "</td><td>" + arr_data.Data[i].FirstP + "</td>";
                                strdata += "<td>" + arr_data.Data[i].AirLevel + "</td></tr>";
                                //<td>" + arr_data.Data[i].AirLevelDes + "</td><td>" + arr_data.Data[i].AirColor + "</td>
                            }

                            $("#tbodyid").html(strdata);
                            hasdata = true;

                        }
                        else {
                            $("#tbodyid").html("<tr><td id='nodatatd' colspan='17' style='background:#fff; height:40px;text-align:center;'>暂无数据</td></tr>");
                            hasdata = false;
                        }

                        if (document.getElementById("CurrentPagecount").value == "") {
                            bindPageCount(1);
                        }
                        else {
                            bindPageCount(document.getElementById("CurrentPagecount").value);
                        }

                    },
                    error: function () { alert('加载错误'); }
                });
            }
        }

        function getPara() {
            debugger;
            if (!hasdata) {
                $("#nodatatd").css("color", "red");
                return false;
            }
            var checkname = document.getElementsByName("checkstation");
            var strCheck = "";
            for (i = 0; i < checkname.length; i++) {
                if (checkname[i].checked == true) {
                    strCheck += checkname[i].value + ",";
                }
            }
            $("#CheckGroup").val(strCheck.substring(0, strCheck.length - 1));
            $("#StartTime").val($('#txt_beginDateSec').val());
            $("#EndTime").val($('#txt_endDateSec').val());
            return true;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" style="background: #fff;">
        <div id="tabs" style="overflow: auto;">
            <%--<ul>
                <li><a href="#tabs-1" style="font-weight: 800; color: #456782;">站点预报</a></li>
                <li><a href="#tabs-2" style="font-weight: 800; color: #456782;">区域预报</a></li>
            </ul>--%>
           <%-- <div id="tabs-1" style="border: 1px solid #d8d8d8; border-top: none; padding: 20px; padding-top: 10px;">--%>
                <div>

                    <div id="stationPanel">
                        <label class="control-label">监测站点：</label>
                    </div>
                    <b>预测时间：  </b>
                    <input type="text" style="color: #1571a0; vertical-align: central; width: 100px;" runat="server" id="txt_beginDateSec" placeholder="Date" onchange="changeCheck(1);" />
                    <select id="shour" style="height: 24px; color: #1571a0; vertical-align: central;" onchange="changeCheck(1)">
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
                    <b>至</b>
                    <input type="text" style="color: #1571a0; vertical-align: central; width: 100px;" runat="server" id="txt_endDateSec" placeholder="Date" onchange="changeCheck(1);" />
                    <select id="thour" style="height: 24px; color: #1571a0; vertical-align: central;" onchange="changeCheck(1)">
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
                    <input type="hidden" id="forecastmodel" runat="server" value="1" />
                    <asp:Button ID="btnExp" runat="server" Text="导出" OnClientClick="return getPara()" OnClick="Button1_Click" />
                    <div id="comment" style="float:right;font-size:10px;display:block;">
                    <p>注：监测项CO浓度单位为mg/m³，除此均为ug/m³</p>
                    </div>
                    <table class="tab" id="tb_report" cellspacing="0px" cellpadding="0" style="width: 100%; padding-top: 0px;">
                        <thead>
                            <tr class="thcss">
                                <th rowspan="2">监测站点</th>
                                <th rowspan="2">预测时间</th>
                                <th rowspan="2">发布时间</th>
                                <th colspan="2">二氧化硫(SO<sub>2</sub>)</th>
                                <th colspan="2">二氧化氮(NO<sub>2</sub>)</th>
                                <th colspan="2">一氧化碳(CO)</th>
                                <th colspan="2">臭氧(O<sub>3</sub>)</th>
                                <th colspan="2">PM<sub>10</sub></th>
                                <th colspan="2">PM<sub>2.5</sub></th>
                                <th rowspan="2">空气质量<br />
                                    指数<br />
                                    (AQI)</th>
                                <th rowspan="2">首要污染物</th>
                                <th rowspan="2">空气质量<br />
                                    指数级别</th>
                                <%--<th rowspan="2">类别</th>
                    <th rowspan="2">颜色</th>--%>
                            </tr>
                            <tr class="thcss">
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
                        </thead>
                        <tbody class="tbcss" id="tbodyid">
                        </tbody>
                        <tfoot>

                            <tr>
                                <td colspan="18" id="pageID"></td>
                            </tr>

                        </tfoot>
                    </table>
                    <input type="hidden" id="PageCount" runat="server" />
                    <input type="hidden" id="CurrentPagecount" runat="server" />
                    <input type="hidden" id="CheckGroup" runat="server" />
                    <input type="hidden" id="StartTime" runat="server" />
                    <input type="hidden" id="EndTime" runat="server" />
                    <div id="container" style="margin-bottom: 10px; border: 1px solid #cfc2c2"></div>
                </div>
           <%-- </div>
            <div id="tabs-2" style="border: 1px solid #d8d8d8; border-top: none; padding: 20px; padding-top: 10px;">
                <iframe src="../shuozhou/AirForeCastNumImg.html" style="width:100%;height:100%;border:none;"></iframe>
            </div>--%>
        </div>

    </form>
</body>
</html>

