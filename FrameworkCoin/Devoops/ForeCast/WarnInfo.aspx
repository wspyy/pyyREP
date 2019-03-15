<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarnInfo.aspx.cs" Inherits="Devoops_ForeCast_WarnInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../plugins/jquery-ui/jquery-ui.min.css" rel="stylesheet" />
    <link href="../plugins/jquery-ui/jquery-ui.theme.css" rel="stylesheet" />
    <link href="../../Demo/plugins/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Demo/css/style.css" rel="stylesheet" />
    <link href="../css/font-awesome.css" rel="stylesheet" />
    <link href="../css/output.css" rel="stylesheet" />
    <script src="../js/jquery-1.7.2.js"></script>
    <script src="../js/jquery-2.1.0.min.js"></script>
    <script src="../js/method.js"></script>
    <script src="../plugins/jquery-ui/jquery-ui.js"></script>
    <script src="../plugins/data-tables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="../js/devoops.js"></script>
    <script src="../js/method.js"></script>
    <script src="../../Controls/Highcharts-4.0.1/highcharts.js"></script>
    <script src="../plugins/jquery-ui-timepicker-addon/jquery-ui-timepicker-addon.js"></script>
    <script src="../../Demo/plugins/jquery-ui/i18n/jquery.ui.datepicker-zh-CN.min.js"></script>



    <style>
        html, body {
            margin: 0px 0px 0px 0px;
            height: 100%;
            overflow: hidden;
            font-size: 12px;
        }

        .tab {
            word-break: keep-all;
            border: 1px solid #EBEBEB;
            border-bottom: none;
            border-left: none;
            border-top-left-radius: 6px;
            width: 100%;
        }

            .tab td {
                border-bottom: 1px solid #EBEBEB;
                border-left: 1px solid #EBEBEB;
                padding: 5px;
                line-height: 20px;
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
            }

        .aqi1 {
            float: left;
            border-radius: 10px;
            width: 15px;
            height: 15px;
            background-color: #6FBE09;
        }

        .aqi2 {
            float: left;
            border-radius: 10px;
            width: 15px;
            height: 15px;
            background-color: #FBD12A;
        }

        .aqi3 {
            float: left;
            border-radius: 10px;
            width: 15px;
            height: 15px;
            background-color: #FFA641;
        }

        .aqi4 {
            float: left;
            border-radius: 10px;
            width: 15px;
            height: 15px;
            background-color: #EB5B13;
        }

        .aqi5 {
            float: left;
            border-radius: 10px;
            width: 15px;
            height: 15px;
            background-color: #960453;
        }

        .aqi6 {
            float: left;
            border-radius: 10px;
            width: 15px;
            height: 15px;
            background-color: #580422;
        }

         .colorf
         {
             font-size:18px;
             font-weight:900;
         }

        #datatable-1 th,td{
            text-align:center;
            font-size:larger;
        }
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
            width:30px;
            height:30px;
            text-align:center;
            vertical-align:central;
        }
    </style>

    <script type="text/javascript">
        $(function () {

         
            $('#txt_begin').datepicker({ defaultDate: new Date().addDays(-3).format("yyyy-MM-dd") });
            $('#txt_end').datepicker({ defaultDate: new Date() });
            $('#txt_begin').val(new Date().addDays(-3).format("yyyy-MM-dd"));
            $('#txt_end').val(new Date().format("yyyy-MM-dd"));
            tbBind(1);
            document.getElementById("CurrentPagecount").value = 1;
            
         
           
        })
        $(function () {
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
          .on('click', '.expand-link', function (e) {
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
          .on('click', '.close-link', function (e) {
              e.preventDefault();
              var content = $(this).closest('div.box');
              content.remove();
          });

        });
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
            tbBind(page);
        }
        function tbBind(page) {
                   
            $.ajax({
                url: '../../Data/GetForCastWarnInfoData.ashx',
                method: 'Get',
                data: { mode: "QryWarnInfo", begin: $('#txt_begin').val(), end: $('#txt_end').val(),page:page,rows:10},
                dataType: 'html',
                cache: false,
                beforeSend: function () { $("#tbdataid").html("<div class=\"NoData\" style=\"margin-top:80px;font-size:12px;\"><img src=\"../ForeCast/images/loading.gif\" />加载中...</div>"); },
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
                            strdata += "<tr><td>" + arr_data.Data[i].FutureDate + "</td><td>" + arr_data.Data[i].AQI + "</td><td style='text-align:left; padding-left:40px; width:150px;'><span style=\" float: left;border-radius: 10px; width: 15px;height: 15px; background-color:" + arr_data.Data[i].RadiusColor + ";\"></span>&nbsp;" + arr_data.Data[i].Category + "</td><td>" + arr_data.Data[i].QualityIndex + "</td><td><font class=\"colorf\" color=\"" + arr_data.Data[i].ColorCode + "\">" + arr_data.Data[i].ColorName + "</font></td><td style=\"cursor:pointer\" value=\"" + arr_data.Data[i].ReportCode + "\"><a  align=\"center\" onmousemove=\"this.style.color='red'\" name=\"" + arr_data.Data[i].ReportCode + "\"  onmouseout=\"this.style.color='blue'\" onclick=\"printbind(this)\"><img title='查看报告' src='../img/rpt.png'/></></td><td style=\"cursor:pointer\" value=\"" + arr_data.Data[i].ReportCode + "\"><a  align=\"center\" onmousemove=\"this.style.color='red'\" name=\"" + arr_data.Data[i].ReportCode + "\"  onmouseout=\"this.style.color='blue'\" onclick=\"delPoll(this)\"><img title='删除' src='../img/del.png'/></></td></tr>";
                        }

                        $("#tbdataid").html(strdata);
                     
                        $("#pageID").show();
                    }
                    else {
                        $("#pageID").hide();
                        $("#tbdataid").html("<tr><td colspan='6' style='text-align:center;'>暂无数据</td></tr>");
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
        function LoadData(wtitle) {
            var url = "WarnInfoDetail.aspx";

            $("#dialog").load(url, function () {

                $("#dialog").dialog({
                    autoOpen: false,
                    width: "100%",
                    title: "详细预警管理",
                    closeText: "关闭",
                    height: 590,
                    position: {
                        my: "right",
                        at: "top"
                    },
                    show: { effect: "blind", duration: 500 },
                    hide: { effect: "blind", duration: 500 }//explode
                });
                $('#set_date').val(new Date().addDays(-0).format("yyyy-MM-dd"));

                $('#set_date').datepicker({ showOn: "click" });
                $('#second_date').val(new Date().addDays(-0).format("yyyy-MM-dd"));

                $('#second_date').datepicker({ showOn: "click" });
                $("#dialog").dialog("open");


            });

        }
        function printbind(obj) {
            var data = obj.name;
            var url = "ConsulCir/PollutionDayliyReport.aspx";
            parent.TRansIframe(url + "?ReportCode=" + data, '重污染天气会商意见');
            //LoadData(data);
            //window.open('WarnInfoDetail.aspx?data='+data, '', 'toolbar=no,menubar=no,scrollbars=yes,resizable=yes,location=no,status=no');
        }
        function delPoll(obj) {
            var data = obj.name;
            $.ajax({
                url: "../../Data/PollConclusions.ashx",
                data: { flag: 4, ReportCode: data },
                success: function (result) {
                    if (result == "True") {
                        tbBind(1);
                    }
                    else alert("删除失败");
                },
                error: function (e,c) {
                    debugger;
                }
            });
        }
    </script>
</head>
<body>


    <form id="form1" runat="server" style="overflow:scroll;overflow-x:hidden;  overflow-y:auto;">
      
        <div>
     
            <div class="box-content no-padding">
                <div style="width: 100%; padding-top: 10px;">
                    <label class="col-sm-1 control-label" style="padding-right: 0px;margin-top: 3px;font-size: larger;">时间范围：</label>
                    <div class="col-sm-2" style="padding-left: 0px;">
                        <input type="text" id="txt_begin" class="form-control" placeholder="Date" style="float: left;" />

                    </div>
                    <label class="col-sm-1 control-label" style="padding-left: 0px; width: 10px;margin-top: 3px;font-size: larger;">至</label>
                    <div class="col-sm-2" style="padding-left: 8px;">
                        <input type="text" id="txt_end" class="form-control" placeholder="Date" />
                    </div>
                    <button class="btn btn-primary btn-label-left" id="imgPaly" type="button" onclick="tbBind(1)">
                        查询
                    </button>
                </div>
                <table class="tab" id="datatable-1" >

                        <tr>
                            <th>时间</th>
                            <th>AQI</th>
                            <th>空气状况</th>
                            <th>预警等级</th>
                            <th>预警颜色</th>
                            <th>报告</th>
                            <th>删除</th>
                        </tr>
                    <tbody id="tbdataid">
                    </tbody>
                      <tfoot>
                    <tr>
                    <td colspan="6"id="pageID" ></td>
                    </tr>
                </tfoot>
                </table>

            </div>
        </div>
   
         <input type="hidden" id="PageCount"  runat="server" />    
         <input type="hidden" id="CurrentPagecount"  runat="server" />   
        <div id="dialog"></div>
    </form>
    <script type="text/javascript">
        $("#form1").css({ height: $(window).height()});
    
        $(window).resize(function () {
            $("#ParentFrame").height($(window).height());
        });
      
    </script>
</body>
</html>
