<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConsultReportDeatil.aspx.cs" Inherits="Devoops_ConsulCir_ConsultReportDeatil" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>打印窗口</title>
    <link href="../plugins/jquery-ui/jquery-ui.min.css" rel="stylesheet" />
    <link href="../plugins/jquery-ui/jquery-ui.theme.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../plugins/jquery-ui/jquery-ui.js"></script>
    <script type="text/javascript">

        $(function () {
            $("#pdf1").click(function () {
                $find('ReportViewer1').exportReport('PDF');
            });
            $("#word1").click(function () {
                $find('ReportViewer1').exportReport('WORD');
            });

            $("#pdf2").click(function () {
                $find('ReportViewer2').exportReport('PDF');
            });
            $("#word2").click(function () {
                $find('ReportViewer2').exportReport('WORD');
            });

            $("#pdf3").click(function () {
                $find('ReportViewer3').exportReport('PDF');
            });
            $("#word3").click(function () {
                $find('ReportViewer3').exportReport('WORD');
            });
        });
        $(function () {

            $("body").height($(window).height());
            $("#tabs").tabs({ height: $(window).height() - 100 });
            //$("div[id^=tabs-]").height($(window).height() - 155);
            $(window).resize(function () {
                $("body").height($(window).height());
                $("#tabs").tabs({ height: $(window).height() - 100 });
                //  $("div[id^=tabs-]").height($(window).height() - 155);
            });

            $("#olID li").removeClass("mli");
            $("#olID li").each(function (i, obj) {
                if ($(this).attr("name") == $("#hid_code").val()) {
                    $(this).addClass("mli");
                }
                $(this).click(function () {
                    $("#olID li").removeClass("mli");
                    $(this).addClass("mli");
                    $("#hid_code").val($(this).attr("name"))
                    $("#Button1").trigger("click");
                }).mouseout(function () {
                    $(this).removeClass("sli");
                }).mousemove(function () {
                    $(this).addClass("sli");
                })
            });
        });
    </script>
    <style>
        #olID li {
            border: solid;
            border-width: 1px;
            border-color: #C7C7C7;
            padding-left: 2px;
            padding-right: 2px;
            padding-top: 2px;
            margin-left: 10px;
            float: left;
            list-style: none;
            cursor: pointer;
        }

        .mli {
            background: #6596BD;
            border: 1px solid #003399;
            color: white;
            text-align: center;
        }

        .sli {
            background: #6596BD;
            border: 1px solid #003399;
            color: white;
            text-align: center;
        }
    </style>

</head>
<body style="background: #fff; overflow: auto;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div style="height: 60px; margin-top: 10px; background: #fff; border: 1px solid #EBEBEB; border-radius: 7px; padding-right: 15px; padding-top: 6px; text-align: right;overflow:auto;">
            <div style="float: left; width: 100px; margin-top:8px;">会商列表： </div>
            <ol id="olID" runat="server" style="float: left; margin-left: -30px; margin-top:5px;">
            </ol>
        </div>
         <div class="box-content" style="background:#fff;">
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1" style="font-weight: 800;  font-size: 14px; color: #456782;">普通会商意见</a></li>
                <li><a href="#tabs-2" style="font-weight: 800;  font-size: 14px; color: #456782;">重污染天气会商意见</a></li>
                <li><a href="#tabs-3" style="font-weight: 800;  font-size: 14px; color: #456782;">重污染天气预警信息发布（解除）审批表</a></li>
            </ul>
            <div id="tabs-1" style="border: 1px solid #d8d8d8; border-top: none; font-size: 14px; font-weight: 500; overflow: auto;">

                <div style="float: right; padding-right: 30px;">
                    <input type="button" id="pdf1" value="导出PDF" />
                    <input type="button" id="word1" value="导出WORD" />
                </div>
                <rsweb:ReportViewer ID="ReportViewer1" ShowToolBar="false" runat="server" SizeToReportContent="true">
                </rsweb:ReportViewer>
            </div>
            <div id="tabs-2" style="border: 1px solid #d8d8d8; border-top: none; font-size: 14px; font-weight: 500; overflow: auto;">
                <div style="float: right; padding-right: 30px;">
                    <input type="button" id="pdf2" value="导出PDF" />
                    <input type="button" id="word2" value="导出WORD" />
                </div>
                <rsweb:ReportViewer ID="ReportViewer2" ShowToolBar="false" runat="server" SizeToReportContent="true">
                </rsweb:ReportViewer>
            </div>
            <div id="tabs-3" style="border: 1px solid #d8d8d8; border-top: none; font-size: 14px; font-weight: 500; overflow: auto;">
                <div style="float: right; padding-right: 30px;">
                    <input type="button" id="pdf3" value="导出PDF" />
                    <input type="button" id="word3" value="导出WORD" />
                </div>
                <rsweb:ReportViewer ID="ReportViewer3" ShowToolBar="false" runat="server" SizeToReportContent="true">
                </rsweb:ReportViewer>
            </div>
        </div>
             </div>
        <asp:Button ID="Button1" Style="display: none;" runat="server" Text="Button" OnClick="Button1_Click" />
        <asp:HiddenField ID="hid_code" runat="server" />
        <asp:HiddenField ID="HiddenField2" runat="server" />
    </form>


</body>
</html>
