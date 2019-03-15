<%@ page language="C#" autoeventwireup="true"  CodeFile="DownClassPackage.aspx.cs" inherits="Framework_DownClassPackage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
            <style type="text/css">
            thead { background-color:#222222;color:#999999;height:50px;}
            tbody { background-color :#EEEEEE;height:50px; width:400px; color:#808080;}
            td { width:400px;}
            </style>

<link href="../../Controls/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
<title>中科宇图.环境GIS平台-小马地图API</title>
<script type="text/javascript">
    var _curfile = null;
    function downfile(file) {
        _curfile = file;
        document.getElementById("body").disabled = true;
        document.getElementById("div1").style.display = "block";
    }
    function OK() {
        document.getElementById("div1").style.display = "none";
        document.getElementById("body").disabled = false;
        try {
            var elemIF = document.createElement("iframe");
            elemIF.src = _curfile;
            elemIF.style.display = "none";
            document.body.appendChild(elemIF);
        } catch (e) {

        }
        _curfile = null;
    }
    function NotOK() {
        document.getElementById("div1").style.display = "none";
        document.getElementById("body").disabled = false;
        _curfile = null;
    }
</script>
</head>
<body>
<div id="div1" style="display:none;position:absolute;left:600px;top:200px;border:double 3px #C0C0C0; background-color:#eeeeee"><br />
  <font size="4"><b>您确认要下载此文件吗？</b></font><br/><br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="button" value="确认" onclick="OK()" />
<input type="button" value="取消" onclick="NotOK()" />
</div>
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
<div id="body" style=" padding-left:300px; padding-top:50px">
<h3><font><b>小马地图API安装包下载</b></font></h3>
<table border="1 1 0 0" style=" border:1 solid #FAF9F8; border-left-color:White; border-right-color:White; border-top-color:White; border-bottom-color:White; border-style:none">
<%--<caption style=" font-size:15px; font-family:黑体"><b>地图接口调用示例程序下载和版本更新说明</b></caption>--%>
  <thead style="background-color:#6F6C6C">
    <tr style=" line-height:50px">
      <th>版本</th>
      <th>文件</th>
    </tr>
  </thead>
  <tbody>
    <tr style=" line-height:50px">
      <td><b>小马地图APIV2.0.10</b></td>
      <td><a href="javascript:void(0)" onclick="return downfile('DownLoadFile/HorseMap_V2.0.10.msi');"><b>HorseMap_V2.0.10.msi</b></a></td>
	</tr>
    
    <tr>
     
    </tr>
  </tbody>
</table>
<h3><font><b>开发指南和类参考的文档下载</b></font></h3>
<table border="1 1 0 0" style=" border:1 solid #FAF9F8; border-left-color:White; border-right-color:White; border-top-color:White; border-bottom-color:White; border-style:none">
  <thead style="background-color:#6F6C6C">
    <tr style=" line-height:50px">
      <th>文档名称</th>
      <th>文件</th>
    </tr>
  </thead>
  <tbody>
    <tr style=" line-height:50px">
      <td><b>开发指南</b></td>
      <td><a href="javascript:void(0)" onclick="return downfile('DownLoadFile/开发指南.rar');"><b>开发指南.rar</b></a></td>
	</tr>
    <tr style=" line-height:50px">
      <td><b>类参考</b></td>
      <td><a href="javascript:void(0)" onclick="return downfile('DownLoadFile/类参考.rar');"><b>类参考.rar</b></a></td>
	</tr>
    
    <tr>
     
    </tr>
  </tbody>
</table>
<h3><font><b>模型相关文件下载</b></font></h3>
<table border="1 1 0 0" style=" border:1 solid #FAF9F8; border-left-color:White; border-right-color:White; border-top-color:White; border-bottom-color:White; border-style:none">
  <thead style="background-color:#6F6C6C">
    <tr style=" line-height:50px">
      <th>模型</th>
      <th>文件</th>
    </tr>
  </thead>
  <tbody>
    <tr style=" line-height:50px">
      <td><b>气模型</b></td>
      <td><a href="javascript:void(0)" onclick="return downfile('DownLoadFile/AirModel.zip');"><b>AirModel.zip</b></a></td>
	</tr>
    <tr style=" line-height:50px">
      <td><b>路径分析模型</b></td>
      <td><a href="javascript:void(0)" onclick="return downfile('DownLoadFile/RoutingAnalysis.zip');"><b>RoutingAnalysis.rar</b></a></td>
	</tr>
    <tr>
     
    </tr>
  </tbody>
</table>
</div >
</body>
</html>
