<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConsultReport.aspx.cs" Inherits="Devoops_ConsulCir_ConsultReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../plugins/jquery-ui/jquery-ui.min.css" rel="stylesheet" />
    <link href="../plugins/jquery-ui/jquery-ui.theme.css" rel="stylesheet" />
    <link href="../../Demo/plugins/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Demo/css/style.css" rel="stylesheet" />
    <link href="../css/font-awesome.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.7.2.js"></script>

    <script src="../js/jquery-1.10.2.min.js"></script>
    <script src="../plugins/jquery-ui/jquery-ui.js"></script>

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
            padding-left: 5px;
            padding-right: 5px;
            padding-top: 5px;
            margin-left: 10px;
            margin-bottom: 10px;
            float: left;
            list-style: none;
            cursor: pointer;
        }

            #reportlist li:hover {
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
            margin-left: 10px;
            float: left;
            list-style: none;
            cursor: pointer;
        }

        .mli {
            background: #6596BD;
            border: 1px solid #003399;
            color: white;
        }

        .sli {
            background: #6596BD;
            border: 1px solid #003399;
            color: white;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {

            var myDate = new Date();
            var year = myDate.getFullYear(); //获取完整的年份
            var month = myDate.getMonth() + 1; //获取当前月份;

            bingYear(year);
            bindLi();
            getDateData(year, month);


        });

        function showdialog(paramdate) {
            url = "ConsultReportDeatil.aspx?date=" + paramdate;
            parent.TRansIframe("ConsulCir/ConsultReportDeatil.aspx?date=" + paramdate);
            //$("#dialog").load(url, function () {
            //    $("#dialog").dialog({
            //        autoOpen: false,
            //        modal: true,
            //        width: "100%",
            //        title: "会商报告",
            //        closeText: "关闭",
            //        height: $(window).height(),
            //        position: {
            //            my: "center",
            //            at: "top"
            //        },
            //        show: { effect: "blind", duration: 500 },
            //        hide: { effect: "blind", duration: 500 }//explode
            //    });
             
            //    $("#dialog").dialog("open");
            //});
        }

        function bingYear(year) {
            var strselect = "";
            for (var i = 2012; i < 2020; i++) {
                strselect += '<option id=\"' + i + '\" value=\"' + i + '\">' + i + '</option>';
            }
            $("#YearPicker").html(strselect);

            $("#YearPicker").find("option[value='" + year + "']").attr("selected", true);
        }
        function getDateData(year, month) {
            var date = GetLastDay(year, month);
            if (month < 10) {
                month = '0' + month;
            }
            $.ajax({
                url: '../../Data/GetConCirculatData.ashx',
                method: 'Get',
                data: { stype: "selecdate", stryear: year, strmonth: month, strdate: date },
                dataType: 'html',
                cache: false,

                success: function (data) {
                    if (data != null && data != "")
                        $("#reportlist").html(data);
                },
                error: function () { }
            });

        }

        function bindLi() {
            var strli = "";
            var mon = new Date().getMonth() + 1;
            for (var i = 1; i <= 12; i++) {
                if (mon == i) {
                    strli += '<li class=\"sli\" value=\"' + i + '\" onclick=\"onclicli(this)\">' + i + '月' + '</li>';
                } else {
                    strli += '<li value=\"' + i + '\" onclick=\"onclicli(this)\">' + i + '月' + '</li>';
                }
            }
            $("#olID").html(strli);
            $("#olID li").each(function (index, o) {

                $(o).mousemove(function () { $(o).addClass("mli") }).mouseleave(function () {
                    $(o).removeClass("mli");
                }).click(function () {
                    $("#olID li").removeClass("sli");
                    $(o).addClass("sli")
                });
            });

        }
        function onclicli(obj) {
            var month = obj.value;
            var year = document.getElementById("YearPicker").value;
            getDateData(year, month);

        }
        //得到最后一天
        function GetLastDay(year, month) {
            var date = new Date(year, month, 1);
            var lastday = new Date(date.getTime() - 1000 * 60 * 60 * 24).getDate();
            return lastday;
        }


    </script>
</head>
<body style="background: #fff;">
    <form id="form1" runat="server">
   
        <div id="TimePicker" class="well" style="background-color: white; height: 50px; margin-top: 10px;">

            <div>
                <p style="float: left;">年份：</p>
                <select id="YearPicker" style="float: left;">
                </select>
                <p style="float: left; margin-left: 50px;">月份：</p>
                <ol id="olID" style="float: left; padding-top: -20px;">
                </ol>
            </div>

        </div>
        <br />
        <div id="reportlist" class="well" style="background-color: white; margin-top: -20px; min-height: 500px; border-bottom: none;">
        </div>
        <div id="dialog" style="display: none; background: #fff;"></div>
        <input type="hidden" id="hid" runat="server" />
    </form>
</body>
<script type="text/javascript">
    $(function () {

        $('.select2-choice li').click(function () {
            $('.anserdh li').select
            $(this).parent().addClass('qhbg');

        });


        $("#reportlist").css({ height: $(window).height() - 120 });
        $("#form1").css({ height: $(window).height() });
        $(window).resize(function () {
            $("#form1").css({ height: $(window).height() });
            $("#reportlist").css({ height: $(window).height() - 120 });
        });
    });
    function ondik(obj) {
        alert(111);
        alert(obj.name);
        showdialog(obj.name);
    }
</script>
</html>
