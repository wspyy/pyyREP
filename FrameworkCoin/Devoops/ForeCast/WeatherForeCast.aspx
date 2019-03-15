﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WeatherForeCast.aspx.cs" Inherits="Devoops_ForeCast_WeatherForeCast" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link href="../../Demo/plugins/bootstrap/bootstrap.css" rel="stylesheet" />

    <link href="../css/font-awesome.css" rel="stylesheet" />
    <!--<link href="../css/style.css" rel="stylesheet" />背景是灰色(btn 按钮样式)-->
    <link href="../css/css.css" rel="stylesheet" />
    <link href="../plugins/data-tables/DT_bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../plugins/fancybox/jquery.fancybox.css" rel="stylesheet" />

    <script src="../js/jquery-2.1.0.min.js"></script>
    <script src="../plugins/jquery-ui/jquery-ui.js"></script>
    <script src="../plugins/fancybox/jquery.fancybox.js"></script>
    <script src="../plugins/bootstrap/bootstrap.min.js"></script>
    <script src="../js/method.js"></script>
    <script src="../js/devoops.js"></script>
    <script src="../../Controls/My97DatePicker/WdatePicker.js"></script>
    <script src="../../API-2.0/MapAPI.js"></script>
    <script src="../plugins/data-tables/jquery.dataTables.js" type="text/javascript"></script>

    <style>
        body {
            background-color: white;
            padding-left:20px;
            text-align:left;
        }

        p {
            font-size: 14px;
        }

        .sli {
            background-color:#6AA6D6;
            border: 1px solid #6AA6D6;
            color: white;
        }

        .TimePicker li {
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
        .TimePicker {
            height:35px;
            width:100%;
            float:left;
        }
        .mli {
            background: #6596BD;
            border: 1px solid #003399;
            color: white;
        }
        #type ,#height {/*style="float: left; margin-left: 20px; padding-left: 30px",style="float: left; margin-left: 20px; padding-left: 20px"*/
            padding-left:20px;
        }
        .form-control {
            width:140px;
        }
        .btn {
  border-width: 1px;
  border-style: solid;
  border-width: 1px;
  text-decoration: none;
  border-color: rgba(0, 0, 0, 0.3);
  cursor: pointer;
  outline: none;
  font-family: "Lucida Grande","Lucida Sans","Lucida Sans Unicode","Segoe UI",Verdana,sans-serif;
  display: inline-block;
  vertical-align: top;
  position: relative;
  font-size: 12px;
  font-weight: bold;
  text-align: center;
  background-color: #a2a2a2;
  background: #a2a2a2 -moz-linear-gradient(top, rgba(255,255,255,0.6), rgba(255,255,255,0));
  background: #a2a2a2 -webkit-gradient(linear, 0% 0%, 0% 100%, from(rgba(255,255,255,0.6)), to(rgba(255,255,255,0)));
  line-height: 24px;
  margin: 0 0 10px 0;
  padding: 0 10px;
  -webkit-border-radius: 4px;
  -moz-border-radius: 4px;
  border-radius: 4px;
  -moz-user-select: none;
  -webkit-user-select: none;
  outline: none !important;
}
.btn-label-left, .btn-label-right {
  padding: 0 10px;
}
.btn-success{
  background-color: #63CC9E;
  border-color: rgba(0, 0, 0, 0.3);
  color: #f8f8f8;
}
.btn-primary{
  background-color: #6AA6D6;
  border-color: rgba(0, 0, 0, 0.3);
  color: #f8f8f8;
}
.form-control {
  height: 26px;
  width:140px;
  padding: 2px 12px;
}
    </style>
    <script type="text/javascript">
        var imgArry = [];//存放图片路径
        var _length = 0;
        var cur = { currimg: 0 };
        //时间控制器
        var InterV = null;
        var optionArray = [];//存下拉框的值
        var re = /^[1-9]{1}[0-9]{2}$/;
        //请求的URL
        var ImgURL;
        ImgURL = "http://" + document.location.host + "/AirData/QxImages/";
        //<img src="../MoniData/images/zanwu.jpg" />
        $(function () {
            //点击图像弹出
            $("#imgpplay").fancybox({
                'overlayShow': true,
                'transitionIn': 'elastic',
                'transitionOut': 'elastic'
            });
          
            $("#txt_beginTime").bind("click", function () { WdatePicker({ dateFmt: 'yyyy-MM-dd' }); });
            $("#txt_endTime").bind("click", function () { WdatePicker({ dateFmt: 'yyyy-MM-dd' }); });
            $("#txt_beginTime").val("2015-10-19");
            $("#txt_endTime").val("2015-10-20");

            loadHeight("相对湿度");
            //alert(ImgURL);
        });
        //根据类型加载高度
        function loadHeight(type) {
            $("#height").empty();
            $.post("../../Data/GetWeatherImg.ashx", { flag: "loadHeight", type: type }, function (result) {

                if ((result && result.DataTable && result.DataTable.length > 0) && result.DataTable[0].MenuLeave4Type != "") {
                    $("#divHeight").css("display", "block");
                    $("#search").css("height", "160px");
                    for (var i = 0; i < result.DataTable.length; i++) {
                        if (i == 0) {
                            $(" <li title='" + result.DataTable[i].MenuLeave4Type + "' class='sli' onclick='Heightclick(this)'>" + result.DataTable[i].MenuLeave4Type + "</li>").appendTo("#height");
                        }
                        else {
                            $(" <li title='" + result.DataTable[i].MenuLeave4Type + "' onclick='Heightclick(this)'>" + result.DataTable[i].MenuLeave4Type + "</li>").appendTo("#height");
                        }
                    }
                    loadData(type, $("#height li[class='sli']").text());
                  
                    bind();                  

                } else {
                    $("#divHeight").css("display", "none");
                    $("#search").css("height", "125px");
                }
            }, "json");
        };
        //加载图片
        function loadData(type, height) {

            $.post("../../Data/GetWeatherImg.ashx", { flag: "loadData", type: type, height: height, begin: $("#txt_beginTime").val(), end: $("#txt_endTime").val() }, function (result) {
                imgArry = [];
                optionArray = [];
                $("#timeSpan").empty();
                if (result && result.DataTable && result.DataTable.length > 0) {

                    $.each(result.DataTable, function (index, item) {
                        $("<option name='" + item.PredictiveTime.split("T")[0] + " " + item.Duration.replace(/\b(0{1})/gi, "") + "小时" + "'>" + item.PredictiveTime.split("T")[0] + " " + item.Duration.replace(/\b(0{1})/gi, "") + "小时</option>").appendTo("#timeSpan");
                        optionArray.push((item.PredictiveTime.split("T")[0] + " " + item.Duration.replace(/\b(0{1})/gi, "") + "小时"));

                        if (item.Duration == "000") {
                            imgArry.push((item.Picposition + "\\" + item.PredictiveTime.split("T")[0].replace("-", "").replace("-", "") + "12" + item.Duration.substr(1, 2) + ".jpg"));
                        } else if (re.test(item.Duration)) {
                            imgArry.push((item.Picposition + "\\" + item.PredictiveTime.split("T")[0].replace("-", "").replace("-", "") + "1200" + item.Duration + "小时.jpg"));
                        }
                        else {
                            imgArry.push((item.Picposition + "\\" + item.PredictiveTime.split("T")[0].replace("-", "").replace("-", "") + "120" + item.Duration + "小时.jpg"));
                        }
                    })
                    _length = imgArry.length;

                    var path = result.DataTable[0].Picposition + "\\" + (result.DataTable[0].PredictiveTime.split("T")[0]).replace("-", "").replace("-", "") + "12" + result.DataTable[0].Duration.substr(1, 2) + ".jpg";
                    optionClick(InterV);
                    //设置点击图片
                    $("#imgpplay").attr("href", ImgURL + imgArry[cur.currimg]);
                    $("#img").attr("src", ImgURL + path);
                } else {
                    $("#img").attr("src", "images/nopicture.gif");
                }
            }, "json");

        }
        //开始播放
        function Stratplay() {
            InterV = setInterval(sliderAdd, $("#txt_speed").val());
            optionClick(InterV);
        }
        //播放方法
        function sliderAdd() {
            if (imgArry.length != 0) {
                if (cur.currimg == _length) {
                    cur.currimg = 0;
                }
                cur.currimg++;
                $("#img").attr("src", ImgURL + imgArry[cur.currimg]);
                $("#imgpplay").attr("href", ImgURL + imgArry[cur.currimg]);
                $("#timeSpan option[name='" + optionArray[cur.currimg] + "']").attr("selected", "selected");

            } else {
                clearInterval(InterV);
                $("#img").attr("src", "../MoniData/images/zanwu.jpg");
                $("#imgpplay").attr("href", "../MoniData/images/zanwu.jpg");
            }
        }
        //暂停播放
        function PausePlay() {
            clearInterval(InterV);
            $("#imgpplay").attr("href", ImgURL + imgArry[cur.currimg]);
            optionClick(InterV);
        }
        //下一张
        function nextImg() {
            if (imgArry) {
                if (cur.currimg == _length) {
                    cur.currimg = 0;
                }
                cur.currimg++;
                $("#img").attr("src", ImgURL + imgArry[cur.currimg]);
                $("#imgpplay").attr("href", ImgURL + imgArry[cur.currimg]);
                $("#timeSpan option[name='" + optionArray[cur.currimg] + "']").attr("selected", "selected");
            } else {
                clearInterval(InterV);
                $("#img").attr("src", "../MoniData/images/zanwu.jpg");
                $("#imgpplay").attr("href", "../MoniData/images/zanwu.jpg");
            }

        }
        //上一张
        function prevImg() {
            if (imgArry) {
                if (cur.currimg == 0) {
                    cur.currimg = _length;
                }
                cur.currimg--;
                $("#img").attr("src", ImgURL + imgArry[cur.currimg]);
                $("#imgpplay").attr("href", ImgURL + imgArry[cur.currimg]);
                $("#timeSpan option[name='" + optionArray[cur.currimg] + "']").attr("selected", "selected");
            } else {
                clearInterval(InterV);
                $("#img").attr("src", "../MoniData/images/zanwu.jpg");
                $("#imgpplay").attr("href", "../MoniData/images/zanwu.jpg");
            }
        }
        //类型样式
        function bind() {
            $(".olID li").each(function (index, o) {
                $(o).mousemove(function () { $(o).addClass("mli") }).mouseleave(function () {
                    $(o).removeClass("mli");
                }).click(function () {
                    $(this).siblings().removeClass("sli");
                    $(o).addClass("sli")
                });
            });
        }
        //单击类型
        function typeClick(obj) {
            var type;
            switch (obj.value) {
                case 1:
                    type = "相对湿度";
                    break;
                case 2:
                    type = "垂直速度";
                    break;
                case 3:
                    type = "500hPa高度场+850hPa风场";
                    break;
                case 4:
                    type = "温度平流";
                    break;
            }
            //var height = $("#height li[class='sli']").text();
            cur.currimg = 0;

            loadHeight(type);
        };
        //单击高度
        function Heightclick(obj) {
            var height = obj.title;

            var type = $("#type li[class='sli']").text();
            cur.currimg = 0;

            loadData(type, height);
        }
        //启用设置
        function setUp() {
            var type = $("#type li[class='sli']").text();
            var height = $("#height li[class='sli']").text();
            cur.currimg = 0;
            loadData(type, height);
        };
        //单击option事件
        function optionClick(timer) {
            $("#timeSpan").change(function () {
                var index = $(this)[0].selectedIndex;
                if (timer == null) {
                    $("#img").attr("src", ImgURL + imgArry[index]);
                    cur.currimg = index;
                } else {
                    cur.currimg = index;
                }
            });

        };
    </script>
</head>
<body>
    <div id="search" style="height:160px;border: 1px solid #EBEBEB; border-radius: 7px; padding-left: 5px; padding-top:5px; ">
    <div class="TimePicker" style="margin-top: 10px;float:left;"><!--height: 50px; background-color: lightgray; padding-top: 15px; -->

        <p style="float: left;font-weight: bold">层次：</p>
        <ol id="type" class="olID" >
            <li class="sli" value="1" onclick="typeClick(this)" >相对湿度</li>
            <li  value="2" onclick="typeClick(this)">垂直速度</li>
            <li  value="3" onclick="typeClick(this)">500hPa高度场+850hPa风场</li>
            <li  value="4" onclick="typeClick(this)">温度平流</li>
        </ol>
    </div>
    <div class="TimePicker" style="margin-top: 1px;display:none" id="divHeight"><!--height: 50px; background-color: lightgray; padding-top: 15px; -->
        <p style="float: left;font-weight: bold">高度：</p>
        <ol id="height" class="olID"  >
           
        </ol>
    </div>
    <div class="TimePicker" style=" margin-top: 1px; "><!--height: 50px; background-color: lightgray;padding-top: 15px;-->
      

       
               <span style="font: 14px; font-weight: bold">时间段:</span>&nbsp&nbsp
                <input type="text" class="form-control" id="txt_beginTime" />&nbsp&nbsp
        <b>至</b>&nbsp&nbsp
        <input type="text" class="form-control" id="txt_endTime" />
        &nbsp&nbsp
                          
         <select class="form-control" id="timeSpan" style="width:auto">
             
         </select> &nbsp&nbsp
         <label >播放速度：</label>          
                <input style="width:80px;" class="form-control" id="txt_speed" value="800" />          
            <label  style="text-align:left;">毫秒</label>&nbsp&nbsp
        </div>
        <div class="TimePicker" style=" margin-top: 1px; ">
            <button class="btn btn-success btn-label-left" id="btn_SetUp" onclick="setUp()" type="button">
                启用设置
            </button>
          <button class="btn btn-primary btn-label-left" id="imgPaly" type="button">
            播放
        </button>
        <button class="btn btn-primary btn-label-left" id="PauseImg" type="button">
            暂停
        </button>
        <button class="btn btn-primary btn-label-left" id="PreImg" type="button">
            上一张
        </button>
        <button class="btn btn-primary btn-label-left" id="NextImg" type="button">
            下一张
        </button> 
     </div>   
    
    </div>
        <center style="padding-top:10px;">
             <a id="imgpplay" href="#">
        <img id="img" style="width:750px;height:500px"/>  </a>  
   </center>

</body>
</html>
<script type="text/javascript">
    $(function () {
        //点击播放按钮
        $("#imgPaly").click(function () {
            Stratplay();
        });
        //暂停
        $("#PauseImg").click(function () {
            PausePlay();
        });
        //下一张
        $("#NextImg").click(function () {

            nextImg();
        });
        //上一张
        $("#PreImg").click(function () {
            prevImg();
        });
    });
</script>