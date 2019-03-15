<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarnInfoDetail.aspx.cs" Inherits="Devoops_ForeCast_WarnInfoDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="../js/OutputPollutant.js"></script>

    <style type="text/css">
        .input {
            width: 50px;

        }

        .inputOn {
            display: block;
        }

        .StatusEdit td {
            text-align: center;
        }

            .StatusEdit td input {
                text-align: center;
                border: 1px solid #C7C7C7;
                width: 50px;
                display: block;
              
            }

            .StatusEdit td select {
                display: block;
                margin: 2px auto;
                border: 1px solid #C7C7C7;
            }

            .StatusEdit td span {
                display: none;
            }

            .StatusEdit td input.Button_Save {
                display: block;
                border: 1px solid #C7C7C7;
                width: 75px;
                      float: left;
                     margin-left:10px;
                margin-right:10px;

            }

            .StatusEdit td input.Button_Add {
                display: block;
                border: 1px solid #C7C7C7;
                width: 75px;
                float: left;
                margin-left:10px;
                margin-right:10px;
            }

            .StatusEdit td input.Button_Closed {
                border: 1px solid #C7C7C7;
                width: 75px;
                display: block;
            }

            .StatusEdit td input.Button_Edit {
                width: 75px;
                display: none;
            
                border: 1px solid #C7C7C7;
            }

            .StatusEdit td input.Button_Del {
                width: 75px;
                display: none;
            }

        .StatusSave td {
            text-align: center;
            height: 22px;
        }

            .StatusSave td input {
                width: 50px;
                display: none;
            }

            .StatusSave td select {
                display: none;
            }

            .StatusSave td span {
                display: block;
            }

            .StatusSave td input.Button_Save {
                border: 1px solid #C7C7C7;
                width: 75px;
                display: none;
            }

            .StatusSave td input.Button_Closed {
                width: 75px;
                display: none;
                border: 1px solid #C7C7C7;

            }

            .StatusSave td input.Button_Del {
                display: block;
                border: 1px solid #C7C7C7;
                width: 75px;
                /*float: right;*/
            }

            .StatusSave td input.Button_Edit {
                display: block;
                border: 1px solid #C7C7C7;
                width: 75px;
                float: left;
                     margin-left:10px;
                margin-right:10px;
            }


    </style>
    <style>
        body {
            background-color: white;
        }

        p {
            font-size: 14px;
        }

        img {
            height: 100px;
            width: 75px;
        }

        #reportlist li {
            border: solid;
            border-width: 1px;
            border-color: #C7C7C7;
            padding-left: 0px;
            padding-right: 5px;
            padding-top: 5px;
            margin-left: 5px;
            margin-bottom: 10px;
            float: left;
            list-style: none;
            cursor: pointer;
        }

        li:hover {
            background: #6596BD;
            border: 1px solid #003399;
            color: white;
        }

        #TimePicker li {
            border: solid;
            border-width: 1px;
            border-color: #C7C7C7;
            padding-left: 2px;
            padding-right: 2px;
            padding-top: 2px;
            margin-left: 5px;
            float: left;
            list-style: none;
            cursor: pointer;
        }

        .CS th {
            font-weight: normal;
            text-align: center;
            /*font-size:large;*/
        }

        #pageIDSecond li {
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
            width: 25px;
            height: 25px;
            text-align: center;
            vertical-align: central;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            var myDate = new Date();
            var h = myDate.getHours();

            getDateData();
            bingHour(h);
            //hourBind("");

            document.getElementById("CurrentPagecountSecond").value = 1;


        });
        function getDateData() {

            $.ajax({
                url: '../../Data/GetForCastWarnInfoData.ashx',
                method: 'Get',
                data: { mode: "region" },
                dataType: 'html',
                cache: false,

                success: function (data) {
                    if (data != null && data != "")
                        $("#olID").html(data);
                },
                error: function () { alert('错误'); }
            });


        }


        function bingHour(h) {
            var strselect = "";
            for (var i = 0; i < 23; i++) {
                strselect += '<option id=\"' + i + '\" value=\"' + i + '\">' + i + '</option>';
            }

            $("#lihourid").html(strselect);
            $("#secseid").html(strselect);
            $("#lihourid").find("option[value='" + h + "']").attr("selected", true);
            $("#secseid").find("option[value='" + h + "']").attr("selected", true);
        }

        function onclicli(obj) {

            var Name = obj.innerText;
            document.getElementById("RegionNameSecond").value = Name;
            hourBind(1, Name);
        }
        function bindPageCountSecond(count) {


            var countadd;
            var strli = "";
            var allcount = parseInt(document.getElementById("PageCountSecond").value);

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
            if (count > 1) {
                strli += '<li style=\"color:#9d9999\" onclick=\"bindPageLiSecond(-1)\" onmousemove=\"this.style.color=\'blue\'" onmouseout=\"this.style.color=\'#9d9999\'"> << </li>'
            }
            for (; countpageprevew <= countadd; countpageprevew++) {
                strli += '<li value=\"' + countpageprevew + '\" style=\"color:#9d9999\" onclick=\"clickliSecond(this)\" onmousemove=\"this.style.color=\'blue\'" onmouseout=\"this.style.color=\'#9d9999\'">' + countpageprevew + '</li>'
            }
            if (allcount > 10 && allcount > count * 10) {
                strli += '<li style=\"color:#9d9999\" onclick=\"bindPageLiSecond(1)\" onmousemove=\"this.style.color=\'blue\'" onmouseout=\"this.style.color=\'#9d9999\'"> >> </li>'
            }
            $("#pageIDSecond").html(strli);
        }
        function bindPageLiSecond(cou) {

            var count = parseInt(document.getElementById("CurrentPagecountSecond").value) + cou;
            document.getElementById("CurrentPagecountSecond").value = count;
            bindPageCountSecond(count);
        }
        function clickliSecond(obj) {
            var page = obj.value;
            var region = document.getElementById("RegionNameSecond").value;

            hourBind(page, region);
        }

        function hourBind(currentpage, name) {
           
            var date = document.getElementById("set_date").value;
            var datesec = document.getElementById("second_date").value;
            var hourF = document.getElementById("lihourid").value;
            var hourS = document.getElementById("secseid").value;


            $.ajax({
                url: '../../Data/GetForCastWarnInfoData.ashx',
                method: 'Get',
                data: { mode: "QryWarnDetail", regionName: name, date: date, hourF: hourF, hourS: hourS, datesec: datesec, page: currentpage, rows: 5 },
                dataType: 'html',
                cache: false,

                beforeSend: function () { $("#tbdetailid").html("<div class=\"NoData\" style=\"margin-top:80px;font-size:12px;\"><img src=\"../ForeCast/images/loading.gif\" />加载中...</div>"); },
                success: function (data) {
                    var strdata = "";

                 
                    var arr_data = eval("(" + data + ")");
                    
                    document.getElementById("PageCountSecond").value = arr_data.CountPageSec;
                    if (arr_data.HasData == 'yes') {

                        for (var i = 0; i < arr_data.Data.length; i++) {
                            strdata += "<tr><td>" + arr_data.Data[i].StationName + "</td>";
                            strdata += " <td>" + arr_data.Data[i].AirLevelDes + "</td>";
                            strdata += " <td>" + arr_data.Data[i].AirColor + "</td>";
                            strdata += " <td>" + arr_data.Data[i].FirstP + "</td>";
                            strdata += " <td>" + arr_data.Data[i].ForecastTime + "</td>";
                            strdata += " <td>" + arr_data.Data[i].MonitorTime + "</td>";
                            strdata += " <td>" + arr_data.Data[i].AQI + "</td>";
                            strdata += " <td>" + arr_data.Data[i].SO2 + "</td>";
                            strdata += " <td>" + arr_data.Data[i].NO2 + "</td><td>" + arr_data.Data[i].CO + "</td><td>" + arr_data.Data[i].O3 + "</td><td>" + arr_data.Data[i].PM25 + " </td><td>" + arr_data.Data[i].PM10 + "</td></tr>";

                        }
                        $("#tbdetailid").html(strdata);

                        if (document.getElementById("CurrentPagecountSecond").value == "") {
                            bindPageCountSecond(1);
                        }
                        else {
                            bindPageCountSecond(document.getElementById("CurrentPagecountSecond").value);
                        }
                        $("#pageIDSecond").parent().show();
                    } else {
                        $("#pageIDSecond").parent().hide();
                        $("#tbdetailid").html($("<tr><td colspan='13' style='background:#fff; height:40px;text-align:center;'>暂无数据</td></tr>"));
                    }
                   

                },

                error: function () { alert('加载错误'); }
            });
        }

    </script>

</head>
<body style="background:#fff;">
    <form id="form1" runat="server" style="overflow-y: auto;">
        <div id="TimePicker" class="well" style="background-color: white; padding-top: 0px; height: auto">

            <div style="padding-top: 3px; height: 20px">
                <div style="width: 100%; padding-top: 2px;">
                    <label class="col-sm-1 control-label" style="padding-right: 0px; width: auto">日期：</label>
                    <div class="col-sm-2" style="padding-left: 0px; width: 120px;">
                        <input type="text" id="set_date" style="width: 120px;" class="form-control" placeholder="Date" />

                    </div>


                    <div class="col-sm-2" style="padding-left: 0px; width: 50px">
                        <select id="lihourid" style="float: left; border-color: #C7C7C7;">
                        </select>
                    </div>

                    <label class="col-sm-1 control-label" style="padding-right: 0px; width: auto">至 &nbsp;&nbsp;</label>
                    <div class="col-sm-2" style="padding-left: 0px; width: 120px">
                        <input type="text" id="second_date" style="width: 120px" class="form-control" placeholder="Date" />
                        &nbsp;
                    </div>

                    <select id="secseid" style="float: left; border-color: #C7C7C7;">
                    </select>


                </div>
                <p style="float: left; padding-left: 30px; margin-left: 30px;">监测区域：</p>
                <ol id="olID" style="float: left; padding-left: 0px; padding-top: -20px;">
                </ol>
            </div>

        </div>
    
            <table class="tab" style="background:#fff;" >
                    <tr>
                        <th>监测区域</th>
                        <th>空气状况</th>
                        <th>预警颜色</th>
                        <th>首要污染物</th>
                        <th>发生时间</th>
                        <th>预测时间</th>
                        <th>AQI</th>
                        <th>SO2</th>
                        <th>NO2</th>
                        <th>CO</th>
                        <th>O3</th>
                        <th>PM2.5</th>
                        <th>PM10</th>

                    </tr>
                <tbody id="tbdetailid">
                </tbody>
                <tfoot>
                    <tr style="height: 30px;">
                        <td colspan="13" id="pageIDSecond" style="height: 30px;">暂无数据</td>
                    </tr>
                </tfoot>

            </table>

        

            <table class="tab" id="datatable-2" style="background:#fff; line-height:50px; margin-top:10px;">
                    <tr>
                        <th>监测区域</th>
                        <th>预警等级</th>
                        <th>AQI范围值</th>
                        <th>首要污染物</th>
                        <th style=" width:200px;">操作</th>
                    </tr>
                <%=_DetailHtml %>
            </table>
       
        <input type="hidden" id="RegionNameSecond" runat="server" />
        <input type="hidden" id="PageCountSecond" runat="server" />
        <input type="hidden" id="CurrentPagecountSecond" runat="server" />
    </form>
</body>
<script type="text/javascript">
    $(function () {

        $('.select2-choice li').click(function () {
            $('.anserdh li').select
            $(this).parent().addClass('qhbg');

        })

    })
    //$("#dialog").css({ height: $(window).height() - 200 });
    $("#detailid").css({ height: $(window).height() - 360 });
    $("#form1").css({ height: $(window).height() });
    $(window).resize(function () {
        $("#form1").css({ height: $(window).height() });
        $("#detailid").css({ height: $(window).height() - 360 });
        $("#dialog").css({ height: $(window).height() });
    });

</script>
</html>
