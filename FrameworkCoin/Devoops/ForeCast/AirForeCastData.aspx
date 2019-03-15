<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirForeCastData.aspx.cs" Inherits="Devoops_ForeCast_AirForeCastData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

        .check {
            margin-left: 0px;
        }
    </style>
    <script type="text/javascript">
        var hasdata = false;
        $(function () {
            getSatationList();
            document.getElementById("CurrentPagecount").value = 1;
            $('#txt_beginDateSec').val(new Date().addDays(-0).format("yyyy-MM-dd"));
            $('#txt_beginDateSec').datepicker({ showOn: "click" });
            $('#txt_endDateSec').val(new Date().addDays(2).format("yyyy-MM-dd"));
            $('#txt_endDateSec').datepicker({ showOn: "click" });
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
                            content += "<div class=\"radio-inline\"><label><input class=\"check\" type=\"checkbox\" name=\"checkstation\" "
                            if (result.DataTable[i]["StationCode"] == "00") content += " checked='checked' ";
                            content += "value=\""
                            + result.DataTable[i]["StationCode"] + "\" onclick=\" changeCheck(1)\">"
                            + result.DataTable[i]["StationName"]
                            + "</label></div>";
                        }

                        $("#stationPanel").append(content + "&nbsp;&nbsp;&nbsp;");
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
                $.ajax({
                    url: "../../Data/GetBaseData.ashx",
                    method: 'Get',
                    data: { flag: "CheckForeCast", begin: $('#txt_beginDateSec').val(), end: $('#txt_endDateSec').val(), model: $('#forecastmodel').val(), page: page, rows: 10, strCheckID: CheckName },
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
                                strdata += "<tr class=\"thcss\"><td>" + arr_data.Data[i].StationName + "</td><td>" + arr_data.Data[i].MonitorTime.substring(0, 10) + "</td><td>" + arr_data.Data[i].SO2 + "</td><td>" + arr_data.Data[i].SO2AQ + "</td>";
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
                            hasdata = false;
                            $("#tbodyid").html("<tr ><td id='nodatatd' colspan='17' style='text-align:center; height:40px;'>暂无数据</td></tr>");
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

        function ExportData() {
            //openWindow("../../Utils/ExcelExport.aspx");
            openPostWindow("", $("#tbdiv").html(), "../../Utils/ExcelExport.aspx");

            return;
            var checkname = document.getElementsByName("checkstation");
            var strCheck = "";
            for (i = 0; i < checkname.length; i++) {
                if (checkname[i].checked == true) {
                    strCheck += checkname[i].value + ",";
                }
            }
            var secondCheck = strCheck.substring(0, strCheck.length - 1);

            if ($('#txt_beginDateSec').val() != "") {
                $.ajax({
                    url: "../../Data/GetBaseData.ashx",
                    method: 'get',
                    data: { flag: "CheckForeCastExport", html: $("#tbdiv").html(), begin: $('#txt_beginDateSec').val(), end: $('#txt_endDateSec').val(), model: $('#forecastmodel').val(), page: 1, rows: 100000, strCheckID: secondCheck },
                    dataType: 'html',
                    cache: false,
                    success: function (data) { },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        debugger;
                        alert('加载错误');
                    }
                });
            }
        }


        function openPostWindow(url, data, name) {
            debugger;
            var tempForm = document.createElement("form");
            tempForm.id = "tempForm1";
            tempForm.method = "post";
            tempForm.action = url;
            tempForm.target = name;

            var hideInput = document.createElement("input");
            hideInput.type = "hidden";
            hideInput.name = "content"
            hideInput.value = data;
            tempForm.appendChild(hideInput);
            tempForm.attachEvent("onsubmit", function () { openWindow(name); });
            document.body.appendChild(tempForm);

            tempForm.fireEvent("onsubmit");
            tempForm.submit();
            document.body.removeChild(tempForm);
        }

        function openWindow(name) {
            window.open(name);
        }

        function getPara() {
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
    <form id="form1" runat="server">

        <div style="background: #fff;">
            <div id="stationPanel">
                <label class="control-label">监测站点：</label>
            </div>
            <div>
                <b>预测时间：  </b>
                <input type="text" style="color: #1571a0; vertical-align: central; width: 100px;" runat="server" id="txt_beginDateSec" placeholder="Date" onchange="changeCheck(1)" />
                <b>至</b>
                <input type="text" style="color: #1571a0; vertical-align: central; width: 100px;" runat="server" id="txt_endDateSec" placeholder="Date" onchange="changeCheck(1)" />

                <input type="hidden" id="forecastmodel" value="0" />
                <asp:Button ID="Button1" runat="server" Text="导出" OnClientClick="return getPara()" OnClick="Button1_Click" />

            </div>
            <div id="tbdiv">
                <table class="tab" id="tb_report" cellspacing="0px" cellpadding="0" style="width: 100%; padding-top: 0px;">
                    <thead>
                        <tr class="thcss">
                            <th rowspan="2">监测站点</th>
                            <th rowspan="2">预测时间</th>
                            <th colspan="2">二氧化硫(SO<sub>2</sub>)</th>
                            <th colspan="2">二氧化氮(NO<sub>2</sub>)</th>
                            <th colspan="2">一氧化碳(CO)</th>
                            <th colspan="2">臭氧(O<sub>3</sub>)</th>
                            <th colspan="2">PM<sub>10</sub></th>
                            <th colspan="2">PM<sub>2.5</sub></th>
                            <th rowspan="2">空气质量指数<br />
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
                            <th>浓度</th>
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
            </div>
            <input type="hidden" id="PageCount" runat="server" />
            <input type="hidden" id="CurrentPagecount" runat="server" />
            <input type="hidden" id="CheckGroup" runat="server" />
            <input type="hidden" id="StartTime" runat="server" />
            <input type="hidden" id="EndTime" runat="server" />
            <div id="container" style="margin-bottom: 10px; border: 1px solid #cfc2c2"></div>
        </div>
    </form>
</body>
</html>
