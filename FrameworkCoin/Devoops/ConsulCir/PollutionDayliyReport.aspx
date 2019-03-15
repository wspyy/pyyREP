<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PollutionDayliyReport.aspx.cs" Inherits="Devoops_ConsulCir_PollutionDayliyReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!--绘图用-->
<%@ Import Namespace = "System" %>
<%@ Import Namespace = "System.Windows.Forms" %>
<%@ Import Namespace = "System.Drawing" %>
<%@ Import Namespace = "System.IO" %>
<%@ Import Namespace = "System.Drawing.Imaging" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>晋城市市重污染天气预报会商意见</title>
    <link href="../plugins/jquery-ui/jquery-ui.min.css" rel="stylesheet" />
    <link href="../plugins/jquery-ui/jquery-ui.theme.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../plugins/bootstrap/bootstrap.css" rel="stylesheet" />

    <script src="../js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../plugins/jquery-ui/jquery-ui.js"></script>
    <script src="../plugins/bootstrap/bootstrap.js"></script>
    <style>
        .imgAdd {
            cursor: pointer;
            background-color: #aca9a9;
        }

            .imgAdd:hover {
                background-color: #464545;
                -webkit-box-shadow: 0 0 10px #808080;
                -moz-box-shadow: 0 0 10px #808080;
                box-shadow: 0 0 10px #8b8888;
            }

        .signul li {
            list-style: none;
            float: left;
        }
    </style>

    <script type="text/javascript">
        var curul = "";
        var expertSign = ",";
        var checkSign = ",";
        var ReportDate = "";
        debugger;
        var loginName = "<%=userName%>";
        $(function () {
            var ReportCode = "<%=Request["ReportCode"]%>";
            var date = "<%=Request["ReportTime"]%>";
            
           // date=date.
            $("#tabs").tabs({ height: $(window).height() - 45, active: "<%=Request["ch"]%>" });
            $("div[id^=tabs-]").height($(window).height() - 90);
            $(window).resize(function () {
                $("#tabs").tabs({ height: $(window).height() - 45 });
                $("div[id^=tabs-]").height($(window).height() - 90);
                $("#tabs").tabs('tabIndex', 1)
            });

            $("#btn_pdf").click(function () {
                //savepoll();
                //window.showModalDialog("poll.aspx?ReportCode=" + ReportCode + "&l=" + new Date().getMilliseconds(), "", "dialogLeft=200px;dialogWidth=800px;dialogHeight=600px;Minimize=yes;");
                window.open("poll.aspx?ReportCode=" + ReportCode + "&date=" + date + "&l=" + new Date().getMilliseconds(), "", "dialogLeft=200px;dialogWidth=800px;dialogHeight=600px;Minimize=yes;");
            });

            $("#btn_view").click(function () {
                //savepollpublish();
                window.showModalDialog("op.aspx?ReportCode=" + ReportCode + "&l=" + new Date().getMilliseconds(), "", "dialogLeft=200px;dialogWidth=800px;dialogHeight=600px;Minimize=yes;");
                //"&level=<%=Request["level"]%>&date="+date+
            });

            $("#btn_savePoll").click(function () {
                savepoll();
            });

            $("#btn_savepublic").click(function () {
                savepollpublish();
            });

            $.ajax({
                url: "../../Data/PollConclusions.ashx",
                dataType: "JSON",
                data: { flag: 2, ReportCode: ReportCode },
                success: function (result) {
                    if (result && result != "") {
                        debugger;
                        var strDate = result[0].ReportTime.replace("T", " ");//ReportDate
                        ReportDate = new Date(strDate);
                        if (ReportDate.getFullYear().toString() == "NaN")
                        {
                            ReportDate = new Date(strDate.split("-")[0], strDate.split("-")[1]-1, strDate.split("-")[2].split(" ")[0]);
                        }
                        ReportDate = ReportDate.getFullYear() + "年" + (ReportDate.getMonth() + 1) + "月" + ReportDate.getDate() + "日";
                        $("#lbl_Report").text(ReportDate);
                        $("#lbl_checkremark").text(ReportDate);
                        $("#lbl_check").text(ReportDate);
                        
                        $("#labelUser").text(result[0].U_RealName == null ? loginName : result[0].U_RealName);
                        $("#txt_poll").val(result[0].Conclusions);
                        $("#txt_Opinion").val(result[0].Opinion);
                        expertSign = result[0].ExpertSign == null ? "" : result[0].ExpertSign;
                        setSignUl(expertSign, "signul1");
                        checkSign = result[0].CheckSign == null ? "" : result[0].CheckSign;
                        setSignUl(checkSign, "signul2");
                        if (expertSign.length > 0 || checkSign.length > 0) {
                            document.getElementById("txt_poll").disabled = true;
                            document.getElementById("txt_Opinion").disabled = true;
                        }
                    }
                },
                error: function () {
                }
            });

            $.ajax({
                method: "post",
                url: "../../Data/GetBaseData.ashx",
                data: { flag: "SMSUserList" },
                success: function (data) {
                    var arr_data = eval("(" + data + ")");
                    var strdata = "";
                    for (var i = 0; i < arr_data.DataTable.length; i++) {
                        strdata += "<option value='" + arr_data.DataTable[i]["U_LoginName"] + "'>" + arr_data.DataTable[i]["U_RealName"] + "</option>";
                        //arr_data.DataTable[i]["U_LoginName"] 
                    }
                    $("#listName").append(strdata);
                },
                error: function (data) {

                }
            });

            function setSignUl(strsign, singul) {
                if (strsign == null) return;
                var signs = strsign.split(",");
                var strli = "";
                for (i = 0; i < signs.length ; i++) {
                    if (signs[i] != "" && signs[i] != "undefined")
                        strli += "<li><img src='../img/sign/" + signs[i] + ".png' /></li>";
                }
                $("#" + singul).append(strli);
            }

            function savepoll() {
                $.ajax({
                    method: "post",
                    url: "../../Data/PollConclusions.ashx",
                    data: {
                        ReportCode: ReportCode,
                        poll: $("#txt_poll").val(),
                        opinion: $("#txt_Opinion").val(),
                        flag: 1,
                        ExpertSign: expertSign,
                        CheckSign: checkSign
                    },
                    success: function (data) {
                        if (data == "True") alert("保存成功");
                        else alert("保存失败");
                    },
                    error: function (data) {
                        return 2;
                    }
                });
            }

            function savepollpublish() {
                $.ajax({
                    url: "../../Data/PollConclusions.ashx",
                    data: { ReportCode: ReportCode, poll: $("#txt_basis").val(), opinion: $("#txt_main").val(), level: $("#lbl_level").text(), flag: 3 },
                    /*success: function () {
                        return 1;
                    */
                    success: function (data) {
                        if (data == "True") alert("保存成功");
                        else alert("保存失败");
                    },
                    error: function () {
                        return 2;
                    }
                });
            }

        });

        function setul(index) {
            curul = "signul" + index;
            $("#txtPwd").text("");
            $("#tip").hide();
        }

        function VerifyUser() {
            $.ajax({
                method: "post",
                url: "../../Data/GetBaseData.ashx",
                data: { flag: "VerifyUser", code: $("#listName").val(), pwd: $("#txtPwd").val() },
                success: function (data) {
                    if (data == "true") {
                        debugger;
                        strli = "<li><img src='../img/sign/" + $("#listName").val() + ".png' /><li>"; 
                        $("#" + curul).append(strli);
                        if (curul == "signul1") expertSign += $("#listName").val() + ",";
                        else if (curul == "signul2") checkSign += $("#listName").val() + ",";
                        $("#closebtn").click();
                    }
                    else {
                        $("#tip").show();
                    }
                },
                error: function (data) {
                    $("#tip").text("验证出错");
                    $("#tip").show();
                }
            });
        }


    </script>
    <style type="text/css">
        .tab {
            border: 1px solid #000;
            width: 800px;
            margin: 0 auto;
            border-collapse: collapse;
        }

            .tab td {
                border: 1px solid #000;
            }
    </style>
</head>
<body style="background: #fff;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="box-content">
            <div id="tabs">
                <ul>
                    <li><a href="#tabs-1" style="font-weight: 800; font-size: 14px; color: #456782;">重污染天气预报会商意见</a></li>
                    <li><a href="#tabs-3" style="font-weight: 800; font-size: 14px; color: #456782;">重污染天气预警信息发布（解除）审批表</a></li>
                </ul>
                <div id="tabs-1" style="border: 1px solid #d8d8d8; border-top: none; font-size: 14px; font-weight: 500; padding: 5px; overflow: auto;">
                    <div class="form-group  has-feedback" style="float: right;">
                        <button class="btn btn-success btn-label-left" id="btn_savePoll" type="button">
                            保存
                        </button>
                        <button class="btn btn-success btn-label-left" id="btn_pdf" type="button">
                            预览
                        </button>
                    </div>
                    <p style="text-align: center; font-size: 24px; font-weight: 600;">
                        朔州市重污染天气预报会商意见
                    </p>
                    <table class="tab">
                        <tr>
                            <td style="width: 50px; padding-left: 20px; font-size: 18px; height: 360px; vertical-align: middle; word-wrap: break-word; letter-spacing: 15px;">会商结论</td>
                            <td>
                                <asp:TextBox TextMode="MultiLine" Width="748" Height="360" Style="font-size: 14px;" runat="server" ID="txt_poll" border="0">

                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 40px; padding-left: 10px;">报告人：<asp:Label ID="labelUser" runat="server"></asp:Label>
                                <span style="float: right; margin-right: 10px;">
                                    <asp:Label ID="lbl_Report" runat="server" Text=""></asp:Label></span></td>
                        </tr>

                        <tr>
                            <td colspan="2" style="padding-left: 10px; height: 40px;">专家组意见：</td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 150px;">
                                <asp:TextBox TextMode="MultiLine" Width="800" Style="font-size: 14px;" Height="150" runat="server" ID="txt_Opinion" border="0">

                                </asp:TextBox>
                                <div id="divSign">
                                    <ul id="signul1" class="signul">
                                    </ul>
                                    <div class="imgAdd" style="float: left; width: 40px; height: 40px; border-radius: 50px; color: white; text-align: center; vertical-align: middle; font-size: 30px; font-weight: 900; margin-top: 15px;"
                                        data-toggle="modal" data-target="#myModal" title="新增签名" onclick="setul(1)">
                                        +
                                    </div>
                                </div>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 40px; padding-left: 10px;">
                                <span style="float: right; margin-right: 10px;">
                                    <asp:Label ID="lbl_checkremark" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 40px; padding-left: 10px;vertical-align:middle;">
                                <div style="float:left;margin-top:20px;">审定人：</div>
                                <ul id="signul2" class="signul">
                                    </ul>
                                <div class="imgAdd" style="float: left; width: 40px; height: 40px; border-radius: 50px; color: white; text-align: center; vertical-align: middle; font-size: 30px; font-weight: 900; margin-top: 15px;"
                                        data-toggle="modal" data-target="#myModal" title="新增签名" onclick="setul(2)">
                                        +
                                    </div>
                                <span style="float: right; margin-right: 10px;margin-top:20px;">
                                    <asp:Label ID="lbl_check" runat="server" Text=""></asp:Label>
                                </span>

                            </td>
                        </tr>
                    </table>

                </div>
                <div id="tabs-3" style="border: 1px solid #d8d8d8; border-top: none; font-size: 14px; font-weight: 500; padding: 5px; overflow: auto;">
                    <div class="form-group  has-feedback" style="float: right;">
                        <button class="btn btn-success btn-label-left" id="btn_savepublic" type="button">
                            保存
                        </button>
                        <button class="btn btn-success btn-label-left" id="btn_view" type="button">
                            预览
                        </button>
                    </div>
                    <p style="text-align: center; font-size: 24px; font-weight: 600;">
                        朔州市重污染天气预警信息发布（解除）审批表
                    </p>
                    <table class="tab">
                        <tr>
                            <td style="height: 40px; padding: 5px;">预警信息级别</td>
                            <td style="width: 280px;">
                                <asp:Label ID="lbl_level" runat="server" Text=""></asp:Label></td>
                            <td style="padding: 5px;width:130px;">发布（解除）时间</td>
                            <td style="padding: 5px;">
                                <asp:Label ID="lbl_pulish" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="height: 100px; width: 120px; padding: 5px;">预警发布（解除）信息依据</td>
                            <td colspan="3">
                                <asp:TextBox TextMode="MultiLine" Style="font-size: 14px;" Width="680" Height="100" runat="server" ID="txt_basis" border="0">

                                </asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="height: 100px; width: 120px; padding: 5px;">预警发布（解除）信息主要内容</td>
                            <td colspan="3">
                                <asp:TextBox TextMode="MultiLine" Style="font-size: 14px;" Width="680" Height="100" runat="server" ID="txt_main" border="0">

                                </asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="height: 100px; width: 120px; padding: 5px;">申请发布（解除）部门领导意见（签字）</td>
                            <td colspan="3"></td>
                        </tr>
                        <tr style="height: 100px; width: 120px; padding: 5px;">
                            <td>市重污染天气应急指挥部办公室意见</td>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td style="height: 100px; padding: 5px;">市重污染天气应急指挥部总指挥审批意见</td>
                            <td colspan="3"></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

        <div class="modal fade" id="myModal" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                            &times;
                        </button>
                        <h4 class="modal-title" id="myModalLabel">选择专家
                        </h4>
                    </div>
                    <div class="modal-body">
                        <b>姓名：</b>
                        <select id="listName" style="width:155px;"></select>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <b>密码：</b><input type="password" id="txtPwd" />
                        <label id="tip" style="color:red;font-size:12px;">密码错误</label>
                    </div>
                    <div class="modal-footer">
                        <button id="closebtn" type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>
                        <button type="button" class="btn btn-primary" onclick="VerifyUser()">
                            确认
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
        </div>
    </form>
</body>
</html>
