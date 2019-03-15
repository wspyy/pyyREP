﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="op.aspx.cs" Inherits="Devoops_ConsulCir_op" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../js/jquery-1.7.2.js"></script>
    <style>
        html, body {
            overflow-x: hidden;
        }
    </style>
        <script type="text/javascript">
            $(function () {
                $("#Button1").click(function () {
                    $find('ReportViewer2').exportReport('PDF');
                });
                $("#btn_word").click(function () {
                    $find('ReportViewer2').exportReport('WORD');
                });
            });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin: 0 auto; width: 650px; padding-top: 20px;">

            <div style=" float: right; padding-right:30px;">
            <input type="button" id="Button1" value="导出PDF" />
            <input type="button" id="btn_word" value="导出WORD" />
                </div>
        </div>
        <div style="margin: 0 auto; width: 650px; padding-top: 20px;">


            <rsweb:ReportViewer ID="ReportViewer2" ShowToolBar="false" runat="server" SizeToReportContent="true">
            </rsweb:ReportViewer>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    </form>
</body>
</html>
