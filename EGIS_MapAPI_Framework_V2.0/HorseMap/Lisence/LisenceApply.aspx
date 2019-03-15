<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LisenceApply.aspx.cs" Inherits="Framework_LisenceApply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>中科宇图.环境GIS平台-小马地图API</title>
    <script type="text/javascript" src="../../Framework/javascript/Calendar4.js"></script>
    <script src="../../Scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <link href="../../Controls/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function submit() {    
           // var  md = DES.DESEncrypt(ip1,ip2,ip3,time1,time2);
            var proNum = document.getElementById("pronum").value;
            var proName = document.getElementById("proname").value;
            var selectObj = document.getElementById('rtl');
            var index = selectObj.selectedIndex; //序号，取当前选中选项的序号
            var application = selectObj.options[index].value;
//          var application = document.getElementById("rtl").selectedIndex.valueOf();
            var domain = document.getElementById("domain").value;
            var hostname = document.getElementById("hostname").value;
            var ip = document.getElementById("ip").value;
            var starttime = document.getElementById("time1").value;
            var endtime = document.getElementById("time2").value;
            if (endtime == "请选择") {
                endtime = "";
            }
            var lisencoding = "";
            if (application == "系统部署") {
                if (proNum == "" || proName == "") {
                    alert("请填写项目或名称项目编号，谢谢！");
                } else {
                    doGetData();
                }
            } else {
                doGetData();
            }
            function doGetData() {
                $.ajax({
                    type: "Post",
                    url: "LisenceApply.aspx/getCode", //方法传参的写法一定要对
                    data: "{'ip1':'" + domain + "','ip2':'" + hostname + "','ip3':'" + ip + "','time1':'" + starttime + "','time2':'" + endtime + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        lisencoding = data.d;
                        document.getElementById("lisencecoding").value = data.d;
                        document.getElementById("lisencecoding").disabled = true;

                        var data = { projectNum: proNum, projectName: proName, application: application, domain: domain, hostName: hostname, ip: ip, startTime: starttime, endTime: endtime, lisenceCoding: lisencoding };
                        $.post("../../Data/LisenceApply.ashx", data,
                function (msg) {
                    if (msg == "未连接上数据库！") {
                        alert(msg);
//                        ShowLittleBoxMessage('提示', '<br/>' + msg + '<br/><br/>');
                    }
                    else {
//                        ShowLittleBoxMessage('提示', '<br/>登录成功！<br/><br/>');
                        $("#aUser").empty().append(msg);
                        $("#divSignIn").hide();
                        $("#divSuccess").show();
                    }
                });
                    },
                    error: function (err) {
                        alert(err.toString());
                    }
                });
            }
           

//            var data = { projectNum: proNum, projectName: proName, application: application, domain: domain, hostName: hostname, ip: ip, startTime: starttime, endTime: endtime, lisenceCoding: lisencoding};
//            $.post("../Data/LisenceApply.ashx", data,
//                function (msg) {
//                    if (msg == "未连接上数据库！") {
////                    ShowLittleBoxMessage('提示', '<br/>' + msg + '<br/><br/>');
//                    }
//                    else {

////                        ShowLittleBoxMessage('提示', '<br/>登录成功！<br/><br/>');
////                        $("#aUser").empty().append(msg);
////                        $("#divSignIn").hide();
////                        $("#divSuccess").show();
//                    }
//                });
        }
    </script>
</head>
<body>
<div class="navbar navbar-inverse navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container">
                <button data-target=".nav-collapse" data-toggle="collapse" class="btn btn-navbar"
                    type="button">
                    <span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar">
                    </span>
                </button>
                <a class="brand" href="../../Index.aspx">中科宇图.环境GIS平台-小马地图API  <span class="label"> BETA </span></a>
               <%-- <div class="nav-collapse collapse">
                    <ul class="nav">
                        <%=strModule%>
                    </ul>
                </div>--%>
                <ul class="nav pull-right">
                    <li><a href="#">技术支持：中科宇图</a></li>
                </ul>
                <!--/.nav-collapse -->
            </div>
        </div>
    </div>
    <div style=" padding-left:300px; line-height:40px; padding-top:80px">
         <h2>许可申请<font color="red" size="2"></font></h2>
         用途：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<select name="sel" id="rtl">
                   <option>系统部署</option> 
                   <option>开发测试</option>
               </select><br />
         项目编号：<input id="pronum" type="text" runat="server"/><br />
         项目名称：<input id="proname" type="text" runat="server"/><br />
         
         IP1:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="domain" type="text" runat="server" /><font color="red" size="2">*包括域名、主机名、IP地址三种类型，最多可同时支持三种</font><br />
         IP2:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="hostname" type="text" runat="server"/><br />
         IP3:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="ip" type="text" runat="server"/><br />
         起止时间：<input type="text" id="time1" onclick="MyCalendar.SetDate(this)" value="2014-1-1" runat="server"/><font color="red" size="2">*</font>&nbsp;&nbsp;&nbsp;&nbsp;-&nbsp;&nbsp;&nbsp;&nbsp;<input type="text" id="time2" onclick="MyCalendar.SetDate(this,document.getElementById('time2'))" runat="server" value="请选择" /><br />
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="btn" type="button" onclick="submit()" value="申请"/><br />
         许可编码：<input type="text" id="lisencecoding" style=" width:500px; height:40px" runat="server"/>
    </div>
   
</body>
</html>
