<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConCirculat.aspx.cs" Inherits="Devoops_ConsulCir_ConCirculat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../plugins/jquery-ui/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../../Demo/plugins/bootstrap/bootstrap.css" rel="stylesheet" />
    <link href="../css/font-awesome.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.7.2.js"></script>
    <script type="text/javascript" src="../js/ devoops.js"></script>
    <script type="text/javascript" src="../../Controls/Highcharts-4.0.1/highcharts.js"></script>
    <script type="text/javascript" src="../js/exporting.js"></script>
    <script type="text/javascript" src="../plugins/jquery-ui/jquery-ui.js"></script>
    <script type="text/javascript" src="../js/method.js"></script>
    <link href="../js/MultiSelect/jquery.multiselect.css" rel="stylesheet" />
    <link href="../js/MultiSelect/demos/assets/jquery-ui.css" rel="stylesheet" />
    <script src="../js/MultiSelect/src/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/MultiSelect/src/jquery.multiselect.js" type="text/javascript"></script>


    <style type="text/css">
        .des {
            font-size: smaller;
        }

        html, body {
            margin: 0px 0px 0px 0px;
            height: 100%;
            overflow: hidden;
            font-size: 12px;
        }
    </style>
    <script type="text/javascript">
        function WasSave(result, yj, jc) {
            debugger;
            if (result == 'OK') {

                alert("保存成功！");

            } else if (result == "modify") {
                alert("保存成功！");
            }
            else {
                alert("保存失败！");
            }

            if (jc == "True") {
                parent.TRansIframe('ConsulCir/PollutionDayliyReport.aspx?ch=1&level=' + yj + '&ReportCode=' + '<%=strCode%>', '重污染天气');
            }
            if (yj != "" && yj != undefined) {
                parent.TRansIframe('ConsulCir/PollutionDayliyReport.aspx?ch=0&level=' + yj + '&ReportCode=' + '<%=strCode%>', '重污染天气');
            }
        }
        $(window).resize(function () {
            $("#frm").css({ height: $(window).height() - 5 });
        });
        $(function () {
            if ("<%=ShowRpt%>" == "true") {
                $("#div_rpb").show();
            }
            $("#hidenTime").val(new Date().format("yyyy-MM-dd"));

            $("body").on('click', '.collapse-link', function (e) {
                e.preventDefault();
                var box = $(this).closest('div.box');
                var button = $(this).find('i');
                var content = box.find('div.box-content');
                content.slideToggle('fast');
                box.css({ "height": "auto" });
                button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
                setTimeout(function () {
                    box.resize();
                    box.find('[id^=map-]').resize();
                }, 50);
            })
            $("body").on('click', '.expand-link', function (e) {
                var body = $('body');
                e.preventDefault();
                var box = $(this).closest('div.box');
                var button = $(this).find('i');
                button.toggleClass('fa-expand').toggleClass('fa-compress');
                box.toggleClass('expanded');
                body.toggleClass('body-expanded');
                var timeout = 0;
                if (body.hasClass('body-expanded')) {
                    timeout = 100;
                }
                setTimeout(function () {
                    box.toggleClass('expanded-padding');
                }, timeout);
                setTimeout(function () {
                    box.resize();
                    box.find('[id^=map-]').resize();
                }, timeout + 50);
            })
            $("body").on('click', '.close-link', function (e) {
                e.preventDefault();
                var content = $(this).closest('div.box');
                content.remove();
            });
            InitMultiSelect('ddl_day', 'hd_day');
            InitMultiSelect('ddl_day1', 'hd_day1');
            InitMultiSelect('ddl_day2', 'hd_day2');

            bindFirstP('ddl_day', 'hd_day');
            bindFirstP('ddl_day1', 'hd_day1');
            bindFirstP('ddl_day2', 'hd_day2');
        });
            //初始化多选框控件
            function InitMultiSelect(ddlName, hdName) {
                $("#" + ddlName).multiselect({
                    noneSelectedText: "(无)",
                    checkAllText: "全选",
                    uncheckAllText: '全不选',
                    minWidth: 180,
                    height: 95,
                    selectedList: 6,
                    beforeopen: function (event, ui) {
                        var selected = $("select[id=" + ddlName + "] option:selected");
                        $("select[id=" + ddlName + "]").prepend(selected);
                        $("#" + ddlName).multiselect('refresh');
                    },
                    close: function () {
                        $("#" + hdName).val($("#" + ddlName).val());
                    }
                });
            }
            //绑定首要污染物默认选中 chenhj
            //ddlname下拉框名称 ，hdname隐藏域名称
            function bindFirstP(ddlname, hdname) {
                var v = $("#" + hdname).val().replace(';', ',');
                if (v.substr(v.length - 1, 1) == ',')
                    v = v.substr(0, v.length - 1);
                v = v.split(',');
                if (v != null) {
                    $("#" + ddlname).val(v);
                    $("#" + ddlname).multiselect("refresh");
                }
            }
            var _DG = frameElement.lhgDG;
            //添加
            function bindreginpo(data) {

                var jsonData = eval("(" + data + ")");
                if (jsonData.HasData == 'yes') {
                    var regionF = "";
                    var pollutF = "";
                    for (var i = 0; i < jsonData.region.length; i++) {
                        regionF += ("<option value='" + jsonData.region[i].Code + "'>" + jsonData.region[i].Name + "</option>");
                    }
                    $("#pollutAgion").html(regionF);
                    $("#pollutAgion").find("option[value='" + jsonData.region[0].Code + "']").attr("selected", true);
                    for (var i = 0; i < jsonData.pollution.length; i++) {
                        pollutF += ("<option value='" + jsonData.pollution[i].Code + "'>" + jsonData.pollution[i].Name + "</option>");
                    }
                    $("#pollutantid").html(pollutF);

                    $("#pollutantid").find("option[value='" + jsonData.pollution[0].Code + "']").attr("selected", true);
                    GetPollutDataChart();

                }
            }

    </script>
    <style type="text/css">
        .Main_Tab_Style_Bgcolor {
            background-color: #eeeeee;
            padding-right: 10px;
            color: #000000;
            text-align: right;
        }

        .box {
            margin-bottom: 5px;
        }

        .tab {
            word-break: keep-all;
            border: 1px solid #EBEBEB;
            border-bottom: none;
            border-left: none;
            border-top-left-radius: 6px;
            width: 100%;
            margin-top: 10px;
        }

            .tab td {
                border-bottom: 1px solid #EBEBEB;
                border-left: 1px solid #EBEBEB;
                padding: 5px;
                line-height: 25px;
                color: #41A7E2;
                cursor: pointer;
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

        .btnCls {
            vertical-align: middle;
            text-align: center;
            font-size: small;
        }


        .inputID {
            width: 70px;
            height: 24px;
            text-align: left;
            border: 1px solid;
            border-color: #b7b5b5;
        }
    </style>
</head>
<body style="background: #EBEBEB;">
    <form id="frm" runat="server" style="overflow-y: auto; overflow-x: hidden; font-size: 15px;">
        <div class="box" style="top: 5px;">
            <div class="box-header">
                <div class="box-name">
                    <i class="fa fa-search"></i>
                    <span><b class="tab">预报结果回顾</b></span>
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
            <div class="box-content" style="height: 140px;">
                <div id="div_Container1" style="margin-top: -10px;">
                </div>
            </div>
        </div>
        <div class="box">
            <div class="box-header">
                <div class="box-name">
                    <i class="fa fa-bar-chart-o"></i>
                    <span><b class="tab">
                        <asp:Label ID="lbl_time" runat="server" Text="Label"></asp:Label>空气质量监测</b></span>
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
                <div style="text-align: right">
                    <select id="pollutAgion" name="D1">
                    </select>
                    <select id="pollutantid">
                    </select>
                </div>
                <div id="container" style="height: 240px;">
                </div>
                0时至9时均值：
                <div id="tbreport" runat="server" style="margin-top: 10px;">
                </div>
            </div>
        </div>

        <div class="box">
            <div class="box-header">
                <div class="box-name">
                    <i class="fa fa-list-ul"></i>
                    <span><b class="tab">空气质量预测</b></span>
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
            <div class="box-content" style="height: 390px;">
                <div class="box-content" style="padding-left: 5px; padding-right: 5px; padding-bottom: 0px; padding-top: 5px;">

                    <div id="tabs" style="overflow: auto; margin-top: -10px;">
                        <ul>
                            <li><a href="#tabs-1" style="font-weight: 800; font-size: 14px; color: #456782;">城市</a></li>
                            <li><a href="#tabs-2" style="font-weight: 800; font-size: 14px; color: #456782;">城区点位</a></li>
                            <li><a href="#tabs-3" style="font-weight: 800; font-size: 14px; color: #456782;">区县点位</a></li>
                            <li style="float: right; text-align: right; width: 600px; color: black; font-size: 16px; font-weight: 500;">预报模式：
                                <select id="ddl_model" style="width: 100px;">
                                    <option value="tj">统计预报</option>
                                    <option value="1">数值预报</option>
                                </select>
                                时段：
                                <select id="ddl_time" style="width: 100px;">
                                    <option value="0">全天</option>
                                    <option value="1">时段</option>
                                </select>
                                <span style="font-size: 12px;">注：监测项CO浓度单位为mg/m³，除此均为ug/m³</span></li>
                        </ul>
                        <div id="tabs-1" style="border: 1px solid #d8d8d8; border-top: none; font-size: 14px; font-weight: 500;">
                        </div>
                        <div id="tabs-3" style="border: 1px solid #d8d8d8; border-top: none; font-size: 14px; font-weight: 500;">
                        </div>
                        <div id="tabs-2" style="border: 1px solid #d8d8d8; border-top: none; font-size: 14px; font-weight: 500;">
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div class="box">
            <div class="box-header">
                <div class="box-name">
                    <i class="fa fa-list-ul"></i>
                    <span><b class="tab">气象监测情况</b></span>
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
                <textarea id="taID" rows="3" runat="server" cols="20" style="width: 100%; height: 150px;"></textarea>
                <div style="text-align: right; padding: 5px;">
                    <input id="btnSave" type="button" value="保存" onclick="btnSaveClick()" />&nbsp;&nbsp;
                </div>
            </div>
        </div>
        <div class="box">
            <div class="box-header">
                <div class="box-name">
                    <i class="fa fa-table"></i>
                    <span><b class="tab">会商结论</b></span>
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
                <table id="Table1" class="table table-striped">
                    <thead>
                        <tr>
                            <th style="width: 15%;">日期</th>
                            <th style="width: 15%;">AQI</th>
                            <th style="width: 15%;">空气质量级别</th>
                            <th style="width: 15%;">首要污染物</th>
                            <th style="width: 55%;">出行建议</th>
                        </tr>
                    </thead>


                    <tr>
                        <td id="Td6" runat="server"><%=ReportDate.Month.ToString() %>月<%=ReportDate.Day.ToString() %>日</td>
                        <td align="left">
                            <input name="txt_td" onchange="BindddlData('txt_td','ddlQuauLeveltd','laQuauLeveltd')" class="inputID" id="txt_td" type="text" runat="server" /></td>
                        <td id="Td7" style="width: 100px;" runat="server">
                            <asp:DropDownList ID="ddlQuauLeveltd" onchange="SelectedChanged('td')" runat="server" OnSelectedIndexChanged="ddlQuauLeveltd_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_day" runat="server" multiple='multiple' OnSelectedIndexChanged="ddl_day_SelectedIndexChanged"></asp:DropDownList>
                            <asp:HiddenField ID="hd_day" runat="server" Value="" OnValueChanged="hd_day_ValueChanged" />
                        </td>
                        <td id="Td8" runat="server" class="des">
                            <asp:Label ID="laQuauLeveltd" runat="server" Height="50"></asp:Label></td>
                    </tr>
                    <tr>
                        <td id="Td12" runat="server"><%=ReportDate.Month.ToString() %>月<%=ReportDate.AddDays(1).Day.ToString() %>日</td>
                        <td align="left">
                            <input class="inputID" name="txt_su" id="txt_su" onchange="BindddlData('txt_su','ddlQuauLevelsu','laQuauLevelsu')" type="text" runat="server" /></td>
                        <td id="Td13" runat="server">
                            <asp:DropDownList ID="ddlQuauLevelsu" onchange="SelectedChanged('su')" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_day1" runat="server" multiple='multiple'></asp:DropDownList>&nbsp;
                            <asp:HiddenField ID="hd_day1" runat="server" Value="" />
                        </td>
                        <td id="Td14" runat="server" class="des">
                            <asp:Label ID="laQuauLevelsu" runat="server" Height="50"></asp:Label></td>
                    </tr>
                    <tr>
                        <td id="Td21" runat="server"><%=ReportDate.Month.ToString() %>月<%=ReportDate.AddDays(2).Day.ToString() %>日</td>
                        <td align="left">
                            <input class="inputID" name="txt_tfu" id="txt_tfu" onchange="BindddlData('txt_tfu','ddlQuauLeveltfu','laQuauLeveltfu')" type="text" runat="server" /></td>
                        <td id="Td22" style="width: 100px;" runat="server">
                            <asp:DropDownList ID="ddlQuauLeveltfu" onchange="SelectedChanged('tfu')" runat="server">
                            </asp:DropDownList>
                        </td>

                        <td>
                            <asp:DropDownList ID="ddl_day2" runat="server" ClientIDMode="Static" multiple='multiple'></asp:DropDownList>
                            <asp:HiddenField ID="hd_day2" runat="server" Value="" />
                        </td>
                        <td class="des">
                            <asp:Label ID="laQuauLeveltfu" runat="server" Height="50"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:CheckBox ID="chk_tsh" runat="server" />&nbsp;是否特殊天气（包括沙尘暴、扬沙、浮尘、首要污染物为臭氧）
                    &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="chk_jch" runat="server" />&nbsp;是否解除预警
                  
                        </td>
                    </tr>
                    <tr>
                        <td id="Td29" style="width: 100px;" runat="server"></td>
                        <td id="Td30">&nbsp;</td>

                        <td id="Td31">&nbsp;</td>

                        <td></td>
                        <td id="btn1" width="100" height="21" style="text-align: right; padding: 5px; clip: rect(auto, auto, auto, auto);">
                            <asp:Button ID="btn_save" CssClass="btnCls" runat="server" Text="保存" OnClientClick="return IsSubmit('false')" OnClick="btn_save_Click" Width="42px" />&nbsp;&nbsp;
                            <asp:Button ID="btn_submit" CssClass="btnCls" runat="server" Text="提交" OnClientClick="return IsSubmit('true')" OnClick="btn_save_Click" Width="42px" />&nbsp;&nbsp;
                            <%--<input id="btn_setup" class="btnCls" type="button" value="提交" onclick="BtnSaveUp()" />--%>
                            <asp:HiddenField ID="hidenTime" runat="server" Value="" />
                            <asp:HiddenField ID="waitSubmit" runat="server" Value="" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
    <script type="text/javascript">
        $(window).resize(function () {
            $("#ParentFrame").height($(window).height() - 130);
        });

        $(document).ready(function () {
            GetStatData("yprocast");
            GetStatData("hourprac");
            GetStatData("selectQXText");
            $("#hidenTime").val(new Date().format("yyyy-MM-dd"));
            BindChart();
            //设置Tab高度
            $("#tabs").tabs({ height: "380px" })
            $("div[id^=tabs-]").height(340);
            initList();

            $("#ddl_time,#ddl_model").change(function () {
                initList();
            });
        });

        function initList() {

            $("#tabs-1").empty().load("../../Data/GetForecastData_Huish.ashx", { cate: "city", model: $("#ddl_model").val(), time: $("#ddl_time").val(), ID: new Date().getMilliseconds() }, null);
            $("#tabs-2").empty().load("../../Data/GetForecastData_Huish.ashx", { cate: "point", model: $("#ddl_model").val(), time: $("#ddl_time").val(), ID: new Date().getMilliseconds() }, null);
            $("#tabs-3").empty().load("../../Data/GetForecastData_Huish.ashx", { cate: "region", model: $("#ddl_model").val(), time: $("#ddl_time").val(), ID: new Date().getMilliseconds() }, null);
        }

        function GetStatData(strstyle) {
            $.ajax({
                url: '../../Data/GetConCirculatData.ashx',
                method: 'Get',
                data: { stype: strstyle },
                dataType: 'html',
                cache: false,
                beforeSend: function () { $("#div_Container1").html("<div class=\"NoData\" style=\"margin-top:80px;font-size:12px;\"><img src=\"../ForeCast/images/loading.gif\" />加载中...</div>"); },
                success: function (data) {
                    try {
                        if (data != "") {
                            if (strstyle == "yprocast") {
                                $("#div_Container1").html(data);
                            }
                            else if (strstyle == "hourprac") {
                                $("#div_Container2").html(data);
                            }
                            else if (strstyle == "selectQXText") {
                                var data = eval(data);
                                document.getElementById("taID").value = data[0].QXText;
                                document.getElementById("chk_jch").checked = data[0].SFJCH == "True" ? true : false;
                                document.getElementById("chk_tsh").checked = data[0].SFTSH == "True" ? true : false;

                            }

                        }
                    }
                    catch (e) {
                    }
                },
                error: function () { alert('加载错误'); }
            });
        }
        $(document).ready(function () {

            var strSession = "<%=Session["departID"] %>".toString();
            if (strSession != "") {
                $.ajax({
                    url: '../../Data/GetConCirculatData.ashx',
                    method: 'Get',
                    data: { stype: "selectsub" },
                    dataType: 'html',
                    cache: false,
                    success: function (data) {
                        try {
                            if (data) {
                                if (data == "已提交") {
                                    submited();
                                }
                                else if (data == "监测站登录") {
                                    document.getElementById("btnSave").style.display = "none";
                                    document.getElementById("taID").style.background = "#c3bcbc";
                                    document.getElementById("taID").readOnly = true;
                                    document.getElementById("btn_save").value == "修改"
                                }
                                else if (data == "气象台登录") {
                                    document.getElementById("btn_setup").style.display = "none";
                                    document.getElementById("btn_save").style.display = "none";
                                    ddlvisibleBind();

                                }
                                else if (data == "监测站登录没保存") {
                                    document.getElementById("btnSave").style.display = "none";
                                    document.getElementById("taID").style.background = "#c3bcbc";
                                    document.getElementById("taID").readOnly = true;
                                    document.getElementById("btn_save").value == "保存";
                                }

                                else if (data == "气象台登录没保存") {
                                    document.getElementById("btn_save").style.display = "none";
                                    document.getElementById("btn_setup").style.display = "none";
                                    ddlvisibleBind();
                                }
                            }
                        }
                        catch (e) {
                        }


                    },
                    error: function () { alert('设置角色出现形式错误'); }
                });
            }
            else {
                window.parent.location.href = "../Login.aspx";
            }
        });

            function BtnSaveUp() {
                debugger;
                if (document.getElementById("btn_save").value == "修改") {

                    $.ajax({
                        url: '../../Data/GetConCirculatData.ashx',
                        method: 'Get',
                        data: { stype: "submit" },
                        dataType: 'html',
                        cache: false,
                        success: function (data) {

                            if (data == "提交成功")
                                submited();
                            document.getElementById("btn_save").click();
                            // alert("提交成功");
                        },
                        error: function () { alert('提交错误'); }
                    });
                }

            }
            var arrobj = ['td', 'su', 'tfu'];

            function tranPage() {

            }
            function ddlvisibleBind() {

                for (var i = 0; i < arrobj.length; i++) {
                    var strddl = "ddlQuauLevel" + arrobj[i];
                    var txtname = "txt_" + arrobj[i];
                    if (arrobj[i] == "tu") {
                        txtname = "txt_";
                    }

                    // document.getElementById(txtname).readOnly = "true";
                    // document.getElementById(strddl).disabled = "disabled";
                }

            }

            function BindddlData(txtID, selectID, laQuauLevel) {
                var txtdata = document.getElementById(txtID).value;
                $.ajax({
                    url: '../../Data/GetConCirculatData.ashx',
                    method: 'Get',
                    data: { stype: "txtddl", txtdata: txtdata },
                    dataType: 'html',
                    cache: false,
                    beforeSend: function () { $("txtID").html(""); },
                    success: function (data) {
                        var arr_data = eval("(" + data + ")");
                        if (arr_data.HasData == 'yes') {

                            document.getElementById(selectID).value = arr_data.selelevel;
                            document.getElementById(laQuauLevel).innerText = arr_data.procal;
                        }

                    },
                    error: function () { alert('加载错误'); }
                });

            }

            function submited() {
                // document.getElementById("btn_setup").value = "已提交";
                //   document.getElementById("btn_setup").disabled = "disabled";
                // document.getElementById("btn_save").style.display = "none";
                //  document.getElementById("btnSave").style.display = "none";
                //  document.getElementById("taID").readOnly = true;
                //  document.getElementById("taID").style.background = "#c3bcbc";
                ddlvisibleBind();


            }

            function btnSaveClick() {
                if (document.getElementById("btnSave").value == "编缉") {
                    document.getElementById("btnSave").value = "保存";
                    document.getElementById("taID").readOnly = false;
                    document.getElementById("taID").style.background = "#fff";

                }
                else {
                    save();
                }

            }
            function save() {
                var QXTextdata = $('#taID').val();
                $.ajax({
                    url: '../../Data/GetConCirculatData.ashx',
                    method: 'Get',
                    data: { stype: "QXtext", QXText: QXTextdata },
                    dataType: 'html',
                    cache: false,
                    success: function (data) {
                        try {
                            document.getElementById("taID").value = data;
                            document.getElementById("taID").readOnly = true;
                            document.getElementById("taID").style.background = "#c3bcbc";
                            document.getElementById("btnSave").value = "编缉";
                            WasSave('OK');
                        }
                        catch (e) {
                        }

                    },
                    error: function () { alert('保存气象台信息错误'); }
                });
            }

            $("#frm").css({ height: $(window).height() - 5 });

            function SelectedChanged(level) {

                var dropDownListValue = null;
                var strlevel = "ddlQuauLevel" + level;
                var strla = "laQuauLevel" + level;
                var strtxt_val = "txt_" + level;
                dropDownListValue = document.getElementById(strlevel).options[document.getElementById(strlevel).selectedIndex].value;
                $.ajax({
                    url: '../../Data/GetConCirculatData.ashx',
                    method: 'Get',
                    data: { stype: "droplist", selectdata: dropDownListValue },
                    dataType: 'html',
                    cache: false,

                    success: function (data) {

                        var arr_data = eval("(" + data + ")");
                        if (arr_data.HasData == 'yes') {
                            document.getElementById(strla).innerHTML = arr_data.Proposal;
                            document.getElementById(strtxt_val).value = arr_data.MinValue;
                            dropDownListValue = null;
                            strlevel = "";
                            strla = "";
                            strtxt_val = "";
                        }
                    },
                    error: function () { alert('错误'); }
                });
            }

            //图表的加载
            function BindChart() {

                $.ajax({
                    url: '../../Data/GetConCirculatData.ashx',
                    method: 'Get',
                    data: { stype: "regionpollut" },
                    dataType: 'html',
                    cache: false,
                    success: function (data) {

                        if (data != "")
                            try {
                                bindreginpo(data);
                            }
                        catch (e) {

                            }
                    },
                    error: function () { alert('错误'); }
                });

            }
            var chart;
            function GetPollutDataChart() {
                var x_data = new Array();
                var y_data = new Array();
                var selectIndexf = document.getElementById("pollutAgion").selectedIndex;//获得是第几个被选中了
                var selectIndexs = document.getElementById("pollutantid").selectedIndex;//获得是第几个被选中了
                var pollutantregion = document.getElementById("pollutAgion").options[selectIndexf].text //获得被选中的项目的文本
                var pollutionname = document.getElementById("pollutantid").options[selectIndexs].text //获得被选中的项目的文本


                $.ajax({
                    url: '../../Data/GetConCirculatData.ashx',
                    method: 'Get',
                    data: { stype: "selectchart", Name: pollutionname, region: pollutantregion },
                    dataType: 'html',
                    cache: false,
                    success: function (data) {
                        if (null != data && data != "") {
                            try {
                                var arr_data = eval("(" + data + ")");

                                var WarnUp = parseInt(arr_data.WarnUp);
                                if (arr_data.HasData == 'yes') {
                                    for (var i = 0; i < arr_data.Data.length; i++) {
                                        x_data.push(arr_data.Data[i].MoniDate);

                                        y_data.push(parseFloat(arr_data.Data[i].MonitorValue));

                                    }

                                    charBind(y_data, x_data, pollutantregion, pollutionname, WarnUp);

                                }
                                else {
                                    $("#container").html("<div class=\"NoData\" style=\"margin-top:10px;font-size:12px;\">暂无数据</div>");
                                }

                            }

                            catch (e) {
                            }
                        }
                    },
                    error: function () { alert('错误'); }
                });
            }
            $("#pollutAgion").change(function () {
                GetPollutDataChart();
            })
            $("#pollutantid").change(function () {
                GetPollutDataChart();
            })
            function charBind(y_data, x_data, pollutantregion, pollutionname, WarnUp) {
                var util = "ug/m³";
                if (pollutionname == "CO") {
                    util = "mg/m³";
                }
                //if (y_data.length > 0) {
                $('#container').highcharts({
                    chart: {
                        type: 'spline'
                    },
                    title: {
                        text: '实时数据监测'
                    },
                    subtitle: {
                        text: pollutantregion + pollutionname + '监测趋势图'//图标标题
                    },
                    xAxis: {
                        tickPixelInterval: 10,
                        categories: x_data

                    },
                    yAxis: {
                        plotLines: [{   //一条延伸到整个绘图区的线，标志着轴中一个特定值。
                            color: 'red',
                            dashStyle: 'Dash', //Dash,Dot,Solid,默认Solid
                            width: 1.5,
                            value: WarnUp,  //y轴显示位置
                            zIndex: 5
                        }],
                        title: {

                            text: '<div style="writing-mode:lr-tb;text-align:left;">浓度（' + util + '）</div> '
                        }
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'left',
                        verticalAlign: 'top',
                        x: 60,
                        y: 20,
                        borderWidth: 0,
                        floating: true
                    },

                    tooltip: {
                        formatter: function () { return '监测项：<b>' + this.series.name + '</b><br/>时间：' + this.x + ' <br/>值： ' + Highcharts.numberFormat(this.y, 2); }
                    },
                    plotOptions: {


                        line: {
                            dataLabels: {
                                enabled: true
                            },
                            enableMouseTracking: true,
                            cursor: 'pointer',

                            lineWidth: 1,
                            marker: {
                                enabled: true,
                                states: {
                                    hover: {
                                        enabled: true,
                                        radius: 5
                                    }
                                }
                            },
                            shadow: true,
                            states: {
                                hover: {
                                    lineWidth: 2
                                }
                            }
                        }

                    },

                    series: [{
                        name: pollutionname,

                        data: y_data

                    }]

                });

            }

            function IsSubmit(s) {
                $("#waitSubmit").val(s);
                return true;
            }
    </script>
</body>

</html>
