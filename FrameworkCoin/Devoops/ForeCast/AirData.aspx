<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirData.aspx.cs" Inherits="Devoops_ForeCast_AirData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>空气质量预测结果</title>
    <script type="text/javascript">
        function getSatationList() {
            $.ajax({
                url: "../../Data/GetBaseData.ashx",
                data: { flag: "AirStation" },
                dataType: "json",
                success: function (result) {
                    var content = "";
                    if (result && result.DataTable && result.DataTable.length > 0) {
                        for (var i = 0; i < result.DataTable.length; i++) {
                            content += "<div class=\"radio-inline\"><label><input type=\"radio\" name=\"station\" "
                            if (result.DataTable[i]["StationCode"] == "51") content += " checked='checked' ";
                            content += "value=\""
                            + result.DataTable[i]["StationCode"] + "\">"
                            + result.DataTable[i]["StationName"]
                            + "<i class=\"fa fa-circle-o small\"></i></label></div>"
                        }
                        $("#stationPanel").append(content);
                        $("[name='station']").change(changeData);
                        changeData();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }
        var code = "";
        $(function () {
            getSatationList();
            //页面加载默认显示
            $("#tabs-1").load("AirDataImg.html", null, function () { SetTime(code)});


        });
        function changeData() {
             code = $("[name='station']:checked").val();
            $("#tabs-1").load("AirDataImg.html", null, function () { SetTime(code)});
        }
    </script>
</head>
<body>
    <div id="queryPanel" class="box ui-draggable ui-droppable" style="width: 100%;">
        <div id="stationPanel" style="width: 100%;">
            <label class="control-label">监测站点：</label>
        </div>
    </div>
        <div id="tabs-1" style="border: 1px solid #d8d8d8; border-top: none; padding: 20px; padding-top: 10px;">
        </div>
</body>
</html>
